using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using PUPFMIS.Models;
using PUPFMIS.Models.HRIS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace PUPFMIS.BusinessLayer
{
    public class PPMPCSEBusinessLayer : Controller
    {
        private FMISDbContext db = new FMISDbContext();
        private HRISDbContext HRISdb = new HRISDbContext();

        public List<PPMPDeadlines> GetFiscalYears()
        {
            return db.PPMPDeadlines.Where(d => d.PurgeFlag == false).OrderBy(d => d.FiscalYear).ToList();
        }

        public List<Months> GetMonths()
        {
            var months = new List<Months>()
                {
                    new Months { MonthValue = 1, MonthName = "January" },
                    new Months { MonthValue = 2, MonthName = "February" },
                    new Months { MonthValue = 3, MonthName = "March" },
                    new Months { MonthValue = 4, MonthName = "April" },
                    new Months { MonthValue = 5, MonthName = "May" },
                    new Months { MonthValue = 6, MonthName = "June" },
                    new Months { MonthValue = 7, MonthName = "July" },
                    new Months { MonthValue = 8, MonthName = "August" },
                    new Months { MonthValue = 9, MonthName = "September" },
                    new Months { MonthValue = 10, MonthName = "October" },
                    new Months { MonthValue = 11, MonthName = "November" },
                    new Months { MonthValue = 12, MonthName = "December" }
                };
            return months;
        }

        public bool CreatePPMPCSE(PPMPCSE ppmpCSE, string UserEmail)
        {
            PPMPHeader header = new PPMPHeader();
            PPMPCSEDetails cseDetails = new PPMPCSEDetails();

            header.FiscalYear = ppmpCSE.BasketHeader.FiscalYear;
            header.PPMPType = 1;
            header.PreparedBy = db.UserAccounts.Where(d => d.Email == UserEmail).FirstOrDefault().ID;
            header.OfficeReference = db.UserAccounts.Where(d => d.Email == UserEmail).FirstOrDefault().FKUserInformationReference.Office;
            header.CreatedAt = DateTime.Now;
            header.ReferenceNo = GenerateReferenceNo(ppmpCSE.BasketHeader.FiscalYear, header.OfficeReference);
            header.Status = "New";

            db.PPMPHeader.Add(header);

            if (db.SaveChanges() == 0)
            {
                return false;
            }

            List<PPMPCSEDetails> ppmpLineItems = new List<PPMPCSEDetails>();
            foreach (var item in ppmpCSE.BasketItems)
            {
                PPMPCSEDetails ppmpLineItem = new PPMPCSEDetails();
                ppmpLineItem.PPMPID = header.ID;
                ppmpLineItem.Item = item.ItemID;
                ppmpLineItem.Qtr1 = (String.IsNullOrEmpty(item.Qtr1Qty.ToString())) ? 0 : (int)item.Qtr1Qty;
                ppmpLineItem.Qtr2 = (String.IsNullOrEmpty(item.Qtr2Qty.ToString())) ? 0 : (int)item.Qtr2Qty;
                ppmpLineItem.Qtr3 = (String.IsNullOrEmpty(item.Qtr3Qty.ToString())) ? 0 : (int)item.Qtr3Qty;
                ppmpLineItem.Qtr4 = (String.IsNullOrEmpty(item.Qtr4Qty.ToString())) ? 0 : (int)item.Qtr4Qty;
                ppmpLineItem.TotalQty = (int)item.TotalQty;
                ppmpLineItem.Remarks = item.Remarks;
                ppmpLineItems.Add(ppmpLineItem);
            }

            db.PPMPCSEDetails.AddRange(ppmpLineItems);

            if (db.SaveChanges() != ppmpLineItems.Count())
            {
                return false;
            }

            string officeName = HRISdb.OfficeModel.Find(ppmpCSE.BasketHeader.OfficeReference).OfficeName;
            PPMPApprovalWorkflow approvalWF = new PPMPApprovalWorkflow();
            approvalWF.PPMPId = header.ID;
            approvalWF.Status = "New";
            approvalWF.Remarks = "New PPMP by " + officeName + " is created.";
            approvalWF.UpdatedAt = DateTime.Now;
            approvalWF.ActionMadeBy = (int)header.PreparedBy;
            approvalWF.ActionMadeByOffice = header.OfficeReference;
            db.PPMPApprovalWorkflow.Add(approvalWF);

            if (db.SaveChanges() == 1)
            {
                return true;
            }

            return false;
        }

        public string GenerateReferenceNo(string FiscalYear, int OfficeReference)
        {
            string referenceNo = string.Empty;
            string officeCode = HRISdb.OfficeModel.Find(OfficeReference).OfficeCode;
            string seqNo = (db.PPMPHeader.Where(d => d.FiscalYear == FiscalYear).Count() + 1).ToString();
            seqNo = seqNo.ToString().Length == 3 ? seqNo : seqNo.ToString().Length == 2 ? "0" + seqNo.ToString() : "00" + seqNo.ToString();
            referenceNo = "PPMP-CUSE-" + officeCode + "-" + seqNo + "-" + FiscalYear;
            return referenceNo;
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