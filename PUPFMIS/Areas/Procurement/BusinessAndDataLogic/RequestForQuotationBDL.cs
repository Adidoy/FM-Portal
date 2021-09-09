//using MigraDoc.DocumentObjectModel;
//using MigraDoc.DocumentObjectModel.Tables;
//using PUPFMIS.Models;
//using PUPFMIS.Models.AIS;
//using PUPFMIS.Models.HRIS;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Web.Mvc;

//namespace PUPFMIS.BusinessAndDataLogic
//{
//    public class RequestForQuotationBL : Controller
//    {
//        private RequestForQuotationDAL rfqDAL = new RequestForQuotationDAL();
//        private FMISDbContext db = new FMISDbContext();
//        private SystemBDL system = new SystemBDL();

//        public MemoryStream PrintRFQ(string LogoPath, string ReferenceNo)
//        {
//            var rfq = rfqDAL.RequestForQuotationDetailsForPosting(ReferenceNo);
//            var agencyDetails = db.AgencyDetails.FirstOrDefault();

//            Reports reports = new Reports();
//            reports.ReportFilename = ReferenceNo;
//            reports.CreateDocument(8.50, 13.00, Orientation.Portrait, 1.00);
//            reports.AddDoubleColumnHeader(LogoPath, false, 1.25);
//            reports.AddPageNumbersFooter();
//            reports.AddColumnHeader(
//                new HeaderLine { Content = "Republic of the Philippines", Bold = false, Italic = false, FontSize = 8, ParagraphAlignment = ParagraphAlignment.Left },
//                new HeaderLine { Content = "PUP-REFQ-6-PRMO-005", Bold = false, Italic = false, FontSize = 6, ParagraphAlignment = ParagraphAlignment.Left }
//            );
//            reports.AddColumnHeader(
//                new HeaderLine { Content = "POLYTECHNIC UNIVERSITY OF THE PHILIPPINES", Bold = true, Italic = false, FontSize = 12, ParagraphAlignment = ParagraphAlignment.Left },
//                new HeaderLine { Content = "Rev. 1", Bold = false, Italic = false, FontSize = 7, ParagraphAlignment = ParagraphAlignment.Left }
//            );
//            reports.AddColumnHeader(
//                new HeaderLine { Content = "A. Mabini Campus, Anonas St., Santa Mesa, Manila\t1016", Bold = false, Italic = false, FontSize = 10, ParagraphAlignment = ParagraphAlignment.Left },
//                new HeaderLine { Content = "February 16, 2021", Bold = false, Italic = false, FontSize = 7, ParagraphAlignment = ParagraphAlignment.Left }
//            );
//            reports.AddColumnHeader(
//                new HeaderLine { Content = "THE COUNTRY'S FIRST POLYTECHNIC UNIVERSITY", Bold = false, Italic = false, FontSize = 10, ParagraphAlignment = ParagraphAlignment.Left },
//                new HeaderLine { Content = "", Bold = true, Italic = false, FontSize = 5, ParagraphAlignment = ParagraphAlignment.Left }
//            );

//            var columns = new List<ContentColumn>();
//            columns.Add(new ContentColumn(6.50));
//            reports.AddTable(columns, false);

//            reports.AddFormattedRowContent(new ContentCell(new TextWithFormat[]
//            {
//               new TextWithFormat("REQUEST FOR QUOTATION", true, false, 14, Underline.Single),
//            }, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center), 0.00);

//            reports.AddNewLine();
//            reports.AddNewLine();

//            columns = new List<ContentColumn>();
//            columns.Add(new ContentColumn(6.50));
//            reports.AddTable(columns, false);

//            var figureWords = Reports.AmountToWords(rfq.ABC);

//            reports.AddFormattedRowContent(new ContentCell(new TextWithFormat[]
//            {
//               new TextWithFormat("The ", false, false, 11),
//               new TextWithFormat("Polytechnic University of the Philippines (PUP) - Bids and Awards Committee (BAC)", true, false, 11),
//               new TextWithFormat(" will undertake a ", false, false, 11),
//               new TextWithFormat(" " + rfq.ModeOfProcurement + " ", true, true, 11),
//               new TextWithFormat(" for FY ", false, false, 11),
//               new TextWithFormat(" "+ rfq.FiscalYear +" ", true, false, 11),
//               new TextWithFormat(" in accordance with Section 53 of the 2016 Revised Implementing Rules and Regulations of Republic Act No. 9184. ", false, false, 11),
//               new TextWithFormat(" The Approved Budget for the Contract (ABC) is ", false, false, 11),
//               new TextWithFormat(" " + figureWords.ToUpper() + " (" + rfq.ABC.ToString("C", new System.Globalization.CultureInfo("en-ph")) + ")", true, true, 11),
//               new TextWithFormat(".", false, false, 11),
//            }, new Unit(0.50, UnitType.Inch), MigraDoc.DocumentObjectModel.ParagraphAlignment.Justify), 0.00);

//            reports.AddNewLine();
//            reports.AddNewLine();

//            columns = new List<ContentColumn>();
//            columns.Add(new ContentColumn(6.50));
//            reports.AddTable(columns, false);

//            reports.AddFormattedRowContent(new ContentCell(new TextWithFormat[]
//            {
//               new TextWithFormat("Please quote your ", false, false, 11),
//               new TextWithFormat(" best offer ", true, false, 11),
//               new TextWithFormat(" for the item described herein, subject to the Terms and Conditions provided at the last page of this Request for Quotation. ", false, false, 11),
//               new TextWithFormat(" Submit your quotation duly signed by you or your duly authorized representative not later than ", false, false, 11),
//               new TextWithFormat(rfq.Deadline.ToString("dd MMMM yyyy hh:mm tt"), true, false, 11, Underline.Single),
//               new TextWithFormat(" at the ", false, false, 11),
//               new TextWithFormat("Procurement Management Office", true, false, 11),
//               new TextWithFormat(", Ground Floor North Wing, PUP A. Mabini Campus, Anonas St., Santa Mesa, Manila. Open submission may be submitted, manually or through email at the address and contact numbers indicated below. ", false, false, 11),
//            }, new Unit(0.50, UnitType.Inch), MigraDoc.DocumentObjectModel.ParagraphAlignment.Justify), 0.00);

//            reports.AddNewLine();
//            reports.AddNewLine();

//            columns = new List<ContentColumn>();
//            columns.Add(new ContentColumn(6.50));
//            reports.AddTable(columns, false);

//            reports.AddFormattedRowContent(new ContentCell(new TextWithFormat[]
//            {
//               new TextWithFormat("Copies of the following documents, in its valid and updated versions, are required to be submitted along with your signed quotation to wit: ", false, false, 11)
//            }, new Unit(0.50, UnitType.Inch), MigraDoc.DocumentObjectModel.ParagraphAlignment.Justify), 0.00);

//            reports.AddNewLine();

//            columns = new List<ContentColumn>();
//            columns.Add(new ContentColumn(1.00));
//            columns.Add(new ContentColumn(0.20));
//            columns.Add(new ContentColumn(5.30));
//            reports.AddTable(columns, false);

//            var rows = new List<ContentCell>();
//            rows.Add(new ContentCell("", 0, 8, false, false, ParagraphAlignment.Left, VerticalAlignment.Center));
//            rows.Add(new ContentCell("1. ", 1, 11, false, false, ParagraphAlignment.Right, VerticalAlignment.Top));
//            rows.Add(new ContentCell("Business or Mayor's Permit;", 2, 11, false, false, ParagraphAlignment.Left, VerticalAlignment.Top));
//            reports.AddRowContent(rows, 0.25);

//            rows = new List<ContentCell>();
//            rows.Add(new ContentCell("", 0, 8, false, false, ParagraphAlignment.Left, VerticalAlignment.Center));
//            rows.Add(new ContentCell("2. ", 1, 11, false, false, ParagraphAlignment.Right, VerticalAlignment.Top));
//            rows.Add(new ContentCell("PhilGEPS Registration;", 2, 11, false, false, ParagraphAlignment.Left, VerticalAlignment.Top));
//            reports.AddRowContent(rows, 0.25);

//            rows = new List<ContentCell>();
//            rows.Add(new ContentCell("", 0, 8, false, false, ParagraphAlignment.Left, VerticalAlignment.Center));
//            rows.Add(new ContentCell("3. ", 1, 11, false, false, ParagraphAlignment.Right, VerticalAlignment.Top));
//            rows.Add(new ContentCell("Income or Business Tax Return;", 2, 11, false, false, ParagraphAlignment.Left, VerticalAlignment.Top));
//            reports.AddRowContent(rows, 0.25);

//            rows = new List<ContentCell>();
//            rows.Add(new ContentCell("", 0, 8, false, false, ParagraphAlignment.Left, VerticalAlignment.Center));
//            rows.Add(new ContentCell("4. ", 1, 11, false, false, ParagraphAlignment.Right, VerticalAlignment.Top));
//            rows.Add(new ContentCell("SEC or DTI Registration;", 2, 11, false, false, ParagraphAlignment.Left, VerticalAlignment.Top));
//            reports.AddRowContent(rows, 0.25);

//            rows = new List<ContentCell>();
//            rows.Add(new ContentCell("", 0, 8, false, false, ParagraphAlignment.Left, VerticalAlignment.Center));
//            rows.Add(new ContentCell("5. ", 1, 11, false, false, ParagraphAlignment.Right, VerticalAlignment.Top));
//            rows.Add(new ContentCell("Tax Clearance;", 2, 11, false, false, ParagraphAlignment.Left, VerticalAlignment.Top));
//            reports.AddRowContent(rows, 0.25);

//            rows = new List<ContentCell>();
//            rows.Add(new ContentCell("", 0, 8, false, false, ParagraphAlignment.Left, VerticalAlignment.Center));
//            rows.Add(new ContentCell("6. ", 1, 11, false, false, ParagraphAlignment.Right, VerticalAlignment.Top));
//            rows.Add(new ContentCell("Notarized Omnibus Sworn Statement for this particular procurement project;", 2, 11, false, false, ParagraphAlignment.Left, VerticalAlignment.Top));
//            reports.AddRowContent(rows, 0.25);

//            rows = new List<ContentCell>();
//            rows.Add(new ContentCell("", 0, 11, false, false, ParagraphAlignment.Left, VerticalAlignment.Center));
//            rows.Add(new ContentCell("7. ", 1, 11, false, false, ParagraphAlignment.Right, VerticalAlignment.Top));
//            rows.Add(new ContentCell(new TextWithFormat[]
//            {
//                            new TextWithFormat("Professional License (", false, false, 11),
//                            new TextWithFormat("for Infrastructure", false, true, 11),
//                            new TextWithFormat(") or Curriculum Vitae (", false, false, 11),
//                            new TextWithFormat("for Consulting Services", false, true, 11),
//                            new TextWithFormat(");", false, false, 11),
//            }, 2, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, false, false));
//            reports.AddRowContent(rows, 0.45);

//            rows = new List<ContentCell>();
//            rows.Add(new ContentCell("", 0, 8, false, false, ParagraphAlignment.Left, VerticalAlignment.Center));
//            rows.Add(new ContentCell("8. ", 1, 11, false, false, ParagraphAlignment.Right, VerticalAlignment.Top));
//            rows.Add(new ContentCell(new TextWithFormat[]
//            {
//                            new TextWithFormat("PCAB License (", false, false, 11),
//                            new TextWithFormat("for Infrastructure", false, true, 11),
//                            new TextWithFormat("); and", false, false, 11),
//            }, 2, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, false, false));
//            reports.AddRowContent(rows, 0.25);

//            rows = new List<ContentCell>();
//            rows.Add(new ContentCell("", 0, 8, false, false, ParagraphAlignment.Left, VerticalAlignment.Center));
//            rows.Add(new ContentCell("9. ", 1, 11, false, false, ParagraphAlignment.Right, VerticalAlignment.Top));
//            rows.Add(new ContentCell(new TextWithFormat[]
//            {
//                            new TextWithFormat("Certification of Product Registration from BFAD (", false, false, 11),
//                            new TextWithFormat("for Drugs and Medicines", false, true, 11),
//                            new TextWithFormat(")", false, false, 11),
//            }, 2, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, false, false));
//            reports.AddRowContent(rows, 0.25);

//            reports.AddNewLine();
//            reports.AddNewLine();

//            columns = new List<ContentColumn>();
//            columns.Add(new ContentColumn(6.50));
//            reports.AddTable(columns, false);

//            reports.AddFormattedRowContent(new ContentCell(new TextWithFormat[]
//            {
//               new TextWithFormat("Failure to submit any or all of the foregoing documents may result to the automatic disqualification of the quotation.", false, false, 11)
//            }, new Unit(0.50, UnitType.Inch), MigraDoc.DocumentObjectModel.ParagraphAlignment.Justify), 0.00);

//            reports.AddNewLine();
//            reports.AddNewLine();

