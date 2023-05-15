using ParallelStaff.Challenge.Domain.Entities;

namespace ParallelStaff.Challenge.Interfaces.IServices
{
    public interface IOpenLibraryService
    {
        Task<Dictionary<string, Book>> GetBooks(string isbns);
    }
}
