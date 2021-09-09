using PUPFMIS.Models;
using PUPFMIS.Models.HRIS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace PUPFMIS.BusinessAndDataLogic
{
    public class MarketSurveyDAL : Controller
    {
        private FMISDbContext db = new FMISDbContext();
        private HRISDataAccess hris = new HRISDataAccess();
        private ABISDataAccess abis = new ABISDataAccess();

        public List<Item> GetNewItems(string UserEmail)
        {
            var user = db.UserAccounts.Where(d => d.Email == UserEmail).FirstOrDefault();
            var itemsWithMarketSurvey = db.MarketSurveys.Select(d => d.FKArticleReference.ArticleCode + "-" + d.ItemSequence).Distinct().ToList();
            return db.Items.ToList().Where(d => !itemsWithMarketSurvey.Contains(d.ItemCode) &&
                                           d.IsSpecsUserDefined == false &&
                                           d.ProcurementSource == ProcurementSources.ExternalSuppliers &&
                                           d.FKArticleReference.FKItemTypeReference.ResponsibilityCenter == user.DepartmentCode)
                                    .ToList();
        }
        public int GetNoOfLapsedMS(string UserEmail)
        {
            var user = db.UserAccounts.Where(d => d.Email == UserEmail).FirstOrDefault();
            var itemsWithMarketSurvey = db.MarketSurveys.Where(d => DateTime.Now > d.ExpirationDate).Select(d => d.FKArticleReference.ArticleCode + "-" + d.ItemSequence).Distinct().ToList();
            return db.Items.ToList().Where(d => itemsWithMarketSurvey.Contains(d.ItemCode) &&
                                           d.IsSpecsUserDefined == false &&
                                           d.ProcurementSource == ProcurementSources.ExternalSuppliers &&
                                           d.FKArticleReference.FKItemTypeReference.ResponsibilityCenter == user.DepartmentCode).Count();
        }
        public int GetNoOfUpdatedMS(string UserEmail)
        {
            var user = db.UserAccounts.Where(d => d.Email == UserEmail).FirstOrDefault();
            var itemsWithMarketSurvey = db.MarketSurveys.Where(d => DateTime.Now <= d.ExpirationDate).Select(d => d.FKArticleReference.ArticleCode + "-" + d.ItemSequence).Distinct().ToList();
            return db.Items.ToList().Where(d => itemsWithMarketSurvey.Contains(d.ItemCode) &&
                                           d.IsSpecsUserDefined == false &&
                                           d.ProcurementSource == ProcurementSources.ExternalSuppliers &&
                                           d.FKArticleReference.FKItemTypeReference.ResponsibilityCenter == user.DepartmentCode).Count();
        }
        public List<MarketSurveyVM> GetItems(string UserEmail)
        {
            var user = db.UserAccounts.Where(d => d.Email == UserEmail).FirstOrDefault();
            var item = (from itemList in db.Items.ToList().Where(d => d.FKArticleReference.FKItemTypeReference.ResponsibilityCenter == user.DepartmentCode &&
                                                                      d.IsSpecsUserDefined == false && d.ProcurementSource == ProcurementSources.ExternalSuppliers).ToList()
                        join marketSurveyList in db.MarketSurveys.ToList() on new { Article = itemList.ArticleReference, Sequence = itemList.Sequence } equals new { Article = marketSurveyList.ArticleReference, Sequence = marketSurveyList.ItemSequence }
                        select new MarketSurveyVM
                        {
                            ItemCode = itemList.ItemCode,
                            ItemName = itemList.ItemFullName,
                            Status = DateTime.Now <= marketSurveyList.ExpirationDate ? MarketSurveyStatus.Updated : MarketSurveyStatus.Lapsed
                        }).ToList();

            return item;
        }
        public MarketSurveyVM GetItem(string ItemCode)
        {
            SuppliersBL suppliersBL = new SuppliersBL();
            var item = (from itemList in db.Items.ToList().Where(d => d.ItemCode == ItemCode).ToList()
                        join marketSurveyList in db.MarketSurveys.ToList() on new { Article = itemList.ArticleReference, Sequence = itemList.Sequence } equals new { Article = marketSurveyList.ArticleReference, Sequence = marketSurveyList.ItemSequence }
                        into marketSurveys
                        from ms in marketSurveys.DefaultIfEmpty()
                        select new MarketSurveyVM
                        {
                            ItemImage = itemList.ItemImage,
                            ItemType = itemList.FKArticleReference.FKItemTypeReference.ItemType,
                            Category = itemList.FKCategoryReference.ItemCategoryName,
                            ItemCode = itemList.ItemCode,
                            ItemName = itemList.ItemFullName,
                            IsSpecsUserDefined = itemList.IsSpecsUserDefined,
                            ItemSpecifications = itemList.IsSpecsUserDefined == true ? null : itemList.ItemSpecifications,
                            ProcurementSource = itemList.ProcurementSource == ProcurementSources.AgencyToAgency ? "Department of Budget and Management - Procurement Service" : "External Suppliers",
                            AccountClass = abis.GetChartOfAccounts(itemList.FKArticleReference.UACSObjectClass).AcctName,
                            MinimumIssuanceQty = itemList.MinimumIssuanceQty,
                            IndividualUOMReference = itemList.FKIndividualUnitReference.UnitName,
                            UnitCost = ms == null ? null : ms.UnitCost,
                            Supplier1ID = ms == null ? null : ms.Supplier1Reference,
                            Supplier1UnitCost = ms == null ? null : ms.Supplier1UnitCost,
                            Supplier2ID = ms == null ? null : ms.Supplier2Reference,
                            Supplier2UnitCost = ms == null ? null : ms.Supplier2UnitCost,
                            Supplier3ID = ms == null ? null : ms.Supplier3Reference,
                            Supplier3UnitCost = ms == null ? null : ms.Supplier3UnitCost
                        }).FirstOrDefault();

            var supplier1Reference = item.Supplier1ID == null ? null : suppliersBL.GetSupplierDetails(item.Supplier1ID);
            var supplier2Reference = item.Supplier2ID == null ? null : suppliersBL.GetSupplierDetails(item.Supplier2ID);
            var supplier3Reference = item.Supplier3ID == null ? null : suppliersBL.GetSupplierDetails(item.Supplier3ID);
            item.Supplier1Name = supplier1Reference == null ? null : supplier1Reference.SupplierName;
            item.Supplier1Address = supplier1Reference == null ? null : supplier1Reference.Address;
            item.Supplier1ContactNo = supplier1Reference == null ? null : supplier1Reference.ContactNumber;
            item.Supplier1EmailAddress = supplier1Reference == null ? null : supplier1Reference.EmailAddress;
            item.Supplier2Name = supplier2Reference == null ? null : supplier2Reference.SupplierName;
            item.Supplier2Address = supplier2Reference == null ? null : supplier2Reference.Address;
            item.Supplier2ContactNo = supplier2Reference == null ? null : supplier2Reference.ContactNumber;
            item.Supplier2EmailAddress = supplier2Reference == null ? null : supplier2Reference.EmailAddress;
            item.Supplier3Name = supplier3Reference == null ? null : supplier3Reference.SupplierName;
            item.Supplier3Address = supplier3Reference == null ? null : supplier3Reference.Address;
            item.Supplier3ContactNo = supplier3Reference == null ? null : supplier3Reference.ContactNumber;
            item.Supplier3EmailAddress = supplier3Reference == null ? null : supplier3Reference.EmailAddress;

            return item;
        }
        public List<MarketSurveyVM> GetMarketSurvey(string UserEmail)
        {
            var user = db.UserAccounts.Where(d => d.Email == UserEmail).FirstOrDefault();
            return (from itemList in db.Items
                                       .ToList().Where(d => d.FKArticleReference.FKItemTypeReference.ResponsibilityCenter == user.DepartmentCode &&
                                                            d.IsSpecsUserDefined == false && d.ProcurementSource == ProcurementSources.ExternalSuppliers)
                                                .ToList()
                    join marketSurveyList in db.MarketSurveys.ToList() on new { Article = itemList.ArticleReference, Sequence = itemList.Sequence } equals new { Article = marketSurveyList.ArticleReference, Sequence = marketSurveyList.ItemSequence }
                    select new MarketSurveyVM
                    {
                        ItemCode = itemList.ItemCode,
                        ItemName = itemList.ItemFullName,
                        UnitCost = marketSurveyList.UnitCost,
                        Status = marketSurveyList.LastUpdated <= marketSurveyList.ExpirationDate ? MarketSurveyStatus.Updated : MarketSurveyStatus.Lapsed,
                        LastUpdated = marketSurveyList.LastUpdated
                    }).ToList();
        }
        public decimal ComputeUnitCost(decimal? Supplier1UnitCost, decimal? Supplier2UnitCost, decimal? Supplier3UnitCost)
        {
            decimal unitCost = 0.00m;
            int count = 0;
            if (Supplier1UnitCost != null && Supplier1UnitCost != 0.00m)
            {
                unitCost += (decimal)Supplier1UnitCost;
                count++;
            }
            if (Supplier2UnitCost != null && Supplier2UnitCost != 0.00m)
            {
                unitCost += (decimal)Supplier2UnitCost;
                count++;
            }
            if (Supplier3UnitCost != null && Supplier3UnitCost != 0.00m)
            {
                unitCost += (decimal)Supplier3UnitCost;
                count++;
            }
            unitCost = unitCost / count;
            return Math.Round(unitCost, 2, MidpointRounding.AwayFromZero);
        }
        public bool CreateMarketSurvey(MarketSurveyVM MarketSurvey, string UserEmail)
        {
            var user = db.UserAccounts.Where(d => d.Email == UserEmail).FirstOrDefault();
            var item = db.Items.ToList().Where(d => d.ItemCode == MarketSurvey.ItemCode).FirstOrDefault();
            var marketSurvey = new MarketSurvey
            {
                ArticleReference = item.ArticleReference,
                ItemSequence = item.Sequence,
                ItemFullName = item.ItemFullName,
                ItemSpecifications = item.ItemSpecifications,
                IsObsolete = false,
                Supplier1Reference = MarketSurvey.Supplier1ID,
                Supplier2Reference = MarketSurvey.Supplier2ID,
                Supplier3Reference = MarketSurvey.Supplier3ID,
                UnitCost = ComputeUnitCost(MarketSurvey.Supplier1UnitCost, MarketSurvey.Supplier2UnitCost, MarketSurvey.Supplier3UnitCost),
                Supplier1UnitCost = MarketSurvey.Supplier1UnitCost,
                Supplier2UnitCost = MarketSurvey.Supplier2UnitCost,
                Supplier3UnitCost = MarketSurvey.Supplier3UnitCost,
                ConductedBy = user.EmpCode,
                CreateAt = DateTime.Now,
                LastUpdated = DateTime.Now,
                ExpirationDate = DateTime.Now.AddMonths(6)
            };

            db.MarketSurveys.Add(marketSurvey);
            var projectItems = db.ProjectDetails.Where(d => (d.ProjectItemStatus == ProjectDetailsStatus.PostedToProject || d.ProjectItemStatus == ProjectDetailsStatus.ForEvaluation) &&
                                                            d.ArticleReference == item.ArticleReference && d.ItemSequence == item.Sequence).ToList();
            if (projectItems.Count > 0)
            {
                foreach (var projItem in projectItems)
                {
                    projItem.UnitCost = marketSurvey.UnitCost;
                    projItem.Supplier1Reference = marketSurvey.Supplier1Reference;
                    projItem.Supplier1UnitCost = marketSurvey.Supplier1UnitCost;
                    projItem.Supplier2Reference = marketSurvey.Supplier2Reference;
                    projItem.Supplier2UnitCost = marketSurvey.Supplier2UnitCost;
                    projItem.Supplier3Reference = marketSurvey.Supplier3Reference;
                    projItem.Supplier3UnitCost = marketSurvey.Supplier3UnitCost;
                    projItem.EstimatedBudget = Math.Round((projItem.TotalQty * (decimal)marketSurvey.UnitCost), 2, MidpointRounding.AwayFromZero);
                }
            }

            if (db.SaveChanges() == 0)
            {
                return false;
            }

            return true;
        }
        public bool UpdateMarketSurvey(MarketSurveyVM MarketSurvey, string UserEmail)
        {
            var user = db.UserAccounts.Where(d => d.Email == UserEmail).FirstOrDefault();
            var itemCode = MarketSurvey.ItemCode.Split("-".ToCharArray(), StringSplitOptions.None);
            var articleCode = itemCode[0];
            var itemSequence = itemCode[1];
            var marketSurvey = db.MarketSurveys.Where(d => d.FKArticleReference.ArticleCode == articleCode && d.ItemSequence == itemSequence).FirstOrDefault();

            marketSurvey.IsObsolete = MarketSurvey.IsObsolete;
            marketSurvey.Supplier1Reference = MarketSurvey.Supplier1ID;
            marketSurvey.Supplier2Reference = MarketSurvey.Supplier2ID;
            marketSurvey.Supplier3Reference = MarketSurvey.Supplier3ID;
            marketSurvey.UnitCost = ComputeUnitCost(MarketSurvey.Supplier1UnitCost, MarketSurvey.Supplier2UnitCost, MarketSurvey.Supplier3UnitCost);
            marketSurvey.Supplier1UnitCost = (MarketSurvey.Supplier1UnitCost == 0.00m || MarketSurvey.Supplier1UnitCost == null) ? null : MarketSurvey.Supplier1UnitCost;
            marketSurvey.Supplier2UnitCost = (MarketSurvey.Supplier2UnitCost == 0.00m || MarketSurvey.Supplier2UnitCost == null) ? null : MarketSurvey.Supplier2UnitCost;
            marketSurvey.Supplier3UnitCost = (MarketSurvey.Supplier3UnitCost == 0.00m || MarketSurvey.Supplier3UnitCost == null) ? null : MarketSurvey.Supplier3UnitCost;
            marketSurvey.ConductedBy = user.EmpCode;
            marketSurvey.LastUpdated = DateTime.Now;

            if (db.SaveChanges() == 0)
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
            }
            base.Dispose(disposing);
        }
    }
}