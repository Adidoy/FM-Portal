//using FluentValidation.Results;
//using PUPFMIS.BusinessAndDataLogic;
//using PUPFMIS.Models;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net;
//using System.Web.Mvc;

//namespace PUPFMIS.Controllers
//{
//    [Authorize]
//    [Route("ops/procurement/planning/cse/{action}")]
//    public class PPMPCSEController : Controller
//    {
//        private PPMPCSEBusinessLayer ppmpCSEBusinessLayer = new PPMPCSEBusinessLayer();
//        private CatalogueBL catalogueBL = new CatalogueBL();
//        private ProcurementProjectsBL procurementProjects = new ProcurementProjectsBL();

//        [ActionName("create")]
//        public ActionResult Create()
//        {
//            ProjectProcurementPlan header = new ProjectProcurementPlan();

//            header.ProjectCode = "CSPR-XXXX-0000-0000";
//            header.ProjectName = "Supply and delivery of Common Use Office Supplies";
//            header.Description = "Supply and delivery of common use office supplies to be used for daily transactions of the Office";
//            header.ProjectMonthStart = 1;

//            ViewBag.StartMonth = "January";
//            ViewBag.FiscalYear = new SelectList(ppmpCSEBusinessLayer.GetFiscalYears(), "FiscalYear", "FiscalYear");
//            return View("Create", header);
//        }

//        [HttpPost]
//        [ActionName("create")]
//        [ValidateAntiForgeryToken]
//        public ActionResult Create(ProjectProcurementPlan header)
//        {
//            if (ModelState.IsValid)
//            {
//                Session["Basket"] = new Basket();
//                ((Basket)Session["Basket"]).BasketHeader = header;
//                ((Basket)Session["Basket"]).BasketItems = new List<BasketItem>();
//                ViewBag.CategoryName = "All Categories";
//                ViewBag.Categories = catalogueBL.GetCategories();
//                return RedirectToAction("catalogue");
//            }

//            header.ProjectCode = "CSPR-XXXX-0000-0000";
//            header.ProjectName = "Supply and delivery of Common Use Office Supplies";
//            header.Description = "Supply and delivery of common use office supplies to be used for daily transactions of the Office";
//            header.ProjectMonthStart = 1;

//            ViewBag.StartMonth = "January";
//            ViewBag.FiscalYear = new SelectList(ppmpCSEBusinessLayer.GetFiscalYears(), "FiscalYear", "FiscalYear");
//            return View("Create", header);
//        }

//        [ActionName("catalogue")]
//        public ActionResult Catalogue()
//        {
//            ViewBag.CategoryName = "All Categories";
//            ViewBag.Categories = catalogueBL.GetCategories();
//            return View("Catalogue", catalogueBL.GetCatalogue(1));
//        }

//        [ActionName("view-by-category")]
//        [Route("ops/procurement/planning/cse/catalogue/{CategoryName}/view-by-category")]
//        public ActionResult ViewCSEByCategory(string CategoryName)
//        {
//            if(Session["Basket"] == null)
//            {
//                return Redirect("create");
//            }

//            if (CategoryName == "All Categories")
//            {
//                return RedirectToAction("catalogue");
//            }

//            if (CategoryName == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }

//            List<Catalogue> catalogueByCategory = catalogueBL.GetCatalogue(CategoryName);
//            if (catalogueByCategory == null)
//            {
//                return HttpNotFound();
//            }

//            ViewBag.CategoryName = CategoryName;
//            ViewBag.Categories = catalogueBL.GetCategories();
//            return View("Catalogue", catalogueByCategory);
//        }

//        [ActionName("add-to-basket")]
//        [Route("ops/procurement/planning/cse/basket/{ItemCode}/add-to-basket")]
//        public ActionResult AddToBasket(string ItemCode)
//        {
//            if (ItemCode == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            BasketItem item = new BasketItem();
//            if (Session["Basket"] != null)
//            {
//                var basketItems = ((Basket)Session["Basket"]).BasketItems;
//                if (basketItems.Where(d => d.ItemCode == ItemCode).Count() == 1)
//                {
//                    item = basketItems.Where(d => d.ItemCode == ItemCode).FirstOrDefault();
//                }
//            }
//            item = catalogueBL.GetItems(ItemCode, User.Identity.Name);
//            if (item == null)
//            {
//                return HttpNotFound();
//            }
//            return View("AddToBasket", item);
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        [ActionName("add-to-basket")]
//        [Route("ops/procurement/planning/cse/basket/{ItemCode}/add-to-basket")]
//        public ActionResult AddToBasket(BasketItem itemBasket)
//        {
//            ValidateAddItem(itemBasket);
//            if (ModelState.IsValid)
//            {
//                itemBasket.Qtr1Qty = (String.IsNullOrEmpty(itemBasket.Qtr1Qty.ToString())) ? 0 : (int)itemBasket.Qtr1Qty;
//                itemBasket.Qtr2Qty = (String.IsNullOrEmpty(itemBasket.Qtr2Qty.ToString())) ? 0 : (int)itemBasket.Qtr2Qty;
//                itemBasket.Qtr3Qty = (String.IsNullOrEmpty(itemBasket.Qtr3Qty.ToString())) ? 0 : (int)itemBasket.Qtr3Qty;
//                itemBasket.Qtr4Qty = (String.IsNullOrEmpty(itemBasket.Qtr4Qty.ToString())) ? 0 : (int)itemBasket.Qtr4Qty;
//                itemBasket.TotalQty = ((int)itemBasket.Qtr1Qty + (int)itemBasket.Qtr2Qty + (int)itemBasket.Qtr3Qty + (int)itemBasket.Qtr4Qty);

