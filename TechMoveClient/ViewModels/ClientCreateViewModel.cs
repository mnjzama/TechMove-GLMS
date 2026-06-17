using System.ComponentModel.DataAnnotations;

namespace TechMoveClient.ViewModels
{
    public class ClientCreateViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string ContactDetails { get; set; }

        [Required]
        public string Region { get; set; }
    }
}