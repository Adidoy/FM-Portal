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
    
    public class ProcurementPurchaseRequestBL : Controller
    {
        private FMISDbContext db = new FMISDbContext();
        
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }

    public class ProcurementPurchaseRequestDAL : Controller
    {
        private FMISDbContext db = new FMISDbContext();
        private HRISDataAccess hris = new HRISDataAccess();
        private ABISDataAccess abis = new ABISDataAccess();
        private SystemBDL systemBDL = new SystemBDL();

        public List<PurchaseRequestVM> GetPurchaseRequests(string UserEmail)
        {
            var purchaseRequestVM = new List<PurchaseRequestVM>();
            var user = db.UserAccounts.Where(d => d.Email == UserEmail).FirstOrDefault();
            var userRole = user.FKRoleReference.Role;

            if (userRole == "Procurement Staff")
            {
                var purchaseRequestItemsReferences = db.PurchaseRequestDetails
                    .Where(d => d.FKItemReference.ProcurementSource == ProcurementSources.PS_DBM)
                    .GroupBy(d => d.FKPRHeaderReference.PRNumber)
                    .Select(d => new { PRNumber = d.Key }).ToList()
                    .Select(d => d.PRNumber).ToList();

                var purchaseRequestHeaders = db.PurchaseRequestHeader.Where(d => purchaseRequestItemsReferences.Contains(d.PRNumber) && d.ReceivedAt == null).ToList();
                foreach(var header in purchaseRequestHeaders)
                {
                    var purchaseRequestDetailsVM = new List<PurchaseRequestDetailsVM>();
                    var details = db.PurchaseRequestDetails.Where(d => d.FKPRHeaderReference.PRNumber == header.PRNumber).ToList();
                    foreach (var detail in details)
                    {
                        purchaseRequestDetailsVM.Add(new PurchaseRequestDetailsVM
                        {
                            ItemCode = detail.FKItemReference.ItemCode,
                            ItemName = detail.FKItemReference.ItemFullName,
                            ItemSpecifications = detail.ItemSpecifications,
                            UOM = detail.FKUnitReference.UnitName,
                            Quantity = detail.Quantity,
                            UnitCost = detail.UnitCost,
                            TotalCost = detail.TotalCost
                        });
                    }

                    purchaseRequestVM.Add(new PurchaseRequestVM
                    {
                        PRNumber = header.PRNumber,
                        FundCluster = header.FundCluster,
                        Purpose = header.Purpose,
                        RequestedBy = header.RequestedBy,
                        RequestedByDesignation = header.RequestedByDesignation,
                        RequestedByDepartment = hris.GetDepartmentDetails(header.RequestedByDepartment).Department,
                        ApprovedBy = header.ApprovedBy,
                        ApprovedByDesignation = header.ApprovedByDesignation,
                        ApprovedByDepartment = hris.GetDepartmentDetails(header.ApprovedByDepartment).Department,
                        CreatedAt = header.CreatedAt.ToString("dd MMMM yyyy"),
                        Department = hris.GetDepartmentDetails(header.Department).Department,
                        PRDetails = purchaseRequestDetailsVM
                    }); 
                }
            }

            if (userRole == "Project Coordinator" || userRole == "Project Support")
            {
                var purchaseRequestItemsReferences = db.PurchaseRequestDetails
                   .Where(d => d.FKItemReference.ProcurementSource == ProcurementSources.Non_DBM)
                   .GroupBy(d => d.FKPRHeaderReference.PRNumber)
                   .Select(d => new { PRNumber = d.Key }).ToList()
                   .Select(d => d.PRNumber).ToList();

                var purchaseRequestHeaders = db.PurchaseRequestHeader.Where(d => purchaseRequestItemsReferences.Contains(d.PRNumber) && d.ReceivedAt == null).ToList();
                foreach (var header in purchaseRequestHeaders)
                {
                    var purchaseRequestDetailsVM = new List<PurchaseRequestDetailsVM>();
                    var details = db.PurchaseRequestDetails.Where(d => d.FKPRHeaderReference.PRNumber == header.PRNumber).ToList();
                    foreach (var detail in details)
                    {
                        purchaseRequestDetailsVM.Add(new PurchaseRequestDetailsVM
                        {
                            ItemCode = detail.FKItemReference.ItemCode,
                            ItemName = detail.FKItemReference.ItemFullName,
                            ItemSpecifications = detail.ItemSpecifications,
                            UOM = detail.UnitReference == null ? null : detail.FKUnitReference.UnitName,
                            Quantity = detail.Quantity,
                            UnitCost = detail.UnitCost,
                            TotalCost = detail.TotalCost
                        });
                    }

                    purchaseRequestVM.Add(new PurchaseRequestVM
                    {
                        PRNumber = header.PRNumber,
                        FundCluster = header.FundCluster,
                        Purpose = header.Purpose,
                        RequestedBy = header.RequestedBy,
                        RequestedByDesignation = header.RequestedByDesignation,
                        RequestedByDepartment = hris.GetDepartmentDetails(header.RequestedByDepartment).Department,
                        ApprovedBy = header.ApprovedBy,
                        ApprovedByDesignation = header.ApprovedByDesignation,
                        ApprovedByDepartment = hris.GetDepartmentDetails(header.ApprovedByDepartment).Department,
                        CreatedAt = header.CreatedAt.ToString("dd MMMM yyyy"),
                        Department = hris.GetDepartmentDetails(header.Department).Department,
                        PRDetails = purchaseRequestDetailsVM
                    });
                }
            }

            return purchaseRequestVM;
        }
        public PurchaseRequestVM GetPurchaseRequest(string PRNumber)
        {
            var header = db.PurchaseRequestHeader.Where(d => d.PRNumber == PRNumber).FirstOrDefault();
            
            var purchaseRequestDetailsVM = new List<PurchaseRequestDetailsVM>();
            var details = db.PurchaseRequestDetails.Where(d => d.FKPRHeaderReference.PRNumber == header.PRNumber).ToList();
            foreach (var detail in details)
            {
                purchaseRequestDetailsVM.Add(new PurchaseRequestDetailsVM
                {
                    ItemCode = detail.FKItemReference.ItemCode,
                    ItemName = detail.FKItemReference.ItemFullName,
                    ItemSpecifications = detail.ItemSpecifications,
                    UOM = detail.UnitReference == null ? null : detail.FKUnitReference.UnitName,
                    Quantity = detail.Quantity,
                    UnitCost = detail.UnitCost,
                    TotalCost = detail.TotalCost
                });
            }

            return new PurchaseRequestVM
            {
                PRNumber = header.PRNumber,
                FundCluster = header.FundCluster,
                Purpose = header.Purpose,
                RequestedBy = header.RequestedBy,
                RequestedByDesignation = header.RequestedByDesignation,
                RequestedByDepartment = hris.GetDepartmentDetails(header.RequestedByDepartment).Department,
                ApprovedBy = header.ApprovedBy,
                ApprovedByDesignation = header.ApprovedByDesignation,
                ApprovedByDepartment = hris.GetDepartmentDetails(header.ApprovedByDepartment).Department,
                CreatedAt = header.CreatedAt.ToString("dd MMMM yyyy"),
                Department = hris.GetDepartmentDetails(header.Department).Department,
                PRDetails = purchaseRequestDetailsVM
            };
        }
        public bool PostPRReceive(string PRNumber, string UserEmail)
        {
            var header = db.PurchaseRequestHeader.Where(d => d.PRNumber == PRNumber).FirstOrDefault();
            var user = db.UserAccounts.Where(d => d.Email == UserEmail).FirstOrDefault();
            header.ReceivedAt = DateTime.Now;
            header.ReceivedBy = user.EmpCode;
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
                systemBDL.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}