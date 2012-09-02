using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PromptCloudNotes.Model;

namespace PromptCloudNotes.Interfaces.Managers
{
    public interface INoteManager
    {

        IEnumerable<Note> GetAllNotes(string userId);
        IEnumerable<Note> GetAllNotes(string userId, string listId);

        Note GetNote(string userId, string listId, string noteId);

        void CreateNote(User user, string listId, string creatorId, Note noteData);

        void UpdateNote(string userId, string listId, string noteId, Note noteData);

        void DeleteNote(string userId, string listId, string creatorId, string noteId);

        void ShareNote(string userId, string listId, string noteId, string shareUserId);

        void ChangeOrder(string userId, string listId, string noteId, int order);
    }
}
