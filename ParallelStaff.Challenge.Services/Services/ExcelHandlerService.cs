using ClosedXML.Excel;
using ParallelStaff.Challenge.Domain.Entities;
using ParallelStaff.Challenge.Interfaces.IServices;

namespace ParallelStaff.Challenge.Services.Services
{
    public class ExcelHandlerService : IExcelHandlerService
    {
        private readonly XLWorkbook _workBook = new();
        private readonly IXLWorksheet _worksheet;
        private int _currentRow = 2;
        private readonly Dictionary<int, XLColor> _colors = new() 
        { 
            { 0, XLColor.FromHtml("#fafabc") }, 
            { 1, XLColor.FromHtml("4a86e8") }, 
            { 2, XLColor.FromHtml("#cbf1ee") } 
        };
        public ExcelHandlerService()
        {
            _worksheet = _workBook.AddWorksheet("new sheet");
            CreateHeaders();
            PaintHeadersBackGround();
        }

        
        private void CreateHeaders()
        {
            CustomizeHeader(_worksheet.Cell(1, 1), "Row Number");
            CustomizeHeader(_worksheet.Cell(1, 2), "Data Retrival Type");
            CustomizeHeader(_worksheet.Cell(1, 3), "ISBN");
            CustomizeHeader(_worksheet.Cell(1, 4), "Title");
            CustomizeHeader(_worksheet.Cell(1, 5), "Subtitle");
            CustomizeHeader(_worksheet.Cell(1, 6), "Author name(s)");
            CustomizeHeader(_worksheet.Cell(1, 7), "Number of pages");
            CustomizeHeader(_worksheet.Cell(1, 8), "Publish Date");
        }
        private void PaintHeadersBackGround()
        {
            var range = _worksheet.Range("A1:H1");
            var color = XLColor.FromHtml("#cccccc");
            range.Style.Fill.SetBackgroundColor(color);
        }

        private void CustomizeHeader(IXLCell cell, string headerName)
        {
            cell.Value = headerName;
            cell.Style.Font.Bold = true;
        }
        public void WriteRows(List<Book> books, int fileRowNumber)
        {
            var startRow = _currentRow;
            books.ForEach(book =>
            {
                var pages = book.Number_of_Pages > 0 ? book.Number_of_Pages.ToString() : "N/A";
                var subtitle = !string.IsNullOrEmpty(book.SubTitle) ? book.SubTitle : "N/A";
                var authorsNamesList = book.Authors.Select(x => x.Name).ToList();
                var authorsNames = string.Join("; ", authorsNamesList);
                _worksheet.Cell(_currentRow, 1).Value = fileRowNumber;
                _worksheet.Cell(_currentRow, 2).Value = book.RetrievalType.ToString();
                _worksheet.Cell(_currentRow, 3).Value = book.ISBN;
                _worksheet.Cell(_currentRow, 4).Value = book.Title;
                _worksheet.Cell(_currentRow, 5).Value = subtitle;
                _worksheet.Cell(_currentRow, 6).Value = authorsNames;
                _worksheet.Cell(_currentRow, 7).Value = pages;
                _worksheet.Cell(_currentRow, 8).Value = book.Publish_Date;                
                _currentRow++;
            });
            PaintValuesBackGround(startRow, fileRowNumber);
        }

        private void PaintValuesBackGround(int startRow, int fileRowNumber)
        {
            var endRow = _currentRow - 1;
            var range = _worksheet.Range($"A{startRow}:H{endRow}");
            var colorNumber = fileRowNumber % 3;            
            range.Style.Fill.SetBackgroundColor(_colors[colorNumber]);

        }        

        public void Save()
        {
            SetBorders();
            IncreaseColumnsWidth();
            var path = Directory.GetCurrentDirectory();
            _workBook.SaveAs($"{path}/books.xlsx");
            _workBook.Dispose();
        }

        private void IncreaseColumnsWidth()
        {
            _worksheet.Column(1).Width = 20;
            _worksheet.Column(2).Width = 20;
            _worksheet.Column(3).Width = 20;
            _worksheet.Column(4).AdjustToContents();
            _worksheet.Column(5).AdjustToContents();
            _worksheet.Column(6).AdjustToContents();
            _worksheet.Column(7).Width = 20;
            _worksheet.Column(8).Width = 20;
        }        

        private void SetBorders()
        {
            var lastRow = _currentRow - 1;
            var range = _worksheet.Range($"A1:H{lastRow}");
            range.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            range.Style.Border.InsideBorder = XLBorderStyleValues.Thin;            
        }
    }
}
