using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PromptCloudNotes.Interfaces;
using StructureMap;
using System.Web.Http;
using System.Net;
using Exceptions;

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
            // TODO get user info
            int userId = 1;
            return _manager.GetAllNotifications(userId).Select(n => new WebApiModel.Notification() { id = n.Id, taskId = n.Task.Id });
        }

        // GET /api/notifications/{id}
        public WebApiModel.Notification Get(int id)
        {
            var notification = _manager.GetNotification(id);
            if (notification == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return new WebApiModel.Notification() { id = notification.Id, taskId = notification.Task.Id };
        }

        // DELETE /api/notifications/{id}
        public void Delete(int id)
        {
            try
            {
                _manager.DeleteNotification(id);
            }
            catch (ObjectNotFoundException)
            {
                // operation Delete must always work
            }
        }
    }
}