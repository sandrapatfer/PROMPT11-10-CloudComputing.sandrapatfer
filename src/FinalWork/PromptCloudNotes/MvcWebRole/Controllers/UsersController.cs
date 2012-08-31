using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PromptCloudNotes.Interfaces.Managers;
using PromptCloudNotes.Model;

namespace Server.Controllers
{
    public class UsersController : BaseController
    {
        private IUserManager _manager;
        private ITaskListManager _listManager;

        public UsersController(IUserManager manager, ITaskListManager listManager)
            : base(manager)
        {
            _manager = manager;
            _listManager = listManager;
        }


        //
        // GET: /Users/
        public JsonResult Index(int exclude)
        {
            var list = _manager.GetAllUsers();
            if (exclude == 1)
            {
                list = list.Where(u => u.UniqueId != User.UniqueId);
            }
            return new JsonResult() { 
                Data = list.Select(u => new MvcModel.User() { id = u.UniqueId, name = u.Name }),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        
        //
        // GET: /Users/TaskListNotShared
        public JsonResult TaskListNotShared(string listId, string creatorId)
        {
            var allUsers = _manager.GetAllUsers();
            if (creatorId == null)
            {
                creatorId = User.UniqueId;
            }
            var list = _listManager.GetTaskList(User.UniqueId, listId, creatorId);
            var usersNotShared = allUsers.Where(u => !list.Users.Any(lu => lu.UniqueId == u.UniqueId)); 
            return new JsonResult() {
                Data = usersNotShared.Select(u => new MvcModel.User() { id = u.UniqueId, name = u.Name }),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
    }
}
