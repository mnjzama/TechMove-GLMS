namespace TechMoveAPI.Domain.Contracts
{
    public class ExpressContract : Contract
    {
        public override bool Validate()
        {
            // Validation for Express contracts where the end date must be after the start date and service level must be "Express"
            return (EndDate > StartDate) && ServiceLevel == "Express";
        }
    }
}