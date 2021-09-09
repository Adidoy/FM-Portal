//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;
//using PUPFMIS.Models;
//using PUPFMIS.BusinessAndDataLogic;

//namespace PUPFMIS.Controllers
//{
//    [Route("{action}")]
//    [RouteArea("procurement")]
//    [RoutePrefix("quotations/requests")]
//    [Authorize(Roles = SystemRoles.ProcurementPlanningChief + ", " + SystemRoles.PropertyDirector + ", " + SystemRoles.ProjectCoordinator + ", " + SystemRoles.ProcurementStaff)]
//    public class RequestForQuotationController : Controller
//    {
//        private RequestForQuotationDAL rfqDAL = new RequestForQuotationDAL();

//        [Route("")]
//        [ActionName("dashboard")]
//        public ActionResult Index()
//        {
//            var rfqDashboard = new RFQDashboardVM();
//            rfqDashboard.AlternativeModeProjects = rfqDAL.GetAlternativeData();
//            rfqDashboard.FiscalYears = rfqDAL.GetFiscalYears();
//            rfqDashboard.AbstractFiscalYears = rfqDAL.GetAbstractFiscalYears();
//            rfqDashboard.PreparedAbstractFiscalYears = rfqDAL.GetPreparedAbstractFiscalYears();
//            return View("Dashboard", rfqDashboard);
//        }

//        [Route("{FiscalYear}")]
//        [ActionName("index")]
//        public ActionResult Index(int FiscalYear)
//        {
//            return View(rfqDAL.GetQuotations(FiscalYear).ToList());
//        }

//        [Route("{FiscalYear}/closed-submissions")]
//        [ActionName("closed-submissions")]
//        public ActionResult IndexAbstract(int FiscalYear)
//        {
//            return View("IndexAbstract", rfqDAL.GetQuotations(FiscalYear).Where(d => d.IsSubmissionOpen == false && d.AbstractPreparedAt == null).ToList());
//        }

//        [Route("{FiscalYear}/view-abstracts")]
//        [ActionName("view-abstracts")]
//        public ActionResult IndexPreparedAbstract(int FiscalYear)
//        {
//            return View("IndexPreparedAbstract", rfqDAL.GetQuotations(FiscalYear).Where(d => d.IsSubmissionOpen == false && d.AbstractPreparedAt != null).ToList());
//        }

//        [Route("{ContractCode}/create")]
//        [ActionName("create")]
//        public ActionResult Create(string ContractCode)
//        {
//            return View(rfqDAL.RequestForQuotationSetup(ContractCode));
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        [Route("{ContractCode}/create")]
//        [ActionName("create")]
//        public ActionResult Create(RequestForQuotationVM RequestForQuotationVM)
//        {
//            if (ModelState.IsValid)
//            {
//                return Json(new { result = rfqDAL.PostRequestForQuotation(RequestForQuotationVM, User.Identity.Name) });
//            }
//            return View(RequestForQuotationVM);
//        }

//        [Route("{SolicitationNo}/details")]
//        [ActionName("details")]
//        public ActionResult Detail(string SolicitationNo)
//        {
//            var rfq = rfqDAL.RequestForQuotationDetails(SolicitationNo);
//            return View(rfq);
//        }

//        [Route("{SolicitationNo}/details/new-quotation")]
//        [ActionName("new-quotation")]
//        public ActionResult NewQuotation(string SolicitationNo)
//        {
//            var suppliers = rfqDAL.GetSuppliers(SolicitationNo);
//            var newQuotation = new QuotationVM();
//            newQuotation.Address = suppliers[0].Address;
//            newQuotation.ContactPerson = suppliers[0].ContactPerson;
//            newQuotation.ContactNumber = suppliers[0].ContactNumber;
//            newQuotation.TaxIdNumber = suppliers[0].TaxIdNumber == null || suppliers[0].TaxIdNumber == string.Empty ? "No data supplied" : suppliers[0].TaxIdNumber;
//            newQuotation.QuotationDetails = rfqDAL.GetRFQItem(SolicitationNo);

//            ViewBag.SolicitationNo = SolicitationNo;
//            ViewBag.SupplierReference = new SelectList(suppliers, "ID", "SupplierName");
//            return View("NewQuotation", newQuotation);
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        [Route("{SolicitationNo}/details/new-quotation")]
//        [ActionName("new-quotation")]
//        public ActionResult NewQuotation(QuotationVM QuoteVM, string SolicitationNo)
//        {
//            var details = QuoteVM.QuotationDetails;
//            for (int i = 0; i < details.Count; i++)
//            {
//                if (details[i].NoOffer == false && details[i].UnitPrice == null)
//                {
//                    ModelState.AddModelError("QuotationDetails[" + i.ToString() + "].UnitPrice", "Unit Price must be filled out");
//                }
//            }

//            if (ModelState.IsValid)
//            {
//                return Json(new { result = rfqDAL.PostSupplierQuotation(QuoteVM, SolicitationNo, User.Identity.Name) });
//            }

//            var suppliers = rfqDAL.GetSuppliers(SolicitationNo);
//            ViewBag.SolicitationNo = SolicitationNo;
//            ViewBag.SupplierReference = new SelectList(suppliers, "ID", "SupplierName", QuoteVM.SupplierReference);
//            return PartialView("_NewQuotation", QuoteVM);
//        }

