using PUPFMIS.BusinessAndDataLogic;
using PUPFMIS.Models;
using System;
using System.Net;
using System.Web.Mvc;

namespace PUPFMIS.Areas.EndUsers.Controllers
{
    [Route("{action}")]
    [RouteArea("end-users")]
    [RoutePrefix("projects/plans")]
    [Authorize(Roles = SystemRoles.SuperUser + ", " + SystemRoles.EndUser)]
    public class ProjectPlansController : Controller
    {
        ProjectPlansDAL projectPlanDAL = new ProjectPlansDAL();
        HRISDataAccess hrisDataAccess = new HRISDataAccess();

        [ActionName("list")]
        [Route("{FiscalYear}")]
        public ActionResult ViewProjectList(int FiscalYear)
        {
            return View("Index", projectPlanDAL.GetProjects(User.Identity.Name, FiscalYear));
        }

        [Route("create/common-supplies")]
        [ActionName("create-common-supplies")]
        public ActionResult CreateCommonSupplies()
        {
            ProjectPlans header = new ProjectPlans();
            header.ProjectCode = "CSPR-XXXX-0000-0000";
            header.ProjectName = "Supply and delivery of Common Use Office Supplies";
            header.Description = "Procurement of  common use office supplies to be used for daily transactions of the Office.";
            header.ProjectMonthStart = 1;

            ViewBag.Unit = new SelectList(hrisDataAccess.GetUserDepartments(User.Identity.Name), "DepartmentCode", "Department");
            ViewBag.StartMonth = "January";
            ViewBag.FiscalYear = projectPlanDAL.GetFiscalYears();

            return View("CreateSupplies", header);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("create/common-supplies")]
        [ActionName("create-common-supplies")]
        public ActionResult CreateCommonSupplies(ProjectPlans projectPlan)
        {
            projectPlan.CreatedAt = DateTime.Now;
            projectPlan.PurgeFlag = false;

            if (ModelState.IsValid)
            {
                string Message = string.Empty;
                if(!projectPlanDAL.ValidateProjectPlan(projectPlan, User.Identity.Name, out Message))
                {
                    ModelState.AddModelError("", Message);
                    ViewBag.Unit = new SelectList(hrisDataAccess.GetUserDepartments(User.Identity.Name), "DepartmentCode", "Department");
                    ViewBag.StartMonth = "January";
                    ViewBag.FiscalYear = projectPlanDAL.GetFiscalYears();
                    return View("CreateSupplies", projectPlan);
                }

                string projectCode = string.Empty;
                if (projectPlanDAL.SaveProjectPlan(projectPlan, User.Identity.Name, out projectCode))
                {
                    if(projectCode == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
                    }
                    return RedirectToAction("project-details", "ProjectPlans", new { Area = "end-users", ProjectCode = projectCode });
                }
            }
            ViewBag.Unit = new SelectList(hrisDataAccess.GetUserDepartments(User.Identity.Name), "DepartmentCode", "Department");
            ViewBag.StartMonth = "January";
            ViewBag.FiscalYear = projectPlanDAL.GetFiscalYears();
            return View("CreateSupplies", projectPlan);
        }

        [Route("create/project-plan")]
        [ActionName("create-project-plan")]
        public ActionResult CreateProjectPlan()
        {
            ProjectPlans header = new ProjectPlans();
            header.ProjectCode = "EUPR-XXXX-0000-0000";

            ViewBag.Unit = new SelectList(hrisDataAccess.GetUserDepartments(User.Identity.Name), "DepartmentCode", "Department");
            ViewBag.ProjectMonthStart = projectPlanDAL.GetMonths();
            ViewBag.FiscalYear = projectPlanDAL.GetFiscalYears();
            return View("CreateProjectPlan", header);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("create/project-plan")]
        [ActionName("create-project-plan")]
        public ActionResult CreateProjectPlan(ProjectPlans projectPlan)
        {
            projectPlan.CreatedAt = DateTime.Now;
            projectPlan.PurgeFlag = false;

            if (ModelState.IsValid)
            {
                string Message = string.Empty;
                if (!projectPlanDAL.ValidateProjectPlan(projectPlan, User.Identity.Name, out Message))
                {
                    ModelState.AddModelError("", Message);
                    ViewBag.Unit = new SelectList(hrisDataAccess.GetUserDepartments(User.Identity.Name), "DepartmentCode", "Department");
                    ViewBag.ProjectMonthStart = projectPlanDAL.GetMonths();
                    ViewBag.FiscalYear = projectPlanDAL.GetFiscalYears();
                    return View("CreateProjectPlan", projectPlan);
                }

                string projectCode = string.Empty;
                if (projectPlanDAL.SaveProjectPlan(projectPlan, User.Identity.Name, out projectCode))
                {
                    if (projectCode == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
                    }
                    return RedirectToAction("project-details", "ProjectPlans", new { Area = "end-users", ProjectCode = projectCode });
                }
            }

            ViewBag.Unit = new SelectList(hrisDataAccess.GetUserDepartments(User.Identity.Name), "DepartmentCode", "Department");
            ViewBag.ProjectMonthStart = projectPlanDAL.GetMonths();
            ViewBag.FiscalYear = projectPlanDAL.GetFiscalYears();
            return View("CreateProjectPlan", projectPlan);
        }

