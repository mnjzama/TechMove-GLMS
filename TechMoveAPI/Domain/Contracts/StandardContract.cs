namespace TechMoveAPI.Domain.Contracts
{
    public class StandardContract : Contract
    {
        public override bool Validate()
        {
            // Validation for Standard contracts where the end date must be after the start date 
            return (EndDate > StartDate);
        }
    }
}