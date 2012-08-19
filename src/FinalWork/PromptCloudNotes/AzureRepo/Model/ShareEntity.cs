using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure.StorageClient;

namespace PromptCloudNotes.AzureRepo.Model
{
    class ShareTaskListEntity : TableServiceEntity
    {
        public ShareTaskListEntity() { }

        public ShareTaskListEntity(int id, int userId)
            : base(id.ToString(), userId.ToString())
        {
        }

        public int Id
        {
            get
            {
                return Convert.ToInt32(PartitionKey);
            }
        }
        public int UserId
        {
            get
            {
                return Convert.ToInt32(RowKey);
            }
        }

        public string UserName { get; set; }
    }
}
