using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PromptCloudNotes.Interfaces.Managers;
using Server.Utils;
using Microsoft.IdentityModel.Claims;
using Exceptions;
using SignalR;
using Server.Utils.SignalR;

namespace Server.Controllers
{
    public class TaskListsController : BaseController
    {
        private ITaskListManager _manager;

        public TaskListsController(IUserManager userManager, ITaskListManager manager)
            : base(userManager)
        {
            _manager = manager;
        }

        //
        // GET: /TaskLists/

        public ActionResult Index()
        {
            var lists = _manager.GetAllLists(User.UniqueId);
            if (lists != null && lists.Count() > 0)
            {
                var selectedList = lists.First();
                var model = new Server.MvcModel.ListViewModel()
                {
                    Lists = lists,
                    SelectedList = selectedList
                };
                return View(model);
            }

            return RedirectToAction("Create", "TaskLists");
        }

        //
        // GET: /TaskLists/Details/{id}?creatorId={creatorId}

        public ActionResult Details(string id, string creatorId)
        {
            var lists = _manager.GetAllLists(User.UniqueId);
            if (lists == null || lists.Count() == 0 || !lists.Any(l => l.Id == id))
            {
                throw new HttpException(404, "List not found");
            }

            var selectedList = lists.First(l => l.Id == id);
            var model = new Server.MvcModel.ListViewModel()
                {
                    Lists = lists,
                    SelectedList = selectedList
                };
            return View(model);
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
                var newList = new PromptCloudNotes.Model.TaskList() { Name = list.name, Description = list.name, Creator = User };
                _manager.CreateTaskList(User, newList);
                return new RedirectJsonResult("Details", "TaskLists", newList.Id);
            }
            catch
            {
                throw new HttpException(500, "Error creating TaskList");
            }
        }
        
        //
        // POST: /TaskLists/Edit/{id}
        [HttpPost]
        public JsonResult Edit(string id, MvcModel.TaskList listData)
        {
            var creator = string.IsNullOrEmpty(listData.creatorId) ? User.UniqueId : listData.creatorId;
            var list = _manager.GetTaskList(User.UniqueId, id, creator);
            if (list == null)
            {
                throw new HttpException(404, "TaskList not found");
            }
            try
            {
                _manager.UpdateTaskList(User.UniqueId, id, creator,
                    new PromptCloudNotes.Model.TaskList() { Name = listData.name, Description = listData.description });
                return new RedirectJsonResult("Details", "TaskLists", id);
            }
            catch
            {
                throw new HttpException(500, "Error editing TaskList");
            }
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
        // GET: /TaskLists/Share/{id}
        
        public PartialViewResult Share(int id)
        {
            return PartialView("_Share", id);
        }

        //
        // POST: /TaskLists/Share/{id}

        [HttpPost]
        public JsonResult Share(string listId, string creatorId, string userId)
        {
            var creator = string.IsNullOrEmpty(creatorId) ? User.UniqueId : creatorId;
            try
            {
                _manager.ShareTaskList(User.UniqueId, listId, creator, userId);
                return null;
            }
            catch (ObjectNotFoundException)
            {
                throw new HttpException(404, "TaskList not found");
            }
            catch
            {
                throw new HttpException(500, "Error sharing TaskList");
            }
        }
    }
}
