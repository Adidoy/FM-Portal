using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PUPFMIS.Models;
using PUPFMIS.BusinessAndDataLogic;

namespace PUPFMIS.Areas.End_Users.Controllers
{
    [Route("{action}")]
    [RouteArea("end-users")]
    [RoutePrefix("procurement-pipeline")]
    [Authorize(Roles = SystemRoles.SuperUser + ", " + SystemRoles.EndUser)]
    public class ProcurementPipelineController : Controller
    {
        private EndUserProcurementPipelineDAL pipelineDAL = new EndUserProcurementPipelineDAL();

        [Route("")]
        [ActionName("dashboard")]
        public ActionResult Dashboard()
        {
            return View(pipelineDAL.GetProcurementPipelineDashboard(User.Identity.Name));
        }
    }
}