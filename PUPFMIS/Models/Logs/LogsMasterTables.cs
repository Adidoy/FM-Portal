using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure;

namespace PUPFMIS.Models
{
    [Table("logs_master_tables")]
    public class LogsMasterTables
    {
        [Key]
        public int ID { get; set; }

        [Display(Name = "Action")]
        [MaxLength(15)]
        public string Action { get; set; }

        [Display(Name = "Table Name")]
        [MaxLength(50)]
        public string TableName { get; set; }

        [Display(Name = "Column Name")]
        [MaxLength(50)]
        public string ColumnName { get; set; }

        [Display(Name = "Auditable Key")]
        public int AuditableKey { get; set; }

        [Display(Name = "Old Value")]
        public string OldValue { get; set; }

        [Display(Name = "New Value")]
        public string NewValue { get; set; }

        [Display(Name = "Processed by")]
        public int? ProcessedBy { get; set; }

        [Display(Name = "Date Updated")]
        public DateTime UpdatedAt { get; set; }
    }

    public class MasterTablesLogger
    {
        private FMISDbContext db = new FMISDbContext();
        public void Log(LogsMasterTables LogValues, DbPropertyValues OriginalValues, DbPropertyValues CurrentValues)
        {
            LogsMasterTables _log = new LogsMasterTables();
            _log.Action = LogValues.Action;
            _log.TableName = LogValues.TableName;
            _log.AuditableKey = LogValues.AuditableKey;
            _log.ProcessedBy = LogValues.ProcessedBy;
            _log.UpdatedAt = DateTime.Now;
            foreach(string _propertyName in CurrentValues.PropertyNames)
            {
                if (_log.Action == "Add Record")
                {
                    if (_propertyName != "ID" && _propertyName != "CreatedAt" && _propertyName != "UpdatedAt" && _propertyName != "DeletedAt")
                    {
                        _log.ColumnName = _propertyName;
                        _log.NewValue = (CurrentValues[_propertyName] == null) ? null : CurrentValues[_propertyName].ToString();
                        _log.OldValue = null;
                        db.LogsMasterTables.Add(_log);
                        db.SaveChanges();
                    }
                }
                else
                {
                    if(!Equals(CurrentValues[_propertyName],OriginalValues[_propertyName]))
                    {
                        if (_propertyName != "ID" && _propertyName != "CreatedAt" && _propertyName != "UpdatedAt" && _propertyName != "DeletedAt")
                        {
                            _log.ColumnName = _propertyName;
                            _log.NewValue = (CurrentValues[_propertyName].ToString() == null) ? null : CurrentValues[_propertyName].ToString();
                            _log.OldValue = (OriginalValues[_propertyName].ToString() == null) ? null : OriginalValues[_propertyName].ToString();
                            db.LogsMasterTables.Add(_log);
                            db.SaveChanges();
                        }
                    }
                }
            }
        }
    }
}