using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure.StorageClient;
using PromptCloudNotes.Model;

namespace PromptCloudNotes.AzureRepo.Model
{
    public class NotificationEntity: TableServiceEntity
    {
        public NotificationEntity() { }

        public NotificationEntity(string userId, string notificationId)
            : base(userId, notificationId)
        { }

        public NotificationEntity(Notification n)
            : base(n.User.UniqueId, n.Id)
        {
            TaskId = n.Task.Id;
            IsList = (n.Task is TaskList);
        }

        public string TaskId { get; set; }
        public bool IsList { get; set; }

        public Notification GetNotification()
        {
            return new Notification()
            {
                User = new User() { UniqueId = PartitionKey },
                Id = RowKey,
                Task = IsList ? (Task)new TaskList()
                {
                    Id = TaskId
                } : (Task)new Note
                {
                    Id = TaskId
                }
            };
        }
    }
}
