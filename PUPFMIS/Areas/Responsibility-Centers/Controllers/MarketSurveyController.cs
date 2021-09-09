using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PUPFMIS.Models;
using PUPFMIS.BusinessAndDataLogic;

namespace PUPFMIS.Areas.Responsibility_Centers.Controllers
{
    [RoutePrefix("market-survey")]
    [RouteArea("responsibility-centers")]
    [Authorize(Roles = SystemRoles.ResponsibilityCenterPlanner + ", " + SystemRoles.SuppliesChief + ", " + SystemRoles.PropertyDirector)]
    public class MarketSurveyController : Controller
    {
        private MarketSurveyDAL msDataAccess = new MarketSurveyDAL();

        [Route("")]
        [ActionName("dashboard")]
        public ActionResult Dashboard()
        {
            var dashboard = new MarketSurveyDashboard();
            dashboard.NoOfNewItems = msDataAccess.GetNewItems(User.Identity.Name).Count();
            dashboard.NoOfLapsedMarketSurvey = msDataAccess.GetNoOfLapsedMS(User.Identity.Name);
            dashboard.NoOfUpdatedMarketSurvey = msDataAccess.GetNoOfUpdatedMS(User.Identity.Name);
            dashboard.MarketSurveyList = msDataAccess.GetMarketSurvey(User.Identity.Name);
            return View(dashboard);
        }

        [Route("new-items")]
        [ActionName("new-items")]
        public ActionResult NewItems()
        {
            return View("NewItems", msDataAccess.GetNewItems(User.Identity.Name));
        }

        [Route("items")]
        [ActionName("items")]
        public ActionResult ItemsList()
        {
            return View("ItemsList", msDataAccess.GetItems(User.Identity.Name));
        }

        [ActionName("details")]
        [Route("{ItemCode}/details")]
        public ActionResult Details(string ItemCode)
        {
            if (ItemCode == null || ItemCode == string.Empty)
            {
                return RedirectToAction("page-not-found", "Errors", new { Area = "" });
            }
            var marketSurvey = msDataAccess.GetItem(ItemCode);
            if (marketSurvey == null)
            {
                return RedirectToAction("record-not-found", "Errors", new { Area = "" });
            }
            return View(msDataAccess.GetItem(ItemCode));
        }

        [ActionName("create-from-planning")]
        [Route("from-planning/{DepartmentCode}/items/{ItemCode}/update")]
        public ActionResult CreateFromPlanning(string ItemCode, string DepartmentCode)
        {
            ViewBag.FromPlanning = true;
            ViewBag.Department = DepartmentCode;
            if (ItemCode == null || ItemCode == string.Empty)
            {
                return RedirectToAction("page-not-found", "Errors", new { Area = "" });
            }
            var marketSurvey = msDataAccess.GetItem(ItemCode);
            if (marketSurvey.UnitCost == null && marketSurvey.LastUpdated == null)
            {
                return View("Create", marketSurvey);
            }
            else
            {
                return RedirectToAction("update", "MarketSurvey", new { ItemCode = ItemCode, Area = "responsibility-centers" });
            }
        }

        [ActionName("create")]
        [Route("{ItemCode}/create")]
        public ActionResult Create(string ItemCode)
        {
            if (ItemCode == null || ItemCode == string.Empty)
            {
                return RedirectToAction("page-not-found", "Errors", new { Area = "" });
            }
            var marketSurvey = msDataAccess.GetItem(ItemCode);
            if (marketSurvey == null)
            {
                return RedirectToAction("record-not-found", "Errors", new { Area = "" });
            }
            return View(marketSurvey);
        }

        [HttpPost]
        [ActionName("create")]
        [Route("{ItemCode}/create")]
        public ActionResult Create(MarketSurveyVM MarketSurvey)
        {
            return Json(new { result = msDataAccess.CreateMarketSurvey(MarketSurvey, User.Identity.Name) });
        }

        [ActionName("update")]
        [Route("{ItemCode}/update")]
        public ActionResult Update(string ItemCode)
        {
            if (ItemCode == null || ItemCode == string.Empty)
            {
                return RedirectToAction("page-not-found", "Errors", new { Area = "" });
            }
            var marketSurvey = msDataAccess.GetItem(ItemCode);
            if (marketSurvey == null)
            {
                return RedirectToAction("record-not-found", "Errors", new { Area = "" });
            }
            return View(marketSurvey);
        }

        [HttpPost]
        [ActionName("update")]
        [Route("{ItemCode}/update")]
        public ActionResult Update(MarketSurveyVM MarketSurvey)
        {
            return Json(new { result = msDataAccess.UpdateMarketSurvey(MarketSurvey, User.Identity.Name) });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                msDataAccess.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}