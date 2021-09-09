using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using PUPFMIS.Models;
using PUPFMIS.Models.AIS;
using PUPFMIS.Models.HRIS;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace PUPFMIS.BusinessAndDataLogic
{
    public class StockCardsBL : Controller
    {
        private StockCardsDAL stockCardDAL = new StockCardsDAL();
        private HRISDataAccess hris = new HRISDataAccess();
        private FMISDbContext db = new FMISDbContext();

        public MemoryStream GenerateStockCard(string ReferenceNo)
        {
            var stockCard = stockCardDAL.StockCardSetup(ReferenceNo);
            Reports reports = new Reports();
            reports.ReportFilename = "Stock Card - " + ReferenceNo;
            reports.CreateDocument(8.50, 13.00, Orientation.Portrait, 0.25);
            
            foreach(var fundSource in stockCard)
            {
                reports.AddPageNumbersFooter();

                var recordCount = 25;
                var fetchCount = 0;
                var itemCount = fundSource.Entries.Count;
                var pageCount = (fundSource.Entries.Count + 24) / recordCount;
                for (int count = 1; count <= pageCount; count++)
                {
                    var columns = new List<ContentColumn>();
                    columns.Add(new ContentColumn(8.00));
                    reports.AddTable(columns, false);

                    var rows = new List<ContentCell>();
                    rows.Add(new ContentCell("Appendix 58", 0, 10, false, true, ParagraphAlignment.Right, VerticalAlignment.Center));
                    reports.AddRowContent(rows, 0.25);

                    columns = new List<ContentColumn>();
                    columns.Add(new ContentColumn(8.00));
                    reports.AddTable(columns, false);

                    reports.AddFormattedRowContent(new ContentCell(new TextWithFormat[]
                    {
                    new TextWithFormat("STOCK CARD", true, false, 14),
                    }, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center), 0.00);

                    reports.AddNewLine();
                    reports.AddNewLine();

                    columns = new List<ContentColumn>();
                    columns.Add(new ContentColumn(1.00));
                    columns.Add(new ContentColumn(3.50));
                    columns.Add(new ContentColumn(0.50));
                    columns.Add(new ContentColumn(1.00));
                    columns.Add(new ContentColumn(2.00));
                    reports.AddTable(columns, false);

                    rows = new List<ContentCell>();
                    rows.Add(new ContentCell("Entity Name : ", 0, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center));
                    rows.Add(new ContentCell(fundSource.EntityName, 1, 10, true, false, ParagraphAlignment.Left, VerticalAlignment.Bottom, 0, 0, true));
                    rows.Add(new ContentCell(string.Empty, 2, 9, true, false, ParagraphAlignment.Left, VerticalAlignment.Bottom));
                    rows.Add(new ContentCell("Fund Cluster :", 3, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center));
                    rows.Add(new ContentCell(fundSource.FundCluster, 4, 10, true, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, true));
                    reports.AddRowContent(rows, 0.20);

                    reports.AddNewLine();

                    columns = new List<ContentColumn>();
                    columns.Add(new ContentColumn(6.00));
                    columns.Add(new ContentColumn(2.00));
                    reports.AddTable(columns, true);

                    rows = new List<ContentCell>();
                    rows.Add(new ContentCell("Item : " + fundSource.Item, 0, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center));
                    rows.Add(new ContentCell("Stock No. : " + fundSource.StockNo, 1, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center));
                    reports.AddRowContent(rows, 0.25);

                    rows = new List<ContentCell>();
                    rows.Add(new ContentCell("Description : " + fundSource.Description, 0, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center));
                    rows.Add(new ContentCell("Re-Order Point : " + fundSource.ReorderPoint, 1, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center));
                    reports.AddRowContent(rows, 0.25);

                    rows = new List<ContentCell>();
                    rows.Add(new ContentCell("Unit of Measurement : " + fundSource.UnitOfMeasurement, 0, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center));
                    rows.Add(new ContentCell(string.Empty, 1, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center));
                    reports.AddRowContent(rows, 0.25);

                    columns = new List<ContentColumn>();
                    columns.Add(new ContentColumn(0.75));
                    columns.Add(new ContentColumn(1.00));
                    columns.Add(new ContentColumn(1.25));
                    columns.Add(new ContentColumn(1.25));
                    columns.Add(new ContentColumn(1.75));
                    columns.Add(new ContentColumn(1.25));
                    columns.Add(new ContentColumn(0.75));
                    reports.AddTable(columns, true);

                    rows = new List<ContentCell>();
                    rows.Add(new ContentCell("Date", 0, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 1));
                    rows.Add(new ContentCell("Reference", 1, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 1));
                    rows.Add(new ContentCell("Receipt\nQty.", 2, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 1));
                    rows.Add(new ContentCell("Issue", 3, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center, 1, 0));
                    rows.Add(new ContentCell("Balance\nQty.", 5, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 1));
                    rows.Add(new ContentCell("No. of Days to Consume", 6, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 1));
                    reports.AddRowContent(rows, 0.30);

                    rows = new List<ContentCell>();
                    rows.Add(new ContentCell("Qty.", 3, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                    rows.Add(new ContentCell("Office", 4, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                    reports.AddRowContent(rows, 0.25);

                    foreach (var entry in fundSource.Entries)
                    {
                        rows = new List<ContentCell>();
                        rows.Add(new ContentCell(entry.Date, 0, 8, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                        rows.Add(new ContentCell(entry.Reference, 1, 8, false, false, ParagraphAlignment.Left, VerticalAlignment.Center));
                        rows.Add(new ContentCell(entry.ReceiptQty.ToString("D", new System.Globalization.CultureInfo("en-ph")), 2, 8, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                        rows.Add(new ContentCell(entry.IssueQty == 0 ? string.Empty : entry.IssueQty.ToString("D", new System.Globalization.CultureInfo("en-ph")), 3, 8, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                        rows.Add(new ContentCell(entry.IssueOffice, 4, 8, false, false, ParagraphAlignment.Left, VerticalAlignment.Center));
                        rows.Add(new ContentCell(entry.BalanceQty.ToString("D", new System.Globalization.CultureInfo("en-ph")), 5, 8, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                        rows.Add(new ContentCell(entry.DaysToConsume.ToString(), 6, 8, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                        reports.AddRowContent(rows, 0.30);
                    }

                    rows = new List<ContentCell>();
                    rows.Add(new ContentCell("****** NOTHING FOLLOWS *******", 0, 9, true, false, ParagraphAlignment.Center, VerticalAlignment.Center, 6));
                    reports.AddRowContent(rows, 0.20);

                    for (var i = fundSource.Entries.Skip(fetchCount).Take(recordCount).ToList().Count; i < 24; i++)
                    {
                        rows = new List<ContentCell>();
                        rows.Add(new ContentCell("", 0, 9, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                        rows.Add(new ContentCell("", 1, 9, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                        rows.Add(new ContentCell("", 2, 9, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                        rows.Add(new ContentCell("", 3, 9, false, false, ParagraphAlignment.Right, VerticalAlignment.Center));
                        reports.AddRowContent(rows, 0.20);
                    }

                    fetchCount += recordCount;
                    if (count < pageCount)
                    {
                        reports.CreateDocument(8.50, 13.00, Orientation.Portrait, 0.25);
                    }
                }
            }

            return reports.GenerateReport();
        }
        public MemoryStream GenerateSuppliesLedgerCard(string ReferenceNo)
        {
            var stockCard = stockCardDAL.SuppliesLedgerCardSetup(ReferenceNo);
            Reports reports = new Reports();
            reports.ReportFilename = "Supplies Ledger Card - " + ReferenceNo;
            reports.CreateDocument(8.50, 13.00, Orientation.Landscape, 0.25);

            foreach (var fundSource in stockCard)
            {
                reports.AddPageNumbersFooter();

                var recordCount = 15;
                var fetchCount = 0;
                var itemCount = fundSource.Entries.Count;
                var pageCount = (fundSource.Entries.Count + 14) / recordCount;
                for (int count = 1; count <= pageCount; count++)
                {
                    var columns = new List<ContentColumn>();
                    columns.Add(new ContentColumn(12.50));
                    reports.AddTable(columns, false);

                    var rows = new List<ContentCell>();
                    rows.Add(new ContentCell("Appendix 57", 0, 10, false, true, ParagraphAlignment.Right, VerticalAlignment.Center));
                    reports.AddRowContent(rows, 0.25);

                    columns = new List<ContentColumn>();
                    columns.Add(new ContentColumn(12.50));
                    reports.AddTable(columns, false);

                    reports.AddFormattedRowContent(new ContentCell(new TextWithFormat[]
                    {
                    new TextWithFormat("SUPPLIES LEDGER CARD", true, false, 14),
                    }, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center), 0.00);

                    reports.AddNewLine();
                    reports.AddNewLine();

                    columns = new List<ContentColumn>();
                    columns.Add(new ContentColumn(1.00));
                    columns.Add(new ContentColumn(7.00));
                    columns.Add(new ContentColumn(0.50));
                    columns.Add(new ContentColumn(1.00));
                    columns.Add(new ContentColumn(3.00));
                    reports.AddTable(columns, false);

                    rows = new List<ContentCell>();
                    rows.Add(new ContentCell("Entity Name : ", 0, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center));
                    rows.Add(new ContentCell(fundSource.EntityName, 1, 10, true, false, ParagraphAlignment.Left, VerticalAlignment.Bottom, 0, 0, true));
                    rows.Add(new ContentCell(string.Empty, 2, 9, true, false, ParagraphAlignment.Left, VerticalAlignment.Bottom));
                    rows.Add(new ContentCell("Fund Cluster :", 3, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center));
                    rows.Add(new ContentCell(fundSource.FundCluster, 4, 10, true, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, true));
                    reports.AddRowContent(rows, 0.20);

                    reports.AddNewLine();

                    columns = new List<ContentColumn>();
                    columns.Add(new ContentColumn(8.50));
                    columns.Add(new ContentColumn(4.00));
                    reports.AddTable(columns, true);

                    rows = new List<ContentCell>();
                    rows.Add(new ContentCell("Item : " + fundSource.Item, 0, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center));
                    rows.Add(new ContentCell("Stock No. : " + fundSource.StockNo, 1, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center));
                    reports.AddRowContent(rows, 0.25);

                    rows = new List<ContentCell>();
                    rows.Add(new ContentCell("Description : " + fundSource.Description, 0, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center));
                    rows.Add(new ContentCell("Re-Order Point : " + fundSource.ReorderPoint, 1, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center));
                    reports.AddRowContent(rows, 0.25);

                    rows = new List<ContentCell>();
                    rows.Add(new ContentCell("Unit of Measurement : " + fundSource.UnitOfMeasurement, 0, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center));
                    rows.Add(new ContentCell(string.Empty, 1, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center));
                    reports.AddRowContent(rows, 0.25);

                    columns = new List<ContentColumn>();
                    columns.Add(new ContentColumn(1.00));
                    columns.Add(new ContentColumn(1.50));
                    columns.Add(new ContentColumn(1.00));
                    columns.Add(new ContentColumn(1.00));
                    columns.Add(new ContentColumn(1.00));
                    columns.Add(new ContentColumn(1.00));
                    columns.Add(new ContentColumn(1.00));
                    columns.Add(new ContentColumn(1.00));
                    columns.Add(new ContentColumn(1.00));
                    columns.Add(new ContentColumn(1.00));
                    columns.Add(new ContentColumn(1.00));
                    columns.Add(new ContentColumn(1.00));
                    reports.AddTable(columns, true);

                    rows = new List<ContentCell>();
                    rows.Add(new ContentCell("Date", 0, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 1));
                    rows.Add(new ContentCell("Reference", 1, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 1));
                    rows.Add(new ContentCell("Receipt", 2, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center, 2, 0));
                    rows.Add(new ContentCell("Issue", 5, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center, 2, 0));
                    rows.Add(new ContentCell("Balance", 8, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center, 2, 0));
                    rows.Add(new ContentCell("No. of Days to Consume", 11, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 1));
                    reports.AddRowContent(rows, 0.30);

                    rows = new List<ContentCell>();
                    rows.Add(new ContentCell("Qty.", 2, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                    rows.Add(new ContentCell("Unit Cost", 3, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                    rows.Add(new ContentCell("Total Cost", 4, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                    rows.Add(new ContentCell("Qty.", 5, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                    rows.Add(new ContentCell("Unit Cost", 6, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                    rows.Add(new ContentCell("Total Cost", 7, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                    rows.Add(new ContentCell("Qty.", 8, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                    rows.Add(new ContentCell("Unit Cost", 9, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                    rows.Add(new ContentCell("Total Cost", 10, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                    reports.AddRowContent(rows, 0.25);

                    foreach (var entry in fundSource.Entries)
                    {
                        rows = new List<ContentCell>();
                        rows.Add(new ContentCell(entry.Date, 0, 8, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                        rows.Add(new ContentCell(entry.Reference, 1, 8, false, false, ParagraphAlignment.Left, VerticalAlignment.Center));
                        rows.Add(new ContentCell(entry.ReceiptQty.ToString("D", new System.Globalization.CultureInfo("en-ph")), 2, 8, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                        rows.Add(new ContentCell(entry.ReceiptUnitCost.ToString("N", new System.Globalization.CultureInfo("en-ph")), 3, 8, false, false, ParagraphAlignment.Right, VerticalAlignment.Center));
                        rows.Add(new ContentCell(entry.ReceiptTotalCost.ToString("N", new System.Globalization.CultureInfo("en-ph")), 4, 8, false, false, ParagraphAlignment.Right, VerticalAlignment.Center));
                        rows.Add(new ContentCell(entry.IssueQty.ToString("D", new System.Globalization.CultureInfo("en-ph")), 5, 8, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                        rows.Add(new ContentCell(entry.IssueUnitCost.ToString("N", new System.Globalization.CultureInfo("en-ph")), 6, 8, false, false, ParagraphAlignment.Right, VerticalAlignment.Center));
                        rows.Add(new ContentCell(entry.IssueTotalCost.ToString("N", new System.Globalization.CultureInfo("en-ph")), 7, 8, false, false, ParagraphAlignment.Right, VerticalAlignment.Center));
                        rows.Add(new ContentCell(entry.BalanceQty.ToString("D", new System.Globalization.CultureInfo("en-ph")), 8, 8, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                        rows.Add(new ContentCell(entry.BalanceUnitCost.ToString("N", new System.Globalization.CultureInfo("en-ph")), 9, 8, false, false, ParagraphAlignment.Right, VerticalAlignment.Center));
                        rows.Add(new ContentCell(entry.BalanceTotalCost.ToString("N", new System.Globalization.CultureInfo("en-ph")), 10, 8, false, false, ParagraphAlignment.Right, VerticalAlignment.Center));
                        rows.Add(new ContentCell(entry.DaysToConsume.ToString(), 11, 8, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                        reports.AddRowContent(rows, 0.30);
                    }

                    rows = new List<ContentCell>();
                    rows.Add(new ContentCell("****** NOTHING FOLLOWS *******", 0, 9, true, false, ParagraphAlignment.Center, VerticalAlignment.Center, 11));
                    reports.AddRowContent(rows, 0.20);

                    for (var i = fundSource.Entries.Skip(fetchCount).Take(recordCount).ToList().Count; i < 14; i++)
                    {
                        rows = new List<ContentCell>();
                        rows.Add(new ContentCell("", 0, 9, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                        rows.Add(new ContentCell("", 1, 9, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                        rows.Add(new ContentCell("", 2, 9, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                        rows.Add(new ContentCell("", 3, 9, false, false, ParagraphAlignment.Right, VerticalAlignment.Center));
                        reports.AddRowContent(rows, 0.20);
                    }

                    fetchCount += recordCount;
                    if (count < pageCount)
                    {
                        reports.CreateDocument(8.50, 13.00, Orientation.Portrait, 0.25);
                    }
                }
            }

            return reports.GenerateReport();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
                hris.Dispose();
                stockCardDAL.Dispose();
            }
            base.Dispose(disposing);
        }
    }

    public class StockCardsDAL : Controller
    {
        private FMISDbContext db = new FMISDbContext();
        private HRISDataAccess hris = new HRISDataAccess();
        private ABISDataAccess abis = new ABISDataAccess();
        private SystemBDL systemBDL = new SystemBDL();

        public List<Supplies> GetSuppliesList()
        {
            return db.Supplies.Where(d => d.PurgeFlag == false).ToList();
        }

        public List<HRISEmployeeDetailsVM> GetInspectorPool()
        {
            var inspectionOfficeReference = db.AgencyDetails.FirstOrDefault().InspectionManagementReference;
            return hris.GetEmployees(inspectionOfficeReference);
        }
        public List<StockCardVM> StockCardSetup(string StockNo)
        {
            var agencyName = db.AgencyDetails.FirstOrDefault().AgencyName;
            var supply = db.Supplies.Where(d => d.StockNumber == StockNo).FirstOrDefault();
            return db.StockCard.Where(d => d.FKSupplyReference.StockNumber == StockNo).ToList()
            .Select(d => new
            {
                FundSource = d.FundSource.Replace("\r\n", string.Empty),
                FundCluster = abis.GetFundSources(d.FundSource).FUND_DESC.Replace("\r\n", string.Empty) + " (" + d.FundSource.Replace("\r\n", string.Empty) + ")",
                StockNo = d.FKSupplyReference.StockNumber,
                Item = d.FKSupplyReference.FKArticleReference.ArticleName,
                Description = d.FKSupplyReference.Description,
                UnitOfMeasurement = d.FKSupplyReference.FKIndividualUnitReference.UnitName,
                ReorderPoint = d.FKSupplyReference.ReOrderPoint
            }).GroupBy(d => d)
            .Select(d => new StockCardVM
            {
                EntityName = agencyName,
                FundCluster = d.Key.FundCluster,
                StockNo = d.Key.StockNo,
                Item = d.Key.Item,
                Description = d.Key.Description,
                UnitOfMeasurement = d.Key.UnitOfMeasurement,
                ReorderPoint = d.Key.ReorderPoint,
                Entries = db.StockCard.Where(x => x.FKSupplyReference.StockNumber == d.Key.StockNo && x.FundSource.StartsWith(d.Key.FundSource)).OrderBy(x => x.Date).ToList().Select(x => new StockCardEntriesVM
                {
                    Date = x.Date.ToString("dd MMM yyyy"),
                    Reference = x.Reference,
                    ReceiptQty = x.ReceiptQty,
                    IssueQty = x.IssuedQty,
                    IssueOffice = x.ReceiptQty > 0 && x.IssuedQty == 0 ? string.Empty : x.Organization,
                    BalanceQty = x.BalanceQty,
                    DaysToConsume = 0
                }).ToList()
            }).ToList();
        }
        public List<SuppliesLedgerCardVM> SuppliesLedgerCardSetup(string StockNo)
        {
            var agencyName = db.AgencyDetails.FirstOrDefault().AgencyName;
            var supply = db.Supplies.Where(d => d.StockNumber == StockNo).FirstOrDefault();
            return db.StockCard.Where(d => d.FKSupplyReference.StockNumber == StockNo).ToList()
            .Select(d => new
            {
                FundSource = d.FundSource.Replace("\r\n", string.Empty),
                FundCluster = abis.GetFundSources(d.FundSource).FUND_DESC.Replace("\r\n", string.Empty) + " (" + d.FundSource.Replace("\r\n", string.Empty) + ")",
                StockNo = d.FKSupplyReference.StockNumber,
                Item = d.FKSupplyReference.FKArticleReference.ArticleName,
                Description = d.FKSupplyReference.Description,
                UnitOfMeasurement = d.FKSupplyReference.FKIndividualUnitReference.UnitName,
                ReorderPoint = d.FKSupplyReference.ReOrderPoint
            }).GroupBy(d => d)
            .Select(d => new SuppliesLedgerCardVM
            {
                EntityName = agencyName,
                FundCluster = d.Key.FundCluster,
                StockNo = d.Key.StockNo,
                Item = d.Key.Item,
                Description = d.Key.Description,
                UnitOfMeasurement = d.Key.UnitOfMeasurement,
                ReorderPoint = d.Key.ReorderPoint,
                Entries = db.StockCard.Where(x => x.FKSupplyReference.StockNumber == d.Key.StockNo && x.FundSource.StartsWith(d.Key.FundSource)).OrderBy(x => x.Date).ToList().Select(x => new SuppliesLedgerCardEntriesVM
                {
                    Date = x.Date.ToString("dd MMM yyyy"),
                    Reference = x.Reference,
                    ReceiptQty = x.ReceiptQty,
                    ReceiptUnitCost = x.ReceiptUnitCost,
                    ReceiptTotalCost = x.ReceiptTotalCost,
                    IssueQty = x.IssuedQty,
                    IssueUnitCost = x.IssuedUnitCost,
                    IssueTotalCost = x.IssuedTotalCost,
                    BalanceQty = x.BalanceQty,
                    BalanceUnitCost = x.BalanceUnitCost,
                    BalanceTotalCost = x.BalanceTotalCost,
                    DaysToConsume = 0
                }).ToList()
            }).ToList();
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