using System;

namespace TechMoveClient.Domain.Observers
{
     /*
        Author: Refactoring Guru
        URL: https://refactoring.guru/design-patterns/observer/csharp/example
        Date: [n.d]
        Date Accessed: 13 April 2026
        */
    public class DriverApp : IObserver
    {
        public static List<string> DriverMessages = new();

        public void Update(string message)
        {
            DriverMessages.Add(message);
        }
    }
}