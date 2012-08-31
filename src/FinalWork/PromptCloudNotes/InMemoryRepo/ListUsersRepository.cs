using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PromptCloudNotes.Interfaces.Repositories;

namespace PromptCloudNotes.InMemoryRepo
{
    public class ListUsersRepository : IListUsersRepository
    {
        ITaskListRepository _listRepository;

        public ListUsersRepository(ITaskListRepository listRepository)
        {
            _listRepository = listRepository;
        }

        public IEnumerable<Model.User> GetAll(string listId)
        {
            var list = _listRepository.Get(null, listId);
            if (list != null)
            {
                return list.Users;
            }
            return null;
        }

        public void Create(string listId, string userId, string userName)
        {
        }

        public void DeleteList(string listId)
        {
        }

        public void DeleteUser(string userId)
        {
        }
    }
}
