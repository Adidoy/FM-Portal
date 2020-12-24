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

            var APPType = APPViewModel.APPType == "Indicative" ? "INDICATIVE " : APPViewModel.APPType == "Original" ? String.Empty : APPViewModel.APPType.ToUpper() + " ";

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

            int count = 0;
            foreach(var acct in accounts)
            {
                columns = new List<ContentColumn>();
                columns.Add(new ContentColumn(12.5, new MigraDoc.DocumentObjectModel.Color(252, 207, 101)));
                reports.AddTable(columns, true);

                rows = new List<ContentCell>();
                rows.Add(new ContentCell(Convert.ToChar(count + 65).ToString() + ". " + acct, 0, 10, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Left, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                reports.AddRowContent(rows, 0.25);
                count++;

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

                var acctItems = APPViewModel.APPLineItems.Where(d => d.ObjectClassification.StartsWith(acct.Substring(0, acct.Length - 4))).ToList();
                foreach (var item in acctItems)
                {
                    rows = new List<ContentCell>();
                    rows.Add(new ContentCell(item.PAPCode, 0, 7, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                    rows.Add(new ContentCell(item.ProcurementProject, 1, 7, true, false, ParagraphAlignment.Left, VerticalAlignment.Center));
                    rows.Add(new ContentCell(item.EndUser, 2, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                    rows.Add(new ContentCell(item.ModeOfProcurement.Replace("\n", " / \n"), 3, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
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
        public List<ModeOfProcurement> GetModesOfProcurement()
        {
            return db.ProcurementModes.ToList();
        }
        public AnnualProcurementPlanVM GetAnnualProcurementPlan(string ReferenceNo)
        {
            AgencyDetails agencyDetails = db.AgencyDetails.First();
            var APPHeader = db.APPHeader.Where(d => d.ReferenceNo == ReferenceNo).FirstOrDefault();
            var APPDetails = new List<AnnualProcurementPlanDetailsVM>();
            var ProcurementPrograms = db.APPDetails.Where(d => d.FKAPPHeaderReference.ReferenceNo == ReferenceNo).ToList();

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
                    UACS = item.ObjectClassification.Substring(0, item.ObjectClassification.Length - 5) + "00000",
                    ObjectClassification = abisDataAccess.GetChartOfAccounts(item.ObjectClassification).SubAcctName,
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
            var appTypeCode = (APPType == "Original" || APPType == "Indicative") ? "ORG" : APPType == "Supplemental" ? "SUP" : "AMD";
            var sequenceNo = (db.APPHeader.Where(d => d.ReferenceNo.Contains("ANPP-" + appTypeCode) && d.FiscalYear == FiscalYear).Count() + 1).ToString();
            sequenceNo = (sequenceNo.Length == 1) ? "00" + sequenceNo : (sequenceNo.Length == 2) ? "0" + sequenceNo : sequenceNo;
            referenceNo = "ANPP-" + appTypeCode + "-" + sequenceNo + "-" + FiscalYear;
            return referenceNo;
        }

        private List<ApprovedItems> CommonSuppliesProjects(int FiscalYear)
        {
            var commonSuppliesProjects = new List<ApprovedItems>();
            var chartOfAccounts = abisDataAccess.GetChartOfAccounts().ToList();
            var commonSuppliesItems = (from chart in chartOfAccounts
                                          join items in db.ProjectPlanItems.ToList() on chart.UACS_Code equals items.FKItemReference.FKItemTypeReference.UACSObjectClass
                                          where items.UnitCost < 15000.00m && (items.Status == "Approved" || items.Status == "Posted to APP") && (items.APPLineReference == null) && 
                                                items.FKItemReference.FKItemTypeReference.FKInventoryTypeReference.InventoryCode == "CUOS"
                                          select new
                                          {
                                              IsTangible = items.FKItemReference.FKItemTypeReference.FKInventoryTypeReference.IsTangible,
                                              IsInstitutional = items.FKItemReference.ResponsibilityCenter == null ? false : true,
                                              ItemCode = items.FKItemReference.ItemCode,
                                              ItemName = items.FKItemReference.ItemFullName,
                                              ItemSpecification = items.FKItemReference.ItemSpecifications,
                                              UACS = items.FKItemReference.FKItemTypeReference.UACSObjectClass,
                                              EstimatedBudget = items.PPMPEstimatedBudget,
                                              FundSource = items.FundSource,
                                              ResponsibilityCenter = items.FKItemReference.ResponsibilityCenter == null ? items.FKPPMPReference.Department : items.FKItemReference.ResponsibilityCenter,
                                              InventoryCode = items.FKItemReference.FKItemTypeReference.FKInventoryTypeReference.InventoryCode,
                                              Month = items.FKProjectReference.ProjectMonthStart,
                                              MOOE = chart.ClassCode == "5" ? items.PPMPEstimatedBudget : 0.00m,
                                              CapitalOutlay = chart.ClassCode == "1" ? items.PPMPEstimatedBudget : 0.00m,
                                              ProcurementSource = items.FKItemReference.ProcurementSource,
                                              EndUser = items.FKItemReference.ResponsibilityCenter == null ? items.FKProjectReference.Department : items.FKItemReference.ResponsibilityCenter
                                          }).ToList();

            commonSuppliesProjects = commonSuppliesItems
                                        .GroupBy(d => new {
                                            d.IsTangible,
                                            d.IsInstitutional,
                                            d.UACS,
                                            d.FundSource,
                                            d.InventoryCode,
                                            d.EndUser,
                                            d.ProcurementSource
                                        })
                                        .Select(d => new ApprovedItems
                                        {
                                            IsInstitutional = d.Key.IsInstitutional,
                                            IsTangible = d.Key.IsTangible,
                                            ItemCode = d.Key.UACS,
                                            UACS = d.Key.UACS,
                                            EstimatedBudget = d.Sum(x => x.EstimatedBudget),
                                            FundSource = d.Key.FundSource,
                                            MOOE = d.Sum(x => x.MOOE),
                                            CapitalOutlay = d.Sum(x => x.CapitalOutlay),
                                            InventoryCode = d.Key.InventoryCode,
                                            Month = 1,
                                            EndUser = d.Key.EndUser,
                                            ProcurementSource = d.Key.ProcurementSource
                                        }).ToList();

            foreach (var project in commonSuppliesProjects)
            {
                project.ItemCode = project.UACS + '-' + project.FundSource.Replace("\r\n", "") + "-" + project.ProcurementSource;
                project.EndUserName = hrisDataAccess.GetDepartmentDetails(project.EndUser).Department;
                project.ItemName = (abisDataAccess.GetChartOfAccounts(project.UACS).AcctName.Replace("Expenses", "") + (project.ProcurementSource == ProcurementSources.PS_DBM ? "available at DBM Procurement Service" : "not available at DBM-PS")).ToUpper();
                project.ModeOfProcurement = project.ProcurementSource == ProcurementSources.PS_DBM ? new string[] { "10" } : new string[] { "1", "5", "14" };
                project.ObjectClassification = abisDataAccess.GetChartOfAccounts(project.UACS).AcctName;
                project.UACSSubClass = project.UACS.Substring(0, project.UACS.Length - 5) + "00000";
                project.ObjectSubClassification = abisDataAccess.GetChartOfAccounts(project.UACS).SubAcctName;
                project.FundDescription = abisDataAccess.GetFundSources(project.FundSource).FUND_DESC;
                project.Schedule = (project.Month <= 5 ? "October " + (FiscalYear - 1) : systemBDL.GetMonthName(project.Month - 5)) + " - " + systemBDL.GetMonthName(project.Month) + " " + FiscalYear.ToString();
                project.Remarks = project.ProcurementSource == ProcurementSources.PS_DBM ? "Common-use supplies available at DBM - Procurement Service." : "Common-use supplies not available at DBM-PS but can be procured under Annex H Sec. 52 of R.A. 9184.";
            }

            return commonSuppliesProjects;
        }
        private List<ApprovedItems> LessThanThresholdProjects(int FiscalYear)
        {
            var lessThanThresholdProjects = new List<ApprovedItems>();
            var chartOfAccounts = abisDataAccess.GetChartOfAccounts().ToList();
            var lessThanThresholdItems = (from chart in chartOfAccounts
                                          join items in db.ProjectPlanItems.ToList() on chart.UACS_Code equals items.FKItemReference.FKItemTypeReference.UACSObjectClass
                                          where items.UnitCost < 15000.00m && items.Status == "Approved" && items.FKItemReference.FKItemTypeReference.FKInventoryTypeReference.InventoryCode != "CUOS"
                                        select new
                                        {
                                            IsTangible = items.FKItemReference.FKItemTypeReference.FKInventoryTypeReference.IsTangible,
                                            IsInstitutional = items.FKItemReference.ResponsibilityCenter == null ? false : true,
                                            ItemCode = items.FKItemReference.ItemCode,
                                            ItemName = items.FKItemReference.ItemFullName,
                                            ItemSpecification = items.FKItemReference.ItemSpecifications,
                                            UACS = items.FKItemReference.FKItemTypeReference.UACSObjectClass,
                                            EstimatedBudget = items.PPMPEstimatedBudget,
                                            FundSource = items.FundSource,
                                            ResponsibilityCenter = items.FKItemReference.ResponsibilityCenter == null ? items.FKPPMPReference.Department : items.FKItemReference.ResponsibilityCenter,
                                            InventoryCode = items.FKItemReference.FKItemTypeReference.FKInventoryTypeReference.InventoryCode,
                                            Month = items.FKProjectReference.ProjectMonthStart,
                                            MOOE = chart.ClassCode == "5" ? items.PPMPEstimatedBudget : 0.00m,
                                            CapitalOutlay = chart.ClassCode == "1" ? items.PPMPEstimatedBudget : 0.00m
                                        } into result
                                        group result by new
                                        {
                                            result.ItemCode,
                                            result.ItemName,
                                            result.FundSource,
                                            result.UACS,
                                            result.IsTangible,
                                            result.InventoryCode,
                                            result.IsInstitutional,
                                            result.ResponsibilityCenter
                                        } into groupedResult
                                        select new ApprovedItems
                                        {
                                            IsTangible = groupedResult.Key.IsTangible,
                                            IsInstitutional = groupedResult.Key.IsInstitutional,
                                            ItemCode = groupedResult.Key.ItemCode,
                                            ItemName = groupedResult.Key.ItemName,
                                            UACS = groupedResult.Key.UACS,
                                            EstimatedBudget = groupedResult.Sum(x => x.EstimatedBudget),
                                            FundSource = groupedResult.Key.FundSource,
                                            MOOE = groupedResult.Sum(x => x.MOOE),
                                            CapitalOutlay = groupedResult.Sum(x => x.CapitalOutlay),
                                            InventoryCode = groupedResult.Key.InventoryCode,
                                            Month = groupedResult.Max(x => x.Month),
                                            EndUser = groupedResult.Key.ResponsibilityCenter
                                        }).ToList();

            var lessThanThresholdServices = (from chart in chartOfAccounts
                                             join items in db.ProjectPlanServices.ToList() on chart.UACS_Code equals items.FKItemReference.FKItemTypeReference.UACSObjectClass
                                             where items.UnitCost < 15000.00m && items.Status == "Approved" && items.FKItemReference.FKItemTypeReference.FKInventoryTypeReference.InventoryCode != "CUOS"
                                             select new
                                             {
                                                 IsTangible = items.FKItemReference.FKItemTypeReference.FKInventoryTypeReference.IsTangible,
                                                 IsInstitutional = items.FKItemReference.ResponsibilityCenter == null ? false : true,
                                                 ItemCode = items.FKItemReference.ItemCode,
                                                 ItemName = items.FKItemReference.ItemFullName,
                                                 ItemSpecification = items.FKItemReference.ItemSpecifications,
                                                 UACS = items.FKItemReference.FKItemTypeReference.UACSObjectClass,
                                                 EstimatedBudget = items.PPMPEstimatedBudget,
                                                 FundSource = items.FundSource,
                                                 ResponsibilityCenter = items.FKItemReference.ResponsibilityCenter == null ? items.FKPPMPReference.Department : items.FKItemReference.ResponsibilityCenter,
                                                 InventoryCode = items.FKItemReference.FKItemTypeReference.FKInventoryTypeReference.InventoryCode,
                                                 Month = items.FKProjectReference.ProjectMonthStart,
                                                 MOOE = chart.ClassCode == "5" ? items.PPMPEstimatedBudget : 0.00m,
                                                 CapitalOutlay = chart.ClassCode == "1" ? items.PPMPEstimatedBudget : 0.00m
                                             } into result
                                             group result by new
                                             {
                                                 result.FundSource,
                                                 result.UACS,
                                                 result.IsTangible,
                                                 result.InventoryCode,
                                                 result.IsInstitutional,
                                                 result.ResponsibilityCenter
                                             } into groupedResult
                                             select new ApprovedItems
                                             {
                                                 IsTangible = groupedResult.Key.IsTangible,
                                                 IsInstitutional = groupedResult.Key.IsInstitutional,
                                                 ItemCode = groupedResult.Key.UACS,
                                                 UACS = groupedResult.Key.UACS,
                                                 EstimatedBudget = groupedResult.Sum(x => x.EstimatedBudget),
                                                 FundSource = groupedResult.Key.FundSource,
                                                 MOOE = groupedResult.Sum(x => x.MOOE),
                                                 CapitalOutlay = groupedResult.Sum(x => x.CapitalOutlay),
                                                 InventoryCode = groupedResult.Key.InventoryCode,
                                                 Month = groupedResult.Max(x => x.Month),
                                                 EndUser = groupedResult.Key.ResponsibilityCenter
                                             }).ToList();

            lessThanThresholdProjects.AddRange(lessThanThresholdItems);
            lessThanThresholdProjects.AddRange(lessThanThresholdServices);

            lessThanThresholdProjects = lessThanThresholdProjects
                                        .GroupBy(d => new {
                                            d.IsTangible,
                                            d.IsInstitutional,
                                            d.UACS,
                                            d.FundSource,
                                            d.InventoryCode,
                                            d.Schedule,
                                            d.EndUser
                                        })
                                        .Select(d => new ApprovedItems {
                                            IsInstitutional = d.Key.IsInstitutional,
                                            IsTangible = d.Key.IsTangible,
                                            ItemCode = d.Key.UACS,
                                            UACS = d.Key.UACS,
                                            EstimatedBudget = d.Sum(x => x.EstimatedBudget),
                                            FundSource = d.Key.FundSource,
                                            MOOE = d.Sum(x => x.MOOE),
                                            CapitalOutlay = d.Sum(x => x.CapitalOutlay),
                                            InventoryCode = d.Key.InventoryCode,
                                            Month = d.Max(x => x.Month),
                                            EndUser = d.Key.EndUser,
                                            Schedule = d.Key.Schedule,
                                        }).ToList();

            foreach (var project in lessThanThresholdProjects)
            {
                project.ItemCode = project.UACS + '-' + project.FundSource.Replace("\r\n", "");
                project.EndUserName = hrisDataAccess.GetDepartmentDetails(project.EndUser).Department;
                project.ItemName = abisDataAccess.GetChartOfAccounts(project.UACS).AcctName.Replace("Expenses", "").ToUpper();
                project.ModeOfProcurement = project.IsTangible ? new string[] { "1", "5", "14" } : new string[] { "1", "14" };
                project.ObjectClassification = abisDataAccess.GetChartOfAccounts(project.UACS).AcctName;
                project.FundDescription = abisDataAccess.GetFundSources(project.FundSource).FUND_DESC;
                project.Schedule = (project.Month <= 5 ? "October " + (FiscalYear - 1) : systemBDL.GetMonthName(project.Month - 5)) + " - " + systemBDL.GetMonthName(project.Month) + " " + FiscalYear.ToString();
                var references = project.IsTangible ? db.ProjectPlanItems.Where(d => d.FKItemReference.FKItemTypeReference.UACSObjectClass == project.UACS && d.FundSource == project.FundSource && d.Status == "Approved" && d.FKPPMPReference.FiscalYear == FiscalYear).Select(d => d.FKPPMPReference.ReferenceNo).Distinct().ToList() : db.ProjectPlanServices.Where(d => d.FKItemReference.FKItemTypeReference.UACSObjectClass == project.UACS && d.FundSource == project.FundSource && d.Status == "Approved" && d.FKPPMPReference.FiscalYear == FiscalYear).Select(d => d.FKPPMPReference.ReferenceNo).Distinct().ToList();
                project.Remarks = "PPMP References:\n";
                project.UACSSubClass = project.UACS.Substring(0, project.UACS.Length - 5) + "00000";
                project.ObjectSubClassification = abisDataAccess.GetChartOfAccounts(project.UACS).SubAcctName;
                foreach (var item in references)
                {
                    project.Remarks += item + "\n";
                }
            }


            return lessThanThresholdProjects;
        }
        private List<ApprovedItems> WithinThresholdProjects(int FiscalYear)
        {
            var withinThresholdProjects = new List<ApprovedItems>();
            var chartOfAccounts = abisDataAccess.GetChartOfAccounts().ToList();
            var fundSources = abisDataAccess.GetFundSources().ToList();
            var withinThresholdItems = (from chart in chartOfAccounts
                                        join items in db.ProjectPlanItems.ToList() on chart.UACS_Code equals items.FKItemReference.FKItemTypeReference.UACSObjectClass
                                        join funds in fundSources on items.FundSource equals funds.FUND_CLUSTER
                                        where items.UnitCost >= 15000.00m && items.Status == "Approved" &&
                                              items.FKItemReference.ResponsibilityCenter != null &&
                                              funds.FUND_DESC.Contains("General Fund")
                                        select new
                                        {
                                            IsTangible = items.FKItemReference.FKItemTypeReference.FKInventoryTypeReference.IsTangible,
                                            IsInstitutional = items.FKItemReference.ResponsibilityCenter == null ? false : true,
                                            ItemCode = items.FKItemReference.FKItemTypeReference.ItemTypeCode,
                                            ItemName = items.FKItemReference.FKItemTypeReference.ItemTypeName,
                                            ItemSpecification = items.FKItemReference.ItemSpecifications,
                                            ProcurementSource = items.FKItemReference.ProcurementSource,
                                            UACS = items.FKItemReference.FKItemTypeReference.UACSObjectClass,
                                            EstimatedBudget = items.PPMPEstimatedBudget,
                                            FundSource = items.FundSource,
                                            ResponsibilityCenter = items.FKItemReference.ResponsibilityCenter == null ? items.FKPPMPReference.Department : items.FKItemReference.ResponsibilityCenter,
                                            InventoryCode = items.FKItemReference.FKItemTypeReference.FKInventoryTypeReference.InventoryCode,
                                            Month = items.FKProjectReference.ProjectMonthStart,
                                            MOOE = chart.ClassCode == "5" ? items.PPMPEstimatedBudget : 0.00m,
                                            CapitalOutlay = chart.ClassCode == "1" ? items.PPMPEstimatedBudget : 0.00m
                                        } into result
                                        group result by new
                                        {
                                            result.ItemCode,
                                            result.ItemName,
                                            result.FundSource,
                                            result.UACS,
                                            result.IsTangible,
                                            result.InventoryCode,
                                            result.IsInstitutional,
                                            result.ResponsibilityCenter,
                                            result.ProcurementSource
                                        } into groupedResult
                                        select new ApprovedItems
                                        {
                                            IsTangible = groupedResult.Key.IsTangible,
                                            IsInstitutional = groupedResult.Key.IsInstitutional,
                                            ItemCode = groupedResult.Key.ItemCode,
                                            ItemName = groupedResult.Key.ItemName,
                                            UACS = groupedResult.Key.UACS,
                                            EstimatedBudget = groupedResult.Sum(x => x.EstimatedBudget),
                                            FundSource = groupedResult.Key.FundSource,
                                            MOOE = groupedResult.Sum(x => x.MOOE),
                                            CapitalOutlay = groupedResult.Sum(x => x.CapitalOutlay),
                                            InventoryCode = groupedResult.Key.InventoryCode,
                                            Month = groupedResult.Max(x => x.Month),
                                            EndUser = groupedResult.Key.ResponsibilityCenter,
                                            ProcurementSource = groupedResult.Key.ProcurementSource
                                        }).ToList();

            var withinThresholdItemsOtherFunds = (from chart in chartOfAccounts
                                                  join items in db.ProjectPlanItems.ToList() on chart.UACS_Code equals items.FKItemReference.FKItemTypeReference.UACSObjectClass
                                                  join funds in fundSources on items.FundSource equals funds.FUND_CLUSTER
                                                  where items.UnitCost >= 15000.00m && items.Status == "Approved" &&
                                                        !funds.FUND_DESC.Contains("General Fund")
                                                  select new
                                                  {
                                                      IsTangible = items.FKItemReference.FKItemTypeReference.FKInventoryTypeReference.IsTangible,
                                                      IsInstitutional = items.FKItemReference.ResponsibilityCenter == null ? false : true,
                                                      ItemCode = items.FKItemReference.FKItemTypeReference.ItemTypeCode,
                                                      ItemName = items.FKItemReference.FKItemTypeReference.ItemTypeName,
                                                      ItemSpecification = items.FKItemReference.ItemSpecifications,
                                                      ProcurementSource = items.FKItemReference.ProcurementSource,
                                                      UACS = items.FKItemReference.FKItemTypeReference.UACSObjectClass,
                                                      EstimatedBudget = items.PPMPEstimatedBudget,
                                                      FundSource = items.FundSource,
                                                      ResponsibilityCenter = items.FKPPMPReference.Department,
                                                      InventoryCode = items.FKItemReference.FKItemTypeReference.FKInventoryTypeReference.InventoryCode,
                                                      Month = items.FKProjectReference.ProjectMonthStart,
                                                      MOOE = chart.ClassCode == "5" ? items.PPMPEstimatedBudget : 0.00m,
                                                      CapitalOutlay = chart.ClassCode == "1" ? items.PPMPEstimatedBudget : 0.00m
                                                  } into result
                                                  group result by new
                                                  {
                                                      result.ItemCode,
                                                      result.ItemName,
                                                      result.FundSource,
                                                      result.UACS,
                                                      result.IsTangible,
                                                      result.InventoryCode,
                                                      result.IsInstitutional,
                                                      result.ResponsibilityCenter,
                                                      result.ProcurementSource
                                                  } into groupedResult
                                                  select new ApprovedItems
                                                  {
                                                      IsTangible = groupedResult.Key.IsTangible,
                                                      IsInstitutional = groupedResult.Key.IsInstitutional,
                                                      ItemCode = groupedResult.Key.ItemCode,
                                                      ItemName = groupedResult.Key.ItemName,
                                                      UACS = groupedResult.Key.UACS,
                                                      EstimatedBudget = groupedResult.Sum(x => x.EstimatedBudget),
                                                      FundSource = groupedResult.Key.FundSource,
                                                      MOOE = groupedResult.Sum(x => x.MOOE),
                                                      CapitalOutlay = groupedResult.Sum(x => x.CapitalOutlay),
                                                      InventoryCode = groupedResult.Key.InventoryCode,
                                                      Month = groupedResult.Max(x => x.Month),
                                                      EndUser = groupedResult.Key.ResponsibilityCenter,
                                                      ProcurementSource = groupedResult.Key.ProcurementSource
                                                  }).ToList();

            var withinThresholdServices = (from chart in chartOfAccounts
                                           join items in db.ProjectPlanServices.ToList() on chart.UACS_Code equals items.FKItemReference.FKItemTypeReference.UACSObjectClass
                                           join funds in fundSources on items.FundSource equals funds.FUND_CLUSTER
                                           where items.UnitCost >= 15000.00m && items.Status == "Approved" &&
                                                 funds.FUND_DESC.Contains("General Fund")
                                           select new
                                           {
                                               IsTangible = items.FKItemReference.FKItemTypeReference.FKInventoryTypeReference.IsTangible,
                                               IsInstitutional = items.FKItemReference.ResponsibilityCenter == null ? false : true,
                                               ItemCode = items.FKItemReference.FKItemTypeReference.ItemTypeCode,
                                               ItemName = items.FKItemReference.ItemFullName + " - " + items.FKProjectReference.ProjectName,
                                               ItemSpecification = items.FKItemReference.ItemSpecifications,
                                               ProcurementSource = items.FKItemReference.ProcurementSource,
                                               UACS = items.FKItemReference.FKItemTypeReference.UACSObjectClass,
                                               EstimatedBudget = items.PPMPEstimatedBudget,
                                               FundSource = items.FundSource,
                                               ResponsibilityCenter = items.FKItemReference.ResponsibilityCenter == null ? items.FKPPMPReference.Department : items.FKItemReference.ResponsibilityCenter,
                                               InventoryCode = items.FKItemReference.FKItemTypeReference.FKInventoryTypeReference.InventoryCode,
                                               Month = items.FKProjectReference.ProjectMonthStart,
                                               MOOE = chart.ClassCode == "5" ? items.PPMPEstimatedBudget : 0.00m,
                                               CapitalOutlay = chart.ClassCode == "1" ? items.PPMPEstimatedBudget : 0.00m
                                           } into result
                                           group result by new
                                           {
                                               result.ItemCode,
                                               result.ItemName,
                                               result.FundSource,
                                               result.UACS,
                                               result.IsTangible,
                                               result.InventoryCode,
                                               result.IsInstitutional,
                                               result.ResponsibilityCenter,
                                               result.ProcurementSource
                                           } into groupedResult
                                           select new ApprovedItems
                                           {
                                               IsTangible = groupedResult.Key.IsTangible,
                                               IsInstitutional = groupedResult.Key.IsInstitutional,
                                               ItemCode = groupedResult.Key.ItemCode,
                                               ItemName = groupedResult.Key.ItemName,
                                               UACS = groupedResult.Key.UACS,
                                               EstimatedBudget = groupedResult.Sum(x => x.EstimatedBudget),
                                               FundSource = groupedResult.Key.FundSource,
                                               MOOE = groupedResult.Sum(x => x.MOOE),
                                               CapitalOutlay = groupedResult.Sum(x => x.CapitalOutlay),
                                               InventoryCode = groupedResult.Key.InventoryCode,
                                               Month = groupedResult.Max(x => x.Month),
                                               EndUser = groupedResult.Key.ResponsibilityCenter,
                                               ProcurementSource = groupedResult.Key.ProcurementSource
                                           }).ToList();

            var withinThresholdServicesOtherFunds = (from chart in chartOfAccounts
                                                     join items in db.ProjectPlanServices.ToList() on chart.UACS_Code equals items.FKItemReference.FKItemTypeReference.UACSObjectClass
                                                     join funds in fundSources on items.FundSource equals funds.FUND_CLUSTER
                                                     where items.UnitCost >= 15000.00m && items.Status == "Approved" &&
                                                           !funds.FUND_DESC.Contains("General Fund")
                                                     select new
                                                     {
                                                         IsTangible = items.FKItemReference.FKItemTypeReference.FKInventoryTypeReference.IsTangible,
                                                         IsInstitutional = items.FKItemReference.ResponsibilityCenter == null ? false : true,
                                                         ItemCode = items.FKItemReference.FKItemTypeReference.ItemTypeCode,
                                                         ItemName = items.FKItemReference.ItemFullName + " - " + items.FKProjectReference.ProjectName,
                                                         ItemSpecification = items.FKItemReference.ItemSpecifications,
                                                         ProcurementSource = items.FKItemReference.ProcurementSource,
                                                         UACS = items.FKItemReference.FKItemTypeReference.UACSObjectClass,
                                                         EstimatedBudget = items.PPMPEstimatedBudget,
                                                         FundSource = items.FundSource,
                                                         ResponsibilityCenter = items.FKItemReference.ResponsibilityCenter == null ? items.FKPPMPReference.Department : items.FKItemReference.ResponsibilityCenter,
                                                         InventoryCode = items.FKItemReference.FKItemTypeReference.FKInventoryTypeReference.InventoryCode,
                                                         Month = items.FKProjectReference.ProjectMonthStart,
                                                         MOOE = chart.ClassCode == "5" ? items.PPMPEstimatedBudget : 0.00m,
                                                         CapitalOutlay = chart.ClassCode == "1" ? items.PPMPEstimatedBudget : 0.00m
                                                     } into result
                                                     group result by new
                                                     {
                                                         result.ItemCode,
                                                         result.ItemName,
                                                         result.FundSource,
                                                         result.UACS,
                                                         result.IsTangible,
                                                         result.InventoryCode,
                                                         result.IsInstitutional,
                                                         result.ResponsibilityCenter,
                                                         result.ProcurementSource
                                                     } into groupedResult
                                                     select new ApprovedItems
                                                     {
                                                         IsTangible = groupedResult.Key.IsTangible,
                                                         IsInstitutional = groupedResult.Key.IsInstitutional,
                                                         ItemCode = groupedResult.Key.ItemCode,
                                                         ItemName = groupedResult.Key.ItemName,
                                                         UACS = groupedResult.Key.UACS,
                                                         EstimatedBudget = groupedResult.Sum(x => x.EstimatedBudget),
                                                         FundSource = groupedResult.Key.FundSource,
                                                         MOOE = groupedResult.Sum(x => x.MOOE),
                                                         CapitalOutlay = groupedResult.Sum(x => x.CapitalOutlay),
                                                         InventoryCode = groupedResult.Key.InventoryCode,
                                                         Month = groupedResult.Max(x => x.Month),
                                                         EndUser = groupedResult.Key.ResponsibilityCenter,
                                                         ProcurementSource = groupedResult.Key.ProcurementSource
                                                     }).ToList();

            withinThresholdProjects.AddRange(withinThresholdItems);
            withinThresholdProjects.AddRange(withinThresholdItemsOtherFunds);
            withinThresholdProjects.AddRange(withinThresholdServices);
            withinThresholdProjects.AddRange(withinThresholdServicesOtherFunds);

            for (int i = 0; i < withinThresholdProjects.Count; i++)
            {
                var itemCode = withinThresholdProjects[i].ItemCode;
                if (withinThresholdProjects[i].ProcurementSource == ProcurementSources.PS_DBM)
                {
                    withinThresholdProjects[i].ModeOfProcurement = new string[] { "1", "10" };
                }
                else
                {
                    withinThresholdProjects[i].ModeOfProcurement = new string[] { "1" };
                }
                withinThresholdProjects[i].ItemCode = withinThresholdProjects[i].IsTangible ? withinThresholdProjects[i].ItemCode : withinThresholdProjects[i].ItemCode + "-" + i;
                withinThresholdProjects[i].EndUserName = hrisDataAccess.GetDepartmentDetails(withinThresholdProjects[i].EndUser).Department;
                withinThresholdProjects[i].ObjectClassification = abisDataAccess.GetChartOfAccounts(withinThresholdProjects[i].UACS).AcctName;
                withinThresholdProjects[i].FundDescription = abisDataAccess.GetFundSources(withinThresholdProjects[i].FundSource).FUND_DESC;
                withinThresholdProjects[i].Schedule = (withinThresholdProjects[i].Month <= 5 ? "October " + (FiscalYear - 1) : systemBDL.GetMonthName(withinThresholdProjects[i].Month - 5)) + " - " + systemBDL.GetMonthName(withinThresholdProjects[i].Month) + " " + FiscalYear.ToString();
                var projectName = withinThresholdProjects[i].ItemName;
                var references = withinThresholdProjects[i].IsTangible ?
                        db.ProjectPlanItems.Where(d => d.FKItemReference.FKItemTypeReference.ItemTypeCode == itemCode && d.Status == "Approved" && d.FKPPMPReference.FiscalYear == FiscalYear).Select(d => d.FKPPMPReference.ReferenceNo).Distinct().ToList() :
                        db.ProjectPlanServices.Where(d => d.FKItemReference.FKItemTypeReference.ItemTypeCode == itemCode && d.Status == "Approved" && d.FKPPMPReference.FiscalYear == FiscalYear && d.FKProjectReference.ProjectName == projectName).Select(d => d.FKPPMPReference.ReferenceNo).Distinct().ToList();
                withinThresholdProjects[i].Remarks = "PPMP References:\n";
                withinThresholdProjects[i].UACSSubClass = withinThresholdProjects[i].UACS.Substring(0, withinThresholdProjects[i].UACS.Length - 5) + "00000";
                withinThresholdProjects[i].ObjectSubClassification = abisDataAccess.GetChartOfAccounts(withinThresholdProjects[i].UACS).SubAcctName;
                foreach (var item in references)
                {
                    withinThresholdProjects[i].Remarks += item + "\n";
                }
            }

            return withinThresholdProjects;
        }
        public List<APPLineItemVM> ConsolidateAPP (int FiscalYear)
        {
            List<APPLineItemVM> APPLineItems = new List<APPLineItemVM>();

            var commonSuppliesProject = CommonSuppliesProjects(FiscalYear);
            var lessThanThresholdProjects = LessThanThresholdProjects(FiscalYear);
            var withinThresholdProjects = WithinThresholdProjects(FiscalYear);

            var lineItems = commonSuppliesProject
                            .Union(lessThanThresholdProjects)
                            .Union(withinThresholdProjects)
                            .ToList();

            var objectClasses = lineItems.Select(d => new { UACS = d.UACSSubClass, ObjectClass = d.ObjectSubClassification })
                                .GroupBy(d => new { d.UACS, d.ObjectClass })
                                .Select(d => new { UACS = d.Key.UACS, ObjectClass = d.Key.ObjectClass })
                                .ToList();

            foreach(var objectClass in objectClasses)
            {
                APPLineItems.Add(new APPLineItemVM {
                    UACS = objectClass.UACS,
                    ObjectClassification = objectClass.ObjectClass,
                    ApprovedItems = lineItems.Where(d => d.UACSSubClass == objectClass.UACS).ToList()
                });
            }

            return APPLineItems;
        }
        public List<AnnualProcurementPlanDetails> ExtractProcurementPrograms(List<APPLineItemVM> APPLineItems, int FiscalYear)
        {
            var objectClasses = APPLineItems.GroupBy(d => new { UACS = d.UACS, ObjectClass = d.ObjectClassification }).ToList();
            var procurementPrograms = new List<AnnualProcurementPlanDetails>();

            foreach (var lineItem in APPLineItems)
            {
                var itemCount = 1;
                foreach (var project in lineItem.ApprovedItems)
                {
                    string modesOfProcurement = string.Empty;
                    for (int i = 0; i < project.ModeOfProcurement.Count(); i++)
                    {
                        modesOfProcurement += i == project.ModeOfProcurement.Count() - 1 ? project.ModeOfProcurement[i] : project.ModeOfProcurement[i] + "_";
                    }
                    procurementPrograms.Add(new AnnualProcurementPlanDetails
                    {
                        PAPCode = project.IsInstitutional ? project.InventoryCode + "-UNIV-" + itemCount.ToString() : project.InventoryCode + "-" + project.EndUser + "-" + itemCount.ToString(),
                        ProcurementProgram = project.ItemName.ToUpper(),
                        APPModeOfProcurementReference = modesOfProcurement,
                        ObjectClassification = project.UACS,
                        EndUser = project.EndUser,
                        Month = project.Month,
                        StartMonth = (project.Month <= 5 ? "October " + (FiscalYear - 1) : systemBDL.GetMonthName(project.Month - 5)),
                        EndMonth = systemBDL.GetMonthName(project.Month) + " " + FiscalYear.ToString(),
                        FundSourceReference = project.FundSource,
                        MOOEAmount = project.MOOE,
                        COAmount = project.CapitalOutlay,
                        Total = project.EstimatedBudget,
                        Remarks = project.Remarks,
                        IsInstitutional = project.IsInstitutional,
                        IsTangible = project.IsTangible
                    });
                    itemCount++;
                }
            }

            return procurementPrograms;
        }
        public bool PostAPP(List<APPLineItemVM> APPLineItems, int FiscalYear, string UserEmail)
        {
            SystemBDL sysBDL = new SystemBDL();
            var procurementPrograms = ExtractProcurementPrograms(APPLineItems, FiscalYear);
            var user = db.UserAccounts.Where(d => d.Email == UserEmail).FirstOrDefault();
            var agencyDetails = db.AgencyDetails.FirstOrDefault();
            var hope = hrisDataAccess.GetDepartmentDetails(agencyDetails.HOPEReference);
            var property = hrisDataAccess.GetDepartmentDetails(agencyDetails.PropertyOfficeReference);
            var accounting = hrisDataAccess.GetDepartmentDetails(agencyDetails.AccountingOfficeReference);
            var procurement = hrisDataAccess.GetDepartmentDetails(agencyDetails.ProcurementOfficeReference);
            var bac = hrisDataAccess.GetDepartmentDetails(agencyDetails.BACOfficeReference);

            var APPType = db.APPHeader.Where(d => d.FiscalYear == FiscalYear && d.APPType == "Indicative").Count() == 0 ? "Indicative" : db.APPHeader.Where(d => d.FiscalYear == FiscalYear && d.APPType == "Original").Count() == 0 ? "Original" : "Supplemental";
            var APPHeader = new AnnualProcurementPlan()
            {
                FiscalYear = FiscalYear,
                APPType = APPType,
                ReferenceNo = GenerateReferenceNo(FiscalYear, APPType),
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

            db.APPHeader.Add(APPHeader);
            if (db.SaveChanges() == 0)
            {
                return false;
            }

            procurementPrograms.ForEach(d => { d.APPHeaderReference = APPHeader.ID; d.PAPCode = FiscalYear.ToString().Substring(2, 2) + (APPHeader.APPType == "Original" || APPHeader.APPType == "Indicative" ?  "-O-" : "-S-") + d.PAPCode; });
            db.APPDetails.AddRange(procurementPrograms);
            if (db.SaveChanges() == 0)
            {
                return false;
            }

            var procurementProgramItems = procurementPrograms.Where(d => d.IsTangible == true).ToList();
            var procurementProgramServices = procurementPrograms.Where(d => d.IsTangible == false).ToList();

            foreach(var item in procurementProgramItems)
            {
                if(item.PAPCode.Contains("CUOS"))
                {
                    var projectItems = db.ProjectPlanItems
                                       .Where(d => d.FKItemReference.FKItemTypeReference.UACSObjectClass == item.ObjectClassification &&
                                                   d.FKPPMPReference.FiscalYear == FiscalYear &&
                                                   d.FundSource == item.FundSourceReference &&
                                                   (d.Status == "Approved" || d.Status == "Posted to APP"))
                                       .ToList();

                    projectItems.ForEach(d => { d.APPLineReference = item.ID; d.Status = "Posted to APP"; });
                }
                else
                {
                    var projectItems = db.ProjectPlanItems
                                       .Where(d => d.FKItemReference.FKItemTypeReference.UACSObjectClass == item.ObjectClassification &&
                                                   d.FKPPMPReference.FiscalYear == FiscalYear &&
                                                   d.FundSource == item.FundSourceReference &&
                                                   d.Status == "Approved")
                                       .ToList();

                    projectItems.ForEach(d => { d.APPReference = APPHeader.ID; d.APPLineReference = item.ID; d.Status = "Posted to APP"; });
                }

            }

            foreach (var item in procurementProgramServices)
            {
                var projectServices = db.ProjectPlanServices
                                   .Where(d => d.FKItemReference.FKItemTypeReference.UACSObjectClass == item.ObjectClassification &&
                                               d.FKPPMPReference.FiscalYear == FiscalYear &&
                                               d.FundSource == item.FundSourceReference &&
                                               d.Status == "Approved")
                                   .ToList();

                projectServices.ForEach(d => { d.APPReference = APPHeader.ID; d.APPLineReference = item.ID; d.Status = "Posted to APP"; });
            }

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
                db.Dispose();
                abisDataAccess.Dispose();
                hrisDataAccess.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}