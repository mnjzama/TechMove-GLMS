using System.Text;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TechMoveClient.Models;
using TechMoveClient.ViewModels;

/*
Author: PROG7311-2026-EMWVL (Lecturer Repository)
URL: https://github.com/PROG7311-2026-EMWVL/MathAPIClient
Date: [n.d]
Date Accessed: 14 May 2026
*/
namespace TechMoveClient.Controllers
{
    public class AuthController : Controller
    {
        private static HttpClient? httpClient;

        public AuthController(IConfiguration configuration)
        {
            // Create a single shared HttpClient instance for API communication
            if (httpClient == null)
            {
                var baseUrl = configuration["ApiSettings:BaseUrl"];

                if (string.IsNullOrWhiteSpace(baseUrl))
                {
                    throw new InvalidOperationException(
                        "ApiSettings:BaseUrl is missing."
                    );
                }

                httpClient = new HttpClient
                {
                    BaseAddress = new Uri(baseUrl)
                };
            }
        }

        /*
        Author: Microsoft Learn
        URL: https://learn.microsoft.com/en-us/aspnet/core/fundamentals/http-requests
        Date: [n.d]
        Date Accessed: 14 May 2026
        */

        // GET: Register page
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        /*
        Author: Microsoft Learn
        URL: https://learn.microsoft.com/en-us/aspnet/core/fundamentals/http-requests
        Date: [n.d]
        Date Accessed: 14 May 2026
        */

        // POST: Register user via API
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel login)
        {
            // Serialize form data into JSON for API request
            var jsonContent = new StringContent(
                JsonConvert.SerializeObject(login),
                Encoding.UTF8,
                "application/json"
            );

            // Send registration request to API
            var response = await httpClient!.PostAsync("api/auth/register", jsonContent);

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Result = await response.Content.ReadAsStringAsync();
                return View();
            }

            // Read and deserialize API response
            var body = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<AuthResponse>(body);

            if (result == null)
            {
                ViewBag.Result = "Invalid server response.";
                return View();
            }

            // Store authenticated user session data
            HttpContext.Session.SetString("UserEmail", result.Email);
            HttpContext.Session.SetString("JwtToken", result.Token);

            // Redirect user after successful registration
            return RedirectToAction("Index", "Home");
        }

        // GET: Login page
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        /*
        Author: Microsoft Learn
        URL: https://learn.microsoft.com/en-us/aspnet/core/fundamentals/http-requests
        Date: [n.d]
        Date Accessed: 14 May 2026
        */

        // POST: Login user via API
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel login)
        {
            // Serialize login request into JSON format
            var jsonContent = new StringContent(
                JsonConvert.SerializeObject(login),
                Encoding.UTF8,
                "application/json"
            );

            // Send login request to API
            var response = await httpClient!.PostAsync("api/auth/login", jsonContent);

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Result = await response.Content.ReadAsStringAsync();
                return View();
            }

            // Read and deserialize API response
            var body = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<AuthResponse>(body);

            if (result == null)
            {
                ViewBag.Result = "Invalid server response.";
                return View();
            }

            // Store user session data after successful login
            HttpContext.Session.SetString("UserEmail", result.Email);
            HttpContext.Session.SetString("JwtToken", result.Token);

            return RedirectToAction("Index", "Home");
        }

        // GET: Logout user
        [HttpGet]
        public IActionResult Logout()
        {
            // Clear user session data
            HttpContext.Session.Clear();

            // Remove authorization header from shared HttpClient
            if (httpClient != null)
            {
                httpClient.DefaultRequestHeaders.Authorization = null;
            }

            // Prevent cached authentication pages
            Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
            Response.Headers["Pragma"] = "no-cache";
            Response.Headers["Expires"] = "0";

            return RedirectToAction("Login", "Auth");
        }
    }
}