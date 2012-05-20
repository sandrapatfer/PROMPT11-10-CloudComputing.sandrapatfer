using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PromptCloudNotes.Model;

namespace PromptCloudNotes.Interfaces
{
    public interface INotificationRepository
    {
        void CreateTaskListNotification(int userId, int listId, Notification notificationData);

        void CreateNoteNotification(int userId, int listId, int noteId, Notification notificationData);

        IEnumerable<Notification> GetAll(int userId);
        Notification Get(int notificationId);
        void Delete(int notificationId);
    }
}
