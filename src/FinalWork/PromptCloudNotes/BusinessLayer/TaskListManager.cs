using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PromptCloudNotes.Interfaces;
using PromptCloudNotes.Model;
using Exceptions;

namespace PromptCloudNotes.BusinessLayer.Managers
{
    public class TaskListManager : ITaskListManager
    {
        private ITaskListRepository _repository;
        private IUserRepository _userRepository;
        private INotificationManager _noticationMgr;

        public TaskListManager(ITaskListRepository repo, IUserRepository userRepo, INotificationManager notify)
        {
            _repository = repo;
            _userRepository = userRepo;
            _noticationMgr = notify;
        }

        #region ITaskListManager Members

        public IEnumerable<TaskList> GetAllLists(int userId)
        {
            return _repository.GetAll(userId);
        }

        public TaskList CreateTaskList(User user, TaskList listData)
        {
            return _repository.Create(user, listData);
        }

        public TaskList GetTaskList(int userId, int listId)
        {
            var list = _repository.GetWithUsers(listId);
            if (list == null)
            {
                throw new ObjectNotFoundException();
            }
            if (list.Users.Any(u => u.Id == userId))
            {
                return list;
            }
            // TODO exception for no permission?
            return null;
        }

        public void UpdateTaskList(int userId, int listId, TaskList listData)
        {
            var list = _repository.GetWithUsers(listId);
            if (list == null)
            {
                throw new ObjectNotFoundException();
            }
            if (list.Users.Any(u => u.Id == userId))
            {
                _repository.Update(listId, listData);

                foreach (var user in list.Users)
                {
                    var notif = new Notification() { Task = list, User = user, Type = Notification.NotificationType.Update };
                    _noticationMgr.CreateTaskListNotification(user.Id, listId, notif);
                }
            }
        }

        public void DeleteTaskList(int userId, int listId)
        {
            var list = _repository.Get(listId);
            if (list == null)
            {
                throw new ObjectNotFoundException();
            }
            // only let the creator user delete a list
            if (list.Creator.Id == userId)
            {
                var users = list.Users;
                _repository.Delete(listId);

                foreach (var user in users)
                {
                    var notif = new Notification() { User = user, Task = list, Type = Notification.NotificationType.Delete };
                    _noticationMgr.CreateTaskListNotification(user.Id, listId, notif);
                }
            }
        }

        public void ShareTaskList(int userId, int listId, int shareUserId)
        {
            var list = _repository.GetWithUsers(listId);
            if (list == null)
            {
                throw new ObjectNotFoundException();
            }
            if (list.Users.Any(u => u.Id == userId))
            {
                var user = _userRepository.Get(shareUserId);
                if (user == null)
                {
                    // TODO throw new exception
                    throw new InvalidOperationException();
                }

                _repository.Share(listId, user);

                var notif = new Notification() { Type = Notification.NotificationType.Share };
                _noticationMgr.CreateTaskListNotification(shareUserId, listId, notif);
            }
        }

        #endregion
    }
}
