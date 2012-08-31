using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PromptCloudNotes.Interfaces;
using SignalR;
using StructureMap;

namespace Server.Utils.SignalR
{
    public class NotificationHubProxy : INotificationProcessor
    {
        public bool Send(PromptCloudNotes.Model.Notification notification)
        {
            if (notification.Task is PromptCloudNotes.Model.TaskList)
            {
            }
            else
            {
                if (notification.Type == PromptCloudNotes.Model.Notification.NotificationType.Insert)
                {
                    var context = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
                    context.Clients[notification.User.UniqueId + notification.Task.ParentList.Id].
                        NewNote(new MvcModel.Note()
                        {
                            id = notification.Task.Id,
                            listId = notification.Task.ParentList.Id,
                            listCreatorId = notification.Task.ParentList.Creator.UniqueId,
                            name = notification.Task.Name,
                            description = notification.Task.Description
                        });
                    return true;
                }
            }

            return false;
        }
    }
}