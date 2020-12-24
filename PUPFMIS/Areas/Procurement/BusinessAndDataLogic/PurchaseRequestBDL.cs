using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using PUPFMIS.Models;
using PUPFMIS.Models.AIS;
using PUPFMIS.Models.HRIS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace PUPFMIS.BusinessAndDataLogic
{
    public class ProcurementPurchaseRequestBL : Controller
    {
        private ProcurementPurchaseRequestDAL prDAL = new ProcurementPurchaseRequestDAL();

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

            foreach (var item in purchaseRequest.PRDetails)
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

    public class ProcurementPurchaseRequestDAL : Controller
    {
        private FMISDbContext db = new FMISDbContext();
        private HRISDataAccess hris = new HRISDataAccess();
        private ABISDataAccess abis = new ABISDataAccess();
        private SystemBDL systemBDL = new SystemBDL();

        public List<PurchaseRequestVM> GetPurchaseRequests(string UserEmail)
        {
            var purchaseRequestVM = new List<PurchaseRequestVM>();
            var user = db.UserAccounts.Where(d => d.Email == UserEmail).FirstOrDefault();
            var userRole = user.FKRoleReference.Role;

            if (userRole == "Procurement Staff")
            {
                var purchaseRequestItemsReferences = db.PurchaseRequestDetails
                    .Where(d => d.FKItemReference.ProcurementSource == ProcurementSources.PS_DBM)
                    .GroupBy(d => d.FKPRHeaderReference.PRNumber)
                    .Select(d => new { PRNumber = d.Key }).ToList()
                    .Select(d => d.PRNumber).ToList();

                var purchaseRequestHeaders = db.PurchaseRequestHeader.Where(d => purchaseRequestItemsReferences.Contains(d.PRNumber) && d.ReceivedAt == null).ToList();
                foreach(var header in purchaseRequestHeaders)
                {
                    var purchaseRequestDetailsVM = new List<PurchaseRequestDetailsVM>();
                    var details = db.PurchaseRequestDetails.Where(d => d.FKPRHeaderReference.PRNumber == header.PRNumber).ToList();
                    foreach (var detail in details)
                    {
                        purchaseRequestDetailsVM.Add(new PurchaseRequestDetailsVM
                        {
                            ItemCode = detail.FKItemReference.ItemCode,
                            ItemName = detail.FKItemReference.ItemFullName,
                            ItemSpecifications = detail.ItemSpecifications,
                            UOM = detail.FKUnitReference.UnitName,
                            Quantity = detail.Quantity,
                            UnitCost = detail.UnitCost,
                            TotalCost = detail.TotalCost
                        });
                    }

                    purchaseRequestVM.Add(new PurchaseRequestVM
                    {
                        PRNumber = header.PRNumber,
                        FundCluster = header.FundCluster,
                        Purpose = header.Purpose,
                        RequestedBy = header.RequestedBy,
                        RequestedByDesignation = header.RequestedByDesignation,
                        RequestedByDepartment = hris.GetDepartmentDetails(header.RequestedByDepartment).Department,
                        ApprovedBy = header.ApprovedBy,
                        ApprovedByDesignation = header.ApprovedByDesignation,
                        ApprovedByDepartment = hris.GetDepartmentDetails(header.ApprovedByDepartment).Department,
                        CreatedAt = header.CreatedAt.ToString("dd MMMM yyyy"),
                        Department = hris.GetDepartmentDetails(header.Department).Department,
                        PRDetails = purchaseRequestDetailsVM
                    }); 
                }
            }

            if (userRole == "Project Coordinator" || userRole == "Project Support")
            {
                var purchaseRequestItemsReferences = db.PurchaseRequestDetails
                   .Where(d => d.FKItemReference.ProcurementSource == ProcurementSources.Non_DBM)
                   .GroupBy(d => d.FKPRHeaderReference.PRNumber)
                   .Select(d => new { PRNumber = d.Key }).ToList()
                   .Select(d => d.PRNumber).ToList();

                var purchaseRequestHeaders = db.PurchaseRequestHeader.Where(d => purchaseRequestItemsReferences.Contains(d.PRNumber) && d.ReceivedAt == null).ToList();
                foreach (var header in purchaseRequestHeaders)
                {
                    var purchaseRequestDetailsVM = new List<PurchaseRequestDetailsVM>();
                    var details = db.PurchaseRequestDetails.Where(d => d.FKPRHeaderReference.PRNumber == header.PRNumber).ToList();
                    foreach (var detail in details)
                    {
                        purchaseRequestDetailsVM.Add(new PurchaseRequestDetailsVM
                        {
                            ItemCode = detail.FKItemReference.ItemCode,
                            ItemName = detail.FKItemReference.ItemFullName,
                            ItemSpecifications = detail.ItemSpecifications,
                            UOM = detail.UnitReference == null ? null : detail.FKUnitReference.UnitName,
                            Quantity = detail.Quantity,
                            UnitCost = detail.UnitCost,
                            TotalCost = detail.TotalCost
                        });
                    }

                    purchaseRequestVM.Add(new PurchaseRequestVM
                    {
                        PRNumber = header.PRNumber,
                        FundCluster = header.FundCluster,
                        Purpose = header.Purpose,
                        RequestedBy = header.RequestedBy,
                        RequestedByDesignation = header.RequestedByDesignation,
                        RequestedByDepartment = hris.GetDepartmentDetails(header.RequestedByDepartment).Department,
                        ApprovedBy = header.ApprovedBy,
                        ApprovedByDesignation = header.ApprovedByDesignation,
                        ApprovedByDepartment = hris.GetDepartmentDetails(header.ApprovedByDepartment).Department,
                        CreatedAt = header.CreatedAt.ToString("dd MMMM yyyy"),
                        Department = hris.GetDepartmentDetails(header.Department).Department,
                        PRDetails = purchaseRequestDetailsVM
                    });
                }
            }

            return purchaseRequestVM;
        }
        public PurchaseRequestVM GetPurchaseRequest(string PRNumber)
        {
            var purchaseRequestVM = new PurchaseRequestVM();
            var header = db.PurchaseRequestHeader.Where(d => d.PRNumber == PRNumber).FirstOrDefault();
            var details = db.PurchaseRequestDetails.Where(d => d.FKPRHeaderReference.PRNumber == PRNumber).ToList();

            purchaseRequestVM.PRDetails = new List<PurchaseRequestDetailsVM>();
            foreach (var detail in details)
            {
                var projectItem = db.ProjectPlanItems.Where(d => d.FKPurchaseRequestReference.PRNumber == detail.FKPRHeaderReference.PRNumber && d.FKItemReference.ItemCode == detail.FKItemReference.ItemCode).FirstOrDefault();
                var projectService = db.ProjectPlanServices.Where(d => d.FKPurchaseRequestReference.PRNumber == detail.FKPRHeaderReference.PRNumber && d.FKItemReference.ItemCode == detail.FKItemReference.ItemCode).FirstOrDefault();

                if (projectItem != null)
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
        public bool PostPRReceive(string PRNumber, string UserEmail)
        {
            var header = db.PurchaseRequestHeader.Where(d => d.PRNumber == PRNumber).FirstOrDefault();
            var user = db.UserAccounts.Where(d => d.Email == UserEmail).FirstOrDefault();
            header.ReceivedAt = DateTime.Now;
            header.ReceivedBy = user.EmpCode;
            if(db.SaveChanges() == 0)
            {
                return false;
            }
            return true;
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
                abis.Dispose();
                hris.Dispose();
                systemBDL.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}