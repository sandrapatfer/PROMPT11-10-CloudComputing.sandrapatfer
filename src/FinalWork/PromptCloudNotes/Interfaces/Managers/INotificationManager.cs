using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PromptCloudNotes.Model;

namespace PromptCloudNotes.Interfaces
{
    public interface INotificationManager
    {
        void CreateTaskListNotification(int userId, int listId, Notification notificationData);

        void CreateNoteNotification(int userId, int listId, int noteId, Notification notificationData);

        IEnumerable<Notification> GetAllNotifications(int userId);

        Notification GetNotification(int notificationId);

        void DeleteNotification(int notificationId);
    }
}
