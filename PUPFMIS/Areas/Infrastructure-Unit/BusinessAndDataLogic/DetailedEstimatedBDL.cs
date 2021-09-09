using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PUPFMIS.Models;

namespace PUPFMIS.BusinessAndDataLogic
{
    public class DetailedEstimatedBDL : Controller
    {
        FMISDbContext db = new FMISDbContext();
        ABISDataAccess abis = new ABISDataAccess();
        HRISDataAccess hris = new HRISDataAccess();
        SystemBDL system = new SystemBDL();

        public List<InfrastructureProjectVM> GetInfraProjectList()
        {
            return db.InfrastructureProject.ToList().Select(d => new InfrastructureProjectVM { 
                ID = d.ID,
                InfraProjectCode = d.InfraProjectCode,
                FiscalYear = d.FKEndUserProjectReference.FiscalYear,
                PAPCode = d.FKEndUserProjectReference.PAPCode,
                Program = abis.GetPrograms(d.FKEndUserProjectReference.PAPCode).GeneralDescription,
                ProjectCode = d.FKEndUserProjectReference.ProjectCode,
                EndUserProjectName = d.FKEndUserProjectReference.ProjectName,
                EndUserProjectReference = d.ID,
                Department = hris.GetDepartmentDetails(d.FKEndUserProjectReference.Department).Department,
                Unit = d.FKEndUserProjectReference.Unit == d.FKEndUserProjectReference.Department ? hris.GetDepartmentDetails(d.FKEndUserProjectReference.Department).Department : hris.GetDepartmentDetails(d.FKEndUserProjectReference.Department).Section,
                DeliveryMonth = system.GetMonthName(d.FKEndUserProjectReference.DeliveryMonth),
                Description = d.FKEndUserProjectReference.Description,
                ProjectTitle = d.ProjectTitle,
                ProjectLocation = d.ProjectLocation,
                ContractDuration = d.ContractDuration,
                ItemCode = d.FKArticleReference.ArticleCode + "-" + d.ItemSequence,
                InfraProjectType = db.Items.ToList().Where(x => x.ItemCode == d.FKArticleReference.ArticleCode + "-" + d.ItemSequence).FirstOrDefault().ItemFullName
            }).ToList();
        }
        public InfrastructureProjectVM GetInfraProjectDetails(string ProjectCode, string ItemCode)
        {
            var item = db.Items.ToList().Where(d => d.ItemCode == ItemCode).FirstOrDefault();
            var detailedEstimates = new List<InfrastructureDetailedEstimateVM>();
            return db.InfrastructureProject.Where(d => d.FKEndUserProjectReference.ProjectCode == ProjectCode && d.ArticleReference == item.ArticleReference && d.ItemSequence == item.Sequence).ToList().Select(d => new InfrastructureProjectVM
            {
                ID = d.ID,
                InfraProjectCode = d.InfraProjectCode,
                FiscalYear = d.FKEndUserProjectReference.FiscalYear,
                PAPCode = d.FKEndUserProjectReference.PAPCode,
                Program = abis.GetPrograms(d.FKEndUserProjectReference.PAPCode).GeneralDescription,
                ProjectCode = d.FKEndUserProjectReference.ProjectCode,
                EndUserProjectName = d.FKEndUserProjectReference.ProjectName,
                EndUserProjectReference = d.ID,
                Department = hris.GetDepartmentDetails(d.FKEndUserProjectReference.Department).Department,
                Unit = d.FKEndUserProjectReference.Unit == d.FKEndUserProjectReference.Department ? hris.GetDepartmentDetails(d.FKEndUserProjectReference.Unit).Department : hris.GetDepartmentDetails(d.FKEndUserProjectReference.Unit).Section,
                DeliveryMonth = system.GetMonthName(d.FKEndUserProjectReference.DeliveryMonth),
                Description = d.FKEndUserProjectReference.Description,
                ProjectTitle = d.ProjectTitle + " (" + d.InfraProjectCode + ")",
                ProjectLocation = d.ProjectLocation,
                ContractDuration = d.ContractDuration,
                ItemCode = d.FKArticleReference.ArticleCode + "-" + d.ItemSequence,
                InfraProjectType = db.Items.ToList().Where(x => x.ItemCode == d.FKArticleReference.ArticleCode + "-" + d.ItemSequence).FirstOrDefault().ItemFullName,
                DetailedEstimates = detailedEstimates
            }).FirstOrDefault();
        }
        public InfrastructureProjectVM GetInfraProjectDetails(string InfraProjectCode)
        {
            var detailedEstimates = db.InfrastructureDetailedEstimate.Where(d => d.FKInfraProjectReference.InfraProjectCode == InfraProjectCode)
                .Select(d => new InfrastructureDetailedEstimateVM {
                    ID = d.FKInfraMaterialsReference.ID,
                    Material = d.FKInfraMaterialsReference.ItemName,
                    MaterialSpecification = d.FKInfraMaterialsReference.ItemSpecifications,
                    Classification = d.FKWorkClass.ClassificationName,
                    Requirement = d.FKWorkRequirement.Requirement,
                    UOM = d.FKUOMReference.UnitName,
                    Quantity = d.Quantity,
                    ItemUnitCost = d.ItemUnitCost,
                    ItemTotalCost = d.ItemTotalCost,
                    LaborUnitCost = d.LaborUnitCost,
                    LaborTotalCost = d.LaborTotalCost,
                    EstimatedDirectCost = d.EstimatedDirectCost,
                    MobDemobilizationCost = d.MobDemobilizationCost,
                    OCMCost = d.OCMCost,
                    ProfitCost = d.ProfitCost,
                    TotalMarkUp = d.TotalMarkUp,
                    VAT = d.VAT,
                    TotalIndirectCost = d.TotalIndirectCost,
                    TotalAmount = d.TotalAmount
                }).ToList();
            return db.InfrastructureProject.Where(d => d.InfraProjectCode == InfraProjectCode).ToList().Select(d => new InfrastructureProjectVM
            {
                ID = d.ID,
                InfraProjectCode = d.InfraProjectCode,
                FiscalYear = d.FKEndUserProjectReference.FiscalYear,
                PAPCode = d.FKEndUserProjectReference.PAPCode,
                Program = abis.GetPrograms(d.FKEndUserProjectReference.PAPCode).GeneralDescription,
                ProjectCode = d.FKEndUserProjectReference.ProjectCode,
                EndUserProjectName = d.FKEndUserProjectReference.ProjectName,
                EndUserProjectReference = d.ID,
                Department = hris.GetDepartmentDetails(d.FKEndUserProjectReference.Department).Department,
                Unit = d.FKEndUserProjectReference.Unit == d.FKEndUserProjectReference.Department ? hris.GetDepartmentDetails(d.FKEndUserProjectReference.Unit).Department : hris.GetDepartmentDetails(d.FKEndUserProjectReference.Unit).Section,
                DeliveryMonth = system.GetMonthName(d.FKEndUserProjectReference.DeliveryMonth),
                Description = d.FKEndUserProjectReference.Description,
                ProjectTitle = d.ProjectTitle + " (" + d.InfraProjectCode + ")",
                ProjectLocation = d.ProjectLocation,
                ContractDuration = d.ContractDuration,
                ItemCode = d.FKArticleReference.ArticleCode + "-" + d.ItemSequence,
                InfraProjectType = db.Items.ToList().Where(x => x.ItemCode == d.FKArticleReference.ArticleCode + "-" + d.ItemSequence).FirstOrDefault().ItemFullName,
                DetailedEstimates = detailedEstimates
            }).FirstOrDefault();
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
                abis.Dispose();
                hris.Dispose();
                system.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}