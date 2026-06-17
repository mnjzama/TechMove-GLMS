using System.Collections.Generic;

/*
    Author: Refactoring Guru
    URL: https://refactoring.guru/design-patterns/observer/csharp/example
    Date: [n.d]
    Date Accessed: 13 April 2026
*/
namespace TechMoveAPI.Domain.Observers
{
    public interface ISubject
    {
        void Attach(IObserver observer); // Method to attach an observer to the subject
        void Detach(IObserver observer); // Method to detach an observer from the subject
        void Notify(string message); // Method to notify all observers about an event
    }
}