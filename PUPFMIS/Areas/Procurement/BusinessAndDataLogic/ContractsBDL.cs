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
    public class ContractsDAL : Controller
    {
        private FMISDbContext db = new FMISDbContext();
        private HRISDataAccess hris = new HRISDataAccess();
        private ABISDataAccess abis = new ABISDataAccess();
        private SystemBDL systemBDL = new SystemBDL();

        public List<ContractListVM> GetContractsForDelivery()
        {
            return db.Contract.ToList().Where(d => (d.ContractType == ContractTypes.AgencyProcurementRequest || d.ContractType == ContractTypes.PurchaseOrder) && (d.ContractStatus == ContractStatus.NTPSignedAndReceived || d.ContractStatus == ContractStatus.PartialDelivery || d.ContractStatus == ContractStatus.InspectedPartialAcceptance))
                .Select(d => new ContractListVM
                {
                    ContractCode = d.FKProcurementProjectReference.ContractCode,
                    ContractName = d.FKProcurementProjectReference.ContractName,
                    ContractPrice = d.ContractPrice,
                    FiscalYear = d.FiscalYear,
                    ReferenceNumber = d.ReferenceNumber,
                    ContractType = d.ContractType
                }).ToList();
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