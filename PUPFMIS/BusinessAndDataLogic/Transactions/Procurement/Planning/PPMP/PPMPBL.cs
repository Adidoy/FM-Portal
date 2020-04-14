//using MigraDoc.DocumentObjectModel;
//using MigraDoc.DocumentObjectModel.Tables;
//using PUPFMIS.Models;
//using PUPFMIS.Models.HRIS;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Web.Mvc;

//namespace PUPFMIS.BusinessAndDataLogic
//{
//    public class PPMPBL : Controller
//    {
//        private FMISDbContext FMISdb = new FMISDbContext();
//        private HRISDbContext HRISdb = new HRISDbContext();
//        private ReportsConfig reportConfig = new ReportsConfig();

//        public List<PPMPHeaderViewModel> GetMyPPMP()
//        {
//            var OfficeID = FMISdb.UserInformation.Find(FMISdb.UserAccounts.Where(d => d.Email == System.Web.HttpContext.Current.User.Identity.Name).FirstOrDefault().ID).Office;
//            var ppmp = (from users in FMISdb.UserAccounts
//                        join ppmpHeader in FMISdb.PPMPHeader on users.ID equals ppmpHeader.PreparedBy
//                        join types in FMISdb.InventoryTypes on ppmpHeader.PPMPType equals types.ID
//                        where ppmpHeader.OfficeReference == OfficeID
//                        select new
//                        {
//                            ppmpHeader.ID,
//                            ppmpHeader.FiscalYear,
//                            ppmpHeader.ReferenceNo,
//                            ppmpHeader.OfficeReference,
//                            PPMPType = types.InventoryTypeName,
//                            ppmpHeader.CreatedAt,
//                            ppmpHeader.SubmittedAt,
//                            users.FKUserInformationReference.FirstName,
//                            users.FKUserInformationReference.LastName,
//                            users.FKUserInformationReference.Designation,
//                            ppmpHeader.Status
//                        }).AsEnumerable();
//            var offices = HRISdb.OfficeModel.AsEnumerable();
//            return (from headers in ppmp
//                    join office in offices on headers.OfficeReference equals office.ID
//                    select new PPMPHeaderViewModel
//                    {

//                        PPMPId = headers.ID,
//                        ReferenceNo = headers.ReferenceNo,
//                        FiscalYear = headers.FiscalYear,
//                        OfficeName = office.OfficeName,
//                        PPMPType = headers.PPMPType,
//                        PreparedBy = headers.FirstName.ToUpper() + " " + headers.LastName.ToUpper() + ", " + headers.Designation,
//                        SubmittedBy = office.OfficeHead.ToUpper() + ", " + office.Designation,
//                        CreatedAt = headers.CreatedAt,
//                        SubmittedAt = headers.SubmittedAt,
//                        Status = headers.Status
//                    }).ToList();
//        }

//        public PPMPHeaderViewModel GetPPMPHeader(string ReferenceNo)
//        {
//            var ppmp = (from users in FMISdb.UserAccounts
//                        join ppmpHeader in FMISdb.PPMPHeader on users.ID equals ppmpHeader.PreparedBy
//                        join types in FMISdb.InventoryTypes on ppmpHeader.PPMPType equals types.ID
//                        where ppmpHeader.ReferenceNo == ReferenceNo
//                        select new
//                        {
//                            ppmpHeader.ID,
//                            ppmpHeader.FiscalYear,
//                            ppmpHeader.ReferenceNo,
//                            ppmpHeader.OfficeReference,
//                            ppmpHeader.CreatedAt,
//                            ppmpHeader.SubmittedAt,
//                            types.InventoryTypeName,
//                            users.FKUserInformationReference.FirstName,
//                            users.FKUserInformationReference.LastName,
//                            users.FKUserInformationReference.Designation,
//                            ppmpHeader.Status
//                        }).AsEnumerable();
//            var offices = HRISdb.OfficeModel.AsEnumerable();
//            return (from headers in ppmp
//                    join office in offices on headers.OfficeReference equals office.ID
//                    select new PPMPHeaderViewModel
//                    {

