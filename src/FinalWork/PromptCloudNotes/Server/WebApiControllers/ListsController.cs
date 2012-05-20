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

        public ListsController()
        {
            _manager = ObjectFactory.GetInstance<ITaskListManager>();
        }

        // GET /api/lists
        public IEnumerable<WebApiModel.TaskList> Get()
        {
            // TODO get user info
            int userId = 1;
            return _manager.GetAllLists(userId).Select(l => new WebApiModel.TaskList() { id = l.Id, name = l.Name });
        }

        // GET /api/lists/{id}
        public WebApiModel.TaskList Get(int id)
        {
            var list = _manager.GetTaskList(id);
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
            // TODO get user info
            int userId = 1;

            var listData = new TaskList() { Name = list.name };
            listData = _manager.CreateTaskList(userId, listData);
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
                var listData = new TaskList() { Name = list.name };
                _manager.UpdateTaskList(id, listData);
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
                _manager.DeleteTaskList(id);
            }
            catch (ObjectNotFoundException)
            {
                // operation Delete must always work
            }
        }
    }
}