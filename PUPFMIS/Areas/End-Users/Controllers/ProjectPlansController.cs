using PUPFMIS.BusinessAndDataLogic;
using PUPFMIS.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Mvc;
using System.Linq;

namespace PUPFMIS.Controllers
{
    [RouteArea("end-users")]
    [RoutePrefix("projects-plans")]
    [UserAuthorization(Roles = SystemRoles.SuperUser + ", " + SystemRoles.EndUser)]
    public class ProjectPlansController : Controller
    {
        ProjectPlansDAL projectPlanDAL = new ProjectPlansDAL();

        [ActionName("infra-list")]
        [Route("{FiscalYear}/infrastructure-plans/request")]
        public ActionResult ViewInfraRequestList(int FiscalYear)
        {
            ViewBag.FiscalYear = FiscalYear;
            return View();
            //return View("IndexInfraRequest", projectPlanDAL.GetInfraRequests(User.Identity.Name, FiscalYear));
        }

        [ActionName("list")]
        [Route("{FiscalYear}")]
        public ActionResult ViewProjectList(int FiscalYear)
        {
            ViewBag.FiscalYear = FiscalYear;
            return View("Index", projectPlanDAL.GetProjects(User.Identity.Name, FiscalYear));
        }

        [Route("create/common-supplies")]
        [ActionName("create-common-supplies")]
        public ActionResult CreateCommonSupplies()
        {
            var fiscalYears = projectPlanDAL.GetFiscalYears();
            var papCodes = projectPlanDAL.GetPrograms();
            var units = projectPlanDAL.GetProjectUnits(ProjectTypes.CommonSuppliesProjectPlan, User.Identity.Name, fiscalYears[0], papCodes[0].PAPCode);

            if (Session["ProjectPlanSupplies"] == null)
            {
                ProjectPlanVM projectPlan = new ProjectPlanVM();
                projectPlan.ProjectCode = "CSPR-XXXX-0000-0000";
                projectPlan.ProjectName = "Supply and Delivery of Common Use Office Supplies";
                projectPlan.Description = "Procurement of common use office supplies to be used for daily transactions of the Office.";
                projectPlan.DeliveryMonth = "January";
                projectPlan.ProjectType = ProjectTypes.CommonSuppliesProjectPlan;
                projectPlan.ProjectPlanItems = new List<BasketItems>();
                projectPlan.ProjectPlanItems = Session["ProjectPlanSupplies"] == null ? projectPlanDAL.GetActualObligation(units[0].DepartmentCode, fiscalYears[0]) : ((ProjectPlanVM)Session["ProjectPlanSupplies"]).ProjectPlanItems;
                Session["ProjectPlanSupplies"] = projectPlan;
            }

            ViewBag.Unit = new SelectList(units, "DepartmentCode", "Department");
            ViewBag.DeliveryMonth = "January";
            ViewBag.FiscalYear = new SelectList(fiscalYears);
            ViewBag.PAPCode = new SelectList(papCodes, "PAPCode", "GeneralDescription");

            return View("CreateSupplies", Session["ProjectPlanSupplies"] as ProjectPlanVM);
        }

