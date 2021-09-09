using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PUPFMIS.Controllers.Errors
{
    [RoutePrefix("errors")]
    public class ErrorsController : Controller
    {
        // GET: Error
        public ActionResult Index()
        {
            return View();
        }

        [Route("default")]
        [ActionName("default")]
        public ActionResult Default()
        {
            return View("Default");
        }

        [Route("unauthorized-access")]
        [ActionName("unauthorized-access")]
        public ActionResult UnauthorizedAccess()
        {
            return View("UnauthorizedAccess");
        }

        [Route("page-not-found")]
        [ActionName("page-not-found")]
        public ActionResult NotFound()
        {
            return View("NotFound");
        }

        [Route("record-not-found")]
        [ActionName("record-not-found")]
        public ActionResult RecordNotFound()
        {
            return View("RecordNotFound");
        }
    }
}