using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using PUPFMIS.Models;
using PUPFMIS.Models.HRIS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace PUPFMIS.BusinessLayer
{
    public class PPMPBL : Controller
    {
        private FMISDbContext FMISdb = new FMISDbContext();
        private HRISDbContext HRISdb = new HRISDbContext();
        private ReportsConfig reportConfig = new ReportsConfig();

        public List<PPMPDeadlines> GetFiscalYears()
        {
            return FMISdb.PPMPDeadlines.Where(d => d.PurgeFlag == false).OrderBy(d => d.FiscalYear).ToList();
        }

        public int UpdateTotalConsumption()
        {
            return 25;
        }

        //====================================================================================================//

        public List<PPMPHeaderViewModel> GetMyPPMP()
        {
            var OfficeID = FMISdb.UserInformation.Find(FMISdb.UserAccounts.Where(d => d.Email == System.Web.HttpContext.Current.User.Identity.Name).FirstOrDefault().ID).Office;
            var ppmp = (from users in FMISdb.UserAccounts
                        join ppmpHeader in FMISdb.PPMPHeader on users.ID equals ppmpHeader.PreparedBy
                        join types in FMISdb.InventoryTypes on ppmpHeader.PPMPType equals types.ID
                        where ppmpHeader.OfficeReference == OfficeID
                        select new
                        {
                            ppmpHeader.ID,
                            ppmpHeader.FiscalYear,
                            ppmpHeader.ReferenceNo,
                            ppmpHeader.OfficeReference,
                            PPMPType = types.InventoryTypeName,
                            ppmpHeader.CreatedAt,
                            ppmpHeader.SubmittedAt,
                            users.FKUserInformationReference.FirstName,
                            users.FKUserInformationReference.LastName,
                            users.FKUserInformationReference.Designation,
                            ppmpHeader.Status
                        }).AsEnumerable();
            var offices = HRISdb.OfficeModel.AsEnumerable();
            return (from headers in ppmp
                    join office in offices on headers.OfficeReference equals office.ID
                    select new PPMPHeaderViewModel
                    {

                        PPMPId = headers.ID,
                        ReferenceNo = headers.ReferenceNo,
                        FiscalYear = headers.FiscalYear,
                        OfficeName = office.OfficeName,
                        PPMPType = headers.PPMPType,
                        PreparedBy = headers.FirstName.ToUpper() + " " + headers.LastName.ToUpper() + ", " + headers.Designation,
                        SubmittedBy = office.OfficeHead.ToUpper() + ", " + office.Designation,
                        CreatedAt = headers.CreatedAt,
                        SubmittedAt = headers.SubmittedAt,
                        Status = headers.Status
                    }).ToList();
        }

        public bool CreatePPMPCSE(PPMPViewModel ppmpCSE, string UserEmail)
        {
            PPMPHeader header = new PPMPHeader();
            PPMPCSEDetails cseDetails = new PPMPCSEDetails();
            
            header.FiscalYear = ppmpCSE.PPMPHeader.FiscalYear;
            header.PPMPType = FMISdb.InventoryTypes.Where(d => d.InventoryTypeName == "Common Use Office Supplies").FirstOrDefault().ID;
            header.PreparedBy = FMISdb.UserAccounts.Where(d => d.Email == UserEmail).FirstOrDefault().ID;
            header.OfficeReference = FMISdb.UserAccounts.Where(d => d.Email == UserEmail).FirstOrDefault().FKUserInformationReference.Office;
            header.CreatedAt = DateTime.Now;
            header.ReferenceNo = GenerateReferenceNo(ppmpCSE.PPMPHeader.FiscalYear, header.OfficeReference);
            header.Status = "New";

            FMISdb.PPMPHeader.Add(header);

            if (FMISdb.SaveChanges() == 0)
            {
                return false;
            }

            List<PPMPCSEDetails> ppmpLineItems = new List<PPMPCSEDetails>();
            foreach (var item in ppmpCSE.DBMItems)
            {
                PPMPCSEDetails ppmpLineItem = new PPMPCSEDetails();
                ppmpLineItem.PPMPID = header.ID;
                ppmpLineItem.Item = item.ItemID;
                ppmpLineItem.Qtr1 = (String.IsNullOrEmpty(item.Qtr1Qty.ToString())) ? 0 : (int)item.Qtr1Qty;
                ppmpLineItem.Qtr2 = (String.IsNullOrEmpty(item.Qtr2Qty.ToString())) ? 0 : (int)item.Qtr2Qty;
                ppmpLineItem.Qtr3 = (String.IsNullOrEmpty(item.Qtr3Qty.ToString())) ? 0 : (int)item.Qtr3Qty;
                ppmpLineItem.Qtr4 = (String.IsNullOrEmpty(item.Qtr4Qty.ToString())) ? 0 : (int)item.Qtr4Qty;
                ppmpLineItem.TotalQty = (int)item.TotalQty;
                ppmpLineItem.Remarks = item.Remarks;
                ppmpLineItems.Add(ppmpLineItem);
            }

            foreach (var item in ppmpCSE.NonDBMItems)
            {
                PPMPCSEDetails ppmpLineItem = new PPMPCSEDetails();
                ppmpLineItem.PPMPID = header.ID;
                ppmpLineItem.Item = item.ItemID;
                ppmpLineItem.Qtr1 = (String.IsNullOrEmpty(item.Qtr1Qty.ToString())) ? 0 : (int)item.Qtr1Qty;
                ppmpLineItem.Qtr2 = (String.IsNullOrEmpty(item.Qtr2Qty.ToString())) ? 0 : (int)item.Qtr2Qty;
                ppmpLineItem.Qtr3 = (String.IsNullOrEmpty(item.Qtr3Qty.ToString())) ? 0 : (int)item.Qtr3Qty;
                ppmpLineItem.Qtr4 = (String.IsNullOrEmpty(item.Qtr4Qty.ToString())) ? 0 : (int)item.Qtr4Qty;
                ppmpLineItem.TotalQty = (int)item.TotalQty;
                ppmpLineItem.Remarks = item.Remarks;
                ppmpLineItems.Add(ppmpLineItem);
            }

            FMISdb.PPMPCSEDetails.AddRange(ppmpLineItems);

            if (FMISdb.SaveChanges() != ppmpLineItems.Count())
            {
                return false;
            }

            PPMPApprovalWorkflow approvalWF = new PPMPApprovalWorkflow();
            approvalWF.PPMPId = header.ID;
            approvalWF.Status = "New";
            approvalWF.Remarks = "New PPMP by " + ppmpCSE.PPMPHeader.OfficeName + " is created.";
            approvalWF.UpdatedAt = DateTime.Now;
            approvalWF.ActionMadeBy = (int)header.PreparedBy;
            approvalWF.ActionMadeByOffice = header.OfficeReference;
            FMISdb.PPMPApprovalWorkflow.Add(approvalWF);

            if (FMISdb.SaveChanges() == 1)
            {
                return true;
            }

            return false;
        }

        public List<PPMPCSEDetails> GetDBMItems(int? PPMPID)
        {
            return FMISdb.PPMPCSEDetails.Where(d => d.PPMPID == PPMPID && d.FKItem.ProcurementSource == ProcurementSources.PS_DBM).ToList();
        }

        public List<PPMPCSEDetails> GetNonDBMItems(int? PPMPID)
        {
            return FMISdb.PPMPCSEDetails.Where(d => d.PPMPID == PPMPID && d.FKItem.ProcurementSource == ProcurementSources.Non_DBM).ToList();
        }

        public PPMPHeaderViewModel GetPPMPHeader(string ReferenceNo)
        {
            var ppmp = (from users in FMISdb.UserAccounts
                        join ppmpHeader in FMISdb.PPMPHeader on users.ID equals ppmpHeader.PreparedBy
                        join types in FMISdb.InventoryTypes on ppmpHeader.PPMPType equals types.ID
                        where ppmpHeader.ReferenceNo == ReferenceNo
                        select new
                        {
                            ppmpHeader.ID,
                            ppmpHeader.FiscalYear,
                            ppmpHeader.ReferenceNo,
                            ppmpHeader.OfficeReference,
                            ppmpHeader.CreatedAt,
                            ppmpHeader.SubmittedAt,
                            types.InventoryTypeName,
                            users.FKUserInformationReference.FirstName,
                            users.FKUserInformationReference.LastName,
                            users.FKUserInformationReference.Designation,
                            ppmpHeader.Status
                        }).AsEnumerable();
            var offices = HRISdb.OfficeModel.AsEnumerable();
            return (from headers in ppmp
                    join office in offices on headers.OfficeReference equals office.ID
                    select new PPMPHeaderViewModel
                    {

                        PPMPId = headers.ID,
                        ReferenceNo = headers.ReferenceNo,
                        FiscalYear = headers.FiscalYear,
                        OfficeName = office.OfficeName,
                        PPMPType = headers.InventoryTypeName,
                        PreparedBy = headers.FirstName.ToUpper() + " " + headers.LastName.ToUpper() + ", " + headers.Designation,
                        SubmittedBy = office.OfficeHead.ToUpper() + ", " + office.Designation,
                        CreatedAt = headers.CreatedAt,
                        SubmittedAt = headers.SubmittedAt,
                        Status = headers.Status
                    }).FirstOrDefault();
        }

        public PPMPCSEViewModel GetPPMPCSEDetails(string ReferenceNo)
        {
            PPMPCSEViewModel ppmpCSE = new PPMPCSEViewModel();
            ppmpCSE.PPMPHeader = GetPPMPHeader(ReferenceNo);
            ppmpCSE.PPMPItems = FMISdb.PPMPCSEDetails.Where(d => d.PPMPID == ppmpCSE.PPMPHeader.PPMPId).ToList();
            return ppmpCSE;
        }

        public List<PPMPApprovalWorkflowViewModel> GetApprovalWorkflow(string ReferenceNo)
        {
            var ppmp = (from ppmpHeader in FMISdb.PPMPHeader
                        join workflow in FMISdb.PPMPApprovalWorkflow on ppmpHeader.ID equals workflow.PPMPId
                        join users in FMISdb.UserAccounts on workflow.ActionMadeBy equals users.ID
                        where ppmpHeader.ReferenceNo == ReferenceNo
                        select new
                        {
                            ppmpHeader.ID,
                            ppmpHeader.ReferenceNo,
                            users.FKUserInformationReference.FirstName,
                            users.FKUserInformationReference.LastName,
                            workflow.ActionMadeByOffice,
                            workflow.Status,
                            workflow.UpdatedAt,
                            workflow.Remarks
                        }).AsEnumerable();
            var offices = HRISdb.OfficeModel.AsEnumerable();
            return (from headers in ppmp
                    join office in offices on headers.ActionMadeByOffice equals office.ID
                    select new PPMPApprovalWorkflowViewModel
                    {
                        PPMPId = headers.ID,
                        ReferenceNo = headers.ReferenceNo,
                        Office = office.OfficeName,
                        Personnel = headers.FirstName + " " + headers.LastName,
                        UpdatedAt = headers.UpdatedAt,
                        Status = headers.Status,
                        Remarks = headers.Remarks
                    }).OrderByDescending(d => d.PPMPId).ToList();
        }

        public bool SubmitPPMP(string ReferenceNo)
        {
            PPMPHeaderViewModel ppmpHeaderVM = GetPPMPHeader(ReferenceNo);
            PPMPHeader header = FMISdb.PPMPHeader.Find(ppmpHeaderVM.PPMPId);
            PPMPApprovalWorkflow approvalWF = new PPMPApprovalWorkflow();

            if (ppmpHeaderVM != null)
            {
                header.SubmittedAt = DateTime.Now;
                header.Status = "Submitted";

                if (FMISdb.SaveChanges() == 0)
                {
                    return false;
                }

                approvalWF.PPMPId = header.ID;
                approvalWF.Status = "Submitted";
                approvalWF.Remarks = "PPMP submitted by " + ppmpHeaderVM.OfficeName + ".";
                approvalWF.UpdatedAt = DateTime.Now;
                approvalWF.ActionMadeBy = (int)header.PreparedBy;
                approvalWF.ActionMadeByOffice = header.OfficeReference;
                FMISdb.PPMPApprovalWorkflow.Add(approvalWF);

                if (FMISdb.SaveChanges() == 1)
                {
                    return true;
                }
            }
            return false;
        }

        public string GenerateReferenceNo(string FiscalYear, int OfficeReference)
        {
            string referenceNo = string.Empty;
            string officeCode = HRISdb.OfficeModel.Find(OfficeReference).OfficeCode;
            string seqNo = (FMISdb.PPMPHeader.Where(d => d.FiscalYear == FiscalYear).Count() + 1).ToString();
            seqNo = seqNo.ToString().Length == 3 ? seqNo : seqNo.ToString().Length == 2 ? "0" + seqNo.ToString() : "00" + seqNo.ToString();
            referenceNo = "PPMP-CUSE-" + officeCode + "-" + seqNo + "-" + FiscalYear;
            return referenceNo;
        }

        public MemoryStream GeneratePPMPReport(string ReferenceNo, string LogoPath)
        {
            PPMPCSEViewModel ppmpVM = GetPPMPCSEDetails(ReferenceNo);
            ppmpVM.PPMPItems = FMISdb.PPMPCSEDetails.Where(d => d.FKPPMPReference.ID == ppmpVM.PPMPHeader.PPMPId).ToList();

            reportConfig.ReportTitle = ppmpVM.PPMPHeader.ReferenceNo;
            reportConfig.ReportFormTitle = "Form D";
            reportConfig.ReportReferenceNo = ppmpVM.PPMPHeader.ReferenceNo;
            reportConfig.LogoPath = LogoPath;
            reportConfig.DocumentSetupLandscape();
            reportConfig.AddHeader("PROJECT PROCUREMENT MANAGEMENT PLAN", new Unit(12, UnitType.Point), true);
            reportConfig.AddHeader("Common-use Supplies and Equipment", new Unit(10, UnitType.Point));
            reportConfig.AddHeader("Fiscal Year " + ppmpVM.PPMPHeader.FiscalYear, new Unit(10, UnitType.Point));
            reportConfig.AddHeader("\n");

            reportConfig.AddTable();
            reportConfig.AddContentColumn(new Unit(2, UnitType.Centimeter), ParagraphAlignment.Left);
            reportConfig.AddContentColumn(new Unit(18, UnitType.Centimeter), ParagraphAlignment.Left);
            reportConfig.AddContentColumn(new Unit(10.48, UnitType.Centimeter));

            reportConfig.AddContentRow();
            reportConfig.AddContent("End User: ", 0, 10);
            reportConfig.AddContent(ppmpVM.PPMPHeader.OfficeName, 1, 10, true, ParagraphAlignment.Left, true);
            reportConfig.AddContentRow();
            reportConfig.AddContent("\n", 0);

            reportConfig.AddTable(true);
            reportConfig.AddContentColumn(new Unit(2, UnitType.Centimeter), ParagraphAlignment.Center, new Color(252, 207, 101));
            reportConfig.AddContentColumn(new Unit(8.5, UnitType.Centimeter), ParagraphAlignment.Center, new Color(252, 207, 101));
            reportConfig.AddContentColumn(new Unit(2.5, UnitType.Centimeter), ParagraphAlignment.Center, new Color(252, 207, 101));
            reportConfig.AddContentColumn(new Unit(2.37, UnitType.Centimeter), ParagraphAlignment.Center, new Color(252, 207, 101));
            reportConfig.AddContentColumn(new Unit(2.37, UnitType.Centimeter), ParagraphAlignment.Center, new Color(252, 207, 101));
            reportConfig.AddContentColumn(new Unit(2.37, UnitType.Centimeter), ParagraphAlignment.Center, new Color(252, 207, 101));
            reportConfig.AddContentColumn(new Unit(2.37, UnitType.Centimeter), ParagraphAlignment.Center, new Color(252, 207, 101));
            reportConfig.AddContentColumn(new Unit(2.37, UnitType.Centimeter), ParagraphAlignment.Center, new Color(252, 207, 101));
            reportConfig.AddContentColumn(new Unit(5.63, UnitType.Centimeter), ParagraphAlignment.Center, new Color(252, 207, 101));

            reportConfig.AddContentRow(new Unit(0.2, UnitType.Inch));
            reportConfig.AddContent("PART I. AVAILABLE AT PROCUREMENT SERVICE STORES", 0, new Unit(10, UnitType.Point), true, ParagraphAlignment.Left, VerticalAlignment.Center, 8);

            reportConfig.AddContentRow(new Unit(0.2, UnitType.Inch));
            reportConfig.AddContent("Item Code", 0, new Unit(10, UnitType.Point), true, ParagraphAlignment.Center, VerticalAlignment.Center, null, 1);
            reportConfig.AddContent("Item and Specifications", 1, new Unit(10, UnitType.Point), true, ParagraphAlignment.Center, VerticalAlignment.Center, null, 1);
            reportConfig.AddContent("Unit of Measure", 2, new Unit(10, UnitType.Point), true, ParagraphAlignment.Center, VerticalAlignment.Center, null, 1);
            reportConfig.AddContent("Quantity Requirement", 3, new Unit(10, UnitType.Point), true, ParagraphAlignment.Center, VerticalAlignment.Center, 4);
            reportConfig.AddContent("Remarks", 8, new Unit(10, UnitType.Point), true, ParagraphAlignment.Center, VerticalAlignment.Center, null, 1);

            reportConfig.AddContentRow(new Unit(0.2, UnitType.Inch));
            reportConfig.AddContent("Qtr 1", 3, new Unit(10, UnitType.Point), true, ParagraphAlignment.Center, VerticalAlignment.Center);
            reportConfig.AddContent("Qtr 2", 4, new Unit(10, UnitType.Point), true, ParagraphAlignment.Center, VerticalAlignment.Center);
            reportConfig.AddContent("Qtr 3", 5, new Unit(10, UnitType.Point), true, ParagraphAlignment.Center, VerticalAlignment.Center);
            reportConfig.AddContent("Qtr 4", 6, new Unit(10, UnitType.Point), true, ParagraphAlignment.Center, VerticalAlignment.Center);
            reportConfig.AddContent("Total", 7, new Unit(10, UnitType.Point), true, ParagraphAlignment.Center, VerticalAlignment.Center);

            reportConfig.AddTable(true);
            reportConfig.AddContentColumn(new Unit(2, UnitType.Centimeter));
            reportConfig.AddContentColumn(new Unit(8.5, UnitType.Centimeter));
            reportConfig.AddContentColumn(new Unit(2.5, UnitType.Centimeter));
            reportConfig.AddContentColumn(new Unit(2.37, UnitType.Centimeter));
            reportConfig.AddContentColumn(new Unit(2.37, UnitType.Centimeter));
            reportConfig.AddContentColumn(new Unit(2.37, UnitType.Centimeter));
            reportConfig.AddContentColumn(new Unit(2.37, UnitType.Centimeter));
            reportConfig.AddContentColumn(new Unit(2.37, UnitType.Centimeter));
            reportConfig.AddContentColumn(new Unit(5.63, UnitType.Centimeter));

            List<PPMPCSEDetails> DBMItems = FMISdb.PPMPCSEDetails.Where(d => d.FKPPMPReference.ID == ppmpVM.PPMPHeader.PPMPId && d.FKItem.ProcurementSource == ProcurementSources.PS_DBM).ToList();
            if (DBMItems.Count == 0)
            {
                reportConfig.AddContentRow(new Unit(0.2, UnitType.Inch));
                reportConfig.AddContent("*** NO ITEMS ***", 0, new Unit(10, UnitType.Point), true, ParagraphAlignment.Center, VerticalAlignment.Center, 8);
            }
            else
            {
                foreach (var item in DBMItems)
                {
                    reportConfig.AddContentRow(new Unit(0.2, UnitType.Inch));
                    var itemName = item.FKItem.ItemName + ((String.IsNullOrEmpty(item.FKItem.ItemShortSpecifications)) ? "" : ", " + item.FKItem.ItemShortSpecifications);
                    reportConfig.AddContent(item.FKItem.ItemCode, 0, new Unit(10, UnitType.Point), false, ParagraphAlignment.Center, VerticalAlignment.Center);
                    reportConfig.AddContent(itemName, 1, new Unit(10, UnitType.Point), false, ParagraphAlignment.Justify, VerticalAlignment.Center);
                    reportConfig.AddContent(item.FKItem.FKIndividualUnitReference.UnitName, 2, new Unit(10, UnitType.Point), false, ParagraphAlignment.Center, VerticalAlignment.Center);
                    reportConfig.AddContent(item.Qtr1.ToString(), 3, new Unit(10, UnitType.Point), false, ParagraphAlignment.Center, VerticalAlignment.Center);
                    reportConfig.AddContent(item.Qtr2.ToString(), 4, new Unit(10, UnitType.Point), false, ParagraphAlignment.Center, VerticalAlignment.Center);
                    reportConfig.AddContent(item.Qtr3.ToString(), 5, new Unit(10, UnitType.Point), false, ParagraphAlignment.Center, VerticalAlignment.Center);
                    reportConfig.AddContent(item.Qtr4.ToString(), 6, new Unit(10, UnitType.Point), false, ParagraphAlignment.Center, VerticalAlignment.Center);
                    reportConfig.AddContent(item.TotalQty.ToString(), 7, new Unit(10, UnitType.Point), true, ParagraphAlignment.Center, VerticalAlignment.Center);
                    reportConfig.AddContent((item.Remarks == "Acceptable" || String.IsNullOrEmpty(item.Remarks)) ? "" : item.Remarks, 8, new Unit(10, UnitType.Point), false, ParagraphAlignment.Justify, VerticalAlignment.Center);
                }
            }

            reportConfig.AddTable(true);
            reportConfig.AddContentColumn(new Unit(2, UnitType.Centimeter), ParagraphAlignment.Center, new Color(252, 207, 101));
            reportConfig.AddContentColumn(new Unit(8.5, UnitType.Centimeter), ParagraphAlignment.Center, new Color(252, 207, 101));
            reportConfig.AddContentColumn(new Unit(2.5, UnitType.Centimeter), ParagraphAlignment.Center, new Color(252, 207, 101));
            reportConfig.AddContentColumn(new Unit(2.37, UnitType.Centimeter), ParagraphAlignment.Center, new Color(252, 207, 101));
            reportConfig.AddContentColumn(new Unit(2.37, UnitType.Centimeter), ParagraphAlignment.Center, new Color(252, 207, 101));
            reportConfig.AddContentColumn(new Unit(2.37, UnitType.Centimeter), ParagraphAlignment.Center, new Color(252, 207, 101));
            reportConfig.AddContentColumn(new Unit(2.37, UnitType.Centimeter), ParagraphAlignment.Center, new Color(252, 207, 101));
            reportConfig.AddContentColumn(new Unit(2.37, UnitType.Centimeter), ParagraphAlignment.Center, new Color(252, 207, 101));
            reportConfig.AddContentColumn(new Unit(5.63, UnitType.Centimeter), ParagraphAlignment.Center, new Color(252, 207, 101));

            reportConfig.AddContentRow(new Unit(0.2, UnitType.Inch));
            reportConfig.AddContent("PART II. OTHER ITEMS NOT AVALABLE AT PS BUT REGULARLY PURCHASED FROM OTHER SOURCES (Note: Please indicate price of items)", 0, new Unit(10, UnitType.Point), true, ParagraphAlignment.Left, VerticalAlignment.Center, 8);

            reportConfig.AddContentRow(new Unit(0.2, UnitType.Inch));
            reportConfig.AddContent("Item Code", 0, new Unit(10, UnitType.Point), true, ParagraphAlignment.Center, VerticalAlignment.Center, null, 1);
            reportConfig.AddContent("Item and Specifications", 1, new Unit(10, UnitType.Point), true, ParagraphAlignment.Center, VerticalAlignment.Center, null, 1);
            reportConfig.AddContent("Unit of Measure", 2, new Unit(10, UnitType.Point), true, ParagraphAlignment.Center, VerticalAlignment.Center, null, 1);
            reportConfig.AddContent("Quantity Requirement", 3, new Unit(10, UnitType.Point), true, ParagraphAlignment.Center, VerticalAlignment.Center, 4);
            reportConfig.AddContent("Remarks", 8, new Unit(10, UnitType.Point), true, ParagraphAlignment.Center, VerticalAlignment.Center, null, 1);

            reportConfig.AddContentRow(new Unit(0.2, UnitType.Inch));
            reportConfig.AddContent("Qtr 1", 3, new Unit(10, UnitType.Point), true, ParagraphAlignment.Center, VerticalAlignment.Center);
            reportConfig.AddContent("Qtr 2", 4, new Unit(10, UnitType.Point), true, ParagraphAlignment.Center, VerticalAlignment.Center);
            reportConfig.AddContent("Qtr 3", 5, new Unit(10, UnitType.Point), true, ParagraphAlignment.Center, VerticalAlignment.Center);
            reportConfig.AddContent("Qtr 4", 6, new Unit(10, UnitType.Point), true, ParagraphAlignment.Center, VerticalAlignment.Center);
            reportConfig.AddContent("Total", 7, new Unit(10, UnitType.Point), true, ParagraphAlignment.Center, VerticalAlignment.Center);

            reportConfig.AddTable(true);
            reportConfig.AddContentColumn(new Unit(2, UnitType.Centimeter));
            reportConfig.AddContentColumn(new Unit(8.5, UnitType.Centimeter));
            reportConfig.AddContentColumn(new Unit(2.5, UnitType.Centimeter));
            reportConfig.AddContentColumn(new Unit(2.37, UnitType.Centimeter));
            reportConfig.AddContentColumn(new Unit(2.37, UnitType.Centimeter));
            reportConfig.AddContentColumn(new Unit(2.37, UnitType.Centimeter));
            reportConfig.AddContentColumn(new Unit(2.37, UnitType.Centimeter));
            reportConfig.AddContentColumn(new Unit(2.37, UnitType.Centimeter));
            reportConfig.AddContentColumn(new Unit(5.63, UnitType.Centimeter));

            List<PPMPCSEDetails> NonDBMItems = FMISdb.PPMPCSEDetails.Where(d => d.FKPPMPReference.ID == ppmpVM.PPMPHeader.PPMPId && d.FKItem.ProcurementSource == ProcurementSources.Non_DBM).ToList();
            if (NonDBMItems.Count == 0)
            {
                reportConfig.AddContentRow(new Unit(0.2, UnitType.Inch));
                reportConfig.AddContent("*** NO ITEMS ***", 0, new Unit(10, UnitType.Point), true, ParagraphAlignment.Center, VerticalAlignment.Center, 8);
            }
            else
            {
                foreach (var item in NonDBMItems)
                {
                    reportConfig.AddContentRow(new Unit(0.2, UnitType.Inch));
                    var itemName = item.FKItem.ItemName + ((String.IsNullOrEmpty(item.FKItem.ItemShortSpecifications)) ? "" : ", " + item.FKItem.ItemShortSpecifications);
                    reportConfig.AddContent(item.FKItem.ItemCode, 0, new Unit(10, UnitType.Point), false, ParagraphAlignment.Center, VerticalAlignment.Center);
                    reportConfig.AddContent(itemName, 1, new Unit(10, UnitType.Point), false, ParagraphAlignment.Justify, VerticalAlignment.Center);
                    reportConfig.AddContent(item.FKItem.FKIndividualUnitReference.UnitName, 2, new Unit(10, UnitType.Point), false, ParagraphAlignment.Center, VerticalAlignment.Center);
                    reportConfig.AddContent(item.Qtr1.ToString(), 3, new Unit(10, UnitType.Point), false, ParagraphAlignment.Center, VerticalAlignment.Center);
                    reportConfig.AddContent(item.Qtr2.ToString(), 4, new Unit(10, UnitType.Point), false, ParagraphAlignment.Center, VerticalAlignment.Center);
                    reportConfig.AddContent(item.Qtr3.ToString(), 5, new Unit(10, UnitType.Point), false, ParagraphAlignment.Center, VerticalAlignment.Center);
                    reportConfig.AddContent(item.Qtr4.ToString(), 6, new Unit(10, UnitType.Point), false, ParagraphAlignment.Center, VerticalAlignment.Center);
                    reportConfig.AddContent(item.TotalQty.ToString(), 7, new Unit(10, UnitType.Point), true, ParagraphAlignment.Center, VerticalAlignment.Center);
                    reportConfig.AddContent((item.Remarks == "Acceptable" || String.IsNullOrEmpty(item.Remarks)) ? "" : item.Remarks, 8, new Unit(10, UnitType.Point), false, ParagraphAlignment.Justify, VerticalAlignment.Center);
                }
            }

            reportConfig.AddTable();
            reportConfig.AddContentColumn(new Unit(0.5, UnitType.Inch));
            reportConfig.AddContentColumn(new Unit(10, UnitType.Inch));

            reportConfig.AddContentRow();
            reportConfig.AddContent("\n", 0);
            reportConfig.AddContentRow();
            reportConfig.AddContent("", 0);
            reportConfig.AddContent("1. Technical Specifications for each Item/Project being proposed shall  be submitted as part of the PPMP.", 1, new Unit(8, UnitType.Point));
            reportConfig.AddContentRow();
            reportConfig.AddContent("", 0);
            reportConfig.AddContent("2. Technical Specifications however,  must be in generic form;  no brand name shall be specified.", 1, new Unit(8, UnitType.Point));
            reportConfig.AddContentRow();
            reportConfig.AddContent("", 0);
            reportConfig.AddContent("3. Non-submission of PPMP for supplies shall mean no budget provision for supplies.", 1, new Unit(8, UnitType.Point));
            reportConfig.AddContentRow();
            reportConfig.AddContent("", 0);
            reportConfig.AddContent("4. Final quantity of items specified is subject to budget approval.", 1, new Unit(8, UnitType.Point));

            reportConfig.AddTable();
            reportConfig.AddContentColumn(new Unit(0.5, UnitType.Inch));
            reportConfig.AddContentColumn(new Unit(4, UnitType.Inch));
            reportConfig.AddContentColumn(new Unit(3, UnitType.Inch));
            reportConfig.AddContentColumn(new Unit(4, UnitType.Inch));

            reportConfig.AddContentRow();
            reportConfig.AddContent("\n\n", 0);
            reportConfig.AddContentRow();
            reportConfig.AddContent("", 0);
            reportConfig.AddContent("Prepared by:", 1, new Unit(10, UnitType.Point), false, ParagraphAlignment.Left, false);
            reportConfig.AddContent("", 2);
            reportConfig.AddContent("Submitted by:", 3, new Unit(10, UnitType.Point), false, ParagraphAlignment.Left, false);
            reportConfig.AddContentRow(new Unit(0.5, UnitType.Inch));
            reportConfig.AddContent("", 0);
            reportConfig.AddContent("", 1, new Unit(10, UnitType.Point), false, ParagraphAlignment.Center, true);
            reportConfig.AddContent("", 2);
            reportConfig.AddContent("", 3, new Unit(10, UnitType.Point), false, ParagraphAlignment.Center, true);
            reportConfig.AddContentRow();
            reportConfig.AddContent("", 0);
            reportConfig.AddContent(ppmpVM.PPMPHeader.PreparedBy.Replace(", ", "\n"), 1, new Unit(10, UnitType.Point), true, ParagraphAlignment.Center, false);
            reportConfig.AddContent("", 2);
            reportConfig.AddContent(ppmpVM.PPMPHeader.SubmittedBy.Replace(", ", "\n"), 3, new Unit(10, UnitType.Point), true, ParagraphAlignment.Center, false);
            reportConfig.AddContentRow();
            reportConfig.AddContent("", 0);
            reportConfig.AddContent(((DateTime)ppmpVM.PPMPHeader.CreatedAt).ToString("dd MMMM yyyy hh:mm tt"), 1, new Unit(7.5, UnitType.Point), false, ParagraphAlignment.Center, false);
            reportConfig.AddContent("", 2);
            reportConfig.AddContent((string.IsNullOrEmpty(ppmpVM.PPMPHeader.SubmittedAt.ToString())) ? "(Submission Pending)" : ((DateTime)ppmpVM.PPMPHeader.SubmittedAt).ToString("dd MMMM yyyy hh:mm tt") , 3, new Unit(7.5, UnitType.Point), false, ParagraphAlignment.Center, false);

            return reportConfig.GenerateReport();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                FMISdb.Dispose();
                HRISdb.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}