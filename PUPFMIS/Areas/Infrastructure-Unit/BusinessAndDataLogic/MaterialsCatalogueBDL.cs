using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PUPFMIS.Models;

namespace PUPFMIS.BusinessAndDataLogic
{
    public class MaterialsCatalogueBDL : Controller
    {
        FMISDbContext db = new FMISDbContext();
        ABISDataAccess abis = new ABISDataAccess();
        HRISDataAccess hris = new HRISDataAccess();
        SystemBDL system = new SystemBDL();

        public List<MaterialsCatalogueVM> GetMaterialsCatalogue()
        {
            return db.InfrastructureMaterials.Where(d => d.PurgeFlag == false).Select(d =>
            new MaterialsCatalogueVM
            {
                ID = d.ID,
                ItemName = d.ItemName,
                ItemSpecifications = d.ItemSpecifications ?? "Not Specified/Not Applicable",
                UnitOfMeasure = d.FKUOMReference.UnitName,
                WorkClassification = d.FKWorkClassificationReference.ClassificationName,
                WorkRequirement = d.FKWorkRequirementReference.Requirement ?? "Not Applicable"
            }).ToList();
        }
        public InfrastructureDetailedEstimateVM GetMaterial(int ID)
        {
            return db.InfrastructureMaterials.Where(d => d.ID == ID && d.PurgeFlag == false).Select(d => new InfrastructureDetailedEstimateVM
            {
                ID = d.ID,
                Material = d.ItemName,
                MaterialSpecification = d.ItemSpecifications ?? "Not Specified/Not Applicable",
                UOM = d.FKUOMReference.UnitName,
                Classification = d.FKWorkClassificationReference.ClassificationName,
                Requirement = d.FKWorkRequirementReference.Requirement ?? "Not Applicable"
            }).FirstOrDefault();
        }
        public bool PostToProject(MaterialsBasket MaterialsBasket, string UserEmail)
        {
            var user = db.UserAccounts.Where(d => d.Email == UserEmail).FirstOrDefault();
            var infraProject = db.InfrastructureProject.Where(d => d.InfraProjectCode == MaterialsBasket.InfraProjectCode).FirstOrDefault();
            var materialItems = MaterialsBasket.ItemList;
            var detailedEstimates = new List<InfrastructureDetailedEstimate>();
            foreach(var item in materialItems)
            {
                var material = db.InfrastructureMaterials.Find(item.ID);
                detailedEstimates.Add(new InfrastructureDetailedEstimate
                {
                    InfrastructureProjectReference = infraProject.ID,
                    InfrastructureMaterialReference = item.ID,
                    InfrastructureWorkClassification = (int)material.WorkClassificationReference,
                    InfrastructureWorkRequirement = material.WorkRequirementReference,
                    UOMReference = material.UOMReference,
                    Quantity = item.Quantity,
                    ItemUnitCost = item.ItemUnitCost,
                    ItemTotalCost = item.ItemTotalCost,
                    LaborUnitCost = item.LaborUnitCost,
                    LaborTotalCost = item.LaborTotalCost,
                    EstimatedDirectCost = item.EstimatedDirectCost,
                    MobDemobilizationCost = item.MobDemobilizationCost,
                    OCMCost = item.OCMCost,
                    ProfitCost = item.ProfitCost,
                    TotalMarkUp = item.TotalMarkUp,
                    VAT = item.VAT,
                    TotalIndirectCost = item.TotalIndirectCost,
                    TotalAmount = item.TotalAmount
                });
            }

            db.InfrastructureDetailedEstimate.AddRange(detailedEstimates);
            if(db.SaveChanges() == 0)
            {
                return false;
            }
            return true;
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