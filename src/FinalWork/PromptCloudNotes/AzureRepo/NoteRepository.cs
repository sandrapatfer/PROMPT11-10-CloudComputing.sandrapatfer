using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PromptCloudNotes.Interfaces;
using PromptCloudNotes.Model;
using Microsoft.WindowsAzure;

namespace PromptCloudNotes.AzureRepo
{
    using Model;

    public class NoteRepository : INoteRepository
    {
        private const string TABLE_NAME = "NoteTable";
        private const string LIST_TABLE_NAME = "TaskListTable";
        private AzureUtils.Table _tableUtils;

        public NoteRepository()
        {
            //var connectionString = CloudStorageAccount.FromConfigurationSetting("DataConnectionString");
            var connectionString = "DefaultEndpointsProtocol=http;AccountName=spfcloudnotes;AccountKey=Gdi0M+gd1mO7a183WgRP8zxag5Fh2t0NNnh8Qvmz47V4vVDBe7JIXjdQS3wH0aLqRlpqNUqfzfuSC3TCgjVkLg==";
            _tableUtils = new AzureUtils.Table(connectionString);

            // ensure the table is created
            _tableUtils.CreateTable(TABLE_NAME);
        }

        public IEnumerable<Note> GetAll(int userId, int listId)
        {
            // TODO validate the list for the user (remember it is used in next function passing -1)

            return _tableUtils.GetEntitiesInPartition<NoteEntity>(TABLE_NAME, listId.ToString()).
                Select(e => new Note() { Id = Convert.ToInt32(e.RowKey), Name = e.Name, Description = e.Description });
        }

        IEnumerable<Note> INoteRepository.GetAll(int userId)
        {
            var userLists = _tableUtils.GetEntitiesInPartition<TaskListEntity>(LIST_TABLE_NAME, userId.ToString());
            return userLists.Aggregate(new List<Note>(), (l, e) =>
            {
                l.AddRange(GetAll(-1, Convert.ToInt32(e.RowKey)));
                return l;
            });
        }

        public Note Create(int userId, int listId, Note noteData)
        {
            // TODO validate user and list

            // TODO see how to improve this query...
            var allNotes = _tableUtils.GetAllEntities<NoteEntity>(TABLE_NAME);
            int max = -1;
            if (allNotes != null && allNotes.Count() > 0)
            {
                max = allNotes.Max(e => Convert.ToInt32(e.RowKey));
            }
            var newEntity = new NoteEntity(listId, ++max) { Name = noteData.Name, Description = noteData.Description };
            if (_tableUtils.Insert(TABLE_NAME, newEntity))
            {
                noteData.Id = Convert.ToInt32(newEntity.RowKey);
                return noteData;
            }
            // TODO excepcao nova
            throw new InvalidOperationException();
        }

        public Note Get(int listId, int noteId)
        {
            var entity = _tableUtils.GetEntity<NoteEntity>(TABLE_NAME, listId.ToString(), noteId.ToString());
            if (entity != null)
            {
                return new Note() { Id = noteId, Name = entity.Name, Description = entity.Description };
            }
            return null;
        }

        public Note Update(int listId, int noteId, Note noteData)
        {
            var entity = _tableUtils.GetEntity<NoteEntity>(TABLE_NAME, listId.ToString(), noteId.ToString());
            if (entity != null)
            {
                entity.Name = noteData.Name;
                entity.Description = noteData.Description;
                if (!_tableUtils.MergeUpdate(TABLE_NAME, entity))
                {
                    // TODO exception?
                }
            }

            return noteData;
        }

        public void Delete(int listId, int noteId)
        {
            throw new NotImplementedException();
        }

        public void ChangeOrder(int list, int noteId, int order)
        {
            throw new NotImplementedException();
        }

        public void ShareNote(int list, int noteId, int userId)
        {
            throw new NotImplementedException();
        }
    }
}
