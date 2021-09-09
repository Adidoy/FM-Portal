using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PUPFMIS.BusinessAndDataLogic;

namespace PUPFMIS.Controllers
{
    [Authorize]
    [RoutePrefix("suppliers/directory")]
    public class SuppliersDirectoryController : Controller
    {
        private SuppliersDirectoryDAL suppliers = new SuppliersDirectoryDAL();

        [Route("")]
        [ActionName("list")]
        public ActionResult Index()
        {
            return View("Index", suppliers.GetSuppliers());
        }

        [Route("{id}/details")]
        [ActionName("details")]
        public ActionResult Details(int? id)
        {
            if(id == null)
            {
                return RedirectToAction("page-not-found", "Errors", new { Area = "" });
            }
            var supplier = suppliers.GetSupplierDetails(id);
            if(supplier == null)
            {
                return RedirectToAction("record-not-found", "Errors", new { Area = "" });
            }
            return View("Details", supplier);
        }

        [Route("print")]
        [ActionName("print")]
        public ActionResult Print()
        {
            var stream = suppliers.PrintDirectory(Server.MapPath("~/Content/imgs/PUPLogo.png"));
            Response.Clear();
            Response.ClearContent();
            Response.ClearHeaders();
            Response.AddHeader("content-length", stream.Length.ToString());
            //Response.AddHeader("content-disposition", "attachment; filename=" + ReferenceNo + ".pdf");
            Response.ContentType = "application/pdf";
            Response.BinaryWrite(stream.ToArray());
            stream.Close();
            Response.End();

            return RedirectToAction("list", new { Area = "" });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                suppliers.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}