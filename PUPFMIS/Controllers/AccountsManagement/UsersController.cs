using FluentValidation.Results;
using Microsoft.Owin.Security;
using PUPFMIS.BusinessLayer;
using PUPFMIS.Models;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace PUPFMIS.Controllers
{
    [Route("users-management/accounts/{action}")]
    [Authorize]
    public class UsersController : Controller
    {
        private FMISDbContext db = new FMISDbContext();
        private AccountsManagementBL accountsManagementBL = new AccountsManagementBL();
        private OfficesBL officesBL = new OfficesBL();

        public ActionResult index()
        {
            return View(db.UserInformation.ToList());
        }

        [Route("login")]
        [AllowAnonymous]
        public ActionResult login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public ActionResult login(LoginVM login, string returnUrl)
        {
            string Message = string.Empty;
            if (ModelState.IsValid)
            {
                UsersVM user;
                if (accountsManagementBL.VerifyUserCredentials(login, out Message, out user))
                {
                    ViewData["User"] = user;
                    var claims = new[]
                    {
                        new Claim(ClaimTypes.Name, user.Email)
                    };
                    var identity = new ClaimsIdentity(claims, "ApplicationCookie");
                    var context = Request.GetOwinContext();
                    var authManager = context.Authentication;
                    authManager.SignIn(new AuthenticationProperties { IsPersistent = false }, identity);
                    return RedirectToLocal(returnUrl);
                }
            }
            return View(login);
        }

        [HttpPost]
        [Route("logout")]
        [AllowAnonymous]
        public ActionResult logout()
        {
            var ctx = Request.GetOwinContext();
            var authManager = ctx.Authentication;

            authManager.SignOut("ApplicationCookie");
            return RedirectToAction("login");
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("index", "Home");
        }

        [AllowAnonymous]
        public ActionResult register()
        {
            ViewBag.Offices = new SelectList(officesBL.GetOffices(), "ID", "OfficeName");
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult register(UsersVM user)
        {
            ModelState.Remove("PasswordSalt");
            string Message = string.Empty;
            ValidateUser(user);
            if (ModelState.IsValid)
            {
                if(accountsManagementBL.RegisterUser(user, out Message))
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            ViewBag.Offices = new SelectList(officesBL.GetOffices(), "ID", "OfficeName");
            return View(user);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserInformation userInformation = db.UserInformation.Find(id);
            if (userInformation == null)
            {
                return HttpNotFound();
            }
            return View(userInformation);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,LastName,FirstName,MiddleName,Designation")] UserInformation userInformation)
        {
            if (ModelState.IsValid)
            {
                db.UserInformation.Add(userInformation);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(userInformation);
        }

        // GET: Users/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserInformation userInformation = db.UserInformation.Find(id);
            if (userInformation == null)
            {
                return HttpNotFound();
            }
            return View(userInformation);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,LastName,FirstName,MiddleName,Designation")] UserInformation userInformation)
        {
            if (ModelState.IsValid)
            {
                db.Entry(userInformation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(userInformation);
        }

        // GET: Users/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserInformation userInformation = db.UserInformation.Find(id);
            if (userInformation == null)
            {
                return HttpNotFound();
            }
            return View(userInformation);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            UserInformation userInformation = db.UserInformation.Find(id);
            db.UserInformation.Remove(userInformation);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        private void ValidateUser(UsersVM user)
        {
            UserAccountsValidator _validator = new UserAccountsValidator();
            ValidationResult _validationResult = _validator.Validate(user);
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
                
                db.Dispose();
                accountsManagementBL.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
