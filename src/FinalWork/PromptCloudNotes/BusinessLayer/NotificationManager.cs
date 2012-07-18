using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PromptCloudNotes.Interfaces;
using PromptCloudNotes.Model;

namespace PromptCloudNotes.BusinessLayer.Managers
{
    public class NotificationManager : INotificationManager
    {
        private INotificationRepository _repository;
        private INotificationProcessor _notifProcessor;

        public NotificationManager(INotificationRepository repo, INotificationProcessor proc)
        {
            _repository = repo;
            _notifProcessor = proc;
        }

        #region INotificationManager Members

        public void CreateTaskListNotification(int userId, int listId, Notification notificationData)
        {
            notificationData.At = DateTime.Now;
            _repository.CreateTaskListNotification(userId, listId, notificationData);
        }

        public void CreateNoteNotification(int userId, int listId, int noteId, Notification notificationData)
        {
            notificationData.At = DateTime.Now;
            _repository.CreateNoteNotification(userId, listId, noteId, notificationData);
        }

        public IEnumerable<Notification> GetAllNotifications(int userId)
        {
            return _repository.GetAll(userId);
        }

        public Notification GetNotification(int notificationId)
        {
            return _repository.Get(notificationId);
        }

        public void DeleteNotification(int notificationId)
        {
            _repository.Delete(notificationId);
        }

        #endregion
    }
}
