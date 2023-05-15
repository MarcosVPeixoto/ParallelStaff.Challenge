using ParallelStaff.Challenge.Domain.Entities;

namespace ParallelStaff.Challenge.Interfaces.IServices
{
    public interface ICSVHandlerService
    {
        void WriteRows(List<Book> books, int fileRowNumber);
        void Save();
    }
}
