using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using PUPFMIS.Models;
using PUPFMIS.Models.AIS;
using PUPFMIS.Models.HRIS;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace PUPFMIS.BusinessAndDataLogic
{
    public class ContractSingleBL : Controller
    {
        private ContractSingleDAL contractDAL = new ContractSingleDAL();
        private FMISDbContext db = new FMISDbContext();
        private HRISDataAccess hris = new HRISDataAccess();

        public MemoryStream GeneratePreProcurementMemo(string ContractCode, string LogoPath, string UserEmail)
        {
            var agencyDetails = db.AgencyDetails.FirstOrDefault();
            var procurementOffice = hris.GetDepartmentDetails(agencyDetails.ProcurementOfficeReference);
            var bidsAndAwards = hris.GetDepartmentDetails(agencyDetails.BACOfficeReference);
            var preProcurementOffices = contractDAL.GetPreProcMemoOffices(ContractCode, UserEmail);
            Reports reports = new Reports();
            reports.ReportFilename = "Pre-Procurement Memorandum - " + DateTime.Now.ToString("ddMMyyyy");
            reports.CreateDocument(8.50, 11.00, Orientation.Portrait, 1.00);
            reports.AddDoubleColumnHeader(LogoPath, false, 1.25);
            reports.AddColumnHeader(
                new HeaderLine { Content = "Republic of the Philippines", Bold = false, Italic = false, FontSize = 8, ParagraphAlignment = ParagraphAlignment.Left },
                new HeaderLine { Content = "", Bold = false, Italic = false, FontSize = 6, ParagraphAlignment = ParagraphAlignment.Left }
            );
            reports.AddColumnHeader(
                new HeaderLine { Content = agencyDetails.AgencyName.ToUpper(), Bold = true, Italic = false, FontSize = 12, ParagraphAlignment = ParagraphAlignment.Left },
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

            for (var i = 0; i < preProcurementOffices.Count; i++)
            {
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
                rows.Add(new ContentCell(preProcurementOffices[i].DepartmentHead.ToUpper(), 0, 10, true, false, ParagraphAlignment.Left, VerticalAlignment.Center));
                reports.AddRowContent(rows, 0.00);

                rows = new List<ContentCell>();
                rows.Add(new ContentCell(preProcurementOffices[i].Designation + (preProcurementOffices[i].Unit == preProcurementOffices[i].Department ? string.Empty : ", " + preProcurementOffices[i].Unit), 0, 10, false, true, ParagraphAlignment.Left, VerticalAlignment.Center));
                reports.AddRowContent(rows, 0.00);

                rows = new List<ContentCell>();
                rows.Add(new ContentCell(preProcurementOffices[i].Department, 0, 10, false, true, ParagraphAlignment.Left, VerticalAlignment.Center));
                reports.AddRowContent(rows, 0.00);

                reports.AddNewLine();
                reports.AddNewLine();

                columns = new List<ContentColumn>();
                columns.Add(new ContentColumn(6.50));
                reports.AddTable(columns, false);

                TextInfo textInfo = new CultureInfo("en-ph", false).TextInfo;

                reports.AddFormattedRowContent(new ContentCell(new TextWithFormat[]
                {
                    new TextWithFormat("Dear ", false, false, 10),
                    new TextWithFormat(preProcurementOffices[i].Designation + " " + textInfo.ToTitleCase(preProcurementOffices[i].DepartmentHeadLastName), true, true, 10),
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
                    new TextWithFormat(procurementOffice.Department + " - " + bidsAndAwards.Section + " (" + bidsAndAwards.SectionCode + ") ", false, false, 10),
                    new TextWithFormat(" will be holding a Pre-Procurement Conference for the project ", false, false, 10),
                    new TextWithFormat(preProcurementOffices[i].ContractName + " (" + preProcurementOffices[i].ContractCode + ")", true, true, 10),
                    new TextWithFormat(" on ", false, false, 10),
                    new TextWithFormat(preProcurementOffices[i].PreProcurementConferenceDate.Value.ToString("dd MMMM yyyy hh:mm tt"), true, true, 10, Underline.Single),
                    new TextWithFormat(" at ", false, false, 10),
                    new TextWithFormat(preProcurementOffices[i].PreProcurementConferenceLocation, true, true, 10, Underline.Single),
                    new TextWithFormat(".", false, false, 10),
                }, MigraDoc.DocumentObjectModel.ParagraphAlignment.Justify), 0.00);

                reports.AddNewLine();

                columns = new List<ContentColumn>();
                columns.Add(new ContentColumn(6.50));
                reports.AddTable(columns, false);

                reports.AddFormattedRowContent(new ContentCell(new TextWithFormat[]
                {
                    new TextWithFormat("This conference shall discuss all aspects of the procurement project, which includes the technical specifications, the Approved Budget for the Contract (ABC), the applicability and appropriateness of the recommended method of procurement and the related milestones, the bidding documents, and availability of the pertinent budget release for the project. ", false, false, 10),
                }, MigraDoc.DocumentObjectModel.ParagraphAlignment.Justify), 0.00);

                reports.AddNewLine();

                columns = new List<ContentColumn>();
                columns.Add(new ContentColumn(6.50));
                reports.AddTable(columns, false);

                reports.AddFormattedRowContent(new ContentCell(new TextWithFormat[]
                {
                    new TextWithFormat("Your attendance to the said Pre-Procurement Conference is hereby requested.", false, false, 10),
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
                rows.Add(new ContentCell(preProcurementOffices[i].ProjectCoordinator, 0, 10, true, false, ParagraphAlignment.Left, VerticalAlignment.Center));
                reports.AddRowContent(rows, 0.00);

                rows = new List<ContentCell>();
                rows.Add(new ContentCell("Project Coordinator", 0, 10, false, true, ParagraphAlignment.Left, VerticalAlignment.Center));
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

                if (i != preProcurementOffices.Count - 1)
                {
                    reports.InsertPageBreak();
                }
            }

            return reports.GenerateReport();
        }
        public MemoryStream GenerateInvitationToBid(string ContractCode, string LogoPath, string UserEmail)
        {
            var agencyDetails = db.AgencyDetails.FirstOrDefault();
            var procurementOffice = hris.GetDepartmentDetails(agencyDetails.ProcurementOfficeReference);
            var bidsAndAwards = hris.GetDepartmentDetails(agencyDetails.BACOfficeReference);
            var bacsecHead = db.BACSecretariat.Where(d => d.Membership == BACMembership.BACSECHead && d.PurgeFlag == false).FirstOrDefault();
            var bacChair = db.BACSecretariat.Where(d => d.Membership == BACMembership.BACChairman && d.PurgeFlag == false).FirstOrDefault();
            var invitationToBid = contractDAL.InvitationToBidSetup(ContractCode);
            Reports reports = new Reports();
            reports.ReportFilename = "Invitation To Bid - " + ContractCode;
            reports.CreateDocument(8.50, 13.00, Orientation.Portrait, 1.00);
            reports.AddPageNumbersFooter();
            reports.AddDoubleColumnHeader(LogoPath, false, 1.25);
            reports.AddColumnHeader(
                new HeaderLine { Content = "Republic of the Philippines", Bold = false, Italic = false, FontSize = 8.5, ParagraphAlignment = ParagraphAlignment.Left },
                new HeaderLine { Content = "", Bold = false, Italic = false, FontSize = 6, ParagraphAlignment = ParagraphAlignment.Left }
            );
            reports.AddColumnHeader(
                new HeaderLine { Content = agencyDetails.AgencyName.ToUpper(), Bold = true, Italic = false, FontSize = 11, ParagraphAlignment = ParagraphAlignment.Left },
                new HeaderLine { Content = "", Bold = false, Italic = false, FontSize = 7, ParagraphAlignment = ParagraphAlignment.Left }
            );
            reports.AddColumnHeader(
                new HeaderLine { Content = bidsAndAwards.Sector, Bold = false, Italic = false, FontSize = 8, ParagraphAlignment = ParagraphAlignment.Left },
                new HeaderLine { Content = "", Bold = false, Italic = false, FontSize = 7, ParagraphAlignment = ParagraphAlignment.Left }
            );
            reports.AddColumnHeader(
                new HeaderLine { Content = bidsAndAwards.Department + " - " + bidsAndAwards.Section, Bold = true, Italic = false, FontSize = 9, ParagraphAlignment = ParagraphAlignment.Left },
                new HeaderLine { Content = "", Bold = true, Italic = false, FontSize = 8, ParagraphAlignment = ParagraphAlignment.Left }
            );
            reports.AddColumnHeader(
                new HeaderLine { Content = agencyDetails.Address + "\n", Bold = false, Italic = false, FontSize = 8, ParagraphAlignment = ParagraphAlignment.Left },
                new HeaderLine { Content = "", Bold = false, Italic = false, FontSize = 8, ParagraphAlignment = ParagraphAlignment.Left }
            );

            var columns = new List<ContentColumn>();
            columns.Add(new ContentColumn(6.50));
            reports.AddTable(columns, false);

            reports.AddFormattedRowContent(new ContentCell(new TextWithFormat[]
            {
               new TextWithFormat("INVITATION TO BID FOR\n", true, false, 14),
               new TextWithFormat(invitationToBid.ContractName, true, false, 16),
            }, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center), 0.00);

            reports.AddNewLine();
            reports.AddNewLine();

            columns = new List<ContentColumn>();
            columns.Add(new ContentColumn(0.50));
            columns.Add(new ContentColumn(6.00));
            reports.AddTable(columns, false);

            var rows = new List<ContentCell>();
            rows.Add(new ContentCell("1. ", 0, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Top));
            rows.Add(new ContentCell(new TextWithFormat[]
            {
                new TextWithFormat("The ", false, false, 10),
                new TextWithFormat(agencyDetails.AgencyName, false, true, 10),
                new TextWithFormat(", through the ", false, false, 10),
                new TextWithFormat(invitationToBid.FundDescription, true, false, 10),
                new TextWithFormat(" intends to apply the sum of ", false, false, 10),
                new TextWithFormat(invitationToBid.ApprovedBudgetForContractWords + " (" + invitationToBid.ApprovedBudgetForContract.ToString("C", new System.Globalization.CultureInfo("en-ph")) + ")", true, false, 10),
                new TextWithFormat(" being the ABC to payments under the contract for ", false, false, 10),
                new TextWithFormat(invitationToBid.ContractName, true, true, 10),
                new TextWithFormat(" with project identification number ", false, false, 10),
                new TextWithFormat(invitationToBid.ContractCode, true, false, 10),
                new TextWithFormat(". Bids received in excess of the ABC shall be automatically rejected at bid opening.", false, false, 10),
            }, 1, ParagraphAlignment.Justify, VerticalAlignment.Top));
            reports.AddRowContent(rows, 0.25);

            reports.AddNewLine();

            columns = new List<ContentColumn>();
            columns.Add(new ContentColumn(0.50));
            columns.Add(new ContentColumn(6.00));
            reports.AddTable(columns, false);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("2. ", 0, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Top));
            rows.Add(new ContentCell(new TextWithFormat[]
            {
                new TextWithFormat("The ", false, false, 10),
                new TextWithFormat(agencyDetails.AgencyName, false, true, 10),
                new TextWithFormat(" now invites bids for the above Procurement Project", false, false, 10),
                new TextWithFormat(invitationToBid.ContractStrategy == ContractStrategies.LineItemBidding ? " - a line item bidding" : string.Empty, true, true, 10),
                new TextWithFormat(". Delivery of the Goods is required within ", false, false, 10),
                new TextWithFormat(invitationToBid.DeliveryPeriodWords + " (" + invitationToBid.DeliveryPeriod + ") Calendar Days", true, true, 10),
                new TextWithFormat(". Bidders should have completed, within the ", false, false, 10),
                new TextWithFormat(" last three (3) years ("+ (invitationToBid.FiscalYear - 3).ToString() +" - present) ", true, true, 10),
                new TextWithFormat(" from the date of submission and receipt of bids, a contract similar to the Project. The description of an eligible bidder is contained in the Bidding Documents, particularly, in Section II (Instructions to Bidders).", false, false, 10),
            }, 1, ParagraphAlignment.Justify, VerticalAlignment.Top));
            reports.AddRowContent(rows, 0.25);

            reports.AddNewLine();

            columns = new List<ContentColumn>();
            columns.Add(new ContentColumn(0.50));
            columns.Add(new ContentColumn(6.00));
            reports.AddTable(columns, false);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("3. ", 0, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Top));
            rows.Add(new ContentCell(new TextWithFormat[]
            {
                new TextWithFormat("Bidding will be conducted through open competitive bidding procedures using a non-discretionary “pass/fail” criterion as specified in the 2016 revised Implementing Rules and Regulations (IRR) of Republic Act (RA) No. 9184. ", false, false, 10),
            }, 1, ParagraphAlignment.Justify, VerticalAlignment.Top));
            reports.AddRowContent(rows, 0.25);

            reports.AddNewLine();

            columns = new List<ContentColumn>();
            columns.Add(new ContentColumn(0.50));
            columns.Add(new ContentColumn(6.00));
            reports.AddTable(columns, false);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("", 0, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Top));
            rows.Add(new ContentCell(new TextWithFormat[]
            {
                new TextWithFormat("Bidding is restricted to Filipino citizens/sole proprietorships, partnerships, or organizations with at least sixty percent (60%) interest or outstanding capital stock belonging to citizens of the Philippines, and to citizens or organizations of a country the laws or regulations of which grant similar rights or privileges to Filipino citizens, pursuant to RA No. 5183.", false, false, 10),
            }, 1, ParagraphAlignment.Justify, VerticalAlignment.Top));
            reports.AddRowContent(rows, 0.25);

            reports.AddNewLine();

            columns = new List<ContentColumn>();
            columns.Add(new ContentColumn(0.50));
            columns.Add(new ContentColumn(6.00));
            reports.AddTable(columns, false);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("4. ", 0, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Top));
            rows.Add(new ContentCell(new TextWithFormat[]
            {
                new TextWithFormat("Prospective Bidders may obtain further information from ", false, false, 10),
                new TextWithFormat(agencyDetails.AgencyName, false, true, 10),
                new TextWithFormat(" and inspect the Bidding Documents at the address given below during ", false, false, 10),
                new TextWithFormat("Monday to Friday 8:00 AM to 5:00 PM", true, true, 10),
                new TextWithFormat(".", false, false, 10),
            }, 1, ParagraphAlignment.Justify, VerticalAlignment.Top));
            reports.AddRowContent(rows, 0.25);

            reports.AddNewLine();

            columns = new List<ContentColumn>();
            columns.Add(new ContentColumn(0.50));
            columns.Add(new ContentColumn(6.00));
            reports.AddTable(columns, false);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("5. ", 0, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Top));
            rows.Add(new ContentCell(new TextWithFormat[]
            {
                new TextWithFormat("A complete set of Bidding Documents may be acquired by interested Bidders on ", false, false, 10),
                new TextWithFormat(invitationToBid.SubmissionStart.ToString("MMMM dd, yyyy"), true, false, 10),
                new TextWithFormat(" to ", false, false, 10),
                new TextWithFormat(invitationToBid.DeadlineOfSubmissionOfBids.ToString("MMMM dd, yyyy"), true, false, 10),
                new TextWithFormat(" from the given address and website(s) below ", false, false, 10),
                new TextWithFormat(" and upon payment of the applicable fee for the Bidding Documents, pursuant to the latest Guidelines issued by the GPPB, in the amount of ", false, true, 10),
                new TextWithFormat(invitationToBid.BidDocumentPriceWords + " (" + invitationToBid.BidDocumentPrice.ToString("C", new System.Globalization.CultureInfo("en-ph")) + ")", true, true, 10),
                new TextWithFormat(". The Procuring Entity shall allow the bidder to present its proof of payment for the fees by presenting the actual original receipt issued by the PUP cashier’s office.", false, false, 10),
            }, 1, ParagraphAlignment.Justify, VerticalAlignment.Top));
            reports.AddRowContent(rows, 0.25);

            reports.AddNewLine();

            columns = new List<ContentColumn>();
            columns.Add(new ContentColumn(0.50));
            columns.Add(new ContentColumn(6.00));
            reports.AddTable(columns, false);

            if (invitationToBid.PreBidVideoConferencingOptions == VideoConferencingOptions.NoVideoConferencing)
            {
                rows = new List<ContentCell>();
                rows.Add(new ContentCell("6. ", 0, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Top));
                rows.Add(new ContentCell(new TextWithFormat[]
                {
                new TextWithFormat("The ", false, false, 10),
                new TextWithFormat(agencyDetails.AgencyName, false, true, 10),
                new TextWithFormat(" will hold a Pre-Bid Conference", false, false, 10),
                new TextWithFormat("1", false, false, 10, true),
                new TextWithFormat(" on ", false, false, 10),
                new TextWithFormat(invitationToBid.PreBidConference.ToString("MMMM dd, yyyy hh:mm tt"), true, false, 10),
                new TextWithFormat(" at ", false, false, 10),
                new TextWithFormat(invitationToBid.PreBidConferenceLocation, true, false, 10),
                new TextWithFormat(" which shall be open to prospective bidders.", false, false, 10),
                }, 1, ParagraphAlignment.Justify, VerticalAlignment.Top));
                reports.AddRowContent(rows, 0.25);
            }
            else if (invitationToBid.PreBidVideoConferencingOptions == VideoConferencingOptions.WithVideoConferencing)
            {
                rows = new List<ContentCell>();
                rows.Add(new ContentCell("6. ", 0, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Top));
                rows.Add(new ContentCell(new TextWithFormat[]
                {
                new TextWithFormat("The ", false, false, 10),
                new TextWithFormat(agencyDetails.AgencyName, false, true, 10),
                new TextWithFormat(" will hold a Pre-Bid Conference", false, false, 10),
                new TextWithFormat("1", false, false, 10, true),
                new TextWithFormat(" on ", false, false, 10),
                new TextWithFormat(invitationToBid.PreBidConference.ToString("MMMM dd, yyyy hh:mm tt"), true, false, 10),
                new TextWithFormat(" at ", false, false, 10),
                new TextWithFormat(invitationToBid.PreBidConferenceLocation, true, false, 10),
                new TextWithFormat(" and/or through videoconferencing/webcasting via " + invitationToBid.PreBidVideoConferenceMode.ToUpper() + " which shall be open to prospective bidders.", false, false, 10),
                }, 1, ParagraphAlignment.Justify, VerticalAlignment.Top));
                reports.AddRowContent(rows, 0.25);
            }
            else
            {
                rows = new List<ContentCell>();
                rows.Add(new ContentCell("6. ", 0, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Top));
                rows.Add(new ContentCell(new TextWithFormat[]
                {
                new TextWithFormat("The ", false, false, 10),
                new TextWithFormat(agencyDetails.AgencyName, false, true, 10),
                new TextWithFormat(" will hold a Pre-Bid Conference", false, false, 10),
                new TextWithFormat("1", false, false, 10, true),
                new TextWithFormat(" on ", false, false, 10),
                new TextWithFormat(invitationToBid.PreBidConference.ToString("MMMM dd, yyyy hh:mm tt"), true, false, 10),
                new TextWithFormat(" through videoconferencing/webcasting via " + invitationToBid.PreBidVideoConferenceMode.ToUpper() + " which shall be open to prospective bidders.", false, false, 10),
                }, 1, ParagraphAlignment.Justify, VerticalAlignment.Top));
                reports.AddRowContent(rows, 0.25);
            }

            reports.AddNewLine();
            reports.AddNewLine();
            reports.AddNewLine();

            columns = new List<ContentColumn>();
            columns.Add(new ContentColumn(2.00));
            columns.Add(new ContentColumn(6.00));
            reports.AddTable(columns, false);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("", 0, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Top, 0, 0, true));
            rows.Add(new ContentCell("", 0, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Top));
            reports.AddRowContent(rows, 0.25);

            columns = new List<ContentColumn>();
            columns.Add(new ContentColumn(0.20));
            columns.Add(new ContentColumn(6.30));
            reports.AddTable(columns, false);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell(new TextWithFormat[]
            {
                new TextWithFormat("1 ", false, false, 8, true),
            }, 0, ParagraphAlignment.Justify, VerticalAlignment.Top));
            rows.Add(new ContentCell(new TextWithFormat[]
            {
                new TextWithFormat("May be deleted in case the ABC is less than One Million Pesos (PhP1,000,000) where the Procuring Entity may not hold a Pre-Bid Conference.", false, false, 8),
            }, 1, ParagraphAlignment.Justify, VerticalAlignment.Top));
            reports.AddRowContent(rows, 0.25);

            reports.InsertPageBreak();



            reports.AddNewLine();

            columns = new List<ContentColumn>();
            columns.Add(new ContentColumn(0.50));
            columns.Add(new ContentColumn(6.00));
            reports.AddTable(columns, false);

            if (invitationToBid.PreBidVideoConferencingOptions != VideoConferencingOptions.NoVideoConferencing)
            {
                columns = new List<ContentColumn>();
                columns.Add(new ContentColumn(0.50));
                columns.Add(new ContentColumn(6.00));
                reports.AddTable(columns, false);

                rows = new List<ContentCell>();
                rows.Add(new ContentCell("", 0, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Top));
                rows.Add(new ContentCell(new TextWithFormat[]
                {
                    new TextWithFormat("Prospective bidders may request for meeting ID and password by sending email at ", false, false, 10),
                    new TextWithFormat(invitationToBid.PreBidVideoConferenceAccessRequestEmail, false, true, 10),
                    new TextWithFormat(" or SMS to ", false, false, 10),
                    new TextWithFormat(invitationToBid.PreBidVideoConferenceAccessRequestContactNo, false, true, 10),
                    new TextWithFormat(" on or before ", false, false, 10),
                    new TextWithFormat(invitationToBid.PreBidConference.AddMinutes(-30).ToString("MMMM dd, yyyy hh:mm tt"), true, false, 10),
                    new TextWithFormat(" providing the following information:", false, false, 10),
                }, 1, ParagraphAlignment.Justify, VerticalAlignment.Top));
                reports.AddRowContent(rows, 0.25);

                reports.AddNewLine();

                columns = new List<ContentColumn>();
                columns.Add(new ContentColumn(1.00));
                columns.Add(new ContentColumn(1.00));
                columns.Add(new ContentColumn(0.50));
                columns.Add(new ContentColumn(3.00));
                columns.Add(new ContentColumn(1.00));
                reports.AddTable(columns, false);

                rows = new List<ContentCell>();
                rows.Add(new ContentCell("", 0, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Top));
                rows.Add(new ContentCell("Project", 1, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Top));
                rows.Add(new ContentCell(":", 2, 10, false, false, ParagraphAlignment.Right, VerticalAlignment.Top));
                rows.Add(new ContentCell("", 3, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Top, 0, 0, true));
                reports.AddRowContent(rows, 0.20);

                rows = new List<ContentCell>();
                rows.Add(new ContentCell("", 0, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Top));
                rows.Add(new ContentCell("Company", 1, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Top));
                rows.Add(new ContentCell(":", 2, 10, false, false, ParagraphAlignment.Right, VerticalAlignment.Top));
                rows.Add(new ContentCell("", 3, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Top, 0, 0, true));
                reports.AddRowContent(rows, 0.20);

                rows = new List<ContentCell>();
                rows.Add(new ContentCell("", 0, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Top));
                rows.Add(new ContentCell("Representative/s", 1, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Top));
                rows.Add(new ContentCell(":", 2, 10, false, false, ParagraphAlignment.Right, VerticalAlignment.Top));
                rows.Add(new ContentCell("", 3, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Top, 0, 0, true));
                reports.AddRowContent(rows, 0.20);

                rows = new List<ContentCell>();
                rows.Add(new ContentCell("", 0, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Top));
                rows.Add(new ContentCell("Position", 1, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Top));
                rows.Add(new ContentCell(":", 2, 10, false, false, ParagraphAlignment.Right, VerticalAlignment.Top));
                rows.Add(new ContentCell("", 3, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Top, 0, 0, true));
                reports.AddRowContent(rows, 0.20);

                rows = new List<ContentCell>();
                rows.Add(new ContentCell("", 0, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Top));
                rows.Add(new ContentCell("Designation/s", 1, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Top));
                rows.Add(new ContentCell(":", 2, 10, false, false, ParagraphAlignment.Right, VerticalAlignment.Top));
                rows.Add(new ContentCell("", 3, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Top, 0, 0, true));
                reports.AddRowContent(rows, 0.20);

                rows = new List<ContentCell>();
                rows.Add(new ContentCell("", 0, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Top));
                rows.Add(new ContentCell("Contact Nos.", 1, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Top));
                rows.Add(new ContentCell(":", 2, 10, false, false, ParagraphAlignment.Right, VerticalAlignment.Top));
                rows.Add(new ContentCell("", 3, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Top, 0, 0, true));
                reports.AddRowContent(rows, 0.20);

                rows = new List<ContentCell>();
                rows.Add(new ContentCell("", 0, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Top));
                rows.Add(new ContentCell("Email Address", 1, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Top));
                rows.Add(new ContentCell(":", 2, 10, false, false, ParagraphAlignment.Right, VerticalAlignment.Top));
                rows.Add(new ContentCell("", 3, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Top, 0, 0, true));
                reports.AddRowContent(rows, 0.20);
            }

            if (invitationToBid.PreBidAdditionalInstructions != null)
            {
                reports.AddNewLine();

                columns = new List<ContentColumn>();
                columns.Add(new ContentColumn(0.50));
                columns.Add(new ContentColumn(6.00));
                reports.AddTable(columns, false);

                rows = new List<ContentCell>();
                rows.Add(new ContentCell("", 0, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Top));
                rows.Add(new ContentCell(new TextWithFormat[]
                {
                new TextWithFormat(invitationToBid.PreBidAdditionalInstructions, false, false, 10),
                }, 1, ParagraphAlignment.Justify, VerticalAlignment.Top));
                reports.AddRowContent(rows, 0.25);
            }

            reports.AddNewLine();

            columns = new List<ContentColumn>();
            columns.Add(new ContentColumn(0.50));
            columns.Add(new ContentColumn(6.00));
            reports.AddTable(columns, false);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("7. ", 0, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Top));
            rows.Add(new ContentCell(new TextWithFormat[]
            {
                new TextWithFormat("Bids must be duly received by the BAC Secretariat through manual submission at the office address indicated below, on or before ", false, false, 10),
                new TextWithFormat(invitationToBid.DeadlineOfSubmissionOfBids.ToString("MMMM dd, yyyy hh:mm tt"), true, false, 10),
                new TextWithFormat(". Late bids shall not be accepted.", false, false, 10),
            }, 1, ParagraphAlignment.Justify, VerticalAlignment.Top));
            reports.AddRowContent(rows, 0.25);

            reports.AddNewLine();

            columns = new List<ContentColumn>();
            columns.Add(new ContentColumn(0.50));
            columns.Add(new ContentColumn(6.00));
            reports.AddTable(columns, false);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("8. ", 0, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Top));
            rows.Add(new ContentCell(new TextWithFormat[]
            {
                new TextWithFormat("All Bids must be accompanied by a bid security in any of the acceptable forms and in the amount stated in ", false, false, 10),
                new TextWithFormat(invitationToBid.Classification == "Goods" || invitationToBid.Classification == "Services" ? "ITB Clause 14" : "ITB Clause 16", true, false, 10),
                new TextWithFormat(".", false, false, 10),
            }, 1, ParagraphAlignment.Justify, VerticalAlignment.Top));
            reports.AddRowContent(rows, 0.25);

            reports.AddNewLine();

            columns = new List<ContentColumn>();
            columns.Add(new ContentColumn(0.50));
            columns.Add(new ContentColumn(6.00));
            reports.AddTable(columns, false);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("9. ", 0, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Top));
            rows.Add(new ContentCell(new TextWithFormat[]
            {
                new TextWithFormat("Bid opening shall be on ", false, false, 10),
                new TextWithFormat(invitationToBid.OpeningOfBids.ToString("MMMM dd, yyyy hh:mm tt"), true, false, 10),
                new TextWithFormat(" at ", false, false, 10),
                new TextWithFormat(invitationToBid.OpeningOfBidsLocation, true, false, 10),
                new TextWithFormat(". Bids will be opened in the presence of the bidders’ representatives who choose to attend the activity.", false, false, 10),
            }, 1, ParagraphAlignment.Justify, VerticalAlignment.Top));
            reports.AddRowContent(rows, 0.25);

            reports.AddNewLine();

            columns = new List<ContentColumn>();
            columns.Add(new ContentColumn(0.50));
            columns.Add(new ContentColumn(6.00));
            reports.AddTable(columns, false);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("10. ", 0, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Top));
            rows.Add(new ContentCell(new TextWithFormat[]
            {
                new TextWithFormat("The ", false, false, 10),
                new TextWithFormat(agencyDetails.AgencyName, false, true, 10),
                new TextWithFormat(" reserves the right to reject any and all bids, declare a failure of bidding, or not award the contract at any time prior to contract award in accordance with Sections 35.6 and 41 of the 2016 revised IRR of RA No. 9184, without thereby incurring any liability to the affected bidder or bidders.", false, false, 10),
            }, 1, ParagraphAlignment.Justify, VerticalAlignment.Top));
            reports.AddRowContent(rows, 0.25);

            reports.AddNewLine();

            columns = new List<ContentColumn>();
            columns.Add(new ContentColumn(0.50));
            columns.Add(new ContentColumn(6.00));
            reports.AddTable(columns, false);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("11. ", 0, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Top));
            rows.Add(new ContentCell(new TextWithFormat[]
            {
                new TextWithFormat("For further information, please refer to:", false, false, 10),
            }, 1, ParagraphAlignment.Justify, VerticalAlignment.Top));
            reports.AddRowContent(rows, 0.25);

            reports.AddNewLine();

            columns = new List<ContentColumn>();
            columns.Add(new ContentColumn(0.50));
            columns.Add(new ContentColumn(6.00));
            reports.AddTable(columns, false);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("", 0, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Top));
            rows.Add(new ContentCell(new TextWithFormat[]
            {
                new TextWithFormat(hris.GetEmployeeDetailByCode(bacsecHead.Member).EmployeeName.ToUpper(), true, false, 10),
            }, 1, ParagraphAlignment.Justify, VerticalAlignment.Top));
            reports.AddRowContent(rows, 0.00);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("", 0, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Top));
            rows.Add(new ContentCell(new TextWithFormat[]
            {
                new TextWithFormat(bacsecHead.Membership.GetAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>().Name, false, true, 10),
            }, 1, ParagraphAlignment.Justify, VerticalAlignment.Top));
            reports.AddRowContent(rows, 0.00);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("", 0, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Top));
            rows.Add(new ContentCell(new TextWithFormat[]
            {
                new TextWithFormat(bidsAndAwards.Section, false, false, 10),
            }, 1, ParagraphAlignment.Justify, VerticalAlignment.Top));
            reports.AddRowContent(rows, 0.00);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("", 0, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Top));
            rows.Add(new ContentCell(new TextWithFormat[]
            {
                new TextWithFormat(agencyDetails.BACOfficeAddress, false, false, 10),
            }, 1, ParagraphAlignment.Justify, VerticalAlignment.Top));
            reports.AddRowContent(rows, 0.00);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("", 0, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Top));
            rows.Add(new ContentCell(new TextWithFormat[]
            {
                new TextWithFormat("Email: " + agencyDetails.BACOfficeEmail, false, false, 10),
            }, 1, ParagraphAlignment.Justify, VerticalAlignment.Top));
            reports.AddRowContent(rows, 0.00);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("", 0, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Top));
            rows.Add(new ContentCell(new TextWithFormat[]
            {
                new TextWithFormat("Contact No.: " + agencyDetails.BACOfficeContactNo + (agencyDetails.BACOfficeAlternateContactNo == null ? string.Empty : " or " + agencyDetails.BACOfficeAlternateContactNo), false, false, 10),
            }, 1, ParagraphAlignment.Justify, VerticalAlignment.Top));
            reports.AddRowContent(rows, 0.00);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("", 0, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Top));
            rows.Add(new ContentCell(new TextWithFormat[]
            {
                new TextWithFormat("Website: " + agencyDetails.Website, false, false, 10),
            }, 1, ParagraphAlignment.Justify, VerticalAlignment.Top));
            reports.AddRowContent(rows, 0.00);

            reports.AddNewLine();

            columns = new List<ContentColumn>();
            columns.Add(new ContentColumn(0.50));
            columns.Add(new ContentColumn(6.00));
            reports.AddTable(columns, false);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("12. ", 0, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Top));
            rows.Add(new ContentCell(new TextWithFormat[]
            {
                new TextWithFormat("For downloading of Bidding Documents, you may visit the following websites:", false, false, 10),
            }, 1, ParagraphAlignment.Justify, VerticalAlignment.Top));
            reports.AddRowContent(rows, 0.25);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("", 0, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Top));
            rows.Add(new ContentCell(new TextWithFormat[]
            {
                new TextWithFormat("https://www.pup.edu.ph/notices/", false, false, 10),
            }, 1, ParagraphAlignment.Justify, VerticalAlignment.Top));
            reports.AddRowContent(rows, 0.00);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("", 0, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Top));
            rows.Add(new ContentCell(new TextWithFormat[]
            {
                new TextWithFormat("https://notices.philgeps.gov.ph/", false, false, 10),
            }, 1, ParagraphAlignment.Justify, VerticalAlignment.Top));
            reports.AddRowContent(rows, 0.00);

            reports.AddNewLine();
            reports.AddNewLine();

            columns = new List<ContentColumn>();
            columns.Add(new ContentColumn(0.50));
            columns.Add(new ContentColumn(6.00));
            reports.AddTable(columns, false);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("", 0, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Top));
            rows.Add(new ContentCell(new TextWithFormat[]
            {
                new TextWithFormat(invitationToBid.Date.ToString("MMMM dd, yyyy"), false, false, 10),
            }, 1, ParagraphAlignment.Justify, VerticalAlignment.Top));
            reports.AddRowContent(rows, 0.00);

            reports.AddNewLine();
            reports.AddNewLine();
            reports.AddNewLine();

            columns = new List<ContentColumn>();
            columns.Add(new ContentColumn(3.50));
            columns.Add(new ContentColumn(3.00));
            reports.AddTable(columns, false);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("", 0, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Top));
            rows.Add(new ContentCell(new TextWithFormat[]
            {
                new TextWithFormat(hris.GetEmployeeDetailByCode(bacChair.Member).EmployeeName.ToUpper(), true, false, 10),
            }, 1, ParagraphAlignment.Justify, VerticalAlignment.Top));
            reports.AddRowContent(rows, 0.00);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("", 0, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Top));
            rows.Add(new ContentCell(new TextWithFormat[]
            {
                new TextWithFormat(bacChair.Membership.GetAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>().Name, false, true, 10),
            }, 1, ParagraphAlignment.Justify, VerticalAlignment.Top));
            reports.AddRowContent(rows, 0.00);

            return reports.GenerateReport();
        }
        public MemoryStream GenerateRequestForQuotation(string LogoPath, string ReferenceNo)
        {
            var rfq = contractDAL.RequestForQuotationSetup(ReferenceNo);
            var agencyDetails = db.AgencyDetails.FirstOrDefault();

            Reports reports = new Reports();
            reports.ReportFilename = ReferenceNo;
            reports.CreateDocument(8.50, 13.00, Orientation.Portrait, 1.00);
            reports.AddDoubleColumnHeader(LogoPath, false, 1.25);
            reports.AddPageNumbersFooter();
            reports.AddColumnHeader(
                new HeaderLine { Content = "Republic of the Philippines", Bold = false, Italic = false, FontSize = 8, ParagraphAlignment = ParagraphAlignment.Left },
                new HeaderLine { Content = "PUP-REFQ-6-PRMO-005", Bold = false, Italic = false, FontSize = 6, ParagraphAlignment = ParagraphAlignment.Left }
            );
            reports.AddColumnHeader(
                new HeaderLine { Content = "POLYTECHNIC UNIVERSITY OF THE PHILIPPINES", Bold = true, Italic = false, FontSize = 12, ParagraphAlignment = ParagraphAlignment.Left },
                new HeaderLine { Content = "Rev. 1", Bold = false, Italic = false, FontSize = 7, ParagraphAlignment = ParagraphAlignment.Left }
            );
            reports.AddColumnHeader(
                new HeaderLine { Content = "A. Mabini Campus, Anonas St., Santa Mesa, Manila\t1016", Bold = false, Italic = false, FontSize = 10, ParagraphAlignment = ParagraphAlignment.Left },
                new HeaderLine { Content = "February 16, 2021", Bold = false, Italic = false, FontSize = 7, ParagraphAlignment = ParagraphAlignment.Left }
            );
            reports.AddColumnHeader(
                new HeaderLine { Content = "THE COUNTRY'S FIRST POLYTECHNIC UNIVERSITY", Bold = false, Italic = false, FontSize = 10, ParagraphAlignment = ParagraphAlignment.Left },
                new HeaderLine { Content = "", Bold = true, Italic = false, FontSize = 5, ParagraphAlignment = ParagraphAlignment.Left }
            );

            var columns = new List<ContentColumn>();
            columns.Add(new ContentColumn(6.50));
            reports.AddTable(columns, false);

            reports.AddFormattedRowContent(new ContentCell(new TextWithFormat[]
            {
               new TextWithFormat("REQUEST FOR QUOTATION", true, false, 14, Underline.Single),
            }, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center), 0.00);

            reports.AddNewLine();
            reports.AddNewLine();

            columns = new List<ContentColumn>();
            columns.Add(new ContentColumn(6.50));
            reports.AddTable(columns, false);

            var figureWords = Reports.AmountToWords(rfq.ABC);

            reports.AddFormattedRowContent(new ContentCell(new TextWithFormat[]
            {
               new TextWithFormat("The ", false, false, 11),
               new TextWithFormat("Polytechnic University of the Philippines (PUP) - Bids and Awards Committee (BAC)", true, false, 11),
               new TextWithFormat(" will undertake a ", false, false, 11),
               new TextWithFormat(" " + rfq.ModeOfProcurement + " ", true, true, 11),
               new TextWithFormat(" for FY ", false, false, 11),
               new TextWithFormat(" "+ rfq.FiscalYear +" ", true, false, 11),
               new TextWithFormat(" in accordance with Section 53 of the 2016 Revised Implementing Rules and Regulations of Republic Act No. 9184. ", false, false, 11),
               new TextWithFormat(" The Approved Budget for the Contract (ABC) is ", false, false, 11),
               new TextWithFormat(" " + figureWords.ToUpper() + " (" + rfq.ABC.ToString("C", new System.Globalization.CultureInfo("en-ph")) + ")", true, true, 11),
               new TextWithFormat(".", false, false, 11),
            }, new Unit(0.50, UnitType.Inch), MigraDoc.DocumentObjectModel.ParagraphAlignment.Justify), 0.00);

            reports.AddNewLine();
            reports.AddNewLine();

            columns = new List<ContentColumn>();
            columns.Add(new ContentColumn(6.50));
            reports.AddTable(columns, false);

            reports.AddFormattedRowContent(new ContentCell(new TextWithFormat[]
            {
               new TextWithFormat("Please quote your ", false, false, 11),
               new TextWithFormat(" best offer ", true, false, 11),
               new TextWithFormat(" for the item described herein, subject to the Terms and Conditions provided at the last page of this Request for Quotation. ", false, false, 11),
               new TextWithFormat(" Submit your quotation duly signed by you or your duly authorized representative not later than ", false, false, 11),
               new TextWithFormat(rfq.Deadline.ToString("dd MMMM yyyy hh:mm tt"), true, false, 11, Underline.Single),
               new TextWithFormat(" at the ", false, false, 11),
               new TextWithFormat("Procurement Management Office", true, false, 11),
               new TextWithFormat(", Ground Floor North Wing, PUP A. Mabini Campus, Anonas St., Santa Mesa, Manila. Open submission may be submitted, manually or through email at the address and contact numbers indicated below. ", false, false, 11),
            }, new Unit(0.50, UnitType.Inch), MigraDoc.DocumentObjectModel.ParagraphAlignment.Justify), 0.00);

            reports.AddNewLine();
            reports.AddNewLine();

            columns = new List<ContentColumn>();
            columns.Add(new ContentColumn(6.50));
            reports.AddTable(columns, false);

            reports.AddFormattedRowContent(new ContentCell(new TextWithFormat[]
            {
               new TextWithFormat("Copies of the following documents, in its valid and updated versions, are required to be submitted along with your signed quotation to wit: ", false, false, 11)
            }, new Unit(0.50, UnitType.Inch), MigraDoc.DocumentObjectModel.ParagraphAlignment.Justify), 0.00);

            reports.AddNewLine();

            columns = new List<ContentColumn>();
            columns.Add(new ContentColumn(1.00));
            columns.Add(new ContentColumn(0.20));
            columns.Add(new ContentColumn(5.30));
            reports.AddTable(columns, false);

            var rows = new List<ContentCell>();
            rows.Add(new ContentCell("", 0, 8, false, false, ParagraphAlignment.Left, VerticalAlignment.Center));
            rows.Add(new ContentCell("1. ", 1, 11, false, false, ParagraphAlignment.Right, VerticalAlignment.Top));
            rows.Add(new ContentCell("Business or Mayor's Permit;", 2, 11, false, false, ParagraphAlignment.Left, VerticalAlignment.Top));
            reports.AddRowContent(rows, 0.25);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("", 0, 8, false, false, ParagraphAlignment.Left, VerticalAlignment.Center));
            rows.Add(new ContentCell("2. ", 1, 11, false, false, ParagraphAlignment.Right, VerticalAlignment.Top));
            rows.Add(new ContentCell("PhilGEPS Registration;", 2, 11, false, false, ParagraphAlignment.Left, VerticalAlignment.Top));
            reports.AddRowContent(rows, 0.25);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("", 0, 8, false, false, ParagraphAlignment.Left, VerticalAlignment.Center));
            rows.Add(new ContentCell("3. ", 1, 11, false, false, ParagraphAlignment.Right, VerticalAlignment.Top));
            rows.Add(new ContentCell("Income or Business Tax Return;", 2, 11, false, false, ParagraphAlignment.Left, VerticalAlignment.Top));
            reports.AddRowContent(rows, 0.25);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("", 0, 8, false, false, ParagraphAlignment.Left, VerticalAlignment.Center));
            rows.Add(new ContentCell("4. ", 1, 11, false, false, ParagraphAlignment.Right, VerticalAlignment.Top));
            rows.Add(new ContentCell("SEC or DTI Registration;", 2, 11, false, false, ParagraphAlignment.Left, VerticalAlignment.Top));
            reports.AddRowContent(rows, 0.25);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("", 0, 8, false, false, ParagraphAlignment.Left, VerticalAlignment.Center));
            rows.Add(new ContentCell("5. ", 1, 11, false, false, ParagraphAlignment.Right, VerticalAlignment.Top));
            rows.Add(new ContentCell("Tax Clearance;", 2, 11, false, false, ParagraphAlignment.Left, VerticalAlignment.Top));
            reports.AddRowContent(rows, 0.25);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("", 0, 8, false, false, ParagraphAlignment.Left, VerticalAlignment.Center));
            rows.Add(new ContentCell("6. ", 1, 11, false, false, ParagraphAlignment.Right, VerticalAlignment.Top));
            rows.Add(new ContentCell("Notarized Omnibus Sworn Statement for this particular procurement project;", 2, 11, false, false, ParagraphAlignment.Left, VerticalAlignment.Top));
            reports.AddRowContent(rows, 0.25);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("", 0, 11, false, false, ParagraphAlignment.Left, VerticalAlignment.Center));
            rows.Add(new ContentCell("7. ", 1, 11, false, false, ParagraphAlignment.Right, VerticalAlignment.Top));
            rows.Add(new ContentCell(new TextWithFormat[]
            {
                            new TextWithFormat("Professional License (", false, false, 11),
                            new TextWithFormat("for Infrastructure", false, true, 11),
                            new TextWithFormat(") or Curriculum Vitae (", false, false, 11),
                            new TextWithFormat("for Consulting Services", false, true, 11),
                            new TextWithFormat(");", false, false, 11),
            }, 2, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, false, false));
            reports.AddRowContent(rows, 0.45);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("", 0, 8, false, false, ParagraphAlignment.Left, VerticalAlignment.Center));
            rows.Add(new ContentCell("8. ", 1, 11, false, false, ParagraphAlignment.Right, VerticalAlignment.Top));
            rows.Add(new ContentCell(new TextWithFormat[]
            {
                            new TextWithFormat("PCAB License (", false, false, 11),
                            new TextWithFormat("for Infrastructure", false, true, 11),
                            new TextWithFormat("); and", false, false, 11),
            }, 2, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, false, false));
            reports.AddRowContent(rows, 0.25);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("", 0, 8, false, false, ParagraphAlignment.Left, VerticalAlignment.Center));
            rows.Add(new ContentCell("9. ", 1, 11, false, false, ParagraphAlignment.Right, VerticalAlignment.Top));
            rows.Add(new ContentCell(new TextWithFormat[]
            {
                            new TextWithFormat("Certification of Product Registration from BFAD (", false, false, 11),
                            new TextWithFormat("for Drugs and Medicines", false, true, 11),
                            new TextWithFormat(")", false, false, 11),
            }, 2, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, false, false));
            reports.AddRowContent(rows, 0.25);

            reports.AddNewLine();
            reports.AddNewLine();

            columns = new List<ContentColumn>();
            columns.Add(new ContentColumn(6.50));
            reports.AddTable(columns, false);

            reports.AddFormattedRowContent(new ContentCell(new TextWithFormat[]
            {
               new TextWithFormat("Failure to submit any or all of the foregoing documents may result to the automatic disqualification of the quotation.", false, false, 11)
            }, new Unit(0.50, UnitType.Inch), MigraDoc.DocumentObjectModel.ParagraphAlignment.Justify), 0.00);

            reports.AddNewLine();
            reports.AddNewLine();

            columns = new List<ContentColumn>();
            columns.Add(new ContentColumn(6.50));
            reports.AddTable(columns, false);

            reports.AddFormattedRowContent(new ContentCell(new TextWithFormat[]
            {
               new TextWithFormat("For inquiries, feel free to contact us at ", false, false, 11),
               new TextWithFormat(" (02) 8713-1504 ", true, false, 11),
               new TextWithFormat(" or email us at ", false, false, 11),
               new TextWithFormat(" procurementoffice@pup.edu.ph.", false, true, 11, Underline.Single)
            }, new Unit(0.50, UnitType.Inch), MigraDoc.DocumentObjectModel.ParagraphAlignment.Justify), 0.00);

            reports.AddNewLine();
            reports.AddNewLine();
            reports.AddNewLine();
            reports.AddNewLine();
            reports.AddNewLine();

            columns = new List<ContentColumn>();
            columns.Add(new ContentColumn(3.00));
            columns.Add(new ContentColumn(3.00));
            columns.Add(new ContentColumn(0.50));
            reports.AddTable(columns, false);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("", 0, 8, false, false, ParagraphAlignment.Left, VerticalAlignment.Center));
            rows.Add(new ContentCell("DR. EMANUEL C. DE GUZMAN", 1, 11, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
            rows.Add(new ContentCell("", 2, 11, false, false, ParagraphAlignment.Left, VerticalAlignment.Top));
            reports.AddRowContent(rows, 0.25);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("", 0, 8, false, false, ParagraphAlignment.Left, VerticalAlignment.Center));
            rows.Add(new ContentCell("BAC Chairman", 1, 11, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
            rows.Add(new ContentCell("", 2, 11, false, false, ParagraphAlignment.Left, VerticalAlignment.Top));
            reports.AddRowContent(rows, 0.25);

            reports.InsertPageBreak();

            columns = new List<ContentColumn>();
            columns.Add(new ContentColumn(6.50));
            reports.AddTable(columns, false);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("\n", 0, 8, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), true, true, true));
            reports.AddRowContent(rows, 0.00);

            columns = new List<ContentColumn>();
            columns.Add(new ContentColumn(2.25));
            columns.Add(new ContentColumn(2.00));
            columns.Add(new ContentColumn(2.25));
            reports.AddTable(columns, false);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("", 0, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), false, false, true));
            rows.Add(new ContentCell("TERMS AND CONDITIONS", 1, 10, true, false, ParagraphAlignment.Center, VerticalAlignment.Bottom, 0, 0, true, new Color(0, 0, 0, 0), false, false, false));
            rows.Add(new ContentCell("", 2, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), true, false, false));
            reports.AddRowContent(rows, 0.00);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("", 0, 5, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), false, false, true));
            rows.Add(new ContentCell("", 1, 5, true, false, ParagraphAlignment.Center, VerticalAlignment.Bottom, 0, 0, false, new Color(0, 0, 0, 0), false, false, false));
            rows.Add(new ContentCell("", 2, 5, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), true, false, false));
            reports.AddRowContent(rows, 0.00);


            columns = new List<ContentColumn>();
            columns.Add(new ContentColumn(0.40));
            columns.Add(new ContentColumn(5.90));
            columns.Add(new ContentColumn(0.20));
            reports.AddTable(columns, false);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("1.", 0, 10, false, false, ParagraphAlignment.Right, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, false, true));
            rows.Add(new ContentCell("Suppliers shall provide correct and accurate information in this form.", 1, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, false, false));
            rows.Add(new ContentCell("", 2, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), true, false, false));
            reports.AddRowContent(rows, 0.30);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("2.", 0, 10, false, false, ParagraphAlignment.Right, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, false, true));
            rows.Add(new ContentCell("Price quotation/s must be valid for a period of thirty (30) calendar days from the date of submission.", 1, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, false, false));
            rows.Add(new ContentCell("", 2, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), true, false, false));
            reports.AddRowContent(rows, 0.40);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("3.", 0, 10, false, false, ParagraphAlignment.Right, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, false, true));
            rows.Add(new ContentCell("Price quotation/s, to be denominated in Philippine peso shall include all taxes, duties and/or levies payable.", 1, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, false, false));
            rows.Add(new ContentCell("", 2, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), true, false, false));
            reports.AddRowContent(rows, 0.40);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("4.", 0, 10, false, false, ParagraphAlignment.Right, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, false, true));
            rows.Add(new ContentCell("Quoations exceeding the Approved Budget for the Contract shall be rejected outright.", 1, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, false, false));
            rows.Add(new ContentCell("", 2, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), true, false, false));
            reports.AddRowContent(rows, 0.30);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("5.", 0, 10, false, false, ParagraphAlignment.Right, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, false, true));
            rows.Add(new ContentCell("Award of contract shall be made to the lowest calculated and responsive quotation (for goods and infrastructure) or, the highest rated offer (for consulting services) which complies with the minimum technical specifications and other terms and conditions stated then.", 1, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, false, false));
            rows.Add(new ContentCell("", 2, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), true, false, false));
            reports.AddRowContent(rows, 0.60);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("6.", 0, 10, false, false, ParagraphAlignment.Right, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, false, true));
            rows.Add(new ContentCell("Contracts, in the form of a purchase order, agreement, or any memorandum, shall be duly signed or received within five (5) days from the receipt of the Notice of Award by the supplier, through its authorized representative/s. Otherwise, failure to receive the contract shall be a ground for the cancellation of the award.", 1, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, false, false));
            rows.Add(new ContentCell("", 2, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), true, false, false));
            reports.AddRowContent(rows, 0.80);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("7.", 0, 10, false, false, ParagraphAlignment.Right, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, false, true));
            rows.Add(new ContentCell("The Notice to Proceed shall, likewise, be duly signed and received within (5) days from the receipt of the contract, by the supplier, through its authorized representative/s. Otherwise, failure to receive the Notice to Proceed shall be a ground for the cancellation of the contract. However, in cases where the contract specifies a condition for its implementation, the Notice to Proceed must be duly signed and received within five (5) days from the satisfaction of the condition and the issuance of Notice to Proceed.", 1, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, false, false));
            rows.Add(new ContentCell("", 2, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), true, false, false));
            reports.AddRowContent(rows, 1.10);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("8.", 0, 10, false, false, ParagraphAlignment.Right, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, false, true));
            rows.Add(new ContentCell("Any interlineations, erasures, or overwriting shall be valid only if they are signed or initialed by you or any or your duly authorized representative/s.", 1, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, false, false));
            rows.Add(new ContentCell("", 2, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), true, false, false));
            reports.AddRowContent(rows, 0.50);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("9.", 0, 10, false, false, ParagraphAlignment.Right, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, false, true));
            rows.Add(new ContentCell("The item/s shall be delivered according to the requirements specified in the Technical Specifications.", 1, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, false, false));
            rows.Add(new ContentCell("", 2, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), true, false, false));
            reports.AddRowContent(rows, 0.50);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("10.", 0, 10, false, false, ParagraphAlignment.Right, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, false, true));
            rows.Add(new ContentCell("The UNIVERSITY, shall have the right to inspect and/or test the goods to confirm their conformity to the technical specifications.", 1, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, false, false));
            rows.Add(new ContentCell("", 2, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), true, false, false));
            reports.AddRowContent(rows, 0.50);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell(" 11.", 0, 10, false, false, ParagraphAlignment.Right, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, false, true));
            rows.Add(new ContentCell("In case two or more suppliers are determined to have submitted the Lowest Calculated Quotation/Lowest Calculated and Responsive Quotation, the UNIVERSITY shall adopt and employ \"draw lots\" as the tie-breaking method to finally determine the winning provider in accordance with GPPB Circular 06-2005.", 1, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, false, false));
            rows.Add(new ContentCell("", 2, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), true, false, false));
            reports.AddRowContent(rows, 0.80);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell(" 12.", 0, 10, false, false, ParagraphAlignment.Right, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, false, true));
            rows.Add(new ContentCell("Supplier shall guarantee his deliveries to be free from defects. Any defective item(s)/products(s), therefore that may be discovered by the UNIVERSITY within three months after acceptance of the same, shall be replaced by the supplier within three (3) calendar days upon receipt of a written notice to that effect. " +
                                      "Warranty period shall be three (3) months, in case of Expendable Supplies, or one (1) year, in case of Non-expendable Supplies. Warranty shall be covered by, at the Supplier's option, either retention money in the amount equivalent to at least five percent (5%) of every progress payment, or a special bank guarantee equivalent to at least five percent (5%) of the Contract Price." +
                                      "The said amounts shall only be released after the lapse of the warranty period; provided however, that the supplies delivered are free from patent and latent defects and all the conditions imposed under this contract have been fully met.", 1, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, false, false));
            rows.Add(new ContentCell("", 2, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), true, false, false));
            reports.AddRowContent(rows, 1.90);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell(" 13.", 0, 10, false, false, ParagraphAlignment.Right, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, false, true));
            rows.Add(new ContentCell("Payment shall be processed after delivery and upon the submission of the required supporting documents, in accordance with existing accounting rules and regulations. Please note that the corresponding bank transfer fee, if any, shall be chargable to the contractor's account.", 1, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, false, false));
            rows.Add(new ContentCell("", 2, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), true, false, false));
            reports.AddRowContent(rows, 0.60);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell(" 14.", 0, 10, false, false, ParagraphAlignment.Right, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, false, true));
            rows.Add(new ContentCell("The performance security may be released by the UNIVERSITY after the issuance of the Certificate of Final Acceptance, subject to the following conditions:", 1, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, false, false));
            rows.Add(new ContentCell("", 2, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), true, false, false));
            reports.AddRowContent(rows, 0.40);

            columns = new List<ContentColumn>();
            columns.Add(new ContentColumn(0.75));
            columns.Add(new ContentColumn(0.25));
            columns.Add(new ContentColumn(5.30));
            columns.Add(new ContentColumn(0.20));
            reports.AddTable(columns, false);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("", 0, 10, false, false, ParagraphAlignment.Right, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, false, true));
            rows.Add(new ContentCell("a.", 1, 10, false, false, ParagraphAlignment.Right, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, false, true));
            rows.Add(new ContentCell("UNIVERSITY has no claims filed against the supplier or the surety company;", 2, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, false, false));
            rows.Add(new ContentCell("", 3, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), true, false, false));
            reports.AddRowContent(rows, 0.20);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("", 0, 10, false, false, ParagraphAlignment.Right, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, false, true));
            rows.Add(new ContentCell("b.", 1, 10, false, false, ParagraphAlignment.Right, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, false, true));
            rows.Add(new ContentCell("It has no claims for labor and materials filed against the CONTRACTOR; and", 2, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, false, false));
            rows.Add(new ContentCell("", 3, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), true, false, false));
            reports.AddRowContent(rows, 0.20);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("", 0, 10, false, false, ParagraphAlignment.Right, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, false, true));
            rows.Add(new ContentCell("c.", 1, 10, false, false, ParagraphAlignment.Right, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, false, true));
            rows.Add(new ContentCell("Other terms of the contract. [Section 39.5 of the 2016 RIRR of Republic Act No. 9184 (R.A. No. 9184) otherwise known as the \"Government Procurement Reform Act\"]", 2, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, false, false));
            rows.Add(new ContentCell("", 3, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), true, false, false));
            reports.AddRowContent(rows, 0.30);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("\n", 0, 10, false, false, ParagraphAlignment.Right, VerticalAlignment.Top, 0, 0, true, new Color(0, 0, 0, 0), false, false, true));
            rows.Add(new ContentCell("\n", 1, 10, false, false, ParagraphAlignment.Right, VerticalAlignment.Top, 0, 0, true, new Color(0, 0, 0, 0), false, false, true));
            rows.Add(new ContentCell("\n", 2, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, true, new Color(0, 0, 0, 0), false, false, false));
            rows.Add(new ContentCell("\n", 3, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, true, new Color(0, 0, 0, 0), true, false, false));
            reports.AddRowContent(rows, 0.00);

            reports.InsertPageBreak();

            columns = new List<ContentColumn>();
            columns.Add(new ContentColumn(0.40));
            columns.Add(new ContentColumn(5.90));
            columns.Add(new ContentColumn(0.20));
            reports.AddTable(columns, false);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("\n", 0, 10, false, false, ParagraphAlignment.Right, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, true, true));
            rows.Add(new ContentCell("\n", 1, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, true, false));
            rows.Add(new ContentCell("\n", 2, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), true, true, false));
            reports.AddRowContent(rows, 0.00);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("15.", 0, 10, false, false, ParagraphAlignment.Right, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, true, true));
            rows.Add(new ContentCell("If delaye are likely to be incured, the supplier must notify the UNIVERSITY in writing. It must stated therein the cause/s and duration of expected delay. The UNIVERSITY may grant time extensions, at its discretion, if based on meritorious grounds, with or without liquidated damages. " +
                "In all cases, the request for extension should be submitted before the lapse of the original delivery date. The maximum allowable extension shall not be longer than the initial delivery period as stated in the original contract. (Manual of Procedures for the Procurement of Goods and Services)", 1, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, true, false));
            rows.Add(new ContentCell("", 2, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), true, true, false));
            reports.AddRowContent(rows, 1.20);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("16.", 0, 10, false, false, ParagraphAlignment.Right, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, false, true));
            rows.Add(new ContentCell("All deliveries by the supplier shall be subject to inspection and acceptance by the UNIVERSITY. The UNIVERSITY reserves the right to reduce the price in case of non-conformity of the product(s)/items(s) with the technical specifications reflected in the purchase order.", 1, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, false, false));
            rows.Add(new ContentCell("", 2, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), true, false, false));
            reports.AddRowContent(rows, 0.80);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("17.", 0, 10, false, false, ParagraphAlignment.Right, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, false, true));
            rows.Add(new ContentCell("If the supplier fails to deliver any or all the items within the period(s) specified in the purchase order, the UNIVERSITY shall, without prejudice to its other remedies under this agreement and other applicable laws, be entitled to liquidated damages, a sum equivalent to one-tenth of one percent " +
                "of the total amount of the undelivered items for each day of delay. Once the cumulative amount of the liquidated damages reaches ten percent (10%), the UNIVERSITY may rescind or terminate the agreement, without prejudice to other courses of action and remedies available under the circumstances. (IRR-A Section 68, Annex \"D\" of R.A. No. 9184)", 1, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, false, false));
            rows.Add(new ContentCell("", 2, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), true, false, false));
            reports.AddRowContent(rows, 1.20);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("18.", 0, 10, false, false, ParagraphAlignment.Right, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, false, true));
            rows.Add(new ContentCell("The UNIVERSITY shall terminate the contract for default when any of the following conditions attend its implementation:", 1, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, false, false));
            rows.Add(new ContentCell("", 2, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), true, false, false));
            reports.AddRowContent(rows, 0.40);

            columns = new List<ContentColumn>();
            columns.Add(new ContentColumn(0.75));
            columns.Add(new ContentColumn(0.25));
            columns.Add(new ContentColumn(5.30));
            columns.Add(new ContentColumn(0.20));
            reports.AddTable(columns, false);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("", 0, 10, false, false, ParagraphAlignment.Right, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, false, true));
            rows.Add(new ContentCell("a.", 1, 10, false, false, ParagraphAlignment.Right, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, false, true));
            rows.Add(new ContentCell("There being no force majeure, the supplier fails to deliver any or all of the goods within the period(s) specified in the contract, or within any extension thereof granted by the UNIVERSITY pursuant to a request made by the supplier prior to the delay, and such failure amounts to at least ten percent (10%) of the contract price;", 2, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, false, false));
            rows.Add(new ContentCell("", 3, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), true, false, false));
            reports.AddRowContent(rows, 0.80);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("", 0, 10, false, false, ParagraphAlignment.Right, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, false, true));
            rows.Add(new ContentCell("b.", 1, 10, false, false, ParagraphAlignment.Right, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, false, true));
            rows.Add(new ContentCell("As a result of force majeure, the supplier is unable to deliver or perform any or all of the goods or services, amounting to at least ten percent (10%) of the contract price, for a period of not less than sixty (60) calendar days after the receipt of the notice from the UNIVERSITY stating that the circumstance of force majeure is deemed to have ceased;", 2, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, false, false));
            rows.Add(new ContentCell("", 3, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), true, false, false));
            reports.AddRowContent(rows, 1.00);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("", 0, 10, false, false, ParagraphAlignment.Right, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, false, true));
            rows.Add(new ContentCell("c.", 1, 10, false, false, ParagraphAlignment.Right, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, false, true));
            rows.Add(new ContentCell("The supplier fails to perform any other obligation(s) under the contract; or", 2, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, false, false));
            rows.Add(new ContentCell("", 3, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), true, false, false));
            reports.AddRowContent(rows, 0.30);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("", 0, 10, false, false, ParagraphAlignment.Right, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, false, true));
            rows.Add(new ContentCell("d.", 1, 10, false, false, ParagraphAlignment.Right, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, false, true));
            rows.Add(new ContentCell("The supplier, in the judgement of the UNIVERSITY, has engaged in corrupt, fraudulent, collusive or coercive practices in competing for or in executing the contract. (Paragraph III, Annex \"I\" of the 2016 IRR of R.A. No. 9184)", 2, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, false, false));
            rows.Add(new ContentCell("", 3, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), true, false, false));
            reports.AddRowContent(rows, 0.60);

            columns = new List<ContentColumn>();
            columns.Add(new ContentColumn(0.40));
            columns.Add(new ContentColumn(5.90));
            columns.Add(new ContentColumn(0.20));
            reports.AddTable(columns, false);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("19.", 0, 10, false, false, ParagraphAlignment.Right, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, false, true));
            rows.Add(new ContentCell("The UNIVERSITY reserves the right to impose administrative penalties upon the supplier when he commits any of the acts which constitute as an offense or violation under Section 69, Article XXIII of the Republic Act No. 9184. Likewise, in addition to the foregoing penalties, the UNIVERSITY reserves its right to blacklist the supplier for offenses or violations committed during competitive bidding and contract implementation, pursuant to the Uniform Guidelines for Blacklisting of Manufacturers, Suppliers, Distributors, and Consultants. (Appendix 17 of R.A. No. 9184)", 1, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, false, false));
            rows.Add(new ContentCell("", 2, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), true, false, false));
            reports.AddRowContent(rows, 1.20);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("20.", 0, 10, false, false, ParagraphAlignment.Right, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, false, true));
            rows.Add(new ContentCell("Any and all disputes arising from this Contract shall be amicably settled by the Parties. For unsettled disputes, the same shall be submitted to arbitration in accordance with the provisions of Republic Act No. 876, otherwise known as the \"Arbitration Law\" and the provisions of Republic Act No. 9285 otherwise known as the \"Alternative Dispute Resolution of 2004\". (Section 59 of the 2016 RIRR of R.A. No. 9184). The arbitral award shall be final and binding upon the parties and may be enforced before a court of competent jurisdiction.", 1, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, false, false));
            rows.Add(new ContentCell("", 2, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), true, false, false));
            reports.AddRowContent(rows, 1.00);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("\n", 0, 10, false, false, ParagraphAlignment.Right, VerticalAlignment.Top, 0, 0, true, new Color(0, 0, 0, 0), false, false, true));
            rows.Add(new ContentCell("\n", 1, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, true, new Color(0, 0, 0, 0), false, false, false));
            rows.Add(new ContentCell("\n", 2, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, true, new Color(0, 0, 0, 0), true, false, false));
            reports.AddRowContent(rows, 0.00);

            reports.InsertPageBreak();

            columns = new List<ContentColumn>();
            columns.Add(new ContentColumn(2.00));
            columns.Add(new ContentColumn(0.20));
            columns.Add(new ContentColumn(4.20));
            reports.AddTable(columns, false);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("Solicitation No.", 0, 10, true, false, ParagraphAlignment.Left, VerticalAlignment.Bottom));
            rows.Add(new ContentCell(" : ", 1, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Bottom));
            rows.Add(new ContentCell(rfq.SolicitationNo, 2, 10, false, true, ParagraphAlignment.Left, VerticalAlignment.Bottom, 0, 0, true));
            reports.AddRowContent(rows, 0.25);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("Project Identification No. ", 0, 10, true, false, ParagraphAlignment.Left, VerticalAlignment.Bottom));
            rows.Add(new ContentCell(" : ", 1, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Bottom));
            rows.Add(new ContentCell(rfq.ContractCode, 2, 10, false, true, ParagraphAlignment.Left, VerticalAlignment.Bottom, 0, 0, true));
            reports.AddRowContent(rows, 0.25);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("Name of Project", 0, 10, true, false, ParagraphAlignment.Left, VerticalAlignment.Bottom));
            rows.Add(new ContentCell(" : ", 1, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Bottom));
            rows.Add(new ContentCell(rfq.ContractName, 2, 8, false, true, ParagraphAlignment.Left, VerticalAlignment.Bottom, 0, 0, true));
            reports.AddRowContent(rows, 0.25);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("Location of the Project", 0, 10, true, false, ParagraphAlignment.Left, VerticalAlignment.Bottom));
            rows.Add(new ContentCell(" : ", 1, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Bottom));
            rows.Add(new ContentCell(agencyDetails.Address, 2, 10, false, true, ParagraphAlignment.Left, VerticalAlignment.Bottom, 0, 0, true));
            reports.AddRowContent(rows, 0.25);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("Date", 0, 10, true, false, ParagraphAlignment.Left, VerticalAlignment.Bottom));
            rows.Add(new ContentCell(" : ", 1, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Bottom));
            rows.Add(new ContentCell(rfq.Date.ToString("dd MMMM yyyy"), 2, 10, false, true, ParagraphAlignment.Left, VerticalAlignment.Bottom, 0, 0, true));
            reports.AddRowContent(rows, 0.25);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("Quotation No.", 0, 10, true, false, ParagraphAlignment.Left, VerticalAlignment.Bottom));
            rows.Add(new ContentCell(" : ", 1, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Bottom));
            rows.Add(new ContentCell(rfq.QuotationNo, 2, 10, false, true, ParagraphAlignment.Left, VerticalAlignment.Bottom, 0, 0, true));
            reports.AddRowContent(rows, 0.25);

            reports.AddNewLine();
            reports.AddNewLine();

            columns = new List<ContentColumn>();
            columns.Add(new ContentColumn(2.50));
            columns.Add(new ContentColumn(4.00));
            reports.AddTable(columns, true);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("Name of Company", 0, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0));
            rows.Add(new ContentCell("", 1, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0));
            reports.AddRowContent(rows, 0.30);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("Address", 0, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0));
            rows.Add(new ContentCell("", 1, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0));
            reports.AddRowContent(rows, 0.30);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("TIN", 0, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0));
            rows.Add(new ContentCell("", 1, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0));
            reports.AddRowContent(rows, 0.30);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("PhilGEPS Registration Number", 0, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0));
            rows.Add(new ContentCell("", 1, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0));
            reports.AddRowContent(rows, 0.30);

            reports.AddNewLine();

            columns = new List<ContentColumn>();
            columns.Add(new ContentColumn(6.50));
            reports.AddTable(columns, false);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("\n", 0, 8, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), true, true, true));
            reports.AddRowContent(rows, 0.00);

            columns = new List<ContentColumn>();
            columns.Add(new ContentColumn(2.25));
            columns.Add(new ContentColumn(2.00));
            columns.Add(new ContentColumn(2.25));
            reports.AddTable(columns, false);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("", 0, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), false, false, true));
            rows.Add(new ContentCell("INSTRUCTIONS", 1, 10, true, false, ParagraphAlignment.Center, VerticalAlignment.Bottom, 0, 0, false, new Color(0, 0, 0, 0), false, false, false));
            rows.Add(new ContentCell("", 2, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), true, false, false));
            reports.AddRowContent(rows, 0.00);

            columns = new List<ContentColumn>();
            columns.Add(new ContentColumn(0.40));
            columns.Add(new ContentColumn(5.90));
            columns.Add(new ContentColumn(0.20));
            reports.AddTable(columns, false);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("\n", 0, 10, false, false, ParagraphAlignment.Right, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, false, true));
            rows.Add(new ContentCell("\n", 1, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, false, false));
            rows.Add(new ContentCell("\n", 2, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), true, false, false));
            reports.AddRowContent(rows, 0.00);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("1.", 0, 10, false, false, ParagraphAlignment.Right, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, true, true));
            rows.Add(new ContentCell("Accoplish this RFQ correctly, accurately and completely.", 1, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, true, false));
            rows.Add(new ContentCell("", 2, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), true, true, false));
            reports.AddRowContent(rows, 0.00);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("2.", 0, 10, false, false, ParagraphAlignment.Right, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, true, true));
            rows.Add(new ContentCell("Do not alter the contents of this form in any way.", 1, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, true, false));
            rows.Add(new ContentCell("", 2, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), true, true, false));
            reports.AddRowContent(rows, 0.00);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("3.", 0, 10, false, false, ParagraphAlignment.Right, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, true, true));
            rows.Add(new ContentCell("All technical specifications are mandatory. Failure to comply with any of the mandatory requirements will disqualify your quotation.", 1, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, true, false));
            rows.Add(new ContentCell("", 2, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), true, true, false));
            reports.AddRowContent(rows, 0.00);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("4.", 0, 10, false, false, ParagraphAlignment.Right, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, true, true));
            rows.Add(new ContentCell("Failure to follow these instructions will disqualify your entire quotation.", 1, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, true, false));
            rows.Add(new ContentCell("", 2, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), true, true, false));
            reports.AddRowContent(rows, 0.00);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("5.", 0, 10, false, false, ParagraphAlignment.Right, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, true, true));
            rows.Add(new ContentCell("Suppliers shall submit the original brochures showing certifications of the products being offered", 1, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, true, false));
            rows.Add(new ContentCell("", 2, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), true, true, false));
            reports.AddRowContent(rows, 0.00);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("\n", 0, 10, false, false, ParagraphAlignment.Right, VerticalAlignment.Top, 0, 0, true, new Color(0, 0, 0, 0), false, false, true));
            rows.Add(new ContentCell("\n", 1, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, true, new Color(0, 0, 0, 0), false, false, false));
            rows.Add(new ContentCell("\n", 2, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, true, new Color(0, 0, 0, 0), true, false, false));
            reports.AddRowContent(rows, 0.00);

            reports.AddNewLine();

            columns = new List<ContentColumn>();
            columns.Add(new ContentColumn(6.50));
            reports.AddTable(columns, false);

            reports.AddFormattedRowContent(new ContentCell(new TextWithFormat[]
            {
               new TextWithFormat("Sir/Madam:\n\n", true, false, 10),
            }, MigraDoc.DocumentObjectModel.ParagraphAlignment.Justify), 0.00);

            reports.AddFormattedRowContent(new ContentCell(new TextWithFormat[]
            {
               new TextWithFormat("After having carefully reading and accepting the Terms and Conditions in this Request for Quotation, I/We hereby provide my/our quotation for the item/s as follows:", false, false, 10),
            }, new Unit(0.50, UnitType.Inch), MigraDoc.DocumentObjectModel.ParagraphAlignment.Justify), 0.00);

            reports.AddNewLine();

            columns = new List<ContentColumn>();
            columns.Add(new ContentColumn(2.00));
            columns.Add(new ContentColumn(2.50));
            columns.Add(new ContentColumn(2.00));
            reports.AddTable(columns, false);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("", 0, 5, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), false, true, true));
            rows.Add(new ContentCell("", 1, 5, true, false, ParagraphAlignment.Center, VerticalAlignment.Bottom, 0, 0, false, new Color(0, 0, 0, 0), false, true, false));
            rows.Add(new ContentCell("", 2, 5, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), true, true, false));
            reports.AddRowContent(rows, 0.00);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("", 0, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), false, false, true));
            rows.Add(new ContentCell(new TextWithFormat[]
            {
                new TextWithFormat("TECHNICAL SPECIFICATION", true, false, 10, Underline.Single),
            }, 1, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, false, false));
            rows.Add(new ContentCell("", 2, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), true, false, false));
            reports.AddRowContent(rows, 0.00);

            columns = new List<ContentColumn>();
            columns.Add(new ContentColumn(0.40));
            columns.Add(new ContentColumn(5.90));
            columns.Add(new ContentColumn(0.20));
            reports.AddTable(columns, false);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("\n", 0, 10, false, false, ParagraphAlignment.Right, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, false, true));
            rows.Add(new ContentCell("\n", 1, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, false, false));
            rows.Add(new ContentCell("\n", 2, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), true, false, false));
            reports.AddRowContent(rows, 0.00);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("1.", 0, 10, false, false, ParagraphAlignment.Right, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, false, true));
            rows.Add(new ContentCell(new TextWithFormat[]
            {
               new TextWithFormat("Please quote your ", false, false, 10),
               new TextWithFormat("best offer", true, false, 10, Underline.Single),
               new TextWithFormat(" the item/s below. Please do not leave any blank items. Indicate ", false, false, 10),
               new TextWithFormat("\"0\"", true, false, 10),
               new TextWithFormat(" if the item/s is/are being offered for free.", false, false, 10),
            }, 1, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, false, false));
            rows.Add(new ContentCell("", 2, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), true, false, false));
            reports.AddRowContent(rows, 0.25);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("2.", 0, 10, false, false, ParagraphAlignment.Right, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, true, true));
            rows.Add(new ContentCell("The quote prices should be inclusive of all costs and applicable taxes.", 1, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, true, false));
            rows.Add(new ContentCell("", 2, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), true, true, false));
            reports.AddRowContent(rows, 0.25);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("3.", 0, 10, false, false, ParagraphAlignment.Right, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, true, true));
            rows.Add(new ContentCell("If the space below is not sufficient, you may attach a separate sheet and index it as \"Annex A\". It shall be submitted aling with this document.", 1, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, true, false));
            rows.Add(new ContentCell("", 2, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), true, true, false));
            reports.AddRowContent(rows, 0.25);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("\n", 0, 10, false, false, ParagraphAlignment.Right, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, true, true));
            rows.Add(new ContentCell("\n", 1, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, true, false));
            rows.Add(new ContentCell("\n", 2, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), true, true, false));
            reports.AddRowContent(rows, 0.25);

            columns = new List<ContentColumn>();
            columns.Add(new ContentColumn(0.75));
            columns.Add(new ContentColumn(0.75));
            columns.Add(new ContentColumn(3.00));
            columns.Add(new ContentColumn(0.75));
            columns.Add(new ContentColumn(1.25));
            reports.AddTable(columns, true);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("Item No.", 0, 10, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
            rows.Add(new ContentCell("Unit", 1, 10, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
            rows.Add(new ContentCell("Description", 2, 10, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
            rows.Add(new ContentCell("Quantity", 3, 10, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
            rows.Add(new ContentCell("Unit Price", 4, 10, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
            reports.AddRowContent(rows, 0.30);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell(rfq.ContractName + " (" + rfq.ContractCode + ")", 0, 10, true, true, ParagraphAlignment.Center, VerticalAlignment.Center, 4));
            reports.AddRowContent(rows, 0.50);

            foreach (var item in rfq.Details)
            {
                rows = new List<ContentCell>();
                rows.Add(new ContentCell("", 0, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Top));
                rows.Add(new ContentCell(item.UnitOfMeasure, 1, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Top));
                rows.Add(new ContentCell(new TextWithFormat[]
                {
                   new TextWithFormat(item.ItemFullName.ToUpper() + "\n\n", true, false, 10, Underline.Single),
                   new TextWithFormat(item.ItemSpecifications, false, true, 7),
                }, 2, ParagraphAlignment.Left, VerticalAlignment.Top));
                rows.Add(new ContentCell(item.Quantity.ToString(), 3, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Top));
                rows.Add(new ContentCell(new TextWithFormat[]
                {
                   new TextWithFormat("_______", false, false, 11),
                   new TextWithFormat(" / " + item.UnitOfMeasure, false, true, 8),
                }, 4, ParagraphAlignment.Center, VerticalAlignment.Top));
                reports.AddRowContent(rows, 0.30);
            }

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("***** NOTHING FOLLOWS ******", 0, 10, true, true, ParagraphAlignment.Center, VerticalAlignment.Center, 4));
            reports.AddRowContent(rows, 0.30);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("TOTAL COST", 0, 10, true, false, ParagraphAlignment.Right, VerticalAlignment.Center, 2));
            rows.Add(new ContentCell("", 3, 10, true, true, ParagraphAlignment.Center, VerticalAlignment.Center, 1));
            reports.AddRowContent(rows, 0.30);

            reports.InsertPageBreak();

            columns = new List<ContentColumn>();
            columns.Add(new ContentColumn(2.00));
            columns.Add(new ContentColumn(2.50));
            columns.Add(new ContentColumn(2.00));
            reports.AddTable(columns, false);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("", 0, 5, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), false, true, true));
            rows.Add(new ContentCell("", 1, 5, true, false, ParagraphAlignment.Center, VerticalAlignment.Bottom, 0, 0, false, new Color(0, 0, 0, 0), false, true, false));
            rows.Add(new ContentCell("", 2, 5, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), true, true, false));
            reports.AddRowContent(rows, 0.00);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("", 0, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), false, false, true));
            rows.Add(new ContentCell(new TextWithFormat[]
            {
                new TextWithFormat("SCHEDULE OF REQUIREMENTS", true, false, 10, Underline.Single),
            }, 1, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, false, false));
            rows.Add(new ContentCell("", 2, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), true, false, false));
            reports.AddRowContent(rows, 0.00);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("", 0, 5, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), false, false, true));
            rows.Add(new ContentCell("", 1, 5, true, false, ParagraphAlignment.Center, VerticalAlignment.Bottom, 0, 0, false, new Color(0, 0, 0, 0), false, false, false));
            rows.Add(new ContentCell("", 2, 5, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), true, false, false));
            reports.AddRowContent(rows, 0.00);

            columns = new List<ContentColumn>();
            columns.Add(new ContentColumn(0.40));
            columns.Add(new ContentColumn(5.90));
            columns.Add(new ContentColumn(0.20));
            reports.AddTable(columns, false);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("1.", 0, 10, false, false, ParagraphAlignment.Right, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, false, true));
            rows.Add(new ContentCell(new TextWithFormat[]
            {
               new TextWithFormat("The delivery schedule stipulates hereafter the delivery date to the project.", false, false, 10),
            }, 1, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, false, false));
            rows.Add(new ContentCell("", 2, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), true, false, false));
            reports.AddRowContent(rows, 0.25);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("2.", 0, 10, false, false, ParagraphAlignment.Right, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, true, true));
            rows.Add(new ContentCell("If the space below is not sufficient, you may attach a separate sheet and index it as \"Anex B\". It shall be submitted along with this document.", 1, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, true, false));
            rows.Add(new ContentCell("", 2, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), true, true, false));
            reports.AddRowContent(rows, 0.25);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("\n", 0, 10, false, false, ParagraphAlignment.Right, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, true, true));
            rows.Add(new ContentCell("\n", 1, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, true, false));
            rows.Add(new ContentCell("\n", 2, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), true, true, false));
            reports.AddRowContent(rows, 0.25);

            columns = new List<ContentColumn>();
            columns.Add(new ContentColumn(0.75));
            columns.Add(new ContentColumn(3.50));
            columns.Add(new ContentColumn(0.75));
            columns.Add(new ContentColumn(1.50));
            reports.AddTable(columns, true);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("Item No.", 0, 10, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
            rows.Add(new ContentCell("Description", 1, 10, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
            rows.Add(new ContentCell("Total Quantity", 2, 10, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
            rows.Add(new ContentCell("Delivery Period", 3, 10, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
            reports.AddRowContent(rows, 0.30);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("", 0, 10, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
            rows.Add(new ContentCell("", 1, 10, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
            rows.Add(new ContentCell("", 2, 10, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
            rows.Add(new ContentCell("", 3, 10, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
            reports.AddRowContent(rows, 1.50);

            reports.AddNewLine();
            reports.AddNewLine();

            columns = new List<ContentColumn>();
            columns.Add(new ContentColumn(3.00));
            columns.Add(new ContentColumn(3.50));
            reports.AddTable(columns, true);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell(new TextWithFormat[]
            {
                new TextWithFormat("FINANCIAL OFFER", true, false, 10, Underline.Single),
            }, 0, ParagraphAlignment.Center, VerticalAlignment.Center, 1));
            reports.AddRowContent(rows, 0.50);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("Approved Budget for the Contract (ABC)", 0, 10, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
            rows.Add(new ContentCell("Total Offered Quotation", 1, 10, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
            reports.AddRowContent(rows, 0.50);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("", 0, 10, false, false, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 1));
            rows.Add(new ContentCell("In words:", 1, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Top));
            reports.AddRowContent(rows, 0.75);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("In Figures:", 1, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Top));
            reports.AddRowContent(rows, 0.75);

            return reports.GenerateReport();
        }
        public MemoryStream GenerateNoticeOfAward(string ContractCode, string LogoPath, string UserEmail)
        {
            var agencyDetails = db.AgencyDetails.FirstOrDefault();
            var hope = hris.GetDepartmentDetails(agencyDetails.HOPEReference);
            var procurementOffice = hris.GetDepartmentDetails(agencyDetails.ProcurementOfficeReference);
            var bidsAndAwards = hris.GetDepartmentDetails(agencyDetails.BACOfficeReference);
            var bacsecHead = db.BACSecretariat.Where(d => d.Membership == BACMembership.BACSECHead && d.PurgeFlag == false).FirstOrDefault();
            var bacChair = db.BACSecretariat.Where(d => d.Membership == BACMembership.BACChairman && d.PurgeFlag == false).FirstOrDefault();
            var noticeOfAward = contractDAL.NoticeOfAwardSetup(ContractCode);
            Reports reports = new Reports();
            reports.ReportFilename = "Notice of Award - " + ContractCode;

            foreach(var noa in noticeOfAward)
            {
                reports.CreateDocument(8.50, 13.00, Orientation.Portrait, 1.00);
                reports.AddDoubleColumnHeader(LogoPath, false, 1.25);
                reports.AddColumnHeader(
                    new HeaderLine { Content = "Republic of the Philippines", Bold = false, Italic = false, FontSize = 8, ParagraphAlignment = ParagraphAlignment.Left },
                    new HeaderLine { Content = "", Bold = false, Italic = false, FontSize = 6, ParagraphAlignment = ParagraphAlignment.Left }
                );
                reports.AddColumnHeader(
                    new HeaderLine { Content = agencyDetails.AgencyName.ToUpper(), Bold = true, Italic = false, FontSize = 12, ParagraphAlignment = ParagraphAlignment.Left },
                    new HeaderLine { Content = "", Bold = false, Italic = false, FontSize = 7, ParagraphAlignment = ParagraphAlignment.Left }
                );
                reports.AddColumnHeader(
                    new HeaderLine { Content = agencyDetails.Address, Bold = false, Italic = false, FontSize = 10, ParagraphAlignment = ParagraphAlignment.Left },
                    new HeaderLine { Content = "", Bold = false, Italic = false, FontSize = 8, ParagraphAlignment = ParagraphAlignment.Left }
                );
                reports.AddColumnHeader(
                    new HeaderLine { Content = "THE COUNTRY'S FIRST POLYTECHNIC UNIVERSITY", Bold = false, Italic = false, FontSize = 10, ParagraphAlignment = ParagraphAlignment.Left },
                    new HeaderLine { Content = "", Bold = true, Italic = false, FontSize = 5, ParagraphAlignment = ParagraphAlignment.Left }
                );

                var columns = new List<ContentColumn>();
                columns.Add(new ContentColumn(6.50));
                reports.AddTable(columns, false);

                reports.AddFormattedRowContent(new ContentCell(new TextWithFormat[]
                {
               new TextWithFormat("NOTICE OF AWARD", true, false, 16),
                }, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center), 0.00);

                reports.AddNewLine();

                columns = new List<ContentColumn>();
                columns.Add(new ContentColumn(6.50));
                reports.AddTable(columns, false);

                var rows = new List<ContentCell>();
                rows.Add(new ContentCell(DateTime.Now.ToString("dd MMMM yyyy"), 0, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center));
                reports.AddRowContent(rows, 0.18);

                reports.AddNewLine();
                reports.AddNewLine();

                columns = new List<ContentColumn>();
                columns.Add(new ContentColumn(6.50));
                reports.AddTable(columns, false);

                rows = new List<ContentCell>();
                rows.Add(new ContentCell(noa.ContactPerson.ToUpper(), 0, 10, true, false, ParagraphAlignment.Left, VerticalAlignment.Center));
                reports.AddRowContent(rows, 0.18);

                rows = new List<ContentCell>();
                rows.Add(new ContentCell((noa.ContactPersonDesignation == string.Empty || noa.ContactPersonDesignation == null ? "Authorized and Designated Representative" : noa.ContactPersonDesignation), 0, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center));
                reports.AddRowContent(rows, 0.18);

                rows = new List<ContentCell>();
                rows.Add(new ContentCell(noa.Supplier.ToUpper(), 0, 10, true, false, ParagraphAlignment.Left, VerticalAlignment.Center));
                reports.AddRowContent(rows, 0.18);

                rows = new List<ContentCell>();
                rows.Add(new ContentCell(noa.Address, 0, 9.5, false, false, ParagraphAlignment.Left, VerticalAlignment.Center));
                reports.AddRowContent(rows, 0.18);

                if (noa.City != null || noa.City != string.Empty)
                {
                    rows = new List<ContentCell>();
                    rows.Add(new ContentCell(noa.City, 0, 9.5, false, false, ParagraphAlignment.Left, VerticalAlignment.Center));
                    reports.AddRowContent(rows, 0.18);
                }

                if (noa.State != null || noa.State != string.Empty)
                {
                    rows = new List<ContentCell>();
                    rows.Add(new ContentCell(noa.State + (noa.PostalCode == string.Empty ? string.Empty : ", " + noa.PostalCode), 0, 9.5, false, false, ParagraphAlignment.Left, VerticalAlignment.Center));
                    reports.AddRowContent(rows, 0.18);
                }

                reports.AddNewLine();
                reports.AddNewLine();

                columns = new List<ContentColumn>();
                columns.Add(new ContentColumn(6.50));
                reports.AddTable(columns, false);

                reports.AddFormattedRowContent(new ContentCell(new TextWithFormat[]
                {
                new TextWithFormat("Dear ", false, true, 10),
                new TextWithFormat(noa.ContactPerson, false, true, 10),
                new TextWithFormat(":", false, true, 10),
                }, MigraDoc.DocumentObjectModel.ParagraphAlignment.Left), 0.18);

                reports.AddNewLine();
                reports.AddNewLine();

                columns = new List<ContentColumn>();
                columns.Add(new ContentColumn(6.50));
                reports.AddTable(columns, false);

                rows = new List<ContentCell>();
                rows.Add(new ContentCell(new TextWithFormat[]
                {
                new TextWithFormat("This serves as notice that the PUP Bids and Awards Committee (PUP BAC), has recommended the award of the ", false, false, 10),
                new TextWithFormat(noa.ContractName, true, true, 10),
                new TextWithFormat(" through " + noa.ModeOfProcurement + " in the amount of ", false, false, 10),
                new TextWithFormat(noa.ContractPriceWords + " (" + noa.ContractPrice.ToString("C", new System.Globalization.CultureInfo("en-ph")) + ")", true, true, 10),
                new TextWithFormat(" to " + noa.Supplier + ".", false, false, 10)
                }, 0, ParagraphAlignment.Justify, VerticalAlignment.Top));
                reports.AddRowContent(rows, 0.25);

                reports.AddNewLine();

                columns = new List<ContentColumn>();
                columns.Add(new ContentColumn(6.50));
                reports.AddTable(columns, false);

                rows = new List<ContentCell>();
                rows.Add(new ContentCell(new TextWithFormat[]
                {
                new TextWithFormat("You are therefore advised within ten (10) calendar days from the receipt of this Notice of Award to formally enter into contract with us. Failure to enter into the said contract  shall constitute a ground for cancellation of this award.", false, false, 10),
                }, 0, ParagraphAlignment.Justify, VerticalAlignment.Top));
                reports.AddRowContent(rows, 0.25);

                reports.AddNewLine();

                columns = new List<ContentColumn>();
                columns.Add(new ContentColumn(6.50));
                reports.AddTable(columns, false);

                rows = new List<ContentCell>();
                rows.Add(new ContentCell(new TextWithFormat[]
                {
                new TextWithFormat("Kindly signify your conformity by signing on the space herein provided.", false, false, 10),
                }, 0, ParagraphAlignment.Justify, VerticalAlignment.Top));
                reports.AddRowContent(rows, 0.25);

                reports.AddNewLine();

                columns = new List<ContentColumn>();
                columns.Add(new ContentColumn(6.50));
                reports.AddTable(columns, false);

                rows = new List<ContentCell>();
                rows.Add(new ContentCell(new TextWithFormat[]
                {
                new TextWithFormat("Thank you.", false, false, 10),
                }, 0, ParagraphAlignment.Justify, VerticalAlignment.Top));
                reports.AddRowContent(rows, 0.25);

                reports.AddNewLine();
                reports.AddNewLine();

                columns = new List<ContentColumn>();
                columns.Add(new ContentColumn(6.50));
                reports.AddTable(columns, false);

                rows = new List<ContentCell>();
                rows.Add(new ContentCell(new TextWithFormat[]
                {
                new TextWithFormat("Very truly yours,", false, false, 10),
                }, 0, ParagraphAlignment.Justify, VerticalAlignment.Top));
                reports.AddRowContent(rows, 0.25);

                reports.AddNewLine();
                reports.AddNewLine();
                reports.AddNewLine();

                columns = new List<ContentColumn>();
                columns.Add(new ContentColumn(6.50));
                reports.AddTable(columns, false);

                rows = new List<ContentCell>();
                rows.Add(new ContentCell(new TextWithFormat[]
                {
                new TextWithFormat(hope.SectorHead, true, false, 10),
                }, 0, ParagraphAlignment.Justify, VerticalAlignment.Top));
                reports.AddRowContent(rows, 0.20);

                rows = new List<ContentCell>();
                rows.Add(new ContentCell(new TextWithFormat[]
                {
                new TextWithFormat(hope.SectorHeadDesignation, false, false, 10),
                }, 0, ParagraphAlignment.Justify, VerticalAlignment.Top));
                reports.AddRowContent(rows, 0.25);

                reports.AddNewLine();
                reports.AddNewLine();
                reports.AddNewLine();
                reports.AddNewLine();

                columns = new List<ContentColumn>();
                columns.Add(new ContentColumn(6.50));
                reports.AddTable(columns, false);

                rows = new List<ContentCell>();
                rows.Add(new ContentCell(new TextWithFormat[]
                {
                new TextWithFormat("Conforme:", false, false, 10),
                }, 0, ParagraphAlignment.Justify, VerticalAlignment.Top));
                reports.AddRowContent(rows, 0.25);

                reports.AddNewLine();
                reports.AddNewLine();
                reports.AddNewLine();

                columns = new List<ContentColumn>();
                columns.Add(new ContentColumn(6.50));
                reports.AddTable(columns, false);

                rows = new List<ContentCell>();
                rows.Add(new ContentCell(noa.ContactPerson.ToUpper(), 0, 10, true, false, ParagraphAlignment.Left, VerticalAlignment.Center));
                reports.AddRowContent(rows, 0.18);

                rows = new List<ContentCell>();
                rows.Add(new ContentCell((noa.ContactPersonDesignation == string.Empty || noa.ContactPersonDesignation == null ? "Authorized and Designated Representative" : noa.ContactPersonDesignation), 0, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center));
                reports.AddRowContent(rows, 0.18);

                rows = new List<ContentCell>();
                rows.Add(new ContentCell("Date: __________________", 0, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center));
                reports.AddRowContent(rows, 0.18);
            }

            return reports.GenerateReport();
        }
        public MemoryStream GenerateContractAgreementFormGoods(string ContractCode, string LogoPath, string UserEmail)
        {
            var agencyDetails = db.AgencyDetails.FirstOrDefault();
            var hope = hris.GetDepartmentDetails(agencyDetails.HOPEReference);
            var procurementOffice = hris.GetDepartmentDetails(agencyDetails.ProcurementOfficeReference);
            var bidsAndAwards = hris.GetDepartmentDetails(agencyDetails.BACOfficeReference);
            var bacsecHead = db.BACSecretariat.Where(d => d.Membership == BACMembership.BACSECHead && d.PurgeFlag == false).FirstOrDefault();
            var bacChair = db.BACSecretariat.Where(d => d.Membership == BACMembership.BACChairman && d.PurgeFlag == false).FirstOrDefault();
            var noticeOfAward = contractDAL.NoticeOfAwardSetup(ContractCode);
            Reports reports = new Reports();
            reports.ReportFilename = "Contract Agreement - " + ContractCode;

            foreach(var noa in noticeOfAward)
            {
                reports.CreateDocument(8.50, 13.00, Orientation.Portrait, 0.50, 1.00);

                var columns = new List<ContentColumn>();
                columns.Add(new ContentColumn(6.50));
                reports.AddTable(columns, false);

                var rows = new List<ContentCell>();
                rows.Add(new ContentCell("Contract Agreement Form", 0, 14, true, false, ParagraphAlignment.Center, VerticalAlignment.Top, 0, 0, true));
                reports.AddRowContent(rows, 0.50);

                reports.AddNewLine();
                reports.AddNewLine();

                columns = new List<ContentColumn>();
                columns.Add(new ContentColumn(6.50));
                reports.AddTable(columns, false);

                rows = new List<ContentCell>();
                rows.Add(new ContentCell(new TextWithFormat[]
                {
                new TextWithFormat("\tTHIS AGREEMENT made the ________ day of _______________ 20_____ between ", false, false, 10),
                new TextWithFormat(agencyDetails.AgencyName, true, true, 10),
                new TextWithFormat(" (hereinafter called “the Entity”) of the one part and ", false, false, 10),
                new TextWithFormat(noa.Supplier, true, true, 10),
                new TextWithFormat(" of  ", false, false, 10),
                new TextWithFormat(noa.State, false, true, 10),
                new TextWithFormat(" (hereinafter called “the Supplier”) of the other part:", false, false, 10),
                }, 0, ParagraphAlignment.Justify, VerticalAlignment.Top));
                reports.AddRowContent(rows, 0.25);

                reports.AddNewLine();

                columns = new List<ContentColumn>();
                columns.Add(new ContentColumn(6.50));
                reports.AddTable(columns, false);

                rows = new List<ContentCell>();
                rows.Add(new ContentCell(new TextWithFormat[]
                {
                new TextWithFormat("\tWHEREAS the Entity invited Bids for certain goods and ancillary services, viz., ", false, false, 10),
                new TextWithFormat(noa.ContractName, false, true, 10),
                new TextWithFormat(" and has accepted a Bid by the Supplier for the supply of those goods and services in the sum of ", false, false, 10),
                new TextWithFormat(noa.ContractPriceWords + " (" + noa.ContractPrice.ToString("C", new System.Globalization.CultureInfo("en-ph")) + ")", false, true, 10),
                new TextWithFormat(" (hereinafter called “the Contract Price”).", false, false, 10),
                }, 0, ParagraphAlignment.Justify, VerticalAlignment.Top));
                reports.AddRowContent(rows, 0.25);

                reports.AddNewLine();
                reports.AddNewLine();

                columns = new List<ContentColumn>();
                columns.Add(new ContentColumn(6.50));
                reports.AddTable(columns, false);

                rows = new List<ContentCell>();
                rows.Add(new ContentCell(new TextWithFormat[]
                {
                new TextWithFormat("\tNOW THIS AGREEMENT WITNESSETH AS FOLLOWS:", false, false, 10),
                }, 0, ParagraphAlignment.Justify, VerticalAlignment.Top));
                reports.AddRowContent(rows, 0.25);

                reports.AddNewLine();

                columns = new List<ContentColumn>();
                columns.Add(new ContentColumn(1.00));
                columns.Add(new ContentColumn(5.50));
                reports.AddTable(columns, false);

                rows = new List<ContentCell>();
                rows.Add(new ContentCell("\t1. ", 0, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Top));
                rows.Add(new ContentCell(new TextWithFormat[]
                {
                new TextWithFormat("In this Agreement words and expressions shall have the same meanings as are respectively assigned to them in the Conditions of Contract referred to.", false, false, 10),
                }, 1, ParagraphAlignment.Justify, VerticalAlignment.Top));
                reports.AddRowContent(rows, 0.25);

                reports.AddNewLine();

                columns = new List<ContentColumn>();
                columns.Add(new ContentColumn(1.00));
                columns.Add(new ContentColumn(5.50));
                reports.AddTable(columns, false);

                rows = new List<ContentCell>();
                rows.Add(new ContentCell("\t2. ", 0, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Top));
                rows.Add(new ContentCell(new TextWithFormat[]
                {
                new TextWithFormat("The following documents shall be deemed to form and be read and construed as part of this Agreement, viz.:", false, false, 10),
                }, 1, ParagraphAlignment.Justify, VerticalAlignment.Top));
                reports.AddRowContent(rows, 0.25);

                reports.AddNewLine();

                columns = new List<ContentColumn>();
                columns.Add(new ContentColumn(1.50));
                columns.Add(new ContentColumn(5.00));
                reports.AddTable(columns, false);

                rows = new List<ContentCell>();
                rows.Add(new ContentCell("\t\t(a)", 0, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Top));
                rows.Add(new ContentCell(new TextWithFormat[]
                {
                new TextWithFormat("the Supplier’s Bid, including the Technical and Financial Proposals, and all other documents/statements submitted (e.g. bidder’s response to clarifications on the bid), including corrections to the bid resulting from the Procuring Entity’s bid evaluation;", false, false, 10),
                }, 1, ParagraphAlignment.Justify, VerticalAlignment.Top));
                reports.AddRowContent(rows, 0.25);

                reports.AddNewLine();

                columns = new List<ContentColumn>();
                columns.Add(new ContentColumn(1.50));
                columns.Add(new ContentColumn(5.00));
                reports.AddTable(columns, false);

                rows = new List<ContentCell>();
                rows.Add(new ContentCell("\t\t(b)", 0, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Top));
                rows.Add(new ContentCell(new TextWithFormat[]
                {
                new TextWithFormat("the Schedule of Requirements;", false, false, 10),
                }, 1, ParagraphAlignment.Justify, VerticalAlignment.Top));
                reports.AddRowContent(rows, 0.10);

                reports.AddNewLine();

                columns = new List<ContentColumn>();
                columns.Add(new ContentColumn(1.50));
                columns.Add(new ContentColumn(5.00));
                reports.AddTable(columns, false);

                rows = new List<ContentCell>();
                rows.Add(new ContentCell("\t\t(c)", 0, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Top));
                rows.Add(new ContentCell(new TextWithFormat[]
                {
                new TextWithFormat("the Technical Specifications;", false, false, 10),
                }, 1, ParagraphAlignment.Justify, VerticalAlignment.Top));
                reports.AddRowContent(rows, 0.10);

                reports.AddNewLine();

                columns = new List<ContentColumn>();
                columns.Add(new ContentColumn(1.50));
                columns.Add(new ContentColumn(5.00));
                reports.AddTable(columns, false);

                rows = new List<ContentCell>();
                rows.Add(new ContentCell("\t\t(d)", 0, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Top));
                rows.Add(new ContentCell(new TextWithFormat[]
                {
                new TextWithFormat("the General Conditions of Contract;", false, false, 10),
                }, 1, ParagraphAlignment.Justify, VerticalAlignment.Top));
                reports.AddRowContent(rows, 0.00);

                reports.AddNewLine();

                columns = new List<ContentColumn>();
                columns.Add(new ContentColumn(1.50));
                columns.Add(new ContentColumn(5.00));
                reports.AddTable(columns, false);

                rows = new List<ContentCell>();
                rows.Add(new ContentCell("\t\t(e)", 0, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Top));
                rows.Add(new ContentCell(new TextWithFormat[]
                {
                new TextWithFormat("the Special Conditions of Contract;", false, false, 10),
                }, 1, ParagraphAlignment.Justify, VerticalAlignment.Top));
                reports.AddRowContent(rows, 0.00);

                reports.AddNewLine();

                columns = new List<ContentColumn>();
                columns.Add(new ContentColumn(1.50));
                columns.Add(new ContentColumn(5.00));
                reports.AddTable(columns, false);

                rows = new List<ContentCell>();
                rows.Add(new ContentCell("\t\t(f)", 0, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Top));
                rows.Add(new ContentCell(new TextWithFormat[]
                {
                new TextWithFormat("the Performance Security; and", false, false, 10),
                }, 1, ParagraphAlignment.Justify, VerticalAlignment.Top));
                reports.AddRowContent(rows, 0.05);

                reports.AddNewLine();

                columns = new List<ContentColumn>();
                columns.Add(new ContentColumn(1.50));
                columns.Add(new ContentColumn(5.00));
                reports.AddTable(columns, false);

                rows = new List<ContentCell>();
                rows.Add(new ContentCell("\t\t(g)", 0, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Top));
                rows.Add(new ContentCell(new TextWithFormat[]
                {
                new TextWithFormat("the Entity’s Notice of Award.", false, false, 10),
                }, 1, ParagraphAlignment.Justify, VerticalAlignment.Top));
                reports.AddRowContent(rows, 0.00);

                reports.AddNewLine();

                columns = new List<ContentColumn>();
                columns.Add(new ContentColumn(1.00));
                columns.Add(new ContentColumn(5.50));
                reports.AddTable(columns, false);

                rows = new List<ContentCell>();
                rows.Add(new ContentCell("\t3. ", 0, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Top));
                rows.Add(new ContentCell(new TextWithFormat[]
                {
                new TextWithFormat("In consideration of the payments to be made by the Entity to the Supplier as hereinafter mentioned, the Supplier hereby covenants with the Entity to provide the goods and services and to remedy defects therein in conformity in all respects with the provisions of the Contract.", false, false, 10),
                }, 1, ParagraphAlignment.Justify, VerticalAlignment.Top));
                reports.AddRowContent(rows, 0.25);

                reports.AddNewLine();

                columns = new List<ContentColumn>();
                columns.Add(new ContentColumn(1.00));
                columns.Add(new ContentColumn(5.50));
                reports.AddTable(columns, false);

                rows = new List<ContentCell>();
                rows.Add(new ContentCell("\t4. ", 0, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Top));
                rows.Add(new ContentCell(new TextWithFormat[]
                {
                new TextWithFormat("The Entity hereby covenants to pay the Supplier in consideration of the provision of the goods and services and the remedying of defects therein, the Contract Price or such other sum as may become payable under the provisions of the contract at the time and in the manner prescribed by the contract.", false, false, 10),
                }, 1, ParagraphAlignment.Justify, VerticalAlignment.Top));
                reports.AddRowContent(rows, 0.25);

                reports.AddNewLine();
                reports.AddNewLine();

                columns = new List<ContentColumn>();
                columns.Add(new ContentColumn(6.50));
                reports.AddTable(columns, false);

                rows = new List<ContentCell>();
                rows.Add(new ContentCell(new TextWithFormat[]
                {
                new TextWithFormat("\tIN WITNESS whereof the parties hereto have caused this Agreement to be executed in accordance with the laws of the Republic of the Philippines on the day and year first above written.", false, false, 10),
                }, 0, ParagraphAlignment.Justify, VerticalAlignment.Top));
                reports.AddRowContent(rows, 0.25);

                reports.AddNewLine();
                reports.AddNewLine();
                reports.AddNewLine();

                columns = new List<ContentColumn>();
                columns.Add(new ContentColumn(6.50));
                reports.AddTable(columns, false);

                rows = new List<ContentCell>();
                rows.Add(new ContentCell(new TextWithFormat[]
                {
                new TextWithFormat("Signed, sealed, delivered by ___________________ the ______________________ (for the Entity)", false, false, 10),
                }, 0, ParagraphAlignment.Justify, VerticalAlignment.Top));
                reports.AddRowContent(rows, 0.25);

                reports.AddNewLine();

                columns = new List<ContentColumn>();
                columns.Add(new ContentColumn(6.50));
                reports.AddTable(columns, false);

                rows = new List<ContentCell>();
                rows.Add(new ContentCell(new TextWithFormat[]
                {
                new TextWithFormat("Signed, sealed, delivered by ___________________ the ______________________ (for the  Supplier)", false, false, 10),
                }, 0, ParagraphAlignment.Justify, VerticalAlignment.Top));
                reports.AddRowContent(rows, 0.25);
            }

            return reports.GenerateReport();
        }
        public MemoryStream GeneratePurchaseOrder(string ContractCode, string LogoPath, string UserEmail)
        {
            Reports reports = new Reports();
            var contracts = contractDAL.PurchaseOrderSetup(ContractCode);
            reports.ReportFilename = "Purchase Order - " + ContractCode;
            foreach (var contract in contracts)
            {
                reports.CreateDocument(8.50, 13.00, Orientation.Portrait, 0.25);

                var agencyDetails = db.AgencyDetails.FirstOrDefault();
                var recordCount = 10;
                var fetchCount = 0;
                var itemCount = contract.Details.Count;
                var pageCount = (contract.Details.Count + 9) / recordCount;

                for (int count = 1; count <= pageCount; count++)
                {
                    var columns = new List<ContentColumn>();
                    columns.Add(new ContentColumn(8.00));
                    reports.AddTable(columns, false);

                    var rows = new List<ContentCell>();
                    rows.Add(new ContentCell("Appendix 61", 0, 10, false, true, ParagraphAlignment.Right, VerticalAlignment.Center));
                    reports.AddRowContent(rows, 0.25);

                    columns = new List<ContentColumn>();
                    columns.Add(new ContentColumn(8.00));
                    reports.AddTable(columns, false);

                    reports.AddFormattedRowContent(new ContentCell(new TextWithFormat[]
                    {
                    new TextWithFormat("PURCHASE ORDER", true, false, 14),
                    }, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center), 0.00);

                    columns = new List<ContentColumn>();
                    columns.Add(new ContentColumn(1.00));
                    columns.Add(new ContentColumn(6.00));
                    columns.Add(new ContentColumn(1.00));
                    reports.AddTable(columns, false);

                    rows = new List<ContentCell>();
                    rows.Add(new ContentCell("", 0, 10, false, false, ParagraphAlignment.Center, VerticalAlignment.Top));
                    rows.Add(new ContentCell(new TextWithFormat[]
                    {
               new TextWithFormat(agencyDetails.AgencyName.ToUpper(), false, false, 12, Underline.Single)
                    }, 1, ParagraphAlignment.Center, VerticalAlignment.Center));
                    rows.Add(new ContentCell("", 2, 10, false, false, ParagraphAlignment.Center, VerticalAlignment.Top));
                    reports.AddRowContent(rows, 0.25);

                    reports.AddNewLine();

                    columns = new List<ContentColumn>();
                    columns.Add(new ContentColumn(1.50));
                    columns.Add(new ContentColumn(3.00));
                    columns.Add(new ContentColumn(1.50));
                    columns.Add(new ContentColumn(2.00));
                    reports.AddTable(columns, false);

                    rows = new List<ContentCell>();
                    rows.Add(new ContentCell("Supplier : ", 0, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), false, true, true));
                    rows.Add(new ContentCell(contract.SupplierName, 1, 10, true, false, ParagraphAlignment.Left, VerticalAlignment.Bottom, 0, 0, true, new Color(0, 0, 0, 0), true, true, false));
                    rows.Add(new ContentCell("P.O. No. : ", 2, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), false, true, false));
                    rows.Add(new ContentCell(contract.PurchaseOrderNumber, 3, 10, true, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, true, new Color(0, 0, 0, 0), true, true, false));
                    reports.AddRowContent(rows, 0.20);

                    rows = new List<ContentCell>();
                    rows.Add(new ContentCell("Address : ", 0, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), false, true, true));
                    rows.Add(new ContentCell(contract.SupplierAddress, 1, 8.75, true, false, ParagraphAlignment.Left, VerticalAlignment.Bottom, 0, 0, true, new Color(0, 0, 0, 0), true, true, false));
                    rows.Add(new ContentCell("Date : ", 2, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), false, true, false));
                    rows.Add(new ContentCell(contract.CreatedAt.ToString("dd MMMM yyyy"), 3, 10, true, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, true, new Color(0, 0, 0, 0), true, true, false));
                    reports.AddRowContent(rows, 0.20);

                    rows = new List<ContentCell>();
                    rows.Add(new ContentCell("TIN : ", 0, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), false, true, true));
                    rows.Add(new ContentCell(contract.SupplierTIN == null ? string.Empty : contract.SupplierTIN, 1, 10, true, false, ParagraphAlignment.Left, VerticalAlignment.Bottom, 0, 0, true, new Color(0, 0, 0, 0), true, true, false));
                    rows.Add(new ContentCell("Mode of Procurement : ", 2, 8.75, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), false, true, false));
                    rows.Add(new ContentCell(contract.ModeOfProcurement, 3, 8, true, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, true, new Color(0, 0, 0, 0), true, true, false));
                    reports.AddRowContent(rows, 0.20);

                    rows = new List<ContentCell>();
                    rows.Add(new ContentCell("", 0, 3, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), false, false, true));
                    rows.Add(new ContentCell("", 1, 3, true, false, ParagraphAlignment.Left, VerticalAlignment.Bottom, 0, 0, false, new Color(0, 0, 0, 0), true, false, false));
                    rows.Add(new ContentCell("", 2, 3, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), false, false, false));
                    rows.Add(new ContentCell("", 3, 3, true, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), true, false, false));
                    reports.AddRowContent(rows, 0.05);

                    columns = new List<ContentColumn>();
                    columns.Add(new ContentColumn(8.00));
                    reports.AddTable(columns, false);

                    rows = new List<ContentCell>();
                    rows.Add(new ContentCell("Gentlemen: ", 0, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), true, true, true));
                    reports.AddRowContent(rows, 0.20);

                    rows = new List<ContentCell>();
                    rows.Add(new ContentCell("\t\tPlease furnish this Office the following articles subject to the terms and conditions contained herein:", 0, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), true, true, true));
                    reports.AddRowContent(rows, 0.20);
                    rows = new List<ContentCell>();
                    rows.Add(new ContentCell("", 0, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), true, true, true));
                    reports.AddRowContent(rows, 0.05);

                    columns = new List<ContentColumn>();
                    columns.Add(new ContentColumn(1.50));
                    columns.Add(new ContentColumn(3.00));
                    columns.Add(new ContentColumn(1.50));
                    columns.Add(new ContentColumn(2.00));
                    reports.AddTable(columns, false);

                    rows = new List<ContentCell>();
                    rows.Add(new ContentCell("Place of Delivery : ", 0, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), false, true, true));
                    rows.Add(new ContentCell(contract.PlaceOfDelivery, 1, 8, false, false, ParagraphAlignment.Left, VerticalAlignment.Bottom, 0, 0, true, new Color(0, 0, 0, 0), true, true, false));
                    rows.Add(new ContentCell("Delivery Term : ", 2, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), false, true, false));
                    rows.Add(new ContentCell("", 3, 10, true, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, true, new Color(0, 0, 0, 0), true, true, false));
                    reports.AddRowContent(rows, 0.20);

                    rows = new List<ContentCell>();
                    rows.Add(new ContentCell("Date of Delivery : ", 0, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), false, false, true));
                    rows.Add(new ContentCell(Reports.DigitsToWords(contract.DeliveryPeriod) + " (" + contract.DeliveryPeriod.ToString() + ") calendar days upon receipt of N.T.P.", 1, 8, false, false, ParagraphAlignment.Left, VerticalAlignment.Bottom, 0, 0, true, new Color(0, 0, 0, 0), true, false, false));
                    rows.Add(new ContentCell("Payment Term : ", 2, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), false, false, false));
                    rows.Add(new ContentCell("", 3, 8, true, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, true, new Color(0, 0, 0, 0), true, false, false));
                    reports.AddRowContent(rows, 0.20);

                    rows = new List<ContentCell>();
                    rows.Add(new ContentCell("", 0, 3, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), false, false, true));
                    rows.Add(new ContentCell("", 1, 3, true, false, ParagraphAlignment.Left, VerticalAlignment.Bottom, 0, 0, false, new Color(0, 0, 0, 0), true, false, false));
                    rows.Add(new ContentCell("", 2, 3, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), false, false, false));
                    rows.Add(new ContentCell("", 3, 3, true, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), true, false, false));
                    reports.AddRowContent(rows, 0.05);

                    columns = new List<ContentColumn>();
                    columns.Add(new ContentColumn(1.50));
                    columns.Add(new ContentColumn(5.00));
                    columns.Add(new ContentColumn(1.50));
                    reports.AddTable(columns, false);

                    rows = new List<ContentCell>();
                    rows.Add(new ContentCell("", 0, 9, true, false, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), false, true, true));
                    rows.Add(new ContentCell(contract.ContractName + " (" + contract.ContractCode + ")", 1, 10, true, true, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), false, true, false));
                    rows.Add(new ContentCell("", 2, 9, false, false, ParagraphAlignment.Right, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), true, true, false));
                    reports.AddRowContent(rows, 0.50);

                    columns = new List<ContentColumn>();
                    columns.Add(new ContentColumn(1.00));
                    columns.Add(new ContentColumn(1.00));
                    columns.Add(new ContentColumn(2.50));
                    columns.Add(new ContentColumn(1.00));
                    columns.Add(new ContentColumn(1.00));
                    columns.Add(new ContentColumn(1.50));
                    reports.AddTable(columns, true);

                    rows = new List<ContentCell>();
                    rows.Add(new ContentCell("Stock/\nProperty No.", 0, 10, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                    rows.Add(new ContentCell("Unit", 1, 10, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                    rows.Add(new ContentCell("Description", 2, 10, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                    rows.Add(new ContentCell("Quantity", 3, 10, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                    rows.Add(new ContentCell("Unit Cost", 4, 10, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                    rows.Add(new ContentCell("Amount", 5, 10, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                    reports.AddRowContent(rows, 0.20);

                    foreach (var item in contract.Details.Skip(fetchCount).Take(recordCount).ToList())
                    {
                        rows = new List<ContentCell>();
                        rows.Add(new ContentCell("", 0, 9, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                        rows.Add(new ContentCell(item.UnitOfMeasure, 1, 9, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                        rows.Add(new ContentCell(new TextWithFormat[]
                        {
                        new TextWithFormat(item.ItemFullName.ToUpper(), false, false, 9)
                        }, 2, ParagraphAlignment.Left, VerticalAlignment.Center));
                        rows.Add(new ContentCell(item.Quantity.ToString("D", new System.Globalization.CultureInfo("en-ph")), 3, 9, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                        rows.Add(new ContentCell(item.UnitCost.ToString("N", new System.Globalization.CultureInfo("en-ph")), 4, 9, false, false, ParagraphAlignment.Right, VerticalAlignment.Center));
                        rows.Add(new ContentCell(item.TotalCost.ToString("N", new System.Globalization.CultureInfo("en-ph")), 5, 9, false, false, ParagraphAlignment.Right, VerticalAlignment.Center));
                        reports.AddRowContent(rows, 0.40);
                    }

                    for (var i = contract.Details.Skip(fetchCount).Take(recordCount).ToList().Count; i < 10; i++)
                    {
                        rows = new List<ContentCell>();
                        rows.Add(new ContentCell("", 0, 9, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                        rows.Add(new ContentCell("", 1, 9, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                        rows.Add(new ContentCell(new TextWithFormat[]
                        {
                    new TextWithFormat("", false, false, 9)
                        }, 2, ParagraphAlignment.Left, VerticalAlignment.Center));
                        rows.Add(new ContentCell("", 3, 9, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                        rows.Add(new ContentCell("", 4, 9, false, false, ParagraphAlignment.Right, VerticalAlignment.Center));
                        rows.Add(new ContentCell("", 5, 9, false, false, ParagraphAlignment.Right, VerticalAlignment.Center));
                        reports.AddRowContent(rows, 0.40);
                    }

                    columns = new List<ContentColumn>();
                    columns.Add(new ContentColumn(1.50));
                    columns.Add(new ContentColumn(5.00));
                    columns.Add(new ContentColumn(1.50));
                    reports.AddTable(columns, false);

                    if (count == pageCount)
                    {
                        rows = new List<ContentCell>();
                        rows.Add(new ContentCell("(Total Amount in Words)", 0, 9, false, false, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 0, true, new Color(0, 0, 0, 0), false, false, true));
                        rows.Add(new ContentCell(Reports.AmountToWords(contract.TotalAmount), 1, 10, true, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, true, new Color(0, 0, 0, 0), true, false, false));
                        rows.Add(new ContentCell(contract.TotalAmount.ToString("C", new System.Globalization.CultureInfo("en-ph")), 2, 10, false, false, ParagraphAlignment.Right, VerticalAlignment.Center, 0, 0, true, new Color(0, 0, 0, 0), true, false, false));
                        reports.AddRowContent(rows, 0.35);

                        rows = new List<ContentCell>();
                        rows.Add(new ContentCell("", 0, 9, true, false, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 0, true, new Color(0, 0, 0, 0), false, false, true));
                        rows.Add(new ContentCell("Page " + count + " of " + pageCount, 1, 10, true, true, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 0, true));
                        rows.Add(new ContentCell("", 2, 9, false, false, ParagraphAlignment.Right, VerticalAlignment.Center, 0, 0, true, new Color(0, 0, 0, 0), true, false, false));
                        reports.AddRowContent(rows, 0.35);
                    }
                    else
                    {
                        rows = new List<ContentCell>();
                        rows.Add(new ContentCell("", 0, 9, true, false, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 0, true, new Color(0, 0, 0, 0), false, false, true));
                        rows.Add(new ContentCell("Page " + count + " of " + pageCount, 1, 10, true, true, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 0, true));
                        rows.Add(new ContentCell("", 2, 9, false, false, ParagraphAlignment.Right, VerticalAlignment.Center, 0, 0, true, new Color(0, 0, 0, 0), true, false, false));
                        reports.AddRowContent(rows, 0.35);
                    }

                    columns = new List<ContentColumn>();
                    columns.Add(new ContentColumn(8.00));
                    reports.AddTable(columns, false);

                    rows = new List<ContentCell>();
                    rows.Add(new ContentCell("\n\tIn case of failure to make the full delivery within the time specified, a penalty of one-tenth (1/10) of one percent for every day of delay shall be imposed on the undelivered item/s.\n", 0, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), true, false, true));
                    reports.AddRowContent(rows, 0.35);

                    columns = new List<ContentColumn>();
                    columns.Add(new ContentColumn(3.00));
                    columns.Add(new ContentColumn(2.00));
                    columns.Add(new ContentColumn(3.00));
                    reports.AddTable(columns, false);

                    rows = new List<ContentCell>();
                    rows.Add(new ContentCell("\n", 0, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), false, false, true));
                    rows.Add(new ContentCell("\n", 1, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), false, false, false));
                    rows.Add(new ContentCell("\n", 2, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), true, false, false));
                    reports.AddRowContent(rows, 0.20);

                    columns = new List<ContentColumn>();
                    columns.Add(new ContentColumn(0.50));
                    columns.Add(new ContentColumn(2.50));
                    columns.Add(new ContentColumn(1.50));
                    columns.Add(new ContentColumn(3.00));
                    columns.Add(new ContentColumn(0.50));
                    reports.AddTable(columns, false);

                    rows = new List<ContentCell>();
                    rows.Add(new ContentCell("", 0, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), false, false, true));
                    rows.Add(new ContentCell("Conforme:", 1, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center));
                    rows.Add(new ContentCell("", 2, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Center));
                    rows.Add(new ContentCell("Very truly yours,", 3, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center));
                    rows.Add(new ContentCell("", 4, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), true, false, false));
                    reports.AddRowContent(rows, 0.35);

                    rows = new List<ContentCell>();
                    rows.Add(new ContentCell("\n\n", 0, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), false, false, true));
                    rows.Add(new ContentCell("\n\n", 1, 10, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                    rows.Add(new ContentCell("\n\n", 2, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Center));
                    rows.Add(new ContentCell("\n\n", 3, 10, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
                    rows.Add(new ContentCell("\n\n", 4, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), true, false, false));
                    reports.AddRowContent(rows, 0.20);

                    rows = new List<ContentCell>();
                    rows.Add(new ContentCell("", 0, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), false, false, true));
                    rows.Add(new ContentCell(contract.SupplierRepresentative.ToUpper(), 1, 10, true, false, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 0, true));
                    rows.Add(new ContentCell("", 2, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Center));
                    rows.Add(new ContentCell(contract.HOPE.ToUpper(), 3, 10, true, false, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 0, true));
                    rows.Add(new ContentCell("", 4, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), true, false, false));
                    reports.AddRowContent(rows, 0.20);

                    rows = new List<ContentCell>();
                    rows.Add(new ContentCell("", 0, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), false, false, true));
                    rows.Add(new ContentCell("Signature over Printed Name of Supplier", 1, 8.5, false, false, ParagraphAlignment.Center, VerticalAlignment.Top));
                    rows.Add(new ContentCell("", 2, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Center));
                    rows.Add(new ContentCell("Signature over Printed Name of Authorized Official", 3, 8.5, false, false, ParagraphAlignment.Center, VerticalAlignment.Top));
                    rows.Add(new ContentCell("", 4, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), true, false, false));
                    reports.AddRowContent(rows, 0.35);

                    rows = new List<ContentCell>();
                    rows.Add(new ContentCell("", 0, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), false, false, true));
                    rows.Add(new ContentCell("", 1, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, true));
                    rows.Add(new ContentCell("", 2, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Center));
                    rows.Add(new ContentCell(contract.HOPEDesignation, 3, 10, false, false, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 0, true));
                    rows.Add(new ContentCell("", 4, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), true, false, false));
                    reports.AddRowContent(rows, 0.20);

                    rows = new List<ContentCell>();
                    rows.Add(new ContentCell("", 0, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), false, false, true));
                    rows.Add(new ContentCell("Date", 1, 8.5, false, false, ParagraphAlignment.Center, VerticalAlignment.Top));
                    rows.Add(new ContentCell("", 2, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Center));
                    rows.Add(new ContentCell("Designation", 3, 8.5, false, false, ParagraphAlignment.Center, VerticalAlignment.Top));
                    rows.Add(new ContentCell("", 4, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), true, false, false));
                    reports.AddRowContent(rows, 0.20);

                    rows = new List<ContentCell>();
                    rows.Add(new ContentCell("", 0, 1, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), false, false, true));
                    rows.Add(new ContentCell("", 1, 1, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0));
                    rows.Add(new ContentCell("", 2, 1, false, false, ParagraphAlignment.Justify, VerticalAlignment.Center));
                    rows.Add(new ContentCell("", 3, 1, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0));
                    rows.Add(new ContentCell("", 4, 1, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), true, false, false));
                    reports.AddRowContent(rows, 0.05);

                    columns = new List<ContentColumn>();
                    columns.Add(new ContentColumn(1.50));
                    columns.Add(new ContentColumn(3.00));
                    columns.Add(new ContentColumn(1.50));
                    columns.Add(new ContentColumn(2.00));
                    reports.AddTable(columns, false);

                    rows = new List<ContentCell>();
                    rows.Add(new ContentCell("Fund Cluster : ", 0, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), false, true, true));
                    rows.Add(new ContentCell(contract.FundSource, 1, 10, true, false, ParagraphAlignment.Left, VerticalAlignment.Bottom, 0, 0, true, new Color(0, 0, 0, 0), true, true, false));
                    rows.Add(new ContentCell("ORS/BURS No. : ", 2, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), false, true, false));
                    rows.Add(new ContentCell("", 3, 10, true, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, true, new Color(0, 0, 0, 0), true, true, false));
                    reports.AddRowContent(rows, 0.20);

                    rows = new List<ContentCell>();
                    rows.Add(new ContentCell("Funds Available : ", 0, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), false, true, true));
                    rows.Add(new ContentCell("", 1, 10, true, false, ParagraphAlignment.Left, VerticalAlignment.Bottom, 0, 0, true, new Color(0, 0, 0, 0), true, true, false));
                    rows.Add(new ContentCell("Date of ORS/BURS : ", 2, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), false, true, false));
                    rows.Add(new ContentCell("", 3, 10, true, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, true, new Color(0, 0, 0, 0), true, true, false));
                    reports.AddRowContent(rows, 0.20);

                    rows = new List<ContentCell>();
                    rows.Add(new ContentCell("", 0, 1, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), false, false, true));
                    rows.Add(new ContentCell("", 1, 1, true, false, ParagraphAlignment.Left, VerticalAlignment.Bottom, 0, 0, false, new Color(0, 0, 0, 0), true, false, false));
                    rows.Add(new ContentCell("", 2, 1, false, false, ParagraphAlignment.Left, VerticalAlignment.Center));
                    rows.Add(new ContentCell("", 3, 1, true, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), true, true, false));
                    reports.AddRowContent(rows, 0.05);

                    columns = new List<ContentColumn>();
                    columns.Add(new ContentColumn(0.75));
                    columns.Add(new ContentColumn(3.00));
                    columns.Add(new ContentColumn(0.75));
                    columns.Add(new ContentColumn(1.50));
                    columns.Add(new ContentColumn(2.00));
                    reports.AddTable(columns, false);

                    rows = new List<ContentCell>();
                    rows.Add(new ContentCell("", 0, 10, false, false, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), false, false, true));
                    rows.Add(new ContentCell("", 1, 10, false, false, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), false, false, false));
                    rows.Add(new ContentCell("", 2, 10, false, false, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), true, false, false));
                    rows.Add(new ContentCell("Amount : ", 3, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), false, false, false));
                    rows.Add(new ContentCell(contract.TotalAmount.ToString("C", new System.Globalization.CultureInfo("en-ph")), 4, 10, true, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, true, new Color(0, 0, 0, 0), true, false, false));
                    reports.AddRowContent(rows, 0.20);

                    rows = new List<ContentCell>();
                    rows.Add(new ContentCell("", 0, 10, false, false, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), false, false, true));
                    rows.Add(new ContentCell(new TextWithFormat[] {
                        new TextWithFormat(contract.AccountingOfficeHead, true, false, 10),
                        new TextWithFormat("\n", false, false, 10),
                        new TextWithFormat(contract.AccountingOfficeHeadDesignation + ", " + contract.AccountingOffice, false, false, 10)
                    }, 1, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 0, true, new Color(0, 0, 0, 0), false, false, false));
                    rows.Add(new ContentCell("", 2, 10, false, false, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), true, false, false));
                    rows.Add(new ContentCell("", 3, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), false, false, false));
                    rows.Add(new ContentCell("", 4, 10, true, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), true, false, false));
                    reports.AddRowContent(rows, 0.20);

                    rows = new List<ContentCell>();
                    rows.Add(new ContentCell("", 0, 10, false, false, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), false, false, true));
                    rows.Add(new ContentCell("Signature over Printed Name of Chief Accountant/Head of Accounting Division/Unit", 1, 8.5, false, false, ParagraphAlignment.Center, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, false, false));
                    rows.Add(new ContentCell("", 2, 10, false, false, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), true, false, false));
                    rows.Add(new ContentCell("", 3, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), false, false, false));
                    rows.Add(new ContentCell("", 4, 10, true, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), true, false, false));
                    reports.AddRowContent(rows, 0.20);

                    rows = new List<ContentCell>();
                    rows.Add(new ContentCell("\n", 0, 10, false, false, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 0, true, new Color(0, 0, 0, 0), false, false, true));
                    rows.Add(new ContentCell("\n", 1, 10, false, false, ParagraphAlignment.Center, VerticalAlignment.Top, 0, 0, true, new Color(0, 0, 0, 0), false, false, false));
                    rows.Add(new ContentCell("\n", 2, 10, false, false, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 0, true, new Color(0, 0, 0, 0), true, false, false));
                    rows.Add(new ContentCell("\n", 3, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, true, new Color(0, 0, 0, 0), false, false, false));
                    rows.Add(new ContentCell("\n", 4, 10, true, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, true, new Color(0, 0, 0, 0), true, false, false));
                    reports.AddRowContent(rows, 0.05);

                    fetchCount += recordCount;
                    if (count < pageCount)
                    {
                        reports.CreateDocument(8.50, 13.00, Orientation.Portrait, 0.25);
                    }
                }
            }
            
            return reports.GenerateReport();
        }
        public MemoryStream GenerateNoticeToProceed(string ContractCode, string LogoPath, string UserEmail)
        {
            var agencyDetails = db.AgencyDetails.FirstOrDefault();
            var hope = hris.GetDepartmentDetails(agencyDetails.HOPEReference);
            var procurementOffice = hris.GetDepartmentDetails(agencyDetails.ProcurementOfficeReference);
            var bidsAndAwards = hris.GetDepartmentDetails(agencyDetails.BACOfficeReference);
            var bacsecHead = db.BACSecretariat.Where(d => d.Membership == BACMembership.BACSECHead && d.PurgeFlag == false).FirstOrDefault();
            var bacChair = db.BACSecretariat.Where(d => d.Membership == BACMembership.BACChairman && d.PurgeFlag == false).FirstOrDefault();
            var noticesOfAward = contractDAL.NoticeToProceedSetup(ContractCode);
            Reports reports = new Reports();
            reports.ReportFilename = "Notice to Proceed - " + ContractCode;
            foreach(var noticeOfAward in noticesOfAward)
            {
                reports.CreateDocument(8.50, 13.00, Orientation.Portrait, 1.00);
                reports.AddDoubleColumnHeader(LogoPath, false, 1.25);
                reports.AddColumnHeader(
                    new HeaderLine { Content = "Republic of the Philippines", Bold = false, Italic = false, FontSize = 8, ParagraphAlignment = ParagraphAlignment.Left },
                    new HeaderLine { Content = "", Bold = false, Italic = false, FontSize = 6, ParagraphAlignment = ParagraphAlignment.Left }
                );
                reports.AddColumnHeader(
                    new HeaderLine { Content = agencyDetails.AgencyName.ToUpper(), Bold = true, Italic = false, FontSize = 12, ParagraphAlignment = ParagraphAlignment.Left },
                    new HeaderLine { Content = "", Bold = false, Italic = false, FontSize = 7, ParagraphAlignment = ParagraphAlignment.Left }
                );
                reports.AddColumnHeader(
                    new HeaderLine { Content = agencyDetails.Address, Bold = false, Italic = false, FontSize = 10, ParagraphAlignment = ParagraphAlignment.Left },
                    new HeaderLine { Content = "", Bold = false, Italic = false, FontSize = 8, ParagraphAlignment = ParagraphAlignment.Left }
                );
                reports.AddColumnHeader(
                    new HeaderLine { Content = "THE COUNTRY'S FIRST POLYTECHNIC UNIVERSITY", Bold = false, Italic = false, FontSize = 10, ParagraphAlignment = ParagraphAlignment.Left },
                    new HeaderLine { Content = "", Bold = true, Italic = false, FontSize = 5, ParagraphAlignment = ParagraphAlignment.Left }
                );

                var columns = new List<ContentColumn>();
                columns.Add(new ContentColumn(6.50));
                reports.AddTable(columns, false);

                reports.AddFormattedRowContent(new ContentCell(new TextWithFormat[]
                {
               new TextWithFormat("NOTICE TO PROCEED", true, false, 16),
                }, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center), 0.00);

                reports.AddNewLine();

                columns = new List<ContentColumn>();
                columns.Add(new ContentColumn(6.50));
                reports.AddTable(columns, false);

                var rows = new List<ContentCell>();
                rows.Add(new ContentCell(DateTime.Now.ToString("dd MMMM yyyy"), 0, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center));
                reports.AddRowContent(rows, 0.18);

                reports.AddNewLine();
                reports.AddNewLine();

                columns = new List<ContentColumn>();
                columns.Add(new ContentColumn(6.50));
                reports.AddTable(columns, false);

                rows = new List<ContentCell>();
                rows.Add(new ContentCell(noticeOfAward.ContactPerson.ToUpper(), 0, 10, true, false, ParagraphAlignment.Left, VerticalAlignment.Center));
                reports.AddRowContent(rows, 0.18);

                rows = new List<ContentCell>();
                rows.Add(new ContentCell((noticeOfAward.ContactPersonDesignation == string.Empty || noticeOfAward.ContactPersonDesignation == null ? "Authorized and Designated Representative" : noticeOfAward.ContactPersonDesignation), 0, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center));
                reports.AddRowContent(rows, 0.18);

                rows = new List<ContentCell>();
                rows.Add(new ContentCell(noticeOfAward.Supplier.ToUpper(), 0, 10, true, false, ParagraphAlignment.Left, VerticalAlignment.Center));
                reports.AddRowContent(rows, 0.18);

                rows = new List<ContentCell>();
                rows.Add(new ContentCell(noticeOfAward.Address, 0, 9.5, false, false, ParagraphAlignment.Left, VerticalAlignment.Center));
                reports.AddRowContent(rows, 0.18);

                if (noticeOfAward.City != null || noticeOfAward.City != string.Empty)
                {
                    rows = new List<ContentCell>();
                    rows.Add(new ContentCell(noticeOfAward.City, 0, 9.5, false, false, ParagraphAlignment.Left, VerticalAlignment.Center));
                    reports.AddRowContent(rows, 0.18);
                }

                if (noticeOfAward.State != null || noticeOfAward.State != string.Empty)
                {
                    rows = new List<ContentCell>();
                    rows.Add(new ContentCell(noticeOfAward.State + (noticeOfAward.PostalCode == string.Empty ? string.Empty : ", " + noticeOfAward.PostalCode), 0, 9.5, false, false, ParagraphAlignment.Left, VerticalAlignment.Center));
                    reports.AddRowContent(rows, 0.18);
                }

                reports.AddNewLine();
                reports.AddNewLine();

                columns = new List<ContentColumn>();
                columns.Add(new ContentColumn(6.50));
                reports.AddTable(columns, false);

                reports.AddFormattedRowContent(new ContentCell(new TextWithFormat[]
                {
                new TextWithFormat("Dear ", false, true, 10),
                new TextWithFormat(noticeOfAward.ContactPerson, false, true, 10),
                new TextWithFormat(":", false, true, 10),
                }, MigraDoc.DocumentObjectModel.ParagraphAlignment.Left), 0.18);

                reports.AddNewLine();
                reports.AddNewLine();

                columns = new List<ContentColumn>();
                columns.Add(new ContentColumn(6.50));
                reports.AddTable(columns, false);

                rows = new List<ContentCell>();
                rows.Add(new ContentCell(new TextWithFormat[]
                {
                new TextWithFormat("The attached ", false, false, 10),
                new TextWithFormat(" Purchase Order (P.O.) No. " + noticeOfAward.PurchaseOrderNo + " dated " + noticeOfAward.PurchaseOrderDate.ToString("dd MMMM yyyy"), true, false, 10),
                new TextWithFormat(" having been approved, notice is hereby given to ", false, false, 10),
                new TextWithFormat(noticeOfAward.Supplier, true, false, 10),
                new TextWithFormat(" to immediately commence the delivery of ", false, false, 10),
                new TextWithFormat(noticeOfAward.ContractName, true, true, 10),
                new TextWithFormat(".", false, false, 10),
                }, 0, ParagraphAlignment.Justify, VerticalAlignment.Top));
                reports.AddRowContent(rows, 0.25);

                reports.AddNewLine();

                columns = new List<ContentColumn>();
                columns.Add(new ContentColumn(6.50));
                reports.AddTable(columns, false);

                rows = new List<ContentCell>();
                rows.Add(new ContentCell(new TextWithFormat[]
                {
                new TextWithFormat("Upon the receipt of this notice, it is understood that the delivery shall be competed within ", false, false, 10),
                new TextWithFormat(noticeOfAward.Delivery.ToString() + " (" + noticeOfAward.DeliveryWords + ") calendar days upon receipt hereof", true, true, 10),
                new TextWithFormat(" and as stipulated in the foregoing Purchase Order.", false, false, 10),
                }, 0, ParagraphAlignment.Justify, VerticalAlignment.Top));
                reports.AddRowContent(rows, 0.25);

                reports.AddNewLine();

                columns = new List<ContentColumn>();
                columns.Add(new ContentColumn(6.50));
                reports.AddTable(columns, false);

                rows = new List<ContentCell>();
                rows.Add(new ContentCell(new TextWithFormat[]
                {
                new TextWithFormat("Please acknowledge receipt and acceptance of this notice by signing both copies in the space proviced below. Keep one (1) copy and return the other to the Polytechnic University of the Philippines.", false, false, 10),
                }, 0, ParagraphAlignment.Justify, VerticalAlignment.Top));
                reports.AddRowContent(rows, 0.25);

                reports.AddNewLine();
                reports.AddNewLine();

                columns = new List<ContentColumn>();
                columns.Add(new ContentColumn(6.50));
                reports.AddTable(columns, false);

                rows = new List<ContentCell>();
                rows.Add(new ContentCell(new TextWithFormat[]
                {
                new TextWithFormat("Very truly yours,", false, false, 10),
                }, 0, ParagraphAlignment.Justify, VerticalAlignment.Top));
                reports.AddRowContent(rows, 0.25);

                reports.AddNewLine();
                reports.AddNewLine();
                reports.AddNewLine();

                columns = new List<ContentColumn>();
                columns.Add(new ContentColumn(6.50));
                reports.AddTable(columns, false);

                rows = new List<ContentCell>();
                rows.Add(new ContentCell(new TextWithFormat[]
                {
                new TextWithFormat(hope.SectorHead, true, false, 10),
                }, 0, ParagraphAlignment.Justify, VerticalAlignment.Top));
                reports.AddRowContent(rows, 0.25);

                rows = new List<ContentCell>();
                rows.Add(new ContentCell(new TextWithFormat[]
                {
                new TextWithFormat(hope.SectorHeadDesignation, false, false, 10),
                }, 0, ParagraphAlignment.Justify, VerticalAlignment.Top));
                reports.AddRowContent(rows, 0.25);

                reports.AddNewLine();
                reports.AddNewLine();
                reports.AddNewLine();
                reports.AddNewLine();

                columns = new List<ContentColumn>();
                columns.Add(new ContentColumn(6.50));
                reports.AddTable(columns, false);

                rows = new List<ContentCell>();
                rows.Add(new ContentCell(new TextWithFormat[]
                {
                new TextWithFormat("Conforme:", false, false, 10),
                }, 0, ParagraphAlignment.Justify, VerticalAlignment.Top));
                reports.AddRowContent(rows, 0.25);

                reports.AddNewLine();
                reports.AddNewLine();
                reports.AddNewLine();

                columns = new List<ContentColumn>();
                columns.Add(new ContentColumn(6.50));
                reports.AddTable(columns, false);

                rows = new List<ContentCell>();
                rows.Add(new ContentCell(noticeOfAward.ContactPerson.ToUpper(), 0, 10, true, false, ParagraphAlignment.Left, VerticalAlignment.Center));
                reports.AddRowContent(rows, 0.18);

                rows = new List<ContentCell>();
                rows.Add(new ContentCell((noticeOfAward.ContactPersonDesignation == string.Empty || noticeOfAward.ContactPersonDesignation == null ? "Authorized and Designated Representative" : noticeOfAward.ContactPersonDesignation), 0, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center));
                reports.AddRowContent(rows, 0.18);

                rows = new List<ContentCell>();
                rows.Add(new ContentCell("Date: __________________", 0, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center));
                reports.AddRowContent(rows, 0.18);
            }

            return reports.GenerateReport();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
                hris.Dispose();
                contractDAL.Dispose();
            }
            base.Dispose(disposing);
        }
    }

    public class ContractSingleDAL : Controller
    {
        private FMISDbContext db = new FMISDbContext();
        private HRISDataAccess hris = new HRISDataAccess();
        private ABISDataAccess abis = new ABISDataAccess();
        private SystemBDL systemBDL = new SystemBDL();
        private ProcurementProjectsDAL contractDAL = new ProcurementProjectsDAL();

        public ProcurementProjectStages GetProcurementProjectStage(string ContractCode)
        {
            return db.ProcurementProjects.Where(d => d.ContractCode == ContractCode).Select(d => d.ProcurementProjectStage).FirstOrDefault();
        }
        public string GetContractClassification(string ContractCode)
        {
            return db.ProcurementProjects.Where(d => d.ContractCode == ContractCode).Select(d => d.FKClassificationReference.GeneralClass).FirstOrDefault();
        }
        public ContractStrategies GetContractStrategy(string ContractCode)
        {
            return db.ProcurementProjects.Where(d => d.ContractCode == ContractCode).Select(d => d.ContractStrategy).FirstOrDefault();
        }

        public OpenProcurementProjectVM GetSingleContractDetails(string ContractCode)
        {
            var contract = db.ProcurementProjects.ToList().Where(d => d.ContractCode == ContractCode).Select(d => new OpenProcurementProjectVM
            {
                PAPCode = d.FKAPPReference.PAPCode,
                ClassificationReference = d.ClassificationReference,
                Classification = d.FKClassificationReference.GeneralClass + " - " + d.FKClassificationReference.Classification,
                ModeOfProcurementReference = d.ModeOfProcurementReference,
                ModeOfProcurement = d.FKModeOfProcurementReference.ModeOfProcurementName,
                FiscalYear = d.FiscalYear,
                FundSource = d.FundSource,
                FundDescription = abis.GetFundSources(d.FundSource).FUND_DESC,
                ProcurementProjectType = d.ProcurementProjectType,
                ContractStrategy = d.ContractStrategy,
                ContractCode = d.ContractCode,
                ContractName = d.ContractName,
                ContractLocation = d.ContractLocation,
                ContractStatus = d.ContractStatus,
                ProcurementProjectStage = d.ProcurementProjectStage,
                ApprovedBudgetForContract = d.ApprovedBudgetForContract,
                DeliveryPeriod = (int)d.DeliveryPeriod,
                ProjectCoordinator = hris.GetEmployeeByCode(d.ProjectCoordinator).EmployeeName,
                PRSubmissionOpen = (DateTime)d.PRSubmissionOpen,
                PRSubmissionClose = (DateTime)d.PRSubmissionClose,
                PreProcurementConference = d.PreProcurementConference,
                PostingOfIB = (DateTime)d.PostingOfIB_RFQPosting,
                PreBidConference = d.PreBidConference,
                DeadlineOfSubmissionOfBids = (DateTime)d.DeadlineOfSubmissionOfBids_DeadlineOfSubmisionOfBids,
                OpeningOfBids = (DateTime)d.OpeningOfBids_OpeningOfQuotations,
                NOAIssuance = d.NOAIssuance,
                NTPIssuance = d.NTPIssuance,
                OpeningOfBidsFailureReason = d.OpeningOfBidsFailureReason,
                PostQualificationFailureReason = d.PostQualificationFailureReason,
                CreatedAt = d.CreatedAt,
                CreatedBy = hris.GetEmployeeByCode(d.CreatedBy).EmployeeName,
                ContractItems = d.ProcurementProjectStage == ProcurementProjectStages.ProcurementClosed ? contractDAL.GetContractItems(d.ContractCode) : contractDAL.GetProcurementProjectItems(d.ContractCode),
                Updates = db.ContractUpdates.Where(x => x.ProcurementProjectReference == d.ID).OrderByDescending(x => x.UpdatedAt).ToList().Select(x => new ContractUpdates
                {
                    ProcurementProjectReference = x.ProcurementProjectReference,
                    ProcurementProjectStage = x.ProcurementProjectStage,
                    Remarks = x.Remarks,
                    UpdatedAt = x.UpdatedAt,
                    UpdatedBy = x.UpdatedBy == null || x.UpdatedBy == "SYSTEM" ? "SYSTEM" : hris.GetEmployeeByCode(x.UpdatedBy).EmployeeName
                }).ToList()
            }).FirstOrDefault();
            return contract;
        }
        public PreProcurementConferenceSetupVM GetPurchaseRequestSubmissionClosing(string ContractCode)
        {
            var contract = db.ProcurementProjects.ToList().Where(d => d.ContractCode == ContractCode).Select(d => new PreProcurementConferenceSetupVM
            {
                PAPCode = d.FKAPPReference.PAPCode,
                ClassificationReference = d.ClassificationReference,
                Classification = d.FKClassificationReference.GeneralClass + " - " + d.FKClassificationReference.Classification,
                ModeOfProcurementReference = d.ModeOfProcurementReference,
                ModeOfProcurement = d.FKModeOfProcurementReference.ModeOfProcurementName,
                FiscalYear = d.FiscalYear,
                FundSource = d.FundSource,
                FundDescription = abis.GetFundSources(d.FundSource).FUND_DESC,
                ProcurementProjectType = d.ProcurementProjectType,
                ContractStrategy = d.ContractStrategy,
                ContractCode = d.ContractCode,
                ContractName = d.ContractName,
                ContractLocation = d.ContractLocation,
                ContractStatus = d.ContractStatus,
                ProcurementProjectStage = d.ProcurementProjectStage,
                ApprovedBudgetForContract = d.ApprovedBudgetForContract,
                DeliveryPeriod = (int)d.DeliveryPeriod,
                ProjectCoordinator = hris.GetEmployeeByCode(d.ProjectCoordinator).EmployeeName,
                PreProcurementConference = d.PreProcurementConference,
                CreatedAt = d.CreatedAt,
                CreatedBy = hris.GetEmployeeByCode(d.CreatedBy).EmployeeName,
                ContractItems = contractDAL.GetProcurementProjectItems(d.ContractCode),
                Updates = db.ContractUpdates.Where(x => x.ProcurementProjectReference == d.ID).OrderByDescending(x => x.UpdatedAt).ToList().Select(x => new ContractUpdates
                {
                    ProcurementProjectReference = x.ProcurementProjectReference,
                    ProcurementProjectStage = x.ProcurementProjectStage,
                    Remarks = x.Remarks,
                    UpdatedAt = x.UpdatedAt,
                    UpdatedBy = x.UpdatedBy == "SYSTEM" ? "SYSTEM" : hris.GetEmployeeByCode(x.UpdatedBy).EmployeeName
                }).ToList()
            }).FirstOrDefault();
            return contract;
        }
        public PreProcurementConferenceUpdateVM GetPreProcurementConferenceUpdate(string ContractCode)
        {
            var contract = db.ProcurementProjects.ToList().Where(d => d.ContractCode == ContractCode).Select(d => new PreProcurementConferenceUpdateVM
            {
                PAPCode = d.FKAPPReference.PAPCode,
                ClassificationReference = d.ClassificationReference,
                Classification = d.FKClassificationReference.GeneralClass + " - " + d.FKClassificationReference.Classification,
                ModeOfProcurementReference = d.ModeOfProcurementReference,
                ModeOfProcurement = d.FKModeOfProcurementReference.ModeOfProcurementName,
                FiscalYear = d.FiscalYear,
                FundSource = d.FundSource,
                FundDescription = abis.GetFundSources(d.FundSource).FUND_DESC,
                ProcurementProjectType = d.ProcurementProjectType,
                ContractStrategy = d.ContractStrategy,
                ContractCode = d.ContractCode,
                ContractName = d.ContractName,
                ContractLocation = d.ContractLocation,
                ContractStatus = d.ContractStatus,
                ProcurementProjectStage = d.ProcurementProjectStage,
                ApprovedBudgetForContract = d.ApprovedBudgetForContract,
                DeliveryPeriod = (int)d.DeliveryPeriod,
                ProjectCoordinator = hris.GetEmployeeByCode(d.ProjectCoordinator).EmployeeName,
                PreProcurementConference = d.PreProcurementConference,
                PreProcurementConferenceLocation = d.PreProcurementConferenceLocation,
                CreatedAt = d.CreatedAt,
                CreatedBy = hris.GetEmployeeByCode(d.CreatedBy).EmployeeName,
                ContractItems = contractDAL.GetProcurementProjectItems(d.ContractCode),
                Updates = db.ContractUpdates.Where(x => x.ProcurementProjectReference == d.ID).OrderByDescending(x => x.UpdatedAt).ToList().Select(x => new ContractUpdates
                {
                    ProcurementProjectReference = x.ProcurementProjectReference,
                    ProcurementProjectStage = x.ProcurementProjectStage,
                    Remarks = x.Remarks,
                    UpdatedAt = x.UpdatedAt,
                    UpdatedBy = x.UpdatedBy == "SYSTEM" ? "SYSTEM" : hris.GetEmployeeByCode(x.UpdatedBy).EmployeeName
                }).ToList()
            }).FirstOrDefault();
            return contract;
        }
        public PreBidConferenceSetupVM GetPreBidConferenceSetup(string ContractCode)
        {
            var contract = db.ProcurementProjects.ToList().Where(d => d.ContractCode == ContractCode).Select(d => new PreBidConferenceSetupVM
            {
                PAPCode = d.FKAPPReference.PAPCode,
                ClassificationReference = d.ClassificationReference,
                Classification = d.FKClassificationReference.GeneralClass + " - " + d.FKClassificationReference.Classification,
                ModeOfProcurementReference = d.ModeOfProcurementReference,
                ModeOfProcurement = d.FKModeOfProcurementReference.ModeOfProcurementName,
                FiscalYear = d.FiscalYear,
                FundSource = d.FundSource,
                FundDescription = abis.GetFundSources(d.FundSource).FUND_DESC,
                ProcurementProjectType = d.ProcurementProjectType,
                ContractStrategy = d.ContractStrategy,
                ContractCode = d.ContractCode,
                ContractName = d.ContractName,
                ContractLocation = d.ContractLocation,
                ContractStatus = d.ContractStatus,
                ProcurementProjectStage = d.ProcurementProjectStage,
                ApprovedBudgetForContract = d.ApprovedBudgetForContract,
                DeliveryPeriod = (int)d.DeliveryPeriod,
                ProjectCoordinator = hris.GetEmployeeByCode(d.ProjectCoordinator).EmployeeName,
                PreBidConference = d.PreBidConference == null ? DateTime.Now : d.PreBidConference.Value,
                DeadlineOfSubmissionOfBids = d.DeadlineOfSubmissionOfBids_DeadlineOfSubmisionOfBids.Value,
                OpeningOfBids = d.OpeningOfBids_OpeningOfQuotations.Value,
                CreatedAt = d.CreatedAt,
                CreatedBy = hris.GetEmployeeByCode(d.CreatedBy).EmployeeName,
                ContractItems = contractDAL.GetProcurementProjectItems(d.ContractCode),
                Updates = db.ContractUpdates.Where(x => x.ProcurementProjectReference == d.ID).OrderByDescending(x => x.UpdatedAt).ToList().Select(x => new ContractUpdates
                {
                    ProcurementProjectReference = x.ProcurementProjectReference,
                    ProcurementProjectStage = x.ProcurementProjectStage,
                    Remarks = x.Remarks,
                    UpdatedAt = x.UpdatedAt,
                    UpdatedBy = x.UpdatedBy == "SYSTEM" ? "SYSTEM" : hris.GetEmployeeByCode(x.UpdatedBy).EmployeeName
                }).ToList()
            }).FirstOrDefault();
            return contract;
        }
        public PreBidConferenceUpdateVM GetPreBidConferenceUpdate(string ContractCode)
        {
            var contract = db.ProcurementProjects.ToList().Where(d => d.ContractCode == ContractCode).Select(d => new PreBidConferenceUpdateVM
            {
                PAPCode = d.FKAPPReference.PAPCode,
                ClassificationReference = d.ClassificationReference,
                Classification = d.FKClassificationReference.GeneralClass + " - " + d.FKClassificationReference.Classification,
                ModeOfProcurementReference = d.ModeOfProcurementReference,
                ModeOfProcurement = d.FKModeOfProcurementReference.ModeOfProcurementName,
                FiscalYear = d.FiscalYear,
                FundSource = d.FundSource,
                FundDescription = abis.GetFundSources(d.FundSource).FUND_DESC,
                ProcurementProjectType = d.ProcurementProjectType,
                ContractStrategy = d.ContractStrategy,
                ContractCode = d.ContractCode,
                ContractName = d.ContractName,
                ContractLocation = d.ContractLocation,
                ContractStatus = d.ContractStatus,
                ProcurementProjectStage = d.ProcurementProjectStage,
                ApprovedBudgetForContract = d.ApprovedBudgetForContract,
                DeliveryPeriod = (int)d.DeliveryPeriod,
                ProjectCoordinator = hris.GetEmployeeByCode(d.ProjectCoordinator).EmployeeName,
                PreBidConference = d.PreBidConference,
                PreBidConferenceLocation = d.PreBidConferenceLocation,
                CreatedAt = d.CreatedAt,
                CreatedBy = hris.GetEmployeeByCode(d.CreatedBy).EmployeeName,
                ContractItems = contractDAL.GetProcurementProjectItems(d.ContractCode),
                Updates = db.ContractUpdates.Where(x => x.ProcurementProjectReference == d.ID).OrderByDescending(x => x.UpdatedAt).ToList().Select(x => new ContractUpdates
                {
                    ProcurementProjectReference = x.ProcurementProjectReference,
                    ProcurementProjectStage = x.ProcurementProjectStage,
                    Remarks = x.Remarks,
                    UpdatedAt = x.UpdatedAt,
                    UpdatedBy = x.UpdatedBy == "SYSTEM" ? "SYSTEM" : hris.GetEmployeeByCode(x.UpdatedBy).EmployeeName
                }).ToList()
            }).FirstOrDefault();
            return contract;
        }
        public OpeningOfBidsUpdateVM GetOpeningOfBids(string ContractCode)
        {
            var contract = db.ProcurementProjects.ToList().Where(d => d.ContractCode == ContractCode).Select(d => new OpeningOfBidsUpdateVM
            {
                PAPCode = d.FKAPPReference.PAPCode,
                ClassificationReference = d.ClassificationReference,
                Classification = d.FKClassificationReference.GeneralClass + " - " + d.FKClassificationReference.Classification,
                ModeOfProcurementReference = d.ModeOfProcurementReference,
                ModeOfProcurement = d.FKModeOfProcurementReference.ModeOfProcurementName,
                FiscalYear = d.FiscalYear,
                FundSource = d.FundSource,
                FundDescription = abis.GetFundSources(d.FundSource).FUND_DESC,
                ProcurementProjectType = d.ProcurementProjectType,
                ContractStrategy = d.ContractStrategy,
                ContractCode = d.ContractCode,
                ContractName = d.ContractName,
                ContractLocation = d.ContractLocation,
                ContractStatus = d.ContractStatus,
                ProcurementProjectStage = d.ProcurementProjectStage,
                ApprovedBudgetForContract = d.ApprovedBudgetForContract,
                DeliveryPeriod = (int)d.DeliveryPeriod,
                ProjectCoordinator = hris.GetEmployeeByCode(d.ProjectCoordinator).EmployeeName,
                OpeningOfBids = d.OpeningOfBids_OpeningOfQuotations.Value,
                OpeningOfBidsLocation = d.OpeningOfBidsLocation,
                CreatedAt = d.CreatedAt,
                CreatedBy = hris.GetEmployeeByCode(d.CreatedBy).EmployeeName,
                ContractItems = contractDAL.GetProcurementProjectItems(d.ContractCode),
                Updates = db.ContractUpdates.Where(x => x.ProcurementProjectReference == d.ID).OrderByDescending(x => x.UpdatedAt).ToList().Select(x => new ContractUpdates
                {
                    ProcurementProjectReference = x.ProcurementProjectReference,
                    ProcurementProjectStage = x.ProcurementProjectStage,
                    Remarks = x.Remarks,
                    UpdatedAt = x.UpdatedAt,
                    UpdatedBy = x.UpdatedBy == "SYSTEM" ? "SYSTEM" : hris.GetEmployeeByCode(x.UpdatedBy).EmployeeName
                }).ToList()
            }).FirstOrDefault();
            return contract;
        }
        public EvaluationOfBidsUpdateVM GetEvaluationOfBids(string ContractCode)
        {
            var contract = db.ProcurementProjects.ToList().Where(d => d.ContractCode == ContractCode).Select(d => new EvaluationOfBidsUpdateVM
            {
                PAPCode = d.FKAPPReference.PAPCode,
                ClassificationReference = d.ClassificationReference,
                Classification = d.FKClassificationReference.GeneralClass + " - " + d.FKClassificationReference.Classification,
                ModeOfProcurementReference = d.ModeOfProcurementReference,
                ModeOfProcurement = d.FKModeOfProcurementReference.ModeOfProcurementName,
                FiscalYear = d.FiscalYear,
                FundSource = d.FundSource,
                FundDescription = abis.GetFundSources(d.FundSource).FUND_DESC,
                ProcurementProjectType = d.ProcurementProjectType,
                ContractStrategy = d.ContractStrategy,
                ContractCode = d.ContractCode,
                ContractName = d.ContractName,
                ContractLocation = d.ContractLocation,
                ContractStatus = d.ContractStatus,
                ProcurementProjectStage = d.ProcurementProjectStage,
                ApprovedBudgetForContract = d.ApprovedBudgetForContract,
                DeliveryPeriod = (int)d.DeliveryPeriod,
                ProjectCoordinator = hris.GetEmployeeByCode(d.ProjectCoordinator).EmployeeName,
                CreatedAt = d.CreatedAt,
                CreatedBy = hris.GetEmployeeByCode(d.CreatedBy).EmployeeName,
                ContractItems = contractDAL.GetProcurementProjectItems(d.ContractCode),
                Updates = db.ContractUpdates.Where(x => x.ProcurementProjectReference == d.ID).OrderByDescending(x => x.UpdatedAt).ToList().Select(x => new ContractUpdates
                {
                    ProcurementProjectReference = x.ProcurementProjectReference,
                    ProcurementProjectStage = x.ProcurementProjectStage,
                    Remarks = x.Remarks,
                    UpdatedAt = x.UpdatedAt,
                    UpdatedBy = x.UpdatedBy == "SYSTEM" ? "SYSTEM" : hris.GetEmployeeByCode(x.UpdatedBy).EmployeeName
                }).ToList(),
            }).FirstOrDefault();

            return contract;
        }
        public PostQualificationUpdateVM GetPostQualificationUpdate(string ContractCode)
        {
            var contract = db.ProcurementProjects.ToList().Where(d => d.ContractCode == ContractCode).Select(d => new PostQualificationUpdateVM
            {
                PAPCode = d.FKAPPReference.PAPCode,
                ClassificationReference = d.ClassificationReference,
                Classification = d.FKClassificationReference.GeneralClass + " - " + d.FKClassificationReference.Classification,
                ModeOfProcurementReference = d.ModeOfProcurementReference,
                ModeOfProcurement = d.FKModeOfProcurementReference.ModeOfProcurementName,
                FiscalYear = d.FiscalYear,
                FundSource = d.FundSource,
                FundDescription = abis.GetFundSources(d.FundSource).FUND_DESC,
                ProcurementProjectType = d.ProcurementProjectType,
                ContractStrategy = d.ContractStrategy,
                ContractCode = d.ContractCode,
                ContractName = d.ContractName,
                ContractLocation = d.ContractLocation,
                ContractStatus = d.ContractStatus,
                ProcurementProjectStage = d.ProcurementProjectStage,
                ApprovedBudgetForContract = d.ApprovedBudgetForContract,
                DeliveryPeriod = (int)d.DeliveryPeriod,
                ProjectCoordinator = hris.GetEmployeeByCode(d.ProjectCoordinator).EmployeeName,
                CreatedAt = d.CreatedAt,
                CreatedBy = hris.GetEmployeeByCode(d.CreatedBy).EmployeeName,
                ContractItems = contractDAL.GetProcurementProjectItems(d.ContractCode),
                Updates = db.ContractUpdates.Where(x => x.ProcurementProjectReference == d.ID).OrderByDescending(x => x.UpdatedAt).ToList().Select(x => new ContractUpdates
                {
                    ProcurementProjectReference = x.ProcurementProjectReference,
                    ProcurementProjectStage = x.ProcurementProjectStage,
                    Remarks = x.Remarks,
                    UpdatedAt = x.UpdatedAt,
                    UpdatedBy = x.UpdatedBy == "SYSTEM" ? "SYSTEM" : hris.GetEmployeeByCode(x.UpdatedBy).EmployeeName
                }).ToList()
            }).FirstOrDefault();
            return contract;
        }
        public NoticeOfAwardSetupVM GetNoticeOfAwardSetup(string ContractCode)
        {
            var contract = db.ProcurementProjects.ToList().Where(d => d.ContractCode == ContractCode).Select(d => new NoticeOfAwardSetupVM
            {
                PAPCode = d.FKAPPReference.PAPCode,
                ClassificationReference = d.ClassificationReference,
                Classification = d.FKClassificationReference.GeneralClass + " - " + d.FKClassificationReference.Classification,
                ModeOfProcurementReference = d.ModeOfProcurementReference,
                ModeOfProcurement = d.FKModeOfProcurementReference.ModeOfProcurementName,
                FiscalYear = d.FiscalYear,
                FundSource = d.FundSource,
                FundDescription = abis.GetFundSources(d.FundSource).FUND_DESC,
                ProcurementProjectType = d.ProcurementProjectType,
                ContractStrategy = d.ContractStrategy,
                ContractCode = d.ContractCode,
                ContractName = d.ContractName,
                ContractLocation = d.ContractLocation,
                ContractStatus = d.ContractStatus,
                ProcurementProjectStage = d.ProcurementProjectStage,
                ApprovedBudgetForContract = d.ApprovedBudgetForContract,
                DeliveryPeriod = (int)d.DeliveryPeriod,
                ProjectCoordinator = hris.GetEmployeeByCode(d.ProjectCoordinator).EmployeeName,
                CreatedAt = d.CreatedAt,
                CreatedBy = hris.GetEmployeeByCode(d.CreatedBy).EmployeeName,
                ContractItems = contractDAL.GetProcurementProjectItems(d.ContractCode),
                Updates = db.ContractUpdates.Where(x => x.ProcurementProjectReference == d.ID).OrderByDescending(x => x.UpdatedAt).ToList().Select(x => new ContractUpdates
                {
                    ProcurementProjectReference = x.ProcurementProjectReference,
                    ProcurementProjectStage = x.ProcurementProjectStage,
                    Remarks = x.Remarks,
                    UpdatedAt = x.UpdatedAt,
                    UpdatedBy = x.UpdatedBy == "SYSTEM" ? "SYSTEM" : hris.GetEmployeeByCode(x.UpdatedBy).EmployeeName
                }).ToList(),
                BidDetails = db.ProcurementProjectDetails.Where(x => x.ProcurementProjectReference == d.ID).ToList().Select(x => new BidDetailsVM
                {
                    ArticleReference = x.ArticleReference,
                    ItemSequence = x.ItemSequence,
                    ItemFullName = x.ItemFullName,
                    ItemSpecifications = x.ItemSpecifications,
                    Quantity = x.Quantity,
                    UnitCost = x.EstimatedUnitCost,
                    TotalCost = x.ApprovedBudgetForItem,
                    UOMReference = x.UOMReference,
                    Unit = x.FKUOMReference.Abbreviation
                }).ToList()
            }).FirstOrDefault();
            return contract;
        }
        public NoticeOfAwardSetupLineItemsVM GetNoticeOfAwardLineSetup(string ContractCode)
        {
            var contract = db.ProcurementProjects.ToList().Where(d => d.ContractCode == ContractCode).Select(d => new NoticeOfAwardSetupLineItemsVM
            {
                PAPCode = d.FKAPPReference.PAPCode,
                ClassificationReference = d.ClassificationReference,
                Classification = d.FKClassificationReference.GeneralClass + " - " + d.FKClassificationReference.Classification,
                ModeOfProcurementReference = d.ModeOfProcurementReference,
                ModeOfProcurement = d.FKModeOfProcurementReference.ModeOfProcurementName,
                FiscalYear = d.FiscalYear,
                FundSource = d.FundSource,
                FundDescription = abis.GetFundSources(d.FundSource).FUND_DESC,
                ProcurementProjectType = d.ProcurementProjectType,
                ContractStrategy = d.ContractStrategy,
                ContractCode = d.ContractCode,
                ContractName = d.ContractName,
                ContractLocation = d.ContractLocation,
                ContractStatus = d.ContractStatus,
                ProcurementProjectStage = d.ProcurementProjectStage,
                ApprovedBudgetForContract = d.ApprovedBudgetForContract,
                DeliveryPeriod = (int)d.DeliveryPeriod,
                ProjectCoordinator = hris.GetEmployeeByCode(d.ProjectCoordinator).EmployeeName,
                CreatedAt = d.CreatedAt,
                CreatedBy = hris.GetEmployeeByCode(d.CreatedBy).EmployeeName,
                ContractItems = contractDAL.GetProcurementProjectItems(d.ContractCode),
                LineItems = contractDAL.GetProcurementProjectItems(d.ContractCode).Select(x => new LineItemContractorVM {
                    ArticleReference = x.ArticleReference,
                    ItemSequence = x.ItemSequence,
                    ItemFullName = x.ItemFullName,
                    ItemSpecifications = x.ItemSpecifications,
                    Unit = x.UnitOfMeasure,
                    UOMReference = x.UOMReference,
                    UnitCost = x.EstimatedUnitCost,
                    TotalCost = x.ApprovedBudgetForItem,
                    Quantity = x.Quantity
                }).ToList(),
                Updates = db.ContractUpdates.Where(x => x.ProcurementProjectReference == d.ID).OrderByDescending(x => x.UpdatedAt).ToList().Select(x => new ContractUpdates
                {
                    ProcurementProjectReference = x.ProcurementProjectReference,
                    ProcurementProjectStage = x.ProcurementProjectStage,
                    Remarks = x.Remarks,
                    UpdatedAt = x.UpdatedAt,
                    UpdatedBy = x.UpdatedBy == null ? "SYSTEM" : hris.GetEmployeeByCode(x.UpdatedBy).EmployeeName
                }).ToList()
            }).FirstOrDefault();
            return contract;
        }
        public NoticeOfAwardUpdateVM GetNoticeOfAwardUpdate(string ContractCode)
        {
            var contract = db.ProcurementProjects.ToList().Where(d => d.ContractCode == ContractCode).Select(d => new NoticeOfAwardUpdateVM
            {
                PAPCode = d.FKAPPReference.PAPCode,
                ClassificationReference = d.ClassificationReference,
                Classification = d.FKClassificationReference.GeneralClass + " - " + d.FKClassificationReference.Classification,
                ModeOfProcurementReference = d.ModeOfProcurementReference,
                ModeOfProcurement = d.FKModeOfProcurementReference.ModeOfProcurementName,
                FiscalYear = d.FiscalYear,
                FundSource = d.FundSource,
                FundDescription = abis.GetFundSources(d.FundSource).FUND_DESC,
                ProcurementProjectType = d.ProcurementProjectType,
                ContractStrategy = d.ContractStrategy,
                ContractCode = d.ContractCode,
                ContractName = d.ContractName,
                ContractLocation = d.ContractLocation,
                ContractStatus = d.ContractStatus,
                ProcurementProjectStage = d.ProcurementProjectStage,
                ApprovedBudgetForContract = d.ApprovedBudgetForContract,
                DeliveryPeriod = (int)d.DeliveryPeriod,
                ProjectCoordinator = hris.GetEmployeeByCode(d.ProjectCoordinator).EmployeeName,
                CreatedAt = d.CreatedAt,
                CreatedBy = hris.GetEmployeeByCode(d.CreatedBy).EmployeeName,
                ContractItems = contractDAL.GetContractItems(d.ContractCode),
                Updates = db.ContractUpdates.Where(x => x.ProcurementProjectReference == d.ID).OrderByDescending(x => x.UpdatedAt).ToList().Select(x => new ContractUpdates
                {
                    ProcurementProjectReference = x.ProcurementProjectReference,
                    ProcurementProjectStage = x.ProcurementProjectStage,
                    Remarks = x.Remarks,
                    UpdatedAt = x.UpdatedAt,
                    UpdatedBy = x.UpdatedBy == "SYSTEM" ? "SYSTEM" : hris.GetEmployeeByCode(x.UpdatedBy).EmployeeName
                }).ToList()
            }).FirstOrDefault();
            return contract;
        }
        public NoticeToProceedSetupVM GetNoticeToProceedSetup(string ContractCode)
        {
            var contract = db.ProcurementProjects.ToList().Where(d => d.ContractCode == ContractCode).Select(d => new NoticeToProceedSetupVM
            {
                PAPCode = d.FKAPPReference.PAPCode,
                ClassificationReference = d.ClassificationReference,
                Classification = d.FKClassificationReference.GeneralClass + " - " + d.FKClassificationReference.Classification,
                ModeOfProcurementReference = d.ModeOfProcurementReference,
                ModeOfProcurement = d.FKModeOfProcurementReference.ModeOfProcurementName,
                FiscalYear = d.FiscalYear,
                FundSource = d.FundSource,
                FundDescription = abis.GetFundSources(d.FundSource).FUND_DESC,
                ProcurementProjectType = d.ProcurementProjectType,
                ContractStrategy = d.ContractStrategy,
                ContractCode = d.ContractCode,
                ContractName = d.ContractName,
                ContractLocation = d.ContractLocation,
                ContractStatus = d.ContractStatus,
                ProcurementProjectStage = d.ProcurementProjectStage,
                ApprovedBudgetForContract = d.ApprovedBudgetForContract,
                DeliveryPeriod = (int)d.DeliveryPeriod,
                ProjectCoordinator = hris.GetEmployeeByCode(d.ProjectCoordinator).EmployeeName,
                CreatedAt = d.CreatedAt,
                CreatedBy = hris.GetEmployeeByCode(d.CreatedBy).EmployeeName,
                ContractItems = contractDAL.GetProcurementProjectItems(d.ContractCode),
                Updates = db.ContractUpdates.Where(x => x.ProcurementProjectReference == d.ID).OrderByDescending(x => x.UpdatedAt).ToList().Select(x => new ContractUpdates
                {
                    ProcurementProjectReference = x.ProcurementProjectReference,
                    ProcurementProjectStage = x.ProcurementProjectStage,
                    Remarks = x.Remarks,
                    UpdatedAt = x.UpdatedAt,
                    UpdatedBy = x.UpdatedBy == "SYSTEM" ? "SYSTEM" : hris.GetEmployeeByCode(x.UpdatedBy).EmployeeName
                }).ToList()
            }).FirstOrDefault();
            return contract;
        }
        public NoticeToProceedUpdateVM GetNoticeToProceedUpdate(string ContractCode)
        {
            var contract = db.ProcurementProjects.ToList().Where(d => d.ContractCode == ContractCode).Select(d => new NoticeToProceedUpdateVM
            {
                PAPCode = d.FKAPPReference.PAPCode,
                ClassificationReference = d.ClassificationReference,
                Classification = d.FKClassificationReference.GeneralClass + " - " + d.FKClassificationReference.Classification,
                ModeOfProcurementReference = d.ModeOfProcurementReference,
                ModeOfProcurement = d.FKModeOfProcurementReference.ModeOfProcurementName,
                FiscalYear = d.FiscalYear,
                FundSource = d.FundSource,
                FundDescription = abis.GetFundSources(d.FundSource).FUND_DESC,
                ProcurementProjectType = d.ProcurementProjectType,
                ContractStrategy = d.ContractStrategy,
                ContractCode = d.ContractCode,
                ContractName = d.ContractName,
                ContractLocation = d.ContractLocation,
                ContractStatus = d.ContractStatus,
                ProcurementProjectStage = d.ProcurementProjectStage,
                ApprovedBudgetForContract = d.ApprovedBudgetForContract,
                DeliveryPeriod = (int)d.DeliveryPeriod,
                ProjectCoordinator = hris.GetEmployeeByCode(d.ProjectCoordinator).EmployeeName,
                CreatedAt = d.CreatedAt,
                CreatedBy = hris.GetEmployeeByCode(d.CreatedBy).EmployeeName,
                ContractItems = contractDAL.GetContractItems(d.ContractCode),
                Updates = db.ContractUpdates.Where(x => x.ProcurementProjectReference == d.ID).OrderByDescending(x => x.UpdatedAt).ToList().Select(x => new ContractUpdates
                {
                    ProcurementProjectReference = x.ProcurementProjectReference,
                    ProcurementProjectStage = x.ProcurementProjectStage,
                    Remarks = x.Remarks,
                    UpdatedAt = x.UpdatedAt,
                    UpdatedBy = x.UpdatedBy == "SYSTEM" ? "SYSTEM" : hris.GetEmployeeByCode(x.UpdatedBy).EmployeeName
                }).ToList()
            }).FirstOrDefault();
            return contract;
        }


        public bool PostPreProcurementConferenceSetup(PreProcurementConferenceSetupVM Contract, string UserEmail)
        {
            var user = db.UserAccounts.Where(d => d.Email == UserEmail).FirstOrDefault();
            var contract = db.ProcurementProjects.Where(d => d.ContractCode == Contract.ContractCode).FirstOrDefault();
            contract.PreProcurementConferenceLocation = Contract.PreProcurementConferenceLocation;
            contract.ProcurementProjectStage = ProcurementProjectStages.PreProcurementConferenceSetup;

            db.ContractUpdates.Add(new ContractUpdates
            {
                ProcurementProjectReference = contract.ID,
                ProcurementProjectStage = ProcurementProjectStages.PreProcurementConferenceSetup,
                UpdatedAt = DateTime.Now,
                UpdatedBy = user.EmpCode,
                Remarks = "Pre-Procurement Conference for " + Contract.ContractCode + " is set up. Memorandum for End-Users/Implementing Units is now available for printing."
            });

            if (db.SaveChanges() == 0)
            {
                return false;
            }

            return true;
        }
        public bool PostPreProcurementConferenceUpdate(PreProcurementConferenceUpdateVM Contract, string UserEmail)
        {
            var user = db.UserAccounts.Where(d => d.Email == UserEmail).FirstOrDefault();
            var contract = db.ProcurementProjects.Where(d => d.ContractCode == Contract.ContractCode).FirstOrDefault();
            contract.ProcurementProjectStage = contract.ProcurementProjectType == ProcurementProjectTypes.CPB ? ProcurementProjectStages.PreProcurementConferenceUpdate : ProcurementProjectStages.GenerationOfRFQ;
            db.ContractUpdates.Add(new ContractUpdates
            {
                ProcurementProjectReference = contract.ID,
                ProcurementProjectStage = ProcurementProjectStages.PreProcurementConferenceUpdate,
                UpdatedAt = DateTime.Now,
                UpdatedBy = user.EmpCode,
                AccomplishedAt = Contract.ContractUpdate.AccomplishedAt,
                Remarks = "Pre-Procurement Conference for " + Contract.ContractCode + " was updated. Additional Information: \n" + Contract.ContractUpdate.Remarks
            });

            if (db.SaveChanges() == 0)
            {
                return false;
            }

            return true;
        }
        public bool PostPreBidConferenceSetup(PreBidConferenceSetupVM Contract, string UserEmail)
        {
            var user = db.UserAccounts.Where(d => d.Email == UserEmail).FirstOrDefault();
            var contract = db.ProcurementProjects.Where(d => d.ContractCode == Contract.ContractCode).FirstOrDefault();
            if (Contract.ProcurementProjectType == ProcurementProjectTypes.CPB)
            {
                contract.ProcurementProjectStage = ProcurementProjectStages.PreBidConferenceSetup;
                contract.PreBidConferenceLocation = Contract.PreBidConferenceLocation;
                contract.OpeningOfBidsLocation = Contract.OpeningOfBidsLocation;
                contract.PreBidVideoConferencingOptions = Contract.PreBidVideoConferencingOptions;
                contract.PreBidVideoConferenceMode = Contract.PreBidVideoConferenceMode;
                contract.PreBidVideoConferenceAccessRequestEmail = Contract.PreBidVideoConferenceAccessRequestEmail;
                contract.PreBidVideoConferenceAccessRequestContactNo = Contract.PreBidVideoConferenceAccessRequestContactNo;
                contract.PreBidAdditionalInstructions = Contract.PreBidAdditionalInstructions;
                contract.BidDocumentPrice = Contract.BidDocumentPrice;
                db.ContractUpdates.Add(new ContractUpdates
                {
                    ProcurementProjectReference = contract.ID,
                    ProcurementProjectStage = ProcurementProjectStages.PreBidConferenceSetup,
                    UpdatedAt = DateTime.Now,
                    UpdatedBy = user.EmpCode,
                    Remarks = "Pre-Bid Conference for " + Contract.ContractCode + " is set up. Invitation To Bid is now available for printing."
                });
            }
            else
            {
                contract.ProcurementProjectStage = ProcurementProjectStages.PreBidConferenceUpdate;
                contract.OpeningOfBidsLocation = Contract.OpeningOfBidsLocation;
                contract.SolicitationNo = GenerateSolicitationNo(Contract.ContractCode);
                db.ContractUpdates.Add(new ContractUpdates
                {
                    ProcurementProjectReference = contract.ID,
                    ProcurementProjectStage = ProcurementProjectStages.PreBidConferenceUpdate,
                    UpdatedAt = DateTime.Now,
                    UpdatedBy = user.EmpCode,
                    Remarks = "Pre-Bid Conference " + Contract.ContractCode + " was updated. Request for Quotation is now available for printing."
                });
            }

            if (db.SaveChanges() == 0)
            {
                return false;
            }

            return true;
        }
        public bool PostPreBidConferenceUpdate(PreBidConferenceUpdateVM Contract, string UserEmail)
        {
            var user = db.UserAccounts.Where(d => d.Email == UserEmail).FirstOrDefault();
            var contract = db.ProcurementProjects.Where(d => d.ContractCode == Contract.ContractCode).FirstOrDefault();
            contract.ProcurementProjectStage = contract.ProcurementProjectStage = ProcurementProjectStages.PreBidConferenceUpdate;
            db.ContractUpdates.Add(new ContractUpdates
            {
                ProcurementProjectReference = contract.ID,
                ProcurementProjectStage = ProcurementProjectStages.PreBidConferenceUpdate,
                UpdatedAt = DateTime.Now,
                UpdatedBy = user.EmpCode,
                AccomplishedAt = Contract.ContractUpdate.AccomplishedAt,
                Remarks = "Pre-Bid Conference " + Contract.ContractCode + " was updated. Additional Information: \n" + Contract.ContractUpdate.Remarks
            });

            if (db.SaveChanges() == 0)
            {
                return false;
            }

            return true;
        }
        public bool PostOpeningOfBidsUpdate(OpeningOfBidsUpdateVM Contract, string UserEmail)
        {
            var user = db.UserAccounts.Where(d => d.Email == UserEmail).FirstOrDefault();
            var contract = db.ProcurementProjects.Where(d => d.ContractCode == Contract.ContractCode).FirstOrDefault();
            contract.ProcurementProjectStage = Contract.FailureOfBiddingDeclared == true ? ProcurementProjectStages.BiddingFailed : ProcurementProjectStages.BidsOpened;
            contract.OpeningOfBidsFailureReason = Contract.FailureOfBiddingDeclared == true ? (BidsFailureReasons?)Contract.OpeningOfBidsFailureReason : null;
            if (Contract.FailureOfBiddingDeclared == true)
            {
                contract.ContractStatus = ProcurementProjectStatus.ContractFailed;
            }
            db.ContractUpdates.Add(new ContractUpdates
            {
                ProcurementProjectReference = contract.ID,
                ProcurementProjectStage = Contract.FailureOfBiddingDeclared == true ? ProcurementProjectStages.BiddingFailed : ProcurementProjectStages.BidsOpened,
                UpdatedAt = DateTime.Now,
                UpdatedBy = user.EmpCode,
                AccomplishedAt = Contract.FailureOfBiddingDeclared == true ? null : Contract.ContractUpdate.AccomplishedAt,
                Remarks = Contract.FailureOfBiddingDeclared == true ? "Failure of Bidding is declared. Reason: " + Contract.OpeningOfBidsFailureReason.GetAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>().Name :
                                                                      "Bids for " + Contract.ContractCode + " are opened. Additional Information:\n" + Contract.ContractUpdate.Remarks
            });

            if (db.SaveChanges() == 0)
            {
                return false;
            }

            return true;
        }
        public bool PostEvaluationOfBidsUpdate(EvaluationOfBidsUpdateVM Contract, string UserEmail)
        {
            var user = db.UserAccounts.Where(d => d.Email == UserEmail).FirstOrDefault();
            var contract = db.ProcurementProjects.Where(d => d.ContractCode == Contract.ContractCode).FirstOrDefault();
            if (contract.ProcurementProjectType == ProcurementProjectTypes.CPB)
            {
                contract.ProcurementProjectStage = ProcurementProjectStages.BidsEvaluated;
                db.ContractUpdates.Add(new ContractUpdates
                {
                    ProcurementProjectReference = contract.ID,
                    ProcurementProjectStage = ProcurementProjectStages.BidsEvaluated,
                    UpdatedAt = DateTime.Now,
                    UpdatedBy = user.EmpCode,
                    AccomplishedAt = Contract.ContractUpdate.AccomplishedAt,
                    Remarks = "Bids for " + Contract.ContractCode + " are evaluated. Additional Information:\n" + Contract.ContractUpdate.Remarks
                });
            }
            else
            {
                contract.ProcurementProjectStage = ProcurementProjectStages.PostQualification;
                db.ContractUpdates.Add(new ContractUpdates
                {
                    ProcurementProjectReference = contract.ID,
                    ProcurementProjectStage = ProcurementProjectStages.PostQualification,
                    UpdatedAt = DateTime.Now,
                    UpdatedBy = user.EmpCode,
                    AccomplishedAt = Contract.ContractUpdate.AccomplishedAt,
                    Remarks = "Bids for " + Contract.ContractCode + " are evaluated."
                });
            }


            if (db.SaveChanges() == 0)
            {
                return false;
            }

            return true;
        }
        public bool PostPostQualificationUpdate(PostQualificationUpdateVM Contract, string UserEmail)
        {
            var user = db.UserAccounts.Where(d => d.Email == UserEmail).FirstOrDefault();
            var contract = db.ProcurementProjects.Where(d => d.ContractCode == Contract.ContractCode).FirstOrDefault();
            contract.ProcurementProjectStage = Contract.FailureOfBiddingDeclared == true ? ProcurementProjectStages.BiddingFailed : ProcurementProjectStages.PostQualification;
            contract.PostQualificationFailureReason = Contract.FailureOfBiddingDeclared == true ? (BidsFailureReasons?)Contract.PostQualificationFailureReason : null;
            if (Contract.FailureOfBiddingDeclared == true)
            {
                contract.ContractStatus = ProcurementProjectStatus.ContractFailed;
            }
            db.ContractUpdates.Add(new ContractUpdates
            {
                ProcurementProjectReference = contract.ID,
                ProcurementProjectStage = Contract.FailureOfBiddingDeclared == true ? ProcurementProjectStages.BiddingFailed : ProcurementProjectStages.PostQualification,
                UpdatedAt = DateTime.Now,
                UpdatedBy = user.EmpCode,
                AccomplishedAt = Contract.FailureOfBiddingDeclared == true ? null : Contract.ContractUpdate.AccomplishedAt,
                Remarks = Contract.FailureOfBiddingDeclared == true ? "Failure of Bidding is declared. Reason: " + Contract.PostQualificationFailureReason.GetAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>().Name :
                                                                      "Bids for " + Contract.ContractCode + " are post-qualified. Additional Information:\n" + Contract.ContractUpdate.Remarks
            });

            if (db.SaveChanges() == 0)
            {
                return false;
            }

            return true;
        }
        public bool PostNoticeOfAwardSetup(NoticeOfAwardSetupVM Contract, string UserEmail)
        {
            var user = db.UserAccounts.Where(d => d.Email == UserEmail).FirstOrDefault();
            var contract = db.ProcurementProjects.Where(d => d.ContractCode == Contract.ContractCode).FirstOrDefault();
            if(Contract.FailureOfBiddingDeclared == true)
            {
                contract.ProcurementProjectStage = ProcurementProjectStages.BiddingFailed;
                contract.ContractStatus = ProcurementProjectStatus.ContractFailed;
                db.ContractUpdates.Add(new ContractUpdates
                {
                    ProcurementProjectReference = contract.ID,
                    ProcurementProjectStage = Contract.FailureOfBiddingDeclared == true ? ProcurementProjectStages.BiddingFailed : ProcurementProjectStages.PostQualification,
                    UpdatedAt = DateTime.Now,
                    UpdatedBy = user.EmpCode,
                    AccomplishedAt = DateTime.Now,
                    Remarks = "Failure of Bidding is declared. Reason: " + Contract.OpeningOfBidsFailureReason.GetAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>().Name
                });

                if (db.SaveChanges() == 0)
                {
                    return false;
                }

                return true;
            }
            contract.ProcurementProjectStage = ProcurementProjectStages.NoticeOfAwardSetup;
            db.ContractUpdates.Add(new ContractUpdates
            {
                ProcurementProjectReference = contract.ID,
                ProcurementProjectStage = ProcurementProjectStages.NoticeOfAwardSetup,
                UpdatedAt = DateTime.Now,
                UpdatedBy = user.EmpCode,
                AccomplishedAt = DateTime.Now,
                Remarks = "Notice of Award for " + Contract.ContractCode + " is setup. Notice of Award and Contract is ready for printing."
            });

            if (db.SaveChanges() == 0)
            {
                return false;
            }

            var agencyDetails = db.AgencyDetails.FirstOrDefault();
            var procurement = hris.GetDepartmentDetails(agencyDetails.ProcurementOfficeReference);
            var accounting = hris.GetDepartmentDetails(agencyDetails.AccountingOfficeReference);
            var hope = hris.GetDepartmentDetails(agencyDetails.HOPEReference);
            var contractHeader = db.Contract.Add(new ContractHeader
            {
                ProcurementProjectReference = contract.ID,
                ContractType = Contract.ContractType,
                FiscalYear = contract.FiscalYear,
                ReferenceNumber = GenerateContractReferenceNo(Contract.ContractType),
                SupplierReference = Contract.Supplier,
                CreatedAt = DateTime.Now,
                CreatedBy = user.EmpCode,
                ContractPrice = Contract.BidDetails.Sum(d => d.BidTotalPrice).Value,
                PMOffice = procurement.DepartmentCode,
                PMOHead = procurement.DepartmentHead,
                PMOHeadDesignation = procurement.DepartmentHeadDesignation,
                AccountingOffice = accounting.DepartmentCode,
                AccountingOfficeHead = accounting.DepartmentHead,
                AccountingOfficeHeadDesignation = accounting.DepartmentHeadDesignation,
                HOPEOffice = hope.SectorCode,
                HOPE = hope.SectorHead,
                HOPEDesignation = hope.SectorHeadDesignation
            });

            if (db.SaveChanges() == 0)
            {
                return false;
            }

            db.ContractDetails.AddRange(Contract.BidDetails.Select(d => new ContractDetails
            {
                ContractReference = contractHeader.ID,
                ArticleReference = d.ArticleReference,
                ItemSequence = d.ItemSequence,
                ItemFullName = d.ItemFullName,
                ItemSpecifications = d.ItemSpecifications,
                UOMReference = d.UOMReference,
                Quantity = d.Quantity,
                ContractUnitPrice = d.BidUnitPrice,
                ContractTotalPrice = d.BidTotalPrice.Value,
                Savings = d.TotalCost - d.BidTotalPrice.Value
            }).ToList());

            if (db.SaveChanges() == 0)
            {
                return false;
            }

            return true;
        }
        public bool PostNoticeOfAwardSetupLine(NoticeOfAwardSetupLineItemsVM Contract, string UserEmail)
        {
            var user = db.UserAccounts.Where(d => d.Email == UserEmail).FirstOrDefault();
            var contract = db.ProcurementProjects.Where(d => d.ContractCode == Contract.ContractCode).FirstOrDefault();
            if (Contract.FailureOfBiddingDeclared == true)
            {
                contract.ProcurementProjectStage = ProcurementProjectStages.BiddingFailed;
                contract.ContractStatus = ProcurementProjectStatus.ContractFailed;
                db.ContractUpdates.Add(new ContractUpdates
                {
                    ProcurementProjectReference = contract.ID,
                    ProcurementProjectStage = Contract.FailureOfBiddingDeclared == true ? ProcurementProjectStages.BiddingFailed : ProcurementProjectStages.PostQualification,
                    UpdatedAt = DateTime.Now,
                    UpdatedBy = user.EmpCode,
                    AccomplishedAt = DateTime.Now,
                    Remarks = "Failure of Bidding is declared. Reason: " + Contract.OpeningOfBidsFailureReason.GetAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>().Name
                });

                if (db.SaveChanges() == 0)
                {
                    return false;
                }

                return true;
            }
            contract.ProcurementProjectStage = ProcurementProjectStages.NoticeOfAwardSetup;
            db.ContractUpdates.Add(new ContractUpdates
            {
                ProcurementProjectReference = contract.ID,
                ProcurementProjectStage = ProcurementProjectStages.NoticeOfAwardSetup,
                UpdatedAt = DateTime.Now,
                UpdatedBy = user.EmpCode,
                AccomplishedAt = DateTime.Now,
                Remarks = "Notice of Award for " + Contract.ContractCode + " is setup. Notice of Award and Contract is ready for printing."
            });

            if (db.SaveChanges() == 0)
            {
                return false;
            }

            var agencyDetails = db.AgencyDetails.FirstOrDefault();
            var procurement = hris.GetDepartmentDetails(agencyDetails.ProcurementOfficeReference);
            var accounting = hris.GetDepartmentDetails(agencyDetails.AccountingOfficeReference);
            var hope = hris.GetDepartmentDetails(agencyDetails.HOPEReference);
            var suppliers = Contract.LineItems.Select(d => d.Supplier).Distinct().ToList();
            foreach(var supplier in suppliers)
            {
                var items = Contract.LineItems.Where(d => d.Supplier == supplier).ToList();
                var contractHeader = db.Contract.Add(new ContractHeader
                {
                    ProcurementProjectReference = contract.ID,
                    ContractType = Contract.ContractType,
                    FiscalYear = contract.FiscalYear,
                    ReferenceNumber = GenerateContractReferenceNo(Contract.ContractType),
                    SupplierReference = supplier,
                    CreatedAt = DateTime.Now,
                    CreatedBy = user.EmpCode,
                    ContractPrice = items.Sum(d => d.BidTotalPrice).Value,
                    PMOffice = procurement.DepartmentCode,
                    PMOHead = procurement.DepartmentHead,
                    PMOHeadDesignation = procurement.DepartmentHeadDesignation,
                    AccountingOffice = accounting.DepartmentCode,
                    AccountingOfficeHead = accounting.DepartmentHead,
                    AccountingOfficeHeadDesignation = accounting.DepartmentHeadDesignation,
                    HOPEOffice = hope.SectorCode,
                    HOPE = hope.SectorHead,
                    HOPEDesignation = hope.SectorHeadDesignation
                });

                if (db.SaveChanges() == 0)
                {
                    return false;
                }

                db.ContractDetails.AddRange(items.Select(d => new ContractDetails
                {
                    ContractReference = contractHeader.ID,
                    ArticleReference = d.ArticleReference,
                    ItemSequence = d.ItemSequence,
                    ItemFullName = d.ItemFullName,
                    ItemSpecifications = d.ItemSpecifications,
                    UOMReference = d.UOMReference,
                    Quantity = d.Quantity,
                    ContractUnitPrice = d.BidUnitPrice,
                    ContractTotalPrice = d.BidTotalPrice.Value,
                    Savings = d.TotalCost - d.BidTotalPrice.Value
                }).ToList());

                if (db.SaveChanges() == 0)
                {
                    return false;
                }
            }

            return true;
        }
        public bool PostNoticeOfAwardUpdate(NoticeOfAwardUpdateVM Contract, string UserEmail)
        {
            var user = db.UserAccounts.Where(d => d.Email == UserEmail).FirstOrDefault();
            var contract = db.ProcurementProjects.Where(d => d.ContractCode == Contract.ContractCode).FirstOrDefault();
            contract.ProcurementProjectStage = ProcurementProjectStages.NoticeOfAwardUpdate;
            contract.NOAAcceptedAt = Contract.NOAAcceptedAt;

            db.ContractUpdates.Add(new ContractUpdates
            {
                ProcurementProjectReference = contract.ID,
                ProcurementProjectStage = ProcurementProjectStages.NoticeOfAwardUpdate,
                UpdatedAt = DateTime.Now,
                UpdatedBy = user.EmpCode,
                AccomplishedAt = Contract.NOAAcceptedAt,
                Remarks = "Notice of Award for " + Contract.ContractCode + " is updated. Contract Agreement Form is ready for printing."
            });

            if (db.SaveChanges() == 0)
            {
                return false;
            }

            return true;
        }
        public bool PostNoticeToProceedSetup(NoticeToProceedSetupVM Contract, string UserEmail)
        {
            var user = db.UserAccounts.Where(d => d.Email == UserEmail).FirstOrDefault();
            var contract = db.ProcurementProjects.Where(d => d.ContractCode == Contract.ContractCode).FirstOrDefault();
            contract.ProcurementProjectStage = ProcurementProjectStages.NoticeToProceedSetup;
            contract.ContractSignedAt = Contract.ContractSignedAt;
            db.ContractUpdates.Add(new ContractUpdates
            {
                ProcurementProjectReference = contract.ID,
                ProcurementProjectStage = ProcurementProjectStages.NoticeToProceedSetup,
                UpdatedAt = DateTime.Now,
                UpdatedBy = user.EmpCode,
                AccomplishedAt = Contract.ContractSignedAt,
                Remarks = "Notice to Proceed for " + Contract.ContractCode + " is setup. Notice to Proceed is ready for printing."
            });

            if (db.SaveChanges() == 0)
            {
                return false;
            }

            return true;
        }
        public bool PostNoticeToProceedUpdate(NoticeToProceedUpdateVM Contract, string UserEmail)
        {
            var user = db.UserAccounts.Where(d => d.Email == UserEmail).FirstOrDefault();
            var procurementProject = db.ProcurementProjects.Where(d => d.ContractCode == Contract.ContractCode).FirstOrDefault();
            var contract = db.Contract.Where(d => d.FKProcurementProjectReference.ContractCode == Contract.ContractCode).FirstOrDefault();
            procurementProject.ProcurementProjectStage = ProcurementProjectStages.ProcurementClosed;
            procurementProject.ContractStatus = ProcurementProjectStatus.ContractProcurementClosed;
            procurementProject.NTPSignedAt = Contract.NTPSignedAt;
            contract.ContractStatus = ContractStatus.NTPSignedAndReceived;
            contract.CommencedAt = Contract.EffectivityAt;
            contract.DeliveryDeadline = Contract.EffectivityAt.AddDays(procurementProject.DeliveryPeriod.Value);
            db.ContractUpdates.Add(new ContractUpdates
            {
                ProcurementProjectReference = procurementProject.ID,
                ProcurementProjectStage = ProcurementProjectStages.NoticeToProceedSetup,
                UpdatedAt = DateTime.Now,
                UpdatedBy = user.EmpCode,
                AccomplishedAt = Contract.NTPSignedAt,
                Remarks = "Notice to Proceed for " + Contract.ContractCode + " is updated. Additional Information: " + Contract.ContractUpdate.Remarks
            });

            db.ContractUpdates.Add(new ContractUpdates
            {
                ProcurementProjectReference = procurementProject.ID,
                ProcurementProjectStage = ProcurementProjectStages.ProcurementClosed,
                UpdatedAt = DateTime.Now,
                UpdatedBy = "SYSTEM",
                AccomplishedAt = Contract.NTPSignedAt,
                Remarks = "Procurement for Project " + Contract.ContractCode + " has been closed. Contract for this project is now up for monitoring"
            });

            db.ContractMonitoringUpdates.Add(new ContractMonitoringUpdates
            {
                ContractReference = contract.ID,
                ContractStatus = ContractStatus.NTPSignedAndReceived,
                UpdatedAt = DateTime.Now,
                UpdatedBy = "SYSTEM",
                AccomplishedAt = Contract.NTPSignedAt,
                Remarks = "Noticed to Proceed for Contract " + Contract.ContractCode + " has been signed and returned by the Supplier/Contractor. Contract is now up for monitoring."
            });

            if (db.SaveChanges() == 0)
            {
                return false;
            }

            return true;
        }

        public List<PreProcurementConferenceTemplateVM> GetPreProcMemoOffices(string ContractCode, string UserEmail)
        {
            var user = db.UserAccounts.Where(d => d.Email == UserEmail).FirstOrDefault();
            var contract = db.ProcurementProjects.Where(d => d.ContractCode == ContractCode).FirstOrDefault();
            var ppmpDetails = db.PPMPDetails.Where(d => d.FKProcurementProject.ContractCode == ContractCode).ToList();
            var endUsers = new List<string>();
            endUsers = ppmpDetails.ToList().Select(d => new
            {
                EndUser = d.FKAPPDetailReference.EndUser == "Various Offices" ? d.FKPPMPHeaderReference.Department : d.FKAPPDetailReference.EndUser
            }).GroupBy(d => d.EndUser).Select(d => d.Key).ToList();

            var preProcurementOffices = new List<PreProcurementConferenceTemplateVM>();
            foreach (var endUser in endUsers)
            {
                var office = hris.GetDepartmentDetails(endUser);
                preProcurementOffices.Add(new PreProcurementConferenceTemplateVM
                {
                    PreProcurementConferenceDate = contract.PreProcurementConference,
                    PreProcurementConferenceLocation = contract.PreProcurementConferenceLocation,
                    ProjectCoordinator = hris.GetEmployeeDetailByCode(contract.ProjectCoordinator).EmployeeName,
                    Department = office.Department,
                    Unit = office.Section,
                    DepartmentHead = office.DepartmentHead,
                    DepartmentHeadLastName = office.DepartmentHeadLastName,
                    Designation = office.DepartmentHeadDesignation,
                    ContractCode = contract.ContractCode,
                    ContractName = contract.ContractName
                });
            }
            return preProcurementOffices;
        }
        public InvitationToBidTemplateVM InvitationToBidSetup(string ContractCode)
        {
            return db.ProcurementProjects.Where(d => d.ContractCode == ContractCode).ToList().Select(d => new InvitationToBidTemplateVM
            {
                Date = (DateTime)db.ContractUpdates.Where(x => x.ProcurementProjectReference == d.ID && x.ProcurementProjectStage == ProcurementProjectStages.PreBidConferenceSetup).Select(x => x.UpdatedAt).FirstOrDefault(),
                FiscalYear = d.FiscalYear,
                Classification = d.FKClassificationReference.GeneralClass,
                ApprovedBudgetForContract = d.ApprovedBudgetForContract,
                ApprovedBudgetForContractWords = Reports.AmountToWords(d.ApprovedBudgetForContract),
                FundDescription = abis.GetFundSources(d.FundSource).FUND_DESC,
                ContractCode = d.ContractCode,
                ContractName = d.ContractName,
                ContractStrategy = d.ContractStrategy,
                DeliveryPeriod = (int)d.DeliveryPeriod,
                DeliveryPeriodWords = Reports.DigitsToWords((int)d.DeliveryPeriod),
                SubmissionStart = d.PostingOfIB_RFQPosting.Value.AddDays(1),
                DeadlineOfSubmissionOfBids = (DateTime)d.DeadlineOfSubmissionOfBids_DeadlineOfSubmisionOfBids,
                OpeningOfBids = (DateTime)d.OpeningOfBids_OpeningOfQuotations,
                OpeningOfBidsLocation = d.OpeningOfBidsLocation,
                BidDocumentPrice = (decimal)d.BidDocumentPrice,
                BidDocumentPriceWords = Reports.AmountToWords((decimal)d.BidDocumentPrice),
                PreBidConference = (DateTime)d.PreBidConference,
                PreBidConferenceLocation = d.PreBidConferenceLocation,
                PreBidVideoConferencingOptions = (VideoConferencingOptions)d.PreBidVideoConferencingOptions,
                PreBidVideoConferenceMode = d.PreBidVideoConferenceMode,
                PreBidVideoConferenceAccessRequestEmail = d.PreBidVideoConferenceAccessRequestEmail,
                PreBidVideoConferenceAccessRequestContactNo = d.PreBidVideoConferenceAccessRequestContactNo,
                PreBidAdditionalInstructions = d.PreBidAdditionalInstructions
            }).FirstOrDefault();
        }
        public List<NoticeOfAwardTemplateVM> NoticeOfAwardSetup(string ContractCode)
        {
            return db.Contract.Where(d => d.FKProcurementProjectReference.ContractCode == ContractCode).ToList().Select(d => new NoticeOfAwardTemplateVM
            {
                Supplier = d.FKSupplierReference.SupplierName,
                Address = d.FKSupplierReference.Address,
                City = (d.FKSupplierReference.City == string.Empty ? string.Empty : d.FKSupplierReference.City),
                State = (d.FKSupplierReference.State == string.Empty ? string.Empty : d.FKSupplierReference.State),
                PostalCode = (d.FKSupplierReference.PostalCode == string.Empty ? string.Empty : d.FKSupplierReference.PostalCode),
                ContactPerson = d.FKSupplierReference.ContactPerson,
                ContactPersonDesignation = d.FKSupplierReference.ContactPersonDesignation,
                AuthorizedAgent = d.FKSupplierReference.AuthorizedAgent,
                AuthorizedDesignation = d.FKSupplierReference.AuthorizedDesignation,
                ContractCode = d.FKProcurementProjectReference.ContractCode,
                ContractName = d.FKProcurementProjectReference.ContractName,
                ContractPrice = d.ContractPrice,
                ContractPriceWords = Reports.AmountToWords(d.ContractPrice),
                ModeOfProcurement = d.FKProcurementProjectReference.FKModeOfProcurementReference.ModeOfProcurementName
            }).ToList();
        }
        public RequestForQuotationTemplateVM RequestForQuotationSetup(string ContractCode)
        {
            var quotationNo = db.PPMPDetails.Where(d => d.FKProcurementProject.ContractCode == ContractCode).ToList()
                    .Select(d => String.Join(", ", d.FKPurchaseRequestReference.PRNumber)).FirstOrDefault();
            return db.ProcurementProjects.Where(d => d.ContractCode == ContractCode).ToList().Select(d => new RequestForQuotationTemplateVM
            {
                FiscalYear = d.FiscalYear,
                ContractCode = d.ContractCode,
                ContractName = d.ContractName,
                ABC = d.ApprovedBudgetForContract,
                ModeOfProcurement = d.FKModeOfProcurementReference.ModeOfProcurementName,
                SolicitationNo = d.SolicitationNo,
                Deadline = d.DeadlineOfSubmissionOfBids_DeadlineOfSubmisionOfBids.Value,
                QuotationNo = quotationNo,
                Date = DateTime.Now,
                Details = db.ProcurementProjectDetails.Where(x => x.ProcurementProjectReference == d.ID).ToList()
                             .Select(x => new RequestForQuotationDetailsVM
                             {
                                 ArticleReference = x.ArticleReference,
                                 ItemSequence = x.ItemSequence,
                                 ItemFullName = x.ItemFullName,
                                 ItemSpecifications = x.ItemSpecifications == null ? string.Empty : x.ItemSpecifications,
                                 UnitOfMeasure = x.FKUOMReference.Abbreviation,
                                 UOMReference = x.UOMReference,
                                 Quantity = x.Quantity
                             }).ToList()
            }).FirstOrDefault();
        }
        public List<NoticeToProceedTemplateVM> NoticeToProceedSetup(string ContractCode)
        {
            return db.Contract.ToList().Where(d => d.FKProcurementProjectReference.ContractCode == ContractCode).Select(d => new NoticeToProceedTemplateVM
            {
                Supplier = d.FKSupplierReference.SupplierName,
                Address = d.FKSupplierReference.Address,
                City = (d.FKSupplierReference.City == string.Empty ? string.Empty : d.FKSupplierReference.City),
                State = (d.FKSupplierReference.State == string.Empty ? string.Empty : d.FKSupplierReference.State),
                PostalCode = (d.FKSupplierReference.PostalCode == string.Empty ? string.Empty : d.FKSupplierReference.PostalCode),
                ContactPerson = d.FKSupplierReference.ContactPerson,
                ContactPersonDesignation = d.FKSupplierReference.ContactPersonDesignation,
                AuthorizedAgent = d.FKSupplierReference.AuthorizedAgent,
                AuthorizedDesignation = d.FKSupplierReference.AuthorizedDesignation,
                ContractCode = d.FKProcurementProjectReference.ContractCode,
                ContractName = d.FKProcurementProjectReference.ContractName,
                ContractPrice = d.ContractPrice,
                ContractPriceWords = Reports.AmountToWords(d.ContractPrice),
                ModeOfProcurement = d.FKProcurementProjectReference.FKModeOfProcurementReference.ModeOfProcurementName,
                PurchaseOrderNo = d.ReferenceNumber,
                PurchaseOrderDate = d.CreatedAt,
                Delivery = d.FKProcurementProjectReference.DeliveryPeriod.Value,
                DeliveryWords = Reports.DigitsToWords(d.FKProcurementProjectReference.DeliveryPeriod.Value)
            }).ToList();
        }
        public List<PurchaseOrderTemplateVM> PurchaseOrderSetup(string ContractCode)
        {
            return db.Contract.ToList().Where(d => d.FKProcurementProjectReference.ContractCode == ContractCode && d.ContractType == ContractTypes.PurchaseOrder)
                .Select(d => new PurchaseOrderTemplateVM
                {
                    FiscalYear = d.FiscalYear,
                    ContractCode = d.FKProcurementProjectReference.ContractCode,
                    ContractName = d.FKProcurementProjectReference.ContractName,
                    PurchaseOrderNumber = d.ReferenceNumber,
                    PlaceOfDelivery = d.FKProcurementProjectReference.ContractLocation,
                    FundSource = abis.GetFundSources(d.FKProcurementProjectReference.FundSource).FUND_DESC.Replace("\r\n", string.Empty) + " (" + d.FKProcurementProjectReference.FundSource.Replace("\r\n", string.Empty) + ")",
                    SupplierName = d.FKSupplierReference.SupplierName,
                    SupplierAddress = d.FKSupplierReference.Address + (d.FKSupplierReference.City == null || d.FKSupplierReference.City == string.Empty ? string.Empty : ", " + d.FKSupplierReference.City) + (d.FKSupplierReference.State == null || d.FKSupplierReference.State == string.Empty ? string.Empty : ", " + d.FKSupplierReference.State) + (d.FKSupplierReference.PostalCode == null || d.FKSupplierReference.PostalCode == string.Empty ? string.Empty : ", " + d.FKSupplierReference.PostalCode),
                    SupplierTIN = d.FKSupplierReference.TaxIdNumber,
                    SupplierRepresentative = d.FKSupplierReference.ContactPerson,
                    SupplierRepresentativeDesignation = d.FKSupplierReference.ContactPersonDesignation,
                    ModeOfProcurement = d.FKProcurementProjectReference.FKModeOfProcurementReference.ModeOfProcurementName,
                    CreatedAt = d.CreatedAt,
                    TotalAmount = d.ContractPrice,
                    DeliveryPeriod = d.FKProcurementProjectReference.DeliveryPeriod.Value,
                    PMOffice = d.PMOffice,
                    PMOHead = d.PMOHead,
                    PMOHeadDesignation = d.PMOHeadDesignation,
                    AccountingOffice = hris.GetDepartmentDetails(d.AccountingOffice).Department,
                    AccountingOfficeHead = d.AccountingOfficeHead,
                    AccountingOfficeHeadDesignation = d.AccountingOfficeHeadDesignation,
                    HOPEOffice = d.HOPEOffice,
                    HOPE = d.HOPE,
                    HOPEDesignation = d.HOPEDesignation,
                    Details = db.ContractDetails.Where(x => x.ContractReference == d.ID).Select(x => new PurchaseOrderDetailsVM {
                        ArticleReference = x.ArticleReference,
                        ItemSequence = x.ItemSequence,
                        ItemFullName = x.ItemFullName,
                        ItemSpecifications = x.ItemSpecifications,
                        UnitOfMeasure = x.FKUOMReference.Abbreviation,
                        UOMReference = x.UOMReference,
                        Quantity = x.Quantity,
                        UnitCost = x.ContractUnitPrice,
                        TotalCost = x.ContractTotalPrice
                    }).ToList()
                }).ToList();
        }

        private string GenerateSolicitationNo(string ContractCode)
        {
            string solicitationNo = string.Empty;
            var contract = db.ProcurementProjects.Where(d => d.ContractCode == ContractCode).FirstOrDefault();
            var modeOfProcurement = db.ProcurementModes.Find(contract.ModeOfProcurementReference).ShortName;
            var series = (db.ProcurementProjects.Where(d => d.FiscalYear == contract.FiscalYear && d.SolicitationNo != null).Count() + 1).ToString();
            solicitationNo = "RFQ-" + modeOfProcurement + "-" + contract.FiscalYear.ToString() + "-" + (series.Length == 1 ? "00" + series : series.Length == 2 ? "0" + series : series);
            return solicitationNo;
        }
        private string GenerateContractReferenceNo(ContractTypes ContractType)
        {
            var series = (db.Contract.Where(d => d.ContractType == ContractType).Count() + 1).ToString();
            var referenceNo = ContractType.GetAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>().ShortName + "-" + DateTime.Now.ToString("yy") + "-" + DateTime.Now.ToString("MM") + (series.Length == 1 ? "-000" + series : series.Length == 2 ? "-00" + series : series.Length == 3 ? "-0" + series : series);
            return referenceNo;
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