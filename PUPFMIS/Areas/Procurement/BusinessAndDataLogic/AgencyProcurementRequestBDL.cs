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
    
    public class AgencyProcurementRequestBL : Controller
    {
        private AgencyProcurementRequestDAL aprDAL = new AgencyProcurementRequestDAL();
        private FMISDbContext db = new FMISDbContext();

        public MemoryStream GenerateAgencyProcurementRequest(string AgencyControlNo, string LogoPath, string UserEmail)
        {
            Reports reports = new Reports();
            var aprVM = aprDAL.GetAPR(AgencyControlNo);
            reports.ReportFilename = "APR " + AgencyControlNo;
            reports.CreateDocument(8.50, 13.00, Orientation.Portrait, 0.25);

            var columns = new List<ContentColumn>();
            columns.Add(new ContentColumn(4.00));
            columns.Add(new ContentColumn(4.00));
            reports.AddTable(columns, false);

            var rows = new List<ContentCell>();
            rows.Add(new ContentCell("APR FORM revised MAY 2015", 0, 8, false, false, ParagraphAlignment.Left, VerticalAlignment.Center));
            rows.Add(new ContentCell("FORM NO. 062\t\t", 1, 8, false, false, ParagraphAlignment.Right, VerticalAlignment.Center));
            reports.AddRowContent(rows, 0.25);
            
            columns = new List<ContentColumn>();
            columns.Add(new ContentColumn(2.00));
            columns.Add(new ContentColumn(3.00));
            columns.Add(new ContentColumn(2.00));
            columns.Add(new ContentColumn(1.00));
            reports.AddTable(columns, false);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("NAME AND ADDRESS OF", 0, 8, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0,0,0), false, true, true));
            rows.Add(new ContentCell("POLYTECHNIC UNIVERSITY OF THE PHILIPPINES", 1, 8, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0,0,0), true, true, true));
            rows.Add(new ContentCell("AGENCY ACCT. CODE", 2, 8, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0), false, true, true));
            rows.Add(new ContentCell("Z006", 3, 8, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0), true, true, true));
            reports.AddRowContent(rows, 0.15);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("REQUESTING AGENCY", 0, 8, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0), false, true, true));
            rows.Add(new ContentCell("Anonas St., Sta. Mesa, Manila", 1, 8, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0), true, true, true));
            rows.Add(new ContentCell("AGENCT CONTROL NO.", 2, 8, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0), false, true, true));
            rows.Add(new ContentCell(aprVM.AgencyControlNo, 3, 8, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0), true, true, true));
            reports.AddRowContent(rows, 0.15);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("TEL NOS.", 0, 8, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, true, new Color(0, 0, 0), false, true, true));
            rows.Add(new ContentCell("713-15-04", 1, 8, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, true, new Color(0, 0, 0), true, true, true));
            rows.Add(new ContentCell("", 2, 8, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, true, new Color(0, 0, 0), false, true, true));
            rows.Add(new ContentCell("", 3, 8, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, true, new Color(0, 0, 0), true, true, true));
            reports.AddRowContent(rows, 0.15);

            columns = new List<ContentColumn>();
            columns.Add(new ContentColumn(5.00));
            columns.Add(new ContentColumn(3.00));
            reports.AddTable(columns, true);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("AGENCY PROCUREMENT REQUEST", 0, 9, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
            rows.Add(new ContentCell("PS APR No.", 1, 8, false, false, ParagraphAlignment.Left, VerticalAlignment.Center));
            reports.AddRowContent(rows, 0.25);

            columns = new List<ContentColumn>();
            columns.Add(new ContentColumn(5.00));
            columns.Add(new ContentColumn(0.50));
            columns.Add(new ContentColumn(2.00));
            columns.Add(new ContentColumn(0.50));
            reports.AddTable(columns, false);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("\nTo\tPROCUREMENT SERVICE", 0, 8, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0), false, true, true));
            rows.Add(new ContentCell("\n", 1, 8, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0), false, true, false));
            rows.Add(new ContentCell("\n" + aprVM.CreatedAt.ToString("dd-MMM-yyyy"), 2, 8, false, false, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 0, true, new Color(0, 0, 0), false, true, false));
            rows.Add(new ContentCell("\n", 3, 8, false, false, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0), true, true, true));
            reports.AddRowContent(rows, 0.05);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("  \tDBM Compount, RR Road", 0, 8, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0), false, true, true));
            rows.Add(new ContentCell("", 1, 8, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0), false, true, false));
            rows.Add(new ContentCell("(Date Prepared)", 2, 8, false, false, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0), false, true, false));
            rows.Add(new ContentCell("", 3, 8, false, false, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0), true, true, true));
            reports.AddRowContent(rows, 0.05);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("  \tCristobal St., Paco, Manila", 0, 8, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0), false, true, true));
            rows.Add(new ContentCell("", 1, 8, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0), false, true, false));
            rows.Add(new ContentCell("", 2, 8, false, false, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0), false, true, false));
            rows.Add(new ContentCell("", 3, 8, false, false, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0), true, true, true));
            reports.AddRowContent(rows, 0.05);

            columns = new List<ContentColumn>();
            columns.Add(new ContentColumn(8.00));
            reports.AddTable(columns, false);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("\n", 0, 8, true, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0), true, false, true));
            reports.AddRowContent(rows, 0.15);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("  \tPLEASE CHECK ( / ) APPROPRIATE BOX ON ACTION REQUESTED ON  THE ITEM/S LISTED BELOW", 0, 8, true, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0), true, false, true));
            reports.AddRowContent(rows, 0.15);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("\n", 0, 8, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0), true, false, true));
            reports.AddRowContent(rows, 0.15);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("[ ] Please issue common-use supplies/materials per Price List No. __________________ dated __________________ ", 0, 8, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0), true, false, true));
            reports.AddRowContent(rows, 0.15);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("\tMode of Delivery: [ ] Reduce Quantity\t[ ] Pick-up (Schedule)\t[ ] Delivery (door-to-door)", 0, 8, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0), true, false, true));
            reports.AddRowContent(rows, 0.15);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("\tIn case fund is not sufficient: [ ] Reduce Quantity\t[ ] Bill Us\t[ ] Charge to Unutilized Deposit, APR No.: __________ Date: _________", 0, 8, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0), true, false, true));
            reports.AddRowContent(rows, 0.15);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("[ ] Please purchase for our agency non-common items. Attached herewith:", 0, 8, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0), true, false, true));
            reports.AddRowContent(rows, 0.15);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("\t[ ] Complete Specifications\t[ ] Obligation Request (ObR)\t[ ] Others, pls. specify ______________________", 0, 8, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0), true, false, true));
            reports.AddRowContent(rows, 0.15);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("[ ] Certificate of Budget Allocation (CBA)\t[ ] Payment\t\t\t\t__________________________", 0, 8, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0), true, false, true));
            reports.AddRowContent(rows, 0.15);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("\n", 0, 8, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0), true, false, true));
            reports.AddRowContent(rows, 0.15);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("This form shall be prepared for requisition of Common-Use goods from PS Depots & Sub-Depots; and for orders of Consumables", 0, 8, false, false, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0), true, false, true));
            reports.AddRowContent(rows, 0.15);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("& Non-Common Use Supplies from PS Main.", 0, 8, false, false, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0), true, false, true));
            reports.AddRowContent(rows, 0.15);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("\n", 0, 8, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0), true, false, true));
            reports.AddRowContent(rows, 0.15);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("For PS Main-Common Use Supplies, please use Form 001 R or Form 001 B", 0, 8, false, true, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 0, true, new Color(0, 0, 0), true, false, true));
            reports.AddRowContent(rows, 0.15);

            columns = new List<ContentColumn>();
            columns.Add(new ContentColumn(0.50));
            columns.Add(new ContentColumn(3.25));
            columns.Add(new ContentColumn(1.00));
            columns.Add(new ContentColumn(1.00));
            columns.Add(new ContentColumn(1.00));
            columns.Add(new ContentColumn(1.25));
            reports.AddTable(columns, true);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("ITEM No.", 0, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
            rows.Add(new ContentCell("ITEM AND DESCRIPTION/SPECIFICATIONS/STOCK NO.", 1, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
            rows.Add(new ContentCell("QUANTITY", 2, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
            rows.Add(new ContentCell("UNIT", 3, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
            rows.Add(new ContentCell("UNIT PRICE", 4, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
            rows.Add(new ContentCell("AMOUNT", 5, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
            reports.AddRowContent(rows, 0.25);

            columns = new List<ContentColumn>();
            columns.Add(new ContentColumn(0.50));
            columns.Add(new ContentColumn(3.25));
            columns.Add(new ContentColumn(1.00));
            columns.Add(new ContentColumn(1.00));
            columns.Add(new ContentColumn(1.00));
            columns.Add(new ContentColumn(1.25));
            reports.AddTable(columns, true);

            int count = 1;
            var aprItems = aprVM.APRDetails
                .GroupBy(d => new { d.ItemFullName, d.UnitReference, d.UnitPrice })
                .Select(d => new {
                    ItemFullName = d.Key.ItemFullName,
                    Quantity = d.Sum(x => x.Quantity),
                    UnitReference = d.Key.UnitReference,
                    UnitPrice = d.Key.UnitPrice,
                    Amount = d.Sum(x => x.Amount)
                }).ToList();
            foreach(var item in aprItems)
            {
                rows = new List<ContentCell>();
                rows.Add(new ContentCell((count++).ToString(), 0, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                rows.Add(new ContentCell(item.ItemFullName, 1, 7, false, false, ParagraphAlignment.Left, VerticalAlignment.Center));
                rows.Add(new ContentCell(item.Quantity.ToString(), 2, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                rows.Add(new ContentCell(item.UnitReference, 3, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                rows.Add(new ContentCell(string.Format("{0:0,0.00}", item.UnitPrice), 4, 7, false, false, ParagraphAlignment.Right, VerticalAlignment.Center));
                rows.Add(new ContentCell(string.Format("{0:0,0.00}", item.Amount), 5, 7, false, false, ParagraphAlignment.Right, VerticalAlignment.Center));
                reports.AddRowContent(rows, 0.20);
            }

            for(int i = aprItems.Count(); i < 30; i++)
            {
                rows = new List<ContentCell>();
                rows.Add(new ContentCell("", 0, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                rows.Add(new ContentCell("", 1, 7, false, false, ParagraphAlignment.Left, VerticalAlignment.Center));
                rows.Add(new ContentCell("", 2, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                rows.Add(new ContentCell("", 3, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                rows.Add(new ContentCell("", 4, 7, false, false, ParagraphAlignment.Right, VerticalAlignment.Center));
                rows.Add(new ContentCell("", 5, 7, false, false, ParagraphAlignment.Right, VerticalAlignment.Center));
                reports.AddRowContent(rows, 0.20);
            }

            columns = new List<ContentColumn>();
            columns.Add(new ContentColumn(0.50));
            columns.Add(new ContentColumn(3.25));
            columns.Add(new ContentColumn(1.00));
            columns.Add(new ContentColumn(1.00));
            columns.Add(new ContentColumn(1.00));
            columns.Add(new ContentColumn(1.25));
            reports.AddTable(columns, true);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("", 0, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
            rows.Add(new ContentCell("", 1, 7, false, false, ParagraphAlignment.Left, VerticalAlignment.Center));
            rows.Add(new ContentCell("", 2, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
            rows.Add(new ContentCell("TOTAL AMOUNT", 3, 7, true, false, ParagraphAlignment.Left, VerticalAlignment.Center, 1));
            rows.Add(new ContentCell(string.Format("{0:0,0.00}", aprVM.APRDetails.Sum(d => d.Amount)), 5, 7, false, false, ParagraphAlignment.Right, VerticalAlignment.Center));
            reports.AddRowContent(rows, 0.20);

            columns = new List<ContentColumn>();
            columns.Add(new ContentColumn(8.00));
            reports.AddTable(columns, true);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("NOTE: ALL SIGNATURES MUST BE OVER PRINTED NAME", 0, 6, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
            reports.AddRowContent(rows, 0.20);

            columns = new List<ContentColumn>();
            columns.Add(new ContentColumn(3.00));
            columns.Add(new ContentColumn(3.00));
            columns.Add(new ContentColumn(2.00));
            reports.AddTable(columns, false);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("STOCKS REQUESTED ARE CERTIFIED TO BE", 0, 7, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0,0,0), true, true, true));
            rows.Add(new ContentCell("FUNDS CERTIFIED AVAILABLE:", 1, 7, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0), true, true, true));
            rows.Add(new ContentCell("APPROVED", 2, 7, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0), true, true, true));
            reports.AddRowContent(rows, 0.15);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("WITHIN APPROVED PROGRAM:", 0, 7, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0), true, true, true));
            rows.Add(new ContentCell("", 1, 7, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0), true, true, true));
            rows.Add(new ContentCell("", 2, 7, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0), true, true, true));
            reports.AddRowContent(rows, 0.15);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("\n\n", 0, 7, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0), true, true, true));
            rows.Add(new ContentCell("\n\n", 1, 7, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0), true, true, true));
            rows.Add(new ContentCell("\n\n", 2, 7, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0), true, true, true));
            reports.AddRowContent(rows, 0.15);

            columns = new List<ContentColumn>();
            columns.Add(new ContentColumn(0.25));
            columns.Add(new ContentColumn(2.50));
            columns.Add(new ContentColumn(0.25));
            columns.Add(new ContentColumn(0.25));
            columns.Add(new ContentColumn(2.50));
            columns.Add(new ContentColumn(0.25));
            columns.Add(new ContentColumn(0.25));
            columns.Add(new ContentColumn(1.50));
            columns.Add(new ContentColumn(0.25));
            reports.AddTable(columns, false);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("", 0, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0), false, false, true));
            rows.Add(new ContentCell(aprVM.ProcurementHead, 1, 7, true, false, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 0, true, new Color(0, 0, 0), false, false, false));
            rows.Add(new ContentCell("", 2, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0), true, false, false));
            rows.Add(new ContentCell("", 3, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0), false, false, true));
            rows.Add(new ContentCell(aprVM.ChiefAccountant, 4, 7, true, false, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 0, true, new Color(0, 0, 0), false, false, false));
            rows.Add(new ContentCell("", 5, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0), true, false, false));
            rows.Add(new ContentCell("", 6, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0), false, false, true));
            rows.Add(new ContentCell(aprVM.AgencyHead, 7, 7, true, false, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 0, true, new Color(0, 0, 0), false, false, false));
            rows.Add(new ContentCell("", 8, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0), true, false, false));
            reports.AddRowContent(rows, 0.15);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("", 0, 5, false, false, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 0, true, new Color(0, 0, 0), false, false, true));
            rows.Add(new ContentCell("PMO DIRECTOR", 1, 5, false, false, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 0, true, new Color(0, 0, 0), false, false, false));
            rows.Add(new ContentCell("", 2, 5, false, false, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 0, true, new Color(0, 0, 0), true, false, false));
            rows.Add(new ContentCell("", 3, 5, false, false, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 0, true, new Color(0, 0, 0), false, false, true));
            rows.Add(new ContentCell("AGENCY CHIEF ACCOUNTANT", 4, 5, false, false, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 0, true, new Color(0, 0, 0), false, false, false));
            rows.Add(new ContentCell("", 5, 5, false, false, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 0, true, new Color(0, 0, 0), true, false, false));
            rows.Add(new ContentCell("", 6, 5, false, false, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 0, true, new Color(0, 0, 0), false, false, true));
            rows.Add(new ContentCell("AGENCY HEAD/AUTHORIZED SIGNATURE", 7, 5, false, false, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 0, true, new Color(0, 0, 0), false, false, false));
            rows.Add(new ContentCell("", 8, 5, false, false, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 0, true, new Color(0, 0, 0), true, false, false));
            reports.AddRowContent(rows, 0.10);

            columns = new List<ContentColumn>();
            columns.Add(new ContentColumn(8.00));

            reports.AddTable(columns, false); rows = new List<ContentCell>();
            rows.Add(new ContentCell("\n", 0, 7, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0), true, true, true));
            reports.AddRowContent(rows, 0.15);

            reports.AddTable(columns, false); rows = new List<ContentCell>();
            rows.Add(new ContentCell("[ ] FUNDS DEPOSITED WITH PS \t[ ] _____________________ CHECK No. ___________________", 0, 7, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0), true, false, true));
            reports.AddRowContent(rows, 0.15);

            reports.AddTable(columns, false); rows = new List<ContentCell>();
            rows.Add(new ContentCell("\tIN THE AMOUNT OF: ___________________________________________________ (P __________________________________) ENCLOSED", 0, 7, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0), true, false, true));
            reports.AddRowContent(rows, 0.15);

            reports.AddTable(columns, false); rows = new List<ContentCell>();
            rows.Add(new ContentCell("\n", 0, 7, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, true, new Color(0, 0, 0), true, false, true));
            reports.AddRowContent(rows, 0.15);

            return reports.GenerateReport();
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                aprDAL.Dispose();
            }
            base.Dispose(disposing);
        }
    }

    public class AgencyProcurementRequestDAL : Controller
    {
        private FMISDbContext db = new FMISDbContext();
        private HRISDataAccess hrisDataAccess = new HRISDataAccess();
        private TEMPAccounting abdb = new TEMPAccounting();
        private ABISDataAccess abisDataAccess = new ABISDataAccess();
        private SystemBDL systemBDL = new SystemBDL();
        
        public List<AgencyProcurementRequestVM> GetAPR()
        {
            var agencyProcurementRequest = new List<AgencyProcurementRequestVM>();
            var aprHeaders = db.APRHeader.ToList();
            
            foreach(var item in aprHeaders)
            {
                var aprDetails = db.APRDetail.Where(d => d.FKAPRReference.AgencyControlNo == item.AgencyControlNo).ToList();
                var aprDetailsVM = new List<APRDetailVM>();
                foreach(var detail in aprDetails)
                {
                    aprDetailsVM.Add(new APRDetailVM
                    {
                        ItemFullName = detail.FKItemReference.ItemFullName,
                        UnitReference = detail.FKUnitOfMeasureReference.Abbreviation,
                        UnitPrice = detail.UnitPrice,
                        Quantity = detail.Quantity,
                        Amount = detail.Amount
                    });
                }
                var procurement = hrisDataAccess.GetDepartmentDetails(item.ProcurementDepartment);
                var accounting = hrisDataAccess.GetDepartmentDetails(item.ChiefAccountantDepartment);
                var agencyHead = hrisDataAccess.GetDepartmentDetails(item.AgencyHeadDepartment);
                agencyProcurementRequest.Add(new AgencyProcurementRequestVM
                {
                    AgencyControlNo = item.AgencyControlNo,
                    CreatedAt = item.CreatedAt,
                    ProcurementHead = procurement.DepartmentHead,
                    ProcurementDepartment = procurement.Department,
                    ProcurementHeadDesignation = procurement.DepartmentHeadDesignation,
                    ChiefAccountant = accounting.DepartmentHead,
                    ChiefAccountantDepartment = accounting.Department,
                    ChiefAccountantDesignation = accounting.DepartmentHeadDesignation,
                    AgencyHead = agencyHead.DepartmentHead,
                    AgencyHeadDepartment = agencyHead.Department,
                    AgencyHeadDesignation = agencyHead.DepartmentHeadDesignation,
                    APRDetails = aprDetailsVM
                });
            }

            return agencyProcurementRequest;
        }
        public AgencyProcurementRequestVM GetAPR(string AgencyControlNo)
        {
            var agencyProcurementRequest = new AgencyProcurementRequestVM();
            var aprHeader = db.APRHeader.Where(d => d.AgencyControlNo == AgencyControlNo).FirstOrDefault();

            var aprDetails = db.APRDetail.Where(d => d.FKAPRReference.AgencyControlNo == aprHeader.AgencyControlNo).ToList();
            var aprDetailsVM = new List<APRDetailVM>();
            foreach (var detail in aprDetails)
            {
                aprDetailsVM.Add(new APRDetailVM
                {
                    ItemFullName = detail.FKItemReference.ItemFullName,
                    UnitReference = detail.FKUnitOfMeasureReference.Abbreviation,
                    UnitPrice = detail.UnitPrice,
                    Quantity = detail.Quantity,
                    Amount = detail.Amount
                });
            }
            var procurement = hrisDataAccess.GetDepartmentDetails(aprHeader.ProcurementDepartment);
            var accounting = hrisDataAccess.GetDepartmentDetails(aprHeader.ChiefAccountantDepartment);
            var agencyHead = hrisDataAccess.GetDepartmentDetails(aprHeader.AgencyHeadDepartment);

            agencyProcurementRequest = new AgencyProcurementRequestVM
            {
                AgencyControlNo = aprHeader.AgencyControlNo,
                CreatedAt = aprHeader.CreatedAt,
                ProcurementHead = procurement.DepartmentHead,
                ProcurementDepartment = procurement.Department,
                ProcurementHeadDesignation = procurement.DepartmentHeadDesignation,
                ChiefAccountant = accounting.DepartmentHead,
                ChiefAccountantDepartment = accounting.Department,
                ChiefAccountantDesignation = accounting.DepartmentHeadDesignation,
                AgencyHead = agencyHead.DepartmentHead,
                AgencyHeadDepartment = agencyHead.Department,
                AgencyHeadDesignation = agencyHead.DepartmentHeadDesignation,
                APRDetails = aprDetailsVM
            };

            return agencyProcurementRequest;
        }
        public List<PurchaseRequestVM> GetPurchaseRequests()
        {
            var purchaseRequestVM = new List<PurchaseRequestVM>();
            var purchaseRequests = db.PurchaseRequestHeader.ToList();

            foreach(var pr in purchaseRequests)
            {
                var purchaseRequestDetails = db.PurchaseRequestDetails.Where(d => d.FKPRHeaderReference.PRNumber == pr.PRNumber && d.FKItemReference.ProcurementSource == ProcurementSources.PS_DBM).ToList();
                var prDetailsVM = new List<PurchaseRequestDetailsVM>();
                foreach (var prDetails in purchaseRequestDetails)
                {
                    prDetailsVM.Add(new PurchaseRequestDetailsVM
                    {
                        ItemCode = prDetails.FKItemReference.ItemCode,
                        ItemName = prDetails.FKItemReference.ItemFullName,
                        UOM = prDetails.FKUnitReference.Abbreviation,
                        Quantity = prDetails.Quantity,
                        UnitCost = prDetails.UnitCost,
                        TotalCost = prDetails.TotalCost
                    });
                }
                if(purchaseRequestDetails.Where(d => d.FKPRHeaderReference.PRNumber.Contains(pr.PRNumber)).Count() == 1)
                {
                    purchaseRequestVM.Add(new PurchaseRequestVM
                    {
                        PRNumber = pr.PRNumber,
                        Purpose = pr.Purpose,
                        Department = hrisDataAccess.GetDepartmentDetails(pr.Department).Department,
                        PRDetails = prDetailsVM,
                    });
                }
            }
            return purchaseRequestVM;
        }
        public List<PurchaseRequestDetailsVM> GetPurchaseRequestDetails()
        {
            var prDetailsVM = new List<PurchaseRequestDetailsVM>();
            var aprItems = db.APRDetail.GroupBy(d => d.PRReference).Select(d => d.Key).ToList();
            var prDetails = db.PurchaseRequestDetails
                .Where(d => d.FKPRHeaderReference.ProcurementSource == ProcurementSources.PS_DBM && !aprItems.Contains(d.FKPRHeaderReference.ID) && d.FKPRHeaderReference.ReceivedAt != null)
                .ToList();

            foreach(var detail in prDetails)
            {
                prDetailsVM.Add(new PurchaseRequestDetailsVM
                {
                    References = detail.FKPRHeaderReference.PRNumber,
                    ItemCode = detail.FKItemReference.ItemCode,
                    ItemName = detail.FKItemReference.ItemFullName,
                    UOM = detail.FKUnitReference.Abbreviation,
                    Quantity = detail.Quantity,
                    UnitCost = detail.UnitCost,
                    TotalCost = detail.TotalCost
                });
            }

            return prDetailsVM.OrderBy(d => d.ItemCode).ThenBy(d => d.References).ToList();
        }
        public bool PostToAgencyProcurementRequest(List<PurchaseRequestDetailsVM> PurchaseRequestDetails, string UserEmail)
        {
            var user = db.UserAccounts.Where(d => d.Email == UserEmail).FirstOrDefault();
            var agencyDetails = db.AgencyDetails.FirstOrDefault();
            var procurement = hrisDataAccess.GetDepartmentDetails(agencyDetails.ProcurementOfficeReference);
            var accounting = hrisDataAccess.GetDepartmentDetails(agencyDetails.AccountingOfficeReference);
            var hope = hrisDataAccess.GetDepartmentDetails(agencyDetails.HOPEReference);
            var aprHeader = new APRHeader
            {
                AgencyControlNo = GenerateAgencyRefereceNo(),
                CreatedAt = DateTime.Now,
                CreatedBy = user.EmpCode,
                ProcurementHead = procurement.DepartmentHead,
                ProcurementDepartment = procurement.DepartmentCode,
                ProcurementHeadDesignation = procurement.DepartmentHeadDesignation,
                ChiefAccountant = accounting.DepartmentHead,
                ChiefAccountantDepartment = accounting.DepartmentCode,
                ChiefAccountantDesignation = accounting.DepartmentHeadDesignation,
                AgencyHead = hope.DepartmentHead,
                AgencyHeadDepartment = hope.DepartmentCode,
                AgencyHeadDesignation = hope.DepartmentHeadDesignation,
            };

            db.APRHeader.Add(aprHeader);
            if(db.SaveChanges() == 0)
            {
                return false;
            }

            var aprDetails = new List<APRDetail>();
            var prDetailsSummary = PurchaseRequestDetails
                .GroupBy(d => new { d.ItemCode, d.UnitCost, d.References })
                .Select(d => new {
                    PRNumber = d.Key.References,
                    ItemCode = d.Key.ItemCode,
                    UnitPrice = d.Key.UnitCost,
                    Quantity = d.Sum(x => x.Quantity),
                    Amount = d.Sum(x => x.TotalCost)
                }).ToList();

            foreach (var item in prDetailsSummary)
            {
                var itemDetails = db.Items.Where(d => d.ItemCode == item.ItemCode).FirstOrDefault();
                var prID = db.PurchaseRequestHeader.Where(d => d.PRNumber == item.PRNumber).FirstOrDefault().ID;
                aprDetails.Add(new APRDetail
                {
                    APRHeaderReference = aprHeader.ID,
                    ItemReference = itemDetails.ID,
                    UnitReference = (int)itemDetails.IndividualUOMReference,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,
                    Amount = item.Amount,
                    PRReference = prID
                });
            }

            db.APRDetail.AddRange(aprDetails);
            if (db.SaveChanges() == 0)
            {
                return false;
            }

            var prReferences = aprDetails.GroupBy(d => d.PRReference).Select(d => d.Key).ToList();
            foreach(var prReference in prReferences)
            {
                var projectItems = db.ProjectPlanItems.Where(d => d.PRReference == prReference).ToList();
                var projectServices = db.ProjectPlanServices.Where(d => d.PRReference == prReference).ToList();
                if (projectItems.Count != 0)
                {
                    projectItems.ForEach(d => { d.APRReference = aprHeader.ID; d.Status = "Posted to APR"; });
                    db.SaveChanges();
                }
                if (projectServices.Count != 0)
                {
                    projectServices.ForEach(d => { d.APPReference = aprHeader.ID; d.Status = "Posted to APR"; });
                    db.SaveChanges();
                }
            }

            return true;
        }
        private string GenerateAgencyRefereceNo()
        {
            var referenceNo = string.Empty;
            var count = (db.APRHeader.Count() + 1).ToString();
            var series = count.Length == 1 ? "00" + count : count.Length == 2 ? "0" + count : count;
            referenceNo = series + "-" + DateTime.Now.ToString("yy");
            return referenceNo;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
                abdb.Dispose();
                hrisDataAccess.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}