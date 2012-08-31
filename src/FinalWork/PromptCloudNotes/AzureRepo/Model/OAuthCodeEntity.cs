using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure.StorageClient;

namespace PromptCloudNotes.AzureRepo.Model
{
    public class OAuthCodeEntity : TableServiceEntity
    {
        public OAuthCodeEntity() { }

        public OAuthCodeEntity(string clientId, string code)
            : base(clientId, code)
        {}

        public string User { get; set; }
    }
}