//                        PPMPId = headers.ID,
//                        ReferenceNo = headers.ReferenceNo,
//                        FiscalYear = headers.FiscalYear,
//                        OfficeName = office.OfficeName,
//                        PPMPType = headers.InventoryTypeName,
//                        PreparedBy = headers.FirstName.ToUpper() + " " + headers.LastName.ToUpper() + ", " + headers.Designation,
//                        SubmittedBy = office.OfficeHead.ToUpper() + ", " + office.Designation,
//                        CreatedAt = headers.CreatedAt,
//                        SubmittedAt = headers.SubmittedAt,
//                        Status = headers.Status
//                    }).FirstOrDefault();
//        }

//        public void AddToPPMP(ProjectProcurementViewModel project, string UserEmail)
//        {
//            var inventoryTypes = FMISdb.InventoryTypes.ToList();
//            foreach(var type in inventoryTypes)
//            {
//                if(type.ID != 1)
//                {
//                    var ppmp = FMISdb.PPMPHeader.Where(d => d.FiscalYear == project.Header.FiscalYear && d.PPMPType == type.ID).FirstOrDefault();
//                    if(ppmp == null)
//                    {
//                        //create
//                        CreatePPMPCSE(project, UserEmail, type);
//                    }
//                    else if(ppmp.Status == "New")
//                    {
//                        //update
//                        UpdatePPMPCSE(project, UserEmail, type);
//                    }
//                    else
//                    {
//                        CreatePPMPCSE(project, UserEmail, type);
//                        //create additional
//                    }
//                }
//            }
//        }

//        public bool CreatePPMPCSE(ProjectProcurementViewModel project, string UserEmail, InventoryType PPMPType)
//        {
//            PPMPHeader header = new PPMPHeader();
//            PPMPCSEDetails cseDetails = new PPMPCSEDetails();

//            header.FiscalYear = project.Header.FiscalYear;
//            header.PPMPType = PPMPType.ID;
//            header.PreparedBy = FMISdb.UserAccounts.Where(d => d.Email == UserEmail).FirstOrDefault().ID;
//            header.OfficeReference = FMISdb.UserAccounts.Where(d => d.Email == UserEmail).FirstOrDefault().FKUserInformationReference.Office;
//            header.CreatedAt = DateTime.Now;
//            header.ReferenceNo = GenerateReferenceNo(project.Header.FiscalYear, header.OfficeReference, PPMPType.InventoryCode);
//            header.Status = "New";

//            FMISdb.PPMPHeader.Add(header);

//            if (FMISdb.SaveChanges() == 0)
//            {
//                return false;
//            }

//            List<PPMPCSEDetails> ppmpLineItems = new List<PPMPCSEDetails>();
//            foreach (var item in project.Items)
//            {
//                if (item.FKItemReference.FKInventoryTypeReference.InventoryTypeName == "Common Use Office Supplies")
//                {
//                    PPMPCSEDetails ppmpLineItem = new PPMPCSEDetails();
//                    ppmpLineItem.ProjectProcurementProjectItemReference = item.ID;
//                    ppmpLineItem.PPMPID = header.ID;
//                    ppmpLineItem.Item = item.ItemReference;
//                    ppmpLineItem.Qtr1 = (String.IsNullOrEmpty(item.Qtr1.ToString())) ? 0 : (int)item.Qtr1;
//                    ppmpLineItem.Qtr2 = (String.IsNullOrEmpty(item.Qtr2.ToString())) ? 0 : (int)item.Qtr2;
//                    ppmpLineItem.Qtr3 = (String.IsNullOrEmpty(item.Qtr3.ToString())) ? 0 : (int)item.Qtr3;
//                    ppmpLineItem.Qtr4 = (String.IsNullOrEmpty(item.Qtr4.ToString())) ? 0 : (int)item.Qtr4;
//                    ppmpLineItem.TotalQty = (int)item.TotalQty;
//                    ppmpLineItem.Remarks = item.Remarks;
//                    ppmpLineItems.Add(ppmpLineItem);
//                }
//            }

//            FMISdb.PPMPCSEDetails.AddRange(ppmpLineItems);

//            if (FMISdb.SaveChanges() != ppmpLineItems.Count())
//            {
//                return false;
//            }

