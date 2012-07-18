using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PromptCloudNotes.Interfaces;
using PromptCloudNotes.Model;

namespace PromptCloudNotes.InMemoryRepo
{
    public class NotificationRepository : INotificationRepository
    {
        private IUserRepository _userRepository;
        private ITaskListRepository _listRepository;
        private INoteRepository _noteRepository;
        private int _notificationId = 0;

        public NotificationRepository(IUserRepository userRepo, ITaskListRepository listRepo, INoteRepository noteRepo)
        {
            _userRepository = userRepo;
            _listRepository = listRepo;
            _noteRepository = noteRepo;
        }

        #region INotificationRepository Members

        public void CreateTaskListNotification(int userId, int listId, Notification notificationData)
        {
            notificationData.Id = ++_notificationId;
            
            var user = _userRepository.Get(userId);
            if (user.Notifications == null)
            {
                user.Notifications = new List<Notification>();
            }
            user.Notifications.Add(notificationData);

            notificationData.User = user;
            if (notificationData.Task == null)
            {
                // this can not be always done, because in delete notifications,
                // at this moment the repository does not have the list anymore
                notificationData.Task = _listRepository.Get(listId);
            }
        }

        public void CreateNoteNotification(int userId, int listId, int noteId, Notification notificationData)
        {
            notificationData.Id = ++_notificationId;

            var user = _userRepository.Get(userId);
            if (user.Notifications == null)
            {
                user.Notifications = new List<Notification>();
            }
            user.Notifications.Add(notificationData);

            notificationData.User = user;
            if (notificationData.Task == null)
            {
                // this can not be always done, because in delete notifications,
                // at this moment the repository does not have the list anymore
                notificationData.Task = _noteRepository.Get(listId, noteId);
            }
        }

        public IEnumerable<Notification> GetAll(int userId)
        {
            var user = _userRepository.Get(userId);
            return user.Notifications;
        }

        public Notification Get(int notificationId)
        {
            var users = _userRepository.GetAll();
            foreach (var user in users)
            {
                var notification = user.Notifications.FirstOrDefault(n => n.Id == notificationId);
                if (notification != null)
                {
                    return notification;
                }
            }
            return null;
        }

        public void Delete(int notificationId)
        {
            var users = _userRepository.GetAll();
            foreach (var user in users)
            {
                var notification = user.Notifications.FirstOrDefault(n => n.Id == notificationId);
                if (notification != null)
                {
                    user.Notifications.Remove(notification);
                }
            }
        }

        #endregion
    }
}
