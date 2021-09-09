using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using PUPFMIS.Models;
using PUPFMIS.Models.AIS;
using PUPFMIS.Models.HRIS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace PUPFMIS.BusinessAndDataLogic
{
    public class ProcurementProjectsDAL : Controller
    {
        private FMISDbContext db = new FMISDbContext();
        private HRISDataAccess hris = new HRISDataAccess();
        private ABISDataAccess abis = new ABISDataAccess();
        private SystemBDL systemBDL = new SystemBDL();

        public List<HRISEmployeeDetailsVM> GetProcurementEmployees()
        {
            var agency = db.AgencyDetails.FirstOrDefault();
            return hris.GetEmployees(agency.ProcurementOfficeReference);
        }
        public List<ModesOfProcurement> GetModesOfProcurement(string PAPCode)
        {
            var modesOfProcurement = new List<string>();
            var modes = db.APPDetails.Where(d => d.PAPCode == PAPCode).Select(d => d.APPModeOfProcurementReference).ToArray();
            foreach (var mode in modes)
            {
                modesOfProcurement.AddRange(mode.Split("_".ToCharArray(), StringSplitOptions.None));
            }
            return db.ProcurementModes.Where(d => modesOfProcurement.Contains(d.ID.ToString())).ToList();
        }
        public List<ProcurementProgramsVM> GetProcurementPrograms(int ModeOfProcurement)
        {
            return db.PPMPDetails.OrderBy(d => d.FKAPPDetailReference.ID).Where(d => d.PPMPDetailStatus == PPMPDetailStatus.PostedToAPP).ToList()
                    .Where(d => d.FKAPPDetailReference.APPModeOfProcurementReference.Contains(ModeOfProcurement.ToString()) &&
                                d.ProcurementSource == ProcurementSources.ExternalSuppliers &&
                                d.FKAPPDetailReference.FKAPPHeaderReference.ApprovedAt != null)
                    .Select(d => new
                    {
                        Code = d.FKAPPDetailReference.PAPCode,
                        ProgramName = d.FKAPPDetailReference.ProcurementProgram,
                        FundSource = d.FKAPPDetailReference.FundSourceReference
                    }).Distinct().ToList()
                    .Select(d => new ProcurementProgramsVM
                    {
                        PAPCode = d.Code,
                        ProgramName = d.ProgramName + " (" + abis.GetFundSources(d.FundSource).FUND_DESC.Replace("\r\n", "") + ")"
                    }).ToList();

        }
        public List<ContractProgramsVM> GetProcurementPrograms()
        {
            return db.PPMPDetails.Where(d => d.ProcurementProject == null && d.ProcurementSource == ProcurementSources.ExternalSuppliers && d.ArticleReference != null).OrderBy(d => d.FKAPPDetailReference.ID).ToList()
                   .Select(d => new
                   {
                       PAPCode = d.FKAPPDetailReference.PAPCode,
                       ProcurementProgram = d.FKAPPDetailReference.ProcurementProgram,
                       FundSource = d.FundSource
                   }).GroupBy(d => d).ToList()
                   .Select(d => new ContractProgramsVM
                   {
                       PAPCode = d.Key.PAPCode,
                       ProcurementProgram = d.Key.ProcurementProgram + " (" + abis.GetFundSources(d.Key.FundSource).FUND_DESC.Replace("\r\n", "") + ")"
                   }).ToList();
        }
        public List<ContractProgramsVM> GetA2AProcurementPrograms()
        {
            return db.PPMPDetails.Where(d => d.ProcurementProject == null && d.ProcurementSource == ProcurementSources.AgencyToAgency && d.ArticleReference != null).OrderBy(d => d.FKAPPDetailReference.ID).ToList()
                   .Select(d => new
                   {
                       PAPCode = d.FKAPPDetailReference.PAPCode,
                       ProcurementProgram = d.FKAPPDetailReference.ProcurementProgram,
                       FundSource = d.FundSource
                   }).GroupBy(d => d).ToList()
                   .Select(d => new ContractProgramsVM
                   {
                       PAPCode = d.Key.PAPCode,
                       ProcurementProgram = d.Key.ProcurementProgram + " (" + abis.GetFundSources(d.Key.FundSource).FUND_DESC.Replace("\r\n", "") + ")"
                   }).ToList();
        }
        public IndividualProcurementProjectVM SetupIndividualContract(ProcurementProjectSetupVM ContractSetup)
        {
            var appDetailReference = db.APPDetails.Where(d => d.PAPCode == ContractSetup.PAPCode).FirstOrDefault();
            var contractItems = GetProgramItems(appDetailReference.PAPCode);

            return new IndividualProcurementProjectVM
            {
                PAPCode = appDetailReference.PAPCode,
                Classification = appDetailReference.FKClassificationReference.GeneralClass + " - " + appDetailReference.FKClassificationReference.Classification,
                ClassificationReference = (int)appDetailReference.ClassificationReference,
                FiscalYear = appDetailReference.FKAPPHeaderReference.FiscalYear,
                FundSource = appDetailReference.FundSourceReference,
                FundDescription = abis.GetFundSources(appDetailReference.FundSourceReference).FUND_DESC,
                ProcurementProjectType = ProcurementProjectTypes.CPB,
                ContractStrategy = ContractSetup.ContractStrategy,
                ContractCode = GenerateContractCode(ProcurementProjectTypes.CPB, ContractSetup.ModeOfProcurement, ContractSetup.ContractStrategy, appDetailReference.FKAPPHeaderReference.FiscalYear),
                ContractName = appDetailReference.ProcurementProgram,
                ContractStatus = ProcurementProjectStatus.ContractCreated,
                ProcurementProjectStage = ProcurementProjectStages.ContractOpened,
                ApprovedBudgetForContract = contractItems.Sum(d => d.ApprovedBudgetForItem),
                ModeOfProcurementReference = ContractSetup.ModeOfProcurement,
                ModeOfProcurement = db.ProcurementModes.Find(ContractSetup.ModeOfProcurement).ModeOfProcurementName,
                ContractItems = contractItems
            };
        }
        public IndividualProcurementProjectVM SetupIndividualAlternativeContract(ProcurementProjectSetupVM ContractSetup)
        {
            var appDetailReference = db.APPDetails.Where(d => d.PAPCode == ContractSetup.PAPCode).FirstOrDefault();
            var contractItems = GetProgramItems(appDetailReference.PAPCode);

            return new IndividualProcurementProjectVM
            {
                PAPCode = appDetailReference.PAPCode,
                Classification = appDetailReference.FKClassificationReference.GeneralClass + " - " + appDetailReference.FKClassificationReference.Classification,
                ClassificationReference = (int)appDetailReference.ClassificationReference,
                FiscalYear = appDetailReference.FKAPPHeaderReference.FiscalYear,
                FundSource = appDetailReference.FundSourceReference,
                FundDescription = abis.GetFundSources(appDetailReference.FundSourceReference).FUND_DESC,
                ProcurementProjectType = ProcurementProjectTypes.CPB,
                ContractStrategy = ContractSetup.ContractStrategy,
                ContractCode = GenerateContractCode(ProcurementProjectTypes.CPB, ContractSetup.ModeOfProcurement, ContractSetup.ContractStrategy, appDetailReference.FKAPPHeaderReference.FiscalYear),
                ContractName = contractItems.Select(d => d.ArticleReference).ToList().Count == 1 ? appDetailReference.FKClassificationReference.ProjectPrefix.ToUpper() + " " + db.ItemArticles.Find(contractItems.FirstOrDefault().ArticleReference).ArticleName : appDetailReference.ProcurementProgram,
                ContractStatus = ProcurementProjectStatus.ContractCreated,
                ProcurementProjectStage = ProcurementProjectStages.ContractOpened,
                ApprovedBudgetForContract = contractItems.Sum(d => d.ApprovedBudgetForItem),
                ModeOfProcurementReference = ContractSetup.ModeOfProcurement,
                ModeOfProcurement = db.ProcurementModes.Find(ContractSetup.ModeOfProcurement).ModeOfProcurementName,
                ContractItems = contractItems
            };
        }
        public LotProcurementProjectVM SetupLotContract(ProcurementProjectSetupVM ContractSetup)
        {
            var individualContracts = new List<IndividualProcurementProjectVM>();
            var appDetailReference = db.APPDetails.Where(d => d.PAPCode == ContractSetup.PAPCode).FirstOrDefault();
            var contractDetails = GetProgramItems(appDetailReference.PAPCode);
            var articles = contractDetails.Select(d => d.ArticleReference).Distinct().ToList();

            var lotContract = new LotProcurementProjectVM
            {
                PAPCode = appDetailReference.PAPCode,
                ClassificationReference = (int)appDetailReference.ClassificationReference,
                Classification = appDetailReference.FKClassificationReference.GeneralClass + " - " + appDetailReference.FKClassificationReference.Classification,
                ModeOfProcurementReference = ContractSetup.ModeOfProcurement,
                ModeOfProcurement = db.ProcurementModes.Find(ContractSetup.ModeOfProcurement).ModeOfProcurementName,
                FiscalYear = appDetailReference.FKAPPHeaderReference.FiscalYear,
                FundSource = appDetailReference.FundSourceReference,
                FundDescription = abis.GetFundSources(appDetailReference.FundSourceReference).FUND_DESC,
                ProcurementProjectType = ContractSetup.ProcurementProjectType,
                ContractStrategy = ContractSetup.ContractStrategy,
                ContractCode = GenerateContractCode(ContractSetup.ProcurementProjectType, ContractSetup.ModeOfProcurement, ContractSetup.ContractStrategy, appDetailReference.FKAPPHeaderReference.FiscalYear),
                ContractName = appDetailReference.ProcurementProgram,
                ContractStatus = ProcurementProjectStatus.ContractCreated,
                ProcurementProjectStage = ProcurementProjectStages.ContractOpened,
                ApprovedBudgetForContract = contractDetails.Sum(d => d.ApprovedBudgetForItem),
                SubContracts = new List<IndividualProcurementProjectVM>()
            };

            var lotNo = 0;
            foreach (var article in articles)
            {
                lotNo++;
                var projectPrefix = db.ItemClassification.Find(lotContract.ClassificationReference).ProjectPrefix.ToUpper();
                var articleName = db.ItemArticles.Find(article).ArticleName;
                var abc = contractDetails.Where(d => d.ArticleReference == article).ToList().Sum(d => d.ApprovedBudgetForItem);

                lotContract.SubContracts.Add(new IndividualProcurementProjectVM
                {
                    LotNo = lotNo,
                    ParentProjectReference = lotContract.ContractCode,
                    PAPCode = lotContract.PAPCode,
                    ClassificationReference = lotContract.ClassificationReference,
                    Classification = lotContract.Classification,
                    ModeOfProcurementReference = lotContract.ModeOfProcurementReference,
                    ModeOfProcurement = lotContract.ModeOfProcurement,
                    FiscalYear = lotContract.FiscalYear,
                    FundSource = lotContract.FundSource,
                    FundDescription = lotContract.FundSource,
                    ProcurementProjectType = lotContract.ProcurementProjectType,
                    ContractStrategy = lotContract.ContractStrategy,
                    ContractCode = lotContract.ContractCode + (lotNo.ToString().Length == 1 ? ("-0" + lotNo.ToString()) : ("-" + lotNo.ToString())),
                    ContractName = projectPrefix + " " + articleName,
                    ContractStatus = lotContract.ContractStatus,
                    ProcurementProjectStage = lotContract.ProcurementProjectStage,
                    ApprovedBudgetForContract = abc,
                    ContractItems = contractDetails.Where(d => d.ArticleReference == article)
                                    .Select(d => new ProcurementProjectDetailsVM
                                    {
                                        ArticleReference = d.ArticleReference,
                                        ItemSequence = d.ItemSequence,
                                        ItemFullName = d.ItemFullName,
                                        ItemSpecifications = d.ItemSpecifications,
                                        EstimatedUnitCost = d.EstimatedUnitCost,
                                        UOMReference = d.UOMReference,
                                        UnitOfMeasure = d.UnitOfMeasure,
                                        ApprovedBudgetForItem = d.ApprovedBudgetForItem,
                                        Quantity = d.Quantity
                                    }).ToList()
                });
            }
            return lotContract;
        }
        public bool PostIndividualContract(IndividualProcurementProjectVM Contract, string UserEmail)
        {
            var user = db.UserAccounts.Where(d => d.Email == UserEmail).FirstOrDefault();
            var appReference = db.APPDetails.Where(d => d.PAPCode == Contract.PAPCode).FirstOrDefault();
            var contract = new ProcurementProject
            {
                ParentProjectReference = null,
                APPReference = appReference.ID,
                ClassificationReference = Contract.ClassificationReference,
                ModeOfProcurementReference = Contract.ModeOfProcurementReference,
                FiscalYear = Contract.FiscalYear,
                FundSource = Contract.FundSource,
                ProcurementProjectType = Contract.ProcurementProjectType,
                ContractStrategy = Contract.ContractStrategy,
                ContractCode = Contract.ContractCode,
                ContractName = Contract.ContractName,
                ContractLocation = Contract.ContractLocation,
                ContractStatus = Contract.ContractStatus,
                ProcurementProjectStage = Contract.ProcurementProjectStage,
                ApprovedBudgetForContract = Contract.ApprovedBudgetForContract,
                DeliveryPeriod = Contract.DeliveryPeriod,
                ProjectCoordinator = Contract.ProjectCoordinator,
                PRSubmissionOpen = Contract.ModeOfProcurementReference == 10 ? DateTime.Now : Contract.PRSubmissionOpen,
                PRSubmissionClose = Contract.ModeOfProcurementReference == 10 ? DateTime.Now.AddDays(30) : Contract.PRSubmissionClose,
                PreProcurementConference = Contract.ModeOfProcurementReference == 10 ? (DateTime?)null : Contract.PreProcurementConference,
                IBPreparation = Contract.IBPreparation,
                PostingOfIB_RFQPosting = Contract.ModeOfProcurementReference == 10 ? (DateTime?)null : Contract.PostingOfIB,
                PreBidConference = Contract.ModeOfProcurementReference == 10 ? null : Contract.PreBidConference,
                DeadlineOfSubmissionOfBids_DeadlineOfSubmisionOfBids = Contract.ModeOfProcurementReference == 10 ? (DateTime?)null : Contract.DeadlineOfSubmissionOfBids,
                OpeningOfBids_OpeningOfQuotations = Contract.ModeOfProcurementReference == 10 ? (DateTime?)null : Contract.OpeningOfBids,
                NOAIssuance = Contract.ModeOfProcurementReference == 10 ? null : Contract.NOAIssuance,
                NTPIssuance = Contract.ModeOfProcurementReference == 10 ? null : Contract.NTPIssuance,
                CreatedAt = DateTime.Now,
                CreatedBy = user.EmpCode
            };

            db.ProcurementProjects.Add(contract);
            if (db.SaveChanges() == 0)
            {
                return false;
            }

            db.ProcurementProjectDetails.AddRange(Contract.ContractItems.Select(d => new ProcurementProjectDetails
            {
                ProcurementProjectReference = contract.ID,
                ArticleReference = d.ArticleReference,
                ItemSequence = d.ItemSequence,
                ItemFullName = d.ItemFullName,
                ItemSpecifications = d.ItemSpecifications,
                Quantity = d.Quantity,
                EstimatedUnitCost = d.EstimatedUnitCost,
                ApprovedBudgetForItem = d.ApprovedBudgetForItem,
                UOMReference = d.UOMReference
            }).ToList());

            var ppmpDetails = db.PPMPDetails.Where(d => d.FKAPPDetailReference.PAPCode == Contract.PAPCode).ToList();
            ppmpDetails.ForEach(d =>
            {
                d.PPMPDetailStatus = PPMPDetailStatus.PostedToProcurementProject;
                d.FKProjectDetailsReference.ProjectItemStatus = ProjectDetailsStatus.PostedToProcurementProject;
                d.ProcurementProjectType = Contract.ProcurementProjectType;
                d.ProcurementProject = contract.ID;
            });

            db.ContractUpdates.Add(new ContractUpdates
            {
                ProcurementProjectReference = contract.ID,
                UpdatedBy = user.EmpCode,
                UpdatedAt = DateTime.Now,
                Remarks = contract.ContractName + " (" + contract.ContractCode + ") is CREATED and OPENED.",
                ProcurementProjectStage = ProcurementProjectStages.ContractOpened
            });

            if (db.SaveChanges() == 0)
            {
                return false;
            }
            return true;
        }
        public bool PostLotContract(LotProcurementProjectVM LotContract, string UserEmail)
        {
            var user = db.UserAccounts.Where(d => d.Email == UserEmail).FirstOrDefault();
            var appReference = db.APPDetails.Where(d => d.PAPCode == LotContract.PAPCode).FirstOrDefault();
            var ParentProjectReference = new ProcurementProject
            {
                ParentProjectReference = null,
                APPReference = appReference.ID,
                ClassificationReference = LotContract.ClassificationReference,
                ModeOfProcurementReference = LotContract.ModeOfProcurementReference,
                FiscalYear = LotContract.FiscalYear,
                FundSource = LotContract.FundSource,
                ProcurementProjectType = LotContract.ProcurementProjectType,
                ContractStrategy = LotContract.ContractStrategy,
                ContractCode = LotContract.ContractCode,
                ContractName = LotContract.ContractName,
                ContractLocation = LotContract.ContractLocation,
                ContractStatus = LotContract.ContractStatus,
                ProcurementProjectStage = LotContract.ProcurementProjectStage,
                ApprovedBudgetForContract = LotContract.ApprovedBudgetForContract,
                ProjectCoordinator = LotContract.ProjectCoordinator,
                CreatedAt = DateTime.Now,
                CreatedBy = user.EmpCode
            };

            db.ProcurementProjects.Add(ParentProjectReference);
            if (db.SaveChanges() == 0)
            {
                return false;
            }

            foreach (var subcontract in LotContract.SubContracts)
            {
                var contract = new ProcurementProject
                {
                    ParentProjectReference = ParentProjectReference.ID,
                    APPReference = appReference.ID,
                    ClassificationReference = ParentProjectReference.ClassificationReference,
                    ModeOfProcurementReference = ParentProjectReference.ModeOfProcurementReference,
                    FiscalYear = ParentProjectReference.FiscalYear,
                    FundSource = ParentProjectReference.FundSource,
                    ProcurementProjectType = ParentProjectReference.ProcurementProjectType,
                    ContractStrategy = ParentProjectReference.ContractStrategy,
                    ContractCode = subcontract.ContractCode,
                    ContractName = subcontract.ContractName,
                    ContractLocation = ParentProjectReference.ContractLocation,
                    ContractStatus = ParentProjectReference.ContractStatus,
                    ProcurementProjectStage = ParentProjectReference.ProcurementProjectStage,
                    ApprovedBudgetForContract = subcontract.ApprovedBudgetForContract,
                    DeliveryPeriod = subcontract.DeliveryPeriod,
                    ProjectCoordinator = LotContract.ProjectCoordinator,
                    PRSubmissionOpen = LotContract.PRSubmissionOpen,
                    PRSubmissionClose = LotContract.PRSubmissionClose,
                    IBPreparation = LotContract.IBPreparation,
                    PostingOfIB_RFQPosting = LotContract.PostingOfIB,
                    PreProcurementConference = subcontract.PreProcurementConference,
                    PreBidConference = subcontract.PreBidConference,
                    DeadlineOfSubmissionOfBids_DeadlineOfSubmisionOfBids = subcontract.DeadlineOfSubmissionOfBids,
                    OpeningOfBids_OpeningOfQuotations = subcontract.OpeningOfBids,
                    NOAIssuance = LotContract.NOAIssuance,
                    NTPIssuance = LotContract.NTPIssuance,
                    CreatedAt = DateTime.Now,
                    CreatedBy = user.EmpCode
                };

                db.ProcurementProjects.Add(contract);
                if (db.SaveChanges() == 0)
                {
                    return false;
                }

                db.ProcurementProjectDetails.AddRange(subcontract.ContractItems.Select(d => new ProcurementProjectDetails
                {
                    ProcurementProjectReference = contract.ID,
                    ArticleReference = d.ArticleReference,
                    ItemSequence = d.ItemSequence,
                    ItemFullName = d.ItemFullName,
                    ItemSpecifications = d.ItemSpecifications,
                    Quantity = d.Quantity,
                    EstimatedUnitCost = d.EstimatedUnitCost,
                    ApprovedBudgetForItem = d.ApprovedBudgetForItem,
                    UOMReference = d.UOMReference
                }).ToList());

                var ppmpDetails = db.PPMPDetails.ToList()
                                    .Where(d => d.FKAPPDetailReference.PAPCode == LotContract.PAPCode &&
                                                subcontract.ContractItems.Select(x => x.ArticleReference).Contains(d.ArticleReference) &&
                                                subcontract.ContractItems.Select(x => x.ItemSequence).Contains(d.ItemSequence))
                                    .ToList();

                ppmpDetails.ForEach(d =>
                {
                    d.PPMPDetailStatus = PPMPDetailStatus.PostedToProcurementProject;
                    d.FKProjectDetailsReference.ProjectItemStatus = ProjectDetailsStatus.PostedToProcurementProject;
                    d.ProcurementProjectType = LotContract.ProcurementProjectType;
                    d.ProcurementProject = contract.ID;
                });

                if (db.SaveChanges() == 0)
                {
                    return false;
                }
            }

            db.ContractUpdates.Add(new ContractUpdates
            {
                ProcurementProjectReference = ParentProjectReference.ID,
                UpdatedBy = user.EmpCode,
                UpdatedAt = DateTime.Now,
                Remarks = ParentProjectReference.ContractName + " (" + ParentProjectReference.ContractCode + ") is CREATED and OPENED.",
                ProcurementProjectStage = ProcurementProjectStages.ContractOpened
            });


            if (db.SaveChanges() == 0)
            {
                return false;
            }

            return true;
        }

        public ContractStrategies GetContractStrategy(string ContractCode)
        {
            return db.ProcurementProjects.Where(d => d.ContractCode == ContractCode).Select(d => d.ContractStrategy).FirstOrDefault();
        }
        public ProcurementProjectStages GetProcurementProjectStage(string ContractCode)
        {
            return db.ProcurementProjects.Where(d => d.ContractCode == ContractCode).Select(d => d.ProcurementProjectStage).FirstOrDefault();
        }

        public OpenProcurementProjectVM GetSingleContractDetails(string ContractCode)
        {
            var contract = db.ProcurementProjects.ToList().Where(d => d.ContractCode == ContractCode).Select(d => new OpenProcurementProjectVM
            {
                PAPCode = d.FKAPPReference.PAPCode,
                ClassificationReference = d.ClassificationReference,
                Classification = d.FKClassificationReference.GeneralClass + " - " + d.FKClassificationReference.Classification,
                ModeOfProcurementReference = d.ModeOfProcurementReference,
                ModeOfProcurement = d.FKModeOfProcurementReference.ModeOfProcurementName,
                FiscalYear = d.FiscalYear,
                FundSource = d.FundSource,
                FundDescription = abis.GetFundSources(d.FundSource).FUND_DESC,
                ProcurementProjectType = d.ProcurementProjectType,
                ContractStrategy = d.ContractStrategy,
                ContractCode = d.ContractCode,
                ContractName = d.ContractName,
                ContractLocation = d.ContractLocation,
                ContractStatus = d.ContractStatus,
                ProcurementProjectStage = d.ProcurementProjectStage,
                ApprovedBudgetForContract = d.ApprovedBudgetForContract,
                DeliveryPeriod = (int)d.DeliveryPeriod,
                ProjectCoordinator = hris.GetEmployeeByCode(d.ProjectCoordinator).EmployeeName,
                PRSubmissionOpen = (DateTime)d.PRSubmissionOpen,
                PRSubmissionClose = (DateTime)d.PRSubmissionClose,
                PreProcurementConference = d.PreProcurementConference,
                PostingOfIB = (DateTime)d.PostingOfIB_RFQPosting,
                PreBidConference = d.PreBidConference,
                DeadlineOfSubmissionOfBids = (DateTime)d.DeadlineOfSubmissionOfBids_DeadlineOfSubmisionOfBids,
                OpeningOfBids = (DateTime)d.OpeningOfBids_OpeningOfQuotations,
                NOAIssuance = d.NOAIssuance,
                NTPIssuance = d.NTPIssuance,
                OpeningOfBidsFailureReason = d.OpeningOfBidsFailureReason,
                PostQualificationFailureReason = d.PostQualificationFailureReason,
                CreatedAt = d.CreatedAt,
                CreatedBy = hris.GetEmployeeByCode(d.CreatedBy).EmployeeName,
                ContractItems = d.ProcurementProjectStage == ProcurementProjectStages.PurchaseRequestSubmissionOpening ? GetProcurementProjectItems(d.ContractCode) : GetContractItems(d.ContractCode),
                Updates = db.ContractUpdates.Where(x => x.ProcurementProjectReference == d.ID).OrderByDescending(x => x.UpdatedAt).ToList().Select(x => new ContractUpdates
                {
                    ProcurementProjectReference = x.ProcurementProjectReference,
                    ProcurementProjectStage = x.ProcurementProjectStage,
                    Remarks = x.Remarks,
                    UpdatedAt = x.UpdatedAt,
                    UpdatedBy = x.UpdatedBy == null ? "SYSTEM" : hris.GetEmployeeByCode(x.UpdatedBy).EmployeeName
                }).ToList()
            }).FirstOrDefault();
            return contract;
        }
        public OpenProcurementProjectVM GetA2AContractDetails(string ContractCode)
        {
            var contract = db.ProcurementProjects.ToList().Where(d => d.ContractCode == ContractCode).Select(d => new OpenProcurementProjectVM
            {
                PAPCode = d.FKAPPReference.PAPCode,
                ClassificationReference = d.ClassificationReference,
                Classification = d.FKClassificationReference.GeneralClass + " - " + d.FKClassificationReference.Classification,
                ModeOfProcurementReference = d.ModeOfProcurementReference,
                ModeOfProcurement = d.FKModeOfProcurementReference.ModeOfProcurementName,
                FiscalYear = d.FiscalYear,
                FundSource = d.FundSource,
                FundDescription = abis.GetFundSources(d.FundSource).FUND_DESC,
                ProcurementProjectType = d.ProcurementProjectType,
                ContractStrategy = d.ContractStrategy,
                ContractCode = d.ContractCode,
                ContractName = d.ContractName,
                ContractLocation = d.ContractLocation,
                ContractStatus = d.ContractStatus,
                ProcurementProjectStage = d.ProcurementProjectStage,
                ApprovedBudgetForContract = d.ApprovedBudgetForContract,
                DeliveryPeriod = (int)d.DeliveryPeriod,
                ProjectCoordinator = hris.GetEmployeeByCode(d.ProjectCoordinator).EmployeeName,
                PRSubmissionOpen = (DateTime)d.PRSubmissionOpen,
                CreatedAt = d.CreatedAt,
                CreatedBy = hris.GetEmployeeByCode(d.CreatedBy).EmployeeName,
                ContractItems = GetProcurementProjectItems(d.ContractCode),
                Updates = db.ContractUpdates.Where(x => x.ProcurementProjectReference == d.ID).OrderByDescending(x => x.UpdatedAt).ToList().Select(x => new ContractUpdates
                {
                    ProcurementProjectReference = x.ProcurementProjectReference,
                    ProcurementProjectStage = x.ProcurementProjectStage,
                    Remarks = x.Remarks,
                    UpdatedAt = x.UpdatedAt,
                    UpdatedBy = x.UpdatedBy == null ? "SYSTEM" : hris.GetEmployeeByCode(x.UpdatedBy).EmployeeName
                }).ToList()
            }).FirstOrDefault();
            return contract;
        }
        public List<ProcurementProjectListVM> GetContractsOpenForSubmission(string UserEmail)
        {
            var user = db.UserAccounts.Where(d => d.Email == UserEmail).FirstOrDefault();
            var ppmpContracts = db.PPMPDetails.Where(d => d.ProcurementProject != null &&
                                                          d.PurchaseRequestReference == null &&
                                                          d.FKProcurementProject.ProcurementProjectStage == ProcurementProjectStages.PurchaseRequestSubmissionOpening &&
                                                          (d.FKProcurementProject.FKAPPReference.EndUser == "Various Offices" ? d.FKPPMPHeaderReference.Department == user.DepartmentCode : d.FKProcurementProject.FKAPPReference.EndUser == user.DepartmentCode))
                                              .Select(d => d.FKProcurementProject.ContractCode).Distinct().ToList();
            var contracts = db.ProcurementProjects.Where(d => ppmpContracts.Contains(d.ContractCode)).ToList()
            .Select(d => new ProcurementProjectListVM
            {
                ProcurementProjectType = d.ProcurementProjectType,
                ContractCode = d.ContractCode,
                ContractName = d.ContractName,
                ModeOfProcurementReference = d.ModeOfProcurementReference,
                ModeOfProcurement = d.FKModeOfProcurementReference.ModeOfProcurementName,
                FiscalYear = d.FiscalYear,
                ContractStrategy = d.ContractStrategy,
                ContractLocation = d.ContractLocation,
                ContractStatus = d.ContractStatus,
                ProcurementProjectStage = d.ProcurementProjectStage,
                ApprovedBudgetForContract = d.ApprovedBudgetForContract
            }).ToList();
            return contracts;
        }
        public List<ProcurementProjectListVM> GetContracts(string UserEmail)
        {
            var user = db.UserAccounts.Where(d => d.Email == UserEmail).FirstOrDefault();
            var purchaseRequests = db.PurchaseRequestHeader.Where(d => d.PRStatus == PurchaseRequestStatus.PurchaseRequestReceived).Select(d => d.FKProcurementProjectReference.ContractCode).ToList();
            return db.ProcurementProjects.Where(d => d.ProjectCoordinator == user.EmpCode && purchaseRequests.Contains(d.ContractCode)).ToList().Select(d => new
            {
                ProcurementProjectType = d.ContractStrategy == ContractStrategies.LotBidding ? d.FKParentProjectReference.ProcurementProjectType : d.ProcurementProjectType,
                ContractCode = d.ContractStrategy == ContractStrategies.LotBidding ? d.FKParentProjectReference.ContractCode : d.ContractCode,
                ContractName = d.ContractStrategy == ContractStrategies.LotBidding ? d.FKParentProjectReference.ContractName : d.ContractName,
                ModeOfProcurementReference = d.ContractStrategy == ContractStrategies.LotBidding ? d.FKParentProjectReference.ModeOfProcurementReference : d.ModeOfProcurementReference,
                ModeOfProcurement = d.ContractStrategy == ContractStrategies.LotBidding ? d.FKParentProjectReference.FKModeOfProcurementReference.ModeOfProcurementName : d.FKModeOfProcurementReference.ModeOfProcurementName,
                FiscalYear = d.ContractStrategy == ContractStrategies.LotBidding ? d.FKParentProjectReference.FiscalYear : d.FiscalYear,
                ContractLocation = d.ContractStrategy == ContractStrategies.LotBidding ? d.FKParentProjectReference.ContractLocation : d.ContractLocation,
                ContractStatus = d.ContractStrategy == ContractStrategies.LotBidding ? d.FKParentProjectReference.ContractStatus : d.ContractStatus,
                ContractStrategy = d.ContractStrategy == ContractStrategies.LotBidding ? d.FKParentProjectReference.ContractStrategy : d.ContractStrategy,
                ProcurementProjectStage = d.ContractStrategy == ContractStrategies.LotBidding ? d.FKParentProjectReference.ProcurementProjectStage : d.ProcurementProjectStage,
                ApprovedBudgetForContract = d.ContractStrategy == ContractStrategies.LotBidding ? d.FKParentProjectReference.ApprovedBudgetForContract : d.ApprovedBudgetForContract
            })
            .GroupBy(d => d)
            .Select(d => new ProcurementProjectListVM {
                ProcurementProjectType = d.Key.ProcurementProjectType,
                ContractCode = d.Key.ContractCode,
                ContractName = d.Key.ContractName,
                ModeOfProcurementReference = d.Key.ModeOfProcurementReference,
                ModeOfProcurement = d.Key.ModeOfProcurement,
                FiscalYear = d.Key.FiscalYear,
                ContractLocation = d.Key.ContractLocation,
                ContractStatus = d.Key.ContractStatus,
                ContractStrategy = d.Key.ContractStrategy,
                ProcurementProjectStage = d.Key.ProcurementProjectStage,
                ApprovedBudgetForContract = d.Key.ApprovedBudgetForContract
            }).OrderBy(d => d.ProcurementProjectStage).ToList();
        }
        public List<ProcurementProjectListVM> GetContracts(string UserEmail, ProcurementProjectStages ProcurementProjectStage)
        {
            return db.ProcurementProjects.Where(d => d.ProcurementProjectStage == ProcurementProjectStage && d.ParentProjectReference == null).ToList()
            .Select(d => new ProcurementProjectListVM
            {
                ProcurementProjectType = d.ProcurementProjectType,
                ContractCode = d.ContractCode,
                ContractName = d.ContractName,
                ModeOfProcurementReference = d.ModeOfProcurementReference,
                ModeOfProcurement = d.FKModeOfProcurementReference.ModeOfProcurementName,
                FiscalYear = d.FiscalYear,
                ContractStrategy = d.ContractStrategy,
                ContractLocation = d.ContractLocation,
                ContractStatus = d.ContractStatus,
                ProcurementProjectStage = d.ProcurementProjectStage,
                ApprovedBudgetForContract = d.ApprovedBudgetForContract
            }).ToList();

            //var contracts = lotContracts.Union(singleContracts).ToList();
            //return contracts;
        }
        public bool OpenPRSubmission(string ContractCode, string UserEmail)
        {
            var user = db.UserAccounts.Where(d => d.Email == UserEmail).FirstOrDefault();
            var ParentProjectReference = db.ProcurementProjects.Where(d => d.ContractCode == ContractCode).FirstOrDefault();
            var childContracts = db.ProcurementProjects.Where(d => d.ParentProjectReference == ParentProjectReference.ID).ToList();
            ParentProjectReference.ProcurementProjectStage = ProcurementProjectStages.PurchaseRequestSubmissionOpening;
            ParentProjectReference.ContractStatus = ProcurementProjectStatus.ContractProcurementOngoing;
            childContracts.ForEach(d => { d.ProcurementProjectStage = ProcurementProjectStages.PurchaseRequestSubmissionOpening; d.ContractStatus = ProcurementProjectStatus.ContractProcurementOngoing; });
            db.ContractUpdates.Add(new ContractUpdates
            {
                ProcurementProjectStage = ProcurementProjectStages.PurchaseRequestSubmissionOpening,
                Remarks = "Purchase Request submission for " + ContractCode + " is opened.",
                UpdatedAt = DateTime.Now,
                UpdatedBy = user.EmpCode,
                ProcurementProjectReference = ParentProjectReference.ID,
            });

            if (db.SaveChanges() == 0)
            {
                return false;
            }

            return true;
        }
        public bool ClosePRSubmission(string ContractCode, string UserEmail)
        {
            var user = db.UserAccounts.Where(d => d.Email == UserEmail).FirstOrDefault();
            var ParentProjectReference = db.ProcurementProjects.Where(d => d.ContractCode == ContractCode).FirstOrDefault();
            var childContracts = db.ProcurementProjects.Where(d => d.ParentProjectReference == ParentProjectReference.ID).ToList();
            ParentProjectReference.ProcurementProjectStage = ProcurementProjectStages.PRSubmissionClosed;
            childContracts.ForEach(d => { d.ProcurementProjectStage = ProcurementProjectStages.PRSubmissionClosed; });
            db.ContractUpdates.Add(new ContractUpdates
            {
                ProcurementProjectStage = ProcurementProjectStages.PRSubmissionClosed,
                Remarks = "Purchase Request submission for " + ContractCode + " is closed.",
                UpdatedAt = DateTime.Now,
                UpdatedBy = user.EmpCode,
                ProcurementProjectReference = ParentProjectReference.ID,
            });

            if (db.SaveChanges() == 0)
            {
                return false;
            }

            return true;
        }
        public List<ProcurementProjectDetailsVM> GetProgramItems(string PAPCode)
        {
            return db.PPMPDetails.Where(x => x.FKAPPDetailReference.PAPCode == PAPCode).ToList()
            .Select(x => new
            {
                ArticleReference = x.ArticleReference,
                ItemSequence = x.ItemSequence,
                ItemFullName = x.ItemFullName,
                ItemSpecifications = x.ItemSpecifications,
                Quantity = x.TotalQty,
                EstimatedUnitCost = x.UnitCost,
                ApprovedBudgetForItem = x.EstimatedBudget,
                UOMReference = x.UOMReference,
                UnitOfMeasure = x.FKUOMReference.Abbreviation
            })
            .GroupBy(x => new
            {
                x.ArticleReference,
                x.ItemSequence,
                x.ItemFullName,
                x.ItemSpecifications,
                x.UnitOfMeasure,
                x.UOMReference,
                x.EstimatedUnitCost
            })
            .Select(x => new ProcurementProjectDetailsVM
            {
                ArticleReference = x.Key.ArticleReference,
                ItemSequence = x.Key.ItemSequence,
                ItemFullName = x.Key.ItemFullName,
                ItemSpecifications = x.Key.ItemSpecifications,
                Quantity = x.Sum(d => d.Quantity),
                EstimatedUnitCost = x.Key.EstimatedUnitCost,
                ApprovedBudgetForItem = x.Sum(d => d.ApprovedBudgetForItem),
                UOMReference = x.Key.UOMReference,
                UnitOfMeasure = x.Key.UnitOfMeasure
            }).ToList();
        }
        public List<ProcurementProjectDetailsVM> GetProcurementProjectItems(string ContractCode)
        {
            return db.ProcurementProjectDetails.Where(x => x.FKProcurementProjectReference.ContractCode == ContractCode).ToList().Select(x => new ProcurementProjectDetailsVM
            {
                ProcurementProjectReference = x.ProcurementProjectReference,
                ArticleReference = x.ArticleReference,
                ItemSequence = x.ItemSequence,
                ItemFullName = x.ItemFullName,
                ItemSpecifications = x.ItemSpecifications,
                Quantity = x.Quantity,
                EstimatedUnitCost = x.EstimatedUnitCost,
                ApprovedBudgetForItem = x.ApprovedBudgetForItem,
                UOMReference = x.UOMReference,
                UnitOfMeasure = x.FKUOMReference.Abbreviation
            }).ToList();
        }
        public List<ProcurementProjectDetailsVM> GetContractItems(string ContractCode)
        {
            return db.ContractDetails.Where(x => x.FKContractReference.FKProcurementProjectReference.ContractCode == ContractCode).ToList().Select(x => new ProcurementProjectDetailsVM
            {
                ProcurementProjectReference = x.FKContractReference.ProcurementProjectReference,
                ArticleReference = x.ArticleReference,
                ItemSequence = x.ItemSequence,
                ItemFullName = x.ItemFullName,
                ItemSpecifications = x.ItemSpecifications,
                Quantity = x.Quantity,
                EstimatedUnitCost = x.ContractUnitPrice,
                ApprovedBudgetForItem = x.ContractTotalPrice,
                UOMReference = x.UOMReference,
                UnitOfMeasure = x.FKUOMReference.Abbreviation
            }).ToList();
        }
        private string GenerateContractCode(ProcurementProjectTypes ProcurementProjectType, int ModeOfProcurement, ContractStrategies ContractStrategy, int FiscalYear)
        {
            var modeOfProcurement = db.ProcurementModes.Find(ModeOfProcurement);
            var series = (db.ProcurementProjects.Where(d => d.FiscalYear == FiscalYear && d.ModeOfProcurementReference == ModeOfProcurement).Count() + 1).ToString();
            var contractStrategy = ContractStrategy.GetAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>().ShortName;
            series = series.Length == 1 ? "00" + series : series.Length == 2 ? "0" + series : series;
            var contractCode = modeOfProcurement.ShortName + "-" + contractStrategy + "-" + FiscalYear.ToString() + "-" + series;
            return contractCode;
        }

        public OpenLotProcurementProjectVM GetLotContractDetails(string ContractCode)
        {
            var procurementProjectDetails = db.ProcurementProjects.Where(d => d.FKParentProjectReference.ContractCode == ContractCode).ToList()
                .Select(d => new LotDetailsVM
                {
                    ContractCode = d.ContractCode,
                    ContractName = d.ContractName,
                    ApprovedBudgetForContract = d.ApprovedBudgetForContract,
                    DeliveryPeriod = d.DeliveryPeriod.Value,
                    ContractItems = GetProcurementProjectItems(d.ContractCode)
                }).ToList();

            var openLotDetails = db.ProcurementProjects.Where(d => d.FKParentProjectReference.ContractCode == ContractCode).ToList()
                .Select(d => new OpenLotProcurementProjectDetailsVM
                {
                    PRSubmissionOpen = d.PRSubmissionOpen.Value,
                    PRSubmissionClose = d.PRSubmissionClose.Value,
                    PreProcurementConference = d.PreProcurementConference.Value,
                    IBPreparation = d.IBPreparation.Value,
                    PostingOfIB = d.PostingOfIB_RFQPosting.Value,
                    PreBidConference = d.PreBidConference.Value,
                    DeadlineOfSubmissionOfBids = d.DeadlineOfSubmissionOfBids_DeadlineOfSubmisionOfBids.Value
                }).ToList();

            var procurementProject = db.ProcurementProjects.ToList().Where(d => d.ContractCode == ContractCode).Select(d => new OpenLotProcurementProjectVM
            {
                PAPCode = d.FKAPPReference.PAPCode,
                ClassificationReference = d.ClassificationReference,
                Classification = d.FKClassificationReference.GeneralClass + " - " + d.FKClassificationReference.Classification,
                ModeOfProcurementReference = d.ModeOfProcurementReference,
                ModeOfProcurement = d.FKModeOfProcurementReference.ModeOfProcurementName,
                FiscalYear = d.FiscalYear,
                FundSource = d.FundSource,
                FundDescription = abis.GetFundSources(d.FundSource).FUND_DESC,
                ProcurementProjectType = d.ProcurementProjectType,
                ContractStrategy = d.ContractStrategy,
                ContractCode = d.ContractCode,
                ContractName = d.ContractName,
                ContractLocation = d.ContractLocation,
                ContractStatus = d.ContractStatus,
                ProcurementProjectStage = d.ProcurementProjectStage,
                ApprovedBudgetForContract = d.ApprovedBudgetForContract,
                ProjectCoordinator = hris.GetEmployeeByCode(d.ProjectCoordinator).EmployeeName,
                OpenLotDetails = openLotDetails,
                CreatedAt = d.CreatedAt,
                CreatedBy = hris.GetEmployeeByCode(d.CreatedBy).EmployeeName,
                Details = procurementProjectDetails,
                Updates = db.ContractUpdates.Where(x => x.ProcurementProjectReference == d.ID).OrderByDescending(x => x.UpdatedAt).ToList().Select(x => new ContractUpdates
                {
                    ProcurementProjectReference = x.ProcurementProjectReference,
                    ProcurementProjectStage = x.ProcurementProjectStage,
                    Remarks = x.Remarks,
                    UpdatedAt = x.UpdatedAt,
                    UpdatedBy = x.UpdatedBy == null ? "SYSTEM" : hris.GetEmployeeByCode(x.UpdatedBy).EmployeeName
                }).ToList()
            }).FirstOrDefault();

            return procurementProject;
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
                abis.Dispose();
                hris.Dispose();
                systemBDL.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}