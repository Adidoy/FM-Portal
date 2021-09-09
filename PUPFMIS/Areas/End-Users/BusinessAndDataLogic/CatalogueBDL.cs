using PUPFMIS.Models;
using PUPFMIS.Models.HRIS;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace PUPFMIS.BusinessAndDataLogic
{
    public class CatalogueDAL : Controller
    {
        private FMISDbContext db = new FMISDbContext();
        private SystemBDL systemBDL = new SystemBDL();
        private ProjectPlansDAL projectPlanDAL = new ProjectPlansDAL();
        private HRISDataAccess hris = new HRISDataAccess();
        private ABISDataAccess abis = new ABISDataAccess();

        public BasketItems GetProjectDetails(string ProjectCode, string ItemCode)
        {
            SuppliersBL suppliersBL = new SuppliersBL();
            var itemReference = db.Items.ToList().Where(d => d.ItemCode == ItemCode).FirstOrDefault();
            return (from projectItem in db.ProjectDetails
                                          .Where(d => (d.ArticleReference == itemReference.ArticleReference && d.ItemSequence == itemReference.Sequence) &&
                                                       d.FKProjectPlanReference.ProjectCode == ProjectCode)
                                          .ToList()
                    select new BasketItems
                    {
                        ProposalType = projectItem.ProposalType,
                        ItemType = projectItem.FKItemArticleReference.FKItemTypeReference.ItemType,
                        Category = projectItem.FKCategoryReference.ItemCategoryName,
                        ItemCode = itemReference.ItemCode,
                        ItemName = projectItem.ItemFullName,
                        ArticleCode = projectItem.FKItemArticleReference.ArticleCode,
                        ItemSequence = projectItem.ItemSequence,
                        IsSpecsUserDefined = itemReference.IsSpecsUserDefined,
                        ItemSpecifications = itemReference.IsSpecsUserDefined == true ? projectItem.ItemSpecifications : itemReference.ItemSpecifications,
                        ProcurementSource = projectItem.ProcurementSource == ProcurementSources.AgencyToAgency ? "Department of Budget and Management - Procurement Service" : "External Suppliers",
                        AccountClass = abis.GetChartOfAccounts(projectItem.FKItemArticleReference.UACSObjectClass).AcctName,
                        ItemImage = itemReference.ItemImage,
                        UnitCost = projectItem.UnitCost,
                        IndividualUOMReference = projectItem.FKUOMReference.UnitName,
                        ResponsibilityCenter = projectItem.FKItemArticleReference.FKItemTypeReference.ResponsibilityCenter == null ? "Requesting Office" : hris.GetDepartmentDetails(projectItem.FKItemArticleReference.FKItemTypeReference.ResponsibilityCenter).Department,
                        PurchaseRequestCenter = projectItem.FKItemArticleReference.FKItemTypeReference.PurchaseRequestCenter == null ? "Requesting Office" : hris.GetDepartmentDetails(projectItem.FKItemArticleReference.FKItemTypeReference.PurchaseRequestCenter).Department,
                        Justification = projectItem.Justification,
                        JanQty = projectItem.JanQty,
                        FebQty = projectItem.FebQty,
                        MarQty = projectItem.MarQty,
                        AprQty = projectItem.AprQty,
                        MayQty = projectItem.MayQty,
                        JunQty = projectItem.JunQty,
                        JulQty = projectItem.JulQty,
                        AugQty = projectItem.AugQty,
                        SepQty = projectItem.SepQty,
                        OctQty = projectItem.OctQty,
                        NovQty = projectItem.NovQty,
                        DecQty = projectItem.DecQty,
                        TotalQty = projectItem.TotalQty,
                        Supplier1ID = projectItem.Supplier1Reference,
                        Supplier1Name = projectItem.Supplier1Reference == null ? null : suppliersBL.GetSupplierDetails(projectItem.Supplier1Reference).SupplierName,
                        Supplier1Address = projectItem.Supplier1Reference == null ? null : suppliersBL.GetSupplierDetails(projectItem.Supplier1Reference).Address,
                        Supplier1ContactNo = projectItem.Supplier1Reference == null ? null : suppliersBL.GetSupplierDetails(projectItem.Supplier1Reference).ContactNumber,
                        Supplier1EmailAddress = projectItem.Supplier1Reference == null ? null : suppliersBL.GetSupplierDetails(projectItem.Supplier1Reference).EmailAddress,
                        Supplier1UnitCost = projectItem.Supplier1UnitCost,
                        Supplier2ID = projectItem.Supplier2Reference,
                        Supplier2Name = projectItem.Supplier2Reference == null ? null : suppliersBL.GetSupplierDetails(projectItem.Supplier2Reference).SupplierName,
                        Supplier2Address = projectItem.Supplier2Reference == null ? null : suppliersBL.GetSupplierDetails(projectItem.Supplier2Reference).Address,
                        Supplier2ContactNo = projectItem.Supplier2Reference == null ? null : suppliersBL.GetSupplierDetails(projectItem.Supplier2Reference).ContactNumber,
                        Supplier2EmailAddress = projectItem.Supplier2Reference == null ? null : suppliersBL.GetSupplierDetails(projectItem.Supplier2Reference).EmailAddress,
                        Supplier2UnitCost = projectItem.Supplier2UnitCost,
                        Supplier3ID = projectItem.Supplier3Reference,
                        Supplier3Name = projectItem.Supplier3Reference == null ? null : suppliersBL.GetSupplierDetails(projectItem.Supplier3Reference).SupplierName,
                        Supplier3Address = projectItem.Supplier3Reference == null ? null : suppliersBL.GetSupplierDetails(projectItem.Supplier3Reference).Address,
                        Supplier3ContactNo = projectItem.Supplier3Reference == null ? null : suppliersBL.GetSupplierDetails(projectItem.Supplier3Reference).ContactNumber,
                        Supplier3EmailAddress = projectItem.Supplier3Reference == null ? null : suppliersBL.GetSupplierDetails(projectItem.Supplier3Reference).EmailAddress,
                        Supplier3UnitCost = projectItem.Supplier3UnitCost,
                        EstimatedBudget = projectItem.EstimatedBudget,
                        ProjectItemStatus = projectItem.ProjectItemStatus.GetAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>().Name,
                        UpdateFlag = projectItem.UpdateFlag
                    }).FirstOrDefault();
        }
        public decimal ComputeUnitCost(decimal? Supplier1UnitCost, decimal? Supplier2UnitCost, decimal? Supplier3UnitCost)
        {
            decimal unitCost = 0.00m;
            int count = 0;
            if (Supplier1UnitCost != null)
            {
                unitCost += (decimal)Supplier1UnitCost;
                count++;
            }
            if (Supplier2UnitCost != null)
            {
                unitCost += (decimal)Supplier2UnitCost;
                count++;
            }
            if (Supplier3UnitCost != null)
            {
                unitCost += (decimal)Supplier3UnitCost;
                count++;
            }
            unitCost = unitCost / count;
            return unitCost;
        }
        public List<Supplier> GetSuppliers(int Supplier1ID, int? Supplier2ID, int? Supplier3ID, string ItemCode)
        {
            var item = GetItem(ItemCode);
            var suppliersItemType = GetSuppliersItemType(item.ItemType);
            var suppliersCategory = GetSuppliersCategory(item.Category);
            var supplierIDs = suppliersItemType.Union(suppliersCategory).Distinct().ToList();

            var supplierList = new List<int>();

            supplierList.Add(Supplier1ID);
            if (Supplier2ID != null)
            {
                supplierList.Add((int)Supplier2ID);
            }
            if (Supplier3ID != null)
            {
                supplierList.Add((int)Supplier3ID);
            }
            var suppliers = db.Suppliers.Where(d => supplierIDs.Contains(d.ID) && (d.PurgeFlag == false) && (d.ID != 1)).ToList();
            return suppliers.Where(d => !supplierList.Contains(d.ID)).ToList();
        }
        private List<int> GetSuppliersItemType(string ItemType)
        {
            return db.SupplierItemTypes.Where(d => d.FKItemTypeReference.ItemType == ItemType).Select(d => d.SupplierReference).ToList();
        }
        private List<int> GetSuppliersCategory(string Category)
        {
            return db.SupplierCategories.Where(d => d.FKCategoryReference.ItemCategoryName == Category).Select(d => d.SupplierReference).ToList();
        }
        public List<ItemCategory> GetItemCategories()
        {
            return db.ItemCategories.Where(d => d.PurgeFlag == false).OrderBy(d => d.ItemCategoryName).ToList();
        }
        public List<ItemTypes> GetItemTypes()
        {
            return db.ItemTypes.Where(d => d.PurgeFlag == false).OrderBy(d => d.ItemType).ToList();
        }
        public List<CatalogueVM> GetCatalogue(string UserEmail, string ProjectCode)
        {
            var user = db.UserAccounts.Where(d => d.Email == UserEmail).FirstOrDefault();
            if (ProjectCode.Contains("CSPR"))
            {
                return GetAllItems(user.DepartmentCode, 1);
            }
            else
            {
                return GetAllItems(user.DepartmentCode);
            }

        }
        public List<CatalogueVM> GetCatalogueFromSearch(string UserEmail, string ProjectCode, int? Category, int? ItemType, string ItemName)
        {
            var user = db.UserAccounts.Where(d => d.Email == UserEmail).FirstOrDefault();
            if (ProjectCode.Contains("CSPR"))
            {
                return GetAllItems(user.DepartmentCode, 1, Category, ItemType, ItemName);
            }
            else
            {
                return GetAllItems(user.DepartmentCode, Category, ItemType, ItemName);
            }
        }
        private List<CatalogueVM> GetAllItems(string DepartmentCode)
        {
            var allowedItemIDs = db.ItemAllowedUsers.Where(d => d.DepartmentReference == DepartmentCode).Select(d => d.ItemReference).Distinct().ToList();
            var restrictedItemIDs = db.ItemAllowedUsers.Where(d => !allowedItemIDs.Contains(d.ItemReference)).Select(d => d.ItemReference).Distinct().ToList();
            var items = db.Items.Where(d => !restrictedItemIDs.Contains(d.ID) && d.PurgeFlag == false).ToList();
            return items.Select(d => new CatalogueVM {
                Classification = d.FKArticleReference.FKItemTypeReference.FKItemClassificationReference.Classification,
                ItemImage = d.ItemImage,
                ItemCode = d.ItemCode,
                ItemName = d.ItemFullName,
                ItemSpecifications = d.ItemSpecifications == null ? "Specification shall be defined by End-User" : d.ItemSpecifications,
                ItemType = d.FKArticleReference.FKItemTypeReference.ItemType,
                Category = d.FKCategoryReference.ItemCategoryName,
                IndividualUOMReference = d.FKIndividualUnitReference.UnitName,
                UnitCost = db.ItemPrices.Where(x => x.Item == d.ID && x.IsPrevailingPrice == true).Select(x => x.UnitPrice).FirstOrDefault(),
                ProcurementSource = d.ProcurementSource == ProcurementSources.AgencyToAgency ? "Department of Budget and Management - Procurement Service" : "External Suppliers",
                MinimumIssuanceQty = d.MinimumIssuanceQty,
                ResponsibilityCenter = d.FKArticleReference.FKItemTypeReference.ResponsibilityCenter == null ? "Requesting Office" : hris.GetDepartmentDetails(d.FKArticleReference.FKItemTypeReference.ResponsibilityCenter).Department,
                PurchaseRequestCenter = d.FKArticleReference.FKItemTypeReference.PurchaseRequestCenter == null ? "Requesting Office" : hris.GetDepartmentDetails(d.FKArticleReference.FKItemTypeReference.PurchaseRequestCenter).Department
            }).ToList();
        }
        private List<CatalogueVM> GetAllItems(string DepartmentCode, int ClassificationID)
        {
            var allowedItemIDs = db.ItemAllowedUsers.Where(d => d.DepartmentReference == DepartmentCode).Select(d => d.ItemReference).Distinct().ToList();
            var restrictedItemIDs = db.ItemAllowedUsers.Where(d => !allowedItemIDs.Contains(d.ItemReference)).Select(d => d.ItemReference).Distinct().ToList();
            var items = db.Items.Where(d => !restrictedItemIDs.Contains(d.ID) && d.PurgeFlag == false && d.FKArticleReference.FKItemTypeReference.ItemClassificationReference == ClassificationID).ToList();
            return items.Select(d => new CatalogueVM
            {
                Classification = d.FKArticleReference.FKItemTypeReference.FKItemClassificationReference.Classification,
                ItemImage = d.ItemImage,
                ItemCode = d.ItemCode,
                ItemName = d.ItemFullName,
                ItemSpecifications = d.ItemSpecifications == null ? "Specification shall be defined by End-User" : d.ItemSpecifications,
                ItemType = d.FKArticleReference.FKItemTypeReference.ItemType,
                Category = d.FKCategoryReference.ItemCategoryName,
                IndividualUOMReference = d.FKIndividualUnitReference.UnitName,
                UnitCost = db.ItemPrices.Where(x => x.Item == d.ID && x.IsPrevailingPrice == true).Select(x => x.UnitPrice).FirstOrDefault(),
                ProcurementSource = d.ProcurementSource == ProcurementSources.AgencyToAgency ? "Department of Budget and Management - Procurement Service" : "External Suppliers",
                MinimumIssuanceQty = d.MinimumIssuanceQty,
                ResponsibilityCenter = d.FKArticleReference.FKItemTypeReference.ResponsibilityCenter == null ? "Requesting Office" : hris.GetDepartmentDetails(d.FKArticleReference.FKItemTypeReference.ResponsibilityCenter).Department,
                PurchaseRequestCenter = d.FKArticleReference.FKItemTypeReference.PurchaseRequestCenter == null ? "Requesting Office" : hris.GetDepartmentDetails(d.FKArticleReference.FKItemTypeReference.PurchaseRequestCenter).Department
            }).ToList();
        }
        private List<CatalogueVM> GetAllItems(string DepartmentCode, int? Category, int? ItemType, string ItemName)
        {
            ItemName = ItemName == null ? "" : ItemName;
            var category = Category == null ? "" : db.ItemCategories.Find(Category).ItemCategoryName;
            var itemType = ItemType == null ? "" : db.ItemTypes.Find(ItemType).ItemType;
            var allowedItemIDs = db.ItemAllowedUsers.Where(d => d.DepartmentReference == DepartmentCode).Select(d => d.ItemReference).Distinct().ToList();
            var restrictedItemIDs = db.ItemAllowedUsers.Where(d => !allowedItemIDs.Contains(d.ItemReference)).Select(d => d.ItemReference).Distinct().ToList();
            return (from items in db.Items.Where(d => !restrictedItemIDs.Contains(d.ID) &&
                                                 d.PurgeFlag == false &&
                                                 (d.ItemFullName.Contains(ItemName) && d.FKCategoryReference.ItemCategoryName.Contains(category) && d.FKArticleReference.FKItemTypeReference.ItemType.Contains(itemType)))
                                          .ToList()
                    join prices in db.ItemPrices.Where(d => d.IsPrevailingPrice == true).ToList() on items.ID equals prices.Item into itemsWithPrices
                    from x in itemsWithPrices.DefaultIfEmpty()
                    select new CatalogueVM
                    {
                        Classification = items.FKArticleReference.FKItemTypeReference.FKItemClassificationReference.Classification,
                        ItemImage = items.ItemImage,
                        ItemCode = items.ItemCode,
                        ItemName = items.ItemFullName,
                        ItemSpecifications = items.ItemSpecifications == null ? "Specification shall be defined by End-User" : items.ItemSpecifications,
                        ItemType = items.FKArticleReference.FKItemTypeReference.ItemType,
                        Category = items.FKCategoryReference.ItemCategoryName,
                        IndividualUOMReference = items.FKIndividualUnitReference.UnitName,
                        UnitCost = x == null ? 0.00m : x.UnitPrice,
                        ProcurementSource = items.ProcurementSource == ProcurementSources.AgencyToAgency ? "Department of Budget and Management - Procurement Service" : "External Suppliers",
                        MinimumIssuanceQty = items.MinimumIssuanceQty,
                        ResponsibilityCenter = items.FKArticleReference.FKItemTypeReference.ResponsibilityCenter == null ? "Requesting Office" : hris.GetDepartmentDetails(items.FKArticleReference.FKItemTypeReference.ResponsibilityCenter).Department,
                        PurchaseRequestCenter = items.FKArticleReference.FKItemTypeReference.PurchaseRequestCenter == null ? "Requesting Office" : hris.GetDepartmentDetails(items.FKArticleReference.FKItemTypeReference.PurchaseRequestCenter).Department
                    }).ToList();
        }
        private List<CatalogueVM> GetAllItems(string DepartmentCode, int ClassificationID, int? Category, int? ItemType, string ItemName)
        {
            ItemName = ItemName == null ? "" : ItemName;
            var category = Category == null ? "" : db.ItemCategories.Find(Category).ItemCategoryName;
            var itemType = ItemType == null ? "" : db.ItemTypes.Find(ItemType).ItemType;
            var allowedItemIDs = db.ItemAllowedUsers.Where(d => d.DepartmentReference == DepartmentCode).Select(d => d.ItemReference).Distinct().ToList();
            var restrictedItemIDs = db.ItemAllowedUsers.Where(d => !allowedItemIDs.Contains(d.ItemReference)).Select(d => d.ItemReference).Distinct().ToList();
            return (from items in db.Items.Where(d => !restrictedItemIDs.Contains(d.ID) &&
                                                 d.PurgeFlag == false &&
                                                 d.FKArticleReference.FKItemTypeReference.ItemClassificationReference == ClassificationID &&
                                                 (d.ItemFullName.Contains(ItemName) && d.FKCategoryReference.ItemCategoryName.Contains(category) && d.FKArticleReference.FKItemTypeReference.ItemType.Contains(itemType)))
                                          .ToList()
                    join prices in db.ItemPrices.Where(d => d.IsPrevailingPrice == true).ToList() on items.ID equals prices.Item into itemsWithPrices
                    from x in itemsWithPrices.DefaultIfEmpty()
                    select new CatalogueVM
                    {
                        Classification = items.FKArticleReference.FKItemTypeReference.FKItemClassificationReference.Classification,
                        ItemImage = items.ItemImage,
                        ItemCode = items.ItemCode,
                        ItemName = items.ItemFullName,
                        ItemSpecifications = items.ItemSpecifications == null ? "Specification shall be defined by End-User" : items.ItemSpecifications,
                        ItemType = items.FKArticleReference.FKItemTypeReference.ItemType,
                        Category = items.FKCategoryReference.ItemCategoryName,
                        IndividualUOMReference = items.FKIndividualUnitReference.UnitName,
                        UnitCost = x == null ? 0.00m : x.UnitPrice,
                        ProcurementSource = items.ProcurementSource == ProcurementSources.AgencyToAgency ? "Department of Budget and Management - Procurement Service" : "External Suppliers",
                        MinimumIssuanceQty = items.MinimumIssuanceQty,
                        ResponsibilityCenter = items.FKArticleReference.FKItemTypeReference.ResponsibilityCenter == null ? "Requesting Office" : hris.GetDepartmentDetails(items.FKArticleReference.FKItemTypeReference.ResponsibilityCenter).Department,
                        PurchaseRequestCenter = items.FKArticleReference.FKItemTypeReference.PurchaseRequestCenter == null ? "Requesting Office" : hris.GetDepartmentDetails(items.FKArticleReference.FKItemTypeReference.PurchaseRequestCenter).Department
                    }).ToList();
        }
        public BasketItems GetItem(string ItemCode)
        {
            BasketItems Item = new BasketItems();
            Item = (from itemList in db.Items.ToList().Where(d => d.ItemCode == ItemCode).ToList()
                    select new BasketItems
                    {
                        Classification = itemList.FKArticleReference.FKItemTypeReference.FKItemClassificationReference.Classification,
                        ProposalType = BudgetProposalType.NewProposal,
                        ItemType = itemList.FKArticleReference.FKItemTypeReference.ItemType,
                        Category = itemList.FKCategoryReference.ItemCategoryName,
                        ItemCode = itemList.ItemCode,
                        ArticleCode = itemList.FKArticleReference.ArticleCode,
                        ItemSequence = itemList.Sequence,
                        ItemName = itemList.ItemFullName,
                        IsSpecsUserDefined = itemList.IsSpecsUserDefined,
                        ItemSpecifications = itemList.IsSpecsUserDefined == true ? null : itemList.ItemSpecifications,
                        ProcurementSource = itemList.ProcurementSource.GetAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>().Name,
                        AccountClass = abis.GetChartOfAccounts(itemList.FKArticleReference.UACSObjectClass).AcctName,
                        ItemImage = itemList.ItemImage,
                        IndividualUOMReference = itemList.FKIndividualUnitReference.UnitName,
                        MinimumIssuanceQty = itemList.MinimumIssuanceQty,
                        ResponsibilityCenter = itemList.FKArticleReference.FKItemTypeReference.ResponsibilityCenter == null ? "Requesting Office" : hris.GetDepartmentDetails(itemList.FKArticleReference.FKItemTypeReference.ResponsibilityCenter).Department,
                        PurchaseRequestCenter = itemList.FKArticleReference.FKItemTypeReference.PurchaseRequestCenter == null ? "Requesting Office" : hris.GetDepartmentDetails(itemList.FKArticleReference.FKItemTypeReference.PurchaseRequestCenter).Department
                    }).FirstOrDefault();

            if (Item.ProcurementSource == ProcurementSources.AgencyToAgency.GetAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>().Name)
            {
                Item.UnitCost = db.ItemPrices.Where(d => (d.FKItemReference.FKArticleReference.ArticleCode == Item.ArticleCode && d.FKItemReference.Sequence == Item.ItemSequence) && (d.IsPrevailingPrice == true)).Select(d => d.UnitPrice).FirstOrDefault();
                Item.Supplier1ID = 1;
            }
            else
            {
                var marketSurvey = db.MarketSurveys.Where(d => (d.FKArticleReference.ArticleCode == Item.ArticleCode && d.ItemSequence == Item.ItemSequence) && (DateTime.Now <= d.ExpirationDate)).FirstOrDefault();
                Item.UnitCost = marketSurvey == null ? null : marketSurvey.UnitCost;
                Item.Supplier1ID = marketSurvey == null ? null : marketSurvey.Supplier1Reference;
                Item.Supplier1UnitCost = marketSurvey == null ? null : marketSurvey.Supplier1UnitCost;
                Item.Supplier2ID = marketSurvey == null ? null : marketSurvey.Supplier2Reference;
                Item.Supplier2UnitCost = marketSurvey == null ? null : marketSurvey.Supplier2UnitCost;
                Item.Supplier3ID = marketSurvey == null ? null : marketSurvey.Supplier3Reference;
                Item.Supplier3UnitCost = marketSurvey == null ? null : marketSurvey.Supplier3UnitCost;
                if (marketSurvey != null)
                {
                    var supplierDataAccess = new SuppliersBL();
                    var supplier1 = new SupplierVM();
                    var supplier2 = new SupplierVM();
                    var supplier3 = new SupplierVM();
                    if (marketSurvey.Supplier1Reference != null)
                    {
                        supplier1 = supplierDataAccess.GetSupplierDetails(marketSurvey.Supplier1Reference);
                        Item.Supplier1Name = supplier1.SupplierName;
                        Item.Supplier1Address = supplier1.Address;
                        Item.Supplier1ContactNo = supplier1.ContactNumber;
                        Item.Supplier1EmailAddress = supplier1.EmailAddress;
                    }
                    if (marketSurvey.Supplier2Reference != null)
                    {
                        supplier2 = supplierDataAccess.GetSupplierDetails(marketSurvey.Supplier2Reference);
                        Item.Supplier2Name = supplier2.SupplierName;
                        Item.Supplier2Address = supplier2.Address;
                        Item.Supplier2ContactNo = supplier2.ContactNumber;
                        Item.Supplier2EmailAddress = supplier2.EmailAddress;
                    }
                    if (marketSurvey.Supplier3Reference != null)
                    {
                        supplier3 = supplierDataAccess.GetSupplierDetails(marketSurvey.Supplier3Reference);
                        Item.Supplier3Name = supplier3.SupplierName;
                        Item.Supplier3Address = supplier3.Address;
                        Item.Supplier3ContactNo = supplier3.ContactNumber;
                        Item.Supplier3EmailAddress = supplier3.EmailAddress;
                    }
                }
            }
            return Item;
        }
        public bool PostToProject(Basket Basket, string UserEmail)
        {
            var project = db.ProjectPlans.Where(d => d.ProjectCode == Basket.ProjectCode).FirstOrDefault();
            var switchBoard = db.SwitchBoard.Where(d => d.Reference == project.ProjectCode).FirstOrDefault();
            var user = db.UserAccounts.Where(d => d.Email == UserEmail).FirstOrDefault();
            var employee = hris.GetEmployeeByCode(user.EmpCode);
            var projectItems = new List<ProjectDetails>();
            var basketItemExistingInProject = (from basketItems in Basket.BasketItems
                                               join projectDetails in db.ProjectDetails.Where(d => d.ArticleReference != null).ToList() on new { Article = basketItems.ArticleCode, Sequence = basketItems.ItemSequence } equals new { Article = projectDetails.FKItemArticleReference.ArticleCode, Sequence = projectDetails.ItemSequence }
                                               where projectDetails.FKProjectPlanReference.ProjectCode == Basket.ProjectCode && projectDetails.ProposalType == BudgetProposalType.NewProposal
                                               select basketItems).ToList();
            var baketItems = Basket.BasketItems
                                   .Where(d => !basketItemExistingInProject.Select(x => x.ArticleCode + "-" + x.ItemSequence).Contains(d.ItemCode))
                                   .ToList();

            if (project == null)
            {
                return false;
            }

            foreach (var item in basketItemExistingInProject)
            {
                var itemLookUp = db.Items.ToList().Where(d => d.ItemCode == item.ItemCode).FirstOrDefault();
                var projectDetail = db.ProjectDetails
                                      .Where(d => d.FKItemArticleReference.ArticleCode == itemLookUp.FKArticleReference.ArticleCode &&
                                                  d.ItemSequence == itemLookUp.Sequence &&
                                                  d.FKProjectPlanReference.ProjectCode == project.ProjectCode &&
                                                  d.ProposalType == BudgetProposalType.NewProposal)
                                      .FirstOrDefault();
                projectDetail.Supplier1Reference = item.Supplier1ID;
                projectDetail.Supplier1UnitCost = item.Supplier1UnitCost;
                projectDetail.Supplier2Reference = item.Supplier2ID;
                projectDetail.Supplier2UnitCost = item.Supplier2UnitCost;
                projectDetail.Supplier3Reference = item.Supplier3ID;
                projectDetail.Supplier3UnitCost = item.Supplier3UnitCost;
                projectDetail.UnitCost = item.UnitCost;
                projectDetail.JanQty = item.JanQty;
                projectDetail.FebQty = item.FebQty;
                projectDetail.MarQty = item.MarQty;
                projectDetail.AprQty = item.AprQty;
                projectDetail.MayQty = item.MayQty;
                projectDetail.JunQty = item.JunQty;
                projectDetail.JulQty = item.JulQty;
                projectDetail.AugQty = item.AugQty;
                projectDetail.SepQty = item.SepQty;
                projectDetail.OctQty = item.OctQty;
                projectDetail.NovQty = item.NovQty;
                projectDetail.DecQty = item.DecQty;
                projectDetail.TotalQty = item.TotalQty;
                projectDetail.Justification = item.Justification;
                projectDetail.EstimatedBudget = item.UnitCost == null ? null : item.UnitCost * item.TotalQty;

                if (db.Entry(projectDetail).State == EntityState.Modified)
                {
                    projectDetail.ProjectItemStatus = ProjectDetailsStatus.ItemRevised;

                    db.SwitchBoardBody.Add(new SwitchBoardBody
                    {
                        SwitchBoardReference = switchBoard.ID,
                        ActionBy = employee.EmployeeCode,
                        Remarks = project.ProjectCode + " has been updated (Item Revised). " + item.ItemCode + " - " + item.ItemName + " has been updated.",
                        DepartmentCode = employee.DepartmentCode,
                        UpdatedAt = DateTime.Now
                    });

                    if (db.SaveChanges() == 0)
                    {
                        return false;
                    }
                }
            }

            foreach (var item in baketItems)
            {
                var itemLookUp = db.Items.ToList().Where(d => d.ItemCode == item.ItemCode).FirstOrDefault();
                var projectStatus = (itemLookUp.ProcurementSource == ProcurementSources.ExternalSuppliers && item.ResponsibilityCenter == "Requesting Office") ? ProjectDetailsStatus.ItemAccepted : ProjectDetailsStatus.PostedToProject;

                projectItems.Add(new ProjectDetails
                {
                    ProjectReference = project.ID,
                    ClassificationReference = itemLookUp.FKArticleReference.FKItemTypeReference.FKItemClassificationReference.ID,
                    UACS = itemLookUp.FKArticleReference.UACSObjectClass,
                    ArticleReference = itemLookUp.FKArticleReference.ID,
                    ItemSequence = itemLookUp.Sequence,
                    ItemFullName = itemLookUp.ItemFullName,
                    ItemSpecifications = item.ItemSpecifications,
                    ProposalType = BudgetProposalType.NewProposal,
                    ProcurementSource = item.ProcurementSource == "External Suppliers" ? ProcurementSources.ExternalSuppliers : ProcurementSources.AgencyToAgency,
                    UOMReference = itemLookUp.FKIndividualUnitReference.ID,
                    CategoryReference = itemLookUp.FKCategoryReference.ID,
                    Justification = item.Justification,
                    JanQty = (project.ProjectCode.Contains("EUPR") && project.DeliveryMonth == 1) ? item.TotalQty : item.JanQty,
                    FebQty = (project.ProjectCode.Contains("EUPR") && project.DeliveryMonth == 2) ? item.TotalQty : item.FebQty,
                    MarQty = (project.ProjectCode.Contains("EUPR") && project.DeliveryMonth == 3) ? item.TotalQty : item.MarQty,
                    AprQty = (project.ProjectCode.Contains("EUPR") && project.DeliveryMonth == 4) ? item.TotalQty : item.AprQty,
                    MayQty = (project.ProjectCode.Contains("EUPR") && project.DeliveryMonth == 5) ? item.TotalQty : item.MayQty,
                    JunQty = (project.ProjectCode.Contains("EUPR") && project.DeliveryMonth == 6) ? item.TotalQty : item.JunQty,
                    JulQty = (project.ProjectCode.Contains("EUPR") && project.DeliveryMonth == 7) ? item.TotalQty : item.JulQty,
                    AugQty = (project.ProjectCode.Contains("EUPR") && project.DeliveryMonth == 8) ? item.TotalQty : item.AugQty,
                    SepQty = (project.ProjectCode.Contains("EUPR") && project.DeliveryMonth == 9) ? item.TotalQty : item.SepQty,
                    OctQty = (project.ProjectCode.Contains("EUPR") && project.DeliveryMonth == 10) ? item.TotalQty : item.OctQty,
                    NovQty = (project.ProjectCode.Contains("EUPR") && project.DeliveryMonth == 11) ? item.TotalQty : item.NovQty,
                    DecQty = (project.ProjectCode.Contains("EUPR") && project.DeliveryMonth == 12) ? item.TotalQty : item.DecQty,
                    TotalQty = item.TotalQty,
                    Supplier1Reference = item.Supplier1ID,
                    Supplier1UnitCost = item.Supplier1UnitCost,
                    Supplier2Reference = item.Supplier2ID,
                    Supplier2UnitCost = item.Supplier2UnitCost,
                    Supplier3Reference = item.Supplier3ID,
                    Supplier3UnitCost = item.Supplier3UnitCost,
                    UnitCost = item.UnitCost,
                    EstimatedBudget = item.EstimatedBudget,
                    ProjectItemStatus = projectStatus,
                    UpdateFlag = true
                });
            }

            if (Basket.BasketItems.Count != basketItemExistingInProject.Count)
            {
                db.ProjectDetails.AddRange(projectItems);
                db.SwitchBoardBody.Add(new SwitchBoardBody
                {
                    SwitchBoardReference = switchBoard.ID,
                    ActionBy = employee.EmployeeCode,
                    Remarks = project.ProjectCode + " has been updated . (Added Items)",
                    DepartmentCode = employee.DepartmentCode,
                    UpdatedAt = DateTime.Now
                });

                if (db.SaveChanges() == 0)
                {
                    return false;
                }
            }
            UpdateProjectStatus(Basket.ProjectCode);
            return true;
        }
        private void UpdateProjectStatus(string ProjectCode)
        {
            var notAcceptedCount = db.ProjectDetails.Where(d => d.FKProjectPlanReference.ProjectCode == ProjectCode && d.ProjectItemStatus == ProjectDetailsStatus.ItemNotAccepted).Count();
            var acceptedCount = db.ProjectDetails.Where(d => d.FKProjectPlanReference.ProjectCode == ProjectCode &&
                                                                 d.ProjectItemStatus == ProjectDetailsStatus.ItemAccepted).Count();
            var itemsCount = notAcceptedCount + acceptedCount;
            var totalNoOfItems = db.ProjectDetails.Where(d => d.FKProjectPlanReference.ProjectCode == ProjectCode).Count();
            if (itemsCount == totalNoOfItems)
            {
                var project = db.ProjectPlans.Where(d => d.ProjectCode == ProjectCode).FirstOrDefault();
                project.ProjectStatus = ProjectStatus.EvaluatedByResponsibilityCenter;
                db.SaveChanges();
            }
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
                hris.Dispose();
                systemBDL.Dispose();
                projectPlanDAL.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}