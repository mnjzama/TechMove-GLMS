using System;

/*
    Author: Refactoring Guru
    URL: https://refactoring.guru/design-patterns/observer/csharp/example
    Date: [n.d]
    Date Accessed: 13 April 2026
*/
namespace TechMoveAPI.Domain.Observers
{

    public class DriverApp : IObserver
    {
        // Static list to store messages received by the driver app
        public static List<string> DriverMessages = new();

        // Method to receive updates from the subject
        public void Update(string message)
        {
            DriverMessages.Add(message);
        }
    }
}