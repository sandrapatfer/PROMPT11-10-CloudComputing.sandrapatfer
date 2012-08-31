using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Server.MvcModel
{
    public class TaskListNotification
    {
        public TaskListNotification(PromptCloudNotes.Model.Notification data)
        {
            listId = data.Task.Id;
            switch (data.Type)
            {
                case PromptCloudNotes.Model.Notification.NotificationType.Share:
                    type = "share";
                    break;
                case PromptCloudNotes.Model.Notification.NotificationType.Insert:
                    type = "insert";
                    break;
                case PromptCloudNotes.Model.Notification.NotificationType.Update:
                    type = "update";
                    break;
                case PromptCloudNotes.Model.Notification.NotificationType.Delete:
                    type = "delete";
                    break;
            }
        }

        public string listId { get; set; }
        public string type { get; set; }
    }
}