//            columns = new List<ContentColumn>();
//            columns.Add(new ContentColumn(6.50));
//            reports.AddTable(columns, false);

//            reports.AddFormattedRowContent(new ContentCell(new TextWithFormat[]
//            {
//               new TextWithFormat("For inquiries, feel free to contact us at ", false, false, 11),
//               new TextWithFormat(" (02) 8713-1504 ", true, false, 11),
//               new TextWithFormat(" or email us at ", false, false, 11),
//               new TextWithFormat(" procurementoffice@pup.edu.ph.", false, true, 11, Underline.Single)
//            }, new Unit(0.50, UnitType.Inch), MigraDoc.DocumentObjectModel.ParagraphAlignment.Justify), 0.00);

//            reports.AddNewLine();
//            reports.AddNewLine();
//            reports.AddNewLine();
//            reports.AddNewLine();
//            reports.AddNewLine();

//            columns = new List<ContentColumn>();
//            columns.Add(new ContentColumn(3.00));
//            columns.Add(new ContentColumn(3.00));
//            columns.Add(new ContentColumn(0.50));
//            reports.AddTable(columns, false);

//            rows = new List<ContentCell>();
//            rows.Add(new ContentCell("", 0, 8, false, false, ParagraphAlignment.Left, VerticalAlignment.Center));
//            rows.Add(new ContentCell("DR. EMANUEL C. DE GUZMAN", 1, 11, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
//            rows.Add(new ContentCell("", 2, 11, false, false, ParagraphAlignment.Left, VerticalAlignment.Top));
//            reports.AddRowContent(rows, 0.25);

//            rows = new List<ContentCell>();
//            rows.Add(new ContentCell("", 0, 8, false, false, ParagraphAlignment.Left, VerticalAlignment.Center));
//            rows.Add(new ContentCell("BAC Chairman", 1, 11, false, false, ParagraphAlignment.Center, VerticalAlignment.Center));
//            rows.Add(new ContentCell("", 2, 11, false, false, ParagraphAlignment.Left, VerticalAlignment.Top));
//            reports.AddRowContent(rows, 0.25);

//            reports.InsertPageBreak();

//            columns = new List<ContentColumn>();
//            columns.Add(new ContentColumn(6.50));
//            reports.AddTable(columns, false);

//            rows = new List<ContentCell>();
//            rows.Add(new ContentCell("\n", 0, 8, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0,0,0,0), true, true, true));
//            reports.AddRowContent(rows, 0.00);

//            columns = new List<ContentColumn>();
//            columns.Add(new ContentColumn(2.25));
//            columns.Add(new ContentColumn(2.00));
//            columns.Add(new ContentColumn(2.25));
//            reports.AddTable(columns, false);

//            rows = new List<ContentCell>();
//            rows.Add(new ContentCell("", 0, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), false, false, true));
//            rows.Add(new ContentCell("TERMS AND CONDITIONS", 1, 10, true, false, ParagraphAlignment.Center, VerticalAlignment.Bottom, 0, 0, true, new Color(0,0,0,0), false, false, false));
//            rows.Add(new ContentCell("", 2, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), true, false, false));
//            reports.AddRowContent(rows, 0.00);

//            rows = new List<ContentCell>();
//            rows.Add(new ContentCell("", 0, 5, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), false, false, true));
//            rows.Add(new ContentCell("", 1, 5, true, false, ParagraphAlignment.Center, VerticalAlignment.Bottom, 0, 0, false, new Color(0, 0, 0, 0), false, false, false));
//            rows.Add(new ContentCell("", 2, 5, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), true, false, false));
//            reports.AddRowContent(rows, 0.00);


//            columns = new List<ContentColumn>();
//            columns.Add(new ContentColumn(0.40));
//            columns.Add(new ContentColumn(5.90));
//            columns.Add(new ContentColumn(0.20));
//            reports.AddTable(columns, false);

//            rows = new List<ContentCell>();
//            rows.Add(new ContentCell("1.", 0, 10, false, false, ParagraphAlignment.Right, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, false, true));
//            rows.Add(new ContentCell("Suppliers shall provide correct and accurate information in this form.", 1, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, false, false));
//            rows.Add(new ContentCell("", 2, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), true, false, false));
//            reports.AddRowContent(rows, 0.30);

//            rows = new List<ContentCell>();
//            rows.Add(new ContentCell("2.", 0, 10, false, false, ParagraphAlignment.Right, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, false, true));
//            rows.Add(new ContentCell("Price quotation/s must be valid for a period of thirty (30) calendar days from the date of submission.", 1, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, false, false));
//            rows.Add(new ContentCell("", 2, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), true, false, false));
//            reports.AddRowContent(rows, 0.40);

//            rows = new List<ContentCell>();
//            rows.Add(new ContentCell("3.", 0, 10, false, false, ParagraphAlignment.Right, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, false, true));
//            rows.Add(new ContentCell("Price quotation/s, to be denominated in Philippine peso shall include all taxes, duties and/or levies payable.", 1, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, false, false));
//            rows.Add(new ContentCell("", 2, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), true, false, false));
//            reports.AddRowContent(rows, 0.40);

//            rows = new List<ContentCell>();
//            rows.Add(new ContentCell("4.", 0, 10, false, false, ParagraphAlignment.Right, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, false, true));
//            rows.Add(new ContentCell("Quoations exceeding the Approved Budget for the Contract shall be rejected outright.", 1, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, false, false));
//            rows.Add(new ContentCell("", 2, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), true, false, false));
//            reports.AddRowContent(rows, 0.30);

//            rows = new List<ContentCell>();
//            rows.Add(new ContentCell("5.", 0, 10, false, false, ParagraphAlignment.Right, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, false, true));
//            rows.Add(new ContentCell("Award of contract shall be made to the lowest calculated and responsive quotation (for goods and infrastructure) or, the highest rated offer (for consulting services) which complies with the minimum technical specifications and other terms and conditions stated then.", 1, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, false, false));
//            rows.Add(new ContentCell("", 2, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), true, false, false));
//            reports.AddRowContent(rows, 0.60);

//            rows = new List<ContentCell>();
//            rows.Add(new ContentCell("6.", 0, 10, false, false, ParagraphAlignment.Right, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, false, true));
//            rows.Add(new ContentCell("Contracts, in the form of a purchase order, agreement, or any memorandum, shall be duly signed or received within five (5) days from the receipt of the Notice of Award by the supplier, through its authorized representative/s. Otherwise, failure to receive the contract shall be a ground for the cancellation of the award.", 1, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, false, false));
//            rows.Add(new ContentCell("", 2, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), true, false, false));
//            reports.AddRowContent(rows, 0.80);

//            rows = new List<ContentCell>();
//            rows.Add(new ContentCell("7.", 0, 10, false, false, ParagraphAlignment.Right, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, false, true));
//            rows.Add(new ContentCell("The Notice to Proceed shall, likewise, be duly signed and received within (5) days from the receipt of the contract, by the supplier, through its authorized representative/s. Otherwise, failure to receive the Notice to Proceed shall be a ground for the cancellation of the contract. However, in cases where the contract specifies a condition for its implementation, the Notice to Proceed must be duly signed and received within five (5) days from the satisfaction of the condition and the issuance of Notice to Proceed.", 1, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, false, false));
//            rows.Add(new ContentCell("", 2, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), true, false, false));
//            reports.AddRowContent(rows, 1.10);

//            rows = new List<ContentCell>();
//            rows.Add(new ContentCell("8.", 0, 10, false, false, ParagraphAlignment.Right, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, false, true));
//            rows.Add(new ContentCell("Any interlineations, erasures, or overwriting shall be valid only if they are signed or initialed by you or any or your duly authorized representative/s.", 1, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, false, false));
//            rows.Add(new ContentCell("", 2, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), true, false, false));
//            reports.AddRowContent(rows, 0.50);

//            rows = new List<ContentCell>();
//            rows.Add(new ContentCell("9.", 0, 10, false, false, ParagraphAlignment.Right, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, false, true));
//            rows.Add(new ContentCell("The item/s shall be delivered according to the requirements specified in the Technical Specifications.", 1, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, false, false));
//            rows.Add(new ContentCell("", 2, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), true, false, false));
//            reports.AddRowContent(rows, 0.50);

//            rows = new List<ContentCell>();
//            rows.Add(new ContentCell("10.", 0, 10, false, false, ParagraphAlignment.Right, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, false, true));
//            rows.Add(new ContentCell("The UNIVERSITY, shall have the right to inspect and/or test the goods to confirm their conformity to the technical specifications.", 1, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, false, false));
//            rows.Add(new ContentCell("", 2, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), true, false, false));
//            reports.AddRowContent(rows, 0.50);

//            rows = new List<ContentCell>();
//            rows.Add(new ContentCell(" 11.", 0, 10, false, false, ParagraphAlignment.Right, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, false, true));
//            rows.Add(new ContentCell("In case two or more suppliers are determined to have submitted the Lowest Calculated Quotation/Lowest Calculated and Responsive Quotation, the UNIVERSITY shall adopt and employ \"draw lots\" as the tie-breaking method to finally determine the winning provider in accordance with GPPB Circular 06-2005.", 1, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, false, false));
//            rows.Add(new ContentCell("", 2, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), true, false, false));
//            reports.AddRowContent(rows, 0.80);

//            rows = new List<ContentCell>();
//            rows.Add(new ContentCell(" 12.", 0, 10, false, false, ParagraphAlignment.Right, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, false, true));
//            rows.Add(new ContentCell("Supplier shall guarantee his deliveries to be free from defects. Any defective item(s)/products(s), therefore that may be discovered by the UNIVERSITY within three months after acceptance of the same, shall be replaced by the supplier within three (3) calendar days upon receipt of a written notice to that effect. " +
//                                      "Warranty period shall be three (3) months, in case of Expendable Supplies, or one (1) year, in case of Non-expendable Supplies. Warranty shall be covered by, at the Supplier's option, either retention money in the amount equivalent to at least five percent (5%) of every progress payment, or a special bank guarantee equivalent to at least five percent (5%) of the Contract Price." + 
//                                      "The said amounts shall only be released after the lapse of the warranty period; provided however, that the supplies delivered are free from patent and latent defects and all the conditions imposed under this contract have been fully met.", 1, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, false, false));
//            rows.Add(new ContentCell("", 2, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), true, false, false));
//            reports.AddRowContent(rows, 1.90);

//            rows = new List<ContentCell>();
//            rows.Add(new ContentCell(" 13.", 0, 10, false, false, ParagraphAlignment.Right, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, false, true));
//            rows.Add(new ContentCell("Payment shall be processed after delivery and upon the submission of the required supporting documents, in accordance with existing accounting rules and regulations. Please note that the corresponding bank transfer fee, if any, shall be chargable to the contractor's account.", 1, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, false, false));
//            rows.Add(new ContentCell("", 2, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), true, false, false));
//            reports.AddRowContent(rows, 0.60);

//            rows = new List<ContentCell>();
//            rows.Add(new ContentCell(" 14.", 0, 10, false, false, ParagraphAlignment.Right, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, false, true));
//            rows.Add(new ContentCell("The performance security may be released by the UNIVERSITY after the issuance of the Certificate of Final Acceptance, subject to the following conditions:", 1, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, false, false));
//            rows.Add(new ContentCell("", 2, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), true, false, false));
//            reports.AddRowContent(rows, 0.40);

//            columns = new List<ContentColumn>();
//            columns.Add(new ContentColumn(0.75));
//            columns.Add(new ContentColumn(0.25));
//            columns.Add(new ContentColumn(5.30));
//            columns.Add(new ContentColumn(0.20));
//            reports.AddTable(columns, false);

//            rows = new List<ContentCell>();
//            rows.Add(new ContentCell("", 0, 10, false, false, ParagraphAlignment.Right, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, false, true));
//            rows.Add(new ContentCell("a.", 1, 10, false, false, ParagraphAlignment.Right, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, false, true));
//            rows.Add(new ContentCell("UNIVERSITY has no claims filed against the supplier or the surety company;", 2, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, false, false));
//            rows.Add(new ContentCell("", 3, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), true, false, false));
//            reports.AddRowContent(rows, 0.20);

//            rows = new List<ContentCell>();
//            rows.Add(new ContentCell("", 0, 10, false, false, ParagraphAlignment.Right, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, false, true));
//            rows.Add(new ContentCell("b.", 1, 10, false, false, ParagraphAlignment.Right, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, false, true));
//            rows.Add(new ContentCell("It has no claims for labor and materials filed against the CONTRACTOR; and", 2, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, false, false));
//            rows.Add(new ContentCell("", 3, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), true, false, false));
//            reports.AddRowContent(rows, 0.20);

