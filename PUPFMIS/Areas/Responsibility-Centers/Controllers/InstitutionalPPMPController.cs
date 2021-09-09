using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PUPFMIS.Models;
using PUPFMIS.BusinessAndDataLogic;

namespace PUPFMIS.Controllers
{
    [RoutePrefix("procurement-plans")]
    [RouteArea("responsibility-centers")]
    [Authorize(Roles = SystemRoles.ResponsibilityCenterPlanner)]
    public class InstitutionalPPMPController : Controller
    {
        private InstitutionalPPMPBDL ppmpDataAccess = new InstitutionalPPMPBDL();

        [Route("")]
        [ActionName("list")]
        public ActionResult Index()
        {
            var fiscalYears = ppmpDataAccess.GetFiscalYears();
            return View("Index", fiscalYears);
        }
    }
}