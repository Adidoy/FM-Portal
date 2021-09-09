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
    public class ProjectPlansBL : Controller
    {
        private ProjectPlansDAL projectPlanDAL = new ProjectPlansDAL();
        private SystemBDL sysBDL = new SystemBDL();
        private HRISDataAccess hris = new HRISDataAccess();
        private FMISDbContext db = new FMISDbContext();

        public MemoryStream GenerateProjectPlansReport(string ReferenceNo, string LogoPath, string UserEmail)
        {
            Reports reports = new Reports();
            var projectPlan = projectPlanDAL.GetProjectDetails(ReferenceNo, UserEmail);
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
                new HeaderLine { Content = "PROJECT DESCRIPTION", Bold = true, Italic = false, FontSize = 10, ParagraphAlignment = ParagraphAlignment.Center }
            );
            reports.AddColumnHeader(
                new HeaderLine { Content = "Fiscal Year " + projectPlan.FiscalYear.ToString(), Bold = false, Italic = false, FontSize = 8, ParagraphAlignment = ParagraphAlignment.Center }
            );

            reports.AddNewLine();

            var columns = new List<ContentColumn>();
            var rows = new List<ContentCell>();

            columns = new List<ContentColumn>();
            columns.Add(new ContentColumn(3.00));
            columns.Add(new ContentColumn(6.50));
            reports.AddTable(columns, false);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("Sector: ", 0, 8, true));
            rows.Add(new ContentCell(projectPlan.Sector, 1, 8, false, true, ParagraphAlignment.Left, VerticalAlignment.Bottom, 0, 0, true));
            reports.AddRowContent(rows, 0.20);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("Department: ", 0, 8, true));
            rows.Add(new ContentCell(projectPlan.Department, 1, 8, false, true, ParagraphAlignment.Left, VerticalAlignment.Bottom, 0, 0, true));
            reports.AddRowContent(rows, 0.20);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("Unit: ", 0, 8, true));
            rows.Add(new ContentCell(projectPlan.Unit, 1, 8, false, true, ParagraphAlignment.Left, VerticalAlignment.Bottom, 0, 0, true));
            reports.AddRowContent(rows, 0.20);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("Programs/Activities/Projects (PAP): ", 0, 8, true));
            rows.Add(new ContentCell(projectPlan.Program + " (" + projectPlan.PAPCode + ")", 1, 8, false, true, ParagraphAlignment.Left, VerticalAlignment.Bottom, 0, 0, true));
            reports.AddRowContent(rows, 0.20);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("Project Title: ", 0, 8, true));
            rows.Add(new ContentCell(projectPlan.ProjectName + " (" + projectPlan.ProjectCode + ")", 1, 8, false, true, ParagraphAlignment.Left, VerticalAlignment.Bottom, 0, 0, true));
            reports.AddRowContent(rows, 0.20);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("Project Description: ", 0, 8, true));
            rows.Add(new ContentCell(projectPlan.Description, 1, 8, false, true, ParagraphAlignment.Left, VerticalAlignment.Bottom, 0, 0, true));
            reports.AddRowContent(rows, 0.20);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("Total Estimated Budget: ", 0, 8, true));
            rows.Add(new ContentCell(projectPlan.ProjectPlanItems.Sum(d => d.EstimatedBudget).Value.ToString("C", new System.Globalization.CultureInfo("en-ph")), 1, 8, false, true, ParagraphAlignment.Left, VerticalAlignment.Bottom, 0, 0, true));
            reports.AddRowContent(rows, 0.20);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("Project Delivery Month: ", 0, 8, true));
            rows.Add(new ContentCell(projectPlan.DeliveryMonth, 1, 8, false, true, ParagraphAlignment.Left, VerticalAlignment.Bottom, 0, 0, true));
            reports.AddRowContent(rows, 0.20);

            reports.AddNewLine();

            columns = new List<ContentColumn>();
            columns.Add(new ContentColumn(0.50, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
            columns.Add(new ContentColumn(3.00, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
            columns.Add(new ContentColumn(3.00, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
            columns.Add(new ContentColumn(1.00, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
            columns.Add(new ContentColumn(1.00, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
            columns.Add(new ContentColumn(1.50, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
            columns.Add(new ContentColumn(2.50, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
            reports.AddTable(columns, true);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("#", 0, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
            rows.Add(new ContentCell("Item Name", 1, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
            rows.Add(new ContentCell("Procurement Source", 2, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
            rows.Add(new ContentCell("Unit", 3, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
            rows.Add(new ContentCell("Quantity", 4, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
            rows.Add(new ContentCell("Estimated Budget (PhP)", 5, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
            rows.Add(new ContentCell("Justification", 6, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
            reports.AddRowContent(rows, 0.25);

            columns = new List<ContentColumn>();
            columns.Add(new ContentColumn(0.50));
            columns.Add(new ContentColumn(3.00));
            columns.Add(new ContentColumn(3.00));
            columns.Add(new ContentColumn(1.00));
            columns.Add(new ContentColumn(1.00));
            columns.Add(new ContentColumn(1.50));
            columns.Add(new ContentColumn(2.50));
            reports.AddTable(columns, true);

            var count = 1;
            foreach(var item in projectPlan.ProjectPlanItems)
            {
                rows = new List<ContentCell>();
                rows.Add(new ContentCell(count.ToString(), 0, 8, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                rows.Add(new ContentCell(item.ItemName, 1, 8, true, false, ParagraphAlignment.Left, VerticalAlignment.Center));
                rows.Add(new ContentCell(item.ProcurementSource, 2, 8, false, false, ParagraphAlignment.Left, VerticalAlignment.Center));
                rows.Add(new ContentCell(item.IndividualUOMReference, 3, 8, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                rows.Add(new ContentCell(item.TotalQty.ToString("G", new System.Globalization.CultureInfo("en-ph")), 4, 8, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                rows.Add(new ContentCell(item.EstimatedBudget.Value.ToString("N", new System.Globalization.CultureInfo("en-ph")), 5, 8, false, false, ParagraphAlignment.Right, VerticalAlignment.Center));
                rows.Add(new ContentCell(item.Justification, 6, 8, false, false, ParagraphAlignment.Justify, VerticalAlignment.Center));
                reports.AddRowContent(rows, 0.25);
                count++;
            }

            columns = new List<ContentColumn>();
            columns.Add(new ContentColumn(8.50));
            columns.Add(new ContentColumn(1.50));
            columns.Add(new ContentColumn(2.50));
            reports.AddTable(columns, true);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("TOTAL ESTIMATED BUDGET:", 0, 8, true, false, ParagraphAlignment.Right, VerticalAlignment.Center));
            rows.Add(new ContentCell(projectPlan.ProjectPlanItems.Sum(d => d.EstimatedBudget).Value.ToString("C", new System.Globalization.CultureInfo("en-ph")), 1, 8, true, false, ParagraphAlignment.Right, VerticalAlignment.Center));
            rows.Add(new ContentCell(string.Empty, 2, 8, true, false, ParagraphAlignment.Right, VerticalAlignment.Center));
            reports.AddRowContent(rows, 0.25);

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
            rows.Add(new ContentCell(projectPlan.PreparedBy, 1, 8.5, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
            rows.Add(new ContentCell("", 2));
            rows.Add(new ContentCell(projectPlan.SubmittedBy, 3, 8.5, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
            rows.Add(new ContentCell("", 4));
            reports.AddRowContent(rows, 0);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("", 0));
            rows.Add(new ContentCell(projectPlan.PreparedByDesignation, 1, 8.5, false, true, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
            rows.Add(new ContentCell("", 2));
            rows.Add(new ContentCell(projectPlan.SubmittedByDesignation.ToUpper(), 3, 8.5, false, true, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
            rows.Add(new ContentCell("", 4));
            reports.AddRowContent(rows, 0);

            return reports.GenerateReport();
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                projectPlanDAL.Dispose();
            }
            base.Dispose(disposing);
        }
    }

    public class ProjectPlansDAL : Controller
    {
        private FMISDbContext db = new FMISDbContext();
        private SystemBDL systemBDL = new SystemBDL();
        private HRISDataAccess hris = new HRISDataAccess();
        private ABISDataAccess abis = new ABISDataAccess();


        public List<Programs> GetPrograms()
        {
            return abis.GetPrograms();
        }
        public List<HRISDepartment> GetUserDepartments(string UserEmail)
        {
            var user = db.UserAccounts.Where(d => d.Email == UserEmail).FirstOrDefault();
            return hris.GetUserDepartments(UserEmail).OrderBy(d => d.DepartmentID).ToList();
        }
        public SelectList GetMonths()
        {
            return systemBDL.GetMonths();
        }
        public List<HRISDepartment> GetProjectUnits(ProjectTypes ProjectType, string UserEmail, int FiscalYear, string PAPCode)
        {
            var unitsWithExistingProjects = db.ProjectPlans.Where(d => d.ProjectType == ProjectType && d.FiscalYear == FiscalYear && d.PAPCode == PAPCode).Select(d => d.Unit).ToList();
            var unitsWithoutProjects = hris.GetUserDepartments(UserEmail).Where(d => !unitsWithExistingProjects.Contains(d.DepartmentCode)).ToList();
            return unitsWithoutProjects;
        }
        public List<int> GetFiscalYears()
        {
            return db.PPMPDeadlines.Where(d => d.PurgeFlag == false).Select(d => new { FiscalYear = d.FiscalYear }).Select(d => d.FiscalYear).Distinct().ToList();
        }
        public ProjectPlans GetProjectPlan(string ProjectCode)
        {
            return db.ProjectPlans.Where(d => d.ProjectCode == ProjectCode).FirstOrDefault();
        }
        public decimal ComputeUnitCost(decimal? Supplier1UnitCost, decimal? Supplier2UnitCost, decimal? Supplier3UnitCost)
        {
            decimal unitCost = 0.00m;
            int count = 0;
            if (Supplier1UnitCost != null)
            {
                unitCost += (decimal)Supplier1UnitCost;
                count++;
            }
            if (Supplier2UnitCost != null)
            {
                unitCost += (decimal)Supplier2UnitCost;
                count++;
            }
            if (Supplier3UnitCost != null)
            {
                unitCost += (decimal)Supplier3UnitCost;
                count++;
            }
            unitCost = unitCost / count;
            return unitCost;
        }
        public List<ProjectPlanListVM> GetProjects(string UserEmail, int FiscalYear)
        {
            var user = db.UserAccounts.Where(d => d.Email == UserEmail).FirstOrDefault();
            return db.ProjectPlans.Where(d => d.Department == user.DepartmentCode && d.FiscalYear == FiscalYear).ToList()
                            .Select(d => new ProjectPlanListVM
                            {
                                PAPCode = d.PAPCode,
                                Program = abis.GetPrograms(d.PAPCode).GeneralDescription,
                                ProjectCode = d.ProjectCode,
                                ProjectName = d.ProjectName,
                                Office = hris.GetDepartmentDetails(d.Unit).Section,
                                ProjectType = d.ProjectType,
                                ProjectStatus = d.ProjectStatus.GetAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>().Name,
                                EstimatedBudget = (decimal)db.ProjectDetails.Where(x => x.FKProjectPlanReference.ProjectCode == d.ProjectCode &&
                                                                                        (x.ProjectItemStatus != ProjectDetailsStatus.ItemNotAccepted) &&
                                                                                        x.ArticleReference != null)
                                                                            .Sum(x => x.EstimatedBudget).GetValueOrDefault(0.00m)
                            }).OrderBy(d => d.ProjectCode).ToList();
        }
        public List<BasketItems> GetActualObligation(string OfficeCode, int FiscalYear)
        {
            var projectItems = new List<BasketItems>();
            if (db.ProjectPlans.Where(d => d.Unit == OfficeCode && d.FiscalYear == FiscalYear - 1).Count() == 0)
            {
                return new List<BasketItems>();
            }
            //var dbmItems = (from projItems in db.PPMPDetails.Where(d => d.FKProjectDetailReference.FKProjectPlanReference.Unit == OfficeCode && d.FKPPMPHeaderReference.FiscalYear == (FiscalYear - 1) && d.ProcurementSource == ProcurementSources.AgencyToAgency).ToList()
            //                join items in db.Items.ToList() on projItems.FKProjectDetailReference.ItemCode equals items.ItemCode
            //                join prices in db.ItemPrices.Where(d => d.IsPrevailingPrice == true) on items.ID equals prices.Item
            //                into itemsWithCost
            //                from itemCosts in itemsWithCost.Select(d => d.UnitPrice).DefaultIfEmpty()
            //                select new BasketItems
            //                {
            //                    ItemImage = items.ItemImage,
            //                    ItemCode = items.ItemCode,
            //                    ItemName = items.ItemFullName.ToUpper(),
            //                    Category = items.FKCategoryReference.ItemCategoryName,
            //                    ItemType = items.FKArticleReference.FKItemTypeReference.ItemType,
            //                    ItemSpecifications = items.ItemSpecifications == null ? "Not Applicable" : items.ItemSpecifications,
            //                    AccountClass = abis.GetChartOfAccounts(items.FKArticleReference.UACSObjectClass).AcctName,
            //                    ProcurementSource = items.ProcurementSource == ProcurementSources.AgencyToAgency ? "Department of Budget and Management - Procurement Service" : "External Suppliers",
            //                    IndividualUOMReference = items.FKIndividualUnitReference.UnitName,
            //                    PackagingUOMReference = items.FKPackagingUnitReference.UnitName,
            //                    QuantityPerPackage = items.QuantityPerPackage,
            //                    MinimumIssuanceQty = items.MinimumIssuanceQty,
            //                    UnitCost = itemCosts,
            //                    ResponsibilityCenter = items.FKArticleReference.FKItemTypeReference.ResponsibilityCenter == null ? "None" : hris.GetDepartmentDetails(items.FKArticleReference.FKItemTypeReference.ResponsibilityCenter).Department,
            //                    PurchaseRequestCenter = items.FKArticleReference.FKItemTypeReference.PurchaseRequestCenter == null ? "Requesting Office" : hris.GetDepartmentDetails(items.FKArticleReference.FKItemTypeReference.PurchaseRequestCenter).Department,
            //                    EstimatedBudget = projItems.ApprovedBudget,
            //                    Justification = projItems.FKProjectDetailReference.Justification,
            //                    JanQty = projItems.JanQty,
            //                    FebQty = projItems.FebQty,
            //                    MarQty = projItems.MarQty,
            //                    AprQty = projItems.AprQty,
            //                    MayQty = projItems.MayQty,
            //                    JunQty = projItems.JunQty,
            //                    JulQty = projItems.JulQty,
            //                    AugQty = projItems.AugQty,
            //                    SepQty = projItems.SepQty,
            //                    OctQty = projItems.OctQty,
            //                    NovQty = projItems.NovQty,
            //                    DecQty = projItems.DecQty,
            //                    TotalQty = projItems.TotalQty,
            //                    Supplier1ID = projItems.FKProjectDetailReference.Supplier1Reference,
            //                    Supplier1Name = projItems.FKProjectDetailReference.FKSupplier1Reference.SupplierName,
            //                    Supplier1Address = projItems.FKProjectDetailReference.FKSupplier1Reference.Address,
            //                    Supplier1ContactNo = projItems.FKProjectDetailReference.FKSupplier1Reference.ContactNumber,
            //                    Supplier1EmailAddress = projItems.FKProjectDetailReference.FKSupplier1Reference.EmailAddress,
            //                    Supplier1UnitCost = itemCosts,
            //                    Supplier2ID = projItems.FKProjectDetailReference.Supplier2Reference == null ? null : projItems.FKProjectDetailReference.Supplier2Reference,
            //                    Supplier2Name = projItems.FKProjectDetailReference.Supplier2Reference == null ? null : projItems.FKProjectDetailReference.FKSupplier2Reference.SupplierName,
            //                    Supplier2Address = projItems.FKProjectDetailReference.Supplier2Reference == null ? null : projItems.FKProjectDetailReference.FKSupplier2Reference.Address,
            //                    Supplier2ContactNo = projItems.FKProjectDetailReference.Supplier2Reference == null ? null : projItems.FKProjectDetailReference.FKSupplier2Reference.ContactNumber,
            //                    Supplier2EmailAddress = projItems.FKProjectDetailReference.Supplier2Reference == null ? null : projItems.FKProjectDetailReference.FKSupplier2Reference.EmailAddress,
            //                    Supplier3ID = projItems.FKProjectDetailReference.Supplier3Reference == null ? null : projItems.FKProjectDetailReference.Supplier3Reference,
            //                    Supplier3Name = projItems.FKProjectDetailReference.Supplier3Reference == null ? null : projItems.FKProjectDetailReference.FKSupplier3Reference.SupplierName,
            //                    Supplier3Address = projItems.FKProjectDetailReference.Supplier3Reference == null ? null : projItems.FKProjectDetailReference.FKSupplier3Reference.Address,
            //                    Supplier3ContactNo = projItems.FKProjectDetailReference.Supplier3Reference == null ? null : projItems.FKProjectDetailReference.FKSupplier3Reference.ContactNumber,
            //                    Supplier3EmailAddress = projItems.FKProjectDetailReference.Supplier3Reference == null ? null : projItems.FKProjectDetailReference.FKSupplier3Reference.EmailAddress
            //                }).ToList();

            //var externalItems = (from projItems in db.PPMPDetails.Where(d => d.FKProjectDetailReference.FKProjectPlanReference.Unit == OfficeCode && d.FKPPMPHeaderReference.FiscalYear == (FiscalYear - 1) && d.ProcurementSource == ProcurementSources.ExternalSuppliers).ToList()
            //                     join items in db.Items.ToList() on projItems.FKProjectDetailReference.ItemCode equals items.ItemCode
            //                     join prices in db.ItemPrices.Where(d => d.IsPrevailingPrice == true) on items.ID equals prices.Item
            //                     into itemsWithCost
            //                     from itemCosts in itemsWithCost.Select(d => d.UnitPrice).DefaultIfEmpty()
            //                     select new BasketItems
            //                     {
            //                         ItemImage = items.ItemImage,
            //                         ItemCode = items.ItemCode,
            //                         ItemName = items.ItemFullName.ToUpper(),
            //                         Category = items.FKCategoryReference.ItemCategoryName,
            //                         ItemType = items.FKArticleReference.FKItemTypeReference.ItemType,
            //                         ItemSpecifications = items.ItemSpecifications == null ? "Not Applicable" : items.ItemSpecifications,
            //                         AccountClass = abis.GetChartOfAccounts(items.FKArticleReference.UACSObjectClass).AcctName,
            //                         ProcurementSource = items.ProcurementSource == ProcurementSources.AgencyToAgency ? "Department of Budget and Management - Procurement Service" : "External Suppliers",
            //                         IndividualUOMReference = items.FKIndividualUnitReference.UnitName,
            //                         PackagingUOMReference = items.FKPackagingUnitReference.UnitName,
            //                         QuantityPerPackage = items.QuantityPerPackage,
            //                         MinimumIssuanceQty = items.MinimumIssuanceQty,
            //                         UnitCost = itemCosts,
            //                         ResponsibilityCenter = items.FKArticleReference.FKItemTypeReference.ResponsibilityCenter == null ? "None" : hris.GetDepartmentDetails(items.FKArticleReference.FKItemTypeReference.ResponsibilityCenter).Department,
            //                         PurchaseRequestCenter = items.FKArticleReference.FKItemTypeReference.PurchaseRequestCenter == null ? "Requesting Office" : hris.GetDepartmentDetails(items.FKArticleReference.FKItemTypeReference.PurchaseRequestCenter).Department,
            //                         EstimatedBudget = projItems.ApprovedBudget,
            //                         Justification = projItems.FKProjectDetailReference.Justification,
            //                         JanQty = projItems.JanQty,
            //                         FebQty = projItems.FebQty,
            //                         MarQty = projItems.MarQty,
            //                         AprQty = projItems.AprQty,
            //                         MayQty = projItems.MayQty,
            //                         JunQty = projItems.JunQty,
            //                         JulQty = projItems.JulQty,
            //                         AugQty = projItems.AugQty,
            //                         SepQty = projItems.SepQty,
            //                         OctQty = projItems.OctQty,
            //                         NovQty = projItems.NovQty,
            //                         DecQty = projItems.DecQty,
            //                         TotalQty = projItems.TotalQty,
            //                         Supplier1ID = projItems.FKProjectDetailReference.Supplier1Reference,
            //                         Supplier1Name = projItems.FKProjectDetailReference.FKSupplier1Reference.SupplierName,
            //                         Supplier1Address = projItems.FKProjectDetailReference.FKSupplier1Reference.Address,
            //                         Supplier1ContactNo = projItems.FKProjectDetailReference.FKSupplier1Reference.ContactNumber,
            //                         Supplier1EmailAddress = projItems.FKProjectDetailReference.FKSupplier1Reference.EmailAddress,
            //                         Supplier1UnitCost = itemCosts,
            //                         Supplier2ID = projItems.FKProjectDetailReference.Supplier2Reference == null ? null : projItems.FKProjectDetailReference.Supplier2Reference,
            //                         Supplier2Name = projItems.FKProjectDetailReference.Supplier2Reference == null ? null : projItems.FKProjectDetailReference.FKSupplier2Reference.SupplierName,
            //                         Supplier2Address = projItems.FKProjectDetailReference.Supplier2Reference == null ? null : projItems.FKProjectDetailReference.FKSupplier2Reference.Address,
            //                         Supplier2ContactNo = projItems.FKProjectDetailReference.Supplier2Reference == null ? null : projItems.FKProjectDetailReference.FKSupplier2Reference.ContactNumber,
            //                         Supplier2EmailAddress = projItems.FKProjectDetailReference.Supplier2Reference == null ? null : projItems.FKProjectDetailReference.FKSupplier2Reference.EmailAddress,
            //                         Supplier3ID = projItems.FKProjectDetailReference.Supplier3Reference == null ? null : projItems.FKProjectDetailReference.Supplier3Reference,
            //                         Supplier3Name = projItems.FKProjectDetailReference.Supplier3Reference == null ? null : projItems.FKProjectDetailReference.FKSupplier3Reference.SupplierName,
            //                         Supplier3Address = projItems.FKProjectDetailReference.Supplier3Reference == null ? null : projItems.FKProjectDetailReference.FKSupplier3Reference.Address,
            //                         Supplier3ContactNo = projItems.FKProjectDetailReference.Supplier3Reference == null ? null : projItems.FKProjectDetailReference.FKSupplier3Reference.ContactNumber,
            //                         Supplier3EmailAddress = projItems.FKProjectDetailReference.Supplier3Reference == null ? null : projItems.FKProjectDetailReference.FKSupplier3Reference.EmailAddress
            //                     }).ToList();

            //projectItems.AddRange(dbmItems);
            //projectItems.AddRange(externalItems);
            return projectItems;
        }
        public ProjectPlanVM GetProjectDetails(string ProjectCode, string UserEmail)
        {
            var projectPlanHeader = db.ProjectPlans.Where(d => d.ProjectCode == ProjectCode).FirstOrDefault();
            var currentUser = hris.GetEmployee(UserEmail);
            var employee = hris.GetEmployee(UserEmail);

            ProjectPlanVM projectPlan = new ProjectPlanVM
            {
                PAPCode = projectPlanHeader.PAPCode,
                Program = abis.GetPrograms(projectPlanHeader.PAPCode).GeneralDescription,
                ProjectCode = projectPlanHeader.ProjectCode,
                ProjectName = projectPlanHeader.ProjectName,
                ProjectType = projectPlanHeader.ProjectType,
                Description = projectPlanHeader.Description,
                FiscalYear = projectPlanHeader.FiscalYear,
                SectorCode = projectPlanHeader.Sector,
                Sector = hris.GetDepartmentDetails(projectPlanHeader.Sector).Department,
                DepartmentCode = projectPlanHeader.Department,
                Department = hris.GetDepartmentDetails(projectPlanHeader.Department).Department,
                UnitCode = projectPlanHeader.Unit,
                Unit = hris.GetDepartmentDetails(projectPlanHeader.Unit).Section,
                PreparedBy = employee.EmployeeName,
                PreparedByDesignation = employee.Designation,
                PreparedByEmpCode = employee.EmployeeCode,
                SubmittedBy = hris.GetDepartmentDetails(projectPlanHeader.Department).DepartmentHead,
                SubmittedByDesignation = hris.GetDepartmentDetails(projectPlanHeader.Department).DepartmentHeadDesignation,
                DeliveryMonth = systemBDL.GetMonthName(projectPlanHeader.DeliveryMonth),
                TotalEstimatedBudget = projectPlanHeader.TotalEstimatedBudget,
                ProjectStatus = projectPlanHeader.ProjectStatus.GetAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>().Name,
                ProjectPlanItems = new List<BasketItems>()
            };


            var projectItems = (from d in db.ProjectDetails.Where(d => d.FKProjectPlanReference.ProjectCode == ProjectCode && d.ArticleReference != null).ToList()
                                select new BasketItems()
                                {
                                    ProposalType = BudgetProposalType.NewProposal,
                                    ItemCode = db.Items.Where(x => x.ArticleReference == d.ArticleReference && x.Sequence == d.ItemSequence).FirstOrDefault().ItemCode,
                                    ItemName = d.ItemFullName,
                                    IndividualUOMReference = d.FKUOMReference.Abbreviation,
                                    ProcurementSource = d.ProcurementSource == ProcurementSources.AgencyToAgency ? "Department of Budget and Management - Procurement Service" : "External Suppliers",
                                    ResponsibilityCenter = d.FKItemArticleReference.FKItemTypeReference.ResponsibilityCenter == null ? "Requesting Office" : d.FKItemArticleReference.FKItemTypeReference.ResponsibilityCenter,
                                    TotalQty = d.TotalQty,
                                    UnitCost = d.UnitCost,
                                    EstimatedBudget = d.ProjectItemStatus == ProjectDetailsStatus.ItemNotAccepted ? null : d.EstimatedBudget,
                                    Justification = d.Justification,
                                    ProjectItemStatus = d.ProjectItemStatus.GetAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>().Name
                                }).OrderBy(d => d.ItemName).ToList();

            projectPlan.ProjectPlanItems.AddRange(projectItems);
            return projectPlan;
        }
        public BasketItems GetProjectDetails(string ProjectCode, string ItemCode, string UserEmail)
        {
            SuppliersBL suppliersBL = new SuppliersBL();
            var itemReference = db.Items.ToList().Where(d => d.ItemCode == ItemCode).FirstOrDefault();
            return (from projectItem in db.ProjectDetails.Where(d => d.ArticleReference == itemReference.ArticleReference &&
                                                                     d.ItemSequence == itemReference.Sequence &&
                                                                     d.FKProjectPlanReference.ProjectCode == ProjectCode).ToList()
                    select new BasketItems
                    {
                        ProposalType = projectItem.ProposalType,
                        ItemType = projectItem.FKItemArticleReference.FKItemTypeReference.ItemType,
                        Category = projectItem.FKCategoryReference.ItemCategoryName,
                        ItemCode = itemReference.ItemCode,
                        ItemName = projectItem.ItemFullName,
                        IsSpecsUserDefined = itemReference.IsSpecsUserDefined,
                        ItemSpecifications = itemReference.IsSpecsUserDefined == true ? projectItem.ItemSpecifications : itemReference.ItemSpecifications,
                        ProcurementSource = projectItem.ProcurementSource == ProcurementSources.AgencyToAgency ? "Department of Budget and Management - Procurement Service" : "External Suppliers",
                        AccountClass = abis.GetChartOfAccounts(projectItem.FKItemArticleReference.UACSObjectClass).AcctName,
                        ItemImage = itemReference.ItemImage,
                        UnitCost = projectItem.UnitCost,
                        MinimumIssuanceQty = itemReference.MinimumIssuanceQty,
                        IndividualUOMReference = projectItem.FKUOMReference.UnitName,
                        ResponsibilityCenter = projectItem.FKItemArticleReference.FKItemTypeReference.ResponsibilityCenter == null ? "Requesting Office" : hris.GetDepartmentDetails(projectItem.FKItemArticleReference.FKItemTypeReference.ResponsibilityCenter).Department,
                        PurchaseRequestCenter = projectItem.FKItemArticleReference.FKItemTypeReference.PurchaseRequestCenter == null ? "Requesting Office" : hris.GetDepartmentDetails(projectItem.FKItemArticleReference.FKItemTypeReference.PurchaseRequestCenter).Department,
                        Justification = projectItem.Justification,
                        JanQty = projectItem.JanQty,
                        FebQty = projectItem.FebQty,
                        MarQty = projectItem.MarQty,
                        AprQty = projectItem.AprQty,
                        MayQty = projectItem.MayQty,
                        JunQty = projectItem.JunQty,
                        JulQty = projectItem.JulQty,
                        AugQty = projectItem.AugQty,
                        SepQty = projectItem.SepQty,
                        OctQty = projectItem.OctQty,
                        NovQty = projectItem.NovQty,
                        DecQty = projectItem.DecQty,
                        TotalQty = projectItem.TotalQty,
                        Supplier1ID = projectItem.Supplier1Reference,
                        Supplier1Name = projectItem.Supplier1Reference == null ? null : suppliersBL.GetSupplierDetails(projectItem.Supplier1Reference).SupplierName,
                        Supplier1Address = projectItem.Supplier1Reference == null ? null : suppliersBL.GetSupplierDetails(projectItem.Supplier1Reference).Address,
                        Supplier1ContactNo = projectItem.Supplier1Reference == null ? null : suppliersBL.GetSupplierDetails(projectItem.Supplier1Reference).ContactNumber,
                        Supplier1EmailAddress = projectItem.Supplier1Reference == null ? null : suppliersBL.GetSupplierDetails(projectItem.Supplier1Reference).EmailAddress,
                        Supplier1UnitCost = projectItem.Supplier1UnitCost,
                        Supplier2ID = projectItem.Supplier2Reference,
                        Supplier2Name = projectItem.Supplier2Reference == null ? null : suppliersBL.GetSupplierDetails(projectItem.Supplier2Reference).SupplierName,
                        Supplier2Address = projectItem.Supplier2Reference == null ? null : suppliersBL.GetSupplierDetails(projectItem.Supplier2Reference).Address,
                        Supplier2ContactNo = projectItem.Supplier2Reference == null ? null : suppliersBL.GetSupplierDetails(projectItem.Supplier2Reference).ContactNumber,
                        Supplier2EmailAddress = projectItem.Supplier2Reference == null ? null : suppliersBL.GetSupplierDetails(projectItem.Supplier2Reference).EmailAddress,
                        Supplier2UnitCost = projectItem.Supplier2UnitCost,
                        Supplier3ID = projectItem.Supplier3Reference,
                        Supplier3Name = projectItem.Supplier3Reference == null ? null : suppliersBL.GetSupplierDetails(projectItem.Supplier3Reference).SupplierName,
                        Supplier3Address = projectItem.Supplier3Reference == null ? null : suppliersBL.GetSupplierDetails(projectItem.Supplier3Reference).Address,
                        Supplier3ContactNo = projectItem.Supplier3Reference == null ? null : suppliersBL.GetSupplierDetails(projectItem.Supplier3Reference).ContactNumber,
                        Supplier3EmailAddress = projectItem.Supplier3Reference == null ? null : suppliersBL.GetSupplierDetails(projectItem.Supplier3Reference).EmailAddress,
                        Supplier3UnitCost = projectItem.Supplier3UnitCost,
                        EstimatedBudget = projectItem.EstimatedBudget,
                        ProjectItemStatus = projectItem.ProjectItemStatus.GetAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>().Name,
                        ReasonForNonAcceptance = projectItem.ReasonForNonAcceptance,
                        UpdateFlag = projectItem.UpdateFlag
                    }).FirstOrDefault();
        }
        public bool ValidateProjectPlan(ProjectPlans projectPlan, string UserEmail, out string ErrorMessage)
        {
            var user = db.UserAccounts.Where(d => d.Email == UserEmail).FirstOrDefault();
            var office = hris.GetDepartmentDetails(projectPlan.Unit);
            if (projectPlan.ProjectCode.Substring(0, 4) == "CSPR")
            {
                if (db.ProjectPlans.Where(d => d.Unit == office.SectionCode && d.FiscalYear == projectPlan.FiscalYear && d.ProjectCode.Contains("CSPR")).Count() >= 1)
                {
                    ErrorMessage = "Common-use Supplies Project for " + office.Department.ToUpper() + " already exists. Only one Common-use Office Supplies Project is allowed per Fiscal Year";
                    return false;
                }
                ErrorMessage = string.Empty;
                return true;
            }
            ErrorMessage = string.Empty;
            return true;
        }
        public bool SaveProjectPlan(ProjectPlans ProjectPlan, List<BasketItems> ActualObligation, string UserEmail, out string ProjectCode)
        {
            var user = db.UserAccounts.Where(d => d.Email == UserEmail).FirstOrDefault();
            var office = hris.GetDepartmentDetails(ProjectPlan.Unit);
            var employee = hris.GetEmployee(user.Email);
            ProjectPlan.ProjectType = ProjectPlan.ProjectType;
            ProjectPlan.Sector = office.SectorCode;
            ProjectPlan.Department = office.DepartmentCode;
            ProjectPlan.Unit = office.SectionCode;
            ProjectPlan.PreparedBy = user.EmpCode;
            ProjectPlan.SubmittedBy = office.DepartmentHead;
            ProjectPlan.ProjectStatus = ProjectStatus.NewProject;
            ProjectPlan.ProjectType = ProjectPlan.ProjectType;
            ProjectPlan.ProjectCode = GenerateProjectCode(ProjectPlan.FiscalYear, ProjectPlan.Unit, ProjectPlan.ProjectType);

            db.ProjectPlans.Add(ProjectPlan);
            if (db.SaveChanges() == 0)
            {
                ProjectCode = null;
                return false;
            }

            if (ActualObligation.Count > 0)
            {
                var supplyDetails = (from actualObligation in ActualObligation
                                     join items in db.Items on actualObligation.ItemCode equals items.ItemCode
                                     select new ProjectDetails
                                     {
                                         ProjectReference = ProjectPlan.ID,
                                         ItemSequence = items.Sequence,
                                         ItemFullName = items.ItemFullName,
                                         ClassificationReference = items.FKArticleReference.FKItemTypeReference.FKItemClassificationReference.ID,
                                         UACS = items.FKArticleReference.UACSObjectClass,
                                         ItemSpecifications = actualObligation.ItemSpecifications,
                                         CategoryReference = items.CategoryReference,
                                         ArticleReference = items.ArticleReference,
                                         ProposalType = BudgetProposalType.Actual,
                                         UOMReference = items.IndividualUOMReference,
                                         ProcurementSource = items.ProcurementSource,
                                         Supplier1Reference = actualObligation.Supplier1ID,
                                         Supplier1UnitCost = actualObligation.Supplier1UnitCost,
                                         Supplier2Reference = actualObligation.Supplier2ID,
                                         Supplier2UnitCost = actualObligation.Supplier2UnitCost,
                                         Supplier3Reference = actualObligation.Supplier3ID,
                                         Supplier3UnitCost = actualObligation.Supplier3UnitCost,
                                         UnitCost = (decimal)actualObligation.UnitCost,
                                         JanQty = actualObligation.JanQty,
                                         FebQty = actualObligation.FebQty,
                                         MarQty = actualObligation.MarQty,
                                         AprQty = actualObligation.AprQty,
                                         MayQty = actualObligation.MayQty,
                                         JunQty = actualObligation.JunQty,
                                         JulQty = actualObligation.JulQty,
                                         AugQty = actualObligation.AugQty,
                                         SepQty = actualObligation.SepQty,
                                         OctQty = actualObligation.OctQty,
                                         NovQty = actualObligation.NovQty,
                                         DecQty = actualObligation.DecQty,
                                         TotalQty = actualObligation.JanQty + actualObligation.FebQty + actualObligation.MarQty + actualObligation.AprQty + actualObligation.MayQty + actualObligation.JunQty + actualObligation.JulQty + actualObligation.SepQty + actualObligation.OctQty + actualObligation.NovQty + actualObligation.DecQty,
                                         Justification = actualObligation.Justification,
                                         EstimatedBudget = (decimal)actualObligation.EstimatedBudget,
                                         ProjectItemStatus = ProjectDetailsStatus.PostedToProject,
                                         UpdateFlag = true
                                     }).ToList();
                db.ProjectDetails.AddRange(supplyDetails);

                if (db.SaveChanges() == 0)
                {
                    ProjectCode = null;
                    return false;
                }
            }

            var newProjectSwitch = new SwitchBoard
            {
                DepartmentCode = ProjectPlan.Department,
                MessageType = "Project Plan",
                Reference = ProjectPlan.ProjectCode,
                Subject = ProjectPlan.ProjectCode + " - " + ProjectPlan.ProjectName
            };

            db.SwitchBoard.Add(newProjectSwitch);
            if (db.SaveChanges() == 0)
            {
                ProjectCode = null;
                return false;
            }

            db.SwitchBoardBody.Add(new SwitchBoardBody
            {
                SwitchBoardReference = newProjectSwitch.ID,
                ActionBy = employee.EmployeeCode,
                Remarks = "A new Project Plan has been created. (" + ProjectPlan.ProjectCode + " - " + ProjectPlan.ProjectName + ")",
                DepartmentCode = employee.DepartmentCode,
                UpdatedAt = DateTime.Now
            });

            if ((ProjectPlan.ProjectCode.Substring(0, 4) == "CSPR") && (ActualObligation.Count > 0))
            {
                db.SwitchBoardBody.Add(new SwitchBoardBody
                {
                    SwitchBoardReference = newProjectSwitch.ID,
                    ActionBy = "SYSTEM",
                    Remarks = "Items based on Actual Obligation from the previous fiscal year are added.",
                    DepartmentCode = employee.DepartmentCode,
                    UpdatedAt = DateTime.Now
                });
            }

            if (db.SaveChanges() == 0)
            {
                ProjectCode = null;
                return false;
            }

            ProjectCode = ProjectPlan.ProjectCode;
            return true;
        }
        public bool ForwardToResponsibilityCenter(string ProjectCode, string UserEmail)
        {
            var employee = hris.GetEmployee(UserEmail);
            var project = db.ProjectPlans.Where(d => d.ProjectCode == ProjectCode).FirstOrDefault();
            var projectItems = db.ProjectDetails.Where(d => d.FKProjectPlanReference.ProjectCode == ProjectCode && d.ProjectItemStatus == ProjectDetailsStatus.PostedToProject).ToList();
            project.ProjectStatus = ProjectStatus.ForwardedToResponsibilityCenter;
            projectItems.ForEach(d => { d.ProjectItemStatus = ProjectDetailsStatus.ForEvaluation; d.UpdateFlag = false; });

            var switchBoard = db.SwitchBoard.Where(d => d.Reference == project.ProjectCode).FirstOrDefault();
            db.SwitchBoardBody.Add(new SwitchBoardBody
            {
                SwitchBoardReference = switchBoard.ID,
                ActionBy = employee.EmployeeCode,
                Remarks = "Project Plan has been " + ProjectStatus.ForwardedToResponsibilityCenter.GetAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>().Name + ".",
                DepartmentCode = employee.DepartmentCode,
                UpdatedAt = DateTime.Now
            });

            if (db.SaveChanges() == 0)
            {
                return false;
            }

            return true;
        }
        private string GenerateProjectCode(int FiscalYear, string DepartmentCode, ProjectTypes Type)
        {
            var prefix = Type.GetAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>().ShortName + "-" + (DepartmentCode.Contains("-") ? DepartmentCode.Replace("-", "").ToString().ToUpper() : DepartmentCode.ToUpper());
            var series = db.ProjectPlans.Where(d => d.ProjectCode.StartsWith(prefix) && d.FiscalYear == FiscalYear).Count() + 1;
            var seriesStr = (series.ToString().Length == 1) ? "00" + series.ToString() : (series.ToString().Length == 2) ? "0" + series.ToString() : series.ToString();
            return prefix + "-" + seriesStr + "-" + FiscalYear.ToString();
        }
        public bool UpdateProjectItem(BasketItems Item, string ProjectCode)
        {
            var itemReference = db.Items.ToList().Where(d => d.ItemCode == Item.ItemCode).FirstOrDefault();
            var projectItem = db.ProjectDetails.Where(d => d.ArticleReference == itemReference.ArticleReference && d.ItemSequence == itemReference.Sequence && d.FKProjectPlanReference.ProjectCode == ProjectCode).FirstOrDefault();

            projectItem.Supplier1Reference = Item.Supplier1ID;
            projectItem.Supplier1UnitCost = Item.Supplier1UnitCost;
            projectItem.Supplier2Reference = Item.Supplier2ID;
            projectItem.Supplier2UnitCost = Item.Supplier2UnitCost;
            projectItem.Supplier3Reference = Item.Supplier3ID;
            projectItem.Supplier3UnitCost = Item.Supplier3UnitCost;
            projectItem.UnitCost = Math.Round((decimal)Item.UnitCost, 2, MidpointRounding.AwayFromZero);
            projectItem.JanQty = Item.JanQty;
            projectItem.FebQty = Item.FebQty;
            projectItem.MarQty = Item.MarQty;
            projectItem.AprQty = Item.AprQty;
            projectItem.MayQty = Item.MayQty;
            projectItem.JunQty = Item.JunQty;
            projectItem.JulQty = Item.JulQty;
            projectItem.AugQty = Item.AugQty;
            projectItem.SepQty = Item.SepQty;
            projectItem.OctQty = Item.OctQty;
            projectItem.NovQty = Item.NovQty;
            projectItem.DecQty = Item.DecQty;
            projectItem.TotalQty = Item.TotalQty;
            projectItem.Justification = Item.Justification;
            projectItem.EstimatedBudget = Math.Round((decimal)(Item.UnitCost * Item.TotalQty), 2, MidpointRounding.AwayFromZero);
            projectItem.ProjectItemStatus = (projectItem.FKProjectPlanReference.ProjectStatus == ProjectStatus.NewProject) ? ProjectDetailsStatus.PostedToProject : ProjectDetailsStatus.ItemRevised;

            if (db.SaveChanges() == 0)
            {
                return false;
            }
            return true;
        }


        //public bool ForwardInfraRequest(int FiscalYear, string UserEmail)
        //{
        //    var employee = hris.GetEmployee(UserEmail);
        //    var projects = db.ProjectInfrastructureRequest.Where(d => d.FiscalYear == FiscalYear && d.Department == employee.DepartmentCode).ToList();
        //    projects.ForEach(d => { d.Status = InfrastructurePlanRequestStatus.ForwardedToImplementingUnit; });
        //    if (db.SaveChanges() == 0)
        //    {
        //        return false;
        //    }

        //    return true;
        //}
        //public bool ValidateUpdateItem(ProjectPlanItemsVM item, string ProjectCode, out string Message)
        //{
        //    if (ProjectCode.Substring(0, 4) == "CSPR")
        //    {
        //        item.JanQty = (item.JanQty == null) ? 0 : item.JanQty;
        //        item.FebQty = (item.FebQty == null) ? 0 : item.FebQty;
        //        item.MarQty = (item.MarQty == null) ? 0 : item.MarQty;
        //        item.AprQty = (item.AprQty == null) ? 0 : item.AprQty;
        //        item.MayQty = (item.MayQty == null) ? 0 : item.MayQty;
        //        item.JunQty = (item.JunQty == null) ? 0 : item.JunQty;
        //        item.JulQty = (item.JulQty == null) ? 0 : item.JulQty;
        //        item.AugQty = (item.AugQty == null) ? 0 : item.AugQty;
        //        item.SepQty = (item.SepQty == null) ? 0 : item.SepQty;
        //        item.OctQty = (item.OctQty == null) ? 0 : item.OctQty;
        //        item.NovQty = (item.NovQty == null) ? 0 : item.NovQty;
        //        item.DecQty = (item.DecQty == null) ? 0 : item.DecQty;
        //        item.TotalQty = (int)(item.JanQty + item.FebQty + item.MarQty + item.AprQty + item.MayQty + item.JunQty + item.JulQty + item.AugQty + item.SepQty + item.OctQty + item.NovQty + item.DecQty);
        //    }
        //    else
        //    {
        //        var StartMonth = db.ProjectPlans.Where(d => d.ProjectCode == ProjectCode).FirstOrDefault().DeliveryMonth;
        //        switch (StartMonth)
        //        {
        //            case 1:
        //                {
        //                    item.JanQty = item.TotalQty;
        //                    item.TotalQty = item.TotalQty = item.TotalQty;
        //                }
        //                break;
        //            case 2:
        //                {
        //                    item.FebQty = item.TotalQty;
        //                    item.TotalQty = item.TotalQty = item.TotalQty;
        //                }
        //                break;
        //            case 3:
        //                {
        //                    item.MarQty = item.TotalQty;
        //                    item.TotalQty = item.TotalQty = item.TotalQty;
        //                }
        //                break;
        //            case 4:
        //                {
        //                    item.AprQty = item.TotalQty;
        //                    item.TotalQty = item.TotalQty = item.TotalQty;
        //                }
        //                break;
        //            case 5:
        //                {
        //                    item.MayQty = item.TotalQty;
        //                    item.TotalQty = item.TotalQty = item.TotalQty;
        //                }
        //                break;
        //            case 6:
        //                {
        //                    item.JunQty = item.TotalQty;
        //                    item.TotalQty = item.TotalQty = item.TotalQty;
        //                }
        //                break;
        //            case 7:
        //                {
        //                    item.JulQty = item.TotalQty;
        //                    item.TotalQty = item.TotalQty = item.TotalQty;
        //                }
        //                break;
        //            case 8:
        //                {
        //                    item.AugQty = item.TotalQty;
        //                    item.TotalQty = item.TotalQty = item.TotalQty;
        //                }
        //                break;
        //            case 9:
        //                {
        //                    item.SepQty = item.TotalQty;
        //                    item.TotalQty = item.TotalQty = item.TotalQty;
        //                }
        //                break;
        //            case 10:
        //                {
        //                    item.OctQty = item.TotalQty;
        //                    item.TotalQty = item.TotalQty = item.TotalQty;
        //                }
        //                break;
        //            case 11:
        //                {
        //                    item.NovQty = item.TotalQty;
        //                    item.TotalQty = item.TotalQty = item.TotalQty;
        //                }
        //                break;
        //            case 12:
        //                {
        //                    item.DecQty = item.TotalQty;
        //                    item.TotalQty = item.TotalQty = item.TotalQty;
        //                }
        //                break;
        //        }
        //    }

        //    if (item.TotalQty <= 0)
        //    {
        //        Message = "Please enter quantity requirement for at least one (1) quarter.";
        //        return false;
        //    }
        //    if (item.Remarks == null || item.Remarks == string.Empty)
        //    {
        //        Message = "Please provide justification/remarks for the new spending proposal.";
        //        return false;
        //    }

        //    Message = string.Empty;
        //    return true;
        //}
        //public bool UpdateItem(BasketItems Item, string ProjectCode)
        //{
        //    if (ProjectCode.Contains("CSPR"))
        //    {
        //        var item = db.ProjectDetailsSupplies.Where(d => d.FKProjectPlanReference.ProjectCode == ProjectCode && d.FKItemReference.ItemCode == Item.ItemCode).FirstOrDefault();
        //        item.JanQty = Item.JanQty;
        //        item.FebQty = Item.FebQty;
        //        item.MarQty = Item.MarQty;
        //        item.AprQty = Item.AprQty;
        //        item.MayQty = Item.MayQty;
        //        item.JunQty = Item.JunQty;
        //        item.JulQty = Item.JulQty;
        //        item.AugQty = Item.AugQty;
        //        item.SepQty = Item.SepQty;
        //        item.OctQty = Item.OctQty;
        //        item.NovQty = Item.NovQty;
        //        item.DecQty = Item.DecQty;
        //        item.UnitCost = (decimal)Item.UnitCost;
        //        item.Supplier1Reference = Item.Supplier1ID;
        //        item.Supplier1UnitCost = Item.Supplier1UnitCost;
        //        item.Supplier2Reference = Item.Supplier2ID;
        //        item.Supplier2UnitCost = Item.Supplier2UnitCost;
        //        item.Supplier3Reference = Item.Supplier3ID;
        //        item.Supplier3UnitCost = Item.Supplier3UnitCost;
        //        item.Justification = Item.Justification;
        //        item.EstimatedBudget = (decimal)Item.EstimatedBudget;

        //        if (db.SaveChanges() == 0)
        //        {
        //            return false;
        //        }
        //    }
        //    else
        //    {
        //        var item = db.ProjectDetails.Where(d => d.FKProjectPlanReference.ProjectCode == ProjectCode && d.FKItemReference.ItemCode == Item.ItemCode).FirstOrDefault();
        //        item.TotalQty = Item.TotalQty;
        //        item.UnitCost = (decimal)Item.UnitCost;
        //        item.Supplier1Reference = Item.Supplier1ID;
        //        item.Supplier1UnitCost = Item.Supplier1UnitCost;
        //        item.Supplier2Reference = Item.Supplier2ID;
        //        item.Supplier2UnitCost = Item.Supplier2UnitCost;
        //        item.Supplier3Reference = Item.Supplier3ID;
        //        item.Supplier3UnitCost = Item.Supplier3UnitCost;
        //        item.Justification = Item.Justification;
        //        item.EstimatedBudget = (decimal)Item.EstimatedBudget;

        //        if (db.SaveChanges() == 0)
        //        {
        //            return false;
        //        }
        //    }

        //    return true;
        //}
        //public bool RemoveItem(string ProjectCode, string ItemCode)
        //{
        //    if (ProjectCode.Contains("CSPR"))
        //    {
        //        var projectItem = db.ProjectDetailsSupplies.Where(d => d.FKProjectPlanReference.ProjectCode == ProjectCode && d.FKItemReference.ItemCode == ItemCode).First();
        //        db.ProjectDetailsSupplies.Remove(projectItem);
        //        if (db.SaveChanges() == 0)
        //        {
        //            return false;
        //        }
        //    }
        //    else
        //    {
        //        var projectItem = db.ProjectDetails.Where(d => d.FKProjectPlanReference.ProjectCode == ProjectCode && d.FKItemReference.ItemCode == ItemCode).First();
        //        db.ProjectDetails.Remove(projectItem);
        //        if (db.SaveChanges() == 0)
        //        {
        //            return false;
        //        }
        //    }
        //    return true;
        //}
        //public bool PostToPPMP(ProjectPlanVM ProjectPlan, string UserEmail)
        //{
        //    if (ProjectPlan.ProjectPlanItems.Count == 0)
        //    {
        //        return false;
        //    }

        //    if (ppmpDAL.PostToPPMP(ProjectPlan, UserEmail))
        //    {
        //        var project = db.ProjectPlans.Where(d => d.ProjectCode == ProjectPlan.ProjectCode).FirstOrDefault();
        //        project.ProjectStatus = "Posted to PPMP";
        //        db.SaveChanges();
        //        return true;
        //    }
        //    return false;
        //}
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