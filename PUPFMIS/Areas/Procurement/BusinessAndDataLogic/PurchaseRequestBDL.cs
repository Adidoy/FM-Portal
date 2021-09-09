using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using PUPFMIS.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace PUPFMIS.BusinessAndDataLogic
{
    public class ProcurementPurchaseRequestBL : Controller
    {
        private ProcurementPurchaseRequestDAL prDAL = new ProcurementPurchaseRequestDAL();
        private FMISDbContext db = new FMISDbContext();
        private HRISDataAccess hris = new HRISDataAccess();

        public MemoryStream GeneratePurchaseRequestMemorandum(string ContractCode, string LogoPath, string UserEmail)
        {
            Reports reports = new Reports();
            var procurementReference = db.AgencyDetails.Select(d => d.ProcurementPlanningOfficeReference).FirstOrDefault();
            var procurementOffice = hris.GetDepartmentDetails(procurementReference);
            var memoOffices = prDAL.GetPurchaseRequestMemoDetails(ContractCode, UserEmail);
            reports.ReportFilename = "Memorandum for Submission - " + ContractCode;
            reports.CreateDocument(8.50, 13.00, Orientation.Portrait, 1.00);
            reports.AddDoubleColumnHeader(LogoPath, false, 1.25);
            reports.AddColumnHeader(
                new HeaderLine { Content = "Republic of the Philippines", Bold = false, Italic = false, FontSize = 8, ParagraphAlignment = ParagraphAlignment.Left },
                new HeaderLine { Content = "", Bold = false, Italic = false, FontSize = 6, ParagraphAlignment = ParagraphAlignment.Left }
            );
            reports.AddColumnHeader(
                new HeaderLine { Content = "POLYTECHNIC UNIVERSITY OF THE PHILIPPINES", Bold = true, Italic = false, FontSize = 12, ParagraphAlignment = ParagraphAlignment.Left },
                new HeaderLine { Content = "", Bold = false, Italic = false, FontSize = 7, ParagraphAlignment = ParagraphAlignment.Left }
            );
            reports.AddColumnHeader(
                new HeaderLine { Content = procurementOffice.Sector, Bold = false, Italic = false, FontSize = 9.5, ParagraphAlignment = ParagraphAlignment.Left },
                new HeaderLine { Content = "", Bold = false, Italic = false, FontSize = 7, ParagraphAlignment = ParagraphAlignment.Left }
            );
            reports.AddColumnHeader(
                new HeaderLine { Content = procurementOffice.Department.ToUpper(), Bold = true, Italic = false, FontSize = 11, ParagraphAlignment = ParagraphAlignment.Left },
                new HeaderLine { Content = "", Bold = true, Italic = false, FontSize = 8, ParagraphAlignment = ParagraphAlignment.Left }
            );
            reports.AddColumnHeader(
                new HeaderLine { Content = procurementOffice.Section, Bold = false, Italic = false, FontSize = 9, ParagraphAlignment = ParagraphAlignment.Left },
                new HeaderLine { Content = "", Bold = true, Italic = false, FontSize = 8, ParagraphAlignment = ParagraphAlignment.Left }
            );

            for (var i = 0; i < memoOffices.Count; i++)
            {
                var contract = db.ProcurementProjects.ToList().Where(d => d.ContractCode == memoOffices[i].ContractCode).FirstOrDefault();

                reports.AddNewLine();

                var columns = new List<ContentColumn>();
                columns.Add(new ContentColumn(6.50));
                reports.AddTable(columns, false);

                var rows = new List<ContentCell>();
                rows.Add(new ContentCell(DateTime.Now.ToString("dd MMMM yyyy"), 0, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center));
                reports.AddRowContent(rows, 0.00);

                reports.AddNewLine();
                reports.AddNewLine();

                columns = new List<ContentColumn>();
                columns.Add(new ContentColumn(6.50));
                reports.AddTable(columns, false);

                rows = new List<ContentCell>();
                rows.Add(new ContentCell(memoOffices[i].DepartmentHead.ToUpper(), 0, 10, true, false, ParagraphAlignment.Left, VerticalAlignment.Center));
                reports.AddRowContent(rows, 0.00);

                rows = new List<ContentCell>();
                rows.Add(new ContentCell(memoOffices[i].Designation + (memoOffices[i].Unit == memoOffices[i].Department ? string.Empty : ", " + memoOffices[i].Unit), 0, 10, false, true, ParagraphAlignment.Left, VerticalAlignment.Center));
                reports.AddRowContent(rows, 0.00);

                rows = new List<ContentCell>();
                rows.Add(new ContentCell(memoOffices[i].Department, 0, 10, false, true, ParagraphAlignment.Left, VerticalAlignment.Center));
                reports.AddRowContent(rows, 0.00);

                reports.AddNewLine();
                reports.AddNewLine();

                columns = new List<ContentColumn>();
                columns.Add(new ContentColumn(6.50));
                reports.AddTable(columns, false);

                reports.AddFormattedRowContent(new ContentCell(new TextWithFormat[]
                {
                                               new TextWithFormat("Dear ", false, false, 10),
                                               new TextWithFormat(memoOffices[i].Designation + " " + memoOffices[i].DepartmentHeadLastName, true, true, 10),
                                               new TextWithFormat(":", false, true, 10),
                }, MigraDoc.DocumentObjectModel.ParagraphAlignment.Left), 0.00);

                reports.AddNewLine();
                reports.AddNewLine();

                columns = new List<ContentColumn>();
                columns.Add(new ContentColumn(6.50));
                reports.AddTable(columns, false);

                reports.AddFormattedRowContent(new ContentCell(new TextWithFormat[]
                {
                               new TextWithFormat("The ", false, false, 10),
                               new TextWithFormat(procurementOffice.Department, false, false, 10),
                               new TextWithFormat(" (", false, false, 10),
                               new TextWithFormat(procurementOffice.DepartmentCode, false, false, 10),
                               new TextWithFormat(")", false, false, 10),
                               new TextWithFormat(" has included the contract project ", false, false, 10),
                               new TextWithFormat(contract.ContractName + " (" + contract.ContractCode + ")", true, true, 10),
                               new TextWithFormat(" in the procurement pipeline and is now preparing the necessary documents to commence the procurement process.", false, false, 10),
                }, MigraDoc.DocumentObjectModel.ParagraphAlignment.Justify), 0.00);

                reports.AddNewLine();

                columns = new List<ContentColumn>();
                columns.Add(new ContentColumn(6.50));
                reports.AddTable(columns, false);

                reports.AddFormattedRowContent(new ContentCell(new TextWithFormat[]
                {
                    new TextWithFormat("In line with this, your Office is hereby requested to prepare the ", false, false, 10),
                    new TextWithFormat("PURCHASE REQUEST", true, true, 10),
                    new TextWithFormat(" for the following items:", false, false, 10),
                }, MigraDoc.DocumentObjectModel.ParagraphAlignment.Justify), 0.00);

                reports.AddNewLine();

                columns = new List<ContentColumn>();
                columns.Add(new ContentColumn(1.00));
                columns.Add(new ContentColumn(1.00));
                columns.Add(new ContentColumn(1.00));
                columns.Add(new ContentColumn(3.50));
                reports.AddTable(columns, true);

                rows = new List<ContentCell>();
                rows.Add(new ContentCell("#", 0, 10, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                rows.Add(new ContentCell("Quantity", 1, 10, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                rows.Add(new ContentCell("Unit", 2, 10, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                rows.Add(new ContentCell("Item and Specifications", 3, 10, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                reports.AddRowContent(rows, 0.25);

                var count = 1;
                foreach (var item in memoOffices[i].Items)
                {
                    rows = new List<ContentCell>();
                    rows.Add(new ContentCell("\n" + count.ToString(), 0, 8, false, false, ParagraphAlignment.Center, VerticalAlignment.Top));
                    rows.Add(new ContentCell("\n" + item.Quantity.ToString(), 1, 8, false, false, ParagraphAlignment.Center, VerticalAlignment.Top));
                    rows.Add(new ContentCell("\n" + item.UnitOfMeasure, 2, 8, false, false, ParagraphAlignment.Center, VerticalAlignment.Top));
                    rows.Add(new ContentCell(new TextWithFormat[]
                    {
                        new TextWithFormat(item.ItemFullName + "\n", true, false, 8.00),
                        new TextWithFormat(item.ItemSpecifications + "\n\n", false, true, 7.50),
                    }, 3, ParagraphAlignment.Left, VerticalAlignment.Top));
                    reports.AddRowContent(rows, 0.40);
                    count++;
                }

                reports.AddNewLine();

                columns = new List<ContentColumn>();
                columns.Add(new ContentColumn(6.50));
                reports.AddTable(columns, false);

                reports.AddFormattedRowContent(new ContentCell(new TextWithFormat[]
                {
                    new TextWithFormat("You may access the PUP Finance Management Portal (PUP FMPortal) for the generation of your purchase request. Submit a copy of duly signed Purchase Request not later than ", false, false, 10),
                    new TextWithFormat(contract.PRSubmissionClose.Value.ToString("dd MMMM yyyy"), true, false, 10, Underline.Single),
                    new TextWithFormat(" at the " + procurementOffice.Department + ".", false, false, 10),
                }, MigraDoc.DocumentObjectModel.ParagraphAlignment.Justify), 0.00);

                reports.AddNewLine();

                columns = new List<ContentColumn>();
                columns.Add(new ContentColumn(6.50));
                reports.AddTable(columns, false);

                reports.AddFormattedRowContent(new ContentCell(new TextWithFormat[]
                {
                    new TextWithFormat("Thank you very much.", false, false, 10),
                }, MigraDoc.DocumentObjectModel.ParagraphAlignment.Justify), 0.00);

                reports.AddNewLine();
                reports.AddNewLine();
                reports.AddNewLine();

                columns = new List<ContentColumn>();
                columns.Add(new ContentColumn(6.50));
                reports.AddTable(columns, false);

                rows = new List<ContentCell>();
                rows.Add(new ContentCell("Respectfully, ", 0, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center));
                reports.AddRowContent(rows, 0.00);

                reports.AddNewLine();
                reports.AddNewLine();

                columns = new List<ContentColumn>();
                columns.Add(new ContentColumn(6.50));
                reports.AddTable(columns, false);
                
                rows = new List<ContentCell>();
                rows.Add(new ContentCell(procurementOffice.SectionHead, 0, 10, true, false, ParagraphAlignment.Left, VerticalAlignment.Center));
                reports.AddRowContent(rows, 0.00);

                rows = new List<ContentCell>();
                rows.Add(new ContentCell(procurementOffice.SectionHeadDesignation, 0, 10, false, true, ParagraphAlignment.Left, VerticalAlignment.Center));
                reports.AddRowContent(rows, 0.00);

                reports.AddNewLine();
                reports.AddNewLine();
                reports.AddNewLine();

                columns = new List<ContentColumn>();
                columns.Add(new ContentColumn(6.50));
                reports.AddTable(columns, false);

                rows = new List<ContentCell>();
                rows.Add(new ContentCell("Noted: ", 0, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center));
                reports.AddRowContent(rows, 0.00);

                reports.AddNewLine();
                reports.AddNewLine();

                columns = new List<ContentColumn>();
                columns.Add(new ContentColumn(6.50));
                reports.AddTable(columns, false);

                rows = new List<ContentCell>();
                rows.Add(new ContentCell(procurementOffice.DepartmentHead, 0, 10, true, false, ParagraphAlignment.Left, VerticalAlignment.Center));
                reports.AddRowContent(rows, 0.00);

                rows = new List<ContentCell>();
                rows.Add(new ContentCell(procurementOffice.DepartmentHeadDesignation, 0, 10, false, true, ParagraphAlignment.Left, VerticalAlignment.Center));
                reports.AddRowContent(rows, 0.00);

                if (i != memoOffices.Count - 1)
                {
                    reports.InsertPageBreak();
                }
            }
            
            return reports.GenerateReport();
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                prDAL.Dispose();
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
        private ProcurementProjectsDAL contractDAL = new ProcurementProjectsDAL();

        public List<int> GetFiscalYears()
        {
            return db.PurchaseRequestHeader.Select(d => d.FiscalYear).Distinct().ToList();
        }
        public List<PurchaseRequestMemoOffices> GetPurchaseRequestMemoDetails(string ContractCode, string UserEmail)
        {
            var user = db.UserAccounts.Where(d => d.Email == UserEmail).FirstOrDefault();
            var ParentProjectReferenceontract = db.ProcurementProjects.Where(d => d.ContractCode == ContractCode).FirstOrDefault();
            var childContract = db.ProcurementProjects.Where(d => d.FKParentProjectReference.ContractCode == ContractCode).ToList();
            var endUsers = new List<string>();
            var ppmpDetails = new List<PPMPDetails>();
            var memoOffices = new List<PurchaseRequestMemoOffices>();
            if (childContract.Count == 0)
            {
                ppmpDetails = db.PPMPDetails.Where(d => d.FKProcurementProject.ContractCode == ContractCode).ToList();
                endUsers = ppmpDetails.ToList().Select(d => new {
                    EndUser = d.FKAPPDetailReference.EndUser == "Various Offices" ? d.FKPPMPHeaderReference.Department : d.FKAPPDetailReference.EndUser
                }).GroupBy(d => d.EndUser).Select(d => d.Key).ToList();

                foreach (var endUser in endUsers)
                {
                    var office = hris.GetDepartmentDetails(endUser);
                    memoOffices.Add(new PurchaseRequestMemoOffices
                    {
                        DepartmentHead = office.DepartmentHead,
                        DepartmentHeadLastName = office.DepartmentHeadLastName,
                        Designation = office.DepartmentHeadDesignation,
                        Department = office.Department,
                        Unit = office.Section,
                        ContractCode = ContractCode,
                        Items = ppmpDetails.Where(d => d.FKAPPDetailReference.EndUser == "Various Offices" ? d.FKPPMPHeaderReference.Department == office.DepartmentCode : d.FKItemArticleReference.FKItemTypeReference.ResponsibilityCenter == office.DepartmentCode)
                        .Select(d => new
                        {
                            ArticleReference = d.ArticleReference,
                            ItemSequence = d.ItemSequence,
                            ItemFullName = d.ItemFullName,
                            Quantity = d.TotalQty,
                            ItemSpecifications = d.ItemSpecifications,
                            UnitOfMeasure = d.FKUOMReference.Abbreviation
                        })
                        .GroupBy(d => new {
                            d.ArticleReference,
                            d.ItemSequence,
                            d.ItemFullName,
                            d.ItemSpecifications,
                            d.UnitOfMeasure,
                        })
                        .Select(d => new ProcurementProjectDetailsVM
                        {
                            ArticleReference = d.Key.ArticleReference,
                            ItemSequence = d.Key.ItemSequence,
                            ItemFullName = d.Key.ItemFullName,
                            Quantity = d.Sum(x => x.Quantity),
                            ItemSpecifications = d.Key.ItemSpecifications,
                            UnitOfMeasure = d.Key.UnitOfMeasure
                        }).ToList()
                    });
                }
            }
            else
            {
                var contractCodes = childContract.Select(x => x.ContractCode).ToList();
                foreach(var code in contractCodes)
                {
                    ppmpDetails = db.PPMPDetails.Where(d => d.FKProcurementProject.ContractCode == code).ToList();
                    endUsers = ppmpDetails.ToList().Select(d => new {
                        EndUser = d.FKAPPDetailReference.EndUser == "Various Offices" ? d.FKPPMPHeaderReference.Department : d.FKAPPDetailReference.EndUser
                    }).GroupBy(d => d.EndUser).Select(d => d.Key).ToList();

                    foreach (var endUser in endUsers)
                    {
                        var office = hris.GetDepartmentDetails(endUser);
                        memoOffices.Add(new PurchaseRequestMemoOffices
                        {
                            DepartmentHead = office.DepartmentHead,
                            Designation = office.DepartmentHeadDesignation,
                            Department = office.Department,
                            DepartmentHeadLastName = office.DepartmentHeadLastName,
                            Unit = office.Section,
                            ContractCode = code,
                            Items = ppmpDetails.Where(d => d.FKAPPDetailReference.EndUser == "Various Offices" ? d.FKPPMPHeaderReference.Department == office.DepartmentCode : d.FKItemArticleReference.FKItemTypeReference.ResponsibilityCenter == office.DepartmentCode)
                            .Select(d => new
                            {
                                ArticleReference = d.ArticleReference,
                                ItemSequence = d.ItemSequence,
                                ItemFullName = d.ItemFullName,
                                Quantity = d.TotalQty,
                                ItemSpecifications = d.ItemSpecifications,
                                UnitOfMeasure = d.FKUOMReference.Abbreviation
                            })
                            .GroupBy(d => new {
                                d.ArticleReference,
                                d.ItemSequence,
                                d.ItemFullName,
                                d.ItemSpecifications,
                                d.UnitOfMeasure,
                            })
                            .Select(d => new ProcurementProjectDetailsVM
                            {
                                ArticleReference = d.Key.ArticleReference,
                                ItemSequence = d.Key.ItemSequence,
                                ItemFullName = d.Key.ItemFullName,
                                Quantity = d.Sum(x => x.Quantity),
                                ItemSpecifications = d.Key.ItemSpecifications,
                                UnitOfMeasure = d.Key.UnitOfMeasure
                            }).ToList()
                        });
                    }
                }
            }

            return memoOffices;
        }
        public List<PurchaseRequestHeaderVM> GetPendingSubmissions()
        {
            return db.PurchaseRequestHeader.ToList().Where(d => d.PRStatus == PurchaseRequestStatus.PurchaseRequestSubmitted)
                     .Select(d => new PurchaseRequestHeaderVM
                     {
                         PRNumber = d.PRNumber,
                         PRStatus = d.PRStatus,
                         ContractCode = d.FKProcurementProjectReference.ContractCode,
                         ContractName = d.FKProcurementProjectReference.ContractName,
                         Department = hris.GetDepartmentDetails(d.Department).Department,
                         FundCluster = abis.GetFundSources(d.FundCluster).FUND_DESC.Replace("\r\n",""),
                         CreatedBy = hris.GetEmployeeByCode(d.CreatedBy).EmployeeName,
                         CreatedAt = d.CreatedAt,
                         SubmittedBy = d.SubmittedAt == null ? null : hris.GetEmployeeByCode(d.SubmittedBy).EmployeeName,
                         SubmittedAt = d.SubmittedAt == null ? null : d.SubmittedAt
                     }).ToList();
        }
        public List<PurchaseRequestHeaderVM> GetPurchaseRequests(int FiscalYear)
        {
            return db.PurchaseRequestHeader.ToList().Where(d => d.FiscalYear == FiscalYear && d.PRStatus == PurchaseRequestStatus.PurchaseRequestReceived)
                     .Select(d => new PurchaseRequestHeaderVM
                     {
                         PRNumber = d.PRNumber,
                         PRStatus = d.PRStatus,
                         ContractCode = d.FKProcurementProjectReference.ContractCode,
                         ContractName = d.FKProcurementProjectReference.ContractName,
                         Department = hris.GetDepartmentDetails(d.Department).Department,
                         FundCluster = abis.GetFundSources(d.FundCluster).FUND_DESC.Replace("\r\n", ""),
                         CreatedBy = hris.GetEmployeeByCode(d.CreatedBy).EmployeeName,
                         CreatedAt = d.CreatedAt,
                         SubmittedBy = d.SubmittedAt == null ? null : hris.GetEmployeeByCode(d.SubmittedBy).EmployeeName,
                         SubmittedAt = d.SubmittedAt == null ? null : d.SubmittedAt,
                         ReceivedBy = d.ReceivedAt == null ? null : hris.GetEmployeeByCode(d.ReceivedBy).EmployeeName,
                         ReceivedAt = d.ReceivedAt == null ? null : d.ReceivedAt,
                     }).ToList();
        }
        public bool ReceivePurchaseRequest(string PRNumber, string UserEmail)
        {
            var user = db.UserAccounts.Where(d => d.Email == UserEmail).FirstOrDefault();
            var purchaseRequest = db.PurchaseRequestHeader.Where(d => d.PRNumber == PRNumber).FirstOrDefault();
            purchaseRequest.ReceivedBy = user.EmpCode;
            purchaseRequest.ReceivedAt = DateTime.Now;
            purchaseRequest.PRStatus = PurchaseRequestStatus.PurchaseRequestReceived;
            if (db.SaveChanges() == 0)
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
                contractDAL.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}