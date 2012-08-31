using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SignalR.Hubs;
using PromptCloudNotes.Interfaces;

namespace Server.Utils.SignalR
{
    [HubName("notificationHub")]
    public class NotificationHub : Hub
    {
        public void Subscribe(string userId, string listId)
        {
            Groups.Add(Context.ConnectionId, userId + listId);
        }
    }
}