namespace TechMoveClient.Domain.Contracts
{
    public class ExpressContract : Contract
    {
        public override bool Validate()
        {
            return (EndDate > StartDate) && ServiceLevel == "Express";
        }
    }
}