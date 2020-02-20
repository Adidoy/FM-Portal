using PUPFMIS.BusinessLayer;
using PUPFMIS.Models;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace PUPFMIS.Controllers
{
    [Authorize]
    [Route("ops/procurement/planning/ppmp/approvals/{action}")]
    public class PPMPApprovalController : Controller
    {
        private FMISDbContext db = new FMISDbContext();
        private PPMPApprovalBL approvalBL = new PPMPApprovalBL();
        private AccountsManagementBL accountsBL = new AccountsManagementBL();

        [ActionName("dashboard")]
        public ActionResult Index()
        {
            ViewBag.TotalSubmitted = approvalBL.GetSubmittedPPMP().Count;
            //ViewBag.ItemCount = approvalBL.GetAcceptedItems().Count();
            return View("Index");
        }

        [ActionName("view-submissions")]
        public ActionResult ViewSubmissions()
        {
            return View("ViewSubmissions", approvalBL.GetSubmittedPPMP());
        }

        [ActionName("submission-details")]
        [Route("ops/procurement/planning/ppmp/approvals/{ReferenceNo}/submission-details")]
        public ActionResult SubmissionDetails(string ReferenceNo)
        {
            if (ReferenceNo == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PPMPCSEViewModel ppmpVM = approvalBL.GetPPMPCSEDetails(ReferenceNo);
            if (ppmpVM == null)
            {
                return HttpNotFound();
            }
            return View("SubmissionDetails", ppmpVM);
        }

        //[Route("ops/procurement/planning/ppmp/approvals/view-submitted-items")]
        //public ActionResult ViewSubmittedItems()
        //{
        //    List<PPMPSubmittedItems> submittedItems = approvalBL.GetSubmittedItems();
        //    foreach (var item in submittedItems)
        //    {
        //        var bulk = (int)item.TotalQty / (int)item.QtyPerPackage;
        //        var bulkremainder = (decimal)item.TotalQty % item.QtyPerPackage;
        //        var threshold = item.QtyPerPackage * 0.75m;
        //        item.BulkQty = (int)bulk + ((bulkremainder >= threshold) ? 1 : 0);
        //    }
        //    return View(submittedItems);
        //}





        //[Route("ops/procurement/planning/ppmp/approvals/{ItemCode}/distribution-list")]
        //public ActionResult DistributionList(string ItemCode)
        //{
        //    if (ItemCode == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    var item = db.Items.Where(d => d.ItemCode == ItemCode).FirstOrDefault();
        //    ViewBag.Item = item;
        //    List<PPMPDistributionList> distributionList = approvalBL.GetDistributionList(ItemCode);
        //    if (distributionList == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(distributionList);
        //}

        //[Route("ops/procurement/planning/ppmp/approvals/{ReferenceNo}/accept-submission")]
        //public ActionResult AcceptSubmission(string ReferenceNo)
        //{
        //    var user = accountsBL.GetUsers(User.Identity.Name, false);
        //    var userID = user.ID;
        //    var officeID = user.Office;
        //    approvalBL.AcceptSubmission(ReferenceNo, userID, officeID);
        //    return RedirectToAction("index");
        //}

        //public ActionResult RejectSubmission(string ReferenceNo)
        //{
        //    PPMPApprovalWorkflowViewModel approvalVM = new PPMPApprovalWorkflowViewModel();
        //    approvalVM.ReferenceNo = ReferenceNo;
        //    ViewBag.ReferenceNo = ReferenceNo;
        //    return View(approvalVM);
        //}

        //public ActionResult Create()
        //{
        //    return View();
        //}

        //public ActionResult AddToPPMP(string ItemCode)
        //{
        //    if (Session["PPMPCSE"] == null)
        //    {
        //        Session["PPMPCSE"] = new List<PPMPSubmittedItems>();
        //        var ppmpCSEItems = approvalBL.GetAcceptedItems(ItemCode);
        //        Session["PPPMPCSE"] = ppmpCSEItems;
        //    }
        //    return RedirectToAction("ViewSubmittedItems");
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "ID,ReferenceNo,FiscalYear,CreatedAt,SubmittedAt,ApprovedAt,PreparedBy,SubmittedBy,Status,OfficeReference")] PPMPHeader pPMPHeader)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.PPMPHeader.Add(pPMPHeader);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    return View(pPMPHeader);
        //}

        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    PPMPHeader pPMPHeader = db.PPMPHeader.Find(id);
        //    if (pPMPHeader == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(pPMPHeader);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "ID,ReferenceNo,FiscalYear,CreatedAt,SubmittedAt,ApprovedAt,PreparedBy,SubmittedBy,Status,OfficeReference")] PPMPHeader pPMPHeader)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(pPMPHeader).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return View(pPMPHeader);
        //}

        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    PPMPHeader pPMPHeader = db.PPMPHeader.Find(id);
        //    if (pPMPHeader == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(pPMPHeader);
        //}

        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    PPMPHeader pPMPHeader = db.PPMPHeader.Find(id);
        //    db.PPMPHeader.Remove(pPMPHeader);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                approvalBL.Dispose();
                accountsBL.Dispose();
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
