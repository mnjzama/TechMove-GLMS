using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechMoveAPI.Data;
using TechMoveAPI.Dto;
using TechMoveAPI.Models;
using TechMoveAPI.Services;

/*
Author: PROG7311-2026-EMWVL (Lecturer Repository)
URL: https://github.com/PROG7311-2026-EMWVL/MathAPI
Date: [n.d]
Date Accessed: 14 May 2026
*/
namespace TechMoveAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ServiceRequestsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ServiceRequestService _serviceRequest;
        private readonly ContractService _contractService;

        public ServiceRequestsController(AppDbContext context, ServiceRequestService serviceRequest, ContractService contractService)
        {
            _context = context;
            _serviceRequest = serviceRequest;
            _contractService = contractService;
        }

        /*
        Author: Microsoft Learn
        URL: https://learn.microsoft.com/en-us/ef/core/querying/related-data/eager
        Date: [n.d]
        Date Accessed: 14 May 2026
        */

        // GET: api/servicerequests
        [HttpGet]
        public IActionResult GetAll()
        {
            // Retrieve all service requests with related contract and client data
            var requests = _context.ServiceRequests
                .Include(r => r.Contract)
                .ThenInclude(c => c.Client)
                .Select(r => new ServiceRequestDto
                {
                    ServiceRequestId = r.ServiceRequestId,
                    Description = r.Description,
                    Cost = r.Cost,
                    OriginalAmount = r.OriginalAmount,
                    Currency = r.Currency,
                    Status = r.Status,
                    ContractId = r.ContractId,
                    ClientName = r.Contract.Client.Name,
                    ServiceLevel = r.Contract.ServiceLevel
                })
                .ToList();

            return Ok(requests);
        }

        /*
        Author: Microsoft Learn
        URL: https://learn.microsoft.com/en-us/ef/core/querying/
        Date: [n.d]
        Date Accessed: 14 May 2026
        */

        // GET: api/servicerequests/{id}
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            // Retrieve a specific service request by ID
            var request = _context.ServiceRequests
                .Include(r => r.Contract)
                .ThenInclude(c => c.Client)
                .Where(r => r.ServiceRequestId == id)
                .Select(r => new ServiceRequestDto
                {
                    ServiceRequestId = r.ServiceRequestId,
                    Description = r.Description,
                    Cost = r.Cost,
                    OriginalAmount = r.OriginalAmount,
                    Currency = r.Currency,
                    Status = r.Status,
                    ContractId = r.ContractId,
                    ClientName = r.Contract.Client.Name,
                    ServiceLevel = r.Contract.ServiceLevel
                })
                .FirstOrDefault();

            // Return 404 if the request does not exist
            if (request == null)
                return NotFound();

            return Ok(request);
        }

        /*
        Author: Microsoft Learn
        URL: https://learn.microsoft.com/en-us/dotnet/api/system.net.http.httpclient
        Date: [n.d]
        Date Accessed: 14 May 2026
        */

        // POST: api/servicerequests
        [HttpPost]
        public async Task<IActionResult> Create(CreateServiceRequestDto dto)
        {
            // Validate incoming request data
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Retrieve the linked contract
            var contract = _context.Contracts
                .FirstOrDefault(c => c.ContractId == dto.ContractId);

            if (contract == null)
                return BadRequest("Contract not found");

            // Ensure service requests can only be created for active contracts
            if (!_contractService.CanCreateServiceRequest(contract))
                return BadRequest("Contract is not active");

            // Convert the request amount into the system currency
            var convertedCost = await _serviceRequest.CalculateCostAsync(
                dto.Currency,
                dto.OriginalAmount
            );

            // Create the service request entity
            var entity = new ServiceRequest
            {
                Description = dto.Description,
                Cost = convertedCost,
                OriginalAmount = dto.OriginalAmount,
                Currency = dto.Currency,

                // New requests are created with Pending status by default
                Status = ServiceRequestStatus.Pending,
                ContractId = dto.ContractId
            };

            _context.ServiceRequests.Add(entity);
            _context.SaveChanges();

            // Prepare response DTO
            var resultDto = new ServiceRequestDto
            {
                ServiceRequestId = entity.ServiceRequestId,
                Description = entity.Description,
                Cost = entity.Cost,
                OriginalAmount = entity.OriginalAmount,
                Currency = entity.Currency,
                Status = entity.Status,
                ContractId = entity.ContractId
            };

            return CreatedAtAction(nameof(GetById), new { id = entity.ServiceRequestId }, resultDto);
        }

        /*
        Author: Microsoft Learn
        URL: https://learn.microsoft.com/en-us/aspnet/core/web-api/
        Date: [n.d]
        Date Accessed: 14 May 2026
        */

        // PATCH: api/servicerequests/{id}/status
        [HttpPatch("{id}/status")]
        public IActionResult UpdateStatus(int id, [FromBody] ServiceRequestStatus status)
        {
            // Retrieve the service request including contract information
            var entity = _serviceRequest.GetByIdWithContract(id);

            if (entity == null)
                return NotFound();

            // Update the service request status
            _serviceRequest.UpdateStatus(entity, status);

            _context.SaveChanges();

            return Ok();
        }

        /*
        Author: Microsoft Learn
        URL: https://learn.microsoft.com/en-us/ef/core/saving/basic
        Date: [n.d]
        Date Accessed: 14 May 2026
        */

        // DELETE: api/servicerequests/{id}
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            // Retrieve the service request by ID
            var request = _context.ServiceRequests.Find(id);

            if (request == null)
                return NotFound();

            // Only pending requests can be deleted
            if (!_serviceRequest.CanDelete(request))
                return BadRequest("Only pending requests can be deleted");

            _context.ServiceRequests.Remove(request);
            _context.SaveChanges();

            return NoContent();
        }
    }
}