using TechMoveClient.Models;

namespace TechMoveClient.ViewModels
{
    public class ServiceRequestViewModel
    {
        public int ServiceRequestId { get; set; }
        public string Description { get; set; }
        public decimal Cost { get; set; }
        public decimal OriginalAmount { get; set; }
        public ServiceRequestStatus Status { get; set; }
        public string Currency { get; set; }

        public int ContractId { get; set; }
        public string ClientName { get; set; }
        public string ServiceLevel { get; set; }
    }
}