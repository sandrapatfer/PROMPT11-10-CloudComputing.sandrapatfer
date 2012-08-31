using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Net.Http;
using System.Collections;
using PromptCloudNotes.Interfaces.Managers;
using StructureMap;
using PromptCloudNotes.Model;
using System.Net;
using System.Globalization;
using Exceptions;
using Server.Utils;

namespace Server.WebApiControllers
{
    [Authorize]
    public class ListsController : ApiController
    {
        private ITaskListManager _manager;

        public ListsController()
        {
            _manager = ObjectFactory.GetInstance<ITaskListManager>();
        }

        // GET /api/lists
        public IEnumerable<WebApiModel.TaskList> Get()
        {
            var userId = (Request.GetUserPrincipal().Identity as UserIdentity).UserId;
            return _manager.GetAllLists(userId).Select(l => new WebApiModel.TaskList() { id = l.Id, name = l.Name });
        }

        // GET /api/lists/{id}
        public WebApiModel.TaskList Get(string listId, string creatorId)
        {
            var userId = (Request.GetUserPrincipal().Identity as UserIdentity).UserId;
            var list = _manager.GetTaskList(userId, listId, string.IsNullOrEmpty(creatorId)? userId : creatorId);
            if (list == null) // TODO is it null or exception?
            {
                // TODO create a better response
                // see http://www.asp.net/web-api/overview/web-api-routing-and-actions/exception-handling
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return new WebApiModel.TaskList() { id = list.Id, name = list.Name };
        }

        // POST /api/lists
        public HttpResponseMessage Post(WebApiModel.TaskList list)
        {
            var userId = (Request.GetUserPrincipal().Identity as UserIdentity).UserId;

            var listData = new TaskList() { Name = list.name };
            _manager.CreateTaskList(new User { UniqueId = userId }, listData);
            list.id = listData.Id;

            var response = new HttpResponseMessage<WebApiModel.TaskList>(list)
            {
                StatusCode = HttpStatusCode.Created
            };
            response.Headers.Location = new Uri(Request.RequestUri,
                "/api/lists/" + listData.Id + "/" + userId);

            return response;
        }

        // PUT /api/lists/{id}
        public void Put(string id, WebApiModel.TaskList list)
        {
            try
            {
                var userId = (Request.GetUserPrincipal().Identity as UserIdentity).UserId;

                var listData = new TaskList() { Name = list.name };
                _manager.UpdateTaskList(userId, id, string.IsNullOrEmpty(list.creatorId)? userId : list.creatorId, listData);
            }
            catch (ObjectNotFoundException)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }

        // DELETE /api/lists/{id}
        public void Delete(string id, string creatorId)
        {
            try
            {
                var userId = (Request.GetUserPrincipal().Identity as UserIdentity).UserId;
                _manager.DeleteTaskList(userId, id, string.IsNullOrEmpty(creatorId) ? userId : creatorId);
            }
            catch (ObjectNotFoundException)
            {
                // operation Delete must always work
            }
        }
    }
}