//            rows = new List<ContentCell>();
//            rows.Add(new ContentCell("", 0, 10, false, false, ParagraphAlignment.Right, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, false, true));
//            rows.Add(new ContentCell("c.", 1, 10, false, false, ParagraphAlignment.Right, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, false, true));
//            rows.Add(new ContentCell("Other terms of the contract. [Section 39.5 of the 2016 RIRR of Republic Act No. 9184 (R.A. No. 9184) otherwise known as the \"Government Procurement Reform Act\"]", 2, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, false, false));
//            rows.Add(new ContentCell("", 3, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), true, false, false));
//            reports.AddRowContent(rows, 0.30);

//            rows = new List<ContentCell>();
//            rows.Add(new ContentCell("\n", 0, 10, false, false, ParagraphAlignment.Right, VerticalAlignment.Top, 0, 0, true, new Color(0, 0, 0, 0), false, false, true));
//            rows.Add(new ContentCell("\n", 1, 10, false, false, ParagraphAlignment.Right, VerticalAlignment.Top, 0, 0, true, new Color(0, 0, 0, 0), false, false, true));
//            rows.Add(new ContentCell("\n", 2, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, true, new Color(0, 0, 0, 0), false, false, false));
//            rows.Add(new ContentCell("\n", 3, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, true, new Color(0, 0, 0, 0), true, false, false));
//            reports.AddRowContent(rows, 0.00);

//            reports.InsertPageBreak();

//            columns = new List<ContentColumn>();
//            columns.Add(new ContentColumn(0.40));
//            columns.Add(new ContentColumn(5.90));
//            columns.Add(new ContentColumn(0.20));
//            reports.AddTable(columns, false);

//            rows = new List<ContentCell>();
//            rows.Add(new ContentCell("\n", 0, 10, false, false, ParagraphAlignment.Right, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, true, true));
//            rows.Add(new ContentCell("\n", 1, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, true, false));
//            rows.Add(new ContentCell("\n", 2, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), true, true, false));
//            reports.AddRowContent(rows, 0.00);

//            rows = new List<ContentCell>();
//            rows.Add(new ContentCell("15.", 0, 10, false, false, ParagraphAlignment.Right, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, true, true));
//            rows.Add(new ContentCell("If delaye are likely to be incured, the supplier must notify the UNIVERSITY in writing. It must stated therein the cause/s and duration of expected delay. The UNIVERSITY may grant time extensions, at its discretion, if based on meritorious grounds, with or without liquidated damages. " + 
//                "In all cases, the request for extension should be submitted before the lapse of the original delivery date. The maximum allowable extension shall not be longer than the initial delivery period as stated in the original contract. (Manual of Procedures for the Procurement of Goods and Services)", 1, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, true, false));
//            rows.Add(new ContentCell("", 2, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), true, true, false));
//            reports.AddRowContent(rows, 1.20);

//            rows = new List<ContentCell>();
//            rows.Add(new ContentCell("16.", 0, 10, false, false, ParagraphAlignment.Right, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, false, true));
//            rows.Add(new ContentCell("All deliveries by the supplier shall be subject to inspection and acceptance by the UNIVERSITY. The UNIVERSITY reserves the right to reduce the price in case of non-conformity of the product(s)/items(s) with the technical specifications reflected in the purchase order.", 1, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, false, false));
//            rows.Add(new ContentCell("", 2, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), true, false, false));
//            reports.AddRowContent(rows, 0.80);

//            rows = new List<ContentCell>();
//            rows.Add(new ContentCell("17.", 0, 10, false, false, ParagraphAlignment.Right, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, false, true));
//            rows.Add(new ContentCell("If the supplier fails to deliver any or all the items within the period(s) specified in the purchase order, the UNIVERSITY shall, without prejudice to its other remedies under this agreement and other applicable laws, be entitled to liquidated damages, a sum equivalent to one-tenth of one percent " + 
//                "of the total amount of the undelivered items for each day of delay. Once the cumulative amount of the liquidated damages reaches ten percent (10%), the UNIVERSITY may rescind or terminate the agreement, without prejudice to other courses of action and remedies available under the circumstances. (IRR-A Section 68, Annex \"D\" of R.A. No. 9184)", 1, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, false, false));
//            rows.Add(new ContentCell("", 2, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), true, false, false));
//            reports.AddRowContent(rows, 1.20);

//            rows = new List<ContentCell>();
//            rows.Add(new ContentCell("18.", 0, 10, false, false, ParagraphAlignment.Right, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, false, true));
//            rows.Add(new ContentCell("The UNIVERSITY shall terminate the contract for default when any of the following conditions attend its implementation:", 1, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, false, false));
//            rows.Add(new ContentCell("", 2, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), true, false, false));
//            reports.AddRowContent(rows, 0.40);

//            columns = new List<ContentColumn>();
//            columns.Add(new ContentColumn(0.75));
//            columns.Add(new ContentColumn(0.25));
//            columns.Add(new ContentColumn(5.30));
//            columns.Add(new ContentColumn(0.20));
//            reports.AddTable(columns, false);

//            rows = new List<ContentCell>();
//            rows.Add(new ContentCell("", 0, 10, false, false, ParagraphAlignment.Right, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, false, true));
//            rows.Add(new ContentCell("a.", 1, 10, false, false, ParagraphAlignment.Right, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, false, true));
//            rows.Add(new ContentCell("There being no force majeure, the supplier fails to deliver any or all of the goods within the period(s) specified in the contract, or within any extension thereof granted by the UNIVERSITY pursuant to a request made by the supplier prior to the delay, and such failure amounts to at least ten percent (10%) of the contract price;", 2, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, false, false));
//            rows.Add(new ContentCell("", 3, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), true, false, false));
//            reports.AddRowContent(rows, 0.80);

//            rows = new List<ContentCell>();
//            rows.Add(new ContentCell("", 0, 10, false, false, ParagraphAlignment.Right, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, false, true));
//            rows.Add(new ContentCell("b.", 1, 10, false, false, ParagraphAlignment.Right, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, false, true));
//            rows.Add(new ContentCell("As a result of force majeure, the supplier is unable to deliver or perform any or all of the goods or services, amounting to at least ten percent (10%) of the contract price, for a period of not less than sixty (60) calendar days after the receipt of the notice from the UNIVERSITY stating that the circumstance of force majeure is deemed to have ceased;", 2, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, false, false));
//            rows.Add(new ContentCell("", 3, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), true, false, false));
//            reports.AddRowContent(rows, 1.00);

//            rows = new List<ContentCell>();
//            rows.Add(new ContentCell("", 0, 10, false, false, ParagraphAlignment.Right, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, false, true));
//            rows.Add(new ContentCell("c.", 1, 10, false, false, ParagraphAlignment.Right, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, false, true));
//            rows.Add(new ContentCell("The supplier fails to perform any other obligation(s) under the contract; or", 2, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, false, false));
//            rows.Add(new ContentCell("", 3, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), true, false, false));
//            reports.AddRowContent(rows, 0.30);

//            rows = new List<ContentCell>();
//            rows.Add(new ContentCell("", 0, 10, false, false, ParagraphAlignment.Right, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, false, true));
//            rows.Add(new ContentCell("d.", 1, 10, false, false, ParagraphAlignment.Right, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, false, true));
//            rows.Add(new ContentCell("The supplier, in the judgement of the UNIVERSITY, has engaged in corrupt, fraudulent, collusive or coercive practices in competing for or in executing the contract. (Paragraph III, Annex \"I\" of the 2016 IRR of R.A. No. 9184)", 2, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, false, false));
//            rows.Add(new ContentCell("", 3, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), true, false, false));
//            reports.AddRowContent(rows, 0.60);

//            columns = new List<ContentColumn>();
//            columns.Add(new ContentColumn(0.40));
//            columns.Add(new ContentColumn(5.90));
//            columns.Add(new ContentColumn(0.20));
//            reports.AddTable(columns, false);

//            rows = new List<ContentCell>();
//            rows.Add(new ContentCell("19.", 0, 10, false, false, ParagraphAlignment.Right, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, false, true));
//            rows.Add(new ContentCell("The UNIVERSITY reserves the right to impose administrative penalties upon the supplier when he commits any of the acts which constitute as an offense or violation under Section 69, Article XXIII of the Republic Act No. 9184. Likewise, in addition to the foregoing penalties, the UNIVERSITY reserves its right to blacklist the supplier for offenses or violations committed during competitive bidding and contract implementation, pursuant to the Uniform Guidelines for Blacklisting of Manufacturers, Suppliers, Distributors, and Consultants. (Appendix 17 of R.A. No. 9184)", 1, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, false, false));
//            rows.Add(new ContentCell("", 2, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), true, false, false));
//            reports.AddRowContent(rows, 1.20);

//            rows = new List<ContentCell>();
//            rows.Add(new ContentCell("20.", 0, 10, false, false, ParagraphAlignment.Right, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, false, true));
//            rows.Add(new ContentCell("Any and all disputes arising from this Contract shall be amicably settled by the Parties. For unsettled disputes, the same shall be submitted to arbitration in accordance with the provisions of Republic Act No. 876, otherwise known as the \"Arbitration Law\" and the provisions of Republic Act No. 9285 otherwise known as the \"Alternative Dispute Resolution of 2004\". (Section 59 of the 2016 RIRR of R.A. No. 9184). The arbitral award shall be final and binding upon the parties and may be enforced before a court of competent jurisdiction.", 1, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, false, false));
//            rows.Add(new ContentCell("", 2, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), true, false, false));
//            reports.AddRowContent(rows, 1.00);

//            rows = new List<ContentCell>();
//            rows.Add(new ContentCell("\n", 0, 10, false, false, ParagraphAlignment.Right, VerticalAlignment.Top, 0, 0, true, new Color(0, 0, 0, 0), false, false, true));
//            rows.Add(new ContentCell("\n", 1, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, true, new Color(0, 0, 0, 0), false, false, false));
//            rows.Add(new ContentCell("\n", 2, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, true, new Color(0, 0, 0, 0), true, false, false));
//            reports.AddRowContent(rows, 0.00);

//            reports.InsertPageBreak();

//            columns = new List<ContentColumn>();
//            columns.Add(new ContentColumn(2.00));
//            columns.Add(new ContentColumn(0.20));
//            columns.Add(new ContentColumn(4.20));
//            reports.AddTable(columns, false);

//            rows = new List<ContentCell>();
//            rows.Add(new ContentCell("Solicitation No.", 0, 10, true, false, ParagraphAlignment.Left, VerticalAlignment.Bottom));
//            rows.Add(new ContentCell(" : ", 1, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Bottom));
//            rows.Add(new ContentCell(rfq.SolicitationNo, 2, 10, false, true, ParagraphAlignment.Left, VerticalAlignment.Bottom, 0, 0, true));
//            reports.AddRowContent(rows, 0.25);

//            rows = new List<ContentCell>();
//            rows.Add(new ContentCell("Project Identification No. ", 0, 10, true, false, ParagraphAlignment.Left, VerticalAlignment.Bottom));
//            rows.Add(new ContentCell(" : ", 1, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Bottom));
//            rows.Add(new ContentCell(rfq.ContractCode, 2, 10, false, true, ParagraphAlignment.Left, VerticalAlignment.Bottom, 0, 0, true));
//            reports.AddRowContent(rows, 0.25);

//            rows = new List<ContentCell>();
//            rows.Add(new ContentCell("Name of Project", 0, 10, true, false, ParagraphAlignment.Left, VerticalAlignment.Bottom));
//            rows.Add(new ContentCell(" : ", 1, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Bottom));
//            rows.Add(new ContentCell(rfq.ContractName, 2, 8, false, true, ParagraphAlignment.Left, VerticalAlignment.Bottom, 0, 0, true));
//            reports.AddRowContent(rows, 0.25);

//            rows = new List<ContentCell>();
//            rows.Add(new ContentCell("Location of the Project", 0, 10, true, false, ParagraphAlignment.Left, VerticalAlignment.Bottom));
//            rows.Add(new ContentCell(" : ", 1, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Bottom));
//            rows.Add(new ContentCell(agencyDetails.Address, 2, 10, false, true, ParagraphAlignment.Left, VerticalAlignment.Bottom, 0, 0, true));
//            reports.AddRowContent(rows, 0.25);

//            rows = new List<ContentCell>();
//            rows.Add(new ContentCell("Date", 0, 10, true, false, ParagraphAlignment.Left, VerticalAlignment.Bottom));
//            rows.Add(new ContentCell(" : ", 1, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Bottom));
//            rows.Add(new ContentCell(rfq.Date.ToString("dd MMMM yyyy"), 2, 10, false, true, ParagraphAlignment.Left, VerticalAlignment.Bottom, 0, 0, true));
//            reports.AddRowContent(rows, 0.25);

//            rows = new List<ContentCell>();
//            rows.Add(new ContentCell("Quotation No.", 0, 10, true, false, ParagraphAlignment.Left, VerticalAlignment.Bottom));
//            rows.Add(new ContentCell(" : ", 1, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Bottom));
//            rows.Add(new ContentCell(rfq.QuotationNo, 2, 10, false, true, ParagraphAlignment.Left, VerticalAlignment.Bottom, 0, 0, true));
//            reports.AddRowContent(rows, 0.25);

