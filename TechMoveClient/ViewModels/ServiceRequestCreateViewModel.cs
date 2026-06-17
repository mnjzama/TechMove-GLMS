using System.ComponentModel.DataAnnotations;

namespace TechMoveClient.ViewModels
{
    public class ServiceRequestCreateViewModel
    {
        public int ContractId { get; set; }
        
        [Required(ErrorMessage = "Description is required")]
        [StringLength(100)]
        public string Description { get; set; }

        [Required(ErrorMessage = "Amount is required")]
        [Range(1, 1000000, ErrorMessage = "Amount must be greater than 0")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Currency is required")]
        public string Currency { get; set; } // USD / EUR / ZAR
    }
}