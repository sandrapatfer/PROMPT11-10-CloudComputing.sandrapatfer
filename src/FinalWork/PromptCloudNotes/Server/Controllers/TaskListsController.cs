using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PromptCloudNotes.Interfaces;

namespace Server.Controllers
{
    public class TaskListsController : Controller
    {
        private ITaskListManager _manager;

        public TaskListsController(ITaskListManager manager)
        {
            _manager = manager;
        }

        //
        // GET: /TaskLists/

        public ActionResult Index()
        {
            return View(_manager.GetAllLists(1).Select(l => new MvcModel.TaskList() { id = l.Id, name = l.Name, description = l.Description }));
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
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
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
