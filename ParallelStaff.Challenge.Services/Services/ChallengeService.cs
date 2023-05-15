using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParallelStaff.Challenge.Domain.Entities;
using ParallelStaff.Challenge.Domain.Enums;
using ParallelStaff.Challenge.Interfaces.IServices;

namespace ParallelStaff.Challenge.Services.Services
{
    public class ChallengeService : IChallengeService
    {
        private readonly IExcelHandlerService _excelHandlerService;
        private readonly IOpenLibraryService _openLibraryService;
        private readonly Dictionary<string, Book> _cachedBooks = new Dictionary<string, Book>();
        public ChallengeService(IExcelHandlerService excelHandlerService, IOpenLibraryService openLibraryService)
        {
            _excelHandlerService = excelHandlerService;
            _openLibraryService = openLibraryService;
        }

        public async Task<IActionResult> RetrieveBookInfoAndExportToCSV(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {                
                var error = new { Message = "File not found" };
                return new BadRequestObjectResult (error);
            }
            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                var rowNumber = 1;
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var isbns = line.Split(",").ToList();
                    var isbnsToSearch = RemoveCachedBooks(isbns);
                    var newBooks = await _openLibraryService.GetBooks(isbnsToSearch);
                    var orderedBooks = OrderBooks(isbns, newBooks);
                    _excelHandlerService.WriteRows(orderedBooks, rowNumber);
                    AddBooksToCache(newBooks);
                    rowNumber++;
                }
                _excelHandlerService.Save();
            }
            return new OkResult();
        }

        private string RemoveCachedBooks(List<string> isbns)
        {            
            var result = "";
            isbns.ForEach(isbn =>
            {
                if (!_cachedBooks.ContainsKey(isbn))
                    result += $"{isbn},";
            });
            return result;
        }

        private List<Book>OrderBooks(List<string> isbns, Dictionary<string, Book> newBooks)
        {
            var books = new List<Book>();
            isbns.ForEach(isbn =>
            {
                
                if (_cachedBooks.ContainsKey(isbn)) books.Add(_cachedBooks[isbn]);
                else books.Add(newBooks[isbn]);
            });
            return books;
        }
        private void AddBooksToCache(Dictionary<string, Book> newBooks)
        {
            foreach(var keyValue in newBooks)
            {
                var book = keyValue.Value;
                book.RetrievalType = RetrievalType.Cache;
                _cachedBooks.Add(keyValue.Key, keyValue.Value);
            }
        }
    }
}
