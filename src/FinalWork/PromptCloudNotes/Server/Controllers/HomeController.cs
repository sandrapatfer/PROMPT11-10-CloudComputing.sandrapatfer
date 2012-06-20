using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Server.Utils;
using PromptCloudNotes.Interfaces;

namespace Server.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        [AllowAnonymous]
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                // redirect the user to the first list of notes
                var listMgr = DependencyResolver.Current.GetService<ITaskListManager>();
                var userMgr = DependencyResolver.Current.GetService<IUserManager>();
                var lists = listMgr.GetAllLists(userMgr.GetUser(User.Identity.Name).Id);
                if (lists.Count() > 0)
                {
                    return RedirectToAction("Index", "Notes", new { listId = lists.First().Id });
                }
                else
                {
                    return RedirectToAction("Index", "TaskLists");
                }
            }
            return View();
        }

    }
}
