using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using PUPFMIS.Models;
using PUPFMIS.Models.HRIS;
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
            PPMPViewModel ppmpVM = ppmpDAL.GetPPMPDetails(ReferenceNo, UserEmail);

            reportConfig.ReportTitle = ppmpVM.Header.ReferenceNo;
            reportConfig.ReportFormTitle = "Form C";
            reportConfig.ReportReferenceNo = ppmpVM.Header.ReferenceNo;
            reportConfig.LogoPath = LogoPath;
            reportConfig.DocumentSetupLandscape();
            reportConfig.AddHeader("PROJECT PROCUREMENT MANAGEMENT PLAN", new Unit(10, UnitType.Point), true);
            reportConfig.AddHeader("Common Use Office Supplies", new Unit(8, UnitType.Point));
            reportConfig.AddHeader("Fiscal Year " + ppmpVM.Header.FiscalYear, new Unit(8, UnitType.Point));
            reportConfig.AddHeader("\n");

            reportConfig.AddTable();
            reportConfig.AddContentColumn(new Unit(2, UnitType.Centimeter), ParagraphAlignment.Left);
            reportConfig.AddContentColumn(new Unit(18, UnitType.Centimeter), ParagraphAlignment.Left);
            reportConfig.AddContentColumn(new Unit(10.48, UnitType.Centimeter));

            reportConfig.AddContentRow();
            reportConfig.AddContent("End User: ", 0, 10);
            reportConfig.AddContent(ppmpVM.Header.Office, 1, 10, true, ParagraphAlignment.Left, true);
            reportConfig.AddContentRow();
            reportConfig.AddContent("\n", 0);

            if (ppmpVM.DBMItems.Count > 0)
            {
                reportConfig.AddTable(true);
                reportConfig.AddContentColumn(new Unit(2, UnitType.Centimeter), ParagraphAlignment.Center, new Color(252, 207, 101));
                reportConfig.AddContentColumn(new Unit(3, UnitType.Centimeter), ParagraphAlignment.Center, new Color(252, 207, 101));
                reportConfig.AddContentColumn(new Unit(1.2, UnitType.Centimeter), ParagraphAlignment.Center, new Color(252, 207, 101));
                reportConfig.AddContentColumn(new Unit(1.4, UnitType.Centimeter), ParagraphAlignment.Center, new Color(252, 207, 101));
                reportConfig.AddContentColumn(new Unit(1.4, UnitType.Centimeter), ParagraphAlignment.Center, new Color(252, 207, 101));
                reportConfig.AddContentColumn(new Unit(1.4, UnitType.Centimeter), ParagraphAlignment.Center, new Color(252, 207, 101));
                reportConfig.AddContentColumn(new Unit(1.4, UnitType.Centimeter), ParagraphAlignment.Center, new Color(252, 207, 101));
                reportConfig.AddContentColumn(new Unit(1.4, UnitType.Centimeter), ParagraphAlignment.Center, new Color(252, 207, 101));
                reportConfig.AddContentColumn(new Unit(1.4, UnitType.Centimeter), ParagraphAlignment.Center, new Color(252, 207, 101));
                reportConfig.AddContentColumn(new Unit(1.4, UnitType.Centimeter), ParagraphAlignment.Center, new Color(252, 207, 101));
                reportConfig.AddContentColumn(new Unit(1.4, UnitType.Centimeter), ParagraphAlignment.Center, new Color(252, 207, 101));
                reportConfig.AddContentColumn(new Unit(1.4, UnitType.Centimeter), ParagraphAlignment.Center, new Color(252, 207, 101));
                reportConfig.AddContentColumn(new Unit(1.4, UnitType.Centimeter), ParagraphAlignment.Center, new Color(252, 207, 101));
                reportConfig.AddContentColumn(new Unit(1.4, UnitType.Centimeter), ParagraphAlignment.Center, new Color(252, 207, 101));
                reportConfig.AddContentColumn(new Unit(1.4, UnitType.Centimeter), ParagraphAlignment.Center, new Color(252, 207, 101));
                reportConfig.AddContentColumn(new Unit(1.4, UnitType.Centimeter), ParagraphAlignment.Center, new Color(252, 207, 101));
                reportConfig.AddContentColumn(new Unit(1.4, UnitType.Centimeter), ParagraphAlignment.Center, new Color(252, 207, 101));
                reportConfig.AddContentColumn(new Unit(2.15, UnitType.Centimeter), ParagraphAlignment.Center, new Color(252, 207, 101));
                reportConfig.AddContentColumn(new Unit(3.7, UnitType.Centimeter), ParagraphAlignment.Center, new Color(252, 207, 101));

                reportConfig.AddContentRow(new Unit(0.2, UnitType.Inch));
                reportConfig.AddContent("AVAILABLE AT PROCUREMENT SERVICE STORES", 0, new Unit(10, UnitType.Point), true, ParagraphAlignment.Left, VerticalAlignment.Center, 18);

                reportConfig.AddContentRow(new Unit(0.2, UnitType.Inch));
                reportConfig.AddContent("PAP\nCode", 0, new Unit(8, UnitType.Point), true, ParagraphAlignment.Center, VerticalAlignment.Center, null, 1);
                reportConfig.AddContent("Item and Specifications", 1, new Unit(8, UnitType.Point), true, ParagraphAlignment.Center, VerticalAlignment.Center, null, 1);
                reportConfig.AddContent("Unit of Measure", 2, new Unit(7, UnitType.Point), true, ParagraphAlignment.Center, VerticalAlignment.Center, null, 1);
                reportConfig.AddContent("Monthly Quantity Requirement", 3, new Unit(10, UnitType.Point), true, ParagraphAlignment.Center, VerticalAlignment.Center, 11);
                reportConfig.AddContent("Total\nQuantity", 15, new Unit(7, UnitType.Point), true, ParagraphAlignment.Center, VerticalAlignment.Center, null, 1);
                reportConfig.AddContent("Unit\nCost", 16, new Unit(8, UnitType.Point), true, ParagraphAlignment.Center, VerticalAlignment.Center, null, 1);
                reportConfig.AddContent("Estimated\nBudget", 17, new Unit(8, UnitType.Point), true, ParagraphAlignment.Center, VerticalAlignment.Center, null, 1);
                reportConfig.AddContent("Remarks", 18, new Unit(8, UnitType.Point), true, ParagraphAlignment.Center, VerticalAlignment.Center, null, 1);

                reportConfig.AddContentRow(new Unit(0.2, UnitType.Inch));
                reportConfig.AddContent("Jan", 3, new Unit(8, UnitType.Point), true, ParagraphAlignment.Center, VerticalAlignment.Center);
                reportConfig.AddContent("Feb", 4, new Unit(8, UnitType.Point), true, ParagraphAlignment.Center, VerticalAlignment.Center);
                reportConfig.AddContent("Mar", 5, new Unit(8, UnitType.Point), true, ParagraphAlignment.Center, VerticalAlignment.Center);
                reportConfig.AddContent("Apr", 6, new Unit(8, UnitType.Point), true, ParagraphAlignment.Center, VerticalAlignment.Center);
                reportConfig.AddContent("May", 7, new Unit(8, UnitType.Point), true, ParagraphAlignment.Center, VerticalAlignment.Center);
                reportConfig.AddContent("Jun", 8, new Unit(8, UnitType.Point), true, ParagraphAlignment.Center, VerticalAlignment.Center);
                reportConfig.AddContent("Jul", 9, new Unit(8, UnitType.Point), true, ParagraphAlignment.Center, VerticalAlignment.Center);
                reportConfig.AddContent("Aug", 10, new Unit(8, UnitType.Point), true, ParagraphAlignment.Center, VerticalAlignment.Center);
                reportConfig.AddContent("Sep", 11, new Unit(8, UnitType.Point), true, ParagraphAlignment.Center, VerticalAlignment.Center);
                reportConfig.AddContent("Oct", 12, new Unit(8, UnitType.Point), true, ParagraphAlignment.Center, VerticalAlignment.Center);
                reportConfig.AddContent("Nov", 13, new Unit(8, UnitType.Point), true, ParagraphAlignment.Center, VerticalAlignment.Center);
                reportConfig.AddContent("Dec", 14, new Unit(8, UnitType.Point), true, ParagraphAlignment.Center, VerticalAlignment.Center);

                reportConfig.AddTable(true);
                reportConfig.AddContentColumn(new Unit(2, UnitType.Centimeter));
                reportConfig.AddContentColumn(new Unit(3, UnitType.Centimeter));
                reportConfig.AddContentColumn(new Unit(1.2, UnitType.Centimeter));
                reportConfig.AddContentColumn(new Unit(1.4, UnitType.Centimeter));
                reportConfig.AddContentColumn(new Unit(1.4, UnitType.Centimeter));
                reportConfig.AddContentColumn(new Unit(1.4, UnitType.Centimeter));
                reportConfig.AddContentColumn(new Unit(1.4, UnitType.Centimeter));
                reportConfig.AddContentColumn(new Unit(1.4, UnitType.Centimeter));
                reportConfig.AddContentColumn(new Unit(1.4, UnitType.Centimeter));
                reportConfig.AddContentColumn(new Unit(1.4, UnitType.Centimeter));
                reportConfig.AddContentColumn(new Unit(1.4, UnitType.Centimeter));
                reportConfig.AddContentColumn(new Unit(1.4, UnitType.Centimeter));
                reportConfig.AddContentColumn(new Unit(1.4, UnitType.Centimeter));
                reportConfig.AddContentColumn(new Unit(1.4, UnitType.Centimeter));
                reportConfig.AddContentColumn(new Unit(1.4, UnitType.Centimeter));
                reportConfig.AddContentColumn(new Unit(1.4, UnitType.Centimeter));
                reportConfig.AddContentColumn(new Unit(1.4, UnitType.Centimeter));
                reportConfig.AddContentColumn(new Unit(2.15, UnitType.Centimeter));
                reportConfig.AddContentColumn(new Unit(3.7, UnitType.Centimeter));

                foreach (var item in ppmpVM.DBMItems.OrderBy(d => d.ItemName))
                {
                    var itemName = item.ItemCode + " - " + item.ItemName;
                    reportConfig.AddContentRow(new Unit(0.2, UnitType.Inch));
                    reportConfig.AddContent(item.ProjectCode, 0, new Unit(8, UnitType.Point), true, ParagraphAlignment.Left, VerticalAlignment.Center);
                    reportConfig.AddContent(itemName, 1, new Unit(8, UnitType.Point), true, ParagraphAlignment.Left, VerticalAlignment.Center);
                    reportConfig.AddContent(item.IndividualUOMReference, 2, new Unit(8, UnitType.Point), false, ParagraphAlignment.Center, VerticalAlignment.Center);
                    reportConfig.AddContent(String.Format("{0:G}", (item.JanMilestone == null) ? "0" : item.JanMilestone.ToString()), 3, new Unit(8, UnitType.Point), false, ParagraphAlignment.Center, VerticalAlignment.Center);
                    reportConfig.AddContent(String.Format("{0:G}", (item.FebMilestone == null) ? "0" : item.FebMilestone.ToString()), 4, new Unit(8, UnitType.Point), false, ParagraphAlignment.Center, VerticalAlignment.Center);
                    reportConfig.AddContent(String.Format("{0:G}", (item.MarMilestone == null) ? "0" : item.MarMilestone.ToString()), 5, new Unit(8, UnitType.Point), false, ParagraphAlignment.Center, VerticalAlignment.Center);
                    reportConfig.AddContent(String.Format("{0:G}", (item.AprMilestone == null) ? "0" : item.AprMilestone.ToString()), 6, new Unit(8, UnitType.Point), false, ParagraphAlignment.Center, VerticalAlignment.Center);
                    reportConfig.AddContent(String.Format("{0:G}", (item.MayMilestone == null) ? "0" : item.MayMilestone.ToString()), 7, new Unit(8, UnitType.Point), false, ParagraphAlignment.Center, VerticalAlignment.Center);
                    reportConfig.AddContent(String.Format("{0:G}", (item.JunMilestone == null) ? "0" : item.JunMilestone.ToString()), 8, new Unit(8, UnitType.Point), false, ParagraphAlignment.Center, VerticalAlignment.Center);
                    reportConfig.AddContent(String.Format("{0:G}", (item.JulMilestone == null) ? "0" : item.JulMilestone.ToString()), 9, new Unit(8, UnitType.Point), false, ParagraphAlignment.Center, VerticalAlignment.Center);
                    reportConfig.AddContent(String.Format("{0:G}", (item.AugMilestone == null) ? "0" : item.AugMilestone.ToString()), 10, new Unit(8, UnitType.Point), false, ParagraphAlignment.Center, VerticalAlignment.Center);
                    reportConfig.AddContent(String.Format("{0:G}", (item.SepMilestone == null) ? "0" : item.SepMilestone.ToString()), 11, new Unit(8, UnitType.Point), false, ParagraphAlignment.Center, VerticalAlignment.Center);
                    reportConfig.AddContent(String.Format("{0:G}", (item.OctMilestone == null) ? "0" : item.OctMilestone.ToString()), 12, new Unit(8, UnitType.Point), false, ParagraphAlignment.Center, VerticalAlignment.Center);
                    reportConfig.AddContent(String.Format("{0:G}", (item.NovMilestone == null) ? "0" : item.NovMilestone.ToString()), 13, new Unit(8, UnitType.Point), false, ParagraphAlignment.Center, VerticalAlignment.Center);
                    reportConfig.AddContent(String.Format("{0:G}", (item.DecMilestone == null) ? "0" : item.DecMilestone.ToString()), 14, new Unit(8, UnitType.Point), false, ParagraphAlignment.Center, VerticalAlignment.Center);
                    reportConfig.AddContent(String.Format("{0:G}", item.TotalQty), 15, new Unit(8, UnitType.Point), true, ParagraphAlignment.Right, VerticalAlignment.Center);
                    reportConfig.AddContent(String.Format("{0:C}", item.UnitCost), 16, new Unit(8, UnitType.Point), true, ParagraphAlignment.Right, VerticalAlignment.Center);
                    reportConfig.AddContent(String.Format("{0:C}", item.EstimatedBudget), 17, new Unit(8, UnitType.Point), true, ParagraphAlignment.Right, VerticalAlignment.Center);
                    reportConfig.AddContent(item.Remarks + ";\n" + item.Project, 18, new Unit(8, UnitType.Point), false, ParagraphAlignment.Justify, VerticalAlignment.Center);
                }

                reportConfig.AddContentRow(new Unit(0.2, UnitType.Inch));
                reportConfig.AddContent("ESTIMATED BUDGET (DBM-PS):", 0, new Unit(10, UnitType.Point), true, ParagraphAlignment.Right, VerticalAlignment.Center, 15);
                reportConfig.AddContent(String.Format("{0:C}", ppmpVM.DBMItems.Sum(d => d.EstimatedBudget)), 16, new Unit(10, UnitType.Point), true, ParagraphAlignment.Right, VerticalAlignment.Center, 1);
                reportConfig.AddContent("", 10, new Unit(9, UnitType.Point), true, ParagraphAlignment.Left, VerticalAlignment.Center);
            }

            if (ppmpVM.NonDBMItems.Count > 0)
            {
                reportConfig.AddTable(true);
                reportConfig.AddContentColumn(new Unit(2, UnitType.Centimeter), ParagraphAlignment.Center, new Color(252, 207, 101));
                reportConfig.AddContentColumn(new Unit(3, UnitType.Centimeter), ParagraphAlignment.Center, new Color(252, 207, 101));
                reportConfig.AddContentColumn(new Unit(1.2, UnitType.Centimeter), ParagraphAlignment.Center, new Color(252, 207, 101));
                reportConfig.AddContentColumn(new Unit(1.4, UnitType.Centimeter), ParagraphAlignment.Center, new Color(252, 207, 101));
                reportConfig.AddContentColumn(new Unit(1.4, UnitType.Centimeter), ParagraphAlignment.Center, new Color(252, 207, 101));
                reportConfig.AddContentColumn(new Unit(1.4, UnitType.Centimeter), ParagraphAlignment.Center, new Color(252, 207, 101));
                reportConfig.AddContentColumn(new Unit(1.4, UnitType.Centimeter), ParagraphAlignment.Center, new Color(252, 207, 101));
                reportConfig.AddContentColumn(new Unit(1.4, UnitType.Centimeter), ParagraphAlignment.Center, new Color(252, 207, 101));
                reportConfig.AddContentColumn(new Unit(1.4, UnitType.Centimeter), ParagraphAlignment.Center, new Color(252, 207, 101));
                reportConfig.AddContentColumn(new Unit(1.4, UnitType.Centimeter), ParagraphAlignment.Center, new Color(252, 207, 101));
                reportConfig.AddContentColumn(new Unit(1.4, UnitType.Centimeter), ParagraphAlignment.Center, new Color(252, 207, 101));
                reportConfig.AddContentColumn(new Unit(1.4, UnitType.Centimeter), ParagraphAlignment.Center, new Color(252, 207, 101));
                reportConfig.AddContentColumn(new Unit(1.4, UnitType.Centimeter), ParagraphAlignment.Center, new Color(252, 207, 101));
                reportConfig.AddContentColumn(new Unit(1.4, UnitType.Centimeter), ParagraphAlignment.Center, new Color(252, 207, 101));
                reportConfig.AddContentColumn(new Unit(1.4, UnitType.Centimeter), ParagraphAlignment.Center, new Color(252, 207, 101));
                reportConfig.AddContentColumn(new Unit(1.4, UnitType.Centimeter), ParagraphAlignment.Center, new Color(252, 207, 101));
                reportConfig.AddContentColumn(new Unit(1.4, UnitType.Centimeter), ParagraphAlignment.Center, new Color(252, 207, 101));
                reportConfig.AddContentColumn(new Unit(2.15, UnitType.Centimeter), ParagraphAlignment.Center, new Color(252, 207, 101));
                reportConfig.AddContentColumn(new Unit(3.7, UnitType.Centimeter), ParagraphAlignment.Center, new Color(252, 207, 101));

                reportConfig.AddContentRow(new Unit(0.2, UnitType.Inch));
                reportConfig.AddContent("OTHER ITEMS NOT AVALABLE AT PS BUT REGULARLY PURCHASED FROM OTHER SOURCES (Note: Please indicate price of items)", 0, new Unit(10, UnitType.Point), true, ParagraphAlignment.Left, VerticalAlignment.Center, 18);

                reportConfig.AddContentRow(new Unit(0.2, UnitType.Inch));
                reportConfig.AddContent("PAP\nCode", 0, new Unit(8, UnitType.Point), true, ParagraphAlignment.Center, VerticalAlignment.Center, null, 1);
                reportConfig.AddContent("Item and Specifications", 1, new Unit(8, UnitType.Point), true, ParagraphAlignment.Center, VerticalAlignment.Center, null, 1);
                reportConfig.AddContent("Unit of Measure", 2, new Unit(7, UnitType.Point), true, ParagraphAlignment.Center, VerticalAlignment.Center, null, 1);
                reportConfig.AddContent("Monthly Quantity Requirement", 3, new Unit(10, UnitType.Point), true, ParagraphAlignment.Center, VerticalAlignment.Center, 11);
                reportConfig.AddContent("Total\nQuantity", 15, new Unit(7, UnitType.Point), true, ParagraphAlignment.Center, VerticalAlignment.Center, null, 1);
                reportConfig.AddContent("Unit\nCost", 16, new Unit(8, UnitType.Point), true, ParagraphAlignment.Center, VerticalAlignment.Center, null, 1);
                reportConfig.AddContent("Estimated\nBudget", 17, new Unit(8, UnitType.Point), true, ParagraphAlignment.Center, VerticalAlignment.Center, null, 1);
                reportConfig.AddContent("Remarks", 18, new Unit(8, UnitType.Point), true, ParagraphAlignment.Center, VerticalAlignment.Center, null, 1);

                reportConfig.AddContentRow(new Unit(0.2, UnitType.Inch));
                reportConfig.AddContent("Jan", 3, new Unit(8, UnitType.Point), true, ParagraphAlignment.Center, VerticalAlignment.Center);
                reportConfig.AddContent("Feb", 4, new Unit(8, UnitType.Point), true, ParagraphAlignment.Center, VerticalAlignment.Center);
                reportConfig.AddContent("Mar", 5, new Unit(8, UnitType.Point), true, ParagraphAlignment.Center, VerticalAlignment.Center);
                reportConfig.AddContent("Apr", 6, new Unit(8, UnitType.Point), true, ParagraphAlignment.Center, VerticalAlignment.Center);
                reportConfig.AddContent("May", 7, new Unit(8, UnitType.Point), true, ParagraphAlignment.Center, VerticalAlignment.Center);
                reportConfig.AddContent("Jun", 8, new Unit(8, UnitType.Point), true, ParagraphAlignment.Center, VerticalAlignment.Center);
                reportConfig.AddContent("Jul", 9, new Unit(8, UnitType.Point), true, ParagraphAlignment.Center, VerticalAlignment.Center);
                reportConfig.AddContent("Aug", 10, new Unit(8, UnitType.Point), true, ParagraphAlignment.Center, VerticalAlignment.Center);
                reportConfig.AddContent("Sep", 11, new Unit(8, UnitType.Point), true, ParagraphAlignment.Center, VerticalAlignment.Center);
                reportConfig.AddContent("Oct", 12, new Unit(8, UnitType.Point), true, ParagraphAlignment.Center, VerticalAlignment.Center);
                reportConfig.AddContent("Nov", 13, new Unit(8, UnitType.Point), true, ParagraphAlignment.Center, VerticalAlignment.Center);
                reportConfig.AddContent("Dec", 14, new Unit(8, UnitType.Point), true, ParagraphAlignment.Center, VerticalAlignment.Center);

                reportConfig.AddTable(true);
                reportConfig.AddContentColumn(new Unit(2, UnitType.Centimeter));
                reportConfig.AddContentColumn(new Unit(3, UnitType.Centimeter));
                reportConfig.AddContentColumn(new Unit(1.2, UnitType.Centimeter));
                reportConfig.AddContentColumn(new Unit(1.4, UnitType.Centimeter));
                reportConfig.AddContentColumn(new Unit(1.4, UnitType.Centimeter));
                reportConfig.AddContentColumn(new Unit(1.4, UnitType.Centimeter));
                reportConfig.AddContentColumn(new Unit(1.4, UnitType.Centimeter));
                reportConfig.AddContentColumn(new Unit(1.4, UnitType.Centimeter));
                reportConfig.AddContentColumn(new Unit(1.4, UnitType.Centimeter));
                reportConfig.AddContentColumn(new Unit(1.4, UnitType.Centimeter));
                reportConfig.AddContentColumn(new Unit(1.4, UnitType.Centimeter));
                reportConfig.AddContentColumn(new Unit(1.4, UnitType.Centimeter));
                reportConfig.AddContentColumn(new Unit(1.4, UnitType.Centimeter));
                reportConfig.AddContentColumn(new Unit(1.4, UnitType.Centimeter));
                reportConfig.AddContentColumn(new Unit(1.4, UnitType.Centimeter));
                reportConfig.AddContentColumn(new Unit(1.4, UnitType.Centimeter));
                reportConfig.AddContentColumn(new Unit(1.4, UnitType.Centimeter));
                reportConfig.AddContentColumn(new Unit(2.15, UnitType.Centimeter));
                reportConfig.AddContentColumn(new Unit(3.7, UnitType.Centimeter));

                foreach (var item in ppmpVM.NonDBMItems.OrderBy(d => d.ItemName))
                {
                    var itemName = item.ItemCode + " - " + item.ItemName;

                    reportConfig.AddContentRow(new Unit(0.2, UnitType.Inch));
                    reportConfig.AddContent(item.ProjectCode, 0, new Unit(8, UnitType.Point), true, ParagraphAlignment.Left, VerticalAlignment.Center);
                    reportConfig.AddContent(itemName, 1, new Unit(8, UnitType.Point), true, ParagraphAlignment.Left, VerticalAlignment.Center);
                    reportConfig.AddContent(item.IndividualUOMReference, 2, new Unit(8, UnitType.Point), false, ParagraphAlignment.Center, VerticalAlignment.Center);
                    reportConfig.AddContent(String.Format("{0:G}", int.Parse(item.JanMilestone)), 3, new Unit(8, UnitType.Point), false, ParagraphAlignment.Center, VerticalAlignment.Center);
                    reportConfig.AddContent(String.Format("{0:G}", int.Parse(item.FebMilestone)), 4, new Unit(8, UnitType.Point), false, ParagraphAlignment.Center, VerticalAlignment.Center);
                    reportConfig.AddContent(String.Format("{0:G}", int.Parse(item.MarMilestone)), 5, new Unit(8, UnitType.Point), false, ParagraphAlignment.Center, VerticalAlignment.Center);
                    reportConfig.AddContent(String.Format("{0:G}", int.Parse(item.AprMilestone)), 6, new Unit(8, UnitType.Point), false, ParagraphAlignment.Center, VerticalAlignment.Center);
                    reportConfig.AddContent(String.Format("{0:G}", int.Parse(item.MayMilestone)), 7, new Unit(8, UnitType.Point), false, ParagraphAlignment.Center, VerticalAlignment.Center);
                    reportConfig.AddContent(String.Format("{0:G}", int.Parse(item.JunMilestone)), 8, new Unit(8, UnitType.Point), false, ParagraphAlignment.Center, VerticalAlignment.Center);
                    reportConfig.AddContent(String.Format("{0:G}", int.Parse(item.JulMilestone)), 9, new Unit(8, UnitType.Point), false, ParagraphAlignment.Center, VerticalAlignment.Center);
                    reportConfig.AddContent(String.Format("{0:G}", int.Parse(item.AugMilestone)), 10, new Unit(8, UnitType.Point), false, ParagraphAlignment.Center, VerticalAlignment.Center);
                    reportConfig.AddContent(String.Format("{0:G}", int.Parse(item.SepMilestone)), 11, new Unit(8, UnitType.Point), false, ParagraphAlignment.Center, VerticalAlignment.Center);
                    reportConfig.AddContent(String.Format("{0:G}", int.Parse(item.OctMilestone)), 12, new Unit(8, UnitType.Point), false, ParagraphAlignment.Center, VerticalAlignment.Center);
                    reportConfig.AddContent(String.Format("{0:G}", int.Parse(item.NovMilestone)), 13, new Unit(8, UnitType.Point), false, ParagraphAlignment.Center, VerticalAlignment.Center);
                    reportConfig.AddContent(String.Format("{0:G}", int.Parse(item.DecMilestone)), 14, new Unit(8, UnitType.Point), false, ParagraphAlignment.Center, VerticalAlignment.Center);
                    reportConfig.AddContent(String.Format("{0:G}", item.TotalQty), 15, new Unit(8, UnitType.Point), true, ParagraphAlignment.Right, VerticalAlignment.Center);
                    reportConfig.AddContent(String.Format("{0:C}", item.UnitCost), 16, new Unit(8, UnitType.Point), true, ParagraphAlignment.Right, VerticalAlignment.Center);
                    reportConfig.AddContent(String.Format("{0:C}", item.EstimatedBudget), 17, new Unit(8, UnitType.Point), true, ParagraphAlignment.Right, VerticalAlignment.Center);
                    reportConfig.AddContent(item.Remarks + ";\n" + item.Project, 18, new Unit(8, UnitType.Point), false, ParagraphAlignment.Justify, VerticalAlignment.Center);

                }

                reportConfig.AddContentRow(new Unit(0.2, UnitType.Inch));
                reportConfig.AddContent("ESTIMATED BUDGET (NON DBM-PS): ", 0, new Unit(10, UnitType.Point), true, ParagraphAlignment.Right, VerticalAlignment.Center, 15);
                reportConfig.AddContent(String.Format("{0:C}", ppmpVM.NonDBMItems.Sum(d => d.EstimatedBudget)), 16, new Unit(10, UnitType.Point), true, ParagraphAlignment.Right, VerticalAlignment.Center, 1);
                reportConfig.AddContent("", 10, new Unit(9, UnitType.Point), true, ParagraphAlignment.Left, VerticalAlignment.Center);
            }

            var totalEstimatedBudget = ppmpVM.DBMItems.Sum(d => d.EstimatedBudget) + ppmpVM.NonDBMItems.Sum(d => d.EstimatedBudget);
            reportConfig.AddContentRow(new Unit(0.2, UnitType.Inch));
            reportConfig.AddContent("TOTAL ESTIMATED BUDGET:", 0, new Unit(10, UnitType.Point), true, ParagraphAlignment.Right, VerticalAlignment.Center, 15);
            reportConfig.AddContent(String.Format("{0:C}", totalEstimatedBudget), 16, new Unit(10, UnitType.Point), true, ParagraphAlignment.Right, VerticalAlignment.Center, 1);
            reportConfig.AddContent("", 10, new Unit(9, UnitType.Point), true, ParagraphAlignment.Left, VerticalAlignment.Center);

            reportConfig.AddTable();
            reportConfig.AddContentColumn(new Unit(0.5, UnitType.Inch));
            reportConfig.AddContentColumn(new Unit(10, UnitType.Inch));

            reportConfig.AddContentRow();
            reportConfig.AddContent("\n\n", 0);
            reportConfig.AddContentRow();
            reportConfig.AddContent("", 0);
            reportConfig.AddContent("1. Technical Specifications for each Item/Project being proposed shall  be submitted as part of the PPMP.", 1, new Unit(7, UnitType.Point));
            reportConfig.AddContentRow();
            reportConfig.AddContent("", 0);
            reportConfig.AddContent("2. Technical Specifications however,  must be in generic form;  no brand name shall be specified.", 1, new Unit(7, UnitType.Point));
            reportConfig.AddContentRow();
            reportConfig.AddContent("", 0);
            reportConfig.AddContent("3. Non-submission of PPMP for supplies shall mean no budget provision for supplies.", 1, new Unit(7, UnitType.Point));
            reportConfig.AddContentRow();
            reportConfig.AddContent("", 0);
            reportConfig.AddContent("4. Final quantity of items specified is subject to budget approval.", 1, new Unit(7, UnitType.Point));

            reportConfig.AddTable();
            reportConfig.AddContentColumn(new Unit(1.6, UnitType.Centimeter));
            reportConfig.AddContentColumn(new Unit(3.5, UnitType.Centimeter));
            reportConfig.AddContentColumn(new Unit(7, UnitType.Centimeter));
            reportConfig.AddContentColumn(new Unit(5, UnitType.Centimeter));
            reportConfig.AddContentColumn(new Unit(3.5, UnitType.Centimeter));
            reportConfig.AddContentColumn(new Unit(7, UnitType.Centimeter));
            reportConfig.AddContentColumn(new Unit(1.6, UnitType.Centimeter));

            reportConfig.AddContentRow();
            reportConfig.AddContent("\n\n\n\n", 0);

            reportConfig.AddContentRow();
            reportConfig.AddContent("", 0);
            reportConfig.AddContent("Prepared by:", 1, new Unit(9, UnitType.Point), false, ParagraphAlignment.Left, false);
            reportConfig.AddContent("", 2, new Unit(9, UnitType.Point), false, ParagraphAlignment.Center, true);
            reportConfig.AddContent("", 3);
            reportConfig.AddContent("Submitted by:", 4, new Unit(9, UnitType.Point), false, ParagraphAlignment.Left, false);
            reportConfig.AddContent("", 5, new Unit(9, UnitType.Point), false, ParagraphAlignment.Center, true);
            reportConfig.AddContent("", 6);

            reportConfig.AddContentRow();
            reportConfig.AddContent("", 0);
            reportConfig.AddContent("", 1);
            reportConfig.AddContent(ppmpVM.Header.PreparedBy.Replace(", ", "\n"), 2, new Unit(9, UnitType.Point), true, ParagraphAlignment.Center, false);
            reportConfig.AddContent("", 3);
            reportConfig.AddContent("", 4);
            reportConfig.AddContent(ppmpVM.Header.SubmittedBy.Replace(", ", "\n"), 5, new Unit(9, UnitType.Point), true, ParagraphAlignment.Center, false);
            reportConfig.AddContent("", 6);

            reportConfig.AddContentRow();
            reportConfig.AddContent("", 0);
            reportConfig.AddContent("", 1);
            reportConfig.AddContent(((DateTime)ppmpVM.Header.CreatedAt).ToString("dd MMMM yyyy hh:mm tt"), 2, new Unit(7, UnitType.Point), false, ParagraphAlignment.Center, false);
            reportConfig.AddContent("", 3);
            reportConfig.AddContent("", 4);
            reportConfig.AddContent((string.IsNullOrEmpty(ppmpVM.Header.SubmittedAt.ToString())) ? "(Submission Pending)" : ((DateTime)ppmpVM.Header.SubmittedAt).ToString("dd MMMM yyyy hh:mm tt"), 5, new Unit(7, UnitType.Point), false, ParagraphAlignment.Center, false);
            reportConfig.AddContent("", 6);

            return reportConfig.GenerateReport();
        }
        public MemoryStream GeneratePPMPReport(string ReferenceNo, string LogoPath, string UserEmail)
        {
            PPMPViewModel ppmpVM = ppmpDAL.GetPPMPDetails(ReferenceNo, UserEmail);

            reportConfig.ReportTitle = ppmpVM.Header.ReferenceNo;
            reportConfig.ReportFormTitle = "Form C";
            reportConfig.ReportReferenceNo = ppmpVM.Header.ReferenceNo;
            reportConfig.LogoPath = LogoPath;
            reportConfig.DocumentSetupLandscape();
            reportConfig.AddHeader("PROJECT PROCUREMENT MANAGEMENT PLAN", new Unit(10, UnitType.Point), true);
            reportConfig.AddHeader(ppmpVM.Header.PPMPType, new Unit(8, UnitType.Point));
            reportConfig.AddHeader("Fiscal Year " + ppmpVM.Header.FiscalYear, new Unit(8, UnitType.Point));
            reportConfig.AddHeader("\n");

            reportConfig.AddTable();
            reportConfig.AddContentColumn(new Unit(2, UnitType.Centimeter), ParagraphAlignment.Left);
            reportConfig.AddContentColumn(new Unit(18, UnitType.Centimeter), ParagraphAlignment.Left);
            reportConfig.AddContentColumn(new Unit(10.48, UnitType.Centimeter));

            reportConfig.AddContentRow();
            reportConfig.AddContent("End User: ", 0, 10);
            reportConfig.AddContent(ppmpVM.Header.Office, 1, 10, true, ParagraphAlignment.Left, true);
            reportConfig.AddContentRow();
            reportConfig.AddContent("\n", 0);

            if (ppmpVM.DBMItems.Count != 0)
            {

                reportConfig.AddTable(true);
                reportConfig.AddContentColumn(new Unit(2, UnitType.Centimeter), ParagraphAlignment.Center, new Color(252, 207, 101));
                reportConfig.AddContentColumn(new Unit(5.15, UnitType.Centimeter), ParagraphAlignment.Center, new Color(252, 207, 101));
                reportConfig.AddContentColumn(new Unit(1.75, UnitType.Centimeter), ParagraphAlignment.Center, new Color(252, 207, 101));
                reportConfig.AddContentColumn(new Unit(2.75, UnitType.Centimeter), ParagraphAlignment.Center, new Color(252, 207, 101));
                reportConfig.AddContentColumn(new Unit(1.4, UnitType.Centimeter), ParagraphAlignment.Center, new Color(252, 207, 101));
                reportConfig.AddContentColumn(new Unit(1.4, UnitType.Centimeter), ParagraphAlignment.Center, new Color(252, 207, 101));
                reportConfig.AddContentColumn(new Unit(1.4, UnitType.Centimeter), ParagraphAlignment.Center, new Color(252, 207, 101));
                reportConfig.AddContentColumn(new Unit(1.4, UnitType.Centimeter), ParagraphAlignment.Center, new Color(252, 207, 101));
                reportConfig.AddContentColumn(new Unit(1.4, UnitType.Centimeter), ParagraphAlignment.Center, new Color(252, 207, 101));
                reportConfig.AddContentColumn(new Unit(1.4, UnitType.Centimeter), ParagraphAlignment.Center, new Color(252, 207, 101));
                reportConfig.AddContentColumn(new Unit(1.4, UnitType.Centimeter), ParagraphAlignment.Center, new Color(252, 207, 101));
                reportConfig.AddContentColumn(new Unit(1.4, UnitType.Centimeter), ParagraphAlignment.Center, new Color(252, 207, 101));
                reportConfig.AddContentColumn(new Unit(1.4, UnitType.Centimeter), ParagraphAlignment.Center, new Color(252, 207, 101));
                reportConfig.AddContentColumn(new Unit(1.4, UnitType.Centimeter), ParagraphAlignment.Center, new Color(252, 207, 101));
                reportConfig.AddContentColumn(new Unit(1.4, UnitType.Centimeter), ParagraphAlignment.Center, new Color(252, 207, 101));
                reportConfig.AddContentColumn(new Unit(1.4, UnitType.Centimeter), ParagraphAlignment.Center, new Color(252, 207, 101));
                reportConfig.AddContentColumn(new Unit(3.3, UnitType.Centimeter), ParagraphAlignment.Center, new Color(252, 207, 101));

                reportConfig.AddContentRow(new Unit(0.2, UnitType.Inch));
                reportConfig.AddContent("AVAILABLE AT PROCUREMENT SERVICE STORES", 0, new Unit(10, UnitType.Point), true, ParagraphAlignment.Left, VerticalAlignment.Center, 16);

                reportConfig.AddContentRow(new Unit(0.2, UnitType.Inch));
                reportConfig.AddContent("PAP\nCode", 0, new Unit(8, UnitType.Point), true, ParagraphAlignment.Center, VerticalAlignment.Center, null, 1);
                reportConfig.AddContent("General Description", 1, new Unit(8, UnitType.Point), true, ParagraphAlignment.Center, VerticalAlignment.Center, null, 1);
                reportConfig.AddContent("Quantity/\nSize", 2, new Unit(7, UnitType.Point), true, ParagraphAlignment.Center, VerticalAlignment.Center, null, 1);
                reportConfig.AddContent("Estimated\nBudget", 3, new Unit(8, UnitType.Point), true, ParagraphAlignment.Center, VerticalAlignment.Center, null, 1);
                reportConfig.AddContent("Schedule/Milestone of Activities", 4, new Unit(10, UnitType.Point), true, ParagraphAlignment.Center, VerticalAlignment.Center, 11);
                reportConfig.AddContent("Remarks", 16, new Unit(8, UnitType.Point), true, ParagraphAlignment.Center, VerticalAlignment.Center, null, 1);

                reportConfig.AddContentRow(new Unit(0.2, UnitType.Inch));
                reportConfig.AddContent("Jan", 4, new Unit(8, UnitType.Point), true, ParagraphAlignment.Center, VerticalAlignment.Center);
                reportConfig.AddContent("Feb", 5, new Unit(8, UnitType.Point), true, ParagraphAlignment.Center, VerticalAlignment.Center);
                reportConfig.AddContent("Mar", 6, new Unit(8, UnitType.Point), true, ParagraphAlignment.Center, VerticalAlignment.Center);
                reportConfig.AddContent("Apr", 7, new Unit(8, UnitType.Point), true, ParagraphAlignment.Center, VerticalAlignment.Center);
                reportConfig.AddContent("May", 8, new Unit(8, UnitType.Point), true, ParagraphAlignment.Center, VerticalAlignment.Center);
                reportConfig.AddContent("Jun", 9, new Unit(8, UnitType.Point), true, ParagraphAlignment.Center, VerticalAlignment.Center);
                reportConfig.AddContent("Jul", 10, new Unit(8, UnitType.Point), true, ParagraphAlignment.Center, VerticalAlignment.Center);
                reportConfig.AddContent("Aug", 11, new Unit(8, UnitType.Point), true, ParagraphAlignment.Center, VerticalAlignment.Center);
                reportConfig.AddContent("Sep", 12, new Unit(8, UnitType.Point), true, ParagraphAlignment.Center, VerticalAlignment.Center);
                reportConfig.AddContent("Oct", 13, new Unit(8, UnitType.Point), true, ParagraphAlignment.Center, VerticalAlignment.Center);
                reportConfig.AddContent("Nov", 14, new Unit(8, UnitType.Point), true, ParagraphAlignment.Center, VerticalAlignment.Center);
                reportConfig.AddContent("Dec", 15, new Unit(8, UnitType.Point), true, ParagraphAlignment.Center, VerticalAlignment.Center);

                reportConfig.AddTable(true);
                reportConfig.AddContentColumn(new Unit(2, UnitType.Centimeter));
                reportConfig.AddContentColumn(new Unit(5.15, UnitType.Centimeter));
                reportConfig.AddContentColumn(new Unit(1.75, UnitType.Centimeter));
                reportConfig.AddContentColumn(new Unit(2.75, UnitType.Centimeter));
                reportConfig.AddContentColumn(new Unit(1.4, UnitType.Centimeter));
                reportConfig.AddContentColumn(new Unit(1.4, UnitType.Centimeter));
                reportConfig.AddContentColumn(new Unit(1.4, UnitType.Centimeter));
                reportConfig.AddContentColumn(new Unit(1.4, UnitType.Centimeter));
                reportConfig.AddContentColumn(new Unit(1.4, UnitType.Centimeter));
                reportConfig.AddContentColumn(new Unit(1.4, UnitType.Centimeter));
                reportConfig.AddContentColumn(new Unit(1.4, UnitType.Centimeter));
                reportConfig.AddContentColumn(new Unit(1.4, UnitType.Centimeter));
                reportConfig.AddContentColumn(new Unit(1.4, UnitType.Centimeter));
                reportConfig.AddContentColumn(new Unit(1.4, UnitType.Centimeter));
                reportConfig.AddContentColumn(new Unit(1.4, UnitType.Centimeter));
                reportConfig.AddContentColumn(new Unit(1.4, UnitType.Centimeter));
                reportConfig.AddContentColumn(new Unit(3.3, UnitType.Centimeter));

                foreach (var item in ppmpVM.DBMItems.OrderBy(d => d.ItemName))
                {
                    var itemName = item.ItemCode + "\n\n" + item.ItemName + ((item.ItemSpecifications == null) ? "" : "\n\n" + item.ItemSpecifications);

                    reportConfig.AddContentRow(new Unit(0.2, UnitType.Inch));
                    reportConfig.AddContent(item.ProjectCode, 0, new Unit(8, UnitType.Point), true, ParagraphAlignment.Left, VerticalAlignment.Top);
                    reportConfig.AddContent(itemName, 1, new Unit(8, UnitType.Point), true, ParagraphAlignment.Justify, VerticalAlignment.Top);
                    reportConfig.AddContent(String.Format("{0:G}", item.TotalQty) + "\n" + item.IndividualUOMReference, 2, new Unit(8, UnitType.Point), false, ParagraphAlignment.Center, VerticalAlignment.Top);
                    reportConfig.AddContent(String.Format("{0:C}", item.EstimatedBudget), 3, new Unit(8, UnitType.Point), true, ParagraphAlignment.Right, VerticalAlignment.Top);
                    reportConfig.AddContent(((item.JanMilestone == null) ? "" : item.JanMilestone), 4, new Unit(8, UnitType.Point), false, ParagraphAlignment.Center, VerticalAlignment.Top);
                    reportConfig.AddContent(((item.FebMilestone == null) ? "" : item.FebMilestone), 5, new Unit(8, UnitType.Point), false, ParagraphAlignment.Center, VerticalAlignment.Top);
                    reportConfig.AddContent(((item.MarMilestone == null) ? "" : item.MarMilestone), 6, new Unit(8, UnitType.Point), false, ParagraphAlignment.Center, VerticalAlignment.Top);
                    reportConfig.AddContent(((item.AprMilestone == null) ? "" : item.AprMilestone), 7, new Unit(8, UnitType.Point), false, ParagraphAlignment.Center, VerticalAlignment.Top);
                    reportConfig.AddContent(((item.MayMilestone == null) ? "" : item.MayMilestone), 8, new Unit(8, UnitType.Point), false, ParagraphAlignment.Center, VerticalAlignment.Top);
                    reportConfig.AddContent(((item.JunMilestone == null) ? "" : item.JunMilestone), 9, new Unit(8, UnitType.Point), false, ParagraphAlignment.Center, VerticalAlignment.Top);
                    reportConfig.AddContent(((item.JulMilestone == null) ? "" : item.JulMilestone), 10, new Unit(8, UnitType.Point), false, ParagraphAlignment.Center, VerticalAlignment.Top);
                    reportConfig.AddContent(((item.AugMilestone == null) ? "" : item.AugMilestone), 11, new Unit(8, UnitType.Point), false, ParagraphAlignment.Center, VerticalAlignment.Top);
                    reportConfig.AddContent(((item.SepMilestone == null) ? "" : item.SepMilestone), 12, new Unit(8, UnitType.Point), false, ParagraphAlignment.Center, VerticalAlignment.Top);
                    reportConfig.AddContent(((item.OctMilestone == null) ? "" : item.OctMilestone), 13, new Unit(8, UnitType.Point), false, ParagraphAlignment.Center, VerticalAlignment.Top);
                    reportConfig.AddContent(((item.NovMilestone == null) ? "" : item.NovMilestone), 14, new Unit(8, UnitType.Point), false, ParagraphAlignment.Center, VerticalAlignment.Top);
                    reportConfig.AddContent(((item.DecMilestone == null) ? "" : item.DecMilestone), 15, new Unit(8, UnitType.Point), false, ParagraphAlignment.Center, VerticalAlignment.Top);
                    reportConfig.AddContent(item.Remarks + ";\n" + item.Project, 16, new Unit(8, UnitType.Point), false, ParagraphAlignment.Justify, VerticalAlignment.Top);
                }

                reportConfig.AddContentRow(new Unit(0.2, UnitType.Inch));
                reportConfig.AddContent("ESTIMATED BUDGET (DBM-PS): ", 0, new Unit(10, UnitType.Point), true, ParagraphAlignment.Left, VerticalAlignment.Center, 1);
                reportConfig.AddContent(String.Format("{0:C}", ppmpVM.DBMItems.Sum(d => d.EstimatedBudget)), 2, new Unit(10, UnitType.Point), true, ParagraphAlignment.Right, VerticalAlignment.Center, 1);
                reportConfig.AddContent("", 4, new Unit(9, UnitType.Point), true, ParagraphAlignment.Left, VerticalAlignment.Center, 12);
            }

            if (ppmpVM.NonDBMItems.Count != 0)
            {
                reportConfig.AddTable(true);
                reportConfig.AddContentColumn(new Unit(2, UnitType.Centimeter), ParagraphAlignment.Center, new Color(252, 207, 101));
                reportConfig.AddContentColumn(new Unit(5.15, UnitType.Centimeter), ParagraphAlignment.Center, new Color(252, 207, 101));
                reportConfig.AddContentColumn(new Unit(1.75, UnitType.Centimeter), ParagraphAlignment.Center, new Color(252, 207, 101));
                reportConfig.AddContentColumn(new Unit(2.75, UnitType.Centimeter), ParagraphAlignment.Center, new Color(252, 207, 101));
                reportConfig.AddContentColumn(new Unit(1.4, UnitType.Centimeter), ParagraphAlignment.Center, new Color(252, 207, 101));
                reportConfig.AddContentColumn(new Unit(1.4, UnitType.Centimeter), ParagraphAlignment.Center, new Color(252, 207, 101));
                reportConfig.AddContentColumn(new Unit(1.4, UnitType.Centimeter), ParagraphAlignment.Center, new Color(252, 207, 101));
                reportConfig.AddContentColumn(new Unit(1.4, UnitType.Centimeter), ParagraphAlignment.Center, new Color(252, 207, 101));
                reportConfig.AddContentColumn(new Unit(1.4, UnitType.Centimeter), ParagraphAlignment.Center, new Color(252, 207, 101));
                reportConfig.AddContentColumn(new Unit(1.4, UnitType.Centimeter), ParagraphAlignment.Center, new Color(252, 207, 101));
                reportConfig.AddContentColumn(new Unit(1.4, UnitType.Centimeter), ParagraphAlignment.Center, new Color(252, 207, 101));
                reportConfig.AddContentColumn(new Unit(1.4, UnitType.Centimeter), ParagraphAlignment.Center, new Color(252, 207, 101));
                reportConfig.AddContentColumn(new Unit(1.4, UnitType.Centimeter), ParagraphAlignment.Center, new Color(252, 207, 101));
                reportConfig.AddContentColumn(new Unit(1.4, UnitType.Centimeter), ParagraphAlignment.Center, new Color(252, 207, 101));
                reportConfig.AddContentColumn(new Unit(1.4, UnitType.Centimeter), ParagraphAlignment.Center, new Color(252, 207, 101));
                reportConfig.AddContentColumn(new Unit(1.4, UnitType.Centimeter), ParagraphAlignment.Center, new Color(252, 207, 101));
                reportConfig.AddContentColumn(new Unit(3.3, UnitType.Centimeter), ParagraphAlignment.Center, new Color(252, 207, 101));

                reportConfig.AddContentRow(new Unit(0.2, UnitType.Inch));
                reportConfig.AddContent("ITEMS PURCHASED FROM OTHER SOURCES (Note: Please indicate price of items)", 0, new Unit(10, UnitType.Point), true, ParagraphAlignment.Left, VerticalAlignment.Center, 16);

                reportConfig.AddContentRow(new Unit(0.2, UnitType.Inch));
                reportConfig.AddContent("PAP\nCode", 0, new Unit(8, UnitType.Point), true, ParagraphAlignment.Center, VerticalAlignment.Center, null, 1);
                reportConfig.AddContent("General Description", 1, new Unit(8, UnitType.Point), true, ParagraphAlignment.Center, VerticalAlignment.Center, null, 1);
                reportConfig.AddContent("Quantity/\nSize", 2, new Unit(7, UnitType.Point), true, ParagraphAlignment.Center, VerticalAlignment.Center, null, 1);
                reportConfig.AddContent("Estimated\nBudget", 3, new Unit(8, UnitType.Point), true, ParagraphAlignment.Center, VerticalAlignment.Center, null, 1);
                reportConfig.AddContent("Schedule/Milestone of Activities", 4, new Unit(10, UnitType.Point), true, ParagraphAlignment.Center, VerticalAlignment.Center, 11);
                reportConfig.AddContent("Remarks", 16, new Unit(8, UnitType.Point), true, ParagraphAlignment.Center, VerticalAlignment.Center, null, 1);

                reportConfig.AddContentRow(new Unit(0.2, UnitType.Inch));
                reportConfig.AddContent("Jan", 4, new Unit(8, UnitType.Point), true, ParagraphAlignment.Center, VerticalAlignment.Center);
                reportConfig.AddContent("Feb", 5, new Unit(8, UnitType.Point), true, ParagraphAlignment.Center, VerticalAlignment.Center);
                reportConfig.AddContent("Mar", 6, new Unit(8, UnitType.Point), true, ParagraphAlignment.Center, VerticalAlignment.Center);
                reportConfig.AddContent("Apr", 7, new Unit(8, UnitType.Point), true, ParagraphAlignment.Center, VerticalAlignment.Center);
                reportConfig.AddContent("May", 8, new Unit(8, UnitType.Point), true, ParagraphAlignment.Center, VerticalAlignment.Center);
                reportConfig.AddContent("Jun", 9, new Unit(8, UnitType.Point), true, ParagraphAlignment.Center, VerticalAlignment.Center);
                reportConfig.AddContent("Jul", 10, new Unit(8, UnitType.Point), true, ParagraphAlignment.Center, VerticalAlignment.Center);
                reportConfig.AddContent("Aug", 11, new Unit(8, UnitType.Point), true, ParagraphAlignment.Center, VerticalAlignment.Center);
                reportConfig.AddContent("Sep", 12, new Unit(8, UnitType.Point), true, ParagraphAlignment.Center, VerticalAlignment.Center);
                reportConfig.AddContent("Oct", 13, new Unit(8, UnitType.Point), true, ParagraphAlignment.Center, VerticalAlignment.Center);
                reportConfig.AddContent("Nov", 14, new Unit(8, UnitType.Point), true, ParagraphAlignment.Center, VerticalAlignment.Center);
                reportConfig.AddContent("Dec", 15, new Unit(8, UnitType.Point), true, ParagraphAlignment.Center, VerticalAlignment.Center);

                reportConfig.AddTable(true);
                reportConfig.AddContentColumn(new Unit(2, UnitType.Centimeter));
                reportConfig.AddContentColumn(new Unit(5.15, UnitType.Centimeter));
                reportConfig.AddContentColumn(new Unit(1.75, UnitType.Centimeter));
                reportConfig.AddContentColumn(new Unit(2.75, UnitType.Centimeter));
                reportConfig.AddContentColumn(new Unit(1.4, UnitType.Centimeter));
                reportConfig.AddContentColumn(new Unit(1.4, UnitType.Centimeter));
                reportConfig.AddContentColumn(new Unit(1.4, UnitType.Centimeter));
                reportConfig.AddContentColumn(new Unit(1.4, UnitType.Centimeter));
                reportConfig.AddContentColumn(new Unit(1.4, UnitType.Centimeter));
                reportConfig.AddContentColumn(new Unit(1.4, UnitType.Centimeter));
                reportConfig.AddContentColumn(new Unit(1.4, UnitType.Centimeter));
                reportConfig.AddContentColumn(new Unit(1.4, UnitType.Centimeter));
                reportConfig.AddContentColumn(new Unit(1.4, UnitType.Centimeter));
                reportConfig.AddContentColumn(new Unit(1.4, UnitType.Centimeter));
                reportConfig.AddContentColumn(new Unit(1.4, UnitType.Centimeter));
                reportConfig.AddContentColumn(new Unit(1.4, UnitType.Centimeter));
                reportConfig.AddContentColumn(new Unit(3.3, UnitType.Centimeter));

                foreach (var item in ppmpVM.NonDBMItems.OrderBy(d => d.ItemName))
                {
                    var itemName = item.ItemCode + "\n\n" + item.ItemName + ((item.ItemSpecifications == null) ? "" : "\n\n" + item.ItemSpecifications);

                    reportConfig.AddContentRow(new Unit(0.2, UnitType.Inch));
                    reportConfig.AddContent(item.ProjectCode, 0, new Unit(8, UnitType.Point), true, ParagraphAlignment.Left, VerticalAlignment.Top);
                    reportConfig.AddContent(itemName, 1, new Unit(8, UnitType.Point), true, ParagraphAlignment.Left, VerticalAlignment.Top);
                    reportConfig.AddContent(String.Format("{0:G}", item.TotalQty) + "\n" + item.IndividualUOMReference, 2, new Unit(8, UnitType.Point), false, ParagraphAlignment.Center, VerticalAlignment.Top);
                    reportConfig.AddContent(String.Format("{0:C}", item.EstimatedBudget), 3, new Unit(8, UnitType.Point), true, ParagraphAlignment.Right, VerticalAlignment.Top);
                    reportConfig.AddContent(((item.JanMilestone == null) ? "" : item.JanMilestone), 4, new Unit(8, UnitType.Point), false, ParagraphAlignment.Center, VerticalAlignment.Top);
                    reportConfig.AddContent(((item.FebMilestone == null) ? "" : item.FebMilestone), 5, new Unit(8, UnitType.Point), false, ParagraphAlignment.Center, VerticalAlignment.Top);
                    reportConfig.AddContent(((item.MarMilestone == null) ? "" : item.MarMilestone), 6, new Unit(8, UnitType.Point), false, ParagraphAlignment.Center, VerticalAlignment.Top);
                    reportConfig.AddContent(((item.AprMilestone == null) ? "" : item.AprMilestone), 7, new Unit(8, UnitType.Point), false, ParagraphAlignment.Center, VerticalAlignment.Top);
                    reportConfig.AddContent(((item.MayMilestone == null) ? "" : item.MayMilestone), 8, new Unit(8, UnitType.Point), false, ParagraphAlignment.Center, VerticalAlignment.Top);
                    reportConfig.AddContent(((item.JunMilestone == null) ? "" : item.JunMilestone), 9, new Unit(8, UnitType.Point), false, ParagraphAlignment.Center, VerticalAlignment.Top);
                    reportConfig.AddContent(((item.JulMilestone == null) ? "" : item.JulMilestone), 10, new Unit(8, UnitType.Point), false, ParagraphAlignment.Center, VerticalAlignment.Top);
                    reportConfig.AddContent(((item.AugMilestone == null) ? "" : item.AugMilestone), 11, new Unit(8, UnitType.Point), false, ParagraphAlignment.Center, VerticalAlignment.Top);
                    reportConfig.AddContent(((item.SepMilestone == null) ? "" : item.SepMilestone), 12, new Unit(8, UnitType.Point), false, ParagraphAlignment.Center, VerticalAlignment.Top);
                    reportConfig.AddContent(((item.OctMilestone == null) ? "" : item.OctMilestone), 13, new Unit(8, UnitType.Point), false, ParagraphAlignment.Center, VerticalAlignment.Top);
                    reportConfig.AddContent(((item.NovMilestone == null) ? "" : item.NovMilestone), 14, new Unit(8, UnitType.Point), false, ParagraphAlignment.Center, VerticalAlignment.Top);
                    reportConfig.AddContent(((item.DecMilestone == null) ? "" : item.DecMilestone), 15, new Unit(8, UnitType.Point), false, ParagraphAlignment.Center, VerticalAlignment.Top);
                    reportConfig.AddContent(item.Remarks + ";\n" + item.Project, 16, new Unit(8, UnitType.Point), false, ParagraphAlignment.Justify, VerticalAlignment.Top);

                }

                reportConfig.AddContentRow(new Unit(0.2, UnitType.Inch));
                reportConfig.AddContent("ESTIMATED BUDGET (NON DBM-PS): ", 0, new Unit(10, UnitType.Point), true, ParagraphAlignment.Left, VerticalAlignment.Center, 1);
                reportConfig.AddContent(String.Format("{0:C}", ppmpVM.NonDBMItems.Sum(d => d.EstimatedBudget)), 2, new Unit(10, UnitType.Point), true, ParagraphAlignment.Right, VerticalAlignment.Center, 1);
                reportConfig.AddContent("", 4, new Unit(9, UnitType.Point), true, ParagraphAlignment.Left, VerticalAlignment.Center, 12);
            }

            var totalEstimatedBudget = ppmpVM.DBMItems.Sum(d => d.EstimatedBudget) + ppmpVM.NonDBMItems.Sum(d => d.EstimatedBudget);
            reportConfig.AddContentRow(new Unit(0.2, UnitType.Inch));
            reportConfig.AddContent("TOTAL ESTIMATED BUDGET: ", 0, new Unit(10, UnitType.Point), true, ParagraphAlignment.Left, VerticalAlignment.Center, 1);
            reportConfig.AddContent(String.Format("{0:C}", totalEstimatedBudget), 2, new Unit(10, UnitType.Point), true, ParagraphAlignment.Right, VerticalAlignment.Center, 1);
            reportConfig.AddContent("", 4, new Unit(9, UnitType.Point), true, ParagraphAlignment.Left, VerticalAlignment.Center, 12);

            reportConfig.AddTable();
            reportConfig.AddContentColumn(new Unit(0.5, UnitType.Inch));
            reportConfig.AddContentColumn(new Unit(10, UnitType.Inch));

            reportConfig.AddContentRow();
            reportConfig.AddContent("\n\n", 0);
            reportConfig.AddContentRow();
            reportConfig.AddContent("", 0);
            reportConfig.AddContent("1. Technical Specifications for each Item/Project being proposed shall  be submitted as part of the PPMP.", 1, new Unit(7, UnitType.Point));
            reportConfig.AddContentRow();
            reportConfig.AddContent("", 0);
            reportConfig.AddContent("2. Technical Specifications however,  must be in generic form;  no brand name shall be specified.", 1, new Unit(7, UnitType.Point));
            reportConfig.AddContentRow();
            reportConfig.AddContent("", 0);
            reportConfig.AddContent("3. Non-submission of PPMP for supplies shall mean no budget provision for supplies.", 1, new Unit(7, UnitType.Point));
            reportConfig.AddContentRow();
            reportConfig.AddContent("", 0);
            reportConfig.AddContent("4. Final quantity of items specified is subject to budget approval.", 1, new Unit(7, UnitType.Point));

            reportConfig.AddTable();
            reportConfig.AddContentColumn(new Unit(1.6, UnitType.Centimeter));
            reportConfig.AddContentColumn(new Unit(3.5, UnitType.Centimeter));
            reportConfig.AddContentColumn(new Unit(7, UnitType.Centimeter));
            reportConfig.AddContentColumn(new Unit(5, UnitType.Centimeter));
            reportConfig.AddContentColumn(new Unit(3.5, UnitType.Centimeter));
            reportConfig.AddContentColumn(new Unit(7, UnitType.Centimeter));
            reportConfig.AddContentColumn(new Unit(1.6, UnitType.Centimeter));

            reportConfig.AddContentRow();
            reportConfig.AddContent("\n\n\n\n", 0);

            reportConfig.AddContentRow();
            reportConfig.AddContent("", 0);
            reportConfig.AddContent("Prepared by:", 1, new Unit(9, UnitType.Point), false, ParagraphAlignment.Left, false);
            reportConfig.AddContent("", 2, new Unit(9, UnitType.Point), false, ParagraphAlignment.Center, true);
            reportConfig.AddContent("", 3);
            reportConfig.AddContent("Submitted by:", 4, new Unit(9, UnitType.Point), false, ParagraphAlignment.Left, false);
            reportConfig.AddContent("", 5, new Unit(9, UnitType.Point), false, ParagraphAlignment.Center, true);
            reportConfig.AddContent("", 6);

            reportConfig.AddContentRow();
            reportConfig.AddContent("", 0);
            reportConfig.AddContent("", 1);
            reportConfig.AddContent(ppmpVM.Header.PreparedBy.Replace(", ", "\n"), 2, new Unit(9, UnitType.Point), true, ParagraphAlignment.Center, false);
            reportConfig.AddContent("", 3);
            reportConfig.AddContent("", 4);
            reportConfig.AddContent(ppmpVM.Header.SubmittedBy.Replace(", ", "\n"), 5, new Unit(9, UnitType.Point), true, ParagraphAlignment.Center, false);
            reportConfig.AddContent("", 6);

            reportConfig.AddContentRow();
            reportConfig.AddContent("", 0);
            reportConfig.AddContent("", 1);
            reportConfig.AddContent(((DateTime)ppmpVM.Header.CreatedAt).ToString("dd MMMM yyyy hh:mm tt"), 2, new Unit(7, UnitType.Point), false, ParagraphAlignment.Center, false);
            reportConfig.AddContent("", 3);
            reportConfig.AddContent("", 4);
            reportConfig.AddContent((string.IsNullOrEmpty(ppmpVM.Header.SubmittedAt.ToString())) ? "(Submission Pending)" : ((DateTime)ppmpVM.Header.SubmittedAt).ToString("dd MMMM yyyy hh:mm tt"), 5, new Unit(7, UnitType.Point), false, ParagraphAlignment.Center, false);
            reportConfig.AddContent("", 6);

            reportConfig.ReportTitle = ppmpVM.Header.ReferenceNo;
            reportConfig.ReportFormTitle = "";
            reportConfig.ReportReferenceNo = "";
            reportConfig.LogoPath = LogoPath;

            reportConfig.NewPage();
            reportConfig.AddHeader("MARKET SURVEY", new Unit(10, UnitType.Point), true);
            reportConfig.AddHeader(ppmpVM.Header.PPMPType, new Unit(8, UnitType.Point));
            reportConfig.AddHeader("Fiscal Year " + ppmpVM.Header.FiscalYear, new Unit(8, UnitType.Point));
            reportConfig.AddHeader("\n");

            reportConfig.AddTable(true);
            reportConfig.AddContentColumn(new Unit(2, UnitType.Centimeter), ParagraphAlignment.Center, new Color(252, 207, 101));
            reportConfig.AddContentColumn(new Unit(2, UnitType.Centimeter), ParagraphAlignment.Center, new Color(252, 207, 101));
            reportConfig.AddContentColumn(new Unit(9, UnitType.Centimeter), ParagraphAlignment.Center, new Color(252, 207, 101));
            reportConfig.AddContentColumn(new Unit(1, UnitType.Centimeter), ParagraphAlignment.Center, new Color(252, 207, 101));
            reportConfig.AddContentColumn(new Unit(10.25, UnitType.Centimeter), ParagraphAlignment.Center, new Color(252, 207, 101));
            reportConfig.AddContentColumn(new Unit(2, UnitType.Centimeter), ParagraphAlignment.Center, new Color(252, 207, 101));
            reportConfig.AddContentColumn(new Unit(2, UnitType.Centimeter), ParagraphAlignment.Center, new Color(252, 207, 101));
            reportConfig.AddContentColumn(new Unit(3, UnitType.Centimeter), ParagraphAlignment.Center, new Color(252, 207, 101));

            reportConfig.AddContentRow();
            reportConfig.AddContent("Item\nNo.", 0, new Unit(8, UnitType.Point), true, ParagraphAlignment.Center, VerticalAlignment.Center, null, 1);
            reportConfig.AddContent("Quantity/\n Size/Unit", 1, new Unit(8, UnitType.Point), true, ParagraphAlignment.Center, VerticalAlignment.Center, null, 1);
            reportConfig.AddContent("General Description", 2, new Unit(8, UnitType.Point), true, ParagraphAlignment.Center, VerticalAlignment.Center, null, 1);
            reportConfig.AddContent("Supplier Details", 3, new Unit(8, UnitType.Point), true, ParagraphAlignment.Center, VerticalAlignment.Center, 1, 1);
            reportConfig.AddContent("Unit Cost", 5, new Unit(8, UnitType.Point), true, ParagraphAlignment.Center, VerticalAlignment.Center, null, 1);
            reportConfig.AddContent("Total Cost", 6, new Unit(8, UnitType.Point), true, ParagraphAlignment.Center, VerticalAlignment.Center, null, 1);
            reportConfig.AddContent("Average Total Cost", 7, new Unit(8, UnitType.Point), true, ParagraphAlignment.Center, VerticalAlignment.Center, null, 1);
            reportConfig.AddContentRow();


            reportConfig.AddTable(true);
            reportConfig.AddContentColumn(new Unit(2, UnitType.Centimeter));
            reportConfig.AddContentColumn(new Unit(2, UnitType.Centimeter));
            reportConfig.AddContentColumn(new Unit(9, UnitType.Centimeter));
            reportConfig.AddContentColumn(new Unit(1, UnitType.Centimeter));
            reportConfig.AddContentColumn(new Unit(10.25, UnitType.Centimeter));
            reportConfig.AddContentColumn(new Unit(2, UnitType.Centimeter));
            reportConfig.AddContentColumn(new Unit(2, UnitType.Centimeter));
            reportConfig.AddContentColumn(new Unit(3, UnitType.Centimeter));

            for (int i = 0; i < ppmpVM.NonDBMItems.Count; i++)
            {
                var item = ppmpVM.NonDBMItems[i].ItemCode + "\n\n" + ppmpVM.NonDBMItems[i].ItemName.ToUpper() + "\n\n" + ppmpVM.NonDBMItems[i].ItemSpecifications;
                var size = ppmpVM.NonDBMItems[i].TotalQty.ToString() + "\n" + ppmpVM.NonDBMItems[i].IndividualUOMReference;
                var supplier1Name = "Name: " + ppmpVM.NonDBMItems[i].Supplier1Name;
                var supplier1Address = "Address: " + ppmpVM.NonDBMItems[i].Supplier1Address;
                var supplier1ContactNo = "Contact No: " + ppmpVM.NonDBMItems[i].Supplier1ContactNo;
                var supplier1EmailAddress = "Email Address: " + ppmpVM.NonDBMItems[i].Supplier1EmailAddress;
                var supplier2Name = "Name: " + ppmpVM.NonDBMItems[i].Supplier2Name;
                var supplier2Address = "Address: " + ppmpVM.NonDBMItems[i].Supplier2Address;
                var supplier2ContactNo = "Contact No: " + ppmpVM.NonDBMItems[i].Supplier2ContactNo;
                var supplier2EmailAddress = "Email Address: " + ppmpVM.NonDBMItems[i].Supplier2EmailAddress;
                var supplier3Name = "Name: " + ppmpVM.NonDBMItems[i].Supplier3Name;
                var supplier3Address = "Address: " + ppmpVM.NonDBMItems[i].Supplier3Address;
                var supplier3ContactNo = "Contact No: " + ppmpVM.NonDBMItems[i].Supplier3ContactNo;
                var supplier3EmailAddress = "Email Address: " + ppmpVM.NonDBMItems[i].Supplier3EmailAddress;
                reportConfig.AddContentRow();
                reportConfig.AddContent((i + 1).ToString(), 0, new Unit(8, UnitType.Point), true, ParagraphAlignment.Center, VerticalAlignment.Center, null, 2);
                reportConfig.AddContent(size, 1, new Unit(8, UnitType.Point), true, ParagraphAlignment.Center, VerticalAlignment.Center, null, 2);
                reportConfig.AddContent(item, 2, new Unit(8, UnitType.Point), true, ParagraphAlignment.Left, VerticalAlignment.Center, null, 2);
                reportConfig.AddContent("1", 3, new Unit(8, UnitType.Point), true, ParagraphAlignment.Center, VerticalAlignment.Center);
                reportConfig.AddContent(supplier1Name + "\n" + supplier1Address + "\n" + supplier1ContactNo + "\n" + supplier1EmailAddress, 4, new Unit(8, UnitType.Point), true, ParagraphAlignment.Left, VerticalAlignment.Top);
                reportConfig.AddContent(String.Format("{0:C}", ppmpVM.NonDBMItems[i].Supplier1UnitCost), 5, new Unit(8, UnitType.Point), true, ParagraphAlignment.Right, VerticalAlignment.Top);
                reportConfig.AddContent(String.Format("{0:C}", (ppmpVM.NonDBMItems[i].Supplier1UnitCost * ppmpVM.NonDBMItems[i].TotalQty)), 6, new Unit(8, UnitType.Point), true, ParagraphAlignment.Right, VerticalAlignment.Top);
                reportConfig.AddContent(String.Format("{0:C}", ppmpVM.NonDBMItems[i].EstimatedBudget), 7, new Unit(8, UnitType.Point), true, ParagraphAlignment.Right, VerticalAlignment.Top, null, 2);
                reportConfig.AddContentRow();
                reportConfig.AddContent("2", 3, new Unit(8, UnitType.Point), true, ParagraphAlignment.Center, VerticalAlignment.Center);
                reportConfig.AddContent(supplier2Name + "\n" + supplier2Address + "\n" + supplier2ContactNo + "\n" + supplier2EmailAddress, 4, new Unit(8, UnitType.Point), true, ParagraphAlignment.Left, VerticalAlignment.Top);
                reportConfig.AddContent(String.Format("{0:C}", ppmpVM.NonDBMItems[i].Supplier2UnitCost), 5, new Unit(8, UnitType.Point), true, ParagraphAlignment.Right, VerticalAlignment.Top);
                reportConfig.AddContent(String.Format("{0:C}", (ppmpVM.NonDBMItems[i].Supplier2UnitCost * ppmpVM.NonDBMItems[i].TotalQty)), 6, new Unit(8, UnitType.Point), true, ParagraphAlignment.Right, VerticalAlignment.Top);
                reportConfig.AddContent(String.Format("{0:C}", ppmpVM.NonDBMItems[i].EstimatedBudget), 7, new Unit(8, UnitType.Point), true, ParagraphAlignment.Right, VerticalAlignment.Top, null, 2);
                reportConfig.AddContentRow();
                reportConfig.AddContent("3", 3, new Unit(8, UnitType.Point), true, ParagraphAlignment.Center, VerticalAlignment.Center);
                reportConfig.AddContent(supplier3Name + "\n" + supplier3Address + "\n" + supplier3ContactNo + "\n" + supplier3EmailAddress, 4, new Unit(8, UnitType.Point), true, ParagraphAlignment.Left, VerticalAlignment.Top);
                reportConfig.AddContent(String.Format("{0:C}", ppmpVM.NonDBMItems[i].Supplier3UnitCost), 5, new Unit(8, UnitType.Point), true, ParagraphAlignment.Right, VerticalAlignment.Top);
                reportConfig.AddContent(String.Format("{0:C}", (ppmpVM.NonDBMItems[i].Supplier3UnitCost * ppmpVM.NonDBMItems[i].TotalQty)), 6, new Unit(8, UnitType.Point), true, ParagraphAlignment.Right, VerticalAlignment.Top);
                reportConfig.AddContent(String.Format("{0:C}", ppmpVM.NonDBMItems[i].EstimatedBudget), 7, new Unit(8, UnitType.Point), true, ParagraphAlignment.Right, VerticalAlignment.Top, null, 2);
            }

            reportConfig.AddTable();
            reportConfig.AddContentColumn(new Unit(0.5, UnitType.Inch));
            reportConfig.AddContentColumn(new Unit(10, UnitType.Inch));

            reportConfig.AddContentRow();
            reportConfig.AddContent("\n\n", 0);
            reportConfig.AddContentRow();
            reportConfig.AddContent("", 0);
            reportConfig.AddContent("Note:", 1, new Unit(7, UnitType.Point), true);
            reportConfig.AddContentRow();
            reportConfig.AddContent("", 0);
            reportConfig.AddContent(" * The items must have same specifications.", 1, new Unit(7, UnitType.Point));
            reportConfig.AddContentRow();
            reportConfig.AddContent("", 0);
            reportConfig.AddContent(" * Quotations should include Taxes, Freights, Delivery, Installation Costs and other incidental costs (if applicable).", 1, new Unit(7, UnitType.Point));
            reportConfig.AddContentRow();
            reportConfig.AddContent("", 0);
            reportConfig.AddContent(" * There should be at least three (3) suppliers .", 1, new Unit(7, UnitType.Point));
            reportConfig.AddContentRow();
            reportConfig.AddContent("", 0);
            reportConfig.AddContent(" * Prices should be in Philippine Peso (Php) Currency.", 1, new Unit(7, UnitType.Point));
            reportConfig.AddContentRow();
            reportConfig.AddContent("", 0);
            reportConfig.AddContent(" * Quotation forms and/or brochures must be attached as proof.", 1, new Unit(7, UnitType.Point));

            reportConfig.AddTable();
            reportConfig.AddContentColumn(new Unit(1.6, UnitType.Centimeter));
            reportConfig.AddContentColumn(new Unit(3.5, UnitType.Centimeter));
            reportConfig.AddContentColumn(new Unit(7, UnitType.Centimeter));
            reportConfig.AddContentColumn(new Unit(5, UnitType.Centimeter));
            reportConfig.AddContentColumn(new Unit(3.5, UnitType.Centimeter));
            reportConfig.AddContentColumn(new Unit(7, UnitType.Centimeter));
            reportConfig.AddContentColumn(new Unit(1.6, UnitType.Centimeter));

            reportConfig.AddContentRow();
            reportConfig.AddContent("\n\n\n\n", 0);

            reportConfig.AddContentRow();
            reportConfig.AddContent("", 0);
            reportConfig.AddContent("Prepared by:", 1, new Unit(9, UnitType.Point), false, ParagraphAlignment.Left, false);
            reportConfig.AddContent("", 2, new Unit(9, UnitType.Point), false, ParagraphAlignment.Center, true);
            reportConfig.AddContent("", 3);
            reportConfig.AddContent("Submitted by:", 4, new Unit(9, UnitType.Point), false, ParagraphAlignment.Left, false);
            reportConfig.AddContent("", 5, new Unit(9, UnitType.Point), false, ParagraphAlignment.Center, true);
            reportConfig.AddContent("", 6);

            reportConfig.AddContentRow();
            reportConfig.AddContent("", 0);
            reportConfig.AddContent("", 1);
            reportConfig.AddContent(ppmpVM.Header.PreparedBy.Replace(", ", "\n"), 2, new Unit(9, UnitType.Point), true, ParagraphAlignment.Center, false);
            reportConfig.AddContent("", 3);
            reportConfig.AddContent("", 4);
            reportConfig.AddContent(ppmpVM.Header.SubmittedBy.Replace(", ", "\n"), 5, new Unit(9, UnitType.Point), true, ParagraphAlignment.Center, false);
            reportConfig.AddContent("", 6);

            reportConfig.AddContentRow();
            reportConfig.AddContent("", 0);
            reportConfig.AddContent("", 1);
            reportConfig.AddContent(((DateTime)ppmpVM.Header.CreatedAt).ToString("dd MMMM yyyy hh:mm tt"), 2, new Unit(7, UnitType.Point), false, ParagraphAlignment.Center, false);
            reportConfig.AddContent("", 3);
            reportConfig.AddContent("", 4);
            reportConfig.AddContent((string.IsNullOrEmpty(ppmpVM.Header.SubmittedAt.ToString())) ? "(Submission Pending)" : ((DateTime)ppmpVM.Header.SubmittedAt).ToString("dd MMMM yyyy hh:mm tt"), 5, new Unit(7, UnitType.Point), false, ParagraphAlignment.Center, false);
            reportConfig.AddContent("", 6);

            return reportConfig.GenerateReport();
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                ppmpDAL.Dispose();
            }
            base.Dispose(disposing);
        }
    }
    public class ProjectProcurementManagementPlanDAL : Controller
    {
        private FMISDbContext db = new FMISDbContext();
        private HRISDbContext hrdb = new HRISDbContext();

        public List<string> GetPPMPFiscalYears()
        {
            return db.PPMPHeader.GroupBy(d => d.FiscalYear).Select(d =>  d.Key).ToList();
        }
        public List<PPMPHeaderViewModel> GetPPMPList(string UserEmail)
        {
            var user = db.UserAccounts.Where(d => d.Email == UserEmail).FirstOrDefault();
            var office = hrdb.OfficeModel.Find(user.FKUserInformationReference.Office);
            return db.PPMPHeader.Where(d => d.OfficeReference == office.ID)
                   .Select(d => new PPMPHeaderViewModel {
                       ReferenceNo = d.ReferenceNo,
                       FiscalYear = d.FiscalYear,
                       PPMPType = d.FKPPMPTypeReference.InventoryTypeName,
                       EstimatedBudget = (db.InventoryTypes.Where(x => x.ID == d.PPMPType).FirstOrDefault().IsTangible) ? (decimal)db.PPMPItemDetails.Where(x => x.PPMPReference == d.ID).Sum(x => x.EstimatedBudget) : (decimal)db.PPMPServiceDetails.Where(x => x.PPMPReference == d.ID).Sum(x => x.EstimatedBudget),
                       Status = d.Status
                   }).ToList();
        }
        public PPMPViewModel GetPPMPDetails(string ReferenceNo, string UserEmail)
        {
            var user = db.UserAccounts.Where(d => d.Email == UserEmail).FirstOrDefault();
            var office = hrdb.OfficeModel.Find(user.FKUserInformationReference.Office);
            PPMPViewModel ppmpVM = new PPMPViewModel();
            ppmpVM.DBMItems = new List<PPMPItemDetailsVM>();
            ppmpVM.NonDBMItems = new List<PPMPItemDetailsVM>();
            ppmpVM.Header = db.PPMPHeader.Where(d => d.ReferenceNo == ReferenceNo && d.OfficeReference == office.ID)
                            .Select(d => new PPMPHeaderViewModel
                            {
                                ReferenceNo = d.ReferenceNo,
                                FiscalYear = d.FiscalYear,
                                PPMPType = d.FKPPMPTypeReference.InventoryTypeName,
                                Office = office.OfficeName,
                                EstimatedBudget = (db.InventoryTypes.Where(x => x.ID == d.PPMPType).FirstOrDefault().IsTangible)? (decimal)db.PPMPItemDetails.Where(x => x.PPMPReference == d.ID).Sum(x => x.EstimatedBudget) : (decimal)db.PPMPServiceDetails.Where(x => x.PPMPReference == d.ID).Sum(x => x.EstimatedBudget),
                                Status = d.Status,
                                CreatedAt = (DateTime)d.CreatedAt,
                                SubmittedAt = d.SubmittedAt,
                                PreparedBy = user.FKUserInformationReference.FirstName.ToUpper() + " " + user.FKUserInformationReference.LastName.ToUpper() + ", " + user.FKUserInformationReference.Designation,
                                SubmittedBy = office.OfficeHead.ToUpper() + ", " + office.Designation
                            }).FirstOrDefault();
            if (ppmpVM.Header == null)
            {
                return null;
            }
            ppmpVM.Workflow = db.PPMPApprovalWorkflow.Where(d => d.FKPPMPHeader.ReferenceNo == ReferenceNo)
                              .Select(d => new PPMPApprovalWorkflowViewModel
                              {
                                  ReferenceNo = ReferenceNo,
                                  Status = d.Status,
                                  UpdatedAt = d.UpdatedAt,
                                  Remarks = d.Remarks,
                                  Office = office.OfficeName,
                                  Personnel = user.FKUserInformationReference.FirstName.ToUpper() + " " + user.FKUserInformationReference.LastName.ToUpper()
                              }).ToList();
            var inventoryType = db.InventoryTypes.Where(d => d.InventoryTypeName == ppmpVM.Header.PPMPType).FirstOrDefault();
            if(inventoryType.IsTangible)
            {
                var DBMItems = db.PPMPItemDetails.Where(d => d.FKPPMPReference.ReferenceNo == ppmpVM.Header.ReferenceNo && d.FKItem.ProcurementSource == ProcurementSources.PS_DBM)
                    .OrderBy(d => d.FKItem.ItemName).OrderBy(d => d.FKProjectPlanReference.ProjectCode).ToList();
                ppmpVM.DBMItems = DBMItems
                    .Select(d => new PPMPItemDetailsVM
                    {
                        ProjectCode = d.FKProjectPlanReference.ProjectCode,
                        Project = d.FKProjectPlanReference.ProjectName,
                        ItemCode = d.FKItem.ItemCode,
                        ItemName = d.FKItem.ItemName.ToUpper() + ", " + d.FKItem.ItemShortSpecifications,
                        ItemSpecifications = d.FKItem.ItemSpecifications,
                        ProcurementSource = d.FKItem.ProcurementSource,
                        ItemImage = d.FKItem.ItemImage,
                        IndividualUOMReference = d.FKItem.FKIndividualUnitReference.Abbreviation,
                        JanMilestone = (d.JanMilestone == null || d.JanMilestone == "0") ? null : d.JanMilestone,
                        FebMilestone = (d.FebMilestone == null || d.FebMilestone == "0") ? null : d.FebMilestone,
                        MarMilestone = (d.MarMilestone == null || d.MarMilestone == "0") ? null : d.MarMilestone,
                        AprMilestone = (d.AprMilestone == null || d.AprMilestone == "0") ? null : d.AprMilestone,
                        MayMilestone = (d.MayMilestone == null || d.MayMilestone == "0") ? null : d.MayMilestone,
                        JunMilestone = (d.JunMilestone == null || d.JunMilestone == "0") ? null : d.JunMilestone,
                        JulMilestone = (d.JulMilestone == null || d.JulMilestone == "0") ? null : d.JulMilestone,
                        AugMilestone = (d.AugMilestone == null || d.AugMilestone == "0") ? null : d.AugMilestone,
                        SepMilestone = (d.SepMilestone == null || d.SepMilestone == "0") ? null : d.SepMilestone,
                        OctMilestone = (d.OctMilestone == null || d.OctMilestone == "0") ? null : d.OctMilestone,
                        NovMilestone = (d.NovMilestone == null || d.NovMilestone == "0") ? null : d.NovMilestone,
                        DecMilestone = (d.DecMilestone == null || d.DecMilestone == "0") ? null : d.DecMilestone,
                        TotalQty = d.TotalQty,
                        UnitCost = (decimal)d.UnitCost,
                        EstimatedBudget = (decimal)d.EstimatedBudget,
                        Remarks = d.Remarks
                    }).ToList();

                var NonDBMItems = db.PPMPItemDetails.Where(d => d.FKPPMPReference.ReferenceNo == ReferenceNo && d.FKItem.ProcurementSource == ProcurementSources.Non_DBM).OrderBy(d => d.FKItem.ItemName).OrderBy(d => d.FKProjectPlanReference.ProjectCode).ToList();
                foreach (var item in NonDBMItems)
                {
                    var supplier1ID = db.ProjectPlanItems.Where(x => x.FKProjectReference.ProjectCode == item.FKProjectPlanReference.ProjectCode && x.FKItemReference.ItemCode == item.FKItem.ItemCode).FirstOrDefault().Supplier1;
                    var supplier1 = db.Suppliers.Find(supplier1ID);
                    var supplier1Cost = db.ProjectPlanItems.Where(x => x.FKProjectReference.ProjectCode == item.FKProjectPlanReference.ProjectCode && x.FKItemReference.ItemCode == item.FKItem.ItemCode).FirstOrDefault().Supplier1UnitCost;
                    var supplier2ID = db.ProjectPlanItems.Where(x => x.FKProjectReference.ProjectCode == item.FKProjectPlanReference.ProjectCode && x.FKItemReference.ItemCode == item.FKItem.ItemCode).FirstOrDefault().Supplier2;
                    var supplier2 = (supplier2ID == null) ? new Supplier() : db.Suppliers.Find(supplier2ID);
                    var supplier2Cost = db.ProjectPlanItems.Where(x => x.FKProjectReference.ProjectCode == item.FKProjectPlanReference.ProjectCode && x.FKItemReference.ItemCode == item.FKItem.ItemCode).FirstOrDefault().Supplier2UnitCost;
                    var supplier3ID = db.ProjectPlanItems.Where(x => x.FKProjectReference.ProjectCode == item.FKProjectPlanReference.ProjectCode && x.FKItemReference.ItemCode == item.FKItem.ItemCode).FirstOrDefault().Supplier3;
                    var supplier3 = (supplier3ID == null) ? new Supplier() : db.Suppliers.Find(supplier3ID);
                    var supplier3Cost = db.ProjectPlanItems.Where(x => x.FKProjectReference.ProjectCode == item.FKProjectPlanReference.ProjectCode && x.FKItemReference.ItemCode == item.FKItem.ItemCode).FirstOrDefault().Supplier3UnitCost;

                    PPMPItemDetailsVM ppmpItem = new PPMPItemDetailsVM()
                    {
                        ProjectCode = item.FKProjectPlanReference.ProjectCode,
                        Project = item.FKProjectPlanReference.ProjectName,
                        ItemCode = item.FKItem.ItemCode,
                        ItemName = item.FKItem.ItemName.ToUpper() + ", " + item.FKItem.ItemShortSpecifications,
                        ItemSpecifications = item.FKItem.ItemSpecifications,
                        ProcurementSource = item.FKItem.ProcurementSource,
                        ItemImage = item.FKItem.ItemImage,
                        IndividualUOMReference = item.FKItem.FKIndividualUnitReference.Abbreviation,
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
                        UnitCost = (decimal)item.UnitCost,
                        EstimatedBudget = (decimal)item.EstimatedBudget,
                        Supplier1 = supplier1ID,
                        Supplier1Name = supplier1.SupplierName,
                        Supplier1Address = supplier1.Address,
                        Supplier1ContactNo = supplier1.ContactNumber,
                        Supplier1EmailAddress = (supplier1.EmailAddress == null) ? "Not Provided" : supplier1.EmailAddress,
                        Supplier1UnitCost = supplier1Cost,
                        Supplier2 = supplier2ID,
                        Supplier2Name = supplier2.SupplierName,
                        Supplier2Address = supplier2.Address,
                        Supplier2ContactNo = supplier2.ContactNumber,
                        Supplier2EmailAddress = (supplier2.EmailAddress == null) ? "Not Provided" : supplier2.EmailAddress,
                        Supplier2UnitCost = supplier2Cost,
                        Supplier3 = supplier3ID,
                        Supplier3Name = supplier3.SupplierName,
                        Supplier3Address = supplier3.Address,
                        Supplier3ContactNo = supplier3.ContactNumber,
                        Supplier3EmailAddress = (supplier3.EmailAddress == null) ? "Not Provided" : supplier3.EmailAddress,
                        Supplier3UnitCost = supplier3Cost,
                        Remarks = item.Remarks
                    };
                    ppmpVM.NonDBMItems.Add(ppmpItem);
                }
            }
            else
            {
                ppmpVM.DBMItems = db.PPMPServiceDetails.Where(d => d.FKPPMPReference.ReferenceNo == ReferenceNo && d.FKServiceReference.ProcurementSource == ProcurementSources.PS_DBM)
                    .OrderBy(d => d.FKServiceReference.ServiceName).OrderBy(d => d.FKProjectPlanReference.ProjectCode)
                    .Select(d => new PPMPItemDetailsVM
                    {
                        ProjectCode = d.FKProjectPlanReference.ProjectCode,
                        Project = d.FKProjectPlanReference.ProjectName,
                        ItemCode = d.FKServiceReference.ServiceCode,
                        ItemName = d.FKServiceReference.ServiceName.ToUpper() + ", " + d.FKServiceReference.ItemShortSpecifications,
                        ItemSpecifications = d.Specifications,
                        ProcurementSource = d.FKServiceReference.ProcurementSource,
                        ItemImage = null,
                        IndividualUOMReference = null,
                        JanMilestone = d.JanMilestone,
                        FebMilestone = d.FebMilestone,
                        MarMilestone = d.MarMilestone,
                        AprMilestone = d.AprMilestone,
                        MayMilestone = d.MayMilestone,
                        JunMilestone = d.JunMilestone,
                        JulMilestone = d.JulMilestone,
                        AugMilestone = d.AugMilestone,
                        SepMilestone = d.SepMilestone,
                        OctMilestone = d.OctMilestone,
                        NovMilestone = d.NovMilestone,
                        DecMilestone = d.DecMilestone,
                        TotalQty = d.TotalQty,
                        UnitCost = (decimal)d.UnitCost,
                        EstimatedBudget = (decimal)d.EstimatedBudget,
                        Remarks = d.Remarks
                    }).ToList();

                ppmpVM.NonDBMItems = new List<PPMPItemDetailsVM>();
                foreach (var item in db.PPMPServiceDetails.Where(d => d.FKPPMPReference.ReferenceNo == ReferenceNo && d.FKServiceReference.ProcurementSource == ProcurementSources.Non_DBM).OrderBy(d => d.FKServiceReference.ServiceName).OrderBy(d => d.FKProjectPlanReference.ProjectCode).ToList())
                {
                    var supplier1ID = db.ProjectPlanServices.Where(x => x.FKProjectReference.ProjectCode == item.FKProjectPlanReference.ProjectCode && x.FKServiceReference.ServiceCode == item.FKServiceReference.ServiceCode).FirstOrDefault().Supplier1;
                    var supplier1 = db.Suppliers.Find(supplier1ID);
                    var supplier1Cost = db.ProjectPlanServices.Where(x => x.FKProjectReference.ProjectCode == item.FKProjectPlanReference.ProjectCode && x.FKServiceReference.ServiceCode == item.FKServiceReference.ServiceCode).FirstOrDefault().Supplier1UnitCost;
                    var supplier2ID = db.ProjectPlanServices.Where(x => x.FKProjectReference.ProjectCode == item.FKProjectPlanReference.ProjectCode && x.FKServiceReference.ServiceCode == item.FKServiceReference.ServiceCode).FirstOrDefault().Supplier2;
                    var supplier2 = db.Suppliers.Find(supplier2ID);
                    var supplier2Cost = db.ProjectPlanServices.Where(x => x.FKProjectReference.ProjectCode == item.FKProjectPlanReference.ProjectCode && x.FKServiceReference.ServiceCode == item.FKServiceReference.ServiceCode).FirstOrDefault().Supplier2UnitCost;
                    var supplier3ID = db.ProjectPlanServices.Where(x => x.FKProjectReference.ProjectCode == item.FKProjectPlanReference.ProjectCode && x.FKServiceReference.ServiceCode == item.FKServiceReference.ServiceCode).FirstOrDefault().Supplier3;
                    var supplier3 = db.Suppliers.Find(supplier3ID);
                    var supplier3Cost = db.ProjectPlanServices.Where(x => x.FKProjectReference.ProjectCode == item.FKProjectPlanReference.ProjectCode && x.FKServiceReference.ServiceCode == item.FKServiceReference.ServiceCode).FirstOrDefault().Supplier3UnitCost;

                    PPMPItemDetailsVM ppmpItem = new PPMPItemDetailsVM()
                    {
                        ProjectCode = item.FKProjectPlanReference.ProjectCode,
                        Project = item.FKProjectPlanReference.ProjectName,
                        ItemCode = item.FKServiceReference.ServiceCode,
                        ItemName = item.FKServiceReference.ServiceName.ToUpper() + ", " + item.FKServiceReference.ItemShortSpecifications,
                        ItemSpecifications = item.Specifications,
                        ProcurementSource = item.FKServiceReference.ProcurementSource,
                        ItemImage = null,
                        IndividualUOMReference = null,
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
                        UnitCost = (decimal)item.UnitCost,
                        EstimatedBudget = (decimal)item.EstimatedBudget,
                        Supplier1 = supplier1ID,
                        Supplier1Name = supplier1.SupplierName,
                        Supplier1Address = supplier1.Address,
                        Supplier1ContactNo = supplier1.ContactNumber,
                        Supplier1EmailAddress = (supplier1.EmailAddress == null) ? "Not Provided" : supplier1.EmailAddress,
                        Supplier1UnitCost = (decimal)supplier1Cost,
                        Supplier2 = supplier2ID,
                        Supplier2Name = supplier2.SupplierName,
                        Supplier2Address = supplier2.Address,
                        Supplier2ContactNo = supplier2.ContactNumber,
                        Supplier2EmailAddress = (supplier2.EmailAddress == null) ? "Not Provided" : supplier2.EmailAddress,
                        Supplier2UnitCost = supplier2Cost,
                        Supplier3 = supplier3ID,
                        Supplier3Name = supplier3.SupplierName,
                        Supplier3Address = supplier3.Address,
                        Supplier3ContactNo = supplier3.ContactNumber,
                        Supplier3EmailAddress = (supplier3.EmailAddress == null) ? "Not Provided" : supplier3.EmailAddress,
                        Supplier3UnitCost = supplier3Cost,
                        Remarks = item.Remarks
                    };
                    ppmpVM.NonDBMItems.Add(ppmpItem);
                }
            }
            ppmpVM.DBMItems = ppmpVM.DBMItems.OrderBy(d => d.ItemName).ToList();
            ppmpVM.NonDBMItems = ppmpVM.NonDBMItems.OrderBy(d => d.ItemName).ToList();
            return ppmpVM;
        }
        public bool PostToPPMP(ProjectPlanVM project, string UserEmail)
        {
            var office = hrdb.OfficeModel.Where(d => d.OfficeName == project.Office).FirstOrDefault();
            var user = db.UserAccounts.Where(d => d.Email == UserEmail).FirstOrDefault();
            var projectItems = db.ProjectPlanItems.Where(d => d.FKProjectReference.ProjectCode == project.ProjectCode).ToList();
            var projectServices = db.ProjectPlanServices.Where(d => d.FKProjectReference.ProjectCode == project.ProjectCode).ToList();
            List<string> projectTypes = new List<string>();
            projectTypes.AddRange(projectItems.GroupBy(d => d.FKItemReference.FKInventoryTypeReference.InventoryTypeName).Select(d => d.Key).ToList());
            projectTypes.AddRange(projectServices.GroupBy(d => d.FKServiceReference.FKInventoryTypeReference.InventoryTypeName).Select(d => d.Key).ToList());
            var inventoryTypes = db.InventoryTypes.Where(d => projectTypes.Contains(d.InventoryTypeName)).ToList();

            if(projectItems == null)
            {
                return false;
            }

            List<PPMPItemDetails> ppmpItems = new List<PPMPItemDetails>();
            List<PPMPServiceDetails> ppmpServices = new List<PPMPServiceDetails>();
            PPMPHeader ppmpHeader = new PPMPHeader();
            foreach (var projectType in inventoryTypes)
            {
                var ppmp = db.PPMPHeader.Where(d => d.FKPPMPTypeReference.InventoryTypeName == projectType.InventoryTypeName && d.FiscalYear == project.FiscalYear && d.OfficeReference == office.ID).FirstOrDefault();
                if(ppmp == null)
                {
                    ppmpHeader = new PPMPHeader()
                    {
                        ReferenceNo = GenerateReferenceNo(project.FiscalYear, office.OfficeCode, inventoryTypes.Where(d => d.InventoryTypeName == projectType.InventoryTypeName).FirstOrDefault().InventoryCode),
                        FiscalYear = project.FiscalYear,
                        PPMPType = inventoryTypes.Where(d => d.InventoryTypeName == projectType.InventoryTypeName).FirstOrDefault().ID,
                        PreparedBy = user.ID,
                        SubmittedBy = office.OfficeHead,
                        OfficeReference = office.ID,
                        CreatedAt = DateTime.Now,
                        Status = "New PPMP"
                    };

                    db.PPMPHeader.Add(ppmpHeader);
                    if (db.SaveChanges() == 0)
                    {
                        return false;
                    }

                    if(ppmpHeader.FKPPMPTypeReference.IsTangible)
                    {
                        foreach (var item in projectItems)
                        {
                            if (item.FKItemReference.FKInventoryTypeReference.InventoryTypeName == projectType.InventoryTypeName)
                            {
                                PPMPItemDetails ppmpItem = new PPMPItemDetails();
                                ppmpItem.PPMPReference = ppmpHeader.ID;
                                ppmpItem.ProjectPlanReference = item.ProjectReference;
                                ppmpItem.ItemReference = item.ItemReference;
                                ppmpItem.Remarks = ((item.ProposalType == BudgetProposalType.Actual) ? "Actual Obligation" : "New Spending Proposal; " + item.Remarks);
                                ppmpItem.TotalQty = item.TotalQty;
                                ppmpItem.UnitCost = item.UnitCost;
                                ppmpItem.EstimatedBudget = item.EstimatedBudget;
                                if (projectType.InventoryTypeName == "Common Use Office Supplies")
                                {
                                    ppmpItem.JanMilestone = item.JanQty.ToString();
                                    ppmpItem.FebMilestone = item.FebQty.ToString();
                                    ppmpItem.MarMilestone = item.MarQty.ToString();
                                    ppmpItem.AprMilestone = item.AprQty.ToString();
                                    ppmpItem.MayMilestone = item.MayQty.ToString();
                                    ppmpItem.JunMilestone = item.JunQty.ToString();
                                    ppmpItem.JulMilestone = item.JulQty.ToString();
                                    ppmpItem.AugMilestone = item.AugQty.ToString();
                                    ppmpItem.SepMilestone = item.SepQty.ToString();
                                    ppmpItem.OctMilestone = item.OctQty.ToString();
                                    ppmpItem.NovMilestone = item.NovQty.ToString();
                                    ppmpItem.DecMilestone = item.DecQty.ToString();
                                }
                                else
                                {
                                    //non cse
                                    ppmpItem = AssignPPMPItemMilestones(ppmpItem);
                                }
                                ppmpItems.Add(ppmpItem);
                            }
                        }
                    }
                    else
                    {
                        foreach (var item in projectServices)
                        {
                            if (item.FKServiceReference.FKInventoryTypeReference.InventoryTypeName == projectType.InventoryTypeName)
                            {
                                PPMPServiceDetails ppmpService = new PPMPServiceDetails();
                                ppmpService.PPMPReference = ppmpHeader.ID;
                                ppmpService.ProjectPlanReference = item.ProjectReference;
                                ppmpService.ServiceReference = item.ServiceReference;
                                ppmpService.Specifications = item.ItemSpecifications;
                                ppmpService.Remarks = ((item.ProposalType == BudgetProposalType.Actual) ? "Actual Obligation" : "New Spending Proposal; " + item.Remarks);
                                ppmpService.TotalQty = item.Quantity;
                                ppmpService.UnitCost = (decimal)item.UnitCost;
                                ppmpService.EstimatedBudget = (decimal)item.EstimatedBudget;
                                ppmpService = AssignPPMPServiceMilestones(ppmpService);
                                ppmpServices.Add(ppmpService);
                            }
                        }
                    }
                }
                else //existing
                {
                    if(projectType.IsTangible)
                    {
                        PPMPItemDetails ppmpItem = new PPMPItemDetails();
                        foreach (var item in projectItems)
                        {
                            ppmpItem = new PPMPItemDetails();
                            var ppmpExistingItem = db.PPMPItemDetails.Where(d => d.ProjectPlanReference == item.ProjectReference && d.ItemReference == item.ItemReference && d.PPMPReference == ppmp.ID).FirstOrDefault();
                            if (ppmpExistingItem == null && item.FKItemReference.FKInventoryTypeReference.InventoryTypeName == projectType.InventoryTypeName)
                            {
                                ppmpItem.PPMPReference = ppmp.ID;
                                ppmpItem.ProjectPlanReference = item.ProjectReference;
                                ppmpItem.ItemReference = item.ItemReference;
                                ppmpItem.Remarks = ((item.ProposalType == BudgetProposalType.Actual) ? "Actual Obligation" : "New Spending Proposal; " + item.Remarks);
                                if (projectType.InventoryTypeName == "Common Use Office Supplies")
                                {
                                    ppmpItem.JanMilestone = (item.JanQty == null) ? "0" : item.JanQty.ToString();
                                    ppmpItem.FebMilestone = (item.FebQty == null) ? "0" : item.FebQty.ToString();
                                    ppmpItem.MarMilestone = (item.MarQty == null) ? "0" : item.MarQty.ToString();
                                    ppmpItem.AprMilestone = (item.AprQty == null) ? "0" : item.AprQty.ToString();
                                    ppmpItem.MayMilestone = (item.MayQty == null) ? "0" : item.MayQty.ToString();
                                    ppmpItem.JunMilestone = (item.JunQty == null) ? "0" : item.JunQty.ToString();
                                    ppmpItem.JulMilestone = (item.JulQty == null) ? "0" : item.JulQty.ToString();
                                    ppmpItem.AugMilestone = (item.AugQty == null) ? "0" : item.AugQty.ToString();
                                    ppmpItem.SepMilestone = (item.SepQty == null) ? "0" : item.SepQty.ToString();
                                    ppmpItem.OctMilestone = (item.OctQty == null) ? "0" : item.OctQty.ToString();
                                    ppmpItem.NovMilestone = (item.NovQty == null) ? "0" : item.NovQty.ToString();
                                    ppmpItem.DecMilestone = (item.DecQty == null) ? "0" : item.DecQty.ToString();
                                    ppmpItem.TotalQty = item.TotalQty;
                                    ppmpItem.UnitCost = item.UnitCost;
                                    ppmpItem.EstimatedBudget = item.EstimatedBudget;
                                }
                                else
                                {
                                    //non cse
                                    ppmpItem = AssignPPMPItemMilestones(ppmpItem);
                                    ppmpItem.TotalQty = item.TotalQty;
                                    ppmpItem.UnitCost = item.UnitCost;
                                    ppmpItem.EstimatedBudget = item.EstimatedBudget;
                                }
                                ppmpItems.Add(ppmpItem);
                            }
                        }
                    }
                    else
                    {
                        foreach (var item in projectServices)
                        {
                            var ppmpExistingItem = db.PPMPServiceDetails.Where(d => d.ProjectPlanReference == item.ProjectReference && d.ServiceReference == item.ServiceReference && d.PPMPReference == ppmp.ID).FirstOrDefault();
                            if (ppmpExistingItem == null && item.FKServiceReference.FKInventoryTypeReference.InventoryTypeName == projectType.InventoryTypeName)
                            {
                                PPMPServiceDetails ppmpService = new PPMPServiceDetails();
                                ppmpService.PPMPReference = ppmp.ID;
                                ppmpService.ProjectPlanReference = item.ProjectReference;
                                ppmpService.ServiceReference = item.ServiceReference;
                                ppmpService.Remarks = ((item.ProposalType == BudgetProposalType.Actual) ? "Actual Obligation" : "New Spending Proposal; " + item.Remarks);
                                ppmpService.TotalQty = item.Quantity;
                                ppmpService.UnitCost = item.UnitCost;
                                ppmpService.EstimatedBudget = item.EstimatedBudget;
                                ppmpService = AssignPPMPServiceMilestones(ppmpService);
                                ppmpServices.Add(ppmpService);
                            }
                        }
                    }
                }
            }

            db.PPMPItemDetails.AddRange(ppmpItems);
            db.PPMPServiceDetails.AddRange(ppmpServices);
            if (db.SaveChanges() == 0)
            {
                return false;
            }

            var ppmpApproval = new PPMPApprovalWorkflow
            {
                PPMPId = ppmpHeader.ID,
                Status = "PPMP Posted",
                UpdatedAt = DateTime.Now,
                Remarks = "PPMP is posted by" + user.FKUserInformationReference.FirstName + " " + user.FKUserInformationReference.LastName + " on " + DateTime.Now.ToString() + ".",
                ActionMadeBy = user.ID,
                ActionMadeByOffice = office.ID
            };

            db.PPMPApprovalWorkflow.Add(ppmpApproval);
            if (db.SaveChanges() == 0)
            {
                return false;
            }
            return true;
        }
        public PPMPItemDetails AssignPPMPItemMilestones(PPMPItemDetails Item)
        {
            var StartMonth = db.ProjectPlans.Where(d => d.ID == Item.ProjectPlanReference).FirstOrDefault().ProjectMonthStart;
            var FiscalYear = int.Parse(db.ProjectPlans.Where(d => d.ID == Item.ProjectPlanReference).FirstOrDefault().FiscalYear) - 1;
            switch (StartMonth)
            {
                case 1:
                    {
                        Item.JanMilestone = "PR Preparation/Pre-Bid Conference - (Oct, "+ FiscalYear +"); Procurement Activities - (Nov-Dec, "+ FiscalYear +"); Issuance of NTP/Delivery" ;
                    }
                    break;
                case 2:
                    {
                        Item.JanMilestone = "PR Preparation/Pre-Bid Conference - (Nov, " + FiscalYear + "); Procurement Activities - (Dec, " + FiscalYear + " - Jan, "+ (FiscalYear + 1) +");";
                        Item.FebMilestone = "Issuance of NTP/Delivery";
                    }
                    break;
                case 3:
                    {
                        Item.JanMilestone = "PR Preparation/Pre-Bid Conference - (Dec, " + FiscalYear + "); Procurement Activities";
                        Item.FebMilestone = "Procurement Activities";
                        Item.MarMilestone = "Issuance of NTP/Delivery";
                    }
                    break;
                case 4:
                    {
                        Item.JanMilestone = "PR Preparation/Pre-Bid Conference";
                        Item.FebMilestone = "Procurement Activities";
                        Item.MarMilestone = "Procurement Activities";
                        Item.AprMilestone = "Issuance of NTP/Delivery";
                    }
                    break;
                case 5:
                    {
                        Item.FebMilestone = "PR Preparation/Pre-Bid Conference";
                        Item.MarMilestone = "Procurement Activities";
                        Item.AprMilestone = "Procurement Activities";
                        Item.MayMilestone = "Issuance of NTP/Delivery";
                    }
                    break;
                case 6:
                    {
                        Item.MarMilestone = "PR Preparation/Pre-Bid Conference";
                        Item.AprMilestone = "Procurement Activities";
                        Item.MayMilestone = "Procurement Activities";
                        Item.JunMilestone = "Issuance of NTP/Delivery";
                    }
                    break;
                case 7:
                    {
                        Item.AprMilestone = "PR Preparation/Pre-Bid Conference";
                        Item.MayMilestone = "Procurement Activities";
                        Item.JunMilestone = "Procurement Activities";
                        Item.JulMilestone = "Issuance of NTP/Delivery";
                    }
                    break;
                case 8:
                    {
                        Item.MayMilestone = "PR Preparation/Pre-Bid Conference";
                        Item.JunMilestone = "Procurement Activities";
                        Item.JulMilestone = "Procurement Activities";
                        Item.AugMilestone = "Issuance of NTP/Delivery";
                    }
                    break;
                case 9:
                    {
                        Item.JunMilestone = "PR Preparation/Pre-Bid Conference";
                        Item.JulMilestone = "Procurement Activities";
                        Item.AugMilestone = "Procurement Activities";
                        Item.SepMilestone = "Issuance of NTP/Delivery";
                    }
                    break;
                case 10:
                    {
                        Item.JulMilestone = "PR Preparation/Pre-Bid Conference";
                        Item.AugMilestone = "Procurement Activities";
                        Item.SepMilestone = "Procurement Activities";
                        Item.OctMilestone = "Issuance of NTP/Delivery";
                    }
                    break;
                case 11:
                    {
                        Item.AugMilestone = "PR Preparation/Pre-Bid Conference";
                        Item.SepMilestone = "Procurement Activities";
                        Item.OctMilestone = "Procurement Activities";
                        Item.NovMilestone = "Issuance of NTP/Delivery";
                    }
                    break;
                case 12:
                    {
                        Item.SepMilestone = "PR Preparation/Pre-Bid Conference";
                        Item.OctMilestone = "Procurement Activities";
                        Item.NovMilestone = "Procurement Activities";
                        Item.DecMilestone = "Issuance of NTP/Delivery";
                    }
                    break;
            }
            return Item;
        }
        public PPMPServiceDetails AssignPPMPServiceMilestones(PPMPServiceDetails Item)
        {
            var StartMonth = db.ProjectPlans.Where(d => d.ID == Item.ProjectPlanReference).FirstOrDefault().ProjectMonthStart;
            var FiscalYear = int.Parse(db.ProjectPlans.Where(d => d.ID == Item.ProjectPlanReference).FirstOrDefault().FiscalYear) - 1;
            switch (StartMonth)
            {
                case 1:
                    {
                        Item.JanMilestone = "PR Preparation/Pre-Bid Conference - (Oct, " + FiscalYear + "); Procurement Activities - (Nov-Dec, " + FiscalYear + "); Issuance of NTP/Delivery";
                    }
                    break;
                case 2:
                    {
                        Item.JanMilestone = "PR Preparation/Pre-Bid Conference - (Nov, " + FiscalYear + "); Procurement Activities - (Dec, " + FiscalYear + " - Jan, " + (FiscalYear + 1) + ");";
                        Item.FebMilestone = "Issuance of NTP/Delivery";
                    }
                    break;
                case 3:
                    {
                        Item.JanMilestone = "PR Preparation/Pre-Bid Conference - (Dec, " + FiscalYear + "); Procurement Activities";
                        Item.FebMilestone = "Procurement Activities";
                        Item.MarMilestone = "Issuance of NTP/Delivery";
                    }
                    break;
                case 4:
                    {
                        Item.JanMilestone = "PR Preparation/Pre-Bid Conference";
                        Item.FebMilestone = "Procurement Activities";
                        Item.MarMilestone = "Procurement Activities";
                        Item.AprMilestone = "Issuance of NTP/Delivery";
                    }
                    break;
                case 5:
                    {
                        Item.FebMilestone = "PR Preparation/Pre-Bid Conference";
                        Item.MarMilestone = "Procurement Activities";
                        Item.AprMilestone = "Procurement Activities";
                        Item.MayMilestone = "Issuance of NTP/Delivery";
                    }
                    break;
                case 6:
                    {
                        Item.MarMilestone = "PR Preparation/Pre-Bid Conference";
                        Item.AprMilestone = "Procurement Activities";
                        Item.MayMilestone = "Procurement Activities";
                        Item.JunMilestone = "Issuance of NTP/Delivery";
                    }
                    break;
                case 7:
                    {
                        Item.AprMilestone = "PR Preparation/Pre-Bid Conference";
                        Item.MayMilestone = "Procurement Activities";
                        Item.JunMilestone = "Procurement Activities";
                        Item.JulMilestone = "Issuance of NTP/Delivery";
                    }
                    break;
                case 8:
                    {
                        Item.MayMilestone = "PR Preparation/Pre-Bid Conference";
                        Item.JunMilestone = "Procurement Activities";
                        Item.JulMilestone = "Procurement Activities";
                        Item.AugMilestone = "Issuance of NTP/Delivery";
                    }
                    break;
                case 9:
                    {
                        Item.JunMilestone = "PR Preparation/Pre-Bid Conference";
                        Item.JulMilestone = "Procurement Activities";
                        Item.AugMilestone = "Procurement Activities";
                        Item.SepMilestone = "Issuance of NTP/Delivery";
                    }
                    break;
                case 10:
                    {
                        Item.JulMilestone = "PR Preparation/Pre-Bid Conference";
                        Item.AugMilestone = "Procurement Activities";
                        Item.SepMilestone = "Procurement Activities";
                        Item.OctMilestone = "Issuance of NTP/Delivery";
                    }
                    break;
                case 11:
                    {
                        Item.AugMilestone = "PR Preparation/Pre-Bid Conference";
                        Item.SepMilestone = "Procurement Activities";
                        Item.OctMilestone = "Procurement Activities";
                        Item.NovMilestone = "Issuance of NTP/Delivery";
                    }
                    break;
                case 12:
                    {
                        Item.SepMilestone = "PR Preparation/Pre-Bid Conference";
                        Item.OctMilestone = "Procurement Activities";
                        Item.NovMilestone = "Procurement Activities";
                        Item.DecMilestone = "Issuance of NTP/Delivery";
                    }
                    break;
            }
            return Item;
        }
        public string GenerateReferenceNo(string FiscalYear, string OfficeCode, string TypeCode)
        {
            string referenceNo = string.Empty;
            string seqNo = (db.PPMPHeader.Where(d => d.FiscalYear == FiscalYear).Count() + 1).ToString();
            seqNo = seqNo.ToString().Length == 3 ? seqNo : seqNo.ToString().Length == 2 ? "0" + seqNo.ToString() : "00" + seqNo.ToString();
            referenceNo = "PPMP-" + TypeCode + "-" + OfficeCode + "-" + seqNo + "-" + FiscalYear;
            return referenceNo;
        }
        public bool SubmitPPMP(string ReferenceNo, string EmailAddress)
        {
            var user = db.UserAccounts.Where(d => d.Email == EmailAddress).FirstOrDefault();
            var ppmp = db.PPMPHeader.Where(d => d.ReferenceNo == ReferenceNo).FirstOrDefault();
            ppmp.Status = "PPMP Submitted";
            ppmp.SubmittedAt = DateTime.Now;
            ppmp.SubmittedBy = user.FKUserInformationReference.FirstName + " " + user.FKUserInformationReference.LastName;

            db.PPMPApprovalWorkflow.Add(new PPMPApprovalWorkflow()
            {
                PPMPId = ppmp.ID,
                Status = "PPMP Submitted",
                UpdatedAt = DateTime.Now,
                Remarks = ppmp.ReferenceNo + " is submitted to the Budget Services Office by " + ppmp.SubmittedBy + " on " + DateTime.Now.ToString("dd MMMM yyyy hh:mm") + ".",
                ActionMadeBy = user.ID,
                ActionMadeByOffice = user.FKUserInformationReference.Office
            });

            if (db.SaveChanges() == 0)
            {
                return false;
            }
            return true;
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                hrdb.Dispose();
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}