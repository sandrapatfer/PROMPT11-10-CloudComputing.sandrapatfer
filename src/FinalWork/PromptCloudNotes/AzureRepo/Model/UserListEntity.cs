using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure.StorageClient;

namespace PromptCloudNotes.AzureRepo.Model
{
    public class UserListEntity : TableServiceEntity
    {
        public UserListEntity() { }

        public UserListEntity(string userId, string listId)
            : base(userId, listId)
        {
        }

        public string CreatorId { get; set; }
    }
}
