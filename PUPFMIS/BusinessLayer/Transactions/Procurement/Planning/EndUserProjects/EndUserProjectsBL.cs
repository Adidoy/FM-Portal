using PUPFMIS.Models;
using PUPFMIS.Models.HRIS;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web.Mvc;

namespace PUPFMIS.BusinessLayer
{
    public class EndUserProjectsBL : Controller
    {
        private FMISDbContext FMISdb = new FMISDbContext();
        private HRISDbContext HRISdb = new HRISDbContext();
        private LogsMasterTables _log = new LogsMasterTables();

        public List<EndUserProject> GetActiveProjects()
        {
            return FMISdb.EndUserProjects.Where(d => d.PurgeFlag == false).ToList();
        }

        public List<MarketSurvey> GetMarketSurveyItems()
        {
            return FMISdb.MarketSurvey.Include(d => d.FKProjectReference).Include(d => d.FKItem).Include(d => d.FkSupplierReference1).Include(d => d.FkSupplierReference2).Include(d => d.FkSupplierReference3).ToList();
        }

        public List<EndUserProject> GetPurgedProject()
        {
            return FMISdb.EndUserProjects.Where(d => d.PurgeFlag == true).ToList();
        }

        public EndUserProject GetProjectDetails(int? ProjectID)
        {
            return FMISdb.EndUserProjects.Find(ProjectID);
        }

        public Offices GetOffice()
        {
            Offices _office = HRISdb.OfficeModel.Find(1);
            return _office;
        }

        public EndUserProject AddEndUserProjectRecord()
        {
            EndUserProject _project = new EndUserProject();
            _project.EndUser = GetOffice().ID;
            _project.ProjectStart = DateTime.Now;
            _project.FiscalYear = (DateTime.Now.Year + 1).ToString();
            return _project;
        }

        public bool AddEndUserProjectRecord(EndUserProject Project)
        {
            EndUserProject _project = FMISdb.EndUserProjects.Find(Project.ID);
            DbPropertyValues _currentValues;

            if (_project == null)
            {
                Project.Code = GenerateProjectCode(GetOffice().ID, Project.FiscalYear);
                Project.ProjectStatus = "New Project";
                Project.EndUser = GetOffice().ID;
                Project.CreatedAt = DateTime.Now;

                FMISdb.EndUserProjects.Add(Project);

                _currentValues = FMISdb.ChangeTracker.Entries().Where(d => d.State == EntityState.Added).First().CurrentValues;
                _log.Action = "Add Record";

                if (FMISdb.SaveChanges() == 1)
                {
                    _log.AuditableKey = Project.ID;
                    _log.ProcessedBy = null;
                    _log.TableName = "master_suppliers";
                    MasterTablesLogger _logger = new MasterTablesLogger();
                    _logger.Log(_log, null, _currentValues);
                    return true;
                }
                return false;
            }
            return false;
        }

        public bool UpdateProjectRecord(EndUserProject Project, bool DeleteFlag)
        {
            EndUserProject _project = FMISdb.EndUserProjects.Find(Project.ID);
            DbPropertyValues _currentValues;
            DbPropertyValues _originalValues;

            if (_project != null)
            {
                if (DeleteFlag == false)
                {
                    _project.ProjectName = Project.ProjectName;
                    _project.Purpose = Project.Purpose;
                    _project.ProjectStart = Project.ProjectStart;
                    _project.FiscalYear = Project.FiscalYear;
                    _project.UpdatedAt = DateTime.Now;

                    _log.Action = "Update Record";
                }
                else
                {
                    _project.PurgeFlag = true;
                    _project.DeletedAt = DateTime.Now;

                    _log.Action = "Void Record";
                }

                _currentValues = FMISdb.ChangeTracker.Entries().Where(t => t.State == EntityState.Modified).First().CurrentValues;
                _originalValues = FMISdb.ChangeTracker.Entries().Where(t => t.State == EntityState.Modified).First().OriginalValues;
                _log.AuditableKey = Project.ID;
                _log.ProcessedBy = null;
                _log.TableName = "master_suppliers";
                MasterTablesLogger _logger = new MasterTablesLogger();
                _logger.Log(_log, _originalValues, _currentValues);

                if (FMISdb.SaveChanges() == 1)
                {
                    return true;
                }
            }
            return false;
        }

        private string GenerateProjectCode(int OfficeID, string FiscalYear)
        {
            string _series;
            string _officeCode = HRISdb.OfficeModel.Find(OfficeID).OfficeCode;
            int _projectCount = FMISdb.EndUserProjects.Where(d => d.FiscalYear == FiscalYear && d.EndUser == OfficeID).Count() + 1;
            _series = (_projectCount.ToString().Length == 1) ? "00" + _projectCount.ToString() : (_projectCount.ToString().Length == 2) ? "0" + _projectCount.ToString() : _projectCount.ToString();
            return "EUPR-" + FiscalYear + "-" + _officeCode + "-" + _series;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                FMISdb.Dispose();
                HRISdb.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}