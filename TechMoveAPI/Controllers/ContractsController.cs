using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using TechMoveAPI.Data;
using TechMoveAPI.Models;
using TechMoveAPI.Services;
using TechMoveAPI.Dto;

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
    public class ContractsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ContractService _contractService;

        public ContractsController(AppDbContext context, ContractService contractService)
        {
            _context = context;
            _contractService = contractService;
        }

        /*
        Author: Microsoft Learn
        URL: https://learn.microsoft.com/en-us/ef/core/querying/related-data/eager
        Date: [n.d]
        Date Accessed: 14 May 2026
        */

        // GET: api/contracts
        [HttpGet]
        public IActionResult GetAll(DateTime? startDate, DateTime? endDate, ContractStatus? status)
        {
            // Filter contracts using optional query parameters
            var contractsQuery = _contractService.FilterContracts(
                _context.Contracts.Include(c => c.Client),
                startDate,
                endDate,
                status
            );

            // Map contract entities to DTO objects
            var result = contractsQuery.Select(c => new ContractDto
            {
                ContractId = c.ContractId,
                ServiceLevel = c.ServiceLevel,
                Status = c.Status,
                StartDate = c.StartDate,
                EndDate = c.EndDate,
                ClientId = c.ClientId,
                ClientName = c.Client.Name,
                AgreementFilePath = c.AgreementFilePath
            }).ToList();

            return Ok(result);
        }

        /*
        Author: Microsoft Learn
        URL: https://learn.microsoft.com/en-us/ef/core/querying/
        Date: [n.d]
        Date Accessed: 14 May 2026
        */

        // GET: api/contracts/{id}
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            // Retrieve a specific contract including related client data
            var contract = _context.Contracts
                .Include(c => c.Client)
                .Where(c => c.ContractId == id)
                .Select(c => new ContractDto
                {
                    ContractId = c.ContractId,
                    ServiceLevel = c.ServiceLevel,
                    Status = c.Status,
                    StartDate = c.StartDate,
                    EndDate = c.EndDate,
                    ClientId = c.ClientId,
                    ClientName = c.Client.Name,
                    AgreementFilePath = c.AgreementFilePath
                })
                .FirstOrDefault();

            // Return 404 if the contract does not exist
            if (contract == null)
                return NotFound();

            return Ok(contract);
        }

        /*
        Author: Microsoft Learn
        URL: https://learn.microsoft.com/en-us/aspnet/core/web-api/
        Date: [n.d]
        Date Accessed: 14 May 2026
        */

        // POST: api/contracts
        [HttpPost]
        public IActionResult Create(CreateContractDto dto)
        {
            // Validate incoming request data
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Ensure contract end date is not before the start date
            if (!_contractService.IsValidContractDates(dto.StartDate, dto.EndDate))
                return BadRequest("End date cannot be before start date.");

            // Ensure the contract duration is at least one day
            if (!_contractService.HasValidDuration(dto.StartDate, dto.EndDate))
                return BadRequest("Contract duration must be at least 1 day.");

            // Create a new contract entity
            var contract = new ContractEntity
            {
                ClientId = dto.ClientId,
                ServiceLevel = dto.ServiceLevel,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                AgreementFilePath = dto.AgreementFilePath,

                // New contracts are created with Active status by default
                Status = ContractStatus.Active
            };

            _context.Contracts.Add(contract);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetById),
                new { id = contract.ContractId },
                new { contractId = contract.ContractId }
            );
        }

        /*
        Author: Microsoft Learn
        URL: https://learn.microsoft.com/en-us/aspnet/core/web-api/
        Date: [n.d]
        Date Accessed: 14 May 2026
        */

        // PATCH: api/contracts/{id}/status
        [HttpPatch("{id}/status")]
        public IActionResult UpdateStatus(int id, [FromBody] ContractStatus status)
        {
            // Retrieve the contract by ID
            var contract = _context.Contracts.FirstOrDefault(c => c.ContractId == id);

            if (contract == null)
                return NotFound();

            // Update the contract status using the service layer
            _contractService.UpdateStatus(contract, status);
            _context.SaveChanges();

            return Ok(contract);
        }

        /*
        Author: Microsoft Learn
        URL: https://learn.microsoft.com/en-us/ef/core/saving/basic
        Date: [n.d]
        Date Accessed: 14 May 2026
        */

        // DELETE: api/contracts/{id}
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            // Retrieve the contract from the database
            var contract = _context.Contracts.FirstOrDefault(c => c.ContractId == id);

            if (contract == null)
                return NotFound();

            // Only Draft contracts can be deleted
            if (!_contractService.CanDelete(contract))
                return BadRequest("Only Draft contracts can be deleted");

            // Delete the contract using the service layer
            _contractService.DeleteContract(contract);

            return NoContent();
        }
    }
}