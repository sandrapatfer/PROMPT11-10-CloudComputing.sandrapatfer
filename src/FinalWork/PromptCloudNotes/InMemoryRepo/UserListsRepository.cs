using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PromptCloudNotes.Interfaces.Repositories;

namespace PromptCloudNotes.InMemoryRepo
{
    public class UserListsRepository : IUserListsRepository
    {
        public IEnumerable<Model.TaskList> GetAll(string userId)
        {
            throw new NotImplementedException();
        }

        public void Create(string userId, string listId, string creatorId)
        {
        }

        public void DeleteUser(string userId)
        {
        }

        public void DeleteList(string listId)
        {
        }
    }
}
