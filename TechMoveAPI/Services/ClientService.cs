using TechMoveAPI.Models;
using TechMoveAPI.Data;

namespace TechMoveAPI.Services
{
    public class ClientService
    {
        private readonly AppDbContext _context;

        public ClientService(AppDbContext context)
        {
            _context = context;
        }

        /*
        Author: Microsoft Learn
        URL: https://learn.microsoft.com/en-us/dotnet/api/system.linq.enumerable.any
        Date: [n.d]
        Date Accessed: 15 April 2026
        */
        public bool CanDelete(Client client)
        {
            return _context.Contracts.Any(c => c.ClientId == client.ClientId) == false;
        }

        /*
        Author: Microsoft Learn
        URL: https://learn.microsoft.com/en-us/ef/core/saving/basic
        Date: [n.d]
        Date Accessed: 15 April 2026
        */
        public void DeleteClient(Client client)
        {
            _context.Clients.Remove(client);
            _context.SaveChanges();
        }
    }
}