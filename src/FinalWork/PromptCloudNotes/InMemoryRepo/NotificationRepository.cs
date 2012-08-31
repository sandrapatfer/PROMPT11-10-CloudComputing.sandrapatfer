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

        public void CreateTaskListNotification(string userId, string listId, Notification notificationData)
        {
            notificationData.Id = (++_notificationId).ToString();

            var memoryRepo = _userRepository as UserRepository;
            var user = memoryRepo.GetById(userId);
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
                notificationData.Task = _listRepository.GetWithUsers(listId);
            }
        }

        public void CreateNoteNotification(string userId, string noteId, Notification notificationData)
        {
            notificationData.Id = (++_notificationId).ToString();

            var memoryRepo = _userRepository as UserRepository;
            var user = memoryRepo.GetById(userId);
            if (user.Notifications == null)
            {
                user.Notifications = new List<Notification>();
            }
            user.Notifications.Add(notificationData);

            notificationData.User = user;
            if (notificationData.Task == null)
            {
                // TODO fix!
                // this can not be always done, because in delete notifications,
                // at this moment the repository does not have the list anymore
                notificationData.Task = _noteRepository.Get(noteId);
            }
        }

        public IEnumerable<Notification> GetAll(string userId)
        {
            var memoryRepo = _userRepository as UserRepository;
            var user = memoryRepo.GetById(userId);
            return user.Notifications;
        }

        public Notification Get(string notificationId)
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

        public void Delete(string notificationId)
        {
            var notif = Get(notificationId);
            if (notif != null)
            {
                notif.User.Notifications.Remove(notif);
            }
        }

        #endregion
    }
}
