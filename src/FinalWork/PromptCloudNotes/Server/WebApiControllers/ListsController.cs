using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Net.Http;
using System.Collections;
using PromptCloudNotes.Interfaces;
using StructureMap;
using PromptCloudNotes.Model;
using System.Net;
using System.Globalization;
using Exceptions;

namespace Server.WebApiControllers
{
    public class ListsController : ApiController
    {
        private ITaskListManager _manager;
        private IUserManager _userManager;

        public ListsController()
        {
            _manager = ObjectFactory.GetInstance<ITaskListManager>();
            _userManager = ObjectFactory.GetInstance<IUserManager>();
        }

        // GET /api/lists
        public IEnumerable<WebApiModel.TaskList> Get()
        {
            var user = _userManager.GetUser(Request.GetUserPrincipal().Identity.Name);
            return _manager.GetAllLists(user.Id).Select(l => new WebApiModel.TaskList() { id = l.Id, name = l.Name });
        }

        // GET /api/lists/{id}
        public WebApiModel.TaskList Get(int id)
        {
            var user = _userManager.GetUser(Request.GetUserPrincipal().Identity.Name);
            var list = _manager.GetTaskList(user.Id, id);
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
            var user = _userManager.GetUser(Request.GetUserPrincipal().Identity.Name);

            var listData = new TaskList() { Name = list.name };
            listData = _manager.CreateTaskList(user.Id, listData);
            list.id = listData.Id;

            var response = new HttpResponseMessage<WebApiModel.TaskList>(list)
            {
                StatusCode = HttpStatusCode.Created
            };
            response.Headers.Location = new Uri(Request.RequestUri,
                "/api/lists/" + listData.Id.ToString(CultureInfo.InvariantCulture));

            return response;
        }

        // PUT /api/lists/{id}
        public void Put(int id, WebApiModel.TaskList list)
        {
            try
            {
                var user = _userManager.GetUser(Request.GetUserPrincipal().Identity.Name);
                var listData = new TaskList() { Name = list.name };
                _manager.UpdateTaskList(user.Id, id, listData);
            }
            catch (ObjectNotFoundException)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }

        // DELETE /api/lists/{id}
        public void Delete(int id)
        {
            try
            {
                var user = _userManager.GetUser(Request.GetUserPrincipal().Identity.Name);
                _manager.DeleteTaskList(user.Id, id);
            }
            catch (ObjectNotFoundException)
            {
                // operation Delete must always work
            }
        }
    }
}