//                if (catalogueBL.ValidateConsumptionVSQuantity(itemBasket))
//                {
//                    if (String.IsNullOrEmpty(itemBasket.Remarks))
//                    {
//                        ModelState.AddModelError(string.Empty, "Total Quantity is greater than the Previous consumption. Please reduce the quantity requirement or provide a justification below.");
//                        ViewBag.NeedsJustification = true;
//                        return View("AddToBasket", itemBasket);
//                    }
//                }
//                var basketItems = ((Basket)Session["Basket"]).BasketItems;
//                if (basketItems.Where(d => d.ItemCode == itemBasket.ItemCode).Count() == 1)
//                {
//                    basketItems.Where(d => d.ItemCode == itemBasket.ItemCode).FirstOrDefault().Qtr1Qty = itemBasket.Qtr1Qty;
//                    basketItems.Where(d => d.ItemCode == itemBasket.ItemCode).FirstOrDefault().Qtr2Qty = itemBasket.Qtr2Qty;
//                    basketItems.Where(d => d.ItemCode == itemBasket.ItemCode).FirstOrDefault().Qtr3Qty = itemBasket.Qtr3Qty;
//                    basketItems.Where(d => d.ItemCode == itemBasket.ItemCode).FirstOrDefault().Qtr4Qty = itemBasket.Qtr4Qty;
//                    basketItems.Where(d => d.ItemCode == itemBasket.ItemCode).FirstOrDefault().TotalQty = itemBasket.TotalQty;
//                    basketItems.Where(d => d.ItemCode == itemBasket.ItemCode).FirstOrDefault().Remarks = (String.IsNullOrEmpty(itemBasket.Remarks)) ? "Acceptable" : itemBasket.Remarks;
//                }
//                else
//                {
//                    itemBasket.Remarks = (String.IsNullOrEmpty(itemBasket.Remarks)) ? "Acceptable" : itemBasket.Remarks;
//                    basketItems.Add(itemBasket);
//                    ((Basket)Session["Basket"]).BasketItems = basketItems;
//                }
//                return RedirectToAction("catalogue");
//            }
//            return View("AddToBasket", itemBasket);
//        }

//        [ActionName("view-basket")]
//        [Route("ops/procurement/planning/cse/basket")]
//        public ActionResult ViewBasket()
//        {
//            return View("ViewBasket", ((Basket)Session["Basket"]));
//        }

//        [ActionName("update-basket-item")]
//        [Route("ops/procurement/planning/cse/basket/{ItemCode}/update-item")]
//        public ActionResult UpdateBasket(string ItemCode)
//        {
//            if (ItemCode == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            BasketItem updateItem = ((Basket)Session["Basket"]).BasketItems.Where(d => d.ItemCode == ItemCode).FirstOrDefault();
//            if (updateItem == null)
//            {
//                return HttpNotFound();
//            }
//            return View("UpdateBasket", updateItem);
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        [ActionName("update-basket")]
//        [Route("ops/procurement/planning/cse/basket/{ItemCode}/update-item")]
//        public ActionResult Update(BasketItem itemBasket)
//        {
//            ValidateAddItem(itemBasket);
//            if (ModelState.IsValid)
//            {
//                itemBasket.Qtr1Qty = (String.IsNullOrEmpty(itemBasket.Qtr1Qty.ToString())) ? 0 : (int)itemBasket.Qtr1Qty;
//                itemBasket.Qtr2Qty = (String.IsNullOrEmpty(itemBasket.Qtr2Qty.ToString())) ? 0 : (int)itemBasket.Qtr2Qty;
//                itemBasket.Qtr3Qty = (String.IsNullOrEmpty(itemBasket.Qtr3Qty.ToString())) ? 0 : (int)itemBasket.Qtr3Qty;
//                itemBasket.Qtr4Qty = (String.IsNullOrEmpty(itemBasket.Qtr4Qty.ToString())) ? 0 : (int)itemBasket.Qtr4Qty;
//                itemBasket.TotalQty = ((int)itemBasket.Qtr1Qty + (int)itemBasket.Qtr2Qty + (int)itemBasket.Qtr3Qty + (int)itemBasket.Qtr4Qty);

