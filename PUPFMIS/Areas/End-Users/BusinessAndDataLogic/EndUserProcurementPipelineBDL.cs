using PUPFMIS.Models;
using PUPFMIS.Models.HRIS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace PUPFMIS.BusinessAndDataLogic
{
    public class EndUserProcurementPipelineDAL : Controller
    {
        private FMISDbContext db = new FMISDbContext();
        private HRISDataAccess hris = new HRISDataAccess();
        private ABISDataAccess abis = new ABISDataAccess();
        private SystemBDL system = new SystemBDL();

        public ProcurementPipelineDashboardVM GetProcurementPipelineDashboard(string UserEmail)
        {
            var dashboard = new ProcurementPipelineDashboardVM();
            var user = db.UserAccounts.Where(d => d.Email == UserEmail).FirstOrDefault();

            var ppmpItems = db.ProjectPlanItems.Where(d => d.FKProjectReference.Department == user.DepartmentCode && (d.Status != null && d.Status != "Disapproved") && d.FKProjectReference.ProjectStatus != "New Project").ToList();
            var ppmpServices = db.ProjectPlanServices.Where(d => d.FKProjectReference.Department == user.DepartmentCode && (d.Status != null && d.Status != "Disapproved") && d.FKProjectReference.ProjectStatus != "New Project").ToList();

            dashboard.PPMPItems = new List<ProcurementProjectItemsVM>();
            foreach (var item in ppmpItems)
            {
                dashboard.PPMPItems.Add(new ProcurementProjectItemsVM
                {
                    ProjectCode = item.FKProjectReference.ProjectCode,
                    ProjectName = item.FKProjectReference.ProjectName,
                    PPMPReference = item.FKPPMPReference.ReferenceNo,
                    ItemCode = item.FKItemReference.ItemCode,
                    ItemName = item.FKItemReference.ItemFullName,
                    ItemSpecifications = item.FKItemReference.ItemSpecifications,
                    ProcurementSource = item.FKItemReference.ProcurementSource == ProcurementSources.PS_DBM ? "Procurement System - Department of Budget and Management" : "Private Suppliers",
                    InventoryType = item.FKItemReference.FKItemTypeReference.FKInventoryTypeReference.InventoryTypeName,
                    ItemCategory = item.FKItemReference.FKCategoryReference.ItemCategoryName,
                    PackagingUOMReference = item.FKItemReference.FKPackagingUnitReference.UnitName,
                    IndividualUOMReference = item.FKItemReference.FKIndividualUnitReference.UnitName,
                    TotalQty = item.PPMPTotalQty,
                    EstimatedBudget = item.PPMPEstimatedBudget,
                    UnitCost = item.UnitCost
                });
            }

            foreach (var item in ppmpServices)
            {
                dashboard.PPMPItems.Add(new ProcurementProjectItemsVM
                {
                    ProjectCode = item.FKProjectReference.ProjectCode,
                    ProjectName = item.FKProjectReference.ProjectName,
                    PPMPReference = item.FKPPMPReference.ReferenceNo,
                    ItemCode = item.FKItemReference.ItemCode,
                    ItemName = item.FKItemReference.ItemFullName,
                    ItemSpecifications = item.FKItemReference.ItemSpecifications,
                    ProcurementSource = item.FKItemReference.ProcurementSource == ProcurementSources.PS_DBM ? "Procurement System - Department of Budget and Management" : "Private Suppliers",
                    InventoryType = item.FKItemReference.FKItemTypeReference.FKInventoryTypeReference.InventoryTypeName,
                    ItemCategory = item.FKItemReference.FKCategoryReference.ItemCategoryName,
                    TotalQty = item.PPMPQuantity,
                    EstimatedBudget = item.PPMPEstimatedBudget,
                    UnitCost = item.UnitCost
                });
            }

            var appItems = db.ProjectPlanItems.Where(d => d.FKProjectReference.Department == user.DepartmentCode && d.APPReference != null).ToList();
            var appServices = db.ProjectPlanServices.Where(d => d.FKProjectReference.Department == user.DepartmentCode && d.APPReference != null).ToList();

            dashboard.APPItems = new List<ProcurementProjectItemsVM>();
            foreach (var item in appItems)
            {
                dashboard.APPItems.Add(new ProcurementProjectItemsVM
                {
                    ProjectCode = item.FKProjectReference.ProjectCode,
                    ProjectName = item.FKProjectReference.ProjectName,
                    PPMPReference = item.FKPPMPReference.ReferenceNo,
                    ItemCode = item.FKItemReference.ItemCode,
                    ItemName = item.FKItemReference.ItemFullName,
                    ItemSpecifications = item.FKItemReference.ItemSpecifications,
                    ProcurementSource = item.FKItemReference.ProcurementSource == ProcurementSources.PS_DBM ? "Procurement System - Department of Budget and Management" : "Private Suppliers",
                    InventoryType = item.FKItemReference.FKItemTypeReference.FKInventoryTypeReference.InventoryTypeName,
                    ItemCategory = item.FKItemReference.FKCategoryReference.ItemCategoryName,
                    PackagingUOMReference = item.FKItemReference.FKPackagingUnitReference.UnitName,
                    IndividualUOMReference = item.FKItemReference.FKIndividualUnitReference.UnitName,
                    TotalQty = item.PPMPTotalQty,
                    EstimatedBudget = item.PPMPEstimatedBudget,
                    UnitCost = item.UnitCost
                });
            }

            foreach (var item in appServices)
            {
                dashboard.APPItems.Add(new ProcurementProjectItemsVM
                {
                    ProjectCode = item.FKProjectReference.ProjectCode,
                    ProjectName = item.FKProjectReference.ProjectName,
                    PPMPReference = item.FKPPMPReference.ReferenceNo,
                    ItemCode = item.FKItemReference.ItemCode,
                    ItemName = item.FKItemReference.ItemFullName,
                    ItemSpecifications = item.FKItemReference.ItemSpecifications,
                    ProcurementSource = item.FKItemReference.ProcurementSource == ProcurementSources.PS_DBM ? "Procurement System - Department of Budget and Management" : "Private Suppliers",
                    InventoryType = item.FKItemReference.FKItemTypeReference.FKInventoryTypeReference.InventoryTypeName,
                    ItemCategory = item.FKItemReference.FKCategoryReference.ItemCategoryName,
                    TotalQty = item.PPMPQuantity,
                    EstimatedBudget = item.PPMPEstimatedBudget,
                    UnitCost = item.UnitCost
                });
            }

            var purchaseRequestItems = db.ProjectPlanItems.Where(d => d.FKProjectReference.Department == user.DepartmentCode && d.APPReference != null && d.PRReference != null).ToList();
            var purchaseRequestServices = db.ProjectPlanServices.Where(d => d.FKProjectReference.Department == user.DepartmentCode && d.APPReference != null && d.PRReference != null).ToList();

            dashboard.PurchaseRequestItems = new List<ProcurementProjectItemsVM>();
            foreach (var item in purchaseRequestItems)
            {
                dashboard.PurchaseRequestItems.Add(new ProcurementProjectItemsVM
                {
                    ProjectCode = item.FKProjectReference.ProjectCode,
                    ProjectName = item.FKProjectReference.ProjectName,
                    PPMPReference = item.FKPPMPReference.ReferenceNo,
                    ItemCode = item.FKItemReference.ItemCode,
                    ItemName = item.FKItemReference.ItemFullName,
                    ItemSpecifications = item.FKItemReference.ItemSpecifications,
                    ProcurementSource = item.FKItemReference.ProcurementSource == ProcurementSources.PS_DBM ? "Procurement System - Department of Budget and Management" : "Private Suppliers",
                    InventoryType = item.FKItemReference.FKItemTypeReference.FKInventoryTypeReference.InventoryTypeName,
                    ItemCategory = item.FKItemReference.FKCategoryReference.ItemCategoryName,
                    PackagingUOMReference = item.FKItemReference.FKPackagingUnitReference.UnitName,
                    IndividualUOMReference = item.FKItemReference.FKIndividualUnitReference.UnitName,
                    TotalQty = item.PPMPTotalQty,
                    EstimatedBudget = item.PPMPEstimatedBudget,
                    UnitCost = item.UnitCost
                });
            }

            foreach (var item in purchaseRequestServices)
            {
                dashboard.PurchaseRequestItems.Add(new ProcurementProjectItemsVM
                {
                    ProjectCode = item.FKProjectReference.ProjectCode,
                    ProjectName = item.FKProjectReference.ProjectName,
                    PPMPReference = item.FKPPMPReference.ReferenceNo,
                    ItemCode = item.FKItemReference.ItemCode,
                    ItemName = item.FKItemReference.ItemFullName,
                    ItemSpecifications = item.FKItemReference.ItemSpecifications,
                    ProcurementSource = item.FKItemReference.ProcurementSource == ProcurementSources.PS_DBM ? "Procurement System - Department of Budget and Management" : "Private Suppliers",
                    InventoryType = item.FKItemReference.FKItemTypeReference.FKInventoryTypeReference.InventoryTypeName,
                    ItemCategory = item.FKItemReference.FKCategoryReference.ItemCategoryName,
                    TotalQty = item.PPMPQuantity,
                    EstimatedBudget = item.PPMPEstimatedBudget,
                    UnitCost = item.UnitCost
                });
            }

            var projectItems = db.ProjectPlanItems.Where(d => d.FKProjectReference.Department == user.DepartmentCode && d.FKProjectReference.ProjectStatus != "New Project").ToList();
            var projectServices = db.ProjectPlanServices.Where(d => d.FKProjectReference.Department == user.DepartmentCode && d.FKProjectReference.ProjectStatus != "New Project").ToList();

            dashboard.ItemStatus = new List<ProcurementProjectItemsVM>();
            foreach (var item in projectItems)
            {
                dashboard.ItemStatus.Add(new ProcurementProjectItemsVM
                {
                    ProjectCode = item.FKProjectReference.ProjectCode,
                    ProjectName = item.FKProjectReference.ProjectName,
                    PPMPReference = item.FKPPMPReference.ReferenceNo,
                    ItemCode = item.FKItemReference.ItemCode,
                    ItemName = item.FKItemReference.ItemFullName,
                    ItemSpecifications = item.FKItemReference.ItemSpecifications,
                    ProcurementSource = item.FKItemReference.ProcurementSource == ProcurementSources.PS_DBM ? "Procurement System - Department of Budget and Management" : "Private Suppliers",
                    InventoryType = item.FKItemReference.FKItemTypeReference.FKInventoryTypeReference.InventoryTypeName,
                    ItemCategory = item.FKItemReference.FKCategoryReference.ItemCategoryName,
                    PackagingUOMReference = item.FKItemReference.FKPackagingUnitReference.UnitName,
                    IndividualUOMReference = item.FKItemReference.FKIndividualUnitReference.UnitName,
                    TotalQty = item.PPMPTotalQty,
                    Status = item.Status,
                    EstimatedBudget = item.PPMPEstimatedBudget,
                    UnitCost = item.UnitCost
                });
            }

            foreach (var item in projectServices)
            {
                dashboard.ItemStatus.Add(new ProcurementProjectItemsVM
                {
                    ProjectCode = item.FKProjectReference.ProjectCode,
                    ProjectName = item.FKProjectReference.ProjectName,
                    PPMPReference = item.FKPPMPReference.ReferenceNo,
                    ItemCode = item.FKItemReference.ItemCode,
                    ItemName = item.FKItemReference.ItemFullName,
                    ItemSpecifications = item.FKItemReference.ItemSpecifications,
                    ProcurementSource = item.FKItemReference.ProcurementSource == ProcurementSources.PS_DBM ? "Procurement System - Department of Budget and Management" : "Private Suppliers",
                    InventoryType = item.FKItemReference.FKItemTypeReference.FKInventoryTypeReference.InventoryTypeName,
                    ItemCategory = item.FKItemReference.FKCategoryReference.ItemCategoryName,
                    TotalQty = item.PPMPQuantity,
                    EstimatedBudget = item.PPMPEstimatedBudget,
                    Status = item.Status,
                    UnitCost = item.UnitCost
                });
            }

            var prDAL = new PurchaseRequestDAL();
            dashboard.TotalProcurementItems = dashboard.ItemStatus.Count();
            dashboard.TotalItemsApproved = dashboard.PPMPItems.Count();
            dashboard.TotalItemsPostedInAPP = dashboard.APPItems.Count();
            dashboard.TotalItemsWithPurchaseRequest = dashboard.PurchaseRequestItems.Count();
            dashboard.ProcurementPrograms = new List<ProcurementProjectsVM>();
            dashboard.ProcurementPrograms.AddRange(prDAL.GetOpenForPRSubmission(user.Email).Where(d => d.Items != null).ToList());

            return dashboard;
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