using FluentValidation.Results;
using PUPFMIS.BusinessLayer;
using PUPFMIS.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace PUPFMIS.Controllers
{
    [Authorize]
    [Route("ops/procurement/planning/projects/{action}")]
    public class ProjectProcurementPlansController : Controller
    {
        private FMISDbContext db = new FMISDbContext();
        private ProcurementProjectsBL projectProcurement = new ProcurementProjectsBL();
        private CatalogueBL catalogueBL = new CatalogueBL();

        [ActionName("index")]
        public ActionResult Index()
        {
            return View(projectProcurement.GetProcurementProjects(User.Identity.Name));
        }

        [ActionName("details")]
        [Route("ops/procurement/planning/projects/{ProjectCode}/details")]
        public ActionResult Details(string ProjectCode)
        {
            if (ProjectCode == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectProcurementViewModel project = projectProcurement.GetProjectDetails(ProjectCode);
            if (project == null)
            {
                return HttpNotFound();
            }
            ViewBag.FiscalYear = new SelectList(projectProcurement.GetFiscalYears(), "FiscalYear", "FiscalYear");
            ViewBag.ProjectMonthStart = new SelectList(projectProcurement.GetMonths(), "MonthValue", "MonthName");
            return View(project);
        }

        [ActionName("create")]
        public ActionResult Create()
        {
            ProjectProcurementPlan header = new ProjectProcurementPlan();
            var months = projectProcurement.GetMonths();
            header.ProjectCode = "EUPR-XXXX-0000-0000";
            ViewBag.FiscalYear = new SelectList(projectProcurement.GetFiscalYears(), "FiscalYear", "FiscalYear");
            ViewBag.ProjectMonthStart = new SelectList(months, "MonthValue", "MonthName");
            return View("Create", header);
        }

        [HttpPost]
        [ActionName("create")]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProjectProcurementPlan projectProcurementPlan)
        {
            if (ModelState.IsValid)
            {
                Session["Basket"] = new Basket();
                ((Basket)Session["Basket"]).BasketHeader = projectProcurementPlan;
                ((Basket)Session["Basket"]).BasketItems = new List<BasketItem>();
                ViewBag.CategoryName = "All Categories";
                ViewBag.Categories = catalogueBL.GetCategories();
                ViewBag.Action = "NONCSE";
                return RedirectToAction("catalogue");
            }
            ViewBag.FiscalYear = new SelectList(projectProcurement.GetFiscalYears(), "FiscalYear", "FiscalYear");
            var months = projectProcurement.GetMonths();
            ViewBag.ProjectMonthStart = new SelectList(months, "MonthValue", "MonthName");
            projectProcurementPlan.ProjectCode = "EUPR-XXXX-0000-0000";
            return View("Create", projectProcurementPlan);
        }

        [ActionName("catalogue")]
        public ActionResult Catalogue()
        {
            if ((Session["Basket"] == null) || ((Basket)Session["Basket"]).BasketHeader == null)
            {
                return RedirectToAction("definition", "ProjectProcurementPlans");
            }

            ViewBag.CategoryName = "All Categories";
            ViewBag.Categories = catalogueBL.GetCategories();
            return View("Catalogue", catalogueBL.GetCatalogue());
        }

        [ActionName("add-to-basket")]
        [Route("ops/procurement/planning/projects/catalogue/{ItemCode}/add-to-basket")]
        public ActionResult AddToBasket(string ItemCode)
        {
            if ((Session["Basket"] == null) || ((Basket)Session["Basket"]).BasketHeader == null)
            {
                return RedirectToAction("definition", "ProjectProcurementPlans");
            }

            if (ItemCode == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            BasketItem item = new BasketItem();
            var basketItem = ((Basket)Session["Basket"]).BasketItems;
            item = (basketItem.Where(d => d.ItemCode == ItemCode).Count() == 1) ? item = basketItem.Where(d => d.ItemCode == ItemCode).FirstOrDefault() : item = catalogueBL.GetItems(ItemCode);

            if (item == null)
            {
                return HttpNotFound();
            }
            return View("AddToBasket", item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("add-to-basket")]
        [Route("ops/procurement/planning/projects/catalogue/{ItemCode}/add-to-basket")]
        public ActionResult AddToBasket(BasketItem itemBasket)
        {
            if ((Session["Basket"] == null) || ((Basket)Session["Basket"]).BasketHeader == null)
            {
                return RedirectToAction("definition", "ProjectProcurementPlans");
            }
            ValidateAddItem(itemBasket);
            if (ModelState.IsValid)
            {
                itemBasket.Qtr1Qty = (String.IsNullOrEmpty(itemBasket.Qtr1Qty.ToString())) ? 0 : (int)itemBasket.Qtr1Qty;
                itemBasket.Qtr2Qty = (String.IsNullOrEmpty(itemBasket.Qtr2Qty.ToString())) ? 0 : (int)itemBasket.Qtr2Qty;
                itemBasket.Qtr3Qty = (String.IsNullOrEmpty(itemBasket.Qtr3Qty.ToString())) ? 0 : (int)itemBasket.Qtr3Qty;
                itemBasket.Qtr4Qty = (String.IsNullOrEmpty(itemBasket.Qtr4Qty.ToString())) ? 0 : (int)itemBasket.Qtr4Qty;
                itemBasket.TotalQty = ((int)itemBasket.Qtr1Qty + (int)itemBasket.Qtr2Qty + (int)itemBasket.Qtr3Qty + (int)itemBasket.Qtr4Qty);


                var basketItems = ((Basket)Session["Basket"]).BasketItems;
                if (basketItems.Where(d => d.ItemCode == itemBasket.ItemCode).Count() == 1)
                {
                    basketItems.Where(d => d.ItemCode == itemBasket.ItemCode).FirstOrDefault().Qtr1Qty = itemBasket.Qtr1Qty;
                    basketItems.Where(d => d.ItemCode == itemBasket.ItemCode).FirstOrDefault().Qtr2Qty = itemBasket.Qtr2Qty;
                    basketItems.Where(d => d.ItemCode == itemBasket.ItemCode).FirstOrDefault().Qtr3Qty = itemBasket.Qtr3Qty;
                    basketItems.Where(d => d.ItemCode == itemBasket.ItemCode).FirstOrDefault().Qtr4Qty = itemBasket.Qtr4Qty;
                    basketItems.Where(d => d.ItemCode == itemBasket.ItemCode).FirstOrDefault().TotalQty = itemBasket.TotalQty;
                    basketItems.Where(d => d.ItemCode == itemBasket.ItemCode).FirstOrDefault().Remarks = (String.IsNullOrEmpty(itemBasket.Remarks)) ? "Acceptable" : itemBasket.Remarks;
                    ((Basket)Session["Basket"]).BasketItems = basketItems;
                }
                else
                {
                    itemBasket.Remarks = (String.IsNullOrEmpty(itemBasket.Remarks)) ? "Acceptable" : itemBasket.Remarks;
                    basketItems.Add(itemBasket);
                    ((Basket)Session["Basket"]).BasketItems = basketItems;
                }
                return RedirectToAction("catalogue", "ProjectProcurementPlans");
            }
            return View("AddToBasket", itemBasket);
        }

        [ActionName("view-basket")]
        [Route("ops/procurement/planning/projects/basket")]
        public ActionResult ViewBasket()
        {
            if ((Session["Basket"] == null) || ((Basket)Session["Basket"]).BasketHeader == null)
            {
                return RedirectToAction("definition", "ProjectProcurementPlans");
            }
            var basket = Session["Basket"];
            return View("ViewBasket", basket);
        }

        [ActionName("view-suppliers")]
        [Route("ops/procurement/planning/projects/catalogue/view-suppliers/{SupplierNo}")]
        public ActionResult ViewSuppliers(int SupplierNo)
        {
            ViewBag.SupplierNo = SupplierNo;
            return PartialView("SupplierView", catalogueBL.GetSuppliers());
        }

        [ActionName("remove-from-basket")]
        [Route("ops/procurement/planning/projects/basket/{ItemCode}/remove-from-basket")]
        public ActionResult RemoveFromBasket(string ItemCode)
        {
            ((Basket)Session["Basket"]).BasketItems.Remove(((Basket)Session["Basket"]).BasketItems.Where(d => d.ItemCode == ItemCode).FirstOrDefault());
            return RedirectToAction("view-basket");
        }

        [ActionName("update-basket-item")]
        [Route("ops/procurement/planning/projects/basket/{ItemCode}/update-basket-item")]
        public ActionResult UpdateBasket(string ItemCode)
        {
            if (ItemCode == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BasketItem updateItem = ((Basket)Session["Basket"]).BasketItems.Where(d => d.ItemCode == ItemCode).FirstOrDefault();
            if (updateItem == null)
            {
                return HttpNotFound();
            }
            return View("UpdateBasket", updateItem);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("update-basket-item")]
        [Route("ops/procurement/planning/projects/basket/{ItemCode}/update-basket-item")]
        public ActionResult Update(BasketItem itemBasket)
        {
            ValidateAddItem(itemBasket);
            if (ModelState.IsValid)
            {
                itemBasket.Qtr1Qty = (String.IsNullOrEmpty(itemBasket.Qtr1Qty.ToString())) ? 0 : (int)itemBasket.Qtr1Qty;
                itemBasket.Qtr2Qty = (String.IsNullOrEmpty(itemBasket.Qtr2Qty.ToString())) ? 0 : (int)itemBasket.Qtr2Qty;
                itemBasket.Qtr3Qty = (String.IsNullOrEmpty(itemBasket.Qtr3Qty.ToString())) ? 0 : (int)itemBasket.Qtr3Qty;
                itemBasket.Qtr4Qty = (String.IsNullOrEmpty(itemBasket.Qtr4Qty.ToString())) ? 0 : (int)itemBasket.Qtr4Qty;
                itemBasket.TotalQty = ((int)itemBasket.Qtr1Qty + (int)itemBasket.Qtr2Qty + (int)itemBasket.Qtr3Qty + (int)itemBasket.Qtr4Qty);

                if (((Basket)Session["Basket"]).BasketItems.Where(d => d.ItemID == itemBasket.ItemID).Count() == 1)
                {
                    ((Basket)Session["Basket"]).BasketItems.Where(d => d.ItemID == itemBasket.ItemID).First().Qtr1Qty = itemBasket.Qtr1Qty;
                    ((Basket)Session["Basket"]).BasketItems.Where(d => d.ItemID == itemBasket.ItemID).First().Qtr2Qty = itemBasket.Qtr2Qty;
                    ((Basket)Session["Basket"]).BasketItems.Where(d => d.ItemID == itemBasket.ItemID).First().Qtr3Qty = itemBasket.Qtr3Qty;
                    ((Basket)Session["Basket"]).BasketItems.Where(d => d.ItemID == itemBasket.ItemID).First().Qtr4Qty = itemBasket.Qtr4Qty;
                    ((Basket)Session["Basket"]).BasketItems.Where(d => d.ItemID == itemBasket.ItemID).First().TotalQty = itemBasket.TotalQty;
                }
                else
                {
                    ((Basket)Session["Basket"]).BasketItems.Add(itemBasket);
                }
                return RedirectToAction("view-basket");
            }
            return View("update-basket", itemBasket);
        }

        private void ValidateAddItem(BasketItem BasketItem)
        {
            BasketValidator basketItemValidator = new BasketValidator();
            ValidationResult validationResult = basketItemValidator.Validate(BasketItem);
            if (!validationResult.IsValid)
            {
                foreach (ValidationFailure result in validationResult.Errors)
                {
                    ModelState.AddModelError(result.PropertyName, result.ErrorMessage);
                }
            }
        }

        [HttpPost]
        [ActionName("save")]
        [ValidateAntiForgeryToken]
        public ActionResult SaveProject(Basket projectBasket)
        {
            projectProcurement.SaveProject(projectBasket, User.Identity.Name, "EUPR");
            return RedirectToAction("dashboard", "PPMP");
        }

        [ActionName("update-item")]
        [Route("ops/procurement/planning/projects/{ProjectCode}/details/{ItemCode}/update-item")]
        public ActionResult UpdateItem(string ProjectCode, string ItemCode)
        {
            if(ProjectCode == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if(ItemCode == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ProjectProcurementPlanItems item = projectProcurement.GetProjectItem(ProjectCode, ItemCode);
            if(item == null)
            {
                return HttpNotFound();
            }
            return View("UpdateItem", item);
        }

        //=======================================================================================================================

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectProcurementPlan projectProcurementPlan = db.ProjectProcurementPlan.Find(id);
            if (projectProcurementPlan == null)
            {
                return HttpNotFound();
            }
            return View(projectProcurementPlan);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,ProjectCode,ProjectName,Description,Purpose,FiscalYear,Office,ProjectStatus,ProjectMonthStart,TotalEstimatedBudget,PurgeFlag,CreatedAt,UpdatedAt,DeletedAt")] ProjectProcurementPlan projectProcurementPlan)
        {
            if (ModelState.IsValid)
            {
                db.Entry(projectProcurementPlan).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(projectProcurementPlan);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectProcurementPlan projectProcurementPlan = db.ProjectProcurementPlan.Find(id);
            if (projectProcurementPlan == null)
            {
                return HttpNotFound();
            }
            return View(projectProcurementPlan);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ProjectProcurementPlan projectProcurementPlan = db.ProjectProcurementPlan.Find(id);
            db.ProjectProcurementPlan.Remove(projectProcurementPlan);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
                projectProcurement.Dispose();
                catalogueBL.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