//                if (((Basket)Session["Basket"]).BasketItems.Where(d => d.ItemID == itemBasket.ItemID).Count() == 1)
//                {
//                    ((Basket)Session["Basket"]).BasketItems.Where(d => d.ItemID == itemBasket.ItemID).First().Qtr1Qty = itemBasket.Qtr1Qty;
//                    ((Basket)Session["Basket"]).BasketItems.Where(d => d.ItemID == itemBasket.ItemID).First().Qtr2Qty = itemBasket.Qtr2Qty;
//                    ((Basket)Session["Basket"]).BasketItems.Where(d => d.ItemID == itemBasket.ItemID).First().Qtr3Qty = itemBasket.Qtr3Qty;
//                    ((Basket)Session["Basket"]).BasketItems.Where(d => d.ItemID == itemBasket.ItemID).First().Qtr4Qty = itemBasket.Qtr4Qty;
//                    ((Basket)Session["Basket"]).BasketItems.Where(d => d.ItemID == itemBasket.ItemID).First().TotalQty = itemBasket.TotalQty;
//                }
//                else
//                {
//                    ((Basket)Session["Basket"]).BasketItems.Add(itemBasket);
//                }
//                return RedirectToAction("view-basket");
//            }
//            return View("update-basket", itemBasket);
//        }

//        [ActionName("remove-from-basket")]
//        [Route("ops/procurement/planning/cse/basket/{ItemCode}/remove-from-basket")]
//        public ActionResult RemoveFromBasket(string ItemCode)
//        {
//            ((Basket)Session["Basket"]).BasketItems.Remove(((Basket)Session["Basket"]).BasketItems.Where(d => d.ItemCode == ItemCode).FirstOrDefault());
//            return RedirectToAction("view-basket");
//        }

//        private void ValidateAddItem(BasketItem BasketItem)
//        {
//            BasketValidator basketItemValidator = new BasketValidator();
//            ValidationResult validationResult = basketItemValidator.Validate(BasketItem);
//            if (!validationResult.IsValid)
//            {
//                foreach (ValidationFailure result in validationResult.Errors)
//                {
//                    ModelState.AddModelError(result.PropertyName, result.ErrorMessage);
//                }
//            }
//        }

//        [HttpPost]
//        [ActionName("save")]
//        [ValidateAntiForgeryToken]
//        public ActionResult SaveProject(Basket projectBasket)
//        {
//            var status = procurementProjects.SaveProject(projectBasket, User.Identity.Name, "CSPR");
//            if(status == "Success")
//            {
//                return RedirectToAction("dashboard", "PPMP");
//            }
//            return RedirectToAction("dashboard", "PPMP");
//        }

//        [ActionName("details")]
//        [Route("ops/procurement/planning/ppmp/{ReferenceNo}/details")]
//        public ActionResult Details(string ReferenceNo)
//        {
//            if (ReferenceNo == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            PPMPCSEViewModel ppmpCSE = ppmpCSEBusinessLayer.GetPPMPCSEDetails(ReferenceNo);
//            if (ppmpCSE == null)
//            {
//                return HttpNotFound();
//            }
//            ViewBag.Workflow = ppmpCSEBusinessLayer.GetApprovalWorkflow(ReferenceNo);
//            if (ViewBag.Workflow == null)
//            {
//                return HttpNotFound();
//            }
//            return View(ppmpCSE);
//        }

//        public ActionResult SubmitPPMP(string ReferenceNo)
//        {
//            if (ReferenceNo == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            PPMPCSEViewModel ppmpCSE = ppmpCSEBusinessLayer.GetPPMPCSEDetails(ReferenceNo);
//            if (ppmpCSEBusinessLayer.SubmitPPMP(ReferenceNo) == false)
//            {
//                return RedirectToAction("index");
//            }
//            return RedirectToAction("index");
//        }

//        [ActionName("print-ppmp")]
//        [Route("ops/procurement/planning/ppmp/{ReferenceNo}/print")]
//        public ActionResult PrintPPMP(string ReferenceNo)
//        {
//            var stream = ppmpCSEBusinessLayer.GeneratePPMPReport(ReferenceNo, Server.MapPath("~/Content/imgs/PUPLogo.png"));
//            Response.Clear();
//            Response.ClearContent();
//            Response.ClearHeaders();
//            Response.AddHeader("content-length", stream.Length.ToString());
//            //Response.AddHeader("content-disposition", "attachment; filename=" + ReferenceNo + ".pdf");
//            Response.ContentType = "application/pdf";
//            Response.BinaryWrite(stream.ToArray());
//            stream.Close();
//            Response.End();

//            return RedirectToAction("index");
//        }

//        protected override void Dispose(bool disposing)
//        {
//            if (disposing)
//            {
//                ppmpCSEBusinessLayer.Dispose();
//                catalogueBL.Dispose();
//                //approvalBL.Dispose();
//                //ppmpBL.Dispose();
//            }
//            base.Dispose(disposing);
//        }
//    }
//}
