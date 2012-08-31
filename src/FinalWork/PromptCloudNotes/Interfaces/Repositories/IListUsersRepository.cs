using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PromptCloudNotes.Model;

namespace PromptCloudNotes.Interfaces.Repositories
{
    // repository to handle users that can see lists

    public interface IListUsersRepository
    {
        IEnumerable<User> GetAll(string listId);

        void Create(string listId, string userId, string userName);

        void DeleteList(string listId);
        void DeleteUser(string userId);
    }
}
