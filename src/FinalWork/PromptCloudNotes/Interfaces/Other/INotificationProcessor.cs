using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PromptCloudNotes.Model;

namespace PromptCloudNotes.Interfaces
{
    public interface INotificationProcessor
    {
        void Send(string userId, Notification notification);
    }
}
