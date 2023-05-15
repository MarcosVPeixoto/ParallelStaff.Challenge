using System.ComponentModel;

namespace ParallelStaff.Challenge.Domain.Enums
{
    public enum RetrievalType
    {
        [Description("Server")]
        Server = 1,
        [Description("Cache")]
        Cache = 2
    }
}
