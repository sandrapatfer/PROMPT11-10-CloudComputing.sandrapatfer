using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PromptCloudNotes.Interfaces;
using PromptCloudNotes.Interfaces.Managers;
using PromptCloudNotes.Interfaces.Repositories;
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

        public void CreateTaskListNotification(string userId, string listId, Notification notificationData)
        {
            notificationData.At = DateTime.Now;

            _notifProcessor.Send(userId, notificationData);
            _repository.Create(notificationData);
        }

        public void CreateNoteNotification(string userId, string noteId, Notification notificationData)
        {
            notificationData.At = DateTime.Now;

            _notifProcessor.Send(userId, notificationData);
            _repository.Create(notificationData);
        }

        public IEnumerable<Notification> GetAllNotifications(string userId)
        {
            return _repository.GetAll(userId);
        }

/*        public Notification GetNotification(string notificationId)
        {
            return _repository.Get(notificationId);
        }

        public void DeleteNotification(string notificationId)
        {
            _repository.Delete(notificationId);
        }*/

        #endregion
    }
}
