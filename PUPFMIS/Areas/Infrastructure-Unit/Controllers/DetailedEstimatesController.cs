using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PUPFMIS.Models;
using PUPFMIS.BusinessAndDataLogic;

namespace PUPFMIS.Controllers
{
    [Route("{action}")]
    [RouteArea("infrastructure-unit")]
    [RoutePrefix("infrasrtructures/project/estimates")]
    [UserAuthorization(Roles = SystemRoles.InfrastructureImplementingUnit)]
    public class DetailedEstimatesController : Controller
    {
        DetailedEstimatedBDL detailedEstimatedBDL = new DetailedEstimatedBDL();

        [Route("list")]
        [ActionName("list")]
        public ActionResult Index()
        {
            var infraProjectList = detailedEstimatedBDL.GetInfraProjectList();
            return View("Index", infraProjectList);
        }

        [Route("{ProjectCode}/{ItemCode}/details")]
        [ActionName("details")]
        public ActionResult Details(string ProjectCode, string ItemCode)
        {
            var project = detailedEstimatedBDL.GetInfraProjectDetails(ProjectCode, ItemCode);
            return View("Details", project);
        }

        [Route("{InfraProjectCode}/details")]
        [ActionName("details")]
        public ActionResult Details(string InfraProjectCode)
        {
            var project = detailedEstimatedBDL.GetInfraProjectDetails(InfraProjectCode);
            return View("Details", project);
        }

        [Route("{InfraProjectCode}/post-to-ppmp")]
        [ActionName("post-to-ppmp")]
        public ActionResult PostToPPMP(string InfraProjectCode)
        {
            var infraProjectBDL = new InfrastructureProjectsBDL();
            return Json(new { result = infraProjectBDL.UpdateProjectDetails(InfraProjectCode, User.Identity.Name) });
        }
    }
}