//            reports.AddNewLine();
//            reports.AddNewLine();

//            columns = new List<ContentColumn>();
//            columns.Add(new ContentColumn(2.50));
//            columns.Add(new ContentColumn(4.00));
//            reports.AddTable(columns, true);

//            rows = new List<ContentCell>();
//            rows.Add(new ContentCell("Name of Company", 0, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0));
//            rows.Add(new ContentCell("", 1, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0));
//            reports.AddRowContent(rows, 0.30);

//            rows = new List<ContentCell>();
//            rows.Add(new ContentCell("Address", 0, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0));
//            rows.Add(new ContentCell("", 1, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0));
//            reports.AddRowContent(rows, 0.30);

//            rows = new List<ContentCell>();
//            rows.Add(new ContentCell("TIN", 0, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0));
//            rows.Add(new ContentCell("", 1, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0));
//            reports.AddRowContent(rows, 0.30);

//            rows = new List<ContentCell>();
//            rows.Add(new ContentCell("PhilGEPS Registration Number", 0, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0));
//            rows.Add(new ContentCell("", 1, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0));
//            reports.AddRowContent(rows, 0.30);

//            reports.AddNewLine();

//            columns = new List<ContentColumn>();
//            columns.Add(new ContentColumn(6.50));
//            reports.AddTable(columns, false);

//            rows = new List<ContentCell>();
//            rows.Add(new ContentCell("\n", 0, 8, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), true, true, true));
//            reports.AddRowContent(rows, 0.00);

//            columns = new List<ContentColumn>();
//            columns.Add(new ContentColumn(2.25));
//            columns.Add(new ContentColumn(2.00));
//            columns.Add(new ContentColumn(2.25));
//            reports.AddTable(columns, false);

//            rows = new List<ContentCell>();
//            rows.Add(new ContentCell("", 0, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), false, false, true));
//            rows.Add(new ContentCell("INSTRUCTIONS", 1, 10, true, false, ParagraphAlignment.Center, VerticalAlignment.Bottom, 0, 0, false, new Color(0, 0, 0, 0), false, false, false));
//            rows.Add(new ContentCell("", 2, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), true, false, false));
//            reports.AddRowContent(rows, 0.00);

//            columns = new List<ContentColumn>();
//            columns.Add(new ContentColumn(0.40));
//            columns.Add(new ContentColumn(5.90));
//            columns.Add(new ContentColumn(0.20));
//            reports.AddTable(columns, false);

//            rows = new List<ContentCell>();
//            rows.Add(new ContentCell("\n", 0, 10, false, false, ParagraphAlignment.Right, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, false, true));
//            rows.Add(new ContentCell("\n", 1, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, false, false));
//            rows.Add(new ContentCell("\n", 2, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), true, false, false));
//            reports.AddRowContent(rows, 0.00);

//            rows = new List<ContentCell>();
//            rows.Add(new ContentCell("1.", 0, 10, false, false, ParagraphAlignment.Right, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, true, true));
//            rows.Add(new ContentCell("Accoplish this RFQ correctly, accurately and completely.", 1, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, true, false));
//            rows.Add(new ContentCell("", 2, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), true, true, false));
//            reports.AddRowContent(rows, 0.00);

//            rows = new List<ContentCell>();
//            rows.Add(new ContentCell("2.", 0, 10, false, false, ParagraphAlignment.Right, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, true, true));
//            rows.Add(new ContentCell("Do not alter the contents of this form in any way.", 1, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, true, false));
//            rows.Add(new ContentCell("", 2, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), true, true, false));
//            reports.AddRowContent(rows, 0.00);

//            rows = new List<ContentCell>();
//            rows.Add(new ContentCell("3.", 0, 10, false, false, ParagraphAlignment.Right, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, true, true));
//            rows.Add(new ContentCell("All technical specifications are mandatory. Failure to comply with any of the mandatory requirements will disqualify your quotation.", 1, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, true, false));
//            rows.Add(new ContentCell("", 2, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), true, true, false));
//            reports.AddRowContent(rows, 0.00);

//            rows = new List<ContentCell>();
//            rows.Add(new ContentCell("4.", 0, 10, false, false, ParagraphAlignment.Right, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, true, true));
//            rows.Add(new ContentCell("Failure to follow these instructions will disqualify your entire quotation.", 1, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, true, false));
//            rows.Add(new ContentCell("", 2, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), true, true, false));
//            reports.AddRowContent(rows, 0.00);

//            rows = new List<ContentCell>();
//            rows.Add(new ContentCell("5.", 0, 10, false, false, ParagraphAlignment.Right, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, true, true));
//            rows.Add(new ContentCell("Suppliers shall submit the original brochures showing certifications of the products being offered", 1, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, true, false));
//            rows.Add(new ContentCell("", 2, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), true, true, false));
//            reports.AddRowContent(rows, 0.00);

//            rows = new List<ContentCell>();
//            rows.Add(new ContentCell("\n", 0, 10, false, false, ParagraphAlignment.Right, VerticalAlignment.Top, 0, 0, true, new Color(0, 0, 0, 0), false, false, true));
//            rows.Add(new ContentCell("\n", 1, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, true, new Color(0, 0, 0, 0), false, false, false));
//            rows.Add(new ContentCell("\n", 2, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, true, new Color(0, 0, 0, 0), true, false, false));
//            reports.AddRowContent(rows, 0.00);

//            reports.AddNewLine();

//            columns = new List<ContentColumn>();
//            columns.Add(new ContentColumn(6.50));
//            reports.AddTable(columns, false);

//            reports.AddFormattedRowContent(new ContentCell(new TextWithFormat[]
//            {
//               new TextWithFormat("Sir/Madam:\n\n", true, false, 10),
//            }, MigraDoc.DocumentObjectModel.ParagraphAlignment.Justify), 0.00);

//            reports.AddFormattedRowContent(new ContentCell(new TextWithFormat[]
//            {
//               new TextWithFormat("After having carefully reading and accepting the Terms and Conditions in this Request for Quotation, I/We hereby provide my/our quotation for the item/s as follows:", false, false, 10),
//            }, new Unit(0.50, UnitType.Inch), MigraDoc.DocumentObjectModel.ParagraphAlignment.Justify), 0.00);

//            reports.AddNewLine();

//            columns = new List<ContentColumn>();
//            columns.Add(new ContentColumn(2.00));
//            columns.Add(new ContentColumn(2.50));
//            columns.Add(new ContentColumn(2.00));
//            reports.AddTable(columns, false);

//            rows = new List<ContentCell>();
//            rows.Add(new ContentCell("", 0, 5, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), false, true, true));
//            rows.Add(new ContentCell("", 1, 5, true, false, ParagraphAlignment.Center, VerticalAlignment.Bottom, 0, 0, false, new Color(0, 0, 0, 0), false, true, false));
//            rows.Add(new ContentCell("", 2, 5, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), true, true, false));
//            reports.AddRowContent(rows, 0.00);

//            rows = new List<ContentCell>();
//            rows.Add(new ContentCell("", 0, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), false, false, true));
//            rows.Add(new ContentCell(new TextWithFormat[]
//            {
//                new TextWithFormat("TECHNICAL SPECIFICATION", true, false, 10, Underline.Single),
//            }, 1, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, false, false));
//            rows.Add(new ContentCell("", 2, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), true, false, false));
//            reports.AddRowContent(rows, 0.00);

//            columns = new List<ContentColumn>();
//            columns.Add(new ContentColumn(0.40));
//            columns.Add(new ContentColumn(5.90));
//            columns.Add(new ContentColumn(0.20));
//            reports.AddTable(columns, false);

//            rows = new List<ContentCell>();
//            rows.Add(new ContentCell("\n", 0, 10, false, false, ParagraphAlignment.Right, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, false, true));
//            rows.Add(new ContentCell("\n", 1, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, false, false));
//            rows.Add(new ContentCell("\n", 2, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), true, false, false));
//            reports.AddRowContent(rows, 0.00);

//            rows = new List<ContentCell>();
//            rows.Add(new ContentCell("1.", 0, 10, false, false, ParagraphAlignment.Right, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, false, true));
//            rows.Add(new ContentCell(new TextWithFormat[]
//            {
//               new TextWithFormat("Please quote your ", false, false, 10),
//               new TextWithFormat("best offer", true, false, 10, Underline.Single),
//               new TextWithFormat(" the item/s below. Please do not leave any blank items. Indicate ", false, false, 10),
//               new TextWithFormat("\"0\"", true, false, 10),
//               new TextWithFormat(" if the item/s is/are being offered for free.", false, false, 10),
//            }, 1, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, false, false));
//            rows.Add(new ContentCell("", 2, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), true, false, false));
//            reports.AddRowContent(rows, 0.25);

//            rows = new List<ContentCell>();
//            rows.Add(new ContentCell("2.", 0, 10, false, false, ParagraphAlignment.Right, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, true, true));
//            rows.Add(new ContentCell("The quote prices should be inclusive of all costs and applicable taxes.", 1, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, true, false));
//            rows.Add(new ContentCell("", 2, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), true, true, false));
//            reports.AddRowContent(rows, 0.25);

//            rows = new List<ContentCell>();
//            rows.Add(new ContentCell("3.", 0, 10, false, false, ParagraphAlignment.Right, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, true, true));
//            rows.Add(new ContentCell("If the space below is not sufficient, you may attach a separate sheet and index it as \"Annex A\". It shall be submitted aling with this document.", 1, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, true, false));
//            rows.Add(new ContentCell("", 2, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), true, true, false));
//            reports.AddRowContent(rows, 0.25);

//            rows = new List<ContentCell>();
//            rows.Add(new ContentCell("\n", 0, 10, false, false, ParagraphAlignment.Right, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, true, true));
//            rows.Add(new ContentCell("\n", 1, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, true, false));
//            rows.Add(new ContentCell("\n", 2, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), true, true, false));
//            reports.AddRowContent(rows, 0.25);

//            columns = new List<ContentColumn>();
//            columns.Add(new ContentColumn(0.75));
//            columns.Add(new ContentColumn(0.75));
//            columns.Add(new ContentColumn(3.00));
//            columns.Add(new ContentColumn(0.75));
//            columns.Add(new ContentColumn(1.25));
//            reports.AddTable(columns, true);

//            rows = new List<ContentCell>();
//            rows.Add(new ContentCell("Item No.", 0, 10, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
//            rows.Add(new ContentCell("Unit", 1, 10, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
//            rows.Add(new ContentCell("Description", 2, 10, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
//            rows.Add(new ContentCell("Quantity", 3, 10, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
//            rows.Add(new ContentCell("Unit Price", 4, 10, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
//            reports.AddRowContent(rows, 0.30);

//            rows = new List<ContentCell>();
//            rows.Add(new ContentCell(rfq.ContractName + " (" + rfq.ContractCode + ")", 0, 10, true, true, ParagraphAlignment.Center, VerticalAlignment.Center, 4));
//            reports.AddRowContent(rows, 0.50);

//            foreach (var item in rfq.Details)
//            {
//                rows = new List<ContentCell>();
//                rows.Add(new ContentCell("", 0, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Top));
//                rows.Add(new ContentCell(item.UnitOfMeasure, 1, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Top));
//                rows.Add(new ContentCell(new TextWithFormat[]
//                {
//                   new TextWithFormat(item.ItemFullName.ToUpper() + "\n\n", true, false, 10, Underline.Single),
//                   new TextWithFormat(item.ItemSpecifications, false, true, 7),
//                }, 2, ParagraphAlignment.Left, VerticalAlignment.Top));
//                rows.Add(new ContentCell(item.Quantity.ToString(), 3, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Top));
//                rows.Add(new ContentCell(new TextWithFormat[]
//                {
//                   new TextWithFormat("_______", false, false, 11),
//                   new TextWithFormat(" / " + item.UnitOfMeasure, false, true, 8),
//                }, 4, ParagraphAlignment.Center, VerticalAlignment.Top));
//                reports.AddRowContent(rows, 0.30);
//            }

//            rows = new List<ContentCell>();
//            rows.Add(new ContentCell("***** NOTHING FOLLOWS ******", 0, 10, true, true, ParagraphAlignment.Center, VerticalAlignment.Center, 4));
//            reports.AddRowContent(rows, 0.30);

//            rows = new List<ContentCell>();
//            rows.Add(new ContentCell("TOTAL COST", 0, 10, true, false, ParagraphAlignment.Right, VerticalAlignment.Center, 2));
//            rows.Add(new ContentCell("", 3, 10, true, true, ParagraphAlignment.Center, VerticalAlignment.Center, 1));
//            reports.AddRowContent(rows, 0.30);

//            reports.InsertPageBreak();

