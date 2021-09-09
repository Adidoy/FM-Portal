using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PUPFMIS.Models;
using PUPFMIS.BusinessAndDataLogic;

namespace PUPFMIS.Areas.ResponsibilityCenters.Controllers
{
    [RoutePrefix("planning")]
    [RouteArea("responsibility-centers")]
    [Authorize(Roles = SystemRoles.ResponsibilityCenterPlanner + ", " + SystemRoles.SuppliesChief + ", " + SystemRoles.PropertyDirector)]
    public class StrategicPlanningController : Controller
    {
        private StrategicPlanningDAL stratPlanDataAccess = new StrategicPlanningDAL();

        [Route("dashboard")]
        public ActionResult Dashboard()
        {
            return View();
        }

        [Route("offices")]
        [ActionName("office")]
        public ActionResult OfficeList()
        {
            return View("OfficeList", stratPlanDataAccess.GetDepartments(User.Identity.Name));
        }

        [Route("{DepartmentCode}/items")]
        [ActionName("item-list")]
        public ActionResult ItemList(string DepartmentCode)
        {
            return View("ItemList", stratPlanDataAccess.GetProjectItems(User.Identity.Name, DepartmentCode));
        }

        [ActionName("details")]
        [Route("{DepartmentCode}/items/{ItemCode}/details")]
        public ActionResult Details(string ItemCode, string DepartmentCode)
        {
            ViewBag.DepartmentCode = DepartmentCode;
            if (ItemCode == null)
            {
                return RedirectToAction("not-found", "Errors", new { Area = "" });
            }
            var planItem = stratPlanDataAccess.GetItem(ItemCode, DepartmentCode);
            if (planItem == null)
            {
                return RedirectToAction("record-not-found", "Errors", new { Area = "" });
            }
            return View("Details", planItem);
        }

        [HttpPost]
        [ActionName("update-plan")]
        [ValidateAntiForgeryToken]
        [Route("{DepartmentCode}/items/{ItemCode}/details")]
        public ActionResult Details(InstitutionalItemPlan InstitutionalPlan)
        {
            return Json(new { result = stratPlanDataAccess.UpdateProjectDetails(InstitutionalPlan, User.Identity.Name) });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                stratPlanDataAccess.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}