using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure.StorageClient;
using PromptCloudNotes.Model;

namespace PromptCloudNotes.AzureRepo.Model
{
    public class TaskListEntity : TableServiceEntity
    {
        public TaskListEntity() { }

        // Use the id as partition key and row key, is heavier to insert, but faster to read

        public TaskListEntity(string userId, string listId)
            : base(userId, listId)
        {}

        public TaskListEntity(TaskList list)
            : base(list.Creator.UniqueId, list.Id)
        {
            Update(list);
        }
        
        public string Name { get; set; }
        public string Description { get; set; }

        public TaskList GetTaskList()
        {
            return new TaskList()
            {
                Creator = new User() { UniqueId = PartitionKey },
                Id = RowKey,
                Name = Name,
                Description = Description
            };
        }

        public void Update(TaskList list)
        {
            Name = list.Name;
            Description = list.Description;
        }

    }
}
