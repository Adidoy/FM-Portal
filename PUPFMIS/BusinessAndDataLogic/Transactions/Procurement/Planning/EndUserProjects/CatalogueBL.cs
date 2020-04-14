//using MigraDoc.DocumentObjectModel;
//using MigraDoc.DocumentObjectModel.Tables;
//using PUPFMIS.Models;
//using PUPFMIS.Models.HRIS;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Web.Mvc;
//using System.Data.Entity;

//namespace PUPFMIS.BusinessAndDataLogic
//{
//    public class CatalogueBL : Controller
//    {
//        private FMISDbContext db = new FMISDbContext();

//        public List<string> GetCategories()
//        {
//            var categories = db.ItemCategories.Select(d => d.ItemCategoryName).ToList();
//            categories.Add("All Categories");
//            categories.Sort();
//            return categories;
//        }

//        public List<InventoryType> GetInventoryTypes()
//        {
//            return db.InventoryTypes.ToList();
//        }

//        public InventoryType GetInventoryTypes(int InventoryTypeID)
//        {
//            return db.InventoryTypes.Find(InventoryTypeID);
//        }

//        public List<Catalogue> GetCatalogue()
//        {
//            return (from items in db.Items
//                    where items.InventoryTypeReference != 1 || items.InventoryTypeReference != 4 || items.InventoryTypeReference != 5
//                    select new Catalogue
//                    {
//                        ItemID = items.ID,
//                        ItemCode = items.ItemCode,
//                        ItemName = items.ItemName,
//                        ItemShortSpecifications = items.ItemShortSpecifications,
//                        ItemSpecifications = items.ItemSpecifications,
//                        ItemCategory = items.FKItemCategoryReference.ItemCategoryName,
//                        ItemImage = items.ItemImage,
//                        IndividualUOMReference = items.FKIndividualUnitReference.UnitName,
//                        MinimumIssuanceQty = items.MinimumIssuanceQty,
//                        ProcurementSource = items.ProcurementSource,
//                        ItemInventoryType = items.FKInventoryTypeReference.InventoryTypeName
//                    }
//                   )
//                   .OrderBy(d => new { d.ItemName, d.ItemCode })
//                   .ToList();
//        }

//        public List<Catalogue> GetCatalogue(string CategoryName)
//        {
//            return (from items in db.Items
//                    where items.FKItemCategoryReference.ItemCategoryName == CategoryName
//                    select new Catalogue
//                    {
//                        ItemID = items.ID,
//                        ItemCode = items.ItemCode,
//                        ItemName = items.ItemName,
//                        ItemShortSpecifications = items.ItemShortSpecifications,
//                        ItemSpecifications = items.ItemSpecifications,
//                        ItemCategory = items.FKItemCategoryReference.ItemCategoryName,
//                        ItemImage = items.ItemImage,
//                        IndividualUOMReference = items.FKIndividualUnitReference.UnitName,
//                        MinimumIssuanceQty = items.MinimumIssuanceQty,
//                        ProcurementSource = items.ProcurementSource,
//                        ItemInventoryType = items.FKInventoryTypeReference.InventoryTypeName
//                    }
//                   )
//                   .OrderBy(d => new { d.ItemName, d.ItemCode })
//                   .ToList();
//        }

//        public List<Catalogue> GetCatalogue(int InventoryTypeID)
//        {
//            return (from items in db.Items
//                    where items.InventoryTypeReference == InventoryTypeID
//                    select new Catalogue
//                    {
//                        ItemID = items.ID,
//                        ItemCode = items.ItemCode,
//                        ItemName = items.ItemName,
//                        ItemShortSpecifications = items.ItemShortSpecifications,
//                        ItemSpecifications = items.ItemSpecifications,
//                        ItemCategory = items.FKItemCategoryReference.ItemCategoryName,
//                        ItemImage = items.ItemImage,
//                        IndividualUOMReference = items.FKIndividualUnitReference.UnitName,
//                        MinimumIssuanceQty = items.MinimumIssuanceQty,
//                        ProcurementSource = items.ProcurementSource,
//                        ItemInventoryType = items.FKInventoryTypeReference.InventoryTypeName
//                    }
//                   )
//                   .OrderBy(d => new { d.ItemName, d.ItemCode })
//                   .ToList();
//        }

