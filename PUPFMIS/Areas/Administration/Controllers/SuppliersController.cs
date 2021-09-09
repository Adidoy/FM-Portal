using FluentValidation.Results;
using PUPFMIS.BusinessAndDataLogic;
using PUPFMIS.Models;
using System.Net;
using System.Web.Mvc;
using System.Linq;

namespace PUPFMIS.Controllers
{
    [Route("{action}")]
    [RouteArea("administration")]
    [RoutePrefix("suppliers")]
    [UserAuthorization(Roles = SystemRoles.SuperUser + ", " + SystemRoles.SystemAdmin)]
    public class SuppliersController : Controller
    {
        private SuppliersBL suppliersBL = new SuppliersBL();

        [Route("")]
        [ActionName("list")]
        public ActionResult Index()
        {
            return View("Index", suppliersBL.GetActiveSuppliers());
        }

        public ActionResult ViewSuppliers(int seqNo)
        {
            ViewBag.SeqNo = seqNo;
            return PartialView(suppliersBL.GetActiveSuppliers());
        }

        [Route("{id}/details")]
        [ActionName("details")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var supplier = suppliersBL.GetSupplierDetails(id);
            if (supplier == null)
            {
                return HttpNotFound();
            }
            return PartialView(supplier);
        }

        [Route("create")]
        [ActionName("create")]
        public ActionResult Create()
        {
            var supplier = new SupplierVM();
            supplier.CategoryList = suppliersBL.GetCategories();
            supplier.ItemTypesList = suppliersBL.GetItemTypes();
            return View(supplier);
        }

        [HttpPost]
        [Route("create")]
        [ActionName("create")]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SupplierVM supplier)
        {
            ValidateSupplier(supplier);
            if (ModelState.IsValid)
            {
                return Json(new { result = suppliersBL.AddSupplierRecord(supplier, User.Identity.Name) });
            }

            return PartialView("_Form", supplier);
        }

        [Route("{id}/edit")]
        [ActionName("edit")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var supplier = suppliersBL.GetSupplierDetails(id);
            if (supplier == null)
            {
                return HttpNotFound();
            }

            return View("Edit", supplier);
        }

        [HttpPost]
        [Route("{id}/edit")]
        [ActionName("edit")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(SupplierVM Supplier)
        {
            if (ModelState.IsValid)
            {
                return Json(new { result = suppliersBL.UpdateSupplierRecord(Supplier, User.Identity.Name) });
            }

            return PartialView("_Form", Supplier);
        }

        [Route("{id}/delete")]
        [ActionName("delete")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var supplier = suppliersBL.GetSupplierDetails(id);
            if (supplier == null)
            {
                return HttpNotFound();
            }
            ViewBag.SupplierID = supplier.ID;
            return View(supplier);
        }

        [Route("{SupplierID}/delete")]
        [HttpPost, ActionName("delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int SupplierID)
        {
            return Json(new { result = suppliersBL.DeleteSupplierRecord(SupplierID, User.Identity.Name) });
        }

        [Route("restore-list")]
        [ActionName("restoreindex")]
        public ActionResult RestoreIndex()
        {
            return View(suppliersBL.GetPurgedSuppliers());
        }

        [Route("{id}/restore")]
        [ActionName("restore")]
        public ActionResult Restore(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var supplier = suppliersBL.GetSupplierDetails(id);
            if (supplier == null)
            {
                return HttpNotFound();
            }
            return PartialView(supplier);
        }

        [Route("{SupplierID}/restore")]
        [HttpPost, ActionName("restore")]
        [ValidateAntiForgeryToken]
        public ActionResult RestoreConfirmed(int SupplierID)
        {
            return Json(new { result = suppliersBL.RestoreSupplierRecord(SupplierID, User.Identity.Name) });
        }

        private void ValidateSupplier(SupplierVM supplier)
        {
            SupplierValidator _validator = new SupplierValidator();
            ValidationResult _validationResult = _validator.Validate(supplier);
            if (!_validationResult.IsValid)
            {
                foreach (ValidationFailure _result in _validationResult.Errors)
                {
                    ModelState.AddModelError(_result.PropertyName, _result.ErrorMessage);
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                suppliersBL.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
