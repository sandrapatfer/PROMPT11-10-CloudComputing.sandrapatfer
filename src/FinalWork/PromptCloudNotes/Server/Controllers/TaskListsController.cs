using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PromptCloudNotes.Interfaces;
using PromptCloudNotes.Model;
using Server.Utils;

namespace Server.Controllers
{
    public class TaskListsController : Controller
    {
        private ITaskListManager _manager;
        private IUserManager _userManager;

        public TaskListsController(IUserManager userManager, ITaskListManager manager)
        {
            _userManager = userManager;
            _manager = manager;
        }

        //
        // GET: /TaskLists/

        public ActionResult Index()
        {
            var user = _userManager.GetUser(User.Identity.Name);
            return View(_manager.GetAllLists(user.Id).Select(l => new MvcModel.TaskList() { id = l.Id, name = l.Name, description = l.Description }));
        }

        //
        // GET: /TaskLists/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // POST: /TaskLists/Create

        [HttpPost]
        public JsonResult Create(MvcModel.TaskList list)
        {
            try
            {
                var user = _userManager.GetUser(User.Identity.Name);
                var newList = _manager.CreateTaskList(user.Id, new TaskList() { Name = list.name, Description = list.name, Creator = user });
                return new RedirectJsonResult("Index", "Notes", newList.Id);
//                return new RedirectJsonResult("Index", "Notes", new { listId = newList.Id });
            }
            catch
            {
                throw new HttpException(500, "Error creating TaskList");
            }
        }
        
        //
        // GET: /TaskLists/Edit/5
 
        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /TaskLists/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here
 
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /TaskLists/Delete/5
 
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /TaskLists/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
 
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
