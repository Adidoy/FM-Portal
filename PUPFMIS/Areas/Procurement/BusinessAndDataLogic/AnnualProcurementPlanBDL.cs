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
                    rows.Add(new ContentCell(item.EndUser, 2, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                    rows.Add(new ContentCell(item.ModeOfProcurement, 3, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                    rows.Add(new ContentCell(item.StartMonth + " - " + item.EndMonth, 4, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center, 3));
                    rows.Add(new ContentCell(item.FundDescription, 8, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                    rows.Add(new ContentCell(String.Format("{0:C}", item.EstimatedBudget), 9, 8, false, false, ParagraphAlignment.Right, VerticalAlignment.Center));
                    rows.Add(new ContentCell(String.Format("{0:C}", item.MOOE), 10, 8, false, false, ParagraphAlignment.Right, VerticalAlignment.Center));
                    rows.Add(new ContentCell(String.Format("{0:C}", item.CapitalOutlay), 11, 8, false, false, ParagraphAlignment.Right, VerticalAlignment.Center));
                    rows.Add(new ContentCell(item.Remarks, 12, 6, false, false, ParagraphAlignment.Left, VerticalAlignment.Top));
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
            rows.Add(new ContentCell(APPViewModel.PreparedBy.ToUpper(), 1, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Top));
            rows.Add(new ContentCell("", 2, 8, true, false, ParagraphAlignment.Left, VerticalAlignment.Center));
            rows.Add(new ContentCell(APPViewModel.CertifiedBy.ToUpper(), 3, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Top));
            rows.Add(new ContentCell("", 4, 8, true, false, ParagraphAlignment.Left, VerticalAlignment.Center));
            rows.Add(new ContentCell(APPViewModel.ApprovedBy.ToUpper(), 5, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Top));
            rows.Add(new ContentCell("", 6, 8, true, false, ParagraphAlignment.Left, VerticalAlignment.Center));
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
            var itemYears = db.ProjectPlanItems.Where(d => d.Status == "Approved").Select(d => d.FKPPMPReference.FiscalYear).Distinct().ToList();
            var serviceYears = db.ProjectPlanServices.Where(d => d.Status == "Approved").Select(d => d.FKPPMPReference.FiscalYear).Distinct().ToList();
            var fiscalYears = itemYears.Union(serviceYears).Distinct().ToList();
            return fiscalYears;
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
        public List<ModeOfProcurement> GetModesOfProcurement()
        {
            return db.ProcurementModes.ToList();
        }
        public AnnualProcurementPlanVM GetAnnualProcurementPlan(string ReferenceNo)
        {
            AgencyDetails agencyDetails = db.AgencyDetails.First();
            var APPHeader = db.APPHeader.Where(d => d.ReferenceNo == ReferenceNo).FirstOrDefault();
            var APPDetails = new List<AnnualProcurementPlanDetailsVM>();
            var ProcurementPrograms = db.ProcurementPrograms.Where(d => d.FKAPPHeaderReference.ReferenceNo == ReferenceNo).ToList();

            foreach(var item in ProcurementPrograms)
            {
                var procurementModes = item.APPModeOfProcurementReference.Split("_".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).ToList();
                var modesOfProcurement = string.Empty;
                for(int i = 0; i < procurementModes.Count; i++)
                {
                    if(i == procurementModes.Count -1)
                    {
                        modesOfProcurement += db.ProcurementModes.Find(int.Parse(procurementModes[i])).ModeOfProcurementName;
                    }
                    else
                    {
                        modesOfProcurement += db.ProcurementModes.Find(int.Parse(procurementModes[i])).ModeOfProcurementName + "\n"; 
                    }
                }

                APPDetails.Add(new AnnualProcurementPlanDetailsVM
                {
                    UACS = item.ObjectClassification,
                    ObjectClassification = abisDataAccess.GetDetailedChartOfAccounts().Where(d => d.UACS_Code == item.ObjectClassification).FirstOrDefault().AcctName,
                    PAPCode = item.PAPCode,
                    ProcurementProject = item.ProcurementProgram,
                    ModeOfProcurement = modesOfProcurement,
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
                ApprovedBy = APPHeader.ApprovedBy + "\n" + APPHeader.ApprovedByDesignation,
                PreparedBy = APPHeader.PreparedBy + "\n" + APPHeader.PreparedByDesignation + "\n" + hrisDataAccess.GetDepartmentDetails(APPHeader.PreparedByDepartmentCode).Department,
                CertifiedBy = APPHeader.RecommendingApproval + "\n" + APPHeader.RecommendingApprovalDesignation + "\n" + hrisDataAccess.GetDepartmentDetails(APPHeader.RecommendingApprovalDepartmentCode).Department,
                ProcurementOfficer = procurement.DepartmentHead + "\n" + procurement.DepartmentHeadDesignation + "\n" + procurement.Department,
                BACSecretariat = bac.SectionCode == null ? bac.DepartmentHead + "\n" + bac.DepartmentHeadDesignation + "\n" + bac.Department : bac.SectionHead + "\n" + bac.SectionHeadDesignation + "\n" + bac.Section,
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
        private string GeneratePAPCode(int FiscalYear, int ItemNo, string InventoryCode)
        {
            string PAPCode = string.Empty;
            PAPCode = InventoryCode + "-UNIV-" + (ItemNo.ToString().Length == 1 ? "00" + ItemNo.ToString() : ItemNo.ToString().Length == 2 ? "0" + ItemNo.ToString() : ItemNo.ToString()) + "-" + FiscalYear.ToString();
            return PAPCode;
        }

        public List<ApprovedItems> GetApprovedItems(int FiscalYear)
        {
            var dbmSupplies = db.ProjectPlanItems
                .Where(d => d.FKProjectReference.FiscalYear == FiscalYear && (d.Status == "Approved" || d.Status == "Posted to APP") && d.FKItemReference.FKItemTypeReference.FKInventoryTypeReference.InventoryCode == "CUOS" && d.FKItemReference.ProcurementSource == ProcurementSources.PS_DBM)
                .Select(d => new
                {
                    IsTangible = d.FKItemReference.FKItemTypeReference.FKInventoryTypeReference.IsTangible,
                    IsInstitutional = d.FKItemReference.ResponsibilityCenter == null ? false : true,
                    UACS = d.FKItemReference.FKItemTypeReference.AccountClass,
                    EstimatedBudget = d.PPMPEstimatedBudget,
                    FundSource = d.FundSource,
                    ResponsibilityCenter = d.FKItemReference.ResponsibilityCenter == null ? d.FKPPMPReference.Department : d.FKItemReference.ResponsibilityCenter,
                    InventoryCode = d.FKItemReference.FKItemTypeReference.FKInventoryTypeReference.InventoryCode,
                    Month = d.FKProjectReference.ProjectMonthStart,
                    MOOE = d.UnitCost < 15000.00m ? d.PPMPEstimatedBudget : 0.00m,
                    CapitalOutlay = d.UnitCost >= 15000.00m ? d.PPMPEstimatedBudget : 0.00m
                })
                .GroupBy(d => new
                {
                    d.FundSource,
                    d.UACS,
                    d.IsTangible,
                    d.InventoryCode,
                    d.IsInstitutional,
                    d.ResponsibilityCenter
                })
                .Select(d => new ApprovedItems
                {
                    IsTangible = d.Key.IsTangible,
                    IsInstitutional = d.Key.IsInstitutional,
                    ItemCode = "CUOS-1",
                    ItemName = "Procurement of Common-use Office Supplies available at PS-DBM",
                    UACS = d.Key.UACS,
                    EstimatedBudget = d.Sum(x => x.EstimatedBudget),
                    FundSource = d.Key.FundSource,
                    MOOE = d.Sum(x => x.MOOE),
                    CapitalOutlay = d.Sum(x => x.CapitalOutlay),
                    InventoryCode = d.Key.InventoryCode,
                    Month = 1,
                    EndUser = d.Key.ResponsibilityCenter,
                    Remarks = "Common-use office supplies available at Procurement Services - Department of Budget and Management"
                }).ToList();

            var nonDBMSupplies = db.ProjectPlanItems
                .Where(d => d.FKProjectReference.FiscalYear == FiscalYear && (d.Status == "Approved" || d.Status == "Posted to APP") && d.FKItemReference.FKItemTypeReference.FKInventoryTypeReference.InventoryCode == "CUOS" && d.FKItemReference.ProcurementSource == ProcurementSources.Non_DBM)
                .Select(d => new
                {
                    IsTangible = d.FKItemReference.FKItemTypeReference.FKInventoryTypeReference.IsTangible,
                    IsInstitutional = d.FKItemReference.ResponsibilityCenter == null ? false : true,
                    UACS = d.FKItemReference.FKItemTypeReference.AccountClass,
                    EstimatedBudget = d.PPMPEstimatedBudget,
                    FundSource = d.FundSource,
                    ResponsibilityCenter = d.FKItemReference.ResponsibilityCenter == null ? d.FKPPMPReference.Department : d.FKItemReference.ResponsibilityCenter,
                    InventoryCode = d.FKItemReference.FKItemTypeReference.FKInventoryTypeReference.InventoryCode,
                    Month = d.FKProjectReference.ProjectMonthStart,
                    MOOE = d.UnitCost < 15000.00m ? d.PPMPEstimatedBudget : 0.00m,
                    CapitalOutlay = d.UnitCost >= 15000.00m ? d.PPMPEstimatedBudget : 0.00m
                })
                .GroupBy(d => new
                {
                    d.FundSource,
                    d.UACS,
                    d.IsTangible,
                    d.InventoryCode,
                    d.IsInstitutional,
                    d.ResponsibilityCenter
                })
                .Select(d => new ApprovedItems
                {
                    IsTangible = d.Key.IsTangible,
                    IsInstitutional = d.Key.IsInstitutional,
                    ItemCode = "CUOS-2",
                    ItemName = "Procurement of Common-use Office Supplies not available at PS-DBM",
                    UACS = d.Key.UACS,
                    EstimatedBudget = d.Sum(x => x.EstimatedBudget),
                    FundSource = d.Key.FundSource,
                    MOOE = d.Sum(x => x.MOOE),
                    CapitalOutlay = d.Sum(x => x.CapitalOutlay),
                    InventoryCode = d.Key.InventoryCode,
                    Month = 1,
                    EndUser = d.Key.ResponsibilityCenter,
                    Remarks = "Common-use office supplies procured from Private Suppliers"
                }).ToList();

            var emergencySupplies = db.ProjectPlanItems
                .Where(d => d.FKProjectReference.FiscalYear == FiscalYear && (d.Status == "Approved" || d.Status == "Posted to APP") && d.FKItemReference.FKItemTypeReference.FKInventoryTypeReference.InventoryCode == "CUOS")
                .Select(d => new
                {
                    IsTangible = d.FKItemReference.FKItemTypeReference.FKInventoryTypeReference.IsTangible,
                    IsInstitutional = d.FKItemReference.ResponsibilityCenter == null ? false : true,
                    UACS = d.FKItemReference.FKItemTypeReference.AccountClass,
                    EstimatedBudget = d.PPMPEstimatedBudget,
                    FundSource = d.FundSource,
                    ResponsibilityCenter = d.FKItemReference.ResponsibilityCenter == null ? d.FKPPMPReference.Department : d.FKItemReference.ResponsibilityCenter,
                    InventoryCode = d.FKItemReference.FKItemTypeReference.FKInventoryTypeReference.InventoryCode,
                    Month = d.FKProjectReference.ProjectMonthStart,
                    MOOE = d.UnitCost < 15000.00m ? d.PPMPEstimatedBudget : 0.00m,
                    CapitalOutlay = d.UnitCost >= 15000.00m ? d.PPMPEstimatedBudget : 0.00m
                })
                .GroupBy(d => new
                {
                    d.FundSource,
                    d.UACS,
                    d.IsTangible,
                    d.InventoryCode,
                    d.IsInstitutional,
                    d.ResponsibilityCenter
                })
                .Select(d => new ApprovedItems
                {
                    IsTangible = d.Key.IsTangible,
                    IsInstitutional = d.Key.IsInstitutional,
                    ItemCode = "CUOS-3",
                    ItemName = "Provision for unforeseen contingency under Section 52.1(a)",
                    UACS = d.Key.UACS,
                    EstimatedBudget = d.Sum(x => x.EstimatedBudget) * 0.04m,
                    FundSource = d.Key.FundSource,
                    MOOE = d.Sum(x => x.MOOE) * 0.04m,
                    CapitalOutlay = d.Sum(x => x.CapitalOutlay) * 0.04m,
                    InventoryCode = d.Key.InventoryCode,
                    Month = 1,
                    EndUser = d.Key.ResponsibilityCenter,
                    Remarks = "Common-use office supplies not available at PS-DBM that can be procured under Annex H Sec. 52"
                }).ToList();

            var projectItems = db.ProjectPlanItems
                .Where(d => d.FKProjectReference.FiscalYear == FiscalYear && d.Status == "Approved" && d.FKItemReference.FKItemTypeReference.FKInventoryTypeReference.InventoryCode != "CUOS")
                .Select(d => new
                {
                    IsTangible = d.FKItemReference.FKItemTypeReference.FKInventoryTypeReference.IsTangible,
                    IsInstitutional = d.FKItemReference.ResponsibilityCenter == null ? false : true,
                    ItemCode = d.FKItemReference.ItemCode,
                    ItemName = d.FKItemReference.ItemFullName,
                    ItemSpecification = d.FKItemReference.ItemSpecifications,
                    UACS = d.FKItemReference.FKItemTypeReference.AccountClass,
                    EstimatedBudget = d.PPMPEstimatedBudget,
                    FundSource = d.FundSource,
                    ResponsibilityCenter = d.FKItemReference.ResponsibilityCenter == null ? d.FKPPMPReference.Department : d.FKItemReference.ResponsibilityCenter,
                    InventoryCode = d.FKItemReference.FKItemTypeReference.FKInventoryTypeReference.InventoryCode,
                    Month = d.FKProjectReference.ProjectMonthStart,
                    MOOE = d.UnitCost < 15000.00m ? d.PPMPEstimatedBudget : 0.00m,
                    CapitalOutlay = d.UnitCost >= 15000.00m ? d.PPMPEstimatedBudget : 0.00m
                })
                .GroupBy(d => new
                    {
                        d.ItemCode,
                        d.ItemName,
                        d.FundSource,
                        d.UACS,
                        d.IsTangible,
                        d.InventoryCode,
                        d.IsInstitutional,
                        d.ResponsibilityCenter
                    })
                .Select(d => new ApprovedItems
                {
                    IsTangible = d.Key.IsTangible,
                    IsInstitutional = d.Key.IsInstitutional,
                    ItemCode = d.Key.ItemCode,
                    ItemName = d.Key.ItemName,
                    UACS = d.Key.UACS,
                    EstimatedBudget = d.Sum(x => x.EstimatedBudget),
                    FundSource = d.Key.FundSource,
                    MOOE = d.Sum(x => x.MOOE),
                    CapitalOutlay = d.Sum(x => x.CapitalOutlay),
                    InventoryCode = d.Key.InventoryCode,
                    Month = d.Max(x => x.Month),
                    EndUser = d.Key.ResponsibilityCenter
                }).ToList();

            var projectServices = db.ProjectPlanServices
                .Where(d => d.FKProjectReference.FiscalYear == FiscalYear && d.Status == "Approved" && d.FKItemReference.FKItemTypeReference.FKInventoryTypeReference.InventoryCode != "CUOS")
                .Select(d => new
                {
                    IsTangible = d.FKItemReference.FKItemTypeReference.FKInventoryTypeReference.IsTangible,
                    IsInstitutional = d.FKItemReference.ResponsibilityCenter == null ? false : true,
                    ItemCode = d.FKItemReference.ItemCode,
                    ItemName = d.FKItemReference.ItemFullName,
                    ItemSpecification = d.ItemSpecifications,
                    UACS = d.FKItemReference.FKItemTypeReference.AccountClass,
                    EstimatedBudget = d.PPMPEstimatedBudget,
                    FundSource = d.FundSource,
                    ResponsibilityCenter = d.FKItemReference.ResponsibilityCenter == null ? d.FKPPMPReference.Department : d.FKItemReference.ResponsibilityCenter,
                    InventoryCode = d.FKItemReference.FKItemTypeReference.FKInventoryTypeReference.InventoryCode,
                    Month = d.FKProjectReference.ProjectMonthStart,
                    MOOE = d.UnitCost < 15000.00m ? d.PPMPEstimatedBudget : 0.00m,
                    CapitalOutlay = d.UnitCost >= 15000.00m ? d.PPMPEstimatedBudget : 0.00m
                })
                .GroupBy(d => new
                {
                    d.ItemCode,
                    d.ItemName,
                    d.FundSource,
                    d.UACS,
                    d.IsTangible,
                    d.InventoryCode,
                    d.IsInstitutional,
                    d.ResponsibilityCenter
                })
                .Select(d => new ApprovedItems
                {
                    IsTangible = d.Key.IsTangible,
                    IsInstitutional = d.Key.IsInstitutional,
                    ItemCode = d.Key.ItemCode,
                    ItemName = d.Key.ItemName,
                    UACS = d.Key.UACS,
                    EstimatedBudget = d.Sum(x => x.EstimatedBudget),
                    FundSource = d.Key.FundSource,
                    MOOE = d.Sum(x => x.MOOE),
                    CapitalOutlay = d.Sum(x => x.CapitalOutlay),
                    InventoryCode = d.Key.InventoryCode,
                    Month = d.Max(x => x.Month),
                    EndUser = d.Key.ResponsibilityCenter
                }).ToList();

            foreach (var item in dbmSupplies)
            {
                item.FundDescription = abisDataAccess.GetFundSources(item.FundSource).FUND_DESC;
                item.ObjectClassification = abisDataAccess.GetChartOfAccounts(item.UACS).AcctName;
            }

            foreach (var item in nonDBMSupplies)
            {
                item.FundDescription = abisDataAccess.GetFundSources(item.FundSource).FUND_DESC;
                item.ObjectClassification = abisDataAccess.GetChartOfAccounts(item.UACS).AcctName;
            }

            foreach (var item in emergencySupplies)
            {
                item.FundDescription = abisDataAccess.GetFundSources(item.FundSource).FUND_DESC;
                item.ObjectClassification = abisDataAccess.GetChartOfAccounts(item.UACS).AcctName;
            }

            foreach (var item in projectItems)
            {
                item.FundDescription = abisDataAccess.GetFundSources(item.FundSource).FUND_DESC;
                item.ObjectClassification = abisDataAccess.GetChartOfAccounts(item.UACS).AcctName;
            }

            foreach (var item in projectServices)
            {
                item.FundDescription = abisDataAccess.GetFundSources(item.FundSource).FUND_DESC;
                item.ObjectClassification = abisDataAccess.GetChartOfAccounts(item.UACS).AcctName;
            }

            List<ApprovedItems> approvedItems = new List<ApprovedItems>();
            approvedItems.AddRange(dbmSupplies);
            approvedItems.AddRange(nonDBMSupplies);
            approvedItems.AddRange(emergencySupplies);
            approvedItems.AddRange(projectItems.OrderBy(d => d.ItemName).ToList());
            approvedItems.AddRange(projectServices.OrderBy(d => d.ItemName).ToList());

            return approvedItems;
        }
        public bool PostAPP(List<ApprovedItems> ApprovedItems, int FiscalYear, string UserEmail)
        {
            SystemBDL sysBDL = new SystemBDL();
            var APPDetail = new List<ProcurementPrograms>();
            var user = db.UserAccounts.Where(d => d.Email == UserEmail).FirstOrDefault();
            var agencyDetails = db.AgencyDetails.FirstOrDefault();
            var hope = hrisDataAccess.GetDepartmentDetails(agencyDetails.HOPEReference);
            var property = hrisDataAccess.GetDepartmentDetails(agencyDetails.PropertyOfficeReference);
            var accounting = hrisDataAccess.GetDepartmentDetails(agencyDetails.AccountingOfficeReference);
            var procurement = hrisDataAccess.GetDepartmentDetails(agencyDetails.ProcurementOfficeReference);
            var bac = hrisDataAccess.GetDepartmentDetails(agencyDetails.BACOfficeReference);

            var app = db.APPHeader.Where(d => d.FiscalYear == FiscalYear && d.APPType == "Original").FirstOrDefault();
            if (app == null)
            {
                app = new APPHeader()
                {
                    FiscalYear = FiscalYear,
                    APPType = "Original",
                    ReferenceNo = GenerateReferenceNo(FiscalYear, "Original"),
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
                    FiscalYear = FiscalYear,
                    APPType = "Supplemental",
                    ReferenceNo = GenerateReferenceNo(FiscalYear, "Supplemental"),
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

            var institutionalItems = ApprovedItems
                .Where(d => d.IsInstitutional)
                .GroupBy(d => new { d.InventoryCode, d.UACS, d.ObjectClassification, d.FundSource, d.EndUser, d.IsTangible, d.ItemCode, d.ItemName, d.IsInstitutional })
                .Select(d => new
                {
                    InventoryCode = d.Key.InventoryCode,
                    ItemCode = d.Key.ItemCode,
                    ItemName = d.Key.ItemName,
                    ObjectClassification = d.Key.ObjectClassification,
                    UACS = d.Key.UACS,
                    FundSource = d.Key.FundSource,
                    MOOEAmount = d.Sum(x => x.MOOE),
                    COAmount = d.Sum(x => x.CapitalOutlay),
                    Total = d.Sum(x => x.EstimatedBudget),
                    EndUser = d.Key.EndUser,
                    Month = d.Max(x => x.Month),
                    IsTangible = d.Key.IsTangible,
                    IsInstitutional = d.Key.IsInstitutional
                }).ToList();

            var projectItems = ApprovedItems.Where(d => !d.IsInstitutional).ToList();

            int itemNo = 1;
            foreach (var item in institutionalItems)
            {
                var remarksList = ApprovedItems.Where(d => d.IsInstitutional && d.FundSource == item.FundSource && d.UACS == item.UACS && d.ItemCode == item.ItemCode).Select(d => d.Remarks).ToList();
                var procurementModes = ApprovedItems.Where(d => d.IsInstitutional && d.FundSource == item.FundSource && d.UACS == item.UACS && d.ItemCode == item.ItemCode).SelectMany(d => d.ModeOfProcurement).Distinct().ToArray();
                var projectCodes = item.IsTangible ? db.ProjectPlanItems.Where(d => d.FKItemReference.ItemCode == item.ItemCode && d.FundSource == item.FundSource && d.FKPPMPReference.FiscalYear == FiscalYear && d.Status == "Approved").Select(d => d.FKProjectReference.ProjectCode).ToList() : db.ProjectPlanServices.Where(d => d.FKItemReference.ItemCode == item.ItemCode && d.FundSource == item.FundSource && d.FKPPMPReference.FiscalYear == FiscalYear && d.Status == "Approved").Select(d => d.FKProjectReference.ProjectCode).ToList();
                var ppmpReferences = item.IsTangible ? db.ProjectPlanItems.Where(d => d.FKItemReference.ItemCode == item.ItemCode && d.FundSource == item.FundSource && d.FKPPMPReference.FiscalYear == FiscalYear && d.Status == "Approved").Select(d => d.FKPPMPReference.ReferenceNo).ToList() : db.ProjectPlanServices.Where(d => d.FKItemReference.ItemCode == item.ItemCode && d.FundSource == item.FundSource && d.FKPPMPReference.FiscalYear == FiscalYear && d.Status == "Approved").Select(d => d.FKPPMPReference.ReferenceNo).ToList();

                var remarks = string.Empty;
                var modesOfProcurement = string.Empty;
                var projectReference = string.Empty;
                var ppmpReference = string.Empty;

                for(int i = 0; i < procurementModes.Count(); i++)
                {
                    if (i == procurementModes.Count() - 1)
                    {
                        modesOfProcurement += procurementModes[i];
                    }
                    else
                    {
                        modesOfProcurement += procurementModes[i] + "_";
                    }
                }

                for(int i = 0; i < remarksList.Count; i++)
                {
                    if(i == remarksList.Count - 1)
                    {
                        remarks += remarksList[i];
                    }
                    else
                    {
                        remarks += remarksList[i] + "; ";
                    }
                }

                for (int i = 0; i < projectCodes.Count; i++)
                {
                    if (i == projectCodes.Count - 1)
                    {
                        projectReference += projectCodes[i];
                    }
                    else
                    {
                        projectReference += projectCodes[i] + "_";
                    }
                }

                for (int i = 0; i < ppmpReferences.Count; i++)
                {
                    if (i == ppmpReferences.Count - 1)
                    {
                        ppmpReference += ppmpReferences[i];
                    }
                    else
                    {
                        ppmpReference += ppmpReferences[i] + "_";
                    }
                }


                APPDetail.Add(new ProcurementPrograms
                {
                    APPHeaderReference = app.ID,
                    PAPCode = GeneratePAPCode(FiscalYear, itemNo, item.InventoryCode),
                    ProcurementProgram = item.ItemName,
                    Month = item.Month,
                    StartMonth = item.Month <= 5 ? "Fourth Quarter, " + (FiscalYear - 1) : sysBDL.GetMonthName(item.Month - 5) + ", " + FiscalYear.ToString(),
                    EndMonth = sysBDL.GetMonthName(item.Month) + ", " + FiscalYear.ToString(),
                    ObjectClassification = item.UACS,
                    EndUser = item.EndUser,
                    APPModeOfProcurementReference = modesOfProcurement,
                    FundSourceReference = item.FundSource,
                    MOOEAmount = item.MOOEAmount,
                    COAmount = item.COAmount,
                    Total = item.Total,
                    Remarks = ((remarks == string.Empty || remarks == null) ? null : (remarks + "\n\n")) + (ppmpReference == string.Empty ? "" : "References:\n" + ppmpReference.Replace("_", "\n")),
                    ProjectReferences = projectReference == string.Empty ? null : projectReference,
                    PPMPReferences = ppmpReference == string.Empty ? null : ppmpReference,
                    ProjectStatus = "For Assingment",
                    IsInstitutional = item.IsInstitutional,
                    IsTangible = item.IsTangible
                });

                itemNo++;
            }

            foreach (var item in projectItems)
            {
                var procurementModes = ApprovedItems.Where(d => !d.IsInstitutional && d.FundSource == item.FundSource && d.UACS == item.UACS).SelectMany(d => d.ModeOfProcurement).Distinct().ToArray();
                var modesOfProcurement = string.Empty;

                for (int i = 0; i < procurementModes.Count(); i++)
                {
                    if (i == procurementModes.Count() - 1)
                    {
                        modesOfProcurement += procurementModes[i];
                    }
                    else
                    {
                        modesOfProcurement += procurementModes[i] + "_";
                    }
                }

                var items = db.ProjectPlanItems.Where(d => d.FKItemReference.ItemCode == item.ItemCode && d.FundSource.Contains(item.FundSource) && d.Status == "Approved" && d.FKPPMPReference.FiscalYear == FiscalYear && d.FKPPMPReference.Department == item.EndUser).Select(d => new { d.FKProjectReference.ProjectCode, d.FKProjectReference.ProjectName, d.FKPPMPReference.Department, d.FKPPMPReference.ReferenceNo }).ToList();
                var services = db.ProjectPlanServices.Where(d => d.FKItemReference.ItemCode == item.ItemCode && d.FundSource.Contains(item.FundSource) && d.Status == "Approved" && d.FKPPMPReference.FiscalYear == FiscalYear && d.FKPPMPReference.Department == item.EndUser).Select(d => new { d.FKProjectReference.ProjectCode, d.FKProjectReference.ProjectName, d.FKPPMPReference.Department, d.FKPPMPReference.ReferenceNo }).ToList();

                var projectPlans = items.Union(services).ToList();

                foreach(var plan in projectPlans)
                {
                    APPDetail.Add(new ProcurementPrograms
                    {
                        APPHeaderReference = app.ID,
                        PAPCode = plan.ProjectCode,
                        ProcurementProgram = item.ItemName,
                        Month = item.Month,
                        StartMonth = item.Month <= 5 ? "Fourth Quarter, " + (FiscalYear - 1) : sysBDL.GetMonthName(item.Month - 5) + ", " + FiscalYear.ToString(),
                        EndMonth = sysBDL.GetMonthName(item.Month) + ", " + FiscalYear.ToString(),
                        ObjectClassification = item.UACS,
                        EndUser = plan.Department,
                        APPModeOfProcurementReference = modesOfProcurement,
                        FundSourceReference = item.FundSource,
                        MOOEAmount = item.MOOE,
                        COAmount = item.CapitalOutlay,
                        Total = item.EstimatedBudget,
                        Remarks = ((item.Remarks == string.Empty || item.Remarks == null) ? string.Empty : (item.Remarks + "; ")) + plan.ProjectName + "\n\nReference: \n" + (item.IsTangible ? db.ProjectPlanItems.Where(d => d.FKProjectReference.ProjectCode == plan.ProjectCode && d.FKItemReference.ItemCode == item.ItemCode).FirstOrDefault().FKPPMPReference.ReferenceNo : db.ProjectPlanServices.Where(d => d.FKProjectReference.ProjectCode == plan.ProjectCode && d.FKItemReference.ItemCode == item.ItemCode).FirstOrDefault().FKPPMPReference.ReferenceNo),
                        ProjectReferences = plan.ProjectCode,
                        PPMPReferences = plan.ReferenceNo,
                        ProjectStatus = "For Assingment",
                        IsInstitutional = item.IsInstitutional,
                        IsTangible = item.IsTangible
                    });
                }
            }

            db.ProcurementPrograms.AddRange(APPDetail);
            if (db.SaveChanges() == 0)
            {
                return false;
            }

            foreach (var item in projectItems)
            {
                if (item.IsTangible)
                {
                    var projectPlanItems = db.ProjectPlanItems
                        .Where(d => d.FKProjectReference.ProjectCode == item.PAPCode && d.FKItemReference.ItemCode == item.ItemCode)
                        .ToList();
                    projectPlanItems.ForEach(d => { d.Status = "Posted to APP"; d.APPReference = app.ID; });
                    db.SaveChanges();
                }
                else
                {
                    var projectPlanServices = db.ProjectPlanServices
                        .Where(d => d.FKProjectReference.ProjectCode == item.PAPCode && d.FKItemReference.ItemCode == item.ItemCode)
                        .ToList();
                    projectPlanServices.ForEach(d => { d.Status = "Posted to APP"; d.APPReference = app.ID; });
                    db.SaveChanges();
                }
            }

            foreach (var item in ApprovedItems)
            {
                if (item.IsTangible)
                {
                    var projectPlanItems = db.ProjectPlanItems
                        .Where(d =>
                            d.FKPPMPReference.FiscalYear == FiscalYear &&
                            d.FKItemReference.ItemCode == item.ItemCode &&
                            d.Status == "Approved" &&
                            (item.IsInstitutional ? d.FKItemReference.ResponsibilityCenter == item.EndUser : d.FKPPMPReference.Department == item.EndUser)
                        ).ToList();
                    projectPlanItems.ForEach(d => { d.Status = "Posted to APP"; d.APPReference = app.ID; });
                    db.SaveChanges();
                    var ppmpReferences = projectPlanItems.Select(d => d.PPMPReference).GroupBy(d => d).Select(d => d.Key);
                    var ppmps = db.PPMPHeader.Where(d => ppmpReferences.Contains(d.ID)).ToList();
                    ppmps.ForEach(d => { d.Status = "Posted to APP"; });
                    db.SaveChanges();
                }
                else
                {
                    var projectPlanServices = db.ProjectPlanServices
                        .Where(d =>
                            d.FKPPMPReference.FiscalYear == FiscalYear &&
                            d.FKItemReference.ItemCode == item.ItemCode &&
                            d.Status == "Approved" &&
                            (item.IsInstitutional ? d.FKItemReference.ResponsibilityCenter == item.EndUser : d.FKPPMPReference.Department == item.EndUser)
                        ).ToList();
                    projectPlanServices.ForEach(d => { d.Status = "Posted to APP"; d.APPReference = app.ID; });
                    db.SaveChanges();
                    var ppmpReferences = projectPlanServices.Select(d => d.PPMPReference).GroupBy(d => d).Select(d => d.Key);
                    var ppmps = db.PPMPHeader.Where(d => ppmpReferences.Contains(d.ID)).ToList();
                    ppmps.ForEach(d => { d.Status = "Posted to APP"; });
                    db.SaveChanges();
                }
            }

            foreach(var item in APPDetail)
            {
                var projectCodes = item.ProjectReferences == null ? null : item.ProjectReferences.Split("_".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).ToList();
                if(projectCodes != null)
                {
                    foreach (var code in projectCodes)
                    {
                        var items = db.ProjectPlanItems.Where(d => d.FKProjectReference.ProjectCode == code && d.APPReference == app.ID && d.FKItemReference.ItemFullName == item.ProcurementProgram && d.FundSource == item.FundSourceReference).ToList();
                        var services = db.ProjectPlanServices.Where(d => d.FKProjectReference.ProjectCode == code && d.APPReference == app.ID && d.FKItemReference.ItemFullName == item.ProcurementProgram && d.FundSource == item.FundSourceReference).ToList();
                        if (items.Count > 0)
                        {
                            items.ForEach(d => { d.APPLineReference = item.PAPCode; });
                            db.SaveChanges();
                        }
                        if (services.Count > 0)
                        {
                            services.ForEach(d => { d.APPLineReference = item.PAPCode; });
                            db.SaveChanges();
                        }
                    }
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
            }
            base.Dispose(disposing);
        }
    }
}