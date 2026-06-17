using System;
using System.Collections.Generic;

namespace TechMoveAPI.Domain.Observers
{
    public class ServiceRequestSubject : ISubject
    {
        /*
            Author: Refactoring Guru
            URL: https://refactoring.guru/design-patterns/observer/csharp/example
            Date: [n.d]
            Date Accessed: 13 April 2026
        */
        private List<IObserver> _observers = new List<IObserver>();

        public int RequestId { get; set; }
        public string Description { get; set; }
        public decimal Cost { get; set; }
        public string Status { get; private set; }

        public void Attach(IObserver observer)
        {
            _observers.Add(observer);
        }

        public void Detach(IObserver observer)
        {
            _observers.Remove(observer);
        }

        public void Notify(string message)
        {
            foreach (var observer in _observers)
            {
                observer.Update(message);
            }
        }

        public void UpdateStatus(string status)
        {
            Status = status;
            Notify($"Service Request {RequestId} status changed to {Status}");
        }
    }
}