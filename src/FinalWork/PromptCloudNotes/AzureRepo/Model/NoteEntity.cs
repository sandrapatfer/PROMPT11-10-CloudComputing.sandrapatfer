using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure.StorageClient;

namespace PromptCloudNotes.AzureRepo.Model
{
    class NoteEntity : TableServiceEntity
    {
        public NoteEntity() { }

        public NoteEntity(int listId, int noteId)
            : base(listId.ToString(), noteId.ToString())
        {}

        public string Name { get; set; }
        public string Description { get; set; }
    }
}