//            columns = new List<ContentColumn>();
//            columns.Add(new ContentColumn(2.00));
//            columns.Add(new ContentColumn(2.50));
//            columns.Add(new ContentColumn(2.00));
//            reports.AddTable(columns, false);

//            rows = new List<ContentCell>();
//            rows.Add(new ContentCell("", 0, 5, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), false, true, true));
//            rows.Add(new ContentCell("", 1, 5, true, false, ParagraphAlignment.Center, VerticalAlignment.Bottom, 0, 0, false, new Color(0, 0, 0, 0), false, true, false));
//            rows.Add(new ContentCell("", 2, 5, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), true, true, false));
//            reports.AddRowContent(rows, 0.00);

//            rows = new List<ContentCell>();
//            rows.Add(new ContentCell("", 0, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), false, false, true));
//            rows.Add(new ContentCell(new TextWithFormat[]
//            {
//                new TextWithFormat("SCHEDULE OF REQUIREMENTS", true, false, 10, Underline.Single),
//            }, 1, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, false, false));
//            rows.Add(new ContentCell("", 2, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), true, false, false));
//            reports.AddRowContent(rows, 0.00);

//            rows = new List<ContentCell>();
//            rows.Add(new ContentCell("", 0, 5, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), false, false, true));
//            rows.Add(new ContentCell("", 1, 5, true, false, ParagraphAlignment.Center, VerticalAlignment.Bottom, 0, 0, false, new Color(0, 0, 0, 0), false, false, false));
//            rows.Add(new ContentCell("", 2, 5, false, false, ParagraphAlignment.Left, VerticalAlignment.Center, 0, 0, false, new Color(0, 0, 0, 0), true, false, false));
//            reports.AddRowContent(rows, 0.00);

//            columns = new List<ContentColumn>();
//            columns.Add(new ContentColumn(0.40));
//            columns.Add(new ContentColumn(5.90));
//            columns.Add(new ContentColumn(0.20));
//            reports.AddTable(columns, false);

//            rows = new List<ContentCell>();
//            rows.Add(new ContentCell("1.", 0, 10, false, false, ParagraphAlignment.Right, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, false, true));
//            rows.Add(new ContentCell(new TextWithFormat[]
//            {
//               new TextWithFormat("The delivery schedule stipulates hereafter the delivery date to the project.", false, false, 10),
//            }, 1, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, false, false));
//            rows.Add(new ContentCell("", 2, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), true, false, false));
//            reports.AddRowContent(rows, 0.25);

//            rows = new List<ContentCell>();
//            rows.Add(new ContentCell("2.", 0, 10, false, false, ParagraphAlignment.Right, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, true, true));
//            rows.Add(new ContentCell("If the space below is not sufficient, you may attach a separate sheet and index it as \"Anex B\". It shall be submitted along with this document.", 1, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, true, false));
//            rows.Add(new ContentCell("", 2, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), true, true, false));
//            reports.AddRowContent(rows, 0.25);

//            rows = new List<ContentCell>();
//            rows.Add(new ContentCell("\n", 0, 10, false, false, ParagraphAlignment.Right, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, true, true));
//            rows.Add(new ContentCell("\n", 1, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), false, true, false));
//            rows.Add(new ContentCell("\n", 2, 10, false, false, ParagraphAlignment.Justify, VerticalAlignment.Top, 0, 0, false, new Color(0, 0, 0, 0), true, true, false));
//            reports.AddRowContent(rows, 0.25);

//            columns = new List<ContentColumn>();
//            columns.Add(new ContentColumn(0.75));
//            columns.Add(new ContentColumn(3.50));
//            columns.Add(new ContentColumn(0.75));
//            columns.Add(new ContentColumn(1.50));
//            reports.AddTable(columns, true);

//            rows = new List<ContentCell>();
//            rows.Add(new ContentCell("Item No.", 0, 10, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
//            rows.Add(new ContentCell("Description", 1, 10, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
//            rows.Add(new ContentCell("Total Quantity", 2, 10, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
//            rows.Add(new ContentCell("Delivery Period", 3, 10, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
//            reports.AddRowContent(rows, 0.30);

//            rows = new List<ContentCell>();
//            rows.Add(new ContentCell("", 0, 10, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
//            rows.Add(new ContentCell("", 1, 10, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
//            rows.Add(new ContentCell("", 2, 10, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
//            rows.Add(new ContentCell("", 3, 10, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
//            reports.AddRowContent(rows, 1.50);

//            reports.AddNewLine();
//            reports.AddNewLine();

//            columns = new List<ContentColumn>();
//            columns.Add(new ContentColumn(3.00));
//            columns.Add(new ContentColumn(3.50));
//            reports.AddTable(columns, true);

//            rows = new List<ContentCell>();
//            rows.Add(new ContentCell(new TextWithFormat[]
//            {
//                new TextWithFormat("FINANCIAL OFFER", true, false, 10, Underline.Single),
//            }, 0, ParagraphAlignment.Center, VerticalAlignment.Center, 1));
//            reports.AddRowContent(rows, 0.50);

//            rows = new List<ContentCell>();
//            rows.Add(new ContentCell("Approved Budget for the Contract (ABC)", 0, 10, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
//            rows.Add(new ContentCell("Total Offered Quotation", 1, 10, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
//            reports.AddRowContent(rows, 0.50);

//            rows = new List<ContentCell>();
//            rows.Add(new ContentCell("", 0, 10, false, false, ParagraphAlignment.Center, VerticalAlignment.Center, 0, 1));
//            rows.Add(new ContentCell("In words:", 1, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Top));
//            reports.AddRowContent(rows, 0.75);

//            rows = new List<ContentCell>();
//            rows.Add(new ContentCell("In Figures:", 1, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Top));
//            reports.AddRowContent(rows, 0.75);

//            return reports.GenerateReport();
//        }
//        public MemoryStream PrintAbstract(string LogoPath, string ReferenceNo)
//        {
//            var rfq = rfqDAL.GetAbstractOfQuotation(ReferenceNo);
//            var agencyDetails = db.AgencyDetails.FirstOrDefault();

//            Reports reports = new Reports();
//            reports.ReportFilename = ReferenceNo;
//            reports.CreateDocument(8.50, 13.00, Orientation.Portrait, 0.50);
//            reports.AddDoubleColumnHeader(LogoPath, false, 1.25);
//            //reports.AddPageNumbersFooter();
//            reports.AddColumnHeader(
//                new HeaderLine { Content = "Republic of the Philippines", Bold = false, Italic = false, FontSize = 8, ParagraphAlignment = ParagraphAlignment.Left },
//                new HeaderLine { Content = "", Bold = false, Italic = false, FontSize = 6, ParagraphAlignment = ParagraphAlignment.Left }
//            );
//            reports.AddColumnHeader(
//                new HeaderLine { Content = "POLYTECHNIC UNIVERSITY OF THE PHILIPPINES", Bold = true, Italic = false, FontSize = 12, ParagraphAlignment = ParagraphAlignment.Left },
//                new HeaderLine { Content = "", Bold = false, Italic = false, FontSize = 7, ParagraphAlignment = ParagraphAlignment.Left }
//            );
//            reports.AddColumnHeader(
//                new HeaderLine { Content = "Office of the Vice President for Administration", Bold = true, Italic = false, FontSize = 8, ParagraphAlignment = ParagraphAlignment.Left },
//                new HeaderLine { Content = "", Bold = false, Italic = false, FontSize = 7, ParagraphAlignment = ParagraphAlignment.Left }
//            );
//            reports.AddColumnHeader(
//                new HeaderLine { Content = "PROCUREMENT MANAGEMENT OFFICE", Bold = true, Italic = false, FontSize = 10, ParagraphAlignment = ParagraphAlignment.Left },
//                new HeaderLine { Content = "", Bold = false, Italic = false, FontSize = 7, ParagraphAlignment = ParagraphAlignment.Left }
//            );
//            reports.AddColumnHeader(
//                new HeaderLine { Content = "A. Mabini Campus, Anonas St., Santa Mesa, Manila\t1016", Bold = false, Italic = false, FontSize = 10, ParagraphAlignment = ParagraphAlignment.Left },
//                new HeaderLine { Content = "", Bold = false, Italic = false, FontSize = 7, ParagraphAlignment = ParagraphAlignment.Left }
//            );

//            var columns = new List<ContentColumn>();
//            columns.Add(new ContentColumn(7.50));
//            reports.AddTable(columns, false);

//            reports.AddFormattedRowContent(new ContentCell(new TextWithFormat[]
//            {
//               new TextWithFormat("ABSTRACT OF QUOTATIONS", true, false, 14, Underline.Single),
//            }, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center), 0.00);

//            reports.AddNewLine();
//            reports.AddNewLine();

//            columns = new List<ContentColumn>();
//            columns.Add(new ContentColumn(2.25));
//            columns.Add(new ContentColumn(0.25));
//            columns.Add(new ContentColumn(5.00));
//            reports.AddTable(columns, false);

//            var rows = new List<ContentCell>();
//            rows.Add(new ContentCell("Contract Name", 0, 9, true, false, ParagraphAlignment.Left, VerticalAlignment.Top));
//            rows.Add(new ContentCell(" : ", 1, 9, false, false, ParagraphAlignment.Left, VerticalAlignment.Top));
//            rows.Add(new ContentCell(rfq.ContractName, 2, 9, false, false, ParagraphAlignment.Left, VerticalAlignment.Top));
//            reports.AddRowContent(rows, 0.20);

//            columns = new List<ContentColumn>();
//            columns.Add(new ContentColumn(2.25));
//            columns.Add(new ContentColumn(0.25));
//            columns.Add(new ContentColumn(2.75));
//            columns.Add(new ContentColumn(0.50));
//            columns.Add(new ContentColumn(0.25));
//            columns.Add(new ContentColumn(1.00));
//            reports.AddTable(columns, false);

//            rows = new List<ContentCell>();
//            rows.Add(new ContentCell("Contract Location", 0, 9, true, false, ParagraphAlignment.Left, VerticalAlignment.Top));
//            rows.Add(new ContentCell(" : ", 1, 9, false, false, ParagraphAlignment.Left, VerticalAlignment.Top));
//            rows.Add(new ContentCell(agencyDetails.Address, 2, 10, false, false, ParagraphAlignment.Left, VerticalAlignment.Top));
//            rows.Add(new ContentCell("Sheet", 3, 9, true, false, ParagraphAlignment.Left, VerticalAlignment.Top));
//            rows.Add(new ContentCell(" : ", 4, 9, false, false, ParagraphAlignment.Left, VerticalAlignment.Top));
//            rows.Add(new ContentCell("", 5, 9, false, false, ParagraphAlignment.Left, VerticalAlignment.Top));
//            reports.AddRowContent(rows, 0.20);

//            rows = new List<ContentCell>();
//            rows.Add(new ContentCell("Approved Budget for the Contract", 0, 9, true, false, ParagraphAlignment.Left, VerticalAlignment.Top));
//            rows.Add(new ContentCell(" : ", 1, 9, false, false, ParagraphAlignment.Left, VerticalAlignment.Top));
//            rows.Add(new ContentCell(rfq.ABC.ToString("C", new System.Globalization.CultureInfo("en-ph")), 2, 9, false, false, ParagraphAlignment.Left, VerticalAlignment.Top));
//            rows.Add(new ContentCell("Date", 3, 9, true, false, ParagraphAlignment.Left, VerticalAlignment.Top));
//            rows.Add(new ContentCell(" : ", 4, 9, false, false, ParagraphAlignment.Left, VerticalAlignment.Top));
//            rows.Add(new ContentCell(rfq.OpenedAt.ToString("dd MMMM yyyy"), 5, 9, false, false, ParagraphAlignment.Left, VerticalAlignment.Top));
//            reports.AddRowContent(rows, 0.20);

//            columns = new List<ContentColumn>();
//            columns.Add(new ContentColumn(2.25));
//            columns.Add(new ContentColumn(0.25));
//            columns.Add(new ContentColumn(5.00));
//            reports.AddTable(columns, false);

//            rows = new List<ContentCell>();
//            rows.Add(new ContentCell("Time and Place of Opening", 0, 9, true, false, ParagraphAlignment.Left, VerticalAlignment.Top));
//            rows.Add(new ContentCell(" : ", 1, 9, false, false, ParagraphAlignment.Left, VerticalAlignment.Top));
//            rows.Add(new ContentCell(rfq.OpenedAt.ToString("hh:mm tt") + ", " + rfq.PlaceOpened, 2, 9, false, false, ParagraphAlignment.Left, VerticalAlignment.Top));
//            reports.AddRowContent(rows, 0.20);

//            reports.AddNewLine();
//            reports.AddNewLine();

//            columns = new List<ContentColumn>();
//            columns.Add(new ContentColumn(2.25));
//            columns.Add(new ContentColumn(1.00));
//            columns.Add(new ContentColumn(1.00));
//            columns.Add(new ContentColumn(1.00));
//            columns.Add(new ContentColumn(2.25));
//            reports.AddTable(columns, true);

