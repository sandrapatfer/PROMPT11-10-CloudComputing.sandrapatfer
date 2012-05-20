using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PromptCloudNotes.Interfaces;
using PromptCloudNotes.Model;
using Exceptions;

namespace BusinessLayer.Managers
{
    public class TaskListManager : ITaskListManager
    {
        private ITaskListRepository _repository;
        private INotificationManager _noticationMgr;

        public TaskListManager(ITaskListRepository repo, INotificationManager notify)
        {
            _repository = repo;
            _noticationMgr = notify;
        }

        #region ITaskListManager Members

        public IEnumerable<TaskList> GetAllLists(int userId)
        {
            return _repository.GetAll(userId);
        }

        public TaskList CreateTaskList(int userId, TaskList listData)
        {
            // TODO check if the name is not used?

            return _repository.Create(userId, listData);
        }

        public TaskList GetTaskList(int listId)
        {
            return _repository.Get(listId);
        }

        public void UpdateTaskList(int listId, TaskList listData)
        {
            var list = _repository.Get(listId); // TODO just get list shares?
            if (list == null)
            {
                throw new ObjectNotFoundException();
            }

            _repository.Update(listId, listData);
            
            foreach (var user in list.Users)
            {
                var notif = new Notification() { Task = list, User = user, Type = Notification.NotificationType.Update };
                _noticationMgr.CreateTaskListNotification(user.Id, listId, notif);
            }
        }

        public void DeleteTaskList(int listId)
        {
            var list = _repository.Get(listId);
            if (list == null)
            {
                throw new ObjectNotFoundException();
            }
            var users = list.Users;
            _repository.Delete(listId);

            foreach (var user in users)
            {
                var notif = new Notification() { User = user, Task = list, Type = Notification.NotificationType.Delete };
                _noticationMgr.CreateTaskListNotification(user.Id, listId, notif);
            }
        }

        public void ShareTaskList(int listId, int userId)
        {
            _repository.Share(listId, userId);

            var notif = new Notification() { Type = Notification.NotificationType.Share };
            _noticationMgr.CreateTaskListNotification(userId, listId, notif);
        }

        #endregion
    }
}
