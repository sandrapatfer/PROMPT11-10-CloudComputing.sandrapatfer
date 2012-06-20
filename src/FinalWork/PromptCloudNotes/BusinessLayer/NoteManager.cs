using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PromptCloudNotes.Interfaces;
using PromptCloudNotes.Model;
using Exceptions;

namespace BusinessLayer.Managers
{
    public class NoteManager : INoteManager
    {
        private INoteRepository _repository;
        private ITaskListManager _taskListManager;
        private INotificationManager _noticationMgr;

        public NoteManager(INoteRepository repo, ITaskListManager mgr, INotificationManager notify)
        {
            _repository = repo;
            _taskListManager = mgr;
            _noticationMgr = notify;
        }

        #region INoteManager Members

        public Note CreateNote(int userId, int listId, Note noteData)
        {
            Note newNote = _repository.Create(userId, listId, noteData);

            var taskList = _taskListManager.GetTaskList(userId, listId);
            foreach (var user in taskList.Users)
            {
                var notif = new Notification() { Task = newNote, User = user, Type = Notification.NotificationType.Insert };
                _noticationMgr.CreateNoteNotification(user.Id, listId, newNote.Id, notif);
            }

            return newNote;
        }

        public IEnumerable<Note> GetAllNotes(int userId, int listId)
        {
            return _repository.GetAll(userId, listId);
        }

        public IEnumerable<Note> GetAllNotes(int userId)
        {
            return _repository.GetAll(userId);
        }

        public Note GetNote(int userId, int list, int id)
        {
            var note = _repository.Get(list, id);
            if (note == null)
            {
                throw new ObjectNotFoundException();
            }
            if (!note.Users.Any(u => u.Id == userId))
            {
                //TODO throw permission exception?
                return null;
            }
            return note;
        }

        public void UpdateNote(int userId, int listId, int noteId, Note noteData)
        {
            var note = _repository.Get(listId, noteId);
            if (note == null)
            {
                throw new ObjectNotFoundException();
            }
            if (!note.Users.Any(u => u.Id == userId))
            {
                //TODO throw permission exception?
                return;
            }

            _repository.Update(listId, noteId, noteData);
            foreach (var user in note.Users)
            {
                var notif = new Notification() { Task = note, User = user, Type = Notification.NotificationType.Update };
                _noticationMgr.CreateNoteNotification(user.Id, listId, noteId, notif);
            }
        }

        public void DeleteNote(int userId, int listId, int noteId)
        {
            var note = _repository.Get(listId, noteId);
            if (note == null)
            {
                throw new ObjectNotFoundException();
            }
            var users = note.Users;
            if (!users.Any(u => u.Id == userId))
            {
                // TODO throw permission exception?
                return;
            }
            var taskList = _taskListManager.GetTaskList(userId, listId);
            if (taskList == null)
            {
                // the user must have permission over the list to remove the note
                // TODO throw permission exception or object not found?
                return;
            }

            _repository.Delete(listId, noteId);

            foreach (var user in users)
            {
                var notif = new Notification() { User = user, Task = note, Type = Notification.NotificationType.Delete };
                _noticationMgr.CreateNoteNotification(user.Id, listId, noteId, notif);
            }

            foreach (var user in taskList.Users)
            {
                if (!users.Contains(user))
                {
                    var notif = new Notification() { User = user, Task = taskList, Type = Notification.NotificationType.Update };
                    _noticationMgr.CreateTaskListNotification(user.Id, listId, notif);
                }
            }
        }

        public void ShareNote(int userId, int listId, int noteId, int shareUserId)
        {
            var note = _repository.Get(listId, noteId);
            if (note == null)
            {
                throw new ObjectNotFoundException();
            }
            var users = note.Users;
            if (!users.Any(u => u.Id == userId))
            {
                // TODO throw permission exception?
                return;
            }

            _repository.ShareNote(listId, noteId, shareUserId);

            var notif = new Notification() { Type = Notification.NotificationType.Share };
            _noticationMgr.CreateNoteNotification(shareUserId, listId, noteId, notif);
        }

        public void ChangeOrder(int userId, int listId, int noteId, int order)
        {
            var note = _repository.Get(listId, noteId);
            if (note == null)
            {
                throw new ObjectNotFoundException();
            }
            var users = note.Users;
            if (!users.Any(u => u.Id == userId))
            {
                // TODO throw permission exception?
                return;
            }

            _repository.ChangeOrder(listId, noteId, order);
        }

        #endregion
    }
}
