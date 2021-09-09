//using MigraDoc.DocumentObjectModel;
//using MigraDoc.DocumentObjectModel.Tables;
//using PUPFMIS.Models;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Web.Mvc;

//namespace PUPFMIS.BusinessAndDataLogic
//{
//    public class BidsRegistrationBL : Controller
//    {
//        private BidsRegistrationDAL bidsRegistrationDAL = new BidsRegistrationDAL();
//        private FMISDbContext db = new FMISDbContext();
//        private HRISDataAccess hris = new HRISDataAccess();

//        protected override void Dispose(bool disposing)
//        {
//            if (disposing)
//            {
//                bidsRegistrationDAL.Dispose();
//            }
//            base.Dispose(disposing);
//        }
//    }

//    public class BidsRegistrationDAL : Controller
//    {
//        private FMISDbContext db = new FMISDbContext();
//        private HRISDataAccess hris = new HRISDataAccess();
//        private ABISDataAccess abis = new ABISDataAccess();
//        private SystemBDL systemBDL = new SystemBDL();

//        public List<ProcurementProjectListVM> GetContractsWithoutBids(string UserEmail)
//        {
//            var user = db.UserAccounts.Where(d => d.Email == UserEmail).FirstOrDefault();
//            var contracts = db.ProcurementProjects
//                            .Where(d => d.ProjectCoordinator == user.EmpCode && d.ProcurementProjectStage == ProcurementProjectStages.BidsOpened)
//                            .ToList()
//                            .Select(d => new ProcurementProjectListVM
//                            {
//                                ProcurementProjectType = d.ProcurementProjectType,
//                                ContractCode = d.ContractCode,
//                                ContractName = d.ContractName,
//                                ModeOfProcurementReference = d.ModeOfProcurementReference,
//                                ModeOfProcurement = d.FKModeOfProcurementReference.ModeOfProcurementName,
//                                FiscalYear = d.FiscalYear,
//                                ContractLocation = d.ContractLocation,
//                                ContractStatus = d.ContractStatus,
//                                ProcurementProjectStage = d.ProcurementProjectStage,
//                                ApprovedBudgetForContract = d.ApprovedBudgetForContract
//                            }).ToList();
//            return contracts;
//        }
//        public List<int> GetExistingBidders(string ContractCode)
//        {
//            return db.BidsHeader.Where(d => d.FKProcurementProject.ContractCode == ContractCode).Select(d => d.SupplierReference).ToList();
//        }
//        public List<BidsListVM> GetBidList(string ContractCode)
//        {
//            return db.BidsHeader.Where(d => d.FKProcurementProject.ContractCode == ContractCode).ToList()
//            .Select(d => new BidsListVM {
//                BidReferenceNo = d.BidReferenceNo,
//                ContractCode = d.FKProcurementProject.ContractCode,
//                ContractName = d.FKProcurementProject.ContractName,
//                SupplierReference = d.SupplierReference,
//                SupplierName = d.FKSupplierReference.SupplierName,
//                TotalAmountOfBid = d.TotalAmountOfBidAsRead,
//                BidSubmittedAt = (DateTime)d.BidSubmittedAt,
//                RecordedAt = d.RecordedAt
//            }).ToList();
//        }
//        public BidsVM BidRegistrationSetup(string ContractCode)
//        {
//            var contract = db.ProcurementProjects.Where(d => d.ContractCode == ContractCode).FirstOrDefault();
//            var bidType = contract.ProcurementProjectType == ProcurementProjectTypes.AMP ? BidTypes.Quotation : BidTypes.Bid;
//            return new BidsVM
//            {
//                BidType = bidType,
//                PAPCode = contract.FKAPPReference.PAPCode,
//                ClassificationReference = contract.ClassificationReference,
//                Classification = contract.FKClassificationReference.GeneralClass + " - " + contract.FKClassificationReference.Classification,
//                ModeOfProcurementReference = contract.ModeOfProcurementReference,
//                ModeOfProcurement = contract.FKModeOfProcurementReference.ModeOfProcurementName,
//                FiscalYear = contract.FiscalYear,
//                FundSource = contract.FundSource,
//                FundDescription = abis.GetFundSources(contract.FundSource).FUND_DESC,
//                ProcurementProjectType = contract.ProcurementProjectType,
//                ContractStrategy = contract.ContractStrategy,
//                ContractCode = contract.ContractCode,
//                ContractName = contract.ContractName,
//                ContractLocation = contract.ContractLocation,
//                ContractStatus = contract.ContractStatus,
//                ProcurementProjectStage = contract.ProcurementProjectStage,
//                ApprovedBudgetForContract = contract.ApprovedBudgetForContract,
//                DeliveryPeriod = (int)contract.DeliveryPeriod,
//                SubmittedAt = DateTime.Now,
//                ProjectCoordinator = hris.GetEmployeeByCode(contract.ProjectCoordinator).EmployeeName,
//                BidReferenceNo = GenerateBidReferenceNo(ContractCode, bidType),
//                BidDetails = db.ProcurementProjectDetails.Where(d => d.ProcurementProject == contract.ID).Select(d => new BidDetailsVM {
//                    ArticleReference = d.ArticleReference,
//                    ItemSequence = d.ItemSequence,
//                    ItemFullName = d.ItemFullName,
//                    ItemSpecifications = d.ItemSpecifications,
//                    UOMReference = d.UOMReference,
//                    Quantity = d.Quantity
//                }).ToList()
//            };
//        }
//        public List<BidDetailsVM> ComputeBidTotalPrice(List<BidDetailsVM> Details)
//        {
//            foreach(var detail in Details)
//            {
//                detail.BidTotalPrice = detail.BidAction == BidActions.WithBid ? Math.Round((decimal)(detail.Quantity * detail.BidUnitPrice), 2) : (decimal?)null;
//            }
//            return Details;
//        }
//        public bool PostBidRegistration(BidsVM Bid, string UserEmail, BidRegistrationOptions BidRegistrationOption)
//        {
//            var user = db.UserAccounts.Where(d => d.Email == UserEmail).FirstOrDefault();
//            var contract = db.ProcurementProjects.Where(d => d.ContractCode == Bid.ContractCode).FirstOrDefault();
//            var bidHeader = new BidsHeader
//            {
//                BidType = Bid.BidType,
//                BidReferenceNo = Bid.BidReferenceNo,
//                SupplierReference = Bid.SupplierReference,
//                ProcurementProject = contract.ID,
//                BidSubmittedAt = Bid.SubmittedAt,
//                TotalAmountOfBidAsRead = Bid.BidDetails.Where(d => d.BidTotalPrice != null).Sum(d => d.BidTotalPrice).Value,
//                IsBidSecurityRequired = Bid.IsBidSecurityRequired,
//                IsPerformaceSecurityRequired = Bid.IsPerformaceSecurityRequired,
//                RecordedAt = DateTime.Now,
//                RecordedBy = user.EmpCode
//            };

