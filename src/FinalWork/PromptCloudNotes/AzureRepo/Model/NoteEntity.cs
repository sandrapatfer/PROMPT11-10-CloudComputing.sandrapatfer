using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure.StorageClient;

namespace PromptCloudNotes.AzureRepo.Model
{
    public class NoteEntity : TableServiceEntity
    {
        public NoteEntity() { }

        public NoteEntity(string listId, string noteId)
            : base(listId, noteId)
        {}

        public NoteEntity(PromptCloudNotes.Model.Note note)
            :base(note.ParentList.Id, note.Id)
        {
            Name = note.Name;
            Description = note.Description;
            CreatorId = note.Creator.UniqueId;
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public string CreatorId { get; set; }

        public PromptCloudNotes.Model.Note GetNote()
        {
            return new PromptCloudNotes.Model.Note() { Id = RowKey, Name = Name, Description = Description, Creator = new PromptCloudNotes.Model.User() { UniqueId = CreatorId } };
        }

        public void UpdateData(PromptCloudNotes.Model.Note noteData)
        {
            Name = noteData.Name;
            Description = noteData.Description;
        }
    }
}
