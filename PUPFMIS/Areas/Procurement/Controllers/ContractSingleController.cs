using PUPFMIS.BusinessAndDataLogic;
using PUPFMIS.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace PUPFMIS.Areas.Procurement.Controllers
{
    [Route("{action}")]
    [RouteArea("procurement")]
    [RoutePrefix("projects/contracts")]
    [Authorize(Roles = SystemRoles.ProjectCoordinator)]
    public class ContractSingleController : Controller
    {
        ContractSingleDAL contractDAL = new ContractSingleDAL();
        ContractSingleBL contractBL = new ContractSingleBL();
        ContractsDAL ctrctDAL = new ContractsDAL();
        SuppliersBL suppliersBL = new SuppliersBL();

        [ActionName("details")]
        [Route("{ContractCode}/details")]
        public ActionResult Details(string ContractCode)
        {
            var ProcurementProjectStage = contractDAL.GetProcurementProjectStage(ContractCode);
            if(ProcurementProjectStage == ProcurementProjectStages.PRSubmissionClosed)
            {
                return PurchaseRequestSubmissionClosing(ContractCode);
            }
            else if (ProcurementProjectStage == ProcurementProjectStages.PreProcurementConferenceSetup)
            {
                return PreProcurementConferenceSetup(ContractCode);
            }
            else if (ProcurementProjectStage == ProcurementProjectStages.PreProcurementConferenceUpdate)
            {
                return PreBidConferenceSetup(ContractCode);
            }
            else if (ProcurementProjectStage == ProcurementProjectStages.PreBidConferenceSetup)
            {
                return PreBidConferenceUpdate(ContractCode);
            }
            else if (ProcurementProjectStage == ProcurementProjectStages.PreBidConferenceUpdate)
            {
                return OpeningOfBidsUpdate(ContractCode);
            }
            else if (ProcurementProjectStage == ProcurementProjectStages.BidsOpened)
            {
                return EvaluationOfBidsUpdate(ContractCode);
            }
            else if (ProcurementProjectStage == ProcurementProjectStages.BidsEvaluated)
            {
                return PostQualificationUpdate(ContractCode);
            }
            else if (ProcurementProjectStage == ProcurementProjectStages.PostQualification)
            {
                var strategy = contractDAL.GetContractStrategy(ContractCode);
                if(strategy == ContractStrategies.BulkBidding)
                {
                    return NoticeOfAwardSetupSingle(ContractCode);
                }
                else
                {
                    return NoticeOfAwardSetupLine(ContractCode);
                }
            }
            else if (ProcurementProjectStage == ProcurementProjectStages.NoticeOfAwardSetup)
            {
                return NoticeOfAwardUpdate(ContractCode);
            }
            else if (ProcurementProjectStage == ProcurementProjectStages.NoticeOfAwardUpdate)
            {
                return NoticeToProceedSetup(ContractCode);
            }
            else if (ProcurementProjectStage == ProcurementProjectStages.NoticeToProceedSetup)
            {
                return NoticeToProceedUpdate(ContractCode);
            }
            else
            {
                return ProjectConclusion(ContractCode);
            }
        }
        public ActionResult PurchaseRequestSubmissionClosing(string ContractCode)
        {
            var contract = contractDAL.GetPurchaseRequestSubmissionClosing(ContractCode);
            return View(contract.ProcurementProjectStage.ToString(), contract);
        }
        public ActionResult PreProcurementConferenceSetup(string ContractCode)
        {
            var contract = contractDAL.GetPreProcurementConferenceUpdate(ContractCode);
            return View(contract.ProcurementProjectStage.ToString(), contract);
        }
        public ActionResult PreBidConferenceSetup(string ContractCode)
        {
            var contract = contractDAL.GetPreBidConferenceSetup(ContractCode);
            return View(contract.ProcurementProjectStage.ToString(), contract);
        }
        public ActionResult PreBidConferenceUpdate(string ContractCode)
        {
            var contract = contractDAL.GetPreBidConferenceUpdate(ContractCode);
            return View(contract.ProcurementProjectStage.ToString(), contract);
        }
        public ActionResult OpeningOfBidsUpdate(string ContractCode)
        {
            var contract = contractDAL.GetOpeningOfBids(ContractCode);
            return View(contract.ProcurementProjectStage.ToString(), contract);
        }
        public ActionResult EvaluationOfBidsUpdate(string ContractCode)
        {
            var contract = contractDAL.GetEvaluationOfBids(ContractCode);
            return View(contract.ProcurementProjectStage.ToString(), contract);
        }
        public ActionResult PostQualificationUpdate(string ContractCode)
        {
            var contract = contractDAL.GetPostQualificationUpdate(ContractCode);
            ViewBag.Supplier = new SelectList(suppliersBL.GetActiveSuppliers().Where(d => d.ID != 1).ToList(), "ID", "SupplierName");
            return View(contract.ProcurementProjectStage.ToString(), contract);
        }
        public ActionResult NoticeOfAwardSetupSingle(string ContractCode)
        {
            var contract = contractDAL.GetNoticeOfAwardSetup(ContractCode);
            var classification = contractDAL.GetContractClassification(ContractCode);
            var suppliers = suppliersBL.GetActiveSuppliers().Where(d => d.ID != 1).ToList();
            var contractTypes = Enum.GetValues(typeof(ContractTypes)).Cast<ContractTypes>()
                                           .Where(d => classification.Contains("Goods") ? d == ContractTypes.PurchaseOrder : d > ContractTypes.PurchaseOrder)
                                           .Select(d => new SelectListItem
                                           {
                                               Value = ((int)d).ToString(),
                                               Text = d.GetAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>().Name
                                           }).ToList();
            ViewBag.ContractType = new SelectList(contractTypes, "Value", "Text");
            ViewBag.Supplier = new SelectList(suppliers, "ID", "SupplierName");
            return View(contract.ProcurementProjectStage.ToString() + "Single", contract);
        }
        public ActionResult NoticeOfAwardSetupLine(string ContractCode)
        {
            var contract = contractDAL.GetNoticeOfAwardLineSetup(ContractCode);
            var classification = contractDAL.GetContractClassification(ContractCode);
            var suppliers = suppliersBL.GetActiveSuppliers().Where(d => d.ID != 1).ToList();
            var contractTypes = Enum.GetValues(typeof(ContractTypes)).Cast<ContractTypes>()
                                           .Where(d => classification.Contains("Goods") ? d == ContractTypes.PurchaseOrder : d > ContractTypes.PurchaseOrder)
                                           .Select(d => new SelectListItem
                                           {
                                               Value = ((int)d).ToString(),
                                               Text = d.GetAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>().Name
                                           }).ToList();
            ViewBag.ContractType = new SelectList(contractTypes, "Value", "Text");
            ViewBag.Supplier = new SelectList(suppliers, "ID", "SupplierName");
            return View(contract.ProcurementProjectStage.ToString() + "Line", contract);
        }
        public ActionResult NoticeOfAwardUpdate(string ContractCode)
        {
            var contract = contractDAL.GetNoticeOfAwardUpdate(ContractCode);
            return View(contract.ProcurementProjectStage.ToString(), contract);
        }
        public ActionResult NoticeToProceedSetup(string ContractCode)
        {
            var contract = contractDAL.GetNoticeToProceedSetup(ContractCode);
            return View(contract.ProcurementProjectStage.ToString(), contract);
        }
        public ActionResult NoticeToProceedUpdate(string ContractCode)
        {
            var contract = contractDAL.GetNoticeToProceedUpdate(ContractCode);
            return View(contract.ProcurementProjectStage.ToString(), contract);
        }
        public ActionResult ProjectConclusion(string ContractCode)
        {
            var contract = contractDAL.GetSingleContractDetails(ContractCode);
            return View("ProjectConclusion", contract);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("pr-closed")]
        [ActionName("PRSubmissionClosed")]
        public ActionResult PRSubmissionClosed(PreProcurementConferenceSetupVM ContractVM)
        {
            if (ModelState.IsValid)
            {
                return Json(new { result = contractDAL.PostPreProcurementConferenceSetup(ContractVM, User.Identity.Name), JsonRequestBehavior.AllowGet });
            }
            return View(ContractVM.ProcurementProjectStage.ToString(), ContractVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("PreProcurementConferenceSetup")]
        public ActionResult PreProcurementConferenceUpdate(PreProcurementConferenceUpdateVM Contract)
        {
            if (ModelState.IsValid)
            {
                return Json(new { result = contractDAL.PostPreProcurementConferenceUpdate(Contract, User.Identity.Name), JsonRequestBehavior.AllowGet });
            }
            return View(Contract.ProcurementProjectStage.ToString(), Contract);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("PreProcurementConferenceUpdate")]
        public ActionResult PreBidConferenceSetup(PreBidConferenceSetupVM Contract)
        {
            if (Contract.ProcurementProjectType == ProcurementProjectTypes.AMP)
            {
                ModelState.Remove("PreBidConference");
                ModelState.Remove("PreBidConferenceLocation");
                ModelState.Remove("PreBidVideoConferencingOptions");
                ModelState.Remove("PreBidVideoConferenceMode");
                ModelState.Remove("PreBidVideoConferenceAccessRequestEmail");
                ModelState.Remove("PreBidVideoConferenceAccessRequestContactNo");
                ModelState.Remove("PreBidAdditionalInstructions");
                ModelState.Remove("BidDocumentPrice");
            }

            if (ModelState.IsValid)
            {
                return Json(new { result = contractDAL.PostPreBidConferenceSetup(Contract, User.Identity.Name), JsonRequestBehavior.AllowGet });
            }
            return View(Contract.ProcurementProjectStage.ToString(), Contract);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("PreBidConferenceSetup")]
        public ActionResult PreBidConferenceUpdate(PreBidConferenceUpdateVM Contract)
        {
            if (ModelState.IsValid)
            {
                return Json(new { result = contractDAL.PostPreBidConferenceUpdate(Contract, User.Identity.Name), JsonRequestBehavior.AllowGet });
            }
            return View(Contract.ProcurementProjectStage.ToString(), Contract);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("PreBidConferenceUpdate")]
        public ActionResult BidsOpened(OpeningOfBidsUpdateVM Contract)
        {
            if (Contract.FailureOfBiddingDeclared == false)
            {
                ModelState.Remove("OpeningOfBidsFailureReason");
            }

            if (ModelState.IsValid)
            {
                return Json(new { result = contractDAL.PostOpeningOfBidsUpdate(Contract, User.Identity.Name), JsonRequestBehavior.AllowGet });
            }
            return View(Contract.ProcurementProjectStage.ToString(), Contract);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("BidsOpened")]
        public ActionResult BidsEvaluated(EvaluationOfBidsUpdateVM Contract)
        {
            if (ModelState.IsValid)
            {
                return Json(new { result = contractDAL.PostEvaluationOfBidsUpdate(Contract, User.Identity.Name), JsonRequestBehavior.AllowGet });
            }
            return View(Contract.ProcurementProjectStage.ToString(), Contract);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("BidsEvaluated")]
        public ActionResult PostQualificationUpdate(PostQualificationUpdateVM Contract)
        {
            if (Contract.FailureOfBiddingDeclared == false)
            {
                ModelState.Remove("PostQualificationFailureReason");
            }

            if (ModelState.IsValid)
            {
                return Json(new { result = contractDAL.PostPostQualificationUpdate(Contract, User.Identity.Name), JsonRequestBehavior.AllowGet });
            }
            return View(Contract.ProcurementProjectStage.ToString(), Contract);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("PostQualification")]
        public ActionResult NoticeOfAwardSetup(NoticeOfAwardSetupVM Contract)
        {
            for (int i = 0; i < Contract.BidDetails.Count; i++)
            {
                Contract.BidDetails[i].BidTotalPrice = Math.Round((Contract.BidDetails[i].Quantity * Contract.BidDetails[i].BidUnitPrice), 2);
            }

            if (ModelState.IsValid)
            {
                return Json(new { result = contractDAL.PostNoticeOfAwardSetup(Contract, User.Identity.Name), JsonRequestBehavior.AllowGet });
            }
            return View(Contract.ProcurementProjectStage.ToString(), Contract);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("PostQualificationLine")]
        public ActionResult NoticeOfAwardSetupLine(NoticeOfAwardSetupLineItemsVM Contract)
        {
            for (int i = 0; i < Contract.LineItems.Count; i++)
            {
                Contract.LineItems[i].BidTotalPrice = Math.Round((Contract.LineItems[i].Quantity * Contract.LineItems[i].BidUnitPrice), 2);
            }

            if (ModelState.IsValid)
            {
                return Json(new { result = contractDAL.PostNoticeOfAwardSetupLine(Contract, User.Identity.Name), JsonRequestBehavior.AllowGet });
            }
            return View(Contract.ProcurementProjectStage.ToString(), Contract);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("NoticeOfAwardSetup")]
        public ActionResult NoticeOfAwardUpdate(NoticeOfAwardUpdateVM Contract)
        {
            if (ModelState.IsValid)
            {
                return Json(new { result = contractDAL.PostNoticeOfAwardUpdate(Contract, User.Identity.Name), JsonRequestBehavior.AllowGet });
            }
            return View(Contract.ProcurementProjectStage.ToString(), Contract);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("NoticeOfAwardUpdate")]
        public ActionResult PostNoticeToProceedSetup(NoticeToProceedSetupVM Contract)
        {
            if (ModelState.IsValid)
            {
                return Json(new { result = contractDAL.PostNoticeToProceedSetup(Contract, User.Identity.Name), JsonRequestBehavior.AllowGet });
            }
            return View(Contract.ProcurementProjectStage.ToString(), Contract);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("NoticeToProceedSetup")]
        public ActionResult PostNoticeToProceedUpdate(NoticeToProceedUpdateVM Contract)
        {
            if (ModelState.IsValid)
            {
                return Json(new { result = contractDAL.PostNoticeToProceedUpdate(Contract, User.Identity.Name), JsonRequestBehavior.AllowGet });
            }
            return View(Contract.ProcurementProjectStage.ToString(), Contract);
        }

        [Route("{ContractCode}/pre-procurement-conference/memo/print")]
        [ActionName("print-preprocurement-memo")]
        public ActionResult PrintSubmissionMemo(string ContractCode)
        {
            var stream = contractBL.GeneratePreProcurementMemo(ContractCode, Server.MapPath("~/Content/imgs/PUPLogo.png"), User.Identity.Name);
            Response.Clear();
            Response.ClearContent();
            Response.ClearHeaders();
            Response.AddHeader("content-length", stream.Length.ToString());
            Response.ContentType = "application/pdf";
            Response.BinaryWrite(stream.ToArray());
            stream.Close();
            Response.End();

            return RedirectToAction("dashboard");
        }

        [Route("{ContractCode}/invitation-to-bid/print")]
        [ActionName("print-invitation-to-bid")]
        public ActionResult PrintInvitationToBid(string ContractCode)
        {
            var stream = contractBL.GenerateInvitationToBid(ContractCode, Server.MapPath("~/Content/imgs/PUPLogo.png"), User.Identity.Name);
            Response.Clear();
            Response.ClearContent();
            Response.ClearHeaders();
            Response.AddHeader("content-length", stream.Length.ToString());
            Response.AppendHeader("Content-Disposition", "inline; filename=Invitation To Bid - " + ContractCode + ".pdf");
            Response.ContentType = "application/pdf";
            Response.BinaryWrite(stream.ToArray());
            stream.Close();
            Response.End();

            return RedirectToAction("dashboard");
        }

        [Route("{ContractCode}/request-for-quotation/print")]
        [ActionName("print-request-for-quotation")]
        public ActionResult PrintRequestForQuotation(string ContractCode)
        {
            var stream = contractBL.GenerateRequestForQuotation(Server.MapPath("~/Content/imgs/PUPLogo.png"), ContractCode);
            Response.Clear();
            Response.ClearContent();
            Response.ClearHeaders();
            Response.AddHeader("content-length", stream.Length.ToString());
            Response.AppendHeader("Content-Disposition", "inline; filename=Invitation To Bid - " + ContractCode + ".pdf");
            Response.ContentType = "application/pdf";
            Response.BinaryWrite(stream.ToArray());
            stream.Close();
            Response.End();

            return RedirectToAction("dashboard");
        }

        [Route("{ContractCode}/notice-of-award/print")]
        [ActionName("print-notice-of-award")]
        public ActionResult PrintNoticeOfAward(string ContractCode)
        {
            var stream = contractBL.GenerateNoticeOfAward(ContractCode, Server.MapPath("~/Content/imgs/PUPLogo.png"), User.Identity.Name);
            Response.Clear();
            Response.ClearContent();
            Response.ClearHeaders();
            Response.AddHeader("content-length", stream.Length.ToString());
            Response.AppendHeader("Content-Disposition", "inline; filename=Invitation To Bid - " + ContractCode + ".pdf");
            Response.ContentType = "application/pdf";
            Response.BinaryWrite(stream.ToArray());
            stream.Close();
            Response.End();

            return RedirectToAction("dashboard");
        }

        [Route("{ContractCode}/contract-agreement-form/print")]
        [ActionName("print-contract-agreement-form")]
        public ActionResult PrintContractAgreementForm(string ContractCode)
        {
            var classification = contractDAL.GetContractClassification(ContractCode);
            var stream = classification == "Goods" ? contractBL.GenerateContractAgreementFormGoods(ContractCode, Server.MapPath("~/Content/imgs/PUPLogo.png"), User.Identity.Name) :
                         classification == "Services" ? contractBL.GenerateNoticeOfAward(ContractCode, Server.MapPath("~/Content/imgs/PUPLogo.png"), User.Identity.Name) :
                         classification == "Consultancy" ? contractBL.GenerateNoticeOfAward(ContractCode, Server.MapPath("~/Content/imgs/PUPLogo.png"), User.Identity.Name) :
                         contractBL.GenerateNoticeOfAward(ContractCode, Server.MapPath("~/Content/imgs/PUPLogo.png"), User.Identity.Name);
            Response.Clear();
            Response.ClearContent();
            Response.ClearHeaders();
            Response.AddHeader("content-length", stream.Length.ToString());
            Response.AppendHeader("Content-Disposition", "inline; filename=Invitation To Bid - " + ContractCode + ".pdf");
            Response.ContentType = "application/pdf";
            Response.BinaryWrite(stream.ToArray());
            stream.Close();
            Response.End();

            return RedirectToAction("dashboard");
        }

        [Route("{ContractCode}/purchase-order/print")]
        [ActionName("print-purchase-order")]
        public ActionResult PrintPurchaseOrder(string ContractCode)
        {
            var stream = contractBL.GeneratePurchaseOrder(ContractCode, Server.MapPath("~/Content/imgs/PUPLogo.png"), User.Identity.Name);
            Response.Clear();
            Response.ClearContent();
            Response.ClearHeaders();
            Response.AddHeader("content-length", stream.Length.ToString());
            Response.AppendHeader("Content-Disposition", "inline; filename=Invitation To Bid - " + ContractCode + ".pdf");
            Response.ContentType = "application/pdf";
            Response.BinaryWrite(stream.ToArray());
            stream.Close();
            Response.End();

            return RedirectToAction("dashboard");
        }

        [Route("{ContractCode}/notice-to-proceed/print")]
        [ActionName("print-notice-to-proceed")]
        public ActionResult PrintNoticeToProceed(string ContractCode)
        {
            var stream = contractBL.GenerateNoticeToProceed(ContractCode, Server.MapPath("~/Content/imgs/PUPLogo.png"), User.Identity.Name);
            Response.Clear();
            Response.ClearContent();
            Response.ClearHeaders();
            Response.AddHeader("content-length", stream.Length.ToString());
            Response.AppendHeader("Content-Disposition", "inline; filename=Invitation To Bid - " + ContractCode + ".pdf");
            Response.ContentType = "application/pdf";
            Response.BinaryWrite(stream.ToArray());
            stream.Close();
            Response.End();

            return RedirectToAction("dashboard");
        }

        public ActionResult GetSupplierDetails(int ID)
        {
            var supplier = suppliersBL.GetActiveSuppliers().Where(d => d.ID == ID).FirstOrDefault();
            supplier.Address = supplier.Address + (supplier.City == string.Empty ? string.Empty : ", " + supplier.City) + (supplier.State == string.Empty ? string.Empty : ", " + supplier.State) + (supplier.PostalCode == string.Empty ? string.Empty : ", " + supplier.PostalCode);
            return Json(supplier, JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                contractBL.Dispose();
                contractDAL.Dispose();
                suppliersBL.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}