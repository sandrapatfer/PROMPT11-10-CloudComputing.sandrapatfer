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

namespace Server.WebApiControllers
{
    [Authorize]
    public class NotificationsController : ApiController
    {
        private INotificationManager _manager;
        private IUserManager _userManager;

        public NotificationsController(IUserManager userManager)
        {
            _manager = ObjectFactory.GetInstance<INotificationManager>();
            _userManager = userManager;
        }

        // GET /api/notifications
        public IEnumerable<WebApiModel.Notification> Get()
        {
            var user = Request.GetUserPrincipal() as User;
            //var user = _userManager.GetUser(Request.GetUserPrincipal().Identity.Name);
            return _manager.GetAllNotifications(user.UniqueId).Select(n => new WebApiModel.Notification() { id = n.Id, taskId = n.Task.Id });
        }

        // GET /api/notifications/{id}
        public WebApiModel.Notification Get(string id)
        {
/*            var notification = _manager.GetNotification(id);
            if (notification == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return new WebApiModel.Notification() { id = notification.Id, taskId = notification.Task.Id };*/
            return null;
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