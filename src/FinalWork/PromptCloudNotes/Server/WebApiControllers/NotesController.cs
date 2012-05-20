using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using StructureMap;
using PromptCloudNotes.Interfaces;
using System.Net;
using PromptCloudNotes.Model;
using System.Net.Http;
using System.Globalization;
using Exceptions;

namespace Server.WebApiControllers
{
    public class NotesController : ApiController
    {
        private INoteManager _manager;

        public NotesController()
        {
            _manager = ObjectFactory.GetInstance<INoteManager>();
        }

        // GET /api/lists/{lid}/notes
        public IEnumerable<WebApiModel.Note> Get(int lid)
        {
            // TODO get user info
            int userId = 1;
            return _manager.GetAllNotes(userId, lid).Select(n => new WebApiModel.Note() { title = n.Name, id = n.Id });
        }

        // GET /api/lists/{lid}/notes/{nid}
        public WebApiModel.Note Get(int lid, int nid)
        {
            var note = _manager.GetNote(lid, nid);
            if (note == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return new WebApiModel.Note() { title = note.Name, id = nid };
        }

        // POST /api/lists/{lid}/notes
        public HttpResponseMessage Post(int lid, WebApiModel.Note note)
        {
            // TODO get user info
            int userId = 1;
            var noteData = new Note() { Name = note.title };
            noteData = _manager.CreateNote(userId, lid, noteData);
            note.id = noteData.Id;

            var response = new HttpResponseMessage<WebApiModel.Note>(note)
            {
                StatusCode = HttpStatusCode.Created
            };
            response.Headers.Location = new Uri(Request.RequestUri,
                string.Format("/api/lists/{0}/notes/{1}",
                lid.ToString(CultureInfo.InvariantCulture),
                noteData.Id.ToString(CultureInfo.InvariantCulture)));
            return response;
        }

        // PUT /api/lists/{lid}/notes/{nid}
        public void Put(int lid, int nid, WebApiModel.Note note)
        {
            try
            {
                var noteData = new Note() { Name = note.title };
                _manager.UpdateNote(lid, nid, noteData);
            }
            catch (ObjectNotFoundException)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }

        // DELETE /api/lists/{lid}/notes/{nid}
        public void Delete(int lid, int nid)
        {
            try
            {
                _manager.DeleteNote(lid, nid);
            }
            catch (ObjectNotFoundException)
            {
                // operation Delete must always work
            }
        }
    }
}