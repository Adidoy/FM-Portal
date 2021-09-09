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
            reports.AddDoubleColumnHeader(LogoPath, false, 1.25);
            reports.AddColumnHeader(
                new HeaderLine { Content = "", Bold = false, Italic = false, FontSize = 5, ParagraphAlignment = ParagraphAlignment.Left },
                new HeaderLine { Content = "", Bold = true, Italic = false, FontSize = 5, ParagraphAlignment = ParagraphAlignment.Left }
            );
            reports.AddColumnHeader(
                new HeaderLine { Content = "Republic of the Philippines", Bold = false, Italic = false, FontSize = 8, ParagraphAlignment = ParagraphAlignment.Left },
                new HeaderLine { Content = "REFERENCE NO: " + ReferenceNo, Bold = true, Italic = false, FontSize = 8, ParagraphAlignment = ParagraphAlignment.Left }
            );
            reports.AddColumnHeader(
                new HeaderLine { Content = "POLYTECHNIC UNIVERSITY OF THE PHILIPPINES", Bold = true, Italic = false, FontSize = 12, ParagraphAlignment = ParagraphAlignment.Left },
                new HeaderLine { Content = "Date Printed: " + DateTime.Now.ToString("dd MMMM yyyy hh:mm tt"), Bold = false, Italic = false, FontSize = 8, ParagraphAlignment = ParagraphAlignment.Left }
            );
            reports.AddColumnHeader(
                new HeaderLine { Content = "A. Mabini Campus, Anonas St., Santa Mesa, Manila\t1016", Bold = false, Italic = false, FontSize = 10, ParagraphAlignment = ParagraphAlignment.Left },
                new HeaderLine { Content = "", Bold = false, Italic = false, FontSize = 8, ParagraphAlignment = ParagraphAlignment.Left }
            );

            reports.AddSingleColumnHeader();
            reports.AddColumnHeader(
                new HeaderLine { Content = APPViewModel.APPType.ToUpper(), Bold = true, Italic = false, FontSize = 10, ParagraphAlignment = ParagraphAlignment.Center }
            );
            reports.AddColumnHeader(
                new HeaderLine { Content = "Fiscal Year " + APPViewModel.FiscalYear.ToString(), Bold = false, Italic = false, FontSize = 8, ParagraphAlignment = ParagraphAlignment.Center }
            );

            reports.AddNewLine();

            var columns = new List<ContentColumn>();
            columns.Add(new ContentColumn(1.00, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
            columns.Add(new ContentColumn(1.65, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
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
            foreach (var acct in accounts)
            {
                columns = new List<ContentColumn>();
                columns.Add(new ContentColumn(12.5, new MigraDoc.DocumentObjectModel.Color(252, 207, 101)));
                reports.AddTable(columns, true);

                rows = new List<ContentCell>();
                rows.Add(new ContentCell(Convert.ToChar(count + 65).ToString() + ". " + acct, 0, 10, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Left, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                reports.AddRowContent(rows, 0.25);
                count++;

                columns = new List<ContentColumn>();
                columns.Add(new ContentColumn(1.00));
                columns.Add(new ContentColumn(1.65));
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
                    rows.Add(new ContentCell(item.EstimatedBudget.ToString("C", new System.Globalization.CultureInfo("en-ph")), 9, 8, false, false, ParagraphAlignment.Right, VerticalAlignment.Center));
                    rows.Add(new ContentCell(item.MOOE.ToString("C", new System.Globalization.CultureInfo("en-ph")), 10, 8, false, false, ParagraphAlignment.Right, VerticalAlignment.Center));
                    rows.Add(new ContentCell(item.CapitalOutlay.ToString("C", new System.Globalization.CultureInfo("en-ph")), 11, 8, false, false, ParagraphAlignment.Right, VerticalAlignment.Center));
                    rows.Add(new ContentCell(item.Remarks == null ? string.Empty : item.Remarks, 12, 6, false, false, ParagraphAlignment.Left, VerticalAlignment.Top));
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
                rows.Add(new ContentCell(APPViewModel.APPLineItems.Where(d => d.ObjectClassification == acct).Sum(d => d.EstimatedBudget).ToString("C", new System.Globalization.CultureInfo("en-ph")), 1, 9, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Right, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center, 0, 0, false, new Color(255, 255, 255)));
                rows.Add(new ContentCell(APPViewModel.APPLineItems.Where(d => d.ObjectClassification == acct).Sum(d => d.MOOE).ToString("C", new System.Globalization.CultureInfo("en-ph")), 2, 9, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Right, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center, 0, 0, false, new Color(255, 255, 255)));
                rows.Add(new ContentCell(APPViewModel.APPLineItems.Where(d => d.ObjectClassification == acct).Sum(d => d.CapitalOutlay).ToString("C", new System.Globalization.CultureInfo("en-ph")), 3, 9, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Right, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center, 0, 0, false, new Color(255, 255, 255)));
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
            rows.Add(new ContentCell(APPViewModel.APPLineItems.Sum(d => d.EstimatedBudget).ToString("C", new System.Globalization.CultureInfo("en-ph")), 1, 9, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Right, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center, 0, 0, false, new Color(255, 255, 255)));
            rows.Add(new ContentCell(APPViewModel.APPLineItems.Sum(d => d.MOOE).ToString("C", new System.Globalization.CultureInfo("en-ph")), 2, 9, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Right, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center, 0, 0, false, new Color(255, 255, 255)));
            rows.Add(new ContentCell(APPViewModel.APPLineItems.Sum(d => d.CapitalOutlay).ToString("C", new System.Globalization.CultureInfo("en-ph")), 3, 9, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Right, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center, 0, 0, false, new Color(255, 255, 255)));
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
        private HRISDataAccess hris = new HRISDataAccess();
        private ABISDataAccess abis = new ABISDataAccess();
        private SystemBDL systemBDL = new SystemBDL();

        public List<int> GetPPMPFiscalYears()
        {
            var fiscalYears = db.PPMPHeader.Where(d => d.PPMPStatus == PPMPStatus.EvaluatedByBudgetOffice).Select(d => d.FiscalYear).Distinct().ToList();
            return fiscalYears;
        }
        public List<int> GetCSEFiscalYears()
        {
            var fiscalYears = db.PPMPDetails.Where(d => d.FKPPMPHeaderReference.PPMPStatus == PPMPStatus.EvaluatedByBudgetOffice && d.FKItemArticleReference.FKItemTypeReference.FKItemClassificationReference.ID == 1).GroupBy(d => d.FKPPMPHeaderReference.FiscalYear).Select(d => d.Key).ToList();
            return fiscalYears;
        }
        public List<int> GetAPPCSEFiscalYears()
        {
            var fiscalYears = db.APPHeader.Where(d => d.APPType == APPTypes.APPCSE).Select(d => d.FiscalYear).Distinct().ToList();
            return fiscalYears;
        }
        public List<int> GetAPPFiscalYears()
        {
            var fiscalYears = db.APPHeader.Select(d => d.FiscalYear).Distinct().ToList();
            return fiscalYears;
        }
        public List<ModesOfProcurement> GetModesOfProcurement()
        {
            return db.ProcurementModes.ToList();
        }
        public AnnualProcurementPlanVM GetAnnualProcurementPlan(string ReferenceNo)
        {
            AgencyDetails agencyDetails = db.AgencyDetails.First();
            var APPHeader = db.APPHeader.Where(d => d.ReferenceNo == ReferenceNo).FirstOrDefault();
            var APPDetails = new List<AnnualProcurementPlanDetailsVM>();
            var ProcurementPrograms = db.APPDetails.Where(d => d.FKAPPHeaderReference.ReferenceNo == ReferenceNo).ToList();

            foreach (var item in ProcurementPrograms)
            {
                var procurementModes = item.APPModeOfProcurementReference.Split("_".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).ToList();
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

                APPDetails.Add(new AnnualProcurementPlanDetailsVM
                {
                    UACS = item.ObjectSubClassification,
                    ObjectClassification = abis.GetChartOfAccounts(item.ObjectClassification).SubAcctName,
                    PAPCode = item.PAPCode,
                    ProcurementProject = item.ProcurementProgram,
                    ModeOfProcurement = modesOfProcurement,
                    EndUser = item.EndUser,
                    Month = item.Month,
                    StartMonth = item.StartMonth,
                    EndMonth = item.EndMonth,
                    FundCluster = item.FundSourceReference,
                    FundDescription = abis.GetFundSources(item.FundSourceReference).FUND_DESC,
                    MOOE = item.MOOEAmount,
                    CapitalOutlay = item.COAmount,
                    EstimatedBudget = item.Total,
                    Remarks = item.Remarks
                });
            }

            var hope = hris.GetDepartmentDetails(APPHeader.ApprovedByDepartmentCode);
            var property = hris.GetDepartmentDetails(agencyDetails.PropertyOfficeReference);
            var accounting = hris.GetDepartmentDetails(agencyDetails.AccountingOfficeReference);
            var procurement = hris.GetDepartmentDetails(APPHeader.PreparedByDepartmentCode);
            var bac = hris.GetDepartmentDetails(APPHeader.RecommendingApprovalDepartmentCode);

            return new AnnualProcurementPlanVM
            {
                APPType = APPHeader.APPType.GetAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>().Name,
                ReferenceNo = APPHeader.ReferenceNo,
                FiscalYear = APPHeader.FiscalYear,
                AccountCode = agencyDetails.AccountCode,
                AgencyName = agencyDetails.AgencyName,
                Address = agencyDetails.Address,
                Region = agencyDetails.Region,
                OrganizationType = agencyDetails.OrganizationType,
                ApprovedBy = APPHeader.ApprovedBy + "\n" + APPHeader.ApprovedByDesignation,
                PreparedBy = APPHeader.PreparedBy + "\n" + APPHeader.PreparedByDesignation + "\n" + hris.GetDepartmentDetails(APPHeader.PreparedByDepartmentCode).Department,
                CertifiedBy = APPHeader.RecommendingApproval + "\n" + APPHeader.RecommendingApprovalDesignation + "\n" + hris.GetDepartmentDetails(APPHeader.RecommendingApprovalDepartmentCode).Section,
                ProcurementOfficer = procurement.DepartmentHead + "\n" + procurement.DepartmentHeadDesignation + "\n" + procurement.Department,
                BACSecretariat = bac.SectionHead + "\n" + bac.SectionHeadDesignation + "\n" + bac.Section,
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
                APPType = d.APPType.GetAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>().Name,
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
            else if (APPHeader.RecommendedAt != APPViewModel.CertifiedAt)
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

            if (db.SaveChanges() == 0)
            {
                return false;
            }

            return true;
        }
        private string GenerateReferenceNo(int FiscalYear, APPTypes APPType)
        {
            string referenceNo = String.Empty;
            var appTypeCode = APPType.GetAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>().ShortName;
            var sequenceNo = (db.APPHeader.Where(d => d.ReferenceNo.Contains("ANPP-" + appTypeCode) && d.FiscalYear == FiscalYear).Count() + 1).ToString();
            sequenceNo = (sequenceNo.Length == 1) ? "00" + sequenceNo : (sequenceNo.Length == 2) ? "0" + sequenceNo : sequenceNo;
            referenceNo = "ANPP-" + appTypeCode + "-" + sequenceNo + "-" + FiscalYear;
            return referenceNo;
        }
        private List<ApprovedItems> CommonSuppliesProjects(int FiscalYear)
        {
            var approvedItems = db.PPMPDetails.Where(d => d.ArticleReference != null).ToList()
                                              .Where(d => d.FKPPMPHeaderReference.PPMPStatus == PPMPStatus.EvaluatedByBudgetOffice &&
                                                                   d.PPMPDetailStatus == PPMPDetailStatus.ItemAccepted &&
                                                                   d.ClassificationReference == 1 &&
                                                                   d.UnitCost < 15000.00m)
                                                       .Select(d => new
                                                       {
                                                           IsInstitutional = d.FKItemArticleReference.FKItemTypeReference.ResponsibilityCenter == null ? false : true,
                                                           ItemSpecification = d.ItemSpecifications,
                                                           Classification = d.ClassificationReference,
                                                           UACS = d.FKPPMPHeaderReference.UACS,
                                                           EstimatedBudget = d.EstimatedBudget,
                                                           FundSource = d.FundSource,
                                                           ResponsibilityCenter = d.FKItemArticleReference.FKItemTypeReference.ResponsibilityCenter == null ? d.FKPPMPHeaderReference.Department : d.FKItemArticleReference.FKItemTypeReference.ResponsibilityCenter,
                                                           InventoryCode = d.FKItemArticleReference.FKItemTypeReference.ItemTypeCode,
                                                           Month = d.FKProjectDetailsReference.FKProjectPlanReference.DeliveryMonth,
                                                           MOOE = d.UnitCost < 15000m ? d.EstimatedBudget : 0.00m,
                                                           CapitalOutlay = d.UnitCost >= 15000m ? d.EstimatedBudget : 0.00m,
                                                           ProcurementSource = d.ProcurementSource,
                                                           EndUser = d.FKItemArticleReference.FKItemTypeReference.ResponsibilityCenter == null ? d.FKPPMPHeaderReference.Department : d.FKItemArticleReference.FKItemTypeReference.ResponsibilityCenter
                                                       })
                                                       .GroupBy(d => new
                                                       {
                                                           d.IsInstitutional,
                                                           d.UACS,
                                                           d.FundSource,
                                                           d.InventoryCode,
                                                           d.EndUser,
                                                           d.ProcurementSource,
                                                           d.Classification
                                                       })
                                                       .Select(d => new ApprovedItems
                                                       {
                                                           IsInstitutional = d.Key.IsInstitutional,
                                                           UACS = d.Key.UACS,
                                                           ItemCode = d.Key.InventoryCode + "-" + d.Key.ProcurementSource,
                                                           ItemName = db.ItemClassification.Find(d.Key.Classification).ProjectPrefix.ToUpper() + " " + (abis.GetChartOfAccounts(d.Key.UACS).AcctName.Replace("Expenses", "") + (d.Key.ProcurementSource == ProcurementSources.AgencyToAgency ? "available at DBM Procurement Service" : "not available at DBM-PS")).ToUpper(),
                                                           ClassificationReference = (int)d.Key.Classification,
                                                           EstimatedBudget = d.Sum(x => x.EstimatedBudget),
                                                           FundSource = d.Key.FundSource,
                                                           MOOE = d.Sum(x => x.MOOE),
                                                           CapitalOutlay = d.Sum(x => x.CapitalOutlay),
                                                           InventoryCode = d.Key.InventoryCode,
                                                           Month = d.Max(x => x.Month),
                                                           EndUser = d.Key.EndUser,
                                                           EndUserName = hris.GetDepartmentDetails(d.Key.EndUser).Department,
                                                           ProcurementSource = d.Key.ProcurementSource,
                                                           ModeOfProcurement = d.Key.ProcurementSource == ProcurementSources.AgencyToAgency ? new string[] { "10" } : new string[] { "1", "5", "14" },
                                                           ObjectClassification = abis.GetChartOfAccounts(d.Key.UACS).AcctName,
                                                           UACSSubClass = d.Key.UACS.Substring(0, d.Key.UACS.Length - 5) + "00000",
                                                           ObjectSubClassification = abis.GetChartOfAccounts(d.Key.UACS).SubAcctName,
                                                           FundDescription = abis.GetFundSources(d.Key.FundSource).FUND_DESC,
                                                           Schedule = (d.Min(x => x.Month) == 1) ? "October " + (FiscalYear - 1) + " - " + systemBDL.GetMonthName(d.Max(x => x.Month)) + " " + FiscalYear.ToString() :
                                                                       (d.Min(x => x.Month) == 2) ? "November " + (FiscalYear - 1) + " - " + systemBDL.GetMonthName(d.Max(x => x.Month)) + " " + FiscalYear.ToString() :
                                                                       (d.Min(x => x.Month) == 3) ? "December " + (FiscalYear - 1) + " - " + systemBDL.GetMonthName(d.Max(x => x.Month)) + " " + FiscalYear.ToString() :
                                                                       systemBDL.GetMonthName(d.Min(x => x.Month) - 3) + " " + FiscalYear.ToString() + " - " + systemBDL.GetMonthName(d.Max(x => x.Month)) + " " + FiscalYear.ToString(),
                                                           Remarks = d.Key.ProcurementSource == ProcurementSources.AgencyToAgency ? "Common-use supplies available at DBM - Procurement Service." : "Common-use supplies not available at DBM-PS but can be procured under Annex H Sec. 52 of R.A. 9184."
                                                       }).ToList();
            return approvedItems;
        }
        private List<ApprovedItems> ConsumablesProjects(int FiscalYear)
        {
            var approvedItems = db.PPMPDetails.Where(d => d.ArticleReference != null).ToList()
                                              .Where(d => d.FKPPMPHeaderReference.PPMPStatus == PPMPStatus.EvaluatedByBudgetOffice &&
                                                                   d.PPMPDetailStatus == PPMPDetailStatus.ItemAccepted &&
                                                                   d.ClassificationReference == 3 &&
                                                                   d.UnitCost < 15000.00m)
                                                       .Select(d => new
                                                       {
                                                           IsInstitutional = d.FKItemArticleReference.FKItemTypeReference.ResponsibilityCenter == null ? false : true,
                                                           ItemSpecification = d.ItemSpecifications,
                                                           Classification = d.ClassificationReference,
                                                           UACS = d.FKPPMPHeaderReference.UACS,
                                                           EstimatedBudget = d.EstimatedBudget,
                                                           FundSource = d.FundSource,
                                                           ResponsibilityCenter = d.FKItemArticleReference.FKItemTypeReference.ResponsibilityCenter == null ? d.FKPPMPHeaderReference.Department : d.FKItemArticleReference.FKItemTypeReference.ResponsibilityCenter,
                                                           InventoryCode = d.FKItemArticleReference.FKItemTypeReference.ItemTypeCode,
                                                           Month = d.FKProjectDetailsReference.FKProjectPlanReference.DeliveryMonth,
                                                           MOOE = d.UnitCost < 15000m ? d.EstimatedBudget : 0.00m,
                                                           CapitalOutlay = d.UnitCost >= 15000m ? d.EstimatedBudget : 0.00m,
                                                           ProcurementSource = d.ProcurementSource,
                                                           EndUser = d.FKItemArticleReference.FKItemTypeReference.ResponsibilityCenter == null ? d.FKPPMPHeaderReference.Department : d.FKItemArticleReference.FKItemTypeReference.ResponsibilityCenter
                                                       })
                                                       .GroupBy(d => new
                                                       {
                                                           d.IsInstitutional,
                                                           d.UACS,
                                                           d.FundSource,
                                                           d.InventoryCode,
                                                           d.EndUser,
                                                           d.ProcurementSource,
                                                           d.Classification
                                                       })
                                                       .Select(d => new ApprovedItems
                                                       {
                                                           IsInstitutional = d.Key.IsInstitutional,
                                                           UACS = d.Key.UACS,
                                                           ItemCode = d.Key.InventoryCode + "-" + d.Key.ProcurementSource,
                                                           ItemName = db.ItemClassification.Find(d.Key.Classification).ProjectPrefix.ToUpper() + " " + (abis.GetChartOfAccounts(d.Key.UACS).AcctName.Replace("Expenses", "") + "(" + db.ItemClassification.Find(d.Key.Classification).Classification + ") " + (d.Key.ProcurementSource == ProcurementSources.AgencyToAgency ? "available at DBM Procurement Service" : "not available at DBM-PS")).ToUpper(),
                                                           ClassificationReference = (int)d.Key.Classification,
                                                           EstimatedBudget = d.Sum(x => x.EstimatedBudget),
                                                           FundSource = d.Key.FundSource,
                                                           MOOE = d.Sum(x => x.MOOE),
                                                           CapitalOutlay = d.Sum(x => x.CapitalOutlay),
                                                           InventoryCode = d.Key.InventoryCode,
                                                           Month = d.Max(x => x.Month),
                                                           EndUser = d.Key.EndUser,
                                                           EndUserName = hris.GetDepartmentDetails(d.Key.EndUser).Department,
                                                           ProcurementSource = d.Key.ProcurementSource,
                                                           ModeOfProcurement = d.Key.ProcurementSource == ProcurementSources.AgencyToAgency ? new string[] { "10" } : new string[] { "1", "5", "14" },
                                                           ObjectClassification = abis.GetChartOfAccounts(d.Key.UACS).AcctName,
                                                           UACSSubClass = d.Key.UACS.Substring(0, d.Key.UACS.Length - 5) + "00000",
                                                           ObjectSubClassification = abis.GetChartOfAccounts(d.Key.UACS).SubAcctName,
                                                           FundDescription = abis.GetFundSources(d.Key.FundSource).FUND_DESC,
                                                           Schedule = (d.Min(x => x.Month) == 1) ? "October " + (FiscalYear - 1) + " - " + systemBDL.GetMonthName(d.Max(x => x.Month)) + " " + FiscalYear.ToString() :
                                                                       (d.Min(x => x.Month) == 2) ? "November " + (FiscalYear - 1) + " - " + systemBDL.GetMonthName(d.Max(x => x.Month)) + " " + FiscalYear.ToString() :
                                                                       (d.Min(x => x.Month) == 3) ? "December " + (FiscalYear - 1) + " - " + systemBDL.GetMonthName(d.Max(x => x.Month)) + " " + FiscalYear.ToString() :
                                                                       systemBDL.GetMonthName(d.Min(x => x.Month) - 3) + " " + FiscalYear.ToString() + " - " + systemBDL.GetMonthName(d.Max(x => x.Month)) + " " + FiscalYear.ToString(),
                                                           Remarks = d.Key.ProcurementSource == ProcurementSources.AgencyToAgency ? "Common-use supplies available at DBM - Procurement Service." : "Common-use supplies not available at DBM-PS but can be procured under Annex H Sec. 52 of R.A. 9184."
                                                       }).ToList();
            return approvedItems;
        }
        private List<ApprovedItems> LessThanThresholdProjects(int FiscalYear)
        {
            var approvedItems = db.PPMPDetails.Where(d => d.ArticleReference != null).ToList()
                                              .Where(d => d.FKPPMPHeaderReference.PPMPStatus == PPMPStatus.EvaluatedByBudgetOffice &&
                                                                   d.PPMPDetailStatus == PPMPDetailStatus.ItemAccepted &&
                                                                   (d.ClassificationReference != 1 && d.ClassificationReference != 3) &&
                                                                   d.UnitCost < 15000.00m)
                                                       .Select(d => new
                                                       {
                                                           IsInstitutional = d.FKItemArticleReference.FKItemTypeReference.ResponsibilityCenter == null ? false : true,
                                                           ItemSpecification = d.ItemSpecifications,
                                                           Classification = d.ClassificationReference,
                                                           UACS = d.FKPPMPHeaderReference.UACS,
                                                           EstimatedBudget = d.EstimatedBudget,
                                                           FundSource = d.FundSource,
                                                           ResponsibilityCenter = d.FKItemArticleReference.FKItemTypeReference.ResponsibilityCenter == null ? "Various Offices" : d.FKItemArticleReference.FKItemTypeReference.ResponsibilityCenter,
                                                           InventoryCode = d.FKItemArticleReference.FKItemTypeReference.ItemTypeCode,
                                                           Month = d.FKProjectDetailsReference.FKProjectPlanReference.DeliveryMonth,
                                                           MOOE = d.UnitCost < 15000m ? d.EstimatedBudget : 0.00m,
                                                           CapitalOutlay = d.UnitCost >= 15000m ? d.EstimatedBudget : 0.00m,
                                                           ProcurementSource = d.ProcurementSource,
                                                           EndUser = d.FKItemArticleReference.FKItemTypeReference.ResponsibilityCenter == null ? "Various Offices" : d.FKItemArticleReference.FKItemTypeReference.ResponsibilityCenter
                                                       })
                                                       .GroupBy(d => new
                                                       {
                                                           d.IsInstitutional,
                                                           d.UACS,
                                                           d.FundSource,
                                                           d.InventoryCode,
                                                           d.EndUser,
                                                           d.ProcurementSource,
                                                           d.Classification
                                                       })
                                                       .Select(d => new ApprovedItems
                                                       {
                                                           IsInstitutional = d.Key.IsInstitutional,
                                                           UACS = d.Key.UACS,
                                                           ItemCode = d.Key.InventoryCode + "-" + d.Key.ProcurementSource,
                                                           ItemName = db.ItemClassification.Find(d.Key.Classification).ProjectPrefix.ToUpper() + " " + abis.GetChartOfAccounts(d.Key.UACS).AcctName.ToUpper() + (d.Key.ProcurementSource == ProcurementSources.AgencyToAgency ? " available at DBM Procurement Service" : " not available at DBM-PS").ToUpper(),
                                                           ClassificationReference = (int)d.Key.Classification,
                                                           EstimatedBudget = d.Sum(x => x.EstimatedBudget),
                                                           FundSource = d.Key.FundSource,
                                                           MOOE = d.Sum(x => x.MOOE),
                                                           CapitalOutlay = d.Sum(x => x.CapitalOutlay),
                                                           InventoryCode = d.Key.InventoryCode,
                                                           Month = d.Min(x => x.Month),
                                                           EndUser = d.Key.EndUser,
                                                           EndUserName = d.Key.EndUser == "Various Offices" ? "Various Offices" : hris.GetDepartmentDetails(d.Key.EndUser).Department,
                                                           ProcurementSource = d.Key.ProcurementSource,
                                                           ModeOfProcurement = d.Key.ProcurementSource == ProcurementSources.AgencyToAgency ? new string[] { "10" } : new string[] { "1", "5", "14" },
                                                           ObjectClassification = abis.GetChartOfAccounts(d.Key.UACS).AcctName,
                                                           UACSSubClass = d.Key.UACS.Substring(0, d.Key.UACS.Length - 5) + "00000",
                                                           ObjectSubClassification = abis.GetChartOfAccounts(d.Key.UACS).SubAcctName,
                                                           FundDescription = abis.GetFundSources(d.Key.FundSource).FUND_DESC,
                                                           Schedule = (d.Min(x => x.Month) == 1) ? "October " + (FiscalYear - 1) + " - " + systemBDL.GetMonthName(d.Max(x => x.Month)) + " " + FiscalYear.ToString() :
                                                                      (d.Min(x => x.Month) == 2) ? "November " + (FiscalYear - 1) + " - " + systemBDL.GetMonthName(d.Max(x => x.Month)) + " " + FiscalYear.ToString() :
                                                                      (d.Min(x => x.Month) == 3) ? "December " + (FiscalYear - 1) + " - " + systemBDL.GetMonthName(d.Max(x => x.Month)) + " " + FiscalYear.ToString() :
                                                                      systemBDL.GetMonthName(d.Min(x => x.Month) - 3) + " " + FiscalYear.ToString() + " - " + systemBDL.GetMonthName(d.Max(x => x.Month)) + " " + FiscalYear.ToString()
                                                       }).ToList();
            return approvedItems;
        }
        private List<ApprovedItems> WithinThresholdProjects(int FiscalYear)
        {
            var approvedItems = db.PPMPDetails.Where(d => d.ArticleReference != null).ToList()
                                              .Where(d => d.FKPPMPHeaderReference.PPMPStatus == PPMPStatus.EvaluatedByBudgetOffice &&
                                                         d.PPMPDetailStatus == PPMPDetailStatus.ItemAccepted &&
                                                         (d.ClassificationReference != 1 && d.ClassificationReference != 3) &&
                                                         d.UnitCost >= 15000.00m && d.ArticleReference != null)
                                             .Select(d => new
                                             {
                                                 PAPCode = d.FKProjectDetailsReference.FKProjectPlanReference.PAPCode,
                                                 IsInstitutional = d.FKItemArticleReference.FKItemTypeReference.ResponsibilityCenter == null ? false : true,
                                                 IsInfraProject = d.FKPPMPHeaderReference.IsInfrastructure,
                                                 ItemCode = d.FKItemArticleReference.ArticleCode + "-" + d.ItemSequence,
                                                 ItemName = d.ItemFullName,
                                                 ItemSpecification = d.ItemSpecifications,
                                                 Classification = d.ClassificationReference,
                                                 UACS = d.FKPPMPHeaderReference.UACS,
                                                 EstimatedBudget = d.EstimatedBudget,
                                                 FundSource = d.FundSource,
                                                 ResponsibilityCenter = d.FKItemArticleReference.FKItemTypeReference.ResponsibilityCenter == null ? d.FKPPMPHeaderReference.Department : d.FKItemArticleReference.FKItemTypeReference.ResponsibilityCenter,
                                                 InventoryCode = d.FKItemArticleReference.FKItemTypeReference.ItemTypeCode,
                                                 Month = d.FKProjectDetailsReference.FKProjectPlanReference.DeliveryMonth,
                                                 MOOE = d.FKPPMPHeaderReference.UACS.Substring(0, 1) == "5" ? d.EstimatedBudget : 0.00m,
                                                 CapitalOutlay = d.FKPPMPHeaderReference.UACS.Substring(0, 1) == "1" ? d.EstimatedBudget : 0.00m,
                                                 ProcurementSource = d.ProcurementSource,
                                                 EndUser = d.FKItemArticleReference.FKItemTypeReference.ResponsibilityCenter == null ? d.FKPPMPHeaderReference.Department : d.FKItemArticleReference.FKItemTypeReference.ResponsibilityCenter
                                             })
                                             .GroupBy(d => new
                                             {
                                                 d.IsInstitutional,
                                                 d.IsInfraProject,
                                                 d.ItemCode,
                                                 d.ItemName,
                                                 d.PAPCode,
                                                 d.UACS,
                                                 d.FundSource,
                                                 d.InventoryCode,
                                                 d.EndUser,
                                                 d.ProcurementSource,
                                                 d.Classification
                                             })
                                             .Select(d => new ApprovedItems
                                             {
                                                 PAPCode = d.Key.PAPCode,
                                                 IsInstitutional = d.Key.IsInstitutional,
                                                 ItemCode = d.Key.ItemCode,
                                                 ItemName = d.Key.IsInfraProject == true ? d.Key.ItemName.ToUpper() : db.ItemClassification.Find(d.Key.Classification).ProjectPrefix.ToUpper() + " " + d.Key.ItemName.ToUpper(),
                                                 UACS = d.Key.UACS,
                                                 ClassificationReference = (int)d.Key.Classification,
                                                 EstimatedBudget = d.Sum(x => x.EstimatedBudget),
                                                 FundSource = d.Key.FundSource,
                                                 MOOE = d.Sum(x => x.MOOE),
                                                 CapitalOutlay = d.Sum(x => x.CapitalOutlay),
                                                 InventoryCode = d.Key.InventoryCode,
                                                 Month = d.Max(x => x.Month),
                                                 EndUser = d.Key.EndUser,
                                                 EndUserName = hris.GetDepartmentDetails(d.Key.EndUser).Department,
                                                 ModeOfProcurement = d.Key.ProcurementSource == ProcurementSources.AgencyToAgency ? new string[] { "10" } : new string[] { "1", "5", "14" },
                                                 ProcurementSource = d.Key.ProcurementSource,
                                                 ObjectClassification = abis.GetChartOfAccounts(d.Key.UACS).AcctName,
                                                 UACSSubClass = d.Key.UACS.Substring(0, d.Key.UACS.Length - 5) + "00000",
                                                 ObjectSubClassification = abis.GetChartOfAccounts(d.Key.UACS).SubAcctName,
                                                 FundDescription = abis.GetFundSources(d.Key.FundSource).FUND_DESC,
                                                 Schedule = (d.Max(x => x.Month) == 1) ? "October " + (FiscalYear - 1) + " - " + systemBDL.GetMonthName(d.Max(x => x.Month)) + " " + FiscalYear.ToString() :
                                                            (d.Max(x => x.Month) == 2) ? "November " + (FiscalYear - 1) + " - " + systemBDL.GetMonthName(d.Max(x => x.Month)) + " " + FiscalYear.ToString() :
                                                            (d.Max(x => x.Month) == 3) ? "December " + (FiscalYear - 1) + " - " + systemBDL.GetMonthName(d.Max(x => x.Month)) + " " + FiscalYear.ToString() :
                                                            systemBDL.GetMonthName(d.Max(x => x.Month) - 3) + " " + FiscalYear.ToString() + " - " + systemBDL.GetMonthName(d.Max(x => x.Month)) + " " + FiscalYear.ToString()
                                             }).ToList();
            return approvedItems;
        }
        private List<ApprovedItems> InfrastructureProjects(int FiscalYear)
        {
            var approvedItems = db.PPMPDetails.Where(d => d.UACS == "1060401000").ToList()
                                              .Where(d => d.FKPPMPHeaderReference.PPMPStatus == PPMPStatus.EvaluatedByBudgetOffice &&
                                                          d.PPMPDetailStatus == PPMPDetailStatus.ItemAccepted)
                                             .Select(d => new
                                             {
                                                 PAPCode = d.FKProjectDetailsReference.FKProjectPlanReference.PAPCode,
                                                 IsInstitutional = true,
                                                 ItemCode = d.FKProjectDetailsReference.FKProjectPlanReference.ProjectCode,
                                                 ItemName = d.FKProjectDetailsReference.FKProjectPlanReference.ProjectName.ToUpper(),
                                                 //ItemSpecification = d.ItemSpecifications,
                                                 UACS = d.FKPPMPHeaderReference.UACS,
                                                 EstimatedBudget = d.EstimatedBudget,
                                                 FundSource = d.FundSource,
                                                 ResponsibilityCenter = "PPDO",
                                                 InventoryCode = "INFR",
                                                 Month = d.FKProjectDetailsReference.FKProjectPlanReference.DeliveryMonth,
                                                 MOOE = d.FKPPMPHeaderReference.UACS.Substring(0, 1) == "5" ? d.EstimatedBudget : 0.00m,
                                                 CapitalOutlay = d.FKPPMPHeaderReference.UACS.Substring(0, 1) == "1" ? d.EstimatedBudget : 0.00m,
                                                 ProcurementSource = d.ProcurementSource,
                                                 EndUser = "PPDO"
                                             })
                                             .GroupBy(d => new
                                             {
                                                 d.IsInstitutional,
                                                 d.ItemCode,
                                                 d.ItemName,
                                                 d.PAPCode,
                                                 d.UACS,
                                                 d.FundSource,
                                                 d.InventoryCode,
                                                 d.EndUser,
                                                 d.ProcurementSource
                                             })
                                             .Select(d => new ApprovedItems
                                             {
                                                 PAPCode = d.Key.PAPCode,
                                                 IsInstitutional = d.Key.IsInstitutional,
                                                 ItemCode = d.Key.ItemCode,
                                                 ItemName = db.ItemClassification.Find(8).ProjectPrefix.ToUpper() + " " + d.Key.ItemName.ToUpper(),
                                                 UACS = d.Key.UACS,
                                                 ClassificationReference = 8,
                                                 EstimatedBudget = d.Sum(x => x.EstimatedBudget),
                                                 FundSource = d.Key.FundSource,
                                                 MOOE = d.Sum(x => x.MOOE),
                                                 CapitalOutlay = d.Sum(x => x.CapitalOutlay),
                                                 InventoryCode = d.Key.InventoryCode,
                                                 Month = d.Max(x => x.Month),
                                                 EndUser = d.Key.EndUser,
                                                 EndUserName = hris.GetDepartmentDetails(d.Key.EndUser).Department,
                                                 ModeOfProcurement = d.Key.ProcurementSource == ProcurementSources.AgencyToAgency ? new string[] { "10" } : new string[] { "1", "5", "14" },
                                                 ProcurementSource = d.Key.ProcurementSource,
                                                 ObjectClassification = abis.GetChartOfAccounts(d.Key.UACS).AcctName,
                                                 UACSSubClass = d.Key.UACS.Substring(0, d.Key.UACS.Length - 5) + "00000",
                                                 ObjectSubClassification = abis.GetChartOfAccounts(d.Key.UACS).SubAcctName,
                                                 FundDescription = abis.GetFundSources(d.Key.FundSource).FUND_DESC,
                                                 Schedule = (d.Max(x => x.Month) == 1) ? "October " + (FiscalYear - 1) + " - " + systemBDL.GetMonthName(d.Max(x => x.Month)) + " " + FiscalYear.ToString() :
                                                            (d.Max(x => x.Month) == 2) ? "November " + (FiscalYear - 1) + " - " + systemBDL.GetMonthName(d.Max(x => x.Month)) + " " + FiscalYear.ToString() :
                                                            (d.Max(x => x.Month) == 3) ? "December " + (FiscalYear - 1) + " - " + systemBDL.GetMonthName(d.Max(x => x.Month)) + " " + FiscalYear.ToString() :
                                                            systemBDL.GetMonthName(d.Max(x => x.Month) - 3) + " " + FiscalYear.ToString() + " - " + systemBDL.GetMonthName(d.Max(x => x.Month)) + " " + FiscalYear.ToString()
                                             }).ToList();
            return approvedItems;
        }
        public List<APPLineItemVM> ConsolidateAPP(int FiscalYear)
        {
            var lineItems = new List<ApprovedItems>();
            var appLineItems = new List<APPLineItemVM>();
            lineItems.AddRange(CommonSuppliesProjects(FiscalYear));
            lineItems.AddRange(ConsumablesProjects(FiscalYear));
            lineItems.AddRange(LessThanThresholdProjects(FiscalYear));
            lineItems.AddRange(WithinThresholdProjects(FiscalYear));
            //lineItems.AddRange(InfrastructureProjects(FiscalYear));

            var uacsList = lineItems.Select(d => d.UACS).Distinct().ToList();
            var papCodes = lineItems.Select(d => d.PAPCode).Distinct().ToList();

            appLineItems.AddRange(lineItems.GroupBy(d => new { d.UACSSubClass, d.ObjectSubClassification }).Select(d => new APPLineItemVM
            {
                UACS = d.Key.UACSSubClass,
                ObjectClassification = d.Key.ObjectSubClassification,
                ApprovedItems = lineItems.Where(x => x.UACSSubClass == d.Key.UACSSubClass).ToList()
            }).ToList());

            return appLineItems;
        }
        public bool PostAPP(List<APPLineItemVM> APPLineItems, int FiscalYear, string UserEmail)
        {
            SystemBDL sysBDL = new SystemBDL();
            var user = db.UserAccounts.Where(d => d.Email == UserEmail).FirstOrDefault();
            var agencyDetails = db.AgencyDetails.FirstOrDefault();
            var hope = hris.GetDepartmentDetails(agencyDetails.HOPEReference);
            var property = hris.GetDepartmentDetails(agencyDetails.PropertyOfficeReference);
            var accounting = hris.GetDepartmentDetails(agencyDetails.AccountingOfficeReference);
            var procurement = hris.GetDepartmentDetails(agencyDetails.ProcurementOfficeReference);
            var bac = hris.GetDepartmentDetails(agencyDetails.BACOfficeReference);

            var APPType = db.APPHeader.Where(d => d.FiscalYear == FiscalYear && d.APPType == APPTypes.Indicative).Count() == 0 ? APPTypes.Indicative : db.APPHeader.Where(d => d.FiscalYear == FiscalYear && d.APPType == APPTypes.Original).Count() == 0 ? APPTypes.Original : APPTypes.Supplemental;
            var APPHeader = new AnnualProcurementPlan()
            {
                FiscalYear = FiscalYear,
                APPType = APPType,
                ReferenceNo = GenerateReferenceNo(FiscalYear, APPType),
                PreparedBy = procurement.DepartmentHead,
                PreparedByDepartmentCode = procurement.DepartmentCode,
                PreparedByDesignation = procurement.DepartmentHeadDesignation,
                RecommendingApproval = bac.SectionHead,
                RecommendingApprovalDepartmentCode = bac.SectionCode,
                RecommendingApprovalDesignation = bac.SectionHeadDesignation,
                ApprovedBy = hope.SectorHead,
                ApprovedByDepartmentCode = hope.SectorCode,
                ApprovedByDesignation = hope.SectorHeadDesignation,
                CreatedBy = user.EmpCode,
                CreatedAt = DateTime.Now
            };

            db.APPHeader.Add(APPHeader);
            if (db.SaveChanges() == 0)
            {
                return false;
            }

            var code = FiscalYear.ToString().Substring(2, 2) + ((APPHeader.APPType == APPTypes.Indicative || APPHeader.APPType == APPTypes.Original) ? "-O-" : APPHeader.APPType == APPTypes.Supplemental ? "-S-" : "-A-");
            var appLineItems = APPLineItems.SelectMany(d => d.ApprovedItems)
                                            .ToList()
                                            .Select(d => new AnnualProcurementPlanDetails
                                            {
                                                PAPCode = code + d.PAPCode,
                                                APPHeaderReference = APPHeader.ID,
                                                ProcurementProgram = d.ItemName,
                                                ClassificationReference = d.ClassificationReference,
                                                APPModeOfProcurementReference = string.Join("_", d.ModeOfProcurement),
                                                ObjectClassification = d.UACS,
                                                ObjectSubClassification = d.UACSSubClass,
                                                EndUser = d.EndUser,
                                                Month = d.Month,
                                                InventoryCode = d.InventoryCode,
                                                StartMonth = (d.Month == 1) ? "October " + (FiscalYear - 1) :
                                                             (d.Month == 2) ? "November " + (FiscalYear - 1) :
                                                             (d.Month == 3) ? "December " + (FiscalYear - 1) :
                                                             systemBDL.GetMonthName(d.Month - 3),
                                                EndMonth = systemBDL.GetMonthName(d.Month),
                                                FundSourceReference = d.FundSource,
                                                MOOEAmount = d.MOOE,
                                                COAmount = d.CapitalOutlay,
                                                Total = d.EstimatedBudget,
                                                Remarks = d.Remarks,
                                                ProjectCost = d.EstimatedBudget,
                                                IsInstitutional = d.IsInstitutional,
                                                IsTangible = d.IsTangible,
                                                //ProjectStatus = APPStatus.PostedToAPP,
                                                ProcurementSource = d.ProcurementSource
                                            }).ToList();

            db.APPDetails.AddRange(appLineItems);
            if (db.SaveChanges() == 0)
            {
                return false;
            }

            var appDetails = db.APPDetails.Where(d => d.FKAPPHeaderReference.ID == APPHeader.ID && d.ObjectClassification != "1060401000").ToList();
            foreach (var detail in appDetails)
            {
                var ppmpDetails = db.PPMPDetails.Where(d => d.ArticleReference != null).ToList()
                                  .Where(d => d.FKPPMPHeaderReference.UACS == detail.ObjectClassification &&
                                              d.FKItemArticleReference.FKItemTypeReference.ItemTypeCode == detail.InventoryCode &&
                                              d.ProcurementSource == detail.ProcurementSource &&
                                              d.FundSource == detail.FundSourceReference &&
                                              d.ClassificationReference == detail.ClassificationReference &&
                                              d.PPMPDetailStatus == PPMPDetailStatus.ItemAccepted).ToList();
                ppmpDetails.ForEach(d =>
                {
                    d.FKPPMPHeaderReference.PPMPStatus = PPMPStatus.PostedToAPP;
                    d.FKProjectDetailsReference.ProjectItemStatus = ProjectDetailsStatus.PostedToAPP;
                    d.PPMPDetailStatus = PPMPDetailStatus.PostedToAPP;
                    d.APPDetailReference = detail.ID;
                });
            }
            var appDetailsInfra = db.APPDetails.Where(d => d.FKAPPHeaderReference.ID == APPHeader.ID && d.ObjectClassification == "1060401000").ToList();
            foreach (var detail in appDetailsInfra)
            {
                var ppmpDetails = db.PPMPDetails.ToList()
                                  .Where(d => d.FKPPMPHeaderReference.UACS == detail.ObjectClassification &&
                                              d.ProcurementSource == detail.ProcurementSource &&
                                              d.FundSource == detail.FundSourceReference &&
                                              d.PPMPDetailStatus == PPMPDetailStatus.ItemAccepted).ToList();
                ppmpDetails.ForEach(d =>
                {
                    d.FKPPMPHeaderReference.PPMPStatus = PPMPStatus.PostedToAPP;
                    d.FKProjectDetailsReference.ProjectItemStatus = ProjectDetailsStatus.PostedToAPP;
                    d.PPMPDetailStatus = PPMPDetailStatus.PostedToAPP;
                    d.APPDetailReference = detail.ID;
                });
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
                abis.Dispose();
                hris.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}