using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using PUPFMIS.Models;
using PUPFMIS.Models.HRIS;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace PUPFMIS.BusinessLayer
{
    public class PPMPApprovalBL : Controller
    {
        private FMISDbContext db = new FMISDbContext();
        private HRISDbContext HRISdb = new HRISDbContext();
        private PPMPBL ppmpBL = new PPMPBL();
        private AccountsManagementBL accountsBL = new AccountsManagementBL();

        public List<PPMPHeaderViewModel> GetSubmittedPPMP()
        {
            var ppmp = (from users in db.UserAccounts
                        join ppmpHeader in db.PPMPHeader on users.ID equals ppmpHeader.PreparedBy
                        where ppmpHeader.Status == "Submitted"
                        select new
                        {
                            ppmpHeader.ReferenceNo,
                            ppmpHeader.OfficeReference,
                            ppmpHeader.SubmittedAt,
                            users.FKUserInformationReference.FirstName,
                            users.FKUserInformationReference.LastName,
                            ppmpHeader.Status
                        }).AsEnumerable();
            var offices = HRISdb.OfficeModel.AsEnumerable();
            return (from headers in ppmp
                    join office in offices on headers.OfficeReference equals office.ID
                    select new PPMPHeaderViewModel
                    {
                        ReferenceNo = headers.ReferenceNo,
                        OfficeName = office.OfficeName,
                        PreparedBy = headers.FirstName + " " + headers.LastName,
                        SubmittedAt = (DateTime)headers.SubmittedAt,
                        Status = headers.Status
                    }).ToList();
        }
        public int GetAcceptedItems()
        {
            return db.PPMPCSEDetails
                    .Where(d => d.AcceptanceCode == AcceptanceCodes.Accepted)
                    .GroupBy(d => d.FKItem.ItemCode)
                    .Count();
        }
        public List<PPMPSubmittedItems> GetSubmittedItems()
        {
            var submittedItems = db.PPMPCSEDetails
                .Join(db.ItemPrices, (d => d.FKItem.ID), (x => x.Item), (d, x) => new { ppmpItems = d, itemPrices = x })
                .Where(d => d.ppmpItems.FKPPMPReference.Status == "Submission Accepted" && d.itemPrices.IsPrevailingPrice == true)
                .GroupBy(d => new
                {
                    d.ppmpItems.FKItem.ID,
                    d.ppmpItems.FKItem.ItemCode,
                    d.ppmpItems.FKItem.ItemName,
                    d.ppmpItems.FKItem.ItemShortSpecifications,
                    d.ppmpItems.FKItem.ItemSpecifications,
                    d.ppmpItems.FKItem.ProcurementSource,
                    d.itemPrices.UnitPrice,
                    d.ppmpItems.FKPPMPReference.Status,
                    d.ppmpItems.FKItem.DistributionQtyPerPack,
                    PackagingUnit = d.ppmpItems.FKItem.FKPackagingUnitReference.UnitName,
                    IndividualUnit = d.ppmpItems.FKItem.FKIndividualUnitReference.UnitName
                })
                .Select(d => new PPMPSubmittedItems
                {
                    ItemID = d.Key.ID,
                    ItemCode = d.Key.ItemCode,
                    ItemName = d.Key.ItemName,
                    ItemShortSpecifications = d.Key.ItemShortSpecifications,
                    ItemSpecifications = d.Key.ItemSpecifications,
                    ProcurementSource = d.Key.ProcurementSource,
                    UnitCost = d.Key.UnitPrice,
                    QtyPerPackage = d.Key.DistributionQtyPerPack,
                    Qtr1 = d.Sum(x => x.ppmpItems.Qtr1),
                    Qtr2 = d.Sum(x => x.ppmpItems.Qtr2),
                    Qtr3 = d.Sum(x => x.ppmpItems.Qtr3),
                    Qtr4 = d.Sum(x => x.ppmpItems.Qtr4),
                    TotalQty = d.Sum(x => x.ppmpItems.TotalQty),
                    BulkUnit = d.Key.PackagingUnit,
                    IndividualUnit = d.Key.IndividualUnit
                })
                .OrderBy(d => d.ItemName)
                .ToList();

            return submittedItems;
        }
        public List<PPMPCSEDetails> GetDBMItems(int? PPMPID)
        {
            return db.PPMPCSEDetails.Where(d => d.PPMPID == PPMPID && d.FKItem.ProcurementSource == ProcurementSources.PS_DBM).ToList();
        }
        public List<PPMPCSEDetails> GetNonDBMItems(int? PPMPID)
        {
            return db.PPMPCSEDetails.Where(d => d.PPMPID == PPMPID && d.FKItem.ProcurementSource == ProcurementSources.Non_DBM).ToList();
        }
        public PPMPHeaderViewModel GetPPMPHeader(string ReferenceNo)
        {
            var ppmp = (from users in db.UserAccounts
                        join ppmpHeader in db.PPMPHeader on users.ID equals ppmpHeader.PreparedBy
                        join types in db.InventoryTypes on ppmpHeader.PPMPType equals types.ID
                        where ppmpHeader.ReferenceNo == ReferenceNo
                        select new
                        {
                            ppmpHeader.ID,
                            ppmpHeader.FiscalYear,
                            ppmpHeader.ReferenceNo,
                            ppmpHeader.OfficeReference,
                            ppmpHeader.CreatedAt,
                            ppmpHeader.SubmittedAt,
                            types.InventoryTypeName,
                            users.FKUserInformationReference.FirstName,
                            users.FKUserInformationReference.LastName,
                            users.FKUserInformationReference.Designation,
                            ppmpHeader.Status
                        }).AsEnumerable();
            var offices = HRISdb.OfficeModel.AsEnumerable();
            return (from headers in ppmp
                    join office in offices on headers.OfficeReference equals office.ID
                    select new PPMPHeaderViewModel
                    {

                        PPMPId = headers.ID,
                        ReferenceNo = headers.ReferenceNo,
                        FiscalYear = headers.FiscalYear,
                        OfficeName = office.OfficeName,
                        PPMPType = headers.InventoryTypeName,
                        PreparedBy = headers.FirstName.ToUpper() + " " + headers.LastName.ToUpper() + ", " + headers.Designation,
                        SubmittedBy = office.OfficeHead.ToUpper() + ", " + office.Designation,
                        CreatedAt = headers.CreatedAt,
                        SubmittedAt = headers.SubmittedAt,
                        Status = headers.Status
                    }).FirstOrDefault();
        }
        public PPMPCSEViewModel GetPPMPCSEDetails(string ReferenceNo)
        {
            PPMPCSEViewModel ppmpCSE = new PPMPCSEViewModel();
            ppmpCSE.PPMPHeader = GetPPMPHeader(ReferenceNo);
            ppmpCSE.PPMPItems = db.PPMPCSEDetails.Where(d => d.FKPPMPReference.ID == ppmpCSE.PPMPHeader.PPMPId).ToList();
            return ppmpCSE;
        }

        //public PPMPCSEViewModel GetSubmissionDetails(int ID)
        //{
        //    return ppmpBL.GetPPMPCSEDetails(ID.ToString());
        //}

        //=========================================================================================================//
        //public List<PPMPApprovalWorkflowViewModel> GetApprovalWorkflow(string ReferenceNo)
        //{
        //    var ppmp = (from ppmpHeader in db.PPMPHeader
        //                join workflow in db.PPMPApprovalWorkflow on ppmpHeader.ID equals workflow.PPMPId
        //                join users in db.UserAccounts on workflow.ActionMadeBy equals users.ID
        //                where ppmpHeader.ReferenceNo == ReferenceNo
        //                select new
        //                {
        //                    ppmpHeader.ID,
        //                    ppmpHeader.ReferenceNo,
        //                    users.FKUserInformationReference.FirstName,
        //                    users.FKUserInformationReference.LastName,
        //                    workflow.ActionMadeByOffice,
        //                    workflow.Status,
        //                    workflow.UpdatedAt,
        //                    workflow.Remarks
        //                }).AsEnumerable();
        //    var offices = HRISdb.OfficeModel.AsEnumerable();
        //    return (from headers in ppmp
        //            join office in offices on headers.ActionMadeByOffice equals office.ID
        //            select new PPMPApprovalWorkflowViewModel
        //            {
        //                PPMPId = headers.ID,
        //                ReferenceNo = headers.ReferenceNo,
        //                Office = office.OfficeName,
        //                Personnel = headers.FirstName + " " + headers.LastName,
        //                UpdatedAt = headers.UpdatedAt,
        //                Status = headers.Status,
        //                Remarks = headers.Remarks
        //            }).OrderByDescending(d => d.PPMPId).ToList();
        //}

        //public int GetSubmittedPPMPCount()
        //{
        //    return db.PPMPHeader.Where(d => d.Status == "Submitted").Count();
        //}

        //public List<PPMPSubmittedItems> GetAcceptedItems()
        //{
        //    var submittedItems = db.PPMPCSEDetails
        //        .Join(db.ItemPrices, (d => d.FKItem.ID), (x => x.Item), (d, x) => new { ppmpItems = d, itemPrices = x })
        //        .Where(d => d.ppmpItems.FKPPMPReference.Status == "Submission Accepted" && d.itemPrices.IsPrevailingPrice == true)
        //        .GroupBy(d => new
        //        {
        //            d.ppmpItems.FKItem.ID,
        //            d.ppmpItems.FKItem.ItemCode,
        //            d.ppmpItems.FKItem.ItemName,
        //            d.ppmpItems.FKItem.ItemShortSpecifications,
        //            d.ppmpItems.FKItem.ItemSpecifications,
        //            d.ppmpItems.FKItem.ProcurementSource,
        //            d.itemPrices.UnitPrice,
        //            d.ppmpItems.FKPPMPReference.Status
        //        })
        //        .Select(d => new PPMPSubmittedItems
        //        {
        //            ItemID = d.Key.ID,
        //            ItemCode = d.Key.ItemCode,
        //            ItemName = d.Key.ItemName,
        //            ItemShortSpecifications = d.Key.ItemShortSpecifications,
        //            ItemSpecifications = d.Key.ItemSpecifications,
        //            ProcurementSource = d.Key.ProcurementSource,
        //            UnitCost = d.Key.UnitPrice,
        //            Qtr1 = d.Sum(x => x.ppmpItems.Qtr1),
        //            Qtr2 = d.Sum(x => x.ppmpItems.Qtr2),
        //            Qtr3 = d.Sum(x => x.ppmpItems.Qtr3),
        //            Qtr4 = d.Sum(x => x.ppmpItems.Qtr4),
        //            TotalQty = d.Sum(x => x.ppmpItems.TotalQty)
        //        })
        //        .OrderBy(d => d.ItemName)
        //        .ToList();

        //    return submittedItems;
        //}

        //public List<PPMPSubmittedItems> GetAcceptedItems(string ItemCode)
        //{
        //    var submittedItems = db.PPMPCSEDetails
        //        .Join(db.ItemPrices, (d => d.FKItem.ID), (x => x.Item), (d, x) => new { ppmpItems = d, itemPrices = x })
        //        .Where(d => d.ppmpItems.FKPPMPReference.Status == "Submission Accepted" && d.itemPrices.IsPrevailingPrice == true && d.ppmpItems.FKItem.ItemCode == ItemCode)
        //        .GroupBy(d => new
        //        {
        //            d.ppmpItems.FKItem.ID,
        //            d.ppmpItems.FKItem.ItemCode,
        //            d.ppmpItems.FKItem.ItemName,
        //            d.ppmpItems.FKItem.ItemShortSpecifications,
        //            d.ppmpItems.FKItem.ItemSpecifications,
        //            d.ppmpItems.FKItem.ProcurementSource,
        //            d.itemPrices.UnitPrice,
        //            d.ppmpItems.FKPPMPReference.Status
        //        })
        //        .Select(d => new PPMPSubmittedItems
        //        {
        //            ItemID = d.Key.ID,
        //            ItemCode = d.Key.ItemCode,
        //            ItemName = d.Key.ItemName,
        //            ItemShortSpecifications = d.Key.ItemShortSpecifications,
        //            ItemSpecifications = d.Key.ItemSpecifications,
        //            ProcurementSource = d.Key.ProcurementSource,
        //            UnitCost = d.Key.UnitPrice,
        //            Qtr1 = d.Sum(x => x.ppmpItems.Qtr1),
        //            Qtr2 = d.Sum(x => x.ppmpItems.Qtr2),
        //            Qtr3 = d.Sum(x => x.ppmpItems.Qtr3),
        //            Qtr4 = d.Sum(x => x.ppmpItems.Qtr4),
        //            TotalQty = d.Sum(x => x.ppmpItems.TotalQty)
        //        })
        //        .OrderBy(d => d.ItemName)
        //        .ToList();

        //    return submittedItems;
        //}

        //public List<PPMPDistributionList> GetDistributionList(string ItemCode)
        //{
        //    var ppmp = db.PPMPCSEDetails.Where(d => d.FKPPMPReference.Status == "Submission Accepted" && d.FKItem.ItemCode == ItemCode).AsEnumerable();
        //    var offices = HRISdb.OfficeModel.AsEnumerable();

        //    return (from headers in ppmp
        //            join office in offices on headers.FKPPMPReference.OfficeReference equals office.ID
        //            select new PPMPDistributionList
        //            {
        //                ReferenceNo = headers.FKPPMPReference.ReferenceNo,
        //                Office = office.OfficeName,
        //                Qtr1 = headers.Qtr1,
        //                Qtr2 = headers.Qtr2,
        //                Qtr3 = headers.Qtr3,
        //                Qtr4 = headers.Qtr4,
        //                TotalQty = headers.TotalQty
        //            }).ToList();
        //}

        public bool AcceptSubmission(string ReferenceNo, string UserEmail)
        {
            PPMPHeaderViewModel ppmpHeader = ppmpBL.GetPPMPHeader(ReferenceNo);
            var header = db.PPMPHeader.Find(ppmpHeader.PPMPId);
            PPMPApprovalWorkflow approvalWF = new PPMPApprovalWorkflow();
            if (ppmpHeader != null)
            {
                header.Status = "Submission Accepted";
                header.SubmittedAt = DateTime.Now;
                if (db.SaveChanges() == 0)
                {
                    return false;
                }

                //approvalWF.PPMPId = ppmpHeader.PPMPId;
                //approvalWF.Remarks = "Submission of PPMP from " + ppmpHeader.OfficeName + "is accepted. Final quantity of items specified is subject to budget approval.";
                //approvalWF.UpdatedAt = DateTime.Now;
                //approvalWF.ActionMadeBy = UserID;
                //approvalWF.ActionMadeByOffice = OfficeID;
                //approvalWF.Status = "Submission Accepted";
                //db.PPMPApprovalWorkflow.Add(approvalWF);

                if (db.SaveChanges() == 1)
                {
                    return true;
                }
            }
            return false;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
                ppmpBL.Dispose();
                accountsBL.Dispose();
                HRISdb.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}