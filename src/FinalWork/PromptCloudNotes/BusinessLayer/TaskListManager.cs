using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PromptCloudNotes.Interfaces.Managers;
using PromptCloudNotes.Interfaces.Repositories;
using PromptCloudNotes.Model;
using Exceptions;

namespace PromptCloudNotes.BusinessLayer.Managers
{
    public class TaskListManager : ITaskListManager
    {
        private ITaskListRepository _repository;
        private IUserRepository _userRepository;
        private INotificationManager _noticationMgr;
        private IUserListsRepository _userListsRepo;
        private IListUsersRepository _listUsersRepo;

        public TaskListManager(ITaskListRepository repo, IUserRepository userRepo, INotificationManager notify,
            IUserListsRepository userListsRepo, IListUsersRepository listUsersRepo)
        {
            _repository = repo;
            _userRepository = userRepo;
            _noticationMgr = notify;
            _userListsRepo = userListsRepo;
            _listUsersRepo = listUsersRepo;
        }

        #region ITaskListManager Members

        public IEnumerable<TaskList> GetAllLists(string userId)
        {
            var visibleLists = _userListsRepo.GetAll(userId);
            return visibleLists.Select(l => _repository.Get(l.Creator.UniqueId, l.Id));
        }

        public void CreateTaskList(User creatorUser, TaskList listData)
        {
            listData.Creator = creatorUser;
            _repository.Create(listData);
            _userListsRepo.Create(creatorUser.UniqueId, listData.Id, creatorUser.UniqueId);
            _listUsersRepo.Create(listData.Id, creatorUser.UniqueId, creatorUser.Name);
        }

        public TaskList GetTaskList(string userId, string listId, string creatorId)
        {
            var listUsers = _listUsersRepo.GetAll(listId);
            if (!listUsers.Any(u => u.UniqueId == userId))
            {
                throw new NoPermissionException();
            }

            var list = _repository.Get(creatorId, listId);
            if (list == null)
            {
                throw new ObjectNotFoundException();
            }

            list.Users = listUsers.ToList();
            return list;
        }

        public void UpdateTaskList(string userId, string listId, string creatorId, TaskList listData)
        {
            var listUsers = _listUsersRepo.GetAll(listId);
            if (!listUsers.Any(u => u.UniqueId == userId))
            {
                throw new NoPermissionException();
            }

            _repository.Update(creatorId, listId, listData);

            foreach (var user in listUsers)
            {
                var notif = new Notification() { Task = listData, User = user, Type = Notification.NotificationType.Update };
                _noticationMgr.CreateTaskListNotification(user.UniqueId, listId, notif);
            }
        }

        public void DeleteTaskList(string userId, string listId, string creatorId)
        {
            // only let the creator user delete a list
            if (userId != creatorId)
            {
                throw new NoPermissionException();
            }

            var listUsers = _listUsersRepo.GetAll(listId);
            if (!listUsers.Any(u => u.UniqueId == userId))
            {
                throw new NoPermissionException();
            }

            var list = _repository.Get(creatorId, listId);
            if (list == null)
            {
                throw new ObjectNotFoundException();
            }

            _repository.Delete(creatorId, listId);

            foreach (var user in listUsers)
            {
                var notif = new Notification() { User = user, Task = list, Type = Notification.NotificationType.Delete };
                _noticationMgr.CreateTaskListNotification(user.UniqueId, listId, notif);
            }
        }

        public void ShareTaskList(string userId, string listId, string creatorId, string shareUserId)
        {
            var listUsers = _listUsersRepo.GetAll(listId);
            if (!listUsers.Any(u => u.UniqueId == userId))
            {
                throw new NoPermissionException();
            }

            var list = _repository.Get(creatorId, listId);
            if (list == null)
            {
                throw new ObjectNotFoundException();
            }

            _userListsRepo.Create(shareUserId, listId, creatorId);
            _listUsersRepo.Create(listId, shareUserId, ""); // TODO see later, really add name, need to check existance?

            var notif = new Notification() { Type = Notification.NotificationType.Share };
            _noticationMgr.CreateTaskListNotification(shareUserId, listId, notif);
        }

        #endregion
    }
}
