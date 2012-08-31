using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure.StorageClient;
using PromptCloudNotes.Model;

namespace PromptCloudNotes.AzureRepo.Model
{
    public class UserEntity : TableServiceEntity
    {
        public UserEntity() { }

        public UserEntity(string provider, string nameIdentifier)
            : base(provider, nameIdentifier)
        { }

        public UserEntity(User user)
            : base(user.Provider, user.NameIdentifier)
        {
            UniqueId = user.UniqueId;
            Name = user.Name;
            Email = user.Email;
        }

        public string UniqueId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public User GetUser()
        {
            return new PromptCloudNotes.Model.User
            {
                Provider = PartitionKey,
                NameIdentifier = RowKey,
                UniqueId = UniqueId,
                Name = Name,
                Email = Email
            };
        }
    }
}
