using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using PUPFMIS.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace PUPFMIS.BusinessAndDataLogic
{
    public class PurchaseRequestBL : Controller
    {
        private PurchaseRequestDAL prDAL = new PurchaseRequestDAL();

        public MemoryStream GeneratePurchaseRequest(string PRNumber, string LogoPath, string UserEmail)
        {
            Reports reports = new Reports();
            var purchaseRequest = prDAL.GetPurchaseRequest(PRNumber);
            reports.ReportFilename = "PR-" + purchaseRequest.PRNumber;
            reports.CreateDocument(8.50, 11.00, Orientation.Portrait, 0.25);
            reports.AddDoubleColumnHeader(LogoPath);
            reports.AddColumnHeader(
                new HeaderLine { Content = "\n", Bold = false, Italic = false, FontSize = 10, ParagraphAlignment = ParagraphAlignment.Left },
                new HeaderLine { Content = "\n", Bold = true, Italic = false, FontSize = 10, ParagraphAlignment = ParagraphAlignment.Left }
            );
            reports.AddColumnHeader(
                new HeaderLine { Content = "Republic of the Philippines", Bold = false, Italic = false, FontSize = 8, ParagraphAlignment = ParagraphAlignment.Left },
                new HeaderLine { Content = "Appendix 60", Bold = false, Italic = true, FontSize = 12, ParagraphAlignment = ParagraphAlignment.Left }
            );
            reports.AddColumnHeader(
                new HeaderLine { Content = "POLYTECHNIC UNIVERSITY OF THE PHILIPPINES", Bold = true, Italic = false, FontSize = 10, ParagraphAlignment = ParagraphAlignment.Left },
                new HeaderLine { Content = "Date Printed: " + DateTime.Now.ToString("dd MMMM yyyy hh:mm tt"), Bold = false, Italic = false, FontSize = 8, ParagraphAlignment = ParagraphAlignment.Left }
            );
            reports.AddColumnHeader(
                new HeaderLine { Content = "Anonas St., Sta. Mesa, Manila", Bold = false, Italic = false, FontSize = 6, ParagraphAlignment = ParagraphAlignment.Left },
                new HeaderLine { Content = string.Empty, Bold = true, Italic = false, FontSize = 8, ParagraphAlignment = ParagraphAlignment.Left }
            );

            reports.AddNewLine();

            reports.AddSingleColumnHeader();
            reports.AddColumnHeader(
                new HeaderLine { Content = "PURCHASE REQUEST", Bold = true, Italic = false, FontSize = 12, ParagraphAlignment = ParagraphAlignment.Center }
            );

            reports.AddNewLine();

            var columns = new List<ContentColumn>();
            columns.Add(new ContentColumn(1.00));
            columns.Add(new ContentColumn(3.75));
            columns.Add(new ContentColumn(0.50));
            columns.Add(new ContentColumn(1.00));
            columns.Add(new ContentColumn(1.75));
            reports.AddTable(columns, false);

            var rows = new List<ContentCell>();
            rows.Add(new ContentCell("Entity Name: ", 0, 10, true));
            rows.Add(new ContentCell("POLTYTECHNIC UNIVERSITY OF THE PHILIPPINES", 1, 10, true, true, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, true));
            rows.Add(new ContentCell("", 2));
            rows.Add(new ContentCell("Fund Cluster: ", 3, 10, true));
            rows.Add(new ContentCell(purchaseRequest.FundCluster, 4, 10, true, true, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, true));
            reports.AddRowContent(rows, 0);

            reports.AddNewLine();

            columns = new List<ContentColumn>();
            columns.Add(new ContentColumn(3.00));
            columns.Add(new ContentColumn(3.00));
            columns.Add(new ContentColumn(2.00));
            reports.AddTable(columns, true);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("Office/Section : " + purchaseRequest.ApprovedByDepartment + " - " + purchaseRequest.Department, 0, 10, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Left, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
            rows.Add(new ContentCell("PR No. : " + purchaseRequest.PRNumber + "\n" + "Responsibility Center Code : " + purchaseRequest.RequestedByDepartment, 1, 10, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Left, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
            rows.Add(new ContentCell("Date : " + purchaseRequest.CreatedAt, 2, 10, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Left, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
            reports.AddRowContent(rows, 0.50);

            columns = new List<ContentColumn>();
            columns.Add(new ContentColumn(1.50));
            columns.Add(new ContentColumn(1.50));
            columns.Add(new ContentColumn(2.00));
            columns.Add(new ContentColumn(1.00));
            columns.Add(new ContentColumn(1.00));
            columns.Add(new ContentColumn(1.00));
            reports.AddTable(columns, true);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("Stock/Property No.", 0, 10, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
            rows.Add(new ContentCell("Unit", 1, 10, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
            rows.Add(new ContentCell("Item Description", 2, 10, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
            rows.Add(new ContentCell("Quantity", 3, 10, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
            rows.Add(new ContentCell("Unit Cost", 4, 10, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
            rows.Add(new ContentCell("Total Cost", 5, 10, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
            reports.AddRowContent(rows, 0.40);

            columns = new List<ContentColumn>();
            columns.Add(new ContentColumn(1.50));
            columns.Add(new ContentColumn(1.50));
            columns.Add(new ContentColumn(2.00));
            columns.Add(new ContentColumn(1.00));
            columns.Add(new ContentColumn(1.00));
            columns.Add(new ContentColumn(1.00));
            reports.AddTable(columns, true);

            foreach(var item in purchaseRequest.PRDetails)
            {
                rows = new List<ContentCell>();
                rows.Add(new ContentCell(item.ItemCode, 0, 8, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                rows.Add(new ContentCell(item.UOM, 1, 8, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                rows.Add(new ContentCell(item.ItemName + "\n\n" + item.ItemSpecifications + "\n\nReferences:\n\n" + item.References, 2, 8, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Left, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                rows.Add(new ContentCell(item.Quantity.ToString(), 3, 8, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                rows.Add(new ContentCell(item.UnitCost.ToString("C", new System.Globalization.CultureInfo("en-ph")), 4, 8, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Right, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                rows.Add(new ContentCell(item.TotalCost.ToString("C", new System.Globalization.CultureInfo("en-ph")), 5, 8, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Right, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                reports.AddRowContent(rows, 0.25);
            }

            columns = new List<ContentColumn>();
            columns.Add(new ContentColumn(8.00));
            reports.AddTable(columns, true);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("Purpose: \n\n" + purchaseRequest.Purpose + "\n", 0, 8, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Justify, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
            reports.AddRowContent(rows, 0.5);

            reports.AddNewLine();
            reports.AddNewLine();

            columns = new List<ContentColumn>();
            columns.Add(new ContentColumn(1.50));
            columns.Add(new ContentColumn(3.00));
            columns.Add(new ContentColumn(0.50));
            columns.Add(new ContentColumn(3.00));
            reports.AddTable(columns, false);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("", 0, 10, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Left, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Top));
            rows.Add(new ContentCell("Requested By:", 1, 10, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Left, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Top));
            rows.Add(new ContentCell("", 2, 10, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Left, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Top));
            rows.Add(new ContentCell("Approved By:", 3, 10, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Left, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Top));
            reports.AddRowContent(rows, 0.25);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("Signature: ", 0, 10, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Left, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Bottom));
            rows.Add(new ContentCell("\n\n\n", 1, 10, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Left, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Bottom, 0, 0, true));
            rows.Add(new ContentCell("", 2, 10, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Left, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Top));
            rows.Add(new ContentCell("\n\n\n", 3, 10, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Left, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Bottom, 0, 0, true));
            reports.AddRowContent(rows, 0.50);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("Printed Name: ", 0, 10, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Left, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Bottom));
            rows.Add(new ContentCell(purchaseRequest.RequestedBy, 1, 10, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Bottom, 0, 0, true));
            rows.Add(new ContentCell("", 2, 10, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Left, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Top));
            rows.Add(new ContentCell(purchaseRequest.ApprovedBy, 3, 10, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Bottom, 0, 0, true));
            reports.AddRowContent(rows, 0.15);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("Designation: ", 0, 10, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Left, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Bottom));
            rows.Add(new ContentCell(purchaseRequest.RequestedByDesignation + " / " + purchaseRequest.RequestedByDepartment, 1, 10, false, true, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Bottom, 0, 0, true));
            rows.Add(new ContentCell("", 2, 10, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Left, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Top));
            rows.Add(new ContentCell(purchaseRequest.ApprovedByDesignation + " / " + purchaseRequest.ApprovedByDepartment, 3, 10, false, true, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Bottom, 0, 0, true));
            reports.AddRowContent(rows, 0.15);

            return reports.GenerateReport();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                prDAL.Dispose();
            }
            base.Dispose(disposing);
        }
    }

    public class PurchaseRequestDAL : Controller
    {
        private FMISDbContext db = new FMISDbContext();
        private HRISDataAccess hris = new HRISDataAccess();
        private ABISDataAccess abis = new ABISDataAccess();
        private SystemBDL system = new SystemBDL();

        public List<ProcurementProjectsVM> GetOpenForPRSubmission(string UserEmail)
        {
            var user = db.UserAccounts.Where(d => d.Email == UserEmail).FirstOrDefault();

            var procurementProject = new List<ProcurementProjectsVM>();
            var institutionalProject = db.APPInstitutionalItems.Where(d => d.ProjectStatus == "PR Submission Open").ToList();
            var project = db.APPProjectItems.Where(d => d.ProjectStatus == "PR Submission Open").ToList();

            foreach(var instPrj in institutionalProject)
            {
                var projectItems = new List<ProcurementProjectItemsVM>();
                var PPMPList = instPrj.PPMPReferences.Split("_".ToCharArray(), System.StringSplitOptions.RemoveEmptyEntries).ToArray();
                foreach (var ppmpReference in PPMPList)
                {
                    var items = db.ProjectPlanItems
                             .Where(d => d.FKAPPReference.ID == instPrj.FKAPPHeaderReference.ID
                                    && d.FKPPMPReference.ReferenceNo == ppmpReference
                                    && d.FKItemReference.FKItemTypeReference.AccountClass == instPrj.ObjectClassification
                                    && d.FundSource == instPrj.FundSourceReference
                                    && (instPrj.MOOEAmount != 0.00m ? d.UnitCost < 15000.00m : d.UnitCost >= 15000.00m)
                                    && d.FKProjectReference.Department == user.DepartmentCode)
                             .ToList();

                    var services = db.ProjectPlanServices
                                 .Where(d => d.FKAPPHeaderReference.ID == instPrj.FKAPPHeaderReference.ID
                                        && d.FKPPMPReference.ReferenceNo == ppmpReference
                                        && d.FKItemReference.FKItemTypeReference.AccountClass == instPrj.ObjectClassification
                                        && d.FundSource == instPrj.FundSourceReference
                                        && (instPrj.MOOEAmount != 0.00m ? d.UnitCost < 15000.00m : d.UnitCost >= 15000.00m)
                                        && d.FKProjectReference.Department == user.DepartmentCode)
                                 .ToList();

                    foreach (var item in items)
                    {
                        projectItems.Add(new ProcurementProjectItemsVM
                        {
                            ProjectCode = item.FKProjectReference.ProjectCode,
                            ProjectName = item.FKProjectReference.ProjectName,
                            DeliveryMonth = system.GetMonthName(item.FKProjectReference.ProjectMonthStart),
                            EndUser = hris.GetDepartmentDetails(item.FKProjectReference.Department).Department,
                            ItemCode = item.FKItemReference.ItemCode,
                            ItemName = item.FKItemReference.ItemFullName,
                            ItemSpecifications = item.FKItemReference.ItemShortSpecifications,
                            ProcurementSource = item.FKItemReference.ProcurementSource == ProcurementSources.PS_DBM ? "Procurement System - Department of Budget and Management" : "Private Suppliers",
                            InventoryType = item.FKItemReference.FKItemTypeReference.FKInventoryTypeReference.InventoryTypeName,
                            ItemCategory = item.FKItemReference.FKCategoryReference.ItemCategoryName,
                            PackagingUOMReference = item.FKItemReference.FKPackagingUnitReference.Abbreviation,
                            IndividualUOMReference = item.FKItemReference.FKIndividualUnitReference.Abbreviation,
                            TotalQty = item.PPMPTotalQty,
                            EstimatedBudget = item.PPMPEstimatedBudget,
                            UnitCost = item.UnitCost,
                        });
                    }

                    foreach (var item in services)
                    {
                        projectItems.Add(new ProcurementProjectItemsVM
                        {
                            ProjectCode = item.FKProjectReference.ProjectCode,
                            ProjectName = item.FKProjectReference.ProjectName,
                            DeliveryMonth = system.GetMonthName(item.FKProjectReference.ProjectMonthStart),
                            EndUser = hris.GetDepartmentDetails(item.FKProjectReference.Department).Department,
                            ItemCode = item.FKItemReference.ItemCode,
                            ItemName = item.FKItemReference.ItemFullName,
                            ItemSpecifications = item.FKItemReference.ItemShortSpecifications,
                            ProcurementSource = item.FKItemReference.ProcurementSource == ProcurementSources.PS_DBM ? "Procurement System - Department of Budget and Management" : "Private Suppliers",
                            InventoryType = item.FKItemReference.FKItemTypeReference.FKInventoryTypeReference.InventoryTypeName,
                            ItemCategory = item.FKItemReference.FKCategoryReference.ItemCategoryName,
                            EstimatedBudget = item.PPMPEstimatedBudget,
                            UnitCost = item.UnitCost,
                            TotalQty = item.PPMPQuantity
                        });
                    }
                }
                var procurementModes = instPrj.ModeOfProcurementReference.Split("_".ToCharArray(), System.StringSplitOptions.RemoveEmptyEntries).ToArray();
                string procurementModeList = string.Empty;
                for (int i = 0; i < procurementModes.Count(); i++)
                {
                    if (i == procurementModes.Count() - 1)
                    {
                        procurementModeList += db.ProcurementModes.Find(int.Parse(procurementModes[i])).ModeOfProcurementName;
                    }
                    else
                    {
                        procurementModeList += db.ProcurementModes.Find(int.Parse(procurementModes[i])).ModeOfProcurementName + "\n";
                    }
                }

                var procurementTimeline = db.ProcurementTimeline
                    .Where(d => d.PAPCode == instPrj.PAPCode)
                    .FirstOrDefault();

                var schedule = new ProcurementProjectScheduleVM
                {
                    PurchaseRequestSubmission = procurementTimeline == null ? null : procurementTimeline.PurchaseRequestSubmission.Value.ToString("MMMM dd, yyyy"),
                    PreProcurementConference = procurementTimeline == null ? null : procurementTimeline.PreProcurementConference.Value.ToString("MMMM dd, yyyy"),
                    PostingOfIB = procurementTimeline == null ? null : procurementTimeline.PostingOfIB.Value.ToString("MMMM dd, yyyy"),
                    PreBidConference = procurementTimeline == null ? null : procurementTimeline.PreBidConference.Value.ToString("MMMM dd, yyyy"),
                    SubmissionOfBids = procurementTimeline == null ? null : procurementTimeline.SubmissionOfBids.Value.ToString("MMMM dd, yyyy"),
                    BidEvaluation = procurementTimeline == null ? null : procurementTimeline.BidEvaluation.Value.ToString("MMMM dd, yyyy"),
                    PostQualification = procurementTimeline == null ? null : procurementTimeline.PostQualification.Value.ToString("MMMM dd, yyyy"),
                    NOAIssuance = procurementTimeline == null ? null : procurementTimeline.NOAIssuance.Value.ToString("MMMM dd, yyyy"),
                    ContractSigning = procurementTimeline == null ? null : procurementTimeline.ContractSigning.Value.ToString("MMMM dd, yyyy"),
                    Approval = procurementTimeline == null ? null : procurementTimeline.Approval.Value.ToString("MMMM dd, yyyy"),
                    NTPIssuance = procurementTimeline == null ? null : procurementTimeline.NTPIssuance.Value.ToString("MMMM dd, yyyy"),
                    POReceived = procurementTimeline == null ? null : procurementTimeline.POReceived.Value.ToString("MMMM dd, yyyy")
                };

                procurementProject.Add(new ProcurementProjectsVM
                {
                    APPReference = instPrj.FKAPPHeaderReference.ReferenceNo,
                    Month = instPrj.Month,
                    PAPCode = instPrj.PAPCode,
                    UACS = instPrj.ObjectClassification,
                    ProcurementProgram = instPrj.ProcurementProgram,
                    ApprovedBudget = instPrj.Total,
                    ObjectClassification = abis.GetChartOfAccounts(instPrj.ObjectClassification).AcctName,
                    FundCluster = instPrj.FundSourceReference,
                    FundSource = abis.GetFundSources(instPrj.FundSourceReference).FUND_DESC,
                    EndUser = hris.GetDepartmentDetails(instPrj.EndUser).Department,
                    StartMonth = instPrj.StartMonth,
                    EndMonth = instPrj.EndMonth,
                    ModeOfProcurement = procurementModeList,
                    MOOETotal = instPrj.MOOEAmount,
                    CapitalOutlayTotal = instPrj.COAmount,
                    TotalEstimatedBudget = instPrj.Total,
                    Remarks = instPrj.Remarks,
                    ProjectCoordinator = instPrj.ProjectCoordinator == null ? null : hris.GetEmployeeDetailByCode(instPrj.ProjectCoordinator).EmployeeName,
                    ProjectSupport = instPrj.ProjectSupport == null ? null : hris.GetEmployeeDetailByCode(instPrj.ProjectSupport).EmployeeName,
                    Items = projectItems,
                    Schedule = schedule,
                    ProcurmentProjectType = "Institutional"
                });
            }

            foreach(var proj in project)
            {
                var projectItems = new List<ProcurementProjectItemsVM>();
                var items = db.ProjectPlanItems
                             .Where(d => d.FKAPPReference.ReferenceNo == proj.FKAPPHeaderReference.ReferenceNo
                                    && d.FKProjectReference.ProjectCode == proj.PAPCode
                                    && d.FKItemReference.FKItemTypeReference.AccountClass == proj.ObjectClassification
                                    && d.FundSource == proj.FundSourceReference
                                    && (proj.MOOEAmount != 0.00m ? d.UnitCost < 15000.00m : d.UnitCost >= 15000.00m)
                                    && d.FKProjectReference.Department == user.DepartmentCode)
                             .ToList();

                var services = db.ProjectPlanServices
                             .Where(d => d.FKAPPHeaderReference.ReferenceNo == proj.FKAPPHeaderReference.ReferenceNo
                                    && d.FKProjectReference.ProjectCode == proj.PAPCode
                                    && d.FKItemReference.FKItemTypeReference.AccountClass == proj.ObjectClassification
                                    && d.FundSource == proj.FundSourceReference
                                    && (proj.MOOEAmount != 0.00m ? d.UnitCost < 15000.00m : d.UnitCost >= 15000.00m)
                                    && d.FKProjectReference.Department == user.DepartmentCode)
                             .ToList();

                foreach (var item in items)
                {
                    projectItems.Add(new ProcurementProjectItemsVM
                    {
                        ProjectCode = item.FKProjectReference.ProjectCode,
                        ProjectName = item.FKProjectReference.ProjectName,
                        DeliveryMonth = system.GetMonthName(item.FKProjectReference.ProjectMonthStart),
                        EndUser = hris.GetDepartmentDetails(item.FKProjectReference.Department).Department,
                        ItemCode = item.FKItemReference.ItemCode,
                        ItemName = item.FKItemReference.ItemFullName,
                        ItemSpecifications = item.FKItemReference.ItemShortSpecifications,
                        ProcurementSource = item.FKItemReference.ProcurementSource == ProcurementSources.PS_DBM ? "Procurement System - Department of Budget and Management" : "Private Suppliers",
                        InventoryType = item.FKItemReference.FKItemTypeReference.FKInventoryTypeReference.InventoryTypeName,
                        ItemCategory = item.FKItemReference.FKCategoryReference.ItemCategoryName,
                        PackagingUOMReference = item.FKItemReference.FKPackagingUnitReference.Abbreviation,
                        IndividualUOMReference = item.FKItemReference.FKIndividualUnitReference.Abbreviation,
                        TotalQty = item.PPMPTotalQty,
                        EstimatedBudget = item.PPMPEstimatedBudget,
                        UnitCost = item.UnitCost
                    });
                }

                foreach (var item in services)
                {
                    projectItems.Add(new ProcurementProjectItemsVM
                    {
                        ProjectCode = item.FKProjectReference.ProjectCode,
                        ProjectName = item.FKProjectReference.ProjectName,
                        DeliveryMonth = system.GetMonthName(item.FKProjectReference.ProjectMonthStart),
                        EndUser = hris.GetDepartmentDetails(item.FKProjectReference.Department).Department,
                        ItemCode = item.FKItemReference.ItemCode,
                        ItemName = item.FKItemReference.ItemFullName,
                        ItemSpecifications = item.FKItemReference.ItemShortSpecifications,
                        ProcurementSource = item.FKItemReference.ProcurementSource == ProcurementSources.PS_DBM ? "Procurement System - Department of Budget and Management" : "Private Suppliers",
                        InventoryType = item.FKItemReference.FKItemTypeReference.FKInventoryTypeReference.InventoryTypeName,
                        ItemCategory = item.FKItemReference.FKCategoryReference.ItemCategoryName,
                        EstimatedBudget = item.PPMPEstimatedBudget,
                        UnitCost = item.UnitCost,
                        TotalQty = item.PPMPQuantity
                    });
                }

                var procurementModes = proj.ModeOfProcurementReference.Split("_".ToCharArray(), System.StringSplitOptions.RemoveEmptyEntries).ToArray();
                string procurementModeList = string.Empty;
                for (int i = 0; i < procurementModes.Count(); i++)
                {
                    if (i == procurementModes.Count() - 1)
                    {
                        procurementModeList += db.ProcurementModes.Find(int.Parse(procurementModes[i])).ModeOfProcurementName;
                    }
                    else
                    {
                        procurementModeList += db.ProcurementModes.Find(int.Parse(procurementModes[i])).ModeOfProcurementName + "\n";
                    }
                }

                var procurementTimeline = db.ProcurementTimeline
                    .Where(d => d.PAPCode == proj.PAPCode)
                    .FirstOrDefault();

                var schedule = new ProcurementProjectScheduleVM
                {
                    PurchaseRequestSubmission = procurementTimeline == null ? null : procurementTimeline.PurchaseRequestSubmission.Value.ToString("MMMM dd, yyyy"),
                    PreProcurementConference = procurementTimeline == null ? null : procurementTimeline.PreProcurementConference.Value.ToString("MMMM dd, yyyy"),
                    PostingOfIB = procurementTimeline == null ? null : procurementTimeline.PostingOfIB.Value.ToString("MMMM dd, yyyy"),
                    PreBidConference = procurementTimeline == null ? null : procurementTimeline.PreBidConference.Value.ToString("MMMM dd, yyyy"),
                    SubmissionOfBids = procurementTimeline == null ? null : procurementTimeline.SubmissionOfBids.Value.ToString("MMMM dd, yyyy"),
                    BidEvaluation = procurementTimeline == null ? null : procurementTimeline.BidEvaluation.Value.ToString("MMMM dd, yyyy"),
                    PostQualification = procurementTimeline == null ? null : procurementTimeline.PostQualification.Value.ToString("MMMM dd, yyyy"),
                    NOAIssuance = procurementTimeline == null ? null : procurementTimeline.NOAIssuance.Value.ToString("MMMM dd, yyyy"),
                    ContractSigning = procurementTimeline == null ? null : procurementTimeline.ContractSigning.Value.ToString("MMMM dd, yyyy"),
                    Approval = procurementTimeline == null ? null : procurementTimeline.Approval.Value.ToString("MMMM dd, yyyy"),
                    NTPIssuance = procurementTimeline == null ? null : procurementTimeline.NTPIssuance.Value.ToString("MMMM dd, yyyy"),
                    POReceived = procurementTimeline == null ? null : procurementTimeline.POReceived.Value.ToString("MMMM dd, yyyy")
                };

                procurementProject.Add(new ProcurementProjectsVM
                {
                    APPReference = proj.FKAPPHeaderReference.ReferenceNo,
                    Month = proj.Month,
                    PAPCode = proj.PAPCode,
                    UACS = proj.ObjectClassification,
                    ProcurementProgram = proj.ProcurementProgram,
                    ApprovedBudget = proj.Total,
                    ObjectClassification = abis.GetChartOfAccounts(proj.ObjectClassification).AcctName,
                    FundCluster = proj.FundSourceReference,
                    FundSource = abis.GetFundSources(proj.FundSourceReference).FUND_DESC,
                    EndUser = hris.GetDepartmentDetails(proj.EndUser).Department,
                    StartMonth = proj.StartMonth,
                    EndMonth = proj.EndMonth,
                    ModeOfProcurement = procurementModeList,
                    MOOETotal = proj.MOOEAmount,
                    CapitalOutlayTotal = proj.COAmount,
                    TotalEstimatedBudget = proj.Total,
                    Remarks = proj.Remarks,
                    ProjectCoordinator = proj.ProjectCoordinator == null ? null : hris.GetEmployeeDetailByCode(proj.ProjectCoordinator).EmployeeName,
                    ProjectSupport = proj.ProjectSupport == null ? null : hris.GetEmployeeDetailByCode(proj.ProjectSupport).EmployeeName,
                    Items = projectItems,
                    Schedule = schedule,
                    ProcurmentProjectType = "Project"
                });
            }

            var purchaseRequestPosted = db.PurchaseRequestHeader.Select(d => d.PAPCode).ToList();
            procurementProject = procurementProject.Where(d => !purchaseRequestPosted.Contains(d.PAPCode)).ToList();

            return procurementProject;
        }
        public List<PurchaseRequestHeader> GetPurchaseRequests(string UserEmail)
        {
            var user = db.UserAccounts.Where(d => d.Email == UserEmail).FirstOrDefault();
            return db.PurchaseRequestHeader.Where(d => d.Department == user.DepartmentCode).ToList();
        }
        public PurchaseRequestVM GetPurchaseRequest(string PRNumber)
        {
            var purchaseRequestVM = new PurchaseRequestVM();
            var header = db.PurchaseRequestHeader.Where(d => d.PRNumber == PRNumber).FirstOrDefault();
            var details = db.PurchaseRequestDetails.Where(d => d.FKPRHeaderReference.PRNumber == PRNumber).ToList();

            purchaseRequestVM.PRDetails = new List<PurchaseRequestDetailsVM>();
            foreach(var detail in details)
            {
                var projectItem = db.ProjectPlanItems.Where(d => d.FKPurchaseRequestReference.PRNumber == detail.FKPRHeaderReference.PRNumber && d.FKItemReference.ItemCode == detail.FKItemReference.ItemCode).FirstOrDefault();
                var projectService = db.ProjectPlanServices.Where(d => d.FKPurchaseRequestReference.PRNumber == detail.FKPRHeaderReference.PRNumber && d.FKItemReference.ItemCode == detail.FKItemReference.ItemCode).FirstOrDefault();

                if(projectItem != null)
                {
                    purchaseRequestVM.PRDetails.Add(new PurchaseRequestDetailsVM
                    {
                        ItemCode = projectItem.FKItemReference.ItemCode,
                        ItemName = projectItem.FKItemReference.ItemFullName,
                        ItemSpecifications = projectItem.FKItemReference.ItemSpecifications,
                        UOM = projectItem.FKItemReference.FKIndividualUnitReference.Abbreviation,
                        Quantity = detail.Quantity,
                        UnitCost = detail.UnitCost,
                        TotalCost = detail.TotalCost,
                        References = projectItem.FKProjectReference.ProjectCode + "\n" + projectItem.FKPPMPReference.ReferenceNo + "\n" + header.PAPCode
                    });
                }
                if (projectService != null)
                {
                    purchaseRequestVM.PRDetails.Add(new PurchaseRequestDetailsVM
                    {
                        ItemCode = projectService.FKItemReference.ItemCode,
                        ItemName = projectService.FKItemReference.ItemFullName,
                        ItemSpecifications = projectService.ItemSpecifications,
                        UOM = string.Empty,
                        Quantity = detail.Quantity,
                        UnitCost = detail.UnitCost,
                        TotalCost = detail.TotalCost,
                        References = projectService.FKProjectReference.ProjectCode + "\n" + projectService.FKPPMPReference.ReferenceNo + "\n" + header.PAPCode
                    });
                }
            }

            purchaseRequestVM.PRNumber = header.PRNumber;
            purchaseRequestVM.Department = hris.GetDepartmentDetails(header.Department).Department;
            purchaseRequestVM.FundCluster = header.FundCluster;
            purchaseRequestVM.Purpose = header.Purpose;
            purchaseRequestVM.RequestedBy = header.RequestedBy;
            purchaseRequestVM.RequestedByDesignation = header.RequestedByDesignation;
            purchaseRequestVM.RequestedByDepartment = header.RequestedByDepartment;
            purchaseRequestVM.ApprovedBy = header.ApprovedBy;
            purchaseRequestVM.ApprovedByDesignation = header.ApprovedByDesignation;
            purchaseRequestVM.ApprovedByDepartment = header.ApprovedByDepartment;
            purchaseRequestVM.CreatedAt = header.CreatedAt.ToString("dd MMM yyyy");

            return purchaseRequestVM;
        }
        public ProcurementProjectsVM GetProcurementProgramDetailsByPAPCode(string PAPCode, string UserEmail)
        {
            var department = db.UserAccounts.Where(d => d.Email == UserEmail).FirstOrDefault().DepartmentCode;

            var procurementProject = new ProcurementProjectsVM();
            var institutionalProject = db.APPInstitutionalItems.Where(d => d.PAPCode == PAPCode).FirstOrDefault();
            var project = db.APPProjectItems.Where(d => d.PAPCode == PAPCode).FirstOrDefault();

            if (institutionalProject != null)
            {
                var projectItems = new List<ProcurementProjectItemsVM>();
                var PPMPList = institutionalProject.PPMPReferences.Split("_".ToCharArray(), System.StringSplitOptions.RemoveEmptyEntries).ToArray();
                foreach (var ppmpReference in PPMPList)
                {
                    var items = db.ProjectPlanItems
                             .Where(d => d.FKAPPReference.ID == institutionalProject.FKAPPHeaderReference.ID
                                    && d.FKPPMPReference.ReferenceNo == ppmpReference
                                    && d.FKItemReference.FKItemTypeReference.AccountClass == institutionalProject.ObjectClassification
                                    && d.FundSource == institutionalProject.FundSourceReference
                                    && (institutionalProject.MOOEAmount != 0.00m ? d.UnitCost < 15000.00m : d.UnitCost >= 15000.00m)
                                    && d.FKProjectReference.Department == department)
                             .ToList();

                    var services = db.ProjectPlanServices
                                 .Where(d => d.FKAPPHeaderReference.ID == institutionalProject.FKAPPHeaderReference.ID
                                        && d.FKPPMPReference.ReferenceNo == ppmpReference
                                        && d.FKItemReference.FKItemTypeReference.AccountClass == institutionalProject.ObjectClassification
                                        && d.FundSource == institutionalProject.FundSourceReference
                                        && (institutionalProject.MOOEAmount != 0.00m ? d.UnitCost < 15000.00m : d.UnitCost >= 15000.00m)
                                        && d.FKProjectReference.Department == department)
                                 .ToList();

                    foreach (var item in items)
                    {
                        projectItems.Add(new ProcurementProjectItemsVM
                        {
                            ProjectCode = item.FKProjectReference.ProjectCode,
                            ProjectName = item.FKProjectReference.ProjectName,
                            DeliveryMonth = system.GetMonthName(item.FKProjectReference.ProjectMonthStart),
                            EndUser = hris.GetDepartmentDetails(item.FKProjectReference.Department).Department,
                            ItemCode = item.FKItemReference.ItemCode,
                            ItemName = item.FKItemReference.ItemFullName,
                            ItemSpecifications = item.FKItemReference.ItemSpecifications,
                            ProcurementSource = item.FKItemReference.ProcurementSource == ProcurementSources.PS_DBM ? "Procurement System - Department of Budget and Management" : "Private Suppliers",
                            InventoryType = item.FKItemReference.FKItemTypeReference.FKInventoryTypeReference.InventoryTypeName,
                            ItemCategory = item.FKItemReference.FKCategoryReference.ItemCategoryName,
                            PackagingUOMReference = item.FKItemReference.FKPackagingUnitReference.Abbreviation,
                            IndividualUOMReference = item.FKItemReference.FKIndividualUnitReference.Abbreviation,
                            Purpose = item.Justification,
                            TotalQty = item.PPMPTotalQty,
                            EstimatedBudget = item.PPMPEstimatedBudget,
                            UnitCost = item.UnitCost
                        });
                    }

                    foreach (var item in services)
                    {
                        projectItems.Add(new ProcurementProjectItemsVM
                        {
                            ProjectCode = item.FKProjectReference.ProjectCode,
                            ProjectName = item.FKProjectReference.ProjectName,
                            DeliveryMonth = system.GetMonthName(item.FKProjectReference.ProjectMonthStart),
                            EndUser = hris.GetDepartmentDetails(item.FKProjectReference.Department).Department,
                            ItemCode = item.FKItemReference.ItemCode,
                            ItemName = item.FKItemReference.ItemFullName,
                            ItemSpecifications = item.ItemSpecifications,
                            ProcurementSource = item.FKItemReference.ProcurementSource == ProcurementSources.PS_DBM ? "Procurement System - Department of Budget and Management" : "Private Suppliers",
                            InventoryType = item.FKItemReference.FKItemTypeReference.FKInventoryTypeReference.InventoryTypeName,
                            ItemCategory = item.FKItemReference.FKCategoryReference.ItemCategoryName,
                            Purpose = item.Justification,
                            EstimatedBudget = item.PPMPEstimatedBudget,
                            UnitCost = item.UnitCost,
                            TotalQty = item.PPMPQuantity
                        });
                    }
                }
                var procurementModes = institutionalProject.ModeOfProcurementReference.Split("_".ToCharArray(), System.StringSplitOptions.RemoveEmptyEntries).ToArray();
                string procurementModeList = string.Empty;
                for (int i = 0; i < procurementModes.Count(); i++)
                {
                    if (i == procurementModes.Count() - 1)
                    {
                        procurementModeList += db.ProcurementModes.Find(int.Parse(procurementModes[i])).ModeOfProcurementName;
                    }
                    else
                    {
                        procurementModeList += db.ProcurementModes.Find(int.Parse(procurementModes[i])).ModeOfProcurementName + "\n";
                    }
                }

                var procurementTimeline = db.ProcurementTimeline
                    .Where(d => d.PAPCode == institutionalProject.PAPCode)
                    .FirstOrDefault();

                var schedule = new ProcurementProjectScheduleVM
                {
                    PurchaseRequestSubmission = procurementTimeline == null ? null : procurementTimeline.PurchaseRequestSubmission.Value.ToString("MMMM dd, yyyy"),
                    PreProcurementConference = procurementTimeline == null ? null : procurementTimeline.PreProcurementConference.Value.ToString("MMMM dd, yyyy"),
                    PostingOfIB = procurementTimeline == null ? null : procurementTimeline.PostingOfIB.Value.ToString("MMMM dd, yyyy"),
                    PreBidConference = procurementTimeline == null ? null : procurementTimeline.PreBidConference.Value.ToString("MMMM dd, yyyy"),
                    SubmissionOfBids = procurementTimeline == null ? null : procurementTimeline.SubmissionOfBids.Value.ToString("MMMM dd, yyyy"),
                    BidEvaluation = procurementTimeline == null ? null : procurementTimeline.BidEvaluation.Value.ToString("MMMM dd, yyyy"),
                    PostQualification = procurementTimeline == null ? null : procurementTimeline.PostQualification.Value.ToString("MMMM dd, yyyy"),
                    NOAIssuance = procurementTimeline == null ? null : procurementTimeline.NOAIssuance.Value.ToString("MMMM dd, yyyy"),
                    ContractSigning = procurementTimeline == null ? null : procurementTimeline.ContractSigning.Value.ToString("MMMM dd, yyyy"),
                    Approval = procurementTimeline == null ? null : procurementTimeline.Approval.Value.ToString("MMMM dd, yyyy"),
                    NTPIssuance = procurementTimeline == null ? null : procurementTimeline.NTPIssuance.Value.ToString("MMMM dd, yyyy"),
                    POReceived = procurementTimeline == null ? null : procurementTimeline.POReceived.Value.ToString("MMMM dd, yyyy")
                };

                procurementProject = new ProcurementProjectsVM()
                {
                    APPReference = institutionalProject.FKAPPHeaderReference.ReferenceNo,
                    Month = institutionalProject.Month,
                    PAPCode = institutionalProject.PAPCode,
                    UACS = institutionalProject.ObjectClassification,
                    ProcurementProgram = institutionalProject.ProcurementProgram,
                    ApprovedBudget = institutionalProject.Total,
                    ObjectClassification = abis.GetChartOfAccounts(institutionalProject.ObjectClassification).AcctName,
                    FundCluster = institutionalProject.FundSourceReference,
                    FundSource = abis.GetFundSources(institutionalProject.FundSourceReference).FUND_DESC,
                    EndUser = hris.GetDepartmentDetails(institutionalProject.EndUser).Department,
                    StartMonth = institutionalProject.StartMonth,
                    EndMonth = institutionalProject.EndMonth,
                    ModeOfProcurement = procurementModeList,
                    MOOETotal = institutionalProject.MOOEAmount,
                    CapitalOutlayTotal = institutionalProject.COAmount,
                    TotalEstimatedBudget = institutionalProject.Total,
                    Remarks = institutionalProject.Remarks,
                    ProjectCoordinator = institutionalProject.ProjectCoordinator == null ? null : hris.GetEmployeeDetailByCode(institutionalProject.ProjectCoordinator).EmployeeName,
                    ProjectSupport = institutionalProject.ProjectSupport == null ? null : hris.GetEmployeeDetailByCode(institutionalProject.ProjectSupport).EmployeeName,
                    Items = projectItems,
                    Schedule = schedule
                };
            }

            if (project != null)
            {
                var projectItems = new List<ProcurementProjectItemsVM>();
                var items = db.ProjectPlanItems
                             .Where(d => d.FKAPPReference.ReferenceNo == project.FKAPPHeaderReference.ReferenceNo
                                    && d.FKProjectReference.ProjectCode == project.PAPCode
                                    && d.FKItemReference.FKItemTypeReference.AccountClass == project.ObjectClassification
                                    && d.FundSource == project.FundSourceReference
                                    && (project.MOOEAmount != 0.00m ? d.UnitCost < 15000.00m : d.UnitCost >= 15000.00m)
                                    && d.FKProjectReference.Department == department)
                             .ToList();

                var services = db.ProjectPlanServices
                             .Where(d => d.FKAPPHeaderReference.ReferenceNo == project.FKAPPHeaderReference.ReferenceNo
                                    && d.FKProjectReference.ProjectCode == project.PAPCode
                                    && d.FKItemReference.FKItemTypeReference.AccountClass == project.ObjectClassification
                                    && d.FundSource == project.FundSourceReference
                                    && (project.MOOEAmount != 0.00m ? d.UnitCost < 15000.00m : d.UnitCost >= 15000.00m)
                                    && d.FKProjectReference.Department == department)
                             .ToList();

                foreach (var item in items)
                {
                    projectItems.Add(new ProcurementProjectItemsVM
                    {
                        ProjectCode = item.FKProjectReference.ProjectCode,
                        ProjectName = item.FKProjectReference.ProjectName,
                        DeliveryMonth = system.GetMonthName(item.FKProjectReference.ProjectMonthStart),
                        EndUser = hris.GetDepartmentDetails(item.FKProjectReference.Department).Department,
                        ItemCode = item.FKItemReference.ItemCode,
                        ItemName = item.FKItemReference.ItemFullName,
                        ItemSpecifications = item.FKItemReference.ItemSpecifications,
                        ProcurementSource = item.FKItemReference.ProcurementSource == ProcurementSources.PS_DBM ? "Procurement System - Department of Budget and Management" : "Private Suppliers",
                        InventoryType = item.FKItemReference.FKItemTypeReference.FKInventoryTypeReference.InventoryTypeName,
                        ItemCategory = item.FKItemReference.FKCategoryReference.ItemCategoryName,
                        PackagingUOMReference = item.FKItemReference.FKPackagingUnitReference.Abbreviation,
                        IndividualUOMReference = item.FKItemReference.FKIndividualUnitReference.Abbreviation,
                        Purpose = item.Justification,
                        TotalQty = item.PPMPTotalQty,
                        EstimatedBudget = item.PPMPEstimatedBudget,
                        UnitCost = item.UnitCost
                    });
                }

                foreach (var item in services)
                {
                    projectItems.Add(new ProcurementProjectItemsVM
                    {
                        ProjectCode = item.FKProjectReference.ProjectCode,
                        ProjectName = item.FKProjectReference.ProjectName,
                        DeliveryMonth = system.GetMonthName(item.FKProjectReference.ProjectMonthStart),
                        EndUser = hris.GetDepartmentDetails(item.FKProjectReference.Department).Department,
                        ItemCode = item.FKItemReference.ItemCode,
                        ItemName = item.FKItemReference.ItemFullName,
                        ItemSpecifications = item.ItemSpecifications,
                        ProcurementSource = item.FKItemReference.ProcurementSource == ProcurementSources.PS_DBM ? "Procurement System - Department of Budget and Management" : "Private Suppliers",
                        InventoryType = item.FKItemReference.FKItemTypeReference.FKInventoryTypeReference.InventoryTypeName,
                        ItemCategory = item.FKItemReference.FKCategoryReference.ItemCategoryName,
                        Purpose = item.Justification,
                        EstimatedBudget = item.PPMPEstimatedBudget,
                        UnitCost = item.UnitCost,
                        TotalQty = item.PPMPQuantity
                    });
                }

                var procurementModes = project.ModeOfProcurementReference.Split("_".ToCharArray(), System.StringSplitOptions.RemoveEmptyEntries).ToArray();
                string procurementModeList = string.Empty;
                for (int i = 0; i < procurementModes.Count(); i++)
                {
                    if (i == procurementModes.Count() - 1)
                    {
                        procurementModeList += db.ProcurementModes.Find(int.Parse(procurementModes[i])).ModeOfProcurementName;
                    }
                    else
                    {
                        procurementModeList += db.ProcurementModes.Find(int.Parse(procurementModes[i])).ModeOfProcurementName + "\n";
                    }
                }
                procurementProject = new ProcurementProjectsVM()
                {
                    APPReference = project.FKAPPHeaderReference.ReferenceNo,
                    Month = project.Month,
                    PAPCode = project.PAPCode,
                    UACS = project.ObjectClassification,
                    ProcurementProgram = project.ProcurementProgram,
                    ApprovedBudget = project.Total,
                    ObjectClassification = abis.GetChartOfAccounts(project.ObjectClassification).AcctName,
                    FundCluster = project.FundSourceReference,
                    FundSource = abis.GetFundSources(project.FundSourceReference).FUND_DESC,
                    EndUser = hris.GetDepartmentDetails(project.EndUser).Department,
                    StartMonth = project.StartMonth,
                    EndMonth = project.EndMonth,
                    ModeOfProcurement = procurementModeList,
                    MOOETotal = project.MOOEAmount,
                    CapitalOutlayTotal = project.COAmount,
                    TotalEstimatedBudget = project.Total,
                    Remarks = project.Remarks,
                    ProjectCoordinator = project.ProjectCoordinator == null ? null : hris.GetEmployeeDetailByCode(project.ProjectCoordinator).EmployeeName,
                    ProjectSupport = project.ProjectSupport == null ? null : hris.GetEmployeeDetailByCode(project.ProjectSupport).EmployeeName,
                    Items = projectItems
                };
            }

            return procurementProject;
        }
        public bool PostPurchaseRequest(ProcurementProjectsVM ProcurementProgram, string UserEmail)
        {
            var procurementProgram = GetProcurementProgramDetailsByPAPCode(ProcurementProgram.PAPCode, UserEmail);
            var fiscalYear = db.APPHeader.Where(d => d.ReferenceNo == procurementProgram.APPReference).FirstOrDefault().FiscalYear;
            var user = db.UserAccounts.Where(d => d.Email == UserEmail).FirstOrDefault();
            var department = hris.GetFullDepartmentDetails(user.DepartmentCode);
            var dbmItems = procurementProgram.Items.Where(d => d.ProcurementSource != "Private Suppliers").ToList();
            var nonDBMtems = procurementProgram.Items.Where(d => d.ProcurementSource == "Private Suppliers").ToList();
            string purpose = string.Empty;

            if(dbmItems.Count() != 0)
            {
                foreach (var item in dbmItems)
                {
                    purpose += item.Purpose + "; ";
                }

                var purchaseRequestHeader = new PurchaseRequestHeader
                {
                    FiscalYear = fiscalYear,
                    PAPCode = ProcurementProgram.PAPCode,
                    PRNumber = GeneratePRNumber(),
                    ProcurementSource = ProcurementSources.PS_DBM,
                    Department = department.DepartmentCode,
                    FundCluster = procurementProgram.FundSource,
                    Purpose = purpose,
                    RequestedBy = department.DepartmentHead,
                    RequestedByDesignation = department.DepartmentHeadDesignation,
                    RequestedByDepartment = department.DepartmentCode,
                    ApprovedBy = department.SectorHead,
                    ApprovedByDesignation = department.SectorHeadDesignation,
                    ApprovedByDepartment = department.SectorCode,
                    CreatedBy = user.EmpCode,
                    CreatedAt = DateTime.Now
                };

                db.PurchaseRequestHeader.Add(purchaseRequestHeader);
                if (db.SaveChanges() == 0)
                {
                    return false;
                }

                var purchaseRequestDetails = new List<PurchaseRequestDetails>();
                foreach (var item in dbmItems)
                {
                    var itm = db.Items.Where(d => d.ItemCode == item.ItemCode).FirstOrDefault();
                    purchaseRequestDetails.Add(new PurchaseRequestDetails
                    {
                        PRHeaderReference = purchaseRequestHeader.ID,
                        ItemReference = itm.ID,
                        ItemSpecifications = item.ItemSpecifications,
                        UnitReference = itm.FKIndividualUnitReference.ID,
                        Quantity = item.TotalQty,
                        UnitCost = item.UnitCost,
                        TotalCost = item.EstimatedBudget
                    });
                }

                db.PurchaseRequestDetails.AddRange(purchaseRequestDetails);
                if (db.SaveChanges() == 0)
                {
                    return false;
                }

                foreach (var item in dbmItems)
                {
                    var projectItem = db.ProjectPlanItems.Where(d => d.FKProjectReference.ProjectCode == item.ProjectCode && d.FKItemReference.ItemCode == item.ItemCode).FirstOrDefault();
                    var projectService = db.ProjectPlanServices.Where(d => d.FKProjectReference.ProjectCode == item.ProjectCode && d.FKItemReference.ItemCode == item.ItemCode).FirstOrDefault();
                    if (projectItem != null)
                    {
                        projectItem.Status = "Purchase Request Submitted";
                        projectItem.PRReference = purchaseRequestHeader.ID;
                        if (db.SaveChanges() == 0)
                        {
                            return false;
                        }
                    }
                    if (projectService != null)
                    {
                        projectService.Status = "Purchase Request Submitted";
                        projectService.PRReference = purchaseRequestHeader.ID;
                        if (db.SaveChanges() == 0)
                        {
                            return false;
                        }
                    }
                }
            }

            if (nonDBMtems.Count() != 0)
            {
                foreach (var item in nonDBMtems)
                {
                    purpose += item.Purpose + "; ";
                }

                var purchaseRequestHeader = new PurchaseRequestHeader
                {
                    FiscalYear = fiscalYear,
                    PAPCode = ProcurementProgram.PAPCode,
                    PRNumber = GeneratePRNumber(),
                    ProcurementSource = ProcurementSources.Non_DBM,
                    Department = department.DepartmentCode,
                    FundCluster = procurementProgram.FundSource,
                    Purpose = purpose,
                    RequestedBy = department.DepartmentHead,
                    RequestedByDesignation = department.DepartmentHeadDesignation,
                    RequestedByDepartment = department.DepartmentCode,
                    ApprovedBy = department.SectorHead,
                    ApprovedByDesignation = department.SectorHeadDesignation,
                    ApprovedByDepartment = department.SectorCode,
                    CreatedBy = user.EmpCode,
                    CreatedAt = DateTime.Now,
                    ReceivedAt = null,
                    ReceivedBy = null,
                };

                db.PurchaseRequestHeader.Add(purchaseRequestHeader);
                if (db.SaveChanges() == 0)
                {
                    return false;
                }

                var purchaseRequestDetails = new List<PurchaseRequestDetails>();
                foreach (var item in nonDBMtems)
                {
                    var itm = db.Items.Where(d => d.ItemCode == item.ItemCode).FirstOrDefault();
                    purchaseRequestDetails.Add(new PurchaseRequestDetails
                    {
                        PRHeaderReference = purchaseRequestHeader.ID,
                        ItemReference = itm.ID,
                        ItemSpecifications = item.ItemSpecifications,
                        UnitReference = itm.IndividualUOMReference == null ? null : (int?)itm.FKIndividualUnitReference.ID,
                        Quantity = item.TotalQty,
                        UnitCost = item.UnitCost,
                        TotalCost = item.EstimatedBudget
                    });
                }

                db.PurchaseRequestDetails.AddRange(purchaseRequestDetails);
                if (db.SaveChanges() == 0)
                {
                    return false;
                }

                foreach (var item in nonDBMtems)
                {
                    var projectItem = db.ProjectPlanItems.Where(d => d.FKProjectReference.ProjectCode == item.ProjectCode && d.FKItemReference.ItemCode == item.ItemCode).FirstOrDefault();
                    var projectService = db.ProjectPlanServices.Where(d => d.FKProjectReference.ProjectCode == item.ProjectCode && d.FKItemReference.ItemCode == item.ItemCode).FirstOrDefault();
                    if (projectItem != null)
                    {
                        projectItem.Status = "Purchase Request Submitted";
                        projectItem.PRReference = purchaseRequestHeader.ID;
                        if (db.SaveChanges() == 0)
                        {
                            return false;
                        }
                    }
                    if (projectService != null)
                    {
                        projectService.Status = "Purchase Request Submitted";
                        projectService.PRReference = purchaseRequestHeader.ID;
                        if (db.SaveChanges() == 0)
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }
        private string GeneratePRNumber()
        {
            string PRNumber = string.Empty;
            var count = (db.PurchaseRequestHeader.Where(d => d.CreatedAt.Year == DateTime.Now.Year).Count() + 1).ToString();
            var series = count.Length == 1 ? "000" + count : count.Length == 2 ? "00" + count : count.Length == 3 ? "0" + count : count;
            PRNumber = DateTime.Now.Year.ToString().Substring(2, 2) + "-" + DateTime.Now.ToString("MM") + "-" + series;
            return PRNumber;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
                abis.Dispose();
                hris.Dispose();
                system.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}