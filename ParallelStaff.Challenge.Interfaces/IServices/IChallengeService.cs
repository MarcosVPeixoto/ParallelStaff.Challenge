using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
namespace ParallelStaff.Challenge.Interfaces.IServices
{
    public interface IChallengeService
    {
        Task<IActionResult> RetrieveBookInfoAndExportToCSV(IFormFile file);
    }
}
