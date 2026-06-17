using System.Collections.Generic;

namespace TechMoveClient.Domain.Observers
{
        /*
        Author: Refactoring Guru
        URL: https://refactoring.guru/design-patterns/observer/csharp/example
        Date: [n.d]
        Date Accessed: 13 April 2026
        */
    public interface ISubject
    {
        void Attach(IObserver observer);
        void Detach(IObserver observer);
        void Notify(string message);
    }
}