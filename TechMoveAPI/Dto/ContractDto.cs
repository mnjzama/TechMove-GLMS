using TechMoveAPI.Models;
namespace TechMoveAPI.Dto
{
    public class ContractDto
    {
        public int ContractId { get; set; }
        public string ServiceLevel { get; set; }
        public ContractStatus Status { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? AgreementFilePath { get; set; }
        public string ClientName { get; set; }
        public int ClientId { get; set; }
    }
}