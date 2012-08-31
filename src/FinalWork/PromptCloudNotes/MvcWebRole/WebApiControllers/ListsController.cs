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

namespace Server.WebApiControllers
{
    //[Authorize]
    public class ListsController : ApiController
    {
        private ITaskListManager _manager;
        private IUserManager _userManager;

        public ListsController()
        {
            //_manager = ObjectFactory.GetInstance<ITaskListManager>();
            //_userManager = ObjectFactory.GetInstance<IUserManager>();
        }

        // GET /api/lists
        public IEnumerable<WebApiModel.TaskList> Get()
        {
/*            var user = Request.GetUserPrincipal() as User;
            //var user = _userManager.GetUser(Request.GetUserPrincipal().Identity.Name);
            return _manager.GetAllLists(user.UniqueId).Select(l => new WebApiModel.TaskList() { id = l.Id, name = l.Name });*/
            return _manager.GetAllLists().Select(l => new WebApiModel.TaskList() { id = l.Id, name = l.Name });*/
        }

        // GET /api/lists/{id}
        public WebApiModel.TaskList Get(string id)
        {
/*            var user = Request.GetUserPrincipal() as User;
            //var user = _userManager.GetUser(Request.GetUserPrincipal().Identity.Name);
            var list = _manager.GetTaskList(user.UniqueId, id, user.UniqueId);
            if (list == null) // TODO is it null or exception?
            {
                // TODO create a better response
                // see http://www.asp.net/web-api/overview/web-api-routing-and-actions/exception-handling
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return new WebApiModel.TaskList() { id = list.Id, name = list.Name };*/
            return null;

        }

        // POST /api/lists
        public HttpResponseMessage Post(WebApiModel.TaskList list)
        {
/*            var user = Request.GetUserPrincipal() as User;
            //var user = _userManager.GetUser(Request.GetUserPrincipal().Identity.Name);

            var listData = new TaskList() { Name = list.name };
            _manager.CreateTaskList(user, listData);
            list.id = listData.Id;

            var response = new HttpResponseMessage<WebApiModel.TaskList>(list)
            {
                StatusCode = HttpStatusCode.Created
            };
            response.Headers.Location = new Uri(Request.RequestUri,
                "/api/lists/" + listData.Id.ToString(CultureInfo.InvariantCulture));

            return response;*/
            return null;
        }

        // PUT /api/lists/{id}
        public void Put(string id, WebApiModel.TaskList list)
        {
/*            try
            {
                var user = Request.GetUserPrincipal() as User;
                //var user = _userManager.GetUser(Request.GetUserPrincipal().Identity.Name);
                var listData = new TaskList() { Name = list.name };
                _manager.UpdateTaskList(user.UniqueId, id, user.UniqueId, listData);
            }
            catch (ObjectNotFoundException)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }*/
        }

        // DELETE /api/lists/{id}
        public void Delete(string id)
        {
/*            try
            {
                var user = Request.GetUserPrincipal() as User;
                //var user = _userManager.GetUser(Request.GetUserPrincipal().Identity.Name);
                _manager.DeleteTaskList(user.UniqueId, id, user.UniqueId);
            }
            catch (ObjectNotFoundException)
            {
                // operation Delete must always work
            }*/
        }
    }
}