/*
    Author: Refactoring Guru
    URL: https://refactoring.guru/design-patterns/observer/csharp/example
    Date: [n.d]
    Date Accessed: 13 April 2026
*/
namespace TechMoveAPI.Domain.Observers
{
    public interface IObserver
    {
        // Method to receive updates from the subject
        void Update(string message);
    }
}