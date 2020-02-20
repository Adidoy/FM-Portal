using FluentValidation.Results;
using PUPFMIS.BusinessLayer;
using PUPFMIS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace PUPFMIS.Controllers
{
    [Authorize]
    [Route("ops/procurement/planning/ppmp/{action}")]
    public class PPMPController : Controller
    {
        private PPMPBL ppmpBL = new PPMPBL();
        private ItemsBL itemsBL = new ItemsBL();
        private ItemCategoriesBL itemCategories = new ItemCategoriesBL();
        private PPMPDeadlinesBL fiscalYearsBL = new PPMPDeadlinesBL();
        private PPMPApprovalBL approvalBL = new PPMPApprovalBL();

        public ActionResult Index()
        {
            return View(ppmpBL.GetMyPPMP());
        }

        [HttpPost]
        [ActionName("create")]
        public ActionResult Create(List<Basket> BasketItems)
        {
            if( (BasketItems != null) || (BasketItems.Count >= 1) )
            {
                PPMPViewModel ppmpCSEViewModel = new PPMPViewModel();
                ppmpCSEViewModel.PPMPHeader.ReferenceNo = "*** New PPMP ***";
                ppmpCSEViewModel.DBMItems = ((List<Basket>)Session["BasketItems"]).Where(d => d.ProcurementSource == ProcurementSources.PS_DBM).ToList();
                ppmpCSEViewModel.NonDBMItems = ((List<Basket>)Session["BasketItems"]).Where(d => d.ProcurementSource == ProcurementSources.Non_DBM).ToList();
                ViewBag.FiscalYear = fiscalYearsBL.GetOpenDeadlines().OrderBy(d => d.FiscalYear).ToList();
                return View("Create", ppmpCSEViewModel);
            }
            return View();
        }

        [HttpPost]
        [ActionName("save")]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PPMPViewModel ppmpCSE)
        {
            if (ppmpBL.CreatePPMPCSE(ppmpCSE, User.Identity.Name) == true)
            {
                Session["BasketItems"] = null;
                return RedirectToAction("index");
            }
            ViewBag.BasketItems = ((List<PPMPItemVM>)Session["BasketItems"]);
            ViewBag.FiscalYear = new SelectList(fiscalYearsBL.GetOpenDeadlines().OrderBy(d => d.FiscalYear).ToList(), "FiscalYear", "FiscalYear");
            return View("Create");
        }

        [Route("ops/procurement/planning/ppmp/{ReferenceNo}/details")]
        public ActionResult Details(string ReferenceNo)
        {
            if (ReferenceNo == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PPMPCSEViewModel ppmpCSE = ppmpBL.GetPPMPCSEDetails(ReferenceNo);
            if (ppmpCSE == null)
            {
                return HttpNotFound();
            }
            ViewBag.Workflow = ppmpBL.GetApprovalWorkflow(ReferenceNo);
            if (ViewBag.Workflow == null)
            {
                return HttpNotFound();
            }
            return View(ppmpCSE);
        }

        public ActionResult SubmitPPMP(string ReferenceNo)
        {
            if (ReferenceNo == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PPMPCSEViewModel ppmpCSE = ppmpBL.GetPPMPCSEDetails(ReferenceNo);
            if (ppmpBL.SubmitPPMP(ReferenceNo) == false)
            {
                return RedirectToAction("index");
            }
            return RedirectToAction("index");
        }

        [Route("ops/procurement/planning/ppmp/{ReferenceNo}/print")]
        public ActionResult PrintPPMP(string ReferenceNo)
        {
            var stream = ppmpBL.GeneratePPMPReport(ReferenceNo, Server.MapPath("~/Content/imgs/PUPLogo.png"));
            Response.Clear();
            Response.ClearContent();
            Response.ClearHeaders();
            Response.AddHeader("content-length", stream.Length.ToString());
            //Response.AddHeader("content-disposition", "attachment; filename=" + reportConfig.ReportReferenceNo + ".pdf");
            Response.ContentType = "application/pdf";
            Response.BinaryWrite(stream.ToArray());
            stream.Close();
            Response.End();

            return RedirectToAction("index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                approvalBL.Dispose();
                ppmpBL.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
