using Microsoft.AspNetCore.Mvc;
using ParallelStaff.Challenge.Interfaces.IServices;

namespace ParallelStaff.Challenge.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChallengeController : ControllerBase
    {
        private readonly IChallengeService _challengeService;

        public ChallengeController(IChallengeService challengeService)
        {
            _challengeService = challengeService;
        }

        [HttpPost]
        [Route("RetrieveBookInfoAndExportToCSV")] 
        
        public async Task<IActionResult> RetrieveBookInfoAndExportToCSV(IFormFile file)
        {            
            return await _challengeService.RetrieveBookInfoAndExportToCSV(file);
        }
    }
}
