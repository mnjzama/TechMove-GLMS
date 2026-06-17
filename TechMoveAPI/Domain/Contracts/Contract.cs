using TechMoveAPI.Models;

namespace TechMoveAPI.Domain.Contracts
{
    public abstract class Contract
    {
        public int ContractId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public ContractStatus Status { get; set; }
        public string ServiceLevel { get; set; }

        public abstract bool Validate();

        public virtual string GetDetails()
        {
            // Basic details for all contracts
            return $"Contract {ContractId} | {ServiceLevel} | {Status}";
        }
    }
}