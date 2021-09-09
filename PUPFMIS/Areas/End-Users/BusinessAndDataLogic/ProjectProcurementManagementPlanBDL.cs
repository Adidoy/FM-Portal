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
        private FMISDbContext db = new FMISDbContext();

        public MemoryStream PrintPPMP(string LogoPath, string ReferenceNo)
        {
            var ppmp = ppmpDAL.GetPPMPDetail(ReferenceNo);

            Reports reports = new Reports();
            reports.ReportFilename = ReferenceNo;
            reports.CreateDocument();
            reports.AddDoubleColumnHeader(LogoPath, false, 1.25);
            reports.AddColumnHeader(
                new HeaderLine { Content = "", Bold = false, Italic = false, FontSize = 5, ParagraphAlignment = ParagraphAlignment.Left },
                new HeaderLine { Content = "", Bold = true, Italic = false, FontSize = 5, ParagraphAlignment = ParagraphAlignment.Left }
            );
            reports.AddColumnHeader(
                new HeaderLine { Content = "Republic of the Philippines", Bold = false, Italic = false, FontSize = 8, ParagraphAlignment = ParagraphAlignment.Left },
                new HeaderLine { Content = "Rev. 0", Bold = true, Italic = false, FontSize = 6, ParagraphAlignment = ParagraphAlignment.Left }
            );
            reports.AddColumnHeader(
                new HeaderLine { Content = "POLYTECHNIC UNIVERSITY OF THE PHILIPPINES", Bold = true, Italic = false, FontSize = 12, ParagraphAlignment = ParagraphAlignment.Left },
                new HeaderLine { Content = "May 15, 2018", Bold = true, Italic = false, FontSize = 6, ParagraphAlignment = ParagraphAlignment.Left }
            );
            reports.AddColumnHeader(
                new HeaderLine { Content = "A. Mabini Campus, Anonas St., Santa Mesa, Manila\t1016", Bold = false, Italic = false, FontSize = 10, ParagraphAlignment = ParagraphAlignment.Left },
                new HeaderLine { Content = "BP FORM ", Bold = false, Italic = false, FontSize = 8, ParagraphAlignment = ParagraphAlignment.Left }
            );
            reports.AddColumnHeader(
                new HeaderLine { Content = "", Bold = false, Italic = false, FontSize = 8, ParagraphAlignment = ParagraphAlignment.Left },
                new HeaderLine { Content = "REFERENCE NO: " + ppmp.Header.ReferenceNo, Bold = false, Italic = false, FontSize = 8, ParagraphAlignment = ParagraphAlignment.Left }
            );
            reports.AddColumnHeader(
                new HeaderLine { Content = "", Bold = false, Italic = false, FontSize = 8, ParagraphAlignment = ParagraphAlignment.Left },
                new HeaderLine { Content = "Date Printed: " + DateTime.Now.ToString("dd MMMM yyyy hh:mm tt"), Bold = false, Italic = false, FontSize = 8, ParagraphAlignment = ParagraphAlignment.Left }
            );

            reports.AddNewLine();

            reports.AddSingleColumnHeader();
            reports.AddColumnHeader(
                new HeaderLine { Content = "PROJECT PROCUREMENT MANAGEMENT PLAN", Bold = false, Italic = false, FontSize = 10, ParagraphAlignment = ParagraphAlignment.Center }
            );
            reports.AddColumnHeader(
                new HeaderLine { Content = ppmp.Header.ProjectName.ToUpper(), Bold = true, Italic = false, FontSize = 10, ParagraphAlignment = ParagraphAlignment.Center }
            );
            reports.AddColumnHeader(
                new HeaderLine { Content = "Fiscal Year " + ppmp.Header.FiscalYear.ToString(), Bold = false, Italic = false, FontSize = 8, ParagraphAlignment = ParagraphAlignment.Center }
            );

            reports.AddNewLine();

            var columns = new List<ContentColumn>();
            var rows = new List<ContentCell>();

            columns = new List<ContentColumn>();
            columns.Add(new ContentColumn(1.00));
            columns.Add(new ContentColumn(5.50));
            columns.Add(new ContentColumn(6.00));
            reports.AddTable(columns, false);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("End-User: ", 0, 10, true));
            rows.Add(new ContentCell(ppmp.Header.Department.ToUpper(), 1, 10, true, true, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, true));
            rows.Add(new ContentCell("", 2));
            reports.AddRowContent(rows, 0);

            reports.AddNewLine();

            if (ppmp.Header.PPMPType == PPMPTypes.CommonUse)
            {
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

                var dbmItems = ppmp.Details.Where(d => d.ProcurementSource == ProcurementSources.AgencyToAgency).OrderBy(d => d.PAPCode).ToList();
                if (dbmItems.Count == 0)
                {
                    columns = new List<ContentColumn>();
                    columns.Add(new ContentColumn(12.5));
                    reports.AddTable(columns, true);

                    rows = new List<ContentCell>();
                    rows.Add(new ContentCell("**** NO ITEMS ****", 0, 7, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                    reports.AddRowContent(rows, 0.25);
                }
                else
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

                    foreach (var item in dbmItems)
                    {
                        rows = new List<ContentCell>();
                        rows.Add(new ContentCell(item.PAPCode, 0, 7, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                        rows.Add(new ContentCell(item.ItemFullName, 1, 7, true, false, ParagraphAlignment.Left, VerticalAlignment.Center));
                        rows.Add(new ContentCell(item.UOMReference, 2, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                        rows.Add(new ContentCell(item.JanQty.ToString(), 3, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                        rows.Add(new ContentCell(item.FebQty.ToString(), 4, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                        rows.Add(new ContentCell(item.MarQty.ToString(), 5, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                        rows.Add(new ContentCell(item.AprQty.ToString(), 6, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                        rows.Add(new ContentCell(item.MayQty.ToString(), 7, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                        rows.Add(new ContentCell(item.JunQty.ToString(), 8, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                        rows.Add(new ContentCell(item.JulQty.ToString(), 9, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                        rows.Add(new ContentCell(item.AugQty.ToString(), 10, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                        rows.Add(new ContentCell(item.SepQty.ToString(), 11, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                        rows.Add(new ContentCell(item.OctQty.ToString(), 12, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                        rows.Add(new ContentCell(item.NovQty.ToString(), 13, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                        rows.Add(new ContentCell(item.DecQty.ToString(), 14, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                        rows.Add(new ContentCell(item.TotalQty.ToString(), 15, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                        rows.Add(new ContentCell(item.UnitCost.ToString("G"), 16, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                        rows.Add(new ContentCell(item.EstimatedBudget.ToString("G"), 17, 7, false, false, ParagraphAlignment.Right, VerticalAlignment.Center));
                        rows.Add(new ContentCell(item.Justification, 18, 7, false, false, ParagraphAlignment.Left, VerticalAlignment.Top));
                        reports.AddRowContent(rows, 0.25);
                    }
                }

                columns = new List<ContentColumn>();
                columns.Add(new ContentColumn(10.41, new MigraDoc.DocumentObjectModel.Color(161, 197, 255)));
                columns.Add(new ContentColumn(1.00, new MigraDoc.DocumentObjectModel.Color(161, 197, 255)));
                columns.Add(new ContentColumn(1.09, new MigraDoc.DocumentObjectModel.Color(161, 197, 255)));
                reports.AddTable(columns, true);

                rows = new List<ContentCell>();
                rows.Add(new ContentCell("SUB-TOTAL: ", 0, 8.5, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Right, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                rows.Add(new ContentCell(dbmItems.Sum(d => d.EstimatedBudget).ToString("C", new System.Globalization.CultureInfo("en-ph")), 1, 8.5, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Right, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                rows.Add(new ContentCell("", 2));
                reports.AddRowContent(rows, 0.25);

                columns = new List<ContentColumn>();
                columns.Add(new ContentColumn(12.5, new MigraDoc.DocumentObjectModel.Color(252, 207, 101)));
                reports.AddTable(columns, true);

                rows = new List<ContentCell>();
                rows.Add(new ContentCell("PART II. OTHER ITEMS NOT AVALABLE AT PS BUT REGULARLY PURCHASED FROM OTHER SOURCES (Note: Please indicate price of items)", 0, 7, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Left, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center, 0, 0));
                reports.AddRowContent(rows, 0.25);

                var nonDBMItems = ppmp.Details.Where(d => d.ProcurementSource == ProcurementSources.ExternalSuppliers).OrderBy(d => d.PAPCode).ToList();
                if (nonDBMItems.Count == 0)
                {
                    columns = new List<ContentColumn>();
                    columns.Add(new ContentColumn(12.5));
                    reports.AddTable(columns, true);

                    rows = new List<ContentCell>();
                    rows.Add(new ContentCell("**** NO ITEMS ****", 0, 7, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                    reports.AddRowContent(rows, 0.25);
                }
                else
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

                    foreach (var item in nonDBMItems)
                    {
                        rows = new List<ContentCell>();
                        rows.Add(new ContentCell(item.PAPCode, 0, 7, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                        if (item.ArticleCode == null)
                        {
                            rows.Add(new ContentCell(item.ItemFullName, 1, 8, true, false, ParagraphAlignment.Left, VerticalAlignment.Top));
                        }
                        else
                        {
                            rows.Add(new ContentCell(new TextWithFormat[]
                            {
                                new TextWithFormat(item.ItemFullName + "\n\n", true, false, 8),
                                new TextWithFormat(item.ItemSpecifications == null ? string.Empty : item.ItemSpecifications, false, true, 7)
                            }, 1, ParagraphAlignment.Left, VerticalAlignment.Top));
                        }
                        rows.Add(new ContentCell(item.UOMReference, 2, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                        rows.Add(new ContentCell(item.JanQty.ToString(), 3, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                        rows.Add(new ContentCell(item.FebQty.ToString(), 4, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                        rows.Add(new ContentCell(item.MarQty.ToString(), 5, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                        rows.Add(new ContentCell(item.AprQty.ToString(), 6, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                        rows.Add(new ContentCell(item.MayQty.ToString(), 7, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                        rows.Add(new ContentCell(item.JunQty.ToString(), 8, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                        rows.Add(new ContentCell(item.JulQty.ToString(), 9, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                        rows.Add(new ContentCell(item.AugQty.ToString(), 10, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                        rows.Add(new ContentCell(item.SepQty.ToString(), 11, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                        rows.Add(new ContentCell(item.OctQty.ToString(), 12, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                        rows.Add(new ContentCell(item.NovQty.ToString(), 13, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                        rows.Add(new ContentCell(item.DecQty.ToString(), 14, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                        rows.Add(new ContentCell(item.TotalQty.ToString(), 15, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                        rows.Add(new ContentCell(item.UnitCost.ToString("N"), 16, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                        rows.Add(new ContentCell(item.EstimatedBudget.ToString("N"), 17, 7, false, false, ParagraphAlignment.Right, VerticalAlignment.Center));
                        rows.Add(new ContentCell(item.Justification, 18, 7, false, false, ParagraphAlignment.Left, VerticalAlignment.Top));
                        reports.AddRowContent(rows, 0.25);
                    }
                }

                columns = new List<ContentColumn>();
                columns.Add(new ContentColumn(10.41, new MigraDoc.DocumentObjectModel.Color(161, 197, 255)));
                columns.Add(new ContentColumn(1.00, new MigraDoc.DocumentObjectModel.Color(161, 197, 255)));
                columns.Add(new ContentColumn(1.09, new MigraDoc.DocumentObjectModel.Color(161, 197, 255)));
                reports.AddTable(columns, true);

                rows = new List<ContentCell>();
                rows.Add(new ContentCell("SUB-TOTAL: ", 0, 8.5, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Right, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                rows.Add(new ContentCell(nonDBMItems.Sum(d => d.EstimatedBudget).ToString("C", new System.Globalization.CultureInfo("en-ph")), 1, 8.5, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Right, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                rows.Add(new ContentCell("", 2));
                reports.AddRowContent(rows, 0.25);

                rows = new List<ContentCell>();
                rows.Add(new ContentCell("TOTAL ESTIMATED BUDGET: ", 0, 8.5, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Right, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                rows.Add(new ContentCell((dbmItems.Sum(d => d.EstimatedBudget) + nonDBMItems.Sum(d => d.EstimatedBudget)).ToString("C", new System.Globalization.CultureInfo("en-ph")), 1, 8.5, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Right, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                rows.Add(new ContentCell("", 2));
                reports.AddRowContent(rows, 0.25);
            }
            else
            {
                columns = new List<ContentColumn>();
                columns.Add(new ContentColumn(1.00, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
                columns.Add(new ContentColumn(2.00, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
                columns.Add(new ContentColumn(0.75, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
                columns.Add(new ContentColumn(1.00, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
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
                columns.Add(new ContentColumn(1.75, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
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

                var dbmDetails = ppmp.Details.Where(d => d.ProcurementSource == ProcurementSources.AgencyToAgency).OrderBy(d => d.ItemFullName).ToList();
                if (dbmDetails.Count == 0)
                {
                    columns = new List<ContentColumn>();
                    columns.Add(new ContentColumn(12.5));
                    reports.AddTable(columns, true);

                    rows = new List<ContentCell>();
                    rows.Add(new ContentCell("**** NO ITEMS ****", 0, 7, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                    reports.AddRowContent(rows, 0.25);
                }
                else
                {
                    columns = new List<ContentColumn>();
                    columns.Add(new ContentColumn(1.00));
                    columns.Add(new ContentColumn(2.00));
                    columns.Add(new ContentColumn(0.75));
                    columns.Add(new ContentColumn(1.00));
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
                    columns.Add(new ContentColumn(1.75));
                    reports.AddTable(columns, true);

                    foreach (var item in dbmDetails)
                    {
                        rows = new List<ContentCell>();
                        rows.Add(new ContentCell(item.PAPCode, 0, 7, true, false, ParagraphAlignment.Center, VerticalAlignment.Top)); 
                        if (item.ArticleCode == null)
                        {
                            rows.Add(new ContentCell(item.ItemFullName, 1, 8, true, false, ParagraphAlignment.Left, VerticalAlignment.Top));
                        }
                        else
                        {
                            rows.Add(new ContentCell(new TextWithFormat[]
                            {
                                new TextWithFormat(item.ItemFullName + "\n\n", true, false, 8),
                                new TextWithFormat(item.ItemSpecifications == null ? string.Empty : item.ItemSpecifications, false, true, 7)
                            }, 1, ParagraphAlignment.Left, VerticalAlignment.Top));
                        }
                        rows.Add(new ContentCell(item.TotalQty + "\n" + item.UOMReference, 2, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Top));
                        rows.Add(new ContentCell(item.EstimatedBudget.ToString("N"), 3, 8, false, false, ParagraphAlignment.Right, VerticalAlignment.Top));
                        rows.Add(new ContentCell(item.JANMilestone, 4, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Top));
                        rows.Add(new ContentCell(item.FEBMilestone, 5, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Top));
                        rows.Add(new ContentCell(item.MARMilestone, 6, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Top));
                        rows.Add(new ContentCell(item.APRMilestone, 7, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Top));
                        rows.Add(new ContentCell(item.MAYMilestone, 8, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Top));
                        rows.Add(new ContentCell(item.JUNMilestone, 9, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Top));
                        rows.Add(new ContentCell(item.JULMilestone, 10, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Top));
                        rows.Add(new ContentCell(item.AUGMilestone, 11, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Top));
                        rows.Add(new ContentCell(item.SEPMilestone, 12, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Top));
                        rows.Add(new ContentCell(item.OCTMilestone, 13, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Top));
                        rows.Add(new ContentCell(item.NOVMilestone, 14, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Top));
                        rows.Add(new ContentCell(item.NOVMilestone, 15, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Top));
                        rows.Add(new ContentCell(item.Justification, 16, 7, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top));
                        reports.AddRowContent(rows, 0.30);
                    }
                }

                columns = new List<ContentColumn>();
                columns.Add(new ContentColumn(3.75, new MigraDoc.DocumentObjectModel.Color(161, 197, 255)));
                columns.Add(new ContentColumn(1.00, new MigraDoc.DocumentObjectModel.Color(161, 197, 255)));
                columns.Add(new ContentColumn(7.75, new MigraDoc.DocumentObjectModel.Color(161, 197, 255)));
                reports.AddTable(columns, true);

                rows = new List<ContentCell>();
                rows.Add(new ContentCell("SUB-TOTAL: ", 0, 8.5, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Right, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                rows.Add(new ContentCell(dbmDetails.Sum(d => d.EstimatedBudget).ToString("C", new System.Globalization.CultureInfo("en-ph")), 1, 8.5, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Right, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                rows.Add(new ContentCell("", 2));
                reports.AddRowContent(rows, 0.25);

                columns = new List<ContentColumn>();
                columns.Add(new ContentColumn(12.5, new MigraDoc.DocumentObjectModel.Color(252, 207, 101)));
                reports.AddTable(columns, true);

                rows = new List<ContentCell>();
                rows.Add(new ContentCell("PART II. OTHER ITEMS NOT AVALABLE AT PS BUT REGULARLY PURCHASED FROM OTHER SOURCES (Note: Please indicate price of items)", 0, 7, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Left, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center, 0, 0));
                reports.AddRowContent(rows, 0.25);

                var nonDBMDetails = ppmp.Details.Where(d => d.ProcurementSource == ProcurementSources.ExternalSuppliers).OrderBy(d => d.ItemFullName).ToList();
                if (nonDBMDetails.Count == 0)
                {
                    columns = new List<ContentColumn>();
                    columns.Add(new ContentColumn(12.5));
                    reports.AddTable(columns, true);

                    rows = new List<ContentCell>();
                    rows.Add(new ContentCell("**** NO ITEMS ****", 0, 7, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                    reports.AddRowContent(rows, 0.25);
                }
                else
                {
                    columns = new List<ContentColumn>();
                    columns.Add(new ContentColumn(1.00));
                    columns.Add(new ContentColumn(2.00));
                    columns.Add(new ContentColumn(0.75));
                    columns.Add(new ContentColumn(1.00));
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
                    columns.Add(new ContentColumn(1.75));
                    reports.AddTable(columns, true);

                    foreach (var item in nonDBMDetails)
                    {
                        
                        rows = new List<ContentCell>();
                        rows.Add(new ContentCell(item.PAPCode, 0, 7, true, false, ParagraphAlignment.Center, VerticalAlignment.Top));
                        if(item.ArticleCode == null)
                        {
                            rows.Add(new ContentCell(item.ItemFullName, 1, 8, true, false, ParagraphAlignment.Left, VerticalAlignment.Top));
                        }
                        else
                        {
                            rows.Add(new ContentCell(new TextWithFormat[]
                            {
                                new TextWithFormat(item.ItemFullName + "\n\n", true, false, 8),
                                new TextWithFormat(item.ItemSpecifications == null ? string.Empty : item.ItemSpecifications, false, true, 7)
                            }, 1, ParagraphAlignment.Left, VerticalAlignment.Top));
                        }
                        rows.Add(new ContentCell(item.TotalQty + "\n" + item.UOMReference, 2, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Top));
                        rows.Add(new ContentCell(item.EstimatedBudget.ToString("N"), 3, 8, false, false, ParagraphAlignment.Right, VerticalAlignment.Top));
                        rows.Add(new ContentCell(item.JANMilestone, 4, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Top));
                        rows.Add(new ContentCell(item.FEBMilestone, 5, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Top));
                        rows.Add(new ContentCell(item.MARMilestone, 6, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Top));
                        rows.Add(new ContentCell(item.APRMilestone, 7, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Top));
                        rows.Add(new ContentCell(item.MAYMilestone, 8, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Top));
                        rows.Add(new ContentCell(item.JUNMilestone, 9, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Top));
                        rows.Add(new ContentCell(item.JULMilestone, 10, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Top));
                        rows.Add(new ContentCell(item.AUGMilestone, 11, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Top));
                        rows.Add(new ContentCell(item.SEPMilestone, 12, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Top));
                        rows.Add(new ContentCell(item.OCTMilestone, 13, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Top));
                        rows.Add(new ContentCell(item.NOVMilestone, 14, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Top));
                        rows.Add(new ContentCell(item.NOVMilestone, 15, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Top));
                        if (item.ArticleCode == null)
                        {
                            rows.Add(new ContentCell(string.Empty, 16, 7, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top));
                        }
                        else
                        {
                            rows.Add(new ContentCell(item.Justification == null ? string.Empty : item.Justification, 16, 7, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top));
                        }
                        reports.AddRowContent(rows, 0.30);
                    }
                }

                columns = new List<ContentColumn>();
                columns.Add(new ContentColumn(3.75, new MigraDoc.DocumentObjectModel.Color(161, 197, 255)));
                columns.Add(new ContentColumn(1.00, new MigraDoc.DocumentObjectModel.Color(161, 197, 255)));
                columns.Add(new ContentColumn(7.75, new MigraDoc.DocumentObjectModel.Color(161, 197, 255)));
                reports.AddTable(columns, true);

                rows = new List<ContentCell>();
                rows.Add(new ContentCell("SUB-TOTAL: ", 0, 8.5, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Right, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                rows.Add(new ContentCell(nonDBMDetails.Sum(d => d.EstimatedBudget).ToString("C", new System.Globalization.CultureInfo("en-ph")), 1, 8.5, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Right, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                rows.Add(new ContentCell("", 2));
                reports.AddRowContent(rows, 0.25);

                rows = new List<ContentCell>();
                rows.Add(new ContentCell("TOTAL ESTIMATED BUDGET: ", 0, 8.5, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Right, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                rows.Add(new ContentCell((dbmDetails.Sum(d => d.EstimatedBudget) + nonDBMDetails.Sum(d => d.EstimatedBudget)).ToString("C", new System.Globalization.CultureInfo("en-ph")), 1, 8.5, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Right, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                rows.Add(new ContentCell("", 2));
                reports.AddRowContent(rows, 0.25);

                reports.AddNewLine();

                reports.AddDoubleColumnHeader();
                reports.AddColumnHeader(
                    new HeaderLine { Content = "SCHDULE/MILESTONE LEDGEND:", Bold = true, Italic = true, FontSize = 6, ParagraphAlignment = ParagraphAlignment.Left },
                    new HeaderLine { Content = "", Bold = false, Italic = true, FontSize = 6, ParagraphAlignment = ParagraphAlignment.Left }
                );
                reports.AddColumnHeader(
                    new HeaderLine { Content = "EPA-OCT - Early Procurement Activities starting OCT " + (ppmp.Header.FiscalYear - 1).ToString(), Bold = false, Italic = true, FontSize = 6, ParagraphAlignment = ParagraphAlignment.Left },
                    new HeaderLine { Content = "PRPC - P/R Preparation / Pre-Procurement Conference", Bold = false, Italic = true, FontSize = 6, ParagraphAlignment = ParagraphAlignment.Left }
                );
                reports.AddColumnHeader(
                    new HeaderLine { Content = "EPA-NOV - Early Procurement Activities starting NOV " + (ppmp.Header.FiscalYear - 1).ToString(), Bold = false, Italic = true, FontSize = 6, ParagraphAlignment = ParagraphAlignment.Left },
                    new HeaderLine { Content = "PRAC - Procurement Activities", Bold = false, Italic = true, FontSize = 6, ParagraphAlignment = ParagraphAlignment.Left }
                );
                reports.AddColumnHeader(
                    new HeaderLine { Content = "EPA-DEC - Early Procurement Activities starting DEC " + (ppmp.Header.FiscalYear - 1).ToString(), Bold = false, Italic = true, FontSize = 6, ParagraphAlignment = ParagraphAlignment.Left },
                    new HeaderLine { Content = "DRIS - Delivery / Preparation of RIS", Bold = false, Italic = true, FontSize = 6, ParagraphAlignment = ParagraphAlignment.Left }
                );
            }

            reports.AddNewLine();
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

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("", 0));
            rows.Add(new ContentCell(ppmp.Header.PreparedBy, 1, 8.5, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
            rows.Add(new ContentCell("", 2));
            rows.Add(new ContentCell(ppmp.Header.SubmittedBy, 3, 8.5, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
            rows.Add(new ContentCell("", 4));
            reports.AddRowContent(rows, 0);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("", 0));
            rows.Add(new ContentCell(ppmp.Header.PreparedByDesignation, 1, 8.5, false, true, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
            rows.Add(new ContentCell("", 2));
            rows.Add(new ContentCell(ppmp.Header.SubmittedByDesignation, 3, 8.5, false, true, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
            rows.Add(new ContentCell("", 4));
            reports.AddRowContent(rows, 0);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("", 0));
            rows.Add(new ContentCell(ppmp.Header.CreatedAt.ToString("dd MMMM yyyy hh:mm tt"), 1, 7, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
            rows.Add(new ContentCell("", 2));
            rows.Add(new ContentCell((ppmp.Header.SubmittedAt == null ? "(Submission Pending)" : ((DateTime)ppmp.Header.SubmittedAt).ToString("dd MMMM yyyy hh:mm tt")), 3, 7, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
            rows.Add(new ContentCell("", 4));
            reports.AddRowContent(rows, 0);

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
        private ABISDataAccess abis = new ABISDataAccess();
        private HRISDataAccess hris = new HRISDataAccess();

        public PPMPDashboard GetDashboardValues(int FiscalYear, string UserEmail)
        {
            var dashboard = new PPMPDashboard();
            dashboard.FiscalYears = GetFiscalYears();
            dashboard.TotalNoOfPPMPs = GetTotalPPMPs();
            dashboard.TotalOrigianalPPMPs = GetTotalOriginalPPMPs();
            dashboard.TotalSupplementalPPMPs = GetTotalSupplementalPPMPs();
            return dashboard;
        }
        public List<int> GetFiscalYears()
        {
            return db.PPMPHeader.OrderByDescending(d => d.FiscalYear).Select(d => d.FiscalYear).Distinct().ToList();
        }
        public int GetTotalPPMPs()
        {
            return db.PPMPHeader.Count();
        }
        public int GetTotalOriginalPPMPs()
        {
            return db.PPMPHeader.Where(d => d.PPMPType == PPMPTypes.Original || d.PPMPType == PPMPTypes.CommonUse).Count();
        }
        public int GetTotalSupplementalPPMPs()
        {
            return db.PPMPHeader.Where(d => d.PPMPType == PPMPTypes.Supplemental).Count();
        }
        public BudgetPropsalVM GetPPMPs(int FiscalYear, string UserEmail)
        {
            var user = db.UserAccounts.Where(d => d.Email == UserEmail).FirstOrDefault();
            var ppmpHeader = db.PPMPHeader.Where(d => d.FiscalYear == FiscalYear && (user.DepartmentCode == user.UnitCode ? d.Department == user.DepartmentCode : d.Department == user.UnitCode)).ToList();

            return new BudgetPropsalVM
            {
                PPMPList = ppmpHeader.Select(d => new PPMPHeaderVM()
                {
                    FiscalYear = d.FiscalYear,
                    ReferenceNo = d.ReferenceNo,
                    Department = hris.GetDepartmentDetails(d.Department).Department,
                    Classification = abis.GetChartOfAccounts(d.UACS).AcctName,
                    Status = d.PPMPStatus,
                    PPMPType = d.PPMPType,
                    EstimatedBudget = db.PPMPDetails.Where(x => x.FKPPMPHeaderReference.ReferenceNo == d.ReferenceNo && (d.IsInfrastructure == true ? (x.ArticleReference == null) : (x.ArticleReference != null))).Sum(x => x.EstimatedBudget),
                    CreatedAt = d.CreatedAt.Value
                }).ToList()
            };
        }
        public PPMPViewModel GetPPMPDetail(string ReferenceNo)
        {
            var ppmp = new PPMPViewModel();
            ppmp.Header = db.PPMPHeader.Where(d => d.ReferenceNo == ReferenceNo).ToList()
                          .Select(d => new PPMPHeaderVM
                          {
                              FiscalYear = d.FiscalYear,
                              ReferenceNo = d.ReferenceNo,
                              ProjectName = d.ProjectName,
                              Department = hris.GetDepartmentDetails(d.Department).Department,
                              UACS = d.UACS,
                              Classification = abis.GetChartOfAccounts(d.UACS).AcctName,
                              Status = d.PPMPStatus,
                              PPMPType = d.PPMPType,
                              EstimatedBudget = db.PPMPDetails.Where(x => x.FKPPMPHeaderReference.ReferenceNo == d.ReferenceNo).Sum(x => x.EstimatedBudget),
                              PreparedBy = d.PreparedBy,
                              PreparedByDesignation = d.PreparedByDesignation,
                              SubmittedBy = d.SubmittedBy,
                              SubmittedByDesignation = d.SubmittedByDesignation,
                              CreatedAt = d.CreatedAt.Value,
                              SubmittedAt = d.SubmittedAt,
                              IsInfrastructure = d.IsInfrastructure
                          }).FirstOrDefault();
            ppmp.Details = new List<PPMPDetailsVM>();
            ppmp.Details.AddRange(db.PPMPDetails
                                    .Where(d => d.FKPPMPHeaderReference.ReferenceNo == ReferenceNo && (ppmp.Header.IsInfrastructure == true ? (d.ArticleReference == null) : (d.ArticleReference != null))).ToList()
                                    .GroupBy(d => new
                                    {
                                        UACS = d.UACS,
                                        Category = d.CategoryReference == null ? null : d.CategoryReference,
                                        ItemType = d.ArticleReference == null ? (int?)null : d.FKItemArticleReference.ItemTypeReference,
                                        ArticleReference = d.ArticleReference == null ? null : d.ArticleReference,
                                        ItemSequence = d.ItemSequence,
                                        ItemCode = d.ArticleReference == null ? null : d.ArticleReference == null ? null : db.ItemArticles.Find(d.ArticleReference).ArticleCode + "-" + d.ItemSequence,
                                        ItemFullName = d.ItemFullName,
                                        ItemSpecifications = d.ItemSpecifications,
                                        ProcurementSource = d.ProcurementSource,
                                        UOMReference = d.UOMReference,
                                        Justification = d.Justification,
                                        JANMilestone = d.JANMilestone == null ? string.Empty : d.JANMilestone.GetAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>().ShortName,
                                        FEBMilestone = d.FEBMilestone == null ? string.Empty : d.FEBMilestone.GetAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>().ShortName,
                                        MARMilestone = d.MARMilestone == null ? string.Empty : d.MARMilestone.GetAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>().ShortName,
                                        APRMilestone = d.APRMilestone == null ? string.Empty : d.APRMilestone.GetAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>().ShortName,
                                        MAYMilestone = d.MAYMilestone == null ? string.Empty : d.MAYMilestone.GetAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>().ShortName,
                                        JUNMilestone = d.JUNMilestone == null ? string.Empty : d.JUNMilestone.GetAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>().ShortName,
                                        JULMilestone = d.JULMilestone == null ? string.Empty : d.JULMilestone.GetAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>().ShortName,
                                        AUGMilestone = d.AUGMilestone == null ? string.Empty : d.AUGMilestone.GetAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>().ShortName,
                                        SEPMilestone = d.SEPMilestone == null ? string.Empty : d.SEPMilestone.GetAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>().ShortName,
                                        OCTMilestone = d.OCTMilestone == null ? string.Empty : d.OCTMilestone.GetAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>().ShortName,
                                        NOVMilestone = d.NOVMilestone == null ? string.Empty : d.NOVMilestone.GetAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>().ShortName,
                                        DECMilestone = d.DECMilestone == null ? string.Empty : d.DECMilestone.GetAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>().ShortName,
                                        UnitCost = d.UnitCost
                                    })
                                    .Select(d => new PPMPDetailsVM
                                    {
                                        PAPCode = d.Key.UACS,
                                        Category = d.Key.Category == null ? null : db.ItemCategories.Find(d.Key.Category).ItemCategoryName,
                                        ItemType = d.Key.ItemType == null ? null : db.ItemTypes.Find(d.Key.ItemType).ItemType,
                                        ArticleCode = d.Key.ArticleReference == null ? null : db.ItemArticles.Find(d.Key.ArticleReference).ArticleCode,
                                        ItemSequence = d.Key.ItemSequence,
                                        ItemCode = d.Key.ItemCode,
                                        ItemFullName = d.Key.ItemFullName,
                                        ItemSpecifications = d.Key.ItemSpecifications,
                                        ProcurementSource = d.Key.ProcurementSource,
                                        UOMReference = db.UOM.Find(d.Key.UOMReference).Abbreviation,
                                        NoOfPostedToAPP = d.Where(x => x.PPMPDetailStatus == PPMPDetailStatus.PostedToAPP).Count(),
                                        NoOfAccepted = d.Where(x => x.BudgetOfficeAction == BudgetOfficeAction.Accepted).Count(),
                                        NoOfForRevision = d.Where(x => x.BudgetOfficeAction == BudgetOfficeAction.ForRevision).Count(),
                                        NoOfNotAccepted = d.Where(x => x.BudgetOfficeAction == BudgetOfficeAction.NotAccepted).Count(),
                                        Justification = d.Key.Justification,
                                        JANMilestone = d.Key.JANMilestone,
                                        FEBMilestone = d.Key.FEBMilestone,
                                        MARMilestone = d.Key.MARMilestone,
                                        APRMilestone = d.Key.APRMilestone,
                                        MAYMilestone = d.Key.MAYMilestone,
                                        JUNMilestone = d.Key.JUNMilestone,
                                        JULMilestone = d.Key.JULMilestone,
                                        AUGMilestone = d.Key.AUGMilestone,
                                        SEPMilestone = d.Key.SEPMilestone,
                                        OCTMilestone = d.Key.OCTMilestone,
                                        NOVMilestone = d.Key.NOVMilestone,
                                        DECMilestone = d.Key.DECMilestone,
                                        JanQty = d.Sum(x => x.JanQty),
                                        FebQty = d.Sum(x => x.FebQty),
                                        MarQty = d.Sum(x => x.MarQty),
                                        Q1TotalQty = d.Sum(x => x.TotalQty),
                                        AprQty = d.Sum(x => x.AprQty),
                                        MayQty = d.Sum(x => x.MayQty),
                                        JunQty = d.Sum(x => x.JunQty),
                                        Q2TotalQty = d.Sum(x => x.Q2TotalQty),
                                        JulQty = d.Sum(x => x.JulQty),
                                        AugQty = d.Sum(x => x.AugQty),
                                        SepQty = d.Sum(x => x.SepQty),
                                        Q3TotalQty = d.Sum(x => x.Q3TotalQty),
                                        OctQty = d.Sum(x => x.OctQty),
                                        NovQty = d.Sum(x => x.NovQty),
                                        DecQty = d.Sum(x => x.DecQty),
                                        Q4TotalQty = d.Sum(x => x.Q4TotalQty),
                                        TotalQty = d.Sum(x => x.TotalQty),
                                        UnitCost = d.Key.UnitCost,
                                        EstimatedBudget = d.Sum(x => x.EstimatedBudget)
                                    }).ToList());
            return ppmp;
        }
        public PPMPItemDetailsVM GetPPMPItemDetails(string ReferenceNo, string ItemCode)
        {
            var ppmpItemDetail = new PPMPItemDetailsVM();
            var itemRecord = db.Items.ToList().Where(d => d.ItemCode == ItemCode).FirstOrDefault();
            var ppmpItem = db.PPMPDetails.ToList().Where(d => d.ArticleReference == itemRecord.ArticleReference &&
                                                              d.ItemSequence == itemRecord.Sequence &&
                                                              d.FKPPMPHeaderReference.ReferenceNo == ReferenceNo)
            .Select(d => new
            {
                ItemType = d.FKItemArticleReference.FKItemTypeReference.ItemType,
                Category = d.FKCategoryReference.ItemCategoryName,
                ItemCode = d.FKItemArticleReference.ArticleCode + "-" + d.ItemSequence,
                ItemName = d.ItemFullName,
                ItemSpecifications = d.ItemSpecifications,
                ProposalType = d.ProposalType,
                ProcurementSource = d.ProcurementSource,
                AccountClass = d.FKItemArticleReference.UACSObjectClass,
                UnitCost = d.UnitCost,
                IndividualUOMReference = d.FKUOMReference.UnitName
            }).Distinct()
            .Select(d => new PPMPItemVM
            {
                ReferenceNo = ReferenceNo,
                ItemType = d.ItemType,
                Category = d.Category,
                ItemCode = d.ItemCode,
                ItemName = d.ItemName,
                ItemSpecifications = d.ItemSpecifications,
                ProposalType = d.ProposalType.GetAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>().Name,
                ProcurementSource = d.ProcurementSource.GetAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>().Name,
                AccountClass = abis.GetChartOfAccounts(d.AccountClass).AcctName,
                UnitCost = d.UnitCost,
                IndividualUOMReference = d.IndividualUOMReference
            }).FirstOrDefault();

            var ppmpProjectDetails = db.PPMPDetails.Where(d => d.ArticleReference == itemRecord.ArticleReference &&
                                                               d.ItemSequence == itemRecord.Sequence &&
                                                               d.FKPPMPHeaderReference.ReferenceNo == ReferenceNo).ToList()
                                 .Select(d => new PPMPProjectItemVM
                                 {
                                     PAPCode = d.FKProjectDetailsReference.FKProjectPlanReference.PAPCode,
                                     Program = abis.GetPrograms(d.FKProjectDetailsReference.FKProjectPlanReference.PAPCode).GeneralDescription,
                                     UnitName = hris.GetDepartmentDetails(d.FKProjectDetailsReference.FKProjectPlanReference.Unit).Section,
                                     ProjectCode = d.FKProjectDetailsReference.FKProjectPlanReference.ProjectCode,
                                     ProjectName = d.FKProjectDetailsReference.FKProjectPlanReference.ProjectName,
                                     Description = d.FKProjectDetailsReference.FKProjectPlanReference.Description,
                                     DeliveryMonth = d.FKProjectDetailsReference.FKProjectPlanReference.DeliveryMonth,
                                     Justification = d.Justification,
                                     ItemStatus = d.PPMPDetailStatus,
                                     BudgetOfficeAction = d.BudgetOfficeAction,
                                     ReasonForNonAcceptance = d.BudgetOfficeReasonForNonAcceptance == null ? null : d.BudgetOfficeReasonForNonAcceptance,
                                     JanQty = d.JanQty,
                                     FebQty = d.FebQty,
                                     MarQty = d.MarQty,
                                     AprQty = d.AprQty,
                                     MayQty = d.MayQty,
                                     JunQty = d.JunQty,
                                     JulQty = d.JulQty,
                                     AugQty = d.AugQty,
                                     SepQty = d.SepQty,
                                     OctQty = d.OctQty,
                                     NovQty = d.NovQty,
                                     DecQty = d.DecQty,
                                     TotalQty = d.TotalQty,
                                     UpdateFlag = d.UpdateFlag
                                 }).ToList();

            ppmpItemDetail.Item = ppmpItem;
            ppmpItemDetail.ProjectPlans = ppmpProjectDetails;
            return ppmpItemDetail;
        }
        public string GenerateReferenceNo(int FiscalYear, string DepartmentCode, PPMPTypes Type)
        {
            string referenceNo = string.Empty;
            string seqNo = (db.PPMPHeader.Where(d => d.FiscalYear == FiscalYear && d.Department == DepartmentCode && d.PPMPType == Type).Count() + 1).ToString();
            string ppmpTypeCode = Type.GetAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>().ShortName;
            seqNo = seqNo.ToString().Length == 3 ? seqNo : seqNo.ToString().Length == 2 ? "0" + seqNo.ToString() : "00" + seqNo.ToString();
            referenceNo = "PPMP-" + ppmpTypeCode + "-" + FiscalYear.ToString() + "-" + DepartmentCode + "-" + seqNo;
            return referenceNo;
        }
        public bool PostToPPMP(int FiscalYear, string UserEmail)
        {
            var ppmpHeader = new PPMPHeader();
            var employee = hris.GetEmployee(UserEmail);
            var user = db.UserAccounts.Where(d => d.Email == UserEmail).FirstOrDefault();
            var office = hris.GetDepartmentDetails(user.DepartmentCode);

            var suppliesProject = db.ProjectPlans
                                    .Where(d => d.FiscalYear == FiscalYear &&
                                                d.ProjectStatus == ProjectStatus.EvaluatedByResponsibilityCenter &&
                                                d.ProjectType == ProjectTypes.CommonSuppliesProjectPlan && d.Department == user.DepartmentCode).ToList();
            var suppliesProjectDetails = db.ProjectDetails.ToList()
                                           .Where(d => suppliesProject.Select(x => x.ProjectCode).Contains(d.FKProjectPlanReference.ProjectCode) &&
                                                       d.ProjectItemStatus == ProjectDetailsStatus.ItemAccepted).ToList();
            var suppliesUACSAccounts = suppliesProjectDetails.Select(d => d.FKItemArticleReference.UACSObjectClass).Distinct().ToList();
            var originalPPMPCSECount = db.PPMPHeader
                        .Where(d => d.PPMPType == PPMPTypes.CommonUse && d.FiscalYear == FiscalYear && d.Department == office.DepartmentCode)
                        .Select(d => new { d.FiscalYear, d.Department, d.PPMPType }).Distinct().Count();

            if (suppliesProject.Count != 0)
            {
                foreach (var uacs in suppliesUACSAccounts)
                {
                    ppmpHeader = new PPMPHeader
                    {
                        ReferenceNo = GenerateReferenceNo(FiscalYear, user.DepartmentCode, (originalPPMPCSECount == 0 ? PPMPTypes.CommonUse : PPMPTypes.Supplemental)),
                        FiscalYear = FiscalYear,
                        UACS = uacs,
                        ProjectName = abis.GetChartOfAccounts(uacs).AcctName,
                        PPMPType = originalPPMPCSECount == 0 ? PPMPTypes.CommonUse : PPMPTypes.Supplemental,
                        PPMPStatus = PPMPStatus.NewPPMP,
                        Sector = office.SectorCode,
                        Department = office.DepartmentCode,
                        IsInstitutional = false,
                        CreatedAt = DateTime.Now,
                        PreparedBy = employee.EmployeeName,
                        PreparedByDesignation = employee.Designation,
                        SubmittedBy = office.DepartmentHead,
                        SubmittedByDesignation = office.DepartmentHeadDesignation.ToUpper(),
                        IsInfrastructure = false
                    };

                    db.PPMPHeader.Add(ppmpHeader);
                    if (db.SaveChanges() == 0)
                    {
                        return false;
                    }

                    db.PPMPDetails.AddRange(suppliesProjectDetails.ToList()
                        .Where(d => d.FKItemArticleReference.UACSObjectClass == uacs &&
                                    d.FKItemArticleReference.FKItemTypeReference.FKItemClassificationReference.ID == 1)
                        .Select(d => new PPMPDetails
                        {
                            ProjectDetailsID = d.ID,
                            UACS = ppmpHeader.UACS,
                            PPMPHeaderReference = ppmpHeader.ID,
                            ClassificationReference = d.ClassificationReference,
                            ArticleReference = (int)d.ArticleReference,
                            ItemSequence = d.ItemSequence,
                            ItemFullName = d.ItemFullName,
                            ItemSpecifications = d.ItemSpecifications,
                            ProcurementSource = d.ProcurementSource,
                            UOMReference = d.UOMReference,
                            CategoryReference = (int)d.CategoryReference,
                            Justification = d.Justification,
                            JANMilestone = d.FKProjectPlanReference.DeliveryMonth == 1 ? PPMPMilestones.PRPreparationOCT :
                                           d.FKProjectPlanReference.DeliveryMonth == 2 ? PPMPMilestones.PRPreparationNOV :
                                           d.FKProjectPlanReference.DeliveryMonth == 3 ? PPMPMilestones.PRPreparationDEC :
                                           d.FKProjectPlanReference.DeliveryMonth == 4 ? PPMPMilestones.PRPreparation : (PPMPMilestones?)null,
                            FEBMilestone = d.FKProjectPlanReference.DeliveryMonth == 2 ? PPMPMilestones.DeliveryRISPreparation :
                                           d.FKProjectPlanReference.DeliveryMonth == 3 ? PPMPMilestones.ProcurementActivities :
                                           d.FKProjectPlanReference.DeliveryMonth == 4 ? PPMPMilestones.ProcurementActivities :
                                           d.FKProjectPlanReference.DeliveryMonth == 5 ? PPMPMilestones.PRPreparation : (PPMPMilestones?)null,
                            MARMilestone = d.FKProjectPlanReference.DeliveryMonth == 3 ? PPMPMilestones.DeliveryRISPreparation :
                                           d.FKProjectPlanReference.DeliveryMonth == 4 ? PPMPMilestones.ProcurementActivities :
                                           d.FKProjectPlanReference.DeliveryMonth == 5 ? PPMPMilestones.ProcurementActivities :
                                           d.FKProjectPlanReference.DeliveryMonth == 6 ? PPMPMilestones.PRPreparation : (PPMPMilestones?)null,
                            APRMilestone = d.FKProjectPlanReference.DeliveryMonth == 4 ? PPMPMilestones.DeliveryRISPreparation :
                                           d.FKProjectPlanReference.DeliveryMonth == 5 ? PPMPMilestones.ProcurementActivities :
                                           d.FKProjectPlanReference.DeliveryMonth == 6 ? PPMPMilestones.ProcurementActivities :
                                           d.FKProjectPlanReference.DeliveryMonth == 7 ? PPMPMilestones.PRPreparation : (PPMPMilestones?)null,
                            MAYMilestone = d.FKProjectPlanReference.DeliveryMonth == 5 ? PPMPMilestones.DeliveryRISPreparation :
                                           d.FKProjectPlanReference.DeliveryMonth == 6 ? PPMPMilestones.ProcurementActivities :
                                           d.FKProjectPlanReference.DeliveryMonth == 7 ? PPMPMilestones.ProcurementActivities :
                                           d.FKProjectPlanReference.DeliveryMonth == 8 ? PPMPMilestones.PRPreparation : (PPMPMilestones?)null,
                            JUNMilestone = d.FKProjectPlanReference.DeliveryMonth == 6 ? PPMPMilestones.DeliveryRISPreparation :
                                           d.FKProjectPlanReference.DeliveryMonth == 7 ? PPMPMilestones.ProcurementActivities :
                                           d.FKProjectPlanReference.DeliveryMonth == 8 ? PPMPMilestones.ProcurementActivities :
                                           d.FKProjectPlanReference.DeliveryMonth == 9 ? PPMPMilestones.PRPreparation : (PPMPMilestones?)null,
                            JULMilestone = d.FKProjectPlanReference.DeliveryMonth == 7 ? PPMPMilestones.DeliveryRISPreparation :
                                           d.FKProjectPlanReference.DeliveryMonth == 8 ? PPMPMilestones.ProcurementActivities :
                                           d.FKProjectPlanReference.DeliveryMonth == 9 ? PPMPMilestones.ProcurementActivities :
                                           d.FKProjectPlanReference.DeliveryMonth == 10 ? PPMPMilestones.PRPreparation : (PPMPMilestones?)null,
                            AUGMilestone = d.FKProjectPlanReference.DeliveryMonth == 8 ? PPMPMilestones.DeliveryRISPreparation :
                                           d.FKProjectPlanReference.DeliveryMonth == 9 ? PPMPMilestones.ProcurementActivities :
                                           d.FKProjectPlanReference.DeliveryMonth == 10 ? PPMPMilestones.ProcurementActivities :
                                           d.FKProjectPlanReference.DeliveryMonth == 11 ? PPMPMilestones.PRPreparation : (PPMPMilestones?)null,
                            SEPMilestone = d.FKProjectPlanReference.DeliveryMonth == 9 ? PPMPMilestones.DeliveryRISPreparation :
                                           d.FKProjectPlanReference.DeliveryMonth == 10 ? PPMPMilestones.ProcurementActivities :
                                           d.FKProjectPlanReference.DeliveryMonth == 11 ? PPMPMilestones.ProcurementActivities :
                                           d.FKProjectPlanReference.DeliveryMonth == 12 ? PPMPMilestones.PRPreparation : (PPMPMilestones?)null,
                            OCTMilestone = d.FKProjectPlanReference.DeliveryMonth == 10 ? PPMPMilestones.DeliveryRISPreparation :
                                           d.FKProjectPlanReference.DeliveryMonth == 11 ? PPMPMilestones.ProcurementActivities :
                                           d.FKProjectPlanReference.DeliveryMonth == 12 ? PPMPMilestones.ProcurementActivities : (PPMPMilestones?)null,
                            NOVMilestone = d.FKProjectPlanReference.DeliveryMonth == 11 ? PPMPMilestones.DeliveryRISPreparation :
                                           d.FKProjectPlanReference.DeliveryMonth == 12 ? PPMPMilestones.ProcurementActivities : (PPMPMilestones?)null,
                            DECMilestone = d.FKProjectPlanReference.DeliveryMonth == 12 ? PPMPMilestones.DeliveryRISPreparation : (PPMPMilestones?)null,
                            JanQty = d.JanQty,
                            FebQty = d.FebQty,
                            MarQty = d.FebQty,
                            Q1TotalQty = d.JanQty + d.FebQty + d.MarQty,
                            AprQty = d.AprQty,
                            MayQty = d.MayQty,
                            JunQty = d.JunQty,
                            Q2TotalQty = d.AprQty + d.MayQty + d.JunQty,
                            JulQty = d.JulQty,
                            AugQty = d.AugQty,
                            SepQty = d.SepQty,
                            Q3TotalQty = d.JulQty + d.AugQty + d.SepQty,
                            OctQty = d.OctQty,
                            NovQty = d.NovQty,
                            DecQty = d.DecQty,
                            Q4TotalQty = d.OctQty + d.NovQty + d.DecQty,
                            TotalQty = d.TotalQty,
                            UnitCost = (decimal)d.UnitCost,
                            EstimatedBudget = Math.Round((d.TotalQty * (decimal)d.UnitCost), 2),
                            PPMPDetailStatus = PPMPDetailStatus.PostedToPPMP
                        }).ToList());
                }

                suppliesProject.ForEach(d => { d.ProjectStatus = ProjectStatus.PostedToPPMP; });
                suppliesProjectDetails.ForEach(d => { d.ProjectItemStatus = ProjectDetailsStatus.PostedToPPMP; });

                if (db.SaveChanges() == 0)
                {
                    return false;
                }
            }

            var project = db.ProjectPlans
                            .Where(d => d.FiscalYear == FiscalYear &&
                                        d.ProjectStatus == ProjectStatus.EvaluatedByResponsibilityCenter &&
                                        d.ProjectType != ProjectTypes.CommonSuppliesProjectPlan && d.Department == user.DepartmentCode).ToList();
            var projectDetails = db.ProjectDetails.ToList()
                                   .Where(d => project.Select(x => x.ProjectCode).Contains(d.FKProjectPlanReference.ProjectCode) &&
                                               d.ProjectItemStatus == ProjectDetailsStatus.ItemAccepted &&
                                               d.ArticleReference != null &&
                                               (d.FKClassificationReference.Classification != "Repair and Maintenance" && d.FKClassificationReference.Classification != "Infrastructure")).ToList();
            var UACSAccounts = projectDetails.Select(d => d.FKItemArticleReference.UACSObjectClass).Distinct().ToList();
            var originalPPMPCount = db.PPMPHeader
                                      .Where(d => d.PPMPType == PPMPTypes.Original && d.FiscalYear == FiscalYear && d.Department == office.DepartmentCode && d.PPMPStatus >= PPMPStatus.ForwardedToBudgetOffice)
                                      .Select(d => new { d.FiscalYear, d.Department, d.PPMPType }).Distinct().Count();

            if (project.Count != 0)
            {
                ///Post to PPMP - Property, Pland and Equipment
                foreach (var uacs in UACSAccounts)
                {
                    ppmpHeader = new PPMPHeader
                    {
                        ReferenceNo = GenerateReferenceNo(FiscalYear, user.DepartmentCode, (originalPPMPCount == 0 ? PPMPTypes.Original : PPMPTypes.Supplemental)),
                        ProjectName = abis.GetChartOfAccounts(uacs).AcctName,
                        FiscalYear = FiscalYear,
                        UACS = uacs,
                        PPMPType = originalPPMPCount == 0 ? PPMPTypes.Original : PPMPTypes.Supplemental,
                        PPMPStatus = PPMPStatus.NewPPMP,
                        Sector = office.SectorCode,
                        Department = office.DepartmentCode,
                        IsInstitutional = false,
                        CreatedAt = DateTime.Now,
                        PreparedBy = employee.EmployeeName,
                        PreparedByDesignation = employee.Designation,
                        SubmittedBy = office.DepartmentHead,
                        SubmittedByDesignation = office.DepartmentHeadDesignation.ToUpper(),
                        IsInfrastructure = false
                    };

                    db.PPMPHeader.Add(ppmpHeader);
                    if (db.SaveChanges() == 0)
                    {
                        return false;
                    }

                    db.PPMPDetails.AddRange(projectDetails
                        .Where(d => d.FKItemArticleReference.UACSObjectClass == uacs)
                        .Select(d => new PPMPDetails
                        {
                            ProjectDetailsID = d.ID,
                            UACS = ppmpHeader.UACS,
                            PPMPHeaderReference = ppmpHeader.ID,
                            ClassificationReference = d.ClassificationReference,
                            ArticleReference = (int)d.ArticleReference,
                            ItemSequence = d.ItemSequence,
                            ItemFullName = d.ItemFullName,
                            ItemSpecifications = d.ItemSpecifications,
                            ProcurementSource = d.ProcurementSource,
                            UOMReference = d.UOMReference,
                            CategoryReference = (int)d.CategoryReference,
                            Justification = d.Justification,
                            JANMilestone = d.FKProjectPlanReference.DeliveryMonth == 1 ? PPMPMilestones.PRPreparationOCT :
                                           d.FKProjectPlanReference.DeliveryMonth == 2 ? PPMPMilestones.PRPreparationNOV :
                                           d.FKProjectPlanReference.DeliveryMonth == 3 ? PPMPMilestones.PRPreparationDEC :
                                           d.FKProjectPlanReference.DeliveryMonth == 4 ? PPMPMilestones.PRPreparation : (PPMPMilestones?)null,
                            FEBMilestone = d.FKProjectPlanReference.DeliveryMonth == 2 ? PPMPMilestones.DeliveryRISPreparation :
                                           d.FKProjectPlanReference.DeliveryMonth == 3 ? PPMPMilestones.ProcurementActivities :
                                           d.FKProjectPlanReference.DeliveryMonth == 4 ? PPMPMilestones.ProcurementActivities :
                                           d.FKProjectPlanReference.DeliveryMonth == 5 ? PPMPMilestones.PRPreparation : (PPMPMilestones?)null,
                            MARMilestone = d.FKProjectPlanReference.DeliveryMonth == 3 ? PPMPMilestones.DeliveryRISPreparation :
                                           d.FKProjectPlanReference.DeliveryMonth == 4 ? PPMPMilestones.ProcurementActivities :
                                           d.FKProjectPlanReference.DeliveryMonth == 5 ? PPMPMilestones.ProcurementActivities :
                                           d.FKProjectPlanReference.DeliveryMonth == 6 ? PPMPMilestones.PRPreparation : (PPMPMilestones?)null,
                            APRMilestone = d.FKProjectPlanReference.DeliveryMonth == 4 ? PPMPMilestones.DeliveryRISPreparation :
                                           d.FKProjectPlanReference.DeliveryMonth == 5 ? PPMPMilestones.ProcurementActivities :
                                           d.FKProjectPlanReference.DeliveryMonth == 6 ? PPMPMilestones.ProcurementActivities :
                                           d.FKProjectPlanReference.DeliveryMonth == 7 ? PPMPMilestones.PRPreparation : (PPMPMilestones?)null,
                            MAYMilestone = d.FKProjectPlanReference.DeliveryMonth == 5 ? PPMPMilestones.DeliveryRISPreparation :
                                           d.FKProjectPlanReference.DeliveryMonth == 6 ? PPMPMilestones.ProcurementActivities :
                                           d.FKProjectPlanReference.DeliveryMonth == 7 ? PPMPMilestones.ProcurementActivities :
                                           d.FKProjectPlanReference.DeliveryMonth == 8 ? PPMPMilestones.PRPreparation : (PPMPMilestones?)null,
                            JUNMilestone = d.FKProjectPlanReference.DeliveryMonth == 6 ? PPMPMilestones.DeliveryRISPreparation :
                                           d.FKProjectPlanReference.DeliveryMonth == 7 ? PPMPMilestones.ProcurementActivities :
                                           d.FKProjectPlanReference.DeliveryMonth == 8 ? PPMPMilestones.ProcurementActivities :
                                           d.FKProjectPlanReference.DeliveryMonth == 9 ? PPMPMilestones.PRPreparation : (PPMPMilestones?)null,
                            JULMilestone = d.FKProjectPlanReference.DeliveryMonth == 7 ? PPMPMilestones.DeliveryRISPreparation :
                                           d.FKProjectPlanReference.DeliveryMonth == 8 ? PPMPMilestones.ProcurementActivities :
                                           d.FKProjectPlanReference.DeliveryMonth == 9 ? PPMPMilestones.ProcurementActivities :
                                           d.FKProjectPlanReference.DeliveryMonth == 10 ? PPMPMilestones.PRPreparation : (PPMPMilestones?)null,
                            AUGMilestone = d.FKProjectPlanReference.DeliveryMonth == 8 ? PPMPMilestones.DeliveryRISPreparation :
                                           d.FKProjectPlanReference.DeliveryMonth == 9 ? PPMPMilestones.ProcurementActivities :
                                           d.FKProjectPlanReference.DeliveryMonth == 10 ? PPMPMilestones.ProcurementActivities :
                                           d.FKProjectPlanReference.DeliveryMonth == 11 ? PPMPMilestones.PRPreparation : (PPMPMilestones?)null,
                            SEPMilestone = d.FKProjectPlanReference.DeliveryMonth == 9 ? PPMPMilestones.DeliveryRISPreparation :
                                           d.FKProjectPlanReference.DeliveryMonth == 10 ? PPMPMilestones.ProcurementActivities :
                                           d.FKProjectPlanReference.DeliveryMonth == 11 ? PPMPMilestones.ProcurementActivities :
                                           d.FKProjectPlanReference.DeliveryMonth == 12 ? PPMPMilestones.PRPreparation : (PPMPMilestones?)null,
                            OCTMilestone = d.FKProjectPlanReference.DeliveryMonth == 10 ? PPMPMilestones.DeliveryRISPreparation :
                                           d.FKProjectPlanReference.DeliveryMonth == 11 ? PPMPMilestones.ProcurementActivities :
                                           d.FKProjectPlanReference.DeliveryMonth == 12 ? PPMPMilestones.ProcurementActivities : (PPMPMilestones?)null,
                            NOVMilestone = d.FKProjectPlanReference.DeliveryMonth == 11 ? PPMPMilestones.DeliveryRISPreparation :
                                           d.FKProjectPlanReference.DeliveryMonth == 12 ? PPMPMilestones.ProcurementActivities : (PPMPMilestones?)null,
                            DECMilestone = d.FKProjectPlanReference.DeliveryMonth == 12 ? PPMPMilestones.DeliveryRISPreparation : (PPMPMilestones?)null,
                            JanQty = d.JanQty,
                            FebQty = d.FebQty,
                            MarQty = d.FebQty,
                            Q1TotalQty = d.JanQty + d.FebQty + d.MarQty,
                            AprQty = d.AprQty,
                            MayQty = d.MayQty,
                            JunQty = d.JunQty,
                            Q2TotalQty = d.AprQty + d.MayQty + d.JunQty,
                            JulQty = d.JulQty,
                            AugQty = d.AugQty,
                            SepQty = d.SepQty,
                            Q3TotalQty = d.JulQty + d.AugQty + d.SepQty,
                            OctQty = d.OctQty,
                            NovQty = d.NovQty,
                            DecQty = d.DecQty,
                            Q4TotalQty = d.OctQty + d.NovQty + d.DecQty,
                            TotalQty = d.TotalQty,
                            UnitCost = (decimal)d.UnitCost,
                            EstimatedBudget = d.TotalQty * (decimal)d.UnitCost
                        }).ToList());
                }

                ///Post to PPMP - Infrastructure Projects
                var projectCodes = project.Select(d => d.ProjectCode).ToList();
                var infraProjects = db.InfrastructureProject.Where(d => projectCodes.Contains(d.FKEndUserProjectReference.ProjectCode)).ToList();

                foreach(var infraProject in infraProjects)
                {
                    var endUserInfraProjectDetail = db.ProjectDetails.Where(d => d.FKProjectPlanReference.ProjectCode == infraProject.FKEndUserProjectReference.ProjectCode &&
                                                                                 d.ArticleReference == infraProject.ArticleReference &&
                                                                                 d.ItemSequence == infraProject.ItemSequence).FirstOrDefault();
                    var infraProjectDetails = db.InfrastructureDetailedEstimate.Where(d => d.InfrastructureProjectReference == infraProject.ID).ToList();

                    ppmpHeader = new PPMPHeader()
                    {
                        ReferenceNo = originalPPMPCount == 0 ? GenerateReferenceNo(infraProject.FKEndUserProjectReference.FiscalYear, infraProject.FKEndUserProjectReference.Department, PPMPTypes.Original) : GenerateReferenceNo(infraProject.FKEndUserProjectReference.FiscalYear, infraProject.FKEndUserProjectReference.Department, PPMPTypes.Supplemental),
                        ProjectName = infraProject.ProjectTitle,
                        FiscalYear = infraProject.FKEndUserProjectReference.FiscalYear,
                        UACS = endUserInfraProjectDetail.FKItemArticleReference.UACSObjectClass,
                        PPMPType = originalPPMPCount == 0 ? PPMPTypes.Original : PPMPTypes.Supplemental,
                        PPMPStatus = PPMPStatus.NewPPMP,
                        EstimatedBudget = infraProjectDetails.Sum(d => d.TotalAmount),
                        Sector = infraProject.FKEndUserProjectReference.Sector,
                        Department = infraProject.FKEndUserProjectReference.Department,
                        IsInstitutional = true,
                        CreatedAt = DateTime.Now,
                        SubmittedAt = DateTime.Now,
                        PreparedBy = hris.GetEmployeeByCode(infraProject.FKEndUserProjectReference.PreparedBy).EmployeeName,
                        PreparedByDesignation = hris.GetEmployeeByCode(infraProject.FKEndUserProjectReference.PreparedBy).Designation,
                        SubmittedBy = hris.GetDepartmentDetails(infraProject.FKEndUserProjectReference.Department).DepartmentHead,
                        SubmittedByDesignation = hris.GetDepartmentDetails(infraProject.FKEndUserProjectReference.Department).DepartmentHeadDesignation,
                        IsInfrastructure = true
                    };

                    db.PPMPHeader.Add(ppmpHeader);
                    if (db.SaveChanges() == 0)
                    {
                        return false;
                    }

                    var infraDetails = new List<PPMPDetails>();
                    infraDetails.Add(new PPMPDetails 
                    {
                        ProjectDetailsID = endUserInfraProjectDetail.ID,
                        UACS = ppmpHeader.UACS,
                        PPMPHeaderReference = ppmpHeader.ID,
                        ArticleReference = endUserInfraProjectDetail.ArticleReference,
                        ItemSequence = endUserInfraProjectDetail.ItemSequence,
                        ClassificationReference = endUserInfraProjectDetail.ClassificationReference,
                        ItemFullName = endUserInfraProjectDetail.ItemFullName,
                        ItemSpecifications = endUserInfraProjectDetail.ItemSpecifications,
                        ProcurementSource = endUserInfraProjectDetail.ProcurementSource,
                        UOMReference = (int)endUserInfraProjectDetail.UOMReference,
                        CategoryReference = endUserInfraProjectDetail.CategoryReference,
                        Justification = endUserInfraProjectDetail.Justification,
                        JANMilestone = infraProject.FKEndUserProjectReference.DeliveryMonth == 1 ? PPMPMilestones.PRPreparationOCT :
                                                   infraProject.FKEndUserProjectReference.DeliveryMonth == 2 ? PPMPMilestones.PRPreparationNOV :
                                                   infraProject.FKEndUserProjectReference.DeliveryMonth == 3 ? PPMPMilestones.PRPreparationDEC :
                                                   infraProject.FKEndUserProjectReference.DeliveryMonth == 4 ? PPMPMilestones.PRPreparation : (PPMPMilestones?)null,
                        FEBMilestone = infraProject.FKEndUserProjectReference.DeliveryMonth == 2 ? PPMPMilestones.DeliveryRISPreparation :
                                                   infraProject.FKEndUserProjectReference.DeliveryMonth == 3 ? PPMPMilestones.ProcurementActivities :
                                                   infraProject.FKEndUserProjectReference.DeliveryMonth == 4 ? PPMPMilestones.ProcurementActivities :
                                                   infraProject.FKEndUserProjectReference.DeliveryMonth == 5 ? PPMPMilestones.PRPreparation : (PPMPMilestones?)null,
                        MARMilestone = infraProject.FKEndUserProjectReference.DeliveryMonth == 3 ? PPMPMilestones.DeliveryRISPreparation :
                                                   infraProject.FKEndUserProjectReference.DeliveryMonth == 4 ? PPMPMilestones.ProcurementActivities :
                                                   infraProject.FKEndUserProjectReference.DeliveryMonth == 5 ? PPMPMilestones.ProcurementActivities :
                                                   infraProject.FKEndUserProjectReference.DeliveryMonth == 6 ? PPMPMilestones.PRPreparation : (PPMPMilestones?)null,
                        APRMilestone = infraProject.FKEndUserProjectReference.DeliveryMonth == 4 ? PPMPMilestones.DeliveryRISPreparation :
                                                   infraProject.FKEndUserProjectReference.DeliveryMonth == 5 ? PPMPMilestones.ProcurementActivities :
                                                   infraProject.FKEndUserProjectReference.DeliveryMonth == 6 ? PPMPMilestones.ProcurementActivities :
                                                   infraProject.FKEndUserProjectReference.DeliveryMonth == 7 ? PPMPMilestones.PRPreparation : (PPMPMilestones?)null,
                        MAYMilestone = infraProject.FKEndUserProjectReference.DeliveryMonth == 5 ? PPMPMilestones.DeliveryRISPreparation :
                                                   infraProject.FKEndUserProjectReference.DeliveryMonth == 6 ? PPMPMilestones.ProcurementActivities :
                                                   infraProject.FKEndUserProjectReference.DeliveryMonth == 7 ? PPMPMilestones.ProcurementActivities :
                                                   infraProject.FKEndUserProjectReference.DeliveryMonth == 8 ? PPMPMilestones.PRPreparation : (PPMPMilestones?)null,
                        JUNMilestone = infraProject.FKEndUserProjectReference.DeliveryMonth == 6 ? PPMPMilestones.DeliveryRISPreparation :
                                                   infraProject.FKEndUserProjectReference.DeliveryMonth == 7 ? PPMPMilestones.ProcurementActivities :
                                                   infraProject.FKEndUserProjectReference.DeliveryMonth == 8 ? PPMPMilestones.ProcurementActivities :
                                                   infraProject.FKEndUserProjectReference.DeliveryMonth == 9 ? PPMPMilestones.PRPreparation : (PPMPMilestones?)null,
                        JULMilestone = infraProject.FKEndUserProjectReference.DeliveryMonth == 7 ? PPMPMilestones.DeliveryRISPreparation :
                                                   infraProject.FKEndUserProjectReference.DeliveryMonth == 8 ? PPMPMilestones.ProcurementActivities :
                                                   infraProject.FKEndUserProjectReference.DeliveryMonth == 9 ? PPMPMilestones.ProcurementActivities :
                                                   infraProject.FKEndUserProjectReference.DeliveryMonth == 10 ? PPMPMilestones.PRPreparation : (PPMPMilestones?)null,
                        AUGMilestone = infraProject.FKEndUserProjectReference.DeliveryMonth == 8 ? PPMPMilestones.DeliveryRISPreparation :
                                                   infraProject.FKEndUserProjectReference.DeliveryMonth == 9 ? PPMPMilestones.ProcurementActivities :
                                                   infraProject.FKEndUserProjectReference.DeliveryMonth == 10 ? PPMPMilestones.ProcurementActivities :
                                                   infraProject.FKEndUserProjectReference.DeliveryMonth == 11 ? PPMPMilestones.PRPreparation : (PPMPMilestones?)null,
                        SEPMilestone = infraProject.FKEndUserProjectReference.DeliveryMonth == 9 ? PPMPMilestones.DeliveryRISPreparation :
                                                   infraProject.FKEndUserProjectReference.DeliveryMonth == 10 ? PPMPMilestones.ProcurementActivities :
                                                   infraProject.FKEndUserProjectReference.DeliveryMonth == 11 ? PPMPMilestones.ProcurementActivities :
                                                   infraProject.FKEndUserProjectReference.DeliveryMonth == 12 ? PPMPMilestones.PRPreparation : (PPMPMilestones?)null,
                        OCTMilestone = infraProject.FKEndUserProjectReference.DeliveryMonth == 10 ? PPMPMilestones.DeliveryRISPreparation :
                                                   infraProject.FKEndUserProjectReference.DeliveryMonth == 11 ? PPMPMilestones.ProcurementActivities :
                                                   infraProject.FKEndUserProjectReference.DeliveryMonth == 12 ? PPMPMilestones.ProcurementActivities : (PPMPMilestones?)null,
                        NOVMilestone = infraProject.FKEndUserProjectReference.DeliveryMonth == 11 ? PPMPMilestones.DeliveryRISPreparation :
                                                   infraProject.FKEndUserProjectReference.DeliveryMonth == 12 ? PPMPMilestones.ProcurementActivities : (PPMPMilestones?)null,
                        DECMilestone = infraProject.FKEndUserProjectReference.DeliveryMonth == 12 ? PPMPMilestones.DeliveryRISPreparation : (PPMPMilestones?)null,
                        JanQty = endUserInfraProjectDetail.JanQty,
                        FebQty = endUserInfraProjectDetail.FebQty,
                        MarQty = endUserInfraProjectDetail.FebQty,
                        Q1TotalQty = endUserInfraProjectDetail.JanQty + endUserInfraProjectDetail.FebQty + endUserInfraProjectDetail.MarQty,
                        AprQty = endUserInfraProjectDetail.AprQty,
                        MayQty = endUserInfraProjectDetail.MayQty,
                        JunQty = endUserInfraProjectDetail.JunQty,
                        Q2TotalQty = endUserInfraProjectDetail.AprQty + endUserInfraProjectDetail.MayQty + endUserInfraProjectDetail.JunQty,
                        JulQty = endUserInfraProjectDetail.JulQty,
                        AugQty = endUserInfraProjectDetail.AugQty,
                        SepQty = endUserInfraProjectDetail.SepQty,
                        Q3TotalQty = endUserInfraProjectDetail.JulQty + endUserInfraProjectDetail.AugQty + endUserInfraProjectDetail.SepQty,
                        OctQty = endUserInfraProjectDetail.OctQty,
                        NovQty = endUserInfraProjectDetail.NovQty,
                        DecQty = endUserInfraProjectDetail.DecQty,
                        Q4TotalQty = endUserInfraProjectDetail.OctQty + endUserInfraProjectDetail.NovQty + endUserInfraProjectDetail.DecQty,
                        TotalQty = endUserInfraProjectDetail.TotalQty,
                        UnitCost = (decimal)endUserInfraProjectDetail.EstimatedBudget,
                        EstimatedBudget = (decimal)endUserInfraProjectDetail.EstimatedBudget,
                        PPMPDetailStatus = PPMPDetailStatus.ItemAccepted,
                        UpdateFlag = false
                    });
                    infraDetails.AddRange(infraProjectDetails.Select(d => new PPMPDetails
                    {
                        ProjectDetailsID = endUserInfraProjectDetail.ID,
                        UACS = ppmpHeader.UACS,
                        PPMPHeaderReference = ppmpHeader.ID,
                        ItemFullName = d.FKInfraMaterialsReference.ItemName,
                        ItemSpecifications = d.FKInfraMaterialsReference.ItemSpecifications,
                        ProcurementSource = endUserInfraProjectDetail.ProcurementSource,
                        UOMReference = (int)d.UOMReference,
                        CategoryReference = endUserInfraProjectDetail.CategoryReference,
                        Justification = endUserInfraProjectDetail.Justification,
                        JANMilestone = infraProject.FKEndUserProjectReference.DeliveryMonth == 1 ? PPMPMilestones.PRPreparationOCT :
                                                   infraProject.FKEndUserProjectReference.DeliveryMonth == 2 ? PPMPMilestones.PRPreparationNOV :
                                                   infraProject.FKEndUserProjectReference.DeliveryMonth == 3 ? PPMPMilestones.PRPreparationDEC :
                                                   infraProject.FKEndUserProjectReference.DeliveryMonth == 4 ? PPMPMilestones.PRPreparation : (PPMPMilestones?)null,
                        FEBMilestone = infraProject.FKEndUserProjectReference.DeliveryMonth == 2 ? PPMPMilestones.DeliveryRISPreparation :
                                                   infraProject.FKEndUserProjectReference.DeliveryMonth == 3 ? PPMPMilestones.ProcurementActivities :
                                                   infraProject.FKEndUserProjectReference.DeliveryMonth == 4 ? PPMPMilestones.ProcurementActivities :
                                                   infraProject.FKEndUserProjectReference.DeliveryMonth == 5 ? PPMPMilestones.PRPreparation : (PPMPMilestones?)null,
                        MARMilestone = infraProject.FKEndUserProjectReference.DeliveryMonth == 3 ? PPMPMilestones.DeliveryRISPreparation :
                                                   infraProject.FKEndUserProjectReference.DeliveryMonth == 4 ? PPMPMilestones.ProcurementActivities :
                                                   infraProject.FKEndUserProjectReference.DeliveryMonth == 5 ? PPMPMilestones.ProcurementActivities :
                                                   infraProject.FKEndUserProjectReference.DeliveryMonth == 6 ? PPMPMilestones.PRPreparation : (PPMPMilestones?)null,
                        APRMilestone = infraProject.FKEndUserProjectReference.DeliveryMonth == 4 ? PPMPMilestones.DeliveryRISPreparation :
                                                   infraProject.FKEndUserProjectReference.DeliveryMonth == 5 ? PPMPMilestones.ProcurementActivities :
                                                   infraProject.FKEndUserProjectReference.DeliveryMonth == 6 ? PPMPMilestones.ProcurementActivities :
                                                   infraProject.FKEndUserProjectReference.DeliveryMonth == 7 ? PPMPMilestones.PRPreparation : (PPMPMilestones?)null,
                        MAYMilestone = infraProject.FKEndUserProjectReference.DeliveryMonth == 5 ? PPMPMilestones.DeliveryRISPreparation :
                                                   infraProject.FKEndUserProjectReference.DeliveryMonth == 6 ? PPMPMilestones.ProcurementActivities :
                                                   infraProject.FKEndUserProjectReference.DeliveryMonth == 7 ? PPMPMilestones.ProcurementActivities :
                                                   infraProject.FKEndUserProjectReference.DeliveryMonth == 8 ? PPMPMilestones.PRPreparation : (PPMPMilestones?)null,
                        JUNMilestone = infraProject.FKEndUserProjectReference.DeliveryMonth == 6 ? PPMPMilestones.DeliveryRISPreparation :
                                                   infraProject.FKEndUserProjectReference.DeliveryMonth == 7 ? PPMPMilestones.ProcurementActivities :
                                                   infraProject.FKEndUserProjectReference.DeliveryMonth == 8 ? PPMPMilestones.ProcurementActivities :
                                                   infraProject.FKEndUserProjectReference.DeliveryMonth == 9 ? PPMPMilestones.PRPreparation : (PPMPMilestones?)null,
                        JULMilestone = infraProject.FKEndUserProjectReference.DeliveryMonth == 7 ? PPMPMilestones.DeliveryRISPreparation :
                                                   infraProject.FKEndUserProjectReference.DeliveryMonth == 8 ? PPMPMilestones.ProcurementActivities :
                                                   infraProject.FKEndUserProjectReference.DeliveryMonth == 9 ? PPMPMilestones.ProcurementActivities :
                                                   infraProject.FKEndUserProjectReference.DeliveryMonth == 10 ? PPMPMilestones.PRPreparation : (PPMPMilestones?)null,
                        AUGMilestone = infraProject.FKEndUserProjectReference.DeliveryMonth == 8 ? PPMPMilestones.DeliveryRISPreparation :
                                                   infraProject.FKEndUserProjectReference.DeliveryMonth == 9 ? PPMPMilestones.ProcurementActivities :
                                                   infraProject.FKEndUserProjectReference.DeliveryMonth == 10 ? PPMPMilestones.ProcurementActivities :
                                                   infraProject.FKEndUserProjectReference.DeliveryMonth == 11 ? PPMPMilestones.PRPreparation : (PPMPMilestones?)null,
                        SEPMilestone = infraProject.FKEndUserProjectReference.DeliveryMonth == 9 ? PPMPMilestones.DeliveryRISPreparation :
                                                   infraProject.FKEndUserProjectReference.DeliveryMonth == 10 ? PPMPMilestones.ProcurementActivities :
                                                   infraProject.FKEndUserProjectReference.DeliveryMonth == 11 ? PPMPMilestones.ProcurementActivities :
                                                   infraProject.FKEndUserProjectReference.DeliveryMonth == 12 ? PPMPMilestones.PRPreparation : (PPMPMilestones?)null,
                        OCTMilestone = infraProject.FKEndUserProjectReference.DeliveryMonth == 10 ? PPMPMilestones.DeliveryRISPreparation :
                                                   infraProject.FKEndUserProjectReference.DeliveryMonth == 11 ? PPMPMilestones.ProcurementActivities :
                                                   infraProject.FKEndUserProjectReference.DeliveryMonth == 12 ? PPMPMilestones.ProcurementActivities : (PPMPMilestones?)null,
                        NOVMilestone = infraProject.FKEndUserProjectReference.DeliveryMonth == 11 ? PPMPMilestones.DeliveryRISPreparation :
                                                   infraProject.FKEndUserProjectReference.DeliveryMonth == 12 ? PPMPMilestones.ProcurementActivities : (PPMPMilestones?)null,
                        DECMilestone = infraProject.FKEndUserProjectReference.DeliveryMonth == 12 ? PPMPMilestones.DeliveryRISPreparation : (PPMPMilestones?)null,
                        JanQty = infraProject.FKEndUserProjectReference.DeliveryMonth == 1 ? d.Quantity : 0,
                        FebQty = infraProject.FKEndUserProjectReference.DeliveryMonth == 2 ? d.Quantity : 0,
                        MarQty = infraProject.FKEndUserProjectReference.DeliveryMonth == 3 ? d.Quantity : 0,
                        Q1TotalQty = d.Quantity,
                        AprQty = infraProject.FKEndUserProjectReference.DeliveryMonth == 4 ? d.Quantity : 0,
                        MayQty = infraProject.FKEndUserProjectReference.DeliveryMonth == 5 ? d.Quantity : 0,
                        JunQty = infraProject.FKEndUserProjectReference.DeliveryMonth == 6 ? d.Quantity : 0,
                        Q2TotalQty = d.Quantity,
                        JulQty = infraProject.FKEndUserProjectReference.DeliveryMonth == 7 ? d.Quantity : 0,
                        AugQty = infraProject.FKEndUserProjectReference.DeliveryMonth == 8 ? d.Quantity : 0,
                        SepQty = infraProject.FKEndUserProjectReference.DeliveryMonth == 9 ? d.Quantity : 0,
                        Q3TotalQty = d.Quantity,
                        OctQty = infraProject.FKEndUserProjectReference.DeliveryMonth == 10 ? d.Quantity : 0,
                        NovQty = infraProject.FKEndUserProjectReference.DeliveryMonth == 11 ? d.Quantity : 0,
                        DecQty = infraProject.FKEndUserProjectReference.DeliveryMonth == 2 ? d.Quantity : 0,
                        Q4TotalQty = d.Quantity,
                        TotalQty = d.Quantity,
                        UnitCost = d.TotalAmount,
                        EstimatedBudget = d.TotalAmount,
                        PPMPDetailStatus = PPMPDetailStatus.ItemAccepted,
                        UpdateFlag = false
                    }).ToList());
                    var ppmpDetails = db.PPMPDetails.AddRange(infraDetails);
                    db.PPMPDetails.AddRange(ppmpDetails);
                    if (db.SaveChanges() == 0)
                    {
                        return false;
                    }

                    var endUserProjectDetails = db.ProjectDetails.Where(d => d.FKProjectPlanReference.ProjectCode == infraProject.FKEndUserProjectReference.ProjectCode).ToList();
                    endUserProjectDetails.ForEach(d => { d.ProjectItemStatus = ProjectDetailsStatus.PostedToPPMP; });
                    if (db.SaveChanges() == 0)
                    {
                        return false;
                    }
                }

                project.ForEach(d => { d.ProjectStatus = ProjectStatus.PostedToPPMP; });
                projectDetails.ForEach(d => { d.ProjectItemStatus = ProjectDetailsStatus.PostedToPPMP; });

                if (db.SaveChanges() == 0)
                {
                    return false;
                }
            }
            

            return true;
        }
        public bool SubmitPPMP(int FiscalYear, PPMPTypes PPMPType, string UserEmail)
        {
            var user = db.UserAccounts.Where(d => d.Email == UserEmail).FirstOrDefault();
            var ppmpList = PPMPType == PPMPTypes.Original ? db.PPMPHeader.Where(d => d.FiscalYear == FiscalYear && d.Department == user.DepartmentCode && (d.PPMPType == PPMPTypes.CommonUse || d.PPMPType == PPMPTypes.Original)).ToList() :
                           PPMPType == PPMPTypes.Supplemental ? db.PPMPHeader.Where(d => d.FiscalYear == FiscalYear && d.Department == user.DepartmentCode && d.PPMPType == PPMPTypes.Supplemental).ToList() :
                                                                db.PPMPHeader.Where(d => d.FiscalYear == FiscalYear && d.Department == user.DepartmentCode && d.PPMPType == PPMPTypes.Amendatory).ToList();
            var projectDetails = db.ProjectDetails.ToList().Where(d => ppmpList.Select(x => x.UACS).Contains(d.ArticleReference != null ? d.FKItemArticleReference.UACSObjectClass : "1060401000") &&
                                                                       d.FKProjectPlanReference.Department == user.DepartmentCode &&
                                                                       d.FKProjectPlanReference.FiscalYear == FiscalYear &&
                                                                       d.ProjectItemStatus == ProjectDetailsStatus.ItemAccepted).ToList();
            var projectList = db.ProjectPlans.ToList().Where(d => d.FiscalYear == FiscalYear &&
                                                                  d.Department == user.DepartmentCode &&
                                                                  d.ProjectStatus == ProjectStatus.PostedToPPMP &&
                                                                  projectDetails.Select(x => x.FKProjectPlanReference.ProjectCode).Distinct().ToList().Contains(d.ProjectCode)).ToList();
            var ppmpDetails = db.PPMPDetails.ToList().Where(d => ppmpList.Select(x => x.ReferenceNo).Distinct().Contains(d.FKPPMPHeaderReference.ReferenceNo)).ToList();
            ppmpList.ForEach(d => { d.PPMPStatus = PPMPStatus.ForwardedToBudgetOffice; d.SubmittedAt = DateTime.Now; });
            ppmpDetails.ForEach(d => { d.PPMPDetailStatus = PPMPDetailStatus.ForEvaluation; });
            projectList.ForEach(d => { d.ProjectStatus = ProjectStatus.ForwardedToBudgetOffice; });
            projectDetails.ForEach(d => { d.ProjectItemStatus = ProjectDetailsStatus.ForApproval; });
            if (db.SaveChanges() == 0)
            {
                return false;
            }
            return true;
        }
        public bool UpdatePPMPItem(PPMPItemDetailsVM ItemDetails)
        {
            var item = db.Items.ToList().Where(d => d.ItemCode == ItemDetails.Item.ItemCode).FirstOrDefault();
            foreach (var details in ItemDetails.ProjectPlans)
            {
                if (details.ItemStatus == PPMPDetailStatus.ForRevision)
                {
                    var ppmpDetails = db.PPMPDetails.Where(d => d.FKProjectDetailsReference.FKProjectPlanReference.ProjectCode == details.ProjectCode &&
                                                                d.FKPPMPHeaderReference.ReferenceNo == ItemDetails.Item.ReferenceNo &&
                                                                d.FKItemArticleReference.ArticleCode + "-" + d.ItemSequence == item.ItemCode).FirstOrDefault();
                    var totalQty = details.JanQty + details.FebQty + details.MarQty + details.AprQty + details.MayQty + details.JunQty + details.JulQty + details.AugQty + details.SepQty + details.OctQty + details.NovQty + details.DecQty;
                    ppmpDetails.JanQty = details.JanQty;
                    ppmpDetails.FebQty = details.FebQty;
                    ppmpDetails.MarQty = details.MarQty;
                    ppmpDetails.AprQty = details.AprQty;
                    ppmpDetails.MayQty = details.MayQty;
                    ppmpDetails.JunQty = details.JunQty;
                    ppmpDetails.JulQty = details.JulQty;
                    ppmpDetails.AugQty = details.AugQty;
                    ppmpDetails.SepQty = details.SepQty;
                    ppmpDetails.OctQty = details.OctQty;
                    ppmpDetails.NovQty = details.NovQty;
                    ppmpDetails.DecQty = details.DecQty;
                    ppmpDetails.TotalQty = totalQty;
                    ppmpDetails.Justification = details.Justification;
                    ppmpDetails.EstimatedBudget = Math.Round((ppmpDetails.UnitCost * totalQty), 2);
                    ppmpDetails.UpdateFlag = false;
                    ppmpDetails.PPMPDetailStatus = PPMPDetailStatus.ItemRevised;

                    var ppmp = db.PPMPHeader.Where(d => d.ReferenceNo == ItemDetails.Item.ReferenceNo).FirstOrDefault();
                    ppmp.PPMPStatus = PPMPStatus.ForwardedToBudgetOffice;

                    if (db.SaveChanges() == 0)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        //public List<MOOEViewModel> GetMOOE(string UserEmail, int FiscalYear)
        //{
        //    List<MOOEViewModel> mooe = new List<MOOEViewModel>();
        //    var user = db.UserAccounts.Where(d => d.Email == UserEmail).FirstOrDefault();
        //    var office = hris.GetDepartmentDetails(user.DepartmentCode);
        //    var accountList = abis.GetChartOfAccounts().Where(d => d.ClassCode == "5").ToList();

        //    var tier1Actual = (from items in db.Items
        //                       join ppmp in db.ProjectDetails on items.ID equals ppmp.ItemReference
        //                       where (office.SectionCode == null ? ppmp.FKPPMPReference.Department == office.DepartmentCode : ppmp.FKPPMPReference.Unit == office.SectionCode) && ppmp.FKPPMPReference.FiscalYear < FiscalYear && ppmp.UnitCost < 15000.00m && ppmp.ProposalType == BudgetProposalType.Actual
        //                       select new { UACS = items.FKItemTypeReference.UACSObjectClass, EstimatedBudget = ppmp.ProjectEstimatedBudget }
        //                       into results
        //                       group results by results.UACS into groupResult
        //                       select new { UACS = groupResult.Key, Amount = groupResult.Sum(d => d.EstimatedBudget) }).ToList();

        //    var tier1Ongoing = (from items in db.Items
        //                        join ppmp in db.ProjectPlanItems on items.ID equals ppmp.ItemReference
        //                        where (office.SectionCode == null ? ppmp.FKPPMPReference.Department == office.DepartmentCode : ppmp.FKPPMPReference.Unit == office.SectionCode) && ppmp.UnitCost < 15000.00m && ppmp.Status == "Procurement On-going"
        //                        select new { UACS = items.FKItemTypeReference.UACSObjectClass, EstimatedBudget = ppmp.ProjectEstimatedBudget }
        //                        into results
        //                        group results by results.UACS into groupResult
        //                        select new { UACS = groupResult.Key, Amount = groupResult.Sum(d => d.EstimatedBudget) }).ToList();

        //    var tier1Items = tier1Actual.Concat(tier1Ongoing).ToList();

        //    var tier2Items = (from items in db.Items
        //                      join ppmp in db.ProjectPlanItems on items.ID equals ppmp.ItemReference
        //                      where (office.SectionCode == null ? ppmp.FKPPMPReference.Department == office.DepartmentCode : ppmp.FKPPMPReference.Unit == office.SectionCode) && ppmp.FKPPMPReference.FiscalYear == FiscalYear && ppmp.UnitCost < 15000.00m && ppmp.ProposalType == BudgetProposalType.NewProposal
        //                      select new { UACS = items.FKItemTypeReference.UACSObjectClass, EstimatedBudget = ppmp.ProjectEstimatedBudget }
        //                      into results
        //                      group results by results.UACS into groupResult
        //                      select new { UACS = groupResult.Key, Amount = groupResult.Sum(d => d.EstimatedBudget) }).ToList();

        //    var mooeItems = (from accounts in accountList
        //                     join tier1 in tier1Items on accounts.UACS_Code equals tier1.UACS into tier1
        //                     from t1 in tier1.DefaultIfEmpty()
        //                     join tier2 in tier2Items on accounts.UACS_Code equals tier2.UACS into tier2
        //                     from t2 in tier2.DefaultIfEmpty()
        //                     where t1 != null || t2 != null
        //                     select new MOOEViewModel
        //                     {
        //                         UACS = accounts.UACS_Code,
        //                         SubClassification = accounts.SubAcctName,
        //                         ObjectClassification = accounts.AcctName,
        //                         Tier1 = t1 == null ? 0.00m : (decimal)t1.Amount,
        //                         Tier2 = t2 == null ? 0.00m : (decimal)t2.Amount,
        //                         TotalProposedProgram = (t1 == null ? 0.00m : (decimal)t1.Amount) + (t2 == null ? 0.00m : (decimal)t2.Amount)
        //                     }).ToList();

        //    var tier1Services = (from services in db.Items
        //                         join ppmp in db.ProjectPlanServices on services.ID equals ppmp.ItemReference
        //                         where (office.SectionCode == null ? ppmp.FKPPMPReference.Department == office.DepartmentCode : ppmp.FKPPMPReference.Unit == office.SectionCode) && ppmp.UnitCost < 15000.00m && ppmp.Status == "Procurement On-going"
        //                         select new { UACS = services.FKItemTypeReference.UACSObjectClass, EstimatedBudget = ppmp.ProjectEstimatedBudget }
        //                         into results
        //                         group results by results.UACS into groupResult
        //                         select new { UACS = groupResult.Key, Amount = groupResult.Sum(d => d.EstimatedBudget) }).ToList();

        //    var tier2Services = (from services in db.Items
        //                         join ppmp in db.ProjectPlanServices on services.ID equals ppmp.ItemReference
        //                         where (office.SectionCode == null ? ppmp.FKPPMPReference.Department == office.DepartmentCode : ppmp.FKPPMPReference.Unit == office.SectionCode) && ppmp.FKPPMPReference.FiscalYear == FiscalYear && ppmp.UnitCost < 15000.00m && ppmp.ProposalType == BudgetProposalType.NewProposal
        //                         select new { UACS = services.FKItemTypeReference.UACSObjectClass, EstimatedBudget = ppmp.ProjectEstimatedBudget }
        //                         into results
        //                         group results by results.UACS into groupResult
        //                         select new { UACS = groupResult.Key, Amount = groupResult.Sum(d => d.EstimatedBudget) }).ToList();

        //    var mooeService = (from accounts in accountList
        //                       join tier1 in tier1Services on accounts.UACS_Code equals tier1.UACS into tier1
        //                       from t1 in tier1.DefaultIfEmpty()
        //                       join tier2 in tier2Services on accounts.UACS_Code equals tier2.UACS into tier2
        //                       from t2 in tier2.DefaultIfEmpty()
        //                       where t1 != null || t2 != null
        //                       select new MOOEViewModel
        //                       {
        //                           UACS = accounts.UACS_Code,
        //                           SubClassification = accounts.SubAcctName,
        //                           ObjectClassification = accounts.AcctName,
        //                           Tier1 = t1 == null ? 0.00m : (decimal)t1.Amount,
        //                           Tier2 = t2 == null ? 0.00m : (decimal)t2.Amount,
        //                           TotalProposedProgram = (t1 == null ? 0.00m : (decimal)t1.Amount) + (t2 == null ? 0.00m : (decimal)t2.Amount)
        //                       }).ToList();

        //    mooe.AddRange(mooeItems);
        //    mooe.AddRange(mooeService);

        //    return mooe;
        //}
        //public List<CapitalOutlayVM> GetCapitalOutlay(string UserEmail, int FiscalYear)
        //{
        //    List<CapitalOutlayVM> capitalOutlay = new List<CapitalOutlayVM>();
        //    var user = db.UserAccounts.Where(d => d.Email == UserEmail).FirstOrDefault();
        //    var office = hrisDataAccess.GetFullDepartmentDetails(user.DepartmentCode);
        //    var accountList = abisDataAccess.GetChartOfAccounts();

        //    var tier1Actual = (from items in db.Items
        //                       join ppmp in db.ProjectPlanItems on items.ID equals ppmp.ItemReference
        //                       where (office.SectionCode == null ? ppmp.FKPPMPReference.Department == office.DepartmentCode : ppmp.FKPPMPReference.Unit == office.SectionCode) && ppmp.FKPPMPReference.FiscalYear == FiscalYear && ppmp.UnitCost >= 15000.00m && ppmp.ProposalType == BudgetProposalType.Actual
        //                       select new { UACS = items.FKItemTypeReference.UACSObjectClass, EstimatedBudget = ppmp.ProjectEstimatedBudget }
        //                       into results
        //                       group results by results.UACS into groupResult
        //                       select new { UACS = groupResult.Key, Amount = groupResult.Sum(d => d.EstimatedBudget) }).ToList();

        //    var tier1Ongoing = (from items in db.Items
        //                        join ppmp in db.ProjectPlanItems on items.ID equals ppmp.ItemReference
        //                        where (office.SectionCode == null ? ppmp.FKPPMPReference.Department == office.DepartmentCode : ppmp.FKPPMPReference.Unit == office.SectionCode) && ppmp.UnitCost >= 15000.00m && ppmp.Status == "Procurement On-going"
        //                        select new { UACS = items.FKItemTypeReference.UACSObjectClass, EstimatedBudget = ppmp.ProjectEstimatedBudget }
        //                        into results
        //                        group results by results.UACS into groupResult
        //                        select new { UACS = groupResult.Key, Amount = groupResult.Sum(d => d.EstimatedBudget) }).ToList();

        //    var tier1Items = tier1Actual.Concat(tier1Ongoing).ToList();

        //    var tier2Items = (from items in db.Items
        //                      join ppmp in db.ProjectPlanItems on items.ID equals ppmp.ItemReference
        //                      where (office.SectionCode == null ? ppmp.FKPPMPReference.Department == office.DepartmentCode : ppmp.FKPPMPReference.Unit == office.SectionCode) && ppmp.FKPPMPReference.FiscalYear == FiscalYear && ppmp.UnitCost >= 15000.00m && ppmp.ProposalType == BudgetProposalType.NewProposal
        //                      select new { UACS = items.FKItemTypeReference.UACSObjectClass, EstimatedBudget = ppmp.ProjectEstimatedBudget }
        //                      into results
        //                      group results by results.UACS into groupResult
        //                      select new { UACS = groupResult.Key, Amount = groupResult.Sum(d => d.EstimatedBudget) }).ToList();

        //    var capitalOutlayItems = (from accounts in accountList
        //                              join tier1 in tier1Items on accounts.UACS_Code equals tier1.UACS into tier1
        //                              from t1 in tier1.DefaultIfEmpty()
        //                              join tier2 in tier2Items on accounts.UACS_Code equals tier2.UACS into tier2
        //                              from t2 in tier2.DefaultIfEmpty()
        //                              where t1 != null || t2 != null
        //                              select new CapitalOutlayVM
        //                              {
        //                                  UACS = accounts.UACS_Code,
        //                                  SubClassification = accounts.SubAcctName,
        //                                  ObjectClassification = accounts.AcctName,
        //                                  Tier1 = t1 == null ? 0.00m : (decimal)t1.Amount,
        //                                  Tier2 = t2 == null ? 0.00m : (decimal)t2.Amount,
        //                                  TotalProposedProgram = (t1 == null ? 0.00m : (decimal)t1.Amount) + (t2 == null ? 0.00m : (decimal)t2.Amount)
        //                              }).ToList();

        //    var tier1Services = (from services in db.Items
        //                         join ppmp in db.ProjectPlanServices on services.ID equals ppmp.ItemReference
        //                         where (office.SectionCode == null ? ppmp.FKPPMPReference.Department == office.DepartmentCode : ppmp.FKPPMPReference.Unit == office.SectionCode) && ppmp.UnitCost >= 15000.00m && ppmp.Status == "Procurement On-going"
        //                         select new { UACS = services.FKItemTypeReference.UACSObjectClass, EstimatedBudget = ppmp.ProjectEstimatedBudget }
        //                         into results
        //                         group results by results.UACS into groupResult
        //                         select new { UACS = groupResult.Key, Amount = groupResult.Sum(d => d.EstimatedBudget) }).ToList();

        //    var tier2Services = (from services in db.Items
        //                         join ppmp in db.ProjectPlanServices on services.ID equals ppmp.ItemReference
        //                         where (office.SectionCode == null ? ppmp.FKPPMPReference.Department == office.DepartmentCode : ppmp.FKPPMPReference.Unit == office.SectionCode) && ppmp.FKPPMPReference.FiscalYear == FiscalYear && ppmp.UnitCost >= 15000.00m && ppmp.ProposalType == BudgetProposalType.NewProposal
        //                         select new { UACS = services.FKItemTypeReference.UACSObjectClass, EstimatedBudget = ppmp.ProjectEstimatedBudget }
        //                         into results
        //                         group results by results.UACS into groupResult
        //                         select new { UACS = groupResult.Key, Amount = groupResult.Sum(d => d.EstimatedBudget) }).ToList();

        //    var capitalOutlayService = (from accounts in accountList
        //                                join tier1 in tier1Services on accounts.UACS_Code equals tier1.UACS into tier1
        //                                from t1 in tier1.DefaultIfEmpty()
        //                                join tier2 in tier2Services on accounts.UACS_Code equals tier2.UACS into tier2
        //                                from t2 in tier2.DefaultIfEmpty()
        //                                where t1 != null || t2 != null
        //                                select new CapitalOutlayVM
        //                                {
        //                                    UACS = accounts.UACS_Code,
        //                                    SubClassification = accounts.SubAcctName,
        //                                    ObjectClassification = accounts.AcctName,
        //                                    Tier1 = t1 == null ? 0.00m : (decimal)t1.Amount,
        //                                    Tier2 = t2 == null ? 0.00m : (decimal)t2.Amount,
        //                                    TotalProposedProgram = (t1 == null ? 0.00m : (decimal)t1.Amount) + (t2 == null ? 0.00m : (decimal)t2.Amount)
        //                                }).ToList();

        //    capitalOutlay.AddRange(capitalOutlayItems);
        //    capitalOutlay.AddRange(capitalOutlayService);

        //    return capitalOutlay;
        //}
        //public List<PPMPHeaderViewModel> GetPPMPList(string UserEmail, int FiscalYear)
        //{
        //    var user = db.UserAccounts.Where(d => d.Email == UserEmail).FirstOrDefault();
        //    var office = hrisDataAccess.GetFullDepartmentDetails(user.DepartmentCode);
        //    return db.PPMPHeader.Where(d => (office.SectionCode == null ? d.Department == office.DepartmentCode : d.Unit == office.SectionCode) && d.FiscalYear == FiscalYear)
        //           .Select(d => new PPMPHeaderViewModel
        //           {
        //               ReferenceNo = d.ReferenceNo,
        //               FiscalYear = d.FiscalYear,
        //               PPMPType = d.FKPPMPTypeReference.InventoryTypeName,
        //               EstimatedBudget = (db.InventoryTypes.Where(x => x.ID == d.PPMPType).FirstOrDefault().IsTangible) ? (decimal)db.ProjectPlanItems.Where(x => x.PPMPReference == d.ID).Sum(x => x.ProjectEstimatedBudget) : (decimal)db.ProjectPlanServices.Where(x => x.PPMPReference == d.ID).Sum(x => x.ProjectEstimatedBudget),
        //               ApprovedBudget = (decimal)d.ABC,
        //               Status = d.Status
        //           }).ToList();
        //}
        //public PPMPViewModel GetPPMPDetails(string ReferenceNo, string UserEmail)
        //{
        //    var preparedBy = db.PPMPHeader.Where(d => d.ReferenceNo == ReferenceNo).FirstOrDefault().PreparedBy;
        //    var employee = hrisDataAccess.GetEmployeeByCode(preparedBy);
        //    var office = hrisDataAccess.GetFullDepartmentDetails(employee.DepartmentCode);
        //    var ppmpHeader = db.PPMPHeader.Where(d => d.ReferenceNo == ReferenceNo && d.Department == office.DepartmentCode).FirstOrDefault();

        //    PPMPViewModel ppmpVM = new PPMPViewModel();
        //    ppmpVM.DBMItems = new List<PPMPItemDetailsVM>();
        //    ppmpVM.NonDBMItems = new List<PPMPItemDetailsVM>();
        //    ppmpVM.Header = new PPMPHeaderViewModel
        //    {
        //        ReferenceNo = ppmpHeader.ReferenceNo,
        //        FiscalYear = ppmpHeader.FiscalYear,
        //        PPMPType = ppmpHeader.FKPPMPTypeReference.InventoryTypeName,
        //        Sector = office.Sector == null ? office.Department : office.Sector,
        //        Department = office.Department,
        //        Unit = office.Section,
        //        EstimatedBudget = (db.InventoryTypes.Where(x => x.ID == ppmpHeader.PPMPType).FirstOrDefault().IsTangible) ? (decimal)db.ProjectPlanItems.Where(x => x.PPMPReference == ppmpHeader.ID).Sum(x => x.ProjectEstimatedBudget) : (decimal)db.ProjectPlanServices.Where(x => x.PPMPReference == ppmpHeader.ID).Sum(x => x.ProjectEstimatedBudget),
        //        Status = ppmpHeader.Status,
        //        CreatedAt = (DateTime)ppmpHeader.CreatedAt,
        //        EvaluatedAt = ppmpHeader.ApprovedAt,
        //        SubmittedAt = ppmpHeader.SubmittedAt,
        //        PreparedBy = employee.EmployeeName + ", " + employee.Designation,
        //        SubmittedBy = ppmpHeader.SubmittedBy
        //    };

        //    if (ppmpVM.Header == null)
        //    {
        //        return null;
        //    }

        //    ppmpVM.Workflow = db.PPMPApprovalWorkflow.Where(d => d.FKPPMPHeader.ReferenceNo == ReferenceNo).Select(d => new PPMPApprovalWorkflowViewModel
        //    {
        //        ReferenceNo = ReferenceNo,
        //        Status = d.Status,
        //        UpdatedAt = d.UpdatedAt,
        //        Remarks = d.Remarks,
        //        Office = office.Department,
        //        Personnel = employee.EmployeeName
        //    }).ToList();
        //    var inventoryType = db.InventoryTypes.Where(d => d.InventoryTypeName == ppmpVM.Header.PPMPType).FirstOrDefault();
        //    if (inventoryType.IsTangible)
        //    {
        //        var DBMItems = db.ProjectPlanItems.Where(d => d.FKPPMPReference.ReferenceNo == ppmpVM.Header.ReferenceNo && d.FKItemReference.ProcurementSource == ProcurementSources.AgencyToAgency).ToList();
        //        ppmpVM.DBMItems = DBMItems.Select(d => new PPMPItemDetailsVM
        //        {
        //            ProjectCode = d.FKProjectReference.ProjectCode,
        //            Project = d.FKProjectReference.ProjectName,
        //            ItemCode = d.FKItemReference.ItemCode,
        //            ItemName = d.FKItemReference.ItemFullName,
        //            ItemSpecifications = d.FKItemReference.ItemSpecifications,
        //            ProcurementSource = d.FKItemReference.ProcurementSource,
        //            ItemImage = d.FKItemReference.ItemImage,
        //            Category = d.FKItemReference.FKCategoryReference.ItemCategoryName,
        //            IndividualUOMReference = d.FKItemReference.FKIndividualUnitReference.Abbreviation,
        //            JanMilestone = d.FKProjectReference.ProjectCode.StartsWith("CSPR") ? d.ProjectJanQty == null ? "0" : d.ProjectJanQty.ToString() : d.FKProjectReference.ProjectCode.StartsWith("EUPR") ? d.PPMPJan : d.ProjectJanQty == null ? "0" : d.ProjectJanQty.ToString(),
        //            FebMilestone = d.FKProjectReference.ProjectCode.StartsWith("CSPR") ? d.ProjectFebQty == null ? "0" : d.ProjectFebQty.ToString() : d.FKProjectReference.ProjectCode.StartsWith("EUPR") ? d.PPMPFeb : d.ProjectFebQty == null ? "0" : d.ProjectFebQty.ToString(),
        //            MarMilestone = d.FKProjectReference.ProjectCode.StartsWith("CSPR") ? d.ProjectMarQty == null ? "0" : d.ProjectMarQty.ToString() : d.FKProjectReference.ProjectCode.StartsWith("EUPR") ? d.PPMPMar : d.ProjectMarQty == null ? "0" : d.ProjectMarQty.ToString(),
        //            AprMilestone = d.FKProjectReference.ProjectCode.StartsWith("CSPR") ? d.ProjectAprQty == null ? "0" : d.ProjectAprQty.ToString() : d.FKProjectReference.ProjectCode.StartsWith("EUPR") ? d.PPMPApr : d.ProjectAprQty == null ? "0" : d.ProjectAprQty.ToString(),
        //            MayMilestone = d.FKProjectReference.ProjectCode.StartsWith("CSPR") ? d.ProjectMayQty == null ? "0" : d.ProjectMayQty.ToString() : d.FKProjectReference.ProjectCode.StartsWith("EUPR") ? d.PPMPMay : d.ProjectMayQty == null ? "0" : d.ProjectMayQty.ToString(),
        //            JunMilestone = d.FKProjectReference.ProjectCode.StartsWith("CSPR") ? d.ProjectJunQty == null ? "0" : d.ProjectJunQty.ToString() : d.FKProjectReference.ProjectCode.StartsWith("EUPR") ? d.PPMPJun : d.ProjectJunQty == null ? "0" : d.ProjectJunQty.ToString(),
        //            JulMilestone = d.FKProjectReference.ProjectCode.StartsWith("CSPR") ? d.ProjectJulQty == null ? "0" : d.ProjectJulQty.ToString() : d.FKProjectReference.ProjectCode.StartsWith("EUPR") ? d.PPMPJul : d.ProjectJulQty == null ? "0" : d.ProjectJulQty.ToString(),
        //            AugMilestone = d.FKProjectReference.ProjectCode.StartsWith("CSPR") ? d.ProjectAugQty == null ? "0" : d.ProjectAugQty.ToString() : d.FKProjectReference.ProjectCode.StartsWith("EUPR") ? d.PPMPAug : d.ProjectAugQty == null ? "0" : d.ProjectAugQty.ToString(),
        //            SepMilestone = d.FKProjectReference.ProjectCode.StartsWith("CSPR") ? d.ProjectSepQty == null ? "0" : d.ProjectSepQty.ToString() : d.FKProjectReference.ProjectCode.StartsWith("EUPR") ? d.PPMPSep : d.ProjectSepQty == null ? "0" : d.ProjectSepQty.ToString(),
        //            OctMilestone = d.FKProjectReference.ProjectCode.StartsWith("CSPR") ? d.ProjectOctQty == null ? "0" : d.ProjectOctQty.ToString() : d.FKProjectReference.ProjectCode.StartsWith("EUPR") ? d.PPMPOct : d.ProjectOctQty == null ? "0" : d.ProjectOctQty.ToString(),
        //            NovMilestone = d.FKProjectReference.ProjectCode.StartsWith("CSPR") ? d.ProjectNovQty == null ? "0" : d.ProjectNovQty.ToString() : d.FKProjectReference.ProjectCode.StartsWith("EUPR") ? d.PPMPNov : d.ProjectNovQty == null ? "0" : d.ProjectNovQty.ToString(),
        //            DecMilestone = d.FKProjectReference.ProjectCode.StartsWith("CSPR") ? d.ProjectDecQty == null ? "0" : d.ProjectDecQty.ToString() : d.FKProjectReference.ProjectCode.StartsWith("EUPR") ? d.PPMPDec : d.ProjectDecQty == null ? "0" : d.ProjectDecQty.ToString(),
        //            TotalQty = String.Format("{0:#,##0}", d.ProjectTotalQty),
        //            UnitCost = String.Format("{0:#,##0.00}", d.UnitCost),
        //            EstimatedBudget = d.ProjectEstimatedBudget,
        //            Remarks = d.Justification,
        //            ResponsibilityCenter = d.FKItemReference.ResponsibilityCenter == null ? null : hrisDataAccess.GetDepartmentDetails(d.FKItemReference.ResponsibilityCenter).Department
        //        }).ToList();

        //        var NonDBMItems = db.ProjectPlanItems.Where(d => d.FKPPMPReference.ReferenceNo == ppmpVM.Header.ReferenceNo && d.FKItemReference.ProcurementSource == ProcurementSources.ExternalSuppliers).ToList();
        //        ppmpVM.NonDBMItems = NonDBMItems.Select(d => new PPMPItemDetailsVM
        //        {
        //            ProjectCode = d.FKProjectReference.ProjectCode,
        //            Project = d.FKProjectReference.ProjectName,
        //            ItemCode = d.FKItemReference.ItemCode,
        //            ItemName = d.FKItemReference.ItemFullName,
        //            ItemSpecifications = d.FKItemReference.ItemSpecifications,
        //            ProcurementSource = d.FKItemReference.ProcurementSource,
        //            ItemImage = d.FKItemReference.ItemImage,
        //            Category = d.FKItemReference.FKCategoryReference.ItemCategoryName,
        //            IndividualUOMReference = d.FKItemReference.FKIndividualUnitReference.Abbreviation,
        //            JanMilestone = d.FKProjectReference.ProjectCode.StartsWith("EUPR") ? d.PPMPJan : d.ProjectJanQty == null ? "0" : d.ProjectJanQty.ToString(),
        //            FebMilestone = d.FKProjectReference.ProjectCode.StartsWith("EUPR") ? d.PPMPFeb : d.ProjectFebQty == null ? "0" : d.ProjectFebQty.ToString(),
        //            MarMilestone = d.FKProjectReference.ProjectCode.StartsWith("EUPR") ? d.PPMPMar : d.ProjectMarQty == null ? "0" : d.ProjectMarQty.ToString(),
        //            AprMilestone = d.FKProjectReference.ProjectCode.StartsWith("EUPR") ? d.PPMPApr : d.ProjectAprQty == null ? "0" : d.ProjectAprQty.ToString(),
        //            MayMilestone = d.FKProjectReference.ProjectCode.StartsWith("EUPR") ? d.PPMPMay : d.ProjectMayQty == null ? "0" : d.ProjectMayQty.ToString(),
        //            JunMilestone = d.FKProjectReference.ProjectCode.StartsWith("EUPR") ? d.PPMPJun : d.ProjectJunQty == null ? "0" : d.ProjectJunQty.ToString(),
        //            JulMilestone = d.FKProjectReference.ProjectCode.StartsWith("EUPR") ? d.PPMPJul : d.ProjectJulQty == null ? "0" : d.ProjectJulQty.ToString(),
        //            AugMilestone = d.FKProjectReference.ProjectCode.StartsWith("EUPR") ? d.PPMPAug : d.ProjectAugQty == null ? "0" : d.ProjectAugQty.ToString(),
        //            SepMilestone = d.FKProjectReference.ProjectCode.StartsWith("EUPR") ? d.PPMPSep : d.ProjectSepQty == null ? "0" : d.ProjectSepQty.ToString(),
        //            OctMilestone = d.FKProjectReference.ProjectCode.StartsWith("EUPR") ? d.PPMPOct : d.ProjectOctQty == null ? "0" : d.ProjectOctQty.ToString(),
        //            NovMilestone = d.FKProjectReference.ProjectCode.StartsWith("EUPR") ? d.PPMPNov : d.ProjectNovQty == null ? "0" : d.ProjectNovQty.ToString(),
        //            DecMilestone = d.FKProjectReference.ProjectCode.StartsWith("EUPR") ? d.PPMPDec : d.ProjectDecQty == null ? "0" : d.ProjectDecQty.ToString(),
        //            Supplier1Name = d.FKSupplier1Reference.SupplierName,
        //            Supplier1Address = d.FKSupplier1Reference.Address,
        //            Supplier1ContactNo = d.FKSupplier1Reference.ContactNumber,
        //            Supplier1EmailAddress = d.FKSupplier1Reference.EmailAddress,
        //            Supplier1UnitCost = d.Supplier1UnitCost,
        //            Supplier2Name = d.FKSupplier2Reference == null ? "N/A" : d.FKSupplier2Reference.SupplierName,
        //            Supplier2Address = d.FKSupplier2Reference == null ? "N/A" : d.FKSupplier2Reference.Address,
        //            Supplier2ContactNo = d.FKSupplier2Reference == null ? "N/A" : d.FKSupplier2Reference.ContactNumber,
        //            Supplier2EmailAddress = d.FKSupplier2Reference == null ? "N/A" : d.FKSupplier2Reference.EmailAddress,
        //            Supplier2UnitCost = d.FKSupplier2Reference == null ? 0.00m : d.Supplier2UnitCost,
        //            Supplier3Name = d.FKSupplier3Reference == null ? "N/A" : d.FKSupplier3Reference.SupplierName,
        //            Supplier3Address = d.FKSupplier3Reference == null ? "N/A" : d.FKSupplier3Reference.Address,
        //            Supplier3ContactNo = d.FKSupplier3Reference == null ? "N/A" : d.FKSupplier3Reference.ContactNumber,
        //            Supplier3EmailAddress = d.FKSupplier3Reference == null ? "N/A" : d.FKSupplier3Reference.EmailAddress,
        //            Supplier3UnitCost = d.FKSupplier3Reference == null ? 0.00m : d.Supplier3UnitCost,
        //            TotalQty = String.Format("{0:#,##0}", d.ProjectTotalQty),
        //            UnitCost = String.Format("{0:#,##0.00}", d.UnitCost),
        //            EstimatedBudget = d.ProjectEstimatedBudget,
        //            Remarks = d.Justification,
        //            ResponsibilityCenter = d.FKItemReference.ResponsibilityCenter == null ? null : hrisDataAccess.GetDepartmentDetails(d.FKItemReference.ResponsibilityCenter).Department
        //        }).ToList();
        //    }
        //    else
        //    {
        //        var DBMItems = db.ProjectPlanServices.Where(d => d.FKPPMPReference.ReferenceNo == ppmpVM.Header.ReferenceNo && d.FKItemReference.ProcurementSource == ProcurementSources.AgencyToAgency).ToList();
        //        ppmpVM.DBMItems = DBMItems.Select(d => new PPMPItemDetailsVM
        //        {
        //            ProjectCode = d.FKProjectReference.ProjectCode,
        //            Project = d.FKProjectReference.ProjectName,
        //            Category = d.FKItemReference.FKCategoryReference.ItemCategoryName,
        //            ItemCode = d.FKItemReference.ItemCode,
        //            ItemName = d.FKItemReference.ItemFullName,
        //            ItemSpecifications = d.ItemSpecifications,
        //            ProcurementSource = d.FKItemReference.ProcurementSource,
        //            ItemImage = null,
        //            IndividualUOMReference = null,
        //            JanMilestone = d.PPMPJan == null ? "0" : d.PPMPJan,
        //            FebMilestone = d.PPMPFeb == null ? "0" : d.PPMPFeb,
        //            MarMilestone = d.PPMPMar == null ? "0" : d.PPMPMar,
        //            AprMilestone = d.PPMPApr == null ? "0" : d.PPMPApr,
        //            MayMilestone = d.PPMPMay == null ? "0" : d.PPMPMay,
        //            JunMilestone = d.PPMPJun == null ? "0" : d.PPMPJun,
        //            JulMilestone = d.PPMPJul == null ? "0" : d.PPMPJul,
        //            AugMilestone = d.PPMPAug == null ? "0" : d.PPMPAug,
        //            SepMilestone = d.PPMPSep == null ? "0" : d.PPMPSep,
        //            OctMilestone = d.PPMPOct == null ? "0" : d.PPMPOct,
        //            NovMilestone = d.PPMPNov == null ? "0" : d.PPMPNov,
        //            DecMilestone = d.PPMPDec == null ? "0" : d.PPMPDec,
        //            TotalQty = String.Format("{0:#,##0}", d.ProjectQuantity),
        //            UnitCost = String.Format("{0:#,##0.00}", d.UnitCost),
        //            EstimatedBudget = d.ProjectEstimatedBudget,
        //            Remarks = d.Justification,
        //            ResponsibilityCenter = d.FKItemReference.ResponsibilityCenter == null ? null : hrisDataAccess.GetDepartmentDetails(d.FKItemReference.ResponsibilityCenter).Department
        //        }).ToList();

        //        var NonDBMItems = db.ProjectPlanServices.Where(d => d.FKPPMPReference.ReferenceNo == ppmpVM.Header.ReferenceNo && d.FKItemReference.ProcurementSource == ProcurementSources.ExternalSuppliers).ToList();
        //        ppmpVM.NonDBMItems = NonDBMItems.Select(d => new PPMPItemDetailsVM
        //        {
        //            ProjectCode = d.FKProjectReference.ProjectCode,
        //            Project = d.FKProjectReference.ProjectName,
        //            Category = d.FKItemReference.FKCategoryReference.ItemCategoryName,
        //            ItemCode = d.FKItemReference.ItemCode,
        //            ItemName = d.FKItemReference.ItemFullName,
        //            ItemSpecifications = d.ItemSpecifications,
        //            ProcurementSource = d.FKItemReference.ProcurementSource,
        //            ItemImage = null,
        //            IndividualUOMReference = null,
        //            JanMilestone = d.PPMPJan,
        //            FebMilestone = d.PPMPFeb,
        //            MarMilestone = d.PPMPMar,
        //            AprMilestone = d.PPMPApr,
        //            MayMilestone = d.PPMPMay,
        //            JunMilestone = d.PPMPJun,
        //            JulMilestone = d.PPMPJul,
        //            AugMilestone = d.PPMPAug,
        //            SepMilestone = d.PPMPSep,
        //            OctMilestone = d.PPMPOct,
        //            NovMilestone = d.PPMPNov,
        //            DecMilestone = d.PPMPDec,
        //            Supplier1Name = d.FKSupplier1Reference.SupplierName,
        //            Supplier1Address = d.FKSupplier1Reference.Address,
        //            Supplier1ContactNo = d.FKSupplier1Reference.ContactNumber,
        //            Supplier1EmailAddress = d.FKSupplier1Reference.EmailAddress,
        //            Supplier1UnitCost = (decimal)d.Supplier1UnitCost,
        //            Supplier2Name = d.Supplier2 == null ? null : d.FKSupplier2Reference.SupplierName,
        //            Supplier2Address = d.Supplier2 == null ? null : d.FKSupplier2Reference.Address,
        //            Supplier2ContactNo = d.Supplier2 == null ? null : d.FKSupplier2Reference.ContactNumber,
        //            Supplier2EmailAddress = d.Supplier2 == null ? null : d.FKSupplier2Reference.EmailAddress,
        //            Supplier2UnitCost = d.Supplier2 == null ? null : d.Supplier2UnitCost,
        //            Supplier3Name = d.Supplier3 == null ? null : d.FKSupplier3Reference.SupplierName,
        //            Supplier3Address = d.Supplier3 == null ? null : d.FKSupplier3Reference.Address,
        //            Supplier3ContactNo = d.Supplier3 == null ? null : d.FKSupplier3Reference.ContactNumber,
        //            Supplier3EmailAddress = d.Supplier3 == null ? null : d.FKSupplier3Reference.EmailAddress,
        //            Supplier3UnitCost = d.Supplier3 == null ? null : d.Supplier3UnitCost,
        //            TotalQty = String.Format("{0:#,##0}", d.ProjectQuantity),
        //            UnitCost = String.Format("{0:#,##0.00}", d.UnitCost),
        //            EstimatedBudget = d.ProjectEstimatedBudget,
        //            Remarks = d.Justification,
        //            ResponsibilityCenter = d.FKItemReference.ResponsibilityCenter == null ? null : hrisDataAccess.GetDepartmentDetails(d.FKItemReference.ResponsibilityCenter).Department
        //        }).ToList();
        //    }

        //    ppmpVM.DBMItems = ppmpVM.DBMItems;
        //    ppmpVM.NonDBMItems = ppmpVM.NonDBMItems;
        //    return ppmpVM;
        //}
        //public List<PPMPItemDetailsVM> GetPPMPResponsibilityCenterItems(string ResponsibilityCenter, string InventoryType)
        //{
        //    var deptCode = hrisDataAccess.GetAllDepartments().Where(x => x.Department == ResponsibilityCenter).FirstOrDefault().DepartmentCode;
        //    List<PPMPItemDetailsVM> responsibilityCenterItems = new List<PPMPItemDetailsVM>();

        //    int temp = 0;
        //    var itemsList = db.ProjectPlanItems
        //                    .Where(d => d.FKItemReference.ResponsibilityCenter == deptCode && d.FKItemReference.FKItemTypeReference.FKInventoryTypeReference.InventoryTypeName == InventoryType && d.FKItemReference.FKItemTypeReference.FKInventoryTypeReference.IsTangible == true)
        //                    .Select(d => new
        //                    {
        //                        ItemCode = d.FKItemReference.ItemCode,
        //                        ItemName = d.FKItemReference.ItemFullName,
        //                        ItemSpecifications = d.FKItemReference.ItemSpecifications,
        //                        ProcurementSource = d.FKItemReference.ProcurementSource,
        //                        ItemImage = d.FKItemReference.ItemImage,
        //                        IndividualUOMReference = d.FKItemReference.FKIndividualUnitReference.Abbreviation,
        //                        Category = d.FKItemReference.FKCategoryReference.ItemCategoryName,
        //                        JanMilestone = d.PPMPJan,
        //                        FebMilestone = d.PPMPFeb,
        //                        MarMilestone = d.PPMPMar,
        //                        AprMilestone = d.PPMPApr,
        //                        MayMilestone = d.PPMPMay,
        //                        JunMilestone = d.PPMPJun,
        //                        JulMilestone = d.PPMPJul,
        //                        AugMilestone = d.PPMPAug,
        //                        SepMilestone = d.PPMPSep,
        //                        OctMilestone = d.PPMPOct,
        //                        NovMilestone = d.PPMPNov,
        //                        DecMilestone = d.PPMPDec,
        //                        TotalQty = d.ProjectTotalQty,
        //                        UnitCost = d.UnitCost,
        //                        EstimatedBudget = d.ProjectEstimatedBudget
        //                    })
        //                    .ToList()
        //                    .GroupBy(d => new { d.ItemCode, d.ItemName, d.ItemSpecifications, d.ProcurementSource, d.IndividualUOMReference, d.Category, d.UnitCost })
        //                    .Select(d => new
        //                    {
        //                        ItemCode = d.Key.ItemCode,
        //                        ItemName = d.Key.ItemName,
        //                        ItemSpecifications = d.Key.ItemSpecifications,
        //                        ProcurementSource = d.Key.ProcurementSource,
        //                        IndividualUOMReference = d.Key.IndividualUOMReference,
        //                        Category = d.Key.Category,
        //                        JanMilestone = d.Sum(x => int.TryParse(x.JanMilestone, out temp) ? temp : 0).ToString(),
        //                        FebMilestone = d.Sum(x => int.TryParse(x.FebMilestone, out temp) ? temp : 0).ToString(),
        //                        MarMilestone = d.Sum(x => int.TryParse(x.MarMilestone, out temp) ? temp : 0).ToString(),
        //                        AprMilestone = d.Sum(x => int.TryParse(x.AprMilestone, out temp) ? temp : 0).ToString(),
        //                        MayMilestone = d.Sum(x => int.TryParse(x.MayMilestone, out temp) ? temp : 0).ToString(),
        //                        JunMilestone = d.Sum(x => int.TryParse(x.JunMilestone, out temp) ? temp : 0).ToString(),
        //                        JulMilestone = d.Sum(x => int.TryParse(x.JulMilestone, out temp) ? temp : 0).ToString(),
        //                        AugMilestone = d.Sum(x => int.TryParse(x.AugMilestone, out temp) ? temp : 0).ToString(),
        //                        SepMilestone = d.Sum(x => int.TryParse(x.SepMilestone, out temp) ? temp : 0).ToString(),
        //                        OctMilestone = d.Sum(x => int.TryParse(x.OctMilestone, out temp) ? temp : 0).ToString(),
        //                        NovMilestone = d.Sum(x => int.TryParse(x.NovMilestone, out temp) ? temp : 0).ToString(),
        //                        DecMilestone = d.Sum(x => int.TryParse(x.DecMilestone, out temp) ? temp : 0).ToString(),
        //                        TotalQty = String.Format("{0:#,##0}", d.Sum(x => x.TotalQty)),
        //                        UnitCost = String.Format("{0:#,##0.00}", d.Key.UnitCost),
        //                        EstimatedBudget = d.Sum(x => x.EstimatedBudget)
        //                    })
        //                    .ToList();
        //    if (itemsList != null)
        //    {
        //        foreach (var item in itemsList)
        //        {
        //            var itemsOffices = db.ProjectPlanItems
        //           .Where(d => d.FKItemReference.ResponsibilityCenter == deptCode && d.FKItemReference.ItemFullName == item.ItemName)
        //           .GroupBy(d => d.FKPPMPReference.ReferenceNo)
        //           .Select(d => d.Key)
        //           .ToList();
        //            var itemsRemarks = "References: \n";
        //            for (int i = 0; i < itemsOffices.Count; i++)
        //            {
        //                if (i == itemsOffices.Count - 1)
        //                {
        //                    itemsRemarks += itemsOffices[i];
        //                }
        //                else
        //                {
        //                    itemsRemarks += itemsOffices[i] + ",\n";
        //                }
        //            }

        //            responsibilityCenterItems.Add(new PPMPItemDetailsVM
        //            {
        //                ItemCode = item.ItemCode,
        //                ItemName = item.ItemName,
        //                ItemSpecifications = item.ItemSpecifications,
        //                ProcurementSource = item.ProcurementSource,
        //                IndividualUOMReference = item.IndividualUOMReference,
        //                Category = item.Category,
        //                JanMilestone = item.JanMilestone,
        //                FebMilestone = item.FebMilestone,
        //                MarMilestone = item.MarMilestone,
        //                AprMilestone = item.AprMilestone,
        //                MayMilestone = item.MayMilestone,
        //                JunMilestone = item.JunMilestone,
        //                JulMilestone = item.JulMilestone,
        //                AugMilestone = item.AugMilestone,
        //                SepMilestone = item.SepMilestone,
        //                OctMilestone = item.OctMilestone,
        //                NovMilestone = item.NovMilestone,
        //                DecMilestone = item.DecMilestone,
        //                TotalQty = item.TotalQty,
        //                UnitCost = item.UnitCost,
        //                EstimatedBudget = item.EstimatedBudget,
        //                Remarks = itemsRemarks
        //            });
        //        }
        //    }

        //    temp = 0;
        //    var serviceList = db.ProjectPlanServices
        //                    .Where(d => d.FKItemReference.ResponsibilityCenter == deptCode && d.FKItemReference.FKItemTypeReference.FKInventoryTypeReference.InventoryTypeName == InventoryType && d.FKItemReference.FKItemTypeReference.FKInventoryTypeReference.IsTangible == false)
        //                    .Select(d => new
        //                    {
        //                        ItemCode = d.FKItemReference.ItemCode,
        //                        ItemName = d.FKItemReference.ItemFullName,
        //                        ItemSpecifications = d.FKItemReference.ItemSpecifications,
        //                        ProcurementSource = d.FKItemReference.ProcurementSource,
        //                        ItemImage = d.FKItemReference.ItemImage,
        //                        IndividualUOMReference = d.FKItemReference.FKIndividualUnitReference.Abbreviation,
        //                        Category = d.FKItemReference.FKCategoryReference.ItemCategoryName,
        //                        JanMilestone = d.PPMPJan,
        //                        FebMilestone = d.PPMPFeb,
        //                        MarMilestone = d.PPMPMar,
        //                        AprMilestone = d.PPMPApr,
        //                        MayMilestone = d.PPMPMay,
        //                        JunMilestone = d.PPMPJun,
        //                        JulMilestone = d.PPMPJul,
        //                        AugMilestone = d.PPMPAug,
        //                        SepMilestone = d.PPMPSep,
        //                        OctMilestone = d.PPMPOct,
        //                        NovMilestone = d.PPMPNov,
        //                        DecMilestone = d.PPMPDec,
        //                        TotalQty = d.PPMPQuantity,
        //                        UnitCost = d.UnitCost,
        //                        EstimatedBudget = d.ProjectEstimatedBudget
        //                    })
        //                    .ToList()
        //                    .GroupBy(d => new { d.ItemCode, d.ItemName, d.ItemSpecifications, d.ProcurementSource, d.IndividualUOMReference, d.Category, d.UnitCost })
        //                    .Select(d => new
        //                    {
        //                        ItemCode = d.Key.ItemCode,
        //                        ItemName = d.Key.ItemName,
        //                        ItemSpecifications = d.Key.ItemSpecifications,
        //                        ProcurementSource = d.Key.ProcurementSource,
        //                        IndividualUOMReference = d.Key.IndividualUOMReference,
        //                        Category = d.Key.Category,
        //                        JanMilestone = d.Sum(x => int.TryParse(x.JanMilestone, out temp) ? temp : 0).ToString(),
        //                        FebMilestone = d.Sum(x => int.TryParse(x.FebMilestone, out temp) ? temp : 0).ToString(),
        //                        MarMilestone = d.Sum(x => int.TryParse(x.MarMilestone, out temp) ? temp : 0).ToString(),
        //                        AprMilestone = d.Sum(x => int.TryParse(x.AprMilestone, out temp) ? temp : 0).ToString(),
        //                        MayMilestone = d.Sum(x => int.TryParse(x.MayMilestone, out temp) ? temp : 0).ToString(),
        //                        JunMilestone = d.Sum(x => int.TryParse(x.JunMilestone, out temp) ? temp : 0).ToString(),
        //                        JulMilestone = d.Sum(x => int.TryParse(x.JulMilestone, out temp) ? temp : 0).ToString(),
        //                        AugMilestone = d.Sum(x => int.TryParse(x.AugMilestone, out temp) ? temp : 0).ToString(),
        //                        SepMilestone = d.Sum(x => int.TryParse(x.SepMilestone, out temp) ? temp : 0).ToString(),
        //                        OctMilestone = d.Sum(x => int.TryParse(x.OctMilestone, out temp) ? temp : 0).ToString(),
        //                        NovMilestone = d.Sum(x => int.TryParse(x.NovMilestone, out temp) ? temp : 0).ToString(),
        //                        DecMilestone = d.Sum(x => int.TryParse(x.DecMilestone, out temp) ? temp : 0).ToString(),
        //                        TotalQty = String.Format("{0:#,##0}", d.Sum(x => x.TotalQty)),
        //                        UnitCost = String.Format("{0:#,##0.00}", d.Key.UnitCost),
        //                        EstimatedBudget = d.Sum(x => x.EstimatedBudget)
        //                    })
        //                    .ToList();
        //    if (serviceList != null)
        //    {
        //        foreach (var item in serviceList)
        //        {
        //            var serviceOffices = db.ProjectPlanItems
        //           .Where(d => d.FKItemReference.ResponsibilityCenter == deptCode && d.FKItemReference.ItemFullName == item.ItemName)
        //           .GroupBy(d => d.FKPPMPReference.ReferenceNo)
        //           .Select(d => d.Key)
        //           .ToList();
        //            var itemsRemarks = "References: \n";
        //            for (int i = 0; i < serviceOffices.Count; i++)
        //            {
        //                if (i == serviceOffices.Count - 1)
        //                {
        //                    itemsRemarks += serviceOffices[i];
        //                }
        //                else
        //                {
        //                    itemsRemarks += serviceOffices[i] + ",\n";
        //                }
        //            }

        //            responsibilityCenterItems.Add(new PPMPItemDetailsVM
        //            {
        //                ItemCode = item.ItemCode,
        //                ItemName = item.ItemName,
        //                ItemSpecifications = item.ItemSpecifications,
        //                ProcurementSource = item.ProcurementSource,
        //                IndividualUOMReference = item.IndividualUOMReference,
        //                Category = item.Category,
        //                JanMilestone = item.JanMilestone,
        //                FebMilestone = item.FebMilestone,
        //                MarMilestone = item.MarMilestone,
        //                AprMilestone = item.AprMilestone,
        //                MayMilestone = item.MayMilestone,
        //                JunMilestone = item.JunMilestone,
        //                JulMilestone = item.JulMilestone,
        //                AugMilestone = item.AugMilestone,
        //                SepMilestone = item.SepMilestone,
        //                OctMilestone = item.OctMilestone,
        //                NovMilestone = item.NovMilestone,
        //                DecMilestone = item.DecMilestone,
        //                TotalQty = item.TotalQty,
        //                UnitCost = item.UnitCost,
        //                EstimatedBudget = item.EstimatedBudget,
        //                Remarks = itemsRemarks
        //            });
        //        }
        //    }

        //    return responsibilityCenterItems;
        //}
        //public BudgetPropsalVM GetBudgetProposalDetails(string UserEmail, int FiscalYear)
        //{
        //    BudgetPropsalVM budgetProposal = new BudgetPropsalVM();
        //    var user = db.UserAccounts.Where(d => d.Email == UserEmail).FirstOrDefault();
        //    var office = hrisDataAccess.GetFullDepartmentDetails(user.DepartmentCode);
        //    if (db.PPMPHeader.Where(d => d.Department == office.DepartmentCode).Count() == 0)
        //    {
        //        return null;
        //    }
        //    var submittedAt = db.PPMPHeader.Where(d => d.FiscalYear == FiscalYear && d.Department == office.DepartmentCode).Any() ? null : db.PPMPHeader.Where(d => d.FiscalYear == FiscalYear && d.Department == office.DepartmentCode).GroupBy(d => d.SubmittedAt).Max(d => d.Key).Value.ToString("dd MMMM yyyy") == null ? null : db.PPMPHeader.Where(d => d.FiscalYear == FiscalYear && d.Department == office.DepartmentCode).GroupBy(d => d.SubmittedAt).Max(d => d.Key).Value.ToString("dd MMMM yyyy");
        //    budgetProposal.FiscalYear = FiscalYear;
        //    budgetProposal.OfficeCode = office.DepartmentCode;
        //    budgetProposal.OfficeName = office.Department;
        //    budgetProposal.SubmittedBy = office.DepartmentHead;
        //    budgetProposal.Designation = office.DepartmentHeadDesignation;
        //    budgetProposal.PPMPList = GetPPMPList(UserEmail, FiscalYear);
        //    budgetProposal.MOOE = GetMOOE(UserEmail, FiscalYear);
        //    budgetProposal.CaptialOutlay = GetCapitalOutlay(UserEmail, FiscalYear);
        //    budgetProposal.TotalProposedBudget = budgetProposal.MOOE.Sum(d => d.TotalProposedProgram) + budgetProposal.CaptialOutlay.Sum(d => d.TotalProposedProgram);
        //    return budgetProposal;
        //}
        //public ProjectPlanItems AssignPPMPItemMilestones(ProjectPlanItems Item)
        //{
        //    var StartMonth = db.ProjectPlans.Where(d => d.ID == Item.FKProjectReference.ID).FirstOrDefault().DeliveryMonth;
        //    var FiscalYear = db.ProjectPlans.Where(d => d.ID == Item.FKProjectReference.ID).FirstOrDefault().FiscalYear - 1;
        //    switch (StartMonth)
        //    {
        //        case 1:
        //            {
        //                Item.PPMPJan = "PR Preparation/Pre-Bid Conference - (Oct, " + FiscalYear + "); Procurement Activities - (Nov-Dec, " + FiscalYear + "); Delivery/Preparation of RIS";
        //                Item.PPMPFeb = null;
        //                Item.PPMPMar = null;
        //                Item.PPMPApr = null;
        //                Item.PPMPMay = null;
        //                Item.PPMPJun = null;
        //                Item.PPMPJul = null;
        //                Item.PPMPAug = null;
        //                Item.PPMPSep = null;
        //                Item.PPMPOct = null;
        //                Item.PPMPNov = null;
        //                Item.PPMPDec = null;
        //            }
        //            break;
        //        case 2:
        //            {
        //                Item.PPMPJan = "PR Preparation/Pre-Bid Conference - (Nov, " + FiscalYear + "); Procurement Activities - (Dec, " + FiscalYear + " - Jan, " + (FiscalYear + 1) + ");";
        //                Item.PPMPFeb = "Delivery/Preparation of RIS";
        //                Item.PPMPMar = null;
        //                Item.PPMPApr = null;
        //                Item.PPMPMay = null;
        //                Item.PPMPJun = null;
        //                Item.PPMPJul = null;
        //                Item.PPMPAug = null;
        //                Item.PPMPSep = null;
        //                Item.PPMPOct = null;
        //                Item.PPMPNov = null;
        //                Item.PPMPDec = null;
        //            }
        //            break;
        //        case 3:
        //            {
        //                Item.PPMPJan = "PR Preparation/Pre-Bid Conference - (Dec, " + FiscalYear + "); Procurement Activities";
        //                Item.PPMPFeb = "Procurement Activities";
        //                Item.PPMPMar = "Delivery/Preparation of RIS";
        //                Item.PPMPApr = null;
        //                Item.PPMPMay = null;
        //                Item.PPMPJun = null;
        //                Item.PPMPJul = null;
        //                Item.PPMPAug = null;
        //                Item.PPMPSep = null;
        //                Item.PPMPOct = null;
        //                Item.PPMPNov = null;
        //                Item.PPMPDec = null;
        //            }
        //            break;
        //        case 4:
        //            {
        //                Item.PPMPJan = "PR Preparation/Pre-Bid Conference";
        //                Item.PPMPFeb = "Procurement Activities";
        //                Item.PPMPMar = "Procurement Activities";
        //                Item.PPMPApr = "Delivery/Preparation of RIS";
        //                Item.PPMPMay = null;
        //                Item.PPMPJun = null;
        //                Item.PPMPJul = null;
        //                Item.PPMPAug = null;
        //                Item.PPMPSep = null;
        //                Item.PPMPOct = null;
        //                Item.PPMPNov = null;
        //                Item.PPMPDec = null;
        //            }
        //            break;
        //        case 5:
        //            {
        //                Item.PPMPJan = null;
        //                Item.PPMPFeb = "PR Preparation/Pre-Bid Conference";
        //                Item.PPMPMar = "Procurement Activities";
        //                Item.PPMPApr = "Procurement Activities";
        //                Item.PPMPMay = "Delivery/Preparation of RIS";
        //                Item.PPMPJun = null;
        //                Item.PPMPJul = null;
        //                Item.PPMPAug = null;
        //                Item.PPMPSep = null;
        //                Item.PPMPOct = null;
        //                Item.PPMPNov = null;
        //                Item.PPMPDec = null;
        //            }
        //            break;
        //        case 6:
        //            {
        //                Item.PPMPJan = null;
        //                Item.PPMPFeb = null;
        //                Item.PPMPMar = "PR Preparation/Pre-Bid Conference";
        //                Item.PPMPApr = "Procurement Activities";
        //                Item.PPMPMay = "Procurement Activities";
        //                Item.PPMPJun = "Delivery/Preparation of RIS";
        //                Item.PPMPJul = null;
        //                Item.PPMPAug = null;
        //                Item.PPMPSep = null;
        //                Item.PPMPOct = null;
        //                Item.PPMPNov = null;
        //                Item.PPMPDec = null;
        //            }
        //            break;
        //        case 7:
        //            {
        //                Item.PPMPJan = null;
        //                Item.PPMPFeb = null;
        //                Item.PPMPMar = null;
        //                Item.PPMPApr = "PR Preparation/Pre-Bid Conference";
        //                Item.PPMPMay = "Procurement Activities";
        //                Item.PPMPJun = "Procurement Activities";
        //                Item.PPMPJul = "Delivery/Preparation of RIS";
        //                Item.PPMPAug = null;
        //                Item.PPMPSep = null;
        //                Item.PPMPOct = null;
        //                Item.PPMPNov = null;
        //                Item.PPMPDec = null;
        //            }
        //            break;
        //        case 8:
        //            {
        //                Item.PPMPJan = null;
        //                Item.PPMPFeb = null;
        //                Item.PPMPMar = null;
        //                Item.PPMPApr = null;
        //                Item.PPMPMay = "PR Preparation/Pre-Bid Conference";
        //                Item.PPMPJun = "Procurement Activities";
        //                Item.PPMPJul = "Procurement Activities";
        //                Item.PPMPAug = "Delivery/Preparation of RIS";
        //                Item.PPMPSep = null;
        //                Item.PPMPOct = null;
        //                Item.PPMPNov = null;
        //                Item.PPMPDec = null;
        //            }
        //            break;
        //        case 9:
        //            {
        //                Item.PPMPJan = null;
        //                Item.PPMPFeb = null;
        //                Item.PPMPMar = null;
        //                Item.PPMPApr = null;
        //                Item.PPMPMay = null;
        //                Item.PPMPJun = "PR Preparation/Pre-Bid Conference";
        //                Item.PPMPJul = "Procurement Activities";
        //                Item.PPMPAug = "Procurement Activities";
        //                Item.PPMPSep = "Delivery/Preparation of RIS";
        //                Item.PPMPOct = null;
        //                Item.PPMPNov = null;
        //                Item.PPMPDec = null;
        //            }
        //            break;
        //        case 10:
        //            {
        //                Item.PPMPJan = null;
        //                Item.PPMPFeb = null;
        //                Item.PPMPMar = null;
        //                Item.PPMPApr = null;
        //                Item.PPMPMay = null;
        //                Item.PPMPJun = null;
        //                Item.PPMPJul = "PR Preparation/Pre-Bid Conference";
        //                Item.PPMPAug = "Procurement Activities";
        //                Item.PPMPSep = "Procurement Activities";
        //                Item.PPMPOct = "Delivery/Preparation of RIS";
        //                Item.PPMPNov = null;
        //                Item.PPMPDec = null;
        //            }
        //            break;
        //        case 11:
        //            {
        //                Item.PPMPJan = null;
        //                Item.PPMPFeb = null;
        //                Item.PPMPMar = null;
        //                Item.PPMPApr = null;
        //                Item.PPMPMay = null;
        //                Item.PPMPJun = null;
        //                Item.PPMPJul = null;
        //                Item.PPMPAug = "PR Preparation/Pre-Bid Conference";
        //                Item.PPMPSep = "Procurement Activities";
        //                Item.PPMPOct = "Procurement Activities";
        //                Item.PPMPNov = "Delivery/Preparation of RIS";
        //                Item.PPMPDec = null;
        //            }
        //            break;
        //        case 12:
        //            {
        //                Item.PPMPJan = null;
        //                Item.PPMPFeb = null;
        //                Item.PPMPMar = null;
        //                Item.PPMPApr = null;
        //                Item.PPMPMay = null;
        //                Item.PPMPJun = null;
        //                Item.PPMPJul = null;
        //                Item.PPMPAug = null;
        //                Item.PPMPSep = "PR Preparation/Pre-Bid Conference";
        //                Item.PPMPOct = "Procurement Activities";
        //                Item.PPMPNov = "Procurement Activities";
        //                Item.PPMPDec = "Delivery/Preparation of RIS";
        //            }
        //            break;
        //    }
        //    return Item;
        //}
        //public ProjectPlanServices AssignPPMPServiceMilestones(ProjectPlanServices Item)
        //{
        //    var StartMonth = db.ProjectPlans.Where(d => d.ID == Item.FKProjectReference.ID).FirstOrDefault().DeliveryMonth;
        //    var FiscalYear = db.ProjectPlans.Where(d => d.ID == Item.FKProjectReference.ID).FirstOrDefault().FiscalYear - 1;
        //    switch (StartMonth)
        //    {
        //        case 1:
        //            {
        //                Item.PPMPJan = "PR Preparation/Pre-Bid Conference - (Oct, " + FiscalYear + "); Procurement Activities - (Nov-Dec, " + FiscalYear + "); Delivery/Preparation of RIS";
        //                Item.PPMPFeb = null;
        //                Item.PPMPMar = null;
        //                Item.PPMPApr = null;
        //                Item.PPMPMay = null;
        //                Item.PPMPJun = null;
        //                Item.PPMPJul = null;
        //                Item.PPMPAug = null;
        //                Item.PPMPSep = null;
        //                Item.PPMPOct = null;
        //                Item.PPMPNov = null;
        //                Item.PPMPDec = null;
        //            }
        //            break;
        //        case 2:
        //            {
        //                Item.PPMPJan = "PR Preparation/Pre-Bid Conference - (Nov, " + FiscalYear + "); Procurement Activities - (Dec, " + FiscalYear + " - Jan, " + (FiscalYear + 1) + ");";
        //                Item.PPMPFeb = "Delivery/Preparation of RIS";
        //                Item.PPMPMar = null;
        //                Item.PPMPApr = null;
        //                Item.PPMPMay = null;
        //                Item.PPMPJun = null;
        //                Item.PPMPJul = null;
        //                Item.PPMPAug = null;
        //                Item.PPMPSep = null;
        //                Item.PPMPOct = null;
        //                Item.PPMPNov = null;
        //                Item.PPMPDec = null;
        //            }
        //            break;
        //        case 3:
        //            {
        //                Item.PPMPJan = "PR Preparation/Pre-Bid Conference - (Dec, " + FiscalYear + "); Procurement Activities";
        //                Item.PPMPFeb = "Procurement Activities";
        //                Item.PPMPMar = "Delivery/Preparation of RIS";
        //                Item.PPMPApr = null;
        //                Item.PPMPMay = null;
        //                Item.PPMPJun = null;
        //                Item.PPMPJul = null;
        //                Item.PPMPAug = null;
        //                Item.PPMPSep = null;
        //                Item.PPMPOct = null;
        //                Item.PPMPNov = null;
        //                Item.PPMPDec = null;
        //            }
        //            break;
        //        case 4:
        //            {
        //                Item.PPMPJan = "PR Preparation/Pre-Bid Conference";
        //                Item.PPMPFeb = "Procurement Activities";
        //                Item.PPMPMar = "Procurement Activities";
        //                Item.PPMPApr = "Delivery/Preparation of RIS";
        //                Item.PPMPMay = null;
        //                Item.PPMPJun = null;
        //                Item.PPMPJul = null;
        //                Item.PPMPAug = null;
        //                Item.PPMPSep = null;
        //                Item.PPMPOct = null;
        //                Item.PPMPNov = null;
        //                Item.PPMPDec = null;
        //            }
        //            break;
        //        case 5:
        //            {
        //                Item.PPMPJan = null;
        //                Item.PPMPFeb = "PR Preparation/Pre-Bid Conference";
        //                Item.PPMPMar = "Procurement Activities";
        //                Item.PPMPApr = "Procurement Activities";
        //                Item.PPMPMay = "Delivery/Preparation of RIS";
        //                Item.PPMPJun = null;
        //                Item.PPMPJul = null;
        //                Item.PPMPAug = null;
        //                Item.PPMPSep = null;
        //                Item.PPMPOct = null;
        //                Item.PPMPNov = null;
        //                Item.PPMPDec = null;
        //            }
        //            break;
        //        case 6:
        //            {
        //                Item.PPMPJan = null;
        //                Item.PPMPFeb = null;
        //                Item.PPMPMar = "PR Preparation/Pre-Bid Conference";
        //                Item.PPMPApr = "Procurement Activities";
        //                Item.PPMPMay = "Procurement Activities";
        //                Item.PPMPJun = "Delivery/Preparation of RIS";
        //                Item.PPMPJul = null;
        //                Item.PPMPAug = null;
        //                Item.PPMPSep = null;
        //                Item.PPMPOct = null;
        //                Item.PPMPNov = null;
        //                Item.PPMPDec = null;
        //            }
        //            break;
        //        case 7:
        //            {
        //                Item.PPMPJan = null;
        //                Item.PPMPFeb = null;
        //                Item.PPMPMar = null;
        //                Item.PPMPApr = "PR Preparation/Pre-Bid Conference";
        //                Item.PPMPMay = "Procurement Activities";
        //                Item.PPMPJun = "Procurement Activities";
        //                Item.PPMPJul = "Delivery/Preparation of RIS";
        //                Item.PPMPAug = null;
        //                Item.PPMPSep = null;
        //                Item.PPMPOct = null;
        //                Item.PPMPNov = null;
        //                Item.PPMPDec = null;
        //            }
        //            break;
        //        case 8:
        //            {
        //                Item.PPMPJan = null;
        //                Item.PPMPFeb = null;
        //                Item.PPMPMar = null;
        //                Item.PPMPApr = null;
        //                Item.PPMPMay = "PR Preparation/Pre-Bid Conference";
        //                Item.PPMPJun = "Procurement Activities";
        //                Item.PPMPJul = "Procurement Activities";
        //                Item.PPMPAug = "Delivery/Preparation of RIS";
        //                Item.PPMPSep = null;
        //                Item.PPMPOct = null;
        //                Item.PPMPNov = null;
        //                Item.PPMPDec = null;
        //            }
        //            break;
        //        case 9:
        //            {
        //                Item.PPMPJan = null;
        //                Item.PPMPFeb = null;
        //                Item.PPMPMar = null;
        //                Item.PPMPApr = null;
        //                Item.PPMPMay = null;
        //                Item.PPMPJun = "PR Preparation/Pre-Bid Conference";
        //                Item.PPMPJul = "Procurement Activities";
        //                Item.PPMPAug = "Procurement Activities";
        //                Item.PPMPSep = "Delivery/Preparation of RIS";
        //                Item.PPMPOct = null;
        //                Item.PPMPNov = null;
        //                Item.PPMPDec = null;
        //            }
        //            break;
        //        case 10:
        //            {
        //                Item.PPMPJan = null;
        //                Item.PPMPFeb = null;
        //                Item.PPMPMar = null;
        //                Item.PPMPApr = null;
        //                Item.PPMPMay = null;
        //                Item.PPMPJun = null;
        //                Item.PPMPJul = "PR Preparation/Pre-Bid Conference";
        //                Item.PPMPAug = "Procurement Activities";
        //                Item.PPMPSep = "Procurement Activities";
        //                Item.PPMPOct = "Delivery/Preparation of RIS";
        //                Item.PPMPNov = null;
        //                Item.PPMPDec = null;
        //            }
        //            break;
        //        case 11:
        //            {
        //                Item.PPMPJan = null;
        //                Item.PPMPFeb = null;
        //                Item.PPMPMar = null;
        //                Item.PPMPApr = null;
        //                Item.PPMPMay = null;
        //                Item.PPMPJun = null;
        //                Item.PPMPJul = null;
        //                Item.PPMPAug = "PR Preparation/Pre-Bid Conference";
        //                Item.PPMPSep = "Procurement Activities";
        //                Item.PPMPOct = "Procurement Activities";
        //                Item.PPMPNov = "Delivery/Preparation of RIS";
        //                Item.PPMPDec = null;
        //            }
        //            break;
        //        case 12:
        //            {
        //                Item.PPMPJan = null;
        //                Item.PPMPFeb = null;
        //                Item.PPMPMar = null;
        //                Item.PPMPApr = null;
        //                Item.PPMPMay = null;
        //                Item.PPMPJun = null;
        //                Item.PPMPJul = null;
        //                Item.PPMPAug = null;
        //                Item.PPMPSep = "PR Preparation/Pre-Bid Conference";
        //                Item.PPMPOct = "Procurement Activities";
        //                Item.PPMPNov = "Procurement Activities";
        //                Item.PPMPDec = "Delivery/Preparation of RIS";
        //            }
        //            break;
        //    }
        //    return Item;
        //}
        //public bool SubmitPPMP(string ReferenceNo, string UserEmail)
        //{
        //    var ppmp = db.PPMPHeader.Where(d => d.ReferenceNo == ReferenceNo).FirstOrDefault();
        //    var user = db.UserAccounts.Where(d => d.Email == UserEmail).FirstOrDefault();
        //    var office = hrisDataAccess.GetFullDepartmentDetails(user.DepartmentCode);
        //    ppmp.Status = "PPMP Submitted";
        //    ppmp.SubmittedAt = DateTime.Now;
        //    ppmp.SubmittedBy = office.DepartmentHead + ", " + office.DepartmentHeadDesignation;

        //    var switchBoard = db.SwitchBoard.Where(d => d.Reference == ReferenceNo).FirstOrDefault();
        //    var employee = hrisDataAccess.GetEmployee(UserEmail);
        //    var newProjectSwitchBody = new SwitchBoardBody
        //    {
        //        SwitchBoardReference = switchBoard.ID,
        //        ActionBy = employee.EmployeeCode,
        //        Remarks = ReferenceNo + " has been submitted by " + employee.EmployeeName + ", " + employee.Designation + ".  (" + DateTime.Now.ToString("dd MMMM yyyy hh:mm tt") + ")",
        //        DepartmentCode = employee.DepartmentCode,
        //        UpdatedAt = DateTime.Now
        //    };
        //    db.SwitchBoardBody.Add(newProjectSwitchBody);
        //    if (db.SaveChanges() == 0)
        //    {
        //        return false;
        //    }

        //    return true;
        //}
        //public bool PostToPPMP(ProjectPlanVM Project, string UserEmail)
        //{
        //    var currentUser = db.UserAccounts.Where(d => d.Email == UserEmail).FirstOrDefault();
        //    var employee = hrisDataAccess.GetEmployee(currentUser.Email);
        //    var office = hrisDataAccess.GetFullDepartmentDetails(Project.UnitCode == null ? Project.DepartmentCode : Project.UnitCode);
        //    var projectItemInventoryTypes = db.ProjectPlanItems.Where(d => d.FKProjectReference.ProjectCode == Project.ProjectCode)
        //                                    .GroupBy(d => d.FKItemReference.FKItemTypeReference.FKInventoryTypeReference.ID)
        //                                    .Select(d => d.Key)
        //                                    .ToList();
        //    var projectServiceTypes = db.ProjectPlanServices.Where(d => d.FKProjectReference.ProjectCode == Project.ProjectCode)
        //                                    .GroupBy(d => d.FKItemReference.FKItemTypeReference.FKInventoryTypeReference.ID)
        //                                    .Select(d => d.Key)
        //                                    .ToList();
        //    var PPMPHeaderRefNo = string.Empty;
        //    SwitchBoard newProjectSwitch = new SwitchBoard();
        //    SwitchBoardBody PPMPSwitchBody = new SwitchBoardBody();

        //    projectItemInventoryTypes.AddRange(projectServiceTypes);

        //    foreach (var inventoryID in projectItemInventoryTypes)
        //    {
        //        var inventoryType = db.InventoryTypes.Find(inventoryID);
        //        if (inventoryType.IsTangible == true)
        //        {
        //            var projectItems = db.ProjectPlanItems.Where(d => d.FKItemReference.FKItemTypeReference.InventoryTypeReference == inventoryID && d.FKProjectReference.ProjectCode == Project.ProjectCode).ToList();
        //            if (projectItems != null)
        //            {
        //                var PPMPHeader = db.PPMPHeader.Where(d => d.PPMPType == inventoryID && d.FiscalYear == Project.FiscalYear && d.Department == office.DepartmentCode && d.Status == "New PPMP").FirstOrDefault();
        //                if (PPMPHeader == null)
        //                {
        //                    PPMPHeader = new PPMPHeader()
        //                    {
        //                        ReferenceNo = GenerateReferenceNo(Project.FiscalYear, office.DepartmentCode, inventoryType.InventoryCode),
        //                        FiscalYear = Project.FiscalYear,
        //                        Sector = office.SectorCode,
        //                        Department = office.DepartmentCode,
        //                        Unit = office.SectionCode,
        //                        PPMPType = inventoryID,
        //                        PreparedBy = Project.PreparedByEmpCode,
        //                        SubmittedBy = office.DepartmentHead.ToUpper() + ", " + office.DepartmentHeadDesignation,
        //                        CreatedAt = DateTime.Now,
        //                        Status = "New PPMP"
        //                    };

        //                    db.PPMPHeader.Add(PPMPHeader);
        //                    if (db.SaveChanges() == 0)
        //                    {
        //                        return false;
        //                    }

        //                    newProjectSwitch = new SwitchBoard
        //                    {
        //                        DepartmentCode = PPMPHeader.Department,
        //                        MessageType = "PPMP",
        //                        Reference = PPMPHeader.ReferenceNo,
        //                        Subject = PPMPHeader.ReferenceNo + " - " + PPMPHeader.FKPPMPTypeReference.InventoryTypeName
        //                    };

        //                    db.SwitchBoard.Add(newProjectSwitch);
        //                    if (db.SaveChanges() == 0)
        //                    {
        //                        return false;
        //                    }

        //                    PPMPSwitchBody = new SwitchBoardBody
        //                    {
        //                        SwitchBoardReference = newProjectSwitch.ID,
        //                        ActionBy = employee.EmployeeCode,
        //                        Remarks = PPMPHeader.ReferenceNo + " - " + PPMPHeader.FKPPMPTypeReference.InventoryTypeName + " has been created by " + employee.EmployeeName + ", " + employee.Designation + ". (" + DateTime.Now.ToString("dd MMMM yyyy hh:mm tt") + ")",
        //                        DepartmentCode = employee.DepartmentCode,
        //                        UpdatedAt = DateTime.Now
        //                    };

        //                    db.SwitchBoardBody.Add(PPMPSwitchBody);
        //                    if (db.SaveChanges() == 0)
        //                    {
        //                        return false;
        //                    }
        //                }

        //                PPMPHeaderRefNo = PPMPHeader.ReferenceNo;
        //                newProjectSwitch = new SwitchBoard
        //                {
        //                    DepartmentCode = PPMPHeader.Department,
        //                    MessageType = "PPMP",
        //                    Reference = PPMPHeader.ReferenceNo,
        //                    Subject = PPMPHeader.ReferenceNo + " - " + PPMPHeader.FKPPMPTypeReference.InventoryTypeName
        //                };

        //                db.SwitchBoard.Add(newProjectSwitch);
        //                if (db.SaveChanges() == 0)
        //                {
        //                    return false;
        //                }

        //                PPMPSwitchBody = new SwitchBoardBody
        //                {
        //                    SwitchBoardReference = newProjectSwitch.ID,
        //                    ActionBy = employee.EmployeeCode,
        //                    Remarks = PPMPHeader.ReferenceNo + " - " + PPMPHeader.FKPPMPTypeReference.InventoryTypeName + " has been created by " + employee.EmployeeName + ", " + employee.Designation + ". (" + DateTime.Now.ToString("dd MMMM yyyy hh:mm tt") + ")",
        //                    DepartmentCode = employee.DepartmentCode,
        //                    UpdatedAt = DateTime.Now
        //                };

        //                db.SwitchBoardBody.Add(PPMPSwitchBody);
        //                if (db.SaveChanges() == 0)
        //                {
        //                    return false;
        //                }

        //                foreach (var item in projectItems)
        //                {
        //                    var projectItem = db.ProjectPlanItems.Where(d => d.ItemReference == item.ItemReference && d.FKProjectReference.ProjectCode == item.FKProjectReference.ProjectCode).FirstOrDefault();
        //                    if (projectItem.FKProjectReference.ProjectCode.StartsWith("CSPR"))
        //                    {
        //                        projectItem.PPMPJan = item.ProjectJanQty == null ? null : item.ProjectJanQty.ToString();
        //                        projectItem.PPMPFeb = item.ProjectFebQty == null ? null : item.ProjectFebQty.ToString();
        //                        projectItem.PPMPMar = item.ProjectMarQty == null ? null : item.ProjectMarQty.ToString();
        //                        projectItem.PPMPApr = item.ProjectAprQty == null ? null : item.ProjectAprQty.ToString();
        //                        projectItem.PPMPMay = item.ProjectMayQty == null ? null : item.ProjectMayQty.ToString();
        //                        projectItem.PPMPJun = item.ProjectJunQty == null ? null : item.ProjectJunQty.ToString();
        //                        projectItem.PPMPJul = item.ProjectJulQty == null ? null : item.ProjectJulQty.ToString();
        //                        projectItem.PPMPAug = item.ProjectAugQty == null ? null : item.ProjectAugQty.ToString();
        //                        projectItem.PPMPSep = item.ProjectSepQty == null ? null : item.ProjectSepQty.ToString();
        //                        projectItem.PPMPOct = item.ProjectOctQty == null ? null : item.ProjectOctQty.ToString();
        //                        projectItem.PPMPNov = item.ProjectNovQty == null ? null : item.ProjectNovQty.ToString();
        //                        projectItem.PPMPDec = item.ProjectDecQty == null ? null : item.ProjectDecQty.ToString();
        //                    }
        //                    else
        //                    {
        //                        projectItem = AssignPPMPItemMilestones(projectItem);
        //                    }
        //                    projectItem.PPMPReference = PPMPHeader.ID;
        //                    projectItem.PPMPTotalQty = item.ProjectTotalQty;
        //                    projectItem.PPMPEstimatedBudget = item.ProjectEstimatedBudget;
        //                    projectItem.Status = "Posted";
        //                    if (db.SaveChanges() == 0)
        //                    {
        //                        return false;
        //                    }
        //                }
        //            }
        //        }
        //        else
        //        {
        //            var projectServices = db.ProjectPlanServices.Where(d => d.FKItemReference.FKItemTypeReference.FKInventoryTypeReference.ID == inventoryID && d.FKProjectReference.ProjectCode == Project.ProjectCode).ToList();
        //            if (projectServices != null)
        //            {
        //                var PPMPHeader = db.PPMPHeader.Where(d => d.PPMPType == inventoryID && d.FiscalYear == Project.FiscalYear && d.Department == office.DepartmentCode && d.Status == "New PPMP").FirstOrDefault();
        //                if (PPMPHeader == null)
        //                {
        //                    PPMPHeader = new PPMPHeader()
        //                    {
        //                        ReferenceNo = GenerateReferenceNo(Project.FiscalYear, office.DepartmentCode, inventoryType.InventoryCode),
        //                        FiscalYear = Project.FiscalYear,
        //                        Sector = office.SectorCode,
        //                        Department = office.DepartmentCode,
        //                        Unit = office.SectionCode,
        //                        PPMPType = inventoryID,
        //                        PreparedBy = Project.PreparedByEmpCode,
        //                        SubmittedBy = office.DepartmentHead.ToUpper() + ", " + office.DepartmentHeadDesignation,
        //                        CreatedAt = DateTime.Now,
        //                        Status = "New PPMP"
        //                    };

        //                    db.PPMPHeader.Add(PPMPHeader);
        //                    if (db.SaveChanges() == 0)
        //                    {
        //                        return false;
        //                    }

        //                    newProjectSwitch = new SwitchBoard
        //                    {
        //                        DepartmentCode = PPMPHeader.Department,
        //                        MessageType = "PPMP",
        //                        Reference = PPMPHeader.ReferenceNo,
        //                        Subject = PPMPHeader.ReferenceNo + " - " + PPMPHeader.FKPPMPTypeReference.InventoryTypeName
        //                    };

        //                    db.SwitchBoard.Add(newProjectSwitch);
        //                    if (db.SaveChanges() == 0)
        //                    {
        //                        return false;
        //                    }

        //                    PPMPSwitchBody = new SwitchBoardBody
        //                    {
        //                        SwitchBoardReference = newProjectSwitch.ID,
        //                        ActionBy = employee.EmployeeCode,
        //                        Remarks = PPMPHeader.ReferenceNo + " - " + PPMPHeader.FKPPMPTypeReference.InventoryTypeName + " has been created by " + employee.EmployeeName + ", " + employee.Designation + ". (" + DateTime.Now.ToString("dd MMMM yyyy hh:mm tt") + ")",
        //                        DepartmentCode = employee.DepartmentCode,
        //                        UpdatedAt = DateTime.Now
        //                    };

        //                    db.SwitchBoardBody.Add(PPMPSwitchBody);
        //                    if (db.SaveChanges() == 0)
        //                    {
        //                        return false;
        //                    }
        //                }

        //                PPMPHeaderRefNo = PPMPHeader.ReferenceNo;
        //                newProjectSwitch = new SwitchBoard
        //                {
        //                    DepartmentCode = PPMPHeader.Department,
        //                    MessageType = "PPMP",
        //                    Reference = PPMPHeader.ReferenceNo,
        //                    Subject = PPMPHeader.ReferenceNo + " - " + PPMPHeader.FKPPMPTypeReference.InventoryTypeName
        //                };

        //                db.SwitchBoard.Add(newProjectSwitch);
        //                if (db.SaveChanges() == 0)
        //                {
        //                    return false;
        //                }

        //                PPMPSwitchBody = new SwitchBoardBody
        //                {
        //                    SwitchBoardReference = newProjectSwitch.ID,
        //                    ActionBy = employee.EmployeeCode,
        //                    Remarks = PPMPHeader.ReferenceNo + " - " + PPMPHeader.FKPPMPTypeReference.InventoryTypeName + " has been created by " + employee.EmployeeName + ", " + employee.Designation + ". (" + DateTime.Now.ToString("dd MMMM yyyy hh:mm tt") + ")",
        //                    DepartmentCode = employee.DepartmentCode,
        //                    UpdatedAt = DateTime.Now
        //                };

        //                db.SwitchBoardBody.Add(PPMPSwitchBody);
        //                if (db.SaveChanges() == 0)
        //                {
        //                    return false;
        //                }

        //                foreach (var item in projectServices)
        //                {
        //                    var projectService = db.ProjectPlanServices.Where(d => d.ItemReference == item.ItemReference && d.FKProjectReference.ProjectCode == item.FKProjectReference.ProjectCode).FirstOrDefault();
        //                    projectService = AssignPPMPServiceMilestones(projectService);
        //                    projectService.PPMPReference = PPMPHeader.ID;
        //                    projectService.PPMPQuantity = item.ProjectQuantity;
        //                    projectService.PPMPEstimatedBudget = item.ProjectEstimatedBudget;
        //                    projectService.Status = "Posted";
        //                    if (db.SaveChanges() == 0)
        //                    {
        //                        return false;
        //                    }
        //                }
        //            }
        //        }

        //        var switchBoard = db.SwitchBoard.Where(d => d.Reference == Project.ProjectCode).FirstOrDefault();
        //        var newProjectSwitchBody = new SwitchBoardBody
        //        {
        //            SwitchBoardReference = switchBoard.ID,
        //            ActionBy = employee.EmployeeCode,
        //            Remarks = Project.ProjectCode + " " + inventoryType.InventoryTypeName + " items were posted in " + PPMPHeaderRefNo + " by " + employee.EmployeeName + ", " + employee.Designation + ". (Added Items) (" + DateTime.Now.ToString("dd MMMM yyyy hh:mm tt") + ")",
        //            DepartmentCode = employee.DepartmentCode,
        //            UpdatedAt = DateTime.Now
        //        };
        //        db.SwitchBoardBody.Add(newProjectSwitchBody);

        //        switchBoard = db.SwitchBoard.Where(d => d.Reference == PPMPHeaderRefNo).FirstOrDefault();
        //        newProjectSwitchBody = new SwitchBoardBody
        //        {
        //            SwitchBoardReference = switchBoard.ID,
        //            ActionBy = employee.EmployeeCode,
        //            Remarks = PPMPHeaderRefNo + " has been updated by " + employee.EmployeeName + ", " + employee.Designation + ". (Added Items) (" + DateTime.Now.ToString("dd MMMM yyyy hh:mm tt") + ")",
        //            DepartmentCode = employee.DepartmentCode,
        //            UpdatedAt = DateTime.Now
        //        };
        //        db.SwitchBoardBody.Add(newProjectSwitchBody);
        //        if (db.SaveChanges() == 0)
        //        {
        //            return false;
        //        }

        //    }

        //    return true;
        //}
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