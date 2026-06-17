using Microsoft.AspNetCore.Mvc;
using TechMoveClient.Services.Api;
using TechMoveClient.ViewModels;
using TechMoveClient.Models;

namespace TechMoveClient.Controllers
{
    public class ClientController : BaseController
    {
        private readonly ClientApiService _clientApi;

        public ClientController(ClientApiService clientApi)
        {
            _clientApi = clientApi;
        }

        /*
        Author: Microsoft Learn
        URL: https://learn.microsoft.com/en-us/aspnet/core/mvc/controllers/actions
        Date: [n.d]
        Date Accessed: 14 May 2026
        */

        // GET: Clients
        public async Task<IActionResult> Index()
        {
            // Ensure user is authenticated before accessing client data
            if (!IsAuthenticated())
                return RedirectToLogin();

            // Attach JWT token for authenticated API calls
            SetToken();

            // Retrieve all clients from API service
            var clients = await _clientApi.GetAllAsync();
            return View(clients);
        }

        // GET: Create client form
        public IActionResult Create()
        {
            // Ensure user is authenticated before allowing creation
            if (!IsAuthenticated())
                return RedirectToLogin();
 
            return View();
        }

        /*
        Author: Microsoft Learn
        URL: https://learn.microsoft.com/en-us/aspnet/core/mvc/models/validation
        Date: [n.d]
        Date Accessed: 14 May 2026
        */

        // POST: Create new client
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ClientCreateViewModel model)
        {
            // Ensure user is authenticated before processing request
            if (!IsAuthenticated())
                return RedirectToLogin();

            // Validate form input
            if (!ModelState.IsValid)
                return View(model);

            // Attach JWT token for API authentication
            SetToken();

            // Map ViewModel to domain model
            var client = new Client
            {
                Name = model.Name,
                ContactDetails = model.ContactDetails,
                Region = model.Region
            };

            // Send create request to API
            await _clientApi.CreateAsync(client);

            return RedirectToAction(nameof(Index));
        }

        // GET: Edit client form
        public async Task<IActionResult> Edit(int id)
        {
            // Ensure user is authenticated before accessing edit functionality
            if (!IsAuthenticated())
                return RedirectToLogin();

            SetToken();

            // Retrieve client details from API
            var client = await _clientApi.GetByIdAsync(id);

            if (client == null)
                return NotFound();

            // Map API model to ViewModel
            var model = new ClientEditViewModel
            {
                ClientId = client.ClientId,
                Name = client.Name,
                ContactDetails = client.ContactDetails,
                Region = client.Region
            };

            return View(model);
        }


        // POST: Edit client
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ClientEditViewModel model)
        {
            // Ensure user is authenticated before updating data
            if (!IsAuthenticated())
                return RedirectToLogin();

            // Validate model state
            if (!ModelState.IsValid)
                return View(model);

            SetToken();

            // Map updated data to domain model
            var client = new Client
            {
                ClientId = model.ClientId,
                Name = model.Name,
                ContactDetails = model.ContactDetails,
                Region = model.Region
            };

            // Send update request to API
            await _clientApi.UpdateAsync(model.ClientId, client);

            return RedirectToAction(nameof(Index));
        }

        // POST: Delete client
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            // Ensure user is authenticated before deleting
            if (!IsAuthenticated())
                return RedirectToLogin();

            SetToken();

            // Send delete request to API
            await _clientApi.DeleteAsync(id);

            return RedirectToAction(nameof(Index));
        }

        /*
        Author: Microsoft Learn
        URL: https://learn.microsoft.com/en-us/aspnet/core/fundamentals/http-context
        Date: [n.d]
        Date Accessed: 14 May 2026
        */

        // Helper method to attach JWT token to API service
        private void SetToken()
        {
            var token = HttpContext.Session.GetString("JwtToken");
            _clientApi.SetToken(token);
        }
    }
}