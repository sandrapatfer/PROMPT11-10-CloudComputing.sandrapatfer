using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure.StorageClient;

namespace PromptCloudNotes.AzureRepo.Model
{
    class ShareEntity : TableServiceEntity
    {
        private const string _idFormat = "{0}_{1}";

        public ShareEntity() { }

        public ShareEntity(int id, bool list, int userId)
            : base(string.Format(_idFormat, id, list? 1: 0), userId.ToString())
        {
        }

        public int Id
        {
            get
            {
                var parts = PartitionKey.Split(new char[] { '_' });
                return Convert.ToInt32(parts[0]);
            }
        }
        public bool IsList
        {
            get
            {
                var parts = PartitionKey.Split(new char[] { '_' });
                return Convert.ToInt32(parts[1]) == 1;
            }
        }
        public int UserId
        {
            get
            {
                return Convert.ToInt32(RowKey);
            }
        }

    }
}
