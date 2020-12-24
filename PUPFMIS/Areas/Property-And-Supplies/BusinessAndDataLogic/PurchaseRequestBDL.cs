using PUPFMIS.Models;
using PUPFMIS.Models.HRIS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.IO;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;

namespace PUPFMIS.BusinessAndDataLogic
{
    public class PurchaseRequestSuppliesBL : Controller
    {
        private PurchaseRequestSuppliesDAL prDAL = new PurchaseRequestSuppliesDAL();

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
                rows.Add(new ContentCell(item.ItemName, 2, 8, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Left, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                rows.Add(new ContentCell(item.Quantity.ToString(), 3, 8, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                rows.Add(new ContentCell(item.UnitCost.ToString("C", new System.Globalization.CultureInfo("en-ph")), 4, 8, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Right, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                rows.Add(new ContentCell(item.TotalCost.ToString("C", new System.Globalization.CultureInfo("en-ph")), 5, 8, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Right, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                reports.AddRowContent(rows, 0.30);
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
            reports.AddRowContent(rows, 0.15);

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

    public class PurchaseRequestSuppliesDAL : Controller
    {
        private FMISDbContext db = new FMISDbContext();
        private HRISDataAccess hris = new HRISDataAccess();
        private ABISDataAccess abis = new ABISDataAccess();
        private SystemBDL system = new SystemBDL();

        public List<int> GetFiscalYears()
        {
            return db.APPHeader.GroupBy(d => d.FiscalYear).Select(d => d.Key).ToList();
        }
        public List<string> GetQuarters()
        {
            var quarter = new List<string>();
            quarter.Add("January-March");
            quarter.Add("April-June");
            quarter.Add("July-September");
            quarter.Add("October-December");
            return quarter;
        }
        public List<PurchaseRequestDetailsVM> GetCommonSupplies(string Quarter, int FiscalYear)
        {
            var cseItems = new List<PurchaseRequestDetailsVM>();
            if(Quarter == "January-March")
            {
                var appCSEDetails = db.APPCSEDetails
                    .Where(d => d.FKAPPHeaderReference.FiscalYear == FiscalYear)
                    .Select(d => new { d.APPHeaderReference, d.ItemReference, d.PriceCatalogue, d.JanQty, d.FebQty, d.MarQty }).ToList();
                foreach(var item in appCSEDetails)
                {
                    var itemDetails = db.Items.Find(item.ItemReference);
                    cseItems.Add(new PurchaseRequestDetailsVM
                    {
                        ItemCode = itemDetails.ItemCode,
                        ItemName = itemDetails.ItemFullName,
                        ItemSpecifications = itemDetails.ItemSpecifications,
                        UOM = itemDetails.FKIndividualUnitReference.Abbreviation,
                        Quantity = item.JanQty + item.FebQty + item.MarQty,
                        UnitCost = item.PriceCatalogue,
                        TotalCost = ((item.JanQty + item.FebQty + item.MarQty) * item.PriceCatalogue)
                    });
                }
            }
            if (Quarter == "April-June")
            {
                var appCSEDetails = db.APPCSEDetails
                    .Where(d => d.FKAPPHeaderReference.FiscalYear == FiscalYear)
                    .Select(d => new { d.APPHeaderReference, d.ItemReference, d.PriceCatalogue, d.AprQty, d.MayQty, d.JunQty }).ToList();
                foreach (var item in appCSEDetails)
                {
                    var itemDetails = db.Items.Find(item.ItemReference);
                    cseItems.Add(new PurchaseRequestDetailsVM
                    {
                        ItemCode = itemDetails.ItemCode,
                        ItemName = itemDetails.ItemFullName,
                        ItemSpecifications = itemDetails.ItemSpecifications,
                        UOM = itemDetails.FKIndividualUnitReference.Abbreviation,
                        Quantity = item.AprQty + item.MayQty + item.JunQty,
                        UnitCost = item.PriceCatalogue,
                        TotalCost = ((item.AprQty + item.MayQty + item.JunQty) * item.PriceCatalogue)
                    });
                }
            }
            if (Quarter == "July-September")
            {
                var appCSEDetails = db.APPCSEDetails
                    .Where(d => d.FKAPPHeaderReference.FiscalYear == FiscalYear)
                    .Select(d => new { d.APPHeaderReference, d.ItemReference, d.PriceCatalogue, d.JulQty, d.AugQty, d.SepQty }).ToList();
                foreach (var item in appCSEDetails)
                {
                    var itemDetails = db.Items.Find(item.ItemReference);
                    cseItems.Add(new PurchaseRequestDetailsVM
                    {
                        ItemCode = itemDetails.ItemCode,
                        ItemName = itemDetails.ItemFullName,
                        ItemSpecifications = itemDetails.ItemSpecifications,
                        UOM = itemDetails.FKIndividualUnitReference.Abbreviation,
                        Quantity = item.JulQty + item.AugQty + item.SepQty,
                        UnitCost = item.PriceCatalogue,
                        TotalCost = ((item.JulQty + item.AugQty + item.SepQty) * item.PriceCatalogue)
                    });
                }
            }
            if (Quarter == "October-December")
            {
                var appCSEDetails = db.APPCSEDetails
                    .Where(d => d.FKAPPHeaderReference.FiscalYear == FiscalYear)
                    .Select(d => new { d.APPHeaderReference, d.ItemReference, d.PriceCatalogue, d.OctQty, d.NovQty, d.DecQty }).ToList();
                foreach (var item in appCSEDetails)
                {
                    var itemDetails = db.Items.Find(item.ItemReference);
                    cseItems.Add(new PurchaseRequestDetailsVM
                    {
                        ItemCode = itemDetails.ItemCode,
                        ItemName = itemDetails.ItemFullName,
                        ItemSpecifications = itemDetails.ItemSpecifications,
                        UOM = itemDetails.FKIndividualUnitReference.Abbreviation,
                        Quantity = item.OctQty + item.NovQty + item.DecQty,
                        UnitCost = item.PriceCatalogue,
                        TotalCost = ((item.OctQty + item.NovQty + item.DecQty) * item.PriceCatalogue)
                    });
                }
            }
            return cseItems;
        }
        public List<PurchaseRequestHeader> GetPurchaseRequests(string UserEmail)
        {
            var user = db.UserAccounts.Where(d => d.Email == UserEmail).FirstOrDefault();
            var purchaseRequests = db.PurchaseRequestHeader.Where(d => d.Department == user.DepartmentCode).ToList();
            return purchaseRequests;
        }
        public PurchaseRequestVM GetPurchaseRequest(string PRNumber)
        {
            var purchaseRequestVM = new PurchaseRequestVM();
            var header = db.PurchaseRequestHeader.Where(d => d.PRNumber == PRNumber).FirstOrDefault();
            var details = db.PurchaseRequestDetails.Where(d => d.FKPRHeaderReference.PRNumber == PRNumber).ToList();

            purchaseRequestVM.PRDetails = new List<PurchaseRequestDetailsVM>();
            foreach(var detail in details)
            {
                purchaseRequestVM.PRDetails.Add(new PurchaseRequestDetailsVM
                {
                    ItemCode = detail.FKItemReference.ItemCode,
                    ItemName = detail.FKItemReference.ItemFullName,
                    ItemSpecifications = detail.FKItemReference.ItemSpecifications,
                    UOM = detail.FKItemReference.FKIndividualUnitReference.Abbreviation,
                    Quantity = detail.Quantity,
                    UnitCost = detail.UnitCost,
                    TotalCost = detail.TotalCost
                });
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
        public bool PostPurchaseRequest(PurchaseRequestCSEVM PurchaseRequestCSE, string UserEmail)
        {
            var appReference = db.APPHeader.Where(d => d.FiscalYear == PurchaseRequestCSE.FiscalYear && d.APPType == "CSE").FirstOrDefault();
            var fiscalYear = appReference.FiscalYear;
            var fundSource = db.APPDetails.Where(d => d.FKAPPHeaderReference.FiscalYear == PurchaseRequestCSE.FiscalYear && d.PAPCode.Contains("CUOS")).FirstOrDefault().FundSourceReference;
            var user = db.UserAccounts.Where(d => d.Email == UserEmail).FirstOrDefault();
            var department = hris.GetFullDepartmentDetails(user.DepartmentCode);
            var items = PurchaseRequestCSE.CSEItems;
            string purpose = "Procurement of Common-Use Office Supplies for the period of " + PurchaseRequestCSE.Period + " " + PurchaseRequestCSE.FiscalYear.ToString();

            var purchaseRequestHeader = new PurchaseRequestHeader
            {
                FiscalYear = fiscalYear,
                PAPCode = null,
                PRNumber = GeneratePRNumber(),
                Department = department.DepartmentCode,
                FundCluster = abis.GetFundSources(fundSource).FUND_DESC,
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
            foreach (var item in PurchaseRequestCSE.CSEItems)
            {
                var itm = db.Items.Where(d => d.ItemCode == item.ItemCode).FirstOrDefault();
                purchaseRequestDetails.Add(new PurchaseRequestDetails
                {
                    PRHeaderReference = purchaseRequestHeader.ID,
                    ItemReference = itm.ID,
                    UnitReference = itm.FKIndividualUnitReference.ID,
                    Quantity = item.Quantity,
                    UnitCost = Convert.ToDecimal(item.UnitCost),
                    TotalCost = Convert.ToDecimal(item.TotalCost)
                });
            }

            db.PurchaseRequestDetails.AddRange(purchaseRequestDetails);
            if (db.SaveChanges() == 0)
            {
                return false;
            }

            foreach (var item in items)
            {
                var projectItem = db.ProjectPlanItems.Where(d => d.APPReference == appReference.ID && d.FKItemReference.ItemCode == item.ItemCode).FirstOrDefault();
                if (projectItem != null)
                {
                    projectItem.Status = "Purchase Request Submitted";
                    if(PurchaseRequestCSE.Period == "January-March")
                    {
                        projectItem.Q1PRReference = purchaseRequestHeader.ID;
                    }
                    if (PurchaseRequestCSE.Period == "April-June")
                    {
                        projectItem.Q2PRReference = purchaseRequestHeader.ID;
                    }
                    if (PurchaseRequestCSE.Period == "July-September")
                    {
                        projectItem.Q3PRReference = purchaseRequestHeader.ID;
                    }
                    if (PurchaseRequestCSE.Period == "October-December")
                    {
                        projectItem.Q4PRReference = purchaseRequestHeader.ID;
                    }
                    if (db.SaveChanges() == 0)
                    {
                        return false;
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