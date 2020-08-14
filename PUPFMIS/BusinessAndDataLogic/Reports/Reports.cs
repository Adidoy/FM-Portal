using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Shapes;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace PUPFMIS.BusinessAndDataLogic
{

    public class ReportPageSetup
    {
        /// <summary>
        /// Gets and sets the width of the page in Inches. 
        /// Data Type: Double
        /// </summary>
        public double? PageWidth { get; set; }
        /// <summary>
        /// Gets and sets the height of the page in Inches.
        /// Data Type: Double
        /// </summary>
        public double? PageHeight { get; set; }
        /// <summary>
        /// Gets and sets the Page Orientation
        /// Data Type: Enum MigraDoc.DocumentObjectModel.Orientation
        /// </summary>
        public Orientation? PageOrientation { get; set; }
        /// <summary>
        /// Gets or sets Top Margin in inches.
        /// Data Type: Double
        /// </summary>
        public double? TopMargin { get; set; }
        /// <summary>
        /// Gets or sets Bottom Margin in inches.
        /// Data Type: Double
        /// </summary>
        public double? BottomMargin { get; set; }
        /// <summary>
        /// Gets or sets Left Margin in inches.
        /// Data Type: Double
        /// </summary>
        public double? LeftMargin { get; set; }
        /// <summary>
        /// Gets or sets Right Margin in inches.
        /// Data Type: Double
        /// </summary>
        public double? RightMargin { get; set; }
    }
    public class HeaderLine
    {
        private Font _font = new Font();

        /// <summary>
        /// Creates new instance of HeaderLine.
        /// </summary>
        public HeaderLine() { }
        /// <summary>
        /// Creates new instance of HeaderLine.
        /// </summary>
        /// <param name="Content">string: Content Text</param>
        /// <param name="FontSize">double Font size in Point (pt)</param>
        /// <param name="IsBold">bool: Sets the Content Text to Bold face</param>
        /// <param name="IsItalic">bool: Sets the Content Text to Italics</param>
        /// <param name="ParagraphAlignment">MigraDoc.DocumentObjectModel: Sets the alignment of the Content Text.</param>
        public HeaderLine(string Content, double FontSize, bool IsBold, bool IsItalic, ParagraphAlignment ParagraphAlignment)
        {
            this.Content = Content;
            this.FontSize = FontSize;
            this.Bold = IsBold;
            this.Italic = IsItalic;
            this.ParagraphAlignment = ParagraphAlignment;
        }
        /// <summary>
        /// Creates new instance of HeaderLine.
        /// </summary>
        /// <param name="Content">string: Content Text</param>
        /// <param name="FontSize">double Font size in Point (pt)</param>
        /// <param name="IsBold">bool: Sets the Content Text to Bold face</param>
        /// <param name="ParagraphAlignment">MigraDoc.DocumentObjectModel: Sets the alignment of the Content Text.</param>
        public HeaderLine(string Content, double FontSize, bool IsBold, ParagraphAlignment ParagraphAlignment)
        {
            this.Content = Content;
            this.FontSize = FontSize;
            this.Bold = IsBold;
            this.ParagraphAlignment = ParagraphAlignment;
        }
        /// <summary>
        /// Creates new instance of HeaderLine.
        /// </summary>
        /// <param name="Content">string: Content Text</param>
        /// <param name="FontSize">double Font size in Point (pt)</param>
        /// <param name="IsBold">bool: Sets the Content Text to Bold face</param>
        /// <param name="IsItalic">bool: Sets the Content Text to Italics</param>
        /// <param name="ParagraphAlignment">MigraDoc.DocumentObjectModel: Sets the alignment of the Content Text.</param>
        /// <param name="LeftIndent">double: Sets left indent of Content Text in Point (pt)</param>
        public HeaderLine(string Content, double FontSize, bool IsBold, bool IsItalic, ParagraphAlignment ParagraphAlignment, double LeftIndent)
        {
            this.Content = Content;
            this.FontSize = FontSize;
            this.Bold = IsBold;
            this.Italic = IsItalic;
            this.ParagraphAlignment = ParagraphAlignment;
            this.LeftIndent = LeftIndent;
        }
        /// <summary>
        /// Gets or sets the Content Text of HeaderLine.
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// Gets or sets the font size of HeaderLine in Point (pt)
        /// </summary>
        public double FontSize
        {
            get { return _font.Size; }
            set { _font.Size = new Unit(value, UnitType.Point); }
        }
        /// <summary>
        /// Gets or sets the Content Text in bold face.
        /// </summary>
        public bool Bold
        {
            get { return _font.Bold; }
            set { _font.Bold = value; }
        }
        /// <summary>
        /// Gets of sets the Content Text in italics.
        /// </summary>
        public bool Italic
        {
            get { return _font.Italic; }
            set { _font.Italic = value; }
        }
        /// <summary>
        /// Gets or set the MigraDoc.DocumentObjectModel.ParagraphAlignment of the Content Text.
        /// </summary>
        public ParagraphAlignment ParagraphAlignment { get; set; }
        /// <summary>
        /// Gets or sets the left indent of the Content Text in Points (pt).
        /// </summary>
        public double LeftIndent { get; set; }
    }
    public class TextWithFormat
    {
        public Font TextFont { get; set; }
        public string Content { get; set; }
        public TextWithFormat()
        {
            TextFont = new Font();
        }
        public TextWithFormat(string Content)
        {
            this.Content = Content;
        }
        public TextWithFormat(string Content, bool Bold, bool Italic, double FontSize)
        {
            TextFont = new Font();
            this.Content = Content;
            TextFont.Bold = Bold;
            TextFont.Italic = Italic;
            TextFont.Size = new Unit(FontSize, UnitType.Point);
        }
    }
    public class Reports
    {
        public ReportPageSetup ReportPageSetup { get; set; }
        public TextWithFormat TextWithFormat { get; set; }
        /// <summary>
        /// Gets or sets the Report Filename
        /// Data Type: String
        /// </summary>
        public string ReportFilename { get; set; }
        public Table ReportTable { get; set; }

        /// <summary>
        /// Class to set paragraph format and content
        /// </summary>
        public Paragraph ReportParagraph { get; set; }

        private Document ReportDocument = new Document();
        private Section ReportSection = new Section();
        private Table HeaderTable;
        private Table SubHeaderTable;
        private Table SubHeaderTable1;
        private Table SubHeaderTable2;
        private TextFrame HeaderTextFrame;
        private TextFrame SubHeader1;
        private TextFrame SubHeader2;
        private Row HeaderRow;
        private Row SubHeaderRow;
        private Row SubHeaderRow1;
        private Row SubHeaderRow2;

        public Reports()
        {
            ReportDocument = new Document();
            ReportSection = new Section();
            ReportPageSetup = new ReportPageSetup();
            TextWithFormat = new TextWithFormat();
            ReportTable = new Table();
        }
        /// <summary>
        /// Creates new document.
        /// Default Paper Size: Folio (8.50 x 13.00 inches)
        /// Default Left and Right Margins: 0.25 inches
        /// Default Top and Bottom Margins: 0.50 inches 
        /// </summary>
        public void CreateDocument()
        {
            ReportSection = ReportDocument.AddSection();
            ReportSection.PageSetup.PageWidth = ReportPageSetup.PageWidth == null ? new Unit(8.5, UnitType.Inch) : new Unit((double)ReportPageSetup.PageWidth, UnitType.Inch);
            ReportSection.PageSetup.PageHeight = ReportPageSetup.PageHeight == null ? new Unit(13.0, UnitType.Inch) : new Unit((double)ReportPageSetup.PageHeight, UnitType.Inch) ;
            ReportSection.PageSetup.TopMargin = ReportPageSetup.TopMargin == null ? new Unit(0.50, UnitType.Inch) : new Unit((double)ReportPageSetup.TopMargin, UnitType.Inch);
            ReportSection.PageSetup.BottomMargin = ReportPageSetup.BottomMargin == null ? new Unit(0.50, UnitType.Inch) : new Unit((double)ReportPageSetup.BottomMargin, UnitType.Inch);
            ReportSection.PageSetup.LeftMargin = ReportPageSetup.LeftMargin == null ? new Unit(0.25, UnitType.Inch) : new Unit((double)ReportPageSetup.LeftMargin, UnitType.Inch);
            ReportSection.PageSetup.RightMargin = ReportPageSetup.RightMargin == null ? new Unit(0.25, UnitType.Inch) : new Unit((double)ReportPageSetup.RightMargin, UnitType.Inch);
            ReportSection.PageSetup.Orientation = ReportPageSetup.PageOrientation == null ? Orientation.Landscape : (Orientation)ReportPageSetup.PageOrientation;

            ReportDocument.Info.Title = ReportFilename;
        }
        public void CreateDocument(ReportPageSetup PageSetup)
        {
            ReportSection = ReportDocument.AddSection();
            ReportSection.PageSetup.PageWidth = (double)PageSetup.PageWidth;
            ReportSection.PageSetup.PageHeight = (double)PageSetup.PageHeight;
            ReportSection.PageSetup.TopMargin = (double)PageSetup.TopMargin;
            ReportSection.PageSetup.BottomMargin = (double)PageSetup.BottomMargin;
            ReportSection.PageSetup.LeftMargin = (double)PageSetup.LeftMargin;
            ReportSection.PageSetup.RightMargin = (double)PageSetup.RightMargin;
            ReportSection.PageSetup.Orientation = (Orientation)PageSetup.PageOrientation;
        }
        /// <summary>
        /// Creates new document.
        /// </summary>
        /// <param name="PageWidth">double: Sets the width of the document page in Inches (in)</param>
        /// <param name="PageHeight">double: Sets the height of the document page in Inches (in)</param>
        public void CreateDocument(double PageWidth, double PageHeight)
        {
            ReportSection = ReportDocument.AddSection();
            ReportSection.PageSetup.PageWidth = new Unit(PageWidth, UnitType.Inch);
            ReportSection.PageSetup.PageHeight = new Unit(PageHeight, UnitType.Inch);

            ReportDocument.Info.Title = ReportFilename;
        }
        /// <summary>
        /// Creates new document.
        /// </summary>
        /// <param name="PageWidth">double: Sets the width of the document page in Inches (in.)</param>
        /// <param name="PageHeight">double: Sets the height of the document page in Inches(in.)</param>
        /// <param name="PaperOrientation">Migradoc.DocumentObjectModel.Orientation: Sets the paper orientation of the document</param>
        public void CreateDocument(double PageWidth, double PageHeight, Orientation PaperOrientation)
        {
            ReportSection = ReportDocument.AddSection();
            ReportSection.PageSetup.PageWidth = new Unit(PageWidth, UnitType.Inch);
            ReportSection.PageSetup.PageHeight = new Unit(PageHeight, UnitType.Inch);
            ReportSection.PageSetup.Orientation = PaperOrientation;

            ReportDocument.Info.Title = ReportFilename;
        }
        /// <summary>
        /// Creates new document.
        /// </summary>
        /// <param name="PageWidth">double: Sets the width of the document page in Inches (in.)</param>
        /// <param name="PageHeight">double: Sets the height of the document page in Inches(in.)</param>
        /// <param name="PaperOrientation">Migradoc.DocumentObjectModel.Orientation: Sets the paper orientation of the document</param>
        /// <param name="Margins">double: Sets the top, bottom, left and right margin in Inches (in.)</param>
        public void CreateDocument(double PageWidth, double PageHeight, Orientation PageOrientation, double Margins)
        {
            ReportSection = ReportDocument.AddSection();
            ReportSection.PageSetup.PageWidth = new Unit(PageWidth, UnitType.Inch);
            ReportSection.PageSetup.PageHeight = new Unit(PageHeight, UnitType.Inch);
            ReportSection.PageSetup.TopMargin = new Unit(Margins, UnitType.Inch);
            ReportSection.PageSetup.BottomMargin = new Unit(Margins, UnitType.Inch);
            ReportSection.PageSetup.LeftMargin = new Unit(Margins, UnitType.Inch);
            ReportSection.PageSetup.RightMargin = new Unit(Margins, UnitType.Inch);
            ReportSection.PageSetup.Orientation = PageOrientation;

            ReportDocument.Info.Title = ReportFilename;
        }
        /// <summary>
        /// Creates new document.
        /// </summary>
        /// <param name="PageWidth">double: Sets the width of the document page in Inches (in.)</param>
        /// <param name="PageHeight">double: Sets the height of the document page in Inches(in.)</param>
        /// <param name="PaperOrientation">Migradoc.DocumentObjectModel.Orientation: Sets the paper orientation of the document.</param>
        /// <param name="TopAndBottomMargins">double: Sets the top and bottom margins equally in Inches (in.)</param>
        /// <param name="LeftAndRightMargins">double: Sets the left and right margins equally in Inches (in.)</param>
        public void CreateDocument(double PageWidth, double PageHeight, Orientation PageOrientation, double TopAndBottomMargins, double LeftAndRightMargins)
        {
            ReportSection = ReportDocument.AddSection();
            ReportSection.PageSetup.PageWidth = new Unit(PageWidth, UnitType.Inch);
            ReportSection.PageSetup.PageHeight = new Unit(PageHeight, UnitType.Inch);
            ReportSection.PageSetup.TopMargin = new Unit(TopAndBottomMargins, UnitType.Inch);
            ReportSection.PageSetup.BottomMargin = new Unit(TopAndBottomMargins, UnitType.Inch);
            ReportSection.PageSetup.LeftMargin = new Unit(LeftAndRightMargins, UnitType.Inch);
            ReportSection.PageSetup.RightMargin = new Unit(LeftAndRightMargins, UnitType.Inch);
            ReportSection.PageSetup.Orientation = Orientation.Landscape;

            ReportDocument.Info.Title = ReportFilename;
        }
        /// <summary>
        /// Creates new document.
        /// </summary>
        /// <param name="PageWidth">double: Sets the width of the document page in Inches (in.)</param>
        /// <param name="PageHeight">double: Sets the height of the document page in Inches(in.)</param>
        /// <param name="PaperOrientation">Migradoc.DocumentObjectModel.Orientation: Sets the paper orientation of the document.</param>
        /// <param name="TopMargin">double: Sets the top margin in Inches (in.)</param>
        /// <param name="BottomMargin">double: Sets the top bottom in Inches (in.)</param>
        /// <param name="LeftMargin">double: Sets the left margin in Inches (in.)</param>
        /// <param name="RightMargin">double: Sets the right margin in Inches (in.)</param>
        public void CreateDocument(double PageWidth, double PageHeight, Orientation PageOrientation, double TopMargin, double BottomMargin, double LeftMargin, double RightMargin)
        {
            ReportSection = ReportDocument.AddSection();
            ReportSection.PageSetup.PageWidth = new Unit(PageWidth, UnitType.Inch);
            ReportSection.PageSetup.PageHeight = new Unit(PageHeight, UnitType.Inch);
            ReportSection.PageSetup.TopMargin = new Unit(TopMargin, UnitType.Inch);
            ReportSection.PageSetup.BottomMargin = new Unit(BottomMargin, UnitType.Inch);
            ReportSection.PageSetup.LeftMargin = new Unit(LeftMargin, UnitType.Inch);
            ReportSection.PageSetup.RightMargin = new Unit(RightMargin, UnitType.Inch);
            ReportSection.PageSetup.Orientation = Orientation.Landscape;

            ReportDocument.Info.Title = ReportFilename;
        }
        /// <summary>
        /// Creates a single column Header for the report document.
        /// </summary>
        public void AddSingleColumnHeader()
        {
            var pageWidth = ReportSection.PageSetup.Orientation == Orientation.Portrait ? ReportSection.PageSetup.PageWidth.Value : ReportSection.PageSetup.PageHeight.Value;
            var headerWidth = pageWidth - (ReportSection.PageSetup.LeftMargin.Value + ReportSection.PageSetup.RightMargin.Value);
            HeaderTable = ReportSection.AddTable();
            HeaderTable.AddColumn(new Unit(headerWidth, UnitType.Inch));
        }
        public void AddSingleColumnHeader(bool WithLogo)
        {
            var logoWidth = 0.19685;
            var pageWidth = ReportSection.PageSetup.Orientation == Orientation.Portrait ? ReportSection.PageSetup.PageWidth.Value : ReportSection.PageSetup.PageHeight.Value;
            var headerWidth = pageWidth - (ReportSection.PageSetup.LeftMargin.Value + ReportSection.PageSetup.RightMargin.Value) - logoWidth;
            HeaderTable = ReportSection.AddTable();
            HeaderTable.AddColumn(new Unit(logoWidth, UnitType.Inch));
            HeaderTable.AddColumn(new Unit(headerWidth, UnitType.Inch));
        }
        public void AddDoubleColumnHeader()
        {
            var pageWidth = ReportSection.PageSetup.Orientation == Orientation.Portrait ? ReportSection.PageSetup.PageWidth.Value : ReportSection.PageSetup.PageHeight.Value;
            var headerWidth = (pageWidth - (ReportSection.PageSetup.LeftMargin.Value + ReportSection.PageSetup.RightMargin.Value))/2;
            HeaderTable = ReportSection.AddTable();
            HeaderTable.AddColumn(new Unit(headerWidth, UnitType.Inch));
            HeaderTable.AddColumn(new Unit(headerWidth, UnitType.Inch));
        }
        public void AddDoubleColumnHeader(string LogoPath)
        {
            var pageWidth = ReportSection.PageSetup.Orientation == Orientation.Portrait ? ReportSection.PageSetup.PageWidth.Value : ReportSection.PageSetup.PageHeight.Value;
            var logoWidth = 0.7;
            var headerWidth = (pageWidth - (ReportSection.PageSetup.LeftMargin.Value + ReportSection.PageSetup.RightMargin.Value) - logoWidth);
            HeaderTable = ReportSection.AddTable();
            HeaderTable.AddColumn(new Unit(logoWidth, UnitType.Inch));
            HeaderTable.AddColumn(new Unit(headerWidth, UnitType.Inch));
            HeaderRow = HeaderTable.AddRow();
            HeaderRow.Cells[0].Format.Alignment = ParagraphAlignment.Center;
            HeaderRow.Cells[0].VerticalAlignment = VerticalAlignment.Center;
            var Logo = HeaderRow.Cells[0].AddParagraph().AddImage(LogoPath);
            Logo.Height = new Unit(0.65, UnitType.Inch);
            Logo.Width = new Unit(0.65, UnitType.Inch);
            HeaderTextFrame = HeaderRow.Cells[1].AddTextFrame();
            HeaderTextFrame.Height = 0;

            SubHeaderTable = HeaderTextFrame.AddTable();
            SubHeaderTable.AddColumn(new Unit((headerWidth * 0.70), UnitType.Inch));
            SubHeaderTable.AddColumn(new Unit((headerWidth * 0.30), UnitType.Inch));

            SubHeaderRow = SubHeaderTable.AddRow();
            SubHeader1 = SubHeaderRow.Cells[0].AddTextFrame();
            SubHeader2 = SubHeaderRow.Cells[1].AddTextFrame();
            SubHeader1.Height = 0;
            SubHeader2.Height = 0;

            SubHeaderTable1 = SubHeader1.AddTable();
            SubHeaderTable1.AddColumn(new Unit((headerWidth * 0.70), UnitType.Inch));

            SubHeaderTable2 = SubHeader2.AddTable();
            SubHeaderTable2.AddColumn(new Unit((headerWidth * 0.30), UnitType.Inch));
        }
        /// <summary>
        /// Creates a single column Header for the report document.
        /// </summary>
        /// <param name="HeaderLine" typeName = "Reports.HeaderLine">Header line content.</param>
        public void AddColumnHeader(HeaderLine HeaderLine)
        {
            HeaderRow = HeaderTable.AddRow();
            HeaderRow.Cells[0].AddParagraph(HeaderLine.Content);
            HeaderRow.Cells[0].Format.Font.Size = HeaderLine.FontSize;
            HeaderRow.Cells[0].Format.Font.Bold = HeaderLine.Bold;
            HeaderRow.Cells[0].Format.Font.Italic = HeaderLine.Italic;
            HeaderRow.Cells[0].Format.Alignment = HeaderLine.ParagraphAlignment;
            HeaderRow.Cells[0].Format.LeftIndent = new Unit(HeaderLine.LeftIndent, UnitType.Point);
        }
        public void AddColumnHeader(TextWithFormat[] FormattedHeaderLine, ParagraphAlignment ParagraphAlignment)
        {
            Paragraph paragraph = new Paragraph();
            foreach(var ft in FormattedHeaderLine)
            {
                FormattedText formattedText = new FormattedText();
                formattedText.AddText(ft.Content);
                formattedText.Bold = ft.TextFont.Bold;
                formattedText.Italic = ft.TextFont.Italic;
                formattedText.Size = ft.TextFont.Size;
                paragraph.Add(formattedText);
            }
            paragraph.AddFormattedText();
            HeaderRow = HeaderTable.AddRow();
            HeaderRow.Cells[0].Add(paragraph);
            HeaderRow.Cells[0].Format.Alignment = ParagraphAlignment;
        }
        public void AddColumnHeader(HeaderLine Column1, HeaderLine Column2)
        {
            SubHeaderRow1 = SubHeaderTable1.AddRow();
            SubHeaderRow1.Cells[0].AddParagraph(Column1.Content);
            SubHeaderRow1.Cells[0].Format.Font.Size = Column1.FontSize;
            SubHeaderRow1.Cells[0].Format.Font.Bold = Column1.Bold;
            SubHeaderRow1.Cells[0].Format.Font.Italic = Column1.Italic;
            SubHeaderRow1.Cells[0].Format.Alignment = Column1.ParagraphAlignment;
            SubHeaderRow1.Cells[0].Format.LeftIndent = new Unit(Column1.LeftIndent, UnitType.Point);

            SubHeaderRow2 = SubHeaderTable2.AddRow();
            SubHeaderRow2.Cells[0].AddParagraph(Column2.Content);
            SubHeaderRow2.Cells[0].Format.Font.Size = Column2.FontSize;
            SubHeaderRow2.Cells[0].Format.Font.Bold = Column2.Bold;
            SubHeaderRow2.Cells[0].Format.Font.Italic = Column2.Italic;
            SubHeaderRow2.Cells[0].Format.Alignment = Column2.ParagraphAlignment;
            SubHeaderRow2.Cells[0].Format.LeftIndent = new Unit(Column2.LeftIndent, UnitType.Point);
        }
        public void AddNewLine()
        {
            ReportSection.AddParagraph("\n");
        }
        public void AddTable(List<ContentColumn> Columns, bool Bordered)
        {
            var table = new ContentTable();
            ReportTable = ReportSection.AddTable();
            ReportTable = table.CreateTable(ReportTable, Columns, Bordered);
        }
        public void AddRowContent(List<ContentCell> Cells, double RowHeight)
        {
            var table = new ContentTable();
            ReportTable = table.AddTableRow(ReportTable, Cells, RowHeight);
        }
        /// <summary>
        /// Generates the PDF Report
        /// </summary>
        /// <returns></returns>
        public MemoryStream GenerateReport()
        {
            ReportDocument.UseCmykColor = true;
            string Filename = ReportFilename;
            PdfDocumentRenderer PDFRenderer = new PdfDocumentRenderer(true);
            PDFRenderer.Document = ReportDocument;
            PDFRenderer.RenderDocument();

            MemoryStream ReportStream = new MemoryStream();
            PDFRenderer.PdfDocument.Save(ReportStream, false);
            return ReportStream;

        }
        public static string AmountToWords(decimal Amount)
        {
            var amt = Amount.ToString().Split(".".ToCharArray(), System.StringSplitOptions.RemoveEmptyEntries).ToArray();
            int number = int.Parse(amt[0]);
            var cents = amt[1];
            var amtWord = NumberToWords(number);
            return amtWord + " Pesos and " + cents + "/100 only."; 
        }
        private static string NumberToWords(int number)
        {
            if (number == 0)
                return "zero";

            if (number < 0)
                return "minus " + NumberToWords(Math.Abs(number));

            string words = "";

            if ((number / 1000000) > 0)
            {
                words += NumberToWords(number / 1000000) + " Million ";
                number %= 1000000;
            }

            if ((number / 1000) > 0)
            {
                words += NumberToWords(number / 1000) + " Thousand ";
                number %= 1000;
            }

            if ((number / 100) > 0)
            {
                words += NumberToWords(number / 100) + " Hundred ";
                number %= 100;
            }

            if (number > 0)
            {
                if (words != "")
                    words += "and ";

                var unitsMap = new[] { "Zero", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen" };
                var tensMap = new[] { "Zero", "Ten", "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };

                if (number < 20)
                    words += unitsMap[number];
                else
                {
                    words += tensMap[number / 10];
                    if ((number % 10) > 0)
                        words += "-" + unitsMap[number % 10];
                }
            }

            return words;
        }
    }

    public class ContentCell
    {
        public string Content { get; set; }
        public int CellIndex { get; set; }
        public double FontSize { get; set; }
        public bool Bold { get; set; }
        public bool Italic { get; set; }
        public ParagraphAlignment HorizontalAlign { get; set; }
        public VerticalAlignment VerticalAlign { get; set; }
        public bool BottomRule { get; set; }
        public bool LeftRule { get; set; }
        public bool TopRule { get; set; }
        public bool RightRule { get; set; }
        public int MergeRight { get; set; }
        public int MergeDown { get; set; }
        public Color FontColor { get; set; }
        public ContentCell(string Content, int CellIndex)
        {
            this.Content = Content;
            this.CellIndex = CellIndex;
        }
        public ContentCell(string Content, int CellIndex, double FontSize)
        {
            this.Content = Content;
            this.CellIndex = CellIndex;
            this.FontSize = FontSize;
        }
        public ContentCell(string Content, int CellIndex, double FontSize, bool Bold)
        {
            this.Content = Content;
            this.CellIndex = CellIndex;
            this.FontSize = FontSize;
            this.Bold = Bold;
        }
        public ContentCell(string Content, int CellIndex, double FontSize, bool Bold, bool Italic)
        {
            this.Content = Content;
            this.CellIndex = CellIndex;
            this.FontSize = FontSize;
            this.Bold = Bold;
            this.Italic = Italic;
        }
        public ContentCell(string Content, int CellIndex, double FontSize, bool Bold, bool Italic, ParagraphAlignment HorizontalAlign)
        {
            this.Content = Content;
            this.CellIndex = CellIndex;
            this.FontSize = FontSize;
            this.Bold = Bold;
            this.Italic = Italic;
            this.HorizontalAlign = HorizontalAlign;
        }
        public ContentCell(string Content, int CellIndex, double FontSize, bool Bold, bool Italic, ParagraphAlignment HorizontalAlign, VerticalAlignment VerticalAlign)
        {
            this.Content = Content;
            this.CellIndex = CellIndex;
            this.FontSize = FontSize;
            this.Bold = Bold;
            this.Italic = Italic;
            this.HorizontalAlign = HorizontalAlign;
            this.VerticalAlign = VerticalAlign;
        }
        public ContentCell(string Content, int CellIndex, double FontSize, bool Bold, bool Italic, ParagraphAlignment HorizontalAlign, VerticalAlignment VerticalAlign, int MergeRight)
        {
            this.Content = Content;
            this.CellIndex = CellIndex;
            this.FontSize = FontSize;
            this.Bold = Bold;
            this.Italic = Italic;
            this.HorizontalAlign = HorizontalAlign;
            this.VerticalAlign = VerticalAlign;
            this.MergeRight = MergeRight;
        }
        public ContentCell(string Content, int CellIndex, double FontSize, bool Bold, bool Italic, ParagraphAlignment HorizontalAlign, VerticalAlignment VerticalAlign, int MergeRight, int MergeDown)
        {
            this.Content = Content;
            this.CellIndex = CellIndex;
            this.FontSize = FontSize;
            this.Bold = Bold;
            this.Italic = Italic;
            this.HorizontalAlign = HorizontalAlign;
            this.VerticalAlign = VerticalAlign;
            this.MergeRight = MergeRight;
            this.MergeDown = MergeDown;
        }
        public ContentCell(string Content, int CellIndex, double FontSize, bool Bold, bool Italic, ParagraphAlignment HorizontalAlign, VerticalAlignment VerticalAlign, int MergeRight, int MergeDown, bool BottomRule)
        {
            this.Content = Content;
            this.CellIndex = CellIndex;
            this.FontSize = FontSize;
            this.Bold = Bold;
            this.Italic = Italic;
            this.HorizontalAlign = HorizontalAlign;
            this.VerticalAlign = VerticalAlign;
            this.MergeRight = MergeRight;
            this.MergeDown = MergeDown;
            this.BottomRule = BottomRule;
        }
        public ContentCell(string Content, int CellIndex, double FontSize, bool Bold, bool Italic, ParagraphAlignment HorizontalAlign, VerticalAlignment VerticalAlign, int MergeRight, int MergeDown, bool BottomRule, Color FontColor)
        {
            this.Content = Content;
            this.CellIndex = CellIndex;
            this.FontSize = FontSize;
            this.Bold = Bold;
            this.Italic = Italic;
            this.HorizontalAlign = HorizontalAlign;
            this.VerticalAlign = VerticalAlign;
            this.MergeRight = MergeRight;
            this.MergeDown = MergeDown;
            this.BottomRule = BottomRule;
            this.FontColor = FontColor;
        }
        public ContentCell(string Content, int CellIndex, double FontSize, bool Bold, bool Italic, ParagraphAlignment HorizontalAlign, VerticalAlignment VerticalAlign, int MergeRight, int MergeDown, bool BottomRule, Color FontColor, bool RightRule)
        {
            this.Content = Content;
            this.CellIndex = CellIndex;
            this.FontSize = FontSize;
            this.Bold = Bold;
            this.Italic = Italic;
            this.HorizontalAlign = HorizontalAlign;
            this.VerticalAlign = VerticalAlign;
            this.MergeRight = MergeRight;
            this.MergeDown = MergeDown;
            this.BottomRule = BottomRule;
            this.RightRule = RightRule;
            this.FontColor = FontColor;
        }
        public ContentCell(string Content, int CellIndex, double FontSize, bool Bold, bool Italic, ParagraphAlignment HorizontalAlign, VerticalAlignment VerticalAlign, int MergeRight, int MergeDown, bool BottomRule, Color FontColor, bool RightRule, bool TopRule)
        {
            this.Content = Content;
            this.CellIndex = CellIndex;
            this.FontSize = FontSize;
            this.Bold = Bold;
            this.Italic = Italic;
            this.HorizontalAlign = HorizontalAlign;
            this.VerticalAlign = VerticalAlign;
            this.MergeRight = MergeRight;
            this.MergeDown = MergeDown;
            this.BottomRule = BottomRule;
            this.RightRule = RightRule;
            this.TopRule = TopRule;
            this.FontColor = FontColor;
        }
        public ContentCell(string Content, int CellIndex, double FontSize, bool Bold, bool Italic, ParagraphAlignment HorizontalAlign, VerticalAlignment VerticalAlign, int MergeRight, int MergeDown, bool BottomRule, Color FontColor, bool RightRule, bool TopRule, bool LeftRule)
        {
            this.Content = Content;
            this.CellIndex = CellIndex;
            this.FontSize = FontSize;
            this.Bold = Bold;
            this.Italic = Italic;
            this.HorizontalAlign = HorizontalAlign;
            this.VerticalAlign = VerticalAlign;
            this.MergeRight = MergeRight;
            this.MergeDown = MergeDown;
            this.BottomRule = BottomRule;
            this.RightRule = RightRule;
            this.TopRule = TopRule;
            this.LeftRule = LeftRule;
            this.FontColor = FontColor;
        }
    }
    public class ContentColumn
    {
        public double ColumnWidth { get; set; }
        public Color ColumnColor { get; set; }
        public bool? WithoutBorder { get; set; }
        public ContentColumn()
        {
            ColumnWidth = 0.0;
            ColumnColor = Colors.White;
            WithoutBorder = false;
        }
        public ContentColumn(double ColumnWidth)
        {
            this.ColumnWidth = ColumnWidth;
        }
        public ContentColumn(double ColumnWidth, Color ColumnColor)
        {
            this.ColumnWidth = ColumnWidth;
            this.ColumnColor = ColumnColor;
        }
        public ContentColumn(double ColumnWidth, Color ColumnColor, bool WithoutBorder)
        {
            this.ColumnWidth = ColumnWidth;
            this.ColumnColor = ColumnColor;
            this.WithoutBorder = WithoutBorder;
        }
    }
    public class ContentTable
    {
        private Table _table = new Table();
        private Column _column = new Column();
        public List<ContentColumn> Columns { get; set; }
        public List<ContentCell> Rows { get; set; }

        public ContentTable()
        {
            _table = new Table();
            _column = new Column();
            Columns = new List<ContentColumn>();
            Rows = new List<ContentCell>();
        }
        public Table CreateTable(Table ReportTable, List<ContentColumn> Columns, bool Bordered)
        {
            ReportTable.Style = "Table";
            if (Bordered)
            {
                ReportTable.Borders.Color = new Color(0, 0, 0);
            }
            foreach(var col in Columns)
            {
                _column = ReportTable.AddColumn();
                _column.Width = new Unit(col.ColumnWidth, UnitType.Inch);
                _column.Shading.Color = col.ColumnColor;
                _column.Borders.Visible = (col.WithoutBorder == null && Bordered == false) ? false : col.WithoutBorder == true ? false : true;
            }
            return ReportTable;
        }
        public Table AddTableRow(Table ReportTable, List<ContentCell> Cells, double RowHeight)
        {
            var _tableRow = new Row();
            _tableRow = ReportTable.AddRow();
            foreach (var row in Cells)
            {
                _tableRow.Height = new Unit(RowHeight, UnitType.Inch);
                _tableRow.Cells[row.CellIndex].Format.KeepTogether = true;
                _tableRow.Cells[row.CellIndex].Format.Font.Size = row.FontSize;
                _tableRow.Cells[row.CellIndex].Format.Font.Bold = row.Bold;
                _tableRow.Cells[row.CellIndex].Format.Font.Italic = row.Italic;
                _tableRow.Cells[row.CellIndex].Format.Alignment = row.HorizontalAlign;
                _tableRow.Cells[row.CellIndex].VerticalAlignment = row.VerticalAlign;
                _tableRow.Cells[row.CellIndex].AddParagraph(AdjustIfTooWideToFitIn(_tableRow.Cells[row.CellIndex], row.Content == null? "\n" : row.Content));
                _tableRow.Cells[row.CellIndex].Format.Font.Color = row.FontColor == null ? new Color(255, 255, 255) : row.FontColor;
                if (!String.IsNullOrEmpty(row.MergeRight.ToString()))
                {
                    _tableRow.Cells[row.CellIndex].MergeRight = row.MergeRight;
                }
                if (!String.IsNullOrEmpty(row.MergeDown.ToString()))
                {
                    _tableRow.Cells[row.CellIndex].MergeDown = row.MergeDown;
                }
                if (row.BottomRule == true)
                {
                    _tableRow.Cells[row.CellIndex].Borders.Bottom.Visible = true;
                    _tableRow.Cells[row.CellIndex].Borders.Color = new Color(0, 0, 0, 0);
                }
                if (row.TopRule == true)
                {
                    _tableRow.Cells[row.CellIndex].Borders.Top.Visible = true;
                    _tableRow.Cells[row.CellIndex].Borders.Color = new Color(0, 0, 0, 0);
                }
                if (row.LeftRule == true)
                {
                    _tableRow.Cells[row.CellIndex].Borders.Left.Visible = true;
                    _tableRow.Cells[row.CellIndex].Borders.Color = new Color(0, 0, 0, 0);
                }
                if (row.RightRule == true)
                {
                    _tableRow.Cells[row.CellIndex].Borders.Right.Visible = true;
                    _tableRow.Cells[row.CellIndex].Borders.Color = new Color(0, 0, 0, 0);
                }
            }
            return ReportTable;
        }
        private static string AdjustIfTooWideToFitIn(Cell cell, string text)
        {
            Column column = cell.Column;
            Unit availableWidth = column.Width - column.Table.Borders.Width - cell.Borders.Width;
            Unit fontSize = cell.Format.Font.Size;

            var tooWideWords = text.Split(" ".ToCharArray()).Distinct().Where(s => TooWide(s, availableWidth, fontSize));

            var adjusted = new StringBuilder(text);
            foreach (string word in tooWideWords)
            {
                var replacementWord = MakeFit(word, availableWidth, fontSize);
                adjusted.Replace(word, replacementWord);
            }

            return adjusted.ToString();
        }
        private static bool TooWide(string word, Unit width, Unit FontSize)
        {
            var tm = new TextMeasurement(new Font("Arial", FontSize));
            double f = tm.MeasureString(word, UnitType.Point).Width;
            return f > width.Point;
        }
        private static string MakeFit(string word, Unit width, Unit FontSize)
        {
            var adjustedWord = new StringBuilder();
            var current = string.Empty;
            foreach (char c in word)
            {
                if (TooWide(current + c, width, FontSize))
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
    }
}