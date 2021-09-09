//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;
//using PUPFMIS.Models;
//using PUPFMIS.BusinessAndDataLogic;

//namespace PUPFMIS.Areas.Procurement.Controllers
//{
//    [Route("{action}")]
//    [RouteArea("procurement")]
//    [RoutePrefix("bids")]
//    [Authorize(Roles = SystemRoles.ProjectCoordinator)]
//    public class BidsRegistrationController : Controller
//    {
//        private BidsRegistrationDAL bidsRegistrationDAL = new BidsRegistrationDAL();
//        private SuppliersBL suppliersBL = new SuppliersBL();

//        [Route("")]
//        [ActionName("dashboard")]
//        public ActionResult Dashboard()
//        {
//            var dashboard = new BidsDashboard();
//            dashboard.Contracts = bidsRegistrationDAL.GetContractsWithoutBids(User.Identity.Name);
//            return View(dashboard);
//        }

//        [Route("registration/{ContractCode}")]
//        [ActionName("index")]
//        public ActionResult Index(string ContractCode)
//        {
//            var bidsList = bidsRegistrationDAL.GetBidList(ContractCode);
//            ViewBag.ContractCode = ContractCode;
//            return View(bidsList);
//        }

//        [Route("registration/{ContractCode}/create")]
//        [ActionName("create")]
//        public ActionResult Create(string ContractCode)
//        {
//            var bid = bidsRegistrationDAL.BidRegistrationSetup(ContractCode);
//            var existingBidders = bidsRegistrationDAL.GetExistingBidders(ContractCode);
//            var bidActions = bid.ContractStrategy == ContractStrategies.LineItemBidding ? Enum.GetValues(typeof(BidActions)).Cast<BidActions>()
//                .Where(d => d == BidActions.Blank || d == BidActions.WithBid)
//                .Select(d => new SelectListItem
//                {
//                    Value = ((int)d).ToString(),
//                    Text = d.GetAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>().Name
//                }).ToList() : Enum.GetValues(typeof(BidActions)).Cast<BidActions>()
//               .Where(d => d == BidActions.NoBid || d == BidActions.WithBid)
//               .Select(d => new SelectListItem
//               {
//                   Value = ((int)d).ToString(),
//                   Text = d.GetAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>().Name
//               }).ToList();
//            ViewBag.SupplierReference = new SelectList(suppliersBL.GetActiveSuppliers().Where(d => !existingBidders.Contains(d.ID) && d.ID != 1).ToList(), "ID", "SupplierName");
//            ViewBag.BidAction = new SelectList(bidActions, "Value", "Text");
//            return View(bid);
//        }

//        [HttpPost]
//        [ActionName("create")]
//        [ValidateAntiForgeryToken]
//        [Route("registration/{ContractCode}/create")]
//        public ActionResult Create(BidsVM Bid)
//        {
//            Bid.BidDetails = bidsRegistrationDAL.ComputeBidTotalPrice(Bid.BidDetails);
//            if (Bid.ApprovedBudgetForContract > Bid.BidDetails.Sum(d => d.TotalCost))
//            {
//                ModelState.AddModelError("", "Total Bid must not be greater than ABC.");
//            }
//            if(ModelState.IsValid)
//            {
//                return Json(new { result = bidsRegistrationDAL.PostBidRegistration(Bid, User.Identity.Name, BidRegistrationOptions.WinningBidsOnly) }, JsonRequestBehavior.AllowGet);
//            }

//            var bidActions = Bid.ContractStrategy == ContractStrategies.LineItemBidding ? Enum.GetValues(typeof(BidActions)).Cast<BidActions>()
//                .Where(d => d == BidActions.Blank || d == BidActions.WithBid)
//                .Select(d => new SelectListItem
//                {
//                    Value = ((int)d).ToString(),
//                    Text = d.GetAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>().Name
//                }).ToList() : Enum.GetValues(typeof(BidActions)).Cast<BidActions>()
//               .Where(d => d == BidActions.NoBid || d == BidActions.WithBid)
//               .Select(d => new SelectListItem
//               {
//                   Value = ((int)d).ToString(),
//                   Text = d.GetAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>().Name
//               }).ToList();

//            ViewBag.SupplierReference = new SelectList(suppliersBL.GetActiveSuppliers().Where(d => d.ID != 1).ToList(), "ID", "SupplierName");
//            ViewBag.BidAction = new SelectList(bidActions, "Value", "Text");
//            return View(Bid);
//        }

//        protected override void Dispose(bool disposing)
//        {
//            if (disposing)
//            {
//                bidsRegistrationDAL.Dispose();
//            }
//            base.Dispose(disposing);
//        }
//    }
//}