using Newtonsoft.Json;
using ParallelStaff.Challenge.Domain.Entities;
using ParallelStaff.Challenge.Domain.Enums;
using ParallelStaff.Challenge.Interfaces.IServices;

namespace ParallelStaff.Challenge.Services.Services
{
    public class OpenLibraryService : IOpenLibraryService
    {
        public async Task<Dictionary<string,Book>> GetBooks(string isbns)
        {
            var client = new HttpClient();
            var response = await client.GetAsync($"https://openlibrary.org/api/books?bibkeys=ISBN:{isbns}&jscmd=data&format=json");
            var content = await response.Content.ReadAsStringAsync();
            var books = JsonConvert.DeserializeObject<Dictionary<string,Book>>(content);
            books = TreatIsbnKeys(books);            
            return books;
        }       

        private void FillIsbnAndRetrievalTypeFields(Book book, string isbn)
        {
            book.ISBN = isbn;
            book.RetrievalType = RetrievalType.Server;            
        }

        private Dictionary<string, Book> TreatIsbnKeys(Dictionary<string, Book> books)
        {
            var result = new Dictionary<string, Book>();
            foreach ( var keyValuePair in books)
            {
                var key = keyValuePair.Key.Split(":");
                var isbn = key.Last();
                var book = keyValuePair.Value;
                result.Add(isbn, book);
                FillIsbnAndRetrievalTypeFields(book, isbn);
            }
            return result;
        }
    }
}
