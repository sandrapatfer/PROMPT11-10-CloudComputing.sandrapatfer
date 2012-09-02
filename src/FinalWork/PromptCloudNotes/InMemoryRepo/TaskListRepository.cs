using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PromptCloudNotes.Interfaces.Repositories;
using PromptCloudNotes.Model;

namespace PromptCloudNotes.InMemoryRepo
{
    public class TaskListRepository : ITaskListRepository
    {
        private IUserRepository _userRepository;
        private List<TaskList> _lists = new List<TaskList>();

        public TaskListRepository(IUserRepository userRepo)
        {
            _userRepository = userRepo;
        }

        public IEnumerable<TaskList> GetAll()
        {
            return _lists;
        }

        public IEnumerable<TaskList> GetAll(string userId)
        {
            return _lists.Where(t => (t.Creator.UniqueId == userId || t.Users.Any(u => u.UniqueId == userId)));
        }

        public void Create(TaskList list)
        {
            list.Id = Guid.NewGuid().ToString();
            if (!list.Users.Contains(list.Creator))
            {
                list.Users.Add(list.Creator);
            }
            _lists.Add(list);
        }

        public TaskList Get(string user, string list)
        {
            return _lists.FirstOrDefault(l => l.Id == list);
        }

        public TaskList GetWithUsers(string id)
        {
            return _lists.FirstOrDefault(l => l.Id == id);
        }

        public void Update(string user, string list, TaskList listData)
        {
        }

        public void Delete(string userId, string listId)
        {
            var list = _lists.First(l => l.Id == listId);
            _lists.Remove(list);
        }

        public void Share(string listId, User user)
        {
            var list = _lists.First(l => l.Id == listId);
            list.Users.Add(user);
        }
    }
}
