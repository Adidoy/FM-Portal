using PUPFMIS.Models;
using PUPFMIS.Models.AIS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace PUPFMIS.BusinessAndDataLogic
{
    public class PPMPApprovalBL : Controller
    {
        private PPMPApprovalDAL ppmpApprovalDAL = new PPMPApprovalDAL();
        private ProjectProcurementManagementPlanBL ppmpBL = new ProjectProcurementManagementPlanBL();

        //public MemoryStream ViewPPMP(string ReferenceNo, string LogoPath, string UserEmail)
        //{
        //    return ppmpBL.GeneratePPMPReport(ReferenceNo, LogoPath, UserEmail);
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                ppmpApprovalDAL.Dispose();
            }
            base.Dispose(disposing);
        }
    }

    public class PPMPApprovalDAL : Controller
    {
        private FMISDbContext db = new FMISDbContext();
        private HRISDataAccess hris = new HRISDataAccess();
        private ABISDataAccess abis = new ABISDataAccess();

        public List<int> GetFiscalYears()
        {
            return db.PPMPHeader.Select(d => d.FiscalYear).Distinct().OrderBy(d => d).ToList();
        }
        public List<FundSources> GetFundSources()
        {
            return abis.GetFundSources();
        }
        public List<PPMPOfficeListVM> GetOffices(int FiscalYear)
        {
            return db.PPMPHeader.ToList()
                     .Where(d => d.FiscalYear == FiscalYear && d.PPMPStatus == PPMPStatus.ForwardedToBudgetOffice)
                     .GroupBy(d => new { d.Department, d.FiscalYear })
                     .Select(d => new PPMPOfficeListVM
                     {
                         DepartmentCode = d.Key.Department,
                         Department = hris.GetDepartmentDetails(d.Key.Department).Department,
                         EstimatedBudget = db.PPMPDetails.Where(x => x.FKPPMPHeaderReference.FiscalYear == FiscalYear && x.FKPPMPHeaderReference.PPMPStatus == PPMPStatus.ForwardedToBudgetOffice && x.ArticleReference != null).Sum(x => x.EstimatedBudget),
                         FiscalYear = d.Key.FiscalYear
                     }).ToList();
        }

        public List<PPMPListVM> GetPPMPs(int FiscalYear, string Department)
        {
            return db.PPMPHeader.Where(d => d.FiscalYear == FiscalYear && d.Department == Department).ToList()
                                .Select(d => new PPMPListVM
                                {
                                    ReferenceNo = d.ReferenceNo,
                                    PPMPType = d.PPMPType,
                                    PPMPStatus = d.PPMPStatus,
                                    UACS = d.UACS,
                                    ObjectClassification = abis.GetChartOfAccounts(d.UACS).AcctName,
                                    EstimatedBudget = db.PPMPDetails.Where(x => x.PPMPHeaderReference == d.ID && x.ArticleReference != null).Sum(x => x.EstimatedBudget)
                                }).OrderBy(d => d.PPMPType).ThenBy(d => d.UACS).ToList();
        }
        public PPMPEvaluationVM GetEvaluationDetails(string ReferenceNo)
        {
            var system = new SystemBDL();
            var ppmp = db.PPMPHeader.Where(d => d.ReferenceNo == ReferenceNo).FirstOrDefault();
            var ppmpProjects = db.PPMPDetails.Where(d => d.PPMPHeaderReference == ppmp.ID && (d.PPMPDetailStatus == PPMPDetailStatus.ForEvaluation || d.PPMPDetailStatus == PPMPDetailStatus.ItemRevised)).ToList();
            var projectDetails = db.ProjectDetails.Where(d => d.FKProjectPlanReference.FiscalYear == ppmp.FiscalYear &&
                                                              d.FKProjectPlanReference.Department == ppmp.Department &&
                                                              d.FKItemArticleReference.UACSObjectClass == ppmp.UACS &&
                                                              d.ProjectItemStatus == ProjectDetailsStatus.PostedToProcurementProject).ToList();
            var projects = db.ProjectPlans.ToList().Where(d => ppmpProjects.Select(x => x.FKProjectDetailsReference.FKProjectPlanReference.ProjectCode).Distinct().Contains(d.ProjectCode))
                                          .Select(d => new PPMPProjectsVM
                                          {
                                              PAPCode = d.PAPCode,
                                              Program = abis.GetPrograms(d.PAPCode).GeneralDescription,
                                              ProjectCode = d.ProjectCode,
                                              ProjectName = d.ProjectName,
                                              Description = d.Description,
                                              DepatmentCode = d.Unit,
                                              UnitName = hris.GetDepartmentDetails(d.Unit).Section,
                                              DeliveryMonth = system.GetMonthName(d.DeliveryMonth),
                                              Details = ppmpProjects.Where(x => x.FKProjectDetailsReference.FKProjectPlanReference.ProjectCode == d.ProjectCode && x.ArticleReference != null &&
                                                                                x.BudgetOfficeAction == null || x.BudgetOfficeAction == BudgetOfficeAction.ForRevision).ToList()
                                                                      .Select(x => new PPMPProjectDetailsVM
                                                                      {
                                                                          PPMPDetailID = x.ID,
                                                                          ItemCode = x.ArticleReference == null ? null : x.FKItemArticleReference.ArticleCode + "-" + x.ItemSequence,
                                                                          ItemFullName = x.ItemFullName,
                                                                          ItemSpecifications = x.ItemSpecifications,
                                                                          Justification = x.Justification,
                                                                          ProposalType = x.ProposalType,
                                                                          ProcurementSource = x.ProcurementSource,
                                                                          JanQty = x.JanQty,
                                                                          FebQty = x.FebQty,
                                                                          MarQty = x.MarQty,
                                                                          AprQty = x.AprQty,
                                                                          MayQty = x.MayQty,
                                                                          JunQty = x.JunQty,
                                                                          JulQty = x.JulQty,
                                                                          AugQty = x.AugQty,
                                                                          SepQty = x.SepQty,
                                                                          OctQty = x.OctQty,
                                                                          NovQty = x.NovQty,
                                                                          DecQty = x.DecQty,
                                                                          TotalQty = x.TotalQty,
                                                                      }).OrderBy(x => x.ItemFullName).ToList()
                                          }).OrderBy(d => d.PAPCode).ThenBy(d => d.ProjectCode).ToList();

            return new PPMPEvaluationVM
            {
                ReferenceNo = ppmp.ReferenceNo,
                PPMPType = ppmp.PPMPType.GetAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>().Name,
                FiscalYear = ppmp.FiscalYear,
                UACS = ppmp.UACS,
                AccountTitle = abis.GetChartOfAccounts(ppmp.UACS).AcctName,
                DepartmentCode = ppmp.Department,
                Department = hris.GetDepartmentDetails(ppmp.Department).Section,
                Sector = hris.GetDepartmentDetails(ppmp.Sector).Section,
                Projects = projects
            };
        }
        public bool EvaluatePPMP(PPMPEvaluationVM EvaluationVM)
        {
            var ppmp = db.PPMPHeader.Where(d => d.ReferenceNo == EvaluationVM.ReferenceNo).FirstOrDefault();
            var details = EvaluationVM.Projects.SelectMany(d => d.Details).ToList();
            if (ppmp.UACS == "1060401000")
            {
                var ppmpDetail = db.PPMPDetails.Where(d => d.FKPPMPHeaderReference.ID == ppmp.ID && d.UACS == "1060401000").ToList();
                if (details.FirstOrDefault().BudgetOfficeAction == BudgetOfficeAction.Accepted)
                {
                    ppmpDetail.ForEach(d =>
                    {
                        d.BudgetOfficeAction = BudgetOfficeAction.Accepted;
                        d.FundSource = details.FirstOrDefault().FundSource;
                        d.PPMPDetailStatus = PPMPDetailStatus.ItemAccepted;
                        d.FKProjectDetailsReference.ProjectItemStatus = ProjectDetailsStatus.Approved;
                    });
                }
                else
                {
                    ppmpDetail.ForEach(d =>
                    {
                        d.BudgetOfficeAction = details.FirstOrDefault().BudgetOfficeAction;
                        d.UpdateFlag = details.FirstOrDefault().BudgetOfficeAction == BudgetOfficeAction.ForRevision ? true : false;
                        d.BudgetOfficeReasonForNonAcceptance = details.FirstOrDefault().ReasonForNonAcceptance;
                        d.PPMPDetailStatus = details.FirstOrDefault().BudgetOfficeAction == BudgetOfficeAction.ForRevision ? PPMPDetailStatus.ForRevision : PPMPDetailStatus.ItemNotAccepted;
                        d.FKProjectDetailsReference.ProjectItemStatus = details.FirstOrDefault().BudgetOfficeAction == BudgetOfficeAction.ForRevision ? ProjectDetailsStatus.ForRevisionFromBudget : ProjectDetailsStatus.Disapproved;
                    });
                }

                if (db.SaveChanges() == 0)
                {
                    return false;
                }
            }
            else
            {
                foreach (var item in details)
                {
                    var itemRecord = db.Items.ToList().Where(d => d.ItemCode == item.ItemCode).FirstOrDefault();
                    var ppmpDetail = db.PPMPDetails.Where(d => d.ID == item.PPMPDetailID && d.ArticleReference == itemRecord.ArticleReference && d.ItemSequence == itemRecord.Sequence).FirstOrDefault();
                    if (item.BudgetOfficeAction == BudgetOfficeAction.Accepted)
                    {
                        ppmpDetail.BudgetOfficeAction = BudgetOfficeAction.Accepted;
                        ppmpDetail.FundSource = item.FundSource;
                        ppmpDetail.PPMPDetailStatus = PPMPDetailStatus.ItemAccepted;
                        ppmpDetail.FKProjectDetailsReference.ProjectItemStatus = ProjectDetailsStatus.Approved;
                    }
                    else
                    {
                        ppmpDetail.BudgetOfficeAction = item.BudgetOfficeAction;
                        ppmpDetail.UpdateFlag = item.BudgetOfficeAction == BudgetOfficeAction.ForRevision ? true : false;
                        ppmpDetail.BudgetOfficeReasonForNonAcceptance = item.ReasonForNonAcceptance;
                        ppmpDetail.PPMPDetailStatus = item.BudgetOfficeAction == BudgetOfficeAction.ForRevision ? PPMPDetailStatus.ForRevision : PPMPDetailStatus.ItemNotAccepted;
                        ppmpDetail.FKProjectDetailsReference.ProjectItemStatus = item.BudgetOfficeAction == BudgetOfficeAction.ForRevision ? ProjectDetailsStatus.ForRevisionFromBudget : ProjectDetailsStatus.Disapproved;
                    }

                    if (db.SaveChanges() == 0)
                    {
                        return false;
                    }
                }
            }


            var ppmpDetails = db.PPMPDetails.Where(d => d.PPMPHeaderReference == ppmp.ID).ToList();
            if (ppmpDetails.Where(d => d.BudgetOfficeAction == BudgetOfficeAction.ForRevision).Count() >= 1)
            {
                ppmp.PPMPStatus = PPMPStatus.ReturnedForRevision;
            }
            else
            {
                ppmp.PPMPStatus = PPMPStatus.EvaluatedByBudgetOffice;
            }

            if (db.SaveChanges() == 0)
            {
                return false;
            }

            UpdateProjectStatus(ppmp.ReferenceNo);

            return true;
        }
        private void UpdateProjectStatus(string ReferenceNo)
        {
            var projectCodes = db.PPMPDetails.Where(d => d.FKPPMPHeaderReference.ReferenceNo == ReferenceNo)
                                             .Select(d => d.FKProjectDetailsReference.FKProjectPlanReference.ProjectCode).Distinct().ToList();
            var forApprovalCount = db.ProjectDetails.Where(d => projectCodes.Contains(d.FKProjectPlanReference.ProjectCode) &&
                                                                d.ProjectItemStatus == ProjectDetailsStatus.ForApproval).Count();
            var forRevisionCount = db.ProjectDetails.Where(d => projectCodes.Contains(d.FKProjectPlanReference.ProjectCode) &&
                                                                d.ProjectItemStatus == ProjectDetailsStatus.ForRevisionFromBudget).Count();

            if (forApprovalCount == 0 && forRevisionCount == 0)
            {
                var projects = db.ProjectPlans.Where(d => projectCodes.Contains(d.ProjectCode)).ToList();
                projects.ForEach(d => { d.ProjectStatus = ProjectStatus.EvaluatedByBudgetOffice; });
                db.SaveChanges();
            }
        }
        //public List<PPMPLineItemsPerAccountVM> GetPPMPLineItemsPerAccount(string DepartmentCode)
        //{
        //    var office = hrisDataAccess.GetFullDepartmentDetails(DepartmentCode);
        //    var accounts = abisDataAccess.GetDetailedChartOfAccounts();
        //    var ppmpItems = db.ProjectPlanItems.Where(d => (office.SectionCode == null ? d.FKPPMPReference.Department == office.DepartmentCode : d.FKPPMPReference.Department == office.SectionCode) && d.FKPPMPReference.Status == "PPMP Submitted" && d.Status == "Posted").ToList();
        //    var ppmpService = db.ProjectPlanServices.Where(d => (office.SectionCode == null ? d.FKPPMPReference.Department == office.DepartmentCode : d.FKPPMPReference.Department == office.SectionCode) && d.FKPPMPReference.Status == "PPMP Submitted" && d.Status == "Posted").ToList();

        //    //var itemAccountsList = (from ppmpLineItems in ppmpItems
        //    //                        join accountsList in accounts
        //    //                        on  ppmpLineItems.FKItemReference.FKItemTypeReference.UACSObjectClass equals accountsList.UACS_Code
        //    //                        select new
        //    //                        {
        //    //                            UACS = accountsList.UACS_Code,
        //    //                            ObjectClassification = accountsList.AcctName,
        //    //                            EstimatedBudget = ppmpLineItems.ProjectEstimatedBudget
        //    //                        }).ToList();

        //    //var serviceAccountsList = (from ppmpServiceList in ppmpService
        //    //                        join accountsList in accounts
        //    //                        on ppmpServiceList.FKItemReference.FKItemTypeReference.UACSObjectClass equals accountsList.UACS_Code
        //    //                        select new
        //    //                        {
        //    //                            UACS = accountsList.UACS_Code,
        //    //                            ObjectClassification = accountsList.AcctName,
        //    //                            EstimatedBudget = ppmpServiceList.ProjectEstimatedBudget
        //    //                        }).ToList();

        //    //var lineItemsPerAccount = itemAccountsList.Union(serviceAccountsList)
        //    //                          .GroupBy(d => new { d.UACS, d.ObjectClassification })
        //    //                          .Select(d => new PPMPLineItemsPerAccountVM
        //    //                          {
        //    //                              UACS = d.Key.UACS,
        //    //                              ObjectClassification = d.Key.ObjectClassification,
        //    //                              EstimatedBudget = d.Sum(x => x.EstimatedBudget)
        //    //                          }).ToList();

        //    //return lineItemsPerAccount;
        //    return new List<PPMPLineItemsPerAccountVM>();
        //}
        //public List<AccountLineItem> GetNewSpendingItems(string UACS, string DepartmentCode, int FiscalYear)
        //{
        //    //var ppmpItemDetails = (from ppmpItems in db.ProjectPlanItems
        //    //                       join projectItems in db.ProjectPlanItems
        //    //                       on new { ProjectReference = ppmpItems.ProjectReference,  ItemReference = ppmpItems.ItemReference } equals new { ProjectReference = projectItems.ProjectReference, ItemReference = projectItems.ItemReference }
        //    //                       where ppmpItems.FKPPMPReference.FiscalYear == FiscalYear && ppmpItems.FKPPMPReference.Department == DepartmentCode && ppmpItems.FKItemReference.FKItemTypeReference.UACSObjectClass == UACS && projectItems.ProposalType == BudgetProposalType.NewProposal && ppmpItems.Status == "Posted"
        //    //                       select new AccountLineItem
        //    //                       {
        //    //                           ApprovalAction = "Approved",
        //    //                           ProjectCode = ppmpItems.FKProjectReference.ProjectCode,
        //    //                           ReferenceNo = ppmpItems.FKPPMPReference.ReferenceNo,
        //    //                           ProjectTitle = ppmpItems.FKProjectReference.ProjectName,
        //    //                           ProposalType = projectItems.ProposalType,
        //    //                           ItemCode = ppmpItems.FKItemReference.ItemCode,
        //    //                           ItemName = ppmpItems.FKItemReference.ItemFullName.ToUpper() + " (" + ppmpItems.FKItemReference.ItemCode + ")",
        //    //                           ItemSpecifications = ppmpItems.FKItemReference.ItemSpecifications,
        //    //                           UnitOfMeasure = ppmpItems.FKItemReference.FKIndividualUnitReference.UnitName.ToUpper(),
        //    //                           UnitCost = ppmpItems.UnitCost,
        //    //                           Quantity = ppmpItems.ProjectTotalQty,
        //    //                           ReducedQuantity = ppmpItems.ProjectTotalQty,
        //    //                           EstimatedCost = ppmpItems.ProjectEstimatedBudget,
        //    //                           Remarks = ppmpItems.Justification
        //    //                       }).ToList();

        //    //var ppmpServiceDetails = (from ppmpService in db.ProjectPlanServices
        //    //                          join projectService in db.ProjectPlanServices
        //    //                          on new { ProjectReference = ppmpService.ProjectReference, ServiceReference = ppmpService.ItemReference } equals new { ProjectReference = projectService.ProjectReference, ServiceReference = projectService.ItemReference }
        //    //                          where ppmpService.FKPPMPReference.FiscalYear == FiscalYear && ppmpService.FKPPMPReference.Department == DepartmentCode && ppmpService.FKItemReference.FKItemTypeReference.UACSObjectClass == UACS && projectService.ProposalType == BudgetProposalType.NewProposal && ppmpService.Status == "Posted"
        //    //                          select new AccountLineItem
        //    //                          {
        //    //                              ApprovalAction = "Approved",
        //    //                              ProjectCode = ppmpService.FKProjectReference.ProjectCode,
        //    //                              ReferenceNo = ppmpService.FKPPMPReference.ReferenceNo,
        //    //                              ProjectTitle = ppmpService.FKProjectReference.ProjectName,
        //    //                              ProposalType = projectService.ProposalType,
        //    //                              ItemCode = ppmpService.FKItemReference.ItemCode,
        //    //                              ItemName = ppmpService.FKItemReference.ItemFullName + " (" + ppmpService.FKItemReference.ItemCode + ")",
        //    //                              ItemSpecifications = ppmpService.ItemSpecifications,
        //    //                              UnitOfMeasure = "",
        //    //                              UnitCost = ppmpService.UnitCost,
        //    //                              Quantity = ppmpService.ProjectQuantity,
        //    //                              ReducedQuantity = ppmpService.ProjectQuantity,
        //    //                              EstimatedCost = ppmpService.ProjectEstimatedBudget,
        //    //                              Remarks = ppmpService.Justification
        //    //                          }).ToList();

        //    //var accountLineItems = ppmpItemDetails.Union(ppmpServiceDetails).ToList();
        //    //return accountLineItems.OrderBy(d => d.ItemName).ThenBy(x => x.ReferenceNo).ToList();
        //    return new List<AccountLineItem>();
        //}
        //public decimal GetNewSpendingProposalAmount(string UACS, string DepartmentCode, int FiscalYear)
        //{
        //    //var projectItems = db.ProjectPlanItems.Where(d => d.FKItemReference.FKItemTypeReference.UACSObjectClass == UACS && d.FKPPMPReference.Department == DepartmentCode && d.FKPPMPReference.FiscalYear == FiscalYear && d.ProposalType == BudgetProposalType.NewProposal && d.Status == "Posted").ToList();
        //    //var projectServices = db.ProjectPlanServices.Where(d => d.FKItemReference.FKItemTypeReference.UACSObjectClass == UACS && d.FKPPMPReference.Department == DepartmentCode && d.FKPPMPReference.FiscalYear == FiscalYear && d.ProposalType == BudgetProposalType.NewProposal && d.Status == "Posted").ToList();
        //    //var newSpendingItemsAmount = projectItems.Count == 0 ? 0.00m : projectItems.Sum(d => d.ProjectEstimatedBudget);
        //    //var newSpendingServiceAmount = projectServices.Count == 0 ? 0.00m : projectServices.Sum(d => d.ProjectEstimatedBudget);
        //    //var newSpendingProposalAmount = newSpendingItemsAmount + newSpendingServiceAmount;
        //    //return newSpendingProposalAmount;
        //    return 0.00m;
        //}
        //private List<PPMPReferences> GetPPMPReferences(string UACS, string DepartmentCode, int FiscalYear)
        //{
        //    //var office = hrisDataAccess.GetFullDepartmentDetails(DepartmentCode);
        //    //List<PPMPReferences> ppmps = new List<PPMPReferences>();
        //    //var ppmpItemList = (from ppmp in db.PPMPHeader
        //    //                    join ppmpItems in db.ProjectPlanItems on ppmp.ID equals ppmpItems.PPMPReference
        //    //                    where ppmp.Department == office.DepartmentCode && ppmpItems.Status == "Posted" && ppmp.Status == "PPMP Submitted" && ppmpItems.FKItemReference.FKItemTypeReference.UACSObjectClass == UACS
        //    //                    select new
        //    //                    {
        //    //                        ReferenceNo = ppmp.ReferenceNo,
        //    //                        SubmittedAt = ppmp.SubmittedAt,
        //    //                        Amount = ppmpItems.ProjectEstimatedBudget
        //    //                    } into result group result by new { result.ReferenceNo, result.SubmittedAt }  into groupResult
        //    //                    select new PPMPReferences
        //    //                    {
        //    //                        ReferenceNo = groupResult.Key.ReferenceNo,
        //    //                        SubmittedAt = (DateTime)groupResult.Key.SubmittedAt,
        //    //                        Amount = (decimal)groupResult.Sum(d => d.Amount)
        //    //                    }).ToList();

        //    //var ppmpServiceList = (from ppmp in db.PPMPHeader
        //    //                       join ppmpServices in db.ProjectPlanServices on ppmp.ID equals ppmpServices.PPMPReference
        //    //                       where ppmp.Department == office.DepartmentCode && ppmpServices.Status == "Posted" && ppmp.Status == "PPMP Submitted" && ppmpServices.FKItemReference.FKItemTypeReference.UACSObjectClass == UACS
        //    //                       select new
        //    //                       {
        //    //                           ReferenceNo = ppmp.ReferenceNo,
        //    //                           Amount = ppmpServices.ProjectEstimatedBudget
        //    //                       } into result
        //    //                       group result by result.ReferenceNo into groupResult
        //    //                       select new PPMPReferences
        //    //                       {
        //    //                           ReferenceNo = groupResult.Key,
        //    //                           Amount = (decimal)groupResult.Sum(d => d.Amount)
        //    //                       }).ToList();

        //    //ppmps.AddRange(ppmpItemList);
        //    //ppmps.AddRange(ppmpServiceList);

        //    //return ppmps;
        //    return new List<PPMPReferences>();
        //}
        //private ProjectPlanItems AdjustQuantity(ProjectPlanItems Item, int ReducedQuantity)
        //{
        //    if (Item.FKProjectReference.ProjectCode.StartsWith("CSPR"))
        //    {
        //        int[] quantities = new int[12];
        //        quantities[0] = Item.PPMPJan != null ? int.Parse(Item.PPMPJan) : 0;
        //        quantities[1] = Item.PPMPFeb != null ? int.Parse(Item.PPMPFeb) : 0;
        //        quantities[2] = Item.PPMPMar != null ? int.Parse(Item.PPMPMar) : 0;
        //        quantities[3] = Item.PPMPApr != null ? int.Parse(Item.PPMPApr) : 0;
        //        quantities[4] = Item.PPMPMay != null ? int.Parse(Item.PPMPMay) : 0;
        //        quantities[5] = Item.PPMPJun != null ? int.Parse(Item.PPMPJun) : 0;
        //        quantities[6] = Item.PPMPJul != null ? int.Parse(Item.PPMPJul) : 0;
        //        quantities[7] = Item.PPMPAug != null ? int.Parse(Item.PPMPAug) : 0;
        //        quantities[8] = Item.PPMPSep != null ? int.Parse(Item.PPMPSep) : 0;
        //        quantities[9] = Item.PPMPOct != null ? int.Parse(Item.PPMPOct) : 0;
        //        quantities[10] = Item.PPMPNov != null ? int.Parse(Item.PPMPNov) : 0;
        //        quantities[11] = Item.PPMPDec != null ? int.Parse(Item.PPMPDec) : 0;

        //        int monthCount = quantities.Where(d => d != 0).Count();
        //        var qtys = quantities.Where(d => d != 0).ToArray();
        //        var reducedMonthlyQty = ReducedQuantity / monthCount;
        //        var reducedMonthlyQtyRemainder = ReducedQuantity % monthCount;
        //        for (int x = 0; x < qtys.Count(); x++)
        //        {
        //            qtys[x] = reducedMonthlyQty;
        //            if (x == (qtys.Count() - 1) && reducedMonthlyQtyRemainder != 0)
        //            {
        //                qtys[x] += reducedMonthlyQtyRemainder;
        //            }
        //        }

        //        var y = 0;
        //        for (int x = 0; x < 12; x++)
        //        {
        //            if (quantities[x] != 0)
        //            {
        //                quantities[x] = qtys[y];
        //                y++;
        //            }
        //        }
        //        Item.PPMPJan = quantities[0] == 0 ? null : quantities[0].ToString();
        //        Item.PPMPFeb = quantities[1] == 0 ? null : quantities[1].ToString();
        //        Item.PPMPMar = quantities[2] == 0 ? null : quantities[2].ToString();
        //        Item.PPMPApr = quantities[3] == 0 ? null : quantities[3].ToString();
        //        Item.PPMPMay = quantities[4] == 0 ? null : quantities[4].ToString();
        //        Item.PPMPJun = quantities[5] == 0 ? null : quantities[5].ToString();
        //        Item.PPMPJul = quantities[6] == 0 ? null : quantities[6].ToString();
        //        Item.PPMPAug = quantities[7] == 0 ? null : quantities[7].ToString();
        //        Item.PPMPSep = quantities[8] == 0 ? null : quantities[8].ToString();
        //        Item.PPMPOct = quantities[9] == 0 ? null : quantities[9].ToString();
        //        Item.PPMPNov = quantities[10] == 0 ? null : quantities[10].ToString();
        //        Item.PPMPDec = quantities[11] == 0 ? null : quantities[11].ToString();
        //    }
        //    //else
        //    //{
        //    //    var startMonth = Item.FKProjectReference.ProjectMonthStart;
        //    //    switch(startMonth)
        //    //    {
        //    //        case 1: Item.PPMPJan = ReducedQuantity.ToString();
        //    //            break;
        //    //        case 2: Item.PPMPFeb = ReducedQuantity.ToString();
        //    //            break;
        //    //        case 3: Item.PPMPMar = ReducedQuantity.ToString();
        //    //            break;
        //    //        case 4: Item.PPMPApr = ReducedQuantity.ToString();
        //    //            break;
        //    //        case 5: Item.PPMPMay = ReducedQuantity.ToString();
        //    //            break;
        //    //        case 6: Item.PPMPJun = ReducedQuantity.ToString();
        //    //            break;
        //    //        case 7: Item.PPMPJul = ReducedQuantity.ToString();
        //    //            break;
        //    //        case 8: Item.PPMPAug = ReducedQuantity.ToString();
        //    //            break;
        //    //        case 9: Item.PPMPSep = ReducedQuantity.ToString();
        //    //            break;
        //    //        case 10: Item.PPMPOct = ReducedQuantity.ToString();
        //    //            break;
        //    //        case 11: Item.PPMPNov = ReducedQuantity.ToString();
        //    //            break;
        //    //        case 12: Item.PPMPDec = ReducedQuantity.ToString();
        //    //            break;
        //    //    }
        //    //}
        //    return Item;
        //}
        //public bool SaveApproval(PPMPEvaluationVM ppmpEvaluation, string UserEmail)
        //{
        //    var employee = hrisDataAccess.GetEmployee(UserEmail);

        //    foreach (var item in ppmpEvaluation.NewSpendingItems)
        //    {
        //        if (item.IsTangible == true)
        //        {
        //            var ppmpItem = db.ProjectPlanItems.Where(d => d.FKPPMPReference.ReferenceNo == item.ReferenceNo && d.FKItemReference.ItemCode == item.ItemCode && d.FKProjectReference.ProjectCode == item.ProjectCode).First();
        //            if (item.ApprovalAction == "Approved" && item.ReducedQuantity != item.Quantity)
        //            {
        //                ppmpItem = AdjustQuantity(ppmpItem, item.ReducedQuantity);
        //            }
        //            ppmpItem.FundSource = item.ApprovalAction == "Approved" ? item.FundSource : null;
        //            ppmpItem.PPMPTotalQty = item.ApprovalAction == "Approved" ? item.ReducedQuantity == 0 ? item.Quantity : item.ReducedQuantity : 0;
        //            ppmpItem.PPMPEstimatedBudget = item.ApprovalAction == "Approved" ? item.EstimatedCost : 0.00m;
        //            ppmpItem.Status = item.ApprovalAction;
        //            if (db.SaveChanges() == 0)
        //            {
        //                return false;
        //            }
        //        }
        //        else
        //        {
        //            var ppmpService = db.ProjectPlanServices.Where(d => d.FKPPMPReference.ReferenceNo == item.ReferenceNo && d.FKItemReference.ItemCode == item.ItemCode).FirstOrDefault();
        //            ppmpService.FundSource = item.ApprovalAction == "Approved" ? item.FundSource : null;
        //            ppmpService.PPMPQuantity = item.ApprovalAction == "Approved" ? item.ReducedQuantity == 0 ? item.Quantity : item.ReducedQuantity : 0;
        //            ppmpService.PPMPEstimatedBudget = item.ApprovalAction == "Approved" ? item.EstimatedCost : 0.00m;
        //            ppmpService.Status = item.ApprovalAction;
        //            if (db.SaveChanges() == 0)
        //            {
        //                return false;
        //            }
        //        }
        //    }

        //    foreach (var item in ppmpEvaluation.NewSpendingItems.GroupBy(d => new { d.ReferenceNo, d.IsTangible }))
        //    {
        //        if (item.Key.IsTangible)
        //        {
        //            var ppmpUnevaluatedCount = db.ProjectPlanItems.Where(d => d.Status == "Posted" && d.FKPPMPReference.ReferenceNo == item.Key.ReferenceNo).Count();
        //            if (ppmpUnevaluatedCount == 0)
        //            {
        //                var ppmpHeader = db.PPMPHeader.Where(d => d.ReferenceNo == item.Key.ReferenceNo).FirstOrDefault();
        //                ppmpHeader.ABC = item.Where(d => d.ApprovalAction == "Approved").Sum(d => d.EstimatedCost);
        //                ppmpHeader.Status = "PPMP Evaluated";
        //                ppmpHeader.ApprovedAt = DateTime.Now;
        //                if (db.SaveChanges() == 0)
        //                {
        //                    return false;
        //                }
        //                var switchBoard = db.SwitchBoard.Where(d => d.Reference == ppmpHeader.ReferenceNo).FirstOrDefault();
        //                var switchBoardBody = new SwitchBoardBody
        //                {
        //                    SwitchBoardReference = switchBoard.ID,
        //                    UpdatedAt = DateTime.Now,
        //                    DepartmentCode = employee.DepartmentCode,
        //                    ActionBy = employee.EmployeeCode,
        //                    Remarks = ppmpHeader.ReferenceNo + " has been evaluated by " + employee.EmployeeName + ", " + employee.Designation + " and is now subject to Annual Procurement Plan Posting. (" + DateTime.Now.ToString("dd MMMM yyyy hh:mm tt") + ")"
        //                };
        //                db.SwitchBoardBody.Add(switchBoardBody);
        //                if (db.SaveChanges() == 0)
        //                {
        //                    return false;
        //                }
        //            }
        //        }
        //        if (!item.Key.IsTangible)
        //        {
        //            var ppmpUnevaluatedCount = db.ProjectPlanServices.Where(d => d.Status == null && d.FKPPMPReference.ReferenceNo == item.Key.ReferenceNo).Count();
        //            if (ppmpUnevaluatedCount == 0)
        //            {
        //                var ppmpHeader = db.PPMPHeader.Where(d => d.ReferenceNo == item.Key.ReferenceNo).FirstOrDefault();
        //                ppmpHeader.ABC = item.Where(d => d.ApprovalAction == "Approved").Sum(d => d.EstimatedCost);
        //                ppmpHeader.Status = "PPMP Evaluated";
        //                ppmpHeader.ApprovedAt = DateTime.Now;
        //                if (db.SaveChanges() == 0)
        //                {
        //                    return false;
        //                }
        //                var switchBoard = db.SwitchBoard.Where(d => d.Reference == ppmpHeader.ReferenceNo).FirstOrDefault();
        //                var switchBoardBody = new SwitchBoardBody
        //                {
        //                    SwitchBoardReference = switchBoard.ID,
        //                    UpdatedAt = DateTime.Now,
        //                    DepartmentCode = employee.DepartmentCode,
        //                    ActionBy = employee.EmployeeCode,
        //                    Remarks = ppmpHeader.ReferenceNo + " has been evaluated by " + employee.EmployeeName + ", " + employee.Designation + " and is now subject to Annual Procurement Plan Posting. (" + DateTime.Now.ToString("dd MMMM yyyy hh:mm tt") + ")"
        //                };
        //                db.SwitchBoardBody.Add(switchBoardBody);
        //                if (db.SaveChanges() == 0)
        //                {
        //                    return false;
        //                }
        //            }
        //        }
        //    }
        //    foreach (var item in ppmpEvaluation.NewSpendingItems.Where(d => d.ApprovalAction == "Approved").GroupBy(d => d.ProjectCode).Select(d => d.Key).ToList())
        //    {
        //        var project = db.ProjectPlans.Where(d => d.ProjectCode == item).FirstOrDefault();
        //        project.ProjectStatus = "Project Item Evaluated";
        //        var switchBoard = db.SwitchBoard.Where(d => d.Reference == item).FirstOrDefault();
        //        var switchBoardBody = new SwitchBoardBody
        //        {
        //            SwitchBoardReference = switchBoard.ID,
        //            UpdatedAt = DateTime.Now,
        //            DepartmentCode = employee.DepartmentCode,
        //            ActionBy = employee.EmployeeCode,
        //            Remarks = item + " has been evaluated by " + employee.EmployeeName + ", " + employee.Designation + " and is now subject to Annual Procurement Plan Posting. (" + DateTime.Now.ToString("dd MMMM yyyy hh:mm tt") + ")"
        //        };
        //        db.SwitchBoardBody.Add(switchBoardBody);
        //        if (db.SaveChanges() == 0)
        //        {
        //            return false;
        //        }

        //    }
        //    return true;
        //}
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
                hris.Dispose();
                abis.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}