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

namespace PUPFMIS.Areas.Admin.Controllers
{
    [Route("{action}")]
    [RouteArea("admin")]
    [RoutePrefix("master-tables/inventory-types")]
    [Authorize(Roles = SystemRoles.SuperUser + ", " + SystemRoles.SystemAdmin)]
    public class InventoryTypesController : Controller
    {
        private FMISDbContext db = new FMISDbContext();
        private HRISDataAccess hrisDataAccess = new HRISDataAccess();
        private InventoryTypeDataAccess inventoryTypesDataAccess = new InventoryTypeDataAccess();

        [Route("")]
        [Route("list")]
        [ActionName("list")]
        public ActionResult Index()
        {
            return View("Index", inventoryTypesDataAccess.GetInventoryTypes());
        }

        [ActionName("edit")]
        [Route("{InventoryTypeCode}/edit")]
        public ActionResult Edit(string InventoryTypeCode)
        {
            if (InventoryTypeCode == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InventoryType inventoryType = inventoryTypesDataAccess.GetInventoryTypes(InventoryTypeCode);
            if (inventoryType == null)
            {
                return HttpNotFound();
            }
            var ResponsibilityCenters = new SelectList(hrisDataAccess.GetAllDepartments().Where(d => d.Lvl == 1).OrderBy(d => d.Department).ToList(), "DepartmentCode", "Department", inventoryType.ResponsibilityCenter).ToList();
            ResponsibilityCenters.Add(new SelectListItem() { Text = "None", Value = "None", Selected = (inventoryType.ResponsibilityCenter == null ? true : false) });
            ViewBag.ResponsibilityCenter = ResponsibilityCenters;
            return PartialView(inventoryType);
        }


        [HttpPost]
        [ActionName("edit")]
        [ValidateAntiForgeryToken]
        [Route("{InventoryTypeCode}/edit")]
        public ActionResult Edit(InventoryType inventoryType)
        {
            if (ModelState.IsValid)
            {
                return Json(new { result = inventoryTypesDataAccess.UpdateResponsibilityCenter(inventoryType) });
            }
            return PartialView("_Form", inventoryType);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
                hrisDataAccess.Dispose();
                inventoryTypesDataAccess.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