        [ActionName("project-details")]
        [Route("{ProjectCode}/details")]
        public ActionResult DetailsSupplies(string ProjectCode)
        {
            if(ProjectCode == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var projectPlan = projectPlanDAL.GetProjectDetails(ProjectCode, User.Identity.Name);
            if(projectPlan == null)
            {
                return HttpNotFound();
            }
            return View("DetailsSupplies", projectPlan);
        }

        [ActionName("update-item")]
        [Route("{ProjectCode}/{InventoryType}/{ItemCode}/update")]
        public ActionResult UpdateItem(string ProjectCode, string InventoryType, string ItemCode)
        {
            if(ProjectCode == null)
            {
                return new HttpNotFoundResult();
            }
            if(ItemCode == null)
            {
                return new HttpNotFoundResult();
            }
            var projectItem = projectPlanDAL.GetProjectItem(ProjectCode, InventoryType, ItemCode);
            if (projectItem == null)
            {
                return new HttpNotFoundResult();
            }
            if(projectItem.ProjectCode.Substring(0, 4) == "CSPR")
            {
                return View("UpdateCommonSupplyItem", projectItem);
            }
            else
            {
                return View("UpdateItem", projectItem);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("update-item")]
        [Route("{ProjectCode}/{InventoryType}/{ItemCode}/update")]
        public ActionResult UpdateItem(ProjectPlanItemsVM item)
        {
            string Message = string.Empty;
            if(!projectPlanDAL.ValidateUpdateItem(item, item.ProjectCode, out Message))
            {
                ModelState.AddModelError("", Message);
                return View("UpdateItem", item);
            }
            if(projectPlanDAL.UpdateItem(item, out Message))
            {
                return RedirectToAction("project-details", "ProjectPlans", new { Area = "end-users", ProjectCode = item.ProjectCode });
            }
            ModelState.AddModelError("", Message);
            return View("UpdateItem", item);
        }

        [ActionName("remove-common-supply-item")]
        [Route("common-supplies/{ProjectCode}/item/{ItemCode}/remove")]
        public ActionResult RemoveItem(CatalogueBasketItemVM item)
        {
            string Message = string.Empty;
            if(!projectPlanDAL.RemoveItem(item, out Message))
            {
                return RedirectToAction("project-details", "ProjectPlans", new { Area = "end-users", ProjectCode = item.ProjectCode });
            }
            return RedirectToAction("project-details", "ProjectPlans", new { Area = "end-users", ProjectCode = item.ProjectCode });
        }

        [ActionName("add-supplies-to-ppmp")]
        [Route("common-supplies/{ProjectCode}/add-to-ppmp")]
        public ActionResult AddToPPMP(string ProjectCode)
        {
            if(ProjectCode == null)
            {
                return HttpNotFound();
            }
            var projectPlan = projectPlanDAL.GetProjectDetails(ProjectCode, User.Identity.Name);
            if (projectPlan == null)
            {
                return HttpNotFound();
            }
            if(projectPlanDAL.PostToPPMP(projectPlan, User.Identity.Name))
            {
                return View("Index", projectPlanDAL.GetProjects(User.Identity.Name, projectPlan.FiscalYear));
            }
            return View("Index", projectPlanDAL.GetProjects(User.Identity.Name, projectPlan.FiscalYear));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                projectPlanDAL.Dispose();
                hrisDataAccess.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
