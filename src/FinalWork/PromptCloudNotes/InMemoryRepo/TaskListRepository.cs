using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PromptCloudNotes.Interfaces;
using PromptCloudNotes.Model;

namespace PromptCloudNotes.InMemoryRepo
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

        public IEnumerable<TaskList> GetAll(string userId)
        {
            return _lists.Where(t => (t.Creator.UniqueId == userId || t.Users.Any(u => u.UniqueId == userId)));
        }

        public TaskList Create(User user, TaskList list)
        {
            list.Id = (++_listId).ToString();
            list.Users = new List<User>();
            list.Users.Add(user);
            list.Creator = user;
            _lists.Add(list);
            return list;
        }

        public TaskList Get(string id)
        {
            return _lists.FirstOrDefault(l => l.Id == id);
        }

        public TaskList GetWithUsers(string id)
        {
            return _lists.FirstOrDefault(l => l.Id == id);
        }

        public void Update(string listId, TaskList listData)
        {
            var list = _lists.First(l => l.Id == listId);
            list.Name = listData.Name;
            list.Description = listData.Description;
        }

        public void Delete(string listId)
        {
            var list = _lists.First(l => l.Id == listId);
            _lists.Remove(list);
        }

        public void Share(string listId, User user)
        {
            var list = _lists.First(l => l.Id == listId);
            list.Users.Add(user);
        }

        #endregion
    }
}