//            string officeName = HRISdb.OfficeModel.Find(project.Header.Office).OfficeName;
//            PPMPApprovalWorkflow approvalWF = new PPMPApprovalWorkflow();
//            approvalWF.PPMPId = header.ID;
//            approvalWF.Status = "New";
//            approvalWF.Remarks = "New PPMP by " + officeName + " is created.";
//            approvalWF.UpdatedAt = DateTime.Now;
//            approvalWF.ActionMadeBy = (int)header.PreparedBy;
//            approvalWF.ActionMadeByOffice = header.OfficeReference;
//            FMISdb.PPMPApprovalWorkflow.Add(approvalWF);

//            if (FMISdb.SaveChanges() == 1)
//            {
//                return true;
//            }

//            return false;
//        }

//        public bool UpdatePPMPCSE(ProjectProcurementViewModel project, string UserEmail, InventoryType PPMPType)
//        {
//            PPMPHeader header = FMISdb.PPMPHeader.Where(d => d.FiscalYear == project.Header.FiscalYear && d.PPMPType == PPMPType.ID && d.Status == "New").FirstOrDefault();
//            if(header == null)
//            {
//                return false;
//            }

//            List<PPMPCSEDetails> ppmpLineItems = new List<PPMPCSEDetails>();
//            foreach (var item in project.Items)
//            {
//                PPMPCSEDetails ppmpLineItem = new PPMPCSEDetails();
//                ppmpLineItem.ProjectProcurementProjectItemReference = item.ID;
//                ppmpLineItem.PPMPID = header.ID;
//                ppmpLineItem.Item = item.ItemReference;
//                ppmpLineItem.Qtr1 = (String.IsNullOrEmpty(item.Qtr1.ToString())) ? 0 : (int)item.Qtr1;
//                ppmpLineItem.Qtr2 = (String.IsNullOrEmpty(item.Qtr2.ToString())) ? 0 : (int)item.Qtr2;
//                ppmpLineItem.Qtr3 = (String.IsNullOrEmpty(item.Qtr3.ToString())) ? 0 : (int)item.Qtr3;
//                ppmpLineItem.Qtr4 = (String.IsNullOrEmpty(item.Qtr4.ToString())) ? 0 : (int)item.Qtr4;
//                ppmpLineItem.TotalQty = (int)item.TotalQty;
//                ppmpLineItem.Remarks = item.Remarks;
//                ppmpLineItems.Add(ppmpLineItem);
//            }

//            FMISdb.PPMPCSEDetails.AddRange(ppmpLineItems);

//            if (FMISdb.SaveChanges() != ppmpLineItems.Count())
//            {
//                return false;
//            }

//            string officeName = HRISdb.OfficeModel.Find(project.Header.Office).OfficeName;
//            PPMPApprovalWorkflow approvalWF = new PPMPApprovalWorkflow();
//            approvalWF.PPMPId = header.ID;
//            approvalWF.Status = "New";
//            approvalWF.Remarks = header.ReferenceNo + " is updated by " + officeName + ".";
//            approvalWF.UpdatedAt = DateTime.Now;
//            approvalWF.ActionMadeBy = (int)header.PreparedBy;
//            approvalWF.ActionMadeByOffice = header.OfficeReference;
//            FMISdb.PPMPApprovalWorkflow.Add(approvalWF);

//            if (FMISdb.SaveChanges() == 1)
//            {
//                return true;
//            }

//            return false;
//        }

//        public string GenerateReferenceNo(string FiscalYear, int OfficeReference, string TypeCode)
//        {
//            string referenceNo = string.Empty;
//            string officeCode = HRISdb.OfficeModel.Find(OfficeReference).OfficeCode;
//            string seqNo = (FMISdb.PPMPHeader.Where(d => d.FiscalYear == FiscalYear).Count() + 1).ToString();
//            seqNo = seqNo.ToString().Length == 3 ? seqNo : seqNo.ToString().Length == 2 ? "0" + seqNo.ToString() : "00" + seqNo.ToString();
//            referenceNo = "PPMP-" + TypeCode + "-" + officeCode + "-" + seqNo + "-" + FiscalYear;
//            return referenceNo;
//        }
//        protected override void Dispose(bool disposing)
//        {
//            if (disposing)
//            {
//                FMISdb.Dispose();
//                HRISdb.Dispose();
//            }
//            base.Dispose(disposing);
//        }
//    }
//}