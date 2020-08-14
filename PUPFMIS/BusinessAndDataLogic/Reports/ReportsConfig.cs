using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Shapes;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using System;
using System.IO;
using System.Linq;
using System.Text;

namespace PUPFMIS.BusinessAndDataLogic
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
            pageSetup.TopMargin = new Unit(1.27, UnitType.Centimeter);
            pageSetup.BottomMargin = new Unit(1.27, UnitType.Centimeter);
            pageSetup.LeftMargin = new Unit(0.635, UnitType.Centimeter);
            pageSetup.RightMargin = new Unit(0.635, UnitType.Centimeter);
            pageSetup.Orientation = Orientation.Landscape;
            pageSetup.PageHeight = new Unit(33.02, UnitType.Centimeter);
            pageSetup.PageWidth = new Unit(21.59, UnitType.Centimeter);

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
            column = table.AddColumn("19.04cm");
            column.Format.Alignment = ParagraphAlignment.Left;
            column = table.AddColumn("11.48cm");
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
            row.Cells[1].Format.Font.Size = 8;
            if(!String.IsNullOrEmpty(ReportFormTitle))
            {
                row.Cells[2].AddParagraph(ReportFormTitle);
                row.Cells[2].Format.Font.Size = 8;
            }

            row = table.AddRow();
            row.Cells[1].AddParagraph("POLYTECHNIC UNIVERSITY OF THE PHILIPPINES");
            row.Cells[1].Format.Font.Size = 11;
            row.Cells[1].Format.Font.Bold = true;
            if(!String.IsNullOrEmpty(ReportReferenceNo))
            {
                row.Cells[2].AddParagraph("Reference No.: " + ReportReferenceNo);
                row.Cells[2].Format.Font.Size = 10;
                row.Cells[2].Format.Font.Bold = true;
            }

            row = table.AddRow();
            row.Cells[1].AddParagraph("Sta. Mesa, Manila");
            row.Cells[1].Format.Font.Size = 7;
            row.Cells[2].AddParagraph("Date Printed: " + DateTime.Now.ToString("dd MMMM yyyy hh:mm tt"));
            row.Cells[2].Format.Font.Size = 8;

            row = table.AddRow();
            row.Cells[1].MergeRight = 1;

            headingTable = section.AddTable();
            headingTable.Style = "Table";
            headingColumn = headingTable.AddColumn("31.75cm");
            headingColumn.Format.Alignment = ParagraphAlignment.Center;
            headingRow = headingTable.AddRow();
            headingRow.Cells[0].AddParagraph("\n");
        }
        public void NewPage()
        {
            section = new Section();
            section = report.AddSection();

            pageSetup = new PageSetup();
            pageSetup.TopMargin = new Unit(1.27, UnitType.Centimeter);
            pageSetup.BottomMargin = new Unit(1.27, UnitType.Centimeter);
            pageSetup.LeftMargin = new Unit(0.635, UnitType.Centimeter);
            pageSetup.RightMargin = new Unit(0.635, UnitType.Centimeter);
            pageSetup.Orientation = Orientation.Landscape;
            pageSetup.PageHeight = new Unit(33.02, UnitType.Centimeter);
            pageSetup.PageWidth = new Unit(21.59, UnitType.Centimeter);

            HeadingSetupLandscape();
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
        public void AddContentRow(bool TopBorder)
        {
            contentRow = new Row();
            contentRow = contentTable.AddRow();
            if(TopBorder == false)
            {
                var border = new Border();
                border.Color = new Color(0, 0, 0, 0);
                contentRow.Borders.Top = border;
            }
        }
        public void AddContent(string Value, int CellIndex)
        {
            contentRow.Cells[CellIndex].AddParagraph(Value);
            contentRow.Cells[CellIndex].Format.KeepTogether = true;
        }
        public void AddContent(string Value, int CellIndex, Unit FontSize)
        {
            contentRow.Cells[CellIndex].AddParagraph(Value);
            contentRow.Cells[CellIndex].Format.KeepTogether = true;
            contentRow.Cells[CellIndex].Format.Font.Size = FontSize;
        }
        public void AddContent(string Value, int CellIndex, Unit FontSize, bool BoldFace)
        {
            contentRow.Cells[CellIndex].AddParagraph(Value);
            contentRow.Cells[CellIndex].Format.KeepTogether = true;
            contentRow.Cells[CellIndex].Format.Font.Size = FontSize;
            contentRow.Cells[CellIndex].Format.Font.Bold = BoldFace;
        }
        public void AddContent(string Value, int CellIndex, Unit FontSize, bool BoldFace, ParagraphAlignment HorizontalAlign)
        {
            contentRow.Cells[CellIndex].AddParagraph(Value);
            contentRow.Cells[CellIndex].Format.KeepTogether = true;
            contentRow.Cells[CellIndex].Format.Font.Size = FontSize;
            contentRow.Cells[CellIndex].Format.Font.Bold = BoldFace;
            contentRow.Cells[CellIndex].Format.Alignment = HorizontalAlign;
        }
        public void AddContent(string Value, int CellIndex, Unit FontSize, bool BoldFace, ParagraphAlignment HorizontalAlign, bool BottomRule)
        {
            contentRow.Cells[CellIndex].AddParagraph(Value);
            contentRow.Cells[CellIndex].Format.KeepTogether = true;
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
            contentRow.Cells[CellIndex].Format.KeepTogether = true;
            contentRow.Cells[CellIndex].Format.Font.Size = FontSize;
            contentRow.Cells[CellIndex].Format.Font.Bold = BoldFace;
            contentRow.Cells[CellIndex].Format.Alignment = HorizontalAlign;
            contentRow.Cells[CellIndex].MergeRight = MergeRightSize;
        }
        public void AddContent(string Value, int CellIndex, Unit FontSize, bool BoldFace, ParagraphAlignment HorizontalAlign, int? MergeRightSize, int MergeDownSize)
        {
            contentRow.Cells[CellIndex].AddParagraph(Value);
            contentRow.Cells[CellIndex].Format.KeepTogether = true;
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
            contentRow.Cells[CellIndex].AddParagraph(AdjustIfTooWideToFitIn(contentRow.Cells[CellIndex], Value));
            contentRow.Cells[CellIndex].Format.Font.Size = FontSize;
            contentRow.Cells[CellIndex].Format.Font.Bold = BoldFace;
            contentRow.Cells[CellIndex].Format.Alignment = HorizontalAlign;
            contentRow.Cells[CellIndex].VerticalAlignment = VerticalAlign;
        }
        public void AddContent(string Value, int CellIndex, Unit FontSize, bool BoldFace, ParagraphAlignment HorizontalAlign, VerticalAlignment VerticalAlign, int MergeRightSize)
        {
            contentRow.Cells[CellIndex].AddParagraph(Value);
            contentRow.Cells[CellIndex].Format.KeepTogether = true;
            contentRow.Cells[CellIndex].Format.Font.Size = FontSize;
            contentRow.Cells[CellIndex].Format.Font.Bold = BoldFace;
            contentRow.Cells[CellIndex].Format.Alignment = HorizontalAlign;
            contentRow.Cells[CellIndex].VerticalAlignment = VerticalAlign;
            contentRow.Cells[CellIndex].MergeRight = MergeRightSize;
        }
        public void AddContent(string Value, int CellIndex, Unit FontSize, bool BoldFace, ParagraphAlignment HorizontalAlign, VerticalAlignment VerticalAlign, int? MergeRightSize, int MergeDownSize)
        {
            contentRow.Cells[CellIndex].AddParagraph(Value);
            contentRow.Cells[CellIndex].Format.KeepTogether = true;
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
        public void AddContent(string Value, int CellIndex, Unit FontSize, bool BoldFace, ParagraphAlignment HorizontalAlign, VerticalAlignment VerticalAlign, int? MergeRightSize, int? MergeDownSize, bool BottomLine)
        {
            contentRow.Cells[CellIndex].AddParagraph(Value);
            contentRow.Cells[CellIndex].Format.KeepTogether = true;
            contentRow.Cells[CellIndex].Format.Font.Size = FontSize;
            contentRow.Cells[CellIndex].Format.Font.Bold = BoldFace;
            contentRow.Cells[CellIndex].Format.Alignment = HorizontalAlign;
            contentRow.Cells[CellIndex].VerticalAlignment = VerticalAlign;
            if (!String.IsNullOrEmpty(MergeRightSize.ToString()))
            {
                contentRow.Cells[CellIndex].MergeRight = (int)MergeRightSize;
            }
            if (!String.IsNullOrEmpty(MergeDownSize.ToString()))
            {
                contentRow.Cells[CellIndex].MergeDown = (int)MergeDownSize;
            }
            if(BottomLine == false)
            {
                contentRow.Borders.Bottom.Color = new Color(0,0,0,0);
                contentRow.Borders.Top.Color = new Color(0, 0, 0, 0);
            }
        }
        private static string AdjustIfTooWideToFitIn(Cell cell, string text)
        {
            Column column = cell.Column;
            Unit availableWidth = column.Width - column.Table.Borders.Width - cell.Borders.Width;

            var tooWideWords = text.Split(" ".ToCharArray()).Distinct().Where(s => TooWide(s, availableWidth));

            var adjusted = new StringBuilder(text);
            foreach (string word in tooWideWords)
            {
                var replacementWord = MakeFit(word, availableWidth);
                adjusted.Replace(word, replacementWord);
            }

            return adjusted.ToString();
        }
        private static bool TooWide(string word, Unit width)
        {
            var tm = new TextMeasurement(new Font("Arial", new Unit(10, UnitType.Point)));
            double f = tm.MeasureString(word, UnitType.Point).Width;
            return f > width.Point;
        }
        private static string MakeFit(string word, Unit width)
        {
            var adjustedWord = new StringBuilder();
            var current = string.Empty;
            foreach (char c in word)
            {
                if (TooWide(current + c, width))
                {
                    adjustedWord.Append(current);
                    adjustedWord.Append(Chars.CR);
                    current = c.ToString();
                }
                else
                {
                    current += c;
                }
            }
            adjustedWord.Append(current);

            return adjustedWord.ToString();
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