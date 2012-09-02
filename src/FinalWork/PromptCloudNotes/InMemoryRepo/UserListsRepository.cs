using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PromptCloudNotes.Interfaces.Repositories;
using Exceptions;
using PromptCloudNotes.Model;

namespace PromptCloudNotes.InMemoryRepo
{
    public class UserListsRepository : IUserListsRepository
    {
        private IUserRepository _userRepository;
        private ITaskListRepository _taskListRepository;
        private INoteRepository _noteRepository;

        public UserListsRepository(IUserRepository userRepository, ITaskListRepository taskListRepository, INoteRepository noteRepository)
        {
            _userRepository = userRepository;
            _taskListRepository = taskListRepository;
            _noteRepository = noteRepository;
        }

        public IEnumerable<Model.TaskList> GetAll(string userId)
        {
            throw new NotImplementedException();
        }

        public void Create(string userId, string listId, string creatorId)
        {
            var user = _userRepository.GetAll().Where(u => u.UniqueId == userId).FirstOrDefault();
            if (user == null)
            {
                throw new ObjectNotFoundException();
            }
            var list = _taskListRepository.Get(creatorId, listId);
            if (list == null)
            {
                var note = _noteRepository.Get(creatorId, listId);
                if (note == null)
                {
                    throw new ObjectNotFoundException();
                }
            }
            else
            {
                if (user.Lists == null)
                {
                    user.Lists = new List<TaskList>();
                }
                if (!user.Lists.Contains(list))
                {
                    user.Lists.Add(list);
                }
            }
        }

        public void DeleteUser(string userId)
        {
        }

        public void DeleteList(string listId)
        {
        }
    }
}
