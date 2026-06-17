using Microsoft.AspNetCore.Mvc;

/*
Author: PROG7311-2026-EMWVL (Lecturer Repository)
URL: https://github.com/PROG7311-2026-EMWVL/MathAPI
Date: [n.d]
Date Accessed: 16 May 2026
*/
namespace TechMoveAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HealthController : ControllerBase
    {
        // GET: api/health
        [HttpGet]
        public IActionResult Get()
        {
            // Simple health check endpoint to confirm the API is running
            return Ok(new
            {
                status = "ok",
                service = "TechMoveAPI"
            });
        }
    }
}