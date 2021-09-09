using PUPFMIS.BusinessAndDataLogic;
using PUPFMIS.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace PUPFMIS.Areas.Procurement.Controllers
{

    [Route("{action}")]
    [RouteArea("procurement")]
    [RoutePrefix("contracts")]
    [Authorize(Roles = SystemRoles.ProcurementAdministrator + ", " + SystemRoles.ProcurementPlanningChief)]
    public class ContractsController : Controller
    {
        ProcurementProjectsDAL contractDAL = new ProcurementProjectsDAL();

        [Route("")]
        [ActionName("dashboard")]
        public ActionResult Dashboard()
        {
            return View();
        }

        [Route("competitive-bidding/setup")]
        [ActionName("setup-bidding-project")]
        public ActionResult SetupCompetitiveBiddingContract()
        {
            var procurementPrograms = contractDAL.GetProcurementPrograms(1);
            var modesOfProcurement = contractDAL.GetModesOfProcurement(procurementPrograms[0].PAPCode).Where(d => d.ID == 1).ToList();
            var contractStrategies = Enum.GetValues(typeof(ContractStrategies)).Cast<ContractStrategies>()
                                           .Where(d => d < ContractStrategies.ItemProcurement)
                                           .Select(d => new SelectListItem
                                           {
                                               Value = ((int)d).ToString(),
                                               Text = d.GetAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>().Name
                                           }).ToList();
            var contractSetup = new ProcurementProjectSetupVM();
            contractSetup.ProcurementProjectType = ProcurementProjectTypes.CPB;
            ViewBag.ContractStrategy = new SelectList(contractStrategies, "Value", "Text");
            ViewBag.ModeOfProcurement = new SelectList(modesOfProcurement, "ID", "ModeOfProcurementName");
            ViewBag.PAPCode = new SelectList(procurementPrograms, "PAPCode", "ProgramName");
            return View("SetupCompetitiveBiddingContract", contractSetup);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("competitive-bidding/setup")]
        [ActionName("setup-bidding-project")]
        public ActionResult SetupCompetitiveBiddingContract(ProcurementProjectSetupVM BiddingSetup)
        {
            if ((BiddingSetup.ContractStrategy == ContractStrategies.LotBidding || BiddingSetup.ContractStrategy == ContractStrategies.LineItemBidding) && contractDAL.GetProgramItems(BiddingSetup.PAPCode).Select(d => d.ArticleReference).ToList().Count <= 1)
            {
                ModelState.AddModelError("", "Procurement Program cannot be converted to Lot/Line Procurement Contract. Please select Single Contract Procurement as the Contract Strategy.");
                var procurementPrograms = contractDAL.GetProcurementPrograms(1);
                var modesOfProcurement = contractDAL.GetModesOfProcurement(procurementPrograms[0].PAPCode);
                var contractStrategies = Enum.GetValues(typeof(ContractStrategies)).Cast<ContractStrategies>()
                                               .Where(d => d < ContractStrategies.ItemProcurement)
                                               .Select(d => new SelectListItem
                                               {
                                                   Value = ((int)d).ToString(),
                                                   Text = d.GetAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>().Name
                                               }).ToList();
                BiddingSetup.ProcurementProjectType = ProcurementProjectTypes.CPB;
                ViewBag.ContractStrategy = new SelectList(contractStrategies, "Value", "Text", BiddingSetup.ContractStrategy);
                ViewBag.ModeOfProcurement = new SelectList(modesOfProcurement, "ID", "ModeOfProcurementName", BiddingSetup.ModeOfProcurement);
                ViewBag.PAPCode = new SelectList(procurementPrograms, "PAPCode", "ProgramName", BiddingSetup.PAPCode);
                return View("SetupCompetitiveBiddingContract", BiddingSetup);
            }

            if (BiddingSetup.ContractStrategy == ContractStrategies.BulkBidding)
            {
                var contract = contractDAL.SetupIndividualContract(BiddingSetup);
                ViewBag.ProjectCoordinator = new SelectList(contractDAL.GetProcurementEmployees(), "EmployeeCode", "EmployeeName");
                ViewBag.ContractStrategy = contract.ContractStrategy;
                return View("CreateIndividualContract", contract);
            }
            else if (BiddingSetup.ContractStrategy == ContractStrategies.LotBidding)
            {
                var contract = contractDAL.SetupLotContract(BiddingSetup);
                ViewBag.ProjectCoordinator = new SelectList(contractDAL.GetProcurementEmployees(), "EmployeeCode", "EmployeeName");
                return View("CreateLotContract", contract);
            }
            else
            {
                var contract = contractDAL.SetupIndividualContract(BiddingSetup);
                ViewBag.ProjectCoordinator = new SelectList(contractDAL.GetProcurementEmployees(), "EmployeeCode", "EmployeeName");
                ViewBag.ContractStrategy = contract.ContractStrategy;
                return View("CreateIndividualContract", contract);
            }
        }

        [Route("alternative-mode/setup")]
        [ActionName("setup-alternative-mode-project")]
        public ActionResult SetupAlternativeModeContract()
        {
            var procurementPrograms = contractDAL.GetProcurementPrograms();
            var modesOfProcurement = contractDAL.GetModesOfProcurement(procurementPrograms[0].PAPCode).Where(d => d.ID != 1 && d.ID != 10).ToList();
            var contractStrategies = Enum.GetValues(typeof(ContractStrategies)).Cast<ContractStrategies>()
                               .Where(d => d == ContractStrategies.BulkBidding)
                               .Select(d => new SelectListItem
                               {
                                   Value = ((int)d).ToString(),
                                   Text = d.GetAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>().Name
                               }).ToList();
            var contractSetup = new ProcurementProjectSetupVM();
            contractSetup.ProcurementProjectType = ProcurementProjectTypes.AMP;
            ViewBag.ContractStrategy = new SelectList(contractStrategies, "Value", "Text");
            ViewBag.ModeOfProcurement = new SelectList(modesOfProcurement, "ID", "ModeOfProcurementName");
            ViewBag.PAPCode = new SelectList(procurementPrograms, "PAPCode", "ProcurementProgram");
            return View("SetupAlternativeModeContract", contractSetup);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("alternative-mode/setup")]
        [ActionName("setup-alternative-mode-project")]
        public ActionResult SetupAlternativeModeContract(ProcurementProjectSetupVM AlternativeModeSetup)
        {
            if (AlternativeModeSetup.ContractStrategy == ContractStrategies.BulkBidding)
            {
                var contract = contractDAL.SetupIndividualAlternativeContract(AlternativeModeSetup);
                ViewBag.ProjectCoordinator = new SelectList(contractDAL.GetProcurementEmployees(), "EmployeeCode", "EmployeeName");
                return View("CreateIndividualContract", contract);
            }
            else if (AlternativeModeSetup.ContractStrategy == ContractStrategies.ItemProcurement)
            {
                var contract = contractDAL.SetupLotContract(AlternativeModeSetup);
                ViewBag.ProjectCoordinator = new SelectList(contractDAL.GetProcurementEmployees(), "EmployeeCode", "EmployeeName");
                ViewBag.ContractStrategy = contract.ContractStrategy;
                return View("CreateLotContract", contract);
            }
            else
            {
                var contract = contractDAL.SetupLotContract(AlternativeModeSetup);
                ViewBag.ProjectCoordinator = new SelectList(contractDAL.GetProcurementEmployees(), "EmployeeCode", "EmployeeName");
                return View("CreateLotContract", contract);
            }
        }

        [Route("agency-to-agency/setup")]
        [ActionName("setup-agency-to-agency")]
        public ActionResult SetupAgencyToAgencyContract()
        {
            var procurementPrograms = contractDAL.GetA2AProcurementPrograms();
            var modesOfProcurement = contractDAL.GetModesOfProcurement(procurementPrograms[0].PAPCode).Where(d => d.ID == 10).ToList();
            var contractStrategies = Enum.GetValues(typeof(ContractStrategies)).Cast<ContractStrategies>()
                               .Where(d => d == ContractStrategies.BulkBidding)
                               .Select(d => new SelectListItem
                               {
                                   Value = ((int)d).ToString(),
                                   Text = d.GetAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>().Name
                               }).ToList();
            var contractSetup = new ProcurementProjectSetupVM();
            contractSetup.ProcurementProjectType = ProcurementProjectTypes.AMP;
            ViewBag.ContractStrategy = new SelectList(contractStrategies, "Value", "Text");
            ViewBag.ModeOfProcurement = new SelectList(modesOfProcurement, "ID", "ModeOfProcurementName");
            ViewBag.PAPCode = new SelectList(procurementPrograms, "PAPCode", "ProcurementProgram");
            return View("SetupA2AContract", contractSetup);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("agency-to-agency/setup")]
        [ActionName("setup-agency-to-agency")]
        public ActionResult SetupAgencyToAgencyContract(ProcurementProjectSetupVM AgencyToAgency)
        {
            if (AgencyToAgency.ContractStrategy == ContractStrategies.BulkBidding)
            {
                var contract = contractDAL.SetupIndividualAlternativeContract(AgencyToAgency);
                ViewBag.ProjectCoordinator = new SelectList(contractDAL.GetProcurementEmployees(), "EmployeeCode", "EmployeeName");
                return View("CreateA2AContract", contract);
            }
            else if (AgencyToAgency.ContractStrategy == ContractStrategies.ItemProcurement)
            {
                var contract = contractDAL.SetupLotContract(AgencyToAgency);
                ViewBag.ProjectCoordinator = new SelectList(contractDAL.GetProcurementEmployees(), "EmployeeCode", "EmployeeName");
                ViewBag.ContractStrategy = contract.ContractStrategy;
                return View("CreateA2AContract", contract);
            }
            else
            {
                var contract = contractDAL.SetupLotContract(AgencyToAgency);
                ViewBag.ProjectCoordinator = new SelectList(contractDAL.GetProcurementEmployees(), "EmployeeCode", "EmployeeName");
                return View("CreateA2AContract", contract);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("create-individual-contract")]
        [ActionName("create-individual-contract")]
        public ActionResult PostIndividualContract(IndividualProcurementProjectVM Contract)
        {
            if (Contract.ModeOfProcurementReference == 10)
            {
                ModelState.Remove("PRSubmissionOpen");
                ModelState.Remove("PRSubmissionClose");
                ModelState.Remove("PreProcurementConference");
                ModelState.Remove("PostingOfIB");
                ModelState.Remove("PreBidConference");
                ModelState.Remove("DeadlineOfSubmissionOfBids");
                ModelState.Remove("OpeningOfBids");
                ModelState.Remove("NOAIssuance");
                ModelState.Remove("NTPIssuance");
            }
            else if (Contract.ModeOfProcurementReference != 1)
            {
                ModelState.Remove("PreBidConference");
            }
            if (ModelState.IsValid)
            {
                return Json(new { result = contractDAL.PostIndividualContract(Contract, User.Identity.Name) });
            }

            ViewBag.ProjectCoordinator = new SelectList(contractDAL.GetProcurementEmployees(), "EmployeeCode", "EmployeeName", Contract.ProjectCoordinator);
            return PartialView("_CreateIndividualContract", Contract);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("create-lot-contract")]
        [ActionName("create-lot-contract")]
        public ActionResult PostLotContract(LotProcurementProjectVM LotContract)
        {
            for (int i = 0; i < LotContract.SubContracts.Count; i++)
            {
                ModelState.Remove("SubContracts[" + i + "].PAPCode");
                ModelState.Remove("SubContracts[" + i + "].Classification");
                ModelState.Remove("SubContracts[" + i + "].ModeOfProcurement");
                ModelState.Remove("SubContracts[" + i + "].FundSource");
                ModelState.Remove("SubContracts[" + i + "].FundDescription");
                ModelState.Remove("SubContracts[" + i + "].ContractLocation");
                ModelState.Remove("SubContracts[" + i + "].ProjectCoordinator");
                ModelState.Remove("SubContracts[" + i + "].PRSubmissionOpen");
                ModelState.Remove("SubContracts[" + i + "].PRSubmissionClose");
                ModelState.Remove("SubContracts[" + i + "].NOAIssuance");
                ModelState.Remove("SubContracts[" + i + "].NTPIssuance");
            }

            if (LotContract.ModeOfProcurementReference != 1)
            {
                ModelState.Remove("PreBidConference");
            }
            if (ModelState.IsValid)
            {
                return Json(new { result = contractDAL.PostLotContract(LotContract, User.Identity.Name) });
            }

            ViewBag.ProjectCoordinator = new SelectList(contractDAL.GetProcurementEmployees(), "EmployeeCode", "EmployeeName");
            return PartialView("_CreateLotContract", LotContract);
        }

        public ActionResult GetModesOfProcurement(string PAPCode)
        {
            var modes = contractDAL.GetModesOfProcurement(PAPCode).Where(d => d.ID != 1 && d.ID != 10).ToList();
            return Json(modes, JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                contractDAL.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}