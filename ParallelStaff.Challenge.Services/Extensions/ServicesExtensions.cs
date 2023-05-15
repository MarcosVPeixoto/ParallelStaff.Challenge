using Microsoft.Extensions.DependencyInjection;
using ParallelStaff.Challenge.Interfaces.IServices;
using ParallelStaff.Challenge.Services.Services;

namespace ParallelStaff.Challenge.Services.Extensions
{
    public static class ServicesExtensions
    {
        public static void AddServices(this IServiceCollection collection)
        {
            collection.AddScoped<IChallengeService, ChallengeService>();
            collection.AddScoped<IOpenLibraryService, OpenLibraryService>();
            collection.AddScoped<ICSVHandlerService, CSVHandlerService>();
        }
    }
}