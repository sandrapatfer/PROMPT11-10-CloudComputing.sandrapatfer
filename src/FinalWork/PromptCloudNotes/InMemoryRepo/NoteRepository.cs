using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PromptCloudNotes.Interfaces.Repositories;
using PromptCloudNotes.Model;

namespace PromptCloudNotes.InMemoryRepo
{
    public class NoteRepository : INoteRepository
    {
        IUserRepository _userRepository;
        ITaskListRepository _listRepository;

        public NoteRepository(IUserRepository userRepo, ITaskListRepository listRepo)
        {
            _userRepository = userRepo;
            _listRepository = listRepo;
        }

        #region INoteRepository Members

        public IEnumerable<Note> GetAll(string listId)
        {
            var list = _listRepository.GetAll().FirstOrDefault(l => l.Id == listId);
            if (list == null || list.Tasks == null)
            {
                return null;
            }
            return list.Tasks.Cast<Note>();
        }

        public IEnumerable<Note> GetAll()
        {
            return null;
        }

        public void Create(Note noteData)
        {
            noteData.Id = Guid.NewGuid().ToString();

            noteData.Users = new List<User>();
            noteData.Users.Add(noteData.Creator);

            noteData.ParentList = _listRepository.Get(noteData.ParentList.Creator.UniqueId, noteData.ParentList.Id);
            if (noteData.ParentList.Tasks == null)
            {
                noteData.ParentList.Tasks = new List<Task>();
            }
            noteData.ListOrder = noteData.ParentList.Tasks.Count;
            noteData.ParentList.Tasks.Add(noteData);
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

        public void Update(string listId, string noteId, Note noteData)
        {
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

        public void ShareNote(string noteId, string userId)
        {
            var user = _userRepository.GetAll().FirstOrDefault(u => u.UniqueId == userId);
            var note = Get(noteId);
            note.Users.Add(user);
        }

        public void ShareNote(string listId, string noteId, string userId)
        {
            var user = _userRepository.GetAll().FirstOrDefault(u => u.UniqueId == userId);
            var note = Get(listId, noteId);
            note.Users.Add(user);
        }

        #endregion
    }
}
