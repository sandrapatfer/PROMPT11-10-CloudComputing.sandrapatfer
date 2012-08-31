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

        public NoteManager(INoteRepository repo, IUserRepository userRepo, ITaskListManager mgr, INotificationManager notify)
        {
            _repository = repo;
            _userRepository = userRepo;
            _taskListManager = mgr;
            _noticationMgr = notify;
        }

        #region INoteManager Members

        public void CreateNote(User user, string listId, string creatorId, Note noteData)
        {
            noteData.Creator = user;

            var list = _taskListManager.GetTaskList(user.UniqueId, listId, creatorId);
            noteData.ParentList = list;

            _repository.Create(noteData);

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
            var note = _repository.Get(listId, noteId);
            if (note == null)
            {
                throw new ObjectNotFoundException();
            }
            /* TODO
            if (!note.Users.Any(u => u.UniqueId == userId))
            {
                //TODO throw permission exception?
                return null;
            }*/
            return note;
        }

        public void UpdateNote(string userId, string listId, string noteId, Note noteData)
        {
            var note = _repository.Get(listId, noteId);
            if (note == null)
            {
                throw new ObjectNotFoundException();
            }
            /* TODO
            if (!note.Users.Any(u => u.UniqueId == userId))
            {
                //TODO throw permission exception?
                return;
            }*/

            _repository.Update(listId, noteId, noteData);

            /*foreach (var user in note.Users)
            {
                var notif = new Notification() { Task = note, User = user, Type = Notification.NotificationType.Update };
                _noticationMgr.CreateNoteNotification(user.UniqueId, noteId, notif);
            }*/
        }

        public void DeleteNote(string userId, string listId, string creatorId, string noteId)
        {
            var note = _repository.Get(listId, noteId);
            if (note == null)
            {
                throw new ObjectNotFoundException();
            }
            /* TODO
            var users = note.Users;
            if (!users.Any(u => u.UniqueId == userId))
            {
                // TODO throw permission exception?
                return;
            }*/
            var taskList = _taskListManager.GetTaskList(userId, listId, creatorId);
            if (taskList == null)
            {
                // the user must have permission over the list to remove the note
                // TODO throw permission exception or object not found?
                return;
            }

            _repository.Delete(listId, noteId);

            /* TODO
            foreach (var user in users)
            {
                var notif = new Notification() { User = user, Task = note, Type = Notification.NotificationType.Delete };
                _noticationMgr.CreateNoteNotification(user.UniqueId, noteId, notif);
            }

            foreach (var user in taskList.Users)
            {
                if (!users.Contains(user))
                {
                    var notif = new Notification() { User = user, Task = taskList, Type = Notification.NotificationType.Update };
                    _noticationMgr.CreateTaskListNotification(user.UniqueId, taskList.Id, notif);
                }
            }*/
        }

        public void ShareNote(string userId, string listId, string noteId, string shareUserId)
        {
            var note = _repository.Get(listId, noteId);
            if (note == null)
            {
                throw new ObjectNotFoundException();
            }
            var users = note.Users;
            if (!users.Any(u => u.UniqueId == userId))
            {
                // TODO throw permission exception?
                return;
            }

            /* TODO
            //_repository.ShareNote(listId, noteId, shareUserId);

            var notif = new Notification() { Type = Notification.NotificationType.Share, User = new User() { UniqueId = shareUserId }, Task=note };
            _noticationMgr.CreateNotification(userId, notif);*/
        }

        public void ChangeOrder(string userId, string noteId, int order)
        {
            // TODO !!!
            var note = _repository.Get("", noteId);
            if (note == null)
            {
                throw new ObjectNotFoundException();
            }
            var users = note.Users;
            if (!users.Any(u => u.UniqueId == userId))
            {
                // TODO throw permission exception?
                return;
            }

            // TODO
            //_repository.ChangeOrder("", noteId, order);
        }

        #endregion
    }
}
