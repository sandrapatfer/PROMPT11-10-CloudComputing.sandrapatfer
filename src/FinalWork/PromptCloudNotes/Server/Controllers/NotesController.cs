using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PromptCloudNotes.Interfaces;

namespace Server.Controllers
{
    public class NotesController : Controller
    {
        private INoteManager _notesManager;

        public NotesController(INoteManager manager)
        {
            _notesManager = manager;
        }

        //
        // GET: /Notes/

        public ActionResult Index(int listId)
        {
            var notes = _notesManager.GetAllNotes(1, listId).Select(n => new Server.MvcModel.Note() { id = n.Id, listId = n.ParentList.Id, name = n.Name, description = n.Description });
            return View(notes);
        }

    }
}
