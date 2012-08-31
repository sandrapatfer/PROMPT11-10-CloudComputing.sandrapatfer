using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PromptCloudNotes.Interfaces;
using PromptCloudNotes.Interfaces.Managers;
using PromptCloudNotes.Interfaces.Repositories;
using PromptCloudNotes.Model;
using PromptCloudNotes.Interfaces.Queues;

namespace PromptCloudNotes.BusinessLayer.Managers
{
    public class NotificationManager : INotificationManager
    {
        private INotificationQueue _queue;
        private INotificationRepository _repository;
        private INotificationProcessor _onlineProcessor;

        public NotificationManager(INotificationQueue queue, INotificationRepository repo, INotificationProcessor proc)
        {
            _queue = queue;
            _repository = repo;
            _onlineProcessor = proc;
        }

        #region INotificationManager Members

        public void CreateNotification(string userId, Notification notificationData)
        {
            // dont notify the user that made the change
            if (userId == notificationData.User.UniqueId)
            {
                return;
            }

            notificationData.At = DateTime.Now;

            if (!_onlineProcessor.Send(notificationData))
            {
                if (ProcessToEmail(notificationData))
                {
                    _queue.SendMessage(notificationData);
                }
                if (ProcessToRepository(notificationData))
                {
                    _repository.Create(notificationData);
                }
            }
        }

        public IEnumerable<Notification> GetAllNotifications(string userId)
        {
            return _repository.GetAll(userId);
        }

        #endregion

        private bool ProcessToEmail(Notification notificationData)
        {
            // TODO read notifications configurations for user

            return notificationData.Type == Notification.NotificationType.Share;
        }

        private bool ProcessToRepository(Notification notificationData)
        {
            // saving all notifications in database when the user is not online
            return true;
        }
    }
}
