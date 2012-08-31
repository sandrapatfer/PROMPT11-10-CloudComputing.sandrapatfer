using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PromptCloudNotes.Interfaces.Repositories;
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

        public void Create(Notification newEntity)
        {
            if (newEntity.Task is TaskList)
            {
                CreateTaskListNotification(newEntity.User.UniqueId, newEntity.Task.Id, newEntity);
            }
            else
            {
                CreateNoteNotification(newEntity.User.UniqueId, newEntity.Task.Id, newEntity);
            }
        }

        public IEnumerable<Notification> GetAll()
        {
            var users = _userRepository.GetAll();
            return users.Aggregate(new List<Notification>(), (l, u) => { l.AddRange(u.Notifications); return l; });
        }

        public IEnumerable<Notification> GetAll(string userId)
        {
            var users = _userRepository.GetAll(userId);
            return users.Aggregate(new List<Notification>(), (l, u) => { l.AddRange(u.Notifications); return l; });
        }

        public Notification Get(string partitionKey, string rowKey)
        {
            var list = GetAll(partitionKey);
            return list.FirstOrDefault(n => n.Id == rowKey);
        }

        public void Update(string partitionKey, string rowKey, Notification changedEntity)
        {
        }

        public void Delete(string partitionKey, string rowKey)
        {
            var n = Get(partitionKey, rowKey);
            n.User.Notifications.Remove(n);
        }

        public void CreateTaskListNotification(string userId, string listId, Notification notificationData)
        {
            notificationData.Id = (++_notificationId).ToString();

            var user = _userRepository.GetAll().FirstOrDefault(u => u.UniqueId == userId);
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
                notificationData.Task = _listRepository.Get(userId, listId);
            }
        }

        public void CreateNoteNotification(string userId, string noteId, Notification notificationData)
        {
            notificationData.Id = (++_notificationId).ToString();

            var user = _userRepository.GetAll().FirstOrDefault(u => u.UniqueId == userId);
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
                notificationData.Task = _noteRepository.Get(userId, noteId);
            }
        }
    }
}
