using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure.StorageClient;

namespace PromptCloudNotes.AzureRepo.Model
{
    public class OAuthTokenEntity : TableServiceEntity
    {
        public OAuthTokenEntity() { }

        public OAuthTokenEntity(string tokenType, string token)
            : base(tokenType, token)
        { }

        public string User { get; set; }
        public DateTime CreatedAt { get; set; }
        public string RefreshToken { get; set; }
    }
}
