using System.ComponentModel.DataAnnotations;

namespace TechMoveClient.ViewModels
{
    public class ClientEditViewModel
    {
        public int ClientId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string ContactDetails { get; set; }

        [Required]
        public string Region { get; set; }
    }
}