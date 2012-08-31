using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PromptCloudNotes.Interfaces.Managers;
using PromptCloudNotes.Model;
using Server.Utils;
using Exceptions;

namespace Server.Controllers
{
    public class NotesController : BaseController
    {
        private INoteManager _notesManager;

        public NotesController(IUserManager userManager, INoteManager manager)
            :base(userManager)
        {
            _notesManager = manager;
        }

        //
        // GET: /Notes/

        public JsonResult Index(string listId)
        {
            ViewBag.SelectedList = listId;
            var listNotes = _notesManager.GetAllNotes(User.UniqueId, listId);
            if (listNotes != null)
            {
                var notes = listNotes.Select(n => new Server.MvcModel.Note() { id = n.Id, listId = listId, name = n.Name, description = n.Description });
                return new JsonResult()
                {
                    Data = notes,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            throw new HttpException(404, "List not found");
        }

        //
        // POST: /Notes/Create

        [HttpPost]
        public JsonResult Create(MvcModel.Note note)
        {
            try
            {
                var newNote = new Note() { Name = note.name, Description = note.description, Creator = User };
                _notesManager.CreateNote(User, note.listId, note.listCreatorId, newNote);
                return new JsonResult()
                {
                    Data = new { id = newNote.Id }
                };
            }
            catch
            {
                throw new HttpException(500, "Error creating Note");
            }
        }

        //
        // POST: /Notes/Edit/{id}

        [HttpPost]
        public JsonResult Edit(string id, MvcModel.Note note)
        {
            try
            {
                var noteData = new Note() { Name = note.name, Description = note.description };
                _notesManager.UpdateNote(User.UniqueId, note.listId, note.id, noteData);
                return new JsonResult()
                {
                    Data = new { id = id }
                };
            }
            catch (ObjectNotFoundException)
            {
                throw new HttpException(404, "Note not found");
            }
            catch
            {
                throw new HttpException(500, "Error updating note");
            }
        }
    }
}
