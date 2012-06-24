using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PromptCloudNotes.Interfaces;
using PromptCloudNotes.Model;

namespace InMemoryRepo
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

        public IEnumerable<Note> GetAll(int userId, int listId)
        {
            var list = _listRepository.Get(listId);
            if (list == null || list.Tasks == null)
            {
                return null;
            }
            return list.Tasks.Where(t => (t.Creator.Id == userId || t.Users.Any(u => u.Id == userId)) && t is Note).Cast<Note>();
        }

        public IEnumerable<Note> GetAll(int userId)
        {
            var lists = _listRepository.GetAll(userId);
            if (lists == null)
            {
                return null;
            }
            return lists.Where(l => l.Tasks != null).Select(l => l.Tasks).Aggregate((l1, l2) => l1.Concat(l2).ToList()).
                Where(t => (t.Creator.Id == userId || t.Users.Any(u => u.Id == userId)) && t is Note).Cast<Note>();
        }

        public Note Create(int userId, int listId, Note noteData)
        {
            var parentList = _listRepository.Get(listId);
            noteData.Id = ++_noteId;

            noteData.Users = new List<User>();
            var user = _userRepository.Get(userId);
            noteData.Creator = user;
            noteData.Users.Add(user);

            noteData.ParentList = _listRepository.Get(listId);
            if (noteData.ParentList.Tasks == null)
            {
                noteData.ParentList.Tasks = new List<Task>();
            }
            noteData.ListOrder = noteData.ParentList.Tasks.Count;
            noteData.ParentList.Tasks.Add(noteData);
            return noteData;
        }

        public Note Get(int list, int id)
        {
            var taskList = _listRepository.Get(list);
            return taskList.Tasks.First(t => t.Id == id && t is Note) as Note;
        }

        public Note Update(int listId, int noteId, Note noteData)
        {
            return noteData;
        }

        public void Delete(int listId, int noteId)
        {
            var taskList = _listRepository.Get(listId);
            var note = taskList.Tasks.First(t => t.Id == noteId);
            foreach (var task in taskList.Tasks)
            {
                if (task.ListOrder > note.ListOrder)
                {
                    task.ListOrder--;
                }
            }
            taskList.Tasks.Remove(note);
        }

        public void ChangeOrder(int list, int noteId, int order)
        {
            var taskList = _listRepository.Get(list);
            foreach (var task in taskList.Tasks)
            {
                if (task.ListOrder >= order)
                {
                    task.ListOrder++;
                }
            }
            taskList.Tasks.First(t => t.Id == noteId).ListOrder = order;
        }

        public void ShareNote(int listId, int noteId, int userId)
        {
            var user = _userRepository.Get(userId);
            var note = _listRepository.Get(listId).Tasks.First(t => t.Id == noteId);
            note.Users.Add(user);
        }

        #endregion
    }
}