//            rows = new List<ContentCell>();
//            rows.Add(new ContentCell("Supplier", 0, 9, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
//            rows.Add(new ContentCell("Total Bid\n(PhP)", 1, 9, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
//            rows.Add(new ContentCell("Variance\n(%)", 2, 9, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
//            rows.Add(new ContentCell("Savings\n(PhP)", 3, 9, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
//            rows.Add(new ContentCell("Remarks", 4, 9, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
//            reports.AddRowContent(rows, 0.40);

//            foreach(var item in rfq.Suppliers)
//            {
//                rows = new List<ContentCell>();
//                rows.Add(new ContentCell(item.SupplierName, 0, 9, true, false, ParagraphAlignment.Left, VerticalAlignment.Top));
//                rows.Add(new ContentCell(item.TotalBid == null ? "No Bid/Incomplete Bid" : item.TotalBid.Value.ToString("C", new System.Globalization.CultureInfo("en-ph")), 1, 9, false, false, item.TotalBid == null ? ParagraphAlignment.Center : ParagraphAlignment.Right, VerticalAlignment.Top));
//                rows.Add(new ContentCell(item.TotalBid == null ? "" : item.Variance.Value.ToString("P", new System.Globalization.CultureInfo("en-ph")), 2, 9, false, false, ParagraphAlignment.Center, VerticalAlignment.Top));
//                rows.Add(new ContentCell(item.TotalBid == null ? "" : item.Savings.Value.ToString("C", new System.Globalization.CultureInfo("en-ph")), 3, 9, false, false, ParagraphAlignment.Right, VerticalAlignment.Top));
//                rows.Add(new ContentCell(new TextWithFormat[]
//                {
//                    new TextWithFormat("Eligibility Requirements: ", true, false, 9),
//                    new TextWithFormat((item.EligibilityRequirements == true ? "PASSED" : "FAILED") + "\n", false, true, 9),
//                    new TextWithFormat("Technical Requirements: ", true, false, 9),
//                    new TextWithFormat((item.TechnicalRequirements == true ? "PASSED" : "FAILED") + "\n", false, true, 9),
//                    new TextWithFormat("Financial Requirements: ", true, false, 9),
//                    new TextWithFormat((item.FinancialRequirements == true ? "PASSED" : "FAILED") + "\n", false, true, 9),
//                    new TextWithFormat("LCRQ: ", true, false, 9),
//                    new TextWithFormat((item.LCRQ == true ? "YES" : "NO") + "\n", false, true, 9),
//                    new TextWithFormat("Recommend to Award Contract: ", true, false, 9),
//                    new TextWithFormat((item.RecommendToAward == true ? "YES" : "NO") + "\n", false, true, 9),
//                    new TextWithFormat(item.Remarks, false, true, 9),
//                }, 4, ParagraphAlignment.Left, VerticalAlignment.Top));
//                reports.AddRowContent(rows, 1.50);
//            }

//            reports.AddNewLine();
//            reports.AddNewLine();

//            columns = new List<ContentColumn>();
//            columns.Add(new ContentColumn(7.50));
//            reports.AddTable(columns, false);

//            reports.AddFormattedRowContent(new ContentCell(new TextWithFormat[]
//            {
//               new TextWithFormat("Prepared by:", false, false, 9),
//            }, MigraDoc.DocumentObjectModel.ParagraphAlignment.Left), 0.00);

//            reports.AddNewLine();
//            reports.AddNewLine();
//            reports.AddNewLine();

//            columns = new List<ContentColumn>();
//            columns.Add(new ContentColumn(2.25));
//            columns.Add(new ContentColumn(0.50));
//            columns.Add(new ContentColumn(2.00));
//            columns.Add(new ContentColumn(0.50));
//            columns.Add(new ContentColumn(2.25));
//            reports.AddTable(columns, false);

//            rows = new List<ContentCell>();
//            rows.Add(new ContentCell("", 0, 9, true, false, ParagraphAlignment.Left, VerticalAlignment.Top, 0, 0, true));
//            rows.Add(new ContentCell("", 1, 9, false, false, ParagraphAlignment.Left, VerticalAlignment.Top));
//            rows.Add(new ContentCell("", 2, 9, false, false, ParagraphAlignment.Left, VerticalAlignment.Top, 0, 0, true));
//            rows.Add(new ContentCell("", 3, 9, true, false, ParagraphAlignment.Left, VerticalAlignment.Top));
//            rows.Add(new ContentCell("", 4, 9, false, false, ParagraphAlignment.Left, VerticalAlignment.Top, 0, 0, true));
//            reports.AddRowContent(rows, 0.40);

//            rows = new List<ContentCell>();
//            rows.Add(new ContentCell("Director, PMO", 0, 9, false, true, ParagraphAlignment.Center, VerticalAlignment.Top));
//            rows.Add(new ContentCell("", 1, 9, false, false, ParagraphAlignment.Left, VerticalAlignment.Top));
//            rows.Add(new ContentCell("Chief, Procurement Planning", 2, 9, false, true, ParagraphAlignment.Center, VerticalAlignment.Top));
//            rows.Add(new ContentCell("", 3, 9, true, false, ParagraphAlignment.Left, VerticalAlignment.Top));
//            rows.Add(new ContentCell("Chief, Contract Management", 4, 9, false, false, ParagraphAlignment.Center, VerticalAlignment.Top));
//            reports.AddRowContent(rows, 0.20);

//            rows = new List<ContentCell>();
//            rows.Add(new ContentCell("", 0, 9, true, false, ParagraphAlignment.Left, VerticalAlignment.Top, 0, 0, true));
//            rows.Add(new ContentCell("", 1, 9, false, false, ParagraphAlignment.Left, VerticalAlignment.Top));
//            rows.Add(new ContentCell("", 2, 9, false, false, ParagraphAlignment.Left, VerticalAlignment.Top));
//            rows.Add(new ContentCell("", 3, 9, true, false, ParagraphAlignment.Left, VerticalAlignment.Top));
//            rows.Add(new ContentCell("", 4, 9, false, false, ParagraphAlignment.Left, VerticalAlignment.Top, 0, 0, true));
//            reports.AddRowContent(rows, 0.40);

//            rows = new List<ContentCell>();
//            rows.Add(new ContentCell("End-User", 0, 9, false, true, ParagraphAlignment.Center, VerticalAlignment.Top));
//            rows.Add(new ContentCell("", 1, 9, false, false, ParagraphAlignment.Left, VerticalAlignment.Top));
//            rows.Add(new ContentCell("", 2, 9, false, true, ParagraphAlignment.Center, VerticalAlignment.Top));
//            rows.Add(new ContentCell("", 3, 9, true, false, ParagraphAlignment.Left, VerticalAlignment.Top));
//            rows.Add(new ContentCell("Staff, PMO", 4, 9, false, false, ParagraphAlignment.Center, VerticalAlignment.Top));
//            reports.AddRowContent(rows, 0.20);

//            return reports.GenerateReport();
//        }
//        protected override void Dispose(bool disposing)
//        {
//            if (disposing)
//            {
//                rfqDAL.Dispose();
//            }
//            base.Dispose(disposing);
//        }
//    }

//    public class RequestForQuotationDAL : Controller
//    {
//        private FMISDbContext db = new FMISDbContext();
//        private HRISDataAccess hris = new HRISDataAccess();
//        private ABISDataAccess abis = new ABISDataAccess();
//        private SystemBDL systemBDL = new SystemBDL();

//        public List<int> GetFiscalYears()
//        {
//            return db.RequestForQuotation.Where(d => d.IsSubmissionOpen == true).Select(d => d.FiscalYear).Distinct().ToList();
//        }
//        public List<int> GetAbstractFiscalYears()
//        {
//            return db.RequestForQuotation.Where(d => d.IsSubmissionOpen == false && d.AbstractCreatedAt == null).Select(d => d.FiscalYear).Distinct().ToList();
//        }
//        public List<int> GetPreparedAbstractFiscalYears()
//        {
//            return db.RequestForQuotation.Where(d => d.IsSubmissionOpen == false && d.AbstractCreatedAt != null).Select(d => d.FiscalYear).Distinct().ToList();
//        }
//        public List<SupplierVM> GetSuppliers(string SolicitationNo)
//        {
//            var supplierWithQuotation = db.RequestForQuotationHeader.Where(d => d.FKRFQReference.SolicitationNo == SolicitationNo).Select(d => d.SupplierReference).ToList();
//            var supplierList = db.Suppliers.Where(d => !supplierWithQuotation.Contains(d.ID) && d.ID != 1).ToList()
//                               .Select(d => new SupplierVM
//                               {
//                                   ID = d.ID,
//                                   SupplierName = d.SupplierName,
//                                   Address = d.Address,
//                                   ContactPerson = d.ContactPerson,
//                                   ContactNumber = d.ContactNumber,
//                                   EmailAddress = d.EmailAddress,
//                                   TaxIdNumber = d.TaxIdNumber
//                               }).ToList();
//            return supplierList.OrderBy(d => d.SupplierName).ToList();
//        }
//        public List<SupplierVM> GetSuppliers(string SolicitationNo, int SupplierReference)
//        {
//            var supplierWithQuotation = db.RequestForQuotationHeader.Where(d => d.FKRFQReference.SolicitationNo == SolicitationNo && d.SupplierReference == 1).Select(d => d.SupplierReference).ToList();
//            var supplierList = db.Suppliers.Where(d => !supplierWithQuotation.Contains(d.ID) && d.ID != 1).ToList()
//                               .Select(d => new SupplierVM
//                               {
//                                   ID = d.ID,
//                                   SupplierName = d.SupplierName,
//                                   Address = d.Address,
//                                   ContactPerson = d.ContactPerson,
//                                   ContactNumber = d.ContactNumber,
//                                   EmailAddress = d.EmailAddress,
//                                   TaxIdNumber = d.TaxIdNumber
//                               }).ToList();
//            return supplierList.OrderBy(d => d.SupplierName).ToList();
//        }
//        public List<RequestForQuotationVM> GetQuotations(int FiscalYear)
//        {
//            return db.RequestForQuotation.Where(d => d.FiscalYear == FiscalYear).ToList().Select(d => new RequestForQuotationVM
//            {
//                FiscalYear = d.FiscalYear,
//                SolicitationNo = d.SolicitationNo,
//                ContractCode = d.FKProcurementProject.ContractCode,
//                ContractName = d.FKProcurementProject.ContractName,
//                Deadline = d.Deadline,
//                CreatedAt = d.CreatedAt,
//                CreatedBy = hris.GetEmployeeByCode(d.CreatedBy).EmployeeName,
//                IsSubmissionOpen = d.IsSubmissionOpen,
//                AbstractPreparedAt = d.AbstractCreatedAt,
//                AbstractPreparedBy = d.AbstractCreatedBy == null ? null : hris.GetEmployeeByCode(d.AbstractCreatedBy).EmployeeName
//            }).ToList();
//        }
//        public List<AlternativeModeProjectsVM> GetAlternativeData()
//        {
//            var alternativeProject = db.AlternativeModeProjects
//                                       .Where(d => d.ProjectStage == AlternativeModeStages.PurchaseRequestSubmissionClosing &&
//                                                   d.FKAPPDetailsReference.ProcurementSource != ProcurementSources.AgencyToAgency &&
//                                                   d.RFQReference == null).ToList()
//            .Select(d => new AlternativeModeProjectsVM
//            {
//                APPDetailsReference = d.APPDetailsReference,
//                PAPCode = d.FKAPPDetailsReference.PAPCode,
//                FiscalYear = d.FiscalYear,
//                FundSource = d.FundSource,
//                FundDescription = abis.GetFundSources(d.FundSource).FUND_DESC,
//                ContractCode = d.ContractCode,
//                ContractName = d.ContractName,
//                IsEPA = (bool)d.IsEPA,
//                ABC = d.ABC,
//                ModeOfProcurementReference = d.ModeOfProcurementReference,
//                ModeOfProcurement = d.FKModeOfProcurementReference.ModeOfProcurementName,
//                ProjectStatus = d.ProjectStatus,
//                ProjectStage = d.ProjectStage,
//                DeliveryPeriod = d.DeliveryPeriod,
//                PRSubmissionOpen = d.PRSubmissionOpen,
//                PRSubmissionClose = d.PRSubmissionClose,
//                PreparationOfRFQ = d.PreparationOfRFQ,
//                PostingOfRFQ = d.PostingOfRFQ,
//                ClosingOfSubmissionOfRFQ = d.ClosingOfSubmissionOfRFQ,
//                OpeningOfQuotations = d.OpeningOfQuotations,
//                NOAIssuance = d.NOAIssuance,
//                NTPIssuance = d.NTPIssuance,
//                ProjectCoordinator = d.ProjectCoordinator,
//                ProjectSupport = d.ProjectSupport,
//                ProjectDetails = db.AlternativeProjectDetails.Where(x => x.AlternativeModeProjectsReference == d.ID).ToList()
//                                 .Select(x => new AlternativeModeProjectsDetailsVM
//                                 {
//                                     APPDetailReference = d.APPDetailsReference,
//                                     ApprovedBudget = x.ApprovedBudget,
//                                     FundSource = d.FundSource,
//                                     FundDescription = abis.GetFundSources(d.FundSource).FUND_DESC,
//                                     ArticleReference = x.ArticleReference,
//                                     ItemSequence = x.ItemSequence,
//                                     ItemFullName = x.ItemFullName,
//                                     UnitOfMeasure = x.FKUOMReference.Abbreviation,
//                                     ItemSpecifications = x.ItemSpecifications,
//                                     UOMReference = x.UOMReference,
//                                     UnitCost = x.UnitCost,
//                                     Quantity = x.Quantity
//                                 }).ToList()
//            }).ToList();

