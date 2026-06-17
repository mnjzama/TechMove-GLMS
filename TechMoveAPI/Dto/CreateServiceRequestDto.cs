namespace TechMoveAPI.Dto
{
    public class CreateServiceRequestDto
    {
        public string Description { get; set; }
        public decimal OriginalAmount { get; set; }
        public string Currency { get; set; }
        public int ContractId { get; set; }
    }
}