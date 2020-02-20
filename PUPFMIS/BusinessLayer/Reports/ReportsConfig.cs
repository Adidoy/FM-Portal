using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Shapes;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using System;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace PUPFMIS.BusinessLayer
{
    public class ReportsConfig
    {

        public string ReportTitle { get; set; }
        public string ReportFormTitle { get; set; }
        public string ReportReferenceNo { get; set; }
        public string LogoPath { get; set; }


        private Document report;
        private PageSetup pageSetup;
        private Table headingTable;
        private Column headingColumn;
        private Row headingRow;
        private Table contentTable;
        private Column contentColumn;
        private Row contentRow;
        private Section section;

        public void DocumentSetupLandscape()
        {
            report = new Document();
            report.Info.Title = ReportTitle;
            section = new Section();
            section = report.AddSection();

            pageSetup = new PageSetup();
            pageSetup.TopMargin = new Unit(0.50, UnitType.Inch);
            pageSetup.BottomMargin = new Unit(0.50, UnitType.Inch);
            pageSetup.LeftMargin = new Unit(0.50, UnitType.Inch);
            pageSetup.RightMargin = new Unit(0.50, UnitType.Inch);
            pageSetup.Orientation = Orientation.Landscape;
            pageSetup.PageHeight = new Unit(13.00, UnitType.Inch);
            pageSetup.PageWidth = new Unit(8.50, UnitType.Inch);

            HeadingSetupLandscape();
        }

        private void HeadingSetupLandscape()
        {
            Image logo = new Image(LogoPath);
            logo.Height = "0.5cm";

            section.PageSetup = pageSetup;

            var table = section.AddTable();
            table.Style = "Table";

            var column = table.AddColumn("2cm");
            column.Format.Alignment = ParagraphAlignment.Center;
            column = table.AddColumn("19cm");
            column.Format.Alignment = ParagraphAlignment.Left;
            column = table.AddColumn("9.48cm");
            column.Format.Alignment = ParagraphAlignment.Left;

            var row = table.AddRow();
            row.Cells[0].AddParagraph().AddImage(logo.Name).Height = "1.75cm";
            row.Cells[0].MergeDown = 4;
            row.Cells[0].Format.Alignment = ParagraphAlignment.Right;
            row.Cells[0].VerticalAlignment = VerticalAlignment.Center;
            row.Cells[1].MergeRight = 1;

            row = table.AddRow();
            row.Cells[1].AddParagraph("Republic of the Philippines");
            row.Cells[1].Format.Font.Bold = false;
            row.Cells[1].Format.Alignment = ParagraphAlignment.Left;
            row.Cells[1].Format.Font.Size = 10;
            if(!String.IsNullOrEmpty(ReportFormTitle))
            {
                row.Cells[2].AddParagraph("Form D");
                row.Cells[2].Format.Font.Size = 10;
            }

            row = table.AddRow();
            row.Cells[1].AddParagraph("POLYTECHNIC UNIVERSITY OF THE PHILIPPINES");
            row.Cells[1].Format.Font.Size = 12;
            row.Cells[1].Format.Font.Bold = true;
            if(!String.IsNullOrEmpty(ReportReferenceNo))
            {
                row.Cells[2].AddParagraph("Reference No.: " + ReportReferenceNo);
                row.Cells[2].Format.Font.Size = 12;
                row.Cells[2].Format.Font.Bold = true;
            }

            row = table.AddRow();
            row.Cells[1].AddParagraph("Sta. Mesa, Manila");
            row.Cells[1].Format.Font.Size = 10;
            row.Cells[2].AddParagraph("Date Printed: " + DateTime.Now.ToString("dd MMMM yyyy hh:mm tt"));
            row.Cells[2].Format.Font.Size = 10;

            row = table.AddRow();
            row.Cells[1].MergeRight = 1;

            headingTable = section.AddTable();
            headingTable.Style = "Table";
            headingColumn = headingTable.AddColumn("30.48cm");
            headingColumn.Format.Alignment = ParagraphAlignment.Center;
            headingRow = headingTable.AddRow();
            headingRow.Cells[0].AddParagraph("\n");
        }

        public void AddHeader(string HeaderText)
        {
            headingRow = headingTable.AddRow();
            headingRow.Cells[0].AddParagraph(HeaderText);
        }

        public void AddHeader(string HeaderText, Unit FontSize)
        {
            headingRow = headingTable.AddRow();
            headingRow.Cells[0].AddParagraph(HeaderText);
            headingRow.Cells[0].Format.Font.Size = FontSize;
        }

        public void AddHeader(string HeaderText, Unit FontSize, bool BoldFace)
        {
            headingRow = headingTable.AddRow();
            headingRow.Cells[0].AddParagraph(HeaderText);
            headingRow.Cells[0].Format.Font.Size = FontSize;
            headingRow.Cells[0].Format.Font.Bold = BoldFace;
        }

        public Table AddTable()
        {
            contentTable = new Table();
            contentTable = section.AddTable();
            contentTable.Style = "Table";
            return contentTable;
        }

        public Table AddTable(bool? Bordered)
        {
            contentTable = new Table();
            contentTable = section.AddTable();
            contentTable.Style = "Table";
            if ((bool)Bordered)
            {
                contentTable.Borders.Color = new Color(0, 0, 0);
            }
            return contentTable;
        }

        public void AddContentColumn(Unit Size)
        {
            contentColumn = contentTable.AddColumn(Size);
        }

        public void AddContentColumn(Unit Size, ParagraphAlignment HorizontalAlignment)
        {
            contentColumn = contentTable.AddColumn(Size);
            contentColumn.Format.Alignment = HorizontalAlignment;
        }

        public void AddContentColumn(Unit Size, ParagraphAlignment HorizontalAlignment, Color BackColor)
        {
            contentColumn = contentTable.AddColumn(Size);
            contentColumn.Format.Alignment = HorizontalAlignment;
            contentColumn.Shading.Color = BackColor;
        }

        public void AddContentRow()
        {
            contentRow = new Row();
            contentRow = contentTable.AddRow();
        }

        public void AddContentRow(Unit RowHeight)
        {
            contentRow = new Row();
            contentRow = contentTable.AddRow();
            contentRow.Height = RowHeight;
        }

        public void AddContent(string Value, int CellIndex)
        {
            contentRow.Cells[CellIndex].AddParagraph(Value);
        }

        public void AddContent(string Value, int CellIndex, Unit FontSize)
        {
            contentRow.Cells[CellIndex].AddParagraph(Value);
            contentRow.Cells[CellIndex].Format.Font.Size = FontSize;
        }

        public void AddContent(string Value, int CellIndex, Unit FontSize, bool BoldFace)
        {
            contentRow.Cells[CellIndex].AddParagraph(Value);
            contentRow.Cells[CellIndex].Format.Font.Size = FontSize;
            contentRow.Cells[CellIndex].Format.Font.Bold = BoldFace;
        }

        public void AddContent(string Value, int CellIndex, Unit FontSize, bool BoldFace, ParagraphAlignment HorizontalAlign)
        {
            contentRow.Cells[CellIndex].AddParagraph(Value);
            contentRow.Cells[CellIndex].Format.Font.Size = FontSize;
            contentRow.Cells[CellIndex].Format.Font.Bold = BoldFace;
            contentRow.Cells[CellIndex].Format.Alignment = HorizontalAlign;
        }

        public void AddContent(string Value, int CellIndex, Unit FontSize, bool BoldFace, ParagraphAlignment HorizontalAlign, bool BottomRule)
        {
            contentRow.Cells[CellIndex].AddParagraph(Value);
            contentRow.Cells[CellIndex].Format.Font.Size = FontSize;
            contentRow.Cells[CellIndex].Format.Font.Bold = BoldFace;
            contentRow.Cells[CellIndex].Format.Alignment = HorizontalAlign;
            if(BottomRule)
            {
                contentRow.Cells[CellIndex].Borders.Bottom.Color = new Color(0, 0, 0);
            }
        }

        public void AddContent(string Value, int CellIndex, Unit FontSize, bool BoldFace, ParagraphAlignment HorizontalAlign, int MergeRightSize)
        {
            contentRow.Cells[CellIndex].AddParagraph(Value);
            contentRow.Cells[CellIndex].Format.Font.Size = FontSize;
            contentRow.Cells[CellIndex].Format.Font.Bold = BoldFace;
            contentRow.Cells[CellIndex].Format.Alignment = HorizontalAlign;
            contentRow.Cells[CellIndex].MergeRight = MergeRightSize;
        }

        public void AddContent(string Value, int CellIndex, Unit FontSize, bool BoldFace, ParagraphAlignment HorizontalAlign, int? MergeRightSize, int MergeDownSize)
        {
            contentRow.Cells[CellIndex].AddParagraph(Value);
            contentRow.Cells[CellIndex].Format.Font.Size = FontSize;
            contentRow.Cells[CellIndex].Format.Font.Bold = BoldFace;
            contentRow.Cells[CellIndex].Format.Alignment = HorizontalAlign;
            if (!String.IsNullOrEmpty(MergeRightSize.ToString()))
            {
                contentRow.Cells[CellIndex].MergeRight = (int)MergeRightSize;
            }
            contentRow.Cells[CellIndex].MergeDown = MergeDownSize;
        }

        public void AddContent(string Value, int CellIndex, Unit FontSize, bool BoldFace, ParagraphAlignment HorizontalAlign, VerticalAlignment VerticalAlign)
        {
            contentRow.Cells[CellIndex].AddParagraph(Value);
            contentRow.Cells[CellIndex].Format.Font.Size = FontSize;
            contentRow.Cells[CellIndex].Format.Font.Bold = BoldFace;
            contentRow.Cells[CellIndex].Format.Alignment = HorizontalAlign;
            contentRow.Cells[CellIndex].VerticalAlignment = VerticalAlign;
        }

        public void AddContent(string Value, int CellIndex, Unit FontSize, bool BoldFace, ParagraphAlignment HorizontalAlign, VerticalAlignment VerticalAlign, int MergeRightSize)
        {
            contentRow.Cells[CellIndex].AddParagraph(Value);
            contentRow.Cells[CellIndex].Format.Font.Size = FontSize;
            contentRow.Cells[CellIndex].Format.Font.Bold = BoldFace;
            contentRow.Cells[CellIndex].Format.Alignment = HorizontalAlign;
            contentRow.Cells[CellIndex].VerticalAlignment = VerticalAlign;
            contentRow.Cells[CellIndex].MergeRight = MergeRightSize;
        }

        public void AddContent(string Value, int CellIndex, Unit FontSize, bool BoldFace, ParagraphAlignment HorizontalAlign, VerticalAlignment VerticalAlign, int? MergeRightSize, int MergeDownSize)
        {
            contentRow.Cells[CellIndex].AddParagraph(Value);
            contentRow.Cells[CellIndex].Format.Font.Size = FontSize;
            contentRow.Cells[CellIndex].Format.Font.Bold = BoldFace;
            contentRow.Cells[CellIndex].Format.Alignment = HorizontalAlign;
            contentRow.Cells[CellIndex].VerticalAlignment = VerticalAlign;
            if (!String.IsNullOrEmpty(MergeRightSize.ToString()))
            {
                contentRow.Cells[CellIndex].MergeRight = (int)MergeRightSize;
            }
            contentRow.Cells[CellIndex].MergeDown = MergeDownSize;
        }

        public MemoryStream GenerateReport()
        {
            report.UseCmykColor = true;
            string filename = ReportReferenceNo;
            PdfDocumentRenderer pdfRenderer = new PdfDocumentRenderer();
            pdfRenderer.Document = report;
            pdfRenderer.RenderDocument();

            MemoryStream stream = new MemoryStream();
            pdfRenderer.PdfDocument.Save(stream, false);
            return stream;

        }
    }
}