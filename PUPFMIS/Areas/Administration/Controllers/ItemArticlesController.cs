using FluentValidation.Results;
using PUPFMIS.BusinessAndDataLogic;
using PUPFMIS.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace PUPFMIS.Controllers
{
    [RouteArea("administration")]
    [RoutePrefix("item/articles")]
    [UserAuthorization(Roles = SystemRoles.SuperUser + ", " + SystemRoles.SystemAdmin)]
    public class ItemArticlesController : Controller
    {
        private ItemArticlesBDL itemArticlesBDL = new ItemArticlesBDL();

        [Route("")]
        [ActionName("list")]
        public ActionResult Index()
        {
            return View("Index", itemArticlesBDL.GetArticles().Where(d => d.PurgeFlag == false).ToList());
        }

        [Route("restore-list")]
        [ActionName("restore-list")]
        public ActionResult RestoreIndex()
        {
            return View("RestoreIndex", itemArticlesBDL.GetArticles().Where(d => d.PurgeFlag == true).ToList());
        }

        [ActionName("details")]
        [Route("{ArticleCode}/details")]
        public ActionResult Details(string ArticleCode)
        {
            if (ArticleCode == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var itemArticle = itemArticlesBDL.GetItemArticle(ArticleCode);
            if (itemArticle == null)
            {
                return HttpNotFound();
            }

            var UACSAccount = itemArticlesBDL.GetChartOfAccounts().Where(d => d.Value == itemArticle.UACSObjectClass).First();
            itemArticle.UACSObjectClass = UACSAccount.Value.ToString() + " - " + UACSAccount.Text;
            return View("Details", itemArticle);
        }

        [ActionName("delete")]
        [Route("{ArticleCode}/delete")]
        public ActionResult Delete(string ArticleCode)
        {
            if (ArticleCode == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var itemArticle = itemArticlesBDL.GetItemArticle(ArticleCode);
            if (itemArticle == null)
            {
                return HttpNotFound();
            }

            var UACSAccount = itemArticlesBDL.GetChartOfAccounts().Where(d => d.Value == itemArticle.UACSObjectClass).First();
            itemArticle.UACSObjectClass = UACSAccount.Value.ToString() + " - " + UACSAccount.Text;
            return View("Delete", itemArticle);
        }

        [ActionName("restore")]
        [Route("{ArticleCode}/restore")]
        public ActionResult Restore(string ArticleCode)
        {
            if (ArticleCode == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var itemArticles = itemArticlesBDL.GetItemArticle(ArticleCode);
            if (itemArticles == null)
            {
                return HttpNotFound();
            }
            return View("Restore", itemArticles);
        }

        [Route("create")]
        [ActionName("create")]
        public ActionResult Create()
        {
            ViewBag.ItemTypeReference = new SelectList(itemArticlesBDL.GetItemTypes(), "ID", "ItemType");
            ViewBag.UACSObjectClass = new SelectList(itemArticlesBDL.GetChartOfAccounts(), "Value", "Text");
            ViewBag.GeneralLedger = itemArticlesBDL.GetChartOfAccounts().First().Value.Substring(5, 2);
            return View();
        }

        [HttpPost]
        [Route("create")]
        [ActionName("create")]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ItemArticles ItemArticleModel)
        {
            if (ItemArticleModel == null)
            {
                return Json(new { result = false });
            }

            ModelState.Remove("ID");
            ModelState.Remove("ArticleCode");
            ModelState.Remove("PurgeFlag");
            ModelState.Remove("CreatedAt");

            if (ModelState.IsValid)
            {
                //var Key = string.Empty;
                //var Message = new List<string>();
                //itemArticlesBDL.Validate(ItemArticleModel, out Key, out Message);
                //if (Key != null)
                //{
                //    foreach (var errorMessage in Message)
                //    {
                //        ModelState.AddModelError(Key, errorMessage);
                //    }
                //    ViewBag.ItemTypeReference = new SelectList(itemArticlesBDL.GetItemTypes(), "ID", "ItemType", ItemArticleModel.ItemTypeReference);
                //    ViewBag.UACSObjectClass = new SelectList(itemArticlesBDL.GetChartOfAccounts(), "Value", "Text", ItemArticleModel.UACSObjectClass);
                //    return PartialView("_Form", ItemArticleModel);
                //}
                return Json(new { result = itemArticlesBDL.AddItemArticleRecord(ItemArticleModel, User.Identity.Name) });
            }
            ViewBag.ItemTypeReference = new SelectList(itemArticlesBDL.GetItemTypes(), "ID", "ItemType", ItemArticleModel.ItemTypeReference);
            ViewBag.UACSObjectClass = new SelectList(itemArticlesBDL.GetChartOfAccounts(), "Value", "Text", ItemArticleModel.UACSObjectClass);
            return PartialView("_Form", ItemArticleModel);
        }

        [ActionName("edit")]
        [Route("{ArticleCode}/edit")]
        public ActionResult Edit(string ArticleCode)
        {
            if (ArticleCode == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var itemArticle = itemArticlesBDL.GetItemArticle(ArticleCode);
            if (itemArticle == null)
            {
                return HttpNotFound();
            }
            ViewBag.ItemTypeReference = new SelectList(itemArticlesBDL.GetItemTypes(), "ID", "ItemType", itemArticle.ItemTypeReference);
            ViewBag.UACSObjectClass = new SelectList(itemArticlesBDL.GetChartOfAccounts(), "Value", "Text", itemArticle.UACSObjectClass);
            return View(itemArticle);
        }

        [HttpPost]
        [ActionName("edit")]
        [ValidateAntiForgeryToken]
        [Route("{ArticleCode}/edit")]
        public ActionResult Edit(ItemArticles ItemArticleModel)
        {
            if (ModelState.IsValid)
            {
                string Key = null;
                string Message = null;
                var result = itemArticlesBDL.UpdateItemArticleRecord(ItemArticleModel, User.Identity.Name, out Key, out Message);
                if (result == false && (Key != null && Message != null))
                {
                    ModelState.AddModelError(Key, Message);
                    ViewBag.ItemTypeReference = new SelectList(itemArticlesBDL.GetItemTypes(), "ID", "ItemType", ItemArticleModel.ItemTypeReference);
                    ViewBag.UACSObjectClass = new SelectList(itemArticlesBDL.GetChartOfAccounts(), "Value", "Text", ItemArticleModel.UACSObjectClass);
                    return PartialView("_Form", ItemArticleModel);
                }
                else
                {
                    return Json(new { result = result });
                }
            }

            ViewBag.ItemTypeReference = new SelectList(itemArticlesBDL.GetItemTypes(), "ID", "ItemType", ItemArticleModel.ItemTypeReference);
            ViewBag.UACSObjectClass = new SelectList(itemArticlesBDL.GetChartOfAccounts(), "Value", "Text", ItemArticleModel.UACSObjectClass);
            return PartialView("_Form", ItemArticleModel);
        }

        [ValidateAntiForgeryToken]
        [Route("{ArticleCode}/delete")]
        [HttpPost, ActionName("delete")]
        public ActionResult DeleteConfirmed(string ArticleCode)
        {
            if (ArticleCode == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return Json(new { result = itemArticlesBDL.PurgeItemArticleRecord(ArticleCode, User.Identity.Name) });
        }

        [HttpPost]
        [ActionName("restore")]
        [ValidateAntiForgeryToken]
        [Route("{ArticleCode}/restore")]
        public ActionResult RestoreConfirmed(string ArticleCode)
        {
            if (ArticleCode == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return Json(new { result = itemArticlesBDL.RestoreItemArticleRecord(ArticleCode, User.Identity.Name) });
        }

        [HttpGet]
        [ActionName("get-gl-account")]
        [Route("{UACS}/get-gl-account")]
        public ActionResult GetGLAccount(string UACS)
        {
            var GLAccount = UACS.Substring(5, 2);
            return Json(new { GLAccount = GLAccount }, JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                itemArticlesBDL.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
