using Microsoft.AspNetCore.Mvc;
using TechMoveAPI.Data;
using TechMoveAPI.Models;
using TechMoveAPI.Services;
using Microsoft.AspNetCore.Authorization;
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
    public class ClientsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ClientService _clientService;

        public ClientsController(AppDbContext context, ClientService clientService)
        {
            _context = context;
            _clientService = clientService;
        }

        /*
        Author: Microsoft Learn
        URL: https://learn.microsoft.com/en-us/ef/core/querying/
        Date: [n.d]
        Date Accessed: 14 May 2026
        */

        // GET: api/clients
        [HttpGet]
        public IActionResult GetAll()
        {
            // Retrieve all clients and map them to DTO objects
            var clients = _context.Clients
                .Select(c => new ClientDto
                {
                    ClientId = c.ClientId,
                    Name = c.Name,
                    ContactDetails = c.ContactDetails,
                    Region = c.Region,

                    // Check whether the client has any linked contracts
                    HasContracts = c.Contracts.Any()
                })
                .ToList();

            return Ok(clients);
        }

        /*
        Author: Microsoft Learn
        URL: https://learn.microsoft.com/en-us/ef/core/querying/
        Date: [n.d]
        Date Accessed: 14 May 2026
        */

        // GET: api/clients/{id}
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            // Retrieve a specific client by ID
            var client = _context.Clients
                .Where(c => c.ClientId == id)
                .Select(c => new ClientDto
                {
                    ClientId = c.ClientId,
                    Name = c.Name,
                    ContactDetails = c.ContactDetails,
                    Region = c.Region
                })
                .FirstOrDefault();

            // Return 404 if the client does not exist
            if (client == null)
                return NotFound();

            return Ok(client);
        }

        /*
        Author: Microsoft Learn
        URL: https://learn.microsoft.com/en-us/aspnet/core/web-api/
        Date: [n.d]
        Date Accessed: 14 May 2026
        */

        // POST: api/clients
        [HttpPost]
        public IActionResult Create(ClientDto dto)
        {
            // Validate incoming request data
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Create a new client entity from the DTO
            var client = new Client
            {
                Name = dto.Name,
                ContactDetails = dto.ContactDetails,
                Region = dto.Region
            };

            _context.Clients.Add(client);
            _context.SaveChanges();

            // Update DTO with generated database ID
            dto.ClientId = client.ClientId;

            return CreatedAtAction(nameof(GetById), new { id = client.ClientId }, dto);
        }

        /*
        Author: Microsoft Learn
        URL: https://learn.microsoft.com/en-us/ef/core/saving/basic
        Date: [n.d]
        Date Accessed: 14 May 2026
        */

        // PUT: api/clients/{id}
        [HttpPut("{id}")]
        public IActionResult Update(int id, ClientDto dto)
        {
            // Find the existing client record
            var client = _context.Clients.Find(id);

            if (client == null)
                return NotFound();

            // Update client details
            client.Name = dto.Name;
            client.ContactDetails = dto.ContactDetails;
            client.Region = dto.Region;

            _context.SaveChanges();

            return Ok(dto);
        }

        /*
        Author: Microsoft Learn
        URL: https://learn.microsoft.com/en-us/ef/core/saving/basic
        Date: [n.d]
        Date Accessed: 14 May 2026
        */

        // DELETE: api/clients/{id}
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            // Retrieve the client from the database
            var client = _context.Clients.FirstOrDefault(c => c.ClientId == id);

            // Return 404 if the client does not exist
            if (client == null)
                return NotFound();

            // Prevent deletion if the client still has contracts
            if (!_clientService.CanDelete(client))
                return BadRequest("Cannot delete client with existing contracts");

            // Delete the client using the service layer
            _clientService.DeleteClient(client);

            return NoContent();
        }
    }
}