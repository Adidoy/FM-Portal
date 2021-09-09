using PUPFMIS.Models;
using PUPFMIS.Models.HRIS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace PUPFMIS.BusinessAndDataLogic
{
    public class StrategicPlanningDAL : Controller
    {
        private FMISDbContext db = new FMISDbContext();
        private HRISDataAccess hris = new HRISDataAccess();
        private ABISDataAccess abis = new ABISDataAccess();

        public List<HRISDepartmentDetailsVM> GetDepartments(string UserEmail)
        {
            var user = db.UserAccounts.Where(d => d.Email == UserEmail).FirstOrDefault();
            var itemLookUp = db.Items.ToList().Where(d => d.FKArticleReference.FKItemTypeReference.ResponsibilityCenter == user.DepartmentCode).Select(d => d.ItemCode).ToList();
            var projectCodes = db.ProjectDetails.Where(d => (d.ProjectItemStatus == ProjectDetailsStatus.ForEvaluation || d.ProjectItemStatus == ProjectDetailsStatus.ItemRevised) && itemLookUp.Select(x => x).Contains(d.FKItemArticleReference.ArticleCode + "-" + d.ItemSequence))
                                                .Select(d => d.FKProjectPlanReference.ProjectCode).Distinct().ToList();
            return db.ProjectPlans.Where(d => projectCodes.Contains(d.ProjectCode))
                                  .ToList()
                                  .Select(d => new
                                  {
                                      DepartmentCode = d.Department,
                                      Sector = hris.GetDepartmentDetails(d.Department).Sector,
                                      Department = hris.GetDepartmentDetails(d.Department).Department,
                                      DepartmentHead = hris.GetDepartmentDetails(d.Department).DepartmentHead
                                  }).GroupBy(d => d).Select(d => new HRISDepartmentDetailsVM
                                  {
                                      DepartmentCode = d.Key.DepartmentCode,
                                      Sector = d.Key.Sector,
                                      Department = d.Key.Department,
                                      DepartmentHead = d.Key.DepartmentHead
                                  }).OrderBy(d => d.Department).ToList();
        }
        public List<BasketItems> GetProjectItems(string UserEmail, string DepartmentCode)
        {
            var user = db.UserAccounts.Where(d => d.Email == UserEmail).FirstOrDefault();
            var projectItems = (from items in db.ProjectDetails
                                                .Where(d => d.FKProjectPlanReference.ProjectStatus == ProjectStatus.ForwardedToResponsibilityCenter &&
                                                            d.FKProjectPlanReference.Department == DepartmentCode &&
                                                            d.FKItemArticleReference.FKItemTypeReference.ResponsibilityCenter == user.DepartmentCode &&
                                                            (d.ProjectItemStatus == ProjectDetailsStatus.ForEvaluation || d.ProjectItemStatus == ProjectDetailsStatus.ItemRevised || d.ProjectItemStatus == ProjectDetailsStatus.PostedToProject || d.ProjectItemStatus == ProjectDetailsStatus.ForEvaluation)).ToList()
                                join marketSurveys in db.MarketSurveys on new { Article = (int)items.ArticleReference, Sequence = items.ItemSequence } equals new { Article = (int)marketSurveys.ArticleReference, Sequence = marketSurveys.ItemSequence }
                                into marketSurveyResults
                                from ms in marketSurveyResults.DefaultIfEmpty()
                                select new
                                {
                                    Classification = items.FKClassificationReference.Classification,
                                    ItemType = items.FKItemArticleReference.FKItemTypeReference.ItemType,
                                    Category = items.FKCategoryReference.ItemCategoryName,
                                    ItemCode = items.FKItemArticleReference.ArticleCode + "-" + items.ItemSequence,
                                    ItemName = items.ItemFullName,
                                    ItemSpecifications = items.ItemSpecifications,
                                    ProcurementSource = items.ProcurementSource.GetAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>().Name,
                                    AccountClass = abis.GetChartOfAccounts(items.FKItemArticleReference.UACSObjectClass).SubAcctName + " - " + abis.GetChartOfAccounts(items.FKItemArticleReference.UACSObjectClass).AcctName,
                                    TotalQty = items.TotalQty,
                                    UnitCost = items.ProcurementSource == ProcurementSources.AgencyToAgency ? db.ItemPrices.Where(d => (d.FKItemReference.ArticleReference == items.ArticleReference && d.FKItemReference.Sequence == items.ItemSequence) && d.IsPrevailingPrice == true).Select(d => d.UnitPrice).FirstOrDefault() : ms == null ? 0 : ms.UnitCost,
                                    EstimatedBudget = items.ProcurementSource == ProcurementSources.AgencyToAgency ? (db.ItemPrices.Where(d => (d.FKItemReference.ArticleReference == items.ArticleReference && d.FKItemReference.Sequence == items.ItemSequence) && d.IsPrevailingPrice == true).Select(d => d.UnitPrice).FirstOrDefault() * items.TotalQty) : ms == null ? 0 : (ms.UnitCost * items.TotalQty)
                                }).ToList()
                                .GroupBy(d => new
                                {
                                    Classification = d.Classification,
                                    ItemType = d.ItemType,
                                    Category = d.Category,
                                    ItemCode = d.ItemCode,
                                    ItemName = d.ItemName,
                                    ItemSpecifications = d.ItemSpecifications,
                                    ProcurementSource = d.ProcurementSource,
                                    AccountClass = d.AccountClass,
                                    UnitCost = d.UnitCost
                                })
                                .Select(d => new BasketItems
                                {
                                    Classification = d.Key.Classification,
                                    ItemType = d.Key.ItemType,
                                    Category = d.Key.Category,
                                    ItemCode = d.Key.ItemCode,
                                    ItemName = d.Key.ItemName,
                                    ItemSpecifications = d.Key.ItemSpecifications,
                                    ProcurementSource = d.Key.ProcurementSource,
                                    AccountClass = d.Key.AccountClass,
                                    TotalQty = d.Sum(x => x.TotalQty),
                                    UnitCost = d.Key.UnitCost,
                                    EstimatedBudget = d.Sum(x => x.EstimatedBudget)
                                }).ToList();
            return projectItems;
        }
        public InstitutionalItemPlan GetItem(string ItemCode, string DepartmentCode)
        {
            var institutionalPlan = new InstitutionalItemPlan();
            var itemRecord = db.Items.ToList().Where(d => d.ItemCode == ItemCode).FirstOrDefault();
            var projectDetail = db.ProjectDetails.Where(d => d.ArticleReference == itemRecord.ArticleReference && d.ItemSequence == itemRecord.Sequence &&
                                                             d.FKProjectPlanReference.Department == DepartmentCode &&
                                                             (d.ProjectItemStatus == ProjectDetailsStatus.ForRevision || d.ProjectItemStatus == ProjectDetailsStatus.ItemRevised || d.ProjectItemStatus == ProjectDetailsStatus.PostedToProject || d.ProjectItemStatus == ProjectDetailsStatus.ForEvaluation)).FirstOrDefault();
            var unitCost = itemRecord.ProcurementSource == ProcurementSources.ExternalSuppliers ?
                           db.MarketSurveys.Where(d => d.FKArticleReference.ArticleCode == itemRecord.FKArticleReference.ArticleCode && d.ItemSequence == itemRecord.Sequence).Select(d => d.UnitCost).FirstOrDefault() :
                           db.ItemPrices.Where(d => d.FKItemReference.ID == itemRecord.ID && d.IsPrevailingPrice == true).Select(d => d.UnitPrice).FirstOrDefault();
            var items = new InstitutionalItemDetails
            {
                Classification = projectDetail.FKClassificationReference.Classification,
                ItemType = projectDetail.FKItemArticleReference.FKItemTypeReference.ItemType,
                Category = projectDetail.FKCategoryReference.ItemCategoryName,
                ItemCode = projectDetail.FKItemArticleReference.ArticleCode + "-" + projectDetail.ItemSequence,
                ItemName = projectDetail.ItemFullName,
                ItemSpecifications = projectDetail.ItemSpecifications,
                ProcurementSource = projectDetail.ProcurementSource.GetAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>().Name,
                AccountClass = abis.GetChartOfAccounts(projectDetail.FKItemArticleReference.UACSObjectClass).AcctName,
                UnitCost = unitCost,
                IndividualUOMReference = projectDetail.FKUOMReference.UnitName,
                MinimumIssuanceQty = itemRecord.MinimumIssuanceQty,
                ItemImage = itemRecord.ItemImage,
            };

            var projectDetails = db.ProjectDetails.Where(d => d.ArticleReference == projectDetail.ArticleReference &&
                                                              d.ItemSequence == projectDetail.ItemSequence &&
                                                              d.FKProjectPlanReference.Department == DepartmentCode &&
                                                              (d.ProjectItemStatus == ProjectDetailsStatus.ForRevision || d.ProjectItemStatus == ProjectDetailsStatus.ItemRevised || d.ProjectItemStatus == ProjectDetailsStatus.PostedToProject || d.ProjectItemStatus == ProjectDetailsStatus.ForEvaluation))
                                 .ToList()
                                 .Select(d => new StrategicProjectPlanDetails
                                 {
                                     PAPCode = d.FKProjectPlanReference.PAPCode,
                                     Program = abis.GetPrograms(d.FKProjectPlanReference.PAPCode).GeneralDescription,
                                     UnitName = hris.GetDepartmentDetails(d.FKProjectPlanReference.Unit).Section,
                                     ProjectCode = d.FKProjectPlanReference.ProjectCode,
                                     ProjectName = d.FKProjectPlanReference.ProjectName,
                                     Description = d.FKProjectPlanReference.Description,
                                     DeliveryMonth = d.FKProjectPlanReference.DeliveryMonth,
                                     Justification = d.Justification,
                                     JanQty = d.JanQty,
                                     FebQty = d.FebQty,
                                     MarQty = d.MarQty,
                                     AprQty = d.AprQty,
                                     MayQty = d.MayQty,
                                     JunQty = d.JunQty,
                                     JulQty = d.JulQty,
                                     AugQty = d.AugQty,
                                     SepQty = d.SepQty,
                                     OctQty = d.OctQty,
                                     NovQty = d.NovQty,
                                     DecQty = d.DecQty,
                                     TotalQty = d.TotalQty
                                 }).ToList();

            institutionalPlan.Item = items;
            institutionalPlan.ProjectPlans = projectDetails;
            return institutionalPlan;
        }
        public bool UpdateProjectDetails(InstitutionalItemPlan Plan, string UserEmail)
        {
            var user = db.UserAccounts.Where(d => d.Email == UserEmail).FirstOrDefault();
            var employee = hris.GetEmployee(UserEmail);
            var itemLookUp = db.Items.ToList().Where(d => d.ItemCode == Plan.Item.ItemCode).FirstOrDefault();
            foreach (var plan in Plan.ProjectPlans)
            {
                var projectDetail = db.ProjectDetails.Where(d => d.FKProjectPlanReference.ProjectCode == plan.ProjectCode &&
                                                             d.ArticleReference == itemLookUp.ArticleReference &&
                                                             d.ItemSequence == itemLookUp.Sequence).FirstOrDefault();
                var switchBoard = db.SwitchBoard.Where(d => d.Reference == plan.ProjectCode).FirstOrDefault();
                if (plan.ResponsibilityCenterAction == ResponsibilityCenterAction.Accepted)
                {
                    projectDetail.ProjectItemStatus = ProjectDetailsStatus.ItemAccepted;
                    projectDetail.ResponsibilityCenterAction = ResponsibilityCenterAction.Accepted;
                    projectDetail.ReasonForNonAcceptance = null;
                    projectDetail.UpdateFlag = false;
                    db.SwitchBoardBody.Add(new SwitchBoardBody
                    {
                        SwitchBoardReference = switchBoard.ID,
                        ActionBy = employee.EmployeeCode,
                        Remarks = projectDetail.FKProjectPlanReference.ProjectCode + " has been updated (" + projectDetail.ProjectItemStatus.GetAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>().Name + "). " + itemLookUp.ItemCode + " - " + itemLookUp.ItemFullName + " has been " + ResponsibilityCenterAction.Accepted.GetAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>().Name + " by Responsibility Center.",
                        DepartmentCode = employee.DepartmentCode,
                        UpdatedAt = DateTime.Now
                    });
                }
                else if (plan.ResponsibilityCenterAction == ResponsibilityCenterAction.ForRevision)
                {
                    projectDetail.ProjectItemStatus = ProjectDetailsStatus.ForRevision;
                    projectDetail.ResponsibilityCenterAction = ResponsibilityCenterAction.ForRevision;
                    projectDetail.ReasonForNonAcceptance = plan.ReasonForNonAcceptance;
                    projectDetail.UpdateFlag = true;
                    db.SwitchBoardBody.Add(new SwitchBoardBody
                    {
                        SwitchBoardReference = switchBoard.ID,
                        ActionBy = employee.EmployeeCode,
                        Remarks = projectDetail.FKProjectPlanReference.ProjectCode + " has been updated (" + projectDetail.ProjectItemStatus.GetAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>().Name + "). " + itemLookUp.ItemCode + " - " + itemLookUp.ItemFullName + " " + ResponsibilityCenterAction.ForRevision.GetAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>().Name + ". \n Reason for revision: " + projectDetail.ReasonForNonAcceptance.GetAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>().Name + ".",
                        DepartmentCode = employee.DepartmentCode,
                        UpdatedAt = DateTime.Now
                    });
                }
                else
                {
                    projectDetail.ProjectItemStatus = ProjectDetailsStatus.ItemNotAccepted;
                    projectDetail.ResponsibilityCenterAction = ResponsibilityCenterAction.RemoveFromProject;
                    projectDetail.ReasonForNonAcceptance = plan.ReasonForNonAcceptance;
                    projectDetail.UpdateFlag = false;
                    db.SwitchBoardBody.Add(new SwitchBoardBody
                    {
                        SwitchBoardReference = switchBoard.ID,
                        ActionBy = employee.EmployeeCode,
                        Remarks = projectDetail.FKProjectPlanReference.ProjectCode + " has been updated (Removed From Project). " + itemLookUp.ItemCode + " - " + itemLookUp.ItemFullName + " was Removed from Project. \n Reason for removal: " + projectDetail.ReasonForNonAcceptance.GetAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>().Name + ".",
                        DepartmentCode = employee.DepartmentCode,
                        UpdatedAt = DateTime.Now
                    });
                }

                if (db.SaveChanges() == 0)
                {
                    return false;
                }

                UpdateProjectStatus(plan.ProjectCode);
            }
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
                abis.Dispose();
                hris.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}