//        [Route("{SolicitationNo}/details/{QuotationNo}")]
//        [ActionName("quotation-details")]
//        public ActionResult QuotationDetails(string SolicitationNo, string QuotationNo)
//        {
//            var quotationDetails = rfqDAL.ViewQuotation(QuotationNo);
//            var suppliers = rfqDAL.GetSuppliers(SolicitationNo, quotationDetails.SupplierReference);
//            ViewBag.SolicitationNo = SolicitationNo;
//            ViewBag.IsEdit = true;
//            ViewBag.SupplierReference = new SelectList(suppliers, "ID", "SupplierName", quotationDetails.SupplierReference);
//            return View("NewQuotation", quotationDetails);
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        [Route("{SolicitationNo}/details/{QuotationNo}")]
//        [ActionName("update-quotation")]
//        public ActionResult QuotationDetails(QuotationVM QuoteVM, string SolicitationNo)
//        {
//            var details = QuoteVM.QuotationDetails;
//            for (int i = 0; i < details.Count; i++)
//            {
//                if (details[i].NoOffer == false && details[i].UnitPrice == null)
//                {
//                    ModelState.AddModelError("QuotationDetails[" + i.ToString() + "].UnitPrice", "Unit Price must be filled out");
//                }
//            }

//            if (ModelState.IsValid)
//            {
//                return Json(new { result = rfqDAL.UpdateSupplierQuotation(QuoteVM, SolicitationNo, User.Identity.Name) });
//            }

//            var suppliers = rfqDAL.GetSuppliers(SolicitationNo);
//            ViewBag.SolicitationNo = SolicitationNo;
//            ViewBag.SupplierReference = new SelectList(suppliers, "ID", "SupplierName", QuoteVM.SupplierReference);
//            return PartialView("_NewQuotation", QuoteVM);
//        }

//        [ActionName("print")]
//        [Route("{ContractCode}/print")]
//        public ActionResult PrintRFQ(string ContractCode)
//        {
//            var rfqBL = new RequestForQuotationBL();
//            var stream = rfqBL.PrintRFQ(Server.MapPath("~/Content/imgs/PUPLogo.png"), ContractCode);
//            Response.Clear();
//            Response.ClearContent();
//            Response.ClearHeaders();
//            Response.AddHeader("content-length", stream.Length.ToString());
//            Response.ContentType = "application/pdf";
//            Response.BinaryWrite(stream.ToArray());
//            stream.Close();
//            Response.End();

//            return RedirectToAction("list", new { Area = "end-users" });
//        }

//        [ActionName("print-abstract")]
//        [Route("{SolicitationNo}/view/abstract/print")]
//        public ActionResult PrintAbstract(string SolicitationNo)
//        {
//            var rfqBL = new RequestForQuotationBL();
//            var stream = rfqBL.PrintAbstract(Server.MapPath("~/Content/imgs/PUPLogo.png"), SolicitationNo);
//            Response.Clear();
//            Response.ClearContent();
//            Response.ClearHeaders();
//            Response.AddHeader("content-length", stream.Length.ToString());
//            Response.ContentType = "application/pdf";
//            Response.BinaryWrite(stream.ToArray());
//            stream.Close();
//            Response.End();

//            return RedirectToAction("list", new { Area = "end-users" });
//        }

//        [ActionName("close-submission")]
//        [Route("{SolicitationNo}/close-submission")]
//        public ActionResult CloseSubmission(string SolicitationNo)
//        {
//            return Json(new { result = rfqDAL.CloseRFQSubmission(SolicitationNo, User.Identity.Name) });
//        }

//        [Route("{SolicitationNo}/abstract")]
//        [ActionName("abstract")]
//        public ActionResult QuotationAbstract(string SolicitationNo)
//        {
//            var quotationDetails = rfqDAL.EvaluationSetup(SolicitationNo);
//            ViewBag.FiscalYear = quotationDetails.FiscalYear;
//            return View("Evaluation", quotationDetails);
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        [Route("{SolicitationNo}/abstract")]
//        [ActionName("abstract")]
//        public ActionResult QuotationAbstract(QuoteEvaluationVM QuoteEvaluation)
//        {
//            if(ModelState.IsValid)
//            {
//                return Json(new { result = rfqDAL.PostAbstractOfQuotations(QuoteEvaluation, User.Identity.Name) });
//            }
//            return PartialView("_Evaluation", QuoteEvaluation);
//        }

//        [Route("{SolicitationNo}/view/abstract")]
//        [ActionName("view-abstract")]
//        public ActionResult ViewAbstract(string SolicitationNo)
//        {
//            var quotationDetails = rfqDAL.GetAbstractOfQuotation(SolicitationNo);
//            ViewBag.SolicitationNo = SolicitationNo;
//            return View("AbstractOfQuotations", quotationDetails);
//        }

//        protected override void Dispose(bool disposing)
//        {
//            if (disposing)
//            {
//                rfqDAL.Dispose();
//            }
//            base.Dispose(disposing);
//        }
//    }
//}