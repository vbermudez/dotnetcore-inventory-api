using GoalSystemsAPI.Providers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoalSystemsAPI.Providers
{
    public interface IMessageRepository
    {
        event EventHandler<Notification> NotificationEvent;
        void Broadcast(Notification notification);
    }

    public class MessageRepository : IMessageRepository
    {
        public MessageRepository()
        { }

        public event EventHandler<Notification> NotificationEvent;

        public void Broadcast(Notification notification)
        {
            NotificationEvent?.Invoke(this, notification);
        }
    }
}
