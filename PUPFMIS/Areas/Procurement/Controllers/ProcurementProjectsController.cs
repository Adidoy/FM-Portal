using PUPFMIS.BusinessAndDataLogic;
using PUPFMIS.Models;
using System.Web.Mvc;

namespace PUPFMIS.Areas.Procurement.Controllers
{
    [Route("{action}")]
    [RouteArea("procurement")]
    [RoutePrefix("projects")]
    public class ProcurementProjectsController : Controller
    {
        ProcurementProjectsDAL contractDAL = new ProcurementProjectsDAL();

        [Route("")]
        [ActionName("list")]
        public ActionResult ContractIndex()
        {
            var projects = new ProcurementProjectsVM();
            projects.NewProjects = contractDAL.GetContracts(User.Identity.Name);
            return View("Index", projects);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                contractDAL.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}