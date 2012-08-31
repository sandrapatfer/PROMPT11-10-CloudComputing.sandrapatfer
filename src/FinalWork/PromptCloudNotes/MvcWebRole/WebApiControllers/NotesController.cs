using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using StructureMap;
using PromptCloudNotes.Interfaces.Managers;
using System.Net;
using PromptCloudNotes.Model;
using System.Net.Http;
using System.Globalization;
using Exceptions;
using Server.Utils;

namespace Server.WebApiControllers
{
    [Authorize]
    public class NotesController : ApiController
    {
        private INoteManager _manager;

        public NotesController()
        {
            _manager = ObjectFactory.GetInstance<INoteManager>();
        }

        // GET /api/notes
        public IEnumerable<WebApiModel.Note> Get()
        {
            var userId = (Request.GetUserPrincipal().Identity as UserIdentity).UserId;
            return _manager.GetAllNotes(userId).Select(n => new WebApiModel.Note() { title = n.Name, id = n.Id });
        }

        // GET /api/lists/{lid}/notes
        public IEnumerable<WebApiModel.Note> Get(string lid)
        {
            var userId = (Request.GetUserPrincipal().Identity as UserIdentity).UserId;
            return _manager.GetAllNotes(userId, lid).Select(n => new WebApiModel.Note() { title = n.Name, id = n.Id });
        }

        // GET /api/lists/{lid}/notes/{nid}
        public WebApiModel.Note Get(string lid, string nid)
        {
            var userId = (Request.GetUserPrincipal().Identity as UserIdentity).UserId;
            var note = _manager.GetNote(userId, lid, nid);
            if (note == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return new WebApiModel.Note() { title = note.Name, id = nid };
        }

        // POST /api/lists/{lid}/notes
        public HttpResponseMessage Post(string lid, WebApiModel.Note note)
        {
            var userId = (Request.GetUserPrincipal().Identity as UserIdentity).UserId;
            var noteData = new Note() { Name = note.title };
            _manager.CreateNote(new User { UniqueId = userId }, lid, string.IsNullOrEmpty(note.listCreatorId) ? userId : note.listCreatorId, noteData);

            var response = new HttpResponseMessage<WebApiModel.Note>(note)
            {
                StatusCode = HttpStatusCode.Created
            };
            response.Headers.Location = new Uri(Request.RequestUri,
                string.Format("/api/lists/{0}/notes/{1}", lid, noteData.Id));
            return response;
        }

        // PUT /api/lists/{lid}/notes/{nid}
        public void Put(string lid, string nid, WebApiModel.Note note)
        {
            try
            {
                var userId = (Request.GetUserPrincipal().Identity as UserIdentity).UserId;
                var noteData = new Note() { Name = note.title };
                _manager.UpdateNote(userId, lid, nid, noteData);
            }
            catch (ObjectNotFoundException)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }

        // DELETE /api/lists/{lid}/notes/{nid}
        public void Delete(string lid, string nid)
        {
            try
            {
                var userId = (Request.GetUserPrincipal().Identity as UserIdentity).UserId;
                // TODO get list creator id
                _manager.DeleteNote(userId, lid, userId, nid);
            }
            catch (ObjectNotFoundException)
            {
                // operation Delete must always work
            }
        }
    }
}