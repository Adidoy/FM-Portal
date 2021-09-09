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
    public class DeliveriesBL : Controller
    {
        private DeliveriesDAL deliveriesDAL = new DeliveriesDAL();
        private HRISDataAccess hris = new HRISDataAccess();
        private FMISDbContext db = new FMISDbContext();

        public MemoryStream GenerateDeliveryAcceptanceReport(string ReferenceNo, string LogoPath, string UserEmail)
        {
            var agencyDetails = db.AgencyDetails.FirstOrDefault();
            var property = hris.GetDepartmentDetails(agencyDetails.PropertyOfficeReference);
            var delivery = deliveriesDAL.GetDeliveryAcceptanceReportSetup(ReferenceNo);
            Reports reports = new Reports();
            reports.ReportFilename = "Delivery Receipt Report - " + ReferenceNo;
            reports.CreateDocument(8.50, 13.00, Orientation.Portrait, 0.50);

            if(delivery.Supplies.Count != 0)
            {
                var recordCount = 10;
                var fetchCount = 0;
                var itemCount = delivery.Supplies.Count;
                var pageCount = (delivery.Supplies.Count + 9) / recordCount;
                var rows = new List<ContentCell>();
                for (int count = 1; count <= pageCount; count++)
                {
                    reports.AddDoubleColumnHeader(LogoPath, false, 1.25);
                    reports.AddColumnHeader(
                        new HeaderLine { Content = "Republic of the Philippines", Bold = false, Italic = false, FontSize = 8.5, ParagraphAlignment = ParagraphAlignment.Left },
                        new HeaderLine { Content = "", Bold = false, Italic = false, FontSize = 6, ParagraphAlignment = ParagraphAlignment.Left }
                    );
                    reports.AddColumnHeader(
                        new HeaderLine { Content = agencyDetails.AgencyName.ToUpper(), Bold = true, Italic = false, FontSize = 11, ParagraphAlignment = ParagraphAlignment.Left },
                        new HeaderLine { Content = "Date Printed: ", Bold = false, Italic = false, FontSize = 7, ParagraphAlignment = ParagraphAlignment.Left }
                    );
                    reports.AddColumnHeader(
                        new HeaderLine { Content = property.Sector, Bold = false, Italic = false, FontSize = 8.5, ParagraphAlignment = ParagraphAlignment.Left },
                        new HeaderLine { Content = DateTime.Now.ToString("dd MMMM yyyy hh:mm tt"), Bold = false, Italic = false, FontSize = 7, ParagraphAlignment = ParagraphAlignment.Left }
                    );
                    reports.AddColumnHeader(
                        new HeaderLine { Content = property.Department.ToUpper(), Bold = true, Italic = false, FontSize = 9, ParagraphAlignment = ParagraphAlignment.Left },
                        new HeaderLine { Content = "Reference No.: ", Bold = false, Italic = false, FontSize = 8, ParagraphAlignment = ParagraphAlignment.Left }
                    );
                    reports.AddColumnHeader(
                        new HeaderLine { Content = agencyDetails.Address + "\n", Bold = false, Italic = false, FontSize = 7, ParagraphAlignment = ParagraphAlignment.Left },
                        new HeaderLine { Content = delivery.DeliveryAcceptanceNumber, Bold = false, Italic = false, FontSize = 8, ParagraphAlignment = ParagraphAlignment.Left }
                    );

                    var columns = new List<ContentColumn>();
                    columns.Add(new ContentColumn(7.50));
                    reports.AddTable(columns, false);

                    reports.AddFormattedRowContent(new ContentCell(new TextWithFormat[]
                    {
                   new TextWithFormat("DELIVERY RECEIPT REPORT", true, false, 14),
                    }, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center), 0.00);

                    reports.AddNewLine();
                    reports.AddNewLine();

                    columns = new List<ContentColumn>();
                    columns.Add(new ContentColumn(2.00));
                    columns.Add(new ContentColumn(2.00));
                    columns.Add(new ContentColumn(1.50));
                    columns.Add(new ContentColumn(2.00));
                    reports.AddTable(columns, false);

                    rows = new List<ContentCell>();
                    rows.Add(new ContentCell("Delivery Receipt No. : ", 0, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), false, true, true));
                    rows.Add(new ContentCell(delivery.DeliveryAcceptanceNumber, 1, 9, true, false, ParagraphAlignment.Left, VerticalAlignment.Bottom, 0, 0, true, new Color(0, 0, 0, 0), true, true, false));
                    rows.Add(new ContentCell("Contract Type :", 2, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), false, true, false));
                    rows.Add(new ContentCell(delivery.ContractType.GetAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>().Name, 3, 9, true, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, true, new Color(0, 0, 0, 0), true, true, false));
                    reports.AddRowContent(rows, 0.20);

                    rows = new List<ContentCell>();
                    rows.Add(new ContentCell("Reference No. : ", 0, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), false, true, true));
                    rows.Add(new ContentCell(delivery.ReferenceNo, 1, 9, true, false, ParagraphAlignment.Left, VerticalAlignment.Bottom, 0, 0, true, new Color(0, 0, 0, 0), true, true, false));
                    rows.Add(new ContentCell("Date : ", 2, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), false, true, false));
                    rows.Add(new ContentCell(delivery.Date.ToString("dd MMMM yyyy"), 3, 9, true, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, true, new Color(0, 0, 0, 0), true, true, false));
                    reports.AddRowContent(rows, 0.20);

                    rows = new List<ContentCell>();
                    rows.Add(new ContentCell("Invoice No. : ", 0, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), false, true, true));
                    rows.Add(new ContentCell(delivery.InvoiceNumber, 1, 9, true, false, ParagraphAlignment.Left, VerticalAlignment.Bottom, 0, 0, true, new Color(0, 0, 0, 0), true, true, false));
                    rows.Add(new ContentCell("Invoice Date : ", 2, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), false, true, false));
                    rows.Add(new ContentCell(delivery.InvoiceDate.ToString("dd MMMM yyyy"), 3, 9, true, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, true, new Color(0, 0, 0, 0), true, true, false));
                    reports.AddRowContent(rows, 0.20);

                    rows = new List<ContentCell>();
                    rows.Add(new ContentCell("Delivery Receipt No. : ", 0, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), false, true, true));
                    rows.Add(new ContentCell(delivery.DRNumber, 1, 9, true, false, ParagraphAlignment.Left, VerticalAlignment.Bottom, 0, 0, true, new Color(0, 0, 0, 0), true, true, false));
                    rows.Add(new ContentCell("Delivery Date : ", 2, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), false, true, false));
                    rows.Add(new ContentCell(delivery.DeliveryDate.ToString("dd MMMM yyyy"), 3, 9, true, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, true, new Color(0, 0, 0, 0), true, true, false));
                    reports.AddRowContent(rows, 0.20);

                    rows = new List<ContentCell>();
                    rows.Add(new ContentCell("", 0, 3, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), false, false, true));
                    rows.Add(new ContentCell("", 1, 3, true, false, ParagraphAlignment.Left, VerticalAlignment.Bottom, 0, 0, false, new Color(0, 0, 0, 0), true, false, false));
                    rows.Add(new ContentCell("", 2, 3, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), false, false, false));
                    rows.Add(new ContentCell("", 3, 3, true, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), true, false, false));
                    reports.AddRowContent(rows, 0.05);

                    columns = new List<ContentColumn>();
                    columns.Add(new ContentColumn(2.00));
                    columns.Add(new ContentColumn(5.50));
                    reports.AddTable(columns, false);

                    rows = new List<ContentCell>();
                    rows.Add(new ContentCell("Supplier : ", 0, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), false, true, true));
                    rows.Add(new ContentCell(delivery.SupplierName, 1, 10, true, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), true, true, false));
                    reports.AddRowContent(rows, 0.30);

                    columns = new List<ContentColumn>();
                    columns.Add(new ContentColumn(7.50));
                    reports.AddTable(columns, false);

                    rows = new List<ContentCell>();
                    rows.Add(new ContentCell("SUPPLIES", 0, 10, true, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), true, true, true));
                    reports.AddRowContent(rows, 0.30);

                    columns = new List<ContentColumn>();
                    columns.Add(new ContentColumn(0.75));
                    columns.Add(new ContentColumn(0.75));
                    columns.Add(new ContentColumn(2.00));
                    columns.Add(new ContentColumn(1.00));
                    columns.Add(new ContentColumn(1.00));
                    columns.Add(new ContentColumn(1.00));
                    columns.Add(new ContentColumn(1.00));
                    reports.AddTable(columns, true);

                    rows = new List<ContentCell>();
                    rows.Add(new ContentCell("Stock No.", 0, 10, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                    rows.Add(new ContentCell("Unit", 1, 10, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                    rows.Add(new ContentCell("Description", 2, 10, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                    rows.Add(new ContentCell("Quantity", 3, 10, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                    rows.Add(new ContentCell("Balance", 4, 10, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                    rows.Add(new ContentCell("Receipt Cost", 5, 10, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                    rows.Add(new ContentCell("Amount", 6, 10, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                    reports.AddRowContent(rows, 0.20);

                    foreach (var item in delivery.Supplies.Skip(fetchCount).Take(recordCount).ToList())
                    {
                        rows = new List<ContentCell>();
                        rows.Add(new ContentCell(item.StockNumber, 0, 9, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                        rows.Add(new ContentCell(item.IndividualUOM, 1, 9, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                        rows.Add(new ContentCell(new TextWithFormat[]
                        {
                        new TextWithFormat(item.Description.ToUpper(), false, false, 9)
                        }, 2, ParagraphAlignment.Left, VerticalAlignment.Center));
                        rows.Add(new ContentCell(item.QuantityDelivered.ToString("D", new System.Globalization.CultureInfo("en-ph")), 3, 9, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                        rows.Add(new ContentCell(item.Quantity.ToString("D", new System.Globalization.CultureInfo("en-ph")), 4, 9, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                        rows.Add(new ContentCell(item.ContractUnitPrice.ToString("N", new System.Globalization.CultureInfo("en-ph")), 5, 9, false, false, ParagraphAlignment.Right, VerticalAlignment.Center));
                        rows.Add(new ContentCell(item.ContractTotalPrice.ToString("N", new System.Globalization.CultureInfo("en-ph")), 6, 9, false, false, ParagraphAlignment.Right, VerticalAlignment.Center));
                        reports.AddRowContent(rows, 0.40);
                    }

                    for (var i = delivery.Supplies.Skip(fetchCount).Take(recordCount).ToList().Count; i < 10; i++)
                    {
                        rows = new List<ContentCell>();
                        rows.Add(new ContentCell("", 0, 9, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                        rows.Add(new ContentCell("", 1, 9, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                        rows.Add(new ContentCell(new TextWithFormat[]
                        {
                    new TextWithFormat("", false, false, 9)
                        }, 2, ParagraphAlignment.Left, VerticalAlignment.Center));
                        rows.Add(new ContentCell("", 3, 9, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                        rows.Add(new ContentCell("", 4, 9, false, false, ParagraphAlignment.Right, VerticalAlignment.Center));
                        rows.Add(new ContentCell("", 5, 9, false, false, ParagraphAlignment.Right, VerticalAlignment.Center));
                        rows.Add(new ContentCell("", 6, 9, false, false, ParagraphAlignment.Right, VerticalAlignment.Center));
                        reports.AddRowContent(rows, 0.40);
                    }

                    columns = new List<ContentColumn>();
                    columns.Add(new ContentColumn(1.50));
                    columns.Add(new ContentColumn(5.00));
                    columns.Add(new ContentColumn(1.00));
                    reports.AddTable(columns, false);

                    if (count == pageCount)
                    {
                        rows = new List<ContentCell>();
                        rows.Add(new ContentCell("(Total Amount in Words)", 0, 9, false, false, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 0, true, new Color(0, 0, 0, 0), false, false, true));
                        rows.Add(new ContentCell(Reports.AmountToWords(delivery.Supplies.Sum(d => d.ContractTotalPrice)), 1, 10, true, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, true, new Color(0, 0, 0, 0), true, false, false));
                        rows.Add(new ContentCell(delivery.Supplies.Sum(d => d.ContractTotalPrice).ToString("C", new System.Globalization.CultureInfo("en-ph")), 2, 10, false, false, ParagraphAlignment.Right, VerticalAlignment.Center, 0, 0, true, new Color(0, 0, 0, 0), true, false, false));
                        reports.AddRowContent(rows, 0.35);

                        rows = new List<ContentCell>();
                        rows.Add(new ContentCell("", 0, 9, true, false, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 0, true, new Color(0, 0, 0, 0), false, false, true));
                        rows.Add(new ContentCell("Page " + count + " of " + pageCount, 1, 10, true, true, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 0, true));
                        rows.Add(new ContentCell("", 2, 9, false, false, ParagraphAlignment.Right, VerticalAlignment.Center, 0, 0, true, new Color(0, 0, 0, 0), true, false, false));
                        reports.AddRowContent(rows, 0.35);
                    }
                    else
                    {
                        rows = new List<ContentCell>();
                        rows.Add(new ContentCell("", 0, 9, true, false, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 0, true, new Color(0, 0, 0, 0), false, false, true));
                        rows.Add(new ContentCell("Page " + count + " of " + pageCount, 1, 10, true, true, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 0, true));
                        rows.Add(new ContentCell("", 2, 9, false, false, ParagraphAlignment.Right, VerticalAlignment.Center, 0, 0, true, new Color(0, 0, 0, 0), true, false, false));
                        reports.AddRowContent(rows, 0.35);
                    }

                    columns = new List<ContentColumn>();
                    columns.Add(new ContentColumn(0.25));
                    columns.Add(new ContentColumn(3.00));
                    columns.Add(new ContentColumn(1.00));
                    columns.Add(new ContentColumn(3.00));
                    columns.Add(new ContentColumn(0.25));
                    reports.AddTable(columns, false);

                    rows = new List<ContentCell>();
                    rows.Add(new ContentCell("", 0, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), false, false, true));
                    rows.Add(new ContentCell("Received, verified and found in order as to quantity and specifications:", 1, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center));
                    rows.Add(new ContentCell("", 2, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Center));
                    rows.Add(new ContentCell("Processed records according to the reference documents:", 3, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center));
                    rows.Add(new ContentCell("", 4, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), true, false, false));
                    reports.AddRowContent(rows, 0.35);

                    rows = new List<ContentCell>();
                    rows.Add(new ContentCell("\n\n", 0, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), false, false, true));
                    rows.Add(new ContentCell("\n\n", 1, 10, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                    rows.Add(new ContentCell("\n\n", 2, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Center));
                    rows.Add(new ContentCell("\n\n", 3, 10, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                    rows.Add(new ContentCell("\n\n", 4, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), true, false, false));
                    reports.AddRowContent(rows, 0.20);

                    rows = new List<ContentCell>();
                    rows.Add(new ContentCell("", 0, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), false, false, true));
                    rows.Add(new ContentCell(delivery.ReceivedBy, 1, 10, true, false, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 0, true));
                    rows.Add(new ContentCell("", 2, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Center));
                    rows.Add(new ContentCell(delivery.ProcessedBy, 3, 10, true, false, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 0, true));
                    rows.Add(new ContentCell("", 4, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), true, false, false));
                    reports.AddRowContent(rows, 0.20);

                    rows = new List<ContentCell>();
                    rows.Add(new ContentCell("", 0, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), false, false, true));
                    rows.Add(new ContentCell("Signature over Printed Name of Receiving Officer", 1, 8.5, false, false, ParagraphAlignment.Center, VerticalAlignment.Top));
                    rows.Add(new ContentCell("", 2, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Center));
                    rows.Add(new ContentCell("Signature over Printed Name of Processing Officer", 3, 8.5, false, false, ParagraphAlignment.Center, VerticalAlignment.Top));
                    rows.Add(new ContentCell("", 4, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), true, false, false));
                    reports.AddRowContent(rows, 0.00);

                    rows = new List<ContentCell>();
                    rows.Add(new ContentCell("", 0, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), false, false, true));
                    rows.Add(new ContentCell(delivery.DeliveryDate.ToString("dd MMMM yyyy"), 1, 8.5, false, false, ParagraphAlignment.Center, VerticalAlignment.Top));
                    rows.Add(new ContentCell("", 2, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Center));
                    rows.Add(new ContentCell(delivery.DateProcessed.ToString("dd MMMM yyyy"), 3, 8.5, false, false, ParagraphAlignment.Center, VerticalAlignment.Top));
                    rows.Add(new ContentCell("", 4, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), true, false, false));
                    reports.AddRowContent(rows, 0.00);

                    rows = new List<ContentCell>();
                    rows.Add(new ContentCell("", 0, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, true, new Color(0, 0, 0, 0), false, false, true));
                    rows.Add(new ContentCell("", 1, 8.5, false, false, ParagraphAlignment.Center, VerticalAlignment.Top, 0, 0, true, new Color(0, 0, 0, 0), false, false, false));
                    rows.Add(new ContentCell("", 2, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Center, 0, 0, true, new Color(0, 0, 0, 0), false, false, false));
                    rows.Add(new ContentCell("", 3, 8.5, false, false, ParagraphAlignment.Center, VerticalAlignment.Top, 0, 0, true, new Color(0, 0, 0, 0), false, false, false));
                    rows.Add(new ContentCell("", 4, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, true, new Color(0, 0, 0, 0), true, false, false));
                    reports.AddRowContent(rows, 0.00);

                    columns = new List<ContentColumn>();
                    columns.Add(new ContentColumn(0.25));
                    columns.Add(new ContentColumn(3.00));
                    columns.Add(new ContentColumn(1.00));
                    columns.Add(new ContentColumn(3.00));
                    columns.Add(new ContentColumn(0.25));
                    reports.AddTable(columns, false);

                    rows = new List<ContentCell>();
                    rows.Add(new ContentCell("", 0, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), false, false, true));
                    rows.Add(new ContentCell("Delivery Status:", 1, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center));
                    rows.Add(new ContentCell("", 2, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Center));
                    rows.Add(new ContentCell("Approval:", 3, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center));
                    rows.Add(new ContentCell("", 4, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), true, false, false));
                    reports.AddRowContent(rows, 0.35);

                    rows = new List<ContentCell>();
                    rows.Add(new ContentCell("\n\n", 0, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), false, false, true));
                    rows.Add(new ContentCell("\n\n", 1, 10, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                    rows.Add(new ContentCell("\n\n", 2, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Center));
                    rows.Add(new ContentCell("\n\n", 3, 10, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                    rows.Add(new ContentCell("\n\n", 4, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), true, false, false));
                    reports.AddRowContent(rows, 0.00);

                    columns = new List<ContentColumn>();
                    columns.Add(new ContentColumn(0.25));
                    columns.Add(new ContentColumn(0.50));
                    columns.Add(new ContentColumn(2.50));
                    columns.Add(new ContentColumn(1.00));
                    columns.Add(new ContentColumn(3.00));
                    columns.Add(new ContentColumn(0.25));
                    reports.AddTable(columns, false);

                    rows = new List<ContentCell>();
                    rows.Add(new ContentCell("", 0, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), true, false, true));
                    rows.Add(new ContentCell((delivery.DeliveryCompleteness == DeliveryCompleteness.CompleteDelivery ? "/" : string.Empty), 1, 10, true, false, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 0, true, new Color(0, 0, 0, 0), true, true, true));
                    rows.Add(new ContentCell(DeliveryCompleteness.CompleteDelivery.GetAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>().Name, 2, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Center));
                    rows.Add(new ContentCell("", 3, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Center));
                    rows.Add(new ContentCell(property.DepartmentHead, 4, 10, true, false, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 0, true));
                    rows.Add(new ContentCell("", 5, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), true, false, false));
                    reports.AddRowContent(rows, 0.20);

                    rows = new List<ContentCell>();
                    rows.Add(new ContentCell("", 0, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), true, false, true));
                    rows.Add(new ContentCell((delivery.DeliveryCompleteness == DeliveryCompleteness.PartialDelivery ? "/" : string.Empty), 1, 10, true, false, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 0, true, new Color(0, 0, 0, 0), true, true, true));
                    rows.Add(new ContentCell(DeliveryCompleteness.PartialDelivery.GetAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>().Name, 2, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Center));
                    rows.Add(new ContentCell("", 3, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Center));
                    rows.Add(new ContentCell(property.DepartmentHeadDesignation, 4, 10, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                    rows.Add(new ContentCell("", 5, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), true, false, false));
                    reports.AddRowContent(rows, 0.20);

                    rows = new List<ContentCell>();
                    rows.Add(new ContentCell("", 0, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, true, new Color(0, 0, 0, 0), false, false, true));
                    rows.Add(new ContentCell("", 1, 10, true, false, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 0, true));
                    rows.Add(new ContentCell("", 2, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Center, 0, 0, true));
                    rows.Add(new ContentCell("", 3, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Center, 0, 0, true));
                    rows.Add(new ContentCell("", 4, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Center, 0, 0, true));
                    rows.Add(new ContentCell("", 5, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, true, new Color(0, 0, 0, 0), true, false, false));
                    reports.AddRowContent(rows, 0.20);

                    fetchCount += recordCount;
                    if (count < pageCount)
                    {
                        reports.CreateDocument(8.50, 13.00, Orientation.Portrait, 0.25);
                    }
                }
            }

            if (delivery.PPE.Count != 0)
            {
                var recordCount = 10;
                var fetchCount = 0;
                var itemCount = delivery.PPE.Count;
                var pageCount = (delivery.PPE.Count + 9) / recordCount;
                var rows = new List<ContentCell>();
                for (int count = 1; count <= pageCount; count++)
                {
                    reports.AddDoubleColumnHeader(LogoPath, false, 1.25);
                    reports.AddColumnHeader(
                        new HeaderLine { Content = "Republic of the Philippines", Bold = false, Italic = false, FontSize = 8.5, ParagraphAlignment = ParagraphAlignment.Left },
                        new HeaderLine { Content = "", Bold = false, Italic = false, FontSize = 6, ParagraphAlignment = ParagraphAlignment.Left }
                    );
                    reports.AddColumnHeader(
                        new HeaderLine { Content = agencyDetails.AgencyName.ToUpper(), Bold = true, Italic = false, FontSize = 11, ParagraphAlignment = ParagraphAlignment.Left },
                        new HeaderLine { Content = "Date Printed: ", Bold = false, Italic = false, FontSize = 7, ParagraphAlignment = ParagraphAlignment.Left }
                    );
                    reports.AddColumnHeader(
                        new HeaderLine { Content = property.Sector, Bold = false, Italic = false, FontSize = 8.5, ParagraphAlignment = ParagraphAlignment.Left },
                        new HeaderLine { Content = DateTime.Now.ToString("dd MMMM yyyy hh:mm tt"), Bold = false, Italic = false, FontSize = 7, ParagraphAlignment = ParagraphAlignment.Left }
                    );
                    reports.AddColumnHeader(
                        new HeaderLine { Content = property.Department.ToUpper(), Bold = true, Italic = false, FontSize = 9, ParagraphAlignment = ParagraphAlignment.Left },
                        new HeaderLine { Content = "Reference No.: ", Bold = false, Italic = false, FontSize = 8, ParagraphAlignment = ParagraphAlignment.Left }
                    );
                    reports.AddColumnHeader(
                        new HeaderLine { Content = agencyDetails.Address + "\n", Bold = false, Italic = false, FontSize = 7, ParagraphAlignment = ParagraphAlignment.Left },
                        new HeaderLine { Content = delivery.DeliveryAcceptanceNumber, Bold = false, Italic = false, FontSize = 8, ParagraphAlignment = ParagraphAlignment.Left }
                    );

                    var columns = new List<ContentColumn>();
                    columns.Add(new ContentColumn(7.50));
                    reports.AddTable(columns, false);

                    reports.AddFormattedRowContent(new ContentCell(new TextWithFormat[]
                    {
                   new TextWithFormat("DELIVERY RECEIPT REPORT", true, false, 14),
                    }, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center), 0.00);

                    reports.AddNewLine();
                    reports.AddNewLine();

                    columns = new List<ContentColumn>();
                    columns.Add(new ContentColumn(2.00));
                    columns.Add(new ContentColumn(2.00));
                    columns.Add(new ContentColumn(1.50));
                    columns.Add(new ContentColumn(2.00));
                    reports.AddTable(columns, false);

                    rows = new List<ContentCell>();
                    rows.Add(new ContentCell("Delivery Receipt No. : ", 0, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), false, true, true));
                    rows.Add(new ContentCell(delivery.DeliveryAcceptanceNumber, 1, 9, true, false, ParagraphAlignment.Left, VerticalAlignment.Bottom, 0, 0, true, new Color(0, 0, 0, 0), true, true, false));
                    rows.Add(new ContentCell("Contract Type :", 2, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), false, true, false));
                    rows.Add(new ContentCell(delivery.ContractType.GetAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>().Name, 3, 9, true, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, true, new Color(0, 0, 0, 0), true, true, false));
                    reports.AddRowContent(rows, 0.20);

                    rows = new List<ContentCell>();
                    rows.Add(new ContentCell("Reference No. : ", 0, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), false, true, true));
                    rows.Add(new ContentCell(delivery.ReferenceNo, 1, 9, true, false, ParagraphAlignment.Left, VerticalAlignment.Bottom, 0, 0, true, new Color(0, 0, 0, 0), true, true, false));
                    rows.Add(new ContentCell("Date : ", 2, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), false, true, false));
                    rows.Add(new ContentCell(delivery.Date.ToString("dd MMMM yyyy"), 3, 9, true, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, true, new Color(0, 0, 0, 0), true, true, false));
                    reports.AddRowContent(rows, 0.20);

                    rows = new List<ContentCell>();
                    rows.Add(new ContentCell("Invoice No. : ", 0, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), false, true, true));
                    rows.Add(new ContentCell(delivery.InvoiceNumber, 1, 9, true, false, ParagraphAlignment.Left, VerticalAlignment.Bottom, 0, 0, true, new Color(0, 0, 0, 0), true, true, false));
                    rows.Add(new ContentCell("Invoice Date : ", 2, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), false, true, false));
                    rows.Add(new ContentCell(delivery.InvoiceDate.ToString("dd MMMM yyyy"), 3, 9, true, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, true, new Color(0, 0, 0, 0), true, true, false));
                    reports.AddRowContent(rows, 0.20);

                    rows = new List<ContentCell>();
                    rows.Add(new ContentCell("Delivery Receipt No. : ", 0, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), false, true, true));
                    rows.Add(new ContentCell(delivery.DRNumber, 1, 9, true, false, ParagraphAlignment.Left, VerticalAlignment.Bottom, 0, 0, true, new Color(0, 0, 0, 0), true, true, false));
                    rows.Add(new ContentCell("Delivery Date : ", 2, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), false, true, false));
                    rows.Add(new ContentCell(delivery.DeliveryDate.ToString("dd MMMM yyyy"), 3, 9, true, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, true, new Color(0, 0, 0, 0), true, true, false));
                    reports.AddRowContent(rows, 0.20);

                    rows = new List<ContentCell>();
                    rows.Add(new ContentCell("", 0, 3, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), false, false, true));
                    rows.Add(new ContentCell("", 1, 3, true, false, ParagraphAlignment.Left, VerticalAlignment.Bottom, 0, 0, false, new Color(0, 0, 0, 0), true, false, false));
                    rows.Add(new ContentCell("", 2, 3, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), false, false, false));
                    rows.Add(new ContentCell("", 3, 3, true, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), true, false, false));
                    reports.AddRowContent(rows, 0.05);

                    columns = new List<ContentColumn>();
                    columns.Add(new ContentColumn(2.00));
                    columns.Add(new ContentColumn(5.50));
                    reports.AddTable(columns, false);

                    rows = new List<ContentCell>();
                    rows.Add(new ContentCell("Supplier : ", 0, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), false, true, true));
                    rows.Add(new ContentCell(delivery.SupplierName, 1, 10, true, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), true, true, false));
                    reports.AddRowContent(rows, 0.30);

                    columns = new List<ContentColumn>();
                    columns.Add(new ContentColumn(7.50));
                    reports.AddTable(columns, false);

                    rows = new List<ContentCell>();
                    rows.Add(new ContentCell("PROPERTY, PLANT AND EQUIPMENT", 0, 10, true, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), true, true, true));
                    reports.AddRowContent(rows, 0.30);

                    columns = new List<ContentColumn>();
                    columns.Add(new ContentColumn(0.75));
                    columns.Add(new ContentColumn(0.75));
                    columns.Add(new ContentColumn(2.00));
                    columns.Add(new ContentColumn(1.00));
                    columns.Add(new ContentColumn(1.00));
                    columns.Add(new ContentColumn(1.00));
                    columns.Add(new ContentColumn(1.00));
                    reports.AddTable(columns, true);

                    rows = new List<ContentCell>();
                    rows.Add(new ContentCell("Property No.", 0, 10, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                    rows.Add(new ContentCell("Unit", 1, 10, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                    rows.Add(new ContentCell("Description", 2, 10, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                    rows.Add(new ContentCell("Quantity", 3, 10, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                    rows.Add(new ContentCell("Balance", 4, 10, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                    rows.Add(new ContentCell("Receipt Cost", 5, 10, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                    rows.Add(new ContentCell("Amount", 6, 10, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                    reports.AddRowContent(rows, 0.20);

                    foreach (var item in delivery.PPE.Skip(fetchCount).Take(recordCount).ToList())
                    {
                        rows = new List<ContentCell>();
                        rows.Add(new ContentCell(item.PropertyNo, 0, 9, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                        rows.Add(new ContentCell(item.IndividualUOM, 1, 9, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                        rows.Add(new ContentCell(new TextWithFormat[]
                        {
                        new TextWithFormat(item.Description.ToUpper(), false, false, 9)
                        }, 2, ParagraphAlignment.Left, VerticalAlignment.Center));
                        rows.Add(new ContentCell(item.QuantityDelivered.ToString("D", new System.Globalization.CultureInfo("en-ph")), 3, 9, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                        rows.Add(new ContentCell(item.Quantity.ToString("D", new System.Globalization.CultureInfo("en-ph")), 4, 9, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                        rows.Add(new ContentCell(item.ContractUnitPrice.ToString("N", new System.Globalization.CultureInfo("en-ph")), 5, 9, false, false, ParagraphAlignment.Right, VerticalAlignment.Center));
                        rows.Add(new ContentCell(item.ContractTotalPrice.ToString("N", new System.Globalization.CultureInfo("en-ph")), 6, 9, false, false, ParagraphAlignment.Right, VerticalAlignment.Center));
                        reports.AddRowContent(rows, 0.40);
                    }

                    for (var i = delivery.PPE.Skip(fetchCount).Take(recordCount).ToList().Count; i < 10; i++)
                    {
                        rows = new List<ContentCell>();
                        rows.Add(new ContentCell("", 0, 9, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                        rows.Add(new ContentCell("", 1, 9, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                        rows.Add(new ContentCell(new TextWithFormat[]
                        {
                            new TextWithFormat("", false, false, 9)
                        }, 2, ParagraphAlignment.Left, VerticalAlignment.Center));
                        rows.Add(new ContentCell("", 3, 9, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                        rows.Add(new ContentCell("", 4, 9, false, false, ParagraphAlignment.Right, VerticalAlignment.Center));
                        rows.Add(new ContentCell("", 5, 9, false, false, ParagraphAlignment.Right, VerticalAlignment.Center));
                        rows.Add(new ContentCell("", 6, 9, false, false, ParagraphAlignment.Right, VerticalAlignment.Center));
                        reports.AddRowContent(rows, 0.40);
                    }

                    columns = new List<ContentColumn>();
                    columns.Add(new ContentColumn(1.50));
                    columns.Add(new ContentColumn(5.00));
                    columns.Add(new ContentColumn(1.00));
                    reports.AddTable(columns, false);

                    if (count == pageCount)
                    {
                        rows = new List<ContentCell>();
                        rows.Add(new ContentCell("(Total Amount in Words)", 0, 9, false, false, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 0, true, new Color(0, 0, 0, 0), false, false, true));
                        rows.Add(new ContentCell(Reports.AmountToWords(delivery.PPE.Sum(d => d.ContractTotalPrice)), 1, 10, true, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, true, new Color(0, 0, 0, 0), true, false, false));
                        rows.Add(new ContentCell(delivery.PPE.Sum(d => d.ContractTotalPrice).ToString("C", new System.Globalization.CultureInfo("en-ph")), 2, 10, false, false, ParagraphAlignment.Right, VerticalAlignment.Center, 0, 0, true, new Color(0, 0, 0, 0), true, false, false));
                        reports.AddRowContent(rows, 0.35);

                        rows = new List<ContentCell>();
                        rows.Add(new ContentCell("", 0, 9, true, false, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 0, true, new Color(0, 0, 0, 0), false, false, true));
                        rows.Add(new ContentCell("Page " + count + " of " + pageCount, 1, 10, true, true, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 0, true));
                        rows.Add(new ContentCell("", 2, 9, false, false, ParagraphAlignment.Right, VerticalAlignment.Center, 0, 0, true, new Color(0, 0, 0, 0), true, false, false));
                        reports.AddRowContent(rows, 0.35);
                    }
                    else
                    {
                        rows = new List<ContentCell>();
                        rows.Add(new ContentCell("", 0, 9, true, false, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 0, true, new Color(0, 0, 0, 0), false, false, true));
                        rows.Add(new ContentCell("Page " + count + " of " + pageCount, 1, 10, true, true, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 0, true));
                        rows.Add(new ContentCell("", 2, 9, false, false, ParagraphAlignment.Right, VerticalAlignment.Center, 0, 0, true, new Color(0, 0, 0, 0), true, false, false));
                        reports.AddRowContent(rows, 0.35);
                    }

                    columns = new List<ContentColumn>();
                    columns.Add(new ContentColumn(0.25));
                    columns.Add(new ContentColumn(3.00));
                    columns.Add(new ContentColumn(1.00));
                    columns.Add(new ContentColumn(3.00));
                    columns.Add(new ContentColumn(0.25));
                    reports.AddTable(columns, false);

                    rows = new List<ContentCell>();
                    rows.Add(new ContentCell("", 0, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), false, false, true));
                    rows.Add(new ContentCell("Received, verified and found in order as to quantity and specifications:", 1, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center));
                    rows.Add(new ContentCell("", 2, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Center));
                    rows.Add(new ContentCell("Processed records according to the reference documents:", 3, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center));
                    rows.Add(new ContentCell("", 4, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), true, false, false));
                    reports.AddRowContent(rows, 0.35);

                    rows = new List<ContentCell>();
                    rows.Add(new ContentCell("\n\n", 0, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), false, false, true));
                    rows.Add(new ContentCell("\n\n", 1, 10, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                    rows.Add(new ContentCell("\n\n", 2, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Center));
                    rows.Add(new ContentCell("\n\n", 3, 10, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                    rows.Add(new ContentCell("\n\n", 4, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), true, false, false));
                    reports.AddRowContent(rows, 0.20);

                    rows = new List<ContentCell>();
                    rows.Add(new ContentCell("", 0, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), false, false, true));
                    rows.Add(new ContentCell(delivery.ReceivedBy, 1, 10, true, false, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 0, true));
                    rows.Add(new ContentCell("", 2, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Center));
                    rows.Add(new ContentCell(delivery.ProcessedBy, 3, 10, true, false, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 0, true));
                    rows.Add(new ContentCell("", 4, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), true, false, false));
                    reports.AddRowContent(rows, 0.20);

                    rows = new List<ContentCell>();
                    rows.Add(new ContentCell("", 0, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), false, false, true));
                    rows.Add(new ContentCell("Signature over Printed Name of Receiving Officer", 1, 8.5, false, false, ParagraphAlignment.Center, VerticalAlignment.Top));
                    rows.Add(new ContentCell("", 2, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Center));
                    rows.Add(new ContentCell("Signature over Printed Name of Processing Officer", 3, 8.5, false, false, ParagraphAlignment.Center, VerticalAlignment.Top));
                    rows.Add(new ContentCell("", 4, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), true, false, false));
                    reports.AddRowContent(rows, 0.00);

                    rows = new List<ContentCell>();
                    rows.Add(new ContentCell("", 0, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), false, false, true));
                    rows.Add(new ContentCell(delivery.DeliveryDate.ToString("dd MMMM yyyy"), 1, 8.5, false, false, ParagraphAlignment.Center, VerticalAlignment.Top));
                    rows.Add(new ContentCell("", 2, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Center));
                    rows.Add(new ContentCell(delivery.DateProcessed.ToString("dd MMMM yyyy"), 3, 8.5, false, false, ParagraphAlignment.Center, VerticalAlignment.Top));
                    rows.Add(new ContentCell("", 4, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), true, false, false));
                    reports.AddRowContent(rows, 0.00);

                    rows = new List<ContentCell>();
                    rows.Add(new ContentCell("", 0, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, true, new Color(0, 0, 0, 0), false, false, true));
                    rows.Add(new ContentCell("", 1, 8.5, false, false, ParagraphAlignment.Center, VerticalAlignment.Top, 0, 0, true, new Color(0, 0, 0, 0), false, false, false));
                    rows.Add(new ContentCell("", 2, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Center, 0, 0, true, new Color(0, 0, 0, 0), false, false, false));
                    rows.Add(new ContentCell("", 3, 8.5, false, false, ParagraphAlignment.Center, VerticalAlignment.Top, 0, 0, true, new Color(0, 0, 0, 0), false, false, false));
                    rows.Add(new ContentCell("", 4, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, true, new Color(0, 0, 0, 0), true, false, false));
                    reports.AddRowContent(rows, 0.00);

                    columns = new List<ContentColumn>();
                    columns.Add(new ContentColumn(0.25));
                    columns.Add(new ContentColumn(3.00));
                    columns.Add(new ContentColumn(1.00));
                    columns.Add(new ContentColumn(3.00));
                    columns.Add(new ContentColumn(0.25));
                    reports.AddTable(columns, false);

                    rows = new List<ContentCell>();
                    rows.Add(new ContentCell("", 0, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), false, false, true));
                    rows.Add(new ContentCell("Delivery Status:", 1, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center));
                    rows.Add(new ContentCell("", 2, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Center));
                    rows.Add(new ContentCell("Approval:", 3, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center));
                    rows.Add(new ContentCell("", 4, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), true, false, false));
                    reports.AddRowContent(rows, 0.35);

                    rows = new List<ContentCell>();
                    rows.Add(new ContentCell("\n\n", 0, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), false, false, true));
                    rows.Add(new ContentCell("\n\n", 1, 10, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                    rows.Add(new ContentCell("\n\n", 2, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Center));
                    rows.Add(new ContentCell("\n\n", 3, 10, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                    rows.Add(new ContentCell("\n\n", 4, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), true, false, false));
                    reports.AddRowContent(rows, 0.00);

                    columns = new List<ContentColumn>();
                    columns.Add(new ContentColumn(0.25));
                    columns.Add(new ContentColumn(0.50));
                    columns.Add(new ContentColumn(2.50));
                    columns.Add(new ContentColumn(1.00));
                    columns.Add(new ContentColumn(3.00));
                    columns.Add(new ContentColumn(0.25));
                    reports.AddTable(columns, false);

                    rows = new List<ContentCell>();
                    rows.Add(new ContentCell("", 0, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), true, false, true));
                    rows.Add(new ContentCell((delivery.DeliveryCompleteness == DeliveryCompleteness.CompleteDelivery ? "/" : string.Empty), 1, 10, true, false, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 0, true, new Color(0, 0, 0, 0), true, true, true));
                    rows.Add(new ContentCell(DeliveryCompleteness.CompleteDelivery.GetAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>().Name, 2, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Center));
                    rows.Add(new ContentCell("", 3, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Center));
                    rows.Add(new ContentCell(property.DepartmentHead, 4, 10, true, false, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 0, true));
                    rows.Add(new ContentCell("", 5, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), true, false, false));
                    reports.AddRowContent(rows, 0.20);

                    rows = new List<ContentCell>();
                    rows.Add(new ContentCell("", 0, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), true, false, true));
                    rows.Add(new ContentCell((delivery.DeliveryCompleteness == DeliveryCompleteness.PartialDelivery ? "/" : string.Empty), 1, 10, true, false, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 0, true, new Color(0, 0, 0, 0), true, true, true));
                    rows.Add(new ContentCell(DeliveryCompleteness.PartialDelivery.GetAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>().Name, 2, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Center));
                    rows.Add(new ContentCell("", 3, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Center));
                    rows.Add(new ContentCell(property.DepartmentHeadDesignation, 4, 10, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                    rows.Add(new ContentCell("", 5, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), true, false, false));
                    reports.AddRowContent(rows, 0.20);

                    rows = new List<ContentCell>();
                    rows.Add(new ContentCell("", 0, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, true, new Color(0, 0, 0, 0), false, false, true));
                    rows.Add(new ContentCell("", 1, 10, true, false, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 0, true));
                    rows.Add(new ContentCell("", 2, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Center, 0, 0, true));
                    rows.Add(new ContentCell("", 3, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Center, 0, 0, true));
                    rows.Add(new ContentCell("", 4, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Center, 0, 0, true));
                    rows.Add(new ContentCell("", 5, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, true, new Color(0, 0, 0, 0), true, false, false));
                    reports.AddRowContent(rows, 0.20);

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
                deliveriesDAL.Dispose();
            }
            base.Dispose(disposing);
        }
    }

    public class DeliveriesDAL : Controller
    {
        private FMISDbContext db = new FMISDbContext();
        private HRISDataAccess hris = new HRISDataAccess();
        private ABISDataAccess abis = new ABISDataAccess();
        private SystemBDL systemBDL = new SystemBDL();

        public List<UnregisteredSuppliesVM> GetUnregisteredSupplies(string ReferenceNo)
        {
            var itemCodes = db.ContractDetails.Where(d => d.FKContractReference.ReferenceNumber == ReferenceNo && (d.FKContractReference.FKProcurementProjectReference.FKClassificationReference.ID == 1 || d.FKContractReference.FKProcurementProjectReference.FKClassificationReference.ID == 3 || d.FKContractReference.FKProcurementProjectReference.FKClassificationReference.ID == 4)).ToList()
                            .Select(d => new { ItemCode = d.FKArticleReference.ArticleCode + "-" + d.ItemSequence }).Select(d => d.ItemCode).ToList();
            var items = db.Items.ToList().Where(d => itemCodes.Contains(d.ItemCode))
                        .Select(d => new UnregisteredSuppliesVM
                        {
                            ArticleReference = d.ArticleReference,
                            Sequence = d.Sequence,
                            ItemCode = d.ItemCode,
                            Article = d.FKArticleReference.ArticleName,
                            Description = d.ItemFullName,
                            IndividualUOMReference = d.IndividualUOMReference,
                            IndividualUOM = d.FKIndividualUnitReference.UnitName
                        }).ToList();
            var supplies = db.Supplies.Select(d => new UnregisteredSuppliesVM
            {
                            ArticleReference = d.ArticleReference,
                            Sequence = d.Sequence,
                            ItemCode = d.FKArticleReference.ArticleCode + "-" + d.Sequence,
                            Article = d.FKArticleReference.ArticleName,
                            Description = d.Description,
                            IndividualUOMReference = d.IndividualUOMReference.Value,
                            IndividualUOM = d.FKIndividualUnitReference.UnitName
            }).ToList();
            var unregisteredItems = items.Where(d => !supplies.Any(x => x.ItemCode == d.ItemCode && x.IndividualUOM == d.IndividualUOM)).ToList();

            return unregisteredItems;
        }
        public List<UnregisteredPPEVM> GetUnregisteredPPE(string ReferenceNo)
        {
            var itemCodes = db.ContractDetails.Where(d => d.FKContractReference.ReferenceNumber == ReferenceNo && (d.FKContractReference.FKProcurementProjectReference.FKClassificationReference.ID == 2)).ToList()
                            .Select(d => new { ItemCode = d.FKArticleReference.ArticleCode + "-" + d.ItemSequence }).Select(d => d.ItemCode).ToList();
            var items = db.Items.ToList().Where(d => itemCodes.Contains(d.ItemCode))
                        .Select(d => new UnregisteredPPEVM
                        {
                            ArticleReference = d.ArticleReference,
                            Sequence = d.Sequence,
                            ItemCode = d.ItemCode,
                            Article = d.FKArticleReference.ArticleName,
                            Description = d.ItemFullName,
                            IndividualUOMReference = d.IndividualUOMReference,
                            IndividualUOM = d.FKIndividualUnitReference.UnitName
                        }).ToList();
            var ppe = db.PPE.Select(d => new UnregisteredPPEVM
            {
                ArticleReference = d.ArticleReference,
                Sequence = d.Sequence,
                ItemCode = d.FKArticleReference.ArticleCode + "-" + d.Sequence,
                Article = d.FKArticleReference.ArticleName,
                Description = d.Description,
                IndividualUOMReference = d.IndividualUOMReference.Value,
                IndividualUOM = d.FKIndividualUnitReference.UnitName
            }).ToList();
            var unregisteredItems = items.Where(d => !ppe.Any(x => x.ItemCode == d.ItemCode && x.IndividualUOM == d.IndividualUOM)).ToList();
            return unregisteredItems;
        }
        public DeliveryVM GetDeliverySetup(string ReferenceNo)
        {
            var contract = db.Contract.Where(d => d.ReferenceNumber == ReferenceNo).FirstOrDefault();
            var supplier = contract.FKSupplierReference.SupplierName;
            var supplierAddress = contract.FKSupplierReference.Address + (contract.FKSupplierReference.City == null || contract.FKSupplierReference.City == string.Empty ? string.Empty : ", " + contract.FKSupplierReference.City) + (contract.FKSupplierReference.State == null || contract.FKSupplierReference.State == string.Empty ? string.Empty : ", " + contract.FKSupplierReference.State) + (contract.FKSupplierReference.PostalCode == null || contract.FKSupplierReference.PostalCode == string.Empty ? string.Empty : ", " + contract.FKSupplierReference.PostalCode);
            var contractDetails = db.ContractDetails.Where(d => (d.ContractReference == contract.ID) && (d.Quantity != d.DeliveredQuantity)).ToList();
            var supplyDelivery = (from supplies in db.Supplies.ToList()
                                  join contractItems in contractDetails on new { Article = supplies.ArticleReference, Sequence = supplies.Sequence } equals new { Article = (int)contractItems.ArticleReference, Sequence = contractItems.ItemSequence }
                                  where contractItems.FKArticleReference.FKItemTypeReference.FKItemClassificationReference.ID == 1 || contractItems.FKArticleReference.FKItemTypeReference.FKItemClassificationReference.ID == 3 || contractItems.FKArticleReference.FKItemTypeReference.FKItemClassificationReference.ID == 4
                                  select new SupplyDeliveryVM
                                  {
                                      SupplyReference = supplies.ID,
                                      ArticleReference = supplies.ArticleReference,
                                      Sequence = supplies.Sequence,
                                      StockNumber = supplies.StockNumber,
                                      Description = supplies.Description,
                                      ContractUnitPrice = contractItems.ContractUnitPrice,
                                      ContractTotalPrice = contractItems.ContractTotalPrice,
                                      Quantity = contractItems.DeliveredQuantity == null ? contractItems.Quantity : contractItems.Quantity - contractItems.DeliveredQuantity.Value,
                                      QuantityDelivered = contractItems.DeliveredQuantity == null ? contractItems.Quantity : contractItems.Quantity - contractItems.DeliveredQuantity.Value,
                                      IndividualUOMReference = supplies.IndividualUOMReference.Value,
                                      IndividualUOM = supplies.FKIndividualUnitReference.UnitName,
                                      MinimumIssuanceQty = supplies.MinimumIssuanceQty.ToString() + " " + supplies.FKIndividualUnitReference.Abbreviation
                                  }).ToList();

            var ppeDelivery = (from ppe in db.PPE.ToList()
                               join contractItems in contractDetails on new { Article = ppe.ArticleReference, Sequence = ppe.Sequence } equals new { Article = (int)contractItems.ArticleReference, Sequence = contractItems.ItemSequence }
                               where contractItems.FKArticleReference.FKItemTypeReference.FKItemClassificationReference.ID == 2
                               select new PPEDeliveryVM
                               {
                                   PPEReference = ppe.ID,
                                   ArticleReference = ppe.ArticleReference,
                                   Sequence = ppe.Sequence,
                                   PropertyNo = ppe.PropertyNumberRoot,
                                   Description = ppe.Description,
                                   ContractUnitPrice = contractItems.ContractUnitPrice,
                                   ContractTotalPrice = contractItems.ContractTotalPrice,
                                   Quantity = contractItems.DeliveredQuantity == null ? contractItems.Quantity : contractItems.Quantity - contractItems.DeliveredQuantity.Value,
                                   QuantityDelivered = contractItems.DeliveredQuantity == null ? contractItems.Quantity : contractItems.Quantity - contractItems.DeliveredQuantity.Value,
                                   IndividualUOMReference = ppe.IndividualUOMReference.Value,
                                   IndividualUOM = ppe.FKIndividualUnitReference.UnitName
                               }).ToList();

            return new DeliveryVM
            {
                DeliveryAcceptanceNumber = GenerateDeliveryAcceptanceNumber(),
                ReferenceNo = contract.ReferenceNumber,
                ContractReference = contract.ID,
                ContractCode = contract.FKProcurementProjectReference.ContractCode,
                ContractName = contract.FKProcurementProjectReference.ContractName,
                ContractType = contract.ContractType,
                ContractPrice = contract.ContractPrice,
                ContractStatus = contract.ContractStatus.Value,
                FundSource = contract.FKProcurementProjectReference.FundSource,
                FundDescription = abis.GetFundSources(contract.FKProcurementProjectReference.FundSource).FUND_DESC,
                SupplierName = supplier,
                SupplierAddress = supplierAddress,
                Supplies = supplyDelivery,
                PPE = ppeDelivery
            };
        }
        public DeliveryAcceptanceTemplateVM GetDeliveryAcceptanceReportSetup(string DeliveryAcceptanceNo)
        {
            var delivery = db.DeliveryHeader.Where(d => d.DeliveryAcceptanceNumber == DeliveryAcceptanceNo).FirstOrDefault();
            var supplier = delivery.FKContractReference.FKSupplierReference.SupplierName;
            var supplierAddress = delivery.FKContractReference.FKSupplierReference.Address + (delivery.FKContractReference.FKSupplierReference.City == null || delivery.FKContractReference.FKSupplierReference.City == string.Empty ? string.Empty : ", " + delivery.FKContractReference.FKSupplierReference.City) + (delivery.FKContractReference.FKSupplierReference.State == null || delivery.FKContractReference.FKSupplierReference.State == string.Empty ? string.Empty : ", " + delivery.FKContractReference.FKSupplierReference.State) + (delivery.FKContractReference.FKSupplierReference.PostalCode == null || delivery.FKContractReference.FKSupplierReference.PostalCode == string.Empty ? string.Empty : ", " + delivery.FKContractReference.FKSupplierReference.PostalCode);
            var supplyDelivery = (from supplies in db.Supplies.ToList()
                                  join contractItems in db.SupplyDelivery.Where(d => d.DeliveryReference == delivery.ID).ToList() on new { Article = supplies.ArticleReference, Sequence = supplies.Sequence } equals new { Article = (int)contractItems.FKSupplyReference.ArticleReference, Sequence = contractItems.FKSupplyReference.Sequence }
                                  where contractItems.FKSupplyReference.FKArticleReference.FKItemTypeReference.FKItemClassificationReference.ID == 1 || contractItems.FKSupplyReference.FKArticleReference.FKItemTypeReference.FKItemClassificationReference.ID == 3 || contractItems.FKSupplyReference.FKArticleReference.FKItemTypeReference.FKItemClassificationReference.ID == 4
                                  select new SupplyDeliveryVM
                                  {
                                      SupplyReference = supplies.ID,
                                      ArticleReference = supplies.ArticleReference,
                                      Sequence = supplies.Sequence,
                                      StockNumber = supplies.StockNumber,
                                      Description = supplies.Description,
                                      ContractUnitPrice = contractItems.ReceiptUnitCost,
                                      ContractTotalPrice = contractItems.ReceiptTotalCost,
                                      Quantity = contractItems.QuantityBacklog,
                                      QuantityDelivered = contractItems.QuantityDelivered,
                                      IndividualUOMReference = supplies.IndividualUOMReference.Value,
                                      IndividualUOM = supplies.FKIndividualUnitReference.UnitName,
                                      MinimumIssuanceQty = supplies.MinimumIssuanceQty.ToString() + " " + supplies.FKIndividualUnitReference.Abbreviation
                                  }).ToList();

            var ppeDelivery = (from ppe in db.PPE.ToList()
                               join contractItems in db.PPEDelivery.Where(d => d.DeliveryReference == delivery.ID).ToList() on new { Article = ppe.ArticleReference, Sequence = ppe.Sequence } equals new { Article = (int)contractItems.FKPPEReference.ArticleReference, Sequence = contractItems.FKPPEReference.Sequence }
                               where contractItems.FKPPEReference.FKArticleReference.FKItemTypeReference.FKItemClassificationReference.ID == 2
                               select new PPEDeliveryVM
                               {
                                   PPEReference = ppe.ID,
                                   ArticleReference = ppe.ArticleReference,
                                   Sequence = ppe.Sequence,
                                   PropertyNo = ppe.PropertyNumberRoot,
                                   Description = ppe.Description,
                                   ContractUnitPrice = contractItems.ReceiptUnitCost,
                                   ContractTotalPrice = contractItems.ReceiptTotalCost,
                                   Quantity = contractItems.QuantityBacklog,
                                   QuantityDelivered = contractItems.QuantityDelivered,
                                   IndividualUOMReference = ppe.IndividualUOMReference.Value,
                                   IndividualUOM = ppe.FKIndividualUnitReference.Abbreviation
                               }).ToList();

            return new DeliveryAcceptanceTemplateVM
            {
                DeliveryAcceptanceNumber = delivery.DeliveryAcceptanceNumber,
                ReferenceNo = delivery.Reference,
                Date = delivery.FKContractReference.CreatedAt,
                InvoiceNumber = delivery.InvoiceNumber,
                InvoiceDate = delivery.InvoiceDate,
                DRNumber = delivery.DRNumber,
                DeliveryDate = delivery.DeliveryDate,
                ContractCode = delivery.FKContractReference.FKProcurementProjectReference.ContractCode,
                ContractName = delivery.FKContractReference.FKProcurementProjectReference.ContractName,
                ContractType = delivery.ContractType,
                ContractPrice = delivery.FKContractReference.ContractPrice,
                ContractStatus = delivery.FKContractReference.ContractStatus.Value,
                FundSource = delivery.FKContractReference.FKProcurementProjectReference.FundSource,
                FundDescription = abis.GetFundSources(delivery.FKContractReference.FKProcurementProjectReference.FundSource).FUND_DESC,
                SupplierName = supplier,
                SupplierAddress = supplierAddress,
                DateProcessed = delivery.DateProcessed,
                ProcessedBy = hris.GetEmployeeDetailByCode(delivery.ProcessedBy).EmployeeName,
                ReceivedBy = hris.GetEmployeeDetailByCode(delivery.ReceivedBy).EmployeeName,
                DeliveryCompleteness = delivery.DeliveryCompleteness.Value,
                Supplies = supplyDelivery,
                PPE = ppeDelivery
            };
        }
        public List<DeliveryListVM> GetDeliveryList()
        {
            return db.DeliveryHeader.ToList().Select(d => new DeliveryListVM
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
        public bool PostUnregisteredItems(UnregisteredItemsVM UnregisteredItems, string UserEmail)
        {
            var user = db.UserAccounts.Where(d => d.Email == UserEmail).FirstOrDefault();
            var supplies = UnregisteredItems.Supplies;
            var ppe = UnregisteredItems.PPE;

            if(supplies != null)
            {
                foreach (var supply in supplies)
                {
                    var supplyRecord = db.Supplies.Add(new Supplies
                    {
                        StockNumber = GenerateStockNumber(supply.ArticleReference),
                        ArticleReference = supply.ArticleReference,
                        Sequence = supply.Sequence,
                        StockSequence = GenerateStockSeries(supply.ArticleReference),
                        Description = supply.Description,
                        ItemImage = supply.ItemImage,
                        ReOrderPoint = supply.ReOrderPoint,
                        IndividualUOMReference = supply.IndividualUOMReference,
                        MinimumIssuanceQty = supply.MinimumIssuanceQty,
                        PurgeFlag = false,
                        CreatedBy = user.EmpCode,
                        CreatedAt = DateTime.Now,
                    });
                    var _currentValues = db.ChangeTracker.Entries().Where(d => d.State == EntityState.Added).First().CurrentValues;
                    if (db.SaveChanges() == 0)
                    {
                        return false;
                    }

                    LogsMasterTables _log = new LogsMasterTables();
                    _log.AuditableKey = supplyRecord.ID;
                    _log.ProcessedBy = user.ID;
                    _log.Action = "Add Record";
                    _log.TableName = "PROP_MSTR_Supplies";
                    MasterTablesLogger _logger = new MasterTablesLogger();
                    _logger.Log(_log, null, _currentValues);
                }
            }

            if(ppe != null)
            {
                foreach (var property in ppe)
                {
                    var propertyRecord = db.PPE.Add(new PPE
                    {
                        PropertyNumberRoot = GeneratePropertyNumberRoot(property.ArticleReference),
                        ArticleReference = property.ArticleReference,
                        Sequence = property.Sequence,
                        Description = property.Description,
                        ItemImage = property.ItemImage,
                        IndividualUOMReference = property.IndividualUOMReference,
                        PurgeFlag = false,
                        CreatedBy = user.EmpCode,
                        CreatedAt = DateTime.Now
                    });
                    var _currentValues = db.ChangeTracker.Entries().Where(d => d.State == EntityState.Added).First().CurrentValues;
                    if (db.SaveChanges() == 0)
                    {
                        return false;
                    }

                    LogsMasterTables _log = new LogsMasterTables();
                    _log.AuditableKey = propertyRecord.ID;
                    _log.ProcessedBy = user.ID;
                    _log.Action = "Add Record";
                    _log.TableName = "PROP_MSTR_PPE";
                    MasterTablesLogger _logger = new MasterTablesLogger();
                    _logger.Log(_log, null, _currentValues);
                }
            }

            return true;
        }
        public bool PostDelivery(DeliveryVM Delivery, string UserEmail)
        {
            var user = db.UserAccounts.Where(d => d.Email == UserEmail).FirstOrDefault();
            var delivery = db.DeliveryHeader.Add(new Delivery
            {
                DeliveryAcceptanceNumber = Delivery.DeliveryAcceptanceNumber,
                ContractReference = Delivery.ContractReference,
                DateProcessed = DateTime.Now,
                ProcessedBy = user.EmpCode,
                ReceivedBy = Delivery.ReceivedBy,
                Reference = Delivery.ReferenceNo,
                ContractType = Delivery.ContractType,
                InvoiceNumber = Delivery.InvoiceNumber,
                InvoiceDate = Delivery.InvoiceDate,
                DRNumber = Delivery.DRNumber,
                DeliveryDate = Delivery.DeliveryDate
            });

            if(db.SaveChanges() == 0)
            {
                return false;
            }

            if(Delivery.Supplies != null)
            {
                foreach (var supply in Delivery.Supplies)
                {
                    db.SupplyDelivery.Add(new DeliverySupply
                    {
                        SupplyReference = supply.SupplyReference,
                        DeliveryReference = delivery.ID,
                        QuantityDelivered = supply.QuantityDelivered,
                        QuantityBacklog = supply.Quantity - supply.QuantityDelivered,
                        ReceiptUnitCost = supply.ContractUnitPrice,
                        ReceiptTotalCost = Math.Round(supply.QuantityDelivered * supply.ContractUnitPrice)
                    });

                    var supplyDetails = db.ContractDetails.Where(d => d.ContractReference == Delivery.ContractReference && d.ArticleReference == supply.ArticleReference && d.ItemSequence == supply.Sequence).FirstOrDefault();
                    supplyDetails.DeliveredQuantity = supplyDetails.DeliveredQuantity == null ? supply.QuantityDelivered : supplyDetails.DeliveredQuantity + supply.QuantityDelivered;

                    if (db.SaveChanges() == 0)
                    {
                        return false;
                    }
                }
            }

            if (Delivery.PPE != null)
            {
                foreach(var ppe in Delivery.PPE)
                {
                    db.PPEDelivery.Add(new DeliveryProperty
                    {
                        PPEReference = ppe.PPEReference,
                        DeliveryReference = delivery.ID,
                        QuantityDelivered = ppe.QuantityDelivered,
                        QuantityBacklog = ppe.Quantity - ppe.QuantityDelivered,
                        ReceiptUnitCost = ppe.ContractUnitPrice,
                        ReceiptTotalCost = Math.Round(ppe.QuantityDelivered * ppe.ContractUnitPrice)
                    });

                    var ppeDetails = db.ContractDetails.Where(d => d.ContractReference == Delivery.ContractReference && d.ArticleReference == ppe.ArticleReference && d.ItemSequence == ppe.Sequence).FirstOrDefault();
                    ppeDetails.DeliveredQuantity = ppeDetails.DeliveredQuantity == null ? ppe.QuantityDelivered : ppeDetails.DeliveredQuantity + ppe.QuantityDelivered;

                    if (db.SaveChanges() == 0)
                    {
                        return false;
                    }
                }
            }

            var contract = db.Contract.Find(Delivery.ContractReference);
            var backlogCount = db.ContractDetails.Where(d => d.Quantity != d.DeliveredQuantity).Count();
            if (backlogCount == 0)
            {
                delivery.DeliveryCompleteness = DeliveryCompleteness.CompleteDelivery;
                contract.ContractStatus = ContractStatus.DeliveryCompleted;
            }
            else
            {
                delivery.DeliveryCompleteness = DeliveryCompleteness.PartialDelivery;
                contract.ContractStatus = ContractStatus.PartialDelivery;
            }

            if (db.SaveChanges() == 0)
            {
                return false;
            }

            return true;
        }

        private string GenerateStockNumber(int ArticleReference)
        {
            var articleCode = db.ItemArticles.Find(ArticleReference).ArticleCode;
            var sequence = (db.Supplies.Where(d => d.ArticleReference == ArticleReference).Count() + 1).ToString();
            return articleCode + "-" + (sequence.Length == 1 ? "0" + sequence : sequence);
        }
        private string GenerateStockSeries(int ArticleReference)
        {
            var sequence = (db.Supplies.Where(d => d.ArticleReference == ArticleReference).Count() + 1).ToString();
            return sequence.Length == 1 ? "0" + sequence : sequence;
        }
        private string GeneratePropertyNumberRoot(int ArticleReference)
        {
            var article = db.ItemArticles.Find(ArticleReference);
            var articleCode = article.ArticleCode;
            var uacs = abis.GetChartOfAccounts(article.UACSObjectClass).SubAcctCode;
            uacs = uacs.Length == 1 ? "0" + uacs : uacs;
            var GLCode = article.GLAccount;
            var sequence = (db.PPE.Where(d => d.ArticleReference == ArticleReference).Count() + 1).ToString();
            return uacs + "-" + GLCode + "-" + (sequence.Length == 1 ? articleCode + "0" + sequence : articleCode + sequence);
        }
        private string GenerateDeliveryAcceptanceNumber()
        {
            string series = (db.DeliveryHeader.Where(d => d.DateProcessed.Year == DateTime.Now.Year).Count() + 1).ToString();
            series = series.Length == 1 ? "000" + series : series.Length == 2 ? "00" + series : series.Length == 3 ? "0" + series : series;
            return "DRR-" + DateTime.Now.ToString("yy") + "-" + DateTime.Now.ToString("MM") + "-" + series;
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