using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PromptCloudNotes.Interfaces.Queues;
using PromptCloudNotes.Model;

namespace PromptCloudNotes.AzureRepo
{
    using Model;

    public class NotificationQueue : AzureQueue<Notification>, INotificationQueue
    {
        private const string QUEUE_NAME = "NotificationQueue";

        public NotificationQueue()
            : base(QUEUE_NAME)
        { }

        public new Notification GetMessage()
        {
            return base.GetMessage();
        }

        public void SendMessage(Notification newEntity)
        {
            newEntity.Id = Guid.NewGuid().ToString();
            PutMessage(newEntity);
        }

        public void DeleteMessage(Notification newEntity)
        {
            throw new NotImplementedException();
        }
    }
}
