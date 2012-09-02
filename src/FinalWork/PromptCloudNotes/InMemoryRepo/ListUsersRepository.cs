using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PromptCloudNotes.Interfaces.Repositories;
using Exceptions;

namespace PromptCloudNotes.InMemoryRepo
{
    public class ListUsersRepository : IListUsersRepository
    {
        private IUserRepository _userRepository;
        private ITaskListRepository _taskListRepository;
        private INoteRepository _noteRepository;

        public ListUsersRepository(IUserRepository userRepository, ITaskListRepository taskListRepository, INoteRepository noteRepository)
        {
            _userRepository = userRepository;
            _taskListRepository = taskListRepository;
            _noteRepository = noteRepository;
        }

        public IEnumerable<Model.User> GetAll(string listId)
        {
            var list = _taskListRepository.Get(null, listId);
            if (list != null)
            {
                return list.Users;
            }
            else
            {
                var lists = _taskListRepository.GetAll();
                list = lists.FirstOrDefault(l => l.Tasks != null && l.Tasks.Any(n => n.Id == listId));
                if (list != null)
                {
                    return list.Tasks.First(n => n.Id == listId).Users;
                }
            }
            return null;
        }

        public void Create(string listId, string userId, string userName)
        {
            var user = _userRepository.GetAll().Where(u => u.UniqueId == userId).FirstOrDefault();
            if (user == null)
            {
                throw new ObjectNotFoundException();
            }
            var list = _taskListRepository.Get(null, listId);
            if (list == null)
            {
                // parameters are sent for notes just for testing purposes
                var note = _noteRepository.Get(userName, listId);
                if (note == null)
                {
                    throw new ObjectNotFoundException();
                }
                if (!note.Users.Contains(user))
                {
                    note.Users.Add(user);
                }
            }
            else
            {
                if (!list.Users.Contains(user))
                {
                    list.Users.Add(user);
                }
            }
        }

        public void DeleteList(string listId)
        {
        }

        public void DeleteUser(string userId)
        {
        }
    }
}
