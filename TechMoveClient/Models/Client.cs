using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TechMoveClient.Models
{
    public class Client
    {
        public int ClientId { get; set; }
        public string Name { get; set; }
        public string ContactDetails { get; set; }
        public string Region { get; set; }
        public bool HasContracts { get; set; }
    }
}