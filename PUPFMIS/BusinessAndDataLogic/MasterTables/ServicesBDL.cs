//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;
//using PUPFMIS.Models;
//using PUPFMIS.Models.AIS;
//using FluentValidation;
//using System.Data.Entity.Infrastructure;
//using System.Data.Entity;

//namespace PUPFMIS.BusinessAndDataLogic
//{
//    public class ServicesBL : Controller
//    {
//        private FMISDbContext db = new FMISDbContext();

//        protected override void Dispose(bool disposing)
//        {
//            if (disposing)
//            {
//                db.Dispose();
//            }
//            base.Dispose(disposing);
//        }
//    }
//    public class ServiceDAL : Controller
//    {
//        private FMISDbContext db = new FMISDbContext();
//        private LogsMasterTables log = new LogsMasterTables();
//        private AccountsManagementBL accountsBL = new AccountsManagementBL();
//        private TEMPAccounting abdb = new TEMPAccounting();

//        public List<ItemCategory> GetCategories(ProcurementSources ProcurementSource)
//        {
//            return db.ItemCategories.ToList();
//        }
//        public List<Services> GetServices(bool DeleteFlag)
//        {
//            return db.Services.Where(d => d.PurgeFlag == DeleteFlag).OrderBy(d => d.ServiceName).ToList();
//        }
//        public Services GetServiceByID(int ID, bool DeleteFlag)
//        {
//            return db.Services.Where(d => d.ID == ID && d.PurgeFlag == DeleteFlag).FirstOrDefault();
//        }
//        public Services GetServiceByCode(string ServiceCode, bool DeleteFlag)
//        {
//            return db.Services.Where(d => d.ServiceCode == ServiceCode && d.PurgeFlag == DeleteFlag).FirstOrDefault();
//        }
//        public ServicesVM GetServiceDetails(string ServiceCode)
//        {
//            var service = db.Services.Where(d => d.ServiceCode == ServiceCode).FirstOrDefault();
//            var UACS = abdb.ChartOfAccounts.Where(d => d.UACS_Code == service.AccountClass).FirstOrDefault();
//            return new ServicesVM
//            {
//                ID = service.ID,
//                ServiceCode = service.ServiceCode,
//                ServiceName = service.ServiceName,
//                ItemShortSpecifications = service.ItemShortSpecifications,
//                AccountClass = UACS.UACS_Code + " - " + UACS.AcctName,
//                ProcurementSource = service.ProcurementSource == ProcurementSources.PS_DBM ? "Procurement System - Department of Budget and Management" : "Private Suppliers",
//                ServiceType = service.FKInventoryTypeReference.InventoryTypeName,
//                ServiceCategory = service.FKCategoryReference.ItemCategoryName,
//                PurgeFlag = service.PurgeFlag == true ? "Yes" : "No",
//                CreatedAt = service.CreatedAt.ToString("dd MMMM yyyy hh:mm tt"),
//                UpdatedAt = service.UpdatedAt == null ? "N/A" : ((DateTime)service.UpdatedAt).ToString("dd MMMM yyyy hh:mm tt"),
//                DeletedAt = service.DeletedAt == null ? "N/A" : ((DateTime)service.DeletedAt).ToString("dd MMMM yyyy hh:mm tt")
//            };
//        }
//        public Services GetServiceByName(string ServiceName, bool DeleteFlag)
//        {
//            return db.Services.Where(d => d.ServiceName == ServiceName && d.PurgeFlag == DeleteFlag).FirstOrDefault();
//        }
//        public List<InventoryType> GetInventoryTypes()
//        {
//            InventoryTypeDataAccess inventoryTypeDAL = new InventoryTypeDataAccess();
//            return inventoryTypeDAL.GetInventoryTypes(false);
//        }
//        public List<ChartOfAccounts> GetAccounts()
//        {
//            return abdb.ChartOfAccounts.Where(d => d.ClassCode == "5" && d.GenAcctName == "Maintenance and Other Operating Expenses").OrderBy(d => d.UACS_Code).ToList();
//        }
//        public bool AddServiceRecord(Services Service, string UserEmail, out string Message)
//        {
//            Services service = db.Services.Find(Service.ID);
//            DbPropertyValues currentValues;

//            if (service == null)
//            {
//                Service.ServiceCode = GenerateServiceCode(Service.ServiceTypeReference, Service.ServiceCategoryReference);
//                Service.PurgeFlag = false;
//                Service.CreatedAt = DateTime.Now;
//                db.Services.Add(Service);

//                currentValues = db.ChangeTracker.Entries().Where(d => d.State == EntityState.Added).First().CurrentValues;
//                log.Action = "Add Record";

//                if (db.SaveChanges() == 0)
//                {
//                    Message = "Something went wrong. Please try again.";
//                    return false;
//                }

//                log.AuditableKey = Service.ID;
//                log.ProcessedBy = accountsBL.GetUser(UserEmail).UserID;
//                log.TableName = "master_services";
//                MasterTablesLogger logger = new MasterTablesLogger();
//                logger.Log(log, null, currentValues);

