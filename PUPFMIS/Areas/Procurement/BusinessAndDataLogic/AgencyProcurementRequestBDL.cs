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
            var aprVM = aprDAL.GetAgencyProcurementRequestDetail(AgencyControlNo);
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
            rows.Add(new ContentCell("NAME AND ADDRESS OF", 0, 8, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0), false, true, true));
            rows.Add(new ContentCell("POLYTECHNIC UNIVERSITY OF THE PHILIPPINES", 1, 8, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0), true, true, true));
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
            var aprItems = aprVM.APRDetails;
            foreach (var item in aprItems)
            {
                rows = new List<ContentCell>();
                rows.Add(new ContentCell((count++).ToString(), 0, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                rows.Add(new ContentCell(item.ItemFullName, 1, 7, false, false, ParagraphAlignment.Left, VerticalAlignment.Center));
                rows.Add(new ContentCell(item.Quantity.ToString(), 2, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                rows.Add(new ContentCell(item.UnitOfMeasure, 3, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                rows.Add(new ContentCell(item.UnitCost.ToString("N", new System.Globalization.CultureInfo("en-ph")), 4, 7, false, false, ParagraphAlignment.Right, VerticalAlignment.Center));
                rows.Add(new ContentCell(item.TotalCost.ToString("N", new System.Globalization.CultureInfo("en-ph")), 5, 7, false, false, ParagraphAlignment.Right, VerticalAlignment.Center));
                reports.AddRowContent(rows, 0.20);
            }

            for (int i = aprItems.Count(); i < 30; i++)
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
            rows.Add(new ContentCell(aprVM.APRDetails.Sum(d => d.TotalCost).ToString("C", new System.Globalization.CultureInfo("en-ph")), 5, 7, false, false, ParagraphAlignment.Right, VerticalAlignment.Center));
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
            rows.Add(new ContentCell("STOCKS REQUESTED ARE CERTIFIED TO BE", 0, 7, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0), true, true, true));
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
        private HRISDataAccess hris = new HRISDataAccess();
        private ABISDataAccess abis = new ABISDataAccess();
        private SystemBDL systemBDL = new SystemBDL();

        public List<int> GetFiscalYears()
        {
            return db.APRHeader.Select(d => d.FiscalYear).Distinct().ToList();
        }
        public AgencyProcurementRequestVM A2AContractSetup(string ContractCode, string UserEmail)
        {
            var user = db.UserAccounts.Where(d => d.Email == UserEmail).FirstOrDefault();
            var agencyDetails = db.AgencyDetails.FirstOrDefault();
            var contract = db.ProcurementProjects.Where(d => d.ContractCode == ContractCode).FirstOrDefault();
            var contractDetails = db.ProcurementProjectDetails.Where(d => d.FKProcurementProjectReference.ContractCode == ContractCode).ToList();
            var articles = contractDetails.Select(d => d.ArticleReference).Distinct().ToList();
            var aprDetails = contractDetails.Select(d => new APRDetailVM 
            {
                ArticleReference = (int)d.ArticleReference,
                ItemSequence = d.ItemSequence,
                ItemFullName = d.ItemFullName,
                ItemSpecifications = d.ItemSpecifications,
                Quantity = d.Quantity,
                UnitCost = d.EstimatedUnitCost,
                TotalCost = d.ApprovedBudgetForItem,
                UOMReference = d.UOMReference,
                UnitOfMeasure = db.UOM.Find(d.UOMReference).UnitName
            }).ToList();
            var procurementReference = hris.GetDepartmentDetails(agencyDetails.ProcurementOfficeReference);
            var accountingReference = hris.GetDepartmentDetails(agencyDetails.AccountingOfficeReference);
            var hopeReference = hris.GetDepartmentDetails(agencyDetails.HOPEReference);
            var aprVM = new AgencyProcurementRequestVM
            {
                AgencyControlNo = GenerateAgencyControlNo(contract.FiscalYear),
                ContractCode = ContractCode,
                FiscalYear = contract.FiscalYear,
                ContractName = contract.ContractName,
                ProcurementHead = procurementReference.DepartmentHead,
                ProcurementDepartment = procurementReference.Department,
                ProcurementDepartmentCode = procurementReference.DepartmentCode,
                ProcurementHeadDesignation = procurementReference.DepartmentHeadDesignation,
                ChiefAccountant = accountingReference.DepartmentHead,
                ChiefAccountantDepartment = accountingReference.Department,
                ChiefAccountantDepartmentCode = accountingReference.DepartmentCode,
                ChiefAccountantDesignation = accountingReference.DepartmentHeadDesignation,
                AgencyHead = hopeReference.SectorHead,
                AgencyHeadDepartment = hopeReference.Sector,
                AgencyHeadDepartmentCode = hopeReference.SectorCode,
                AgencyHeadDesignation = hopeReference.SectorHeadDesignation,
                APRDetails = aprDetails
            };

            return aprVM;
        }
        public List<ProcurementProjectListVM> GetContracts(string UserEmail)
        {
            var user = db.UserAccounts.Where(d => d.Email == UserEmail).FirstOrDefault();
            var purchaseRequests = db.PurchaseRequestHeader.Where(d => d.PRStatus == PurchaseRequestStatus.PurchaseRequestReceived && d.FKProcurementProjectReference.ModeOfProcurementReference == 10 && d.FKProcurementProjectReference.ProcurementProjectStage != ProcurementProjectStages.ProcurementClosed).Select(d => d.FKProcurementProjectReference.ContractCode).ToList();
            return db.ProcurementProjects.Where(d => d.ProjectCoordinator == user.EmpCode && purchaseRequests.Contains(d.ContractCode)).ToList().Select(d => new
            {
                ProcurementProjectType = d.ContractStrategy == ContractStrategies.LotBidding ? d.FKParentProjectReference.ProcurementProjectType : d.ProcurementProjectType,
                ContractCode = d.ContractStrategy == ContractStrategies.LotBidding ? d.FKParentProjectReference.ContractCode : d.ContractCode,
                ContractName = d.ContractStrategy == ContractStrategies.LotBidding ? d.FKParentProjectReference.ContractName : d.ContractName,
                ModeOfProcurementReference = d.ContractStrategy == ContractStrategies.LotBidding ? d.FKParentProjectReference.ModeOfProcurementReference : d.ModeOfProcurementReference,
                ModeOfProcurement = d.ContractStrategy == ContractStrategies.LotBidding ? d.FKParentProjectReference.FKModeOfProcurementReference.ModeOfProcurementName : d.FKModeOfProcurementReference.ModeOfProcurementName,
                FiscalYear = d.ContractStrategy == ContractStrategies.LotBidding ? d.FKParentProjectReference.FiscalYear : d.FiscalYear,
                ContractLocation = d.ContractStrategy == ContractStrategies.LotBidding ? d.FKParentProjectReference.ContractLocation : d.ContractLocation,
                ContractStatus = d.ContractStrategy == ContractStrategies.LotBidding ? d.FKParentProjectReference.ContractStatus : d.ContractStatus,
                ContractStrategy = d.ContractStrategy == ContractStrategies.LotBidding ? d.FKParentProjectReference.ContractStrategy : d.ContractStrategy,
                ProcurementProjectStage = d.ContractStrategy == ContractStrategies.LotBidding ? d.FKParentProjectReference.ProcurementProjectStage : d.ProcurementProjectStage,
                ApprovedBudgetForContract = d.ContractStrategy == ContractStrategies.LotBidding ? d.FKParentProjectReference.ApprovedBudgetForContract : d.ApprovedBudgetForContract
            })
            .GroupBy(d => d)
            .Select(d => new ProcurementProjectListVM
            {
                ProcurementProjectType = d.Key.ProcurementProjectType,
                ContractCode = d.Key.ContractCode,
                ContractName = d.Key.ContractName,
                ModeOfProcurementReference = d.Key.ModeOfProcurementReference,
                ModeOfProcurement = d.Key.ModeOfProcurement,
                FiscalYear = d.Key.FiscalYear,
                ContractLocation = d.Key.ContractLocation,
                ContractStatus = d.Key.ContractStatus,
                ContractStrategy = d.Key.ContractStrategy,
                ProcurementProjectStage = d.Key.ProcurementProjectStage,
                ApprovedBudgetForContract = d.Key.ApprovedBudgetForContract
            }).OrderBy(d => d.ProcurementProjectStage).ToList();
        }
        public List<AgencyProcurementRequest> GetAgencyProcurementRequests(int FiscalYear)
        {
            return db.APRHeader.Where(d => d.FiscalYear == FiscalYear).ToList();
        }
        public AgencyProcurementRequestVM GetAgencyProcurementRequestDetail(string  AgencyControlNo)
        {
            var aprHeader = db.APRHeader.Where(d => d.AgencyControlNo == AgencyControlNo).Select(d => new AgencyProcurementRequestVM 
            {
                FiscalYear = d.FiscalYear,
                AgencyControlNo = d.AgencyControlNo,
                CreatedAt = d.CreatedAt,
                ProcurementHead = d.ProcurementHead,
                ProcurementDepartment = d.ProcurementDepartment,
                ProcurementHeadDesignation = d.ProcurementHeadDesignation,
                ChiefAccountant = d.ChiefAccountant,
                ChiefAccountantDepartment = d.ChiefAccountantDepartment,
                ChiefAccountantDesignation = d.ChiefAccountantDesignation,
                AgencyHead = d.AgencyHead,
                AgencyHeadDepartment = d.AgencyHeadDepartment,
                AgencyHeadDesignation = d.AgencyHeadDesignation,
            }).FirstOrDefault();
            var aprDetail = db.APRDetail.Where(d => d.FKAPRReference.AgencyControlNo == aprHeader.AgencyControlNo).Select(d => new APRDetailVM 
            {
                ArticleReference = (int)d.ArticleReference,
                ItemSequence = d.ItemSequence,
                ItemFullName = d.ItemFullName,
                ItemSpecifications = d.ItemSpecifications,
                Quantity = d.Quantity,
                UnitCost = d.UnitCost,
                TotalCost = d.TotalCost,
                UnitOfMeasure = d.FKUOMReference.UnitName
            }).ToList();
            aprHeader.APRDetails = aprDetail;
            return aprHeader;
        }
        public bool PostAgencyProcurementRequest(AgencyProcurementRequestVM AgencyProcurementRequestVM, string UserEmail)
        {
            var user = db.UserAccounts.Where(d => d.Email == UserEmail).FirstOrDefault();
            var aprHeader = db.APRHeader.Add(new AgencyProcurementRequest 
            {
                FiscalYear = AgencyProcurementRequestVM.FiscalYear,
                AgencyControlNo = AgencyProcurementRequestVM.AgencyControlNo,
                CreatedAt = DateTime.Now,
                CreatedBy = user.EmpCode,
                ProcurementHead = AgencyProcurementRequestVM.ProcurementHead,
                ProcurementDepartment = AgencyProcurementRequestVM.ProcurementDepartment,
                ProcurementHeadDesignation = AgencyProcurementRequestVM.ProcurementHeadDesignation,
                ChiefAccountant = AgencyProcurementRequestVM.ChiefAccountant,
                ChiefAccountantDepartment = AgencyProcurementRequestVM.ChiefAccountantDepartment,
                ChiefAccountantDesignation = AgencyProcurementRequestVM.ChiefAccountantDesignation,
                AgencyHead = AgencyProcurementRequestVM.AgencyHead,
                AgencyHeadDepartment = AgencyProcurementRequestVM.AgencyHeadDepartment,
                AgencyHeadDesignation = AgencyProcurementRequestVM.AgencyHeadDesignation
            });

            if(db.SaveChanges() == 0)
            {
                return false;
            }

            var aprDetails = db.APRDetail.AddRange(AgencyProcurementRequestVM.APRDetails.Select(d => new AgencyProcurementRequestDetails
            {
                ArticleReference = d.ArticleReference,
                ItemSequence = d.ItemSequence,
                ItemFullName = d.ItemFullName,
                ItemSpecifications = d.ItemSpecifications,
                Quantity = d.Quantity,
                UnitCost = d.UnitCost,
                TotalCost = d.TotalCost,
                UOMReference = d.UOMReference,
                APRReference = aprHeader.ID
            }).ToList());

            var contract = db.ProcurementProjects.Where(d => d.ContractCode == AgencyProcurementRequestVM.ContractCode).FirstOrDefault();
            contract.ProcurementProjectStage = ProcurementProjectStages.ProcurementClosed;
            contract.ContractStatus = ProcurementProjectStatus.ContractCreated;

            var contractHeader = db.Contract.Add(new ContractHeader
            {
                ProcurementProjectReference = contract.ID,
                ContractType = ContractTypes.AgencyProcurementRequest,
                FiscalYear = contract.FiscalYear,
                ReferenceNumber = GenerateContractReferenceNo(ContractTypes.AgencyProcurementRequest),
                SupplierReference = 1,
                CreatedAt = DateTime.Now,
                CreatedBy = user.EmpCode,
                ContractPrice = aprDetails.Sum(d => d.TotalCost),
                PMOffice = AgencyProcurementRequestVM.ProcurementDepartmentCode,
                PMOHead = AgencyProcurementRequestVM.ProcurementHead,
                PMOHeadDesignation = AgencyProcurementRequestVM.ProcurementHeadDesignation,
                AccountingOffice = AgencyProcurementRequestVM.ChiefAccountantDepartmentCode,
                AccountingOfficeHead = AgencyProcurementRequestVM.ChiefAccountant,
                AccountingOfficeHeadDesignation = AgencyProcurementRequestVM.ChiefAccountantDesignation,
                HOPEOffice = AgencyProcurementRequestVM.AgencyHeadDepartmentCode,
                HOPE = AgencyProcurementRequestVM.AgencyHeadDepartmentCode,
                HOPEDesignation = AgencyProcurementRequestVM.AgencyHeadDesignation
            });


            if (db.SaveChanges() == 0)
            {
                return false;
            }

            db.ContractDetails.AddRange(aprDetails.Select(d => new ContractDetails
            {
                ContractReference = contractHeader.ID,
                ArticleReference = d.ArticleReference,
                ItemSequence = d.ItemSequence,
                ItemFullName = d.ItemFullName,
                ItemSpecifications = d.ItemSpecifications,
                UOMReference = d.UOMReference,
                Quantity = d.Quantity,
                ContractUnitPrice = d.UnitCost,
                ContractTotalPrice = d.TotalCost,
                Savings = d.TotalCost - d.TotalCost
            }).ToList());

            if (db.SaveChanges() == 0)
            {
                return false;
            }

            return true;
        }
        private string GenerateAgencyControlNo(int FiscalYear)
        {
            var controlNo = string.Empty;
            var aprCount = (db.APRHeader.Where(d => d.FiscalYear == FiscalYear).Count() + 1).ToString();
            controlNo = FiscalYear.ToString().Substring(1, 2) + "-" + (aprCount.Length == 3 ? aprCount : aprCount.Length == 2 ? "0" + aprCount : "00" + aprCount);
            return controlNo;
        }
        private string GenerateContractReferenceNo(ContractTypes ContractType)
        {
            var series = (db.Contract.Where(d => d.ContractType == ContractType).Count() + 1).ToString();
            var referenceNo = ContractType.GetAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>().ShortName + "-" + DateTime.Now.ToString("yy") + "-" + DateTime.Now.ToString("MM") + (series.Length == 1 ? "-000" + series : series.Length == 2 ? "-00" + series : series.Length == 3 ? "-0" + series : series);
            return referenceNo;
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
                hris.Dispose();
                abis.Dispose();
                systemBDL.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}