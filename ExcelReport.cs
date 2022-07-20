using System.Collections.Generic;
using System.Linq;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;


namespace WarpScheduling
{
    class ExcelReport
    {
        public  enum Wpr
        {
            BlueWarper1=1,
            GreenWarper2=2,
            BlueWarper3=3,
            BlueWarper4=4,
            BlueWarper5=5
        }
        internal static void CreateSpreadSheet(List<Warp> wp , string docname)
        {
            
            using (SpreadsheetDocument document = SpreadsheetDocument.Create(@"C:\STIRoot\" +docname , SpreadsheetDocumentType.Workbook))
            {
                var x =wp.GroupBy(j=> new { j.WarperID }).Select(group => new {warperid=group.Key.WarperID});

                uint sheetID = 1;
                WorkbookPart workbookpart = document.AddWorkbookPart();
                workbookpart.Workbook = new Workbook();
                document.WorkbookPart.Workbook.Sheets = new Sheets();
                Sheets sheets = document.WorkbookPart.Workbook.GetFirstChild<Sheets>();
                WorkbookStylesPart stylePart = workbookpart.AddNewPart<WorkbookStylesPart>();
                stylePart.Stylesheet = GenerateStylesheet();
                stylePart.Stylesheet.Save();

               

                foreach (var v in x)
                { 
                foreach (var i in wp.Where(w=> w.WarperID ==v.warperid).Take(1) )
                {

                      
                      
                        WorksheetPart worksheetpart = workbookpart.AddNewPart<WorksheetPart>();

                      
                        //  worksheetpart.Worksheet = new Worksheet(new SheetData());
                        // Sheets sheets = workbookpart.Workbook.AppendChild(new Sheets());
                        Wpr sn = (Wpr)i.WarperID;
                    Sheet sheet = new Sheet() { Id = document.WorkbookPart.GetIdOfPart(worksheetpart), SheetId = sheetID, Name =sn.ToString()};
                    sheets.Append(sheet);

                        Columns columns = new Columns();

                        columns.Append(new Column() { Min = 1, Max = 6, Width = 11, CustomWidth = true });
                    
                        SheetData sheetdata = new SheetData();
                       worksheetpart.Worksheet = new Worksheet(sheetdata);
                        //append custom columns before sheetdata
                        worksheetpart.Worksheet.InsertBefore(columns, worksheetpart.Worksheet.Elements<SheetData>().First());

                        Row TitleRow = new Row();
                        TitleRow.Append(new Cell() { DataType = CellValues.String, CellValue = new CellValue("Warping Priority"), StyleIndex=3 });
                        sheetdata.AppendChild(TitleRow);
                        Row TitleRow2 = new Row();
                        TitleRow2.Append(new Cell() { DataType = CellValues.String, CellValue = new CellValue(sn.ToString()), StyleIndex=2});
                        sheetdata.AppendChild(TitleRow2);
                        //
                        MergeCells mergeCells = new MergeCells();
                        mergeCells.Append(new MergeCell() { Reference = new StringValue("A1:H1") });
                        mergeCells.Append(new MergeCell() { Reference = new StringValue("A2:H2") });
                        worksheetpart.Worksheet.InsertAfter(mergeCells, worksheetpart.Worksheet.Elements<SheetData>().First());

                        //

                        Row headerRow = new Row();
                        headerRow.Append(new Cell() { DataType = CellValues.String, CellValue = new CellValue("Loom Type"), StyleIndex = 1 });
                        headerRow.Append(new Cell() { DataType = CellValues.String, CellValue = new CellValue("Priority") , StyleIndex=1});
                        headerRow.Append(new Cell() { DataType = CellValues.String, CellValue = new CellValue("Warp MO"), StyleIndex = 1 });
                        headerRow.Append(new Cell() { DataType = CellValues.String, CellValue = new CellValue("Warp Style"), StyleIndex = 1 });
                        headerRow.Append(new Cell() { DataType = CellValues.String, CellValue = new CellValue("Tickets"), StyleIndex = 1 });
                        headerRow.Append(new Cell() { DataType = CellValues.String, CellValue = new CellValue("Priorities"), StyleIndex = 1 });
                        headerRow.Append(new Cell() { DataType = CellValues.String, CellValue = new CellValue("Due Date"), StyleIndex = 1 });
                        headerRow.Append(new Cell() { DataType = CellValues.String, CellValue = new CellValue("Notes"), StyleIndex = 1 });
                        //headerRow.Append(Constructcell("Priority",CellValues.SharedString), 
                        //                Constructcell("Warp MO",CellValues.SharedString),
                        //                Constructcell("Warp Style",CellValues.SharedString),
                        //                Constructcell("PCS",CellValues.SharedString),
                        //                Constructcell("Due Date", CellValues.SharedString)
                        //                );
                        sheetdata.AppendChild(headerRow);

                        foreach (var z in wp.Where(w => w.WarperID == v.warperid).OrderBy(w => w.Priority))
                        {
                            Row mrow = new Row();
                            mrow.Append(new Cell() { DataType = CellValues.String, CellValue = new CellValue(z.SingleDouble), StyleIndex = 4 });
                            mrow.Append(new Cell() { DataType = CellValues.Number, CellValue = new CellValue(z.Priority.ToString()), StyleIndex = 4 });
                            mrow.Append(new Cell() { DataType = CellValues.String, CellValue = new CellValue(z.WarpMO), StyleIndex = 4 });
                            mrow.Append(new Cell() { DataType = CellValues.String, CellValue = new CellValue(z.WarpStyle), StyleIndex = 4 });
                            mrow.Append(new Cell() { DataType = CellValues.Number, CellValue = new CellValue(z.TotalTickets.ToString()), StyleIndex = 4 });
                            mrow.Append(new Cell() { DataType = CellValues.Number, CellValue = new CellValue(z.ChangesThisWarp.ToString()), StyleIndex = 4 });
                            mrow.Append(new Cell() { DataType = CellValues.String, CellValue = new CellValue(z.EarliestDueDate.ToShortDateString()), StyleIndex = 4 });
                            mrow.Append(new Cell() { DataType = CellValues.String, CellValue = new CellValue(z.Notes), StyleIndex = 4 });
                            //mrow.Append(Constructcell(z.Priority.ToString(), CellValues.Number),
                            //    Constructcell(z.WarpMO, CellValues.String),
                            //    Constructcell(z.WarpStyle, CellValues.String),
                            //    Constructcell(z.TotalTickets.ToString(), CellValues.Number),
                            //    Constructcell(z.EarliestDueDate.ToShortDateString(), CellValues.Date)
                            //    );
                            sheetdata.AppendChild(mrow);

                        }
                        sheetID++;
                }




                }

            }

        }
        internal static void CreateSpreadSheetAll(List<Warp> wp, string docname, Wpr n)
        {

            using (SpreadsheetDocument document = SpreadsheetDocument.Create(@"C:\STIRoot\" + docname, SpreadsheetDocumentType.Workbook))
            {
                var x = wp.GroupBy(j => new { j.WarperID }).Select(group => new { warperid = group.Key.WarperID });

                uint sheetID = 1;
                WorkbookPart workbookpart = document.AddWorkbookPart();
                workbookpart.Workbook = new Workbook();
                document.WorkbookPart.Workbook.Sheets = new Sheets();
                Sheets sheets = document.WorkbookPart.Workbook.GetFirstChild<Sheets>();
                WorkbookStylesPart stylePart = workbookpart.AddNewPart<WorkbookStylesPart>();
                stylePart.Stylesheet = GenerateStylesheet();
                stylePart.Stylesheet.Save();



                foreach (var v in x.Take(1))
                {
                    // foreach (var i in wp).Take(1)
                    {



                        WorksheetPart worksheetpart = workbookpart.AddNewPart<WorksheetPart>();


                        //  worksheetpart.Worksheet = new Worksheet(new SheetData());
                        // Sheets sheets = workbookpart.Workbook.AppendChild(new Sheets());

                        Wpr sn = n;// (Wpr)i.WarperID;
                        Sheet sheet = new Sheet() { Id = document.WorkbookPart.GetIdOfPart(worksheetpart), SheetId = sheetID, Name = sn.ToString() };
                        sheets.Append(sheet);

                        Columns columns = new Columns();

                        columns.Append(new Column() { Min = 1, Max = 6, Width = 11, CustomWidth = true });

                        SheetData sheetdata = new SheetData();
                        worksheetpart.Worksheet = new Worksheet(sheetdata);
                        //append custom columns before sheetdata
                        worksheetpart.Worksheet.InsertBefore(columns, worksheetpart.Worksheet.Elements<SheetData>().First());

                        Row TitleRow = new Row();
                        TitleRow.Append(new Cell() { DataType = CellValues.String, CellValue = new CellValue("Warping Priority"), StyleIndex = 3 });
                        sheetdata.AppendChild(TitleRow);
                        Row TitleRow2 = new Row();
                        TitleRow2.Append(new Cell() { DataType = CellValues.String, CellValue = new CellValue(sn.ToString()), StyleIndex = 2 });
                        sheetdata.AppendChild(TitleRow2);
                        //
                        MergeCells mergeCells = new MergeCells();
                        mergeCells.Append(new MergeCell() { Reference = new StringValue("A1:H1") });
                        mergeCells.Append(new MergeCell() { Reference = new StringValue("A2:H2") });
                        worksheetpart.Worksheet.InsertAfter(mergeCells, worksheetpart.Worksheet.Elements<SheetData>().First());

                        //

                        Row headerRow = new Row();
                        headerRow.Append(new Cell() { DataType = CellValues.String, CellValue = new CellValue("Loom Type"), StyleIndex = 1 });
                        headerRow.Append(new Cell() { DataType = CellValues.String, CellValue = new CellValue("Priority"), StyleIndex = 1 });
                        headerRow.Append(new Cell() { DataType = CellValues.String, CellValue = new CellValue("Warp MO"), StyleIndex = 1 });
                        headerRow.Append(new Cell() { DataType = CellValues.String, CellValue = new CellValue("Warp Style"), StyleIndex = 1 });
                        headerRow.Append(new Cell() { DataType = CellValues.String, CellValue = new CellValue("Tickets"), StyleIndex = 1 });
                        headerRow.Append(new Cell() { DataType = CellValues.String, CellValue = new CellValue("Priorities"), StyleIndex = 1 });
                        headerRow.Append(new Cell() { DataType = CellValues.String, CellValue = new CellValue("Due Date"), StyleIndex = 1 });
                        headerRow.Append(new Cell() { DataType = CellValues.String, CellValue = new CellValue("Notes"), StyleIndex = 1 });
                        //headerRow.Append(Constructcell("Priority",CellValues.SharedString), 
                        //                Constructcell("Warp MO",CellValues.SharedString),
                        //                Constructcell("Warp Style",CellValues.SharedString),
                        //                Constructcell("PCS",CellValues.SharedString),
                        //                Constructcell("Due Date", CellValues.SharedString)
                        //                );
                        sheetdata.AppendChild(headerRow);

                        foreach (var z in wp.Where(w => w.WarperID == v.warperid || w.WarperID == null).OrderBy(w => w.Priority))
                        {
                            Row mrow = new Row();
                            mrow.Append(new Cell() { DataType = CellValues.String, CellValue = new CellValue(z.SingleDouble), StyleIndex = 4 });
                            mrow.Append(new Cell() { DataType = CellValues.Number, CellValue = new CellValue(z.Priority.ToString()), StyleIndex = 4 });
                            mrow.Append(new Cell() { DataType = CellValues.String, CellValue = new CellValue(z.WarpMO), StyleIndex = 4 });
                            mrow.Append(new Cell() { DataType = CellValues.String, CellValue = new CellValue(z.WarpStyle), StyleIndex = 4 });
                            mrow.Append(new Cell() { DataType = CellValues.Number, CellValue = new CellValue(z.TotalTickets.ToString()), StyleIndex = 4 });
                            mrow.Append(new Cell() { DataType = CellValues.Number, CellValue = new CellValue(z.ChangesThisWarp.ToString()), StyleIndex = 4 });
                            mrow.Append(new Cell() { DataType = CellValues.String, CellValue = new CellValue(z.EarliestDueDate.ToShortDateString()), StyleIndex = 4 });
                            mrow.Append(new Cell() { DataType = CellValues.String, CellValue = new CellValue(z.Notes), StyleIndex = 4 });
                            //mrow.Append(Constructcell(z.Priority.ToString(), CellValues.Number),
                            //    Constructcell(z.WarpMO, CellValues.String),
                            //    Constructcell(z.WarpStyle, CellValues.String),
                            //    Constructcell(z.TotalTickets.ToString(), CellValues.Number),
                            //    Constructcell(z.EarliestDueDate.ToShortDateString(), CellValues.Date)
                            //    );
                            sheetdata.AppendChild(mrow);

                        }
                        sheetID++;
                    }




                }

            }

        }
        internal static Cell Constructcell (string value, CellValues dataType)
        {
            return new Cell
            {
                CellValue = new CellValue(value),
                DataType = new EnumValue<CellValues>(dataType)
            };
        }
        internal static Cell Constructcell(string value, CellValues dataType, uint styleIndex=0)
        {
            return new Cell
            {
                CellValue = new CellValue(value),
                DataType = new EnumValue<CellValues>(dataType),
                StyleIndex = styleIndex
            };
        }

        private static Stylesheet GenerateStylesheet()
        {
            Stylesheet styleSheet = null;

            Fonts fonts = new Fonts(
                new Font( // Index 0 - default
                    new FontSize() { Val = 15 },
                    new Bold()

                ),
                new Font( // Index 1 - header
                    new FontSize() { Val = 22 },
                    new Bold(),
                    new Color() { Rgb = "FFFFFF" }

                ),
                  new Font( // Index 2 - header
                    new FontSize() { Val = 28 },
                    new Bold(),
                    new Underline(),
                    new Color() { Rgb = "FFFFFF" }

                ),
                  new Font(new FontSize() { Val = 12 })
                  );

            Fills fills = new Fills(
                    new Fill(new PatternFill() { PatternType = PatternValues.None }), // Index 0 - default
                    new Fill(new PatternFill() { PatternType = PatternValues.Gray125 }), // Index 1 - default
                    new Fill(new PatternFill(new ForegroundColor { Rgb = new HexBinaryValue() { Value = "66666666" } })
                    { PatternType = PatternValues.Solid }) // Index 2 - header
                );

            Borders borders = new Borders(
                    new Border(), // index 0 default
                    new Border( // index 1 black border
                        new LeftBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                        new RightBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                        new TopBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                        new BottomBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                        new DiagonalBorder())
                );

            CellFormats cellFormats = new CellFormats(
                    new CellFormat(), // default
                    new CellFormat { FontId = 0, FillId = 0, BorderId = 1, ApplyBorder = true, Alignment = new Alignment() { Horizontal = HorizontalAlignmentValues.Center } }, // body
                     new CellFormat { FontId = 1, FillId = 2, BorderId = 0, ApplyFill = true, Alignment = new Alignment() { Horizontal = HorizontalAlignmentValues.Center } }, // header
                    new CellFormat { FontId = 2, FillId = 2, BorderId = 0, ApplyFill = true, ApplyAlignment=true, Alignment=new Alignment() { Horizontal=HorizontalAlignmentValues.Center} }, // header2
                     new CellFormat { FontId = 3, FillId = 0, BorderId = 1, ApplyBorder = true, Alignment = new Alignment() { Horizontal = HorizontalAlignmentValues.Center } }
                    );

            styleSheet = new Stylesheet(fonts, fills, borders, cellFormats);

            return styleSheet;
        }
    }
}

//Excel.Application app = new Excel.Application();
//app.Visiable=false;
//Excel.Workbook workbook = app.Workbooks.Open("YourFile.xls",
//    Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
//    Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
//    Type.Missing, Type.Missing, Type.Missing, Type.Missing);


////if you want to get the second worksheet of the excel ,just like below:

//Excel.Worksheet worksheet = (Excel.Worksheet)workbook.Sheets[2];
//worksheet.Activate();
////set value to the cell of the worksheet.
//worksheet.Cells[rownum, colnum]="xx";
//workbook.Save();