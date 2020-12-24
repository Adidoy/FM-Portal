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
    
    public class MyProjectsBL : Controller
    {
        MyProjectsDAL myProjectsDAL = new MyProjectsDAL();
        FMISDbContext db = new FMISDbContext();

        public MemoryStream GenerateNoticeOfAward(string PAPCode, string LogoPath, string UserEmail)
        {
            Reports reports = new Reports();
            var procurementProgram = myProjectsDAL.GetProcurementProgramDetailsByPAPCode(PAPCode);
            var procurementTimeLine = db.ProcurementTimeline.Where(d => d.PAPCode == PAPCode).FirstOrDefault();
            var APPReference = db.APPHeader.Where(d => d.ReferenceNo == procurementProgram.APPReference).FirstOrDefault();
            var supplier = db.Suppliers.Find(procurementProgram.Supplier);
            reports.ReportFilename = "NOA-" + PAPCode;
            reports.CreateDocument(8.50, 13.00, Orientation.Portrait, 0.50, 1.00);
            reports.AddDoubleColumnHeader(LogoPath);
            reports.AddColumnHeader(
                new HeaderLine { Content = "Republic of the Philippines", Bold = false, Italic = false, FontSize = 10, ParagraphAlignment = ParagraphAlignment.Left },
                new HeaderLine { Content = "", Bold = true, Italic = false, FontSize = 10, ParagraphAlignment = ParagraphAlignment.Left }
            );
            reports.AddColumnHeader(
                new HeaderLine { Content = "POLYTECHNIC UNIVERSITY OF THE PHILIPPINES", Bold = false, Italic = false, FontSize = 12, ParagraphAlignment = ParagraphAlignment.Left },
                new HeaderLine { Content = "", Bold = false, Italic = true, FontSize = 8, ParagraphAlignment = ParagraphAlignment.Left }
            );
            reports.AddColumnHeader(
                new HeaderLine { Content = "OFFICE OF THE PRESIDENT", Bold = true, Italic = false, FontSize = 16, ParagraphAlignment = ParagraphAlignment.Left },
                new HeaderLine { Content = "", Bold = false, Italic = false, FontSize = 8, ParagraphAlignment = ParagraphAlignment.Left }
            );

            var columns = new List<ContentColumn>();
            columns.Add(new ContentColumn(6.50));
            reports.AddTable(columns, false);

            var rows = new List<ContentCell>();
            rows.Add(new ContentCell("", 0, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Center, 0, 0, true));
            reports.AddRowContent(rows, 0);

            reports.AddNewLine();
            reports.AddNewLine();

            columns = new List<ContentColumn>();
            columns.Add(new ContentColumn(6.50));
            reports.AddTable(columns, false);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("NOTICE OF AWARD", 0, 14, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
            reports.AddRowContent(rows, 0);

            reports.AddNewLine();
            reports.AddNewLine();

            columns = new List<ContentColumn>();
            columns.Add(new ContentColumn(6.50));
            reports.AddTable(columns, false);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell(procurementTimeLine.ActualNOAIssuance.Value.ToString("dd MMMM yyyy"), 0, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Center));
            reports.AddRowContent(rows, 0);

            reports.AddNewLine();
            reports.AddNewLine();

            columns = new List<ContentColumn>();
            columns.Add(new ContentColumn(6.50));
            reports.AddTable(columns, false);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell(supplier.ContactPerson.ToUpper(), 0, 10, true, false, ParagraphAlignment.Justify, VerticalAlignment.Center));
            reports.AddRowContent(rows, 0);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("Authorized and Designated Representative", 0, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Center));
            reports.AddRowContent(rows, 0);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell(supplier.SupplierName, 0, 10, true, false, ParagraphAlignment.Justify, VerticalAlignment.Center));
            reports.AddRowContent(rows, 0);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell(supplier.Address, 0, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Center));
            reports.AddRowContent(rows, 0);

            reports.AddNewLine();
            reports.AddNewLine();

            columns = new List<ContentColumn>();
            columns.Add(new ContentColumn(6.50));
            reports.AddTable(columns, false);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("Dear " + supplier.ContactPerson.ToUpper() + ":", 0, 10, false, true, ParagraphAlignment.Justify, VerticalAlignment.Center));
            reports.AddRowContent(rows, 0);

            reports.AddNewLine();

            columns = new List<ContentColumn>();
            columns.Add(new ContentColumn(6.50));
            reports.AddTable(columns, false);

            var projectCostWords = Reports.AmountToWords(procurementProgram.ProjectCost);

            reports.AddFormattedRowContent(new ContentCell(new TextWithFormat[]
            {
                new TextWithFormat("This serves as notice that the PUP Bids and Awards Committee (PUP BAC), has recommended the award of the procurement of ", false, false, 10),
                new TextWithFormat("“"+ procurementProgram.ProcurementProgram +"”", true, true, 10),
                new TextWithFormat(" through Emergency Procurement in the amount of ", false, false, 10),
                new TextWithFormat(projectCostWords + " (" + procurementProgram.ProjectCost.ToString("C", new System.Globalization.CultureInfo("en-ph")) + ")", true, true, 10),
                new TextWithFormat(" to " + supplier.SupplierName + ".", false, false, 10),
            }, MigraDoc.DocumentObjectModel.ParagraphAlignment.Justify), 0);

            reports.AddNewLine();

            columns = new List<ContentColumn>();
            columns.Add(new ContentColumn(6.50));
            reports.AddTable(columns, false);

            reports.AddFormattedRowContent(new ContentCell(new TextWithFormat[]
            {
                new TextWithFormat("You are therefore advised within ten (10) calendar days from the receipt of this Notice of Award to formally enter into contract with us. Failure to enter into the said contract shall constitute a ground for cancellation of this award.", false, false, 10),
            }, MigraDoc.DocumentObjectModel.ParagraphAlignment.Justify), 0);

            reports.AddNewLine();

            columns = new List<ContentColumn>();
            columns.Add(new ContentColumn(6.50));
            reports.AddTable(columns, false);

            reports.AddFormattedRowContent(new ContentCell(new TextWithFormat[]
            {
                new TextWithFormat("Kindly signify your conformity by signing on the space herein provided.", false, false, 10),
            }, MigraDoc.DocumentObjectModel.ParagraphAlignment.Justify), 0);

            reports.AddNewLine();

            columns = new List<ContentColumn>();
            columns.Add(new ContentColumn(6.50));
            reports.AddTable(columns, false);

            reports.AddFormattedRowContent(new ContentCell(new TextWithFormat[]
            {
                new TextWithFormat("Thank you.", false, false, 10),
            }, MigraDoc.DocumentObjectModel.ParagraphAlignment.Justify), 0);

            reports.AddNewLine();

            columns = new List<ContentColumn>();
            columns.Add(new ContentColumn(6.50));
            reports.AddTable(columns, false);

            reports.AddFormattedRowContent(new ContentCell(new TextWithFormat[]
            {
                new TextWithFormat("Very truly yours,", false, false, 10),
            }, MigraDoc.DocumentObjectModel.ParagraphAlignment.Justify), 0);

            reports.AddNewLine();
            reports.AddNewLine();
            reports.AddNewLine();

            columns = new List<ContentColumn>();
            columns.Add(new ContentColumn(6.50));
            reports.AddTable(columns, false);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell(APPReference.ApprovedBy.ToUpper(), 0, 10, true, false, ParagraphAlignment.Justify, VerticalAlignment.Center));
            reports.AddRowContent(rows, 0);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell(APPReference.ApprovedByDesignation, 0, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Center));
            reports.AddRowContent(rows, 0);

            reports.AddNewLine();
            reports.AddNewLine();
            reports.AddNewLine();

            columns = new List<ContentColumn>();
            columns.Add(new ContentColumn(6.50));
            reports.AddTable(columns, false);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("Conforme:", 0, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Center));
            reports.AddRowContent(rows, 0);

            reports.AddNewLine();
            reports.AddNewLine();

            columns = new List<ContentColumn>();
            columns.Add(new ContentColumn(6.50));
            reports.AddTable(columns, false);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell(supplier.ContactPerson.ToUpper(), 0, 10, true, false, ParagraphAlignment.Justify, VerticalAlignment.Bottom));
            reports.AddRowContent(rows, 0);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("Authorized and Designated Representative", 0, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Bottom));
            reports.AddRowContent(rows, 0);

            columns = new List<ContentColumn>();
            columns.Add(new ContentColumn(0.50));
            columns.Add(new ContentColumn(1.50));
            columns.Add(new ContentColumn(3.50));
            reports.AddTable(columns, false);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("Date:", 0, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Bottom));
            rows.Add(new ContentCell("", 1, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Bottom, 0, 0, true));
            rows.Add(new ContentCell("", 1, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Bottom));
            reports.AddRowContent(rows, 0);

            return reports.GenerateReport();
        }
        public MemoryStream GeneratePurchaseOrder(string PAPCode, string LogoPath, string UserEmail)
        {
            Reports reports = new Reports();
            var procurementProgram = myProjectsDAL.GetProcurementProgramDetailsByPAPCode(PAPCode);
            var purchaseOrderHeader = db.PurchaseOrderHeader.Where(d => d.PurchaseOrderNumber == procurementProgram.PONumber).FirstOrDefault();
            var purchaseOrderDetails = db.PurchaseOrderDetails.Where(d => d.FKPurchaseOrderHeaderReference.PurchaseOrderNumber == procurementProgram.PONumber).ToList();
            var modeOfProcurement = db.ProcurementModes.Find(procurementProgram.ModeOfProcurement);
            var supplier = db.Suppliers.Find(procurementProgram.Supplier);
            reports.ReportFilename = "PO-" + purchaseOrderHeader.PurchaseOrderNumber + "(" + PAPCode + ")";
            reports.CreateDocument(8.50, 13.00, Orientation.Portrait, 0.25);
            
            var columns = new List<ContentColumn>();
            columns.Add(new ContentColumn(8.00));
            reports.AddTable(columns, false);

            var rows = new List<ContentCell>();
            rows.Add(new ContentCell("Apendix 61", 0, 10, false, true, ParagraphAlignment.Right, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0), true, true, true));
            reports.AddRowContent(rows, 0);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("", 0, 8, false, false, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0), true, false, true));
            reports.AddRowContent(rows, 0);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("PURCHASE ORDER", 0, 12, true, false, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0), true, false, true));
            reports.AddRowContent(rows, 0);

            columns = new List<ContentColumn>();
            columns.Add(new ContentColumn(1.50));
            columns.Add(new ContentColumn(5.00));
            columns.Add(new ContentColumn(1.50));
            reports.AddTable(columns, false);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("", 0, 10, false, false, ParagraphAlignment.Center, VerticalAlignment.Bottom, 0, 0, false, new Color(0, 0, 0), false, false, true));
            rows.Add(new ContentCell("POLYTECHNIC UNIVERSITY OF THE PHILIPPINES", 1, 10, false, false, ParagraphAlignment.Center, VerticalAlignment.Bottom, 0, 0, true));
            rows.Add(new ContentCell("", 2, 10, false, false, ParagraphAlignment.Center, VerticalAlignment.Bottom, 0, 0, false, new Color(0, 0, 0), true, false, false));
            reports.AddRowContent(rows, 0);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("", 0, 10, false, false, ParagraphAlignment.Center, VerticalAlignment.Bottom, 0, 0, false, new Color(0, 0, 0), false, false, true));
            rows.Add(new ContentCell("(Agency)", 1, 8, false, true, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 0, false));
            rows.Add(new ContentCell("", 2, 10, false, false, ParagraphAlignment.Center, VerticalAlignment.Bottom, 0, 0, false, new Color(0, 0, 0), true, false, false));
            reports.AddRowContent(rows, 0);

            columns = new List<ContentColumn>();
            columns.Add(new ContentColumn(1.00));
            columns.Add(new ContentColumn(3.75));
            columns.Add(new ContentColumn(1.50));
            columns.Add(new ContentColumn(1.75));
            reports.AddTable(columns, false);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("Supplier: ", 0, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Bottom, 0, 0, false, new Color(0, 0, 0), false, true, true));
            rows.Add(new ContentCell(supplier.SupplierName.ToUpper(), 1, 10, true, false, ParagraphAlignment.Left, VerticalAlignment.Bottom, 0, 0, true, new Color(0, 0, 0), true, true, false));
            rows.Add(new ContentCell("P.O. No.: ", 2, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Bottom, 0, 0, false, new Color(0, 0, 0), false, true, false));
            rows.Add(new ContentCell(purchaseOrderHeader.PurchaseOrderNumber, 3, 10, true, false, ParagraphAlignment.Left, VerticalAlignment.Bottom, 0, 0, true, new Color(0, 0, 0), true, true, false));
            reports.AddRowContent(rows, 0.20);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("Address: ", 0, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Bottom, 0, 0, false, new Color(0, 0, 0), false, true, true));
            rows.Add(new ContentCell(supplier.Address, 1, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Bottom, 0, 0, true, new Color(0, 0, 0), true, true, false));
            rows.Add(new ContentCell("Date: ", 2, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Bottom, 0, 0, false, new Color(0, 0, 0), false, true, false));
            rows.Add(new ContentCell(purchaseOrderHeader.CreatedAt.ToString("dd MMMM yyyy"), 3, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Bottom, 0, 0, true, new Color(0, 0, 0), true, true, false));
            reports.AddRowContent(rows, 0.20);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("TIN: ", 0, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Bottom, 0, 0, false, new Color(0, 0, 0), false, true, true));
            rows.Add(new ContentCell((supplier.TaxIdNumber == null ? string.Empty : supplier.TaxIdNumber), 1, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Bottom, 0, 0, true, new Color(0, 0, 0), true, true, false));
            rows.Add(new ContentCell("Mode of Procurement: ", 2, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Bottom, 0, 0, false, new Color(0, 0, 0), false, true, false));
            rows.Add(new ContentCell(modeOfProcurement.ModeOfProcurementName, 3, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Bottom, 0, 0, true, new Color(0, 0, 0), true, true, false));
            reports.AddRowContent(rows, 0.20);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("", 0, 3, false, false, ParagraphAlignment.Left, VerticalAlignment.Bottom, 0, 0, true, new Color(0, 0, 0), false, false, true));
            rows.Add(new ContentCell("", 1, 3, false, false, ParagraphAlignment.Left, VerticalAlignment.Bottom, 0, 0, true, new Color(0, 0, 0), true, false, false));
            rows.Add(new ContentCell("", 2, 3, false, false, ParagraphAlignment.Left, VerticalAlignment.Bottom, 0, 0, true, new Color(0, 0, 0), false, false, false));
            rows.Add(new ContentCell("", 3, 3, false, false, ParagraphAlignment.Left, VerticalAlignment.Bottom, 0, 0, true, new Color(0, 0, 0), true, false, false));
            reports.AddRowContent(rows, 0);

            columns = new List<ContentColumn>();
            columns.Add(new ContentColumn(8.00));
            reports.AddTable(columns, true);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("Gentlemen:\n\n\tPlease furnish this office the following articles subject to the terms and conditions contained herein:", 0, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Bottom));
            reports.AddRowContent(rows, 0);

            columns = new List<ContentColumn>();
            columns.Add(new ContentColumn(1.25));
            columns.Add(new ContentColumn(4.50));
            columns.Add(new ContentColumn(1.25));
            columns.Add(new ContentColumn(1.00));
            reports.AddTable(columns, false);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("Place of Delivery: ", 0, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Bottom, 0, 0, false, new Color(0, 0, 0), false, true, true));
            rows.Add(new ContentCell(purchaseOrderHeader.PlaceOfDelivery, 1, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Bottom, 0, 0, true, new Color(0, 0, 0), true, true, false));
            rows.Add(new ContentCell("Delivery Term: ", 2, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Bottom, 0, 0, false, new Color(0, 0, 0), false, true, false));
            rows.Add(new ContentCell("", 3, 10, true, false, ParagraphAlignment.Left, VerticalAlignment.Bottom, 0, 0, true, new Color(0, 0, 0), true, true, false));
            reports.AddRowContent(rows, 0.20);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("Date of Delivery: ", 0, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Bottom, 0, 0, false, new Color(0, 0, 0), false, true, true));
            rows.Add(new ContentCell(Reports.DigitsToWords((int)purchaseOrderHeader.DateOfDelivery) + " (" + purchaseOrderHeader.DateOfDelivery.ToString() + ") calendar days upon receipt of N.T.P.", 1, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Bottom, 0, 0, true, new Color(0, 0, 0), true, true, false));
            rows.Add(new ContentCell("Payment Terms: ", 2, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Bottom, 0, 0, false, new Color(0, 0, 0), false, true, false));
            rows.Add(new ContentCell("", 3, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Bottom, 0, 0, true, new Color(0, 0, 0), true, true, false));
            reports.AddRowContent(rows, 0.20);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("", 0, 3, false, false, ParagraphAlignment.Left, VerticalAlignment.Bottom, 0, 0, true, new Color(0, 0, 0), false, false, true));
            rows.Add(new ContentCell("", 1, 3, false, false, ParagraphAlignment.Left, VerticalAlignment.Bottom, 0, 0, true, new Color(0, 0, 0), true, false, false));
            rows.Add(new ContentCell("", 2, 3, false, false, ParagraphAlignment.Left, VerticalAlignment.Bottom, 0, 0, true, new Color(0, 0, 0), false, false, false));
            rows.Add(new ContentCell("", 3, 3, false, false, ParagraphAlignment.Left, VerticalAlignment.Bottom, 0, 0, true, new Color(0, 0, 0), true, false, false));
            reports.AddRowContent(rows, 0);

            columns = new List<ContentColumn>();
            columns.Add(new ContentColumn(0.75));
            columns.Add(new ContentColumn(0.75));
            columns.Add(new ContentColumn(0.75));
            columns.Add(new ContentColumn(3.75));
            columns.Add(new ContentColumn(1.00));
            columns.Add(new ContentColumn(1.00));
            reports.AddTable(columns, true);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("Item No.", 0, 10, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
            rows.Add(new ContentCell("Unit", 1, 10, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
            rows.Add(new ContentCell("Quantity", 2, 10, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
            rows.Add(new ContentCell("Description", 3, 10, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
            rows.Add(new ContentCell("Unit Cost", 4, 10, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
            rows.Add(new ContentCell("Total Cost", 5, 10, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
            reports.AddRowContent(rows, 0.25);

            columns = new List<ContentColumn>();
            columns.Add(new ContentColumn(0.75));
            columns.Add(new ContentColumn(0.75));
            columns.Add(new ContentColumn(0.75));
            columns.Add(new ContentColumn(3.75));
            columns.Add(new ContentColumn(1.00));
            columns.Add(new ContentColumn(1.00));
            reports.AddTable(columns, true);

            int count = 1;
            foreach(var item in purchaseOrderDetails)
            {
                rows = new List<ContentCell>();
                rows.Add(new ContentCell(count.ToString(), 0, 9, false, false, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 1));
                rows.Add(new ContentCell(item.FKItemReference.FKIndividualUnitReference.UnitName, 1, 8, false, false, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 1));
                rows.Add(new ContentCell(item.Quantity.ToString(), 2, 9, false, false, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 1));
                rows.Add(new ContentCell(item.FKItemReference.ItemFullName.ToString(), 3, 9, true, false, ParagraphAlignment.Left, VerticalAlignment.Center));
                rows.Add(new ContentCell(String.Format("{0:0,000.00}", item.UnitCost), 4, 8.5, false, false, ParagraphAlignment.Right, VerticalAlignment.Center, 0, 1));
                rows.Add(new ContentCell(String.Format("{0:0,000.00}",(item.UnitCost * item.Quantity)), 5, 8.5, false, false, ParagraphAlignment.Right, VerticalAlignment.Center, 0, 1));
                reports.AddRowContent(rows, 0.25);

                rows = new List<ContentCell>();
                rows.Add(new ContentCell(item.FKItemReference.ItemSpecifications, 3, 9, false, false, ParagraphAlignment.Justify, VerticalAlignment.Center));
                reports.AddRowContent(rows, 0.25);

                count++;
            }

            if(count <= 20)
            {
                for (int i = count; i < 15; i++)
                {
                    rows = new List<ContentCell>();
                    rows.Add(new ContentCell("", 0, 9, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                    rows.Add(new ContentCell("", 1, 9, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                    rows.Add(new ContentCell("", 2, 9, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                    rows.Add(new ContentCell("", 3, 9, true, false, ParagraphAlignment.Left, VerticalAlignment.Center));
                    rows.Add(new ContentCell("", 4, 9, false, false, ParagraphAlignment.Right, VerticalAlignment.Center));
                    rows.Add(new ContentCell("", 5, 9, false, false, ParagraphAlignment.Right, VerticalAlignment.Center));
                    reports.AddRowContent(rows, 0.25);
                }
            }

            columns = new List<ContentColumn>();
            columns.Add(new ContentColumn(1.50));
            columns.Add(new ContentColumn(4.50));
            columns.Add(new ContentColumn(2.00));
            reports.AddTable(columns, false);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("(Total Amount in Words) ", 0, 9, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, true, new Color(0, 0, 0), false, true, true));
            rows.Add(new ContentCell(Reports.AmountToWords(procurementProgram.ProjectCost).ToUpper() + "ONLY", 1, 10, true, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, true, new Color(0, 0, 0), false, true, false));
            rows.Add(new ContentCell(procurementProgram.ProjectCost.ToString("C", new System.Globalization.CultureInfo("en-ph")), 2, 10, false, false, ParagraphAlignment.Right, VerticalAlignment.Center, 0, 0, true, new Color(0, 0, 0), true, true, true));
            reports.AddRowContent(rows, 0.50);

            columns = new List<ContentColumn>();
            columns.Add(new ContentColumn(8.00));
            reports.AddTable(columns, false);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("\n\tIn case of failure to make the full delivery within the time specified above, a penalty of one-tenth (1/10) of one percent for every day of delay shall be imposed on the undelivered item/s.", 0, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0), true, true, true));
            reports.AddRowContent(rows, 0);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("\n\n", 0, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0), true, true, true));
            reports.AddRowContent(rows, 0);

            columns = new List<ContentColumn>();
            columns.Add(new ContentColumn(0.50));
            columns.Add(new ContentColumn(3.00));
            columns.Add(new ContentColumn(1.00));
            columns.Add(new ContentColumn(3.00));
            columns.Add(new ContentColumn(0.50));
            reports.AddTable(columns, false);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("", 0, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0), false, false, true));
            rows.Add(new ContentCell("Conforme:", 1, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0), false, false, false));
            rows.Add(new ContentCell("", 2, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0), false, false, false));
            rows.Add(new ContentCell("Very truly yours,", 3, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0), false, false, false));
            rows.Add(new ContentCell("", 4, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0), true, false, false));
            reports.AddRowContent(rows, 0);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("\n\n\n", 0, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0), false, false, true));
            rows.Add(new ContentCell("\n\n\n", 1, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0), false, false, false));
            rows.Add(new ContentCell("\n\n\n", 2, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0), false, false, false));
            rows.Add(new ContentCell("\n\n\n", 3, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0), false, false, false));
            rows.Add(new ContentCell("\n\n\n", 4, 10, true, false, ParagraphAlignment.Justify, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0), true, false, false));
            reports.AddRowContent(rows, 0);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("", 0, 10, true, false, ParagraphAlignment.Justify, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0), false, false, true));
            rows.Add(new ContentCell("", 1, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Center, 0, 0, true, new Color(0, 0, 0), false, false, false));
            rows.Add(new ContentCell("", 2, 10, true, false, ParagraphAlignment.Justify, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0), false, false, false));
            rows.Add(new ContentCell(purchaseOrderHeader.AuthorizedSignature.ToUpper(), 3, 10, true, false, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 0, true, new Color(0, 0, 0), false, false, false));
            rows.Add(new ContentCell("", 4, 10, true, false, ParagraphAlignment.Justify, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0), true, false, false));
            reports.AddRowContent(rows, 0);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("", 0, 10, true, false, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0), false, false, true));
            rows.Add(new ContentCell("Signature over Printed Name of Supplier", 1, 10, false, false, ParagraphAlignment.Center, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0), false, false, false));
            rows.Add(new ContentCell("", 2, 10, true, false, ParagraphAlignment.Justify, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0), false, false, false));
            rows.Add(new ContentCell("Signature over Printed Name of Authorized Official", 3, 9, false, false, ParagraphAlignment.Center, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0), false, false, false));
            rows.Add(new ContentCell("", 4, 10, true, false, ParagraphAlignment.Justify, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0), true, false, false));
            reports.AddRowContent(rows, 0);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("\n", 0, 10, true, false, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0), false, false, true));
            rows.Add(new ContentCell("\n", 1, 10, false, false, ParagraphAlignment.Center, VerticalAlignment.Top, 0, 0, true, new Color(0, 0, 0), false, false, false));
            rows.Add(new ContentCell("\n", 2, 10, true, false, ParagraphAlignment.Justify, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0), false, false, false));
            rows.Add(new ContentCell("\n" + purchaseOrderHeader.AuthorizedSignatureDesignation, 3, 10, false, false, ParagraphAlignment.Center, VerticalAlignment.Top, 0, 0, true, new Color(0, 0, 0), false, false, false));
            rows.Add(new ContentCell("\n", 4, 10, true, false, ParagraphAlignment.Justify, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0), true, false, false));
            reports.AddRowContent(rows, 0);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("", 0, 10, true, false, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0), false, false, true));
            rows.Add(new ContentCell("Date", 1, 10, false, false, ParagraphAlignment.Center, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0), false, false, false));
            rows.Add(new ContentCell("", 2, 10, true, false, ParagraphAlignment.Justify, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0), false, false, false));
            rows.Add(new ContentCell("Designation", 3, 10, false, false, ParagraphAlignment.Center, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0), false, false, false));
            rows.Add(new ContentCell("", 4, 10, true, false, ParagraphAlignment.Justify, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0), true, false, false));
            reports.AddRowContent(rows, 0);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("\n\n", 0, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Center, 0, 0, true, new Color(0, 0, 0), false, false, true));
            rows.Add(new ContentCell("\n\n", 1, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Center, 0, 0, true, new Color(0, 0, 0), false, false, false));
            rows.Add(new ContentCell("\n\n", 2, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Center, 0, 0, true, new Color(0, 0, 0), false, false, false));
            rows.Add(new ContentCell("\n\n", 3, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Center, 0, 0, true, new Color(0, 0, 0), false, false, false));
            rows.Add(new ContentCell("\n\n", 4, 10, true, false, ParagraphAlignment.Justify, VerticalAlignment.Center, 0, 0, true, new Color(0, 0, 0), true, false, false));
            reports.AddRowContent(rows, 0);

            columns = new List<ContentColumn>();
            columns.Add(new ContentColumn(4.00));
            columns.Add(new ContentColumn(1.00));
            columns.Add(new ContentColumn(2.00));
            columns.Add(new ContentColumn(1.00));
            reports.AddTable(columns, false);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("\n", 0, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Bottom, 0, 0, false, new Color(0, 0, 0), true, false, true));
            rows.Add(new ContentCell("\n", 1, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Bottom, 0, 0, false, new Color(0, 0, 0), false, false, false));
            rows.Add(new ContentCell("\n", 2, 10, false, false, ParagraphAlignment.Center, VerticalAlignment.Bottom, 0, 0, false, new Color(0, 0, 0), false, false, false));
            rows.Add(new ContentCell("\n", 3, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Bottom, 0, 0, false, new Color(0, 0, 0), true, false, false));
            reports.AddRowContent(rows, 0);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("Funds Available:", 0, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Bottom, 0, 0, false, new Color(0, 0, 0), true, false, true));
            rows.Add(new ContentCell("Amount:", 1, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Bottom, 0, 0, false, new Color(0, 0, 0), false, false, false));
            rows.Add(new ContentCell(procurementProgram.ProjectCost.ToString("C", new System.Globalization.CultureInfo("en-ph")), 2, 10, false, false, ParagraphAlignment.Center, VerticalAlignment.Bottom, 0, 0, true, new Color(0, 0, 0), false, false, false));
            rows.Add(new ContentCell("", 3, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Bottom, 0, 0, false, new Color(0, 0, 0), true, false, false));
            reports.AddRowContent(rows, 0);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("\n", 0, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Bottom, 0, 0, false, new Color(0, 0, 0), true, false, true));
            rows.Add(new ContentCell("\n", 1, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Bottom, 0, 0, false, new Color(0, 0, 0), false, false, false));
            rows.Add(new ContentCell("\n", 2, 10, false, false, ParagraphAlignment.Center, VerticalAlignment.Bottom, 0, 0, false, new Color(0, 0, 0), false, false, false));
            rows.Add(new ContentCell("\n", 3, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Bottom, 0, 0, false, new Color(0, 0, 0), true, false, false));
            reports.AddRowContent(rows, 0);

            columns = new List<ContentColumn>();
            columns.Add(new ContentColumn(0.50));
            columns.Add(new ContentColumn(3.00));
            columns.Add(new ContentColumn(0.50));
            columns.Add(new ContentColumn(1.00));
            columns.Add(new ContentColumn(2.00));
            columns.Add(new ContentColumn(1.00));
            reports.AddTable(columns, false);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("", 0, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Bottom, 0, 0, false, new Color(0, 0, 0), false, false, true));
            rows.Add(new ContentCell(purchaseOrderHeader.ChiefAccountant.ToUpper(), 1, 10, true, false, ParagraphAlignment.Center, VerticalAlignment.Bottom, 0, 0, true, new Color(0, 0, 0), false, false, false));
            rows.Add(new ContentCell("", 2, 10, false, false, ParagraphAlignment.Center, VerticalAlignment.Bottom, 0, 0, false, new Color(0, 0, 0), true, false, false));
            rows.Add(new ContentCell("ALOBS NO.", 3, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Bottom, 0, 0, false, new Color(0, 0, 0), false, false, true));
            rows.Add(new ContentCell("", 4, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Bottom, 0, 0, true, new Color(0, 0, 0), false, false, false));
            rows.Add(new ContentCell("", 5, 10, false, false, ParagraphAlignment.Center, VerticalAlignment.Bottom, 0, 0, false, new Color(0, 0, 0), true, false, false));
            reports.AddRowContent(rows, 0);

            HRISDataAccess hris = new HRISDataAccess();
            rows = new List<ContentCell>();
            rows.Add(new ContentCell("", 0, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Bottom, 0, 0, false, new Color(0, 0, 0), false, false, true));
            rows.Add(new ContentCell(purchaseOrderHeader.ChiefAccountantDesignation + ", " + hris.GetDepartmentDetails(purchaseOrderHeader.ChiefAccountantOffice).Department, 1, 10, false, false, ParagraphAlignment.Center, VerticalAlignment.Bottom, 0, 0, false, new Color(0, 0, 0), false, false, false));
            rows.Add(new ContentCell("", 2, 10, false, false, ParagraphAlignment.Center, VerticalAlignment.Bottom, 0, 0, false, new Color(0, 0, 0), true, false, false));
            rows.Add(new ContentCell("", 3, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Bottom, 0, 0, false, new Color(0, 0, 0), false, true, true));
            rows.Add(new ContentCell("", 4, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Bottom, 0, 0, false, new Color(0, 0, 0), false, true, false));
            rows.Add(new ContentCell("", 5, 10, false, false, ParagraphAlignment.Center, VerticalAlignment.Bottom, 0, 0, false, new Color(0, 0, 0), true, true, false));
            reports.AddRowContent(rows, 0);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("", 0, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Bottom, 0, 0, false, new Color(0, 0, 0), false, false, true));
            rows.Add(new ContentCell("Chief Accountant", 1, 10, false, false, ParagraphAlignment.Center, VerticalAlignment.Bottom, 0, 0, false, new Color(0, 0, 0), false, false, false));
            rows.Add(new ContentCell("", 2, 10, false, false, ParagraphAlignment.Center, VerticalAlignment.Bottom, 0, 0, false, new Color(0, 0, 0), true, false, false));
            rows.Add(new ContentCell("", 3, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Bottom, 0, 0, false, new Color(0, 0, 0), false, true, true));
            rows.Add(new ContentCell("", 4, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Bottom, 0, 0, false, new Color(0, 0, 0), false, true, false));
            rows.Add(new ContentCell("", 5, 10, false, false, ParagraphAlignment.Center, VerticalAlignment.Bottom, 0, 0, false, new Color(0, 0, 0), true, true, false));
            reports.AddRowContent(rows, 0);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("", 0, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Bottom, 0, 0, true, new Color(0, 0, 0), false, false, true));
            rows.Add(new ContentCell("", 1, 10, false, false, ParagraphAlignment.Center, VerticalAlignment.Bottom, 0, 0, true, new Color(0, 0, 0), false, false, false));
            rows.Add(new ContentCell("", 2, 10, false, false, ParagraphAlignment.Center, VerticalAlignment.Bottom, 0, 0, true, new Color(0, 0, 0), true, false, false));
            rows.Add(new ContentCell("", 3, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Bottom, 0, 0, true, new Color(0, 0, 0), false, true, true));
            rows.Add(new ContentCell("", 4, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Bottom, 0, 0, true, new Color(0, 0, 0), false, true, false));
            rows.Add(new ContentCell("", 5, 10, false, false, ParagraphAlignment.Center, VerticalAlignment.Bottom, 0, 0, true, new Color(0, 0, 0), true, true, false));
            reports.AddRowContent(rows, 0);

            return reports.GenerateReport();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                myProjectsDAL.Dispose();
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }

    public class MyProjectsDAL : Controller
    {
        private FMISDbContext db = new FMISDbContext();
        private HRISDataAccess hris = new HRISDataAccess();
        private ABISDataAccess abis = new ABISDataAccess();
        private SystemBDL system = new SystemBDL();

        public List<ModeOfProcurement> GetModesOfPrpcurement(string ProcurementModes)
        {
            var modesOfProcurement = new List<ModeOfProcurement>();
            var modes = ProcurementModes.Split("\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            foreach (var item in modes)
            {
                var mode = db.ProcurementModes.Where(d => d.ModeOfProcurementName == item).FirstOrDefault();
                modesOfProcurement.Add(mode);
            }
            return modesOfProcurement;
        }
        public int GetTotalProjectsAssigned(string Email)
        {
            var empCode = db.UserAccounts.Where(d => d.Email == Email).FirstOrDefault().EmpCode;
            return db.APPDetails.Where(d => d.ProjectCoordinator == empCode || d.ProjectSupport == empCode).Count();
        }
        public int GetTotalProjectsCoordinated(string Email)
        {
            var empCode = db.UserAccounts.Where(d => d.Email == Email).FirstOrDefault().EmpCode;
            return db.APPDetails.Where(d => d.ProjectCoordinator == empCode).Count();
        }
        public int GetTotalProjectsupported(string Email)
        {
            var empCode = db.UserAccounts.Where(d => d.Email == Email).FirstOrDefault().EmpCode;
            return db.APPDetails.Where(d => d.ProjectSupport == empCode).Count();
        }
        public List<Supplier> GetSuppliers()
        {
            return db.Suppliers.Where(d => d.PurgeFlag == false).ToList();
        }
        public bool OpenPRSubmission(string PAPCode, int ModeOfProcurement)
        {
            var appDetails = db.APPDetails.Where(d => d.PAPCode == PAPCode).First();
            var procurementTimeline = db.ProcurementTimeline.Where(d => d.PAPCode == PAPCode).FirstOrDefault();
            var procurementProgram = db.APPDetails.Where(d => d.PAPCode == PAPCode).FirstOrDefault();
            if(procurementTimeline == null)
            {
                procurementTimeline = new ProcurementTimeline
                {
                    PAPCode = PAPCode,
                    PurchaseRequestSubmission = DateTime.Now,
                };
                db.ProcurementTimeline.Add(procurementTimeline);
                //procurementProgram.ModeOfProcurementReference = ModeOfProcurement;
                if (db.SaveChanges() == 0)
                {
                    return false;
                }
            }

            var items = db.ProjectPlanItems.Where(d => d.APPLineReference == appDetails.ID).ToList();
            if(items.Count > 0)
            {
                items.ForEach(d => { d.Status = "P/R Submission Open"; });
                if (db.SaveChanges() == 0)
                {
                    return false;
                }
            }

            var services = db.ProjectPlanServices.Where(d => d.APPLineReference == appDetails.ID).ToList();
            if(services.Count > 0)
            {
                services.ForEach(d => { d.Status = "P/R Submission Open"; });
                if(db.SaveChanges() == 0)
                {
                    return false;
                }
            }

            procurementProgram.ProjectStatus = "P/R Submission Open";
            if(db.SaveChanges() == 0)
            {
                return false;
            }

            return true;
        }
        public bool ClosePRSubmission(string PAPCode)
        {
            var procurementProgram = db.APPDetails.Where(d => d.PAPCode == PAPCode).FirstOrDefault();
            var procurementTimeline = db.ProcurementTimeline.Where(d => d.PAPCode == PAPCode).FirstOrDefault();
            procurementProgram.ProjectStatus = "PR Submission Closed";
            procurementTimeline.PurchaseRequestClosing = DateTime.Now;
            if (db.SaveChanges() == 0)
            {
                return false;
            }
            return true;
        }
        public List<ProcurementProjectsVM> GetProcurementProgramDetails(string UserEmail)
        {
            var user = db.UserAccounts.Where(d => d.Email == UserEmail).FirstOrDefault();
            var procurementProjectList = new List<ProcurementProjectsVM>();
            var procurementPrograms = db.APPDetails.Where(d => (d.ProjectCoordinator == user.EmpCode || d.ProjectSupport == user.EmpCode)).ToList();

            foreach (var item in procurementPrograms)
            {
                var procurementModes = item.APPModeOfProcurementReference.Split("_".ToCharArray(), System.StringSplitOptions.RemoveEmptyEntries).ToArray();
                string procurementModeList = string.Empty;
                for (int i = 0; i < procurementModes.Count(); i++)
                {
                    if (i == procurementModes.Count() - 1)
                    {
                        procurementModeList += procurementModes[i];
                    }
                    else
                    {
                        procurementModeList += procurementModes[i] + "\n";
                    }
                }
                procurementProjectList.Add(new ProcurementProjectsVM
                {
                    Month = item.Month,
                    PAPCode = item.PAPCode,
                    UACS = item.ObjectClassification,
                    ProcurementProgram = item.ProcurementProgram,
                    ApprovedBudget = item.Total,
                    ObjectClassification = abis.GetChartOfAccounts(item.ObjectClassification).AcctName,
                    FundCluster = item.FundSourceReference,
                    FundSource = abis.GetFundSources(item.FundSourceReference).FUND_DESC,
                    StartMonth = item.StartMonth,
                    EndMonth = item.EndMonth,
                    APPModeOfProcurement = procurementModeList,
                    ProjectStatus = item.ProjectStatus,
                    ProjectCoordinator = hris.GetEmployeeByCode(item.ProjectCoordinator).EmployeeName,
                    ProjectSupport = hris.GetEmployeeByCode(item.ProjectSupport).EmployeeName
                });
            }

            return procurementProjectList;
        }
        public bool UpdatePreProcurement(ProcurementProjectsVM ProcurementPrograms, string UserEmail)
        {
            var timeline = db.ProcurementTimeline.Where(d => d.PAPCode == ProcurementPrograms.PAPCode).FirstOrDefault();
            if(timeline.PreProcurementConference == null && ProcurementPrograms.PreProcurementConference != null)
            {
                timeline.PreProcurementConference = ProcurementPrograms.PreProcurementConference;
                if(db.SaveChanges() == 0)
                {
                    return false;
                }
            }
            if(timeline.ActualPreProcurementConference == null && ProcurementPrograms.ActualPreProcurementConference != null)
            {
                var procurementProgram = db.APPDetails.Where(d => d.PAPCode == ProcurementPrograms.PAPCode).FirstOrDefault();
                procurementProgram.ProjectStatus = "Pre-Procurement Conference Conducted";
                timeline.ActualPreProcurementConference = ProcurementPrograms.ActualPreProcurementConference;
                timeline.PreProcurementConferenceRemarks = ProcurementPrograms.PreProcurementConferenceRemarks;
                if (db.SaveChanges() == 0)
                {
                    return false;
                }
            }
            return true;
        }
        public bool UpdatePostingOfIB(ProcurementProjectsVM ProcurementPrograms, string UserEmail)
        {
            var timeline = db.ProcurementTimeline.Where(d => d.PAPCode == ProcurementPrograms.PAPCode).FirstOrDefault();
            if (timeline.PostingOfIB == null && ProcurementPrograms.PostingOfIB != null)
            {
                timeline.PostingOfIB = ProcurementPrograms.PostingOfIB;
                if (db.SaveChanges() == 0)
                {
                    return false;
                }
            }
            if (timeline.ActualPostingOfIB == null && ProcurementPrograms.ActualPostingOfIB != null)
            {
                var procurementProgram = db.APPDetails.Where(d => d.PAPCode == ProcurementPrograms.PAPCode).FirstOrDefault();
                procurementProgram.ProjectStatus = "PhilGEPS Posting Done";
                timeline.ActualPostingOfIB = ProcurementPrograms.ActualPostingOfIB;
                timeline.PostingOfIBRemarks = ProcurementPrograms.PostingOfIBRemarks;
                if (db.SaveChanges() == 0)
                {
                    return false;
                }
            }
            return true;
        }
        public bool UpdatePreBid(ProcurementProjectsVM ProcurementPrograms, string UserEmail)
        {
            var timeline = db.ProcurementTimeline.Where(d => d.PAPCode == ProcurementPrograms.PAPCode).FirstOrDefault();
            if (timeline.PreBidConference == null && ProcurementPrograms.PreBidConference != null)
            {
                timeline.PreBidConference = ProcurementPrograms.PreBidConference;
                if (db.SaveChanges() == 0)
                {
                    return false;
                }
            }
            if (timeline.ActualPreBidConference == null && ProcurementPrograms.ActualPreBidConference != null)
            {
                var procurementProgram = db.APPDetails.Where(d => d.PAPCode == ProcurementPrograms.PAPCode).FirstOrDefault();
                procurementProgram.ProjectStatus = "Pre-Bid Conference Conducted";
                timeline.ActualPreBidConference = ProcurementPrograms.ActualPreBidConference;
                timeline.PreBidConferenceRemarks = ProcurementPrograms.PreBidConferenceRemarks;
                if (db.SaveChanges() == 0)
                {
                    return false;
                }
            }
            return true;
        }
        public bool UpdateBidsEvaluation(ProcurementProjectsVM ProcurementPrograms, string UserEmail)
        {
            var timeline = db.ProcurementTimeline.Where(d => d.PAPCode == ProcurementPrograms.PAPCode).FirstOrDefault();
            if (timeline.OpeningOfBids == null && ProcurementPrograms.OpeningOfBids != null)
            {
                timeline.OpeningOfBids = ProcurementPrograms.OpeningOfBids;
                if (db.SaveChanges() == 0)
                {
                    return false;
                }
            }
            if ((timeline.PrelimenryExamination == null && ProcurementPrograms.PrelimenryExamination != null) && (timeline.DetailedExamination == null && ProcurementPrograms.DetailedExamination != null && (timeline.EvaluationReporting == null && ProcurementPrograms.EvaluationReporting != null)))
            {
                var procurementProgram = db.APPDetails.Where(d => d.PAPCode == ProcurementPrograms.PAPCode).FirstOrDefault();
                procurementProgram.ProjectStatus = "Bids Evaluated";
                timeline.PrelimenryExamination = ProcurementPrograms.PrelimenryExamination;
                timeline.DetailedExamination = ProcurementPrograms.DetailedExamination;
                timeline.EvaluationReporting = ProcurementPrograms.EvaluationReporting;
                timeline.BidsExaminationRemarks = ProcurementPrograms.BidsExaminationRemarks;
                if (db.SaveChanges() == 0)
                {
                    return false;
                }
            }
            return true;
        }
        public bool UpdatePostQualification(ProcurementProjectsVM ProcurementPrograms, string UserEmail)
        {
            var timeline = db.ProcurementTimeline.Where(d => d.PAPCode == ProcurementPrograms.PAPCode).FirstOrDefault();
            if (timeline.PostQualification == null && ProcurementPrograms.PostQualification != null)
            {
                timeline.PostQualification = ProcurementPrograms.PostQualification;
                if (db.SaveChanges() == 0)
                {
                    return false;
                }
            }
            if ((timeline.ActualPostQualification == null && ProcurementPrograms.ActualPostQualification != null) && (timeline.PostQualificationReportedToBAC == null && ProcurementPrograms.PostQualificationReportedToBAC != null))
            {
                var procurementProgram = db.APPDetails.Where(d => d.PAPCode == ProcurementPrograms.PAPCode).FirstOrDefault();
                procurementProgram.ProjectStatus = "Post Qualification";
                timeline.ActualPostQualification = ProcurementPrograms.ActualPostQualification;
                timeline.PostQualificationReportedToBAC = ProcurementPrograms.PostQualificationReportedToBAC;
                timeline.PostQualificationRemarks = ProcurementPrograms.PostQualificationRemarks;
                if (db.SaveChanges() == 0)
                {
                    return false;
                }
            }
            return true;
        }
        public bool UpdateBACResoNoticeOfAward(ProcurementProjectsVM ProcurementPrograms, string UserEmail)
        {
            var timeline = db.ProcurementTimeline.Where(d => d.PAPCode == ProcurementPrograms.PAPCode).FirstOrDefault();
            var procurementPrograms = db.APPDetails.Where(d => d.PAPCode == ProcurementPrograms.PAPCode).FirstOrDefault();
            if (timeline.PMOReceived == null && ProcurementPrograms.PMOReceived != null)
            {
                timeline.BACResolutionCreated = ProcurementPrograms.BACResolutionCreated;
                timeline.BACMemberForwarded = ProcurementPrograms.BACMemberForwarded;
                timeline.HOPEForwarded = ProcurementPrograms.HOPEForwarded;
                timeline.PMOReceived = ProcurementPrograms.PMOReceived;
                if (db.SaveChanges() == 0)
                {
                    return false;
                }
            }
            return true;
        }
        public bool UpdateNoticeOfAwardIssuance(ProcurementProjectsVM ProcurementPrograms, string UserEmail)
        {
            var timeline = db.ProcurementTimeline.Where(d => d.PAPCode == ProcurementPrograms.PAPCode).FirstOrDefault();
            var procurementPrograms = db.APPDetails.Where(d => d.PAPCode == ProcurementPrograms.PAPCode).FirstOrDefault();

            if (timeline.ActualNOAIssuance == null && ProcurementPrograms.ActualNOAIssuance != null)
            {
                procurementPrograms.ProjectStatus = "Notice of Award Created";
                timeline.ActualNOAIssuance = ProcurementPrograms.ActualNOAIssuance;
                db.SaveChanges();
            }
            if (timeline.NOASignedByHOPE == null && ProcurementPrograms.NOASignedByHOPE != null)
            {
                procurementPrograms.ProjectStatus = "Notice of Award signed by HOPE";
                timeline.NOASignedByHOPE = ProcurementPrograms.NOASignedByHOPE;
                timeline.NOAIssuanceRemarks = ProcurementPrograms.NOAIssuanceRemarks;
                db.SaveChanges();
            }
            if (timeline.NOAReceivedBySupplier == null && ProcurementPrograms.NOAReceivedBySupplier != null)
            {
                procurementPrograms.ProjectStatus = "Notice of Award Received by Supplier";
                timeline.NOAReceivedBySupplier = ProcurementPrograms.NOAReceivedBySupplier;
                timeline.NOAIssuanceRemarks = ProcurementPrograms.NOAIssuanceRemarks;
                db.SaveChanges();
            }
            return true;
        }
        public bool UpdateContractDetails(ProcurementProjectsVM ProcurementPrograms, string UserEmail)
        {
            var appDetails = db.APPDetails.Where(d => d.PAPCode == ProcurementPrograms.PAPCode).First();
            var agencyDetails = db.AgencyDetails.FirstOrDefault();
            var chiefAccountant = hris.GetFullDepartmentDetails(agencyDetails.AccountingOfficeReference);
            var authorizedSignature = hris.GetFullDepartmentDetails(agencyDetails.HOPEReference);
            var procurementProgram = db.APPDetails.Where(d => d.PAPCode == ProcurementPrograms.PAPCode).FirstOrDefault();

            var projectCost = 0.00m;
            foreach(var item in ProcurementPrograms.Items)
            {
                projectCost += item.UnitCost * item.TotalQty;
            }

            if(ProcurementPrograms.IsTangible)
            {
                var projectItems = db.ProjectPlanItems.Where(d => d.APPLineReference == appDetails.ID).ToList();

                var purchaseOrderHeader = new PurchaseOrderHeader
                {
                    PurchaseOrderNumber = GeneratePurchaseOrderNumber(),
                    SupplierReference  = (int)ProcurementPrograms.Supplier,
                    PlaceOfDelivery = ProcurementPrograms.PlaceOfDelivery,
                    DateOfDelivery = ProcurementPrograms.DateOfDelivery,
                    CreatedAt = (DateTime)ProcurementPrograms.POCreatedAt,
                    TotalAmount = projectCost, 
                    ChiefAccountant = chiefAccountant.DepartmentHead,
                    ChiefAccountantOffice = chiefAccountant.DepartmentCode,
                    ChiefAccountantDesignation = chiefAccountant.DepartmentHeadDesignation,
                    AuthorizedSignature = authorizedSignature.DepartmentHead,
                    AuthorizedSignatureOffice = authorizedSignature.DepartmentCode,
                    AuthorizedSignatureDesignation = authorizedSignature.DepartmentHeadDesignation
                };
                db.PurchaseOrderHeader.Add(purchaseOrderHeader);
                if(db.SaveChanges() == 0)
                {
                    return false;
                }

                var purchaseOrderDetails = new List<PurchaseOrderDetails>();
                foreach(var item in ProcurementPrograms.Items)
                {
                    var itemReference = db.Items.Where(d => d.ItemCode == item.ItemCode).FirstOrDefault();
                    purchaseOrderDetails.Add(new PurchaseOrderDetails
                    {
                        PurchaseOrderReference = purchaseOrderHeader.ID,
                        ItemReference = itemReference.ID,
                        Quantity = item.TotalQty,
                        UnitCost = item.UnitCost
                    });
                }

                db.PurchaseOrderDetails.AddRange(purchaseOrderDetails);
                if (db.SaveChanges() == 0)
                {
                    return false;
                }

                procurementProgram.ProjectCost = projectCost;
                projectItems.ForEach(d => { d.POReference = purchaseOrderHeader.ID; });
                if (db.SaveChanges() == 0)
                {
                    return false;
                }
            }

            return true;
        }
        private string GeneratePurchaseOrderNumber()
        {
            var purchaseOrderNumber = string.Empty;
            var series = (db.PurchaseOrderHeader.Count() + 1).ToString();
            series = series.Length == 1 ? "000" + series : series.Length == 2 ? "00" + series : series.Length == 3 ? "0" + series : series;
            purchaseOrderNumber = DateTime.Now.Year.ToString().Substring(2, 2) + "-" + DateTime.Now.ToString("MM") + "-" + series;
            return purchaseOrderNumber;
        }
        public List<ProcurementProjectItemsVM> GetProjectItems(string PAPCode)
        {
            var appDetails = db.APPDetails.Where(d => d.PAPCode == PAPCode).First();
            var procurementItems = new List<ProcurementProjectItemsVM>();
            var procurementProgram = db.APPDetails.Where(d => d.PAPCode == PAPCode).FirstOrDefault();
            var items = db.ProjectPlanItems.Where(d => d.APPLineReference == appDetails.ID).ToList();
            var services = db.ProjectPlanServices.Where(d => d.APPLineReference == appDetails.ID).ToList();

            foreach (var item in items)
            {
                var purchaseRequestHeader = db.PurchaseRequestHeader.Find(item.PRReference);
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
                    PRNumber = item.PRReference == null ? null : purchaseRequestHeader.PRNumber,
                    DatePRReceived = item.PRReference == null ? null : purchaseRequestHeader.ReceivedAt,
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

            return procurementItems;
        }
        public List<ProcurementProjectItemsVM> GetPurchaseOrderItems(int PurchaseOrderID)
        {
            var procurementItems = new List<ProcurementProjectItemsVM>();
            var purchaseOrderDetails = db.PurchaseOrderDetails.Where(d => d.PurchaseOrderReference == PurchaseOrderID).ToList();
            foreach (var item in purchaseOrderDetails)
            {
                procurementItems.Add(new ProcurementProjectItemsVM
                {
                    ItemCode = item.FKItemReference.ItemCode,
                    ItemName = item.FKItemReference.ItemFullName,
                    TotalQty = item.Quantity,
                    UnitCost = item.UnitCost
                });
            }
            return procurementItems;
        }


        public ProcurementProjectsVM GetProcurementProgramDetailsByPAPCode(string PAPCode)
        {
            var procurementProgram = db.APPDetails.Where(d => d.PAPCode == PAPCode).FirstOrDefault();
            var procurementTimeline = db.ProcurementTimeline.Where(d => d.PAPCode == PAPCode).FirstOrDefault();

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
                IsTangible = procurementProgram.IsTangible,
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
                PurchaseRequestSubmission = procurementTimeline == null ? null : procurementTimeline.PurchaseRequestSubmission == null ? null : procurementTimeline.PurchaseRequestSubmission,
                PurchaseRequestClosing = procurementTimeline == null ? null : procurementTimeline.PurchaseRequestClosing == null ? null : procurementTimeline.PurchaseRequestClosing,
                ActualPreProcurementConference = procurementTimeline == null ? null : procurementTimeline.ActualPreProcurementConference,
                PreProcurementConferenceRemarks = procurementTimeline == null ? null : procurementTimeline.PreProcurementConferenceRemarks,
                ActualPostingOfIB = procurementTimeline == null ? null : procurementTimeline.ActualPostingOfIB,
                PostingOfIBRemarks = procurementTimeline == null ? null : procurementTimeline.PostingOfIBRemarks,
                ActualPreBidConference = procurementTimeline == null ? null : procurementTimeline.ActualPreBidConference,
                PreBidConferenceRemarks = procurementTimeline == null ? null : procurementTimeline.PreBidConferenceRemarks,
                PrelimenryExamination = procurementTimeline == null ? null : procurementTimeline.PrelimenryExamination,
                DetailedExamination = procurementTimeline == null ? null : procurementTimeline.DetailedExamination,
                EvaluationReporting = procurementTimeline == null ? null : procurementTimeline.EvaluationReporting,
                BidsExaminationRemarks = procurementTimeline == null ? null : procurementTimeline.BidsExaminationRemarks,
                ActualPostQualification = procurementTimeline == null ? null : procurementTimeline.ActualPostQualification,
                PostQualificationReportedToBAC = procurementTimeline == null ? null : procurementTimeline.PostQualificationReportedToBAC,
                PostQualificationRemarks = procurementTimeline == null ? null : procurementTimeline.PostQualificationRemarks,
                BACResolutionCreated = procurementTimeline == null ? null : procurementTimeline.BACResolutionCreated,
                BACMemberForwarded = procurementTimeline == null ? null : procurementTimeline.BACMemberForwarded,
                HOPEForwarded = procurementTimeline == null ? null : procurementTimeline.HOPEForwarded,
                PMOReceived = procurementTimeline == null ? null : procurementTimeline.PMOReceived,
                ActualNOAIssuance = procurementTimeline == null ? null : procurementTimeline.ActualNOAIssuance,
                NOASignedByHOPE = procurementTimeline == null ? null : procurementTimeline.NOASignedByHOPE,
                NOAReceivedBySupplier = procurementTimeline == null ? null : procurementTimeline.NOAReceivedBySupplier,
                NOAIssuanceRemarks = procurementTimeline == null ? null : procurementTimeline.NOAIssuanceRemarks,
                ProjectCost = procurementProgram.ProjectCost,
            };

            return procurementProject;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
                abis.Dispose();
                hris.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}