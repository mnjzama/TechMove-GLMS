using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechMoveAPI.Models
{
    public class ServiceRequest
    {
        [Key]
        public int ServiceRequestId { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public decimal Cost { get; set; }

        [Required]
        public ServiceRequestStatus Status { get; set; }

        // FK
        public int ContractId { get; set; }

        [ForeignKey("ContractId")]
        public ContractEntity Contract { get; set; }

        public decimal OriginalAmount { get; set; } // user input (USD/EUR)

        public string Currency { get; set; }
    }

    public enum ServiceRequestStatus
    {
        Pending,
        InProgress,
        Completed
    }
}