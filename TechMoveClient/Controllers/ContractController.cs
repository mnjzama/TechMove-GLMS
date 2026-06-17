using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TechMoveClient.Models;
using TechMoveClient.Services.Api;
using TechMoveClient.Services;
using TechMoveClient.ViewModels;

namespace TechMoveClient.Controllers
{
    public class ContractController : BaseController
    {
        private readonly ContractApiService _contractApi;
        private readonly ClientApiService _clientApi;
        private readonly FileService _fileService;
        private readonly SupabaseFileService _supabaseFileService;
        private readonly IConfiguration _configuration;

        public ContractController(
            ContractApiService contractApi,
            ClientApiService clientApi,
            FileService fileService,
            SupabaseFileService supabaseFileService,
            IConfiguration configuration)
        {
            _contractApi = contractApi;
            _clientApi = clientApi;
            _fileService = fileService;
            _supabaseFileService = supabaseFileService;
            _configuration = configuration;
        }

        /*
        Author: Microsoft Learn
        URL: https://learn.microsoft.com/en-us/aspnet/core/mvc/models/validation
        Date: [n.d]
        Date Accessed: 14 May 2026
        */

        // Load clients for dropdown selection
        private async Task LoadClientsAsync(ContractCreateViewModel model)
        {
            SetToken();

            var clients = await _clientApi.GetAllAsync();

            model.Clients = clients.Select(c => new SelectListItem
            {
                Value = c.ClientId.ToString(),
                Text = c.Name
            }).ToList();
        }

        /*
        Author: Microsoft Learn
        URL: https://learn.microsoft.com/en-us/aspnet/core/mvc/controllers/actions
        Date: [n.d]
        Date Accessed: 14 May 2026
        */

        // Index: Display all contracts with optional filtering
        public async Task<IActionResult> Index(DateTime? startDate, DateTime? endDate, ContractStatus? status)
        {
            // Ensure user is authenticated before accessing contracts
            if (!IsAuthenticated())
                return RedirectToLogin();

            SetToken();

            // Retrieve filtered contracts from API
            var contracts = await _contractApi.GetAllAsync(startDate, endDate, status);

            // Map API data to view model
            var viewModel = contracts.Select(c => new ContractIndexViewModel
            {
                ContractId = c.ContractId,
                ServiceLevel = c.ServiceLevel,
                Status = c.Status,
                StartDate = c.StartDate,
                EndDate = c.EndDate,
                AgreementFilePath = c.AgreementFilePath,
                ClientName = c.ClientName
            }).ToList();

            return View(viewModel);
        }

        // CREATE: GET
        public async Task<IActionResult> Create()
        {
            if (!IsAuthenticated())
                return RedirectToLogin();

            var model = new ContractCreateViewModel();

            await LoadClientsAsync(model);

            return View(model);
        }

        /*
        Author: Microsoft Learn
        URL: https://learn.microsoft.com/en-us/aspnet/core/mvc/models/validation
        Date: [n.d]
        Date Accessed: 14 May 2026
        */

        // CREATE: POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ContractCreateViewModel model)
        {
            if (!IsAuthenticated())
                return RedirectToLogin();

            await LoadClientsAsync(model);

            if (!ModelState.IsValid)
                return View(model);

            // Validate contract date logic
            if (model.EndDate < model.StartDate)
            {
                ModelState.AddModelError("", "End date cannot be before start date.");
                return View(model);
            }

            // Ensure contract duration is at least 1 day
            if ((model.EndDate - model.StartDate).TotalDays < 1)
            {
                ModelState.AddModelError("", "Contract duration must be at least 1 day.");
                return View(model);
            }

            string filePath = null;

            // Validate and upload agreement file
            if (model.AgreementFile != null)
            {
                if (!_fileService.IsValidPdf(model.AgreementFile))
                {
                    ModelState.AddModelError("AgreementFile", "Only PDF files are allowed.");
                    return View(model);
                }

                filePath = await _supabaseFileService.UploadContractAsync(model.AgreementFile);
            }

            // Map ViewModel to DTO for API request
            var dto = new ContractCreateDto
            {
                ClientId = model.ClientId,
                ServiceLevel = model.ServiceLevel,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                AgreementFilePath = filePath
            };

            // Send contract creation request to API
            await _contractApi.CreateAsync(dto);

            return RedirectToAction(nameof(Index));
        }

        // Update contract status
        [HttpPost]
        public async Task<IActionResult> UpdateStatus(int id, ContractStatus status)
        {
            if (!IsAuthenticated())
                return RedirectToLogin();

            SetToken();

            await _contractApi.UpdateStatusAsync(id, status);

            return RedirectToAction(nameof(Index));
        }

        /*
        Author: Microsoft Learn
        URL: https://learn.microsoft.com/en-us/aspnet/core/fundamentals/file-providers
        Date: [n.d]
        Date Accessed: 14 May 2026
        */

        // DELETE: POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            if (!IsAuthenticated())
                return RedirectToLogin();

            SetToken();

            var contract = await _contractApi.GetByIdAsync(id);

            if (contract == null)
                return NotFound();

            // Remove file from storage if it exists
            if (!string.IsNullOrEmpty(contract.AgreementFilePath))
            {
                if (contract.AgreementFilePath.StartsWith("http"))
                {
                    try
                    {
                        await _supabaseFileService.DeleteContractAsync(contract.AgreementFilePath);
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", "Failed to delete agreement file.");
                    }
                }
                else
                {
                    _fileService.DeleteFile(contract.AgreementFilePath);
                }
            }

            await _contractApi.DeleteAsync(id);

            return RedirectToAction(nameof(Index));
        }

        // Download agreement file
        public IActionResult DownloadAgreement(string path)
        {
            if (!IsAuthenticated())
                return RedirectToLogin();

            if (string.IsNullOrWhiteSpace(path))
                return NotFound();

            // If the path is a URL, redirect to it. Otherwise, serve the file from local storage
            return Redirect(path);
        }

        /*
        Author: Microsoft Learn
        URL: https://learn.microsoft.com/en-us/aspnet/core/fundamentals/http-context
        Date: [n.d]
        Date Accessed: 14 May 2026
        */

        // TOKEN HELPER
        private void SetToken()
        {
            var token = HttpContext.Session.GetString("JwtToken");
            _contractApi.SetToken(token);
            _clientApi.SetToken(token);
        }
    }
}