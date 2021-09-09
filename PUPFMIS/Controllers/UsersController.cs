using FluentValidation.Results;
using Microsoft.Owin.Security;
using PUPFMIS.BusinessAndDataLogic;
using PUPFMIS.Models;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace PUPFMIS.Controllers
{

    [Route("{action}")]
    public class UsersController : Controller
    {
        private HRISDataAccess hrisDataAccess = new HRISDataAccess();
        private FMISDbContext db = new FMISDbContext();
        private AccountsManagementBL accountsManagementBL = new AccountsManagementBL();

        [ActionName("index")]
        [Route("users-management")]
        [Route("users-management/list")]
        [UserAuthorization(Roles = SystemRoles.SuperUser + ", " + SystemRoles.SystemAdmin)]
        public ActionResult Index()
        {
            return View("Index", accountsManagementBL.GetUserAccountsList());
        }

        [AllowAnonymous]
        [Route("login")]
        public ActionResult Login(string ReturnUrl)
        {
            TempData["ReturnUrl"] = ReturnUrl;
            return RedirectToAction("user-login");
        }

        [AllowAnonymous]
        [Route("user-login")]
        [ActionName("user-login")]
        public ActionResult LoginView()
        {
            ViewBag.ReturnUrl = TempData["ReturnUrl"];
            return View("login");
        }

        [HandleError]
        [HttpPost]
        [AllowAnonymous]
        [Route("login")]
        public ActionResult Login(LoginVM login)
        {
            ModelState.Remove("NoOfAttempts");
            string Error = string.Empty;
            Claim[] claims;

            if (ModelState.IsValid)
            {
                UsersVM User;
                if (accountsManagementBL.VerifyUserCredentials(login, out User, out Error))
                {
                    User.IsResponsibilityCenter = db.ItemTypes.Where(d => d.ResponsibilityCenter == User.DepartmentCode).Select(d => d.ResponsibilityCenter).Distinct().Count() == 1 ? "Yes" : "No";
                    claims = new[]
                    {
                        new Claim(ClaimTypes.Name, User.Email),
                        new Claim(ClaimTypes.Role, User.UserRole),
                        new Claim("Employee", User.Employee.ToUpperInvariant()),
                        new Claim("Office", User.Unit.ToUpperInvariant()),
                        new Claim("Designation", User.UserRole),
                        new Claim("IsResponsibilityCenter", User.IsResponsibilityCenter.ToString())
                    };
                    var identity = new ClaimsIdentity(claims, "ApplicationCookie");
                    var context = Request.GetOwinContext();
                    var authManager = context.Authentication;
                    authManager.SignIn(new AuthenticationProperties { IsPersistent = false }, identity);

                    return RedirectToLocal(login.ReturnUrl);
                }
                else
                {
                    if (Error == "Invalid Email")
                    {
                        ModelState.AddModelError("", "Couldn't find account. Please contact the System Administrator.");
                    }
                    else if (Error == "Incorrect Password")
                    {
                        login.NoOfAttempts += 1;
                        ModelState.AddModelError("", "Incorrect Password. Please try again. Number of attempts left: " + (5 - login.NoOfAttempts) + ".");
                    }
                    else if (Error == "Locked Out")
                    {
                        ModelState.AddModelError("", "You have been locked out of the system. Please try logging in after one (1) hour.");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Couldn't find account. Please contact the System Administrator.");
                    }
                }
            }
            return View(login);
        }

        [HttpPost]
        [Route("logout")]
        [AllowAnonymous]
        public ActionResult Logout()
        {
            var ctx = Request.GetOwinContext();
            var authManager = ctx.Authentication;

            authManager.SignOut("ApplicationCookie");
            return RedirectToAction("login");
        }

        [Route("logout")]
        [ActionName("logout-link")]
        [AllowAnonymous]
        public ActionResult LogoutLink()
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

        [Route("details")]
        [ActionName("details")]
        public ActionResult Details(int? UserID)
        {
            if(UserID == null)
            {
                return HttpNotFound();
            }
            var user = accountsManagementBL.GetUser((int)UserID);
            return PartialView(user);
        }

        [Route("delete")]
        [ActionName("delete")]
        public ActionResult Delete(int? UserID)
        {
            if (UserID == null)
            {
                return HttpNotFound();
            }
            var user = accountsManagementBL.GetUser((int)UserID);
            return PartialView(user);
        }

        [HttpPost, ActionName("delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int UserID)
        {
            var user = accountsManagementBL.GetUser(UserID);
            return Json(new { result = accountsManagementBL.DeleteUser(UserID) });
        }

        [Route("register")]
        [ActionName("register")]
        public ActionResult Register()
        {
            ViewBag.DepartmentCode = new SelectList(hrisDataAccess.GetAllDepartments().Where(d => d.Lvl <= 1).ToList(), "DepartmentCode", "Department");
            var DepartmentCode = hrisDataAccess.GetAllDepartments().First().DepartmentCode;
            ViewBag.EmpCode = new SelectList(hrisDataAccess.GetEmployees(DepartmentCode), "EmployeeCode", "EmployeeName");
            ViewBag.UnitCode = new SelectList(hrisDataAccess.GetAllUnits(DepartmentCode), "DepartmentCode", "Department");
            ViewBag.UserRole = new SelectList(accountsManagementBL.GetRoles(), "ID", "Role");
            return PartialView("Register");
        }

        [HttpPost]
        [Route("register")]
        [ActionName("register")]
        [ValidateAntiForgeryToken]
        public ActionResult Register(UsersVM User)
        {
            if(User == null)
            {
                return HttpNotFound();
            }
            string Message = string.Empty;
            return Json(new { result = accountsManagementBL.RegisterUser(User) });
        }

        [Route("update")]
        [ActionName("update")]
        public ActionResult Update(int? UserID)
        {
            if(UserID == null)
            {
                return HttpNotFound();
            }
            var user = accountsManagementBL.GetUser((int)UserID);
            if(user == null)
            {
                return HttpNotFound();
            }
            ViewBag.UnitCode = new SelectList(hrisDataAccess.GetAllUnits(user.DepartmentCode), "DepartmentCode", "Department", user.UnitCode);
            ViewBag.UserRole = new SelectList(accountsManagementBL.GetRoles(), "ID", "Role", user.RoleID);
            return PartialView("Edit", user);
        }

        [HttpPost]
        [Route("update")]
        [ActionName("update")]
        [ValidateAntiForgeryToken]
        public ActionResult Update(UsersVM User)
        {
            if (User == null)
            {
                return HttpNotFound();
            }
            return Json(new { result = accountsManagementBL.UpdateUser(User) });
        }

        [ActionName("get-employees")]
        public ActionResult GetEmployees(string DepartmentCode)
        {
            return Json(hrisDataAccess.GetEmployees(DepartmentCode), JsonRequestBehavior.AllowGet);
        }

        [ActionName("get-units")]
        public ActionResult GetUnits(string DepartmentCode)
        {
            return Json(hrisDataAccess.GetAllUnits(DepartmentCode), JsonRequestBehavior.AllowGet);
        }

        [ActionName("get-employee-details")]
        public ActionResult GetEmployeeDetails(string EmpCode)
        {
            return Json(accountsManagementBL.GetEmployeeDetails(EmpCode), JsonRequestBehavior.AllowGet);
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
    public class UserAuthorization : AuthorizeAttribute
    {
        protected override void HandleUnauthorizedRequest(AuthorizationContext context)
        {
            if (context.HttpContext.User.Identity.IsAuthenticated)
            {

                context.Result = new RedirectResult("/errors/unauthorized-access"); // Give error controller or Url name
            }
            else
            {
                context.Result = new RedirectResult("/login");
            }
        }
    }

}
