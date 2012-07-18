using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure.StorageClient;

namespace AzureRepo.Model
{
    class TaskListEntity : TableServiceEntity
    {
        public TaskListEntity() { }

        // Use the id as partition key and row key, is heavier to insert, but faster to read

        public TaskListEntity(int userId, int listId)
            : base(userId.ToString(), listId.ToString())
        {}

        public string Name { get; set; }
        public string Description { get; set; }
    }
}