//                Message = "Saved";
//                return true;
//            }
//            Message = "Something went wrong. Please try again.";
//            return false;
//        }
//        public bool UpdateServiceRecord(Services Service, string UserEmail, out string Message)
//        {
//            Services service = db.Services.Find(Service.ID);
//            DbPropertyValues currentValues;

//            if (service == null)
//            {
//                var baseItemCode = Service.ServiceCode.Substring(0, 10);
//                var newSeriesEnd = (db.Items.Where(d => d.ItemCode.StartsWith(baseItemCode)).Count()).ToString();
//                var NewItemCode = baseItemCode + (newSeriesEnd.Length == 1 ? "0" + newSeriesEnd : newSeriesEnd);

//                Service.ServiceCode = NewItemCode;
//                Service.PurgeFlag = false;
//                Service.CreatedAt = DateTime.Now;
//                db.Services.Add(Service);

//                currentValues = db.ChangeTracker.Entries().Where(d => d.State == EntityState.Added).First().CurrentValues;
//                log.Action = "Add Record";

//                if (db.SaveChanges() == 0)
//                {
//                    Message = "Something went wrong. Please try again.";
//                    return false;
//                }

//                log.AuditableKey = Service.ID;
//                log.ProcessedBy = accountsBL.GetUser(UserEmail).UserID;
//                log.TableName = "master_services";
//                MasterTablesLogger logger = new MasterTablesLogger();
//                logger.Log(log, null, currentValues);

//                if (db.SaveChanges() == 1)
//                {
//                    Message = "Saved";
//                    return true;
//                }
//                Message = "Something went wrong. Please try again.";
//                return false;
//            }
//            Message = "Something went wrong. Please try again.";
//            return false;
//        }
//        public bool DeleteServiceRecord(string ServiceCode, string UserEmail, out string Message)
//        {
//            Services service = db.Services.Where(d => d.ServiceCode == ServiceCode).FirstOrDefault();
//            DbPropertyValues currentValues;
//            DbPropertyValues originalValues;

//            if (service != null)
//            {
//                service.PurgeFlag = true;
//                service.DeletedAt = DateTime.Now;

//                log.Action = "Purge Record";

//                currentValues = db.ChangeTracker.Entries().Where(t => t.State == EntityState.Modified).First().CurrentValues;
//                originalValues = db.ChangeTracker.Entries().Where(t => t.State == EntityState.Modified).First().OriginalValues;
//                log.AuditableKey = service.ID;
//                log.ProcessedBy = accountsBL.GetUser(UserEmail).UserID;
//                log.TableName = "master_services";
//                MasterTablesLogger logger = new MasterTablesLogger();
//                logger.Log(log, originalValues, currentValues);

//                if (db.SaveChanges() == 1)
//                {
//                    Message = "Saved";
//                    return true;
//                }
//            }
//            Message = "Something went wrong. Please try again.";
//            return false;
//        }
//        public bool RestoreService(string ServiceCode, string UserEmail, out string Message)
//        {
//            Services service = db.Services.Where(d => d.ServiceCode == ServiceCode).FirstOrDefault();
//            DbPropertyValues currentValues;
//            DbPropertyValues originalValues;

//            if (service != null)
//            {
//                service.PurgeFlag = false;
//                service.DeletedAt = DateTime.Now;

//                log.Action = "Restore Record";

//                currentValues = db.ChangeTracker.Entries().Where(t => t.State == EntityState.Modified).First().CurrentValues;
//                originalValues = db.ChangeTracker.Entries().Where(t => t.State == EntityState.Modified).First().OriginalValues;
//                log.AuditableKey = service.ID;
//                log.ProcessedBy = accountsBL.GetUser(UserEmail).UserID;
//                log.TableName = "master_services";
//                MasterTablesLogger logger = new MasterTablesLogger();
//                logger.Log(log, originalValues, currentValues);

//                if (db.SaveChanges() == 1)
//                {
//                    Message = "Saved";
//                    return true;
//                }
//            }
//            Message = "Something went wrong. Please try again.";
//            return false;
//        }
//        private string GenerateServiceCode(int InventoryTypeCode, int CategoryID)
//        {
//            string ServiceCode = string.Empty;
//            var category = (CategoryID.ToString().Length == 1) ? "00" + CategoryID.ToString() : (CategoryID.ToString().Length == 2) ? "0" + CategoryID.ToString() : CategoryID.ToString();
//            var series = (db.Services.Where(d => d.ServiceTypeReference == InventoryTypeCode && d.ServiceCategoryReference == CategoryID).Count() + 1).ToString();
//            series = (series.Length == 1) ? "00" + series : (series.Length == 2) ? "0" + series : series;
//            var inventoryTypeCode = (InventoryTypeCode.ToString().Length == 1) ? "0" + InventoryTypeCode.ToString() : InventoryTypeCode.ToString();
//            ServiceCode = inventoryTypeCode + "-" + category + "-" + series + "00" ;
//            return ServiceCode;
//        }
//        protected override void Dispose(bool disposing)
//        {
//            if (disposing)
//            {
//                db.Dispose();
//                accountsBL.Dispose();
//            }
//            base.Dispose(disposing);
//        }
//    }
//}