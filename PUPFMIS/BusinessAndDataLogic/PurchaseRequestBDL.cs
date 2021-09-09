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
    public class EndUsersPurchaseRequestBL : Controller
    {
        private EndUsersPurchaseRequestDAL prDAL = new EndUsersPurchaseRequestDAL();
        private FMISDbContext db = new FMISDbContext();
        public MemoryStream GeneratePurchaseRequest(string PRNumber, string LogoPath, string UserEmail)
        {
            Reports reports = new Reports();
            var purchaseRequest = prDAL.GetPurchaseRequest(PRNumber);
            var agencyDetails = db.AgencyDetails.FirstOrDefault();
            reports.ReportFilename = "Purchase Request - " + purchaseRequest.PRNumber;
            reports.CreateDocument(8.50, 11.00, Orientation.Portrait, 0.25);

            var recordCount = 10;
            var fetchCount = 0;
            var itemCount = purchaseRequest.PRDetails.Count;
            var pageCount = (purchaseRequest.PRDetails.Count + 9) / recordCount;
            for (int count = 1; count <= pageCount; count++)
            {
                reports.AddDoubleColumnHeader();
                reports.AddColumnHeader(
                    new HeaderLine { Content = "", Bold = true, Italic = false, FontSize = 10, ParagraphAlignment = ParagraphAlignment.Left },
                    new HeaderLine { Content = "Appendix 60", Bold = false, Italic = true, FontSize = 12, ParagraphAlignment = ParagraphAlignment.Left }
                );
                reports.AddColumnHeader(
                    new HeaderLine { Content = "", Bold = false, Italic = false, FontSize = 8, ParagraphAlignment = ParagraphAlignment.Left },
                    new HeaderLine { Content = "Date Printed: " + purchaseRequest.CreatedAt.ToString("dd MMMM yyyy"), Bold = false, Italic = false, FontSize = 8, ParagraphAlignment = ParagraphAlignment.Left }
                );

                reports.AddNewLine();
                reports.AddNewLine();

                reports.AddSingleColumnHeader();
                reports.AddColumnHeader(
                    new HeaderLine { Content = "PURCHASE REQUEST", Bold = true, Italic = false, FontSize = 15, ParagraphAlignment = ParagraphAlignment.Center }
                );

                reports.AddNewLine();
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
                rows.Add(new ContentCell(agencyDetails.AgencyName.ToUpper(), 1, 10, true, true, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, true));
                rows.Add(new ContentCell("", 2));
                rows.Add(new ContentCell("Fund Cluster: ", 3, 10, true));
                rows.Add(new ContentCell(purchaseRequest.FundCluster, 4, 10, true, true, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, true));
                reports.AddRowContent(rows, 0);

                reports.AddNewLine();

                columns = new List<ContentColumn>();
                columns.Add(new ContentColumn(2.25));
                columns.Add(new ContentColumn(3.75));
                columns.Add(new ContentColumn(2.00));
                reports.AddTable(columns, true);

                rows = new List<ContentCell>();
                rows.Add(new ContentCell(new TextWithFormat[]
                {
                    new TextWithFormat("Office/Section : ", true, false, 10),
                    new TextWithFormat(purchaseRequest.ApprovedByDepartment + " - " + purchaseRequest.RequestedByDepartment, false, false, 10),
                }, 0, ParagraphAlignment.Left, VerticalAlignment.Top));
                rows.Add(new ContentCell(new TextWithFormat[]
                {
                    new TextWithFormat("Purchase Request No. : ", true, false, 10),
                    new TextWithFormat(purchaseRequest.PRNumber, false, false, 10),
                    new TextWithFormat("\n", true, false, 10),
                    new TextWithFormat("Responsibility Center Code : ", true, false, 10),
                    new TextWithFormat(purchaseRequest.Department, false, false, 10),
                }, 1, ParagraphAlignment.Left, VerticalAlignment.Top));
                rows.Add(new ContentCell(new TextWithFormat[]
                {
                    new TextWithFormat("Date : ", true, false, 10),
                    new TextWithFormat(purchaseRequest.CreatedAt.ToString("dd MMMM yyyy"), false, false, 10),
                }, 2, ParagraphAlignment.Left, VerticalAlignment.Top));
                reports.AddRowContent(rows, 0.50);

                columns = new List<ContentColumn>();
                columns.Add(new ContentColumn(1.25));
                columns.Add(new ContentColumn(1.00));
                columns.Add(new ContentColumn(3.00));
                columns.Add(new ContentColumn(0.75));
                columns.Add(new ContentColumn(1.00));
                columns.Add(new ContentColumn(1.00));
                reports.AddTable(columns, false);

                rows = new List<ContentCell>();
                rows.Add(new ContentCell("Stock/Property No.", 0, 7.5, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0), true, false, true));
                rows.Add(new ContentCell("Unit", 1, 8, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0), true, false, true));
                rows.Add(new ContentCell("Item Description", 2, 8, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0), true, false, true));
                rows.Add(new ContentCell("Quantity", 3, 8, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0), true, false, true));
                rows.Add(new ContentCell("Unit Cost", 4, 8, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0), true, false, true));
                rows.Add(new ContentCell("Total Cost", 5, 8, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0), true, false, true));
                reports.AddRowContent(rows, 0.30);

                foreach (var item in purchaseRequest.PRDetails.Skip(fetchCount).Take(recordCount).ToList())
                {
                    columns = new List<ContentColumn>();
                    columns.Add(new ContentColumn(1.25));
                    columns.Add(new ContentColumn(1.00));
                    columns.Add(new ContentColumn(3.00));
                    columns.Add(new ContentColumn(0.75));
                    columns.Add(new ContentColumn(1.00));
                    columns.Add(new ContentColumn(1.00));
                    reports.AddTable(columns, false);

                    rows = new List<ContentCell>();
                    rows.Add(new ContentCell(item.ItemCode, 0, 8, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Top, 0, 1, false, new Color(0, 0, 0), true, true, true));
                    rows.Add(new ContentCell(item.Unit, 1, 8, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Top, 0, 1, false, new Color(0, 0, 0), true, true, true));
                    rows.Add(new ContentCell(item.ItemFullName, 2, 8, true, true, MigraDoc.DocumentObjectModel.ParagraphAlignment.Left, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0), true, true, true));
                    rows.Add(new ContentCell(item.Quantity.ToString(), 3, 8, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Top, 0, 1, false, new Color(0, 0, 0), true, true, true));
                    rows.Add(new ContentCell(item.UnitCost.ToString("N", new System.Globalization.CultureInfo("en-ph")), 4, 8, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Right, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Top, 0, 1, false, new Color(0, 0, 0), true, true, true));
                    rows.Add(new ContentCell(item.TotalCost.ToString("C", new System.Globalization.CultureInfo("en-ph")), 5, 8, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Right, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Top, 0, 1, false, new Color(0, 0, 0), true, true, true));
                    reports.AddRowContent(rows, 0.15);

                    rows = new List<ContentCell>();
                    rows.Add(new ContentCell("\n" + item.ItemSpecifications, 2, 7, false, true, MigraDoc.DocumentObjectModel.ParagraphAlignment.Left, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0), true, false, true));
                    reports.AddRowContent(rows, 0.15);
                }

                columns = new List<ContentColumn>();
                columns.Add(new ContentColumn(8.00));
                reports.AddTable(columns, true);

                rows = new List<ContentCell>();
                rows.Add(new ContentCell("****** NOTHING FOLLOWS *******", 0, 9, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                reports.AddRowContent(rows, 0.20);

                columns = new List<ContentColumn>();
                columns.Add(new ContentColumn(1.25));
                columns.Add(new ContentColumn(1.00));
                columns.Add(new ContentColumn(3.00));
                columns.Add(new ContentColumn(0.75));
                columns.Add(new ContentColumn(1.00));
                columns.Add(new ContentColumn(1.00));
                reports.AddTable(columns, true);

                for (var i = purchaseRequest.PRDetails.Skip(fetchCount).Take(recordCount).ToList().Count; i < 9; i++)
                {
                    rows = new List<ContentCell>();
                    rows.Add(new ContentCell(string.Empty, 0, 8, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Top));
                    rows.Add(new ContentCell(string.Empty, 1, 8, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Top));
                    rows.Add(new ContentCell(string.Empty, 2, 8, true, true, MigraDoc.DocumentObjectModel.ParagraphAlignment.Left, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Top));
                    rows.Add(new ContentCell(string.Empty, 3, 8, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Top));
                    rows.Add(new ContentCell(string.Empty, 4, 8, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Right, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Top));
                    rows.Add(new ContentCell(string.Empty, 5, 8, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Right, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Top));
                    reports.AddRowContent(rows, 0.15);
                }

                columns = new List<ContentColumn>();
                columns.Add(new ContentColumn(7.00));
                columns.Add(new ContentColumn(1.00));
                reports.AddTable(columns, true);

                rows = new List<ContentCell>();
                rows.Add(new ContentCell("TOTAL COST:", 0, 9, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Right, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                rows.Add(new ContentCell(purchaseRequest.PRDetails.Sum(d => d.TotalCost).ToString("C", new System.Globalization.CultureInfo("en-ph")), 1, 9, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Right, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                reports.AddRowContent(rows, 0.20);

                columns = new List<ContentColumn>();
                columns.Add(new ContentColumn(8.00));
                reports.AddTable(columns, false);

                rows = new List<ContentCell>();
                rows.Add(new ContentCell("Purpose: \n\n" + purchaseRequest.Purpose + "\n(" + purchaseRequest.ContractCode + " - " + purchaseRequest.ContractName + ")", 0, 9.5, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Left, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center, 0, 0, true, new Color(0, 0, 0), true, false, true));
                reports.AddRowContent(rows, 0.5);

                rows = new List<ContentCell>();
                rows.Add(new ContentCell("Page " + count + " of " + pageCount, 0, 9, true, false, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 0, true, new Color(0, 0, 0), true, false, true));
                reports.AddRowContent(rows, 0.35);

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
                reports.AddRowContent(rows, 0.25);

                rows = new List<ContentCell>();
                rows.Add(new ContentCell("Printed Name: ", 0, 10, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Left, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Bottom));
                rows.Add(new ContentCell(purchaseRequest.RequestedBy, 1, 10, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center, 0, 0, true));
                rows.Add(new ContentCell("", 2, 10, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Left, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Bottom));
                rows.Add(new ContentCell(purchaseRequest.ApprovedBy, 3, 10, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center, 0, 0, true));
                reports.AddRowContent(rows, 0.20);

                rows = new List<ContentCell>();
                rows.Add(new ContentCell("Designation: ", 0, 10, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Left, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Bottom));
                rows.Add(new ContentCell(purchaseRequest.RequestedByDesignation + " / " + purchaseRequest.Department, 1, 10, false, true, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Bottom, 0, 0, true));
                rows.Add(new ContentCell("", 2, 10, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Left, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Bottom));
                rows.Add(new ContentCell(purchaseRequest.ApprovedByDesignation + " / " + purchaseRequest.ApprovedByDepartment, 3, 10, false, true, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Bottom, 0, 0, true));
                reports.AddRowContent(rows, 0.20);

                fetchCount += recordCount;
                if (count < pageCount)
                {
                    reports.CreateDocument(8.50, 13.00, Orientation.Portrait, 0.25);
                }
            }
            
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

    public class EndUsersPurchaseRequestDAL : Controller
    {
        private FMISDbContext db = new FMISDbContext();
        private ABISDataAccess abis = new ABISDataAccess();
        private HRISDataAccess hris = new HRISDataAccess();

        public List<int> GetFiscalYears(string UserEmail)
        {
            var user = db.UserAccounts.Where(d => d.Email == UserEmail).FirstOrDefault();
            return db.PurchaseRequestHeader.Where(d => d.Department == user.DepartmentCode && d.PRStatus > PurchaseRequestStatus.PurchaseRequestCreated).GroupBy(d => d.FiscalYear).Select(d => d.Key).ToList();
        }
        public List<PurchaseRequestHeaderVM> GetPurchaseRequests(int FiscalYear, string UserEmail)
        {
            var user = db.UserAccounts.Where(d => d.Email == UserEmail).FirstOrDefault();
            return db.PurchaseRequestHeader.ToList().Where(d => d.Department == user.DepartmentCode && d.FiscalYear == FiscalYear)
                     .Select(d => new PurchaseRequestHeaderVM
                     {
                         PRNumber = d.PRNumber,
                         PRStatus = d.PRStatus,
                         ContractCode = d.FKProcurementProjectReference.ContractCode,
                         ContractName = d.FKProcurementProjectReference.ContractName,
                         FundCluster = abis.GetFundSources(d.FundCluster).FUND_DESC.Replace("\r\n", ""),
                         CreatedBy = hris.GetEmployeeByCode(d.CreatedBy).EmployeeName,
                         CreatedAt = d.CreatedAt,
                         SubmittedBy = d.SubmittedAt == null ? null : hris.GetEmployeeByCode(d.SubmittedBy).EmployeeName,
                         SubmittedAt = d.SubmittedAt == null ? null : d.SubmittedAt,
                         ReceivedBy = d.ReceivedAt == null ? null : hris.GetEmployeeByCode(d.ReceivedBy).EmployeeName,
                         ReceivedAt = d.ReceivedAt == null ? null : d.ReceivedAt
                     }).ToList();
        }
        public PurchaseRequestVM SetupPurchaseRequest(string ContractCode, string UserEmail)
        {
            var purchaseRequest = new PurchaseRequestVM();
            var user = db.UserAccounts.Where(d => d.Email == UserEmail).FirstOrDefault();
            var contract = db.ProcurementProjects.Where(d => d.ContractCode == ContractCode).FirstOrDefault();
            var contractDetails = db.ProcurementProjectDetails.Where(d => d.ProcurementProjectReference == contract.ID).ToList();
            var office = hris.GetDepartmentDetails(user.DepartmentCode);
            var purposeList = db.PPMPDetails.Where(d => d.FKProcurementProject.ContractCode == contract.ContractCode).Select(d => d.FKProjectDetailsReference.Justification).Distinct().ToList();
            var purpose = String.Join("; ", purposeList);
            purchaseRequest = new PurchaseRequestVM
            {
                ProcurementProjectType = contract.ProcurementProjectType,
                ContractCode = contract.ContractCode,
                ContractName = contract.ContractName,
                Department = office.DepartmentCode,
                FiscalYear = contract.FiscalYear,
                FundSource = contract.FundSource,
                FundCluster = abis.GetFundSources(contract.FundSource).FUND_DESC,
                RequestedBy = office.DepartmentHead,
                RequestedByDesignation = office.DepartmentHeadDesignation,
                RequestedByDepartment = office.Department,
                ApprovedBy = office.SectorHead,
                ApprovedByDesignation = office.SectorHeadDesignation,
                ApprovedByDepartment = office.SectorCode,
                CreatedAt = DateTime.Now,
                Purpose = contract.FKAPPReference.EndUser == user.DepartmentCode ? null : purpose,
                PRDetails = contractDetails.Select(d => new PurchaseRequestDetailsVM
                {
                    ClassificationID = d.FKProcurementProjectReference.ClassificationReference,
                    ItemCode = db.ItemArticles.Find(d.ArticleReference).ArticleCode + "-" + d.ItemSequence,
                    ArticleReference = d.ArticleReference,
                    ItemSequence = d.ItemSequence,
                    ItemFullName = d.ItemFullName,
                    ItemSpecifications = d.ItemSpecifications,
                    UOMReference = d.UOMReference,
                    Unit = d.FKUOMReference.Abbreviation,
                    Quantity = d.Quantity,
                    UnitCost = d.EstimatedUnitCost,
                    TotalCost = d.ApprovedBudgetForItem
                }).OrderBy(x => x.ItemCode).ToList()
            };

            return purchaseRequest;
        }
        public PurchaseRequestVM GetPurchaseRequest(string PRNumber)
        {
            var purchaseRequest = db.PurchaseRequestHeader.ToList().Where(d => d.PRNumber == PRNumber).Select(d => new PurchaseRequestVM
            {
                PRNumber = d.PRNumber,
                Department = d.Department,
                ContractCode = d.FKProcurementProjectReference.ContractCode,
                ContractName = d.FKProcurementProjectReference.ContractName,
                FiscalYear = d.FiscalYear,
                FundSource = d.FundCluster,
                FundCluster = abis.GetFundSources(d.FundCluster).FUND_DESC,
                Purpose = d.Purpose,
                RequestedBy = d.RequestedBy,
                RequestedByDesignation = d.RequestedByDesignation,
                RequestedByDepartment = hris.GetDepartmentDetails(d.RequestedByDepartment).Department,
                ApprovedBy = d.ApprovedBy,
                ApprovedByDesignation = d.ApprovedByDesignation,
                ApprovedByDepartment = d.ApprovedByDepartment,
                CreatedAt = DateTime.Now,
                PRDetails = db.PurchaseRequestDetails.Where(x => x.PRHeaderReference == d.ID).ToList().Select(x => new PurchaseRequestDetailsVM
                {
                    ClassificationID = x.ClassificationID,
                    ItemCode = db.ItemArticles.Find(x.ArticleReference).ArticleCode + "-" + x.ItemSequence,
                    ArticleReference = x.ArticleReference,
                    ItemSequence = x.ItemSequence,
                    ItemFullName = x.ItemFullName,
                    ItemSpecifications = x.ItemSpecifications,
                    UOMReference = x.UOMReference,
                    Unit = x.FKUOMReference.Abbreviation,
                    Quantity = x.Quantity,
                    UnitCost = x.UnitCost,
                    TotalCost = x.TotalCost
                }).ToList()
            }).FirstOrDefault();

            return purchaseRequest;
        }
        public List<PurchaseRequestHeaderVM> GetPurchaseRequests(string UserEmail)
        {
            var user = db.UserAccounts.Where(d => d.Email == UserEmail).FirstOrDefault();
            return db.PurchaseRequestHeader.ToList().Where(d => d.Department == user.DepartmentCode)
                     .Select(d => new PurchaseRequestHeaderVM
                     {
                         PRNumber = d.PRNumber,
                         PRStatus = d.PRStatus,
                         ContractCode = d.FKProcurementProjectReference.ContractCode,
                         ContractName = d.FKProcurementProjectReference.ContractName,
                         FundCluster = abis.GetFundSources(d.FundCluster).FUND_DESC.Replace("\r\n", ""),
                         CreatedBy = hris.GetEmployeeByCode(d.CreatedBy).EmployeeName,
                         CreatedAt = d.CreatedAt,
                         SubmittedBy = d.SubmittedAt == null ? null : hris.GetEmployeeByCode(d.SubmittedBy).EmployeeName,
                         SubmittedAt = d.SubmittedAt == null ? null : d.SubmittedAt,
                         ReceivedBy = d.ReceivedAt == null ? null : hris.GetEmployeeByCode(d.ReceivedBy).EmployeeName,
                         ReceivedAt = d.ReceivedAt == null ? null : d.ReceivedAt
                     }).ToList();
        }
        public bool PostPurchaseRequest(string UserEmail, PurchaseRequestVM PurchaseRequest)
        {
            var user = db.UserAccounts.Where(d => d.Email == UserEmail).FirstOrDefault();
            var contract = db.ProcurementProjects.Where(d => d.ContractCode == PurchaseRequest.ContractCode).FirstOrDefault();
            var purchaseRequestHeader = new PurchaseRequestHeader
            {
                ProcurementProjectReference = contract.ID,
                FiscalYear = PurchaseRequest.FiscalYear,
                PRNumber = GeneratePRNumber(),
                Department = PurchaseRequest.Department,
                FundCluster = PurchaseRequest.FundSource,
                Purpose = PurchaseRequest.Purpose,
                RequestedBy = PurchaseRequest.RequestedBy,
                RequestedByDesignation = PurchaseRequest.RequestedByDesignation,
                RequestedByDepartment = PurchaseRequest.Department,
                ApprovedBy = PurchaseRequest.ApprovedBy,
                ApprovedByDesignation = PurchaseRequest.ApprovedByDesignation,
                ApprovedByDepartment = PurchaseRequest.ApprovedByDepartment,
                CreatedBy = user.EmpCode,
                CreatedAt = DateTime.Now
            };

            db.PurchaseRequestHeader.Add(purchaseRequestHeader);
            if (db.SaveChanges() == 0)
            {
                return false;
            }

            db.PurchaseRequestDetails.AddRange(PurchaseRequest.PRDetails.Select(d => new PurchaseRequestDetails
            {
                PRHeaderReference = purchaseRequestHeader.ID,
                ClassificationID = d.ClassificationID,
                ArticleReference = d.ArticleReference,
                ItemSequence = d.ItemSequence,
                ItemFullName = d.ItemFullName,
                ItemSpecifications = d.ItemSpecifications,
                UOMReference = d.UOMReference,
                Quantity = d.Quantity,
                UnitCost = d.UnitCost,
                TotalCost = d.TotalCost
            }));

            var ppmpDetails = db.PPMPDetails.Where(d => d.FKProcurementProject.ContractCode == PurchaseRequest.ContractCode &&
                                            d.PPMPDetailStatus == PPMPDetailStatus.PostedToProcurementProject &&
                                            d.PurchaseRequestReference == null).ToList();
            ppmpDetails.ForEach(d =>
            {
                d.PurchaseRequestReference = purchaseRequestHeader.ID;
                d.FKProjectDetailsReference.ProjectItemStatus = ProjectDetailsStatus.PostedToPurchaseRequest;
                d.PPMPDetailStatus = PPMPDetailStatus.PostedToPurchaseRequest;
            });

            if (db.SaveChanges() == 0)
            {
                return false;
            }

            var ppmpDetailsCount = db.PPMPDetails.Where(d => d.FKProcurementProject.ContractCode == PurchaseRequest.ContractCode &&
                                            d.PPMPDetailStatus == PPMPDetailStatus.PostedToProcurementProject &&
                                            d.PurchaseRequestReference == null).Count();
            if (ppmpDetailsCount == 0)
            {
                contract.ProcurementProjectStage = contract.ProcurementProjectType == ProcurementProjectTypes.AMP ? ProcurementProjectStages.GenerationOfRFQ : ProcurementProjectStages.PRSubmissionClosed;
                if(contract.ParentProjectReference != null)
                {
                    contract.FKParentProjectReference.ProcurementProjectStage = ProcurementProjectStages.PRSubmissionClosed;
                }
                db.ContractUpdates.Add(new ContractUpdates
                {
                    ProcurementProjectReference = contract.ContractStrategy == ContractStrategies.LotBidding ? contract.FKParentProjectReference.ID : contract.ID,
                    UpdatedAt = DateTime.Now,
                    Remarks = "Purchase Requests for this contract are submitted. SYSTEM automatically closed the submission.",
                    ProcurementProjectStage = ProcurementProjectStages.PRSubmissionClosed,
                    UpdatedBy = "SYSTEM"
                });
                if (db.SaveChanges() == 0)
                {
                    return false;
                }
            }
            return true;
        }
        public bool SubmitPurchaseRequest(string PRNumber, string UserEmail)
        {
            var user = db.UserAccounts.Where(d => d.Email == UserEmail).FirstOrDefault();
            var purchaseRequest = db.PurchaseRequestHeader.Where(d => d.PRNumber == PRNumber).FirstOrDefault();
            purchaseRequest.SubmittedBy = user.EmpCode;
            purchaseRequest.SubmittedAt = DateTime.Now;
            purchaseRequest.PRStatus = PurchaseRequestStatus.PurchaseRequestSubmitted;
            if (db.SaveChanges() == 0)
            {
                return false;
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
                hris.Dispose();
                abis.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}