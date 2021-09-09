using PUPFMIS.Models;
using PUPFMIS.Models.HRIS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace PUPFMIS.BusinessAndDataLogic
{
    public class InfrastructureProjectsBDL : Controller
    {
        private FMISDbContext db = new FMISDbContext();
        private SystemBDL systemBDL = new SystemBDL();
        private HRISDataAccess hris = new HRISDataAccess();
        private ABISDataAccess abis = new ABISDataAccess();

        public List<HRISDepartmentDetailsVM> GetDepartments(string UserEmail)
        {
            var user = db.UserAccounts.Where(d => d.Email == UserEmail).FirstOrDefault();
            var infraProjects = db.InfrastructureProject.Select(d => d.FKEndUserProjectReference.ProjectCode).ToList();
            var itemLookUp = db.Items.ToList().Where(d => d.FKArticleReference.FKItemTypeReference.ResponsibilityCenter == user.DepartmentCode).Select(d => d.ItemCode).ToList();
            var projectCodes = db.ProjectDetails.Where(d => !infraProjects.Contains(d.FKProjectPlanReference.ProjectCode) && (d.ProjectItemStatus == ProjectDetailsStatus.ForEvaluation || d.ProjectItemStatus == ProjectDetailsStatus.ItemRevised) && itemLookUp.Select(x => x).Contains(d.FKItemArticleReference.ArticleCode + "-" + d.ItemSequence))
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
        public List<InfrastructureRequestsVM> GetProjectItems(string UserEmail, string DepartmentCode)
        {
            var user = db.UserAccounts.Where(d => d.Email == UserEmail).FirstOrDefault();
            var infraPlans = db.InfrastructureProject.Select(d => d.FKEndUserProjectReference.ProjectCode).ToList();
            var projectItems = db.ProjectDetails.Where(d => !infraPlans.Contains(d.FKProjectPlanReference.ProjectCode) && 
                                                       d.FKProjectPlanReference.ProjectStatus == ProjectStatus.ForwardedToResponsibilityCenter &&
                                                       d.FKProjectPlanReference.Department == DepartmentCode &&
                                                       d.FKItemArticleReference.FKItemTypeReference.ResponsibilityCenter == user.DepartmentCode &&
                                                       (d.FKClassificationReference.Classification == "Repair and Maintenance" || d.FKClassificationReference.Classification == "Infrastructure") &&
                                                       (d.ProjectItemStatus == ProjectDetailsStatus.ForEvaluation || d.ProjectItemStatus == ProjectDetailsStatus.ItemRevised || d.ProjectItemStatus == ProjectDetailsStatus.PostedToProject || d.ProjectItemStatus == ProjectDetailsStatus.ForEvaluation)).ToList()
                                .Select(d => new InfrastructureRequestsVM
                                {
                                    ProjectCode = d.FKProjectPlanReference.ProjectCode,
                                    ProjectTitle = d.FKProjectPlanReference.ProjectName,
                                    InfrastructureType = d.FKClassificationReference.Classification,
                                    ItemCode = d.FKItemArticleReference.ArticleCode + "-" + d.ItemSequence
                                }).ToList();
            return projectItems;
        }
        public InfrastructureProjectVM InfraProjectSetup(string ProjectCode, string ItemCode)
        {
            var project = db.ProjectPlans.Where(d => d.ProjectCode == ProjectCode).FirstOrDefault();
            var item = db.Items.ToList().Where(d => d.ItemCode == ItemCode).FirstOrDefault();
            return new InfrastructureProjectVM
            {
                PAPCode = project.PAPCode,
                Program = abis.GetPrograms(project.PAPCode).GeneralDescription,
                ProjectCode = project.ProjectCode,
                EndUserProjectName = project.ProjectName,
                Description = project.Description,
                FiscalYear = project.FiscalYear,
                DeliveryMonth = systemBDL.GetMonthName(project.DeliveryMonth),
                DepartmentCode = project.Department,
                Department = hris.GetDepartmentDetails(project.Department).Department,
                Unit = project.Unit == project.Department ? hris.GetDepartmentDetails(project.Unit).Department : hris.GetDepartmentDetails(project.Unit).Section,
                EndUserProjectReference = project.ID,
                ItemCode = item.ItemCode,
                InfraProjectType = item.ItemFullName
            };
        }
        public bool SaveInfraProject(InfrastructureProjectVM InfraProject)
        {
            var itemCode = InfraProject.ItemCode.Split("-".ToCharArray(), System.StringSplitOptions.RemoveEmptyEntries).ToList();
            var articleCode = itemCode[0].ToString();
            var articleID = db.ItemArticles.Where(d => d.ArticleCode == articleCode).FirstOrDefault().ID;
            var itemSequence = itemCode[1];
            db.InfrastructureProject.Add(new InfrastructureProject
            {
                InfraProjectCode = GenerateInfraProjectCode(articleID, InfraProject.DepartmentCode, InfraProject.FiscalYear),
                ProjectTitle = InfraProject.ProjectTitle,
                ProjectLocation = InfraProject.ProjectLocation,
                ContractDuration = InfraProject.ContractDuration,
                EndUserProjectReference = InfraProject.EndUserProjectReference,
                ArticleReference = articleID,
                ItemSequence = itemSequence
            });

            if (db.SaveChanges() == 0)
            {
                return false;
            }
            return true;
        }
        public string GenerateInfraProjectCode(int ArticleID, string DepartmentCode, int FiscalYear)
        {
            var infraProjectCode = string.Empty;
            var classificationCode = db.ItemArticles.Find(ArticleID).FKItemTypeReference.ItemTypeCode;
            var count = (db.InfrastructureProject.Count() + 1).ToString();
            infraProjectCode = FiscalYear + "-" + classificationCode + "-" + DepartmentCode + "-" + (count.Length == 1 ? "0" + count : count); 
            return infraProjectCode;
        }
        public bool UpdateProjectDetails(string InfraProjectCode, string UserEmail)
        {
            var user = db.UserAccounts.Where(d => d.Email == UserEmail).FirstOrDefault();
            var infraProject = db.InfrastructureProject.Where(d => d.InfraProjectCode == InfraProjectCode).FirstOrDefault();
            var infraProjectDetails = db.InfrastructureDetailedEstimate.Where(d => d.FKInfraProjectReference.InfraProjectCode == InfraProjectCode).ToList();
            var projectPlan = db.ProjectPlans.Where(d => d.ProjectCode == infraProject.FKEndUserProjectReference.ProjectCode).FirstOrDefault();
            var projectPlanDetails = db.ProjectDetails.Where(d => d.ProjectReference == projectPlan.ID && d.FKItemArticleReference.ID == infraProject.ArticleReference && d.ItemSequence == infraProject.ItemSequence).FirstOrDefault();

            projectPlanDetails.ItemSpecifications = projectPlanDetails.ItemFullName;
            projectPlanDetails.ItemFullName = infraProject.ProjectTitle;
            projectPlanDetails.UnitCost = infraProjectDetails.Sum(d => d.TotalAmount);
            projectPlanDetails.EstimatedBudget = infraProjectDetails.Sum(d => d.TotalAmount) * projectPlanDetails.TotalQty;
            projectPlanDetails.ResponsibilityCenterAction = ResponsibilityCenterAction.Accepted;
            projectPlanDetails.ProjectItemStatus = ProjectDetailsStatus.ItemAccepted;

            if (db.SaveChanges() == 0)
            {
                return false;
            }

            var projectDetails = new List<ProjectDetails>();
            projectDetails.AddRange(infraProjectDetails.Select(d => new ProjectDetails
            {
                ProjectReference = projectPlan.ID,
                ItemFullName = d.FKInfraMaterialsReference.ItemName,
                ItemSpecifications = d.FKInfraMaterialsReference.ItemSpecifications,
                ProposalType = projectPlanDetails.ProposalType,
                ProcurementSource = projectPlanDetails.ProcurementSource,
                UOMReference = d.FKInfraMaterialsReference.FKUOMReference.ID,
                ProjectItemStatus = ProjectDetailsStatus.ItemAccepted,
                JanQty = projectPlan.DeliveryMonth == 1 ? d.Quantity : 0,
                FebQty = projectPlan.DeliveryMonth == 2 ? d.Quantity : 0,
                MarQty = projectPlan.DeliveryMonth == 3 ? d.Quantity : 0,
                AprQty = projectPlan.DeliveryMonth == 4 ? d.Quantity : 0,
                MayQty = projectPlan.DeliveryMonth == 5 ? d.Quantity : 0,
                JunQty = projectPlan.DeliveryMonth == 6 ? d.Quantity : 0,
                JulQty = projectPlan.DeliveryMonth == 7 ? d.Quantity : 0,
                AugQty = projectPlan.DeliveryMonth == 8 ? d.Quantity : 0,
                SepQty = projectPlan.DeliveryMonth == 9 ? d.Quantity : 0,
                OctQty = projectPlan.DeliveryMonth == 10 ? d.Quantity : 0,
                NovQty = projectPlan.DeliveryMonth == 11 ? d.Quantity : 0,
                DecQty = projectPlan.DeliveryMonth == 12 ? d.Quantity : 0,
                UACS = projectPlanDetails.FKItemArticleReference.UACSObjectClass,
                ClassificationReference = projectPlanDetails.FKItemArticleReference.FKItemTypeReference.ItemClassificationReference,
                TotalQty = d.Quantity,
                UnitCost = d.TotalAmount,
                EstimatedBudget = d.TotalAmount,
                UpdateFlag = false,
                ResponsibilityCenterAction = ResponsibilityCenterAction.Accepted
            }).ToList());

            db.ProjectDetails.AddRange(projectDetails);
            if (db.SaveChanges() == 0)
            {
                return false;
            }

            UpdateProjectStatus(projectPlan.ProjectCode);

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
            }
            base.Dispose(disposing);
        }
    }
}