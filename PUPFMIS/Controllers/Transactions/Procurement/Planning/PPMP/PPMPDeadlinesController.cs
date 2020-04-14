using PUPFMIS.BusinessAndDataLogic;
using PUPFMIS.Models;
using System.Net;
using System.Web.Mvc;
using FluentValidation.Results;

namespace PUPFMIS.Controllers
{
    [Route("ops/procurement/ppmp/deadlines/{action}")]
    [Authorize]
    public class PPMPDeadlinesController : Controller
    {
        private PPMPDeadlinesBL ppmpDeadlinesBL = new PPMPDeadlinesBL();

        public ActionResult Index()
        {
            var deadlineList = ppmpDeadlinesBL.GetOpenDeadlines();
            return View(deadlineList);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PPMPDeadlines PPMPDeadlines = ppmpDeadlinesBL.GetDeadlineDetails(id);
            if (PPMPDeadlines == null)
            {
                return HttpNotFound();
            }
            return PartialView(PPMPDeadlines);
        }

        public ActionResult Create()
        {
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PPMPDeadlines PPMPDeadlines)
        {
            ModelState.Remove("Status");
            ValidatePPMPDeadline(PPMPDeadlines);
            if (ModelState.IsValid)
            {
                if(ppmpDeadlinesBL.SetNewDeadline(PPMPDeadlines) == true)
                {
                    return Json(new { status = "success" });
                }
                return View(PPMPDeadlines);
            }
            return PartialView(PPMPDeadlines);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PPMPDeadlines PPMPDeadlines = ppmpDeadlinesBL.GetDeadlineDetails(id);
            if (PPMPDeadlines == null)
            {
                return HttpNotFound();
            }
            return PartialView(PPMPDeadlines);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PPMPDeadlines PPMPDeadlines)
        {
            ModelState.Remove("Status");
            if (ModelState.IsValid)
            {
                if (ppmpDeadlinesBL.UpdateDeadline(PPMPDeadlines, false) == true)
                {
                    return Json(new { status = "success" });
                }
                return View(PPMPDeadlines);
            }
            return PartialView(PPMPDeadlines);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PPMPDeadlines PPMPDeadline = ppmpDeadlinesBL.GetDeadlineDetails(id);
            if (PPMPDeadline == null)
            {
                return HttpNotFound();
            }
            return PartialView(PPMPDeadline);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PPMPDeadlines PPMPDeadline = ppmpDeadlinesBL.GetDeadlineDetails(id);
            if(ppmpDeadlinesBL.UpdateDeadline(PPMPDeadline, true))
            {
                return Json(new { status = "success" });
            }
            return RedirectToAction("Index");
        }

        public ActionResult Stop(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PPMPDeadlines PPMPDeadline = ppmpDeadlinesBL.GetDeadlineDetails(id);
            if (PPMPDeadline == null)
            {
                return HttpNotFound();
            }
            return PartialView(PPMPDeadline);
        }

        [HttpPost, ActionName("Stop")]
        [ValidateAntiForgeryToken]
        public ActionResult StopConfirmed(int id)
        {
            PPMPDeadlines PPMPDeadline = ppmpDeadlinesBL.GetDeadlineDetails(id);
            if (ppmpDeadlinesBL.StopDeadline(PPMPDeadline))
            {
                return Json(new { status = "success" });
            }
            return RedirectToAction("Index");
        }

        private void ValidatePPMPDeadline(PPMPDeadlines PPMPDeadline)
        {
            PPMPDeadlinesValidation validator = new PPMPDeadlinesValidation();
            ValidationResult validationResult = validator.Validate(PPMPDeadline);
            if (!validationResult.IsValid)
            {
                foreach (ValidationFailure result in validationResult.Errors)
                {
                    ModelState.AddModelError(result.PropertyName, result.ErrorMessage);
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                ppmpDeadlinesBL.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
