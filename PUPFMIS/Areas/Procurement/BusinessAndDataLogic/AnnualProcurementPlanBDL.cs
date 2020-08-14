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
    
    public class AnnualProcurementPlanBL : Controller
    {
        private AnnualProcurementPlanDAL APPDataAccess = new AnnualProcurementPlanDAL();
        private FMISDbContext db = new FMISDbContext();

        public MemoryStream GenerateAPPReport(string ReferenceNo, string LogoPath, string UserEmail)
        {
            Reports reports = new Reports();
            AnnualProcurementPlanVM APPViewModel = APPDataAccess.GetAnnualProcurementPlan(ReferenceNo);
            reports.ReportFilename = ReferenceNo;
            reports.CreateDocument();
            reports.AddDoubleColumnHeader(LogoPath);
            reports.AddColumnHeader(
                new HeaderLine { Content = "", Bold = false, Italic = false, FontSize = 5, ParagraphAlignment = ParagraphAlignment.Left },
                new HeaderLine { Content = "", Bold = true, Italic = false, FontSize = 5, ParagraphAlignment = ParagraphAlignment.Left }
            );
            reports.AddColumnHeader(
                new HeaderLine { Content = "Republic of the Philippines", Bold = false, Italic = false, FontSize = 10, ParagraphAlignment = ParagraphAlignment.Left },
                new HeaderLine { Content = "REFERENCE NO: " + ReferenceNo, Bold = true, Italic = false, FontSize = 8, ParagraphAlignment = ParagraphAlignment.Left }
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

            reports.AddSingleColumnHeader();

            var APPType = APPViewModel.APPType == "Original" ? APPViewModel.ApprovedAt == null ? "INDICATIVE " : String.Empty : APPViewModel.APPType.ToUpper() + " ";

            reports.AddColumnHeader(
                new HeaderLine { Content = APPType + "ANNUAL PROCUREMENT PLAN", Bold = true, Italic = false, FontSize = 10, ParagraphAlignment = ParagraphAlignment.Center }
            );
            reports.AddColumnHeader(
                new HeaderLine { Content = "Fiscal Year " + APPViewModel.FiscalYear.ToString(), Bold = false, Italic = false, FontSize = 8, ParagraphAlignment = ParagraphAlignment.Center }
            );

            reports.AddNewLine();

            var columns = new List<ContentColumn>();
            columns.Add(new ContentColumn(0.75, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
            columns.Add(new ContentColumn(1.90, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
            columns.Add(new ContentColumn(0.75, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
            columns.Add(new ContentColumn(0.75, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
            columns.Add(new ContentColumn(0.60, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
            columns.Add(new ContentColumn(0.60, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
            columns.Add(new ContentColumn(0.60, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
            columns.Add(new ContentColumn(0.60, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
            columns.Add(new ContentColumn(1.00, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
            columns.Add(new ContentColumn(1.15, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
            columns.Add(new ContentColumn(1.15, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
            columns.Add(new ContentColumn(1.15, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
            columns.Add(new ContentColumn(1.50, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
            reports.AddTable(columns, true);

            var rows = new List<ContentCell>();
            rows.Add(new ContentCell("PAP\nCode", 0, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 1));
            rows.Add(new ContentCell("Procurement\nProgram/Project", 1, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 1));
            rows.Add(new ContentCell("PMO\nEnd-User", 2, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 1));
            rows.Add(new ContentCell("Mode of\nProcurement", 3, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 1));
            rows.Add(new ContentCell("Schedule for Each Procurement Activity", 4, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center, 3));
            rows.Add(new ContentCell("Source\nof Funds", 8, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 1));
            rows.Add(new ContentCell("Estimated Budget (PhP)", 9, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center, 2));
            rows.Add(new ContentCell("Remarks\n(brief description of Program/Project)", 12, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 1));
            reports.AddRowContent(rows, 0.25);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("Ads Post\nof IB/REI", 4, 6, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
            rows.Add(new ContentCell("Sub/Open\nof Bids", 5, 6, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
            rows.Add(new ContentCell("Notice of\nAward", 6, 6, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
            rows.Add(new ContentCell("Contract\nSigning", 7, 6, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
            rows.Add(new ContentCell("TOTAL", 9, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
            rows.Add(new ContentCell("MOOE", 10, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
            rows.Add(new ContentCell("CO", 11, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
            reports.AddRowContent(rows, 0.25);

            var accounts = APPViewModel.APPLineItems.GroupBy(d => d.ObjectClassification).Select(d => d.Key).ToList();

            foreach(var acct in accounts)
            {
                columns = new List<ContentColumn>();
                columns.Add(new ContentColumn(12.5, new MigraDoc.DocumentObjectModel.Color(252, 207, 101)));
                reports.AddTable(columns, true);

                rows = new List<ContentCell>();
                rows.Add(new ContentCell(acct, 0, 10, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Left, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                reports.AddRowContent(rows, 0.25);

                columns = new List<ContentColumn>();
                columns.Add(new ContentColumn(0.75));
                columns.Add(new ContentColumn(1.90));
                columns.Add(new ContentColumn(0.75));
                columns.Add(new ContentColumn(0.75));
                columns.Add(new ContentColumn(0.60));
                columns.Add(new ContentColumn(0.60));
                columns.Add(new ContentColumn(0.60));
                columns.Add(new ContentColumn(0.60));
                columns.Add(new ContentColumn(1.00));
                columns.Add(new ContentColumn(1.15));
                columns.Add(new ContentColumn(1.15));
                columns.Add(new ContentColumn(1.15));
                columns.Add(new ContentColumn(1.50));
                reports.AddTable(columns, true);

                var acctItems = APPViewModel.APPLineItems.Where(d => d.ObjectClassification == acct).ToList();
                foreach (var item in acctItems)
                {
                    rows = new List<ContentCell>();
                    rows.Add(new ContentCell(item.PAPCode, 0, 7, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                    rows.Add(new ContentCell(item.ProcurementProject, 1, 7, true, false, ParagraphAlignment.Left, VerticalAlignment.Center));
                    rows.Add(new ContentCell(item.EndUser, 2, 7, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                    var procurementModes = string.Empty;
                    var modes = item.ModeOfProcurement[0].Split("_".ToCharArray(), System.StringSplitOptions.RemoveEmptyEntries).ToArray();
                    for(int i = 0; i < modes.Count(); i++)
                    {
                        var mode = int.Parse(modes[i]);
                        if(i == modes.Count() - 1)
                        {
                            procurementModes += db.ProcurementModes.Find(mode).ModeOfProcurementName;
                        }
                        else
                        {
                            procurementModes += db.ProcurementModes.Find(mode).ModeOfProcurementName + "\n\n";
                        }
                    }

                    rows.Add(new ContentCell(procurementModes, 3, 7, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                    rows.Add(new ContentCell(item.StartMonth + " - " + item.EndMonth, 4, 7, true, false, ParagraphAlignment.Center, VerticalAlignment.Center, 3));
                    rows.Add(new ContentCell(item.FundDescription, 8, 7, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                    rows.Add(new ContentCell(String.Format("{0:C}", item.EstimatedBudget), 9, 8, true, false, ParagraphAlignment.Right, VerticalAlignment.Center));
                    rows.Add(new ContentCell(String.Format("{0:C}", item.MOOE), 10, 8, true, false, ParagraphAlignment.Right, VerticalAlignment.Center));
                    rows.Add(new ContentCell(String.Format("{0:C}", item.CapitalOutlay), 11, 8, true, false, ParagraphAlignment.Right, VerticalAlignment.Center));
                    rows.Add(new ContentCell(item.Remarks, 12, 6, true, false, ParagraphAlignment.Left, VerticalAlignment.Center));
                    reports.AddRowContent(rows, 0.4);
                }

                columns = new List<ContentColumn>();
                columns.Add(new ContentColumn(7.55, new MigraDoc.DocumentObjectModel.Color(74, 132, 249)));
                columns.Add(new ContentColumn(1.15, new MigraDoc.DocumentObjectModel.Color(74, 132, 249)));
                columns.Add(new ContentColumn(1.15, new MigraDoc.DocumentObjectModel.Color(74, 132, 249)));
                columns.Add(new ContentColumn(1.15, new MigraDoc.DocumentObjectModel.Color(74, 132, 249)));
                columns.Add(new ContentColumn(1.50, new MigraDoc.DocumentObjectModel.Color(74, 132, 249)));
                reports.AddTable(columns, true);

                rows = new List<ContentCell>();
                rows.Add(new ContentCell("SUB-TOTAL: ", 0, 9, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Right, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center, 0, 0, false, new Color(255, 255, 255)));
                rows.Add(new ContentCell(String.Format("{0:C}", APPViewModel.APPLineItems.Where(d => d.ObjectClassification == acct).Sum(d => d.EstimatedBudget)), 1, 9, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Right, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center, 0, 0, false, new Color(255, 255, 255)));
                rows.Add(new ContentCell(String.Format("{0:C}", APPViewModel.APPLineItems.Where(d => d.ObjectClassification == acct).Sum(d => d.MOOE)), 2, 9, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Right, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center, 0, 0, false, new Color(255, 255, 255)));
                rows.Add(new ContentCell(String.Format("{0:C}", APPViewModel.APPLineItems.Where(d => d.ObjectClassification == acct).Sum(d => d.CapitalOutlay)), 3, 9, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Right, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center, 0, 0, false, new Color(255, 255, 255)));
                rows.Add(new ContentCell(String.Empty, 4, 9, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Right, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center, 0, 0, false, new Color(255, 255, 255)));
                reports.AddRowContent(rows, 0.25);
            }
            columns = new List<ContentColumn>();
            columns.Add(new ContentColumn(7.55, new MigraDoc.DocumentObjectModel.Color(74, 132, 249)));
            columns.Add(new ContentColumn(1.15, new MigraDoc.DocumentObjectModel.Color(74, 132, 249)));
            columns.Add(new ContentColumn(1.15, new MigraDoc.DocumentObjectModel.Color(74, 132, 249)));
            columns.Add(new ContentColumn(1.15, new MigraDoc.DocumentObjectModel.Color(74, 132, 249)));
            columns.Add(new ContentColumn(1.50, new MigraDoc.DocumentObjectModel.Color(74, 132, 249)));
            reports.AddTable(columns, true);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("GRAND TOTAL: ", 0, 9, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Right, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center, 0, 0, false, new Color(255, 255, 255)));
            rows.Add(new ContentCell(String.Format("{0:C}", APPViewModel.APPLineItems.Sum(d => d.EstimatedBudget)), 1, 9, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Right, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center, 0, 0, false, new Color(255, 255, 255)));
            rows.Add(new ContentCell(String.Format("{0:C}", APPViewModel.APPLineItems.Sum(d => d.MOOE)), 2, 9, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Right, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center, 0, 0, false, new Color(255, 255, 255)));
            rows.Add(new ContentCell(String.Format("{0:C}", APPViewModel.APPLineItems.Sum(d => d.CapitalOutlay)), 3, 9, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Right, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center, 0, 0, false, new Color(255, 255, 255)));
            rows.Add(new ContentCell(String.Empty, 4, 9, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Right, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center, 0, 0, false, new Color(255, 255, 255)));
            reports.AddRowContent(rows, 0.25);

            reports.AddNewLine();
            reports.AddNewLine();

            columns = new List<ContentColumn>();
            columns.Add(new ContentColumn(0.75));
            columns.Add(new ContentColumn(3.00));
            columns.Add(new ContentColumn(1.00));
            columns.Add(new ContentColumn(3.00));
            columns.Add(new ContentColumn(1.00));
            columns.Add(new ContentColumn(3.00));
            columns.Add(new ContentColumn(0.75));
            reports.AddTable(columns, false);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("", 0));
            rows.Add(new ContentCell("Prepared By:", 1, 8, true, false, ParagraphAlignment.Left, VerticalAlignment.Center));
            rows.Add(new ContentCell("", 2, 8, true, false, ParagraphAlignment.Left, VerticalAlignment.Center));
            rows.Add(new ContentCell("Recommending Approval:", 3, 8, true, false, ParagraphAlignment.Left, VerticalAlignment.Center));
            rows.Add(new ContentCell("", 4, 8, true, false, ParagraphAlignment.Left, VerticalAlignment.Center));
            rows.Add(new ContentCell("Approved By:", 5, 8, true, false, ParagraphAlignment.Left, VerticalAlignment.Center));
            rows.Add(new ContentCell("", 6, 8, true, false, ParagraphAlignment.Left, VerticalAlignment.Center));
            reports.AddRowContent(rows, 0.25);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("", 0));
            rows.Add(new ContentCell("", 1, 8, true, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, true));
            rows.Add(new ContentCell("", 2, 8, true, false, ParagraphAlignment.Left, VerticalAlignment.Center));
            rows.Add(new ContentCell("", 3, 8, true, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, true));
            rows.Add(new ContentCell("", 4, 8, true, false, ParagraphAlignment.Left, VerticalAlignment.Center));
            rows.Add(new ContentCell("", 5, 8, true, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, true));
            rows.Add(new ContentCell("", 6));
            reports.AddRowContent(rows, 0.5);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("", 0));
            rows.Add(new ContentCell(APPViewModel.PreparedBy.ToUpper(), 1, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
            rows.Add(new ContentCell("", 2, 8, true, false, ParagraphAlignment.Left, VerticalAlignment.Center));
            rows.Add(new ContentCell(APPViewModel.CertifiedBy.ToUpper(), 3, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
            rows.Add(new ContentCell("", 4, 8, true, false, ParagraphAlignment.Left, VerticalAlignment.Center));
            rows.Add(new ContentCell(APPViewModel.ApprovedBy.ToUpper(), 5, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
            rows.Add(new ContentCell("", 6, 8, true, false, ParagraphAlignment.Left, VerticalAlignment.Center));
            reports.AddRowContent(rows, 0);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("", 0));
            rows.Add(new ContentCell(APPViewModel.PreparedByDesignation + ", " + APPViewModel.PreparedByDepartment, 1, 8, false, true, ParagraphAlignment.Center, VerticalAlignment.Center));
            rows.Add(new ContentCell("", 2, 7, false, true, ParagraphAlignment.Left, VerticalAlignment.Center));
            rows.Add(new ContentCell(APPViewModel.CertifiedByDesignation + ", " + APPViewModel.CertifiedByDepartment, 3, 8, false, true, ParagraphAlignment.Center, VerticalAlignment.Center));
            rows.Add(new ContentCell("", 4, 7, false, true, ParagraphAlignment.Left, VerticalAlignment.Center));
            rows.Add(new ContentCell(APPViewModel.ApprovedByDesignation + ", " + APPViewModel.ApprovedByDepartment, 5, 8, false, true, ParagraphAlignment.Center, VerticalAlignment.Center));
            rows.Add(new ContentCell("", 6, 7, false, true, ParagraphAlignment.Left, VerticalAlignment.Center));
            reports.AddRowContent(rows, 0);

            return reports.GenerateReport();
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                APPDataAccess.Dispose();
            }
            base.Dispose(disposing);
        }
    }

    public class AnnualProcurementPlanDAL : Controller
    {
        private FMISDbContext db = new FMISDbContext();
        private HRISDataAccess hrisDataAccess = new HRISDataAccess();
        private TEMPAccounting abdb = new TEMPAccounting();
        private ABISDataAccess abisDataAccess = new ABISDataAccess();
        private SystemBDL systemBDL = new SystemBDL();
        
        public List<int> GetPPMPFiscalYears()
        {
            return db.PPMPHeader.Where(d => d.Status == "PPMP Evaluated" && d.FKPPMPTypeReference.InventoryCode != "CUOS").GroupBy(d => d.FiscalYear).Select(d => d.Key).ToList();
        }
        public List<int> GetCSEFiscalYears()
        {
            var fiscalYears = db.PPMPHeader.Where(d => d.Status == "PPMP Evaluated" && d.FKPPMPTypeReference.InventoryCode == "CUOS").GroupBy(d => d.FiscalYear).Select(d => d.Key).ToList();
            return fiscalYears;
        }
        public List<int> GetAPPCSEFiscalYears()
        {
            var fiscalYears = db.APPHeader.Where(d => d.APPType == "CSE").GroupBy(d => d.FiscalYear).Select(d => d.Key).ToList();
            return fiscalYears;
        }
        public List<int> GetAPPFiscalYears()
        {
            var fiscalYears = db.APPHeader.Where(d => d.APPType == "CSE").Select(d => d.FiscalYear).Union(db.APPHeader.Where(d => d.APPType != "CSE").Select(d => d.FiscalYear)).GroupBy(d => d).Select(d => d.Key).ToList();
            return fiscalYears;
        }
        public int GetNoOfPPMPsToBeReviewed()
        {
            return db.PPMPHeader.Where(d => d.Status == "PPMP Submitted").Any() ? db.PPMPHeader.Where(d => d.Status == "PPMP Submitted").Count() : 0;
        }
        public int GetNoOfPPMPsToBeEvaluated()
        {
            return db.PPMPHeader.Where(d => d.Status == "PPMP Evaluated").Any() ? db.PPMPHeader.Where(d => d.Status == "PPMP Evaluated").Count() : 0;
        }
        public List<AnnualProcurementPlanHeaderVM> GetAnnualProcurementPlans(int FiscalYear)
        {
            var APPList = db.APPHeader.Where(d => d.FiscalYear == FiscalYear).ToList();
            return APPList.Select(d => new AnnualProcurementPlanHeaderVM
            {
                ReferenceNo = d.ReferenceNo,
                APPType = d.APPType,
                PreparedAt = d.PreparedAt == null ? "For Signature" : ((DateTime)d.PreparedAt).ToString("dd MMMM yyyy"),
                RecommendedAt = d.RecommendedAt == null ? "Pending" : ((DateTime)d.RecommendedAt).ToString("dd MMMM yyyy"),
                ApprovedAt = d.ApprovedAt == null ? "Pending" : ((DateTime)d.ApprovedAt).ToString("dd MMMM yyyy"),
            }).ToList();
        }
        public List<ModeOfProcurement> GetModesOfProcurement()
        {
            return db.ProcurementModes.ToList();
        }
        public List<AnnualProcurementPlanDetailsVM> GetCommonSuppliesItems(int FiscalYear)
        {
            var accounts = abisDataAccess.GetChartOfAccounts();
            var fundSources = abisDataAccess.GetFundSources();

            var appCSEItemsCount = db.APPCSEDetails.Where(d => d.FKAPPHeaderReference.FiscalYear == FiscalYear).Count();
            var ppmpRefList = db.APPInstitutionalItems.Where(d => d.PAPCode.Contains("CUOS") && d.PPMPReferences != null && d.FKAPPHeaderReference.FiscalYear == FiscalYear).GroupBy(d => d.PPMPReferences).Select(d => d.Key).ToList();
            List<string> ppmpCollection = new List<string>();
            foreach(var reference in ppmpRefList)
            {
                var references = reference.Split("_".ToCharArray(), System.StringSplitOptions.RemoveEmptyEntries).ToArray();
                ppmpCollection.AddRange(references);
            }

            var institutionalDBMItemsMOOE = db.ProjectPlanItems.Where(d => d.UnitCost < 15000.00m
                                            && ((!ppmpCollection.Contains(d.FKPPMPReference.ReferenceNo) && d.Status == "Approved") || (!ppmpCollection.Contains(d.FKPPMPReference.ReferenceNo) && d.Status == "Posted to APP"))
                                            && d.FKItemReference.ResponsibilityCenter != null 
                                            && d.FKItemReference.ProcurementSource == ProcurementSources.PS_DBM 
                                            && d.FKItemReference.FKItemTypeReference.FKInventoryTypeReference.InventoryCode == "CUOS"
                                            && d.FKPPMPReference.FiscalYear == FiscalYear)
                                            .GroupBy(d => new { d.FundSource, d.FKItemReference.FKItemTypeReference.AccountClass, d.FKItemReference.ResponsibilityCenter })
                                            .Select(d => new
                                            {
                                                UACS = d.Key.AccountClass,
                                                ProjectCode = "1",
                                                ProjectName = "Procurement of Common-Use Office Supplies Available at DBM-PS",
                                                EndUser = d.Key.ResponsibilityCenter,
                                                StartMonth = d.Max(x => x.FKProjectReference.ProjectMonthStart),
                                                EstimatedBudget = d.Sum(x => x.PPMPEstimatedBudget),
                                                FundSource = d.Key.FundSource,
                                                Remarks = "Common-use supplies not available in the Procurement System - Department of Budget and Management (PS-DBM), to be sourced from private suppliers."
                                            }).FirstOrDefault();

            var institutionalNonDBMItemsMOOE = db.ProjectPlanItems.Where(d => d.UnitCost < 15000.00m && (d.Status == "Approved" || d.Status == "Posted to APP") && d.FKItemReference.ResponsibilityCenter != null && d.FKItemReference.ProcurementSource == ProcurementSources.Non_DBM && d.FKItemReference.FKItemTypeReference.FKInventoryTypeReference.InventoryCode == "CUOS")
                                .GroupBy(d => new { d.FundSource, d.FKItemReference.FKItemTypeReference.AccountClass, d.FKItemReference.ResponsibilityCenter })
                                .Select(d => new
                                {
                                    UACS = d.Key.AccountClass,
                                    ProjectCode = "1",
                                    ProjectName = "Procurement of Common-Use Office Supplies Available not available at DBM-PS",
                                    EndUser = d.Key.ResponsibilityCenter,
                                    StartMonth = d.Max(x => x.FKProjectReference.ProjectMonthStart),
                                    EstimatedBudget = d.Sum(x => x.PPMPEstimatedBudget),
                                    FundSource = d.Key.FundSource,
                                    Remarks = "Common-use supplies available in the Procurement System - Department of Budget and Management (PS-DBM)."
                                }).FirstOrDefault();

            var items = new List<AnnualProcurementPlanDetailsVM>();
            SystemBDL sysBDL = new SystemBDL();
            if (institutionalDBMItemsMOOE != null)
            {
                var ppmpList = db.ProjectPlanItems
                               .Where(d => d.FKItemReference.ProcurementSource == ProcurementSources.PS_DBM
                                           && d.FKItemReference.FKItemTypeReference.FKInventoryTypeReference.InventoryCode == "CUOS"
                                           && (d.APPReference == null || d.Status == "Posted to APP"))
                               .Select(d => d.FKPPMPReference.ReferenceNo)
                               .GroupBy(d => d).ToList();

                var projectList = db.ProjectPlanItems
                               .Where(d => d.FKItemReference.ProcurementSource == ProcurementSources.PS_DBM
                                           && d.FKItemReference.FKItemTypeReference.FKInventoryTypeReference.InventoryCode == "CUOS"
                                           && (d.APPReference == null || d.Status == "Posted to APP"))
                               .Select(d => d.FKProjectReference.ProjectCode)
                               .GroupBy(d => d).ToList();

                string ppmps = string.Empty;
                string projects = string.Empty;
                for(int i = 0; i < ppmpList.Count; i++)
                {
                    if(i == ppmpList.Count - 1)
                    {
                        ppmps += ppmpList[i].Key;
                    }
                    else
                    {
                        ppmps += ppmpList[i].Key + "_";
                    }
                }

                for (int i = 0; i < projectList.Count; i++)
                {
                    if (i == projectList.Count - 1)
                    {
                        projects += projectList[i].Key;
                    }
                    else
                    {
                        projects += projectList[i].Key + "_";
                    }
                }

                items.Add(new AnnualProcurementPlanDetailsVM
                {
                    UACS = institutionalDBMItemsMOOE.UACS,
                    ObjectClassification = abisDataAccess.GetDetailedChartOfAccounts().Where(d => d.UACS_Code == institutionalDBMItemsMOOE.UACS).FirstOrDefault().AcctName,
                    PAPCode = institutionalDBMItemsMOOE.ProjectCode,
                    ProcurementProject = institutionalDBMItemsMOOE.ProjectName,
                    ModeOfProcurement = new List<string>() { "10" },
                    EndUser = institutionalDBMItemsMOOE.EndUser,
                    Month = institutionalDBMItemsMOOE.StartMonth,
                    StartMonth = sysBDL.GetMonthName(10) + ", " + (FiscalYear - 1).ToString(),
                    EndMonth = sysBDL.GetMonthName(institutionalDBMItemsMOOE.StartMonth) + ", " + FiscalYear.ToString(),
                    FundCluster = institutionalDBMItemsMOOE.FundSource,
                    FundDescription = abisDataAccess.GetFundSources(institutionalDBMItemsMOOE.FundSource).FUND_DESC,
                    MOOE = institutionalDBMItemsMOOE.EstimatedBudget,
                    CapitalOutlay = 0.00m,
                    EstimatedBudget = institutionalDBMItemsMOOE.EstimatedBudget,
                    Remarks = institutionalDBMItemsMOOE.Remarks + "\n\nReferences:\n" + ppmps.Replace("_","\n"),
                    PPMPReferences = ppmps,
                    ProjectReferences = projects,
                    APPItemType = "Institutional"
                });
            }

            if (institutionalNonDBMItemsMOOE != null)
            {
                var ppmpList = db.ProjectPlanItems
                               .Where(d => d.FKItemReference.ProcurementSource == ProcurementSources.Non_DBM 
                                           && d.FKItemReference.FKItemTypeReference.FKInventoryTypeReference.InventoryCode == "CUOS"
                                           && (d.APPReference == null || d.Status == "Posted to APP"))
                               .Select(d => d.FKPPMPReference.ReferenceNo)
                               .GroupBy(d => d).ToList();

                var projectList = db.ProjectPlanItems
                               .Where(d => d.FKItemReference.ProcurementSource == ProcurementSources.Non_DBM 
                                           && d.FKItemReference.FKItemTypeReference.FKInventoryTypeReference.InventoryCode == "CUOS"
                                           && (d.APPReference == null || d.Status == "Posted to APP"))
                               .Select(d => d.FKProjectReference.ProjectCode)
                               .GroupBy(d => d).ToList();

                string ppmps = string.Empty;
                string projects = string.Empty;
                for (int i = 0; i < ppmpList.Count; i++)
                {
                    if (i == ppmpList.Count - 1)
                    {
                        ppmps += ppmpList[i].Key;
                    }
                    else
                    {
                        ppmps += ppmpList[i].Key + "_";
                    }
                }

                for (int i = 0; i < projectList.Count; i++)
                {
                    if (i == projectList.Count - 1)
                    {
                        projects += projectList[i].Key;
                    }
                    else
                    {
                        projects += projectList[i].Key + "_";
                    }
                }
                items.Add(new AnnualProcurementPlanDetailsVM
                {
                    UACS = institutionalNonDBMItemsMOOE.UACS,
                    ObjectClassification = abisDataAccess.GetDetailedChartOfAccounts().Where(d => d.UACS_Code == institutionalNonDBMItemsMOOE.UACS).FirstOrDefault().AcctName,
                    PAPCode = "1",
                    ProcurementProject = institutionalNonDBMItemsMOOE.ProjectName,
                    ModeOfProcurement = new List<string>() { "1", "14" },
                    EndUser = institutionalNonDBMItemsMOOE.EndUser,
                    Month = institutionalDBMItemsMOOE.StartMonth,
                    StartMonth = sysBDL.GetMonthName(10) + (FiscalYear - 1).ToString(),
                    EndMonth = sysBDL.GetMonthName(institutionalDBMItemsMOOE.StartMonth) + ", " + FiscalYear.ToString(),
                    FundCluster = institutionalDBMItemsMOOE.FundSource,
                    FundDescription = abisDataAccess.GetFundSources(institutionalNonDBMItemsMOOE.FundSource).FUND_DESC,
                    MOOE = institutionalNonDBMItemsMOOE.EstimatedBudget,
                    CapitalOutlay = 0.00m,
                    EstimatedBudget = institutionalNonDBMItemsMOOE.EstimatedBudget,
                    Remarks = institutionalNonDBMItemsMOOE.Remarks,
                    PPMPReferences = ppmps,
                    ProjectReferences = projects,
                    APPItemType = "Institutional"
                });
            }

            if (institutionalDBMItemsMOOE != null)
            {
                items.Add(new AnnualProcurementPlanDetailsVM
                {
                    UACS = institutionalDBMItemsMOOE.UACS,
                    ObjectClassification = abisDataAccess.GetDetailedChartOfAccounts().Where(d => d.UACS_Code == institutionalDBMItemsMOOE.UACS).FirstOrDefault().AcctName,
                    PAPCode = institutionalDBMItemsMOOE.ProjectCode,
                    ProcurementProject = "Provision for Procurement of Common-Use Supplies for Forseeable Emergencies.",
                    ModeOfProcurement = new List<string>() { "7" },
                    EndUser = institutionalDBMItemsMOOE.EndUser,
                    Month = 1,
                    StartMonth = "January, " + FiscalYear.ToString(),
                    EndMonth = "October, " + FiscalYear.ToString(),
                    FundCluster = institutionalDBMItemsMOOE.FundSource,
                    FundDescription = abisDataAccess.GetFundSources(institutionalDBMItemsMOOE.FundSource).FUND_DESC,
                    MOOE = institutionalDBMItemsMOOE.EstimatedBudget * 0.04m,
                    CapitalOutlay = 0.00m,
                    EstimatedBudget = institutionalDBMItemsMOOE.EstimatedBudget * 0.04m,
                    Remarks = "Provision for unforeseen contingency requiring immediate purchase under Section 51.1(a) of Annex H.",
                    APPItemType = "Institutional"
                });
            }

            return items;
        }
        public List<AnnualProcurementPlanDetailsVM> GetInstitutionalItems(int FiscalYear)
        {
            var accounts = abisDataAccess.GetChartOfAccounts();
            var fundSources = abisDataAccess.GetFundSources();
            var institutionalItemsMOOE = db.ProjectPlanItems.Where(d => d.UnitCost < 15000.00m 
                                            && d.Status == "Approved"
                                            && d.FKItemReference.ResponsibilityCenter != null
                                            && d.FKItemReference.FKItemTypeReference.FKInventoryTypeReference.InventoryCode != "CUOS"
                                            && d.FKProjectReference.ProjectStatus != "New Project")
                                            .GroupBy(d => new { d.FKItemReference.FKItemTypeReference.AccountClass, d.FKItemReference.FKItemTypeReference.FKInventoryTypeReference.ID, d.FKItemReference.ResponsibilityCenter, d.FundSource })
                                            .Select(d => new
                                            {
                                                UACS = d.Key.AccountClass,
                                                PAPCode = d.Key.ID,
                                                EndUser = d.Key.ResponsibilityCenter,
                                                StartMonth = d.Max(x => x.FKProjectReference.ProjectMonthStart),
                                                EstimatedBudget = d.Sum(x => x.PPMPEstimatedBudget),
                                                FundSource = d.Key.FundSource
                                            }).ToList();

            var institutionalServicesMOOE = db.ProjectPlanServices.Where(d => d.UnitCost < 15000.00m 
                                            && d.Status == "Approved"
                                            && d.FKItemReference.ResponsibilityCenter != null
                                            && d.FKItemReference.FKItemTypeReference.FKInventoryTypeReference.InventoryCode != "CUOS"
                                            && d.FKProjectReference.ProjectStatus != "New Project")
                                            .GroupBy(d => new { d.FKItemReference.FKItemTypeReference.AccountClass, d.FKItemReference.FKItemTypeReference.FKInventoryTypeReference.ID, d.FKItemReference.ResponsibilityCenter, d.FundSource })
                                            .Select(d => new
                                            {
                                                UACS = d.Key.AccountClass,
                                                PAPCode = d.Key.ID,
                                                EndUser = d.Key.ResponsibilityCenter,
                                                StartMonth = d.Max(x => x.FKProjectReference.ProjectMonthStart),
                                                EstimatedBudget = d.Sum(x => x.PPMPEstimatedBudget),
                                                FundSource = d.Key.FundSource
                                            }).ToList();

            var institutionalMOOE = institutionalItemsMOOE.Union(institutionalServicesMOOE).ToList();

            var institutionalItemsCO = db.ProjectPlanItems.Where(d => d.UnitCost >= 15000.00m 
                                            && d.Status == "Approved"
                                            && d.FKItemReference.ResponsibilityCenter != null
                                            && d.FKItemReference.FKItemTypeReference.FKInventoryTypeReference.InventoryCode != "CUOS"
                                            && d.FKProjectReference.ProjectStatus != "New Project")
                                            .GroupBy(d => new { d.FKItemReference.FKItemTypeReference.AccountClass, d.FKItemReference.FKItemTypeReference.FKInventoryTypeReference.ID, d.FKItemReference.ResponsibilityCenter, d.FundSource })
                                            .Select(d => new
                                            {
                                                UACS = d.Key.AccountClass,
                                                PAPCode = d.Key.ID,
                                                EndUser = d.Key.ResponsibilityCenter,
                                                StartMonth = d.Max(x => x.FKProjectReference.ProjectMonthStart),
                                                EstimatedBudget = d.Sum(x => x.PPMPEstimatedBudget),
                                                FundSource = d.Key.FundSource
                                            }).ToList();

            var institutionalServicesCO = db.ProjectPlanServices.Where(d => d.UnitCost >= 15000.00m 
                                            && d.Status == "Approved"
                                            && d.FKItemReference.ResponsibilityCenter != null
                                            && d.FKItemReference.FKItemTypeReference.FKInventoryTypeReference.InventoryCode != "CUOS"
                                            && d.FKProjectReference.ProjectStatus != "New Project")
                                            .GroupBy(d => new { d.FKItemReference.FKItemTypeReference.AccountClass, d.FKItemReference.FKItemTypeReference.FKInventoryTypeReference.ID, d.FKItemReference.ResponsibilityCenter, d.FundSource })
                                            .Select(d => new
                                            {
                                                UACS = d.Key.AccountClass,
                                                PAPCode = d.Key.ID,
                                                EndUser = d.Key.ResponsibilityCenter,
                                                StartMonth = d.Max(x => x.FKProjectReference.ProjectMonthStart),
                                                EstimatedBudget = d.Sum(x => x.PPMPEstimatedBudget),
                                                FundSource = d.Key.FundSource
                                            }).ToList();

            var institutionalCO = institutionalItemsCO.Union(institutionalServicesCO).ToList();

            var items = new List<AnnualProcurementPlanDetailsVM>();
            SystemBDL sysBDL = new SystemBDL();
            foreach (var item in institutionalMOOE)
            {
                var ppmpItemList = db.ProjectPlanItems
                               .Where(d => d.FKItemReference.FKItemTypeReference.AccountClass == item.UACS && d.UnitCost < 15000.00m && d.FundSource == item.FundSource && d.APPReference == null)
                               .Select(d => d.FKPPMPReference.ReferenceNo)
                               .GroupBy(d => d).ToList();

                var projectItemList = db.ProjectPlanItems
                               .Where(d => d.FKItemReference.FKItemTypeReference.AccountClass == item.UACS && d.UnitCost < 15000.00m && d.FundSource == item.FundSource && d.APPReference == null)
                               .Select(d => d.FKProjectReference.ProjectCode)
                               .GroupBy(d => d).ToList();

                var ppmpServiceList = db.ProjectPlanServices
               .Where(d => d.FKItemReference.FKItemTypeReference.AccountClass == item.UACS && d.UnitCost < 15000.00m && d.FundSource == item.FundSource && d.APPReference == null)
               .Select(d => d.FKPPMPReference.ReferenceNo)
               .GroupBy(d => d).ToList();

                var projectServiceList = db.ProjectPlanServices
                               .Where(d => d.FKItemReference.FKItemTypeReference.AccountClass == item.UACS && d.UnitCost < 15000.00m && d.FundSource == item.FundSource && d.APPReference == null)
                               .Select(d => d.FKProjectReference.ProjectCode)
                               .GroupBy(d => d).ToList();
                var ppmpList = ppmpItemList.Union(ppmpServiceList).ToList();
                var projectList = projectItemList.Union(projectServiceList).ToList();

                string ppmps = string.Empty;
                string projects = string.Empty;
                for (int i = 0; i < ppmpList.Count; i++)
                {
                    if (i == ppmpList.Count - 1)
                    {
                        ppmps += ppmpList[i].Key;
                    }
                    else
                    {
                        ppmps += ppmpList[i].Key + "_";
                    }
                }

                for (int i = 0; i < projectList.Count; i++)
                {
                    if (i == projectList.Count - 1)
                    {
                        projects += projectList[i].Key;
                    }
                    else
                    {
                        projects += projectList[i].Key + "_";
                    }
                }
                items.Add(new AnnualProcurementPlanDetailsVM
                {
                    UACS = item.UACS,
                    ObjectClassification = abisDataAccess.GetDetailedChartOfAccounts().Where(d => d.UACS_Code == item.UACS).FirstOrDefault().AcctName,
                    PAPCode = item.PAPCode.ToString(),
                    ProcurementProject = abisDataAccess.GetChartOfAccounts(item.UACS).AcctName,
                    ModeOfProcurement = new List<string>() { "1", "14" },
                    EndUser = item.EndUser,
                    Month = item.StartMonth,
                    StartMonth = item.StartMonth <= 5 ? "Fourth Quarter, " + (FiscalYear - 1) : sysBDL.GetMonthName(item.StartMonth - 5) + ", " + FiscalYear.ToString(),
                    EndMonth = sysBDL.GetMonthName(item.StartMonth) + ", " + FiscalYear.ToString(),
                    FundCluster = item.FundSource,
                    FundDescription = abisDataAccess.GetFundSources(item.FundSource).FUND_DESC,
                    MOOE = item.EstimatedBudget,
                    CapitalOutlay = 0.00m,
                    EstimatedBudget = item.EstimatedBudget,
                    Remarks = "References:\n\n" + ppmps.Replace("_", "\n"),
                    PPMPReferences = ppmps,
                    ProjectReferences = projects,
                    APPItemType = "Institutional"
                });
            }

            foreach (var item in institutionalCO)
            {
                var ppmpItemList = db.ProjectPlanItems
                               .Where(d => d.FKItemReference.FKItemTypeReference.AccountClass == item.UACS && d.UnitCost >= 15000.00m && d.FundSource == item.FundSource && d.APPReference == null)
                               .Select(d => d.FKPPMPReference.ReferenceNo)
                               .GroupBy(d => d).ToList();

                var projectItemList = db.ProjectPlanItems
                               .Where(d => d.FKItemReference.FKItemTypeReference.AccountClass == item.UACS && d.UnitCost >= 15000.00m && d.FundSource == item.FundSource && d.APPReference == null)
                               .Select(d => d.FKProjectReference.ProjectCode)
                               .GroupBy(d => d).ToList();

                var ppmpServiceList = db.ProjectPlanServices
               .Where(d => d.FKItemReference.FKItemTypeReference.AccountClass == item.UACS && d.UnitCost >= 15000.00m && d.FundSource == item.FundSource && d.APPReference == null)
               .Select(d => d.FKPPMPReference.ReferenceNo)
               .GroupBy(d => d).ToList();

                var projectServiceList = db.ProjectPlanServices
                               .Where(d => d.FKItemReference.FKItemTypeReference.AccountClass == item.UACS && d.UnitCost >= 15000.00m && d.FundSource == item.FundSource && d.APPReference == null)
                               .Select(d => d.FKProjectReference.ProjectCode)
                               .GroupBy(d => d).ToList();
                var ppmpList = ppmpItemList.Union(ppmpServiceList).ToList();
                var projectList = projectItemList.Union(projectServiceList).ToList();

                string ppmps = string.Empty;
                string projects = string.Empty;
                for (int i = 0; i < ppmpList.Count; i++)
                {
                    if (i == ppmpList.Count - 1)
                    {
                        ppmps += ppmpList[i].Key;
                    }
                    else
                    {
                        ppmps += ppmpList[i].Key + "_";
                    }
                }

                for (int i = 0; i < projectList.Count; i++)
                {
                    if (i == projectList.Count - 1)
                    {
                        projects += projectList[i].Key;
                    }
                    else
                    {
                        projects += projectList[i].Key + "_";
                    }
                }
                items.Add(new AnnualProcurementPlanDetailsVM
                {
                    UACS = item.UACS,
                    ObjectClassification = abisDataAccess.GetDetailedChartOfAccounts().Where(d => d.UACS_Code == item.UACS).FirstOrDefault().AcctName,
                    PAPCode = item.PAPCode.ToString(),
                    ProcurementProject = abisDataAccess.GetChartOfAccounts(item.UACS).AcctName,
                    ModeOfProcurement = new List<string>() { "1", "14" },
                    EndUser = item.EndUser,
                    Month = item.StartMonth,
                    StartMonth = item.StartMonth <= 5 ? "Fourth Quarter, " + (FiscalYear - 1) : sysBDL.GetMonthName(item.StartMonth - 5) + ", " + FiscalYear.ToString(),
                    EndMonth = sysBDL.GetMonthName(item.StartMonth) + ", " + FiscalYear.ToString(),
                    FundCluster = item.FundSource,
                    FundDescription = abisDataAccess.GetFundSources(item.FundSource).FUND_DESC,
                    MOOE = 0.00m,
                    CapitalOutlay = item.EstimatedBudget,
                    EstimatedBudget = item.EstimatedBudget,
                    Remarks = "References:\n\n" + ppmps.Replace("_", "\n"),
                    PPMPReferences = ppmps,
                    ProjectReferences = projects,
                    APPItemType = "Institutional"
                });
            }

            return items;
        }
        public List<AnnualProcurementPlanDetailsVM> GetNonInstitutionalItems(int FiscalYear)
        {
            var accounts = abisDataAccess.GetChartOfAccounts();
            var fundSources = abisDataAccess.GetFundSources();
            var nonInstitutionalItemsMOOE = db.ProjectPlanItems.Where(d => d.UnitCost < 15000.00m 
                                            && d.Status == "Approved" 
                                            && d.FKItemReference.ResponsibilityCenter == null 
                                            && d.FKItemReference.FKItemTypeReference.FKInventoryTypeReference.InventoryCode != "CUOS")
                                            .GroupBy(d => new { d.FKItemReference.FKItemTypeReference.AccountClass, d.FKProjectReference.ProjectCode, d.FKProjectReference.ProjectName, d.FKPPMPReference.Department, d.FundSource, d.FKPPMPReference.ReferenceNo })
                                            .Select(d => new
                                            {
                                                UACS = d.Key.AccountClass,
                                                ProjectCode = d.Key.ProjectCode,
                                                ProjectName = d.Key.ProjectName,
                                                EndUser = d.Key.Department,
                                                StartMonth = d.Max(x => x.FKProjectReference.ProjectMonthStart),
                                                EstimatedBudget = d.Sum(x => x.PPMPEstimatedBudget),
                                                FundSource = d.Key.FundSource,
                                                Remarks = d.Key.ProjectName + "\n\nReference:\n" + d.Key.ReferenceNo,
                                            }).ToList();

            var nonInstitutionalServicesMOOE = db.ProjectPlanServices.Where(d => d.UnitCost < 15000.00m 
                                               && d.Status == "Approved" 
                                               && d.FKItemReference.ResponsibilityCenter == null 
                                               && d.FKItemReference.FKItemTypeReference.FKInventoryTypeReference.InventoryCode != "CUOS")
                                            .GroupBy(d => new { d.FKItemReference.FKItemTypeReference.AccountClass, d.FKProjectReference.ProjectCode, d.FKProjectReference.ProjectName, d.FKPPMPReference.Department, d.FundSource, d.FKPPMPReference.ReferenceNo })
                                            .Select(d => new
                                            {
                                                UACS = d.Key.AccountClass,
                                                ProjectCode = d.Key.ProjectCode,
                                                ProjectName = d.Key.ProjectName,
                                                EndUser = d.Key.Department,
                                                StartMonth = d.Max(x => x.FKProjectReference.ProjectMonthStart),
                                                EstimatedBudget = d.Sum(x => x.PPMPEstimatedBudget),
                                                FundSource = d.Key.FundSource,
                                                Remarks = d.Key.ProjectName + "\n\nReference:\n" + d.Key.ReferenceNo,
                                            }).ToList();

            var nonInstitutionalMOOE = nonInstitutionalItemsMOOE.Union(nonInstitutionalServicesMOOE).ToList();

            var nonInstitutionalItemsCO = db.ProjectPlanItems.Where(d => d.UnitCost >= 15000.00m 
                                          && d.Status == "Approved" 
                                          && d.FKItemReference.ResponsibilityCenter == null 
                                          && d.FKItemReference.FKItemTypeReference.FKInventoryTypeReference.InventoryCode != "CUOS")
                                .GroupBy(d => new { d.FKItemReference.FKItemTypeReference.AccountClass, d.FKProjectReference.ProjectCode, d.FKProjectReference.ProjectName, d.FKPPMPReference.Department, d.FundSource, d.FKPPMPReference.ReferenceNo })
                                .Select(d => new
                                {
                                    UACS = d.Key.AccountClass,
                                    ProjectCode = d.Key.ProjectCode,
                                    ProjectName = d.Key.ProjectName,
                                    EndUser = d.Key.Department,
                                    StartMonth = d.Max(x => x.FKProjectReference.ProjectMonthStart),
                                    EstimatedBudget = d.Sum(x => x.PPMPEstimatedBudget),
                                    FundSource = d.Key.FundSource,
                                    Remarks = d.Key.ProjectName + "\n\nReference:\n" + d.Key.ReferenceNo,
                                }).ToList();

            var nonInstitutionalServicesCO = db.ProjectPlanServices.Where(d => d.UnitCost >= 15000.00m 
                                             && d.Status == "Approved" 
                                             && d.FKItemReference.ResponsibilityCenter == null 
                                             && d.FKItemReference.FKItemTypeReference.FKInventoryTypeReference.InventoryCode != "CUOS")
                                            .GroupBy(d => new { d.FKItemReference.FKItemTypeReference.AccountClass, d.FKProjectReference.ProjectCode, d.FKProjectReference.ProjectName, d.FKPPMPReference.Department, d.FundSource, d.FKPPMPReference.ReferenceNo })
                                            .Select(d => new
                                            {
                                                UACS = d.Key.AccountClass,
                                                ProjectCode = d.Key.ProjectCode,
                                                ProjectName = d.Key.ProjectName,
                                                EndUser = d.Key.Department,
                                                StartMonth = d.Max(x => x.FKProjectReference.ProjectMonthStart),
                                                EstimatedBudget = d.Sum(x => x.PPMPEstimatedBudget),
                                                FundSource = d.Key.FundSource,
                                                Remarks = d.Key.ProjectName + "\n\nReference:\n" + d.Key.ReferenceNo,
                                            }).ToList();

            var nonInstitutionalCO = nonInstitutionalItemsCO.Union(nonInstitutionalServicesCO).ToList();

            var items = new List<AnnualProcurementPlanDetailsVM>();
            SystemBDL sysBDL = new SystemBDL();
            foreach (var item in nonInstitutionalMOOE)
            {
                items.Add(new AnnualProcurementPlanDetailsVM
                {
                    UACS = item.UACS,
                    ObjectClassification = abisDataAccess.GetDetailedChartOfAccounts().Where(d => d.UACS_Code == item.UACS).FirstOrDefault().AcctName,
                    PAPCode = item.ProjectCode,
                    ProcurementProject = item.ProjectName,
                    ModeOfProcurement = new List<string>() { "1" },
                    EndUser = item.EndUser,
                    Month = item.StartMonth,
                    StartMonth = item.StartMonth <= 5 ? "Fourth Quarter, " + (FiscalYear - 1) : sysBDL.GetMonthName(item.StartMonth - 5) + ", " + FiscalYear.ToString(),
                    EndMonth = sysBDL.GetMonthName(item.StartMonth) + ", " + FiscalYear.ToString(),
                    FundCluster = item.FundSource,
                    MOOE = item.EstimatedBudget,
                    CapitalOutlay = 0.00m,
                    EstimatedBudget = item.EstimatedBudget,
                    Remarks = item.Remarks,
                    APPItemType = "Project"
                });
            }

            foreach (var item in nonInstitutionalCO)
            {
                items.Add(new AnnualProcurementPlanDetailsVM
                {
                    UACS = item.UACS,
                    ObjectClassification = abisDataAccess.GetDetailedChartOfAccounts().Where(d => d.UACS_Code == item.UACS).FirstOrDefault().AcctName,
                    PAPCode = item.ProjectCode,
                    ProcurementProject = item.ProjectName,
                    ModeOfProcurement = new List<string>() { "1" },
                    EndUser = item.EndUser,
                    Month = item.StartMonth,
                    StartMonth = item.StartMonth <= 5 ? "Fourth Quarter, " + (FiscalYear - 1) : sysBDL.GetMonthName(item.StartMonth - 5) + ", " + FiscalYear.ToString(),
                    EndMonth = sysBDL.GetMonthName(item.StartMonth) + ", " + FiscalYear.ToString(),
                    FundCluster = item.FundSource,
                    MOOE = 0.00m,
                    CapitalOutlay = item.EstimatedBudget,
                    EstimatedBudget = item.EstimatedBudget,
                    Remarks = item.Remarks,
                    APPItemType = "Project"
                });
            }

            return items;
        }
        public AnnualProcurementPlanVM GetAnnualProcurementPlan(int FiscalYear)
        {
            AgencyDetails agencyDetails = db.AgencyDetails.First();

            var hope = hrisDataAccess.GetFullDepartmentDetails(agencyDetails.HOPEReference);
            var property = hrisDataAccess.GetFullDepartmentDetails(agencyDetails.PropertyOfficeReference);
            var accounting = hrisDataAccess.GetFullDepartmentDetails(agencyDetails.AccountingOfficeReference);
            var procurement = hrisDataAccess.GetFullDepartmentDetails(agencyDetails.ProcurementOfficeReference);
            var bac = hrisDataAccess.GetFullDepartmentDetails(agencyDetails.BACOfficeReference);

            var appItems = new List<AnnualProcurementPlanDetailsVM>();
            appItems.AddRange(GetCommonSuppliesItems(FiscalYear));
            appItems.AddRange(GetInstitutionalItems(FiscalYear));
            appItems.AddRange(GetNonInstitutionalItems(FiscalYear));

            var institutionalItems = appItems.Where(d => d.APPItemType == "Institutional").OrderBy(d => d.PAPCode).ToList();
            var itemNo = db.APPInstitutionalItems.Count() + 1;
            for(int i = 0; i < institutionalItems.Count; i++)
            {
                institutionalItems[i].PAPCode = GeneratePAPCode(FiscalYear, itemNo, int.Parse(institutionalItems[i].PAPCode.Substring(0, 1)));
                itemNo++;
            }

            appItems = appItems.OrderBy(d => d.UACS).ToList();

            return new AnnualProcurementPlanVM
            {
                AccountCode = agencyDetails.AccountCode,
                AgencyName = agencyDetails.AgencyName,
                Address = agencyDetails.Address,
                Region = agencyDetails.Region,
                OrganizationType = agencyDetails.OrganizationType,
                ApprovedBy = hope.DepartmentHead + ", " + hope.DepartmentHeadDesignation + " / " + hope.Department,
                PreparedBy = property.DepartmentHead + ", " + property.DepartmentHeadDesignation + " / " + property.Department,
                CertifiedBy = accounting.DepartmentHead + ", " + accounting.DepartmentHeadDesignation + " / " + accounting.Department,
                ProcurementOfficer = procurement.DepartmentHead + ", " + procurement.DepartmentHeadDesignation + " / " + procurement.Department,
                BACSecretariat = bac.SectionCode == null ? bac.DepartmentHead + ", " + bac.DepartmentHeadDesignation + " / " + bac.Department : bac.SectionHead + ", " + bac.SectionHeadDesignation + " / " + bac.Section,
                APPLineItems = appItems
            };
        }
        public AnnualProcurementPlanVM GetAnnualProcurementPlan(string ReferenceNo)
        {
            AgencyDetails agencyDetails = db.AgencyDetails.First();
            var APPHeader = db.APPHeader.Where(d => d.ReferenceNo == ReferenceNo).FirstOrDefault();
            var APPDetails = new List<AnnualProcurementPlanDetailsVM>();
            var InstitutionalItems = db.APPInstitutionalItems.Where(d => d.FKAPPHeaderReference.ReferenceNo == ReferenceNo).ToList();
            var ProjectItems = db.APPProjectItems.Where(d => d.FKAPPHeaderReference.ReferenceNo == ReferenceNo).ToList();

            foreach(var item in InstitutionalItems)
            {
                APPDetails.Add(new AnnualProcurementPlanDetailsVM
                {
                    UACS = item.ObjectClassification,
                    ObjectClassification = abisDataAccess.GetDetailedChartOfAccounts().Where(d => d.UACS_Code == item.ObjectClassification).FirstOrDefault().AcctName,
                    PAPCode = item.PAPCode,
                    ProcurementProject = item.ProcurementProgram,
                    ModeOfProcurement = new List<string>() { item.ModeOfProcurementReference },
                    EndUser = item.EndUser,
                    Month = item.Month,
                    StartMonth = item.StartMonth,
                    EndMonth = item.EndMonth,
                    FundCluster = item.FundSourceReference,
                    FundDescription = abisDataAccess.GetFundSources(item.FundSourceReference).FUND_DESC,
                    MOOE = item.MOOEAmount,
                    CapitalOutlay = item.COAmount,
                    EstimatedBudget = item.Total,
                    Remarks = item.Remarks
                });
            }

            foreach (var item in ProjectItems)
            {
                APPDetails.Add(new AnnualProcurementPlanDetailsVM
                {
                    UACS = item.ObjectClassification,
                    ObjectClassification = abisDataAccess.GetDetailedChartOfAccounts().Where(d => d.UACS_Code == item.ObjectClassification).FirstOrDefault().AcctName,
                    PAPCode = item.PAPCode,
                    ProcurementProject = item.ProcurementProgram,
                    ModeOfProcurement = new List<string>() { item.ModeOfProcurementReference },
                    EndUser = item.EndUser,
                    Month = item.Month,
                    StartMonth = item.StartMonth,
                    EndMonth = item.EndMonth,
                    FundCluster = item.FundSourceReference,
                    FundDescription = abisDataAccess.GetFundSources(item.FundSourceReference).FUND_DESC,
                    MOOE = item.MOOEAmount,
                    CapitalOutlay = item.COAmount,
                    EstimatedBudget = item.Total,
                    Remarks = item.Remarks
                });
            }

            var hope = hrisDataAccess.GetFullDepartmentDetails(APPHeader.ApprovedByDepartmentCode);
            var property = hrisDataAccess.GetFullDepartmentDetails(agencyDetails.PropertyOfficeReference);
            var accounting = hrisDataAccess.GetFullDepartmentDetails(agencyDetails.AccountingOfficeReference);
            var procurement = hrisDataAccess.GetFullDepartmentDetails(APPHeader.PreparedByDepartmentCode);
            var bac = hrisDataAccess.GetFullDepartmentDetails(APPHeader.RecommendingApprovalDepartmentCode);

            return new AnnualProcurementPlanVM
            {
                APPType = APPHeader.APPType,
                ReferenceNo = APPHeader.ReferenceNo,
                FiscalYear = APPHeader.FiscalYear,
                AccountCode = agencyDetails.AccountCode,
                AgencyName = agencyDetails.AgencyName,
                Address = agencyDetails.Address,
                Region = agencyDetails.Region,
                OrganizationType = agencyDetails.OrganizationType,
                ApprovedBy = APPHeader.ApprovedBy + ", " + APPHeader.ApprovedByDesignation + " / " + hrisDataAccess.GetDepartmentDetails(APPHeader.ApprovedByDepartmentCode).Department,
                PreparedBy = APPHeader.PreparedBy + ", " + APPHeader.PreparedByDesignation + " / " + hrisDataAccess.GetDepartmentDetails(APPHeader.PreparedByDepartmentCode).Department,
                CertifiedBy = APPHeader.RecommendingApproval + ", " + APPHeader.RecommendingApprovalDesignation + " / " + hrisDataAccess.GetDepartmentDetails(APPHeader.RecommendingApprovalDepartmentCode).Department,
                ProcurementOfficer = procurement.DepartmentHead + ", " + procurement.DepartmentHeadDesignation + " / " + procurement.Department,
                BACSecretariat = bac.SectionCode == null ? bac.DepartmentHead + ", " + bac.DepartmentHeadDesignation + " / " + bac.Department : bac.SectionHead + ", " + bac.SectionHeadDesignation + " / " + bac.Section,
                CreatedAt = APPHeader.CreatedAt,
                PreparedAt = APPHeader.PreparedAt,
                CertifiedAt = APPHeader.RecommendedAt,
                ApprovedAt = APPHeader.ApprovedAt,
                APPLineItems = APPDetails
            };
        }
        public AnnualProcurementPlanVM GetAPPHeader(string ReferenceNo)
        {
            AgencyDetails agencyDetails = db.AgencyDetails.First();
            var APPHeader = db.APPHeader.Where(d => d.ReferenceNo == ReferenceNo).FirstOrDefault();

            return new AnnualProcurementPlanVM
            {
                FiscalYear = APPHeader.FiscalYear,
                APPType = APPHeader.APPType,
                ReferenceNo = APPHeader.ReferenceNo,
                ApprovedBy = APPHeader.ApprovedBy,
                ApprovedAt = APPHeader.ApprovedAt,
                ApprovedByDesignation = APPHeader.ApprovedByDesignation,
                ApprovedByDepartment = hrisDataAccess.GetDepartmentDetails(APPHeader.ApprovedByDepartmentCode).Department,
                CreatedAt = APPHeader.CreatedAt,
                PreparedBy = APPHeader.PreparedBy,
                PreparedAt = (DateTime)APPHeader.PreparedAt,
                PreparedByDesignation = APPHeader.PreparedByDesignation,
                PreparedByDepartment = hrisDataAccess.GetDepartmentDetails(APPHeader.PreparedByDepartmentCode).Department,
                CertifiedBy = APPHeader.RecommendingApproval,
                CertifiedAt = APPHeader.RecommendedAt,
                CertifiedByDesignation = APPHeader.RecommendingApprovalDesignation,
                CertifiedByDepartment = hrisDataAccess.GetDepartmentDetails(APPHeader.RecommendingApprovalDepartmentCode).Department,
            };
        }
        public bool PostAPP(AnnualProcurementPlanVM APPViewModel, string UserEmail)
        {
            var user = db.UserAccounts.Where(d => d.Email == UserEmail).FirstOrDefault();
            var agencyDetails = db.AgencyDetails.FirstOrDefault();
            var hope = hrisDataAccess.GetDepartmentDetails(agencyDetails.HOPEReference);
            var property = hrisDataAccess.GetDepartmentDetails(agencyDetails.PropertyOfficeReference);
            var accounting = hrisDataAccess.GetDepartmentDetails(agencyDetails.AccountingOfficeReference);
            var procurement = hrisDataAccess.GetDepartmentDetails(agencyDetails.ProcurementOfficeReference);
            var bac = hrisDataAccess.GetDepartmentDetails(agencyDetails.BACOfficeReference);

            var app = db.APPHeader.Where(d => d.FiscalYear == APPViewModel.FiscalYear && d.APPType == "Original").FirstOrDefault();
            if(app == null)
            {
                app = new APPHeader()
                {
                    FiscalYear = APPViewModel.FiscalYear,
                    APPType = "Original",
                    ReferenceNo = GenerateReferenceNo(APPViewModel.FiscalYear, "Original"),
                    PreparedBy = procurement.DepartmentHead,
                    PreparedByDepartmentCode = procurement.DepartmentCode,
                    PreparedByDesignation = procurement.DepartmentHeadDesignation,
                    RecommendingApproval = bac.DepartmentHead,
                    RecommendingApprovalDepartmentCode = bac.DepartmentCode,
                    RecommendingApprovalDesignation = bac.DepartmentHeadDesignation,
                    ApprovedBy = hope.DepartmentHead,
                    ApprovedByDepartmentCode = hope.DepartmentCode,
                    ApprovedByDesignation = hope.DepartmentHeadDesignation,
                    CreatedBy = user.EmpCode,
                    CreatedAt = DateTime.Now
                };

                db.APPHeader.Add(app);
                if (db.SaveChanges() == 0)
                {
                    return false;
                }
            }
            else
            {
                app = new APPHeader()
                {
                    FiscalYear = APPViewModel.FiscalYear,
                    APPType = "Supplemental",
                    ReferenceNo = GenerateReferenceNo(APPViewModel.FiscalYear, "Supplemental"),
                    PreparedBy = procurement.DepartmentHead,
                    PreparedByDepartmentCode = procurement.DepartmentCode,
                    PreparedByDesignation = procurement.DepartmentHeadDesignation,
                    PreparedAt = DateTime.Now,
                    RecommendingApproval = bac.DepartmentHead,
                    RecommendingApprovalDepartmentCode = bac.DepartmentCode,
                    RecommendingApprovalDesignation = bac.DepartmentHeadDesignation,
                    ApprovedBy = hope.DepartmentHead,
                    ApprovedByDepartmentCode = hope.DepartmentCode,
                    ApprovedByDesignation = hope.DepartmentHeadDesignation,
                    CreatedBy = user.EmpCode,
                    CreatedAt = DateTime.Now
                };

                db.APPHeader.Add(app);
                if (db.SaveChanges() == 0)
                {
                    return false;
                }
            }

            var projectItems = APPViewModel.APPLineItems.Where(d => d.APPItemType == "Institutional").ToList();
            for (int i = 0; i < projectItems.Count; i++)
            {
                string projectProcurementMode = string.Empty;
                for (int x = 0; x < projectItems[i].ModeOfProcurement.Count; x++)
                {
                    if (x == projectItems[i].ModeOfProcurement.Count - 1)
                    {
                        projectProcurementMode += projectItems[i].ModeOfProcurement[x];
                    }
                    else
                    {
                        projectProcurementMode += projectItems[i].ModeOfProcurement[x] + "_";
                    }
                }

                var APPDetail = new APPInstitutionalItems
                {
                    APPHeaderReference = app.ID,
                    PAPCode = projectItems[i].PAPCode,
                    ProcurementProgram = projectItems[i].ProcurementProject,
                    Month = projectItems[i].Month,
                    StartMonth = projectItems[i].StartMonth,
                    EndMonth = projectItems[i].EndMonth,
                    ObjectClassification = projectItems[i].UACS,
                    EndUser = projectItems[i].EndUser,
                    ModeOfProcurementReference = projectProcurementMode,
                    FundSourceReference = projectItems[i].FundCluster,
                    MOOEAmount = projectItems[i].MOOE,
                    COAmount = projectItems[i].CapitalOutlay,
                    Total = projectItems[i].EstimatedBudget,
                    Remarks = projectItems[i].Remarks,
                    ProjectReferences = projectItems[i].ProjectReferences,
                    PPMPReferences = projectItems[i].PPMPReferences,
                    ProjectStatus = "For Assingment"
                };

                db.APPInstitutionalItems.Add(APPDetail);
                if (db.SaveChanges() == 0)
                {
                    return false;
                }

                var projectCodes = projectItems[i].ProjectReferences == null ? null : projectItems[i].ProjectReferences.Split("_".ToCharArray(), System.StringSplitOptions.RemoveEmptyEntries).ToArray();
                if(!projectItems[i].PAPCode.Contains("CUOS") && projectCodes != null)
                foreach (var code in projectCodes)
                {
                    var projectItemList = db.ProjectPlanItems.Where(d => d.FKProjectReference.ProjectCode == code && d.Status == "Approved" && d.FKItemReference.FKItemTypeReference.FKInventoryTypeReference.InventoryCode != "CUOS").ToList();
                    var projectServiceList = db.ProjectPlanServices.Where(d => d.FKProjectReference.ProjectCode == code && d.Status == "Approved").ToList();
                    if (projectItemList != null)
                    {
                        projectItemList.ForEach(d => { d.APPReference = app.ID; d.Status = "Posted to APP"; });
                        db.SaveChanges();
                    }
                    if (projectServiceList != null)
                    {
                        projectServiceList.ForEach(d => { d.APPReference = app.ID; d.Status = "Posted to APP"; });
                        db.SaveChanges();
                    }
                }

            }

            projectItems = APPViewModel.APPLineItems.Where(d => d.APPItemType == "Project").ToList();
            for (int i = 0; i < projectItems.Count; i++)
            {
                string projectProcurementMode = string.Empty;
                for (int x = 0; x < projectItems[i].ModeOfProcurement.Count; x++)
                {
                    if (x == projectItems[i].ModeOfProcurement.Count - 1)
                    {
                        projectProcurementMode += projectItems[i].ModeOfProcurement[x];
                    }
                    else
                    {
                        projectProcurementMode += projectItems[i].ModeOfProcurement[x] + "_";
                    }
                }

                var APPDetail = new APPProjectItems
                {
                    APPHeaderReference = app.ID,
                    PAPCode = projectItems[i].PAPCode,
                    ProcurementProgram = projectItems[i].ProcurementProject,
                    Month = projectItems[i].Month,
                    StartMonth = projectItems[i].StartMonth,
                    EndMonth = projectItems[i].EndMonth,
                    ObjectClassification = projectItems[i].UACS,
                    EndUser = projectItems[i].EndUser,
                    ModeOfProcurementReference = projectProcurementMode,
                    FundSourceReference = projectItems[i].FundCluster,
                    MOOEAmount = projectItems[i].MOOE,
                    COAmount = projectItems[i].CapitalOutlay,
                    Total = projectItems[i].EstimatedBudget,
                    Remarks = projectItems[i].Remarks,
                    ProjectStatus = "For Assingment"
                };

                db.APPProjectItems.Add(APPDetail);
                if (db.SaveChanges() == 0)
                {
                    return false;
                }

                var projectItemList = db.ProjectPlanItems.Where(d => d.FKProjectReference.ProjectCode == APPDetail.PAPCode 
                                                                && d.FKItemReference.FKItemTypeReference.AccountClass == APPDetail.ObjectClassification
                                                                && d.Status == "Approved" 
                                                                && d.FKItemReference.FKItemTypeReference.FKInventoryTypeReference.InventoryCode != "CUOS").ToList();
                var projectServiceList = db.ProjectPlanServices.Where(d => d.FKProjectReference.ProjectCode == APPDetail.PAPCode
                                                                && d.FKItemReference.FKItemTypeReference.AccountClass == APPDetail.ObjectClassification
                                                                && d.Status == "Approved"
                                                                && d.FKItemReference.FKItemTypeReference.FKInventoryTypeReference.InventoryCode != "CUOS").ToList();
                if (projectItemList != null)
                {
                    projectItemList.ForEach(d => { d.APPReference = app.ID; d.Status = "Posted to APP"; });
                    db.SaveChanges();
                }
                if (projectServiceList != null)
                {
                    projectServiceList.ForEach(d => { d.APPReference = app.ID; d.Status = "Posted to APP"; });
                    db.SaveChanges();
                }
            }

            return true;
        }
        public bool UpdateAPP(AnnualProcurementPlanVM APPViewModel, string UserEmail)
        {
            var user = db.UserAccounts.Where(d => d.Email == UserEmail).FirstOrDefault();
            var APPHeader = db.APPHeader.Where(d => d.ReferenceNo == APPViewModel.ReferenceNo).FirstOrDefault();

            if (APPHeader.PreparedAt != APPViewModel.PreparedAt)
            {
                APPHeader.PreparedAt = APPViewModel.PreparedAt;
            }
            else if(APPHeader.RecommendedAt != APPViewModel.CertifiedAt)
            {
                APPHeader.RecommendedAt = APPViewModel.CertifiedAt;
                APPHeader.RecommendingApprovalActionBy = user.EmpCode;
            }
            else if (APPHeader.ApprovedAt != APPViewModel.ApprovedAt)
            {
                APPHeader.ApprovedAt = APPViewModel.ApprovedAt;
                APPHeader.ApprovalActionBy = user.EmpCode;
            }
            else
            {
                return false;
            }

            if(db.SaveChanges() == 0)
            {
                return false;
            }

            return true;
        }
        private string GenerateReferenceNo(int FiscalYear, string APPType)
        {
            string referenceNo = String.Empty;
            var appTypeCode = APPType == "Original" ? "ORG" : APPType == "Supplemental" ? "SUP" : "AMD";
            var sequenceNo = (db.APPHeader.Where(d => d.ReferenceNo.Contains("ANPP-" + appTypeCode) && d.FiscalYear == FiscalYear).Count() + 1).ToString();
            sequenceNo = (sequenceNo.Length == 1) ? "00" + sequenceNo : (sequenceNo.Length == 2) ? "0" + sequenceNo : sequenceNo;
            referenceNo = "ANPP-" + appTypeCode + "-" + sequenceNo + "-" + FiscalYear;
            return referenceNo;
        }
        private string GeneratePAPCode(int FiscalYear, int ItemNo, int InventoryTypeID)
        {
            string PAPCode = string.Empty;
            var inventoryTypeCode = db.InventoryTypes.Find(InventoryTypeID).InventoryCode;
            PAPCode = inventoryTypeCode + "-UNIV-" + (ItemNo.ToString().Length == 1 ? "00" + ItemNo.ToString() : ItemNo.ToString().Length == 2 ? "0" + ItemNo.ToString() : ItemNo.ToString()) + "-" + FiscalYear.ToString();
            return PAPCode;
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