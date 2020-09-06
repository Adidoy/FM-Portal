using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using PUPFMIS.Models;
using PUPFMIS.Models.HRIS;
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
                new HeaderLine { Content = "Appendix 60", Bold = false, Italic = true, FontSize = 8, ParagraphAlignment = ParagraphAlignment.Left }
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
                new HeaderLine { Content = "PURCHASE REQUEST", Bold = true, Italic = false, FontSize = 11, ParagraphAlignment = ParagraphAlignment.Center }
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
            rows.Add(new ContentCell("POLTYTECHNIC UNIVERSITY OF THE PHILIPPINES", 1, 9, true, true, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, true));
            rows.Add(new ContentCell("", 2));
            rows.Add(new ContentCell("Fund Cluster: ", 3, 9, true));
            rows.Add(new ContentCell(purchaseRequest.FundCluster, 4, 9, true, true, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, true));
            reports.AddRowContent(rows, 0);

            reports.AddNewLine();

            columns = new List<ContentColumn>();
            columns.Add(new ContentColumn(2.25));
            columns.Add(new ContentColumn(3.75));
            columns.Add(new ContentColumn(2.00));
            reports.AddTable(columns, false);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("Office/Section : " + purchaseRequest.ApprovedByDepartment + " - " + purchaseRequest.Department, 0, 8, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Left, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center, 0, 0, true, new Color(0, 0, 0), true, true, true));
            rows.Add(new ContentCell("PR No. : " + purchaseRequest.PRNumber + "\n" + "Responsibility Center Code : " + purchaseRequest.RequestedByDepartment, 1, 8, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Left, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center, 0, 0, true, new Color(0, 0, 0), true, true, true));
            rows.Add(new ContentCell("Date : " + purchaseRequest.CreatedAt, 2, 8, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Left, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center, 0, 0, true, new Color(0, 0, 0), true, true, true));
            reports.AddRowContent(rows, 0.40);

            columns = new List<ContentColumn>();
            columns.Add(new ContentColumn(1.125));
            columns.Add(new ContentColumn(1.125));
            columns.Add(new ContentColumn(3.00));
            columns.Add(new ContentColumn(0.75));
            columns.Add(new ContentColumn(1.00));
            columns.Add(new ContentColumn(1.00));
            reports.AddTable(columns, false);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("Stock/Property No.", 0, 8, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0), true, false, true));
            rows.Add(new ContentCell("Unit", 1, 8, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0), true, false, true));
            rows.Add(new ContentCell("Item Description", 2, 8, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0), true, false, true));
            rows.Add(new ContentCell("Quantity", 3, 8, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0), true, false, true));
            rows.Add(new ContentCell("Unit Cost", 4, 8, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0), true, false, true));
            rows.Add(new ContentCell("Total Cost", 5, 8, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0), true, false, true));
            reports.AddRowContent(rows, 0.40);

            foreach(var item in purchaseRequest.PRDetails)
            {
                columns = new List<ContentColumn>();
                columns.Add(new ContentColumn(1.125));
                columns.Add(new ContentColumn(1.125));
                columns.Add(new ContentColumn(3.00));
                columns.Add(new ContentColumn(0.75));
                columns.Add(new ContentColumn(1.00));
                columns.Add(new ContentColumn(1.00));
                reports.AddTable(columns, false);

                rows = new List<ContentCell>();
                rows.Add(new ContentCell(item.ItemCode, 0, 8, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Top, 0, 2, false, new Color(0, 0, 0), true, true, true));
                rows.Add(new ContentCell(item.UOM, 1, 8, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Top, 0, 2, false, new Color(0, 0, 0), true, true, true));
                rows.Add(new ContentCell(item.ItemName, 2, 8, true, true, MigraDoc.DocumentObjectModel.ParagraphAlignment.Left, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0), true, true, true));
                rows.Add(new ContentCell(item.Quantity.ToString(), 3, 8, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Top, 0, 2, false, new Color(0, 0, 0), true, true, true));
                rows.Add(new ContentCell(item.UnitCost.ToString("C", new System.Globalization.CultureInfo("en-ph")), 4, 8, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Right, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Top, 0, 2, false, new Color(0, 0, 0), true, true, true));
                rows.Add(new ContentCell(item.TotalCost.ToString("C", new System.Globalization.CultureInfo("en-ph")), 5, 8, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Right, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Top, 0, 2, false, new Color(0, 0, 0), true, true, true));
                reports.AddRowContent(rows, 0.10);

                rows = new List<ContentCell>();
                rows.Add(new ContentCell("\n" + item.ItemSpecifications, 2, 8, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Justify, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0), true, false, true));
                reports.AddRowContent(rows, 0.10);

                rows = new List<ContentCell>();
                rows.Add(new ContentCell("\nReferences:\n\n" + item.References, 2, 8, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Left, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0), true, false, true));
                reports.AddRowContent(rows, 0.10);
            }

            columns = new List<ContentColumn>();
            columns.Add(new ContentColumn(8.00));
            reports.AddTable(columns, false);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("Purpose: \n\n" + purchaseRequest.Purpose + "\n", 0, 8, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Justify, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center, 0, 0, true, new Color(0, 0, 0), true, true, true));
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
            rows.Add(new ContentCell("", 0, 8, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Left, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Top));
            rows.Add(new ContentCell("Requested By:", 1, 8, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Left, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Top));
            rows.Add(new ContentCell("", 2, 8, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Left, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Top));
            rows.Add(new ContentCell("Approved By:", 3, 8, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Left, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Top));
            reports.AddRowContent(rows, 0.25);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("Signature: ", 0, 8, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Left, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Bottom));
            rows.Add(new ContentCell("\n\n\n", 1, 8, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Left, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Bottom, 0, 0, true));
            rows.Add(new ContentCell("", 2, 8, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Left, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Top));
            rows.Add(new ContentCell("\n\n\n", 3, 8, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Left, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Bottom, 0, 0, true));
            reports.AddRowContent(rows, 0.25);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("Printed Name: ", 0, 8, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Left, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Bottom));
            rows.Add(new ContentCell(purchaseRequest.RequestedBy, 1, 8, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center, 0, 0, true));
            rows.Add(new ContentCell("", 2, 8, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Left, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Bottom));
            rows.Add(new ContentCell(purchaseRequest.ApprovedBy, 3, 8, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center, 0, 0, true));
            reports.AddRowContent(rows, 0.15);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("Designation: ", 0, 8, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Left, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Bottom));
            rows.Add(new ContentCell(purchaseRequest.RequestedByDesignation + " / " + purchaseRequest.RequestedByDepartment, 1, 7, false, true, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Bottom, 0, 0, true));
            rows.Add(new ContentCell("", 2, 8, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Left, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Bottom));
            rows.Add(new ContentCell(purchaseRequest.ApprovedByDesignation + " / " + purchaseRequest.ApprovedByDepartment, 3, 7, false, true, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Bottom, 0, 0, true));
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

        public List<int> GetFiscalYears(string UserEmail)
        {
            var user = db.UserAccounts.Where(d => d.Email == UserEmail).FirstOrDefault();
            return db.PurchaseRequestHeader.Where(d => d.Department == user.DepartmentCode).GroupBy(d => d.FiscalYear).Select(d => d.Key).ToList();
        }
        public List<ProcurementProgramsVM> GetOpenForPRSubmission(string UserEmail)
        {
            var programs = new List<ProcurementProgramsVM>();
            var user = db.UserAccounts.Where(d => d.Email == UserEmail).FirstOrDefault();
            var items = db.ProjectPlanItems.Where(d => d.Status == "P/R Submission Open" && d.FKPPMPReference.Department == user.DepartmentCode).Select(d => d.APPLineReference).Distinct().ToList();
            var services = db.ProjectPlanServices.Where(d => d.Status == "P/R Submission Open" && d.FKPPMPReference.Department == user.DepartmentCode).Select(d => d.APPLineReference).Distinct().ToList();
            var papCodes = items.Union(services).Distinct().ToList();

            var openPrograms = db.ProcurementPrograms.Where(d => d.ProjectStatus == "P/R Submission Open" && papCodes.Contains(d.PAPCode)).ToList();
            foreach (var item in openPrograms)
            {
                programs.Add(new ProcurementProgramsVM
                {
                    PAPCode = item.PAPCode,
                    ProcurementProgram = item.ProcurementProgram,
                    ProjectCoordinator = hris.GetEmployeeByCode(item.ProjectCoordinator).EmployeeName,
                    FundSource = abis.GetFundSources(item.FundSourceReference).FUND_DESC
                });
            }

            return programs;
        }
        public List<PurchaseRequestHeader> GetPurchaseRequests(int FiscalYear, string UserEmail)
        {
            var user = db.UserAccounts.Where(d => d.Email == UserEmail).FirstOrDefault();
            var purchaseRequests = db.PurchaseRequestHeader.Where(d => d.Department == user.DepartmentCode && d.FiscalYear == FiscalYear).ToList();
            return purchaseRequests.Select(d => new PurchaseRequestHeader
            {
                PRNumber = d.PRNumber,
                FundCluster = abis.GetFundSources(d.FundCluster).FUND_DESC,
                CreatedAt = d.CreatedAt
            }).ToList();
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
            purchaseRequestVM.FundCluster = abis.GetFundSources(header.FundCluster).FUND_DESC;
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
            var procurementItems = new List<ProcurementProjectItemsVM>();
            var procurementProgram = db.ProcurementPrograms.Where(d => d.PAPCode == PAPCode).FirstOrDefault();
            var user = db.UserAccounts.Where(d => d.Email == UserEmail).FirstOrDefault();
            var items = db.ProjectPlanItems.Where(d => d.APPLineReference == PAPCode && d.Status == "P/R Submission Open" && d.FKPPMPReference.Department == user.DepartmentCode).ToList();
            var services = db.ProjectPlanServices.Where(d => d.APPLineReference == PAPCode && d.Status == "P/R Submission Open" && d.FKPPMPReference.Department == user.DepartmentCode).ToList();

            foreach (var item in items)
            {
                procurementItems.Add(new ProcurementProjectItemsVM
                {
                    EndUser = hris.GetDepartmentDetails(item.FKProjectReference.Department).Department + (item.FKProjectReference.Department == item.FKProjectReference.Unit ? string.Empty : " - " + hris.GetDepartmentDetails(item.FKProjectReference.Unit).Department),
                    ProjectCode = item.FKProjectReference.ProjectCode,
                    ProjectName = item.FKProjectReference.ProjectName,
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
                    DeliveryMonth = system.GetMonthName(item.FKProjectReference.ProjectMonthStart) + ", " + item.FKProjectReference.FiscalYear.ToString(),
                    PRNumber = item.PRReference == null ? null : db.PurchaseRequestHeader.Find(item.PRReference).PRNumber,
                    Status = item.Status
                });
            }

            foreach (var item in services)
            {
                procurementItems.Add(new ProcurementProjectItemsVM
                {
                    EndUser = hris.GetDepartmentDetails(item.FKProjectReference.Department).Department + (item.FKProjectReference.Department == item.FKProjectReference.Unit ? string.Empty : " - " + hris.GetDepartmentDetails(item.FKProjectReference.Unit).Department),
                    ProjectCode = item.FKProjectReference.ProjectCode,
                    ProjectName = item.FKProjectReference.ProjectName,
                    ItemCode = item.FKItemReference.ItemCode,
                    ItemName = item.FKItemReference.ItemFullName,
                    ItemSpecifications = item.ItemSpecifications,
                    ProcurementSource = item.FKItemReference.ProcurementSource == ProcurementSources.PS_DBM ? "Procurement System - Department of Budget and Management" : "Private Suppliers",
                    InventoryType = item.FKItemReference.FKItemTypeReference.FKInventoryTypeReference.InventoryTypeName,
                    ItemCategory = item.FKItemReference.FKCategoryReference.ItemCategoryName,
                    PackagingUOMReference = item.FKItemReference.PackagingUOMReference == null ? string.Empty : item.FKItemReference.FKPackagingUnitReference.Abbreviation,
                    IndividualUOMReference = item.FKItemReference.IndividualUOMReference == null ? string.Empty : item.FKItemReference.FKIndividualUnitReference.Abbreviation,
                    TotalQty = item.PPMPQuantity,
                    EstimatedBudget = item.PPMPEstimatedBudget,
                    UnitCost = item.UnitCost,
                    DeliveryMonth = system.GetMonthName(item.FKProjectReference.ProjectMonthStart) + ", " + item.FKProjectReference.FiscalYear.ToString(),
                    PRNumber = item.PRReference == null ? null : db.PurchaseRequestHeader.Find(item.PRReference).PRNumber,
                    Status = item.Status
                });
            }

            var procurementModes = procurementProgram.APPModeOfProcurementReference.Split("_".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).ToList();
            var modesOfProcurement = string.Empty;
            for (int i = 0; i < procurementModes.Count; i++)
            {
                if (i == procurementModes.Count - 1)
                {
                    modesOfProcurement += db.ProcurementModes.Find(int.Parse(procurementModes[i])).ModeOfProcurementName;
                }
                else
                {
                    modesOfProcurement += db.ProcurementModes.Find(int.Parse(procurementModes[i])).ModeOfProcurementName + "\n";
                }
            }


            var procurementProject = new ProcurementProjectsVM()
            {
                APPReference = procurementProgram.FKAPPHeaderReference.ReferenceNo,
                Month = procurementProgram.Month,
                PAPCode = procurementProgram.PAPCode,
                UACS = procurementProgram.ObjectClassification,
                ProcurementProgram = procurementProgram.ProcurementProgram,
                ApprovedBudget = procurementProgram.Total,
                ObjectClassification = abis.GetChartOfAccounts(procurementProgram.ObjectClassification).AcctName,
                FundCluster = procurementProgram.FundSourceReference,
                FundSource = abis.GetFundSources(procurementProgram.FundSourceReference).FUND_DESC,
                EndUser = hris.GetDepartmentDetails(procurementProgram.EndUser).Department,
                StartMonth = procurementProgram.StartMonth,
                EndMonth = procurementProgram.EndMonth,
                APPModeOfProcurement = modesOfProcurement,
                MOOETotal = procurementProgram.MOOEAmount,
                CapitalOutlayTotal = procurementProgram.COAmount,
                TotalEstimatedBudget = procurementProgram.Total,
                Remarks = procurementProgram.Remarks,
                ProjectCoordinator = procurementProgram.ProjectCoordinator == null ? null : hris.GetEmployeeDetailByCode(procurementProgram.ProjectCoordinator).EmployeeName,
                ProjectSupport = procurementProgram.ProjectSupport == null ? null : hris.GetEmployeeDetailByCode(procurementProgram.ProjectSupport).EmployeeName,
                ProjectStatus = procurementProgram.ProjectStatus,
                Items = procurementItems
            };

            return procurementProject;
        }
        public bool PostPurchaseRequest(ProcurementProjectsVM ProcurementProgram, string UserEmail)
        {
            var user = db.UserAccounts.Where(d => d.Email == UserEmail).FirstOrDefault();
            var fiscalYear = db.ProcurementPrograms.Where(d => d.PAPCode == ProcurementProgram.PAPCode).FirstOrDefault().FKAPPHeaderReference.FiscalYear;
            var office = hris.GetFullDepartmentDetails(user.DepartmentCode);
            var officeRoot = hris.GetAllDepartments().Where(d => d.DepartmentCode == office.DepartmentCode).FirstOrDefault().RootID;
            var items = db.ProjectPlanItems.Where(d => d.APPLineReference == ProcurementProgram.PAPCode && d.Status == "P/R Submission Open" && d.FKPPMPReference.Department == user.DepartmentCode).ToList();
            var services = db.ProjectPlanServices.Where(d => d.APPLineReference == ProcurementProgram.PAPCode && d.Status == "P/R Submission Open" && d.FKPPMPReference.Department == user.DepartmentCode).ToList();

            var Purpose = string.Empty;

            for(var i = 0; i < items.Count; i++)
            {
                if(i == items.Count - 1)
                {
                    Purpose += items[i].Justification;
                }
                else
                {
                    Purpose += items[i].Justification + "; ";
                }
            }

            for (var i = 0; i < services.Count; i++)
            {
                if (i == services.Count - 1)
                {
                    Purpose += services[i].Justification;
                }
                else
                {
                    Purpose += services[i].Justification + "; ";
                }
            }

            var requestedBy = string.Empty;
            var requestedByDesignation = string.Empty;
            var requestedByOffice = string.Empty;
            var approvedBy = string.Empty;
            var approvedByDesignation = string.Empty;
            var approvedByOffice = string.Empty;

            if(officeRoot == 0)
            {
                var deptOfficials = hris.GetDepartmentOfficials(office.DepartmentCode).Where(d => d.Designation.Contains("Assistant to the")).FirstOrDefault();
                requestedBy = deptOfficials.EmployeeName;
                requestedByDesignation = deptOfficials.Designation;
                requestedByOffice = office.DepartmentCode;
                approvedBy = office.DepartmentHead;
                approvedByDesignation = office.DepartmentHeadDesignation;
                approvedByOffice = office.DepartmentCode;
            }
            else
            {
                requestedBy = office.DepartmentHead;
                requestedByDesignation = office.DepartmentHeadDesignation;
                requestedByOffice = office.DepartmentCode;
                approvedBy = office.SectorHead;
                approvedByDesignation = office.SectorHeadDesignation;
                approvedByOffice = office.SectorCode;
            }

            var purchaseRequestHeader = new PurchaseRequestHeader
            {
                FiscalYear = fiscalYear,
                PAPCode = ProcurementProgram.PAPCode,
                PRNumber = GeneratePRNumber(),
                Department = office.DepartmentCode,
                FundCluster = ProcurementProgram.FundCluster,
                Purpose = Purpose,
                RequestedBy =  requestedBy,
                RequestedByDesignation = requestedByDesignation,
                RequestedByDepartment = requestedByOffice,
                ApprovedBy = approvedBy,
                ApprovedByDesignation = approvedByDesignation,
                ApprovedByDepartment = approvedByOffice,
                CreatedBy = user.EmpCode,
                CreatedAt = DateTime.Now
            };

            db.PurchaseRequestHeader.Add(purchaseRequestHeader);
            if (db.SaveChanges() == 0)
            {
                return false;
            }

            var purchaseRequestDetails = new List<PurchaseRequestDetails>();

            foreach(var item in ProcurementProgram.Items)
            {
                purchaseRequestDetails.Add(new PurchaseRequestDetails
                {
                    PRHeaderReference = purchaseRequestHeader.ID,
                    ItemReference = db.Items.Where(d => d.ItemCode == item.ItemCode).FirstOrDefault().ID,
                    UnitCost = item.UnitCost,
                    ItemSpecifications = item.ItemSpecifications,
                    Quantity = item.TotalQty,
                    TotalCost = item.EstimatedBudget,
                    UnitReference = item.IndividualUOMReference == null ? null : (int?)db.UOM.Where(d => d.UnitName == item.IndividualUOMReference).FirstOrDefault().ID
                });
            }

            db.PurchaseRequestDetails.AddRange(purchaseRequestDetails);
            if (db.SaveChanges() == 0)
            {
                return false;
            }

            items.ForEach(d => { d.Status = "P/R Submitted"; d.PRReference = purchaseRequestHeader.ID; });
            services.ForEach(d => { d.Status = "P/R Submitted"; d.PRReference = purchaseRequestHeader.ID; });
            db.SaveChanges();

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