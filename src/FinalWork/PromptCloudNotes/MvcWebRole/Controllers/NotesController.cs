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
    public class NotesController : Controller
    {
        private INoteManager _notesManager;
        private IUserManager _userManager;

        public NotesController(IUserManager userManager, INoteManager manager)
        {
            _notesManager = manager;
            _userManager = userManager;
        }

        //
        // GET: /Notes/

        public ActionResult Index(int listId)
        {
            ViewBag.SelectedList = listId;
            var user = _userManager.GetUser(User.Identity.Name);
            var listNotes = _notesManager.GetAllNotes(user.Id, listId);
            if (listNotes != null)
            {
                var notes = listNotes.Select(n => new Server.MvcModel.Note() { id = n.Id, listId = listId, name = n.Name, description = n.Description });
                return View(notes);
            }
            else
            {
                return View();
            }
        }

        //
        // POST: /TaskLists/Create

        [HttpPost]
        public JsonResult Create(MvcModel.Note note)
        {
            try
            {
                var user = _userManager.GetUser(User.Identity.Name);
                var newNote = _notesManager.CreateNote(user.Id, note.listId, new Note() { Name = note.name, Description = note.description, Creator = user });
                return new RedirectJsonResult("Index", "Notes", note.listId);
            }
            catch
            {
                throw new HttpException(500, "Error creating Note");
            }
        }

    }
}
