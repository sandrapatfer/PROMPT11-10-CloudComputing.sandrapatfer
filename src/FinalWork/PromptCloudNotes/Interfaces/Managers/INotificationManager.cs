using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PromptCloudNotes.Model;

namespace PromptCloudNotes.Interfaces.Managers
{
    public interface INotificationManager
    {
        void CreateTaskListNotification(string userId, string listId, Notification notificationData);

        void CreateNoteNotification(string userId, string noteId, Notification notificationData);

        IEnumerable<Notification> GetAllNotifications(string userId);

//        Notification GetNotification(string notificationId);

//        void DeleteNotification(string notificationId);
    }
}