//        public BasketItem GetItems(string ItemCode)
//        {
//            return db.Items
//                            .Where(d => d.ItemCode == ItemCode)
//                            .Select(d => new BasketItem {
//                                ItemID = d.ID,
//                                ItemCode = d.ItemCode,
//                                ItemName = d.ItemName,
//                                ItemShortSpecifications = d.ItemShortSpecifications,
//                                ItemSpecifications = d.ItemSpecifications,
//                                ItemCategory = d.FKItemCategoryReference.ItemCategoryName,
//                                ItemImage = d.ItemImage,
//                                IndividualUOMReference = d.FKIndividualUnitReference.UnitName,
//                                MinimumIssuanceQty = d.MinimumIssuanceQty,
//                                ProcurementSource = d.ProcurementSource,
//                                ItemInventoryType = d.FKInventoryTypeReference.InventoryTypeName
//                            })
//                            .OrderBy(d => new { d.ItemName, d.ItemCode })
//                            .FirstOrDefault();
//        }

//        public BasketItem GetItems(string ItemCode, string Email)
//        {
//            var basketItem = db.Items
//                            .Where(d => d.FKInventoryTypeReference.InventoryTypeName == "Common Use Office Supplies" && d.ItemCode == ItemCode)
//                            .Select(d => new BasketItem
//                            {
//                                ItemID = d.ID,
//                                ItemCode = d.ItemCode,
//                                ItemName = d.ItemName,
//                                ItemShortSpecifications = d.ItemShortSpecifications,
//                                ItemSpecifications = d.ItemSpecifications,
//                                ItemCategory = d.FKItemCategoryReference.ItemCategoryName,
//                                ItemImage = d.ItemImage,
//                                IndividualUOMReference = d.FKIndividualUnitReference.UnitName,
//                                MinimumIssuanceQty = d.MinimumIssuanceQty,
//                                ProcurementSource = d.ProcurementSource,
//                                ItemInventoryType = d.FKInventoryTypeReference.InventoryTypeName
//                            })
//                            .OrderBy(d => new { d.ItemName, d.ItemCode })
//                            .FirstOrDefault();

//            var officeID = db.UserAccounts.Where(d => d.Email == Email).Select(d => d.FKUserInformationReference.Office).FirstOrDefault();
//            var inflationRate = Convert.ToInt32(db.SystemVariables.Find(1).Value);
//            var totalConsumption = db.SuppliesIssueDetails
//                                   .Where(d => d.FKRequestHeader.Office == officeID && d.FKSuppliesMaster.FKItem.ItemCode == ItemCode)
//                                   .Select(d => new { IssuedYear = d.FKRequestHeader.IssuedAt.Value.Year, Quantity = d.QtyIssued })
//                                   .GroupBy(d => d.IssuedYear)
//                                   .Select(d => new { Total = d.Sum(x => x.Quantity), Year = d.Key })
//                                   .OrderByDescending(d => d.Year)
//                                   .Select(d => d.Total)
//                                   .FirstOrDefault();
//            var inflation = ((Convert.ToDecimal(totalConsumption) / 100) * inflationRate);
//            basketItem.TotalConsumption = totalConsumption + Convert.ToInt32(Decimal.Round(inflation,0));
//            return basketItem;
//        }

//        public bool ValidateConsumptionVSQuantity(BasketItem item)
//        {
//            return (item.TotalQty > item.TotalConsumption) ? true : false;
//        }

//        public List<Supplier> GetSuppliers()
//        {
//            return db.Suppliers.Where(d => d.PurgeFlag == false).ToList();
//        }

//        protected override void Dispose(bool disposing)
//        {
//            if (disposing)
//            {
//                db.Dispose();
//            }
//            base.Dispose(disposing);
//        }
//    }
//}