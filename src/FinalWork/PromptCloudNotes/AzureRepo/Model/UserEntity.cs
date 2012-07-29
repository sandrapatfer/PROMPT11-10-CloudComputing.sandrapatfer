using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure.StorageClient;

namespace PromptCloudNotes.AzureRepo.Model
{
    class UserEntity : TableServiceEntity
    {
        public UserEntity() { }

        public UserEntity(int userId, string email)
            : base(userId.ToString(), email)
        { }

        public string Name { get; set; }
    }
}
