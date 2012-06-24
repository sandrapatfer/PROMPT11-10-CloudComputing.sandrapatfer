using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PromptCloudNotes.Interfaces;
using PromptCloudNotes.Model;

namespace InMemoryRepo
{
    public class TaskListRepository : ITaskListRepository
    {
        private IUserRepository _userRepository;
        private List<TaskList> _lists = new List<TaskList>();
        private int _listId = 0;

        public TaskListRepository(IUserRepository userRepo)
        {
            _userRepository = userRepo;
        }

        #region ITaskListRepository Members

        public IEnumerable<TaskList> GetAll(int userId)
        {
            return _lists.Where(t => (t.Creator.Id == userId || t.Users.Any(u => u.Id == userId)));
        }

        public TaskList Create(int userId, TaskList list)
        {
            list.Id = ++_listId;
            var user = _userRepository.Get(userId);
            list.Users = new List<User>();
            list.Users.Add(user);
            list.Creator = user;
            _lists.Add(list);
            return list;
        }

        public TaskList Get(int id)
        {
            return _lists.FirstOrDefault(l => l.Id == id);
        }

        public void Update(int listId, TaskList listData)
        {
            var list = _lists.First(l => l.Id == listId);
            list.Name = listData.Name;
            list.Description = listData.Description;
        }

        public void Delete(int listId)
        {
            var list = _lists.First(l => l.Id == listId);
            _lists.Remove(list);
        }

        public void Share(int listId, int userId)
        {
            var user = _userRepository.Get(userId);
            var list = _lists.First(l => l.Id == listId);
            list.Users.Add(user);
        }

        #endregion
    }
}
