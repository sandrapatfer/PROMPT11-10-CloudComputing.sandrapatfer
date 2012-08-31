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

namespace Server.WebApiControllers
{
    [Authorize]
    public class NotesController : ApiController
    {
        private IUserManager _userManager;
        private INoteManager _manager;

        public NotesController()
        {
            //_userManager = userManager;
            //_manager = manager;
        }

        // GET /api/notes
        public IEnumerable<WebApiModel.Note> Get()
        {
            // TODO get user info
            var user = Request.GetUserPrincipal() as User;
            //var user = _userManager.GetUser(Request.GetUserPrincipal().Identity.Name);
            return _manager.GetAllNotes(user.UniqueId).Select(n => new WebApiModel.Note() { title = n.Name, id = n.Id });
        }

        // GET /api/notes/{nid}
        public WebApiModel.Note Get(string lid, string nid)
        {
            var user = Request.GetUserPrincipal() as User;
            //var user = _userManager.GetUser(Request.GetUserPrincipal().Identity.Name);
            var note = _manager.GetNote(user.UniqueId, lid, nid);
            if (note == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return new WebApiModel.Note() { title = note.Name, id = nid };
        }

        // POST /api/lists/{lid}/notes
        public HttpResponseMessage Post(string lid, WebApiModel.Note note)
        {
            var user = Request.GetUserPrincipal() as User;
            //var user = _userManager.GetUser(Request.GetUserPrincipal().Identity.Name);
            var noteData = new Note() { Name = note.title };
            // TODO get creator id!!!
            _manager.CreateNote(user, lid, user.UniqueId, noteData);
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
        public void Put(string lid, string nid, WebApiModel.Note note)
        {
            try
            {
                var user = Request.GetUserPrincipal() as User;
                //var user = _userManager.GetUser(Request.GetUserPrincipal().Identity.Name);
                var noteData = new Note() { Name = note.title };
                _manager.UpdateNote(user.UniqueId, lid, nid, noteData);
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
                var user = Request.GetUserPrincipal() as User;
                //var user = _userManager.GetUser(Request.GetUserPrincipal().Identity.Name);
                _manager.DeleteNote(user.UniqueId, lid, user.UniqueId, nid);
            }
            catch (ObjectNotFoundException)
            {
                // operation Delete must always work
            }
        }
    }
}