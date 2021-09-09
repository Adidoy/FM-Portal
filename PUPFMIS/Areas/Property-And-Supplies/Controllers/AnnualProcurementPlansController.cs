using PUPFMIS.BusinessAndDataLogic;
using PUPFMIS.Models;
using System.Web.Mvc;
using System.Linq;

namespace PUPFMIS.Areas.Property.Controllers
{
    [Route("{action}")]
    [RouteArea("property-and-supplies")]
    [RoutePrefix("annual-procurement-plan/common-supplies")]
    [Authorize(Roles = SystemRoles.SuperUser + ", " + SystemRoles.SystemAdmin + ", " + SystemRoles.PropertyDirector + ", " + SystemRoles.SuppliesChief + ", " + SystemRoles.ProcurementAdministrator + ", " + SystemRoles.ProcurementPlanningChief)]
    public class APP_CSEController : Controller
    {
        private AnnualProcurementPlanCSEDAL appcseDAL = new AnnualProcurementPlanCSEDAL();
        private AnnualProcurementPlanCSEBL appcseBL = new AnnualProcurementPlanCSEBL();

        [ActionName("create-cse")]
        [Route("{FiscalYear}/create")]
        public ActionResult CreateCSE(int FiscalYear)
        {
            FMISDbContext db = new FMISDbContext();
            AnnualProcurementPlanCSEVM appcse = appcseDAL.GetAPPCSE(FiscalYear);
            ViewBag.UserRole = db.UserAccounts.Where(d => d.Email == User.Identity.Name).FirstOrDefault().FKRoleReference.Role;
            return View("CreateCSE", appcse);
        }

        [Route("select-year")]
        [ActionName("select-year")]
        public ActionResult SelectYear()
        {
            ViewBag.FiscalYear = new SelectList(appcseDAL.GetPPMPFiscalYears());
            return View("CreateSelectYear");
        }

        [HttpPost]
        [Route("select-year")]
        [ActionName("select-year")]
        public ActionResult SelectYear(int FiscalYear)
        {
            ViewBag.FiscalYear = new SelectList(appcseDAL.GetPPMPFiscalYears());
            return RedirectToAction("create-cse", new { FiscalYear = FiscalYear });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("post-app-cse")]
        [Route("{FiscalYear}/create")]
        public ActionResult CreateCSE(AnnualProcurementPlanCSEVM APPCSEViewModel)
        {
            return Json(new { result = appcseDAL.PostAPPCSE(APPCSEViewModel, User.Identity.Name) });
        }

        [ActionName("print-app-cse")]
        [Route("{ReferenceNo}/print")]
        public ActionResult PrintAPPCSE(string ReferenceNo)
        {
            var stream = appcseBL.GenerateAPPCSE(ReferenceNo, Server.MapPath("~/Content/imgs/PUPLogo.png"), User.Identity.Name);
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
                appcseDAL.Dispose();
                //appcseBL.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
