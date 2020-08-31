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

        public MemoryStream ViewPPMP(string ReferenceNo, string LogoPath, string UserEmail)
        {
            return ppmpBL.GeneratePPMPReport(ReferenceNo, LogoPath, UserEmail);
        }

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
        private TEMPAccounting abdb = new TEMPAccounting();
        private HRISDataAccess hrisDataAccess = new HRISDataAccess();
        private ABISDataAccess abisDataAccess = new ABISDataAccess();

        public List<PPMPByOfficeVM> GetOffices(int FiscalYear)
        {
            var offices = hrisDataAccess.GetAllDepartments();
            var ppmpItemsPerOffice = (from ppmp in db.PPMPHeader
                                      join ppmpItems in db.ProjectPlanItems
                                      on ppmp.ID equals ppmpItems.PPMPReference
                                      where ppmp.FiscalYear == FiscalYear && ppmp.Status == "PPMP Submitted" && ppmpItems.Status == "Posted"
                                      select new
                                      {
                                          DepartmentCode = ppmp.Department,
                                          FiscalYear = ppmp.FiscalYear,
                                          EstimatedBudget = ppmpItems.ProjectEstimatedBudget
                                      }
                                      into list
                                      group list by new
                                      {
                                          DepartmentCode = list.DepartmentCode,
                                          FiscalYear = list.FiscalYear
                                      }
                                     ).Select( d => new
                                       {
                                           DepartmentCode = d.Key.DepartmentCode,
                                           FiscalYear = d.Key.FiscalYear,
                                           EstimatedBudget = d.Sum(x => x.EstimatedBudget)
                                       }).ToList();

            var ppmpServicePerOffice = (from ppmp in db.PPMPHeader
                                        join ppmpServices in db.ProjectPlanServices
                                        on ppmp.ID equals ppmpServices.PPMPReference
                                        where ppmp.FiscalYear == FiscalYear && ppmp.Status == "PPMP Submitted" && ppmpServices.Status == "Posted"
                                        select new
                                        {
                                            DepartmentCode = ppmp.Department,
                                            FiscalYear = ppmp.FiscalYear,
                                            EstimatedBudget = ppmpServices.ProjectEstimatedBudget
                                        }
                                      into list
                                        group list by new
                                        {
                                            DepartmentCode = list.DepartmentCode,
                                            FiscalYear = list.FiscalYear
                                        }
                                      ).Select(d => new
                                      {
                                          DepartmentCode = d.Key.DepartmentCode,
                                          FiscalYear = d.Key.FiscalYear,
                                          EstimatedBudget = d.Sum(x => x.EstimatedBudget)
                                      }).ToList();

            var ppmpPerOfficeUnion = ppmpItemsPerOffice
                                     .Union(ppmpServicePerOffice)
                                     .GroupBy(d => new
                                     {
                                        DepartmentCode = d.DepartmentCode,
                                        FiscalYear = d.FiscalYear
                                     }).Select(d => new {
                                         DepartmentCode = d.Key.DepartmentCode,
                                         FiscalYear = d.Key.FiscalYear,
                                         EstimatedBudget = d.Sum(x => x.EstimatedBudget)
                                     }).ToList();

            return ppmpPerOfficeUnion.Join(offices, ppmpList => ppmpList.DepartmentCode, officeList => officeList.DepartmentCode, (ppmpList, officeList) =>
                                 new PPMPByOfficeVM
                                 {
                                     DepartmentCode = officeList.DepartmentCode,
                                     Department = officeList.Department,
                                     FiscalYear = ppmpList.FiscalYear,
                                     EstimatedBudget = ppmpList.EstimatedBudget
                                 }).ToList();
        }
        public List<PPMPLineItemsPerAccountVM> GetPPMPLineItemsPerAccount(string DepartmentCode)
        {
            var office = hrisDataAccess.GetFullDepartmentDetails(DepartmentCode);
            var accounts = abisDataAccess.GetDetailedChartOfAccounts();
            var ppmpItems = db.ProjectPlanItems.Where(d => (office.SectionCode == null ? d.FKPPMPReference.Department == office.DepartmentCode : d.FKPPMPReference.Department == office.SectionCode) && d.FKPPMPReference.Status == "PPMP Submitted" && d.Status == "Posted").ToList();
            var ppmpService = db.ProjectPlanServices.Where(d => (office.SectionCode == null ? d.FKPPMPReference.Department == office.DepartmentCode : d.FKPPMPReference.Department == office.SectionCode) && d.FKPPMPReference.Status == "PPMP Submitted" && d.Status == "Posted").ToList();

            var itemAccountsList = (from ppmpLineItems in ppmpItems
                                    join accountsList in accounts
                                    on  ppmpLineItems.FKItemReference.FKItemTypeReference.AccountClass equals accountsList.UACS_Code
                                    select new
                                    {
                                        UACS = accountsList.UACS_Code,
                                        ObjectClassification = accountsList.AcctName,
                                        EstimatedBudget = ppmpLineItems.ProjectEstimatedBudget
                                    }).ToList();

            var serviceAccountsList = (from ppmpServiceList in ppmpService
                                    join accountsList in accounts
                                    on ppmpServiceList.FKItemReference.FKItemTypeReference.AccountClass equals accountsList.UACS_Code
                                    select new
                                    {
                                        UACS = accountsList.UACS_Code,
                                        ObjectClassification = accountsList.AcctName,
                                        EstimatedBudget = ppmpServiceList.ProjectEstimatedBudget
                                    }).ToList();

            var lineItemsPerAccount = itemAccountsList.Union(serviceAccountsList)
                                      .GroupBy(d => new { d.UACS, d.ObjectClassification })
                                      .Select(d => new PPMPLineItemsPerAccountVM
                                      {
                                          UACS = d.Key.UACS,
                                          ObjectClassification = d.Key.ObjectClassification,
                                          EstimatedBudget = d.Sum(x => x.EstimatedBudget)
                                      }).ToList();

            return lineItemsPerAccount;
        }
        public List<AccountLineItem> GetNewSpendingItems(string UACS, string DepartmentCode, int FiscalYear)
        {
            var ppmpItemDetails = (from ppmpItems in db.ProjectPlanItems
                                   join projectItems in db.ProjectPlanItems
                                   on new { ProjectReference = ppmpItems.ProjectReference,  ItemReference = ppmpItems.ItemReference } equals new { ProjectReference = projectItems.ProjectReference, ItemReference = projectItems.ItemReference }
                                   where ppmpItems.FKPPMPReference.FiscalYear == FiscalYear && ppmpItems.FKPPMPReference.Department == DepartmentCode && ppmpItems.FKItemReference.FKItemTypeReference.AccountClass == UACS && projectItems.ProposalType == BudgetProposalType.NewProposal && ppmpItems.Status == "Posted"
                                   select new AccountLineItem
                                   {
                                       ApprovalAction = "Approved",
                                       ProjectCode = ppmpItems.FKProjectReference.ProjectCode,
                                       ReferenceNo = ppmpItems.FKPPMPReference.ReferenceNo,
                                       ProjectTitle = ppmpItems.FKProjectReference.ProjectName,
                                       ProposalType = projectItems.ProposalType,
                                       ItemCode = ppmpItems.FKItemReference.ItemCode,
                                       ItemName = ppmpItems.FKItemReference.ItemFullName.ToUpper() + " (" + ppmpItems.FKItemReference.ItemCode + ")",
                                       ItemSpecifications = ppmpItems.FKItemReference.ItemSpecifications,
                                       UnitOfMeasure = ppmpItems.FKItemReference.FKIndividualUnitReference.UnitName.ToUpper(),
                                       UnitCost = ppmpItems.UnitCost,
                                       Quantity = ppmpItems.ProjectTotalQty,
                                       ReducedQuantity = ppmpItems.ProjectTotalQty,
                                       IsTangible = ppmpItems.FKItemReference.FKItemTypeReference.FKInventoryTypeReference.IsTangible,
                                       EstimatedCost = ppmpItems.ProjectEstimatedBudget,
                                       Remarks = ppmpItems.Justification
                                   }).ToList();

            var ppmpServiceDetails = (from ppmpService in db.ProjectPlanServices
                                      join projectService in db.ProjectPlanServices
                                      on new { ProjectReference = ppmpService.ProjectReference, ServiceReference = ppmpService.ItemReference } equals new { ProjectReference = projectService.ProjectReference, ServiceReference = projectService.ItemReference }
                                      where ppmpService.FKPPMPReference.FiscalYear == FiscalYear && ppmpService.FKPPMPReference.Department == DepartmentCode && ppmpService.FKItemReference.FKItemTypeReference.AccountClass == UACS && projectService.ProposalType == BudgetProposalType.NewProposal && ppmpService.Status == "Posted"
                                      select new AccountLineItem
                                      {
                                          ApprovalAction = "Approved",
                                          ProjectCode = ppmpService.FKProjectReference.ProjectCode,
                                          ReferenceNo = ppmpService.FKPPMPReference.ReferenceNo,
                                          ProjectTitle = ppmpService.FKProjectReference.ProjectName,
                                          ProposalType = projectService.ProposalType,
                                          ItemCode = ppmpService.FKItemReference.ItemCode,
                                          ItemName = ppmpService.FKItemReference.ItemFullName + " (" + ppmpService.FKItemReference.ItemCode + ")",
                                          ItemSpecifications = ppmpService.ItemSpecifications,
                                          UnitOfMeasure = "",
                                          UnitCost = ppmpService.UnitCost,
                                          Quantity = ppmpService.ProjectQuantity,
                                          ReducedQuantity = ppmpService.ProjectQuantity,
                                          IsTangible = ppmpService.FKItemReference.FKItemTypeReference.FKInventoryTypeReference.IsTangible,
                                          EstimatedCost = ppmpService.ProjectEstimatedBudget,
                                          Remarks = ppmpService.Justification
                                      }).ToList();

            var accountLineItems = ppmpItemDetails.Union(ppmpServiceDetails).ToList();
            return accountLineItems.OrderBy(d => d.ItemName).ThenBy(x => x.ReferenceNo).ToList();
        }
        public decimal GetNewSpendingProposalAmount(string UACS, string DepartmentCode, int FiscalYear)
        {
            var projectItems = db.ProjectPlanItems.Where(d => d.FKItemReference.FKItemTypeReference.AccountClass == UACS && d.FKPPMPReference.Department == DepartmentCode && d.FKPPMPReference.FiscalYear == FiscalYear && d.ProposalType == BudgetProposalType.NewProposal && d.Status == "Posted").ToList();
            var projectServices = db.ProjectPlanServices.Where(d => d.FKItemReference.FKItemTypeReference.AccountClass == UACS && d.FKPPMPReference.Department == DepartmentCode && d.FKPPMPReference.FiscalYear == FiscalYear && d.ProposalType == BudgetProposalType.NewProposal && d.Status == "Posted").ToList();
            var newSpendingItemsAmount = projectItems.Count == 0 ? 0.00m : projectItems.Sum(d => d.ProjectEstimatedBudget);
            var newSpendingServiceAmount = projectServices.Count == 0 ? 0.00m : projectServices.Sum(d => d.ProjectEstimatedBudget);
            var newSpendingProposalAmount = newSpendingItemsAmount + newSpendingServiceAmount;
            return newSpendingProposalAmount;
        }
        private List<PPMPReferences> GetPPMPReferences(string UACS, string DepartmentCode, int FiscalYear)
        {
            var office = hrisDataAccess.GetFullDepartmentDetails(DepartmentCode);
            List<PPMPReferences> ppmps = new List<PPMPReferences>();
            var ppmpItemList = (from ppmp in db.PPMPHeader
                                join ppmpItems in db.ProjectPlanItems on ppmp.ID equals ppmpItems.PPMPReference
                                where ppmp.Department == office.DepartmentCode && ppmpItems.Status == "Posted" && ppmp.Status == "PPMP Submitted" && ppmpItems.FKItemReference.FKItemTypeReference.AccountClass == UACS
                                select new
                                {
                                    ReferenceNo = ppmp.ReferenceNo,
                                    SubmittedAt = ppmp.SubmittedAt,
                                    Amount = ppmpItems.ProjectEstimatedBudget
                                } into result group result by new { result.ReferenceNo, result.SubmittedAt }  into groupResult
                                select new PPMPReferences
                                {
                                    ReferenceNo = groupResult.Key.ReferenceNo,
                                    SubmittedAt = (DateTime)groupResult.Key.SubmittedAt,
                                    Amount = (decimal)groupResult.Sum(d => d.Amount)
                                }).ToList();

            var ppmpServiceList = (from ppmp in db.PPMPHeader
                                   join ppmpServices in db.ProjectPlanServices on ppmp.ID equals ppmpServices.PPMPReference
                                   where ppmp.Department == office.DepartmentCode && ppmpServices.Status == "Posted" && ppmp.Status == "PPMP Submitted" && ppmpServices.FKItemReference.FKItemTypeReference.AccountClass == UACS
                                   select new
                                   {
                                       ReferenceNo = ppmp.ReferenceNo,
                                       Amount = ppmpServices.ProjectEstimatedBudget
                                   } into result
                                   group result by result.ReferenceNo into groupResult
                                   select new PPMPReferences
                                   {
                                       ReferenceNo = groupResult.Key,
                                       Amount = (decimal)groupResult.Sum(d => d.Amount)
                                   }).ToList();

            ppmps.AddRange(ppmpItemList);
            ppmps.AddRange(ppmpServiceList);

            return ppmps;
        } 
        public PPMPEvaluationVM GetEvaluationDetails(string UACS, string DepartmentCode, int FiscalYear)
        {
            var office = hrisDataAccess.GetFullDepartmentDetails(DepartmentCode);
            var accountTitle = abdb.ChartOfAccounts.Where(d => d.UACS_Code == UACS).FirstOrDefault().AcctName;
            PPMPEvaluationVM ppmpEvaluation = new PPMPEvaluationVM();
            ppmpEvaluation.UACS = UACS;
            ppmpEvaluation.AccountTitle = accountTitle;
            ppmpEvaluation.OfficeCode = office.DepartmentCode;
            ppmpEvaluation.OfficeName = office.Department;
            ppmpEvaluation.PPMPReferences = GetPPMPReferences(UACS, DepartmentCode, FiscalYear);
            ppmpEvaluation.NewSpendingItems = GetNewSpendingItems(UACS, DepartmentCode, FiscalYear);
            ppmpEvaluation.TotalProposedBudget = GetNewSpendingProposalAmount(UACS, office.DepartmentCode, FiscalYear);
            ppmpEvaluation.ApprovedBudget = GetNewSpendingProposalAmount(UACS, office.DepartmentCode, FiscalYear);
            return ppmpEvaluation;
        }
        private ProjectPlanItems AdjustQuantity(ProjectPlanItems Item, int ReducedQuantity)
        {
            if(Item.FKProjectReference.ProjectCode.StartsWith("CSPR"))
            {
                int[] quantities = new int[12];
                quantities[0] = Item.PPMPJan != null ? int.Parse(Item.PPMPJan) : 0;
                quantities[1] = Item.PPMPFeb != null ? int.Parse(Item.PPMPFeb) : 0;
                quantities[2] = Item.PPMPMar != null ? int.Parse(Item.PPMPMar) : 0;
                quantities[3] = Item.PPMPApr != null ? int.Parse(Item.PPMPApr) : 0;
                quantities[4] = Item.PPMPMay != null ? int.Parse(Item.PPMPMay) : 0;
                quantities[5] = Item.PPMPJun != null ? int.Parse(Item.PPMPJun) : 0;
                quantities[6] = Item.PPMPJul != null ? int.Parse(Item.PPMPJul) : 0;
                quantities[7] = Item.PPMPAug != null ? int.Parse(Item.PPMPAug) : 0;
                quantities[8] = Item.PPMPSep != null ? int.Parse(Item.PPMPSep) : 0;
                quantities[9] = Item.PPMPOct != null ? int.Parse(Item.PPMPOct) : 0;
                quantities[10] = Item.PPMPNov != null ? int.Parse(Item.PPMPNov) : 0;
                quantities[11] = Item.PPMPDec != null ? int.Parse(Item.PPMPDec) : 0;

                int monthCount = quantities.Where(d => d != 0).Count();
                var qtys = quantities.Where(d => d != 0).ToArray();
                var reducedMonthlyQty = ReducedQuantity / monthCount;
                var reducedMonthlyQtyRemainder = ReducedQuantity % monthCount;
                for(int x = 0; x < qtys.Count(); x++)
                {
                    qtys[x] = reducedMonthlyQty;
                    if (x == (qtys.Count() - 1) && reducedMonthlyQtyRemainder != 0)
                    {
                        qtys[x] += reducedMonthlyQtyRemainder;
                    }
                }

                var y = 0;
                for (int x = 0; x < 12; x++)
                {
                    if(quantities[x] != 0)
                    {
                        quantities[x] = qtys[y];
                        y++;
                    }
                }
                Item.PPMPJan = quantities[0] == 0 ? null : quantities[0].ToString();
                Item.PPMPFeb = quantities[1] == 0 ? null : quantities[1].ToString();
                Item.PPMPMar = quantities[2] == 0 ? null : quantities[2].ToString();
                Item.PPMPApr = quantities[3] == 0 ? null : quantities[3].ToString();
                Item.PPMPMay = quantities[4] == 0 ? null : quantities[4].ToString();
                Item.PPMPJun = quantities[5] == 0 ? null : quantities[5].ToString();
                Item.PPMPJul = quantities[6] == 0 ? null : quantities[6].ToString();
                Item.PPMPAug = quantities[7] == 0 ? null : quantities[7].ToString();
                Item.PPMPSep = quantities[8] == 0 ? null : quantities[8].ToString();
                Item.PPMPOct = quantities[9] == 0 ? null : quantities[9].ToString();
                Item.PPMPNov = quantities[10] == 0 ? null : quantities[10].ToString();
                Item.PPMPDec = quantities[11] == 0 ? null : quantities[11].ToString();
            }
            //else
            //{
            //    var startMonth = Item.FKProjectReference.ProjectMonthStart;
            //    switch(startMonth)
            //    {
            //        case 1: Item.PPMPJan = ReducedQuantity.ToString();
            //            break;
            //        case 2: Item.PPMPFeb = ReducedQuantity.ToString();
            //            break;
            //        case 3: Item.PPMPMar = ReducedQuantity.ToString();
            //            break;
            //        case 4: Item.PPMPApr = ReducedQuantity.ToString();
            //            break;
            //        case 5: Item.PPMPMay = ReducedQuantity.ToString();
            //            break;
            //        case 6: Item.PPMPJun = ReducedQuantity.ToString();
            //            break;
            //        case 7: Item.PPMPJul = ReducedQuantity.ToString();
            //            break;
            //        case 8: Item.PPMPAug = ReducedQuantity.ToString();
            //            break;
            //        case 9: Item.PPMPSep = ReducedQuantity.ToString();
            //            break;
            //        case 10: Item.PPMPOct = ReducedQuantity.ToString();
            //            break;
            //        case 11: Item.PPMPNov = ReducedQuantity.ToString();
            //            break;
            //        case 12: Item.PPMPDec = ReducedQuantity.ToString();
            //            break;
            //    }
            //}
            return Item;
        }
        public bool SaveApproval(PPMPEvaluationVM ppmpEvaluation, string UserEmail)
        {
            var employee = hrisDataAccess.GetEmployee(UserEmail);

            foreach(var item in ppmpEvaluation.NewSpendingItems)
            {
                if(item.IsTangible == true)
                {
                    var ppmpItem = db.ProjectPlanItems.Where(d => d.FKPPMPReference.ReferenceNo == item.ReferenceNo && d.FKItemReference.ItemCode == item.ItemCode && d.FKProjectReference.ProjectCode == item.ProjectCode).First();
                    if(item.ApprovalAction == "Approved" && item.ReducedQuantity != item.Quantity)
                    {
                        ppmpItem = AdjustQuantity(ppmpItem, item.ReducedQuantity);
                    }
                    ppmpItem.FundSource = item.ApprovalAction == "Approved" ? item.FundSource : null;
                    ppmpItem.PPMPTotalQty = item.ApprovalAction == "Approved" ? item.ReducedQuantity == 0 ? item.Quantity : item.ReducedQuantity : 0;
                    ppmpItem.PPMPEstimatedBudget = item.ApprovalAction == "Approved" ? item.EstimatedCost : 0.00m;
                    ppmpItem.Status = item.ApprovalAction;
                    if (db.SaveChanges() == 0)
                    {
                        return false;
                    }
                }
                else
                {
                    var ppmpService = db.ProjectPlanServices.Where(d => d.FKPPMPReference.ReferenceNo == item.ReferenceNo && d.FKItemReference.ItemCode == item.ItemCode).FirstOrDefault();
                    ppmpService.FundSource = item.ApprovalAction == "Approved" ? item.FundSource : null;
                    ppmpService.PPMPQuantity = item.ApprovalAction == "Approved" ? item.ReducedQuantity == 0 ? item.Quantity : item.ReducedQuantity : 0;
                    ppmpService.PPMPEstimatedBudget = item.ApprovalAction == "Approved" ? item.EstimatedCost : 0.00m;
                    ppmpService.Status = item.ApprovalAction;
                    if (db.SaveChanges() == 0)
                    {
                        return false;
                    }
                }
            }

            foreach (var item in ppmpEvaluation.NewSpendingItems.GroupBy(d => d.ReferenceNo))
            {
                var ppmpHeader = db.PPMPHeader.Where(d => d.ReferenceNo == item.Key).FirstOrDefault();
                ppmpHeader.ABC = item.Where(d => d.ApprovalAction == "Approved").Sum(d => d.EstimatedCost);
                ppmpHeader.Status = "PPMP Evaluated";
                ppmpHeader.ApprovedAt = DateTime.Now;
                if(db.SaveChanges() == 0)
                {
                    return false;
                }
                var switchBoard = db.SwitchBoard.Where(d => d.Reference == ppmpHeader.ReferenceNo).FirstOrDefault();
                var switchBoardBody = new SwitchBoardBody
                {
                    SwitchBoardReference = switchBoard.ID,
                    UpdatedAt = DateTime.Now,
                    DepartmentCode = employee.DepartmentCode,
                    ActionBy = employee.EmployeeCode,
                    Remarks = ppmpHeader.ReferenceNo + " has been evaluated by " + employee.EmployeeName + ", " + employee.Designation + " and is now subject to Annual Procurement Plan Posting. (" + DateTime.Now.ToString("dd MMMM yyyy hh:mm tt") + ")"
                };
                db.SwitchBoardBody.Add(switchBoardBody);
                if (db.SaveChanges() == 0)
                {
                    return false;
                }
            }
            foreach(var item in ppmpEvaluation.NewSpendingItems.Where(d => d.ApprovalAction == "Approved").GroupBy(d => d.ProjectCode).Select(d => d.Key).ToList())
            {
                var project = db.ProjectPlans.Where(d => d.ProjectCode == item).FirstOrDefault();
                project.ProjectStatus = "Project Item Evaluated";
                var switchBoard = db.SwitchBoard.Where(d => d.Reference == item).FirstOrDefault();
                var switchBoardBody = new SwitchBoardBody
                {
                    SwitchBoardReference = switchBoard.ID,
                    UpdatedAt = DateTime.Now,
                    DepartmentCode = employee.DepartmentCode,
                    ActionBy = employee.EmployeeCode,
                    Remarks = item + " has been evaluated by " + employee.EmployeeName + ", " + employee.Designation + " and is now subject to Annual Procurement Plan Posting. (" + DateTime.Now.ToString("dd MMMM yyyy hh:mm tt") + ")"
                };
                db.SwitchBoardBody.Add(switchBoardBody);
                if (db.SaveChanges() == 0)
                {
                    return false;
                }

            }
            return true;
        }
        public List<FundSources> GetFundSource()
        {
            return abdb.FundSources.ToList();
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
                hrisDataAccess.Dispose();
                abdb.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}