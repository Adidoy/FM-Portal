using PUPFMIS.BusinessAndDataLogic;
using PUPFMIS.Models;
using System.Net;
using System.Web.Mvc;

namespace PUPFMIS.Controllers
{
    [RouteArea("")]
    [Authorize]
    [Route("ops/master-tables/items-prices/{action}")]
    public class ItemPricesController : Controller
    {
        private ItemPricesBL itemPricesBL = new ItemPricesBL();

        public ActionResult Index()
        {
            return View(itemPricesBL.GetPrevailingPrices());
        }

        [Route("ops/master-tables/items-prices/{ItemCode}/history")]
        public ActionResult IndexPriceHistory(string ItemCode)
        {
            if(ItemCode == null)
            {
                return HttpNotFound();
            }
            return View(itemPricesBL.GetPriceHistory(ItemCode));
        }

        public ActionResult UpdatePrices(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ItemPrice itemPrice = itemPricesBL.GetPriceDetails(id);
            if (itemPrice == null)
            {
                return HttpNotFound();
            }
            return PartialView(itemPrice);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdatePrices(ItemPrice itemPrice)
        {
            ModelState.Remove("EffectivityDate");
            if (ModelState.IsValid)
            {
                if (itemPricesBL.AddPriceRecord(itemPrice))
                {
                    return Json(new { status = "success" });
                }
                else
                {
                    return Json(new { status = "failed" });
                }
            }
            return PartialView(itemPricesBL);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                itemPricesBL.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
