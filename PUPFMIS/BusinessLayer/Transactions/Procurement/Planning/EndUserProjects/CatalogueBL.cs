﻿using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using PUPFMIS.Models;
using PUPFMIS.Models.HRIS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;

namespace PUPFMIS.BusinessLayer
{
    public class CatalogueBL : Controller
    {
        private FMISDbContext db = new FMISDbContext();

        public List<string> GetCategories()
        {
            var categories = db.ItemCategories.Select(d => d.ItemCategoryName).ToList();
            categories.Add("All Categories");
            categories.Sort();
            return categories;
        }

        public List<Catalogue> GetCSEItems()
        {
            return (from items in db.Items
                    where items.FKInventoryTypeReference.InventoryTypeName == "Common Use Office Supplies"
                    select new Catalogue
                    {
                        ItemID = items.ID,
                        ItemCode = items.ItemCode,
                        ItemName = items.ItemName,
                        ItemShortSpecifications = items.ItemShortSpecifications,
                        ItemSpecifications = items.ItemSpecifications,
                        ItemCategory = items.FKItemCategoryReference.ItemCategoryName,
                        ItemImage = items.ItemImage,
                        IndividualUOMReference = items.FKIndividualUnitReference.UnitName,
                        MinimumIssuanceQty = items.MinimumIssuanceQty,
                        ProcurementSource = items.ProcurementSource
                    }
                   )
                   .OrderBy(d => new { d.ItemName, d.ItemCode })
                   .ToList();
        }

        public Basket GetCSECItems(string ItemCode)
        {
            return db.Items
                            .Where(d => d.FKInventoryTypeReference.InventoryTypeName == "Common Use Office Supplies" && d.ItemCode == ItemCode)
                            .Select(d => new Basket {
                                ItemID = d.ID,
                                ItemCode = d.ItemCode,
                                ItemName = d.ItemName,
                                ItemShortSpecifications = d.ItemShortSpecifications,
                                ItemSpecifications = d.ItemSpecifications,
                                ItemCategory = d.FKItemCategoryReference.ItemCategoryName,
                                ItemImage = d.ItemImage,
                                IndividualUOMReference = d.FKIndividualUnitReference.UnitName,
                                MinimumIssuanceQty = d.MinimumIssuanceQty,
                                ProcurementSource = d.ProcurementSource
                            })
                            .OrderBy(d => new { d.ItemName, d.ItemCode })
                            .FirstOrDefault();
        }

        public Basket GetCSECItems(string ItemCode, string Email)
        {
            var basketItem = db.Items
                            .Where(d => d.FKInventoryTypeReference.InventoryTypeName == "Common Use Office Supplies" && d.ItemCode == ItemCode)
                            .Select(d => new Basket
                            {
                                ItemID = d.ID,
                                ItemCode = d.ItemCode,
                                ItemName = d.ItemName,
                                ItemShortSpecifications = d.ItemShortSpecifications,
                                ItemSpecifications = d.ItemSpecifications,
                                ItemCategory = d.FKItemCategoryReference.ItemCategoryName,
                                ItemImage = d.ItemImage,
                                IndividualUOMReference = d.FKIndividualUnitReference.UnitName,
                                MinimumIssuanceQty = d.MinimumIssuanceQty,
                                ProcurementSource = d.ProcurementSource
                            })
                            .OrderBy(d => new { d.ItemName, d.ItemCode })
                            .FirstOrDefault();

            var officeID = db.UserAccounts.Where(d => d.Email == Email).Select(d => d.FKUserInformationReference.Office).FirstOrDefault();
            var totalConsumption = db.SuppliesIssueDetails
                                   .Where(d => d.FKRequestHeader.Office == officeID && d.FKSuppliesMaster.FKItem.ItemCode == ItemCode)
                                   .Select(d => new { IssuedYear = d.FKRequestHeader.IssuedAt.Value.Year, Quantity = d.QtyIssued })
                                   .GroupBy(d => d.IssuedYear)
                                   .Select(d => new { Total = d.Sum(x => x.Quantity), Year = d.Key })
                                   .OrderByDescending(d => d.Year)
                                   .Select(d => d.Total)
                                   .First();
            basketItem.TotalConsumption = totalConsumption;
            return basketItem;
        }

        public List<Catalogue> GetPropertyItems()
        {
            return (from items in db.Items
                    where items.FKInventoryTypeReference.InventoryTypeName == "Property and Equipment"
                    select new Catalogue
                    {
                        ItemID = items.ID,
                        ItemCode = items.ItemCode,
                        ItemName = items.ItemName,
                        ItemShortSpecifications = items.ItemShortSpecifications,
                        ItemSpecifications = items.ItemSpecifications,
                        ItemCategory = items.FKItemCategoryReference.ItemCategoryName,
                        ItemImage = items.ItemImage,
                        IndividualUOMReference = items.FKIndividualUnitReference.UnitName,
                        MinimumIssuanceQty = items.MinimumIssuanceQty,
                        ProcurementSource = items.ProcurementSource
                    }
                   )
                   .OrderBy(d => new { d.ItemName, d.ItemCode })
                   .ToList();
        }

