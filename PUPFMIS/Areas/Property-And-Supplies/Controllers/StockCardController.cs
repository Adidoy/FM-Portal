using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PUPFMIS.Models;
using PUPFMIS.BusinessAndDataLogic;

namespace PUPFMIS.Areas.Property_And_Supplies.Controllers
{
    [Route("{action}")]
    [RouteArea("property-and-supplies")]
    [RoutePrefix("supplies/cards")]
    [Authorize(Roles = SystemRoles.SuppliesChief + ", " + SystemRoles.PropertyDirector)]
    public class StockCardController : Controller
    {
        private StockCardsDAL stockCards = new StockCardsDAL();

        [Route("")]
        [ActionName("dashboard")]
        public ActionResult Dashboard()
        {
            var supplies = stockCards.GetSuppliesList();
            return View("Dashboard", supplies);
        }

        [Route("stock/{StockNo}/print")]
        [ActionName("print-stock-card")]
        public ActionResult PrintStockCard(string StockNo)
        {
            var stockCardBL = new StockCardsBL();
            var stream = stockCardBL.GenerateStockCard(StockNo);
            Response.Clear();
            Response.ClearContent();
            Response.ClearHeaders();
            Response.AddHeader("content-length", stream.Length.ToString());
            Response.AppendHeader("Content-Disposition", "inline; filename=Stock Card - " + StockNo + ".pdf");
            Response.ContentType = "application/pdf";
            Response.BinaryWrite(stream.ToArray());
            stream.Close();
            Response.End();

            return RedirectToAction("dashboard");
        }

        [Route("ledger/{StockNo}/print")]
        [ActionName("print-supply-ledger-card")]
        public ActionResult PrintSuppliesLedgerCard(string StockNo)
        {
            var stockCardBL = new StockCardsBL();
            var stream = stockCardBL.GenerateSuppliesLedgerCard(StockNo);
            Response.Clear();
            Response.ClearContent();
            Response.ClearHeaders();
            Response.AddHeader("content-length", stream.Length.ToString());
            Response.AppendHeader("Content-Disposition", "inline; filename=Supplies Ledger Card - " + StockNo + ".pdf");
            Response.ContentType = "application/pdf";
            Response.BinaryWrite(stream.ToArray());
            stream.Close();
            Response.End();

            return RedirectToAction("dashboard");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                stockCards.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}