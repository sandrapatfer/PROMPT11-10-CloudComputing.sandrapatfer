using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PromptCloudNotes.Interfaces.Repositories;
using PromptCloudNotes.Model;
using PromptCloudNotes.AzureRepo.Model;

namespace PromptCloudNotes.AzureRepo
{
    public class ListUsersRepository : AzureTable<User, ListUserEntity>, IListUsersRepository
    {
        private const string TABLE_NAME = "ListUserTable";

        public ListUsersRepository()
            : base(TABLE_NAME)
        { }

        public new IEnumerable<User> GetAll(string listId)
        {
            return GetAll(listId, u => new User() { UniqueId = u.RowKey, Name = u.UserName });
        }

        public void Create(string listId, string userId, string name)
        {
            Create(new ListUserEntity(listId, userId) { UserName = name });
        }

        public void DeleteList(string listId)
        {
            var entities = base.GetAll(listId);
            DeleteEntities(entities.ToList());
        }

        public void DeleteUser(string userId)
        {
            var entities = base.GetAllInRow(userId);
            DeleteEntities(entities.ToList());
        }

    }
}
