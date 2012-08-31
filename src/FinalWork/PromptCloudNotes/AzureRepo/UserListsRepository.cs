using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PromptCloudNotes.Interfaces.Repositories;
using PromptCloudNotes.Model;
using PromptCloudNotes.AzureRepo.Model;

namespace PromptCloudNotes.AzureRepo
{
    public class UserListsRepository : AzureTable<TaskList, UserListEntity>, IUserListsRepository
    {
        private const string TABLE_NAME = "UserListTable";

        public UserListsRepository()
            : base(TABLE_NAME)
        { }

        public new IEnumerable<TaskList> GetAll(string userId)
        {
            return GetAll(userId, l => new TaskList() { Id = l.RowKey, Creator = new User() { UniqueId = l.CreatorId } });
        }

        public void Create(string userId, string listId, string creatorId)
        {
            Create(new UserListEntity(userId, listId) { CreatorId = creatorId });
        }

        public void DeleteUser(string userId)
        {
            var entities = base.GetAll(userId);
            DeleteEntities(entities.ToList());
        }

        public void DeleteList(string listId)
        {
            var entities = base.GetAllInRow(listId);
            DeleteEntities(entities.ToList());
        }
    }
}
