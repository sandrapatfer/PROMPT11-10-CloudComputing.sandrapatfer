using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PromptCloudNotes.Model;

namespace PromptCloudNotes.Interfaces.Repositories
{
    // repository to handle shared lists for users

    public interface IUserListsRepository
    {
        IEnumerable<TaskList> GetAll(string userId);

        void Create(string userId, string listId, string creatorId);

        void DeleteUser(string userId);
        void DeleteList(string listId);
    }
}