//            db.BidsHeader.Add(bidHeader);
//            if(db.SaveChanges() == 0)
//            {
//                return false;
//            }

//            db.BidDetails.AddRange(Bid.BidDetails.Select(d => new BidDetails {
//                BidAction = d.BidAction,
//                BidReference = bidHeader.ID,
//                ArticleReference = d.ArticleReference,
//                ItemSequence = d.ItemSequence,
//                ItemFullName = d.ItemFullName,
//                ItemSpecifications = d.ItemSpecifications,
//                UOMReference = d.UOMReference,
//                Quantity = d.Quantity,
//                BidUnitPrice = d.BidUnitPrice,
//                BidTotalPrice = d.BidTotalPrice
//            }).ToList());

//            db.ContractUpdates.Add(new ContractUpdates
//            {
//                ProcurementProject = contract.ID,
//                ProcurementProjectStage = ProcurementProjectStages.BiddingFailed,
//                UpdatedAt = DateTime.Now,
//                UpdatedBy = user.EmpCode,
//                Remarks = bidHeader.BidReferenceNo + " is registered. Evaluation of this bid is now open."
//            });

//            if (db.SaveChanges() == 0)
//            {
//                return false;
//            }

//            return true;
//        }
//        public bool UpdateBidRegistration(BidsVM Bid)
//        {
//            var bid = db.BidsHeader.Where(d => d.BidReferenceNo == Bid.BidReferenceNo).FirstOrDefault();
//            bid.SupplierReference = Bid.SupplierReference;
//            bid.BidSubmittedAt = Bid.SubmittedAt;
//            bid.TotalAmountOfBidAsRead = Bid.TotalAmountOfBidAsRead;
//            bid.IsBidSecurityRequired = Bid.IsBidSecurityRequired;
//            bid.IsPerformaceSecurityRequired = Bid.IsPerformaceSecurityRequired;

//            if (db.SaveChanges() == 0)
//            {
//                return false;
//            }

//            foreach(var detail in Bid.BidDetails)
//            {
//                var bidDetail = db.BidDetails.Where(d => d.BidReference == bid.ID && d.ArticleReference == detail.ArticleReference && d.ItemSequence == d.ItemSequence).FirstOrDefault();
//                bidDetail.BidAction = detail.BidAction;
//                bidDetail.BidUnitPrice = detail.BidUnitPrice;
//                bidDetail.BidTotalPrice = detail.BidTotalPrice;

//                if (db.SaveChanges() == 0)
//                {
//                    return false;
//                }
//            }

