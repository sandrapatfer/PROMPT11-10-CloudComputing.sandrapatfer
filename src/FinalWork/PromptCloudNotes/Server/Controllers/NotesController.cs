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
            var user = _userManager.GetUser(User.Identity.Name);
            var notes = _notesManager.GetAllNotes(user.Id, listId).Select(n => new Server.MvcModel.Note() { id = n.Id, listId = n.ParentList.Id, name = n.Name, description = n.Description });
            return View(notes);
        }

    }
}
