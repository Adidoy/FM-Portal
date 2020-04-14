using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PUPFMIS.Models;
using PUPFMIS.BusinessAndDataLogic;

namespace PUPFMIS.Areas.EndUsers.Controllers
{
    [Route("{action}")]
    [RoutePrefix("ppmp")]
    [RouteArea("end-users")]
    [Authorize(Roles = SystemRoles.SuperUser + ", " + SystemRoles.EndUser)]
    public class ProjectProcurementManagementPlansController : Controller
    {
        private ProjectProcurementManagementPlanBL ppmpBL = new ProjectProcurementManagementPlanBL();
        private ProjectProcurementManagementPlanDAL ppmpDAL = new ProjectProcurementManagementPlanDAL();

        [Route("")]
        [Route("list")]
        [ActionName("list")]
        public ActionResult Index()
        {
            var ppmpList = ppmpDAL.GetPPMPList(User.Identity.Name);
            return View("index", ppmpList);
        }

        [Route("{ReferenceNo}/details")]
        [ActionName("view-ppmp-details")]
        public ActionResult Details(string ReferenceNo)
        {
            if (ReferenceNo == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var ppmpVM = ppmpDAL.GetPPMPDetails(ReferenceNo, User.Identity.Name);
            if (ppmpVM == null)
            {
                return HttpNotFound();
            }
            return View("details", ppmpVM);
        }

        [ActionName("print-ppmp")]
        [Route("{ReferenceNo}/print")]
        public ActionResult PrintPPMP(string ReferenceNo)
        { 
            var stream = ppmpBL.PrintPPMP(ReferenceNo, Server.MapPath("~/Content/imgs/PUPLogo.png"), User.Identity.Name);
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

        [HttpPost]
        [Route("submit-ppmp")]
        [ValidateAntiForgeryToken]
        [ActionName("submit-ppmp")]
        public ActionResult SubmitPPMP(List<PPMPHeaderViewModel> PPMP)
        {
            foreach(var item in PPMP)
            {
                if(item.IsSelected == true)
                {
                    ppmpDAL.SubmitPPMP(item.ReferenceNo, User.Identity.Name);
                }
            }
            return RedirectToAction("list", "ProjectProcurementManagementPlans", new { Area = "end-users" });
        }
        
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                ppmpDAL.Dispose();
                ppmpBL.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
