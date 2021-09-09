using PUPFMIS.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web.Mvc;

namespace PUPFMIS.BusinessAndDataLogic
{
    public class SuppliersBL : Controller
    {
        private FMISDbContext db = new FMISDbContext();
        LogsMasterTables _log = new LogsMasterTables();

        public List<SupplierCategoriesVM> GetCategories()
        {
            return db.ItemCategories.Where(d => d.PurgeFlag == false)
                     .Select(d => new SupplierCategoriesVM {
                         IsSelected = false,
                         ID = d.ID,
                         Category = d.ItemCategoryName.ToUpper()
                     }).ToList();
        }
        public List<SupplierItemTypesVM> GetItemTypes()
        {
            return db.ItemTypes.Where(d => d.PurgeFlag == false)
                     .Select(d => new SupplierItemTypesVM {
                         IsSelected = false,
                         ID = d.ID,
                         ItemType = d.ItemType.ToUpper()
                     }).ToList();
        }
        public List<Supplier> GetActiveSuppliers()
        {
            return db.Suppliers.Where(d => d.PurgeFlag == false).OrderBy(d => d.SupplierName).ToList();
        }
        public List<Supplier> GetPurgedSuppliers()
        {
            return db.Suppliers.Where(d => d.PurgeFlag == true).OrderBy(d => d.SupplierName).ToList();
        }
        public SupplierVM GetSupplierDetails(int? SupplierID)
        {
            var supplierDetails = db.Suppliers.Where(d => d.ID == SupplierID).Select(d => new SupplierVM
            {
                ID = d.ID,
                SupplierName = d.SupplierName,
                ContactPerson = d.ContactPerson,
                ContactPersonDesignation = d.ContactPersonDesignation,
                AuthorizedAgent = d.AuthorizedAgent,
                AuthorizedDesignation = d.AuthorizedDesignation,
                ContactNumber = d.ContactNumber,
                AlternateContactNumber = d.AlternateContactNumber,
                Address = d.Address,
                City = d.City,
                State = d.State,
                PostalCode = d.PostalCode,
                TaxIdNumber = d.TaxIdNumber,
                EmailAddress = d.EmailAddress,
                Website = d.Website,
                PurgeFlag = d.PurgeFlag,
                CreatedAt = d.CreatedAt,
                UpdatedAt = d.UpdatedAt,
                DeletedAt = d.DeletedAt
            }).FirstOrDefault();

            supplierDetails.CategoryList = (from categoryList in db.ItemCategories.Where(d => d.PurgeFlag == false).ToList()
                                            join supplierCategories in db.SupplierCategories.Where(d => d.SupplierReference == supplierDetails.ID).ToList() on categoryList.ID equals supplierCategories.CategoryReference
                                            into supplierCategoryList
                                            from list in supplierCategoryList.DefaultIfEmpty()
                                            select new SupplierCategoriesVM
                                            {
                                                IsSelected = list == null ? false : true,
                                                ID = categoryList.ID,
                                                Category = categoryList.ItemCategoryName
                                            }).ToList();
                
            supplierDetails.ItemTypesList = (from itemTypesList in db.ItemTypes.Where(d => d.PurgeFlag == false).ToList()
                                             join supplierItemTypes in db.SupplierItemTypes.Where(d => d.SupplierReference == supplierDetails.ID).ToList() on itemTypesList.ID equals supplierItemTypes.ItemTypeReference
                                             into supplierItemTypesList
                                             from list in supplierItemTypesList.DefaultIfEmpty()
                                             select new SupplierItemTypesVM
                                             {
                                                 IsSelected = list == null ? false : true,
                                                 ID = itemTypesList.ID,
                                                 ItemType = itemTypesList.ItemType
                                             }).ToList();
            return supplierDetails;
        }
        public bool AddSupplierRecord(SupplierVM Supplier, string UserEmail)
        {
            var user = db.UserAccounts.Where(d => d.Email == UserEmail).FirstOrDefault();
            DbPropertyValues _currentValues;
            var supplierRecord = new Supplier {
                SupplierName = Supplier.SupplierName,
                ContactPerson = Supplier.ContactPerson,
                ContactPersonDesignation = Supplier.ContactPersonDesignation,
                ContactNumber = Supplier.ContactNumber,
                AuthorizedAgent = Supplier.AuthorizedAgent,
                AuthorizedDesignation = Supplier.AuthorizedDesignation,
                AlternateContactNumber = Supplier.AlternateContactNumber,
                Address = Supplier.Address,
                City = Supplier.City,
                State = Supplier.State,
                PostalCode = Supplier.PostalCode,
                TaxIdNumber = Supplier.TaxIdNumber,
                EmailAddress = Supplier.EmailAddress,
                Website = Supplier.Website,
                PurgeFlag = false,
                CreatedAt = DateTime.Now
            };

            db.Suppliers.Add(supplierRecord);
            _currentValues = db.ChangeTracker.Entries().Where(d => d.State == EntityState.Added).First().CurrentValues;
            _log.Action = "Add Record";

            if (db.SaveChanges() == 0)
            {
                return false;
            }

            _log.AuditableKey = supplierRecord.ID;
            _log.ProcessedBy = user.ID;
            _log.TableName = "PROC_MSTR_Suppliers";
            MasterTablesLogger _logger = new MasterTablesLogger();
            _logger.Log(_log, null, _currentValues);

            var supplierCategories = new List<SupplierCategories>();
            var supplierItemTypes = new List<SupplierItemTypes>();

            foreach (var category in Supplier.CategoryList)
            {
                if(category.IsSelected == true)
                {
                    supplierCategories.Add(new SupplierCategories
                    {
                        SupplierReference = supplierRecord.ID,
                        CategoryReference = category.ID
                    });
                }

            }

            foreach(var itemType in Supplier.ItemTypesList)
            {
                if(itemType.IsSelected == true)
                {
                    supplierItemTypes.Add(new SupplierItemTypes
                    {
                        SupplierReference = supplierRecord.ID,
                        ItemTypeReference = itemType.ID
                    });
                }
            }

            db.SupplierCategories.AddRange(supplierCategories);
            db.SupplierItemTypes.AddRange(supplierItemTypes);

            if(db.SaveChanges() == 0)
            {
                return false;
            }

            return true;
        }
        public bool UpdateSupplierRecord(SupplierVM Supplier, string UserEmail)
        {
            var user = db.UserAccounts.Where(d => d.Email == UserEmail).FirstOrDefault();
            Supplier supplier = db.Suppliers.Find(Supplier.ID);
            DbPropertyValues _currentValues;
            DbPropertyValues _originalValues;

            supplier.SupplierName = Supplier.SupplierName;
            supplier.ContactPerson = Supplier.ContactPerson;
            supplier.ContactPersonDesignation = Supplier.ContactPersonDesignation;
            supplier.AuthorizedAgent = Supplier.AuthorizedAgent;
            supplier.AuthorizedDesignation = Supplier.AuthorizedDesignation;
            supplier.Address = Supplier.Address;
            supplier.City = Supplier.City;
            supplier.State = Supplier.State;
            supplier.PostalCode = Supplier.PostalCode;
            supplier.ContactNumber = Supplier.ContactNumber;
            supplier.AlternateContactNumber = Supplier.AlternateContactNumber;
            supplier.EmailAddress = Supplier.EmailAddress;
            supplier.Website = Supplier.Website;
            supplier.UpdatedAt = DateTime.Now;

            _log.Action = "Update Record";

            _currentValues = db.ChangeTracker.Entries().Where(t => t.State == EntityState.Modified).First().CurrentValues;
            _originalValues = db.ChangeTracker.Entries().Where(t => t.State == EntityState.Modified).First().OriginalValues;
            _log.AuditableKey = supplier.ID;
            _log.ProcessedBy = user.ID;
            _log.TableName = "PROC_MSTR_Suppliers";
            MasterTablesLogger _logger = new MasterTablesLogger();
            _logger.Log(_log, _originalValues, _currentValues);

            if (db.SaveChanges() == 0)
            {
                return false;
            }

            var categories = db.SupplierCategories.Where(d => d.SupplierReference == supplier.ID).ToList();
            var itemTypes = db.SupplierItemTypes.Where(d => d.SupplierReference == supplier.ID).ToList();

            if(categories.Count != 0)
            {
                db.SupplierCategories.RemoveRange(categories);
                if (db.SaveChanges() == 0)
                {
                    return false;
                }
            }

            if (itemTypes.Count != 0)
            {
                db.SupplierItemTypes.RemoveRange(itemTypes);
                if (db.SaveChanges() == 0)
                {
                    db.SupplierItemTypes.RemoveRange(itemTypes);
                    return false;
                }
            }


            categories = new List<SupplierCategories>();
            itemTypes = new List<SupplierItemTypes>();

            foreach (var category in Supplier.CategoryList)
            {
                if (category.IsSelected == true)
                {
                    categories.Add(new SupplierCategories
                    {
                        SupplierReference = supplier.ID,
                        CategoryReference = category.ID
                    });
                }

            }

            foreach (var itemType in Supplier.ItemTypesList)
            {
                if (itemType.IsSelected == true)
                {
                    itemTypes.Add(new SupplierItemTypes
                    {
                        SupplierReference = supplier.ID,
                        ItemTypeReference = itemType.ID
                    });
                }
            }

            db.SupplierCategories.AddRange(categories);
            db.SupplierItemTypes.AddRange(itemTypes);

            if (db.SaveChanges() == 0)
            {
                return false;
            }

            return true;
        }
        public bool DeleteSupplierRecord(int SupplierID, string UserEmail)
        {
            var user = db.UserAccounts.Where(d => d.Email == UserEmail).FirstOrDefault();
            var supplier = db.Suppliers.Find(SupplierID);
            DbPropertyValues _currentValues;
            DbPropertyValues _originalValues;

            supplier.PurgeFlag = true;
            supplier.DeletedAt = DateTime.Now;

            _log.Action = "Purge Record";

            _currentValues = db.ChangeTracker.Entries().Where(t => t.State == EntityState.Modified).First().CurrentValues;
            _originalValues = db.ChangeTracker.Entries().Where(t => t.State == EntityState.Modified).First().OriginalValues;
            _log.AuditableKey = supplier.ID;
            _log.ProcessedBy = user.ID;
            _log.TableName = "PROC_MSTR_Suppliers";
            MasterTablesLogger _logger = new MasterTablesLogger();
            _logger.Log(_log, _originalValues, _currentValues);

            if (db.SaveChanges() == 0)
            {
                return false;
            }

            return true;
        }
        public bool RestoreSupplierRecord(int ID, string UserEmail)
        {
            var user = db.UserAccounts.Where(d => d.Email == UserEmail).FirstOrDefault();
            var supplier = db.Suppliers.Find(ID);
            DbPropertyValues _currentValues;
            DbPropertyValues _originalValues;

            if (supplier != null)
            {
                supplier.PurgeFlag = false;
                supplier.UpdatedAt = DateTime.Now;
                supplier.DeletedAt = null;
                _log.Action = "Restore Record";
                _currentValues = db.ChangeTracker.Entries().Where(t => t.State == EntityState.Modified).First().CurrentValues;
                _originalValues = db.ChangeTracker.Entries().Where(t => t.State == EntityState.Modified).First().OriginalValues;
                _log.AuditableKey = supplier.ID;
                _log.ProcessedBy = user.ID;
                _log.TableName = "PROC_MSTR_Suppliers";
                MasterTablesLogger _logger = new MasterTablesLogger();
                _logger.Log(_log, _originalValues, _currentValues);

                if (db.SaveChanges() == 0)
                {
                    return false;
                }
            }
            return true;
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