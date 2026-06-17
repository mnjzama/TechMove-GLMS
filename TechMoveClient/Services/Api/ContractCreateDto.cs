namespace TechMoveClient.Services.Api
{
    public class ContractCreateDto
    {
        public int ClientId { get; set; }
        public string ServiceLevel { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string AgreementFilePath { get; set; }
    }
}