using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PromptCloudNotes.Interfaces;
using PromptCloudNotes.Model;

namespace PromptCloudNotes.InMemoryRepo
{
    public class NoteRepository : INoteRepository
    {
        IUserRepository _userRepository;
        ITaskListRepository _listRepository;
        private int _noteId = 0;

        public NoteRepository(IUserRepository userRepo, ITaskListRepository listRepo)
        {
            _userRepository = userRepo;
            _listRepository = listRepo;
        }

        #region INoteRepository Members

        public IEnumerable<Note> GetAll(string userId, string listId)
        {
            var list = _listRepository.Get(listId);
            if (list == null || list.Tasks == null)
            {
                return null;
            }
            return list.Tasks.Where(t => (t.Creator.UniqueId == userId || t.Users.Any(u => u.UniqueId == userId)) && t is Note).Cast<Note>();
        }

        public IEnumerable<Note> GetAll(string userId)
        {
            var lists = _listRepository.GetAll(userId);
            if (lists == null)
            {
                return null;
            }
            return lists.Where(l => l.Tasks != null).Select(l => l.Tasks).Aggregate((l1, l2) => l1.Concat(l2).ToList()).
                Where(t => (t.Creator.UniqueId == userId || t.Users.Any(u => u.UniqueId == userId)) && t is Note).Cast<Note>();
        }

        public Note Create(string userId, string listId, Note noteData)
        {
            noteData.Id = (++_noteId).ToString();

            noteData.Users = new List<User>();
            noteData.Users.Add(noteData.Creator);

            noteData.ParentList = _listRepository.Get(listId);
            if (noteData.ParentList.Tasks == null)
            {
                noteData.ParentList.Tasks = new List<Task>();
            }
            noteData.ListOrder = noteData.ParentList.Tasks.Count;
            noteData.ParentList.Tasks.Add(noteData);
            return noteData;
        }

        public Note Get(string noteId)
        {
            var users = _userRepository.GetAll();
            foreach (var user in users)
            {
                var lists = _listRepository.GetAll(user.UniqueId);
                if (lists != null)
                {
                    foreach (var list in lists)
                    {
                        var note = list.Tasks.FirstOrDefault(t => t.Id == noteId);
                        if (note != null)
                        {
                            return note as Note;
                        }
                    }
                }
            }
            return null;
        }

        public Note Get(string listId, string noteId)
        {
            var users = _userRepository.GetAll();
            foreach (var user in users)
            {
                var lists = _listRepository.GetAll(user.UniqueId);
                if (lists != null)
                {
                    var list = lists.FirstOrDefault(l => l.Id == listId);
                    if (list != null)
                    {
                        var note = list.Tasks.FirstOrDefault(t => t.Id == noteId);
                        if (note != null)
                        {
                            return note as Note;
                        }
                    }
                }
            }
            return null;
        }

        public Note Update(string noteId, Note noteData)
        {
            return noteData;
        }

        public Note Update(string listId, string noteId, Note noteData)
        {
            return noteData;
        }

        public void Delete(string noteId)
        {
            var note = Get(noteId);
            if (note != null)
            {
                foreach (var task in note.ParentList.Tasks)
                {
                    if (task.ListOrder > note.ListOrder)
                    {
                        task.ListOrder--;
                    }
                }
                note.ParentList.Tasks.Remove(note);
            }
        }

        public void Delete(string listId, string noteId)
        {
            var note = Get(listId, noteId);
            if (note != null)
            {
                foreach (var task in note.ParentList.Tasks)
                {
                    if (task.ListOrder > note.ListOrder)
                    {
                        task.ListOrder--;
                    }
                }
                note.ParentList.Tasks.Remove(note);
            }
        }

        public void ChangeOrder(string noteId, int order)
        {
            var note = Get(noteId);
            foreach (var task in note.ParentList.Tasks)
            {
                if (task.ListOrder >= order)
                {
                    task.ListOrder++;
                }
            }
            note.ListOrder = order;
        }

        public void ChangeOrder(string listId, string noteId, int order)
        {
            var note = Get(listId, noteId);
            foreach (var task in note.ParentList.Tasks)
            {
                if (task.ListOrder >= order)
                {
                    task.ListOrder++;
                }
            }
            note.ListOrder = order;
        }

        public void ShareNote(string noteId, string userId)
        {
            var memoryRepo = _userRepository as UserRepository;
            var user = memoryRepo.GetById(userId);
            var note = Get(noteId);
            note.Users.Add(user);
        }

        public void ShareNote(string listId, string noteId, string userId)
        {
            var memoryRepo = _userRepository as UserRepository;
            var user = memoryRepo.GetById(userId);
            var note = Get(listId, noteId);
            note.Users.Add(user);
        }

        #endregion
    }
}