        [Route("create/common-supplies/{FiscalYear}/{UnitCode}/{PAPCode}")]
        [ActionName("create-common-supplies-selected")]
        public ActionResult CreateCommonSupplies(int FiscalYear, string UnitCode, string PAPCode)
        {
            var fiscalYears = projectPlanDAL.GetFiscalYears();
            var papCodes = projectPlanDAL.GetPrograms();
            var units = projectPlanDAL.GetProjectUnits(ProjectTypes.CommonSuppliesProjectPlan, User.Identity.Name, FiscalYear, PAPCode);

            if (Session["ProjectPlanSupplies"] == null)
            {
                ProjectPlanVM projectPlan = new ProjectPlanVM();
                projectPlan.ProjectCode = "CSPR-XXXX-0000-0000";
                projectPlan.ProjectName = "Supply and delivery of Common Use Office Supplies";
                projectPlan.Description = "Procurement of common use office supplies to be used for daily transactions of the Office.";
                projectPlan.DeliveryMonth = "January";
                projectPlan.ProjectPlanItems = new List<BasketItems>();
                projectPlan.ProjectPlanItems = projectPlanDAL.GetActualObligation(UnitCode, FiscalYear);
                Session["ProjectPlanSupplies"] = projectPlan;
            }

            ViewBag.Unit = new SelectList(units, "DepartmentCode", "Department", UnitCode);
            ViewBag.DeliveryMonth = "January";
            ViewBag.FiscalYear = new SelectList(fiscalYears, "", "", FiscalYear);
            ViewBag.PAPCode = new SelectList(papCodes, "PAPCode", "GeneralDescription", PAPCode);

            return View("CreateSupplies", Session["ProjectPlanSupplies"] as ProjectPlanVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("create/common-supplies")]
        [ActionName("create-common-supplies")]
        public ActionResult CreateCommonSupplies(ProjectPlanVM ProjectPlanVM)
        {
            var actualObligation = ((ProjectPlanVM)Session["ProjectPlanSupplies"]).ProjectPlanItems;

            if (actualObligation != null && actualObligation.Where(d => d.EstimatedBudget == null).Count() != 0)
            {
                ModelState.AddModelError("", "Please update the Supplier Information and Unit Cost for items from External Suppliers.");
                var units = projectPlanDAL.GetUserDepartments(User.Identity.Name);
                var fiscalYears = projectPlanDAL.GetFiscalYears();

                ViewBag.Unit = new SelectList(units, "DepartmentCode", "Department", ProjectPlanVM.Unit);
                ViewBag.DeliveryMonth = "January";
                ViewBag.FiscalYear = new SelectList(fiscalYears, ProjectPlanVM.FiscalYear);
                ViewBag.PAPCode = new SelectList(projectPlanDAL.GetPrograms(), "PAPCode", "GeneralDescription");

                return PartialView("_CreateSupplies", Session["ProjectPlanSupplies"] as ProjectPlanVM);
            }
            var system = new SystemBDL();
            var projectPlan = new ProjectPlans()
            {
                PAPCode = ProjectPlanVM.PAPCode,
                ProjectCode = ProjectPlanVM.ProjectCode,
                ProjectName = ProjectPlanVM.ProjectName,
                DeliveryMonth = 1,
                Description = ProjectPlanVM.Description,
                Unit = ProjectPlanVM.Unit,
                FiscalYear = ProjectPlanVM.FiscalYear,
                CreatedAt = DateTime.Now,
                PurgeFlag = false
            };

            if (ModelState.IsValid)
            {
                string Message = string.Empty;
                if (!projectPlanDAL.ValidateProjectPlan(projectPlan, User.Identity.Name, out Message))
                {
                    ModelState.AddModelError("", Message);
                    var units = projectPlanDAL.GetUserDepartments(User.Identity.Name);
                    var fiscalYears = projectPlanDAL.GetFiscalYears();

                    ViewBag.Unit = new SelectList(units, "DepartmentCode", "Department", ProjectPlanVM.Unit);
                    ViewBag.DeliveryMonth = "January";
                    ViewBag.FiscalYear = new SelectList(fiscalYears, ProjectPlanVM.FiscalYear);
                    ViewBag.PAPCode = new SelectList(projectPlanDAL.GetPrograms(), "PAPCode", "GeneralDescription");

                    return PartialView("_CreateSupplies", Session["ProjectPlanSupplies"] as ProjectPlanVM);
                }

                string projectCode = string.Empty;
                if (projectPlanDAL.SaveProjectPlan(projectPlan, actualObligation, User.Identity.Name, out projectCode))
                {
                    if (projectCode == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
                    }
                    Session.Clear();
                    return Json(new { result = true, projectCode = projectCode });
                }
            }
            ViewBag.Unit = new SelectList(projectPlanDAL.GetUserDepartments(User.Identity.Name), "DepartmentCode", "Department");
            ViewBag.StartMonth = "January";
            ViewBag.FiscalYear = projectPlanDAL.GetFiscalYears();
            ViewBag.PAPCode = new SelectList(projectPlanDAL.GetPrograms(), "PAPCode", "GeneralDescription");
            return PartialView("_CreateSupplies", projectPlan);
        }

        [Route("create/project-plan")]
        [ActionName("create-project-plan")]
        public ActionResult CreateProjectPlan()
        {
            var fiscalYears = projectPlanDAL.GetFiscalYears();

            ProjectPlans header = new ProjectPlans();
            header.ProjectCode = "EUPR-XXXX-0000-0000";
            header.ProjectType = ProjectTypes.EndUserProjectPlan;

            ViewBag.Unit = new SelectList(projectPlanDAL.GetUserDepartments(User.Identity.Name), "DepartmentCode", "Department");
            ViewBag.DeliveryMonth = projectPlanDAL.GetMonths();
            ViewBag.FiscalYear = new SelectList(fiscalYears);
            ViewBag.PAPCode = new SelectList(projectPlanDAL.GetPrograms(), "PAPCode", "GeneralDescription");
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
            var fiscalYears = projectPlanDAL.GetFiscalYears();
            if (ModelState.IsValid)
            {
                string Message = string.Empty;
                if (!projectPlanDAL.ValidateProjectPlan(projectPlan, User.Identity.Name, out Message))
                {
                    ModelState.AddModelError("", Message);
                    ViewBag.Unit = new SelectList(projectPlanDAL.GetUserDepartments(User.Identity.Name), "DepartmentCode", "Department");
                    ViewBag.ProjectMonthStart = projectPlanDAL.GetMonths();
                    ViewBag.FiscalYear = new SelectList(fiscalYears);
                    ViewBag.PAPCode = new SelectList(projectPlanDAL.GetPrograms(), "PAPCode", "GeneralDescription");
                    return PartialView("_CreateProjectPlan", projectPlan);
                }

                string projectCode = string.Empty;
                if (projectPlanDAL.SaveProjectPlan(projectPlan, new List<BasketItems>(), User.Identity.Name, out projectCode))
                {
                    if (projectCode == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
                    }
                    return Json(new { result = true, projectCode = projectCode });
                }
            }

            ViewBag.Unit = new SelectList(projectPlanDAL.GetUserDepartments(User.Identity.Name), "DepartmentCode", "Department");
            ViewBag.DeliveryMonth = projectPlanDAL.GetMonths();
            ViewBag.FiscalYear = new SelectList(fiscalYears);
            ViewBag.PAPCode = new SelectList(projectPlanDAL.GetPrograms(), "PAPCode", "GeneralDescription");
            return PartialView("_CreateProjectPlan", projectPlan);
        }

        [ActionName("project-details")]
        [Route("{ProjectCode}/details")]
        public ActionResult ProjectDetails(string ProjectCode)
        {
            if (ProjectCode == null)
            {
                return RedirectToAction("not-found", "Errors", new { Area = "" });
            }
            var projectPlan = projectPlanDAL.GetProjectDetails(ProjectCode, User.Identity.Name);
            if (projectPlan == null)
            {
                return RedirectToAction("record-not-found", "Errors", new { Area = "" });
            }
            var projectItemsCount = projectPlan.ProjectPlanItems.Where(d => d.ProcurementSource == Models.ProcurementSources.ExternalSuppliers.GetAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>().Name).Count();
            var acceptedItems = projectPlan.ProjectPlanItems.Where(d => d.ProjectItemStatus == Models.ProjectDetailsStatus.ItemAccepted.GetAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>().Name).Count();
            ViewBag.AllAccepted = projectItemsCount == acceptedItems ? true : false;
            return View("Details", projectPlan);
        }

        [ActionName("project-item-details")]
        [Route("{ProjectCode}/{ItemCode}/details")]
        public ActionResult ItemDetails(string ProjectCode, string ItemCode)
        {
            if (ProjectCode == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.ProjectCode = ProjectCode;
            return View("ItemDetails", projectPlanDAL.GetProjectDetails(ProjectCode, ItemCode, User.Identity.Name));
        }

        [HttpPost]
        [ActionName("project-item-details")]
        [Route("{ProjectCode}/{ItemCode}/details")]
        public ActionResult ItemDetails(BasketItems Item, string ProjectCode)
        {
            if (Item == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item.UnitCost = projectPlanDAL.ComputeUnitCost(Item.Supplier1UnitCost, Item.Supplier2UnitCost, Item.Supplier3UnitCost);
            Item.TotalQty = Item.JanQty + Item.FebQty + Item.MarQty + Item.AprQty + Item.MayQty + Item.JunQty + Item.JulQty + Item.AugQty + Item.SepQty + Item.OctQty + Item.NovQty + Item.DecQty;
            Item.EstimatedBudget = Item.UnitCost * Item.TotalQty;
            return Json(new { result = projectPlanDAL.UpdateProjectItem(Item, ProjectCode) });
        }

        [ActionName("update-actual-obligation")]
        [Route("update-actual-obligation/{ItemCode}")]
        public ActionResult UpdateActualObligation(string ItemCode)
        {
            var projectPlan = Session["ProjectPlanSupplies"] as ProjectPlanVM;
            var projectItem = projectPlan.ProjectPlanItems.Where(d => d.ItemCode == ItemCode).FirstOrDefault();
            return View("UpdateActualObligation", projectItem);
        }

        [HttpPost]
        [ActionName("update-actual-obligation")]
        [Route("update-actual-obligation/{ItemCode}")]
        public ActionResult UpdateActualObligation(BasketItems BasketItem)
        {
            var projectPlan = Session["ProjectPlanSupplies"] as ProjectPlanVM;
            var projectItem = projectPlan.ProjectPlanItems.Where(d => d.ItemCode == BasketItem.ItemCode).FirstOrDefault();

            BasketItem.TotalQty = BasketItem.JanQty + BasketItem.FebQty + BasketItem.MarQty + BasketItem.AprQty + BasketItem.MayQty + BasketItem.JunQty + BasketItem.JulQty + BasketItem.AugQty + BasketItem.SepQty + BasketItem.OctQty + BasketItem.NovQty + BasketItem.DecQty;
            if (BasketItem.ProcurementSource == "External Suppliers")
            {
                BasketItem.UnitCost = projectPlanDAL.ComputeUnitCost(BasketItem.Supplier1UnitCost, BasketItem.Supplier2UnitCost, BasketItem.Supplier3UnitCost);
                BasketItem.UnitCost = Math.Round((decimal)BasketItem.UnitCost, 2, MidpointRounding.AwayFromZero);
                BasketItem.EstimatedBudget = BasketItem.UnitCost * BasketItem.TotalQty;
            }

            projectPlan.ProjectPlanItems.Remove(projectItem);

            projectPlan.ProjectPlanItems.Add(BasketItem);
            Session["ProjectPlanSupplies"] = projectPlan;
            return Json(new { result = true });
        }

        [HttpGet]
        [ActionName("remove-actual-obligation")]
        [Route("remove-actual-obligation/{ItemCode}")]
        public ActionResult RemoveActualObligation(string ItemCode)
        {
            var projectPlan = Session["ProjectPlanSupplies"] as ProjectPlanVM;
            var itemToRemove = projectPlan.ProjectPlanItems.Where(d => d.ItemCode == ItemCode).FirstOrDefault();
            projectPlan.ProjectPlanItems.Remove(itemToRemove);
            return Json(new { result = true }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ActionName("submit-project")]
        [Route("{ProjectCode}/details")]
        public ActionResult SubmitProject(string ProjectCode)
        {
            if (ProjectCode == null)
            {
                return HttpNotFound();
            }
            return Json(new { result = projectPlanDAL.ForwardToResponsibilityCenter(ProjectCode, User.Identity.Name) });
        }

        [HttpPost]
        [Route("{FiscalYear}/post-to-ppmp")]
        [ValidateAntiForgeryToken]
        [ActionName("post-project-to-ppmp")]
        public ActionResult PosttoPPMP(int FiscalYear)
        {
            var ppmpDAL = new ProjectProcurementManagementPlanDAL();
            return Json(new { result = ppmpDAL.PostToPPMP(FiscalYear, User.Identity.Name) });
        }

        [ActionName("get-project")]
        [Route("{ProjectCode}/get-project")]
        public ActionResult GetProject(string ProjectCode)
        {
            return Json(projectPlanDAL.GetProjectDetails(ProjectCode, User.Identity.Name), JsonRequestBehavior.AllowGet);
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //[ActionName("forward-to-implementing-unit")]
        //[Route("{FiscalYear}infrastructure-plans/request")]
        //public ActionResult ForwardInfraRequests(int FiscalYear)
        //{
        //    return Json(new { result = projectPlanDAL.ForwardInfraRequest(FiscalYear, User.Identity.Name) });
        //}

        [ActionName("print-description")]
        [Route("{ProjectCode}/print")]
        public ActionResult PrintProjectDescription(string ProjectCode)
        {
            var ppmpBL = new ProjectPlansBL();
            var stream = ppmpBL.GenerateProjectPlansReport(ProjectCode, Server.MapPath("~/Content/imgs/PUPLogo.png"), User.Identity.Name);
            Response.Clear();
            Response.ClearContent();
            Response.ClearHeaders();
            Response.AddHeader("content-length", stream.Length.ToString());
            //Response.AddHeader("content-disposition", "attachment; filename=" + ReferenceNo + ".pdf");
            Response.ContentType = "application/pdf";
            Response.BinaryWrite(stream.ToArray());
            stream.Close();
            Response.End();

            return RedirectToAction("list", new { Area = "end-users" });
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
}