using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PUPFMIS.Models;

namespace PUPFMIS.BusinessAndDataLogic
{
    public class ProcurementProjectBL : Controller
    {
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
            }
            base.Dispose(disposing);
        }
    }

    public class ProcurementProjectsDAL : Controller
    {
        private FMISDbContext db = new FMISDbContext();
        private HRISDataAccess hrisDataAccess = new HRISDataAccess();
        private ABISDataAccess abisDataAccess = new ABISDataAccess();

        public List<ProcurementProgramVM> GetProcurementPrograms(BiddingTypes BiddingType)
        {
            var programs = new List<ProcurementProgramVM>();
            if(BiddingType == BiddingTypes.ProgramBased)
            {
                var projectItems = (from appDetails in db.APPDetails.ToList()
                                    join items in db.ProjectPlanItems.ToList() on appDetails.ID equals items.APPLineReference
                                    join objectClasses in abisDataAccess.GetChartOfAccounts() on appDetails.ObjectClassification equals objectClasses.UACS_Code
                                    where items.Status == "Posted to APP" &&
                                          items.FKItemReference.ProcurementSource == ProcurementSources.Non_DBM
                                    select new ProcurementProgramVM
                                    {
                                        Code = objectClasses.UACS_Code,
                                        ProgramName = objectClasses.AcctName
                                    }).ToList();

                var projectServices = (from appDetails in db.APPDetails.ToList()
                                       join services in db.ProjectPlanServices.ToList() on appDetails.ID equals services.APPLineReference
                                       join objectClasses in abisDataAccess.GetChartOfAccounts() on appDetails.ObjectClassification equals objectClasses.UACS_Code
                                       where services.Status == "Posted to APP" &&
                                             services.FKItemReference.ProcurementSource == ProcurementSources.Non_DBM
                                       select new ProcurementProgramVM
                                       {
                                           Code = objectClasses.UACS_Code,
                                           ProgramName = objectClasses.AcctName
                                       }).ToList();

                var appPrograms = projectItems.Union(projectServices).ToList();
                appPrograms = appPrograms
                              .GroupBy(d => new { d.Code, d.ProgramName })
                              .Select(d => new ProcurementProgramVM {
                                  Code = d.Key.Code,
                                  ProgramName = d.Key.ProgramName
                              }).ToList();

                programs = appPrograms;
            }
            else
            {
                var projectsFromItems = (from project in db.ProjectPlans.ToList()
                                         join item in db.ProjectPlanItems.ToList() on project.ID equals item.ProjectReference
                                         join appDetails in db.APPDetails.ToList() on item.APPLineReference equals appDetails.ID
                                         where item.Status == "Posted to APP" && 
                                               item.FKItemReference.ProcurementSource == ProcurementSources.Non_DBM &&
                                               appDetails.APPModeOfProcurementReference.Contains("1")
                                         select new {
                                            Code = project.ProjectCode,
                                            ProgramName = project.ProjectName
                                         }).Distinct().ToList()
                                         .Select(d => new ProcurementProgramVM
                                         {
                                             Code = d.Code,
                                             ProgramName = d.ProgramName
                                         }).ToList();

                var projectsFromServices = (from project in db.ProjectPlans.ToList()
                                            join service in db.ProjectPlanServices.ToList() on project.ID equals service.ProjectReference
                                            join appDetails in db.APPDetails.ToList() on service.APPLineReference equals appDetails.ID
                                            where service.Status == "Posted to APP" &&
                                                  service.FKItemReference.ProcurementSource == ProcurementSources.Non_DBM &&
                                                  appDetails.APPModeOfProcurementReference.Contains("1")
                                            select new
                                            {
                                                Code = project.ProjectCode,
                                                ProgramName = project.ProjectName
                                            }).Distinct().ToList()
                                            .Select(d => new ProcurementProgramVM
                                            {
                                                Code = d.Code,
                                                ProgramName = d.ProgramName
                                            }).ToList();

                var projects = projectsFromItems.Union(projectsFromServices).Distinct().ToList();
                programs = projects;
            }
            return programs;
        }

        public BiddingProjectVM GetBiddingItemsProjectBased(BiddingTypes BiddingType, string ProgramCode, BiddingStrategies BiddingStrategy)
        {
            var biddingProject = new BiddingProjectVM();
            var project = db.ProjectPlans.Where(d => d.ProjectCode == ProgramCode).FirstOrDefault();
            var projectItems = (from items in db.ProjectPlanItems.ToList()
                                join appDetails in db.APPDetails.ToList() on items.APPLineReference equals appDetails.ID
                                where items.FKProjectReference.ProjectCode == project.ProjectCode &&
                                      items.FKItemReference.FKItemTypeReference.FKInventoryTypeReference.InventoryCode != "CUOS" &&
                                      items.Status == "Posted to APP" &&
                                      appDetails.APPModeOfProcurementReference.Contains("1")
                                select new
                                {
                                    ItemCode = items.FKItemReference.ItemCode,
                                    ItemName = items.FKItemReference.ItemShortSpecifications,
                                    ItemSpecifications = items.FKItemReference.ItemSpecifications,
                                    UnitOfMeasure = items.FKItemReference.FKIndividualUnitReference.Abbreviation,
                                    Quantity = items.PPMPTotalQty,
                                    ModesOfProcurement = appDetails.APPModeOfProcurementReference,
                                    ApprovedBudget = items.PPMPEstimatedBudget,
                                    ReferenceAPP = appDetails.FKAPPHeaderReference.ReferenceNo + "\n" + appDetails.PAPCode
                                }).ToList();
            var projectServices = (from services in db.ProjectPlanServices.ToList()
                                   join appDetails in db.APPDetails.ToList() on services.APPLineReference equals appDetails.ID
                                   where services.FKProjectReference.ProjectCode == project.ProjectCode &&
                                         services.FKItemReference.FKItemTypeReference.FKInventoryTypeReference.InventoryCode != "CUOS" &&
                                         services.Status == "Posted to APP" &&
                                         appDetails.APPModeOfProcurementReference.Contains("1")
                                   select new
                                   {
                                       ItemCode = services.FKItemReference.ItemCode,
                                       ItemName = services.FKItemReference.ItemShortSpecifications,
                                       ItemSpecifications = services.FKItemReference.ItemSpecifications,
                                       UnitOfMeasure = services.FKItemReference.IndividualUOMReference == null ? "N/A" : services.FKItemReference.FKIndividualUnitReference.Abbreviation,
                                       Quantity = services.PPMPQuantity,
                                       ModesOfProcurement = appDetails.APPModeOfProcurementReference,
                                       ApprovedBudget = services.PPMPEstimatedBudget,
                                       ReferenceAPP = appDetails.FKAPPHeaderReference.ReferenceNo + "\n" + appDetails.PAPCode
                                   }).ToList();
            var projectDetails = projectItems.Union(projectServices).ToList();

            biddingProject.BiddingType = BiddingType == BiddingTypes.ProgramBased ? "Program-based" : "Project-based";
            biddingProject.ProgramCode = project.ProjectCode;
            biddingProject.ProcurementProgram = project.ProjectName;
            biddingProject.BiddingDetails = new List<BiddingDetailsVM>();
            foreach (var item in projectDetails)
            {
                var modesOfProcurement = item.ModesOfProcurement.Split("_".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                var alternativeModesOfProcurement = string.Empty;
                foreach (var mode in modesOfProcurement)
                {
                    if (mode != "1")
                    {
                        var alternativeMode = Convert.ToInt32(mode);
                        alternativeModesOfProcurement += db.ProcurementModes.Where(d => d.ID == alternativeMode).First().ModeOfProcurementName + "\n";
                    }
                }
                biddingProject.BiddingDetails.Add(new BiddingDetailsVM
                {
                    IncludeToProject = alternativeModesOfProcurement == string.Empty ? true : false,
                    ItemCode = item.ItemCode,
                    ItemName = item.ItemName.ToUpper(),
                    ItemSpecifications = item.ItemSpecifications,
                    UnitOfMeasure = item.UnitOfMeasure,
                    EndUser = project.Department,
                    Quantity = item.Quantity,
                    AlternativeModes = alternativeModesOfProcurement,
                    ApprovedBudget = item.ApprovedBudget,
                    ReferenceAPP = item.ReferenceAPP
                });
            }
            return biddingProject;
        }

        public BiddingProjectVM GetBiddingItemsProgramBased(BiddingTypes BiddingType, string ProgramCode, BiddingStrategies BiddingStrategy)
        {
            var biddingProject = new BiddingProjectVM();
            var appLineItem = db.APPDetails.Where(d => d.ObjectClassification == ProgramCode).ToList();
            var biddingItems = (from items in db.ProjectPlanItems.ToList()
                                join appDetails in appLineItem on items.APPLineReference equals appDetails.ID
                                where items.FKItemReference.ProcurementSource == ProcurementSources.Non_DBM &&
                                      items.Status == "Posted to APP"
                                select new
                                {
                                    ItemCode = items.FKItemReference.ItemCode,
                                    ItemName = items.FKItemReference.ItemShortSpecifications,
                                    ItemSpecifications = items.FKItemReference.ItemSpecifications,
                                    UnitOfMeasure = items.FKItemReference.FKIndividualUnitReference.Abbreviation,
                                    Quantity = items.PPMPTotalQty,
                                    EstimateBudget = items.PPMPEstimatedBudget
                                }
                                into programItems
                                group programItems by new
                                {
                                    ItemCode = programItems.ItemCode,
                                    ItemName = programItems.ItemName,
                                    ItemSpecifications = programItems.ItemSpecifications,
                                    UnitOfMeasure = programItems.UnitOfMeasure
                                }
                                into groupedItems
                                select new
                                {
                                    ItemCode = groupedItems.Key.ItemCode,
                                    ItemName = groupedItems.Key.ItemName,
                                    ItemSpecifications = groupedItems.Key.ItemSpecifications,
                                    UnitOfMeasure = groupedItems.Key.UnitOfMeasure,
                                    Quantity = groupedItems.Sum(d => d.Quantity),
                                    ApprovedBudget = groupedItems.Sum(d => d.EstimateBudget),
                                    Type = "Item"
                                }).ToList();

            var biddingServices = (from services in db.ProjectPlanServices.ToList()
                                join appDetails in appLineItem on services.APPLineReference equals appDetails.ID
                                where services.FKItemReference.ProcurementSource == ProcurementSources.Non_DBM &&
                                      services.Status == "Posted to APP"
                                select new
                                {
                                    ItemCode = services.FKItemReference.ItemCode,
                                    ItemName = services.FKItemReference.ItemShortSpecifications,
                                    ItemSpecifications = services.FKItemReference.ItemSpecifications,
                                    UnitOfMeasure = services.FKItemReference.IndividualUOMReference == null ? "N/A" : services.FKItemReference.FKIndividualUnitReference.Abbreviation,
                                    Quantity = services.PPMPQuantity,
                                    EstimateBudget = services.PPMPEstimatedBudget
                                }
                                into programItems
                                group programItems by new
                                {
                                    ItemCode = programItems.ItemCode,
                                    ItemName = programItems.ItemName,
                                    ItemSpecifications = programItems.ItemSpecifications,
                                    UnitOfMeasure = programItems.UnitOfMeasure
                                }
                                into groupedItems
                                select new
                                {
                                    ItemCode = groupedItems.Key.ItemCode,
                                    ItemName = groupedItems.Key.ItemName,
                                    ItemSpecifications = groupedItems.Key.ItemSpecifications,
                                    UnitOfMeasure = groupedItems.Key.UnitOfMeasure,
                                    Quantity = groupedItems.Sum(d => d.Quantity),
                                    ApprovedBudget = groupedItems.Sum(d => d.EstimateBudget),
                                    Type = "Service"
                                }).ToList();

            var biddingProjectDetails = biddingItems.Union(biddingServices).ToList();

            var objectClass = abisDataAccess.GetChartOfAccounts(ProgramCode);
            biddingProject.ProgramCode = ProgramCode;
            biddingProject.ProcurementProgram = objectClass.SubAcctName + " - " + objectClass.AcctName;
            biddingProject.BiddingType = BiddingType == BiddingTypes.ProgramBased ? "Program-Based" : "Project-Based";
            biddingProject.BiddingDetails = new List<BiddingDetailsVM>();

            foreach(var item in biddingProjectDetails)
            {
                biddingProject.BiddingDetails.Add(new BiddingDetailsVM
                {
                    ItemCode = item.ItemCode,
                    ItemName = item.ItemName,
                    ItemSpecifications = item.ItemSpecifications,
                    Quantity = item.Quantity,
                    UnitOfMeasure = item.UnitOfMeasure,
                    ApprovedBudget = item.ApprovedBudget
                });
            }

            return biddingProject;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
                hrisDataAccess.Dispose();
                abisDataAccess.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}