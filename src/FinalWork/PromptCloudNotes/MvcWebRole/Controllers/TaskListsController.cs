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
        // GET: /TaskLists/Create

        public ActionResult Create()
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
        public PartialViewResult Edit(int id)
        {
            var user = _userManager.GetUser(User.Identity.Name);
            var list = _manager.GetTaskList(user.Id, id);
            if (list == null)
            {
                throw new HttpException(404, "TaskList not found");
            }
            return PartialView("_ModalEdit", new MvcModel.TaskList() { id = list.Id, name = list.Name, description = list.Description });
        }

        //
        // POST: /TaskLists/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, MvcModel.TaskList listData)
        {
            var user = _userManager.GetUser(User.Identity.Name);
            var list = _manager.GetTaskList(user.Id, id);
            if (list == null)
            {
                throw new HttpException(404, "TaskList not found");
            }
            try
            {
                _manager.UpdateTaskList(user.Id, id, new TaskList() { Name = listData.name, Description = listData.description });
                return new RedirectJsonResult("Index", "Notes", id);
            }
            catch
            {
                throw new HttpException(500, "Error editing TaskList");
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

        //
        // GET: /TaskLists/Share/5
        public PartialViewResult Share(int id)
        {
            return PartialView("_Share", id);
        }

        //
        // POST: /TaskLists/Share/5
        [HttpPost]
        public ActionResult Share(int id, int userId)
        {
            var user = _userManager.GetUser(User.Identity.Name);
            var list = _manager.GetTaskList(user.Id, id);
            if (list == null)
            {
                throw new HttpException(404, "TaskList not found");
            }
            try
            {
                _manager.ShareTaskList(user.Id, id, userId);
                return new RedirectJsonResult("Index", "Notes", id);
            }
            catch
            {
                throw new HttpException(500, "Error sharing TaskList");
            }
        }
    }
}
