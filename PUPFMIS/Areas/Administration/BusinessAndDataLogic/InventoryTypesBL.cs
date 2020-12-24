using FluentValidation.Results;
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
    public class InventoryTypeDataAccess : Controller
    {
        FMISDbContext db = new FMISDbContext();
        ABISDataAccess abis = new ABISDataAccess();
        HRISDataAccess hris = new HRISDataAccess();

        public List<InventoryType> GetInventoryTypes()
        {
            return db.InventoryTypes.OrderBy(d => d.InventoryTypeName).ToList();
        }
        public List<InventoryType> GetInventoryTypes(bool IsTangible)
        {
            return db.InventoryTypes.Where(d => d.IsTangible == IsTangible).OrderBy(d => d.InventoryTypeName).ToList();
        }
        public InventoryType GetInventoryTypes(string InventoryTypeCode)
        {
            return db.InventoryTypes.Where(d => d.InventoryCode == InventoryTypeCode).FirstOrDefault();
        }
        public InventoryType GetInventoryTypes(int ID)
        {
            return db.InventoryTypes.Find(ID);
        }
        public bool UpdateResponsibilityCenter(InventoryType inventoryTypeModel)
        {
            var inventoryType = db.InventoryTypes.Where(d => d.InventoryCode == inventoryTypeModel.InventoryCode).FirstOrDefault();
            inventoryType.ResponsibilityCenter = inventoryTypeModel.ResponsibilityCenter == "None" ? null : inventoryTypeModel.ResponsibilityCenter;
            inventoryType.UpdatedAt = DateTime.Now;
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
            }
            base.Dispose(disposing);
        }
    }
}