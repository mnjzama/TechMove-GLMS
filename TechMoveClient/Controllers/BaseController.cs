using Microsoft.AspNetCore.Mvc;
using TechMoveClient.Services.Api;

namespace TechMoveClient.Controllers
{
    public class BaseController : Controller
    {
        /*
        Author: Microsoft Learn
        URL: https://domburf.medium.com/mocking-the-httpcontext-session-object-in-asp-net-core-2-0-7cda5ec134e
        Date: 30 April 2018
        Date Accessed: 16 April 2026
        */
        protected bool IsAuthenticated()
        {
            // Check if user email exists in session to determine authentication status
            return HttpContext.Session.GetString("UserEmail") != null;
        }

        /*
        Author: Khushbu Saini
        URL: https://www.c-sharpcorner.com/article/login-and-role-based-custom-authentication-in-asp-net-core-3-1/
        Date: 11 September 2020
        Date Accessed: 16 April 2026
        */
        protected IActionResult RedirectToLogin()
        {
            return RedirectToAction("Login", "Auth");
        }

        /*
        Author: Microsoft Learn
        URL: https://learn.microsoft.com/en-us/aspnet/core/fundamentals/http-requests
        Date: [n.d]
        Date Accessed: 15 May 2026
        */
        protected void AttachJwtToApiServices(params dynamic[] apiServices)
        {
            // Retrieve JWT token from session and attach to API services for authenticated requests
            var token = HttpContext.Session.GetString("JwtToken");

            // If no token is found, do not attempt to attach it
            if (string.IsNullOrEmpty(token))
                return;

            // Attach token to each API service that requires authentication
            foreach (var service in apiServices)
            {
                service.SetToken(token);
            }
        }
    }
}