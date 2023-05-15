using ParallelStaff.Challenge.Domain.Enums;

namespace ParallelStaff.Challenge.Domain.Entities
{
    public class Book
    {
        public RetrievalType RetrievalType { get; set; }
        public string ISBN { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public List<Author> Authors{ get; set; }
        public int Number_of_Pages { get; set; }
        public string Publish_Date { get; set; }
    }
}