//            return true;
//        }
//        public BidsVM GetBid(string BidReferenceNo)
//        {
//            return db.BidsHeader.Where(d => d.BidReferenceNo == BidReferenceNo).ToList()
//            .Select(d => new BidsVM
//            {
//                SubmittedAt = d.BidSubmittedAt.Value,
//                PAPCode = d.FKProcurementProject.FKAPPReference.PAPCode,
//                ClassificationReference = d.FKProcurementProject.ClassificationReference,
//                Classification = d.FKProcurementProject.FKClassificationReference.GeneralClass + "-" + d.FKProcurementProject.FKClassificationReference.Classification,
//                ModeOfProcurementReference = d.FKProcurementProject.ModeOfProcurementReference,
//                ModeOfProcurement = d.FKProcurementProject.FKModeOfProcurementReference.ModeOfProcurementName,
//                FiscalYear = d.FKProcurementProject.FiscalYear,
//                FundSource = d.FKProcurementProject.FundSource,
//                FundDescription = abis.GetFundSources(d.FKProcurementProject.FundSource).FUND_CLUSTER,
//                ProcurementProjectType = d.FKProcurementProject.ProcurementProjectType,
//                ContractStrategy = d.FKProcurementProject.ContractStrategy,
//                ContractCode = d.FKProcurementProject.ContractCode,
//                ContractName = d.FKProcurementProject.ContractName,
//                ContractLocation = d.FKProcurementProject.ContractLocation,
//                ContractStatus = d.FKProcurementProject.ContractStatus,
//                ProcurementProjectStage = d.FKProcurementProject.ProcurementProjectStage,
//                ApprovedBudgetForContract = d.FKProcurementProject.ApprovedBudgetForContract,
//                DeliveryPeriod = d.FKProcurementProject.DeliveryPeriod.Value,
//                ProjectCoordinator = hris.GetEmployeeByCode(d.FKProcurementProject.ProjectCoordinator).EmployeeName,
//                BidType = d.BidType,
//                BidReferenceNo = d.BidReferenceNo,
//                SupplierReference = d.SupplierReference,
//                ProcurementProject = d.ProcurementProject,
//                IsBidSecurityRequired = d.IsBidSecurityRequired,
//                IsPerformaceSecurityRequired = d.IsPerformaceSecurityRequired,
//                BidDetails = db.BidDetails.Where(x => x.BidReference == d.ID).ToList().Select(x => new BidDetailsVM {
//                    BidAction = x.BidAction,
//                    BidReference = x.BidReference,
//                    ArticleReference = x.ArticleReference,
//                    ItemSequence = x.ItemSequence,
//                    ItemFullName = x.ItemFullName,
//                    ItemSpecifications = x.ItemSpecifications,
//                    Unit = x.FKUOMReference.Abbreviation,
//                    UOMReference = x.UOMReference,
//                    Quantity = x.Quantity,
//                    BidUnitPrice = x.BidUnitPrice.Value,
//                    BidTotalPrice = x.BidTotalPrice
//                }).ToList()
//            }).FirstOrDefault();
//        }


//        public void UpdateContractStatus(string ContractCode, string UserEmail)
//        {
//            var user = db.UserAccounts.Where(d => d.Email == UserEmail).FirstOrDefault();
//            var contract = db.ProcurementProjects.Where(d => d.ContractCode == ContractCode).FirstOrDefault();
//            contract.ProcurementProjectStage = ProcurementProjectStages.BiddingFailed;
//            db.ContractUpdates.Add(new ContractUpdates
//            {
//                ProcurementProject = contract.ID,
//                ProcurementProjectStage = ProcurementProjectStages.BiddingFailed,
//                UpdatedAt = DateTime.Now,
//                UpdatedBy = user.EmpCode,
//                Remarks = "Bids registration is closed."
//            });

//            db.SaveChanges();
//        }
//        private string GenerateBidReferenceNo(string ContractCode, BidTypes BidType)
//        {
//            var contract = db.ProcurementProjects.Where(d => d.ContractCode == ContractCode).FirstOrDefault();
//            var series = (db.BidsHeader.Where(d => d.ProcurementProject == contract.ID).Count() + 1).ToString();
//            var referenceNo = BidType.GetAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>().ShortName + "-" + contract.FiscalYear.ToString() + "-" + contract.ProcurementProjectType.GetAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>().ShortName + "-" + (series.Length == 1 ? "00" + series : series.Length == 2 ? "0" + series : series);
//            return referenceNo;
//        }
//        protected override void Dispose(bool disposing)
//        {
//            if (disposing)
//            {
//                db.Dispose();
//                abis.Dispose();
//                hris.Dispose();
//                systemBDL.Dispose();
//            }
//            base.Dispose(disposing);
//        }
//    }
//}