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
        public void Send(string userId, PromptCloudNotes.Model.Notification notification)
        {
            
            string notificationData = null;
            if (notification.Task is PromptCloudNotes.Model.TaskList)
            {
                //notificationData = new MvcModel.TaskListNotification(notification);
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
                }
            }

            if (notificationData != null)
            {
                var context = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
                context.Clients.Send(userId, notificationData);
            }
        }
    }
}