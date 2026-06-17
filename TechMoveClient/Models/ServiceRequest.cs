using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechMoveClient.Models
{
    public class ServiceRequest
    {
        public int ServiceRequestId { get; set; }
        public string Description { get; set; }
        public decimal Cost { get; set; }
        public ServiceRequestStatus Status { get; set; }
        public int ContractId { get; set; }
        public decimal OriginalAmount { get; set; }
        public string Currency { get; set; }
    }

    public enum ServiceRequestStatus
    {
        Pending,
        InProgress,
        Completed
    }
}