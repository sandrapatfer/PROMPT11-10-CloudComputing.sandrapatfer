using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure.StorageClient;

namespace PromptCloudNotes.AzureRepo.Model
{
    public class ListUserEntity : TableServiceEntity
    {
        public ListUserEntity() { }

        public ListUserEntity(string listId, string userId)
            : base(listId, userId)
        {
        }

        public string UserName { get; set; }
    }
}
