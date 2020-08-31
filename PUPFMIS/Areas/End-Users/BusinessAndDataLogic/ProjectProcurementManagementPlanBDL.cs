using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using PUPFMIS.Models;
using PUPFMIS.Models.AIS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace PUPFMIS.BusinessAndDataLogic
{
    public class ProjectProcurementManagementPlanBL : Controller
    {
        private ProjectProcurementManagementPlanDAL ppmpDAL = new ProjectProcurementManagementPlanDAL();
        private ReportsConfig reportConfig = new ReportsConfig();
        private FMISDbContext db = new FMISDbContext();

        public MemoryStream PrintPPMP(string ReferenceNo, string LogoPath, string UserEmail)
        {
            if(ReferenceNo == null)
            {
                return null;
            }
            if (ReferenceNo.Substring(5, 4) == "CUOS")
            {
                return GeneratePPMPSuppliesReport(ReferenceNo, LogoPath, UserEmail);
            }
            else
            {
                return GeneratePPMPReport(ReferenceNo, LogoPath, UserEmail);
            }
        }
        public MemoryStream GeneratePPMPSuppliesReport(string ReferenceNo, string LogoPath, string UserEmail)
        {
            Reports reports = new Reports();
            PPMPViewModel ppmpVM = ppmpDAL.GetPPMPDetails(ReferenceNo, UserEmail);
            var inventoryID = db.InventoryTypes.Where(d => d.InventoryTypeName == ppmpVM.Header.PPMPType).FirstOrDefault().ID;
            reports.ReportFilename = ppmpVM.Header.ReferenceNo;
            reports.CreateDocument(8.50, 13.00, Orientation.Landscape, 0.25);
            reports.AddDoubleColumnHeader(LogoPath);
            reports.AddColumnHeader(
                new HeaderLine { Content = "", Bold = false, Italic = false, FontSize = 5, ParagraphAlignment = ParagraphAlignment.Left },
                new HeaderLine { Content = "", Bold = true, Italic = false, FontSize = 5, ParagraphAlignment = ParagraphAlignment.Left }
            );
            reports.AddColumnHeader(
                new HeaderLine { Content = "Republic of the Philippines", Bold = false, Italic = false, FontSize = 10, ParagraphAlignment = ParagraphAlignment.Left },
                new HeaderLine { Content = "BP FORM " + Convert.ToChar(66 + inventoryID).ToString(), Bold = true, Italic = false, FontSize = 8, ParagraphAlignment = ParagraphAlignment.Left }
            );
            reports.AddColumnHeader(
                new HeaderLine { Content = "POLYTECHNIC UNIVERSITY OF THE PHILIPPINES", Bold = true, Italic = false, FontSize = 12, ParagraphAlignment = ParagraphAlignment.Left },
                new HeaderLine { Content = "REFERENCE NO: " + ppmpVM.Header.ReferenceNo, Bold = true, Italic = false, FontSize = 8, ParagraphAlignment = ParagraphAlignment.Left }
            );
            reports.AddColumnHeader(
                new HeaderLine { Content = "Anonas St., Sta. Mesa, Manila", Bold = false, Italic = false, FontSize = 8, ParagraphAlignment = ParagraphAlignment.Left },
                new HeaderLine { Content = "Date Printed: " + DateTime.Now.ToString("dd MMMM yyyy hh:mm tt"), Bold = false, Italic = false, FontSize = 8, ParagraphAlignment = ParagraphAlignment.Left }
            );

            if(ppmpVM.Header.Status == "PPMP Evaluated")
            {
                reports.AddColumnHeader(
                    new HeaderLine { Content = "", Bold = false, Italic = false, FontSize = 8, ParagraphAlignment = ParagraphAlignment.Left },
                    new HeaderLine { Content = "Date Evaluated: " + ((DateTime)ppmpVM.Header.EvaluatedAt).ToString("dd MMMM yyyy hh:mm tt"), Bold = true, Italic = false, FontSize = 8, ParagraphAlignment = ParagraphAlignment.Left }
                );
            }

            reports.AddNewLine();
            reports.AddNewLine();

            reports.AddSingleColumnHeader();
            reports.AddColumnHeader(
                new HeaderLine { Content = "PROJECT PROCUREMENT MANAGEMENT PLAN (PPMP)", Bold = false, Italic = false, FontSize = 10, ParagraphAlignment = ParagraphAlignment.Center }
            );
            reports.AddColumnHeader(
                new HeaderLine { Content = ppmpVM.Header.PPMPType, Bold = true, Italic = false, FontSize = 10, ParagraphAlignment = ParagraphAlignment.Center }
            );
            reports.AddColumnHeader(
                new HeaderLine { Content = "Fiscal Year " + ppmpVM.Header.FiscalYear.ToString(), Bold = false, Italic = false, FontSize = 8, ParagraphAlignment = ParagraphAlignment.Center }
            );

            reports.AddNewLine();

            var columns = new List<ContentColumn>();
            columns.Add(new ContentColumn(1.00));
            columns.Add(new ContentColumn(5.50));
            columns.Add(new ContentColumn(6.00));
            reports.AddTable(columns, false);

            var rows = new List<ContentCell>();
            rows.Add(new ContentCell("End-User: ", 0, 10, true));
            rows.Add(new ContentCell(ppmpVM.Header.Department, 1, 10, true, true, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, true));
            rows.Add(new ContentCell("", 2));
            reports.AddRowContent(rows, 0);

            reports.AddNewLine();

            columns = new List<ContentColumn>();
            columns.Add(new ContentColumn(1.00, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
            columns.Add(new ContentColumn(1.16, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
            columns.Add(new ContentColumn(0.75, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
            columns.Add(new ContentColumn(0.50, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
            columns.Add(new ContentColumn(0.50, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
            columns.Add(new ContentColumn(0.50, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
            columns.Add(new ContentColumn(0.50, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
            columns.Add(new ContentColumn(0.50, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
            columns.Add(new ContentColumn(0.50, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
            columns.Add(new ContentColumn(0.50, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
            columns.Add(new ContentColumn(0.50, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
            columns.Add(new ContentColumn(0.50, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
            columns.Add(new ContentColumn(0.50, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
            columns.Add(new ContentColumn(0.50, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
            columns.Add(new ContentColumn(0.50, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
            columns.Add(new ContentColumn(0.75, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
            columns.Add(new ContentColumn(0.75, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
            columns.Add(new ContentColumn(1.00, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
            columns.Add(new ContentColumn(1.09, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
            reports.AddTable(columns, true);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("PAP\nCode", 0, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 1));
            rows.Add(new ContentCell("Item and\nSpecifications", 1, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 1));
            rows.Add(new ContentCell("Unit of\nMeasure", 2, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 1));
            rows.Add(new ContentCell("Monthly Quantity Requirement", 3, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center, 11, 0));
            rows.Add(new ContentCell("Total\nQuantity", 15, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 1));
            rows.Add(new ContentCell("Unit\nCost", 16, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 1));
            rows.Add(new ContentCell("Estimated\nBudget", 17, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 1));
            rows.Add(new ContentCell("Remarks", 18, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 1));
            reports.AddRowContent(rows, 0.25);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("Jan", 3, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
            rows.Add(new ContentCell("Feb", 4, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
            rows.Add(new ContentCell("Mar", 5, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
            rows.Add(new ContentCell("Apr", 6, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
            rows.Add(new ContentCell("May", 7, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
            rows.Add(new ContentCell("Jun", 8, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
            rows.Add(new ContentCell("Jul", 9, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
            rows.Add(new ContentCell("Aug", 10, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
            rows.Add(new ContentCell("Sep", 11, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
            rows.Add(new ContentCell("Oct", 12, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
            rows.Add(new ContentCell("Nov", 13, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
            rows.Add(new ContentCell("Dec", 14, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
            reports.AddRowContent(rows, 0.25);

            columns = new List<ContentColumn>();
            columns.Add(new ContentColumn(12.5, new MigraDoc.DocumentObjectModel.Color(252, 207, 101)));
            reports.AddTable(columns, true);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("PART I. AVAILABLE AT PROCUREMENT SERVICE STORES", 0, 7, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Left, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center, 0, 0));
            reports.AddRowContent(rows, 0.25);

            var categories = ppmpVM.DBMItems.GroupBy(d => d.Category).Select(d => d.Key).ToList();
            if (categories.Count == 0)
            {
                columns = new List<ContentColumn>();
                columns.Add(new ContentColumn(12.5));
                reports.AddTable(columns, true);

                rows = new List<ContentCell>();
                rows.Add(new ContentCell("*** NO ITEMS ***", 0, 8.5, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                reports.AddRowContent(rows, 0.25);
            }
            foreach (var category in categories)
            {
                columns = new List<ContentColumn>();
                columns.Add(new ContentColumn(12.5, new MigraDoc.DocumentObjectModel.Color(74, 132, 249)));
                reports.AddTable(columns, true);

                rows = new List<ContentCell>();
                rows.Add(new ContentCell(category, 0, 7, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Left, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center, 0, 0));
                reports.AddRowContent(rows, 0.25);

                foreach(var item in ppmpVM.DBMItems.Where(d => d.Category == category).OrderBy(d => d.ProjectCode).ThenBy(d => d.ItemName))
                {
                    columns = new List<ContentColumn>();
                    columns.Add(new ContentColumn(1.00));
                    columns.Add(new ContentColumn(1.16));
                    columns.Add(new ContentColumn(0.75));
                    columns.Add(new ContentColumn(0.50));
                    columns.Add(new ContentColumn(0.50));
                    columns.Add(new ContentColumn(0.50));
                    columns.Add(new ContentColumn(0.50));
                    columns.Add(new ContentColumn(0.50));
                    columns.Add(new ContentColumn(0.50));
                    columns.Add(new ContentColumn(0.50));
                    columns.Add(new ContentColumn(0.50));
                    columns.Add(new ContentColumn(0.50));
                    columns.Add(new ContentColumn(0.50));
                    columns.Add(new ContentColumn(0.50));
                    columns.Add(new ContentColumn(0.50));
                    columns.Add(new ContentColumn(0.75));
                    columns.Add(new ContentColumn(0.75));
                    columns.Add(new ContentColumn(1.00));
                    columns.Add(new ContentColumn(1.09));
                    reports.AddTable(columns, true);

                    rows = new List<ContentCell>();
                    rows.Add(new ContentCell(item.ProjectCode, 0, 7, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                    rows.Add(new ContentCell(item.ItemName, 1, 7, true, false, ParagraphAlignment.Left, VerticalAlignment.Center));
                    rows.Add(new ContentCell(item.IndividualUOMReference, 2, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                    rows.Add(new ContentCell(item.JanMilestone, 3, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                    rows.Add(new ContentCell(item.FebMilestone, 4, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                    rows.Add(new ContentCell(item.MarMilestone, 5, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                    rows.Add(new ContentCell(item.AprMilestone, 6, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                    rows.Add(new ContentCell(item.MayMilestone, 7, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                    rows.Add(new ContentCell(item.JunMilestone, 8, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                    rows.Add(new ContentCell(item.JulMilestone, 9, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                    rows.Add(new ContentCell(item.AugMilestone, 10, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                    rows.Add(new ContentCell(item.SepMilestone, 11, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                    rows.Add(new ContentCell(item.OctMilestone, 12, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                    rows.Add(new ContentCell(item.NovMilestone, 13, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                    rows.Add(new ContentCell(item.DecMilestone, 14, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                    rows.Add(new ContentCell(item.TotalQty, 15, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                    rows.Add(new ContentCell(item.UnitCost, 16, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                    rows.Add(new ContentCell(String.Format("{0:#,##0.00}", item.EstimatedBudget), 17, 7, false, false, ParagraphAlignment.Right, VerticalAlignment.Center));
                    rows.Add(new ContentCell(item.Remarks + (item.ResponsibilityCenter == ppmpVM.Header.Department ? "" : ("\n\nRESPONSIBILITY CENTER: " + item.ResponsibilityCenter)), 18, 7, false, false, ParagraphAlignment.Left, VerticalAlignment.Top));
                    reports.AddRowContent(rows, 0.25);
                }
            }

            columns = new List<ContentColumn>();
            columns.Add(new ContentColumn(10.41));
            columns.Add(new ContentColumn(1.00));
            columns.Add(new ContentColumn(1.09));
            reports.AddTable(columns, true);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("SUB-TOTAL: ", 0, 8.5, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Right, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
            rows.Add(new ContentCell(String.Format("{0:C}", ppmpVM.DBMItems.Sum(d => d.EstimatedBudget)), 1, 8.5, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Right, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
            rows.Add(new ContentCell("", 2));
            reports.AddRowContent(rows, 0.25);

            columns = new List<ContentColumn>();
            columns.Add(new ContentColumn(1.00, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
            columns.Add(new ContentColumn(1.16, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
            columns.Add(new ContentColumn(0.75, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
            columns.Add(new ContentColumn(0.50, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
            columns.Add(new ContentColumn(0.50, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
            columns.Add(new ContentColumn(0.50, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
            columns.Add(new ContentColumn(0.50, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
            columns.Add(new ContentColumn(0.50, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
            columns.Add(new ContentColumn(0.50, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
            columns.Add(new ContentColumn(0.50, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
            columns.Add(new ContentColumn(0.50, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
            columns.Add(new ContentColumn(0.50, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
            columns.Add(new ContentColumn(0.50, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
            columns.Add(new ContentColumn(0.50, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
            columns.Add(new ContentColumn(0.50, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
            columns.Add(new ContentColumn(0.75, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
            columns.Add(new ContentColumn(0.75, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
            columns.Add(new ContentColumn(1.00, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
            columns.Add(new ContentColumn(1.09, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
            reports.AddTable(columns, true);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("PAP\nCode", 0, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 1));
            rows.Add(new ContentCell("Item and\nSpecifications", 1, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 1));
            rows.Add(new ContentCell("Unit of\nMeasure", 2, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 1));
            rows.Add(new ContentCell("Monthly Quantity Requirement", 3, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center, 11, 0));
            rows.Add(new ContentCell("Total\nQuantity", 15, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 1));
            rows.Add(new ContentCell("Unit\nCost", 16, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 1));
            rows.Add(new ContentCell("Estimated\nBudget", 17, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 1));
            rows.Add(new ContentCell("Remarks", 18, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 1));
            reports.AddRowContent(rows, 0.25);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("Jan", 3, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
            rows.Add(new ContentCell("Feb", 4, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
            rows.Add(new ContentCell("Mar", 5, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
            rows.Add(new ContentCell("Apr", 6, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
            rows.Add(new ContentCell("May", 7, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
            rows.Add(new ContentCell("Jun", 8, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
            rows.Add(new ContentCell("Jul", 9, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
            rows.Add(new ContentCell("Aug", 10, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
            rows.Add(new ContentCell("Sep", 11, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
            rows.Add(new ContentCell("Oct", 12, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
            rows.Add(new ContentCell("Nov", 13, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
            rows.Add(new ContentCell("Dec", 14, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
            reports.AddRowContent(rows, 0.25);

            columns = new List<ContentColumn>();
            columns.Add(new ContentColumn(12.5, new MigraDoc.DocumentObjectModel.Color(252, 207, 101)));
            reports.AddTable(columns, true);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("PART II. OTHER ITEMS NOT AVALABLE AT PS BUT REGULARLY PURCHASED FROM OTHER SOURCES (Note: Please indicate price of items)", 0, 7, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Left, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center, 0, 0));
            reports.AddRowContent(rows, 0.25);

            categories = ppmpVM.NonDBMItems.GroupBy(d => d.Category).Select(d => d.Key).ToList();
            if (categories.Count == 0)
            {
                columns = new List<ContentColumn>();
                columns.Add(new ContentColumn(12.5));
                reports.AddTable(columns, true);

                rows = new List<ContentCell>();
                rows.Add(new ContentCell("*** NO ITEMS ***", 0, 8.5, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                reports.AddRowContent(rows, 0.25);
            }
            foreach (var category in categories)
            {
                columns = new List<ContentColumn>();
                columns.Add(new ContentColumn(12.5, new MigraDoc.DocumentObjectModel.Color(74, 132, 249)));
                reports.AddTable(columns, true);

                rows = new List<ContentCell>();
                rows.Add(new ContentCell(category, 0, 7, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Left, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center, 0, 0));
                reports.AddRowContent(rows, 0.25);

                foreach (var item in ppmpVM.NonDBMItems.Where(d => d.Category == category).OrderBy(d => d.ProjectCode).ThenBy(d => d.ItemName))
                {
                    columns = new List<ContentColumn>();
                    columns.Add(new ContentColumn(1.00));
                    columns.Add(new ContentColumn(1.16));
                    columns.Add(new ContentColumn(0.75));
                    columns.Add(new ContentColumn(0.50));
                    columns.Add(new ContentColumn(0.50));
                    columns.Add(new ContentColumn(0.50));
                    columns.Add(new ContentColumn(0.50));
                    columns.Add(new ContentColumn(0.50));
                    columns.Add(new ContentColumn(0.50));
                    columns.Add(new ContentColumn(0.50));
                    columns.Add(new ContentColumn(0.50));
                    columns.Add(new ContentColumn(0.50));
                    columns.Add(new ContentColumn(0.50));
                    columns.Add(new ContentColumn(0.50));
                    columns.Add(new ContentColumn(0.50));
                    columns.Add(new ContentColumn(0.75));
                    columns.Add(new ContentColumn(0.75));
                    columns.Add(new ContentColumn(1.00));
                    columns.Add(new ContentColumn(1.09));
                    reports.AddTable(columns, true);

                    rows = new List<ContentCell>();
                    rows.Add(new ContentCell(item.ProjectCode, 0, 7, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                    rows.Add(new ContentCell(item.ItemName, 1, 7, true, false, ParagraphAlignment.Left, VerticalAlignment.Center));
                    rows.Add(new ContentCell(item.IndividualUOMReference, 2, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                    rows.Add(new ContentCell(item.JanMilestone, 3, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                    rows.Add(new ContentCell(item.FebMilestone, 4, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                    rows.Add(new ContentCell(item.MarMilestone, 5, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                    rows.Add(new ContentCell(item.AprMilestone, 6, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                    rows.Add(new ContentCell(item.MayMilestone, 7, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                    rows.Add(new ContentCell(item.JunMilestone, 8, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                    rows.Add(new ContentCell(item.JulMilestone, 9, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                    rows.Add(new ContentCell(item.AugMilestone, 10, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                    rows.Add(new ContentCell(item.SepMilestone, 11, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                    rows.Add(new ContentCell(item.OctMilestone, 12, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                    rows.Add(new ContentCell(item.NovMilestone, 13, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                    rows.Add(new ContentCell(item.DecMilestone, 14, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                    rows.Add(new ContentCell(item.TotalQty, 15, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                    rows.Add(new ContentCell(item.UnitCost, 16, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                    rows.Add(new ContentCell(String.Format("{0:#,##0.00}", item.EstimatedBudget), 17, 7, false, false, ParagraphAlignment.Right, VerticalAlignment.Center));
                    rows.Add(new ContentCell(item.Remarks + item.ResponsibilityCenter == ppmpVM.Header.Department ? "" : ("\n\nRESPONSIBILITY CENTER: " + item.ResponsibilityCenter), 18, 7, false, false, ParagraphAlignment.Left, VerticalAlignment.Top));
                    reports.AddRowContent(rows, 0.25);
                }
            }

            columns = new List<ContentColumn>();
            columns.Add(new ContentColumn(10.41));
            columns.Add(new ContentColumn(1.00));
            columns.Add(new ContentColumn(1.09));
            reports.AddTable(columns, true);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("SUB-TOTAL: ", 0, 8.5, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Right, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
            rows.Add(new ContentCell(String.Format("{0:C}", ppmpVM.NonDBMItems.Sum(d => d.EstimatedBudget)), 1, 8.5, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Right, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
            rows.Add(new ContentCell("", 2));
            reports.AddRowContent(rows, 0.25);

            columns = new List<ContentColumn>();
            columns.Add(new ContentColumn(10.41));
            columns.Add(new ContentColumn(1.00));
            columns.Add(new ContentColumn(1.09));
            reports.AddTable(columns, true);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("TOTAL ESTIMATED BUDGET: ", 0, 8.5, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Right, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
            rows.Add(new ContentCell(String.Format("{0:C}", (ppmpVM.DBMItems.Sum(d => d.EstimatedBudget) + ppmpVM.NonDBMItems.Sum(d => d.EstimatedBudget))), 1, 8.5, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Right, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
            rows.Add(new ContentCell("", 2));
            reports.AddRowContent(rows, 0.25);

            reports.AddNewLine();
            reports.AddSingleColumnHeader();
            reports.AddColumnHeader(
                new HeaderLine { Content = "NOTE: \n", Bold = false, Italic = false, FontSize = 7, ParagraphAlignment = ParagraphAlignment.Left, LeftIndent = 25 }
            );
            reports.AddColumnHeader(
                new HeaderLine { Content = "1. Technical Specifications for each Item/Project being proposed shall  be submitted as part of the PPMP.", Bold = false, Italic = false, FontSize = 7, ParagraphAlignment = ParagraphAlignment.Left, LeftIndent = 35 }
            );
            reports.AddColumnHeader(
                new HeaderLine { Content = "2.  Technical Specifications however,  must be in generic form;  no brand name shall be specified.", Bold = false, Italic = false, FontSize = 7, ParagraphAlignment = ParagraphAlignment.Left, LeftIndent = 35 }
            );
            reports.AddColumnHeader(
                new HeaderLine { Content = "3.  Non-submission of PPMP for supplies shall mean no budget provision for supplies.", Bold = false, Italic = false, FontSize = 7, ParagraphAlignment = ParagraphAlignment.Left, LeftIndent = 35 }
            );

            reports.AddNewLine();
            reports.AddNewLine();

            columns = new List<ContentColumn>();
            columns.Add(new ContentColumn(1.50));
            columns.Add(new ContentColumn(4.00));
            columns.Add(new ContentColumn(1.50));
            columns.Add(new ContentColumn(4.00));
            columns.Add(new ContentColumn(1.50));
            reports.AddTable(columns, false);


            rows = new List<ContentCell>();
            rows.Add(new ContentCell("", 0));
            rows.Add(new ContentCell("Prepared By:", 1, 8.5, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Left, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
            rows.Add(new ContentCell("", 2));
            rows.Add(new ContentCell("Submitted By:", 3, 8.5, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Left, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
            rows.Add(new ContentCell("", 4));
            reports.AddRowContent(rows, 0.25);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("\n\n", 0));
            rows.Add(new ContentCell("\n\n", 1, 8.5, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center, 0, 0, true));
            rows.Add(new ContentCell("\n\n", 2));
            rows.Add(new ContentCell("\n\n", 3, 8.5, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center, 0, 0, true));
            rows.Add(new ContentCell("\n\n", 4));
            reports.AddRowContent(rows, 0.75);

            var preparedBy = ppmpVM.Header.PreparedBy.Split(',');
            var submittedBy = ppmpVM.Header.SubmittedBy.Split(',');

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("", 0));
            rows.Add(new ContentCell(preparedBy[0], 1, 8.5, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
            rows.Add(new ContentCell("", 2));
            rows.Add(new ContentCell(submittedBy[0], 3, 8.5, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
            rows.Add(new ContentCell("", 4));
            reports.AddRowContent(rows, 0);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("", 0));
            rows.Add(new ContentCell(preparedBy[1], 1, 8.5, false, true, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
            rows.Add(new ContentCell("", 2));
            rows.Add(new ContentCell(submittedBy[1], 3, 8.5, false, true, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
            rows.Add(new ContentCell("", 4));
            reports.AddRowContent(rows, 0);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("", 0));
            rows.Add(new ContentCell(ppmpVM.Header.CreatedAt.ToString("dd MMMM yyyy hh:mm tt"), 1, 7, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
            rows.Add(new ContentCell("", 2));
            rows.Add(new ContentCell((ppmpVM.Header.SubmittedAt == null ? "(Submission Pending)" : ((DateTime)ppmpVM.Header.SubmittedAt).ToString("dd MMMM yyyy hh:mm tt")), 3, 7, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
            rows.Add(new ContentCell("", 4));
            reports.AddRowContent(rows, 0);

            var responsibilityCenterItems = ppmpDAL.GetPPMPResponsibilityCenterItems(ppmpVM.Header.Department, ppmpVM.Header.PPMPType);
            if(responsibilityCenterItems.Count != 0)
            {
                reports.CreateDocument();
                reports.AddDoubleColumnHeader(LogoPath);
                reports.AddColumnHeader(
                    new HeaderLine { Content = "", Bold = false, Italic = false, FontSize = 5, ParagraphAlignment = ParagraphAlignment.Left },
                    new HeaderLine { Content = "", Bold = true, Italic = false, FontSize = 5, ParagraphAlignment = ParagraphAlignment.Left }
                );
                reports.AddColumnHeader(
                    new HeaderLine { Content = "Republic of the Philippines", Bold = false, Italic = false, FontSize = 10, ParagraphAlignment = ParagraphAlignment.Left },
                    new HeaderLine { Content = "BP FORM " + Convert.ToChar(66 + inventoryID).ToString(), Bold = true, Italic = false, FontSize = 8, ParagraphAlignment = ParagraphAlignment.Left }
                );
                reports.AddColumnHeader(
                    new HeaderLine { Content = "POLYTECHNIC UNIVERSITY OF THE PHILIPPINES", Bold = true, Italic = false, FontSize = 12, ParagraphAlignment = ParagraphAlignment.Left },
                    new HeaderLine { Content = "Date Printed: " + DateTime.Now.ToString("dd MMMM yyyy hh:mm tt"), Bold = false, Italic = false, FontSize = 8, ParagraphAlignment = ParagraphAlignment.Left }
                );
                reports.AddColumnHeader(
                    new HeaderLine { Content = "Anonas St., Sta. Mesa, Manila", Bold = false, Italic = false, FontSize = 8, ParagraphAlignment = ParagraphAlignment.Left },
                    new HeaderLine { Content = "", Bold = false, Italic = false, FontSize = 8, ParagraphAlignment = ParagraphAlignment.Left }
                );

                reports.AddNewLine();
                reports.AddNewLine();

                reports.AddSingleColumnHeader();
                reports.AddColumnHeader(
                    new HeaderLine { Content = "PROJECT PROCUREMENT MANAGEMENT PLAN (PPMP)", Bold = false, Italic = false, FontSize = 10, ParagraphAlignment = ParagraphAlignment.Center }
                );
                reports.AddColumnHeader(
                    new HeaderLine { Content = ppmpVM.Header.PPMPType + " (Institutional)", Bold = true, Italic = false, FontSize = 10, ParagraphAlignment = ParagraphAlignment.Center }
                );
                reports.AddColumnHeader(
                    new HeaderLine { Content = "Fiscal Year " + ppmpVM.Header.FiscalYear.ToString(), Bold = false, Italic = false, FontSize = 8, ParagraphAlignment = ParagraphAlignment.Center }
                );

                reports.AddNewLine();

                columns = new List<ContentColumn>();
                columns.Add(new ContentColumn(2.00));
                columns.Add(new ContentColumn(5.00));
                columns.Add(new ContentColumn(6.00));
                reports.AddTable(columns, false);

                rows = new List<ContentCell>();
                rows.Add(new ContentCell("Responsibility Center: ", 0, 10, true));
                rows.Add(new ContentCell(ppmpVM.Header.Department, 1, 10, true, true, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, true));
                rows.Add(new ContentCell("", 2));
                reports.AddRowContent(rows, 0);

                reports.AddNewLine();

                columns = new List<ContentColumn>();
                columns.Add(new ContentColumn(1.16, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
                columns.Add(new ContentColumn(0.75, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
                columns.Add(new ContentColumn(0.50, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
                columns.Add(new ContentColumn(0.50, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
                columns.Add(new ContentColumn(0.50, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
                columns.Add(new ContentColumn(0.50, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
                columns.Add(new ContentColumn(0.50, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
                columns.Add(new ContentColumn(0.50, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
                columns.Add(new ContentColumn(0.50, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
                columns.Add(new ContentColumn(0.50, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
                columns.Add(new ContentColumn(0.50, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
                columns.Add(new ContentColumn(0.50, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
                columns.Add(new ContentColumn(0.50, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
                columns.Add(new ContentColumn(0.50, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
                columns.Add(new ContentColumn(0.75, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
                columns.Add(new ContentColumn(0.75, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
                columns.Add(new ContentColumn(1.00, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
                columns.Add(new ContentColumn(2.09, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
                reports.AddTable(columns, true);

                rows = new List<ContentCell>();
                rows.Add(new ContentCell("Item and\nSpecifications", 0, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 1));
                rows.Add(new ContentCell("Unit of\nMeasure", 1, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 1));
                rows.Add(new ContentCell("Monthly Quantity Requirement", 2, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center, 11, 0));
                rows.Add(new ContentCell("Total\nQuantity", 14, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 1));
                rows.Add(new ContentCell("Unit\nCost", 15, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 1));
                rows.Add(new ContentCell("Estimated\nBudget", 16, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 1));
                rows.Add(new ContentCell("Remarks", 17, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 1));
                reports.AddRowContent(rows, 0.25);

                rows = new List<ContentCell>();
                rows.Add(new ContentCell("Jan", 2, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                rows.Add(new ContentCell("Feb", 3, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                rows.Add(new ContentCell("Mar", 4, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                rows.Add(new ContentCell("Apr", 5, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                rows.Add(new ContentCell("May", 6, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                rows.Add(new ContentCell("Jun", 7, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                rows.Add(new ContentCell("Jul", 8, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                rows.Add(new ContentCell("Aug", 9, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                rows.Add(new ContentCell("Sep", 10, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                rows.Add(new ContentCell("Oct", 11, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                rows.Add(new ContentCell("Nov", 12, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                rows.Add(new ContentCell("Dec", 13, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                reports.AddRowContent(rows, 0.25);

                columns = new List<ContentColumn>();
                columns.Add(new ContentColumn(12.5, new MigraDoc.DocumentObjectModel.Color(252, 207, 101)));
                reports.AddTable(columns, true);

                rows = new List<ContentCell>();
                rows.Add(new ContentCell("PART I. AVAILABLE AT PROCUREMENT SERVICE STORES", 0, 7, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Left, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center, 0, 0));
                reports.AddRowContent(rows, 0.25);

                categories = responsibilityCenterItems.Where(d => d.ProcurementSource == ProcurementSources.PS_DBM).GroupBy(d => d.Category).Select(d => d.Key).ToList();
                if (categories.Count == 0)
                {
                    columns = new List<ContentColumn>();
                    columns.Add(new ContentColumn(12.5));
                    reports.AddTable(columns, true);

                    rows = new List<ContentCell>();
                    rows.Add(new ContentCell("*** NO ITEMS ***", 0, 8.5, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                    reports.AddRowContent(rows, 0.25);
                }
                foreach (var category in categories)
                {
                    columns = new List<ContentColumn>();
                    columns.Add(new ContentColumn(12.5, new MigraDoc.DocumentObjectModel.Color(74, 132, 249)));
                    reports.AddTable(columns, true);

                    rows = new List<ContentCell>();
                    rows.Add(new ContentCell(category, 0, 7, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Left, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center, 0, 0));
                    reports.AddRowContent(rows, 0.25);

                    foreach (var item in responsibilityCenterItems.Where(d => d.ProcurementSource == ProcurementSources.PS_DBM && d.Category == category).OrderBy(d => d.ItemName))
                    {
                        columns = new List<ContentColumn>();
                        columns.Add(new ContentColumn(1.16));
                        columns.Add(new ContentColumn(0.75));
                        columns.Add(new ContentColumn(0.50));
                        columns.Add(new ContentColumn(0.50));
                        columns.Add(new ContentColumn(0.50));
                        columns.Add(new ContentColumn(0.50));
                        columns.Add(new ContentColumn(0.50));
                        columns.Add(new ContentColumn(0.50));
                        columns.Add(new ContentColumn(0.50));
                        columns.Add(new ContentColumn(0.50));
                        columns.Add(new ContentColumn(0.50));
                        columns.Add(new ContentColumn(0.50));
                        columns.Add(new ContentColumn(0.50));
                        columns.Add(new ContentColumn(0.50));
                        columns.Add(new ContentColumn(0.75));
                        columns.Add(new ContentColumn(0.75));
                        columns.Add(new ContentColumn(1.00));
                        columns.Add(new ContentColumn(2.09));
                        reports.AddTable(columns, true);

                        rows = new List<ContentCell>();
                        rows.Add(new ContentCell(item.ItemName, 0, 7, true, false, ParagraphAlignment.Left, VerticalAlignment.Center));
                        rows.Add(new ContentCell(item.IndividualUOMReference, 1, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                        rows.Add(new ContentCell(item.JanMilestone, 2, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                        rows.Add(new ContentCell(item.FebMilestone, 3, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                        rows.Add(new ContentCell(item.MarMilestone, 4, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                        rows.Add(new ContentCell(item.AprMilestone, 5, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                        rows.Add(new ContentCell(item.MayMilestone, 6, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                        rows.Add(new ContentCell(item.JunMilestone, 7, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                        rows.Add(new ContentCell(item.JulMilestone, 8, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                        rows.Add(new ContentCell(item.AugMilestone, 9, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                        rows.Add(new ContentCell(item.SepMilestone, 10, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                        rows.Add(new ContentCell(item.OctMilestone, 11, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                        rows.Add(new ContentCell(item.NovMilestone, 12, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                        rows.Add(new ContentCell(item.DecMilestone, 13, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                        rows.Add(new ContentCell(item.TotalQty, 14, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                        rows.Add(new ContentCell(item.UnitCost, 15, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                        rows.Add(new ContentCell(String.Format("{0:#,##0.00}", item.EstimatedBudget), 16, 7, false, false, ParagraphAlignment.Right, VerticalAlignment.Center));
                        rows.Add(new ContentCell(item.Remarks, 17, 7, false, false, ParagraphAlignment.Left, VerticalAlignment.Center));

                        reports.AddRowContent(rows, 0.25);
                    }
                }

                columns = new List<ContentColumn>();
                columns.Add(new ContentColumn(9.41));
                columns.Add(new ContentColumn(1.00));
                columns.Add(new ContentColumn(2.09));
                reports.AddTable(columns, true);

                rows = new List<ContentCell>();
                rows.Add(new ContentCell("SUB-TOTAL: ", 0, 8.5, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Right, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                rows.Add(new ContentCell(String.Format("{0:C}", responsibilityCenterItems.Where(d => d.ProcurementSource == ProcurementSources.PS_DBM).Sum(d => d.EstimatedBudget)), 1, 8.5, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Right, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                rows.Add(new ContentCell("", 2));
                reports.AddRowContent(rows, 0.25);

                columns = new List<ContentColumn>();
                columns.Add(new ContentColumn(1.16, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
                columns.Add(new ContentColumn(0.75, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
                columns.Add(new ContentColumn(0.50, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
                columns.Add(new ContentColumn(0.50, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
                columns.Add(new ContentColumn(0.50, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
                columns.Add(new ContentColumn(0.50, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
                columns.Add(new ContentColumn(0.50, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
                columns.Add(new ContentColumn(0.50, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
                columns.Add(new ContentColumn(0.50, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
                columns.Add(new ContentColumn(0.50, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
                columns.Add(new ContentColumn(0.50, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
                columns.Add(new ContentColumn(0.50, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
                columns.Add(new ContentColumn(0.50, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
                columns.Add(new ContentColumn(0.50, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
                columns.Add(new ContentColumn(0.75, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
                columns.Add(new ContentColumn(0.75, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
                columns.Add(new ContentColumn(1.00, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
                columns.Add(new ContentColumn(2.09, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
                reports.AddTable(columns, true);

                rows = new List<ContentCell>();
                rows.Add(new ContentCell("Item and\nSpecifications",0, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 1));
                rows.Add(new ContentCell("Unit of\nMeasure", 1, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 1));
                rows.Add(new ContentCell("Monthly Quantity Requirement", 2, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center, 11, 0));
                rows.Add(new ContentCell("Total\nQuantity", 14, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 1));
                rows.Add(new ContentCell("Unit\nCost", 15, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 1));
                rows.Add(new ContentCell("Estimated\nBudget", 16, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 1));
                rows.Add(new ContentCell("Remarks", 17, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 1));
                reports.AddRowContent(rows, 0.25);

                rows = new List<ContentCell>();
                rows.Add(new ContentCell("Jan", 2, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                rows.Add(new ContentCell("Feb", 3, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                rows.Add(new ContentCell("Mar", 4, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                rows.Add(new ContentCell("Apr", 5, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                rows.Add(new ContentCell("May", 6, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                rows.Add(new ContentCell("Jun", 7, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                rows.Add(new ContentCell("Jul", 8, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                rows.Add(new ContentCell("Aug", 9, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                rows.Add(new ContentCell("Sep", 10, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                rows.Add(new ContentCell("Oct", 11, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                rows.Add(new ContentCell("Nov", 12, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                rows.Add(new ContentCell("Dec", 13, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                reports.AddRowContent(rows, 0.25);

                columns = new List<ContentColumn>();
                columns.Add(new ContentColumn(12.5, new MigraDoc.DocumentObjectModel.Color(252, 207, 101)));
                reports.AddTable(columns, true);

                rows = new List<ContentCell>();
                rows.Add(new ContentCell("PART II. OTHER ITEMS NOT AVALABLE AT PS BUT REGULARLY PURCHASED FROM OTHER SOURCES (Note: Please indicate price of items)", 0, 7, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Left, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center, 0, 0));
                reports.AddRowContent(rows, 0.25);

                categories = responsibilityCenterItems.Where(d => d.ProcurementSource == ProcurementSources.Non_DBM).GroupBy(d => d.Category).Select(d => d.Key).ToList();
                if (categories.Count == 0)
                {
                    columns = new List<ContentColumn>();
                    columns.Add(new ContentColumn(12.5));
                    reports.AddTable(columns, true);

                    rows = new List<ContentCell>();
                    rows.Add(new ContentCell("*** NO ITEMS ***", 0, 8.5, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                    reports.AddRowContent(rows, 0.25);
                }
                foreach (var category in categories)
                {
                    columns = new List<ContentColumn>();
                    columns.Add(new ContentColumn(12.5, new MigraDoc.DocumentObjectModel.Color(74, 132, 249)));
                    reports.AddTable(columns, true);

                    rows = new List<ContentCell>();
                    rows.Add(new ContentCell(category, 0, 7, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Left, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center, 0, 0));
                    reports.AddRowContent(rows, 0.25);

                    foreach (var item in responsibilityCenterItems.Where(d => d.ProcurementSource == ProcurementSources.Non_DBM && d.Category == category).OrderBy(d => d.ProjectCode).ThenBy(d => d.ItemName))
                    {
                        columns = new List<ContentColumn>();
                        columns.Add(new ContentColumn(1.16));
                        columns.Add(new ContentColumn(0.75));
                        columns.Add(new ContentColumn(0.50));
                        columns.Add(new ContentColumn(0.50));
                        columns.Add(new ContentColumn(0.50));
                        columns.Add(new ContentColumn(0.50));
                        columns.Add(new ContentColumn(0.50));
                        columns.Add(new ContentColumn(0.50));
                        columns.Add(new ContentColumn(0.50));
                        columns.Add(new ContentColumn(0.50));
                        columns.Add(new ContentColumn(0.50));
                        columns.Add(new ContentColumn(0.50));
                        columns.Add(new ContentColumn(0.50));
                        columns.Add(new ContentColumn(0.50));
                        columns.Add(new ContentColumn(0.75));
                        columns.Add(new ContentColumn(0.75));
                        columns.Add(new ContentColumn(1.00));
                        columns.Add(new ContentColumn(2.09));
                        reports.AddTable(columns, true);

                        rows = new List<ContentCell>();
                        rows.Add(new ContentCell(item.ItemName, 0, 7, true, false, ParagraphAlignment.Left, VerticalAlignment.Center));
                        rows.Add(new ContentCell(item.IndividualUOMReference, 1, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                        rows.Add(new ContentCell(item.JanMilestone, 2, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                        rows.Add(new ContentCell(item.FebMilestone, 3, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                        rows.Add(new ContentCell(item.MarMilestone, 4, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                        rows.Add(new ContentCell(item.AprMilestone, 5, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                        rows.Add(new ContentCell(item.MayMilestone, 6, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                        rows.Add(new ContentCell(item.JunMilestone, 7, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                        rows.Add(new ContentCell(item.JulMilestone, 8, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                        rows.Add(new ContentCell(item.AugMilestone, 9, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                        rows.Add(new ContentCell(item.SepMilestone, 10, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                        rows.Add(new ContentCell(item.OctMilestone, 11, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                        rows.Add(new ContentCell(item.NovMilestone, 12, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                        rows.Add(new ContentCell(item.DecMilestone, 13, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                        rows.Add(new ContentCell(item.TotalQty, 14, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                        rows.Add(new ContentCell(item.UnitCost, 15, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                        rows.Add(new ContentCell(String.Format("{0:#,##0.00}", item.EstimatedBudget), 16, 7, false, false, ParagraphAlignment.Right, VerticalAlignment.Center));
                        rows.Add(new ContentCell(item.Remarks, 17, 7, false, false, ParagraphAlignment.Left, VerticalAlignment.Center));
                        reports.AddRowContent(rows, 0.25);
                    }
                }

                columns = new List<ContentColumn>();
                columns.Add(new ContentColumn(9.41));
                columns.Add(new ContentColumn(1.00));
                columns.Add(new ContentColumn(2.09));
                reports.AddTable(columns, true);

                rows = new List<ContentCell>();
                rows.Add(new ContentCell("SUB-TOTAL: ", 0, 8.5, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Right, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                rows.Add(new ContentCell(String.Format("{0:C}", responsibilityCenterItems.Where(d => d.ProcurementSource == ProcurementSources.Non_DBM).Sum(d => d.EstimatedBudget)), 1, 8.5, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Right, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                rows.Add(new ContentCell("", 2));
                reports.AddRowContent(rows, 0.25);

                columns = new List<ContentColumn>();
                columns.Add(new ContentColumn(9.41));
                columns.Add(new ContentColumn(1.00));
                columns.Add(new ContentColumn(2.09));
                reports.AddTable(columns, true);

                rows = new List<ContentCell>();
                rows.Add(new ContentCell("TOTAL ESTIMATED BUDGET: ", 0, 8.5, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Right, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                rows.Add(new ContentCell(String.Format("{0:C}", (responsibilityCenterItems.Where(d => d.ProcurementSource == ProcurementSources.PS_DBM).Sum(d => d.EstimatedBudget) + responsibilityCenterItems.Where(d => d.ProcurementSource == ProcurementSources.Non_DBM).Sum(d => d.EstimatedBudget))), 1, 8.5, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Right, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                rows.Add(new ContentCell("", 2));
                reports.AddRowContent(rows, 0.25);
            

            reports.AddNewLine();
            reports.AddNewLine();

            columns = new List<ContentColumn>();
            columns.Add(new ContentColumn(1.50));
            columns.Add(new ContentColumn(4.00));
            columns.Add(new ContentColumn(1.50));
            columns.Add(new ContentColumn(4.00));
            columns.Add(new ContentColumn(1.50));
            reports.AddTable(columns, false);


            rows = new List<ContentCell>();
            rows.Add(new ContentCell("", 0));
            rows.Add(new ContentCell("Prepared By:", 1, 8.5, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Left, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
            rows.Add(new ContentCell("", 2));
            rows.Add(new ContentCell("Submitted By:", 3, 8.5, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Left, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
            rows.Add(new ContentCell("", 4));
            reports.AddRowContent(rows, 0.25);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("\n\n", 0));
            rows.Add(new ContentCell("\n\n", 1, 8.5, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center, 0, 0, true));
            rows.Add(new ContentCell("\n\n", 2));
            rows.Add(new ContentCell("\n\n", 3, 8.5, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center, 0, 0, true));
            rows.Add(new ContentCell("\n\n", 4));
            reports.AddRowContent(rows, 0.75);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("", 0));
            rows.Add(new ContentCell(preparedBy[0], 1, 8.5, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
            rows.Add(new ContentCell("", 2));
            rows.Add(new ContentCell(submittedBy[0], 3, 8.5, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
            rows.Add(new ContentCell("", 4));
            reports.AddRowContent(rows, 0);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("", 0));
            rows.Add(new ContentCell(preparedBy[1], 1, 8.5, false, true, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
            rows.Add(new ContentCell("", 2));
            rows.Add(new ContentCell(submittedBy[1], 3, 8.5, false, true, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
            rows.Add(new ContentCell("", 4));
            reports.AddRowContent(rows, 0);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("", 0));
            rows.Add(new ContentCell(ppmpVM.Header.CreatedAt.ToString("dd MMMM yyyy hh:mm tt"), 1, 7, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
            rows.Add(new ContentCell("", 2));
            rows.Add(new ContentCell((ppmpVM.Header.SubmittedAt == null ? "(Submission Pending)" : ((DateTime)ppmpVM.Header.SubmittedAt).ToString("dd MMMM yyyy hh:mm tt")), 3, 7, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
            rows.Add(new ContentCell("", 4));
            reports.AddRowContent(rows, 0);
            }
            return reports.GenerateReport();
        }
        public MemoryStream GeneratePPMPReport(string ReferenceNo, string LogoPath, string UserEmail)
        {
            Reports reports = new Reports();
            PPMPViewModel ppmpVM = ppmpDAL.GetPPMPDetails(ReferenceNo, UserEmail);
            var inventoryID = db.InventoryTypes.Where(d => d.InventoryTypeName == ppmpVM.Header.PPMPType).FirstOrDefault().ID;
            reports.ReportFilename = ppmpVM.Header.ReferenceNo;
            reports.CreateDocument(8.50, 13.00, Orientation.Landscape, 0.25);
            reports.AddDoubleColumnHeader(LogoPath);
            reports.AddColumnHeader(
                new HeaderLine { Content = "", Bold = false, Italic = false, FontSize = 5, ParagraphAlignment = ParagraphAlignment.Left },
                new HeaderLine { Content = "", Bold = true, Italic = false, FontSize = 5, ParagraphAlignment = ParagraphAlignment.Left }
            );
            reports.AddColumnHeader(
                new HeaderLine { Content = "Republic of the Philippines", Bold = false, Italic = false, FontSize = 10, ParagraphAlignment = ParagraphAlignment.Left },
                new HeaderLine { Content = "BP FORM " + Convert.ToChar(66 + inventoryID).ToString(), Bold = true, Italic = false, FontSize = 8, ParagraphAlignment = ParagraphAlignment.Left }
            );
            reports.AddColumnHeader(
                new HeaderLine { Content = "POLYTECHNIC UNIVERSITY OF THE PHILIPPINES", Bold = true, Italic = false, FontSize = 12, ParagraphAlignment = ParagraphAlignment.Left },
                new HeaderLine { Content = "REFERENCE NO: " + ppmpVM.Header.ReferenceNo, Bold = true, Italic = false, FontSize = 8, ParagraphAlignment = ParagraphAlignment.Left }
            );
            reports.AddColumnHeader(
                new HeaderLine { Content = "Anonas St., Sta. Mesa, Manila", Bold = false, Italic = false, FontSize = 8, ParagraphAlignment = ParagraphAlignment.Left },
                new HeaderLine { Content = "Date Printed: " + DateTime.Now.ToString("dd MMMM yyyy hh:mm tt"), Bold = false, Italic = false, FontSize = 8, ParagraphAlignment = ParagraphAlignment.Left }
            );

            reports.AddNewLine();
            reports.AddNewLine();

            reports.AddSingleColumnHeader();
            reports.AddColumnHeader(
                new HeaderLine { Content = "PROJECT PROCUREMENT MANAGEMENT PLAN (PPMP)", Bold = false, Italic = false, FontSize = 10, ParagraphAlignment = ParagraphAlignment.Center }
            );
            reports.AddColumnHeader(
                new HeaderLine { Content = ppmpVM.Header.PPMPType, Bold = true, Italic = false, FontSize = 10, ParagraphAlignment = ParagraphAlignment.Center }
            );
            reports.AddColumnHeader(
                new HeaderLine { Content = "Fiscal Year " + ppmpVM.Header.FiscalYear.ToString(), Bold = false, Italic = false, FontSize = 8, ParagraphAlignment = ParagraphAlignment.Center }
            );

            reports.AddNewLine();

            var columns = new List<ContentColumn>();
            columns.Add(new ContentColumn(1.00));
            columns.Add(new ContentColumn(5.50));
            columns.Add(new ContentColumn(6.00));
            reports.AddTable(columns, false);

            var rows = new List<ContentCell>();
            rows.Add(new ContentCell("End-User: ", 0, 10, true));
            rows.Add(new ContentCell(ppmpVM.Header.Department, 1, 10, true, true, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, true));
            rows.Add(new ContentCell("", 2));
            reports.AddRowContent(rows, 0);

            reports.AddNewLine();

            columns = new List<ContentColumn>();
            columns.Add(new ContentColumn(1.25, new MigraDoc.DocumentObjectModel.Color(254, 159, 89))); //PAP Code
            columns.Add(new ContentColumn(1.75, new MigraDoc.DocumentObjectModel.Color(254, 159, 89))); //Item
            columns.Add(new ContentColumn(0.75, new MigraDoc.DocumentObjectModel.Color(254, 159, 89))); //Qty/size
            columns.Add(new ContentColumn(1.00, new MigraDoc.DocumentObjectModel.Color(254, 159, 89))); //Estimated Budget
            columns.Add(new ContentColumn(0.50, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
            columns.Add(new ContentColumn(0.50, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
            columns.Add(new ContentColumn(0.50, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
            columns.Add(new ContentColumn(0.50, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
            columns.Add(new ContentColumn(0.50, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
            columns.Add(new ContentColumn(0.50, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
            columns.Add(new ContentColumn(0.50, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
            columns.Add(new ContentColumn(0.50, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
            columns.Add(new ContentColumn(0.50, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
            columns.Add(new ContentColumn(0.50, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
            columns.Add(new ContentColumn(0.50, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
            columns.Add(new ContentColumn(0.50, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
            columns.Add(new ContentColumn(1.75, new MigraDoc.DocumentObjectModel.Color(254, 159, 89))); //Remarks
            reports.AddTable(columns, true);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("PAP\nCode", 0, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 1));
            rows.Add(new ContentCell("Item and\nSpecifications", 1, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 1));
            rows.Add(new ContentCell("Quantity/\nSize", 2, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 1));
            rows.Add(new ContentCell("Estimated\nBudget", 3, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 1));
            rows.Add(new ContentCell("Schedule/Milestone of Activities", 4, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center, 11, 0));
            rows.Add(new ContentCell("Total\nQuantity", 15, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 1));
            rows.Add(new ContentCell("Remarks", 16, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 1));
            reports.AddRowContent(rows, 0.25);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("Jan", 4, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
            rows.Add(new ContentCell("Feb", 5, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
            rows.Add(new ContentCell("Mar", 6, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
            rows.Add(new ContentCell("Apr", 7, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
            rows.Add(new ContentCell("May", 8, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
            rows.Add(new ContentCell("Jun", 9, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
            rows.Add(new ContentCell("Jul", 10, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
            rows.Add(new ContentCell("Aug", 11, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
            rows.Add(new ContentCell("Sep", 12, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
            rows.Add(new ContentCell("Oct", 13, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
            rows.Add(new ContentCell("Nov", 14, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
            rows.Add(new ContentCell("Dec", 15, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
            reports.AddRowContent(rows, 0.25);

            columns = new List<ContentColumn>();
            columns.Add(new ContentColumn(12.5, new MigraDoc.DocumentObjectModel.Color(252, 207, 101)));
            reports.AddTable(columns, true);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("PART I. AVAILABLE AT PROCUREMENT SERVICE STORES", 0, 7, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Left, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
            reports.AddRowContent(rows, 0.25);

            var categories = ppmpVM.DBMItems.GroupBy(d => d.Category).Select(d => d.Key).ToList();
            if (categories.Count == 0)
            {
                columns = new List<ContentColumn>();
                columns.Add(new ContentColumn(12.5));
                reports.AddTable(columns, true);

                rows = new List<ContentCell>();
                rows.Add(new ContentCell("*** NO ITEMS ***", 0, 8.5, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                reports.AddRowContent(rows, 0.25);
            }
            foreach (var category in categories)
            {
                columns = new List<ContentColumn>();
                columns.Add(new ContentColumn(12.5, new MigraDoc.DocumentObjectModel.Color(74, 132, 249)));
                reports.AddTable(columns, true);

                rows = new List<ContentCell>();
                rows.Add(new ContentCell(category, 0, 7, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Left, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                reports.AddRowContent(rows, 0.25);

                foreach (var item in ppmpVM.DBMItems.Where(d => d.Category == category).OrderBy(d => d.ProjectCode).ThenBy(d => d.ItemName))
                {
                    columns = new List<ContentColumn>();
                    columns.Add(new ContentColumn(1.25)); //PAP Code
                    columns.Add(new ContentColumn(1.75)); //Item
                    columns.Add(new ContentColumn(0.75)); //Qty/size
                    columns.Add(new ContentColumn(1.00)); //Estimated Budget
                    columns.Add(new ContentColumn(0.50));
                    columns.Add(new ContentColumn(0.50));
                    columns.Add(new ContentColumn(0.50));
                    columns.Add(new ContentColumn(0.50));
                    columns.Add(new ContentColumn(0.50));
                    columns.Add(new ContentColumn(0.50));
                    columns.Add(new ContentColumn(0.50));
                    columns.Add(new ContentColumn(0.50));
                    columns.Add(new ContentColumn(0.50));
                    columns.Add(new ContentColumn(0.50));
                    columns.Add(new ContentColumn(0.50));
                    columns.Add(new ContentColumn(0.50));
                    columns.Add(new ContentColumn(1.75)); //Remarks
                    reports.AddTable(columns, true);

                    rows = new List<ContentCell>();
                    rows.Add(new ContentCell(item.ProjectCode, 0, 7, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                    rows.Add(new ContentCell(item.ItemName, 1, 7, true, false, ParagraphAlignment.Justify, VerticalAlignment.Center));
                    rows.Add(new ContentCell(item.TotalQty + "\n" + item.IndividualUOMReference, 2, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                    rows.Add(new ContentCell(String.Format("{0:#,##0.00}", item.EstimatedBudget), 3, 7, false, false, ParagraphAlignment.Right, VerticalAlignment.Center));
                    rows.Add(new ContentCell(item.JanMilestone, 4, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                    rows.Add(new ContentCell(item.FebMilestone, 5, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                    rows.Add(new ContentCell(item.MarMilestone, 6, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                    rows.Add(new ContentCell(item.AprMilestone, 7, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                    rows.Add(new ContentCell(item.MayMilestone, 8, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                    rows.Add(new ContentCell(item.JunMilestone, 9, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                    rows.Add(new ContentCell(item.JulMilestone, 10, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                    rows.Add(new ContentCell(item.AugMilestone, 11, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                    rows.Add(new ContentCell(item.SepMilestone, 12, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                    rows.Add(new ContentCell(item.OctMilestone, 13, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                    rows.Add(new ContentCell(item.NovMilestone, 14, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                    rows.Add(new ContentCell(item.DecMilestone, 15, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                    rows.Add(new ContentCell(item.Remarks + "; " + item.Project + (item.ResponsibilityCenter == null ? "" : item.ResponsibilityCenter == ppmpVM.Header.Department ? "" : "\n\nRESPONSIBILITY CENTER: " + item.ResponsibilityCenter), 16, 7, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top));

                    reports.AddRowContent(rows, 0.25);
                }
            }

            columns = new List<ContentColumn>();
            columns.Add(new ContentColumn(3.75));
            columns.Add(new ContentColumn(1.00));
            columns.Add(new ContentColumn(7.75));
            reports.AddTable(columns, true);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("SUB-TOTAL: ", 0, 8.5, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Right, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
            rows.Add(new ContentCell(String.Format("{0:C}", ppmpVM.DBMItems.Sum(d => d.EstimatedBudget)), 1, 8.5, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Right, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
            rows.Add(new ContentCell("", 2));
            reports.AddRowContent(rows, 0.25);

            columns = new List<ContentColumn>();
            columns.Add(new ContentColumn(1.25, new MigraDoc.DocumentObjectModel.Color(254, 159, 89))); //PAP Code
            columns.Add(new ContentColumn(1.75, new MigraDoc.DocumentObjectModel.Color(254, 159, 89))); //Item
            columns.Add(new ContentColumn(0.75, new MigraDoc.DocumentObjectModel.Color(254, 159, 89))); //Qty/size
            columns.Add(new ContentColumn(1.00, new MigraDoc.DocumentObjectModel.Color(254, 159, 89))); //Estimated Budget
            columns.Add(new ContentColumn(0.50, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
            columns.Add(new ContentColumn(0.50, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
            columns.Add(new ContentColumn(0.50, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
            columns.Add(new ContentColumn(0.50, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
            columns.Add(new ContentColumn(0.50, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
            columns.Add(new ContentColumn(0.50, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
            columns.Add(new ContentColumn(0.50, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
            columns.Add(new ContentColumn(0.50, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
            columns.Add(new ContentColumn(0.50, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
            columns.Add(new ContentColumn(0.50, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
            columns.Add(new ContentColumn(0.50, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
            columns.Add(new ContentColumn(0.50, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
            columns.Add(new ContentColumn(1.75, new MigraDoc.DocumentObjectModel.Color(254, 159, 89))); //Remarks
            reports.AddTable(columns, true);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("PAP\nCode", 0, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 1));
            rows.Add(new ContentCell("Item and\nSpecifications", 1, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 1));
            rows.Add(new ContentCell("Quantity/\nSize", 2, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 1));
            rows.Add(new ContentCell("Estimated\nBudget", 3, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 1));
            rows.Add(new ContentCell("Schedule/Milestone of Activities", 4, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center, 11, 0));
            rows.Add(new ContentCell("Total\nQuantity", 15, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 1));
            rows.Add(new ContentCell("Remarks", 16, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 1));
            reports.AddRowContent(rows, 0.25);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("Jan", 4, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
            rows.Add(new ContentCell("Feb", 5, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
            rows.Add(new ContentCell("Mar", 6, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
            rows.Add(new ContentCell("Apr", 7, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
            rows.Add(new ContentCell("May", 8, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
            rows.Add(new ContentCell("Jun", 9, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
            rows.Add(new ContentCell("Jul", 10, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
            rows.Add(new ContentCell("Aug", 11, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
            rows.Add(new ContentCell("Sep", 12, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
            rows.Add(new ContentCell("Oct", 13, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
            rows.Add(new ContentCell("Nov", 14, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
            rows.Add(new ContentCell("Dec", 15, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
            reports.AddRowContent(rows, 0.25);

            columns = new List<ContentColumn>();
            columns.Add(new ContentColumn(12.5, new MigraDoc.DocumentObjectModel.Color(252, 207, 101)));
            reports.AddTable(columns, true);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("PART II. OTHER ITEMS NOT AVALABLE AT PS BUT REGULARLY PURCHASED FROM OTHER SOURCES (Note: Please indicate price of items)", 0, 7, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Left, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center, 0, 0));
            reports.AddRowContent(rows, 0.25);

            categories = ppmpVM.NonDBMItems.GroupBy(d => d.Category).Select(d => d.Key).ToList();
            if (categories.Count == 0)
            {
                columns = new List<ContentColumn>();
                columns.Add(new ContentColumn(12.5));
                reports.AddTable(columns, true);

                rows = new List<ContentCell>();
                rows.Add(new ContentCell("*** NO ITEMS ***", 0, 8.5, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                reports.AddRowContent(rows, 0.25);
            }

            foreach (var category in categories)
            {
                columns = new List<ContentColumn>();
                columns.Add(new ContentColumn(12.5, new MigraDoc.DocumentObjectModel.Color(74, 132, 249)));
                reports.AddTable(columns, true);

                rows = new List<ContentCell>();
                rows.Add(new ContentCell(category, 0, 7, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Left, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center, 0, 0));
                reports.AddRowContent(rows, 0.25);

                foreach (var item in ppmpVM.NonDBMItems.Where(d => d.Category == category).OrderBy(d => d.ProjectCode).ThenBy(d => d.ItemName))
                {
                    columns = new List<ContentColumn>();
                    columns.Add(new ContentColumn(1.25)); //PAP Code
                    columns.Add(new ContentColumn(1.75)); //Item
                    columns.Add(new ContentColumn(0.75)); //Qty/size
                    columns.Add(new ContentColumn(1.00)); //Estimated Budget
                    columns.Add(new ContentColumn(0.50));
                    columns.Add(new ContentColumn(0.50));
                    columns.Add(new ContentColumn(0.50));
                    columns.Add(new ContentColumn(0.50));
                    columns.Add(new ContentColumn(0.50));
                    columns.Add(new ContentColumn(0.50));
                    columns.Add(new ContentColumn(0.50));
                    columns.Add(new ContentColumn(0.50));
                    columns.Add(new ContentColumn(0.50));
                    columns.Add(new ContentColumn(0.50));
                    columns.Add(new ContentColumn(0.50));
                    columns.Add(new ContentColumn(0.50));
                    columns.Add(new ContentColumn(1.75)); //Remarks
                    reports.AddTable(columns, true);

                    rows = new List<ContentCell>();
                    rows.Add(new ContentCell(item.ProjectCode, 0, 7, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                    rows.Add(new ContentCell(item.ItemName + "\n\n" + item.ItemSpecifications, 1, 7, true, false, ParagraphAlignment.Justify, VerticalAlignment.Center));
                    rows.Add(new ContentCell(item.TotalQty + (item.IndividualUOMReference == null ? "" : "\n" + item.IndividualUOMReference), 2, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                    rows.Add(new ContentCell(String.Format("{0:#,##0.00}", item.EstimatedBudget), 3, 7, false, false, ParagraphAlignment.Right, VerticalAlignment.Center));
                    rows.Add(new ContentCell(item.JanMilestone, 4, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                    rows.Add(new ContentCell(item.FebMilestone, 5, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                    rows.Add(new ContentCell(item.MarMilestone, 6, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                    rows.Add(new ContentCell(item.AprMilestone, 7, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                    rows.Add(new ContentCell(item.MayMilestone, 8, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                    rows.Add(new ContentCell(item.JunMilestone, 9, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                    rows.Add(new ContentCell(item.JulMilestone, 10, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                    rows.Add(new ContentCell(item.AugMilestone, 11, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                    rows.Add(new ContentCell(item.SepMilestone, 12, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                    rows.Add(new ContentCell(item.OctMilestone, 13, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                    rows.Add(new ContentCell(item.NovMilestone, 14, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                    rows.Add(new ContentCell(item.DecMilestone, 15, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                    rows.Add(new ContentCell(item.Remarks + "; " + item.Project + (item.ResponsibilityCenter == null ? "" : item.ResponsibilityCenter == ppmpVM.Header.Department ? "" : "\n\nRESPONSIBILITY CENTER: " + item.ResponsibilityCenter), 16, 7, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top));

                    reports.AddRowContent(rows, 0.25);
                }
            }

            columns = new List<ContentColumn>();
            columns.Add(new ContentColumn(3.75));
            columns.Add(new ContentColumn(1.00));
            columns.Add(new ContentColumn(7.75));
            reports.AddTable(columns, true);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("SUB-TOTAL: ", 0, 8.5, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Right, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
            rows.Add(new ContentCell(String.Format("{0:C}", ppmpVM.NonDBMItems.Sum(d => d.EstimatedBudget)), 1, 8.5, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Right, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
            rows.Add(new ContentCell("", 2));
            reports.AddRowContent(rows, 0.25);

            columns = new List<ContentColumn>();
            columns.Add(new ContentColumn(3.75));
            columns.Add(new ContentColumn(1.00));
            columns.Add(new ContentColumn(7.75));
            reports.AddTable(columns, true);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("TOTAL BUDGET: ", 0, 8.5, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Right, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
            rows.Add(new ContentCell(String.Format("{0:C}", (ppmpVM.DBMItems.Sum(d => d.EstimatedBudget) + ppmpVM.NonDBMItems.Sum(d => d.EstimatedBudget))), 1, 8.5, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Right, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
            rows.Add(new ContentCell("", 2));
            reports.AddRowContent(rows, 0.25);

            reports.AddNewLine();
            reports.AddSingleColumnHeader();
            reports.AddColumnHeader(
                new HeaderLine { Content = "NOTE: \n", Bold = false, Italic = false, FontSize = 7, ParagraphAlignment = ParagraphAlignment.Left, LeftIndent = 25 }
            );
            reports.AddColumnHeader(
                new HeaderLine { Content = "1. Technical Specifications for each Item/Project being proposed shall  be submitted as part of the PPMP.", Bold = false, Italic = false, FontSize = 7, ParagraphAlignment = ParagraphAlignment.Left, LeftIndent = 35 }
            );
            reports.AddColumnHeader(
                new HeaderLine { Content = "2.  Technical Specifications however,  must be in generic form;  no brand name shall be specified.", Bold = false, Italic = false, FontSize = 7, ParagraphAlignment = ParagraphAlignment.Left, LeftIndent = 35 }
            );
            reports.AddColumnHeader(
                new HeaderLine { Content = "3.  Non-submission of PPMP for supplies shall mean no budget provision for supplies.", Bold = false, Italic = false, FontSize = 7, ParagraphAlignment = ParagraphAlignment.Left, LeftIndent = 35 }
            );

            reports.AddNewLine();

            columns = new List<ContentColumn>();
            columns.Add(new ContentColumn(1.00));
            columns.Add(new ContentColumn(4.00));
            columns.Add(new ContentColumn(2.00));
            columns.Add(new ContentColumn(4.00));
            columns.Add(new ContentColumn(1.50));
            reports.AddTable(columns, false);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("", 0));
            rows.Add(new ContentCell("Prepared By:", 1, 8.5, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Left, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
            rows.Add(new ContentCell("", 2));
            rows.Add(new ContentCell("Submitted By:", 3, 8.5, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Left, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
            rows.Add(new ContentCell("", 4));
            reports.AddRowContent(rows, 0.25);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("\n\n", 0));
            rows.Add(new ContentCell("\n\n", 1, 8.5, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center, 0, 0, true));
            rows.Add(new ContentCell("\n\n", 2));
            rows.Add(new ContentCell("\n\n", 3, 8.5, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center, 0, 0, true));
            rows.Add(new ContentCell("\n\n", 4));
            reports.AddRowContent(rows, 0.4);

            var preparedBy = ppmpVM.Header.PreparedBy.Split(',');
            var submittedBy = ppmpVM.Header.SubmittedBy.Split(',');

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("", 0));
            rows.Add(new ContentCell(preparedBy[0], 1, 8.5, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
            rows.Add(new ContentCell("", 2));
            rows.Add(new ContentCell(submittedBy[0], 3, 8.5, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
            rows.Add(new ContentCell("", 4));
            reports.AddRowContent(rows, 0);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("", 0));
            rows.Add(new ContentCell(preparedBy[1], 1, 8.5, false, true, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
            rows.Add(new ContentCell("", 2));
            rows.Add(new ContentCell(submittedBy[1], 3, 8.5, false, true, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
            rows.Add(new ContentCell("", 4));
            reports.AddRowContent(rows, 0);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("", 0));
            rows.Add(new ContentCell(ppmpVM.Header.CreatedAt.ToString("dd MMMM yyyy hh:mm tt"), 1, 7, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
            rows.Add(new ContentCell("", 2));
            rows.Add(new ContentCell((ppmpVM.Header.SubmittedAt == null ? "(Submission Pending)" : ((DateTime)ppmpVM.Header.SubmittedAt).ToString("dd MMMM yyyy hh:mm tt")), 3, 7, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
            rows.Add(new ContentCell("", 4));
            reports.AddRowContent(rows, 0);

            if(ppmpVM.NonDBMItems.Count != 0)
            {
                var projects = ppmpVM.NonDBMItems.GroupBy(d => new { d.Project, d.ProjectCode }).Select(d => new { d.Key.Project, d.Key.ProjectCode }).ToList();
                foreach(var project in projects)
                {
                    reports.CreateDocument();
                    reports.AddDoubleColumnHeader(LogoPath);
                    reports.AddColumnHeader(
                        new HeaderLine { Content = "", Bold = false, Italic = false, FontSize = 3, ParagraphAlignment = ParagraphAlignment.Left },
                        new HeaderLine { Content = "", Bold = true, Italic = false, FontSize = 3, ParagraphAlignment = ParagraphAlignment.Left }
                    );
                    reports.AddColumnHeader(
                        new HeaderLine { Content = "Republic of the Philippines", Bold = false, Italic = false, FontSize = 7, ParagraphAlignment = ParagraphAlignment.Left },
                        new HeaderLine { Content = "PUP-MASU-6-PRMO-004", Bold = false, Italic = false, FontSize = 7, ParagraphAlignment = ParagraphAlignment.Left, LeftIndent = 120 }
                    );
                    reports.AddColumnHeader(
                        new HeaderLine { Content = "POLYTECHNIC UNIVERSITY OF THE PHILIPPINES", Bold = true, Italic = false, FontSize = 10, ParagraphAlignment = ParagraphAlignment.Left },
                        new HeaderLine { Content = "Rev. 0", Bold = false, Italic = false, FontSize = 7, ParagraphAlignment = ParagraphAlignment.Left, LeftIndent = 120 }
                    );
                    reports.AddColumnHeader(
                        new HeaderLine { Content = ppmpVM.Header.Sector, Bold = false, Italic = false, FontSize = 9, ParagraphAlignment = ParagraphAlignment.Left },
                        new HeaderLine { Content = "May 15, 2018", Bold = false, Italic = false, FontSize = 7, ParagraphAlignment = ParagraphAlignment.Left, LeftIndent = 120 }
                    );
                    reports.AddColumnHeader(
                        new HeaderLine { Content = ppmpVM.Header.Department.ToUpper(), Bold = true, Italic = false, FontSize = 10, ParagraphAlignment = ParagraphAlignment.Left },
                        new HeaderLine { Content = "", Bold = true, Italic = false, FontSize = 7, ParagraphAlignment = ParagraphAlignment.Left, LeftIndent = 120 }
                    );

                    reports.AddNewLine();

                    reports.AddSingleColumnHeader();
                    reports.AddColumnHeader(
                        new HeaderLine { Content = "MARKET SURVEY", Bold = true, Italic = false, FontSize = 12, ParagraphAlignment = ParagraphAlignment.Center }
                    );

                    reports.AddNewLine();
                    reports.AddNewLine();

                    columns = new List<ContentColumn>();
                    columns.Add(new ContentColumn(1.00));
                    columns.Add(new ContentColumn(8.50));
                    columns.Add(new ContentColumn(3.00));
                    reports.AddTable(columns, false);

                    rows = new List<ContentCell>();
                    rows.Add(new ContentCell("End-User: ", 0, 9, true));
                    rows.Add(new ContentCell(ppmpVM.Header.Department, 1, 9, true, true, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, true));
                    rows.Add(new ContentCell("", 2));
                    reports.AddRowContent(rows, 0);

                    rows = new List<ContentCell>();
                    rows.Add(new ContentCell("Project Title: ", 0, 9, true, false, ParagraphAlignment.Left, VerticalAlignment.Bottom));
                    rows.Add(new ContentCell(project.Project + " (" + project.ProjectCode + ")", 1, 9, true, true, ParagraphAlignment.Left, VerticalAlignment.Bottom, 0, 0, true));
                    rows.Add(new ContentCell("", 2));
                    reports.AddRowContent(rows, 0.2);
                    reports.AddNewLine();

                    columns = new List<ContentColumn>();
                    columns.Add(new ContentColumn(0.75, new MigraDoc.DocumentObjectModel.Color(254, 159, 89))); //Item No.
                    columns.Add(new ContentColumn(3.625, new MigraDoc.DocumentObjectModel.Color(254, 159, 89))); //Item Description
                    columns.Add(new ContentColumn(1, new MigraDoc.DocumentObjectModel.Color(254, 159, 89))); //Qty
                    columns.Add(new ContentColumn(0.625, new MigraDoc.DocumentObjectModel.Color(254, 159, 89))); //Supplier No.
                    columns.Add(new ContentColumn(3.5, new MigraDoc.DocumentObjectModel.Color(254, 159, 89))); //Supplier Details
                    columns.Add(new ContentColumn(0.75, new MigraDoc.DocumentObjectModel.Color(254, 159, 89))); //Unit Cost
                    columns.Add(new ContentColumn(0.75, new MigraDoc.DocumentObjectModel.Color(254, 159, 89))); //Total Cost
                    columns.Add(new ContentColumn(1.5, new MigraDoc.DocumentObjectModel.Color(254, 159, 89))); //Average Unit Cost
                    reports.AddTable(columns, true);

                    rows = new List<ContentCell>();
                    rows.Add(new ContentCell("Item No.", 0, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 1));
                    rows.Add(new ContentCell("Item Description", 1, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 1));
                    rows.Add(new ContentCell("Qty. / Unit / \nNo. of Copies", 2, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 1));
                    rows.Add(new ContentCell("Supplier Details", 3, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center, 3));
                    rows.Add(new ContentCell("Average Unit Cost", 7, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 1));
                    reports.AddRowContent(rows, 0.3);

                    rows = new List<ContentCell>();
                    rows.Add(new ContentCell("Supplier", 3, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center, 1));
                    rows.Add(new ContentCell("Unit Cost", 5, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                    rows.Add(new ContentCell("Total Cost", 6, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                    reports.AddRowContent(rows, 0.3);

                    int itemNo = 1;
                    foreach (var item in ppmpVM.NonDBMItems.Where(d => d.ProjectCode == project.ProjectCode).OrderBy(d => d.ProjectCode).ThenBy(d => d.ItemName).ToList())
                    {
                        columns = new List<ContentColumn>();
                        columns.Add(new ContentColumn(0.75)); //Item No.
                        columns.Add(new ContentColumn(3.625)); //Item Description
                        columns.Add(new ContentColumn(1)); //Qty
                        columns.Add(new ContentColumn(0.625)); //Supplier No.
                        columns.Add(new ContentColumn(3.5)); //Supplier Details
                        columns.Add(new ContentColumn(0.75)); //Unit Cost
                        columns.Add(new ContentColumn(0.75)); //Total Cost
                        columns.Add(new ContentColumn(1.5)); //Average Unit Cost
                        reports.AddTable(columns, true);

                        rows = new List<ContentCell>();
                        rows.Add(new ContentCell(itemNo.ToString(), 0, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 2));
                        rows.Add(new ContentCell(item.ItemName + "\n\n" + item.ItemSpecifications, 1, 8, true, false, ParagraphAlignment.Justify, VerticalAlignment.Center, 0, 2));
                        rows.Add(new ContentCell(item.TotalQty + (item.IndividualUOMReference == null ? "" : "\n" + item.IndividualUOMReference), 2, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 2));
                        rows.Add(new ContentCell("1", 3, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                        rows.Add(new ContentCell("Name: " + item.Supplier1Name + "\nAddress: " + item.Supplier1Address + "\nContact No.: " + item.Supplier1ContactNo + "\nEmail Address: " + item.Supplier1EmailAddress, 4, 8, true, false, ParagraphAlignment.Left, VerticalAlignment.Center));
                        rows.Add(new ContentCell(String.Format("{0:C}", item.Supplier1UnitCost), 5, 8, true, false, ParagraphAlignment.Right, VerticalAlignment.Center));
                        rows.Add(new ContentCell(String.Format("{0:C}", ((decimal)item.Supplier1UnitCost * int.Parse(item.TotalQty))), 6, 8, true, false, ParagraphAlignment.Right, VerticalAlignment.Center));
                        rows.Add(new ContentCell(String.Format("{0:C}", item.EstimatedBudget), 7, 8, true, false, ParagraphAlignment.Right, VerticalAlignment.Center, 0, 2));
                        reports.AddRowContent(rows, 0.25);


                        rows = new List<ContentCell>();
                        rows.Add(new ContentCell("2", 3, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                        rows.Add(new ContentCell("Name: " + (item.Supplier2Name == null ? "N/A" : item.Supplier2Name) + "\nAddress: " + (item.Supplier2Name == null ? "N/A" : item.Supplier2Address) + "\nContact No.: " + (item.Supplier2Name == null ? "N/A" : item.Supplier2ContactNo) + "\nEmail Address: " + (item.Supplier2Name == null ? "N/A" : item.Supplier2EmailAddress), 4, 8, true, false, ParagraphAlignment.Left, VerticalAlignment.Center));
                        rows.Add(new ContentCell((item.Supplier2Name == null ? (0.00m).ToString("C", new System.Globalization.CultureInfo("en-ph")) : ((decimal)(item.Supplier2UnitCost)).ToString("C", new System.Globalization.CultureInfo("en-ph"))), 5, 8, true, false, ParagraphAlignment.Right, VerticalAlignment.Center));
                        rows.Add(new ContentCell((item.Supplier2Name == null ? (0.00m).ToString("C", new System.Globalization.CultureInfo("en-ph")) : ((decimal)item.Supplier2UnitCost * int.Parse(item.TotalQty)).ToString("C", new System.Globalization.CultureInfo("en-ph"))), 6, 8, true, false, ParagraphAlignment.Right, VerticalAlignment.Center));
                        reports.AddRowContent(rows, 0.25);

                        rows = new List<ContentCell>();
                        rows.Add(new ContentCell("3", 3, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                        rows.Add(new ContentCell("Name: " + (item.Supplier3Name == null ? "N/A" : item.Supplier3Name) + "\nAddress: " + (item.Supplier3Name == null ? "N/A" : item.Supplier3Address) + "\nContact No.: " + (item.Supplier3Name == null ? "N/A" : item.Supplier3ContactNo) + "\nEmail Address: " + (item.Supplier3Name == null ? "N/A" : item.Supplier3EmailAddress), 4, 8, true, false, ParagraphAlignment.Left, VerticalAlignment.Center));
                        rows.Add(new ContentCell((item.Supplier3Name == null ? (0.00m).ToString("C", new System.Globalization.CultureInfo("en-ph")) : ((decimal)(item.Supplier3UnitCost)).ToString("C", new System.Globalization.CultureInfo("en-ph"))), 5, 8, true, false, ParagraphAlignment.Right, VerticalAlignment.Center));
                        rows.Add(new ContentCell((item.Supplier3Name == null ? (0.00m).ToString("C", new System.Globalization.CultureInfo("en-ph")) : ((decimal)item.Supplier3UnitCost * int.Parse(item.TotalQty)).ToString("C", new System.Globalization.CultureInfo("en-ph"))), 6, 8, true, false, ParagraphAlignment.Right, VerticalAlignment.Center));
                        reports.AddRowContent(rows, 0.25);

                        itemNo++;
                    }

                    reports.AddNewLine();
                    reports.AddNewLine();

                    columns = new List<ContentColumn>();
                    columns.Add(new ContentColumn(1.00));
                    columns.Add(new ContentColumn(4.00));
                    columns.Add(new ContentColumn(2.00));
                    columns.Add(new ContentColumn(4.00));
                    columns.Add(new ContentColumn(1.50));
                    reports.AddTable(columns, false);

                    rows = new List<ContentCell>();
                    rows.Add(new ContentCell("", 0));
                    rows.Add(new ContentCell("Prepared By:", 1, 8.5, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Left, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                    rows.Add(new ContentCell("", 2));
                    rows.Add(new ContentCell("Approved By:", 3, 8.5, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Left, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                    rows.Add(new ContentCell("", 4));
                    reports.AddRowContent(rows, 0.25);

                    rows = new List<ContentCell>();
                    rows.Add(new ContentCell("\n\n", 0));
                    rows.Add(new ContentCell("\n\n", 1, 8.5, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center, 0, 0, true));
                    rows.Add(new ContentCell("\n\n", 2));
                    rows.Add(new ContentCell("\n\n", 3, 8.5, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center, 0, 0, true));
                    rows.Add(new ContentCell("\n\n", 4));
                    reports.AddRowContent(rows, 0.4);

                    preparedBy = ppmpVM.Header.PreparedBy.Split(',');
                    submittedBy = ppmpVM.Header.SubmittedBy.Split(',');

                    rows = new List<ContentCell>();
                    rows.Add(new ContentCell("", 0));
                    rows.Add(new ContentCell(preparedBy[0], 1, 8.5, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                    rows.Add(new ContentCell("", 2));
                    rows.Add(new ContentCell(submittedBy[0], 3, 8.5, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                    rows.Add(new ContentCell("", 4));
                    reports.AddRowContent(rows, 0);

                    rows = new List<ContentCell>();
                    rows.Add(new ContentCell("", 0));
                    rows.Add(new ContentCell(preparedBy[1], 1, 8.5, false, true, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                    rows.Add(new ContentCell("", 2));
                    rows.Add(new ContentCell(submittedBy[1], 3, 8.5, false, true, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                    rows.Add(new ContentCell("", 4));
                    reports.AddRowContent(rows, 0);

                    rows = new List<ContentCell>();
                    rows.Add(new ContentCell("", 0));
                    rows.Add(new ContentCell(ppmpVM.Header.CreatedAt.ToString("dd MMMM yyyy hh:mm tt"), 1, 7, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                    rows.Add(new ContentCell("", 2));
                    rows.Add(new ContentCell((ppmpVM.Header.SubmittedAt == null ? "(Submission Pending)" : ((DateTime)ppmpVM.Header.SubmittedAt).ToString("dd MMMM yyyy hh:mm tt")), 3, 7, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                    rows.Add(new ContentCell("", 4));
                    reports.AddRowContent(rows, 0);

                    reports.AddNewLine();
                    reports.AddNewLine();

                    reports.AddSingleColumnHeader();
                    reports.AddColumnHeader(
                        new HeaderLine { Content = "NOTE: \n", Bold = false, Italic = false, FontSize = 7, ParagraphAlignment = ParagraphAlignment.Left, LeftIndent = 25 }
                    );
                    reports.AddColumnHeader(
                        new HeaderLine { Content = "* The items must have same specifications.", Bold = false, Italic = false, FontSize = 7, ParagraphAlignment = ParagraphAlignment.Left, LeftIndent = 35 }
                    );
                    reports.AddColumnHeader(
                        new HeaderLine { Content = "* Quotations should include Taxes, Freights, Delivery, Installation Costs and other incidental costs (if applicable).", Bold = false, Italic = false, FontSize = 7, ParagraphAlignment = ParagraphAlignment.Left, LeftIndent = 35 }
                    );
                    reports.AddColumnHeader(
                        new HeaderLine { Content = "* There should be at least three (3) suppliers .", Bold = false, Italic = false, FontSize = 7, ParagraphAlignment = ParagraphAlignment.Left, LeftIndent = 35 }
                    );
                    reports.AddColumnHeader(
                        new HeaderLine { Content = "* Prices should be in Philippine Peso (Php) Currency.", Bold = false, Italic = false, FontSize = 7, ParagraphAlignment = ParagraphAlignment.Left, LeftIndent = 35 }
                    );
                    reports.AddColumnHeader(
                        new HeaderLine { Content = "* Quotation forms and/or brochures must be attached as proof.", Bold = false, Italic = false, FontSize = 7, ParagraphAlignment = ParagraphAlignment.Left, LeftIndent = 35 }
                    );
                }
            }

            var responsibilityCenterItems = ppmpDAL.GetPPMPResponsibilityCenterItems(ppmpVM.Header.Department, ppmpVM.Header.PPMPType);
            if (responsibilityCenterItems.Count != 0)
            {
                reports.CreateDocument();
                reports.AddDoubleColumnHeader(LogoPath);
                reports.AddColumnHeader(
                    new HeaderLine { Content = "", Bold = false, Italic = false, FontSize = 5, ParagraphAlignment = ParagraphAlignment.Left },
                    new HeaderLine { Content = "", Bold = true, Italic = false, FontSize = 5, ParagraphAlignment = ParagraphAlignment.Left }
                );
                reports.AddColumnHeader(
                    new HeaderLine { Content = "Republic of the Philippines", Bold = false, Italic = false, FontSize = 10, ParagraphAlignment = ParagraphAlignment.Left },
                    new HeaderLine { Content = "BP FORM " + Convert.ToChar(66 + inventoryID).ToString(), Bold = true, Italic = false, FontSize = 8, ParagraphAlignment = ParagraphAlignment.Left }
                );
                reports.AddColumnHeader(
                    new HeaderLine { Content = "POLYTECHNIC UNIVERSITY OF THE PHILIPPINES", Bold = true, Italic = false, FontSize = 12, ParagraphAlignment = ParagraphAlignment.Left },
                    new HeaderLine { Content = "Date Printed: " + DateTime.Now.ToString("dd MMMM yyyy hh:mm tt"), Bold = false, Italic = false, FontSize = 8, ParagraphAlignment = ParagraphAlignment.Left }
                );
                reports.AddColumnHeader(
                    new HeaderLine { Content = "Anonas St., Sta. Mesa, Manila", Bold = false, Italic = false, FontSize = 8, ParagraphAlignment = ParagraphAlignment.Left },
                    new HeaderLine { Content = "", Bold = false, Italic = false, FontSize = 8, ParagraphAlignment = ParagraphAlignment.Left }
                );

                reports.AddNewLine();
                reports.AddNewLine();

                reports.AddSingleColumnHeader();
                reports.AddColumnHeader(
                    new HeaderLine { Content = "PROJECT PROCUREMENT MANAGEMENT PLAN (PPMP)", Bold = false, Italic = false, FontSize = 10, ParagraphAlignment = ParagraphAlignment.Center }
                );
                reports.AddColumnHeader(
                    new HeaderLine { Content = ppmpVM.Header.PPMPType + " (Institutional)", Bold = true, Italic = false, FontSize = 10, ParagraphAlignment = ParagraphAlignment.Center }
                );
                reports.AddColumnHeader(
                    new HeaderLine { Content = "Fiscal Year " + ppmpVM.Header.FiscalYear.ToString(), Bold = false, Italic = false, FontSize = 8, ParagraphAlignment = ParagraphAlignment.Center }
                );

                reports.AddNewLine();

                columns = new List<ContentColumn>();
                columns.Add(new ContentColumn(2.00));
                columns.Add(new ContentColumn(5.00));
                columns.Add(new ContentColumn(6.00));
                reports.AddTable(columns, false);

                rows = new List<ContentCell>();
                rows.Add(new ContentCell("Responsibility Center: ", 0, 10, true));
                rows.Add(new ContentCell(ppmpVM.Header.Department, 1, 10, true, true, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, true));
                rows.Add(new ContentCell("", 2));
                reports.AddRowContent(rows, 0);

                reports.AddNewLine();

                columns = new List<ContentColumn>();
                columns.Add(new ContentColumn(1.16, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
                columns.Add(new ContentColumn(0.75, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
                columns.Add(new ContentColumn(0.50, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
                columns.Add(new ContentColumn(0.50, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
                columns.Add(new ContentColumn(0.50, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
                columns.Add(new ContentColumn(0.50, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
                columns.Add(new ContentColumn(0.50, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
                columns.Add(new ContentColumn(0.50, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
                columns.Add(new ContentColumn(0.50, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
                columns.Add(new ContentColumn(0.50, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
                columns.Add(new ContentColumn(0.50, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
                columns.Add(new ContentColumn(0.50, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
                columns.Add(new ContentColumn(0.50, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
                columns.Add(new ContentColumn(0.50, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
                columns.Add(new ContentColumn(0.75, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
                columns.Add(new ContentColumn(0.75, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
                columns.Add(new ContentColumn(1.00, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
                columns.Add(new ContentColumn(2.09, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
                reports.AddTable(columns, true);

                rows = new List<ContentCell>();
                rows.Add(new ContentCell("Item and\nSpecifications", 0, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 1));
                rows.Add(new ContentCell("Unit of\nMeasure", 1, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 1));
                rows.Add(new ContentCell("Monthly Quantity Requirement", 2, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center, 11, 0));
                rows.Add(new ContentCell("Total\nQuantity", 14, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 1));
                rows.Add(new ContentCell("Unit\nCost", 15, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 1));
                rows.Add(new ContentCell("Estimated\nBudget", 16, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 1));
                rows.Add(new ContentCell("Remarks", 17, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 1));
                reports.AddRowContent(rows, 0.25);

                rows = new List<ContentCell>();
                rows.Add(new ContentCell("Jan", 2, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                rows.Add(new ContentCell("Feb", 3, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                rows.Add(new ContentCell("Mar", 4, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                rows.Add(new ContentCell("Apr", 5, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                rows.Add(new ContentCell("May", 6, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                rows.Add(new ContentCell("Jun", 7, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                rows.Add(new ContentCell("Jul", 8, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                rows.Add(new ContentCell("Aug", 9, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                rows.Add(new ContentCell("Sep", 10, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                rows.Add(new ContentCell("Oct", 11, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                rows.Add(new ContentCell("Nov", 12, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                rows.Add(new ContentCell("Dec", 13, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                reports.AddRowContent(rows, 0.25);

                columns = new List<ContentColumn>();
                columns.Add(new ContentColumn(12.5, new MigraDoc.DocumentObjectModel.Color(252, 207, 101)));
                reports.AddTable(columns, true);

                rows = new List<ContentCell>();
                rows.Add(new ContentCell("PART I. AVAILABLE AT PROCUREMENT SERVICE STORES", 0, 7, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Left, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center, 0, 0));
                reports.AddRowContent(rows, 0.25);

                categories = responsibilityCenterItems.Where(d => d.ProcurementSource == ProcurementSources.PS_DBM).GroupBy(d => d.Category).Select(d => d.Key).ToList();
                if (categories.Count == 0)
                {
                    columns = new List<ContentColumn>();
                    columns.Add(new ContentColumn(12.5));
                    reports.AddTable(columns, true);

                    rows = new List<ContentCell>();
                    rows.Add(new ContentCell("*** NO ITEMS ***", 0, 8.5, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                    reports.AddRowContent(rows, 0.25);
                }
                foreach (var category in categories)
                {
                    columns = new List<ContentColumn>();
                    columns.Add(new ContentColumn(12.5, new MigraDoc.DocumentObjectModel.Color(74, 132, 249)));
                    reports.AddTable(columns, true);

                    rows = new List<ContentCell>();
                    rows.Add(new ContentCell(category, 0, 7, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Left, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center, 0, 0));
                    reports.AddRowContent(rows, 0.25);

                    foreach (var item in responsibilityCenterItems.Where(d => d.ProcurementSource == ProcurementSources.PS_DBM && d.Category == category).OrderBy(d => d.ItemName))
                    {
                        columns = new List<ContentColumn>();
                        columns.Add(new ContentColumn(1.16));
                        columns.Add(new ContentColumn(0.75));
                        columns.Add(new ContentColumn(0.50));
                        columns.Add(new ContentColumn(0.50));
                        columns.Add(new ContentColumn(0.50));
                        columns.Add(new ContentColumn(0.50));
                        columns.Add(new ContentColumn(0.50));
                        columns.Add(new ContentColumn(0.50));
                        columns.Add(new ContentColumn(0.50));
                        columns.Add(new ContentColumn(0.50));
                        columns.Add(new ContentColumn(0.50));
                        columns.Add(new ContentColumn(0.50));
                        columns.Add(new ContentColumn(0.50));
                        columns.Add(new ContentColumn(0.50));
                        columns.Add(new ContentColumn(0.75));
                        columns.Add(new ContentColumn(0.75));
                        columns.Add(new ContentColumn(1.00));
                        columns.Add(new ContentColumn(2.09));
                        reports.AddTable(columns, true);

                        rows = new List<ContentCell>();
                        rows.Add(new ContentCell(item.ItemName, 0, 7, true, false, ParagraphAlignment.Left, VerticalAlignment.Center));
                        rows.Add(new ContentCell(item.IndividualUOMReference, 1, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                        rows.Add(new ContentCell(item.JanMilestone, 2, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                        rows.Add(new ContentCell(item.FebMilestone, 3, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                        rows.Add(new ContentCell(item.MarMilestone, 4, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                        rows.Add(new ContentCell(item.AprMilestone, 5, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                        rows.Add(new ContentCell(item.MayMilestone, 6, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                        rows.Add(new ContentCell(item.JunMilestone, 7, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                        rows.Add(new ContentCell(item.JulMilestone, 8, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                        rows.Add(new ContentCell(item.AugMilestone, 9, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                        rows.Add(new ContentCell(item.SepMilestone, 10, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                        rows.Add(new ContentCell(item.OctMilestone, 11, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                        rows.Add(new ContentCell(item.NovMilestone, 12, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                        rows.Add(new ContentCell(item.DecMilestone, 13, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                        rows.Add(new ContentCell(item.TotalQty, 14, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                        rows.Add(new ContentCell(item.UnitCost, 15, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                        rows.Add(new ContentCell(String.Format("{0:#,##0.00}", item.EstimatedBudget), 16, 7, false, false, ParagraphAlignment.Right, VerticalAlignment.Center));
                        rows.Add(new ContentCell(item.Remarks, 17, 7, false, false, ParagraphAlignment.Left, VerticalAlignment.Center));

                        reports.AddRowContent(rows, 0.25);
                    }
                }//

                columns = new List<ContentColumn>();
                columns.Add(new ContentColumn(9.41));
                columns.Add(new ContentColumn(1.00));
                columns.Add(new ContentColumn(2.09));
                reports.AddTable(columns, true);

                rows = new List<ContentCell>();
                rows.Add(new ContentCell("SUB-TOTAL: ", 0, 8.5, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Right, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                rows.Add(new ContentCell(String.Format("{0:C}", responsibilityCenterItems.Where(d => d.ProcurementSource == ProcurementSources.PS_DBM).Sum(d => d.EstimatedBudget)), 1, 8.5, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Right, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                rows.Add(new ContentCell("", 2));
                reports.AddRowContent(rows, 0.25);

                columns = new List<ContentColumn>();
                columns.Add(new ContentColumn(1.16, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
                columns.Add(new ContentColumn(0.75, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
                columns.Add(new ContentColumn(0.50, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
                columns.Add(new ContentColumn(0.50, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
                columns.Add(new ContentColumn(0.50, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
                columns.Add(new ContentColumn(0.50, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
                columns.Add(new ContentColumn(0.50, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
                columns.Add(new ContentColumn(0.50, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
                columns.Add(new ContentColumn(0.50, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
                columns.Add(new ContentColumn(0.50, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
                columns.Add(new ContentColumn(0.50, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
                columns.Add(new ContentColumn(0.50, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
                columns.Add(new ContentColumn(0.50, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
                columns.Add(new ContentColumn(0.50, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
                columns.Add(new ContentColumn(0.75, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
                columns.Add(new ContentColumn(0.75, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
                columns.Add(new ContentColumn(1.00, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
                columns.Add(new ContentColumn(2.09, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
                reports.AddTable(columns, true);

                rows = new List<ContentCell>();
                rows.Add(new ContentCell("Item and\nSpecifications", 0, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 1));
                rows.Add(new ContentCell("Unit of\nMeasure", 1, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 1));
                rows.Add(new ContentCell("Monthly Quantity Requirement", 2, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center, 11, 0));
                rows.Add(new ContentCell("Total\nQuantity", 14, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 1));
                rows.Add(new ContentCell("Unit\nCost", 15, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 1));
                rows.Add(new ContentCell("Estimated\nBudget", 16, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 1));
                rows.Add(new ContentCell("Remarks", 17, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 1));
                reports.AddRowContent(rows, 0.25);

                rows = new List<ContentCell>();
                rows.Add(new ContentCell("Jan", 2, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                rows.Add(new ContentCell("Feb", 3, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                rows.Add(new ContentCell("Mar", 4, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                rows.Add(new ContentCell("Apr", 5, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                rows.Add(new ContentCell("May", 6, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                rows.Add(new ContentCell("Jun", 7, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                rows.Add(new ContentCell("Jul", 8, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                rows.Add(new ContentCell("Aug", 9, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                rows.Add(new ContentCell("Sep", 10, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                rows.Add(new ContentCell("Oct", 11, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                rows.Add(new ContentCell("Nov", 12, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                rows.Add(new ContentCell("Dec", 13, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                reports.AddRowContent(rows, 0.25);

                columns = new List<ContentColumn>();
                columns.Add(new ContentColumn(12.5, new MigraDoc.DocumentObjectModel.Color(252, 207, 101)));
                reports.AddTable(columns, true);

                rows = new List<ContentCell>();
                rows.Add(new ContentCell("PART II. OTHER ITEMS NOT AVALABLE AT PS BUT REGULARLY PURCHASED FROM OTHER SOURCES (Note: Please indicate price of items)", 0, 7, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Left, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center, 0, 0));
                reports.AddRowContent(rows, 0.25);

                categories = responsibilityCenterItems.Where(d => d.ProcurementSource == ProcurementSources.Non_DBM).GroupBy(d => d.Category).Select(d => d.Key).ToList();
                if (categories.Count == 0)
                {
                    columns = new List<ContentColumn>();
                    columns.Add(new ContentColumn(12.5));
                    reports.AddTable(columns, true);

                    rows = new List<ContentCell>();
                    rows.Add(new ContentCell("*** NO ITEMS ***", 0, 8.5, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                    reports.AddRowContent(rows, 0.25);
                }
                foreach (var category in categories)
                {
                    columns = new List<ContentColumn>();
                    columns.Add(new ContentColumn(12.5, new MigraDoc.DocumentObjectModel.Color(74, 132, 249)));
                    reports.AddTable(columns, true);

                    rows = new List<ContentCell>();
                    rows.Add(new ContentCell(category, 0, 7, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Left, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center, 0, 0));
                    reports.AddRowContent(rows, 0.25);

                    foreach (var item in responsibilityCenterItems.Where(d => d.ProcurementSource == ProcurementSources.Non_DBM && d.Category == category).OrderBy(d => d.ProjectCode).ThenBy(d => d.ItemName))
                    {
                        columns = new List<ContentColumn>();
                        columns.Add(new ContentColumn(1.16));
                        columns.Add(new ContentColumn(0.75));
                        columns.Add(new ContentColumn(0.50));
                        columns.Add(new ContentColumn(0.50));
                        columns.Add(new ContentColumn(0.50));
                        columns.Add(new ContentColumn(0.50));
                        columns.Add(new ContentColumn(0.50));
                        columns.Add(new ContentColumn(0.50));
                        columns.Add(new ContentColumn(0.50));
                        columns.Add(new ContentColumn(0.50));
                        columns.Add(new ContentColumn(0.50));
                        columns.Add(new ContentColumn(0.50));
                        columns.Add(new ContentColumn(0.50));
                        columns.Add(new ContentColumn(0.50));
                        columns.Add(new ContentColumn(0.75));
                        columns.Add(new ContentColumn(0.75));
                        columns.Add(new ContentColumn(1.00));
                        columns.Add(new ContentColumn(2.09));
                        reports.AddTable(columns, true);

                        rows = new List<ContentCell>();
                        rows.Add(new ContentCell(item.ItemName, 0, 7, true, false, ParagraphAlignment.Left, VerticalAlignment.Center));
                        rows.Add(new ContentCell(item.IndividualUOMReference, 1, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                        rows.Add(new ContentCell(item.JanMilestone, 2, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                        rows.Add(new ContentCell(item.FebMilestone, 3, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                        rows.Add(new ContentCell(item.MarMilestone, 4, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                        rows.Add(new ContentCell(item.AprMilestone, 5, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                        rows.Add(new ContentCell(item.MayMilestone, 6, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                        rows.Add(new ContentCell(item.JunMilestone, 7, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                        rows.Add(new ContentCell(item.JulMilestone, 8, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                        rows.Add(new ContentCell(item.AugMilestone, 9, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                        rows.Add(new ContentCell(item.SepMilestone, 10, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                        rows.Add(new ContentCell(item.OctMilestone, 11, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                        rows.Add(new ContentCell(item.NovMilestone, 12, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                        rows.Add(new ContentCell(item.DecMilestone, 13, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                        rows.Add(new ContentCell(item.TotalQty, 14, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                        rows.Add(new ContentCell(item.UnitCost, 15, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                        rows.Add(new ContentCell(String.Format("{0:#,##0.00}", item.EstimatedBudget), 16, 7, false, false, ParagraphAlignment.Right, VerticalAlignment.Center));
                        rows.Add(new ContentCell(item.Remarks, 17, 7, false, false, ParagraphAlignment.Left, VerticalAlignment.Center));
                        reports.AddRowContent(rows, 0.25);
                    }
                }

                columns = new List<ContentColumn>();
                columns.Add(new ContentColumn(9.41));
                columns.Add(new ContentColumn(1.00));
                columns.Add(new ContentColumn(2.09));
                reports.AddTable(columns, true);

                rows = new List<ContentCell>();
                rows.Add(new ContentCell("SUB-TOTAL: ", 0, 8.5, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Right, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                rows.Add(new ContentCell(String.Format("{0:C}", responsibilityCenterItems.Where(d => d.ProcurementSource == ProcurementSources.Non_DBM).Sum(d => d.EstimatedBudget)), 1, 8.5, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Right, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                rows.Add(new ContentCell("", 2));
                reports.AddRowContent(rows, 0.25);

                columns = new List<ContentColumn>();
                columns.Add(new ContentColumn(9.41));
                columns.Add(new ContentColumn(1.00));
                columns.Add(new ContentColumn(2.09));
                reports.AddTable(columns, true);

                rows = new List<ContentCell>();
                rows.Add(new ContentCell("TOTAL ESTIMATED BUDGET: ", 0, 8.5, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Right, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                rows.Add(new ContentCell(String.Format("{0:C}", (responsibilityCenterItems.Where(d => d.ProcurementSource == ProcurementSources.PS_DBM).Sum(d => d.EstimatedBudget) + responsibilityCenterItems.Where(d => d.ProcurementSource == ProcurementSources.Non_DBM).Sum(d => d.EstimatedBudget))), 1, 8.5, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Right, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                rows.Add(new ContentCell("", 2));
                reports.AddRowContent(rows, 0.25);

                reports.AddNewLine();
                reports.AddNewLine();

                columns = new List<ContentColumn>();
                columns.Add(new ContentColumn(1.50));
                columns.Add(new ContentColumn(4.00));
                columns.Add(new ContentColumn(1.50));
                columns.Add(new ContentColumn(4.00));
                columns.Add(new ContentColumn(1.50));
                reports.AddTable(columns, false);


                rows = new List<ContentCell>();
                rows.Add(new ContentCell("", 0));
                rows.Add(new ContentCell("Prepared By:", 1, 8.5, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Left, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                rows.Add(new ContentCell("", 2));
                rows.Add(new ContentCell("Submitted By:", 3, 8.5, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Left, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                rows.Add(new ContentCell("", 4));
                reports.AddRowContent(rows, 0.25);

                rows = new List<ContentCell>();
                rows.Add(new ContentCell("\n\n", 0));
                rows.Add(new ContentCell("\n\n", 1, 8.5, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center, 0, 0, true));
                rows.Add(new ContentCell("\n\n", 2));
                rows.Add(new ContentCell("\n\n", 3, 8.5, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center, 0, 0, true));
                rows.Add(new ContentCell("\n\n", 4));
                reports.AddRowContent(rows, 0.75);

                rows = new List<ContentCell>();
                rows.Add(new ContentCell("", 0));
                rows.Add(new ContentCell(preparedBy[0], 1, 8.5, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                rows.Add(new ContentCell("", 2));
                rows.Add(new ContentCell(submittedBy[0], 3, 8.5, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                rows.Add(new ContentCell("", 4));
                reports.AddRowContent(rows, 0);

                rows = new List<ContentCell>();
                rows.Add(new ContentCell("", 0));
                rows.Add(new ContentCell(preparedBy[1], 1, 8.5, false, true, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                rows.Add(new ContentCell("", 2));
                rows.Add(new ContentCell(submittedBy[1], 3, 8.5, false, true, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                rows.Add(new ContentCell("", 4));
                reports.AddRowContent(rows, 0);

                rows = new List<ContentCell>();
                rows.Add(new ContentCell("", 0));
                rows.Add(new ContentCell(ppmpVM.Header.CreatedAt.ToString("dd MMMM yyyy hh:mm tt"), 1, 7, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                rows.Add(new ContentCell("", 2));
                rows.Add(new ContentCell((ppmpVM.Header.SubmittedAt == null ? "(Submission Pending)" : ((DateTime)ppmpVM.Header.SubmittedAt).ToString("dd MMMM yyyy hh:mm tt")), 3, 7, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                rows.Add(new ContentCell("", 4));
                reports.AddRowContent(rows, 0);
            }

            return reports.GenerateReport();
        }
        public MemoryStream GenerateBudgetProposalReport(string LogoPath, string UserEmail, int FiscalYear)
        {
            Reports reports = new Reports();
            BudgetPropsalVM BudgetProposal = ppmpDAL.GetBudgetProposalDetails(UserEmail, FiscalYear);
            
            if(BudgetProposal.MOOE.Count > 0)
            {
                reports.ReportFilename = "BP Form A - " + BudgetProposal.OfficeName + "-" + FiscalYear.ToString();
                reports.CreateDocument();
                reports.AddDoubleColumnHeader(LogoPath);
                reports.AddColumnHeader(
                    new HeaderLine { Content = "", Bold = false, Italic = false, FontSize = 5, ParagraphAlignment = ParagraphAlignment.Left },
                    new HeaderLine { Content = "PUP-MOOE-7-BUSO-001", Bold = false, Italic = false, FontSize = 8, ParagraphAlignment = ParagraphAlignment.Left, LeftIndent = 80 }
                );
                reports.AddColumnHeader(
                    new HeaderLine { Content = "Republic of the Philippines", Bold = false, Italic = false, FontSize = 10, ParagraphAlignment = ParagraphAlignment.Left },
                    new HeaderLine { Content = "Rev.0", Bold = false, Italic = false, FontSize = 8, ParagraphAlignment = ParagraphAlignment.Left, LeftIndent = 80 }
                );
                reports.AddColumnHeader(
                    new HeaderLine { Content = "POLYTECHNIC UNIVERSITY OF THE PHILIPPINES", Bold = true, Italic = false, FontSize = 12, ParagraphAlignment = ParagraphAlignment.Left },
                    new HeaderLine { Content = "May 15, 2018", Bold = false, Italic = false, FontSize = 8, ParagraphAlignment = ParagraphAlignment.Left, LeftIndent = 80 }
                );
                reports.AddColumnHeader(
                    new HeaderLine { Content = "Anonas St., Sta. Mesa, Manila", Bold = false, Italic = false, FontSize = 9, ParagraphAlignment = ParagraphAlignment.Left },
                    new HeaderLine { Content = "Date Printed: " + DateTime.Now.ToString("dd MMMM yyyy hh:mm tt"), Bold = true, Italic = false, FontSize = 9, ParagraphAlignment = ParagraphAlignment.Left, LeftIndent = 80 }
                );

                reports.AddNewLine();
                reports.AddNewLine();

                reports.AddSingleColumnHeader();
                reports.AddColumnHeader(
                    new HeaderLine { Content = "SUMMARY OF PROPOSED MAINTENANCE AND OTHER OPERATING EXPENSES (MOOE)", Bold = true, Italic = false, FontSize = 10, ParagraphAlignment = ParagraphAlignment.Center }
                );
                reports.AddColumnHeader(
                    new HeaderLine { Content = "Fiscal Year " + BudgetProposal.FiscalYear.ToString(), Bold = false, Italic = false, FontSize = 8, ParagraphAlignment = ParagraphAlignment.Center }
                );

                reports.AddNewLine();

                var columns = new List<ContentColumn>();
                columns.Add(new ContentColumn(10.10));
                columns.Add(new ContentColumn(2.40));
                reports.AddTable(columns, false);

                var rows = new List<ContentCell>();
                rows.Add(new ContentCell("", 0, 10, true, true, ParagraphAlignment.Left, VerticalAlignment.Center));
                rows.Add(new ContentCell("BP FORM A", 1, 10, true, false, ParagraphAlignment.Left, VerticalAlignment.Center));
                reports.AddRowContent(rows, 0);

                columns = new List<ContentColumn>();
                columns.Add(new ContentColumn(1.00, new MigraDoc.DocumentObjectModel.Color(255, 255, 255), true));
                columns.Add(new ContentColumn(2.00));
                columns.Add(new ContentColumn(5.00));
                columns.Add(new ContentColumn(4.50));
                reports.AddTable(columns, false);

                rows = new List<ContentCell>();
                rows.Add(new ContentCell("", 0));
                rows.Add(new ContentCell("College/Division/Office/Unit: ", 1, 10, true));
                rows.Add(new ContentCell(BudgetProposal.OfficeName, 2, 10, true, true, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, true));
                rows.Add(new ContentCell("", 3));
                reports.AddRowContent(rows, 0);

                reports.AddNewLine();

                columns = new List<ContentColumn>();
                columns.Add(new ContentColumn(1.00, new MigraDoc.DocumentObjectModel.Color(255, 255, 255), true));
                columns.Add(new ContentColumn(1.00, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
                columns.Add(new ContentColumn(5.00, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
                columns.Add(new ContentColumn(1.50, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
                columns.Add(new ContentColumn(1.50, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
                columns.Add(new ContentColumn(1.50, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
                columns.Add(new ContentColumn(1.00, new MigraDoc.DocumentObjectModel.Color(255, 255, 255), true));
                reports.AddTable(columns, true);

                rows = new List<ContentCell>();
                rows.Add(new ContentCell("", 0));
                rows.Add(new ContentCell("UACS", 1, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                rows.Add(new ContentCell("Object Classification", 2, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                rows.Add(new ContentCell("Tier 1*\n(On-going/existing programs/projects)", 3, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                rows.Add(new ContentCell("Tier 2**\n(New Spending Proposals)", 4, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                rows.Add(new ContentCell("Total Proposed Budget\nTier 1 + Tier 2", 5, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                rows.Add(new ContentCell("", 6));
                reports.AddRowContent(rows, 0.50);

                var subAccounts = BudgetProposal.MOOE.GroupBy(d => d.SubClassification).Select(d => d.Key).ToList();
                foreach (var subAcct in subAccounts)
                {
                    columns = new List<ContentColumn>();
                    columns.Add(new ContentColumn(1.00, new MigraDoc.DocumentObjectModel.Color(255, 255, 255), true));
                    columns.Add(new ContentColumn(10.50, new MigraDoc.DocumentObjectModel.Color(74, 132, 249)));
                    columns.Add(new ContentColumn(1.00, new MigraDoc.DocumentObjectModel.Color(255, 255, 255), true));
                    reports.AddTable(columns, true);

                    rows = new List<ContentCell>();
                    rows.Add(new ContentCell("", 0));
                    rows.Add(new ContentCell(subAcct, 1, 8, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Left, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                    rows.Add(new ContentCell("", 0));
                    reports.AddRowContent(rows, 0.25);

                    foreach (var item in BudgetProposal.MOOE.Where(d => d.SubClassification == subAcct))
                    {
                        columns = new List<ContentColumn>();
                        columns.Add(new ContentColumn(1.00, new MigraDoc.DocumentObjectModel.Color(255, 255, 255), true));
                        columns.Add(new ContentColumn(1.00));
                        columns.Add(new ContentColumn(5.00));
                        columns.Add(new ContentColumn(1.50));
                        columns.Add(new ContentColumn(1.50));
                        columns.Add(new ContentColumn(1.50));
                        columns.Add(new ContentColumn(1.00, new MigraDoc.DocumentObjectModel.Color(255, 255, 255), true));
                        reports.AddTable(columns, true);

                        rows = new List<ContentCell>();
                        rows.Add(new ContentCell("", 0));
                        rows.Add(new ContentCell(item.UACS, 1, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                        rows.Add(new ContentCell(item.ObjectClassification, 2, 8, false, false, ParagraphAlignment.Left, VerticalAlignment.Center));
                        rows.Add(new ContentCell(String.Format("{0:C}", item.Tier1), 3, 8, false, false, ParagraphAlignment.Right, VerticalAlignment.Center));
                        rows.Add(new ContentCell(String.Format("{0:C}", item.Tier2), 4, 8, false, false, ParagraphAlignment.Right, VerticalAlignment.Center));
                        rows.Add(new ContentCell(String.Format("{0:C}", item.TotalProposedProgram), 5, 8, false, false, ParagraphAlignment.Right, VerticalAlignment.Center));
                        rows.Add(new ContentCell("", 6));
                        reports.AddRowContent(rows, 0.25);
                    }
                }
                columns = new List<ContentColumn>();
                columns.Add(new ContentColumn(1.00, new MigraDoc.DocumentObjectModel.Color(255, 255, 255), true));
                columns.Add(new ContentColumn(1.00));
                columns.Add(new ContentColumn(5.00));
                columns.Add(new ContentColumn(1.50));
                columns.Add(new ContentColumn(1.50));
                columns.Add(new ContentColumn(1.50));
                columns.Add(new ContentColumn(1.00, new MigraDoc.DocumentObjectModel.Color(255, 255, 255), true));
                reports.AddTable(columns, true);

                rows = new List<ContentCell>();
                rows.Add(new ContentCell("", 0));
                rows.Add(new ContentCell("TOTAL", 1, 9, true, false, ParagraphAlignment.Right, VerticalAlignment.Center, 1));
                rows.Add(new ContentCell(String.Format("{0:C}", BudgetProposal.MOOE.Sum(d => d.Tier1)), 3, 10, true, false, ParagraphAlignment.Right, VerticalAlignment.Center));
                rows.Add(new ContentCell(String.Format("{0:C}", BudgetProposal.MOOE.Sum(d => d.Tier2)), 4, 10, true, false, ParagraphAlignment.Right, VerticalAlignment.Center));
                rows.Add(new ContentCell(String.Format("{0:C}", BudgetProposal.MOOE.Sum(d => d.TotalProposedProgram)), 5, 10, true, false, ParagraphAlignment.Right, VerticalAlignment.Center));
                rows.Add(new ContentCell("", 6));
                reports.AddRowContent(rows, 0.25);

                columns = new List<ContentColumn>();
                columns.Add(new ContentColumn(1.00, new MigraDoc.DocumentObjectModel.Color(255, 255, 255), true));
                columns.Add(new ContentColumn(9.00));
                columns.Add(new ContentColumn(1.50));
                columns.Add(new ContentColumn(1.00, new MigraDoc.DocumentObjectModel.Color(255, 255, 255), true));
                reports.AddTable(columns, true);

                rows = new List<ContentCell>();
                rows.Add(new ContentCell("", 0));
                rows.Add(new ContentCell("Submitted By:\n\n\n\n" + BudgetProposal.SubmittedBy + "\n" + BudgetProposal.Designation, 1, 9, true, false, ParagraphAlignment.Left, VerticalAlignment.Top));
                rows.Add(new ContentCell("Date:\n\n\n\n" + BudgetProposal.SubmittedAt, 2, 9, true, false, ParagraphAlignment.Left, VerticalAlignment.Top));
                rows.Add(new ContentCell("", 3));
                reports.AddRowContent(rows, 0.25);

                reports.AddNewLine();
                reports.AddNewLine();

                columns = new List<ContentColumn>();
                columns.Add(new ContentColumn(1.00, new MigraDoc.DocumentObjectModel.Color(255, 255, 255), true));
                columns.Add(new ContentColumn(10.50));
                columns.Add(new ContentColumn(1.00, new MigraDoc.DocumentObjectModel.Color(255, 255, 255), true));
                reports.AddTable(columns, false);

                rows = new List<ContentCell>();
                rows.Add(new ContentCell("", 0));
                rows.Add(new ContentCell("* Tier I - Annual cost of on-going programs and projects based on actual obligations for FY " + (BudgetProposal.FiscalYear - 1).ToString() + " plus adjustments\narising from changes in inflation rate.", 1, 9, false, false, ParagraphAlignment.Left, VerticalAlignment.Top));
                rows.Add(new ContentCell("", 2));
                reports.AddRowContent(rows, 0.25);

                rows = new List<ContentCell>();
                rows.Add(new ContentCell("", 0));
                rows.Add(new ContentCell("\n\n** Tier II - New Spending Proposals.", 1, 9, false, false, ParagraphAlignment.Left, VerticalAlignment.Top));
                rows.Add(new ContentCell("", 2));
                reports.AddRowContent(rows, 0.25);
            }

            if(BudgetProposal.CaptialOutlay.Count > 0)
            {
                reports.ReportFilename = "BP Form A - " + BudgetProposal.OfficeName + "-" + FiscalYear.ToString();
                reports.CreateDocument();
                reports.AddDoubleColumnHeader(LogoPath);
                reports.AddColumnHeader(
                    new HeaderLine { Content = "", Bold = false, Italic = false, FontSize = 5, ParagraphAlignment = ParagraphAlignment.Left },
                    new HeaderLine { Content = "PUP-MOOE-7-BUSO-002", Bold = false, Italic = false, FontSize = 8, ParagraphAlignment = ParagraphAlignment.Left, LeftIndent = 80 }
                );
                reports.AddColumnHeader(
                    new HeaderLine { Content = "Republic of the Philippines", Bold = false, Italic = false, FontSize = 10, ParagraphAlignment = ParagraphAlignment.Left },
                    new HeaderLine { Content = "Rev.0", Bold = false, Italic = false, FontSize = 8, ParagraphAlignment = ParagraphAlignment.Left, LeftIndent = 80 }
                );
                reports.AddColumnHeader(
                    new HeaderLine { Content = "POLYTECHNIC UNIVERSITY OF THE PHILIPPINES", Bold = true, Italic = false, FontSize = 12, ParagraphAlignment = ParagraphAlignment.Left },
                    new HeaderLine { Content = "May 15, 2018", Bold = false, Italic = false, FontSize = 8, ParagraphAlignment = ParagraphAlignment.Left, LeftIndent = 80 }
                );
                reports.AddColumnHeader(
                    new HeaderLine { Content = "Anonas St., Sta. Mesa, Manila", Bold = false, Italic = false, FontSize = 9, ParagraphAlignment = ParagraphAlignment.Left },
                    new HeaderLine { Content = "Date Printed: " + DateTime.Now.ToString("dd MMMM yyyy hh:mm tt"), Bold = true, Italic = false, FontSize = 9, ParagraphAlignment = ParagraphAlignment.Left, LeftIndent = 80 }
                );

                reports.AddNewLine();
                reports.AddNewLine();

                reports.AddSingleColumnHeader();
                reports.AddColumnHeader(
                    new HeaderLine { Content = "SUMMARY OF PROPOSED CAPITAL OUTLAYS", Bold = true, Italic = false, FontSize = 10, ParagraphAlignment = ParagraphAlignment.Center }
                );
                reports.AddColumnHeader(
                    new HeaderLine { Content = "Fiscal Year " + BudgetProposal.FiscalYear.ToString(), Bold = false, Italic = false, FontSize = 8, ParagraphAlignment = ParagraphAlignment.Center }
                );

                reports.AddNewLine();

                var columns = new List<ContentColumn>();
                columns.Add(new ContentColumn(10.10));
                columns.Add(new ContentColumn(2.40));
                reports.AddTable(columns, false);

                var rows = new List<ContentCell>();
                rows.Add(new ContentCell("", 0, 10, true, true, ParagraphAlignment.Left, VerticalAlignment.Center));
                rows.Add(new ContentCell("BP FORM B", 1, 10, true, false, ParagraphAlignment.Left, VerticalAlignment.Center));
                reports.AddRowContent(rows, 0);

                columns = new List<ContentColumn>();
                columns.Add(new ContentColumn(1.00, new MigraDoc.DocumentObjectModel.Color(255, 255, 255), true));
                columns.Add(new ContentColumn(2.00));
                columns.Add(new ContentColumn(5.00));
                columns.Add(new ContentColumn(4.50));
                reports.AddTable(columns, false);

                rows = new List<ContentCell>();
                rows.Add(new ContentCell("", 0));
                rows.Add(new ContentCell("College/Division/Office/Unit: ", 1, 10, true));
                rows.Add(new ContentCell(BudgetProposal.OfficeName, 2, 10, true, true, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, true));
                rows.Add(new ContentCell("", 3));
                reports.AddRowContent(rows, 0);

                reports.AddNewLine();

                columns = new List<ContentColumn>();
                columns.Add(new ContentColumn(1.00, new MigraDoc.DocumentObjectModel.Color(255, 255, 255), true));
                columns.Add(new ContentColumn(1.00, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
                columns.Add(new ContentColumn(5.00, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
                columns.Add(new ContentColumn(1.50, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
                columns.Add(new ContentColumn(1.50, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
                columns.Add(new ContentColumn(1.50, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
                columns.Add(new ContentColumn(1.00, new MigraDoc.DocumentObjectModel.Color(255, 255, 255), true));
                reports.AddTable(columns, true);

                rows = new List<ContentCell>();
                rows.Add(new ContentCell("", 0));
                rows.Add(new ContentCell("UACS", 1, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                rows.Add(new ContentCell("Object Classification", 2, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                rows.Add(new ContentCell("Tier 1*\n(On-going/existing programs/projects)", 3, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                rows.Add(new ContentCell("Tier 2**\n(New Spending Proposals)", 4, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                rows.Add(new ContentCell("Total Proposed Budget\nTier 1 + Tier 2", 5, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                rows.Add(new ContentCell("", 6));
                reports.AddRowContent(rows, 0.50);

                var subAccounts = BudgetProposal.CaptialOutlay.GroupBy(d => d.SubClassification).Select(d => d.Key).ToList();
                foreach (var subAcct in subAccounts)
                {
                    columns = new List<ContentColumn>();
                    columns.Add(new ContentColumn(1.00, new MigraDoc.DocumentObjectModel.Color(255, 255, 255), true));
                    columns.Add(new ContentColumn(10.50, new MigraDoc.DocumentObjectModel.Color(74, 132, 249)));
                    columns.Add(new ContentColumn(1.00, new MigraDoc.DocumentObjectModel.Color(255, 255, 255), true));
                    reports.AddTable(columns, true);

                    rows = new List<ContentCell>();
                    rows.Add(new ContentCell("", 0));
                    rows.Add(new ContentCell(subAcct, 1, 8, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Left, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                    rows.Add(new ContentCell("", 0));
                    reports.AddRowContent(rows, 0.25);

                    foreach (var item in BudgetProposal.CaptialOutlay.Where(d => d.SubClassification == subAcct))
                    {
                        columns = new List<ContentColumn>();
                        columns.Add(new ContentColumn(1.00, new MigraDoc.DocumentObjectModel.Color(255, 255, 255), true));
                        columns.Add(new ContentColumn(1.00));
                        columns.Add(new ContentColumn(5.00));
                        columns.Add(new ContentColumn(1.50));
                        columns.Add(new ContentColumn(1.50));
                        columns.Add(new ContentColumn(1.50));
                        columns.Add(new ContentColumn(1.00, new MigraDoc.DocumentObjectModel.Color(255, 255, 255), true));
                        reports.AddTable(columns, true);

                        rows = new List<ContentCell>();
                        rows.Add(new ContentCell("", 0));
                        rows.Add(new ContentCell(item.UACS, 1, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                        rows.Add(new ContentCell(item.ObjectClassification, 2, 8, false, false, ParagraphAlignment.Left, VerticalAlignment.Center));
                        rows.Add(new ContentCell(String.Format("{0:C}", item.Tier1), 3, 8, false, false, ParagraphAlignment.Right, VerticalAlignment.Center));
                        rows.Add(new ContentCell(String.Format("{0:C}", item.Tier2), 4, 8, false, false, ParagraphAlignment.Right, VerticalAlignment.Center));
                        rows.Add(new ContentCell(String.Format("{0:C}", item.TotalProposedProgram), 5, 8, false, false, ParagraphAlignment.Right, VerticalAlignment.Center));
                        rows.Add(new ContentCell("", 6));
                        reports.AddRowContent(rows, 0.25);
                    }
                }
                columns = new List<ContentColumn>();
                columns.Add(new ContentColumn(1.00, new MigraDoc.DocumentObjectModel.Color(255, 255, 255), true));
                columns.Add(new ContentColumn(1.00));
                columns.Add(new ContentColumn(5.00));
                columns.Add(new ContentColumn(1.50));
                columns.Add(new ContentColumn(1.50));
                columns.Add(new ContentColumn(1.50));
                columns.Add(new ContentColumn(1.00, new MigraDoc.DocumentObjectModel.Color(255, 255, 255), true));
                reports.AddTable(columns, true);

                rows = new List<ContentCell>();
                rows.Add(new ContentCell("", 0));
                rows.Add(new ContentCell("TOTAL", 1, 9, true, false, ParagraphAlignment.Right, VerticalAlignment.Center, 1));
                rows.Add(new ContentCell(String.Format("{0:C}", BudgetProposal.CaptialOutlay.Sum(d => d.Tier1)), 3, 10, true, false, ParagraphAlignment.Right, VerticalAlignment.Center));
                rows.Add(new ContentCell(String.Format("{0:C}", BudgetProposal.CaptialOutlay.Sum(d => d.Tier2)), 4, 10, true, false, ParagraphAlignment.Right, VerticalAlignment.Center));
                rows.Add(new ContentCell(String.Format("{0:C}", BudgetProposal.CaptialOutlay.Sum(d => d.TotalProposedProgram)), 5, 10, true, false, ParagraphAlignment.Right, VerticalAlignment.Center));
                rows.Add(new ContentCell("", 6));
                reports.AddRowContent(rows, 0.25);

                columns = new List<ContentColumn>();
                columns.Add(new ContentColumn(1.00, new MigraDoc.DocumentObjectModel.Color(255, 255, 255), true));
                columns.Add(new ContentColumn(9.00));
                columns.Add(new ContentColumn(1.50));
                columns.Add(new ContentColumn(1.00, new MigraDoc.DocumentObjectModel.Color(255, 255, 255), true));
                reports.AddTable(columns, true);

                rows = new List<ContentCell>();
                rows.Add(new ContentCell("", 0));
                rows.Add(new ContentCell("Submitted By:\n\n\n\n" + BudgetProposal.SubmittedBy + "\n" + BudgetProposal.Designation, 1, 9, true, false, ParagraphAlignment.Left, VerticalAlignment.Top));
                rows.Add(new ContentCell("Date:\n\n\n\n" + BudgetProposal.SubmittedAt, 2, 9, true, false, ParagraphAlignment.Left, VerticalAlignment.Top));
                rows.Add(new ContentCell("", 3));
                reports.AddRowContent(rows, 0.25);

                reports.AddNewLine();
                reports.AddNewLine();

                columns = new List<ContentColumn>();
                columns.Add(new ContentColumn(1.00, new MigraDoc.DocumentObjectModel.Color(255, 255, 255), true));
                columns.Add(new ContentColumn(10.50));
                columns.Add(new ContentColumn(1.00, new MigraDoc.DocumentObjectModel.Color(255, 255, 255), true));
                reports.AddTable(columns, false);

                rows = new List<ContentCell>();
                rows.Add(new ContentCell("", 0));
                rows.Add(new ContentCell("* Tier I - Annual cost of on-going programs and projects based on actual obligations for FY " + (BudgetProposal.FiscalYear - 1).ToString() + " plus adjustments\narising from changes in inflation rate.", 1, 9, false, false, ParagraphAlignment.Left, VerticalAlignment.Top));
                rows.Add(new ContentCell("", 2));
                reports.AddRowContent(rows, 0.25);

                rows = new List<ContentCell>();
                rows.Add(new ContentCell("", 0));
                rows.Add(new ContentCell("\n\n** Tier II - New Spending Proposals.", 1, 9, false, false, ParagraphAlignment.Left, VerticalAlignment.Top));
                rows.Add(new ContentCell("", 2));
                reports.AddRowContent(rows, 0.25);
            }

            return reports.GenerateReport();
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
                ppmpDAL.Dispose();
            }
            base.Dispose(disposing);
        }
    }
    public class ProjectProcurementManagementPlanDAL : Controller
    {
        private FMISDbContext db = new FMISDbContext();
        private TEMPAccounting abdb = new TEMPAccounting();
        private HRISDataAccess hrisDataAccess = new HRISDataAccess();
        private AccountsManagementBL accountsManagement = new AccountsManagementBL();

        public List<int> GetPPMPFiscalYears()
        {
            return db.PPMPHeader.GroupBy(d => d.FiscalYear).Select(d =>  d.Key).ToList();
        }
        public List<MOOEViewModel> GetMOOE(string UserEmail, int FiscalYear)
        {
            List<MOOEViewModel> mooe = new List<MOOEViewModel>();
            var user = db.UserAccounts.Where(d => d.Email == UserEmail).FirstOrDefault();
            var office = hrisDataAccess.GetFullDepartmentDetails(user.DepartmentCode);
            var accountList = abdb.ChartOfAccounts.Where(d => d.GenAcctName == "Maintenance and Other Operating Expenses").ToList();

            var tier1Actual = (from items in db.Items
                               join ppmp in db.ProjectPlanItems on items.ID equals ppmp.ItemReference
                               where (office.SectionCode == null ? ppmp.FKPPMPReference.Department == office.DepartmentCode : ppmp.FKPPMPReference.Unit == office.SectionCode) && ppmp.FKPPMPReference.FiscalYear < FiscalYear && ppmp.UnitCost < 15000.00m && ppmp.ProposalType == BudgetProposalType.Actual
                               select new { UACS = items.FKItemTypeReference.AccountClass, EstimatedBudget = ppmp.ProjectEstimatedBudget }
                               into results
                               group results by results.UACS into groupResult
                               select new { UACS = groupResult.Key, Amount = groupResult.Sum(d => d.EstimatedBudget) }).ToList();

            var tier1Ongoing = (from items in db.Items
                                join ppmp in db.ProjectPlanItems on items.ID equals ppmp.ItemReference
                                where (office.SectionCode == null ? ppmp.FKPPMPReference.Department == office.DepartmentCode : ppmp.FKPPMPReference.Unit == office.SectionCode) && ppmp.UnitCost < 15000.00m && ppmp.Status == "Procurement On-going"
                                select new { UACS = items.FKItemTypeReference.AccountClass, EstimatedBudget = ppmp.ProjectEstimatedBudget }
                                into results
                                group results by results.UACS into groupResult
                                select new { UACS = groupResult.Key, Amount = groupResult.Sum(d => d.EstimatedBudget) }).ToList();

            var tier1Items = tier1Actual.Concat(tier1Ongoing).ToList();

            var tier2Items = (from items in db.Items
                              join ppmp in db.ProjectPlanItems on items.ID equals ppmp.ItemReference
                              where (office.SectionCode == null ? ppmp.FKPPMPReference.Department == office.DepartmentCode : ppmp.FKPPMPReference.Unit == office.SectionCode) && ppmp.FKPPMPReference.FiscalYear == FiscalYear && ppmp.UnitCost < 15000.00m && ppmp.ProposalType == BudgetProposalType.NewProposal
                              select new { UACS = items.FKItemTypeReference.AccountClass, EstimatedBudget = ppmp.ProjectEstimatedBudget }
                              into results
                              group results by results.UACS into groupResult
                              select new { UACS = groupResult.Key, Amount = groupResult.Sum(d => d.EstimatedBudget) }).ToList();

            var mooeItems = (from accounts in accountList
                                      join tier1 in tier1Items on accounts.UACS_Code equals tier1.UACS into tier1
                                      from t1 in tier1.DefaultIfEmpty()
                                      join tier2 in tier2Items on accounts.UACS_Code equals tier2.UACS into tier2
                                      from t2 in tier2.DefaultIfEmpty()
                                      where t1 != null || t2 != null
                                      select new MOOEViewModel
                                      {
                                          UACS = accounts.UACS_Code,
                                          SubClassification = accounts.SubAcctName,
                                          ObjectClassification = accounts.AcctName,
                                          Tier1 = t1 == null ? 0.00m : (decimal)t1.Amount,
                                          Tier2 = t2 == null ? 0.00m : (decimal)t2.Amount,
                                          TotalProposedProgram = (t1 == null ? 0.00m : (decimal)t1.Amount) + (t2 == null ? 0.00m : (decimal)t2.Amount)
                                      }).ToList();

            var tier1Services = (from services in db.Items
                                 join ppmp in db.ProjectPlanServices on services.ID equals ppmp.ItemReference
                                 where (office.SectionCode == null ? ppmp.FKPPMPReference.Department == office.DepartmentCode : ppmp.FKPPMPReference.Unit == office.SectionCode) && ppmp.UnitCost < 15000.00m && ppmp.Status == "Procurement On-going"
                                 select new { UACS = services.FKItemTypeReference.AccountClass, EstimatedBudget = ppmp.ProjectEstimatedBudget }
                                 into results
                                 group results by results.UACS into groupResult
                                 select new { UACS = groupResult.Key, Amount = groupResult.Sum(d => d.EstimatedBudget) }).ToList();

            var tier2Services = (from services in db.Items
                                 join ppmp in db.ProjectPlanServices on services.ID equals ppmp.ItemReference
                                 where (office.SectionCode == null ? ppmp.FKPPMPReference.Department == office.DepartmentCode : ppmp.FKPPMPReference.Unit == office.SectionCode) && ppmp.FKPPMPReference.FiscalYear == FiscalYear && ppmp.UnitCost < 15000.00m && ppmp.ProposalType == BudgetProposalType.NewProposal
                                 select new { UACS = services.FKItemTypeReference.AccountClass, EstimatedBudget = ppmp.ProjectEstimatedBudget }
                                 into results
                                 group results by results.UACS into groupResult
                                 select new { UACS = groupResult.Key, Amount = groupResult.Sum(d => d.EstimatedBudget) }).ToList();

            var mooeService = (from accounts in accountList
                               join tier1 in tier1Services on accounts.UACS_Code equals tier1.UACS into tier1
                               from t1 in tier1.DefaultIfEmpty()
                               join tier2 in tier2Services on accounts.UACS_Code equals tier2.UACS into tier2
                               from t2 in tier2.DefaultIfEmpty()
                               where t1 != null || t2 != null
                               select new MOOEViewModel
                               {
                                   UACS = accounts.UACS_Code,
                                   SubClassification = accounts.SubAcctName,
                                   ObjectClassification = accounts.AcctName,
                                   Tier1 = t1 == null ? 0.00m : (decimal)t1.Amount,
                                   Tier2 = t2 == null ? 0.00m : (decimal)t2.Amount,
                                   TotalProposedProgram = (t1 == null ? 0.00m : (decimal)t1.Amount) + (t2 == null ? 0.00m : (decimal)t2.Amount)
                               }).ToList();

            mooe.AddRange(mooeItems);
            mooe.AddRange(mooeService);

            return mooe;
        }
        public List<CapitalOutlayVM> GetCapitalOutlay(string UserEmail, int FiscalYear)
        {
            List<CapitalOutlayVM> capitalOutlay = new List<CapitalOutlayVM>();
            var user = db.UserAccounts.Where(d => d.Email == UserEmail).FirstOrDefault();
            var office = hrisDataAccess.GetFullDepartmentDetails(user.DepartmentCode);
            var accountList = abdb.ChartOfAccounts.Where(d => d.GenAcctName == "Maintenance and Other Operating Expenses").ToList();

            var tier1Actual = (from items in db.Items
                               join ppmp in db.ProjectPlanItems on items.ID equals ppmp.ItemReference
                               where (office.SectionCode == null ? ppmp.FKPPMPReference.Department == office.DepartmentCode : ppmp.FKPPMPReference.Unit == office.SectionCode) && ppmp.FKPPMPReference.FiscalYear == FiscalYear && ppmp.UnitCost >= 15000.00m && ppmp.ProposalType == BudgetProposalType.Actual
                               select new { UACS = items.FKItemTypeReference.AccountClass, EstimatedBudget = ppmp.ProjectEstimatedBudget }
                               into results
                               group results by results.UACS into groupResult
                               select new { UACS = groupResult.Key, Amount = groupResult.Sum(d => d.EstimatedBudget) }).ToList();

            var tier1Ongoing = (from items in db.Items
                                join ppmp in db.ProjectPlanItems on items.ID equals ppmp.ItemReference
                                where (office.SectionCode == null ? ppmp.FKPPMPReference.Department == office.DepartmentCode : ppmp.FKPPMPReference.Unit == office.SectionCode) && ppmp.UnitCost >= 15000.00m && ppmp.Status == "Procurement On-going"
                                select new { UACS = items.FKItemTypeReference.AccountClass, EstimatedBudget = ppmp.ProjectEstimatedBudget }
                                into results
                                group results by results.UACS into groupResult
                                select new { UACS = groupResult.Key, Amount = groupResult.Sum(d => d.EstimatedBudget) }).ToList();

            var tier1Items = tier1Actual.Concat(tier1Ongoing).ToList();

            var tier2Items = (from items in db.Items
                              join ppmp in db.ProjectPlanItems on items.ID equals ppmp.ItemReference
                              where (office.SectionCode == null ? ppmp.FKPPMPReference.Department == office.DepartmentCode : ppmp.FKPPMPReference.Unit == office.SectionCode) && ppmp.FKPPMPReference.FiscalYear == FiscalYear && ppmp.UnitCost >= 15000.00m && ppmp.ProposalType == BudgetProposalType.NewProposal
                              select new { UACS = items.FKItemTypeReference.AccountClass, EstimatedBudget = ppmp.ProjectEstimatedBudget }
                              into results
                              group results by results.UACS into groupResult
                              select new { UACS = groupResult.Key, Amount = groupResult.Sum(d => d.EstimatedBudget) }).ToList();

            var capitalOutlayItems = (from accounts in accountList
                                      join tier1 in tier1Items on accounts.UACS_Code equals tier1.UACS into tier1
                                      from t1 in tier1.DefaultIfEmpty()
                                      join tier2 in tier2Items on accounts.UACS_Code equals tier2.UACS into tier2
                                      from t2 in tier2.DefaultIfEmpty()
                                      where t1 != null || t2 != null
                                      select new CapitalOutlayVM
                                      {
                                          UACS = accounts.UACS_Code,
                                          SubClassification = accounts.SubAcctName,
                                          ObjectClassification = accounts.AcctName,
                                          Tier1 = t1 == null ? 0.00m : (decimal)t1.Amount,
                                          Tier2 = t2 == null ? 0.00m : (decimal)t2.Amount,
                                          TotalProposedProgram = (t1 == null ? 0.00m : (decimal)t1.Amount) + (t2 == null ? 0.00m : (decimal)t2.Amount)
                                      }).ToList();

            var tier1Services = (from services in db.Items
                                 join ppmp in db.ProjectPlanServices on services.ID equals ppmp.ItemReference
                                 where (office.SectionCode == null ? ppmp.FKPPMPReference.Department == office.DepartmentCode : ppmp.FKPPMPReference.Unit == office.SectionCode) && ppmp.UnitCost >= 15000.00m && ppmp.Status == "Procurement On-going"
                                 select new { UACS = services.FKItemTypeReference.AccountClass, EstimatedBudget = ppmp.ProjectEstimatedBudget }
                                 into results
                                 group results by results.UACS into groupResult
                                 select new { UACS = groupResult.Key, Amount = groupResult.Sum(d => d.EstimatedBudget) }).ToList();

            var tier2Services = (from services in db.Items
                                 join ppmp in db.ProjectPlanServices on services.ID equals ppmp.ItemReference
                                 where (office.SectionCode == null ? ppmp.FKPPMPReference.Department == office.DepartmentCode : ppmp.FKPPMPReference.Unit == office.SectionCode) && ppmp.FKPPMPReference.FiscalYear == FiscalYear && ppmp.UnitCost >= 15000.00m && ppmp.ProposalType == BudgetProposalType.NewProposal
                                 select new { UACS = services.FKItemTypeReference.AccountClass, EstimatedBudget = ppmp.ProjectEstimatedBudget }
                                 into results
                                 group results by results.UACS into groupResult
                                 select new { UACS = groupResult.Key, Amount = groupResult.Sum(d => d.EstimatedBudget) }).ToList();

            var capitalOutlayService = (from accounts in accountList
                                        join tier1 in tier1Services on accounts.UACS_Code equals tier1.UACS into tier1
                                        from t1 in tier1.DefaultIfEmpty()
                                        join tier2 in tier2Services on accounts.UACS_Code equals tier2.UACS into tier2
                                        from t2 in tier2.DefaultIfEmpty()
                                        where t1 != null || t2 != null
                                        select new CapitalOutlayVM
                                        {
                                            UACS = accounts.UACS_Code,
                                            SubClassification = accounts.SubAcctName,
                                            ObjectClassification = accounts.AcctName,
                                            Tier1 = t1 == null ? 0.00m : (decimal)t1.Amount,
                                            Tier2 = t2 == null ? 0.00m : (decimal)t2.Amount,
                                            TotalProposedProgram = (t1 == null ? 0.00m : (decimal)t1.Amount) + (t2 == null ? 0.00m : (decimal)t2.Amount)
                                        }).ToList();

            capitalOutlay.AddRange(capitalOutlayItems);
            capitalOutlay.AddRange(capitalOutlayService);

            return capitalOutlay;
        }
        public List<PPMPHeaderViewModel> GetPPMPList(string UserEmail, int FiscalYear)
        {
            var user = db.UserAccounts.Where(d => d.Email == UserEmail).FirstOrDefault();
            var office = hrisDataAccess.GetFullDepartmentDetails(user.DepartmentCode);
            return db.PPMPHeader.Where(d => (office.SectionCode == null ? d.Department == office.DepartmentCode : d.Unit == office.SectionCode) && d.FiscalYear == FiscalYear)
                   .Select(d => new PPMPHeaderViewModel {
                       ReferenceNo = d.ReferenceNo,
                       FiscalYear = d.FiscalYear,
                       PPMPType = d.FKPPMPTypeReference.InventoryTypeName,
                       EstimatedBudget = (db.InventoryTypes.Where(x => x.ID == d.PPMPType).FirstOrDefault().IsTangible) ? (decimal)db.ProjectPlanItems.Where(x => x.PPMPReference == d.ID).Sum(x => x.ProjectEstimatedBudget) : (decimal)db.ProjectPlanServices.Where(x => x.PPMPReference == d.ID).Sum(x => x.ProjectEstimatedBudget),
                       ApprovedBudget = (decimal)d.ABC,
                       Status = d.Status
                   }).ToList();
        }
        public PPMPViewModel GetPPMPDetails(string ReferenceNo, string UserEmail)
        {
            var preparedBy = db.PPMPHeader.Where(d => d.ReferenceNo == ReferenceNo).FirstOrDefault().PreparedBy;
            var employee = hrisDataAccess.GetEmployeeByCode(preparedBy);
            var office = hrisDataAccess.GetFullDepartmentDetails(employee.DepartmentCode);
            var ppmpHeader = db.PPMPHeader.Where(d => d.ReferenceNo == ReferenceNo && d.Department == office.DepartmentCode).FirstOrDefault();

            PPMPViewModel ppmpVM = new PPMPViewModel();
            ppmpVM.DBMItems = new List<PPMPItemDetailsVM>();
            ppmpVM.NonDBMItems = new List<PPMPItemDetailsVM>();
            ppmpVM.Header = new PPMPHeaderViewModel
            {
                ReferenceNo = ppmpHeader.ReferenceNo,
                FiscalYear = ppmpHeader.FiscalYear,
                PPMPType = ppmpHeader.FKPPMPTypeReference.InventoryTypeName,
                Sector = office.Sector == null ? office.Department : office.Sector,
                Department = office.Department,
                Unit = office.Section,
                EstimatedBudget = (db.InventoryTypes.Where(x => x.ID == ppmpHeader.PPMPType).FirstOrDefault().IsTangible) ? (decimal)db.ProjectPlanItems.Where(x => x.PPMPReference == ppmpHeader.ID).Sum(x => x.ProjectEstimatedBudget) : (decimal)db.ProjectPlanServices.Where(x => x.PPMPReference == ppmpHeader.ID).Sum(x => x.ProjectEstimatedBudget),
                Status = ppmpHeader.Status,
                CreatedAt = (DateTime)ppmpHeader.CreatedAt,
                EvaluatedAt = ppmpHeader.ApprovedAt,
                SubmittedAt = ppmpHeader.SubmittedAt,
                PreparedBy = employee.EmployeeName + ", " + employee.Designation,
                SubmittedBy = ppmpHeader.SubmittedBy
            };

            if (ppmpVM.Header == null)
            {
                return null;
            }

            ppmpVM.Workflow = db.PPMPApprovalWorkflow.Where(d => d.FKPPMPHeader.ReferenceNo == ReferenceNo).Select(d => new PPMPApprovalWorkflowViewModel
            {
                ReferenceNo = ReferenceNo,
                Status = d.Status,
                UpdatedAt = d.UpdatedAt,
                Remarks = d.Remarks,
                Office = office.Department,
                Personnel = employee.EmployeeName
            }).ToList();
            var inventoryType = db.InventoryTypes.Where(d => d.InventoryTypeName == ppmpVM.Header.PPMPType).FirstOrDefault();
            if (inventoryType.IsTangible)
            {
                var DBMItems = db.ProjectPlanItems.Where(d => d.FKPPMPReference.ReferenceNo == ppmpVM.Header.ReferenceNo && d.FKItemReference.ProcurementSource == ProcurementSources.PS_DBM).ToList();
                ppmpVM.DBMItems = DBMItems.Select(d => new PPMPItemDetailsVM
                {
                    ProjectCode = d.FKProjectReference.ProjectCode,
                    Project = d.FKProjectReference.ProjectName,
                    ItemCode = d.FKItemReference.ItemCode,
                    ItemName = d.FKItemReference.ItemFullName,
                    ItemSpecifications = d.FKItemReference.ItemSpecifications,
                    ProcurementSource = d.FKItemReference.ProcurementSource,
                    ItemImage = d.FKItemReference.ItemImage,
                    Category = d.FKItemReference.FKCategoryReference.ItemCategoryName,
                    IndividualUOMReference = d.FKItemReference.FKIndividualUnitReference.Abbreviation,
                    JanMilestone = d.FKProjectReference.ProjectCode.StartsWith("CSPR") ? d.ProjectJanQty == null ? "0" : d.ProjectJanQty.ToString() : d.FKProjectReference.ProjectCode.StartsWith("EUPR") ? d.PPMPJan : d.ProjectJanQty == null ? "0" : d.ProjectJanQty.ToString(),
                    FebMilestone = d.FKProjectReference.ProjectCode.StartsWith("CSPR") ? d.ProjectFebQty == null ? "0" : d.ProjectFebQty.ToString() : d.FKProjectReference.ProjectCode.StartsWith("EUPR") ? d.PPMPFeb : d.ProjectFebQty == null ? "0" : d.ProjectFebQty.ToString(),
                    MarMilestone = d.FKProjectReference.ProjectCode.StartsWith("CSPR") ? d.ProjectMarQty == null ? "0" : d.ProjectMarQty.ToString() : d.FKProjectReference.ProjectCode.StartsWith("EUPR") ? d.PPMPMar : d.ProjectMarQty == null ? "0" : d.ProjectMarQty.ToString(),
                    AprMilestone = d.FKProjectReference.ProjectCode.StartsWith("CSPR") ? d.ProjectAprQty == null ? "0" : d.ProjectAprQty.ToString() : d.FKProjectReference.ProjectCode.StartsWith("EUPR") ? d.PPMPApr : d.ProjectAprQty == null ? "0" : d.ProjectAprQty.ToString(),
                    MayMilestone = d.FKProjectReference.ProjectCode.StartsWith("CSPR") ? d.ProjectMayQty == null ? "0" : d.ProjectMayQty.ToString() : d.FKProjectReference.ProjectCode.StartsWith("EUPR") ? d.PPMPMay : d.ProjectMayQty == null ? "0" : d.ProjectMayQty.ToString(),
                    JunMilestone = d.FKProjectReference.ProjectCode.StartsWith("CSPR") ? d.ProjectJunQty == null ? "0" : d.ProjectJunQty.ToString() : d.FKProjectReference.ProjectCode.StartsWith("EUPR") ? d.PPMPJun : d.ProjectJunQty == null ? "0" : d.ProjectJunQty.ToString(),
                    JulMilestone = d.FKProjectReference.ProjectCode.StartsWith("CSPR") ? d.ProjectJulQty == null ? "0" : d.ProjectJulQty.ToString() : d.FKProjectReference.ProjectCode.StartsWith("EUPR") ? d.PPMPJul : d.ProjectJulQty == null ? "0" : d.ProjectJulQty.ToString(),
                    AugMilestone = d.FKProjectReference.ProjectCode.StartsWith("CSPR") ? d.ProjectAugQty == null ? "0" : d.ProjectAugQty.ToString() : d.FKProjectReference.ProjectCode.StartsWith("EUPR") ? d.PPMPAug : d.ProjectAugQty == null ? "0" : d.ProjectAugQty.ToString(),
                    SepMilestone = d.FKProjectReference.ProjectCode.StartsWith("CSPR") ? d.ProjectSepQty == null ? "0" : d.ProjectSepQty.ToString() : d.FKProjectReference.ProjectCode.StartsWith("EUPR") ? d.PPMPSep : d.ProjectSepQty == null ? "0" : d.ProjectSepQty.ToString(),
                    OctMilestone = d.FKProjectReference.ProjectCode.StartsWith("CSPR") ? d.ProjectOctQty == null ? "0" : d.ProjectOctQty.ToString() : d.FKProjectReference.ProjectCode.StartsWith("EUPR") ? d.PPMPOct : d.ProjectOctQty == null ? "0" : d.ProjectOctQty.ToString(),
                    NovMilestone = d.FKProjectReference.ProjectCode.StartsWith("CSPR") ? d.ProjectNovQty == null ? "0" : d.ProjectNovQty.ToString() : d.FKProjectReference.ProjectCode.StartsWith("EUPR") ? d.PPMPNov : d.ProjectNovQty == null ? "0" : d.ProjectNovQty.ToString(),
                    DecMilestone = d.FKProjectReference.ProjectCode.StartsWith("CSPR") ? d.ProjectDecQty == null ? "0" : d.ProjectDecQty.ToString() : d.FKProjectReference.ProjectCode.StartsWith("EUPR") ? d.PPMPDec : d.ProjectDecQty == null ? "0" : d.ProjectDecQty.ToString(),
                    TotalQty = String.Format("{0:#,##0}", d.ProjectTotalQty),
                    UnitCost = String.Format("{0:#,##0.00}", d.UnitCost),
                    EstimatedBudget = d.ProjectEstimatedBudget,
                    Remarks = d.Justification,
                    ResponsibilityCenter = d.FKItemReference.ResponsibilityCenter == null ? null : hrisDataAccess.GetDepartmentDetails(d.FKItemReference.ResponsibilityCenter).Department 
                }).ToList();

                var NonDBMItems = db.ProjectPlanItems.Where(d => d.FKPPMPReference.ReferenceNo == ppmpVM.Header.ReferenceNo && d.FKItemReference.ProcurementSource == ProcurementSources.Non_DBM).ToList();
                ppmpVM.NonDBMItems = NonDBMItems.Select(d => new PPMPItemDetailsVM
                {
                    ProjectCode = d.FKProjectReference.ProjectCode,
                    Project = d.FKProjectReference.ProjectName,
                    ItemCode = d.FKItemReference.ItemCode,
                    ItemName = d.FKItemReference.ItemFullName,
                    ItemSpecifications = d.FKItemReference.ItemSpecifications,
                    ProcurementSource = d.FKItemReference.ProcurementSource,
                    ItemImage = d.FKItemReference.ItemImage,
                    Category = d.FKItemReference.FKCategoryReference.ItemCategoryName,
                    IndividualUOMReference = d.FKItemReference.FKIndividualUnitReference.Abbreviation,
                    JanMilestone = d.FKProjectReference.ProjectCode.StartsWith("EUPR") ? d.PPMPJan : d.ProjectJanQty == null ? "0" : d.ProjectJanQty.ToString(),
                    FebMilestone = d.FKProjectReference.ProjectCode.StartsWith("EUPR") ? d.PPMPFeb : d.ProjectFebQty == null ? "0" : d.ProjectFebQty.ToString(),
                    MarMilestone = d.FKProjectReference.ProjectCode.StartsWith("EUPR") ? d.PPMPMar : d.ProjectMarQty == null ? "0" : d.ProjectMarQty.ToString(),
                    AprMilestone = d.FKProjectReference.ProjectCode.StartsWith("EUPR") ? d.PPMPApr : d.ProjectAprQty == null ? "0" : d.ProjectAprQty.ToString(),
                    MayMilestone = d.FKProjectReference.ProjectCode.StartsWith("EUPR") ? d.PPMPMay : d.ProjectMayQty == null ? "0" : d.ProjectMayQty.ToString(),
                    JunMilestone = d.FKProjectReference.ProjectCode.StartsWith("EUPR") ? d.PPMPJun : d.ProjectJunQty == null ? "0" : d.ProjectJunQty.ToString(),
                    JulMilestone = d.FKProjectReference.ProjectCode.StartsWith("EUPR") ? d.PPMPJul : d.ProjectJulQty == null ? "0" : d.ProjectJulQty.ToString(),
                    AugMilestone = d.FKProjectReference.ProjectCode.StartsWith("EUPR") ? d.PPMPAug : d.ProjectAugQty == null ? "0" : d.ProjectAugQty.ToString(),
                    SepMilestone = d.FKProjectReference.ProjectCode.StartsWith("EUPR") ? d.PPMPSep : d.ProjectSepQty == null ? "0" : d.ProjectSepQty.ToString(),
                    OctMilestone = d.FKProjectReference.ProjectCode.StartsWith("EUPR") ? d.PPMPOct : d.ProjectOctQty == null ? "0" : d.ProjectOctQty.ToString(),
                    NovMilestone = d.FKProjectReference.ProjectCode.StartsWith("EUPR") ? d.PPMPNov : d.ProjectNovQty == null ? "0" : d.ProjectNovQty.ToString(),
                    DecMilestone = d.FKProjectReference.ProjectCode.StartsWith("EUPR") ? d.PPMPDec : d.ProjectDecQty == null ? "0" : d.ProjectDecQty.ToString(),
                    Supplier1Name = d.FKSupplier1Reference.SupplierName,
                    Supplier1Address = d.FKSupplier1Reference.Address,
                    Supplier1ContactNo = d.FKSupplier1Reference.ContactNumber,
                    Supplier1EmailAddress = d.FKSupplier1Reference.EmailAddress,
                    Supplier1UnitCost = d.Supplier1UnitCost,
                    Supplier2Name = d.FKSupplier2Reference.SupplierName,
                    Supplier2Address = d.FKSupplier2Reference.Address,
                    Supplier2ContactNo = d.FKSupplier2Reference.ContactNumber,
                    Supplier2EmailAddress = d.FKSupplier2Reference.EmailAddress,
                    Supplier2UnitCost = d.Supplier2UnitCost,
                    Supplier3Name = d.FKSupplier3Reference.SupplierName,
                    Supplier3Address = d.FKSupplier3Reference.Address,
                    Supplier3ContactNo = d.FKSupplier3Reference.ContactNumber,
                    Supplier3EmailAddress = d.FKSupplier3Reference.EmailAddress,
                    Supplier3UnitCost = d.Supplier3UnitCost,
                    TotalQty = String.Format("{0:#,##0}", d.ProjectTotalQty),
                    UnitCost = String.Format("{0:#,##0.00}", d.UnitCost),
                    EstimatedBudget = d.ProjectEstimatedBudget,
                    Remarks = d.Justification,
                    ResponsibilityCenter = d.FKItemReference.ResponsibilityCenter == null ? null : hrisDataAccess.GetDepartmentDetails(d.FKItemReference.ResponsibilityCenter).Department
                }).ToList();
            }
            else
            {
                var DBMItems = db.ProjectPlanServices.Where(d => d.FKPPMPReference.ReferenceNo == ppmpVM.Header.ReferenceNo && d.FKItemReference.ProcurementSource == ProcurementSources.PS_DBM).ToList();
                ppmpVM.DBMItems = DBMItems.Select(d => new PPMPItemDetailsVM
                {
                    ProjectCode = d.FKProjectReference.ProjectCode,
                    Project = d.FKProjectReference.ProjectName,
                    Category = d.FKItemReference.FKCategoryReference.ItemCategoryName,
                    ItemCode = d.FKItemReference.ItemCode,
                    ItemName = d.FKItemReference.ItemFullName,
                    ItemSpecifications = d.ItemSpecifications,
                    ProcurementSource = d.FKItemReference.ProcurementSource,
                    ItemImage = null,
                    IndividualUOMReference = null,
                    JanMilestone = d.PPMPJan == null ? "0" : d.PPMPJan,
                    FebMilestone = d.PPMPFeb == null ? "0" : d.PPMPFeb,
                    MarMilestone = d.PPMPMar == null ? "0" : d.PPMPMar,
                    AprMilestone = d.PPMPApr == null ? "0" : d.PPMPApr,
                    MayMilestone = d.PPMPMay == null ? "0" : d.PPMPMay,
                    JunMilestone = d.PPMPJun == null ? "0" : d.PPMPJun,
                    JulMilestone = d.PPMPJul == null ? "0" : d.PPMPJul,
                    AugMilestone = d.PPMPAug == null ? "0" : d.PPMPAug,
                    SepMilestone = d.PPMPSep == null ? "0" : d.PPMPSep,
                    OctMilestone = d.PPMPOct == null ? "0" : d.PPMPOct,
                    NovMilestone = d.PPMPNov == null ? "0" : d.PPMPNov,
                    DecMilestone = d.PPMPDec == null ? "0" : d.PPMPDec,
                    TotalQty = String.Format("{0:#,##0}", d.ProjectQuantity),
                    UnitCost = String.Format("{0:#,##0.00}", d.UnitCost),
                    EstimatedBudget = d.ProjectEstimatedBudget,
                    Remarks = d.Justification,
                    ResponsibilityCenter = d.FKItemReference.ResponsibilityCenter == null ? null : hrisDataAccess.GetDepartmentDetails(d.FKItemReference.ResponsibilityCenter).Department
                }).ToList();

                var NonDBMItems = db.ProjectPlanServices.Where(d => d.FKPPMPReference.ReferenceNo == ppmpVM.Header.ReferenceNo && d.FKItemReference.ProcurementSource == ProcurementSources.Non_DBM).ToList();
                ppmpVM.NonDBMItems = NonDBMItems.Select(d => new PPMPItemDetailsVM
                {
                    ProjectCode = d.FKProjectReference.ProjectCode,
                    Project = d.FKProjectReference.ProjectName,
                    Category = d.FKItemReference.FKCategoryReference.ItemCategoryName,
                    ItemCode = d.FKItemReference.ItemCode,
                    ItemName = d.FKItemReference.ItemFullName,
                    ItemSpecifications = d.ItemSpecifications,
                    ProcurementSource = d.FKItemReference.ProcurementSource,
                    ItemImage = null,
                    IndividualUOMReference = null,
                    JanMilestone = d.PPMPJan,
                    FebMilestone = d.PPMPFeb,
                    MarMilestone = d.PPMPMar,
                    AprMilestone = d.PPMPApr,
                    MayMilestone = d.PPMPMay,
                    JunMilestone = d.PPMPJun,
                    JulMilestone = d.PPMPJul,
                    AugMilestone = d.PPMPAug,
                    SepMilestone = d.PPMPSep,
                    OctMilestone = d.PPMPOct,
                    NovMilestone = d.PPMPNov,
                    DecMilestone = d.PPMPDec,
                    Supplier1Name = d.FKSupplier1Reference.SupplierName,
                    Supplier1Address = d.FKSupplier1Reference.Address,
                    Supplier1ContactNo = d.FKSupplier1Reference.ContactNumber,
                    Supplier1EmailAddress = d.FKSupplier1Reference.EmailAddress,
                    Supplier1UnitCost = (decimal)d.Supplier1UnitCost,
                    Supplier2Name = d.Supplier2 == null ? null : d.FKSupplier2Reference.SupplierName,
                    Supplier2Address = d.Supplier2 == null ? null : d.FKSupplier2Reference.Address,
                    Supplier2ContactNo = d.Supplier2 == null ? null : d.FKSupplier2Reference.ContactNumber,
                    Supplier2EmailAddress = d.Supplier2 == null ? null : d.FKSupplier2Reference.EmailAddress,
                    Supplier2UnitCost = d.Supplier2 == null ? null : d.Supplier2UnitCost,
                    Supplier3Name = d.Supplier3 == null ? null : d.FKSupplier3Reference.SupplierName,
                    Supplier3Address = d.Supplier3 == null ? null : d.FKSupplier3Reference.Address,
                    Supplier3ContactNo = d.Supplier3 == null ? null : d.FKSupplier3Reference.ContactNumber,
                    Supplier3EmailAddress = d.Supplier3 == null ? null : d.FKSupplier3Reference.EmailAddress,
                    Supplier3UnitCost = d.Supplier3 == null ? null : d.Supplier3UnitCost,
                    TotalQty = String.Format("{0:#,##0}", d.ProjectQuantity),
                    UnitCost = String.Format("{0:#,##0.00}", d.UnitCost),
                    EstimatedBudget = d.ProjectEstimatedBudget,
                    Remarks = d.Justification,
                    ResponsibilityCenter = d.FKItemReference.ResponsibilityCenter == null ? null : hrisDataAccess.GetDepartmentDetails(d.FKItemReference.ResponsibilityCenter).Department
                }).ToList();
            }

            ppmpVM.DBMItems = ppmpVM.DBMItems;
            ppmpVM.NonDBMItems = ppmpVM.NonDBMItems;
            return ppmpVM;
        }
        public List<PPMPItemDetailsVM> GetPPMPResponsibilityCenterItems(string ResponsibilityCenter, string InventoryType)
        {
            var deptCode = hrisDataAccess.GetAllDepartments().Where(x => x.Department == ResponsibilityCenter).FirstOrDefault().DepartmentCode;
            List<PPMPItemDetailsVM> responsibilityCenterItems = new List<PPMPItemDetailsVM>();

            int temp = 0;
            var itemsList = db.ProjectPlanItems
                            .Where(d => d.FKItemReference.ResponsibilityCenter == deptCode && d.FKItemReference.FKItemTypeReference.FKInventoryTypeReference.InventoryTypeName == InventoryType && d.FKItemReference.FKItemTypeReference.FKInventoryTypeReference.IsTangible == true)
                            .Select(d => new {
                                ItemCode = d.FKItemReference.ItemCode,
                                ItemName = d.FKItemReference.ItemFullName,
                                ItemSpecifications = d.FKItemReference.ItemSpecifications,
                                ProcurementSource = d.FKItemReference.ProcurementSource,
                                ItemImage = d.FKItemReference.ItemImage,
                                IndividualUOMReference = d.FKItemReference.FKIndividualUnitReference.Abbreviation,
                                Category = d.FKItemReference.FKCategoryReference.ItemCategoryName,
                                JanMilestone = d.PPMPJan,
                                FebMilestone = d.PPMPFeb,
                                MarMilestone = d.PPMPMar,
                                AprMilestone = d.PPMPApr,
                                MayMilestone = d.PPMPMay,
                                JunMilestone = d.PPMPJun,
                                JulMilestone = d.PPMPJul,
                                AugMilestone = d.PPMPAug,
                                SepMilestone = d.PPMPSep,
                                OctMilestone = d.PPMPOct,
                                NovMilestone = d.PPMPNov,
                                DecMilestone = d.PPMPDec,
                                TotalQty = d.ProjectTotalQty,
                                UnitCost = d.UnitCost,
                                EstimatedBudget = d.ProjectEstimatedBudget
                            })
                            .ToList()
                            .GroupBy(d => new { d.ItemCode, d.ItemName, d.ItemSpecifications, d.ProcurementSource, d.IndividualUOMReference, d.Category, d.UnitCost })
                            .Select(d => new {
                                ItemCode = d.Key.ItemCode,
                                ItemName = d.Key.ItemName,
                                ItemSpecifications = d.Key.ItemSpecifications,
                                ProcurementSource = d.Key.ProcurementSource,
                                IndividualUOMReference = d.Key.IndividualUOMReference,
                                Category = d.Key.Category,
                                JanMilestone = d.Sum(x => int.TryParse(x.JanMilestone, out temp) ? temp : 0).ToString(),
                                FebMilestone = d.Sum(x => int.TryParse(x.FebMilestone, out temp) ? temp : 0).ToString(),
                                MarMilestone = d.Sum(x => int.TryParse(x.MarMilestone, out temp) ? temp : 0).ToString(),
                                AprMilestone = d.Sum(x => int.TryParse(x.AprMilestone, out temp) ? temp : 0).ToString(),
                                MayMilestone = d.Sum(x => int.TryParse(x.MayMilestone, out temp) ? temp : 0).ToString(),
                                JunMilestone = d.Sum(x => int.TryParse(x.JunMilestone, out temp) ? temp : 0).ToString(),
                                JulMilestone = d.Sum(x => int.TryParse(x.JulMilestone, out temp) ? temp : 0).ToString(),
                                AugMilestone = d.Sum(x => int.TryParse(x.AugMilestone, out temp) ? temp : 0).ToString(),
                                SepMilestone = d.Sum(x => int.TryParse(x.SepMilestone, out temp) ? temp : 0).ToString(),
                                OctMilestone = d.Sum(x => int.TryParse(x.OctMilestone, out temp) ? temp : 0).ToString(),
                                NovMilestone = d.Sum(x => int.TryParse(x.NovMilestone, out temp) ? temp : 0).ToString(),
                                DecMilestone = d.Sum(x => int.TryParse(x.DecMilestone, out temp) ? temp : 0).ToString(),
                                TotalQty = String.Format("{0:#,##0}", d.Sum(x => x.TotalQty)),
                                UnitCost = String.Format("{0:#,##0.00}", d.Key.UnitCost),
                                EstimatedBudget = d.Sum(x => x.EstimatedBudget)
                            })
                            .ToList();
            if(itemsList != null)
            {
                foreach(var item in itemsList)
                {
                    var itemsOffices = db.ProjectPlanItems
                   .Where(d => d.FKItemReference.ResponsibilityCenter == deptCode && d.FKItemReference.ItemFullName == item.ItemName)
                   .GroupBy(d => d.FKPPMPReference.ReferenceNo)
                   .Select(d => d.Key)
                   .ToList();
                    var itemsRemarks = "References: \n";
                    for (int i = 0; i < itemsOffices.Count; i++)
                    {
                        if (i == itemsOffices.Count - 1)
                        {
                            itemsRemarks += itemsOffices[i];
                        }
                        else
                        {
                            itemsRemarks += itemsOffices[i] + ",\n";
                        }
                    }

                    responsibilityCenterItems.Add(new PPMPItemDetailsVM
                    {
                        ItemCode = item.ItemCode,
                        ItemName = item.ItemName,
                        ItemSpecifications = item.ItemSpecifications,
                        ProcurementSource = item.ProcurementSource,
                        IndividualUOMReference = item.IndividualUOMReference,
                        Category = item.Category,
                        JanMilestone = item.JanMilestone,
                        FebMilestone = item.FebMilestone,
                        MarMilestone = item.MarMilestone,
                        AprMilestone = item.AprMilestone,
                        MayMilestone = item.MayMilestone,
                        JunMilestone = item.JunMilestone,
                        JulMilestone = item.JulMilestone,
                        AugMilestone = item.AugMilestone,
                        SepMilestone = item.SepMilestone,
                        OctMilestone = item.OctMilestone,
                        NovMilestone = item.NovMilestone,
                        DecMilestone = item.DecMilestone,
                        TotalQty = item.TotalQty,
                        UnitCost = item.UnitCost,
                        EstimatedBudget = item.EstimatedBudget,
                        Remarks = itemsRemarks
                    });
                }
            }

            temp = 0;
            var serviceList = db.ProjectPlanServices
                            .Where(d => d.FKItemReference.ResponsibilityCenter == deptCode && d.FKItemReference.FKItemTypeReference.FKInventoryTypeReference.InventoryTypeName == InventoryType && d.FKItemReference.FKItemTypeReference.FKInventoryTypeReference.IsTangible == false)
                            .Select(d => new {
                                ItemCode = d.FKItemReference.ItemCode,
                                ItemName = d.FKItemReference.ItemFullName,
                                ItemSpecifications = d.FKItemReference.ItemSpecifications,
                                ProcurementSource = d.FKItemReference.ProcurementSource,
                                ItemImage = d.FKItemReference.ItemImage,
                                IndividualUOMReference = d.FKItemReference.FKIndividualUnitReference.Abbreviation,
                                Category = d.FKItemReference.FKCategoryReference.ItemCategoryName,
                                JanMilestone = d.PPMPJan,
                                FebMilestone = d.PPMPFeb,
                                MarMilestone = d.PPMPMar,
                                AprMilestone = d.PPMPApr,
                                MayMilestone = d.PPMPMay,
                                JunMilestone = d.PPMPJun,
                                JulMilestone = d.PPMPJul,
                                AugMilestone = d.PPMPAug,
                                SepMilestone = d.PPMPSep,
                                OctMilestone = d.PPMPOct,
                                NovMilestone = d.PPMPNov,
                                DecMilestone = d.PPMPDec,
                                TotalQty = d.PPMPQuantity,
                                UnitCost = d.UnitCost,
                                EstimatedBudget = d.ProjectEstimatedBudget
                            })
                            .ToList()
                            .GroupBy(d => new { d.ItemCode, d.ItemName, d.ItemSpecifications, d.ProcurementSource, d.IndividualUOMReference, d.Category, d.UnitCost })
                            .Select(d => new {
                                ItemCode = d.Key.ItemCode,
                                ItemName = d.Key.ItemName,
                                ItemSpecifications = d.Key.ItemSpecifications,
                                ProcurementSource = d.Key.ProcurementSource,
                                IndividualUOMReference = d.Key.IndividualUOMReference,
                                Category = d.Key.Category,
                                JanMilestone = d.Sum(x => int.TryParse(x.JanMilestone, out temp) ? temp : 0).ToString(),
                                FebMilestone = d.Sum(x => int.TryParse(x.FebMilestone, out temp) ? temp : 0).ToString(),
                                MarMilestone = d.Sum(x => int.TryParse(x.MarMilestone, out temp) ? temp : 0).ToString(),
                                AprMilestone = d.Sum(x => int.TryParse(x.AprMilestone, out temp) ? temp : 0).ToString(),
                                MayMilestone = d.Sum(x => int.TryParse(x.MayMilestone, out temp) ? temp : 0).ToString(),
                                JunMilestone = d.Sum(x => int.TryParse(x.JunMilestone, out temp) ? temp : 0).ToString(),
                                JulMilestone = d.Sum(x => int.TryParse(x.JulMilestone, out temp) ? temp : 0).ToString(),
                                AugMilestone = d.Sum(x => int.TryParse(x.AugMilestone, out temp) ? temp : 0).ToString(),
                                SepMilestone = d.Sum(x => int.TryParse(x.SepMilestone, out temp) ? temp : 0).ToString(),
                                OctMilestone = d.Sum(x => int.TryParse(x.OctMilestone, out temp) ? temp : 0).ToString(),
                                NovMilestone = d.Sum(x => int.TryParse(x.NovMilestone, out temp) ? temp : 0).ToString(),
                                DecMilestone = d.Sum(x => int.TryParse(x.DecMilestone, out temp) ? temp : 0).ToString(),
                                TotalQty = String.Format("{0:#,##0}", d.Sum(x => x.TotalQty)),
                                UnitCost = String.Format("{0:#,##0.00}", d.Key.UnitCost),
                                EstimatedBudget = d.Sum(x => x.EstimatedBudget)
                            })
                            .ToList();
            if (serviceList != null)
            {
                foreach (var item in serviceList)
                {
                    var serviceOffices = db.ProjectPlanItems
                   .Where(d => d.FKItemReference.ResponsibilityCenter == deptCode && d.FKItemReference.ItemFullName == item.ItemName)
                   .GroupBy(d => d.FKPPMPReference.ReferenceNo)
                   .Select(d => d.Key)
                   .ToList();
                    var itemsRemarks = "References: \n";
                    for (int i = 0; i < serviceOffices.Count; i++)
                    {
                        if (i == serviceOffices.Count - 1)
                        {
                            itemsRemarks += serviceOffices[i];
                        }
                        else
                        {
                            itemsRemarks += serviceOffices[i] + ",\n";
                        }
                    }

                    responsibilityCenterItems.Add(new PPMPItemDetailsVM
                    {
                        ItemCode = item.ItemCode,
                        ItemName = item.ItemName,
                        ItemSpecifications = item.ItemSpecifications,
                        ProcurementSource = item.ProcurementSource,
                        IndividualUOMReference = item.IndividualUOMReference,
                        Category = item.Category,
                        JanMilestone = item.JanMilestone,
                        FebMilestone = item.FebMilestone,
                        MarMilestone = item.MarMilestone,
                        AprMilestone = item.AprMilestone,
                        MayMilestone = item.MayMilestone,
                        JunMilestone = item.JunMilestone,
                        JulMilestone = item.JulMilestone,
                        AugMilestone = item.AugMilestone,
                        SepMilestone = item.SepMilestone,
                        OctMilestone = item.OctMilestone,
                        NovMilestone = item.NovMilestone,
                        DecMilestone = item.DecMilestone,
                        TotalQty = item.TotalQty,
                        UnitCost = item.UnitCost,
                        EstimatedBudget = item.EstimatedBudget,
                        Remarks = itemsRemarks
                    });
                }
            }

            return responsibilityCenterItems;
        }
        public BudgetPropsalVM GetBudgetProposalDetails(string UserEmail, int FiscalYear)
        {
            BudgetPropsalVM budgetProposal = new BudgetPropsalVM();
            var user = db.UserAccounts.Where(d => d.Email == UserEmail).FirstOrDefault();
            var office = hrisDataAccess.GetFullDepartmentDetails(user.DepartmentCode);
            if(db.PPMPHeader.Where(d => d.Department == office.DepartmentCode).Count() == 0)
            {
                return null;
            }
            var submittedAt = db.PPMPHeader.Where(d => d.FiscalYear == FiscalYear && d.Department == office.DepartmentCode).Any() ? null : db.PPMPHeader.Where(d => d.FiscalYear == FiscalYear && d.Department == office.DepartmentCode).GroupBy(d => d.SubmittedAt).Max(d => d.Key).Value.ToString("dd MMMM yyyy") == null ? null : db.PPMPHeader.Where(d => d.FiscalYear == FiscalYear && d.Department == office.DepartmentCode).GroupBy(d => d.SubmittedAt).Max(d => d.Key).Value.ToString("dd MMMM yyyy");
            budgetProposal.FiscalYear = FiscalYear;
            budgetProposal.OfficeCode = office.DepartmentCode;
            budgetProposal.OfficeName = office.Department;
            budgetProposal.SubmittedBy = office.DepartmentHead;
            budgetProposal.Designation = office.DepartmentHeadDesignation;
            budgetProposal.PPMPList = GetPPMPList(UserEmail, FiscalYear);
            budgetProposal.MOOE = GetMOOE(UserEmail, FiscalYear);
            budgetProposal.CaptialOutlay = GetCapitalOutlay(UserEmail, FiscalYear);
            budgetProposal.TotalProposedBudget = budgetProposal.MOOE.Sum(d => d.TotalProposedProgram) + budgetProposal.CaptialOutlay.Sum(d => d.TotalProposedProgram);
            return budgetProposal;
        }
        public ProjectPlanItems AssignPPMPItemMilestones(ProjectPlanItems Item)
        {
            var StartMonth = db.ProjectPlans.Where(d => d.ID == Item.FKProjectReference.ID).FirstOrDefault().ProjectMonthStart;
            var FiscalYear = db.ProjectPlans.Where(d => d.ID == Item.FKProjectReference.ID).FirstOrDefault().FiscalYear - 1;
            switch (StartMonth)
            {
                case 1:
                    {
                        Item.PPMPJan = "PR Preparation/Pre-Bid Conference - (Oct, "+ FiscalYear +"); Procurement Activities - (Nov-Dec, "+ FiscalYear +"); Delivery/Preparation of RIS" ;
                        Item.PPMPFeb = null;
                        Item.PPMPMar = null;
                        Item.PPMPApr = null;
                        Item.PPMPMay = null;
                        Item.PPMPJun = null;
                        Item.PPMPJul = null;
                        Item.PPMPAug = null;
                        Item.PPMPSep = null;
                        Item.PPMPOct = null;
                        Item.PPMPNov = null;
                        Item.PPMPDec = null;
                    }
                    break;
                case 2:
                    {
                        Item.PPMPJan = "PR Preparation/Pre-Bid Conference - (Nov, " + FiscalYear + "); Procurement Activities - (Dec, " + FiscalYear + " - Jan, "+ (FiscalYear + 1) +");";
                        Item.PPMPFeb = "Delivery/Preparation of RIS";
                        Item.PPMPMar = null;
                        Item.PPMPApr = null;
                        Item.PPMPMay = null;
                        Item.PPMPJun = null;
                        Item.PPMPJul = null;
                        Item.PPMPAug = null;
                        Item.PPMPSep = null;
                        Item.PPMPOct = null;
                        Item.PPMPNov = null;
                        Item.PPMPDec = null;
                    }
                    break;
                case 3:
                    {
                        Item.PPMPJan = "PR Preparation/Pre-Bid Conference - (Dec, " + FiscalYear + "); Procurement Activities";
                        Item.PPMPFeb = "Procurement Activities";
                        Item.PPMPMar = "Delivery/Preparation of RIS";
                        Item.PPMPApr = null;
                        Item.PPMPMay = null;
                        Item.PPMPJun = null;
                        Item.PPMPJul = null;
                        Item.PPMPAug = null;
                        Item.PPMPSep = null;
                        Item.PPMPOct = null;
                        Item.PPMPNov = null;
                        Item.PPMPDec = null;
                    }
                    break;
                case 4:
                    {
                        Item.PPMPJan = "PR Preparation/Pre-Bid Conference";
                        Item.PPMPFeb = "Procurement Activities";
                        Item.PPMPMar = "Procurement Activities";
                        Item.PPMPApr = "Delivery/Preparation of RIS";
                        Item.PPMPMay = null;
                        Item.PPMPJun = null;
                        Item.PPMPJul = null;
                        Item.PPMPAug = null;
                        Item.PPMPSep = null;
                        Item.PPMPOct = null;
                        Item.PPMPNov = null;
                        Item.PPMPDec = null;
                    }
                    break;
                case 5:
                    {
                        Item.PPMPJan = null;
                        Item.PPMPFeb = "PR Preparation/Pre-Bid Conference";
                        Item.PPMPMar = "Procurement Activities";
                        Item.PPMPApr = "Procurement Activities";
                        Item.PPMPMay = "Delivery/Preparation of RIS";
                        Item.PPMPJun = null;
                        Item.PPMPJul = null;
                        Item.PPMPAug = null;
                        Item.PPMPSep = null;
                        Item.PPMPOct = null;
                        Item.PPMPNov = null;
                        Item.PPMPDec = null;
                    }
                    break;
                case 6:
                    {
                        Item.PPMPJan = null;
                        Item.PPMPFeb = null;
                        Item.PPMPMar = "PR Preparation/Pre-Bid Conference";
                        Item.PPMPApr = "Procurement Activities";
                        Item.PPMPMay = "Procurement Activities";
                        Item.PPMPJun = "Delivery/Preparation of RIS";
                        Item.PPMPJul = null;
                        Item.PPMPAug = null;
                        Item.PPMPSep = null;
                        Item.PPMPOct = null;
                        Item.PPMPNov = null;
                        Item.PPMPDec = null;
                    }
                    break;
                case 7:
                    {
                        Item.PPMPJan = null;
                        Item.PPMPFeb = null;
                        Item.PPMPMar = null;
                        Item.PPMPApr = "PR Preparation/Pre-Bid Conference";
                        Item.PPMPMay = "Procurement Activities";
                        Item.PPMPJun = "Procurement Activities";
                        Item.PPMPJul = "Delivery/Preparation of RIS";
                        Item.PPMPAug = null;
                        Item.PPMPSep = null;
                        Item.PPMPOct = null;
                        Item.PPMPNov = null;
                        Item.PPMPDec = null;
                    }
                    break;
                case 8:
                    {
                        Item.PPMPJan = null;
                        Item.PPMPFeb = null;
                        Item.PPMPMar = null;
                        Item.PPMPApr = null;
                        Item.PPMPMay = "PR Preparation/Pre-Bid Conference";
                        Item.PPMPJun = "Procurement Activities";
                        Item.PPMPJul = "Procurement Activities";
                        Item.PPMPAug = "Delivery/Preparation of RIS";
                        Item.PPMPSep = null;
                        Item.PPMPOct = null;
                        Item.PPMPNov = null;
                        Item.PPMPDec = null;
                    }
                    break;
                case 9:
                    {
                        Item.PPMPJan = null;
                        Item.PPMPFeb = null;
                        Item.PPMPMar = null;
                        Item.PPMPApr = null;
                        Item.PPMPMay = null;
                        Item.PPMPJun = "PR Preparation/Pre-Bid Conference";
                        Item.PPMPJul = "Procurement Activities";
                        Item.PPMPAug = "Procurement Activities";
                        Item.PPMPSep = "Delivery/Preparation of RIS";
                        Item.PPMPOct = null;
                        Item.PPMPNov = null;
                        Item.PPMPDec = null;
                    }
                    break;
                case 10:
                    {
                        Item.PPMPJan = null;
                        Item.PPMPFeb = null;
                        Item.PPMPMar = null;
                        Item.PPMPApr = null;
                        Item.PPMPMay = null;
                        Item.PPMPJun = null;
                        Item.PPMPJul = "PR Preparation/Pre-Bid Conference";
                        Item.PPMPAug = "Procurement Activities";
                        Item.PPMPSep = "Procurement Activities";
                        Item.PPMPOct = "Delivery/Preparation of RIS";
                        Item.PPMPNov = null;
                        Item.PPMPDec = null;
                    }
                    break;
                case 11:
                    {
                        Item.PPMPJan = null;
                        Item.PPMPFeb = null;
                        Item.PPMPMar = null;
                        Item.PPMPApr = null;
                        Item.PPMPMay = null;
                        Item.PPMPJun = null;
                        Item.PPMPJul = null;
                        Item.PPMPAug = "PR Preparation/Pre-Bid Conference";
                        Item.PPMPSep = "Procurement Activities";
                        Item.PPMPOct = "Procurement Activities";
                        Item.PPMPNov = "Delivery/Preparation of RIS";
                        Item.PPMPDec = null;
                    }
                    break;
                case 12:
                    {
                        Item.PPMPJan = null;
                        Item.PPMPFeb = null;
                        Item.PPMPMar = null;
                        Item.PPMPApr = null;
                        Item.PPMPMay = null;
                        Item.PPMPJun = null;
                        Item.PPMPJul = null;
                        Item.PPMPAug = null;
                        Item.PPMPSep = "PR Preparation/Pre-Bid Conference";
                        Item.PPMPOct = "Procurement Activities";
                        Item.PPMPNov = "Procurement Activities";
                        Item.PPMPDec = "Delivery/Preparation of RIS";
                    }
                    break;
            }
            return Item;
        }
        public ProjectPlanServices AssignPPMPServiceMilestones(ProjectPlanServices Item)
        {
            var StartMonth = db.ProjectPlans.Where(d => d.ID == Item.FKProjectReference.ID).FirstOrDefault().ProjectMonthStart;
            var FiscalYear = db.ProjectPlans.Where(d => d.ID == Item.FKProjectReference.ID).FirstOrDefault().FiscalYear - 1;
            switch (StartMonth)
            {
                case 1:
                    {
                        Item.PPMPJan = "PR Preparation/Pre-Bid Conference - (Oct, " + FiscalYear + "); Procurement Activities - (Nov-Dec, " + FiscalYear + "); Delivery/Preparation of RIS";
                        Item.PPMPFeb = null;
                        Item.PPMPMar = null;
                        Item.PPMPApr = null;
                        Item.PPMPMay = null;
                        Item.PPMPJun = null;
                        Item.PPMPJul = null;
                        Item.PPMPAug = null;
                        Item.PPMPSep = null;
                        Item.PPMPOct = null;
                        Item.PPMPNov = null;
                        Item.PPMPDec = null;
                    }
                    break;
                case 2:
                    {
                        Item.PPMPJan = "PR Preparation/Pre-Bid Conference - (Nov, " + FiscalYear + "); Procurement Activities - (Dec, " + FiscalYear + " - Jan, " + (FiscalYear + 1) + ");";
                        Item.PPMPFeb = "Delivery/Preparation of RIS";
                        Item.PPMPMar = null;
                        Item.PPMPApr = null;
                        Item.PPMPMay = null;
                        Item.PPMPJun = null;
                        Item.PPMPJul = null;
                        Item.PPMPAug = null;
                        Item.PPMPSep = null;
                        Item.PPMPOct = null;
                        Item.PPMPNov = null;
                        Item.PPMPDec = null;
                    }
                    break;
                case 3:
                    {
                        Item.PPMPJan = "PR Preparation/Pre-Bid Conference - (Dec, " + FiscalYear + "); Procurement Activities";
                        Item.PPMPFeb = "Procurement Activities";
                        Item.PPMPMar = "Delivery/Preparation of RIS";
                        Item.PPMPApr = null;
                        Item.PPMPMay = null;
                        Item.PPMPJun = null;
                        Item.PPMPJul = null;
                        Item.PPMPAug = null;
                        Item.PPMPSep = null;
                        Item.PPMPOct = null;
                        Item.PPMPNov = null;
                        Item.PPMPDec = null;
                    }
                    break;
                case 4:
                    {
                        Item.PPMPJan = "PR Preparation/Pre-Bid Conference";
                        Item.PPMPFeb = "Procurement Activities";
                        Item.PPMPMar = "Procurement Activities";
                        Item.PPMPApr = "Delivery/Preparation of RIS";
                        Item.PPMPMay = null;
                        Item.PPMPJun = null;
                        Item.PPMPJul = null;
                        Item.PPMPAug = null;
                        Item.PPMPSep = null;
                        Item.PPMPOct = null;
                        Item.PPMPNov = null;
                        Item.PPMPDec = null;
                    }
                    break;
                case 5:
                    {
                        Item.PPMPJan = null;
                        Item.PPMPFeb = "PR Preparation/Pre-Bid Conference";
                        Item.PPMPMar = "Procurement Activities";
                        Item.PPMPApr = "Procurement Activities";
                        Item.PPMPMay = "Delivery/Preparation of RIS";
                        Item.PPMPJun = null;
                        Item.PPMPJul = null;
                        Item.PPMPAug = null;
                        Item.PPMPSep = null;
                        Item.PPMPOct = null;
                        Item.PPMPNov = null;
                        Item.PPMPDec = null;
                    }
                    break;
                case 6:
                    {
                        Item.PPMPJan = null;
                        Item.PPMPFeb = null;
                        Item.PPMPMar = "PR Preparation/Pre-Bid Conference";
                        Item.PPMPApr = "Procurement Activities";
                        Item.PPMPMay = "Procurement Activities";
                        Item.PPMPJun = "Delivery/Preparation of RIS";
                        Item.PPMPJul = null;
                        Item.PPMPAug = null;
                        Item.PPMPSep = null;
                        Item.PPMPOct = null;
                        Item.PPMPNov = null;
                        Item.PPMPDec = null;
                    }
                    break;
                case 7:
                    {
                        Item.PPMPJan = null;
                        Item.PPMPFeb = null;
                        Item.PPMPMar = null;
                        Item.PPMPApr = "PR Preparation/Pre-Bid Conference";
                        Item.PPMPMay = "Procurement Activities";
                        Item.PPMPJun = "Procurement Activities";
                        Item.PPMPJul = "Delivery/Preparation of RIS";
                        Item.PPMPAug = null;
                        Item.PPMPSep = null;
                        Item.PPMPOct = null;
                        Item.PPMPNov = null;
                        Item.PPMPDec = null;
                    }
                    break;
                case 8:
                    {
                        Item.PPMPJan = null;
                        Item.PPMPFeb = null;
                        Item.PPMPMar = null;
                        Item.PPMPApr = null;
                        Item.PPMPMay = "PR Preparation/Pre-Bid Conference";
                        Item.PPMPJun = "Procurement Activities";
                        Item.PPMPJul = "Procurement Activities";
                        Item.PPMPAug = "Delivery/Preparation of RIS";
                        Item.PPMPSep = null;
                        Item.PPMPOct = null;
                        Item.PPMPNov = null;
                        Item.PPMPDec = null;
                    }
                    break;
                case 9:
                    {
                        Item.PPMPJan = null;
                        Item.PPMPFeb = null;
                        Item.PPMPMar = null;
                        Item.PPMPApr = null;
                        Item.PPMPMay = null;
                        Item.PPMPJun = "PR Preparation/Pre-Bid Conference";
                        Item.PPMPJul = "Procurement Activities";
                        Item.PPMPAug = "Procurement Activities";
                        Item.PPMPSep = "Delivery/Preparation of RIS";
                        Item.PPMPOct = null;
                        Item.PPMPNov = null;
                        Item.PPMPDec = null;
                    }
                    break;
                case 10:
                    {
                        Item.PPMPJan = null;
                        Item.PPMPFeb = null;
                        Item.PPMPMar = null;
                        Item.PPMPApr = null;
                        Item.PPMPMay = null;
                        Item.PPMPJun = null;
                        Item.PPMPJul = "PR Preparation/Pre-Bid Conference";
                        Item.PPMPAug = "Procurement Activities";
                        Item.PPMPSep = "Procurement Activities";
                        Item.PPMPOct = "Delivery/Preparation of RIS";
                        Item.PPMPNov = null;
                        Item.PPMPDec = null;
                    }
                    break;
                case 11:
                    {
                        Item.PPMPJan = null;
                        Item.PPMPFeb = null;
                        Item.PPMPMar = null;
                        Item.PPMPApr = null;
                        Item.PPMPMay = null;
                        Item.PPMPJun = null;
                        Item.PPMPJul = null;
                        Item.PPMPAug = "PR Preparation/Pre-Bid Conference";
                        Item.PPMPSep = "Procurement Activities";
                        Item.PPMPOct = "Procurement Activities";
                        Item.PPMPNov = "Delivery/Preparation of RIS";
                        Item.PPMPDec = null;
                    }
                    break;
                case 12:
                    {
                        Item.PPMPJan = null;
                        Item.PPMPFeb = null;
                        Item.PPMPMar = null;
                        Item.PPMPApr = null;
                        Item.PPMPMay = null;
                        Item.PPMPJun = null;
                        Item.PPMPJul = null;
                        Item.PPMPAug = null;
                        Item.PPMPSep = "PR Preparation/Pre-Bid Conference";
                        Item.PPMPOct = "Procurement Activities";
                        Item.PPMPNov = "Procurement Activities";
                        Item.PPMPDec = "Delivery/Preparation of RIS";
                    }
                    break;
            }
            return Item;
        }
        public string GenerateReferenceNo(int FiscalYear, string DepartmentCode, string TypeCode)
        {
            string referenceNo = string.Empty;
            string seqNo = (db.PPMPHeader.Where(d => d.FiscalYear == FiscalYear && d.Department == DepartmentCode).Count() + 1).ToString();
            seqNo = seqNo.ToString().Length == 3 ? seqNo : seqNo.ToString().Length == 2 ? "0" + seqNo.ToString() : "00" + seqNo.ToString();
            referenceNo = "PPMP-" + TypeCode + "-" + DepartmentCode + "-" + seqNo + "-" + FiscalYear.ToString();
            return referenceNo;
        }
        public bool SubmitPPMP(string ReferenceNo, string UserEmail)
        {
            var ppmp = db.PPMPHeader.Where(d => d.ReferenceNo == ReferenceNo).FirstOrDefault();
            var user = db.UserAccounts.Where(d => d.Email == UserEmail).FirstOrDefault();
            var office = hrisDataAccess.GetFullDepartmentDetails(user.DepartmentCode);
            ppmp.Status = "PPMP Submitted";
            ppmp.SubmittedAt = DateTime.Now;
            ppmp.SubmittedBy = office.DepartmentHead + ", " + office.DepartmentHeadDesignation;

            var switchBoard = db.SwitchBoard.Where(d => d.Reference == ReferenceNo).FirstOrDefault();
            var employee = hrisDataAccess.GetEmployee(UserEmail);
            var newProjectSwitchBody = new SwitchBoardBody
            {
                SwitchBoardReference = switchBoard.ID,
                ActionBy = employee.EmployeeCode,
                Remarks = ReferenceNo + " has been submitted by " + employee.EmployeeName + ", " + employee.Designation + ".  (" + DateTime.Now.ToString("dd MMMM yyyy hh:mm tt") + ")",
                DepartmentCode = employee.DepartmentCode,
                UpdatedAt = DateTime.Now
            };
            db.SwitchBoardBody.Add(newProjectSwitchBody);
            if (db.SaveChanges() == 0)
            {
                return false;
            }

            return true;
        }
        public bool PostToPPMP(ProjectPlanVM Project, string UserEmail)
        {
            var currentUser = db.UserAccounts.Where(d => d.Email == UserEmail).FirstOrDefault();
            var employee = hrisDataAccess.GetEmployee(currentUser.Email);
            var office = hrisDataAccess.GetFullDepartmentDetails(Project.UnitCode == null ? Project.DepartmentCode : Project.UnitCode);
            var projectItemInventoryTypes = db.ProjectPlanItems.Where(d => d.FKProjectReference.ProjectCode == Project.ProjectCode)
                                            .GroupBy(d => d.FKItemReference.FKItemTypeReference.FKInventoryTypeReference.ID)
                                            .Select(d => d.Key)
                                            .ToList();
            var projectServiceTypes = db.ProjectPlanServices.Where(d => d.FKProjectReference.ProjectCode == Project.ProjectCode)
                                            .GroupBy(d => d.FKItemReference.FKItemTypeReference.FKInventoryTypeReference.ID)
                                            .Select(d => d.Key)
                                            .ToList();
            var PPMPHeaderRefNo = string.Empty;
            SwitchBoard newProjectSwitch = new SwitchBoard();
            SwitchBoardBody PPMPSwitchBody = new SwitchBoardBody();

            projectItemInventoryTypes.AddRange(projectServiceTypes);

            foreach(var inventoryID in projectItemInventoryTypes)
            {
                var inventoryType = db.InventoryTypes.Find(inventoryID);
                if(inventoryType.IsTangible == true)
                {
                    var projectItems = db.ProjectPlanItems.Where(d => d.FKItemReference.FKItemTypeReference.InventoryTypeReference == inventoryID && d.FKProjectReference.ProjectCode == Project.ProjectCode).ToList();
                    if(projectItems != null)
                    {
                        var PPMPHeader = db.PPMPHeader.Where(d => d.PPMPType == inventoryID && d.FiscalYear == Project.FiscalYear && d.Department == office.DepartmentCode && d.Status == "New PPMP").FirstOrDefault();
                        if (PPMPHeader == null)
                        {
                            PPMPHeader = new PPMPHeader()
                            {
                                ReferenceNo = GenerateReferenceNo(Project.FiscalYear, office.DepartmentCode, inventoryType.InventoryCode),
                                FiscalYear = Project.FiscalYear,
                                Sector = office.SectorCode,
                                Department = office.DepartmentCode,
                                Unit = office.SectionCode,
                                PPMPType = inventoryID,
                                PreparedBy = Project.PreparedByEmpCode,
                                SubmittedBy = office.DepartmentHead.ToUpper() + ", " + office.DepartmentHeadDesignation,
                                CreatedAt = DateTime.Now,
                                Status = "New PPMP"
                            };

                            db.PPMPHeader.Add(PPMPHeader);
                            if (db.SaveChanges() == 0)
                            {
                                return false;
                            }
                            
                            newProjectSwitch = new SwitchBoard
                            {
                                DepartmentCode = PPMPHeader.Department,
                                MessageType = "PPMP",
                                Reference = PPMPHeader.ReferenceNo,
                                Subject = PPMPHeader.ReferenceNo + " - " + PPMPHeader.FKPPMPTypeReference.InventoryTypeName
                            };

                            db.SwitchBoard.Add(newProjectSwitch);
                            if (db.SaveChanges() == 0)
                            {
                                return false;
                            }

                            PPMPSwitchBody = new SwitchBoardBody
                            {
                                SwitchBoardReference = newProjectSwitch.ID,
                                ActionBy = employee.EmployeeCode,
                                Remarks = PPMPHeader.ReferenceNo + " - " + PPMPHeader.FKPPMPTypeReference.InventoryTypeName + " has been created by " + employee.EmployeeName + ", " + employee.Designation + ". (" + DateTime.Now.ToString("dd MMMM yyyy hh:mm tt") + ")",
                                DepartmentCode = employee.DepartmentCode,
                                UpdatedAt = DateTime.Now
                            };

                            db.SwitchBoardBody.Add(PPMPSwitchBody);
                            if (db.SaveChanges() == 0)
                            {
                                return false;
                            }
                        }

                        PPMPHeaderRefNo = PPMPHeader.ReferenceNo;
                        newProjectSwitch = new SwitchBoard
                        {
                            DepartmentCode = PPMPHeader.Department,
                            MessageType = "PPMP",
                            Reference = PPMPHeader.ReferenceNo,
                            Subject = PPMPHeader.ReferenceNo + " - " + PPMPHeader.FKPPMPTypeReference.InventoryTypeName
                        };

                        db.SwitchBoard.Add(newProjectSwitch);
                        if (db.SaveChanges() == 0)
                        {
                            return false;
                        }

                        PPMPSwitchBody = new SwitchBoardBody
                        {
                            SwitchBoardReference = newProjectSwitch.ID,
                            ActionBy = employee.EmployeeCode,
                            Remarks = PPMPHeader.ReferenceNo + " - " + PPMPHeader.FKPPMPTypeReference.InventoryTypeName + " has been created by " + employee.EmployeeName + ", " + employee.Designation + ". (" + DateTime.Now.ToString("dd MMMM yyyy hh:mm tt") + ")",
                            DepartmentCode = employee.DepartmentCode,
                            UpdatedAt = DateTime.Now
                        };

                        db.SwitchBoardBody.Add(PPMPSwitchBody);
                        if (db.SaveChanges() == 0)
                        {
                            return false;
                        }

                        foreach (var item in projectItems)
                        {
                            var projectItem = db.ProjectPlanItems.Where(d => d.ItemReference == item.ItemReference && d.FKProjectReference.ProjectCode == item.FKProjectReference.ProjectCode).FirstOrDefault();
                            if(projectItem.FKProjectReference.ProjectCode.StartsWith("CSPR"))
                            {
                                projectItem.PPMPJan = item.ProjectJanQty == null ? null : item.ProjectJanQty.ToString();
                                projectItem.PPMPFeb = item.ProjectFebQty == null ? null : item.ProjectFebQty.ToString();
                                projectItem.PPMPMar = item.ProjectMarQty == null ? null : item.ProjectMarQty.ToString();
                                projectItem.PPMPApr = item.ProjectAprQty == null ? null : item.ProjectAprQty.ToString();
                                projectItem.PPMPMay = item.ProjectMayQty == null ? null : item.ProjectMayQty.ToString();
                                projectItem.PPMPJun = item.ProjectJunQty == null ? null : item.ProjectJunQty.ToString();
                                projectItem.PPMPJul = item.ProjectJulQty == null ? null : item.ProjectJulQty.ToString();
                                projectItem.PPMPAug = item.ProjectAugQty == null ? null : item.ProjectAugQty.ToString();
                                projectItem.PPMPSep = item.ProjectSepQty == null ? null : item.ProjectSepQty.ToString();
                                projectItem.PPMPOct = item.ProjectOctQty == null ? null : item.ProjectOctQty.ToString();
                                projectItem.PPMPNov = item.ProjectNovQty == null ? null : item.ProjectNovQty.ToString();
                                projectItem.PPMPDec = item.ProjectDecQty == null ? null : item.ProjectDecQty.ToString();
                            }
                            else
                            {
                                projectItem = AssignPPMPItemMilestones(projectItem);
                            }
                            projectItem.PPMPReference = PPMPHeader.ID;
                            projectItem.PPMPTotalQty = item.ProjectTotalQty;
                            projectItem.PPMPEstimatedBudget = item.ProjectEstimatedBudget;
                            projectItem.Status = "Posted";
                            if (db.SaveChanges() == 0)
                            {
                                return false;
                            }
                        }
                    }
                }
                else
                {
                    var projectServices = db.ProjectPlanServices.Where(d => d.FKItemReference.FKItemTypeReference.FKInventoryTypeReference.ID == inventoryID && d.FKProjectReference.ProjectCode == Project.ProjectCode).ToList();
                    if (projectServices != null)
                    {
                        var PPMPHeader = db.PPMPHeader.Where(d => d.PPMPType == inventoryID && d.FiscalYear == Project.FiscalYear && d.Department == office.DepartmentCode && d.Status == "New PPMP").FirstOrDefault();
                        if (PPMPHeader == null)
                        {
                            PPMPHeader = new PPMPHeader()
                            {
                                ReferenceNo = GenerateReferenceNo(Project.FiscalYear, office.DepartmentCode, inventoryType.InventoryCode),
                                FiscalYear = Project.FiscalYear,
                                Sector = office.SectorCode,
                                Department = office.DepartmentCode,
                                Unit = office.SectionCode,
                                PPMPType = inventoryID,
                                PreparedBy = Project.PreparedByEmpCode,
                                SubmittedBy = office.DepartmentHead.ToUpper() + ", " + office.DepartmentHeadDesignation,
                                CreatedAt = DateTime.Now,
                                Status = "New PPMP"
                            };

                            db.PPMPHeader.Add(PPMPHeader);
                            if (db.SaveChanges() == 0)
                            {
                                return false;
                            }

                            newProjectSwitch = new SwitchBoard
                            {
                                DepartmentCode = PPMPHeader.Department,
                                MessageType = "PPMP",
                                Reference = PPMPHeader.ReferenceNo,
                                Subject = PPMPHeader.ReferenceNo + " - " + PPMPHeader.FKPPMPTypeReference.InventoryTypeName
                            };

                            db.SwitchBoard.Add(newProjectSwitch);
                            if (db.SaveChanges() == 0)
                            {
                                return false;
                            }

                            PPMPSwitchBody = new SwitchBoardBody
                            {
                                SwitchBoardReference = newProjectSwitch.ID,
                                ActionBy = employee.EmployeeCode,
                                Remarks = PPMPHeader.ReferenceNo + " - " + PPMPHeader.FKPPMPTypeReference.InventoryTypeName + " has been created by " + employee.EmployeeName + ", " + employee.Designation + ". (" + DateTime.Now.ToString("dd MMMM yyyy hh:mm tt") + ")",
                                DepartmentCode = employee.DepartmentCode,
                                UpdatedAt = DateTime.Now
                            };

                            db.SwitchBoardBody.Add(PPMPSwitchBody);
                            if (db.SaveChanges() == 0)
                            {
                                return false;
                            }
                        }

                        PPMPHeaderRefNo = PPMPHeader.ReferenceNo;
                        newProjectSwitch = new SwitchBoard
                        {
                            DepartmentCode = PPMPHeader.Department,
                            MessageType = "PPMP",
                            Reference = PPMPHeader.ReferenceNo,
                            Subject = PPMPHeader.ReferenceNo + " - " + PPMPHeader.FKPPMPTypeReference.InventoryTypeName
                        };

                        db.SwitchBoard.Add(newProjectSwitch);
                        if (db.SaveChanges() == 0)
                        {
                            return false;
                        }

                        PPMPSwitchBody = new SwitchBoardBody
                        {
                            SwitchBoardReference = newProjectSwitch.ID,
                            ActionBy = employee.EmployeeCode,
                            Remarks = PPMPHeader.ReferenceNo + " - " + PPMPHeader.FKPPMPTypeReference.InventoryTypeName + " has been created by " + employee.EmployeeName + ", " + employee.Designation + ". (" + DateTime.Now.ToString("dd MMMM yyyy hh:mm tt") + ")",
                            DepartmentCode = employee.DepartmentCode,
                            UpdatedAt = DateTime.Now
                        };

                        db.SwitchBoardBody.Add(PPMPSwitchBody);
                        if (db.SaveChanges() == 0)
                        {
                            return false;
                        }

                        foreach (var item in projectServices)
                        {
                            var projectService = db.ProjectPlanServices.Where(d => d.ItemReference == item.ItemReference && d.FKProjectReference.ProjectCode == item.FKProjectReference.ProjectCode).FirstOrDefault();
                            projectService = AssignPPMPServiceMilestones(projectService);
                            projectService.PPMPReference = PPMPHeader.ID;
                            projectService.PPMPQuantity = item.ProjectQuantity;
                            projectService.PPMPEstimatedBudget = item.ProjectEstimatedBudget;
                            projectService.Status = "Posted";
                            if (db.SaveChanges() == 0)
                            {
                                return false;
                            }
                        }
                    }
                }

                var switchBoard = db.SwitchBoard.Where(d => d.Reference == Project.ProjectCode).FirstOrDefault();
                var newProjectSwitchBody = new SwitchBoardBody
                {
                    SwitchBoardReference = switchBoard.ID,
                    ActionBy = employee.EmployeeCode,
                    Remarks = Project.ProjectCode + " " + inventoryType.InventoryTypeName + " items were posted in " + PPMPHeaderRefNo + " by " + employee.EmployeeName + ", " + employee.Designation + ". (Added Items) (" + DateTime.Now.ToString("dd MMMM yyyy hh:mm tt") + ")",
                    DepartmentCode = employee.DepartmentCode,
                    UpdatedAt = DateTime.Now
                };
                db.SwitchBoardBody.Add(newProjectSwitchBody);

                switchBoard = db.SwitchBoard.Where(d => d.Reference == PPMPHeaderRefNo).FirstOrDefault();
                newProjectSwitchBody = new SwitchBoardBody
                {
                    SwitchBoardReference = switchBoard.ID,
                    ActionBy = employee.EmployeeCode,
                    Remarks = PPMPHeaderRefNo + " has been updated by " + employee.EmployeeName + ", " + employee.Designation + ". (Added Items) (" + DateTime.Now.ToString("dd MMMM yyyy hh:mm tt") + ")",
                    DepartmentCode = employee.DepartmentCode,
                    UpdatedAt = DateTime.Now
                };
                db.SwitchBoardBody.Add(newProjectSwitchBody);
                if (db.SaveChanges() == 0)
                {
                    return false;
                }

            }

            return true;
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
                abdb.Dispose();
                hrisDataAccess.Dispose();
                accountsManagement.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}