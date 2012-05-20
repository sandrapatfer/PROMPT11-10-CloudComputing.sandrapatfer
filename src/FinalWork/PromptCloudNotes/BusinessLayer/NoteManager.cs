using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PromptCloudNotes.Interfaces;
using PromptCloudNotes.Model;

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

            var taskList = _taskListManager.GetTaskList(listId);
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

        public Note GetNote(int list, int id)
        {
            return _repository.Get(list, id);
        }

        public void UpdateNote(int listId, int noteId, Note noteData)
        {
            _repository.Update(listId, noteId, noteData);
            var note = _repository.Get(listId, noteId);
            foreach (var user in note.Users)
            {
                var notif = new Notification() { Task = note, User = user, Type = Notification.NotificationType.Update };
                _noticationMgr.CreateNoteNotification(user.Id, listId, noteId, notif);
            }
        }

        public void DeleteNote(int listId, int noteId)
        {
            var note = _repository.Get(listId, noteId); // TODO GetShares ??
            var users = note.Users;
            _repository.Delete(listId, noteId);

            foreach (var user in users)
            {
                var notif = new Notification() { User = user, Task = note, Type = Notification.NotificationType.Delete };
                _noticationMgr.CreateNoteNotification(user.Id, listId, noteId, notif);
            }

            var taskList = _taskListManager.GetTaskList(listId);
            foreach (var user in taskList.Users)
            {
                var notif = new Notification() { User = user, Task = taskList, Type = Notification.NotificationType.Update };
                _noticationMgr.CreateTaskListNotification(user.Id, listId, notif);
            }
        }

        public void ShareNote(int listId, int noteId, int userId)
        {
            _repository.ShareNote(listId, noteId, userId);

            var notif = new Notification() { Type = Notification.NotificationType.Share };
            _noticationMgr.CreateNoteNotification(userId, listId, noteId, notif);
        }

        public void ChangeOrder(int listId, int noteId, int order)
        {
            _repository.ChangeOrder(listId, noteId, order);
        }

        #endregion
    }
}
