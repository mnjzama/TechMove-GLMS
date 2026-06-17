using System;

namespace TechMoveClient.Domain.Observers
{
    public class NotificationService : IObserver
    {
        /*
        Author: Refactoring Guru
        URL: https://refactoring.guru/design-patterns/observer/csharp/example
        Date: [n.d]
        Date Accessed: 13 April 2026
        */
        public static List<string> Notifications = new();

        public void Update(string message)
        {
            Notifications.Add(message);
        }
    }
}