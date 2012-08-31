using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PromptCloudNotes.Model;

namespace PromptCloudNotes.Interfaces.Queues
{
    public interface INotificationQueue : IQueue<Notification>
    {}
}
