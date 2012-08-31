using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PromptCloudNotes.Model;

namespace PromptCloudNotes.Interfaces.Repositories
{
    public interface INoteRepository : IRepository<Note>
    {
/*        //IEnumerable<Note> GetAll(string userId);
        IQueryable<Note> GetAll(string userId, string listId);

        Note Create(string userId, string listId, Note noteData);

        //Note Get(string noteId);
        Note Get(string listId, string noteId);

        //Note Update(string noteId, Note noteData);
        Note Update(string listId, string noteId, Note noteData);

        //void Delete(string noteId);
        void Delete(string listId, string noteId);

        //void ChangeOrder(string noteId, int order);
        void ChangeOrder(string listId, string noteId, int order);

        //void ShareNote(string noteId, string userId);
        void ShareNote(string listId, string noteId, string userId);*/
    }
}
