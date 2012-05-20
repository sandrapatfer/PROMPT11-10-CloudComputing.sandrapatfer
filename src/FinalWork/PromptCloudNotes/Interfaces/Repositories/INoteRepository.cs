using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PromptCloudNotes.Model;

namespace PromptCloudNotes.Interfaces
{
    public interface INoteRepository
    {
        IEnumerable<Note> GetAll(int userId, int listId);

        Note Create(int userId, int listId, Note noteData);
        Note Get(int listId, int noteId);
        Note Update(int listId, int noteId, Note noteData);
        void Delete(int listId, int noteId);

        void ChangeOrder(int list, int noteId, int order);

        void ShareNote(int list, int noteId, int userId);
    }
}
