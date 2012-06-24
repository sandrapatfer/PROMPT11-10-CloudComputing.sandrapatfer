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

        public UsersController(IUserManager manager)
        {
            _manager = manager;
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

    }
}
