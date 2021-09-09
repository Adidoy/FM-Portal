using PUPFMIS.BusinessAndDataLogic;
using PUPFMIS.Models;
using System.Web.Mvc;

namespace PUPFMIS.Controllers
{
    [Route("{action}")]
    [RouteArea("infrastructure-unit")]
    [RoutePrefix("infrasrtructures/project")]
    [UserAuthorization(Roles = SystemRoles.InfrastructureImplementingUnit)]
    public class InfrastructureProjectsController : Controller
    {
        InfrastructureProjectsBDL projectsBDL = new InfrastructureProjectsBDL();

        [Route("offices")]
        [ActionName("office")]
        public ActionResult OfficeList()
        {
            return View("OfficeList", projectsBDL.GetDepartments(User.Identity.Name));
        }

        [Route("{DepartmentCode}/infra/items")]
        [ActionName("infra-list")]
        public ActionResult InfraList(string DepartmentCode)
        {
            return View("InfraList", projectsBDL.GetProjectItems(User.Identity.Name, DepartmentCode));
        }

        [ActionName("create-estimates")]
        [Route("{ItemCode}/{ProjectCode}/create/infra-project")]
        public ActionResult CreateEstimates(string ItemCode, string ProjectCode)
        {
            var infraProject = projectsBDL.InfraProjectSetup(ProjectCode, ItemCode);
            return View("CreateEstimates", infraProject);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("create-estimates")]
        [Route("{ItemCode}/{ProjectCode}/create/infra-project")]
        public ActionResult CreateEstimates(InfrastructureProjectVM InfrastructureProject)
        {
            if (ModelState.IsValid)
            {
                return Json(new { result = projectsBDL.SaveInfraProject(InfrastructureProject) });
            }
            return View("CreateEstimates", InfrastructureProject);
        }
    }
}