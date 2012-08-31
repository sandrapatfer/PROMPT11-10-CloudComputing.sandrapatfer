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
        // Returns all the users that can see the list
        IEnumerable<User> GetAll(string listId);

        // Adds a user that can see the list
        void Create(string listId, string userId, string userName);

        // Deletes a list
        void DeleteList(string listId);

        // Deletes all the entries of lists for the user
        void DeleteUser(string userId);
    }
}
