using TechMoveAPI.Models;
namespace TechMoveAPI.Dto
{
    public class ServiceRequestDto
    {
        public int ServiceRequestId { get; set; }
        public string Description { get; set; }
        public decimal Cost { get; set; }
        public decimal OriginalAmount { get; set; }
        public string Currency { get; set; }
        public ServiceRequestStatus Status { get; set; }
        public int ContractId { get; set; }
        public string ClientName { get; set; }
        public string ServiceLevel { get; set; }
    }
}