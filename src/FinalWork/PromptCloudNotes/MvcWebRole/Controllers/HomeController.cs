using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Server.Utils;
using PromptCloudNotes.Interfaces.Managers;
using PromptCloudNotes.Model;

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
                var user = Session["user"] as User;
                if (user == null)
                {
                    return RedirectToAction("LogOff", "Account");
                }
                var lists = listMgr.GetAllLists(user.UniqueId);
                if (lists.Count() > 0)
                {
                    return RedirectToAction("Index", "Notes", new { listId = lists.First().Id });
                }
                else
                {
                    return RedirectToAction("Create", "TaskLists");
                }
            }
            return View();
        }
    }
}
