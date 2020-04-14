using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PUPFMIS.Models;
using PUPFMIS.Models.AIS;
using FluentValidation;

namespace PUPFMIS.BusinessAndDataLogic
{
    public class ServicesBL : Controller
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
    public class ServiceDAL : Controller
    {
        private FMISDbContext db = new FMISDbContext();

        public List<Services> GetServices(bool DeleteFlag)
        {
            return db.Services.Where(d => d.PurgeFlag == DeleteFlag).OrderBy(d => d.ServiceName).ToList();
        }
        public Services GetServiceByID(int ID, bool DeleteFlag)
        {
            return db.Services.Where(d => d.ID == ID && d.PurgeFlag == DeleteFlag).FirstOrDefault();
        }
        public Services GetServiceByCode(string ServiceCode, bool DeleteFlag)
        {
            return db.Services.Where(d => d.ServiceCode == ServiceCode && d.PurgeFlag == DeleteFlag).FirstOrDefault();
        }
        public Services GetServiceByName(string ServiceName, bool DeleteFlag)
        {
            return db.Services.Where(d => d.ServiceName == ServiceName && d.PurgeFlag == DeleteFlag).FirstOrDefault();
        }
        public List<PhilGEPSCategories> GetCategories()
        {
            ItemCategoriesDAL itemDAL = new ItemCategoriesDAL();
            return itemDAL.GetPhilGEPSCategories("Services", false);
        }
        public List<InventoryType> GetInventoryTypes()
        {
            InventoryTypeDataAccess inventoryTypeDAL = new InventoryTypeDataAccess();
            return inventoryTypeDAL.GetInventoryTypes(false);
        }
        public List<ChartOfAccounts> GetAccounts()
        {
            ChartOfAccountsDAL coaDAL = new ChartOfAccountsDAL();
            return coaDAL.GetChartOfAccounts();
        }
        public bool AddServiceRecord(Services Service, out string Message)
        {
            Service.ServiceCode = GenerateServiceCode(Service.ServiceTypeReference, Service.ServiceCategoryReference);
            Service.CreatedAt = DateTime.Now;
            Service.PurgeFlag = false;
            db.Services.Add(Service);
            if (db.SaveChanges() == 0)
            {
                Message = "Something went wrong. Please try again.";
                return false;
            }
            Message = string.Empty;
            return true;
        }
        public bool UpdateService(Services Service, out string Message)
        {
            var service = db.Services.Find(Service.ID);
            service.ServiceName = Service.ServiceName;
            service.ItemShortSpecifications = Service.ItemShortSpecifications;
            service.ServiceCategoryReference = Service.ServiceCategoryReference;
            service.ServiceTypeReference = Service.ServiceTypeReference;
            service.UpdatedAt = DateTime.Now;
            if(db.SaveChanges() == 0)
            {
                Message = "Something went wrong. Please try again";
                return false;
            }
            Message = string.Empty;
            return true;
        }
        public bool PurgeService(Services Service, out string Message)
        {
            var service = db.Services.Find(Service.ID);
            service.PurgeFlag = true;
            service.DeletedAt = DateTime.Now;
            if (db.SaveChanges() == 0)
            {
                Message = "Something went wrong. Please try again";
                return false;
            }
            Message = string.Empty;
            return true;
        }
        public bool RestoreService(string ServiceCode, out string Message)
        {
            var service = GetServiceByCode(ServiceCode, true);
            service.PurgeFlag = false;
            service.UpdatedAt = DateTime.Now;
            if (db.SaveChanges() == 0)
            {
                Message = "Something went wrong. Please try again";
                return false;
            }
            Message = string.Empty;
            return true;
        }
        private string GenerateServiceCode(int InventoryTypeCode, int CategoryID)
        {
            string ServiceCode = string.Empty;
            var category = (CategoryID.ToString().Length == 1) ? "00" + CategoryID.ToString() : (CategoryID.ToString().Length == 2) ? "0" + CategoryID.ToString() : CategoryID.ToString();
            var series = (db.Services.Where(d => d.ServiceTypeReference == InventoryTypeCode && d.ServiceCategoryReference == CategoryID).Count() + 1).ToString();
            series = (series.Length == 1) ? "00" + series : (series.Length == 2) ? "0" + series : series;
            var inventoryTypeCode = (InventoryTypeCode.ToString().Length == 1) ? "0" + InventoryTypeCode.ToString() : InventoryTypeCode.ToString();
            ServiceCode = inventoryTypeCode + "-" + category + "-" + series;
            return ServiceCode;
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