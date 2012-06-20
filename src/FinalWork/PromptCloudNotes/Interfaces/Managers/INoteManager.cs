using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PromptCloudNotes.Model;

namespace PromptCloudNotes.Interfaces
{
    public interface INoteManager
    {
        Note CreateNote(int userId, int listId, Note noteData);

        IEnumerable<Note> GetAllNotes(int userId);
        IEnumerable<Note> GetAllNotes(int userId, int listId);

        Note GetNote(int userId, int listId, int noteId);

        void UpdateNote(int userId, int listId, int noteId, Note noteData);

        void DeleteNote(int userId, int listId, int noteId);

        void ShareNote(int userId, int listId, int noteId, int shareUserId);

        void ChangeOrder(int userId, int listId, int noteId, int order);
    }
}
