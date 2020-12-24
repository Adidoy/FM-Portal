using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using PUPFMIS.Models;
using PUPFMIS.Models.AIS;
using PUPFMIS.Models.HRIS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace PUPFMIS.BusinessAndDataLogic
{
    public class APPReviewDAL : Controller
    {
        private FMISDbContext db = new FMISDbContext();
        private HRISDataAccess hrisDataAccess = new HRISDataAccess();
        private ABISDataAccess abisDataAccess = new ABISDataAccess();
        private SystemBDL systemBDL = new SystemBDL();

        public AnnualProcurementPlanVM GetAnnualProcurementPlan(string ReferenceNo)
        {
            AgencyDetails agencyDetails = db.AgencyDetails.First();
            var APPHeader = db.APPHeader.Where(d => d.ReferenceNo == ReferenceNo).FirstOrDefault();
            var APPDetails = new List<AnnualProcurementPlanDetailsVM>();
            var ProcurementPrograms = db.APPDetails.Where(d => d.FKAPPHeaderReference.ReferenceNo == ReferenceNo).ToList();

            foreach (var item in ProcurementPrograms)
            {
                var procurementModes = item.APPModeOfProcurementReference.Split("_".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).ToList();
                var modesOfProcurement = string.Empty;
                modesOfProcurement = "<ul>";
                for (int i = 0; i < procurementModes.Count; i++)
                {
                    modesOfProcurement += "<li>" + db.ProcurementModes.Find(int.Parse(procurementModes[i])).ModeOfProcurementName + "</li>";
                }
                modesOfProcurement += "</ul>";

                APPDetails.Add(new AnnualProcurementPlanDetailsVM
                {
                    UACS = item.ObjectClassification,
                    ObjectClassification = abisDataAccess.GetDetailedChartOfAccounts().Where(d => d.UACS_Code == item.ObjectClassification).FirstOrDefault().AcctName,
                    PAPCode = item.PAPCode,
                    ProcurementProject = item.ProcurementProgram,
                    ModeOfProcurement = modesOfProcurement,
                    EndUser = item.EndUser,
                    Month = item.Month,
                    StartMonth = item.StartMonth,
                    EndMonth = item.EndMonth,
                    FundCluster = item.FundSourceReference,
                    FundDescription = abisDataAccess.GetFundSources(item.FundSourceReference).FUND_DESC,
                    MOOE = item.MOOEAmount,
                    CapitalOutlay = item.COAmount,
                    EstimatedBudget = item.Total,
                    Remarks = item.Remarks
                });
            }

            var hope = hrisDataAccess.GetFullDepartmentDetails(APPHeader.ApprovedByDepartmentCode);
            var property = hrisDataAccess.GetFullDepartmentDetails(agencyDetails.PropertyOfficeReference);
            var accounting = hrisDataAccess.GetFullDepartmentDetails(agencyDetails.AccountingOfficeReference);
            var procurement = hrisDataAccess.GetFullDepartmentDetails(APPHeader.PreparedByDepartmentCode);
            var bac = hrisDataAccess.GetFullDepartmentDetails(APPHeader.RecommendingApprovalDepartmentCode);

            return new AnnualProcurementPlanVM
            {
                APPType = APPHeader.APPType,
                ReferenceNo = APPHeader.ReferenceNo,
                FiscalYear = APPHeader.FiscalYear,
                AccountCode = agencyDetails.AccountCode,
                AgencyName = agencyDetails.AgencyName,
                Address = agencyDetails.Address,
                Region = agencyDetails.Region,
                OrganizationType = agencyDetails.OrganizationType,
                ApprovedBy = APPHeader.ApprovedBy + "\n" + APPHeader.ApprovedByDesignation,
                PreparedBy = APPHeader.PreparedBy + "\n" + APPHeader.PreparedByDesignation + "\n" + hrisDataAccess.GetDepartmentDetails(APPHeader.PreparedByDepartmentCode).Department,
                CertifiedBy = APPHeader.RecommendingApproval + "\n" + APPHeader.RecommendingApprovalDesignation + "\n" + hrisDataAccess.GetDepartmentDetails(APPHeader.RecommendingApprovalDepartmentCode).Department,
                ProcurementOfficer = procurement.DepartmentHead + "\n" + procurement.DepartmentHeadDesignation + "\n" + procurement.Department,
                BACSecretariat = bac.SectionCode == null ? bac.DepartmentHead + "\n" + bac.DepartmentHeadDesignation + "\n" + bac.Department : bac.SectionHead + "\n" + bac.SectionHeadDesignation + "\n" + bac.Section,
                CreatedAt = APPHeader.CreatedAt,
                PreparedAt = APPHeader.PreparedAt,
                CertifiedAt = APPHeader.RecommendedAt,
                ApprovedAt = APPHeader.ApprovedAt,
                APPLineItems = APPDetails
            };
        }
        public List<ProcurementProjectsVM> GetProcurementProgams(string ReferenceNo)
        {
            var procurementPrograms = new List<ProcurementProjectsVM>();
            var institutionalPrograms = db.APPDetails.Where(d => d.FKAPPHeaderReference.ReferenceNo == ReferenceNo).ToList();
            foreach (var program in institutionalPrograms)
            {
                var procurementModes = program.APPModeOfProcurementReference.Split("_".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).ToList();
                var modesOfProcurement = string.Empty;
                modesOfProcurement = "<ul class='p-0 m-0'>";
                for (int i = 0; i < procurementModes.Count; i++)
                {
                    modesOfProcurement += "<li>" + db.ProcurementModes.Find(int.Parse(procurementModes[i])).ModeOfProcurementName + "</li>";
                }
                modesOfProcurement += "</ul>";
                procurementPrograms.Add(new ProcurementProjectsVM
                {
                    APPReference = ReferenceNo,
                    Month = program.Month,
                    PAPCode = program.PAPCode,
                    UACS = program.ObjectClassification,
                    ProcurementProgram = program.ProcurementProgram,
                    ApprovedBudget = program.Total,
                    ObjectClassification = abisDataAccess.GetChartOfAccounts(program.ObjectClassification).AcctName,
                    FundCluster = program.FundSourceReference.Replace("\r\n", ""),
                    FundSource = "<ul><li>" + abisDataAccess.GetFundSources(program.FundSourceReference).FUND_DESC + "</li></ul>",
                    StartMonth = program.StartMonth,
                    EndMonth = program.EndMonth,
                    APPModeOfProcurement = modesOfProcurement,
                });
            }

            return procurementPrograms;
        }
        public ProcurementProjectsVM GetProcurementProgamsByPAPCode(string PAPCode, string APPReferenceNo)
        {
            var program = db.APPDetails.Where(d => d.PAPCode == PAPCode).FirstOrDefault();
            var procurementModes = program.APPModeOfProcurementReference.Split("_".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).ToList();
            var modesOfProcurement = string.Empty;
            for (int i = 0; i < procurementModes.Count; i++)
            {
                modesOfProcurement += db.ProcurementModes.Find(int.Parse(procurementModes[i])).ModeOfProcurementName + "\n";
            }
            return new ProcurementProjectsVM
            {
                APPReference = APPReferenceNo,
                Month = program.Month,
                PAPCode = program.PAPCode,
                UACS = program.ObjectClassification,
                ProcurementProgram = program.ProcurementProgram,
                ApprovedBudget = program.Total,
                ObjectClassification = abisDataAccess.GetChartOfAccounts(program.ObjectClassification).AcctName,
                FundCluster = program.FundSourceReference.Replace("\r\n", ""),
                FundSource = abisDataAccess.GetFundSources(program.FundSourceReference).FUND_DESC,
                MOOETotal = program.MOOEAmount,
                CapitalOutlayTotal = program.COAmount,
                TotalEstimatedBudget = program.Total,
                EndUser = hrisDataAccess.GetDepartmentDetails(program.EndUser).Department,
                StartMonth = program.StartMonth,
                EndMonth = program.EndMonth,
                APPModeOfProcurement = modesOfProcurement,
                Remarks = program.Remarks,
                Items = GetProjectItems(PAPCode)
            };
        }
        private List<ProcurementProjectItemsVM> GetProjectItems(string PAPCode)
        {
            var procurementItems = new List<ProcurementProjectItemsVM>();
            var appDetails = db.APPDetails.Where(d => d.PAPCode == PAPCode).First();
            var procurementProgram = db.APPDetails.Where(d => d.PAPCode == PAPCode).FirstOrDefault();
            var items = db.ProjectPlanItems.Where(d => d.APPLineReference == appDetails.ID).ToList();
            var services = db.ProjectPlanServices.Where(d => d.APPLineReference == appDetails.ID).ToList();

            foreach (var item in items)
            {
                var purchaseRequestHeader = db.PurchaseRequestHeader.Find(item.PRReference);
                procurementItems.Add(new ProcurementProjectItemsVM
                {
                    EndUser = hrisDataAccess.GetDepartmentDetails(item.FKProjectReference.Department).Department + (item.FKProjectReference.Department == item.FKProjectReference.Unit ? string.Empty : " - " + hrisDataAccess.GetDepartmentDetails(item.FKProjectReference.Unit).Department),
                    ProjectCode = item.FKProjectReference.ProjectCode,
                    ProjectName = item.FKProjectReference.ProjectName,
                    ItemCode = item.FKItemReference.ItemCode,
                    ItemName = item.FKItemReference.ItemFullName,
                    ItemSpecifications = item.FKItemReference.ItemShortSpecifications,
                    ProcurementSource = item.FKItemReference.ProcurementSource == ProcurementSources.PS_DBM ? "Procurement System - Department of Budget and Management" : "Private Suppliers",
                    InventoryType = item.FKItemReference.FKItemTypeReference.FKInventoryTypeReference.InventoryTypeName,
                    ItemCategory = item.FKItemReference.FKCategoryReference.ItemCategoryName,
                    PackagingUOMReference = item.FKItemReference.FKPackagingUnitReference.Abbreviation,
                    IndividualUOMReference = item.FKItemReference.FKIndividualUnitReference.Abbreviation,
                    TotalQty = item.PPMPTotalQty,
                    EstimatedBudget = item.PPMPEstimatedBudget,
                    UnitCost = item.UnitCost,
                    DeliveryMonth = systemBDL.GetMonthName(item.FKProjectReference.ProjectMonthStart) + ", " + item.FKProjectReference.FiscalYear.ToString(),
                    PRNumber = item.PRReference == null ? null : purchaseRequestHeader.PRNumber,
                    DatePRReceived = item.PRReference == null ? null : purchaseRequestHeader.ReceivedAt,
                    Status = item.Status
                });
            }

            foreach (var item in services)
            {
                procurementItems.Add(new ProcurementProjectItemsVM
                {
                    EndUser = hrisDataAccess.GetDepartmentDetails(item.FKProjectReference.Department).Department + (item.FKProjectReference.Department == item.FKProjectReference.Unit ? string.Empty : " - " + hrisDataAccess.GetDepartmentDetails(item.FKProjectReference.Unit).Department),
                    ProjectCode = item.FKProjectReference.ProjectCode,
                    ProjectName = item.FKProjectReference.ProjectName,
                    ItemCode = item.FKItemReference.ItemCode,
                    ItemName = item.FKItemReference.ItemFullName,
                    ItemSpecifications = item.ItemSpecifications,
                    ProcurementSource = item.FKItemReference.ProcurementSource == ProcurementSources.PS_DBM ? "Procurement System - Department of Budget and Management" : "Private Suppliers",
                    InventoryType = item.FKItemReference.FKItemTypeReference.FKInventoryTypeReference.InventoryTypeName,
                    ItemCategory = item.FKItemReference.FKCategoryReference.ItemCategoryName,
                    PackagingUOMReference = item.FKItemReference.PackagingUOMReference == null ? string.Empty : item.FKItemReference.FKPackagingUnitReference.Abbreviation,
                    IndividualUOMReference = item.FKItemReference.IndividualUOMReference == null ? string.Empty : item.FKItemReference.FKIndividualUnitReference.Abbreviation,
                    TotalQty = item.PPMPQuantity,
                    EstimatedBudget = item.PPMPEstimatedBudget,
                    UnitCost = item.UnitCost,
                    DeliveryMonth = systemBDL.GetMonthName(item.FKProjectReference.ProjectMonthStart) + ", " + item.FKProjectReference.FiscalYear.ToString(),
                    PRNumber = item.PRReference == null ? null : db.PurchaseRequestHeader.Find(item.PRReference).PRNumber,
                    Status = item.Status
                });
            }

            procurementItems = procurementItems
                .GroupBy(d => new { d.ItemCode, d.ItemName, d.TotalQty, d.ItemSpecifications, d.UnitCost })
                .Select(d => new ProcurementProjectItemsVM
                {
                    ItemCode = d.Key.ItemCode,
                    ItemName = d.Key.ItemName,
                    ItemSpecifications = d.Key.ItemSpecifications,
                    TotalQty = d.Sum(x => x.TotalQty),
                    UnitCost = d.Key.UnitCost,
                    EstimatedBudget = d.Sum(x => x.TotalQty) * d.Key.UnitCost
                }).ToList();

            return procurementItems.OrderBy(d => d.ItemCode).ToList();
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
                abisDataAccess.Dispose();
                hrisDataAccess.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}