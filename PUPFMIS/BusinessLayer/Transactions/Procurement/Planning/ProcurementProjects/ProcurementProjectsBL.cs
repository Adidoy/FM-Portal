using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PUPFMIS.Models;
using PUPFMIS.Models.HRIS;

namespace PUPFMIS.BusinessLayer
{
    public class ProcurementProjectsBL : Controller
    {
        private FMISDbContext db = new FMISDbContext();
        private HRISDbContext hris = new HRISDbContext();

        public List<PPMPDeadlines> GetFiscalYears()
        {
            return db.PPMPDeadlines.Where(d => d.PurgeFlag == false).OrderBy(d => d.FiscalYear).ToList();
        }

        public List<Months> GetMonths()
        {
            var months = new List<Months>()
                {
                    new Months { MonthValue = 1, MonthName = "January" },
                    new Months { MonthValue = 2, MonthName = "February" },
                    new Months { MonthValue = 3, MonthName = "March" },
                    new Months { MonthValue = 4, MonthName = "April" },
                    new Months { MonthValue = 5, MonthName = "May" },
                    new Months { MonthValue = 6, MonthName = "June" },
                    new Months { MonthValue = 7, MonthName = "July" },
                    new Months { MonthValue = 8, MonthName = "August" },
                    new Months { MonthValue = 9, MonthName = "September" },
                    new Months { MonthValue = 10, MonthName = "October" },
                    new Months { MonthValue = 11, MonthName = "November" },
                    new Months { MonthValue = 12, MonthName = "December" }
                };
            return months;
        }

        public List<ProjectsProcurementHeaderViewModel> GetProcurementProjects(string Email)
        {
            var Office = db.UserAccounts.Where(d => d.Email == Email).FirstOrDefault().FKUserInformationReference.Office;
            return db.ProjectProcurementPlan
                   .Where(d => d.Office == Office)
                   .Select(d => new ProjectsProcurementHeaderViewModel
                   {
                       ProjectCode = d.ProjectCode,
                       ProjectName = d.ProjectName,
                       Description = d.Description,
                       FiscalYear = d.FiscalYear,
                       ProjectStatus = d.ProjectStatus,
                       ProjectMonthStart = d.ProjectMonthStart,
                       CreatedAt = d.CreatedAt
                   }).ToList();
        }

        public bool SaveProject(Basket projectBasket, string Email, string Type)
        {
            var user = db.UserAccounts.Where(d => d.Email == Email).FirstOrDefault();
            var office = hris.OfficeModel.Find(user.FKUserInformationReference.Office);
            projectBasket.BasketHeader.CreatedAt = DateTime.Now;
            projectBasket.BasketHeader.Office = office.ID;
            projectBasket.BasketHeader.PreparedBy = user.ID;
            projectBasket.BasketHeader.SubmittedBy = office.OfficeHead;
            projectBasket.BasketHeader.ProjectStatus = "New Project";
            projectBasket.BasketHeader.PurgeFlag = false;
            projectBasket.BasketHeader.ProjectCode = GenerateProjectCode(projectBasket.BasketHeader.FiscalYear, office.OfficeName, Type);

            ProjectProcurementPlan header = new ProjectProcurementPlan();
            header = projectBasket.BasketHeader;

            db.ProjectProcurementPlan.Add(header);
            if(db.SaveChanges() == 0)
            {
                return false;
            }

            foreach(var item in projectBasket.BasketItems)
            {
                ProjectProcurementPlanItems items = new ProjectProcurementPlanItems();
                items.ProjectReference = header.ID;
                items.ItemReference = item.ItemID;
                items.Qtr1 = (int)item.Qtr1Qty;
                items.Qtr2 = (int)item.Qtr2Qty;
                items.Qtr3 = (int)item.Qtr3Qty;
                items.Qtr4 = (int)item.Qtr4Qty;
                items.TotalQty = (int)item.TotalQty;
                items.Remarks = item.Remarks;

                db.ProjectProcurementPlanItems.Add(items);
                if (db.SaveChanges() == 0)
                {
                    return false;
                }

                if (!string.IsNullOrEmpty(item.Supplier1ID.ToString()) || !string.IsNullOrEmpty(item.Supplier2ID.ToString()) || !string.IsNullOrEmpty(item.Supplier3ID.ToString()) )
                {
                    MarketSurvey ms = new MarketSurvey();
                    ms.ProjectItemReference = items.ID;
                    ms.IsSoleDistributor = item.IsSoleDistributor;
                    ms.Supplier1Reference = item.Supplier1ID;
                    ms.Supplier1UnitCost = item.Supplier1UnitCost;
                    ms.Supplier1EstimatedBudget = (string.IsNullOrEmpty(item.Supplier1UnitCost.ToString())) ? 0 : (item.TotalQty * item.Supplier1UnitCost);
                    ms.Supplier2Reference = item.Supplier2ID;
                    ms.Supplier2UnitCost = item.Supplier2UnitCost;
                    ms.Supplier2EstimatedBudget = (string.IsNullOrEmpty(item.Supplier2UnitCost.ToString())) ? 0 : (item.TotalQty * item.Supplier2UnitCost);
                    ms.Supplier3Reference = item.Supplier3ID;
                    ms.Supplier3UnitCost = item.Supplier3UnitCost;
                    ms.Supplier3EstimatedBudget = (string.IsNullOrEmpty(item.Supplier3UnitCost.ToString())) ? 0 : (item.TotalQty * item.Supplier3UnitCost);
                    ms.TotalEstimatedBudget = ms.Supplier1EstimatedBudget + ms.Supplier2EstimatedBudget + ms.Supplier3EstimatedBudget;

                    db.MarketSurvey.Add(ms);
                    if (db.SaveChanges() == 0)
                    {
                        return false;
                    }

                    return true;
                }
            }
            return false;
        }

        private string GenerateProjectCode(string FiscalYear, string OfficeName, string Type)
        {
            var office = hris.OfficeModel.Where(d => d.OfficeName == OfficeName).FirstOrDefault();
            var series = db.ProjectProcurementPlan.Where(d => d.Office == office.ID && d.FiscalYear == FiscalYear).Count() + 1;
            var seriesStr = (series.ToString().Length == 1) ? "00" + series.ToString() : (series.ToString().Length == 2) ? "0" + series.ToString() : series.ToString();
            return Type + "-" + office.OfficeCode + "-" + seriesStr + "-" + FiscalYear;
        }

        public ProjectProcurementViewModel GetProjectDetails(string ProjectCode)
        {
            ProjectProcurementViewModel projectViewModel = new ProjectProcurementViewModel();
            projectViewModel.Header = db.ProjectProcurementPlan.Where(d => d.ProjectCode == ProjectCode && d.PurgeFlag == false).FirstOrDefault();
            projectViewModel.Items = db.ProjectProcurementPlanItems.Where(d => d.FKProjectReference.ID == projectViewModel.Header.ID).ToList();
            return projectViewModel;
        }

        public ProjectProcurementPlanItems GetProjectItem(string ProjectCode, string ItemCode)
        {
            return db.ProjectProcurementPlanItems.Where(d => d.FKProjectReference.ProjectCode == ProjectCode && d.FKItemReference.ItemCode == ItemCode).FirstOrDefault();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
                hris.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}