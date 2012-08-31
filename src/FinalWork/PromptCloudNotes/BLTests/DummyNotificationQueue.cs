using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PromptCloudNotes.Interfaces.Queues;

namespace BLTests
{
    class DummyNotificationQueue : INotificationQueue
    {
        public PromptCloudNotes.Model.Notification GetMessage()
        {
            throw new NotImplementedException();
        }

        public void SendMessage(PromptCloudNotes.Model.Notification newEntity)
        {
        }

        public void DeleteMessage(PromptCloudNotes.Model.Notification newEntity)
        {
            throw new NotImplementedException();
        }
    }
}
