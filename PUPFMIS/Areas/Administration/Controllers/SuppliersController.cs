using FluentValidation.Results;
using PUPFMIS.BusinessAndDataLogic;
using PUPFMIS.Models;
using System.Net;
using System.Web.Mvc;

namespace PUPFMIS.Controllers
{
    [Route("{action}")]
    [RouteArea("administration")]
    [RoutePrefix("suppliers")]
    [Authorize(Roles = SystemRoles.SuperUser + ", " + SystemRoles.SystemAdmin)]
    public class SuppliersController : Controller
    {
        private SuppliersBL suppliersBL = new SuppliersBL();

        [Route("")]
        [Route("list")]
        [ActionName("index")]
        public ActionResult Index()
        {
            return View("Index", suppliersBL.GetActiveSuppliers());
        }

        public ActionResult ViewSuppliers(int seqNo)
        {
            ViewBag.SeqNo = seqNo;
            return PartialView(suppliersBL.GetActiveSuppliers());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Supplier _supplier = suppliersBL.GetSupplierDetails(id);
            if (_supplier == null)
            {
                return HttpNotFound();
            }
            return PartialView(_supplier);
        }

        public ActionResult Create()
        {
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Supplier supplier)
        {
            ValidateSupplier(supplier);
            if (ModelState.IsValid)
            {
                if (suppliersBL.AddSupplierRecord(supplier))
                {
                    return Json(new { status = "success" });
                }
                else
                {
                    return Json(new { status = "failed" });
                }
            }
            return PartialView(supplier);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Supplier _supplier = suppliersBL.GetSupplierDetails(id);
            if (_supplier == null)
            {
                return HttpNotFound();
            }
            return PartialView(_supplier);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Supplier supplier)
        {
            ValidateSupplier(supplier);
            if (ModelState.IsValid)
            {
                if (suppliersBL.UpdateSupplierRecord(supplier, false))
                {
                    return Json(new { status = "success" });
                }
                else
                {
                    return Json(new { status = "failed" });
                }
            }
            return PartialView(supplier);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Supplier _supplier = suppliersBL.GetSupplierDetails(id);
            if (_supplier == null)
            {
                return HttpNotFound();
            }
            return PartialView(_supplier);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Supplier _supplier = suppliersBL.GetSupplierDetails(id);
            if(suppliersBL.UpdateSupplierRecord(_supplier, true))
            {
                return Json(new { status = "success" });
            }
            return Json(new { status = "failed" });
        }

        [Route("restore-list")]
        [ActionName("restoreindex")]
        public ActionResult RestoreIndex()
        {
            return View(suppliersBL.GetPurgedSuppliers());
        }

        public ActionResult Restore(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Supplier _supplier = suppliersBL.GetSupplierDetails(id);
            if (_supplier == null)
            {
                return HttpNotFound();
            }
            return PartialView(_supplier);
        }

        [HttpPost, ActionName("Restore")]
        [ValidateAntiForgeryToken]
        public ActionResult RestoreConfirmed(int id)
        {
            Supplier _supplier = suppliersBL.GetSupplierDetails(id);
            if (suppliersBL.RestoreSupplierRecord(_supplier))
            {
                return Json(new { status = "success" });
            }
            return Json(new { status = "failed" });
        }

        private void ValidateSupplier(Supplier supplier)
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
