using Microsoft.AspNetCore.Mvc;

/*
Author: PROG7311-2026-EMWVL (Lecturer Repository)
URL: https://github.com/PROG7311-2026-EMWVL/MathAPIClient
Date: [n.d]
Date Accessed: 16 May 2026
*/
namespace TechMoveClient.Controllers
{
    public class HealthController : Controller
    {
        [HttpGet("/health")]
        public IActionResult Index()
        {
            // Simple endpoint used to verify that the MVC client application is running
            return Content("ok");
        }
    }
}