using TechMoveAPI.Models;
using TechMoveAPI.Services;
namespace TechMoveAPI.Dto
{
    public class ClientDto
    {
        public int ClientId { get; set; }
        public string Name { get; set; }
        public string ContactDetails { get; set; }
        public string Region { get; set; }
        public bool HasContracts { get; set; }
    }
}