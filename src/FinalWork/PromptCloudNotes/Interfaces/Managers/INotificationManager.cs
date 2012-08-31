using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PromptCloudNotes.Model;

namespace PromptCloudNotes.Interfaces.Managers
{
    public interface INotificationManager
    {
        void CreateNotification(string userId, Notification notificationData);

        IEnumerable<Notification> GetAllNotifications(string userId);

    }
}