//            return alternativeProject;
//        }
//        public AlternativeModeProjectsVM GetAlternativeData(string ContractCode)
//        {
//            var alternativeProject = db.AlternativeModeProjects.Where(d => d.ContractCode == ContractCode).ToList()
//            .Select(d => new AlternativeModeProjectsVM
//            {
//                APPDetailsReference = d.APPDetailsReference,
//                PAPCode = d.FKAPPDetailsReference.PAPCode,
//                FiscalYear = d.FiscalYear,
//                FundSource = d.FundSource,
//                FundDescription = abis.GetFundSources(d.FundSource).FUND_DESC,
//                ContractCode = d.ContractCode,
//                ContractName = d.ContractName,
//                IsEPA = (bool)d.IsEPA,
//                ABC = d.ABC,
//                ModeOfProcurementReference = d.ModeOfProcurementReference,
//                ModeOfProcurement = d.FKModeOfProcurementReference.ModeOfProcurementName,
//                ProjectStatus = d.ProjectStatus,
//                ProjectStage = d.ProjectStage,
//                DeliveryPeriod = d.DeliveryPeriod,
//                PRSubmissionOpen = d.PRSubmissionOpen,
//                PRSubmissionClose = d.PRSubmissionClose,
//                PreparationOfRFQ = d.PreparationOfRFQ,
//                PostingOfRFQ = d.PostingOfRFQ,
//                ClosingOfSubmissionOfRFQ = d.ClosingOfSubmissionOfRFQ,
//                OpeningOfQuotations = d.OpeningOfQuotations,
//                NOAIssuance = d.NOAIssuance,
//                NTPIssuance = d.NTPIssuance,
//                ProjectCoordinator = d.ProjectCoordinator,
//                ProjectSupport = d.ProjectSupport,
//                ProjectDetails = db.AlternativeProjectDetails.Where(x => x.AlternativeModeProjectsReference == d.ID).ToList()
//                                 .Select(x => new AlternativeModeProjectsDetailsVM
//                                 {
//                                     APPDetailReference = d.APPDetailsReference,
//                                     ApprovedBudget = x.ApprovedBudget,
//                                     FundSource = d.FundSource,
//                                     FundDescription = abis.GetFundSources(d.FundSource).FUND_DESC,
//                                     ArticleReference = x.ArticleReference,
//                                     ItemSequence = x.ItemSequence,
//                                     ItemFullName = x.ItemFullName,
//                                     UnitOfMeasure = x.FKUOMReference.Abbreviation,
//                                     ItemSpecifications = x.ItemSpecifications,
//                                     UOMReference = x.UOMReference,
//                                     UnitCost = x.UnitCost,
//                                     Quantity = x.Quantity
//                                 }).ToList()
//            }).FirstOrDefault();

//            return alternativeProject;
//        }
//        public RequestForQuotationVM RequestForQuotationSetup(string ContractCode)
//        {
//            var alternativeProject = db.ProcurementProjects.Where(d => d.ContractCode == ContractCode).FirstOrDefault();
//            return new RequestForQuotationVM
//            {
//                FiscalYear = alternativeProject.FiscalYear,
//                SolicitationNo = GenerateSolicitationNo(alternativeProject.FKModeOfProcurementReference.ShortName, alternativeProject.FiscalYear),
//                ContractCode = alternativeProject.ContractCode,
//                ContractName = alternativeProject.ContractName,
//                Deadline = (DateTime)alternativeProject.DeadlineOfSubmissionOfBids_DeadlineOfSubmisionOfBids
//            };
//        }
//        public RequestForQuotationFullVM RequestForQuotationDetails(string SolicitationNo)
//        {
//            var rfq = db.RequestForQuotation.Where(d => d.SolicitationNo == SolicitationNo).FirstOrDefault();
//            var contract = db.ProcurementProjects.Where(d => d.ID == rfq.ProcurementProject).FirstOrDefault();
//            var rfqVM = new RequestForQuotationFullVM
//            {
//                FiscalYear = rfq.FiscalYear,
//                ContractCode = contract.ContractCode,
//                ContractName = contract.ContractName,
//                ABC = contract.ApprovedBudgetForContract,
//                ModeOfProcurement = contract.FKModeOfProcurementReference.ModeOfProcurementName,
//                SolicitationNo = rfq.SolicitationNo,
//                Deadline = rfq.Deadline,
//                IsSubmissionOpen = rfq.IsSubmissionOpen,
//                Quotations = db.RequestForQuotationHeader.Where(d => d.RFQReference == rfq.ID)
//                             .Select(d => new QuotationVM {
//                                QuotationNo = d.QuotationNo,
//                                SupplierReference= d.FKSupplierReference.ID,
//                                SupplierName = d.FKSupplierReference.SupplierName,
//                                Address = d.FKSupplierReference.Address,
//                                ContactPerson = d.FKSupplierReference.ContactPerson,
//                                ContactNumber = d.FKSupplierReference.ContactNumber,
//                                EmailAddress = d.FKSupplierReference.EmailAddress,
//                                AlternateContactNumber = d.FKSupplierReference.AlternateContactNumber,
//                                TaxIdNumber = d.FKSupplierReference.TaxIdNumber,
//                                RecordedAt = d.RecordedAt,
//                                QuotationDetails = db.RequestForQuotationDetails.Where(x => x.RFQHeaderReference == d.ID).ToList()
//                                                    .Select(x => new RequestForQuotationDetailsVM {
//                                                        ArticleReference = x.ArticleReference,
//                                                        ItemSequence = x.ItemSequence,
//                                                        ItemFullName = x.ItemFullName,
//                                                        ItemSpecifications = x.ItemSpecifications,
//                                                        UnitOfMeasure = x.FKUOMReference.Abbreviation,
//                                                        UOMReference = x.UOMReference,
//                                                        Quantity = x.Quantity,
//                                                        UnitPrice = x.UnitPrice,
//                                                        TotalUnitPrice = x.TotalUnitPrice
//                                                    }).ToList()
//                             }).ToList()
//            };
//            return rfqVM;
//        }
//        public RequestForQuotationForPostingVM RequestForQuotationDetailsForPosting(string ContractCode)
//        {
//            var rfq = db.RequestForQuotation.Where(d => d.FKProcurementProject.ContractCode == ContractCode).FirstOrDefault();
//            var contract = db.ProcurementProjects.Where(d => d.ID == rfq.ProcurementProject).FirstOrDefault();
//            var quotationNo = db.PPMPDetails.Where(d => d.FKProcurementProject.ContractCode == contract.ContractCode).ToList()
//                                .Select(d => String.Join(", ", d.FKPurchaseRequestReference.PRNumber)).FirstOrDefault();
//            var rfqVM = new RequestForQuotationForPostingVM
//            {
//                FiscalYear = rfq.FiscalYear,
//                ContractCode = contract.ContractCode,
//                ContractName = contract.ContractName,
//                ABC = contract.ApprovedBudgetForContract,
//                ModeOfProcurement = contract.FKModeOfProcurementReference.ModeOfProcurementName,
//                SolicitationNo = rfq.SolicitationNo,
//                Deadline = rfq.Deadline,
//                QuotationNo = quotationNo,
//                Date = rfq.CreatedAt,
//                Details = db.ProcurementProjectDetails.Where(x => x.ProcurementProject == contract.ID).ToList()
//                             .Select(x => new RequestForQuotationDetailsVM
//                             {
//                                 ArticleReference = x.ArticleReference,
//                                 ItemSequence = x.ItemSequence,
//                                 ItemFullName = x.ItemFullName,
//                                 ItemSpecifications = x.ItemSpecifications == null ? string.Empty : x.ItemSpecifications, 
//                                 UnitOfMeasure = x.FKUOMReference.Abbreviation,
//                                 UOMReference = x.UOMReference,
//                                 Quantity = x.Quantity
//                             }).ToList()
//            };

//            return rfqVM;
//        }
//        public List<RequestForQuotationDetailsVM> GetRFQItem(string SolicitationNo)
//        {
//            var contractDetails = db.AlternativeProjectDetails.Where(d => d.FKAlternativeModeProjectsReference.FKRFQReference.SolicitationNo == SolicitationNo)
//                                  .Select(d => new RequestForQuotationDetailsVM{
//                                      ArticleReference = d.ArticleReference,
//                                      ItemSequence = d.ItemSequence,
//                                      ItemFullName = d.ItemFullName,
//                                      ItemSpecifications = d.ItemSpecifications,
//                                      UOMReference = d.UOMReference,
//                                      UnitOfMeasure = d.FKUOMReference.Abbreviation,
//                                      Quantity = d.Quantity
//                                  }).ToList();
//            return contractDetails;
//        }
//        public AbstactOfQuotationVM GetAbstractOfQuotation(string SolicitationNo)
//        {
//            var contract = db.AlternativeModeProjects.Where(d => d.FKRFQReference.SolicitationNo == SolicitationNo).FirstOrDefault();
//            var rfq = db.RequestForQuotation.Where(d => d.SolicitationNo == SolicitationNo).ToList().Select(d => new AbstactOfQuotationVM {
//                FiscalYear = d.FiscalYear,
//                ContractCode = contract.ContractCode,
//                ContractName = contract.ContractName,
//                ABC = contract.ABC,
//                OpenedAt = d.OpenedAt.Value,
//                PlaceOpened = d.PlaceOpened,
//                Suppliers = new List<SupplierQuotesVM>()
//            }).FirstOrDefault();

//            var supplierQuotes = db.RequestForQuotationHeader.Where(d => d.FKRFQReference.SolicitationNo == SolicitationNo).ToList();
//            foreach(var supplier in supplierQuotes)
//            {
//                var totalBid = db.RequestForQuotationDetails.Where(x => x.RFQHeaderReference == supplier.ID).Any(d => d.TotalUnitPrice.HasValue == false) ? (decimal?)null : (decimal)db.RequestForQuotationDetails.Where(x => x.RFQHeaderReference == supplier.ID).Sum(x => x.TotalUnitPrice);
//                var variance = totalBid == null ? (decimal?)null : Math.Abs((((decimal)totalBid - contract.ABC) / contract.ABC));
//                var savings = totalBid == null ? (decimal?)null : Math.Abs((decimal)totalBid - contract.ABC);
//                rfq.Suppliers.Add(new SupplierQuotesVM
//                {
//                    SupplierName = supplier.FKSupplierReference.SupplierName,
//                    EligibilityRequirements = supplier.EligibilityRequirements,
//                    TechnicalRequirements = supplier.TechnicalRequirements,
//                    FinancialRequirements = supplier.FinancialRequirements,
//                    RecommendToAward = supplier.RecommendToAward,
//                    LCRQ = supplier.LCRQ,
//                    Remarks = supplier.Remarks,
//                    TotalBid = totalBid,
//                    Savings = savings,
//                    Variance = variance
//                });
//            }

