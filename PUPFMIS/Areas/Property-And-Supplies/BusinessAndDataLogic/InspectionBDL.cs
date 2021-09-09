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
    public class InspectionBL : Controller
    {
        private InspectionDAL inspectionDAL = new InspectionDAL();
        private HRISDataAccess hris = new HRISDataAccess();
        private FMISDbContext db = new FMISDbContext();

        public MemoryStream GenerateSuppliesInspectionAndAcceptanceReport(string ReferenceNo, string LogoPath, string UserEmail)
        {
            var agencyDetails = db.AgencyDetails.FirstOrDefault();
            var propertyOffice = hris.GetDepartmentDetails(agencyDetails.PropertyOfficeReference);
            var inspectionOffice = hris.GetDepartmentDetails(agencyDetails.InspectionManagementReference);
            var inspection = inspectionDAL.GetSuppliesInspectionAndAcceptanceReportSetup(ReferenceNo);
            Reports reports = new Reports();
            reports.ReportFilename = "Inspection and Acceptance Report - " + ReferenceNo;
            reports.CreateDocument(8.50, 13.00, Orientation.Portrait, 0.25);

            var recordCount = 15;
            var fetchCount = 0;
            var itemCount = inspection.Supplies.Count;
            var pageCount = (inspection.Supplies.Count + 14) / recordCount;
            for (int count = 1; count <= pageCount; count++)
            {

                reports.AddDoubleColumnHeader();
                reports.AddColumnHeader(
                    new HeaderLine { Content = "", Bold = true, Italic = false, FontSize = 10, ParagraphAlignment = ParagraphAlignment.Left },
                    new HeaderLine { Content = "Appendix 63", Bold = false, Italic = true, FontSize = 12, ParagraphAlignment = ParagraphAlignment.Left }
                );
                reports.AddColumnHeader(
                    new HeaderLine { Content = "", Bold = false, Italic = false, FontSize = 8, ParagraphAlignment = ParagraphAlignment.Left },
                    new HeaderLine { Content = "Date Printed: " + DateTime.Now.ToString("dd MMMM yyyy hh:mm tt"), Bold = false, Italic = false, FontSize = 8, ParagraphAlignment = ParagraphAlignment.Left }
                );

                reports.AddNewLine();
                reports.AddNewLine();

                reports.AddSingleColumnHeader();
                reports.AddColumnHeader(
                    new HeaderLine { Content = "INSPECTION AND ACCEPTANCE REPORT", Bold = true, Italic = false, FontSize = 15, ParagraphAlignment = ParagraphAlignment.Center }
                );


                reports.AddNewLine();
                reports.AddNewLine();

                var columns = new List<ContentColumn>();
                columns.Add(new ContentColumn(1.00));
                columns.Add(new ContentColumn(3.50));
                columns.Add(new ContentColumn(0.50));
                columns.Add(new ContentColumn(1.00));
                columns.Add(new ContentColumn(2.00));
                reports.AddTable(columns, false);

                var rows = new List<ContentCell>();
                rows.Add(new ContentCell("Entity Name : ", 0, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center));
                rows.Add(new ContentCell(agencyDetails.AgencyName.ToUpper(), 1, 9, true, false, ParagraphAlignment.Left, VerticalAlignment.Bottom, 0, 0, true));
                rows.Add(new ContentCell(string.Empty, 2, 9, true, false, ParagraphAlignment.Left, VerticalAlignment.Bottom));
                rows.Add(new ContentCell("Fund Cluster :", 3, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center));
                rows.Add(new ContentCell(inspection.FundDescription.Replace("\r\n", string.Empty) + " (" + inspection.FundSource.Replace("\r\n", string.Empty) + ")", 4, 9, true, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, true));
                reports.AddRowContent(rows, 0.20);

                reports.AddNewLine();

                columns = new List<ContentColumn>();
                columns.Add(new ContentColumn(1.00));
                columns.Add(new ContentColumn(4.00));
                columns.Add(new ContentColumn(1.00));
                columns.Add(new ContentColumn(2.00));
                reports.AddTable(columns, false);

                rows = new List<ContentCell>();
                rows.Add(new ContentCell("Supplier : ", 0, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), false, true, true));
                rows.Add(new ContentCell(inspection.Supplier, 1, 9, true, false, ParagraphAlignment.Left, VerticalAlignment.Bottom, 0, 0, true, new Color(0, 0, 0, 0), true, true, false));
                rows.Add(new ContentCell("IAR No. : ", 2, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), false, true, false));
                rows.Add(new ContentCell(inspection.IARNumber, 3, 9, true, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, true, new Color(0, 0, 0, 0), true, true, false));
                reports.AddRowContent(rows, 0.20);

                rows = new List<ContentCell>();
                rows.Add(new ContentCell("PO No./Date : ", 0, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), false, true, true));
                rows.Add(new ContentCell(inspection.ReferenceNumber + " / " + inspection.ReferenceDate.ToString("dd MMMM yyyy"), 1, 9, true, false, ParagraphAlignment.Left, VerticalAlignment.Bottom, 0, 0, true, new Color(0, 0, 0, 0), true, true, false));
                rows.Add(new ContentCell("Date : ", 2, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), false, true, false));
                rows.Add(new ContentCell(inspection.ProcessedAt.ToString("dd MMMM yyyy"), 3, 9, true, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, true, new Color(0, 0, 0, 0), true, true, false));
                reports.AddRowContent(rows, 0.20);

                columns = new List<ContentColumn>();
                columns.Add(new ContentColumn(2.00));
                columns.Add(new ContentColumn(3.00));
                columns.Add(new ContentColumn(1.00));
                columns.Add(new ContentColumn(2.00));
                reports.AddTable(columns, false);

                rows = new List<ContentCell>();
                rows.Add(new ContentCell("Requisitioning Office/Dept. : ", 0, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), false, false, true));
                rows.Add(new ContentCell(inspection.RequisitioningOffice, 1, 9, true, false, ParagraphAlignment.Left, VerticalAlignment.Bottom, 0, 0, true, new Color(0, 0, 0, 0), true, false, false));
                rows.Add(new ContentCell("Invoice No. : ", 2, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), false, false, false));
                rows.Add(new ContentCell(inspection.InvoiceNumber, 3, 9, true, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, true, new Color(0, 0, 0, 0), true, false, false));
                reports.AddRowContent(rows, 0.20);

                rows = new List<ContentCell>();
                rows.Add(new ContentCell("Responsibility Center Code : ", 0, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), false, false, true));
                rows.Add(new ContentCell(inspection.ResponsibilityCenter, 1, 9, true, false, ParagraphAlignment.Left, VerticalAlignment.Bottom, 0, 0, true, new Color(0, 0, 0, 0), true, false, false));
                rows.Add(new ContentCell("Date : ", 2, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), false, false, false));
                rows.Add(new ContentCell(inspection.InvoiceDate.ToString("dd MMMM yyyy"), 3, 9, true, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, true, new Color(0, 0, 0, 0), true, false, false));
                reports.AddRowContent(rows, 0.20);

                rows = new List<ContentCell>();
                rows.Add(new ContentCell("", 0, 3, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), false, false, true));
                rows.Add(new ContentCell("", 1, 3, true, false, ParagraphAlignment.Left, VerticalAlignment.Bottom, 0, 0, false, new Color(0, 0, 0, 0), true, false, false));
                rows.Add(new ContentCell("", 2, 3, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), false, false, false));
                rows.Add(new ContentCell("", 3, 3, true, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), true, false, false));
                reports.AddRowContent(rows, 0.05);

                columns = new List<ContentColumn>();
                columns.Add(new ContentColumn(1.75));
                columns.Add(new ContentColumn(3.25));
                columns.Add(new ContentColumn(1.50));
                columns.Add(new ContentColumn(1.50));
                reports.AddTable(columns, true);

                rows = new List<ContentCell>();
                rows.Add(new ContentCell("Stock No.", 0, 10, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                rows.Add(new ContentCell("Description", 1, 10, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                rows.Add(new ContentCell("Unit", 2, 10, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                rows.Add(new ContentCell("Quantity", 3, 10, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                reports.AddRowContent(rows, 0.30);

                foreach (var item in inspection.Supplies.Skip(fetchCount).Take(recordCount).ToList())
                {
                    rows = new List<ContentCell>();
                    rows.Add(new ContentCell(item.StockNumber, 0, 9, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                    rows.Add(new ContentCell(new TextWithFormat[]
                    {
                        new TextWithFormat(item.Description.ToUpper(), false, false, 9),
                        new TextWithFormat("\n", false, false, 9),
                        new TextWithFormat(item.InspectionNotes, false, true, 7)
                    }, 1, ParagraphAlignment.Left, VerticalAlignment.Center));
                    rows.Add(new ContentCell(item.UnitOfMeasure, 2, 9, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                    rows.Add(new ContentCell(item.QuantityAccepted.ToString("D", new System.Globalization.CultureInfo("en-ph")), 3, 9, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                    reports.AddRowContent(rows, 0.40);
                }

                rows = new List<ContentCell>();
                rows.Add(new ContentCell("****** NOTHING FOLLOWS *******", 0, 9, true, false, ParagraphAlignment.Center, VerticalAlignment.Center, 3));
                reports.AddRowContent(rows, 0.20);

                for (var i = inspection.Supplies.Skip(fetchCount).Take(recordCount).ToList().Count; i < 14; i++)
                {
                    rows = new List<ContentCell>();
                    rows.Add(new ContentCell("", 0, 9, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                    rows.Add(new ContentCell("", 1, 9, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                    rows.Add(new ContentCell("", 2, 9, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                    rows.Add(new ContentCell("", 3, 9, false, false, ParagraphAlignment.Right, VerticalAlignment.Center));
                    reports.AddRowContent(rows, 0.20);
                }

                columns = new List<ContentColumn>();
                columns.Add(new ContentColumn(1.00));
                columns.Add(new ContentColumn(6.00));
                columns.Add(new ContentColumn(1.00));
                reports.AddTable(columns, false);

                rows = new List<ContentCell>();
                rows.Add(new ContentCell("Remarks:", 0, 10, true, false, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), false, false, true));
                rows.Add(new ContentCell(string.Empty, 1, 10, true, true, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 0, false));
                rows.Add(new ContentCell(string.Empty, 2, 9, false, false, ParagraphAlignment.Right, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), true, false, false));
                reports.AddRowContent(rows, 0.35);

                rows = new List<ContentCell>();
                rows.Add(new ContentCell(string.Empty, 0, 9, true, false, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 0, true, new Color(0, 0, 0, 0), false, false, true));
                rows.Add(new ContentCell(inspection.Remarks + "\n\n", 1, 10, false, true, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, true));
                rows.Add(new ContentCell(string.Empty, 2, 9, false, false, ParagraphAlignment.Right, VerticalAlignment.Center, 0, 0, true, new Color(0, 0, 0, 0), true, false, false));
                reports.AddRowContent(rows, 0.35);

                rows = new List<ContentCell>();
                rows.Add(new ContentCell("", 0, 9, true, false, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 0, true, new Color(0, 0, 0, 0), false, false, true));
                rows.Add(new ContentCell("Page " + count + " of " + pageCount, 1, 10, true, true, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 0, true));
                rows.Add(new ContentCell("", 2, 9, false, false, ParagraphAlignment.Right, VerticalAlignment.Center, 0, 0, true, new Color(0, 0, 0, 0), true, false, false));
                reports.AddRowContent(rows, 0.35);

                columns = new List<ContentColumn>();
                columns.Add(new ContentColumn(4.00));
                columns.Add(new ContentColumn(4.00));
                reports.AddTable(columns, false);

                rows = new List<ContentCell>();
                rows.Add(new ContentCell("INSPECTION", 0, 10, true, true, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 0, true, new Color(0, 0, 0, 0), true, false, true));
                rows.Add(new ContentCell("ACCEPTANCE", 1, 10, true, true, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 0, true, new Color(0, 0, 0, 0), true, false, true));
                reports.AddRowContent(rows, 0.35);

                columns = new List<ContentColumn>();
                columns.Add(new ContentColumn(0.20));
                columns.Add(new ContentColumn(1.20));
                columns.Add(new ContentColumn(2.40));
                columns.Add(new ContentColumn(0.20));
                columns.Add(new ContentColumn(0.20));
                columns.Add(new ContentColumn(1.20));
                columns.Add(new ContentColumn(2.40));
                columns.Add(new ContentColumn(0.20));
                reports.AddTable(columns, false);

                rows = new List<ContentCell>();
                rows.Add(new ContentCell("\n", 0, 10, true, false, ParagraphAlignment.Left, VerticalAlignment.Bottom, 0, 0, false, new Color(0, 0, 0, 0), false, false, true));
                rows.Add(new ContentCell("\nDate Inspected: ", 1, 10, true, false, ParagraphAlignment.Left, VerticalAlignment.Bottom, 0, 0, false, new Color(0, 0, 0, 0), false, false, false));
                rows.Add(new ContentCell("\n" + inspection.InspectedAt.ToString("dd MMMM yyyy"), 2, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Bottom, 0, 0, true, new Color(0, 0, 0, 0), false, false, false));
                rows.Add(new ContentCell("\n", 3, 10, true, false, ParagraphAlignment.Left, VerticalAlignment.Bottom, 0, 0, false, new Color(0, 0, 0, 0), true, false, false));
                rows.Add(new ContentCell("\n", 4, 10, true, false, ParagraphAlignment.Left, VerticalAlignment.Bottom, 0, 0, false, new Color(0, 0, 0, 0), false, false, true));
                rows.Add(new ContentCell("\nDate Received: ", 5, 10, true, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), false, false, false));
                rows.Add(new ContentCell("\n" + inspection.DeliveryDate.ToString("dd MMMM yyyy"), 6, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Bottom, 0, 0, true, new Color(0, 0, 0, 0), false, false, false));
                rows.Add(new ContentCell("\n", 7, 10, true, false, ParagraphAlignment.Left, VerticalAlignment.Bottom, 0, 0, false, new Color(0, 0, 0, 0), true, false, false));
                reports.AddRowContent(rows, 0.20);

                columns = new List<ContentColumn>();
                columns.Add(new ContentColumn(0.20));
                columns.Add(new ContentColumn(0.50));
                columns.Add(new ContentColumn(3.10));
                columns.Add(new ContentColumn(0.20));
                columns.Add(new ContentColumn(0.20));
                columns.Add(new ContentColumn(0.50));
                columns.Add(new ContentColumn(3.10));
                columns.Add(new ContentColumn(0.20));
                reports.AddTable(columns, false);

                rows = new List<ContentCell>();
                rows.Add(new ContentCell("\n", 0, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), false, false, true));
                rows.Add(new ContentCell("\n", 1, 10, true, false, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 0, true, new Color(0, 0, 0, 0), false, false, false));
                rows.Add(new ContentCell("\n", 2, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Center));
                rows.Add(new ContentCell("\n", 3, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), true, false, false));
                rows.Add(new ContentCell("\n", 4, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), false, false, true));
                rows.Add(new ContentCell("\n", 5, 10, true, false, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 0, true, new Color(0, 0, 0, 0), false, false, false));
                rows.Add(new ContentCell("\n", 6, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Center));
                rows.Add(new ContentCell("\n", 7, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), true, false, false));
                reports.AddRowContent(rows, 0.20);

                rows = new List<ContentCell>();
                rows.Add(new ContentCell(string.Empty, 0, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), true, false, true));
                rows.Add(new ContentCell((inspection.Supplies.Where(d => d.QuantityRejected > 0).ToList().Count == 0 ? "/" : string.Empty), 1, 10, true, false, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 0, true, new Color(0, 0, 0, 0), true, true, true));
                rows.Add(new ContentCell("Inspected, verified and found in order as to quantity and specifications", 2, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Center));
                rows.Add(new ContentCell(string.Empty, 3, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), true, false, false));
                rows.Add(new ContentCell(string.Empty, 4, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), true, false, true));
                rows.Add(new ContentCell((inspection.Supplies.Where(d => d.QuantityRejected > 0).ToList().Count == 0 ? "/" : string.Empty), 5, 10, true, false, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 0, true, new Color(0, 0, 0, 0), true, true, true));
                rows.Add(new ContentCell("Complete", 6, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Center));
                rows.Add(new ContentCell(string.Empty, 7, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), true, false, false));
                reports.AddRowContent(rows, 0.40);

                rows = new List<ContentCell>();
                rows.Add(new ContentCell(string.Empty, 0, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), false, false, true));
                rows.Add(new ContentCell(string.Empty, 1, 10, true, false, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 0));
                rows.Add(new ContentCell(string.Empty, 2, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Center));
                rows.Add(new ContentCell(string.Empty, 3, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), true, false, false));
                rows.Add(new ContentCell(string.Empty, 4, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), true, false, true));
                rows.Add(new ContentCell((inspection.Supplies.Where(d => d.QuantityRejected > 0).ToList().Count > 0 ? "/" : string.Empty), 5, 10, true, false, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 0, true, new Color(0, 0, 0, 0), true, true, true));
                rows.Add(new ContentCell((inspection.Supplies.Where(d => d.QuantityRejected > 0).ToList().Count == 0 ? "Partial (pls. specify quantity)" : "Partial (pls. specify quantity:      " + (inspection.Supplies.Select(d => d.QuantityAccepted).Sum().ToString()) + ")"), 6, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Center));
                rows.Add(new ContentCell(string.Empty, 7, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), true, false, false));
                reports.AddRowContent(rows, 0.40);

                columns = new List<ContentColumn>();
                columns.Add(new ContentColumn(0.20));
                columns.Add(new ContentColumn(3.60));
                columns.Add(new ContentColumn(0.20));
                columns.Add(new ContentColumn(0.20));
                columns.Add(new ContentColumn(3.60));
                columns.Add(new ContentColumn(0.20));
                reports.AddTable(columns, false);

                rows = new List<ContentCell>();
                rows.Add(new ContentCell("\n\n\n", 0, 10, true, false, ParagraphAlignment.Left, VerticalAlignment.Bottom, 0, 0, false, new Color(0, 0, 0, 0), false, false, true));
                rows.Add(new ContentCell("\n\n\n" + inspection.InspectedBy, 1, 10, true, false, ParagraphAlignment.Center, VerticalAlignment.Bottom, 0, 0, true, new Color(0, 0, 0, 0), false, false, false));
                rows.Add(new ContentCell("\n\n\n", 2, 10, true, false, ParagraphAlignment.Left, VerticalAlignment.Bottom, 0, 0, false, new Color(0, 0, 0, 0), true, false, false));
                rows.Add(new ContentCell("\n\n\n", 3, 10, true, false, ParagraphAlignment.Left, VerticalAlignment.Bottom, 0, 0, false, new Color(0, 0, 0, 0), false, false, true));
                rows.Add(new ContentCell("\n\n\n" + propertyOffice.DepartmentHead, 4, 10, true, false, ParagraphAlignment.Center, VerticalAlignment.Bottom, 0, 0, true, new Color(0, 0, 0, 0), false, false, false));
                rows.Add(new ContentCell("\n\n\n", 5, 10, true, false, ParagraphAlignment.Left, VerticalAlignment.Bottom, 0, 0, false, new Color(0, 0, 0, 0), true, false, false));
                reports.AddRowContent(rows, 0.20);

                rows = new List<ContentCell>();
                rows.Add(new ContentCell(string.Empty, 0, 10, true, false, ParagraphAlignment.Left, VerticalAlignment.Bottom, 0, 0, false, new Color(0, 0, 0, 0), false, false, true));
                rows.Add(new ContentCell("Inspection Officer/Inspection Committee", 1, 10, false, false, ParagraphAlignment.Center, VerticalAlignment.Bottom, 0, 0, false, new Color(0, 0, 0, 0), false, false, false));
                rows.Add(new ContentCell(string.Empty, 2, 10, true, false, ParagraphAlignment.Left, VerticalAlignment.Bottom, 0, 0, false, new Color(0, 0, 0, 0), true, false, false));
                rows.Add(new ContentCell(string.Empty, 3, 10, true, false, ParagraphAlignment.Left, VerticalAlignment.Bottom, 0, 0, false, new Color(0, 0, 0, 0), false, false, true));
                rows.Add(new ContentCell("Supply and/or Property Custodian", 4, 10, false, false, ParagraphAlignment.Center, VerticalAlignment.Bottom, 0, 0, false, new Color(0, 0, 0, 0), false, false, false));
                rows.Add(new ContentCell(string.Empty, 5, 10, true, false, ParagraphAlignment.Left, VerticalAlignment.Bottom, 0, 0, false, new Color(0, 0, 0, 0), true, false, false));
                reports.AddRowContent(rows, 0.20);

                rows = new List<ContentCell>();
                rows.Add(new ContentCell("", 0, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, true, new Color(0, 0, 0, 0), false, false, true));
                rows.Add(new ContentCell("", 1, 10, true, false, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 0, true));
                rows.Add(new ContentCell("", 2, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Center, 0, 0, true, new Color(0, 0, 0, 0), true, false, false));
                rows.Add(new ContentCell("", 3, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Center, 0, 0, true, new Color(0, 0, 0, 0), false, false, true));
                rows.Add(new ContentCell("", 4, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Center, 0, 0, true));
                rows.Add(new ContentCell("", 5, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, true, new Color(0, 0, 0, 0), true, false, false));
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
                db.Dispose();
                hris.Dispose();
                inspectionDAL.Dispose();
            }
            base.Dispose(disposing);
        }
    }

    public class InspectionDAL : Controller
    {
        private FMISDbContext db = new FMISDbContext();
        private HRISDataAccess hris = new HRISDataAccess();
        private ABISDataAccess abis = new ABISDataAccess();
        private SystemBDL systemBDL = new SystemBDL();

        public List<int> GetReportYears()
        {
            return db.Inspection.Select(d => d.InspectedAt.Year).Distinct().ToList();
        }
        public List<InspectionListVM> GetInspectionList(int ReportYear)
        {
            return db.Inspection.Where(d => d.InspectedAt.Year == ReportYear).OrderByDescending(d => d.ProcessedAt).ToList().Select(d => new InspectionListVM
            {
                IARNumber = d.IARNumber,
                Supplier = d.FKSupplierReference.SupplierName,
                InspectedBy = hris.GetEmployeeDetailByCode(d.InspectedBy).EmployeeName,
                InspectedAt = d.InspectedAt.ToString("dd MMMM yyyy")
            }).ToList();
        }
        public List<HRISEmployeeDetailsVM> GetInspectorPool()
        {
            var inspectionOfficeReference = db.AgencyDetails.FirstOrDefault().InspectionManagementReference;
            return hris.GetEmployees(inspectionOfficeReference);
        }
        public List<DeliveryListVM> GetSupplyDeliveriesList()
        {
            var suppliesDeliveryIDs = db.SupplyDelivery.Select(d => d.DeliveryReference).Distinct().ToList();
            var suppliesInspectionIDs = db.SupplyInspection.Select(d => d.FKInspectionReference.DeliveryReference).Distinct().ToList();
            var references = suppliesDeliveryIDs.Except(suppliesInspectionIDs).ToList();
            return db.DeliveryHeader.Where(d => references.Contains(d.ID)).ToList().Select(d => new DeliveryListVM
            {
                DeliveryAcceptanceNumber = d.DeliveryAcceptanceNumber,
                ReferenceNo = d.Reference,
                ContractType = d.ContractType,
                InvoiceNumber = d.InvoiceNumber,
                InvoiceDate = d.InvoiceDate,
                DRNumber = d.DRNumber,
                DeliveryDate = d.DeliveryDate,
                SupplierName = d.FKContractReference.FKSupplierReference.SupplierName
            }).ToList();
        }
        public List<DeliveryListVM> GetPPEDeliveriesList()
        {
            var ppeDeliveryIDs = db.PPEDelivery.Select(d => d.DeliveryReference).Distinct().ToList();
            return db.DeliveryHeader.Where(d => ppeDeliveryIDs.Contains(d.ID)).ToList().Select(d => new DeliveryListVM
            {
                DeliveryAcceptanceNumber = d.DeliveryAcceptanceNumber,
                ReferenceNo = d.Reference,
                ContractType = d.ContractType,
                InvoiceNumber = d.InvoiceNumber,
                InvoiceDate = d.InvoiceDate,
                DRNumber = d.DRNumber,
                DeliveryDate = d.DeliveryDate,
                SupplierName = d.FKContractReference.FKSupplierReference.SupplierName
            }).ToList();
        }
        public InspectionSuppliesDeliveredVM GetInspectionSuppliesSetup(string DeliveryReceiptReportNo)
        {
            var delivery = db.DeliveryHeader.Where(d => d.DeliveryAcceptanceNumber == DeliveryReceiptReportNo).FirstOrDefault();
            var suppliesDelivered = db.SupplyDelivery.Where(d => d.DeliveryReference == delivery.ID).ToList();
            var inspection = new InspectionSuppliesDeliveredVM
            {
                SupplierReference = delivery.FKContractReference.SupplierReference,
                Supplier = delivery.FKContractReference.FKSupplierReference.SupplierName,
                ContractType = delivery.ContractType,
                ReferenceNumber = delivery.Reference,
                ReferenceDate = delivery.FKContractReference.CreatedAt,
                InvoiceNumber = delivery.InvoiceNumber,
                InvoiceDate = delivery.InvoiceDate,
                DRNumber = delivery.DRNumber,
                DeliveryDate = delivery.DeliveryDate,
                RequisitioningOffice = hris.GetDepartmentDetails(delivery.FKContractReference.FKProcurementProjectReference.FKAPPReference.EndUser).Department,
                ResponsibilityCenter = delivery.FKContractReference.FKProcurementProjectReference.FKAPPReference.EndUser,
                FundSource = delivery.FKContractReference.FKProcurementProjectReference.FundSource,
                FundDescription = abis.GetFundSources(delivery.FKContractReference.FKProcurementProjectReference.FundSource).FUND_DESC,
                DeliveryAcceptanceNumber = delivery.DeliveryAcceptanceNumber,
                DeliveryReference = delivery.ID,
                DeliveryCompleteness = delivery.DeliveryCompleteness,
                DateProcessed = delivery.DateProcessed,
                ProcessedBy = hris.GetEmployeeDetailByCode(delivery.ProcessedBy).EmployeeName,
                ReceivedBy = hris.GetEmployeeDetailByCode(delivery.ReceivedBy).EmployeeName,
                Supplies = suppliesDelivered.Select(d => new InspectionSupplyVM
                {
                    SupplyReference = d.SupplyReference,
                    StockNumber = d.FKSupplyReference.StockNumber,
                    Description = d.FKSupplyReference.Description,
                    UnitReference = d.FKSupplyReference.IndividualUOMReference.Value,
                    UnitOfMeasure = d.FKSupplyReference.FKIndividualUnitReference.Abbreviation,
                    ReceivedUnitCost = d.ReceiptUnitCost,
                    QuantityReceived = d.QuantityDelivered
                }).ToList()
            };

            return inspection;
        }
        public InspectionSuppliesDeliveredVM GetSuppliesInspectionAndAcceptanceReportSetup(string IARNumber)
        {
            var suppliesInspection = db.SupplyInspection.Where(d => d.FKInspectionReference.IARNumber == IARNumber).Select(d => new InspectionSupplyVM
            {
                SupplyReference = d.SupplyReference,
                StockNumber = d.FKSupplyReference.StockNumber,
                Description = d.FKSupplyReference.Description,
                UnitReference = d.FKSupplyReference.IndividualUOMReference.Value,
                UnitOfMeasure = d.FKSupplyReference.FKIndividualUnitReference.Abbreviation,
                QuantityReceived = d.QuantityReceived,
                QuantityAccepted = d.QuantityAccepted,
                QuantityRejected = d.QuantityRejected,
                InspectionNotes = d.InspectionNotes
            }).ToList();

            var inspection = db.Inspection.Where(d => d.IARNumber == IARNumber).FirstOrDefault();
            var delivery = db.DeliveryHeader.Find(inspection.DeliveryReference);
            var contract = db.Contract.Find(delivery.ContractReference);
            return new InspectionSuppliesDeliveredVM
            {
                IARNumber = inspection.IARNumber,
                SupplierReference = inspection.SupplierReference,
                Supplier = inspection.FKSupplierReference.SupplierName,
                ContractType = contract.ContractType,
                ReferenceNumber = contract.ReferenceNumber,
                ReferenceDate = contract.CreatedAt,
                InvoiceNumber = inspection.InvoiceNumber,
                InvoiceDate = inspection.InvoiceDate,
                DRNumber = delivery.DRNumber,
                DeliveryDate = delivery.DeliveryDate,
                RequisitioningOffice = hris.GetDepartmentDetails(contract.FKProcurementProjectReference.FKAPPReference.EndUser).Department,
                ResponsibilityCenter = contract.FKProcurementProjectReference.FKAPPReference.EndUser,
                FundSource = inspection.FundSource,
                FundDescription = abis.GetFundSources(inspection.FundSource).FUND_DESC,
                InspectedBy = hris.GetEmployeeDetailByCode(inspection.InspectedBy).EmployeeName,
                InspectedAt = inspection.InspectedAt,
                ProcessedAt = inspection.ProcessedAt,
                DeliveryReference = delivery.ID,
                DeliveryAcceptanceNumber = delivery.DeliveryAcceptanceNumber,
                DeliveryCompleteness = delivery.DeliveryCompleteness,
                DateProcessed = delivery.DateProcessed,
                ProcessedBy = hris.GetEmployeeDetailByCode(delivery.ProcessedBy).EmployeeName,
                ReceivedBy = hris.GetEmployeeDetailByCode(delivery.ReceivedBy).EmployeeName,
                Remarks = inspection.Remarks,
                Supplies = suppliesInspection
            };
        }
        public bool PostSupplyInspection(InspectionSuppliesDeliveredVM Inspection, string UserEmail)
        {
            var user = db.UserAccounts.Where(d => d.Email == UserEmail).FirstOrDefault();
            var inspection = db.Inspection.Add(new Inspection
            {
                IARNumber = GenerateIARNumber(),
                SupplierReference = Inspection.SupplierReference,
                ContractType = Inspection.ContractType,
                ReferenceNumber = Inspection.ReferenceNumber,
                ReferenceDate = Inspection.ReferenceDate,
                InvoiceNumber = Inspection.InvoiceNumber,
                InvoiceDate = Inspection.InvoiceDate,
                ResponsibilityCenter = Inspection.ResponsibilityCenter,
                Remarks = Inspection.Remarks,
                FundSource = Inspection.FundSource,
                DeliveryCompleteness = Inspection.DeliveryCompleteness,
                DeliveryReference = Inspection.DeliveryReference,
                InspectedBy = Inspection.InspectedBy,
                InspectedAt = Inspection.InspectedAt,
                ProcessedBy = user.EmpCode,
                ProcessedAt = DateTime.Now
            });

            if (db.SaveChanges() == 0)
            {
                return false;
            }

            db.SupplyInspection.AddRange(Inspection.Supplies.Select(d => new InspectionSupply
            {
                InspectionReference = inspection.ID,
                SupplyReference = d.SupplyReference,
                UnitReference = d.UnitReference,
                QuantityReceived = d.QuantityReceived,
                QuantityAccepted = d.QuantityAccepted,
                QuantityRejected = d.QuantityRejected,
                InspectionNotes = d.InspectionNotes
            }).ToList());

            if (db.SaveChanges() == 0)
            {
                return false;
            }

            var rejectedItems = Inspection.Supplies.Where(d => d.QuantityRejected > 0).ToList();
            var contract = db.Contract.Where(d => d.ReferenceNumber == Inspection.ReferenceNumber).FirstOrDefault();
            if (rejectedItems.Count > 0)
            {
                foreach(var item in rejectedItems)
                {
                    var supply = db.Supplies.Find(item.SupplyReference);
                    var contractDetail = db.ContractDetails.Where(d => d.ContractReference == contract.ID && d.ArticleReference == supply.ArticleReference && d.ItemSequence == supply.Sequence).FirstOrDefault();
                    contractDetail.DeliveredQuantity -= item.QuantityRejected;

                    if (db.SaveChanges() == 0)
                    {
                        return false;
                    }
                }

                contract.ContractStatus = ContractStatus.InspectedPartialAcceptance;
                if (db.SaveChanges() == 0)
                {
                    return false;
                }
            }
            else
            {
                contract.ContractStatus = ContractStatus.InspectedAndAccepted;
                if (db.SaveChanges() == 0)
                {
                    return false;
                }
            }

            foreach(var supply in Inspection.Supplies)
            {
                if(supply.QuantityAccepted > 0)
                {
                    var stockCardLastEntry = db.StockCard.Where(d => d.SupplyID == supply.SupplyReference).OrderByDescending(d => d.Date).FirstOrDefault();
                    var stockCardLastEntryBalanceQty = stockCardLastEntry == null ? 0 : stockCardLastEntry.BalanceQty;
                    var stockCardLastEntryBalanceUnitCost = stockCardLastEntry == null ? 0 : stockCardLastEntry.BalanceUnitCost;

                    var receiptTotalCost = Math.Round((supply.QuantityAccepted * supply.ReceivedUnitCost), 2);
                    var currentBalanceTotalCost = Math.Round((stockCardLastEntryBalanceQty * stockCardLastEntryBalanceUnitCost), 2);
                    var balanceQty = stockCardLastEntryBalanceQty + supply.QuantityAccepted;
                    var balanceUnitCost = (receiptTotalCost + currentBalanceTotalCost) / balanceQty;
                    var balanceTotalCost = Math.Round((balanceQty * balanceUnitCost), 2);

                    var stockCard = db.StockCard.Add(new StockCard
                    {
                        SupplyID = supply.SupplyReference,
                        Date = DateTime.Now.Date,
                        Organization = Inspection.Supplier.ToUpper(),
                        Reference = inspection.IARNumber,
                        FundSource = Inspection.FundSource,
                        ReferenceType = ReferenceTypes.InspectionAndAcceptance,
                        ReceiptQty = supply.QuantityAccepted,
                        ReceiptUnitCost = supply.ReceivedUnitCost,
                        ReceiptTotalCost = receiptTotalCost,
                        IssuedQty = 0,
                        IssuedUnitCost = 0.00m,
                        IssuedTotalCost = 0.00m,
                        BalanceQty = balanceQty,
                        BalanceUnitCost = balanceUnitCost,
                        BalanceTotalCost = balanceTotalCost
                    });

                    if (db.SaveChanges() == 0)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private string GenerateIARNumber()
        {
            string series = (db.Inspection.Where(d => d.InspectedAt.Year == DateTime.Now.Year).Count() + 1).ToString();
            series = series.Length == 1 ? "000" + series : series.Length == 2 ? "00" + series : series.Length == 3 ? "0" + series : series;
            return "IAR-" + DateTime.Now.ToString("yy") + "-" + DateTime.Now.ToString("MM") + "-" + series;
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