        public Basket GetPropertyItems(string ItemCode)
        {
            return (from items in db.Items
                    where items.FKInventoryTypeReference.InventoryTypeName == "Property and Equipment" && items.ItemCode == ItemCode
                    select new Basket
                    {
                        ItemID = items.ID,
                        ItemCode = items.ItemCode,
                        ItemName = items.ItemName,
                        ItemShortSpecifications = items.ItemShortSpecifications,
                        ItemSpecifications = items.ItemSpecifications,
                        ItemCategory = items.FKItemCategoryReference.ItemCategoryName,
                        ItemImage = items.ItemImage,
                        IndividualUOMReference = items.FKIndividualUnitReference.UnitName,
                        MinimumIssuanceQty = items.MinimumIssuanceQty,
                        ProcurementSource = items.ProcurementSource
                    }
                   )
                   .OrderBy(d => new { d.ItemName, d.ItemCode }).FirstOrDefault();
        }

        public List<Catalogue> GetSemiExpandablePropertyItems()
        {
            return (from items in db.Items
                    where items.FKInventoryTypeReference.InventoryTypeName == "Semi-Expendable Property and Equipment"
                    select new Catalogue
                    {
                        ItemID = items.ID,
                        ItemCode = items.ItemCode,
                        ItemName = items.ItemName,
                        ItemShortSpecifications = items.ItemShortSpecifications,
                        ItemSpecifications = items.ItemSpecifications,
                        ItemCategory = items.FKItemCategoryReference.ItemCategoryName,
                        ItemImage = items.ItemImage,
                        IndividualUOMReference = items.FKIndividualUnitReference.UnitName,
                        MinimumIssuanceQty = items.MinimumIssuanceQty,
                        ProcurementSource = items.ProcurementSource
                    }
                   )
                   .OrderBy(d => new { d.ItemName, d.ItemCode })
                   .ToList();
        }

        public Basket GetSemiExpandablePropertyItems(string ItemCode)
        {
            return (from items in db.Items
                    where items.FKInventoryTypeReference.InventoryTypeName == "Semi-Expendable Property and Equipment" && items.ItemCode == ItemCode
                    select new Basket
                    {
                        ItemID = items.ID,
                        ItemCode = items.ItemCode,
                        ItemName = items.ItemName,
                        ItemShortSpecifications = items.ItemShortSpecifications,
                        ItemSpecifications = items.ItemSpecifications,
                        ItemCategory = items.FKItemCategoryReference.ItemCategoryName,
                        ItemImage = items.ItemImage,
                        IndividualUOMReference = items.FKIndividualUnitReference.UnitName,
                        MinimumIssuanceQty = items.MinimumIssuanceQty,
                        ProcurementSource = items.ProcurementSource
                    }
                   )
                   .OrderBy(d => new { d.ItemName, d.ItemCode }).FirstOrDefault();
        }

        public List<Catalogue> GetCSEItemsByCategory(string CategoryName)
        {
            return (from items in db.Items
                    where items.FKInventoryTypeReference.InventoryTypeName == "Common Use Office Supplies" && items.FKItemCategoryReference.ItemCategoryName == CategoryName
                    select new Catalogue
                    {
                        ItemID = items.ID,
                        ItemCode = items.ItemCode,
                        ItemName = items.ItemName,
                        ItemShortSpecifications = items.ItemShortSpecifications,
                        ItemSpecifications = items.ItemSpecifications,
                        ItemCategory = items.FKItemCategoryReference.ItemCategoryName,
                        ItemImage = items.ItemImage,
                        IndividualUOMReference = items.FKIndividualUnitReference.UnitName,
                        MinimumIssuanceQty = items.MinimumIssuanceQty,
                        ProcurementSource = items.ProcurementSource
                    }
                   )
                   .OrderBy(d => new { d.ItemName, d.ItemCode })
                   .ToList();
        }

        public List<Catalogue> GetPropertyItemsByCategory(string CategoryName)
        {
            return (from items in db.Items
                    where items.FKInventoryTypeReference.InventoryTypeName == "Property and Equipment" && items.FKItemCategoryReference.ItemCategoryName == CategoryName
                    select new Catalogue
                    {
                        ItemID = items.ID,
                        ItemCode = items.ItemCode,
                        ItemName = items.ItemName,
                        ItemShortSpecifications = items.ItemShortSpecifications,
                        ItemSpecifications = items.ItemSpecifications,
                        ItemCategory = items.FKItemCategoryReference.ItemCategoryName,
                        ItemImage = items.ItemImage,
                        IndividualUOMReference = items.FKIndividualUnitReference.UnitName,
                        MinimumIssuanceQty = items.MinimumIssuanceQty,
                        ProcurementSource = items.ProcurementSource
                    }
                   )
                   .OrderBy(d => new { d.ItemName, d.ItemCode })
                   .ToList();
        }

        public List<Catalogue> GetSemiExpandableProprertyItemsByCategory(string CategoryName)
        {
            return (from items in db.Items
                    where items.FKInventoryTypeReference.InventoryTypeName == "Semi-Expandable Property and Equipment" && items.FKItemCategoryReference.ItemCategoryName == CategoryName
                    select new Catalogue
                    {
                        ItemID = items.ID,
                        ItemCode = items.ItemCode,
                        ItemName = items.ItemName,
                        ItemShortSpecifications = items.ItemShortSpecifications,
                        ItemSpecifications = items.ItemSpecifications,
                        ItemCategory = items.FKItemCategoryReference.ItemCategoryName,
                        ItemImage = items.ItemImage,
                        IndividualUOMReference = items.FKIndividualUnitReference.UnitName,
                        MinimumIssuanceQty = items.MinimumIssuanceQty,
                        ProcurementSource = items.ProcurementSource
                    }
                   )
                   .OrderBy(d => new { d.ItemName, d.ItemCode })
                   .ToList();
        }

        public bool ValidateConsumptionVSQuantity(Basket item)
        {
            return (item.TotalQty > item.TotalConsumption) ? true : false;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}