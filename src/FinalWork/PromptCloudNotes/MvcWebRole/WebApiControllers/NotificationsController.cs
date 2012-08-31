using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PromptCloudNotes.Interfaces.Managers;
using StructureMap;
using System.Web.Http;
using System.Net;
using Exceptions;
using PromptCloudNotes.Model;
using Server.Utils;

namespace Server.WebApiControllers
{
    [Authorize]
    public class NotificationsController : ApiController
    {
        private INotificationManager _manager;

        public NotificationsController()
        {
            _manager = ObjectFactory.GetInstance<INotificationManager>();
        }

        // GET /api/notifications
        public IEnumerable<WebApiModel.Notification> Get()
        {
            var userId = (Request.GetUserPrincipal().Identity as UserIdentity).UserId;
            return _manager.GetAllNotifications(userId).Select(n => new WebApiModel.Notification() { id = n.Id, taskId = n.Task.Id });
        }

        // DELETE /api/notifications/{id}
        public void Delete(string id)
        {
            try
            {
                //_manager.DeleteNotification(id);
            }
            catch (ObjectNotFoundException)
            {
                // operation Delete must always work
            }
        }
    }
}