using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TechMoveAPI.Models
{
    public class Client
    {
        [Key]
        public int ClientId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string ContactDetails { get; set; }

        [Required]
        public string Region { get; set; }

        // Navigation Property
        public ICollection<ContractEntity> Contracts { get; set; } = new List<ContractEntity>();
    }
}