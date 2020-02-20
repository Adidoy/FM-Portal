//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;
//using PUPFMIS.Models;
//using System.Data.Entity.Infrastructure;

//namespace PUPFMIS.Controllers
//{
//    public class MasterTablesAuditController : Controller
//    {
//        private FMISDbContext db = new FMISDbContext();

//        public AuditActions Action { get; set; }
//        public int AuditableKey { get; set; }
//        public string TableName { get; set; }
//        public DbPropertyValues OriginalValues { get; set; }
//        public DbPropertyValues CurrentValues { get; set; }
//        public string UserID { get; set; }

//        public void Log()
//        {
//            foreach (string _propName in CurrentValues.PropertyNames)
//            {
//                if ((OriginalValues == null) || (!Equals(OriginalValues[_propName], CurrentValues[_propName]) && OriginalValues != null))
//                    if (_propName != "ID" && _propName != "CreatedAt" && _propName != "UpdatedAt" && _propName != "DeletedAt")
//                    {
//                        MasterTablesAudit _masterTableAudit = new MasterTablesAudit();
//                        _masterTableAudit.Action = Action;
//                        _masterTableAudit.TableName = TableName;
//                        _masterTableAudit.ColumnName = _propName;
//                        _masterTableAudit.AuditableKey = AuditableKey;
//                        _masterTableAudit.NewValue = (CurrentValues[_propName] == null) ? null : CurrentValues[_propName].ToString();
//                        _masterTableAudit.OldValue = (OriginalValues == null) ? null : (OriginalValues[_propName].ToString() == null) ? null : OriginalValues[_propName].ToString();
//                        _masterTableAudit.DateUpdated = DateTime.Now;
//                        _masterTableAudit.UserId = UserID;
//                        db.MasterTablesAudit.Add(_masterTableAudit);
//                        db.SaveChanges();
//                    }
//            }
//        }
//    }
//}