//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Data.Entity;
//using System.Linq;
//using System.Net;
//using System.Web;
//using System.Web.Mvc;
//using PUPFMIS.Models;
//using PUPFMIS.BusinessLayer;

//namespace PUPFMIS.Areas.Budget.Controllers
//{
//    [RouteArea("budget")]
//    [RoutePrefix("projects/approval")]
//    [Route("{action}")]
//    [Authorize(Roles = SystemRoles.BudgetAdmin + ", " + SystemRoles.BudgetOfficer + ", " + SystemRoles.SuperUser)]
//    public class ProjectApprovalsController : Controller
//    {
//        private FMISDbContext db = new FMISDbContext();
//        private ProjectApprovalsBDL projectApprovals = new ProjectApprovalsBDL();

//        [ActionName("index")]
//        public ActionResult Index()
//        {
//            return View(projectApprovals.GetProjects());
//        }

//        [ActionName("view-summary")]
//        [Route("{FiscalYear}/{Office}/view-summary")]
//        public ActionResult ViewSummary()
//        {
//            return View("ViewSummary", projectApprovals.GetAccountsSummary("Procurement Management Office", "2021"));
//        }

//        // GET: Budget/ProjectApprovals/Details/5
//        public ActionResult Details(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            ProjectProcurementPlan projectProcurementPlan = db.ProjectProcurementPlan.Find(id);
//            if (projectProcurementPlan == null)
//            {
//                return HttpNotFound();
//            }
//            return View(projectProcurementPlan);
//        }

//        // GET: Budget/ProjectApprovals/Create
//        public ActionResult Create()
//        {
//            return View();
//        }

//        // POST: Budget/ProjectApprovals/Create
//        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
//        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Create([Bind(Include = "ID,ProjectCode,ProjectName,Description,FiscalYear,Office,PreparedBy,SubmittedBy,ProjectStatus,ProjectMonthStart,TotalEstimatedBudget,PurgeFlag,CreatedAt,UpdatedAt,DeletedAt")] ProjectProcurementPlan projectProcurementPlan)
//        {
//            if (ModelState.IsValid)
//            {
//                db.ProjectProcurementPlan.Add(projectProcurementPlan);
//                db.SaveChanges();
//                return RedirectToAction("Index");
//            }

//            return View(projectProcurementPlan);
//        }

//        // GET: Budget/ProjectApprovals/Edit/5
//        public ActionResult Edit(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            ProjectProcurementPlan projectProcurementPlan = db.ProjectProcurementPlan.Find(id);
//            if (projectProcurementPlan == null)
//            {
//                return HttpNotFound();
//            }
//            return View(projectProcurementPlan);
//        }

//        // POST: Budget/ProjectApprovals/Edit/5
//        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
//        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Edit([Bind(Include = "ID,ProjectCode,ProjectName,Description,FiscalYear,Office,PreparedBy,SubmittedBy,ProjectStatus,ProjectMonthStart,TotalEstimatedBudget,PurgeFlag,CreatedAt,UpdatedAt,DeletedAt")] ProjectProcurementPlan projectProcurementPlan)
//        {
//            if (ModelState.IsValid)
//            {
//                db.Entry(projectProcurementPlan).State = EntityState.Modified;
//                db.SaveChanges();
//                return RedirectToAction("Index");
//            }
//            return View(projectProcurementPlan);
//        }

//        // GET: Budget/ProjectApprovals/Delete/5
//        public ActionResult Delete(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            ProjectProcurementPlan projectProcurementPlan = db.ProjectProcurementPlan.Find(id);
//            if (projectProcurementPlan == null)
//            {
//                return HttpNotFound();
//            }
//            return View(projectProcurementPlan);
//        }

//        // POST: Budget/ProjectApprovals/Delete/5
//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public ActionResult DeleteConfirmed(int id)
//        {
//            ProjectProcurementPlan projectProcurementPlan = db.ProjectProcurementPlan.Find(id);
//            db.ProjectProcurementPlan.Remove(projectProcurementPlan);
//            db.SaveChanges();
//            return RedirectToAction("Index");
//        }

//        protected override void Dispose(bool disposing)
//        {
//            if (disposing)
//            {
//                db.Dispose();
//            }
//            base.Dispose(disposing);
//        }
//    }
//}
