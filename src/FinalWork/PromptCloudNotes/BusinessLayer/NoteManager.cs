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
    public class NoteManager : INoteManager
    {
        private INoteRepository _repository;
        private IUserRepository _userRepository;
        private ITaskListManager _taskListManager;
        private INotificationManager _noticationMgr;
        private IUserListsRepository _userNotesRepo;
        private IListUsersRepository _noteUsersRepo;

        public NoteManager(INoteRepository repo, IUserRepository userRepo, ITaskListManager mgr, INotificationManager notify,
            IUserListsRepository userNotesRepo, IListUsersRepository noteUsersRepo)
        {
            _repository = repo;
            _userRepository = userRepo;
            _taskListManager = mgr;
            _noticationMgr = notify;
            _userNotesRepo = userNotesRepo;
            _noteUsersRepo = noteUsersRepo;
        }

        #region INoteManager Members

        public void CreateNote(User user, string listId, string creatorId, Note noteData)
        {
            noteData.Creator = user;

            var list = _taskListManager.GetTaskList(user.UniqueId, listId, creatorId);
            noteData.ParentList = list;

            _repository.Create(noteData);
            // TODO create specific repo for note shares
            _userNotesRepo.Create(user.UniqueId, noteData.Id, listId);
            _noteUsersRepo.Create(noteData.Id, user.UniqueId, listId);

            foreach (var u in list.Users)
            {
                var notif = new Notification() { Task = noteData, User = u, Type = Notification.NotificationType.Insert };
                _noticationMgr.CreateNotification(user.UniqueId, notif);
            }
        }

        public IEnumerable<Note> GetAllNotes(string userId, string listId)
        {
            return _repository.GetAll(listId);
        }

        public IEnumerable<Note> GetAllNotes(string userId)
        {
            // TODO???
            return null;
        }

        public Note GetNote(string userId, string listId, string noteId)
        {
            var noteUsers = _noteUsersRepo.GetAll(noteId);
            if (noteUsers == null)
            {
                throw new ObjectNotFoundException();
            }
            if (!noteUsers.Any(u => u.UniqueId == userId))
            {
                throw new NoPermissionException();
            }

            var note = _repository.Get(listId, noteId);
            if (note == null)
            {
                throw new ObjectNotFoundException();
            }

            note.Users = noteUsers.ToList();
            return note;
        }

        public void UpdateNote(string userId, string listId, string noteId, Note noteData)
        {
            var noteUsers = _noteUsersRepo.GetAll(noteId);
            if (!noteUsers.Any(u => u.UniqueId == userId))
            {
                throw new NoPermissionException();
            }

            _repository.Update(listId, noteId, noteData);

            foreach (var user in noteUsers)
            {
                var notif = new Notification() { Task = noteData, User = user, Type = Notification.NotificationType.Update };
                _noticationMgr.CreateNotification(userId, notif);
            }
        }

        public void DeleteNote(string userId, string listId, string creatorId, string noteId)
        {
            var noteUsers = _noteUsersRepo.GetAll(noteId);
            if (!noteUsers.Any(u => u.UniqueId == userId))
            {
                throw new NoPermissionException();
            }

            var note = _repository.Get(listId, noteId);
            if (note == null)
            {
                throw new ObjectNotFoundException();
            }

            _repository.Delete(listId, noteId);

            foreach (var user in noteUsers)
            {
                var notif = new Notification() { User = user, Task = note, Type = Notification.NotificationType.Delete };
                _noticationMgr.CreateNotification(userId, notif);
            }

            var list = _taskListManager.GetTaskList(userId, listId, creatorId);
            foreach (var user in list.Users)
            {
                var notif = new Notification() { User = user, Task = list, Type = Notification.NotificationType.Update };
                _noticationMgr.CreateNotification(userId, notif);
            }
        }

        public void ShareNote(string userId, string listId, string noteId, string shareUserId)
        {
            var noteUsers = _noteUsersRepo.GetAll(noteId);
            if (!noteUsers.Any(u => u.UniqueId == userId))
            {
                throw new NoPermissionException();
            }

            var note = _repository.Get(listId, noteId);
            if (note == null)
            {
                throw new ObjectNotFoundException();
            }

            _userNotesRepo.Create(shareUserId, noteId, listId);
            _noteUsersRepo.Create(noteId, shareUserId, listId);

            var notif = new Notification() { Type = Notification.NotificationType.Share, Task = note, User = new User() { UniqueId = shareUserId } };
            _noticationMgr.CreateNotification(userId, notif);
        }

        public void ChangeOrder(string userId, string listId, string noteId, int order)
        {
            var noteUsers = _noteUsersRepo.GetAll(noteId);
            if (!noteUsers.Any(u => u.UniqueId == userId))
            {
                throw new NoPermissionException();
            }

            var note = _repository.Get(listId, noteId);
            if (note == null)
            {
                throw new ObjectNotFoundException();
            }

            var allListNotes = _repository.GetAll(listId);
            foreach (var task in allListNotes)
            {
                if (task.ListOrder >= order)
                {
                    task.ListOrder++;
                    UpdateNote(userId, listId, task.Id, task);
                }
            }
            note.ListOrder = order;
            UpdateNote(userId, listId, noteId, note);
        }

        #endregion
    }
}
