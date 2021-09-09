using Microsoft.Ajax.Utilities;
using PUPFMIS.Models;
using PUPFMIS.Models.AIS;
using PUPFMIS.Models.HRIS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace PUPFMIS.BusinessAndDataLogic
{
    public class AnnualProcurementPlanCSEBL : Controller
    {
        private AnnualProcurementPlanCSEDAL appCSEDAL = new AnnualProcurementPlanCSEDAL();
        private FMISDbContext db = new FMISDbContext();

        public MemoryStream GenerateAPPCSE(string ReferenceNo, string LogoPath, string UserEmail)
        {
            //var ReferenceNo = db.APPHeader.Where(d => d.FiscalYear == FiscalYear && d.APPType == "CSE").FirstOrDefault().ReferenceNo;
            AnnualProcurementPlanCSEVM appCSEVM = appCSEDAL.GetAPPCSE(ReferenceNo);
            Reports APPReport = new Reports();
            APPReport.ReportFilename = ReferenceNo;
            APPReport.CreateDocument(8.00, 13.00, MigraDoc.DocumentObjectModel.Orientation.Landscape, 0.25);
            APPReport.AddSingleColumnHeader();
            APPReport.AddColumnHeader(new HeaderLine("APP-CSE 2020 FORM - 10 September 2019\n", 6, false, true, MigraDoc.DocumentObjectModel.ParagraphAlignment.Left));
            APPReport.AddColumnHeader(new HeaderLine("ANNUAL PROCUREMENT PLAN-COMMON SUPPLIES AND EQUIPMENT (APP-CSE) " + appCSEVM.FiscalYear + " FORM\n\n", 10, true, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center));
            APPReport.AddColumnHeader(new HeaderLine("Introduction:\n\n", 7, false, true, MigraDoc.DocumentObjectModel.ParagraphAlignment.Left));
            APPReport.AddColumnHeader(new HeaderLine("Listed in this template are all the common supplies and equipment (CSE) carried in stock by the Procurement Service (PS) that may be purchased by government agencies. Agencies must accomplish this form and submit  in order to purchase CSEs from the PS.  Consistent with DBM Circular No. 2018-10 dated November 8,2018 , the APP-CSE shall serve as the agency's APR for all its CSE requirements. Items in the template has been arranged in accordance with UNSPSC coding and this is in preparation for integration of the APP-CSE template in the Modernized Government Electronic Procurement System (MGEPS).", 7, true, MigraDoc.DocumentObjectModel.ParagraphAlignment.Justify));
            APPReport.AddColumnHeader(new HeaderLine("\n\nInstructions:\n\n", 7, false, true, MigraDoc.DocumentObjectModel.ParagraphAlignment.Left));
            APPReport.AddColumnHeader(new HeaderLine("1. Download the worksheet file APP-CSE 2020 template at www.ps-philgeps.gov.ph", 7, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Left));
            APPReport.AddColumnHeader(new HeaderLine("2. Indicate the agency’s monthly requirement per item in the APP-CSE 2020 form.", 7, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Left));
            APPReport.AddColumnHeader(new HeaderLine("3. The agency should indicate zero if an item is not being purchased by the agency or purchased for a particular month.", 7, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Left));
            APPReport.AddColumnHeader(new HeaderLine("4. Agency must not delete any item in the template; neither should it include line items or revise the template.", 7, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Left));
            APPReport.AddColumnHeader(new HeaderLine("5. An APP-CSE is considered incorrect or invalid if", 7, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Left));
            APPReport.AddColumnHeader(new HeaderLine("a. form used is other than the prescribed format which can be downloaded only at www.ps- philgeps.gov.ph and;", 7, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Left, 11));
            APPReport.AddColumnHeader(new HeaderLine("b. correct format is used but fields were deleted and/or inserted  in PART I of the template", 7, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Left, 11));
            APPReport.AddColumnHeader(new HeaderLine("6. Fill out the CSE requirements that are available for purchase in the PS under the PART I.  For other Items that are not available from the PS but is regularly purchased by the agency from other sources, agency must indicate the items  in the PART II  and indicate likewise the unit prices based on its last purchase.", 7, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Left));
            APPReport.AddColumnHeader(new HeaderLine("7. Once accomplished and finalized, the APP-CSE 2019 form should be:", 7, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Left));
            APPReport.AddColumnHeader(new HeaderLine("a. Saved using this format: APP2020_Name of Agency_Main or Regional Office (e.g. APP2020 _DBM_Central Office, APP2020 _DBM_Region IVA).", 7, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Left, 11));
            APPReport.AddColumnHeader(new HeaderLine("b. Printed and signed by the agency Property/Supply Officer, Budget Officer and Head of the Procuring Entity.  An unsigned APP-CSE or that which lacks any of the three (3) signatures will be considered as an invalid submission.", 7, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Left, 11));
            APPReport.AddColumnHeader(new HeaderLine("8. The SIGNED COPY of the APP-CSE must be scanned and saved as pdf format for reference of the agency. The file in excel format should be submitted online using the Virtual Store (VS) facility at PhilGEPS website. (Only buyer coordinators will be allowed to upload APP-CSEs.)", 7, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Left));
            APPReport.AddColumnHeader(new HeaderLine("9. An agency may revise its APP-CSE during the year if there will be changes in its requirements.  However, it should submit an original APP-CSE within the prescribed deadline.  Agency may follow the same procedure as indicated in No. 7 when submitting the revised copy. All requirements in excess of the quantities indicated in the original APP-CSE will not be served if not covered by a revised APP-CSE.", 7, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Left));
            APPReport.AddColumnHeader(new HeaderLine("10. For further assistance/clarification, agencies may call the Marketing and Sales Division of the Procurement Service at telephone no. (02)689-7750 local 4019.", 7, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Left));
            APPReport.AddColumnHeader(new TextWithFormat[]
            {
                new TextWithFormat("\n\nNote: Consistent with ", false, true, 7),
                new TextWithFormat("Memorandum Circular No. 2019 -1 dated 03 September 2019, issued by AO 25, ", true, true, 7),
                new TextWithFormat("the APP-CSE for FY 2020 must be submitted on or before ", false, true, 7),
                new TextWithFormat("October 31, 2019.", true, false, 7)
            }, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center);
            APPReport.AddNewLine();
            var columns = new List<ContentColumn>();
            columns.Add(new ContentColumn(0.50));
            columns.Add(new ContentColumn(1.3));
            columns.Add(new ContentColumn(2.7));
            columns.Add(new ContentColumn(0.25));
            columns.Add(new ContentColumn(1.5));
            columns.Add(new ContentColumn(1.7));
            columns.Add(new ContentColumn(0.25));
            columns.Add(new ContentColumn(1.7));
            columns.Add(new ContentColumn(2.1));
            columns.Add(new ContentColumn(0.75));
            APPReport.AddTable(columns, false);

            var rows = new List<ContentCell>();
            rows.Add(new ContentCell("", 0));
            rows.Add(new ContentCell("Department/Bureau/Office:", 1, 7, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Left, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Bottom));
            rows.Add(new ContentCell(appCSEVM.AgencyName, 2, 7, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Left, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Bottom, 0, 0, true));
            rows.Add(new ContentCell("", 3));
            rows.Add(new ContentCell("Agency Account Code:", 4, 7, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Left, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Bottom));
            rows.Add(new ContentCell(appCSEVM.AccountCode, 5, 7, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Left, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Bottom, 0, 0, true));
            rows.Add(new ContentCell("", 6));
            rows.Add(new ContentCell("Contact Person:", 7, 7, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Left, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Bottom));
            rows.Add(new ContentCell(appCSEVM.PreparedBy, 8, 7, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Left, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Bottom, 0, 0, true));
            rows.Add(new ContentCell("", 9));
            APPReport.AddRowContent(rows, 0);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("", 0));
            rows.Add(new ContentCell("Region:", 1, 7, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Left, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Bottom));
            rows.Add(new ContentCell(appCSEVM.Region, 2, 7, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Left, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Bottom, 0, 0, true));
            rows.Add(new ContentCell("", 3));
            rows.Add(new ContentCell("Organization Type:", 4, 7, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Left, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Bottom));
            rows.Add(new ContentCell(appCSEVM.OrganizationType, 5, 6, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Left, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Bottom, 0, 0, true));
            rows.Add(new ContentCell("", 6));
            rows.Add(new ContentCell("Position:", 7, 7, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Left, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Bottom));
            rows.Add(new ContentCell(appCSEVM.PreparedByDesignation + ", " + appCSEVM.PreparedByDepartment, 8, 6, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Left, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Bottom, 0, 0, true));
            rows.Add(new ContentCell("", 9));
            APPReport.AddRowContent(rows, 0);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("", 0));
            rows.Add(new ContentCell("Address:", 1, 7, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Left, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Top, 0, 1));
            rows.Add(new ContentCell(appCSEVM.Address, 2, 7, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Left, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Top, 0, 1, true));
            rows.Add(new ContentCell("", 3));
            rows.Add(new ContentCell("", 4, 7, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Left, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Bottom, 0, 1));
            rows.Add(new ContentCell("", 5, 6, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Left, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Bottom, 0, 1));
            rows.Add(new ContentCell("", 6));
            rows.Add(new ContentCell("Email:", 7, 7, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Left, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Bottom));
            rows.Add(new ContentCell("", 8, 6, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Left, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Bottom, 0, 0, true));
            rows.Add(new ContentCell("", 9));
            APPReport.AddRowContent(rows, 0);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("", 0));
            rows.Add(new ContentCell("", 2, 7, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Left, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Top, 0, 1, true));
            rows.Add(new ContentCell("", 3));
            rows.Add(new ContentCell("", 6));
            rows.Add(new ContentCell("Telephone/Mobile Nos.:", 7, 7, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Left, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Bottom));
            rows.Add(new ContentCell("", 8, 6, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Left, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Bottom, 0, 0, true));
            rows.Add(new ContentCell("", 9));
            APPReport.AddRowContent(rows, 0);

            APPReport.AddNewLine();

            columns = new List<ContentColumn>();
            columns.Add(new ContentColumn(2.15, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
            columns.Add(new ContentColumn(0.50, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
            columns.Add(new ContentColumn(0.30, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
            columns.Add(new ContentColumn(0.30, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
            columns.Add(new ContentColumn(0.30, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
            columns.Add(new ContentColumn(0.50, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
            columns.Add(new ContentColumn(0.50, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
            columns.Add(new ContentColumn(0.30, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
            columns.Add(new ContentColumn(0.30, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
            columns.Add(new ContentColumn(0.30, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
            columns.Add(new ContentColumn(0.50, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
            columns.Add(new ContentColumn(0.50, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
            columns.Add(new ContentColumn(0.30, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
            columns.Add(new ContentColumn(0.30, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
            columns.Add(new ContentColumn(0.30, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
            columns.Add(new ContentColumn(0.50, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
            columns.Add(new ContentColumn(0.50, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
            columns.Add(new ContentColumn(0.30, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
            columns.Add(new ContentColumn(0.30, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
            columns.Add(new ContentColumn(0.30, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
            columns.Add(new ContentColumn(0.50, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
            columns.Add(new ContentColumn(0.50, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
            columns.Add(new ContentColumn(0.75, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
            columns.Add(new ContentColumn(0.75, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
            columns.Add(new ContentColumn(0.75, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
            APPReport.AddTable(columns, true);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("Item and Specifications", 0, 7, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center, 0, 1));
            rows.Add(new ContentCell("Unit of\nMeasure", 1, 6.5, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center, 0, 1));
            rows.Add(new ContentCell("Monthly Quantity Requirement", 2, 7, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center, 19, 0));
            rows.Add(new ContentCell("Total\nQuantity\nfor the Year", 22, 7, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center, 0, 1));
            rows.Add(new ContentCell("Price\nCatalogue", 23, 7, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center, 0, 1));
            rows.Add(new ContentCell("Total Amount\nfor the Year", 24, 7, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center, 0, 1));
            APPReport.AddRowContent(rows, 0.25);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("Jan", 2, 7, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center, 0, 0));
            rows.Add(new ContentCell("Feb", 3, 7, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center, 0, 0));
            rows.Add(new ContentCell("Mar", 4, 7, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center, 0, 0));
            rows.Add(new ContentCell("Q1", 5, 7, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center, 0, 0));
            rows.Add(new ContentCell("Q1\nAmount", 6, 7, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center, 0, 0));
            rows.Add(new ContentCell("Apr", 7, 7, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center, 0, 0));
            rows.Add(new ContentCell("May", 8, 7, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center, 0, 0));
            rows.Add(new ContentCell("Jun", 9, 7, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center, 0, 0));
            rows.Add(new ContentCell("Q2", 10, 7, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center, 0, 0));
            rows.Add(new ContentCell("Q2\nAmount", 11, 7, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center, 0, 0));
            rows.Add(new ContentCell("Jul", 12, 7, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center, 0, 0));
            rows.Add(new ContentCell("Aug", 13, 7, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center, 0, 0));
            rows.Add(new ContentCell("Sep", 14, 7, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center, 0, 0));
            rows.Add(new ContentCell("Q3", 15, 7, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center, 0, 0));
            rows.Add(new ContentCell("Q3\nAmount", 16, 7, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center, 0, 0));
            rows.Add(new ContentCell("Oct", 17, 7, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center, 0, 0));
            rows.Add(new ContentCell("Nov", 18, 7, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center, 0, 0));
            rows.Add(new ContentCell("Dec", 19, 7, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center, 0, 0));
            rows.Add(new ContentCell("Q4", 20, 7, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center, 0, 0));
            rows.Add(new ContentCell("Q4\nAmount", 21, 7, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center, 0, 0));
            APPReport.AddRowContent(rows, 0.25);

            columns = new List<ContentColumn>();
            columns.Add(new ContentColumn(12.5, new MigraDoc.DocumentObjectModel.Color(252, 207, 101)));
            APPReport.AddTable(columns, true);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("PART I. AVAILABLE AT PROCUREMENT SERVICE STORES", 0, 7, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Left, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center, 0, 0));
            APPReport.AddRowContent(rows, 0.25);


            columns = new List<ContentColumn>();
            columns.Add(new ContentColumn(2.15));
            columns.Add(new ContentColumn(0.50));
            columns.Add(new ContentColumn(0.30));
            columns.Add(new ContentColumn(0.30));
            columns.Add(new ContentColumn(0.30));
            columns.Add(new ContentColumn(0.50));
            columns.Add(new ContentColumn(0.50));
            columns.Add(new ContentColumn(0.30));
            columns.Add(new ContentColumn(0.30));
            columns.Add(new ContentColumn(0.30));
            columns.Add(new ContentColumn(0.50));
            columns.Add(new ContentColumn(0.50));
            columns.Add(new ContentColumn(0.30));
            columns.Add(new ContentColumn(0.30));
            columns.Add(new ContentColumn(0.30));
            columns.Add(new ContentColumn(0.50));
            columns.Add(new ContentColumn(0.50));
            columns.Add(new ContentColumn(0.30));
            columns.Add(new ContentColumn(0.30));
            columns.Add(new ContentColumn(0.30));
            columns.Add(new ContentColumn(0.50));
            columns.Add(new ContentColumn(0.50));
            columns.Add(new ContentColumn(0.75));
            columns.Add(new ContentColumn(0.75));
            columns.Add(new ContentColumn(0.75));
            APPReport.AddTable(columns, true);

            if (appCSEVM.APPDBMItems.Count == 0)
            {
                rows = new List<ContentCell>();
                rows.Add(new ContentCell("", 0, 5, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Left, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                rows.Add(new ContentCell("", 1, 5, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                rows.Add(new ContentCell(String.Format("{0:#,##0}", 0), 2, 5, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                rows.Add(new ContentCell(String.Format("{0:#,##0}", 0), 3, 5, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                rows.Add(new ContentCell(String.Format("{0:#,##0}", 0), 4, 5, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                rows.Add(new ContentCell(String.Format("{0:#,##0}", 0), 5, 5, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                rows.Add(new ContentCell(String.Format("{0:#,0.00}", 0), 6, 5, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Right, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                rows.Add(new ContentCell(String.Format("{0:#,##0}", 0), 7, 5, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                rows.Add(new ContentCell(String.Format("{0:#,##0}", 0), 8, 5, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                rows.Add(new ContentCell(String.Format("{0:#,##0}", 0), 9, 5, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                rows.Add(new ContentCell(String.Format("{0:#,##0}", 0), 10, 5, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                rows.Add(new ContentCell(String.Format("{0:#,0.00}", 0), 11, 5, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Right, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                rows.Add(new ContentCell(String.Format("{0:#,##0}", 0), 12, 5, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                rows.Add(new ContentCell(String.Format("{0:#,##0}", 0), 13, 5, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                rows.Add(new ContentCell(String.Format("{0:#,##0}", 0), 14, 5, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                rows.Add(new ContentCell(String.Format("{0:#,##0}", 0), 15, 5, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                rows.Add(new ContentCell(String.Format("{0:#,0.00}", 0), 16, 5, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Right, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                rows.Add(new ContentCell(String.Format("{0:#,##0}", 0), 17, 5, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                rows.Add(new ContentCell(String.Format("{0:#,##0}", 0), 18, 5, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                rows.Add(new ContentCell(String.Format("{0:#,##0}", 0), 19, 5, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                rows.Add(new ContentCell(String.Format("{0:#,##0}", 0), 20, 5, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                rows.Add(new ContentCell(String.Format("{0:#,0.00}", 0), 21, 5, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Right, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                rows.Add(new ContentCell(String.Format("{0:#,##0}", 0), 22, 5, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                rows.Add(new ContentCell(String.Format("{0:#,0.00}", 0), 23, 5, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Right, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                rows.Add(new ContentCell(String.Format("{0:#,0.00}", 0), 24, 5, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Right, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                APPReport.AddRowContent(rows, 0.25);
            }
            else
            {
                foreach (var item in appCSEVM.APPDBMItems)
                {
                    rows = new List<ContentCell>();
                    rows.Add(new ContentCell(item.ItemSpecifications, 0, 5, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Left, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                    rows.Add(new ContentCell(db.UOM.Find(item.UnitOfMeasure).Abbreviation, 1, 5, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                    rows.Add(new ContentCell(String.Format("{0:#,##0}", item.JanQty), 2, 5, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                    rows.Add(new ContentCell(String.Format("{0:#,##0}", item.FebQty), 3, 5, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                    rows.Add(new ContentCell(String.Format("{0:#,##0}", item.MarQty), 4, 5, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                    rows.Add(new ContentCell(String.Format("{0:#,##0}", item.Q1TotalQty), 5, 5, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                    rows.Add(new ContentCell(String.Format("{0:#,0.00}", item.Q1TotalQty), 6, 5, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Right, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                    rows.Add(new ContentCell(String.Format("{0:#,##0}", item.AprQty), 7, 5, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                    rows.Add(new ContentCell(String.Format("{0:#,##0}", item.MayQty), 8, 5, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                    rows.Add(new ContentCell(String.Format("{0:#,##0}", item.JunQty), 9, 5, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                    rows.Add(new ContentCell(String.Format("{0:#,##0}", item.Q2TotalQty), 10, 5, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                    rows.Add(new ContentCell(String.Format("{0:#,0.00}", item.Q2TotalAmount), 11, 5, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Right, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                    rows.Add(new ContentCell(String.Format("{0:#,##0}", item.JulQty), 12, 5, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                    rows.Add(new ContentCell(String.Format("{0:#,##0}", item.AugQty), 13, 5, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                    rows.Add(new ContentCell(String.Format("{0:#,##0}", item.SepQty), 14, 5, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                    rows.Add(new ContentCell(String.Format("{0:#,##0}", item.Q3TotalQty), 15, 5, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                    rows.Add(new ContentCell(String.Format("{0:#,0.00}", item.Q3TotalAmount), 16, 5, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Right, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                    rows.Add(new ContentCell(String.Format("{0:#,##0}", item.OctQty), 17, 5, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                    rows.Add(new ContentCell(String.Format("{0:#,##0}", item.NovQty), 18, 5, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                    rows.Add(new ContentCell(String.Format("{0:#,##0}", item.DecQty), 19, 5, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                    rows.Add(new ContentCell(String.Format("{0:#,##0}", item.Q4TotalQty), 20, 5, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                    rows.Add(new ContentCell(String.Format("{0:#,0.00}", item.Q4TotalAmount), 21, 5, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Right, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                    rows.Add(new ContentCell(String.Format("{0:#,##0}", item.TotalQty), 22, 5, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                    rows.Add(new ContentCell(String.Format("{0:#,0.00}", item.UnitCost), 23, 5, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Right, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                    rows.Add(new ContentCell(String.Format("{0:#,0.00}", item.ApprovedBudget), 24, 5, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Right, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                    APPReport.AddRowContent(rows, 0.25);
                }
            }

            columns = new List<ContentColumn>();
            columns.Add(new ContentColumn(12.5, new MigraDoc.DocumentObjectModel.Color(252, 207, 101)));
            APPReport.AddTable(columns, true);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("PART II. OTHER ITEMS NOT AVALABLE AT PS BUT REGULARLY PURCHASED FROM OTHER SOURCES (Note: Please indicate price of items)", 0, 7, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Left, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center, 0, 0));
            APPReport.AddRowContent(rows, 0.25);

            columns = new List<ContentColumn>();
            columns.Add(new ContentColumn(2.15));
            columns.Add(new ContentColumn(0.50));
            columns.Add(new ContentColumn(0.30));
            columns.Add(new ContentColumn(0.30));
            columns.Add(new ContentColumn(0.30));
            columns.Add(new ContentColumn(0.50));
            columns.Add(new ContentColumn(0.50));
            columns.Add(new ContentColumn(0.30));
            columns.Add(new ContentColumn(0.30));
            columns.Add(new ContentColumn(0.30));
            columns.Add(new ContentColumn(0.50));
            columns.Add(new ContentColumn(0.50));
            columns.Add(new ContentColumn(0.30));
            columns.Add(new ContentColumn(0.30));
            columns.Add(new ContentColumn(0.30));
            columns.Add(new ContentColumn(0.50));
            columns.Add(new ContentColumn(0.50));
            columns.Add(new ContentColumn(0.30));
            columns.Add(new ContentColumn(0.30));
            columns.Add(new ContentColumn(0.30));
            columns.Add(new ContentColumn(0.50));
            columns.Add(new ContentColumn(0.50));
            columns.Add(new ContentColumn(0.75));
            columns.Add(new ContentColumn(0.75));
            columns.Add(new ContentColumn(0.75));
            APPReport.AddTable(columns, true);

            if (appCSEVM.APPNonDBMItems.Count == 0)
            {
                rows = new List<ContentCell>();
                rows.Add(new ContentCell("", 0, 5, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Left, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                rows.Add(new ContentCell("", 1, 5, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                rows.Add(new ContentCell(String.Format("{0:#,##0}", 0), 2, 5, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                rows.Add(new ContentCell(String.Format("{0:#,##0}", 0), 3, 5, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                rows.Add(new ContentCell(String.Format("{0:#,##0}", 0), 4, 5, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                rows.Add(new ContentCell(String.Format("{0:#,##0}", 0), 5, 5, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                rows.Add(new ContentCell(String.Format("{0:#,0.00}", 0), 6, 5, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Right, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                rows.Add(new ContentCell(String.Format("{0:#,##0}", 0), 7, 5, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                rows.Add(new ContentCell(String.Format("{0:#,##0}", 0), 8, 5, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                rows.Add(new ContentCell(String.Format("{0:#,##0}", 0), 9, 5, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                rows.Add(new ContentCell(String.Format("{0:#,##0}", 0), 10, 5, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                rows.Add(new ContentCell(String.Format("{0:#,0.00}", 0), 11, 5, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Right, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                rows.Add(new ContentCell(String.Format("{0:#,##0}", 0), 12, 5, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                rows.Add(new ContentCell(String.Format("{0:#,##0}", 0), 13, 5, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                rows.Add(new ContentCell(String.Format("{0:#,##0}", 0), 14, 5, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                rows.Add(new ContentCell(String.Format("{0:#,##0}", 0), 15, 5, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                rows.Add(new ContentCell(String.Format("{0:#,0.00}", 0), 16, 5, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Right, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                rows.Add(new ContentCell(String.Format("{0:#,##0}", 0), 17, 5, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                rows.Add(new ContentCell(String.Format("{0:#,##0}", 0), 18, 5, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                rows.Add(new ContentCell(String.Format("{0:#,##0}", 0), 19, 5, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                rows.Add(new ContentCell(String.Format("{0:#,##0}", 0), 20, 5, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                rows.Add(new ContentCell(String.Format("{0:#,0.00}", 0), 21, 5, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Right, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                rows.Add(new ContentCell(String.Format("{0:#,##0}", 0), 22, 5, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                rows.Add(new ContentCell(String.Format("{0:#,0.00}", 0), 23, 5, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Right, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                rows.Add(new ContentCell(String.Format("{0:#,0.00}", 0), 24, 5, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Right, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                APPReport.AddRowContent(rows, 0.25);
            }
            else
            {
                foreach (var item in appCSEVM.APPNonDBMItems)
                {
                    rows = new List<ContentCell>();
                    rows.Add(new ContentCell(item.ItemSpecifications, 0, 5, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Left, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                    rows.Add(new ContentCell(db.UOM.Find(item.UnitOfMeasure).Abbreviation, 1, 5, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                    rows.Add(new ContentCell(String.Format("{0:#,##0}", item.JanQty), 2, 5, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                    rows.Add(new ContentCell(String.Format("{0:#,##0}", item.FebQty), 3, 5, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                    rows.Add(new ContentCell(String.Format("{0:#,##0}", item.MarQty), 4, 5, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                    rows.Add(new ContentCell(String.Format("{0:#,##0}", item.Q1TotalQty), 5, 5, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                    rows.Add(new ContentCell(String.Format("{0:#,0.00}", item.Q1TotalAmount), 6, 5, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Right, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                    rows.Add(new ContentCell(String.Format("{0:#,##0}", item.AprQty), 7, 5, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                    rows.Add(new ContentCell(String.Format("{0:#,##0}", item.MayQty), 8, 5, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                    rows.Add(new ContentCell(String.Format("{0:#,##0}", item.JunQty), 9, 5, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                    rows.Add(new ContentCell(String.Format("{0:#,##0}", item.Q2TotalQty), 10, 5, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                    rows.Add(new ContentCell(String.Format("{0:#,0.00}", item.Q2TotalAmount), 11, 5, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Right, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                    rows.Add(new ContentCell(String.Format("{0:#,##0}", item.JulQty), 12, 5, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                    rows.Add(new ContentCell(String.Format("{0:#,##0}", item.AugQty), 13, 5, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                    rows.Add(new ContentCell(String.Format("{0:#,##0}", item.SepQty), 14, 5, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                    rows.Add(new ContentCell(String.Format("{0:#,##0}", item.Q3TotalQty), 15, 5, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                    rows.Add(new ContentCell(String.Format("{0:#,0.00}", item.Q3TotalAmount), 16, 5, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Right, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                    rows.Add(new ContentCell(String.Format("{0:#,##0}", item.OctQty), 17, 5, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                    rows.Add(new ContentCell(String.Format("{0:#,##0}", item.NovQty), 18, 5, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                    rows.Add(new ContentCell(String.Format("{0:#,##0}", item.DecQty), 19, 5, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                    rows.Add(new ContentCell(String.Format("{0:#,##0}", item.Q4TotalQty), 20, 5, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                    rows.Add(new ContentCell(String.Format("{0:#,0.00}", item.Q4TotalAmount), 21, 5, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Right, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                    rows.Add(new ContentCell(String.Format("{0:#,##0}", item.TotalQty), 22, 5, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                    rows.Add(new ContentCell(String.Format("{0:#,0.00}", item.UnitCost), 23, 5, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Right, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                    rows.Add(new ContentCell(String.Format("{0:#,0.00}", item.ApprovedBudget), 24, 5, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Right, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
                    APPReport.AddRowContent(rows, 0.25);
                }
            }


            APPReport.AddNewLine();

            columns = new List<ContentColumn>();
            columns.Add(new ContentColumn(2.15, new MigraDoc.DocumentObjectModel.Color(74, 132, 249)));
            columns.Add(new ContentColumn(8.10, new MigraDoc.DocumentObjectModel.Color(74, 132, 249)));
            columns.Add(new ContentColumn(2.25, new MigraDoc.DocumentObjectModel.Color(74, 132, 249)));
            APPReport.AddTable(columns, true);

            var total = appCSEVM.APPDBMItems.Sum(d => d.ApprovedBudget) + appCSEVM.APPNonDBMItems.Sum(d => d.ApprovedBudget);
            rows = new List<ContentCell>();
            rows.Add(new ContentCell("A. TOTAL", 0, 7, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Left, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
            rows.Add(new ContentCell("", 1, 5, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
            rows.Add(new ContentCell(String.Format("{0:C}", total), 2, 7, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Right, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
            APPReport.AddRowContent(rows, 0.30);

            decimal inflation = Math.Round(((appCSEVM.APPDBMItems.Sum(d => d.ApprovedBudget) + appCSEVM.APPNonDBMItems.Sum(d => d.ApprovedBudget)) * 0.1m), 2);
            rows = new List<ContentCell>();
            rows.Add(new ContentCell("B. ADDITIONAL PROVISION FOR INFLATION (10% of TOTAL)", 0, 7, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Left, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
            rows.Add(new ContentCell("", 1, 5, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
            rows.Add(new ContentCell(String.Format("{0:C}", inflation), 2, 7, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Right, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
            APPReport.AddRowContent(rows, 0.30);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("C.  ADDITIONAL PROVISION FOR TRANSPORT AND FREIGHT COST (If applicable for motor vehicle and other items)", 0, 7, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Left, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
            rows.Add(new ContentCell("", 1, 5, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
            rows.Add(new ContentCell("", 2, 7, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Right, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
            APPReport.AddRowContent(rows, 0.30);

            decimal grandTotal = total + inflation;
            rows = new List<ContentCell>();
            rows.Add(new ContentCell("D. GRAND TOTAL (A + B + C)", 0, 7, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Left, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
            rows.Add(new ContentCell("", 1, 5, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
            rows.Add(new ContentCell(String.Format("{0:C}", grandTotal), 2, 7, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Right, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
            APPReport.AddRowContent(rows, 0.30);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("E.APPROVED BUDGET BY THE AGENCY HEAD In Figures and Words:", 0, 7, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Left, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
            rows.Add(new ContentCell("", 1, 5, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
            rows.Add(new ContentCell("", 2, 7, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Justify, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
            APPReport.AddRowContent(rows, 0.30);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("F. MONTHLY CASH REQUIREMENTS", 0, 7, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Left, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
            rows.Add(new ContentCell("", 1, 5, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
            rows.Add(new ContentCell("", 2, 7, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Justify, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
            APPReport.AddRowContent(rows, 0.30);

            columns = new List<ContentColumn>();
            columns.Add(new ContentColumn(2.65));
            columns.Add(new ContentColumn(0.90));
            columns.Add(new ContentColumn(1.00));
            columns.Add(new ContentColumn(0.90));
            columns.Add(new ContentColumn(1.00));
            columns.Add(new ContentColumn(0.90));
            columns.Add(new ContentColumn(1.00));
            columns.Add(new ContentColumn(0.90));
            columns.Add(new ContentColumn(1.00));
            columns.Add(new ContentColumn(2.25));
            APPReport.AddTable(columns, true);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("G.1 Available at Procurement Service Stores", 0, 7, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Left, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
            rows.Add(new ContentCell("", 1, 7, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center, 0, 2));
            rows.Add(new ContentCell(String.Format("{0:#,0.00}", appCSEVM.APPDBMItems.Sum(d => d.Q1TotalAmount)), 2, 7, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Right, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
            rows.Add(new ContentCell("", 3, 7, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center, 0, 2));
            rows.Add(new ContentCell(String.Format("{0:#,0.00}", appCSEVM.APPDBMItems.Sum(d => d.Q2TotalAmount)), 4, 7, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Right, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
            rows.Add(new ContentCell("", 5, 7, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center, 0, 2));
            rows.Add(new ContentCell(String.Format("{0:#,0.00}", appCSEVM.APPDBMItems.Sum(d => d.Q3TotalAmount)), 6, 7, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Right, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
            rows.Add(new ContentCell("", 7, 7, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center, 0, 2));
            rows.Add(new ContentCell(String.Format("{0:#,0.00}", appCSEVM.APPDBMItems.Sum(d => d.Q4TotalAmount)), 8, 7, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Right, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
            rows.Add(new ContentCell(String.Format("{0:C}", appCSEVM.APPDBMItems.Sum(d => d.ApprovedBudget)), 9, 7, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Right, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
            APPReport.AddRowContent(rows, 0.30);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("G.2 Other Items not available at PS but regulary purchased from other sources", 0, 7, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Left, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
            rows.Add(new ContentCell(String.Format("{0:#,0.00}", appCSEVM.APPNonDBMItems.Sum(d => d.Q1TotalAmount)), 2, 7, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Right, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
            rows.Add(new ContentCell(String.Format("{0:#,0.00}", appCSEVM.APPNonDBMItems.Sum(d => d.Q2TotalAmount)), 4, 7, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Right, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
            rows.Add(new ContentCell(String.Format("{0:#,0.00}", appCSEVM.APPNonDBMItems.Sum(d => d.Q3TotalAmount)), 6, 7, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Right, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
            rows.Add(new ContentCell(String.Format("{0:#,0.00}", appCSEVM.APPNonDBMItems.Sum(d => d.Q4TotalAmount)), 8, 7, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Right, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
            rows.Add(new ContentCell(String.Format("{0:C}", appCSEVM.APPNonDBMItems.Sum(d => d.ApprovedBudget)), 9, 7, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Right, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
            APPReport.AddRowContent(rows, 0.30);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("TOTAL MONTHLY CASH REQUIREMENTS", 0, 7, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Left, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
            rows.Add(new ContentCell(String.Format("{0:#,0.00}", (appCSEVM.APPDBMItems.Sum(d => d.Q1TotalAmount) + appCSEVM.APPNonDBMItems.Sum(d => d.Q1TotalAmount))), 2, 7, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Right, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
            rows.Add(new ContentCell(String.Format("{0:#,0.00}", (appCSEVM.APPDBMItems.Sum(d => d.Q2TotalAmount) + appCSEVM.APPNonDBMItems.Sum(d => d.Q2TotalAmount))), 4, 7, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Right, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
            rows.Add(new ContentCell(String.Format("{0:#,0.00}", (appCSEVM.APPDBMItems.Sum(d => d.Q3TotalAmount) + appCSEVM.APPNonDBMItems.Sum(d => d.Q3TotalAmount))), 6, 7, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Right, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
            rows.Add(new ContentCell(String.Format("{0:#,0.00}", (appCSEVM.APPDBMItems.Sum(d => d.Q4TotalAmount) + appCSEVM.APPNonDBMItems.Sum(d => d.Q4TotalAmount))), 8, 7, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Right, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
            rows.Add(new ContentCell(String.Format("{0:C}", total), 9, 7, false, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Right, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
            APPReport.AddRowContent(rows, 0.30);

            APPReport.AddSingleColumnHeader();
            APPReport.AddColumnHeader(new HeaderLine("*Agency must put the monthly requirement for air tickets both local and international.", 7, true, true, MigraDoc.DocumentObjectModel.ParagraphAlignment.Left, 25));
            APPReport.AddNewLine();
            APPReport.AddNewLine();
            APPReport.AddSingleColumnHeader();
            APPReport.AddColumnHeader(new HeaderLine("We hereby warrant that the total amount reflected in this Annual Supplies/ Equipment Procurement Plan to procure the listed common-use supplies, materials and equipment has been included in or is within our approved budget for the year.", 7.5, true, true, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center));
            APPReport.AddNewLine();
            APPReport.AddNewLine();
            columns = new List<ContentColumn>();
            columns.Add(new ContentColumn(1.025));
            columns.Add(new ContentColumn(3.15));
            columns.Add(new ContentColumn(0.50));
            columns.Add(new ContentColumn(3.15));
            columns.Add(new ContentColumn(0.50));
            columns.Add(new ContentColumn(3.15));
            columns.Add(new ContentColumn(1.025));
            APPReport.AddTable(columns, false);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("", 0, 7, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Left, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
            rows.Add(new ContentCell("Prepared By:", 1, 7, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Left, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
            rows.Add(new ContentCell("", 2, 7, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Left, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
            rows.Add(new ContentCell("Certified Funds Available / Certified Appropriate Funds Available:", 3, 7, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Left, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
            rows.Add(new ContentCell("", 4, 7, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Left, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
            rows.Add(new ContentCell("Approved by:", 5, 7, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Left, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
            rows.Add(new ContentCell("", 6, 7, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Left, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
            APPReport.AddRowContent(rows, 0);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("", 0, 7, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Left, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
            rows.Add(new ContentCell("", 1, 7, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Left, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center, 0, 0, true));
            rows.Add(new ContentCell("", 2, 7, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Left, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
            rows.Add(new ContentCell("", 3, 7, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Left, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center, 0, 0, true));
            rows.Add(new ContentCell("", 4, 7, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Left, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
            rows.Add(new ContentCell("", 5, 7, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Left, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center, 0, 0, true));
            rows.Add(new ContentCell("", 6, 7, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Left, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
            APPReport.AddRowContent(rows, 0.50);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("", 0, 7, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Left, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
            rows.Add(new ContentCell(appCSEVM.PreparedBy, 1, 7, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
            rows.Add(new ContentCell("", 2, 7, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Left, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
            rows.Add(new ContentCell(appCSEVM.CertifiedBy, 3, 7, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
            rows.Add(new ContentCell("", 4, 7, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Left, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
            rows.Add(new ContentCell(appCSEVM.ApprovedBy, 5, 7, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
            rows.Add(new ContentCell("", 6, 7, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Left, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
            APPReport.AddRowContent(rows, 0);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("", 0, 7, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Left, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
            rows.Add(new ContentCell("Property / Supplies Officer", 1, 7, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
            rows.Add(new ContentCell("", 2, 7, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Left, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
            rows.Add(new ContentCell("Accountant / Local Budget Officer", 3, 7, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
            rows.Add(new ContentCell("", 4, 7, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Left, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
            rows.Add(new ContentCell("Head of Office / Agency", 5, 7, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Center, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
            rows.Add(new ContentCell("", 6, 7, true, false, MigraDoc.DocumentObjectModel.ParagraphAlignment.Left, MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center));
            APPReport.AddRowContent(rows, 0);

            return APPReport.GenerateReport();
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                appCSEDAL.Dispose();
            }
            base.Dispose(disposing);
        }
    }

    public class AnnualProcurementPlanCSEDAL : Controller
    {
        private FMISDbContext db = new FMISDbContext();
        private ABISDataAccess abis = new ABISDataAccess();
        private HRISDataAccess hris = new HRISDataAccess();

        public List<int> GetFiscalYears()
        {
            var fiscalYears = db.PPMPHeader.Where(d => d.PPMPStatus == PPMPStatus.EvaluatedByBudgetOffice).GroupBy(d => d.FiscalYear).Select(d => d.Key).ToList();
            return fiscalYears;
        }
        public List<int> GetPPMPFiscalYears()
        {
            var fiscalYears = db.PPMPHeader.Select(d => d.FiscalYear).Distinct().ToList();
            return fiscalYears;
        }
        public AnnualProcurementPlanCSEVM GetAPPCSE(int FiscalYear)
        {
            AgencyDetails agencyDetails = db.AgencyDetails.First();

            var hope = hris.GetDepartmentDetails(agencyDetails.HOPEReference);
            var property = hris.GetDepartmentDetails(agencyDetails.PropertyOfficeReference);
            var accounting = hris.GetDepartmentDetails(agencyDetails.AccountingOfficeReference);
            var procurement = hris.GetDepartmentDetails(agencyDetails.ProcurementOfficeReference);
            var bac = hris.GetDepartmentDetails(agencyDetails.BACOfficeReference);

            return new AnnualProcurementPlanCSEVM
            {
                FiscalYear = FiscalYear,
                AccountCode = agencyDetails.AccountCode,
                AgencyName = agencyDetails.AgencyName,
                Address = agencyDetails.Address,
                Region = agencyDetails.Region,
                OrganizationType = agencyDetails.OrganizationType,
                ApprovedBy = hope.SectorHead + ", " + hope.SectorHeadDesignation + " / " + hope.Sector,
                PreparedBy = property.DepartmentHead + ", " + property.DepartmentHeadDesignation + " / " + property.Department,
                CertifiedBy = accounting.DepartmentHead + ", " + accounting.DepartmentHeadDesignation + " / " + accounting.Department,
                ProcurementOfficer = procurement.DepartmentHead + ", " + procurement.DepartmentHeadDesignation + " / " + procurement.Department,
                BACSecretariat = bac.SectionCode == null ? bac.DepartmentHead + ", " + bac.DepartmentHeadDesignation + " / " + bac.Department : bac.SectionHead + ", " + bac.SectionHeadDesignation + " / " + bac.Section,
                APPDBMItems = GetAPPDBMItems(FiscalYear),
                APPNonDBMItems = GetAPPNonDBMItems(FiscalYear)
            };
        }
        public AnnualProcurementPlanCSEVM GetAPPCSE(string ReferenceNo)
        {
            AgencyDetails agencyDetails = db.AgencyDetails.First();

            var hope = hris.GetDepartmentDetails(agencyDetails.HOPEReference);
            var property = hris.GetDepartmentDetails(agencyDetails.PropertyOfficeReference);
            var accounting = hris.GetDepartmentDetails(agencyDetails.AccountingOfficeReference);
            var procurement = hris.GetDepartmentDetails(agencyDetails.ProcurementOfficeReference);
            var bac = hris.GetDepartmentDetails(agencyDetails.BACOfficeReference);
            var appCSE = db.APPHeader.Where(d => d.ReferenceNo == ReferenceNo).FirstOrDefault();
            var appDBM = db.APPCSEDetails.Where(d => d.APPHeaderReference == appCSE.ID && d.ProcurementSource == ProcurementSources.AgencyToAgency).ToList()
                         .Select(d => new AnnualProcurementPlanCSEItemsVM
                         {
                             ItemCode = db.ItemArticles.Find(d.ArticleReference).ArticleCode + "-" + d.ItemSequence,
                             ItemFullName = d.ItemFullName,
                             ItemSpecifications = d.ItemFullName,
                             UnitOfMeasure = d.UOMReference,
                             ArticleReference = d.ArticleReference,
                             UACS = d.FKItemArticleReference.UACSObjectClass,
                             CategoryReference = d.CategoryReference,
                             FundSource = d.FundSource,
                             ItemSequence = d.ItemSequence,
                             ProcurementSource = d.ProcurementSource,
                             JanQty = d.JanQty,
                             FebQty = d.FebQty,
                             MarQty = d.MarQty,
                             Q1TotalQty = d.Q1TotalQty,
                             Q1TotalAmount = Math.Round((d.Q1TotalQty * d.UnitCost), 2),
                             AprQty = d.AprQty,
                             MayQty = d.MayQty,
                             JunQty = d.JunQty,
                             Q2TotalQty = d.Q2TotalQty,
                             Q2TotalAmount = Math.Round((d.Q2TotalQty * d.UnitCost), 2),
                             JulQty = d.JulQty,
                             AugQty = d.AugQty,
                             SepQty = d.SepQty,
                             Q3TotalQty = d.Q3TotalQty,
                             Q3TotalAmount = Math.Round((d.Q3TotalQty * d.UnitCost), 2),
                             OctQty = d.OctQty,
                             NovQty = d.NovQty,
                             DecQty = d.DecQty,
                             Q4TotalQty = d.Q1TotalQty,
                             Q4TotalAmount = Math.Round((d.Q4TotalQty * d.UnitCost), 2),
                             TotalQty = d.TotalQty,
                             ApprovedBudget = d.ApprovedBudget,
                             UnitCost = d.UnitCost
                         }).ToList();
            var appNonDB = db.APPCSEDetails.Where(d => d.APPHeaderReference == appCSE.ID && d.ProcurementSource == ProcurementSources.ExternalSuppliers).ToList()
                         .Select(d => new AnnualProcurementPlanCSEItemsVM
                         {
                             ItemCode = db.ItemArticles.Find(d.ArticleReference).ArticleCode + "-" + d.ItemSequence,
                             ItemFullName = d.ItemFullName,
                             ItemSpecifications = d.ItemFullName,
                             UnitOfMeasure = d.UOMReference,
                             ArticleReference = d.ArticleReference,
                             UACS = d.FKItemArticleReference.UACSObjectClass,
                             CategoryReference = d.CategoryReference,
                             FundSource = d.FundSource,
                             ItemSequence = d.ItemSequence,
                             ProcurementSource = d.ProcurementSource,
                             JanQty = d.JanQty,
                             FebQty = d.FebQty,
                             MarQty = d.MarQty,
                             Q1TotalQty = d.Q1TotalQty,
                             Q1TotalAmount = Math.Round((d.Q1TotalQty * d.UnitCost), 2),
                             AprQty = d.AprQty,
                             MayQty = d.MayQty,
                             JunQty = d.JunQty,
                             Q2TotalQty = d.Q2TotalQty,
                             Q2TotalAmount = Math.Round((d.Q2TotalQty * d.UnitCost), 2),
                             JulQty = d.JulQty,
                             AugQty = d.AugQty,
                             SepQty = d.SepQty,
                             Q3TotalQty = d.Q3TotalQty,
                             Q3TotalAmount = Math.Round((d.Q3TotalQty * d.UnitCost), 2),
                             OctQty = d.OctQty,
                             NovQty = d.NovQty,
                             DecQty = d.DecQty,
                             Q4TotalQty = d.Q1TotalQty,
                             Q4TotalAmount = Math.Round((d.Q4TotalQty * d.UnitCost), 2),
                             TotalQty = d.TotalQty,
                             ApprovedBudget = d.ApprovedBudget,
                             UnitCost = d.UnitCost
                         }).ToList();

            return new AnnualProcurementPlanCSEVM
            {
                FiscalYear = appCSE.FiscalYear,
                ReferenceNo = appCSE.ReferenceNo,
                AccountCode = agencyDetails.AccountCode,
                AgencyName = agencyDetails.AgencyName,
                Address = agencyDetails.Address,
                Region = agencyDetails.Region,
                OrganizationType = agencyDetails.OrganizationType,
                PreparedBy = property.DepartmentHead,
                PreparedByDepartment = property.DepartmentCode,
                PreparedByDesignation = property.DepartmentHeadDesignation,
                ApprovedBy = hope.SectorHead,
                ApprovedByDepartment = hope.Sector,
                ApprovedByDesignation = hope.SectorHeadDesignation,
                CertifiedBy = accounting.DepartmentHead,
                CertifiedByDepartment = accounting.DepartmentCode,
                CertifiedByDesignation = accounting.DepartmentHeadDesignation,
                APPDBMItems = appDBM,
                APPNonDBMItems = appNonDB
            };
        }
        public List<AnnualProcurementPlanCSEItemsVM> GetAPPDBMItems(int FiscalYear)
        {
            return db.PPMPDetails.ToList()
                                 .Where(d => d.FKPPMPHeaderReference.FiscalYear == FiscalYear &&
                                             d.ProcurementSource == ProcurementSources.AgencyToAgency &&
                                             d.FKItemArticleReference.FKItemTypeReference.FKItemClassificationReference.ID == 1 &&
                                             d.PPMPDetailStatus == PPMPDetailStatus.ItemAccepted)
                                 .GroupBy(d => new { d.ArticleReference, d.ItemSequence, d.UACS, d.CategoryReference, d.ItemFullName, d.ItemSpecifications, d.UnitCost, d.UOMReference, d.ProcurementSource })
                                 .Select(d => new AnnualProcurementPlanCSEItemsVM
                                 {
                                     ItemCode = db.ItemArticles.Find(d.Key.ArticleReference).ArticleCode + "-" + d.Key.ItemSequence,
                                     ArticleReference = (int)d.Key.ArticleReference,
                                     ItemSequence = d.Key.ItemSequence,
                                     ItemFullName = d.Key.ItemFullName,
                                     ProcurementSource = d.Key.ProcurementSource,
                                     ItemSpecifications = d.Key.ItemSpecifications,
                                     UnitOfMeasure = d.Key.UOMReference,
                                     UOM = db.UOM.Find(d.Key.UOMReference).Abbreviation,
                                     CategoryReference = d.Key.CategoryReference,
                                     UnitCost = d.Key.UnitCost,
                                     UACS = d.Key.UACS,
                                     JanQty = d.Sum(x => x.JanQty),
                                     FebQty = d.Sum(x => x.FebQty),
                                     MarQty = d.Sum(x => x.MarQty),
                                     Q1TotalQty = d.Sum(x => x.Q1TotalQty),
                                     Q1TotalAmount = Math.Round((d.Sum(x => x.Q1TotalQty) * d.Key.UnitCost), 2),
                                     AprQty = d.Sum(x => x.AprQty),
                                     MayQty = d.Sum(x => x.MayQty),
                                     JunQty = d.Sum(x => x.JunQty),
                                     Q2TotalQty = d.Sum(x => x.Q2TotalQty),
                                     Q2TotalAmount = Math.Round((d.Sum(x => x.Q2TotalQty) * d.Key.UnitCost), 2),
                                     JulQty = d.Sum(x => x.JulQty),
                                     AugQty = d.Sum(x => x.AugQty),
                                     SepQty = d.Sum(x => x.SepQty),
                                     Q3TotalQty = d.Sum(x => x.Q3TotalQty),
                                     Q3TotalAmount = Math.Round((d.Sum(x => x.Q3TotalQty) * d.Key.UnitCost), 2),
                                     OctQty = d.Sum(x => x.OctQty),
                                     NovQty = d.Sum(x => x.NovQty),
                                     DecQty = d.Sum(x => x.DecQty),
                                     Q4TotalQty = d.Sum(x => x.Q3TotalQty),
                                     Q4TotalAmount = Math.Round((d.Sum(x => x.Q4TotalQty) * d.Key.UnitCost), 2),
                                     TotalQty = d.Sum(x => x.TotalQty),
                                     ApprovedBudget = Math.Round((d.Sum(x => x.TotalQty) * d.Key.UnitCost), 2)
                                 }).ToList();
        }
        public List<AnnualProcurementPlanCSEItemsVM> GetAPPNonDBMItems(int FiscalYear)
        {
            return db.PPMPDetails.ToList()
                                 .Where(d => d.FKPPMPHeaderReference.FiscalYear == FiscalYear &&
                                             d.ProcurementSource == ProcurementSources.ExternalSuppliers &&
                                             d.ArticleReference != null &&
                                             d.FKItemArticleReference.FKItemTypeReference.FKItemClassificationReference.ID == 1 &&
                                             d.PPMPDetailStatus == PPMPDetailStatus.ItemAccepted)
                                 .GroupBy(d => new { d.ArticleReference, d.ItemSequence, d.UACS, d.CategoryReference, d.ItemFullName, d.ItemSpecifications, d.UnitCost, d.UOMReference, d.ProcurementSource })
                                 .Select(d => new AnnualProcurementPlanCSEItemsVM
                                 {
                                     ItemCode = db.ItemArticles.Find(d.Key.ArticleReference).ArticleCode + "-" + d.Key.ItemSequence,
                                     ArticleReference = (int)d.Key.ArticleReference,
                                     ItemSequence = d.Key.ItemSequence,
                                     ItemFullName = d.Key.ItemFullName,
                                     ProcurementSource = d.Key.ProcurementSource,
                                     ItemSpecifications = d.Key.ItemSpecifications,
                                     UnitOfMeasure = d.Key.UOMReference,
                                     UOM = db.UOM.Find(d.Key.UOMReference).Abbreviation,
                                     CategoryReference = d.Key.CategoryReference,
                                     UnitCost = d.Key.UnitCost,
                                     UACS = d.Key.UACS,
                                     JanQty = d.Sum(x => x.JanQty),
                                     FebQty = d.Sum(x => x.FebQty),
                                     MarQty = d.Sum(x => x.MarQty),
                                     Q1TotalQty = d.Sum(x => x.Q1TotalQty),
                                     Q1TotalAmount = Math.Round((d.Sum(x => x.Q1TotalQty) * d.Key.UnitCost), 2),
                                     AprQty = d.Sum(x => x.AprQty),
                                     MayQty = d.Sum(x => x.MayQty),
                                     JunQty = d.Sum(x => x.JunQty),
                                     Q2TotalQty = d.Sum(x => x.Q2TotalQty),
                                     Q2TotalAmount = Math.Round((d.Sum(x => x.Q2TotalQty) * d.Key.UnitCost), 2),
                                     JulQty = d.Sum(x => x.JulQty),
                                     AugQty = d.Sum(x => x.AugQty),
                                     SepQty = d.Sum(x => x.SepQty),
                                     Q3TotalQty = d.Sum(x => x.Q3TotalQty),
                                     Q3TotalAmount = Math.Round((d.Sum(x => x.Q3TotalQty) * d.Key.UnitCost), 2),
                                     OctQty = d.Sum(x => x.OctQty),
                                     NovQty = d.Sum(x => x.NovQty),
                                     DecQty = d.Sum(x => x.DecQty),
                                     Q4TotalQty = d.Sum(x => x.Q3TotalQty),
                                     Q4TotalAmount = Math.Round((d.Sum(x => x.Q4TotalQty) * d.Key.UnitCost), 2),
                                     TotalQty = d.Sum(x => x.TotalQty),
                                     ApprovedBudget = Math.Round((d.Sum(x => x.TotalQty) * d.Key.UnitCost), 2)
                                 }).ToList();
        }
        public bool PostAPPCSE(AnnualProcurementPlanCSEVM APPCSEViewModel, string UserEmail)
        {
            var user = db.UserAccounts.Where(d => d.Email == UserEmail).FirstOrDefault();

            AgencyDetails agencyDetails = db.AgencyDetails.First();

            var hope = hris.GetDepartmentDetails(agencyDetails.HOPEReference);
            var property = hris.GetDepartmentDetails(agencyDetails.PropertyOfficeReference);
            var accounting = hris.GetDepartmentDetails(agencyDetails.AccountingOfficeReference);
            var procurement = hris.GetDepartmentDetails(agencyDetails.ProcurementOfficeReference);
            var bac = hris.GetDepartmentDetails(agencyDetails.BACOfficeReference);

            AnnualProcurementPlan appHeader = new AnnualProcurementPlan()
            {
                FiscalYear = APPCSEViewModel.FiscalYear,
                APPType = APPTypes.APPCSE,
                ReferenceNo = GenerateAPPCSEReferenceNo(APPCSEViewModel.FiscalYear),
                PreparedBy = property.DepartmentHead,
                PreparedByDepartmentCode = property.DepartmentCode,
                PreparedByDesignation = property.DepartmentHeadDesignation,
                RecommendingApproval = accounting.DepartmentHead,
                RecommendingApprovalDesignation = accounting.DepartmentHeadDesignation,
                RecommendingApprovalDepartmentCode = accounting.DepartmentCode,
                ApprovedBy = hope.SectorHead,
                ApprovedByDesignation = hope.SectorHeadDesignation,
                ApprovedByDepartmentCode = hope.SectorCode,
                CreatedBy = user.EmpCode,
                CreatedAt = DateTime.Now
            };

            db.APPHeader.Add(appHeader);
            if (db.SaveChanges() == 0)
            {
                return false;
            }

            db.APPCSEDetails.AddRange(APPCSEViewModel.APPDBMItems
                                               .Union(APPCSEViewModel.APPNonDBMItems)
                                               .ToList()
                                               .Select(d => new APPCSEDetails
                                               {
                                                   APPHeaderReference = appHeader.ID,
                                                   ArticleReference = d.ArticleReference,
                                                   ItemFullName = d.ItemFullName,
                                                   ItemSpecifications = d.ItemSpecifications,
                                                   CategoryReference = d.CategoryReference,
                                                   ItemSequence = d.ItemSequence,
                                                   ProcurementSource = d.ProcurementSource,
                                                   UOMReference = d.UnitOfMeasure,
                                                   UACS = d.UACS,
                                                   FundSource = d.FundSource,
                                                   JanQty = d.JanQty,
                                                   FebQty = d.FebQty,
                                                   MarQty = d.MarQty,
                                                   Q1TotalQty = d.Q1TotalQty,
                                                   AprQty = d.AprQty,
                                                   MayQty = d.MayQty,
                                                   JunQty = d.JunQty,
                                                   Q2TotalQty = d.Q2TotalQty,
                                                   JulQty = d.JulQty,
                                                   AugQty = d.AugQty,
                                                   SepQty = d.SepQty,
                                                   Q3TotalQty = d.Q3TotalQty,
                                                   OctQty = d.OctQty,
                                                   NovQty = d.NovQty,
                                                   DecQty = d.DecQty,
                                                   Q4TotalQty = d.Q4TotalQty,
                                                   TotalQty = d.TotalQty,
                                                   UnitCost = d.UnitCost,
                                                   ApprovedBudget = d.ApprovedBudget
                                               }).ToList());

            if (db.SaveChanges() == 0)
            {
                return false;
            }

            //if (APPCSEViewModel.APPDBMItems != null)
            //{
            //    foreach (var appItem in APPCSEViewModel.APPDBMItems)
            //    {
            //        var ppmpItems = db.ProjectPlanItems.Where(d => d.FKPPMPReference.FiscalYear == APPCSEViewModel.FiscalYear && d.FKItemReference.ID == appItem.ItemID && d.Status == "Approved").ToList();
            //        ppmpItems.ForEach(d => { d.APPReference = appHeader.ID; d.Status = "Posted to APP"; });
            //        db.SaveChanges();
            //        var ppmpReferences = ppmpItems.Select(d => d.PPMPReference).GroupBy(d => d).Select(d => d.Key).ToList();
            //        var ppmps = db.PPMPHeader.Where(d => ppmpReferences.Contains(d.ID)).ToList();
            //        ppmps.ForEach(d => { d.Status = "Posted to APP"; });
            //        db.SaveChanges();
            //    }
            //}

            //if (APPCSEViewModel.APPNonDBMItems != null)
            //{
            //    foreach (var appItem in APPCSEViewModel.APPNonDBMItems)
            //    {
            //        var ppmpItems = db.ProjectPlanItems.Where(d => d.FKPPMPReference.FiscalYear == APPCSEViewModel.FiscalYear && d.FKItemReference.ID == appItem.ItemID && d.Status == "Approved").ToList();
            //        ppmpItems.ForEach(d => { d.APPReference = appHeader.ID; d.Status = "Posted to APP"; });
            //        db.SaveChanges();
            //        var ppmpReferences = ppmpItems.Select(d => d.PPMPReference).GroupBy(d => d).Select(d => d.Key).ToList();
            //        var ppmps = db.PPMPHeader.Where(d => ppmpReferences.Contains(d.ID)).ToList();
            //        ppmps.ForEach(d => { d.Status = "Posted to APP"; });
            //        db.SaveChanges();
            //    }
            //}

            return true;
        }
        private string GenerateAPPCSEReferenceNo(int FiscalYear)
        {
            string referenceNo = String.Empty;
            var sequenceNo = (db.APPHeader.Where(d => d.ReferenceNo.Contains("ANPP-CSE") && d.FiscalYear == FiscalYear).Count() + 1).ToString();
            sequenceNo = (sequenceNo.Length == 1) ? "00" + sequenceNo : (sequenceNo.Length == 2) ? "0" + sequenceNo : sequenceNo;
            referenceNo = "ANPP-" + APPTypes.APPCSE.GetAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>().ShortName + "-" + sequenceNo + "-" + FiscalYear;
            return referenceNo;
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
                hris.Dispose();
                abis.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}