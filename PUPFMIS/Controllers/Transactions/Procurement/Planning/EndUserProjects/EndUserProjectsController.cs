using FluentValidation.Results;
using PUPFMIS.BusinessLayer;
using PUPFMIS.Models;
using System.Net;
using System.Web.Mvc;
using System.Collections.Generic;

namespace PUPFMIS.Controllers
{
    [Route("ops/procurement/projects/{action}")]
    public class EndUserProjectsController : Controller
    {
        EndUserProjectsBL endUserProjectBL = new EndUserProjectsBL();
        static ProjectMarketSurveyVM projectMarketSurveyVM = new ProjectMarketSurveyVM();

        [Route("ops/procurement/projects/index")]
        public ActionResult Index()
        {
            return View(endUserProjectBL.GetActiveProjects());
        }

        [Route("ops/procurement/projects/create")]
        public ActionResult Create()
        {
            return View(endUserProjectBL.AddEndUserProjectRecord());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(EndUserProject endUserProject)
        {
            Validate(endUserProject);
            if (ModelState.IsValid)
            {
                if(endUserProjectBL.AddEndUserProjectRecord(endUserProject))
                {
                    return RedirectToAction("index");
                }
            }
            return View(endUserProject);
        }

        [Route("ops/procurement/projects/{id}/edit")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EndUserProject endUserProjects = endUserProjectBL.GetProjectDetails(id);
            if (endUserProjects == null)
            {
                return HttpNotFound();
            }
            return View(endUserProjects);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EndUserProject endUserProjects)
        {
            Validate(endUserProjects);
            if (ModelState.IsValid)
            {
                if (endUserProjectBL.UpdateProjectRecord(endUserProjects, false))
                {
                    return Json(new { status = "success" });
                }
                else
                {
                    return Json(new { status = "failed" });
                }
            }
            return PartialView(endUserProjects);
        }

        [Route("ops/procurement/projects/{id}/market-survey/index")]
        public ActionResult IndexMarketSurvey(int id)
        {
            ViewData["ProjectDetails"] = endUserProjectBL.GetProjectDetails(id);
            return View();
        }

        [Route("ops/procurement/projects/{id}/market-survey/create")]
        public ActionResult CreateMarketSurvey(int id)
        {
            projectMarketSurveyVM.Project = endUserProjectBL.GetProjectDetails(id);
            projectMarketSurveyVM.MarketSurveyItemList = new List<MarketSurvey>();
            ViewBag.ProjectCode = projectMarketSurveyVM.Project.Code;
            ViewBag.ProjectName = projectMarketSurveyVM.Project.ProjectName;
            return View();
        }

        [HttpPost]
        [Route("ops/procurement/projects/add-details")]
        public ActionResult AddDetails(MarketSurvey marketSurveyItem)
        {
            foreach(MarketSurvey item in projectMarketSurveyVM.MarketSurveyItemList)
            {
                if(marketSurveyItem.ItemReference == item.ItemReference)
                {
                    
                }
            }
            marketSurveyItem.TotalQty = (marketSurveyItem.Qtr1 + marketSurveyItem.Qtr2 + marketSurveyItem.Qtr3 + marketSurveyItem.Qtr4);
            marketSurveyItem.Supplier1EstimatedBudget = marketSurveyItem.TotalQty * marketSurveyItem.Supplier1UnitCost;
            marketSurveyItem.Supplier2EstimatedBudget = marketSurveyItem.TotalQty * marketSurveyItem.Supplier2UnitCost;
            marketSurveyItem.Supplier3EstimatedBudget = marketSurveyItem.TotalQty * marketSurveyItem.Supplier3UnitCost;
            marketSurveyItem.TotalEstimatedBudget = (marketSurveyItem.Supplier1EstimatedBudget + marketSurveyItem.Supplier2EstimatedBudget + marketSurveyItem.Supplier3EstimatedBudget) / 3;
            projectMarketSurveyVM.MarketSurveyItemList.Add(marketSurveyItem);
            return PartialView("MarketSurveyItems", projectMarketSurveyVM.MarketSurveyItemList);
        }

        //[Route("ops/procurement/projects/{id}/{status}/market-survey-details")]
        //public ActionResult DetailsMarketSurveyItem(int id, string status)
        //{
        //    ViewBag.Status = status;
        //    return View();
        //}

        //[Route("ops/procurement/projects/{id}/details")]
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    EndUserProject endUserProjects = FMISdb.EndUserProjects.Find(id);
        //    if (endUserProjects == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(endUserProjects);
        //}



        //// GET: ProcurementProjects/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    EndUserProject procurementProjects = FMISdb.EndUserProjects.Find(id);
        //    if (procurementProjects == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(procurementProjects);
        //}

        //// POST: ProcurementProjects/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    EndUserProject endUserProjects = FMISdb.EndUserProjects.Find(id);
        //    FMISdb.EndUserProjects.Remove(endUserProjects);
        //    FMISdb.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        private void Validate(EndUserProject Project)
        {
            EndUserProjectValidator _validator = new EndUserProjectValidator();
            ValidationResult _validationResult = _validator.Validate(Project);
            if (!_validationResult.IsValid)
            {
                foreach (ValidationFailure _result in _validationResult.Errors)
                {
                    ModelState.AddModelError(_result.PropertyName, _result.ErrorMessage);
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                endUserProjectBL.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
