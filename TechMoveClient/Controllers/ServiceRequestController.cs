using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TechMoveClient.Models;
using TechMoveClient.Services.Api;
using TechMoveClient.ViewModels;

namespace TechMoveClient.Controllers
{
    public class ServiceRequestController : BaseController
    {
        private readonly ServiceRequestApiService _serviceRequestApi;
        private readonly ContractApiService _contractApi;

        public ServiceRequestController(
            ServiceRequestApiService serviceRequestApi,
            ContractApiService contractApi)
        {
            _serviceRequestApi = serviceRequestApi;
            _contractApi = contractApi;
        }

        /*
        Author: Microsoft Learn
        URL: https://learn.microsoft.com/en-us/aspnet/core/mvc/views/overview
        Date: [n.d]
        Date Accessed: 24 May 2026
        */

        // Load active contracts for dropdown selection
        private async Task LoadContracts()
        {
            // Attach JWT token to API services for authenticated requests
            AttachJwtToApiServices(_serviceRequestApi, _contractApi);

            var contracts = await _contractApi.GetAllAsync();

            ViewBag.Contracts = contracts
                .Where(c => c.Status == ContractStatus.Active)
                .Select(c => new SelectListItem
                {
                    Value = c.ContractId.ToString(),
                    Text = $"{c.ContractId} - {c.ServiceLevel} ({c.ClientName})"
                })
                .ToList();
        }

        /*
        Author: Microsoft Learn
        URL: https://learn.microsoft.com/en-us/aspnet/core/mvc/controllers/actions
        Date: [n.d]
        Date Accessed: 24 May 2026
        */

        // Index 
        public async Task<IActionResult> Index()
        {
            // Ensure user is authenticated before accessing data
            if (!IsAuthenticated())
                return RedirectToLogin();

            AttachJwtToApiServices(_serviceRequestApi, _contractApi);

            // Retrieve all service requests from API
            var requests = await _serviceRequestApi.GetAllAsync();

            // Map API data to view model
            var viewModel = requests.Select(r => new ServiceRequestViewModel
            {
                ServiceRequestId = r.ServiceRequestId,
                Description = r.Description,
                Cost = r.Cost,
                OriginalAmount = r.OriginalAmount,
                Currency = r.Currency,
                Status = r.Status,
                ContractId = r.ContractId,
                ClientName = r.ClientName,
                ServiceLevel = r.ServiceLevel
            }).ToList();

            return View(viewModel);
        }

        // CREATE: GET
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            if (!IsAuthenticated())
                return RedirectToLogin();

            await LoadContracts();

            return View();
        }

        /*
        Author: Microsoft Learn
        URL: https://learn.microsoft.com/en-us/aspnet/core/mvc/models/validation
        Date: [n.d]
        Date Accessed: 24 May 2026
        */

        // CREATE: POST
        [HttpPost]
        public async Task<IActionResult> Create(ServiceRequestCreateViewModel model)
        {
            if (!IsAuthenticated())
                return RedirectToLogin();

            AttachJwtToApiServices(_serviceRequestApi, _contractApi);

            await LoadContracts();

            if (!ModelState.IsValid)
                return View(model);

            // Validate selected contract exists
            var contract = await _contractApi.GetByIdAsync(model.ContractId);

            if (contract == null)
            {
                ModelState.AddModelError("", "Selected contract does not exist.");
                return View(model);
            }

            // Map ViewModel to DTO for API request
            var dto = new ServiceRequestCreateDto
            {
                Description = model.Description,
                OriginalAmount = model.Amount,
                Currency = model.Currency,
                ContractId = model.ContractId
            };

            // Send create request to API
            await _serviceRequestApi.CreateAsync(dto);

            return RedirectToAction("Index");
        }

        // Update service request status [HttpGet]
        public async Task<IActionResult> UpdateStatus(int id)
        {
            if (!IsAuthenticated())
                return RedirectToLogin();

            AttachJwtToApiServices(_serviceRequestApi, _contractApi);

            var request = await _serviceRequestApi.GetByIdAsync(id);

            if (request == null)
                return NotFound();

            return View(request);
        }


        // Update service request status [HttpPost]
        [HttpPost]
        public async Task<IActionResult> UpdateStatus(int id, ServiceRequestStatus status)
        {
            if (!IsAuthenticated())
                return RedirectToLogin();

            AttachJwtToApiServices(_serviceRequestApi, _contractApi);

            await _serviceRequestApi.UpdateStatusAsync(id, status);

            return RedirectToAction("Index");
        }

        // Delete service request
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            if (!IsAuthenticated())
                return RedirectToLogin();

            AttachJwtToApiServices(_serviceRequestApi, _contractApi);

            var request = await _serviceRequestApi.GetByIdAsync(id);

            if (request == null)
                return NotFound();

            // Only pending requests can be deleted
            if (request.Status != ServiceRequestStatus.Pending)
            {
                TempData["Error"] = "Only pending requests can be deleted.";
                return RedirectToAction("Index");
            }

            await _serviceRequestApi.DeleteAsync(id);

            return RedirectToAction("Index");
        }
    }
}