//            return rfq;
//        }
//        public QuotationVM ViewQuotation(string QuotationNo)
//        {
//            return db.RequestForQuotationHeader.Where(d => d.QuotationNo == QuotationNo).ToList()
//                   .Select(d => new QuotationVM
//                   {
//                       QuotationNo = d.QuotationNo,
//                       RecordedAt = d.RecordedAt,
//                       SubmittedAt = (DateTime)d.SubmittedAt,
//                       SupplierReference = (int)d.SupplierReference,
//                       SupplierName = d.FKSupplierReference.SupplierName,
//                       Address = d.FKSupplierReference.Address,
//                       ContactPerson = d.FKSupplierReference.ContactPerson,
//                       ContactNumber = d.FKSupplierReference.ContactNumber,
//                       AlternateContactNumber = d.FKSupplierReference.AlternateContactNumber,
//                       EmailAddress = d.FKSupplierReference.EmailAddress,
//                       TaxIdNumber = d.FKSupplierReference.TaxIdNumber,
//                       QuotationDetails = db.RequestForQuotationDetails.Where(x => x.RFQHeaderReference == d.ID).ToList()
//                                          .Select(x => new RequestForQuotationDetailsVM {
//                                              ArticleReference = x.ArticleReference,
//                                              ItemSequence = x.ItemSequence,
//                                              ItemFullName = x.ItemFullName,
//                                              ItemSpecifications = x.ItemSpecifications,
//                                              NoOffer = x.NoOffer,
//                                              Quantity = x.Quantity,
//                                              UnitPrice = x.UnitPrice,
//                                              UnitOfMeasure = x.FKUOMReference.Abbreviation,
//                                              UOMReference = x.UOMReference
//                                          }).ToList()
//                   }).FirstOrDefault();
//        }
//        public QuoteEvaluationVM EvaluationSetup(string SolicitationNo)
//        {
//            var rfq = db.RequestForQuotation.Where(d => d.SolicitationNo == SolicitationNo && d.IsSubmissionOpen == false).FirstOrDefault();
//            var quote = db.RequestForQuotationHeader.Where(d => d.RFQReference == rfq.ID).ToList();
//            var contract = db.AlternativeModeProjects.Where(d => d.RFQReference == rfq.ID).FirstOrDefault();
//            var contractDetails = db.AlternativeProjectDetails.Where(d => d.AlternativeModeProjectsReference == contract.ID).ToList();
//            var quotes = quote.Select(d => new QuoteVM
//            {
//                QuotationNo = d.QuotationNo,
//                Supplier = d.FKSupplierReference.SupplierName,
//                Items = (from cDetails in contractDetails
//                         join qDetails in db.RequestForQuotationDetails.Where(x => x.RFQHeaderReference == d.ID).ToList() on new { ArticleReference = cDetails.ArticleReference, ItemSequence = cDetails.ItemSequence } equals new { ArticleReference = qDetails.ArticleReference, ItemSequence = qDetails.ItemSequence }
//                         select new
//                         {
//                             ArticleReference = qDetails.ArticleReference,
//                             ItemSequence = qDetails.ItemSequence,
//                             ItemFullName = qDetails.ItemFullName,
//                             ItemSpecifications = qDetails.ItemSpecifications,
//                             UnitOfMeasure = qDetails.FKUOMReference.Abbreviation,
//                             UOMReference = qDetails.UOMReference,
//                             Quantity = qDetails.Quantity,
//                             UnitCost = cDetails.UnitCost,
//                             TotalCost = cDetails.ApprovedBudget,
//                             UnitPrice = qDetails.NoOffer == true ? null : qDetails.UnitPrice,
//                             TotalUnitPrice = qDetails.NoOffer == true ? null : qDetails.TotalUnitPrice,
//                         }).GroupBy(x => new
//                         {
//                             x.ArticleReference,
//                             x.ItemSequence,
//                             x.ItemFullName,
//                             x.ItemSpecifications,
//                             x.UnitOfMeasure,
//                             x.UOMReference,
//                             x.Quantity,
//                             x.UnitCost,
//                             x.UnitPrice
//                         }).Select(x => new QuoteEvaluationDetailsVM {
//                             ArticleReference = x.Key.ArticleReference,
//                             ItemSequence = x.Key.ItemSequence,
//                             ItemFullName = x.Key.ItemFullName,
//                             ItemSpecifications = x.Key.ItemSpecifications,
//                             Quantity = x.Key.Quantity,
//                             UOMReference = x.Key.UOMReference,
//                             UnitOfMeasure = x.Key.UnitOfMeasure,
//                             UnitBidPrice = x.Key.UnitPrice,
//                             TotalBid = x.Sum(y => y.TotalUnitPrice),
//                             UnitCost = x.Key.UnitCost,
//                             TotalCost = x.Sum(y => y.TotalCost),
//                             Variance = Math.Abs(Math.Round(((((decimal)x.Sum(y => y.TotalUnitPrice) - x.Sum(y => y.TotalCost)) / x.Sum(y => y.TotalCost))), 2)),
//                             Savings = Math.Round((x.Sum(y => y.TotalCost) - (decimal)x.Sum(y => y.TotalUnitPrice)), 2)
//                         }).ToList()
//            }).ToList();

//            return new QuoteEvaluationVM
//            {
//                FiscalYear = contract.FiscalYear,
//                SolicitationNo = rfq.SolicitationNo,
//                ContractCode = contract.ContractCode,
//                ContractName = contract.ContractName,
//                ABC = contract.ABC,
//                Details = quotes
//            };
//        }
//        public bool PostRequestForQuotation(RequestForQuotationVM RequestForQuotation, string UserEmail)
//        {
//            var user = db.UserAccounts.Where(d => d.Email == UserEmail).FirstOrDefault();
//            var ProcurementProject = db.ProcurementProjects.Where(d => d.ContractCode == RequestForQuotation.ContractCode).FirstOrDefault();
//            var rfq = new RequestForQuotation
//            {
//                SolicitationNo = RequestForQuotation.SolicitationNo,
//                FiscalYear = RequestForQuotation.FiscalYear,
//                Deadline = RequestForQuotation.Deadline,
//                CreatedAt = DateTime.Now,
//                CreatedBy = user.EmpCode,
//                ModeOfProcurementReference = ProcurementProject.ModeOfProcurementReference,
//                ProcurementProject = ProcurementProject.ID,
//                IsSubmissionOpen = true
//            };
//            db.RequestForQuotation.Add(rfq);
//            ProcurementProject.ProcurementProjectStage = ProcurementProjectStages.BidsOpened;
//            if (db.SaveChanges() == 0)
//            {
//                return false;
//            }

//            //ProcurementProject.RFQReference = rfq.ID;
//            //if (db.SaveChanges() == 0)
//            //{
//            //    return false;
//            //}

//            return true;
//        }
//        public bool PostSupplierQuotation(QuotationVM QuoteVM, string SolicitationNo, string UserEmail)
//        {
//            var user = db.UserAccounts.Where(d => d.Email == UserEmail).FirstOrDefault();
//            var rfq = db.RequestForQuotation.Where(d => d.SolicitationNo == SolicitationNo).FirstOrDefault();
//            var supplierQuote = new RequestForQuotationHeader
//            {
//                QuotationNo = GenerateQuotationNo(),
//                SupplierReference = QuoteVM.SupplierReference,
//                RFQReference = rfq.ID,
//                RecordedAt = DateTime.Now,
//                SubmittedAt = QuoteVM.SubmittedAt
//            };

//            db.RequestForQuotationHeader.Add(supplierQuote);
//            if(db.SaveChanges() == 0)
//            {
//                return false;
//            }
//            db.RequestForQuotationDetails.AddRange(QuoteVM.QuotationDetails.ToList().Select(d => new RequestForQuotationDetails {
//                ArticleReference = d.ArticleReference,
//                ItemSequence = d.ItemSequence,
//                ItemFullName = d.ItemFullName,
//                ItemSpecifications = d.ItemSpecifications,
//                NoOffer = d.NoOffer,
//                Quantity = d.Quantity,
//                UnitPrice = d.NoOffer == true ? null : d.UnitPrice,
//                TotalUnitPrice = d.NoOffer == true ? null : (decimal?)Math.Round((d.UnitPrice.Value * d.Quantity), 2),
//                UOMReference = d.UOMReference,
//                RFQHeaderReference = supplierQuote.ID
//            }));
//            if (db.SaveChanges() == 0)
//            {
//                return false;
//            }
//            return true;
//        }
//        public bool UpdateSupplierQuotation(QuotationVM QuoteVM, string SolicitationNo, string UserEmail)
//        {
//            var user = db.UserAccounts.Where(d => d.Email == UserEmail).FirstOrDefault();
//            var quotation = db.RequestForQuotationHeader.Where(d => d.QuotationNo == QuoteVM.QuotationNo).FirstOrDefault();
//            quotation.SubmittedAt = QuoteVM.SubmittedAt;
//            if (db.SaveChanges() == 0)
//            {
//                return false;
//            }

//            foreach (var item in QuoteVM.QuotationDetails)
//            {
//                var quoteDetail = db.RequestForQuotationDetails.Where(d => d.ArticleReference == item.ArticleReference && d.ItemSequence == item.ItemSequence && d.RFQHeaderReference == quotation.ID).FirstOrDefault();
//                quoteDetail.UnitPrice = item.NoOffer == true ? null : item.UnitPrice;
//                quoteDetail.TotalUnitPrice = item.NoOffer == true ? null : (decimal?)Math.Round((item.UnitPrice.Value * item.Quantity), 2);
//                quoteDetail.UOMReference = item.UOMReference;
//                if (db.SaveChanges() == 0)
//                {
//                    return false;
//                }
//            }
            
//            return true;
//        }
//        public bool CloseRFQSubmission(string SolicitaitionNo, string UserEmail)
//        {
//            var user = db.UserAccounts.Where(d => d.Email == UserEmail).FirstOrDefault();
//            var rfq = db.RequestForQuotation.Where(d => d.SolicitationNo == SolicitaitionNo).FirstOrDefault();
//            var contract = db.AlternativeModeProjects.Where(d => d.RFQReference == rfq.ID).FirstOrDefault();
//            rfq.IsSubmissionOpen = false;
//            rfq.ClosedAt = DateTime.Now;
//            rfq.ClosedBy = user.EmpCode;
//            contract.ProjectStage = AlternativeModeStages.RFQSubmissionClosing;

//            db.AlternativeProjectUpdates.Add(new AlternativeModeProjectsUpdates
//            {
//                AlternativeProjectReference = contract.ID,
//                DateUpdated = DateTime.Now,
//                ProjectStages = AlternativeModeStages.RFQSubmissionClosing,
//                Remarks = "Request for Quotation submission closed.",
//                UpdatedBy = user.EmpCode
//            });

//            if (db.SaveChanges() == 0)
//            {
//                return false;
//            }
//            return true;
//        }
//        public bool PostAbstractOfQuotations(QuoteEvaluationVM QuoteEvaluation, string UserEmail)
//        {
//            var quotation = QuoteEvaluation.Details;
//            foreach(var detail in quotation)
//            {
//                var supplierQuote = db.RequestForQuotationHeader.Where(d => d.QuotationNo == detail.QuotationNo).FirstOrDefault();
//                supplierQuote.EligibilityRequirements = detail.EligibilityRequirements;
//                supplierQuote.TechnicalRequirements = detail.TechnicalRequirements;
//                supplierQuote.FinancialRequirements = detail.FinancialRequirements;
//                supplierQuote.RecommendToAward = detail.RecommendToAward;
//                supplierQuote.Remarks = detail.Remarks;

//                if(db.SaveChanges() == 0)
//                {
//                    return false;
//                }
//            }

//            var user = db.UserAccounts.Where(d => d.Email == UserEmail).FirstOrDefault();
//            var rfq = db.RequestForQuotation.Where(d => d.SolicitationNo == QuoteEvaluation.SolicitationNo).FirstOrDefault();
//            var recommendedSupplier = db.RequestForQuotationHeader.Where(d => d.RFQReference == rfq.ID && d.RecommendToAward == true).Select(d => d.SupplierReference).FirstOrDefault();
//            var contract = db.AlternativeModeProjects.Where(d => d.RFQReference == rfq.ID).FirstOrDefault();
//            contract.SupplierReference = recommendedSupplier;
//            rfq.PlaceOpened = QuoteEvaluation.PlaceOpened;
//            rfq.OpenedAt = QuoteEvaluation.OpenedAt;
//            rfq.AbstractCreatedAt = DateTime.Now;
//            rfq.AbstractCreatedBy = user.EmpCode;

//            if (db.SaveChanges() == 0)
//            {
//                return false;
//            }

//            return true;
//        }
//        private string GenerateSolicitationNo(string ModeOfProcurement, int FiscalYear)
//        {
//            string solicitationNo = string.Empty;
//            var series = (db.RequestForQuotation.Where(d => d.FKModeOfProcurementReference.ShortName == ModeOfProcurement && d.FiscalYear == FiscalYear).Count() + 1).ToString();
//            solicitationNo = "RFQ-" + ModeOfProcurement + "-" + FiscalYear.ToString() + "-" + (series.Length == 1 ? "00" + series : series.Length == 2 ? "0" + series : series);
//            return solicitationNo;
//        }
//        private string GenerateQuotationNo()
//        {
//            var series = (db.RequestForQuotationHeader.Count() + 1).ToString();
//            return DateTime.Now.ToString("yy") + "-" + DateTime.Now.ToString("MM") + "-" + (series.Length == 1 ? "00" + series : series.Length == 2 ? "0" + series : series);
//        }

//        protected override void Dispose(bool disposing)
//        {
//            if (disposing)
//            {
//                db.Dispose();
//                abis.Dispose();
//                hris.Dispose();
//                systemBDL.Dispose();
//            }
//            base.Dispose(disposing);
//        }
//    }
//}