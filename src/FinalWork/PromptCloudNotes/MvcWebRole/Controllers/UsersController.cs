using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PromptCloudNotes.Interfaces;

namespace Server.Controllers
{
    public class UsersController : Controller
    {
        private IUserManager _manager;
        private ITaskListManager _listManager;

        public UsersController(IUserManager manager, ITaskListManager listManager)
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
                var user = _manager.GetUser(User.Identity.Name);
                list = list.Where(u => u.Id != user.Id);
            }
            return new JsonResult() { 
                Data = list.Select(u => new MvcModel.User() { id = u.Id, name = u.UserName }),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        
        //
        // GET: /Users/TaskListNotShared?listId
        public JsonResult TaskListNotShared(int listId)
        {
            var user = _manager.GetUser(User.Identity.Name);
            var allUsers = _manager.GetAllUsers();
            var list = _listManager.GetTaskList(user.Id, listId);
            return new JsonResult() { 
                Data = allUsers.Except(list.Users).Select(u => new MvcModel.User() { id = u.Id, name = u.UserName }),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
    }
}
