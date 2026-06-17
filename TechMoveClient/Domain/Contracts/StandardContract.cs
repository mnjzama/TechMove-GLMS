namespace TechMoveClient.Domain.Contracts
{
    public class StandardContract : Contract
    {
        public override bool Validate()
        {
            return (EndDate > StartDate);
        }
    }
}