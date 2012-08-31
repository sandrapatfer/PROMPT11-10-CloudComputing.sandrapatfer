using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PromptCloudNotes.Interfaces.Repositories;
using PromptCloudNotes.Model;
using Microsoft.WindowsAzure;

namespace PromptCloudNotes.AzureRepo
{
    using Model;

    public class NoteRepository : AzureTable<Note, NoteEntity>, INoteRepository
    {
        private const string TABLE_NAME = "NoteTable";

        public NoteRepository()
            : base(TABLE_NAME)
        { }

        public IEnumerable<Note> GetAll()
        {
            return GetAll(e => e.GetNote());
        }

        public new IEnumerable<Note> GetAll(string partitionKey)
        {
            return GetAll(partitionKey, e => e.GetNote());
        }

        public new Note Get(string partitionKey, string rowKey)
        {
            return Get(partitionKey, rowKey, e => e.GetNote());
        }

        public void Create(Note newEntity)
        {
            newEntity.Id = Guid.NewGuid().ToString();
            Create(new NoteEntity(newEntity));
        }

        public void Update(string partitionKey, string rowKey, Note changedEntity)
        {
            var entity = base.Get(partitionKey, rowKey); 
            entity.UpdateData(changedEntity);
            Update(entity);
        }

        public void Delete(string partitionKey, string rowKey)
        {
            var entity = base.Get(partitionKey, rowKey);
            DeleteEntity(entity);
        }

/*        private const string TABLE_NAME = "NoteTable";
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

        public IQueryable<Note> GetAll(string userId, string listId)
        {
            // TODO validate the list for the user (remember it is used in next function passing -1)

            var list = new TaskList() { Id = listId };
            return _tableUtils.GetEntitiesInPartition<NoteEntity>(TABLE_NAME, listId.ToString()).
                Select(e => new Note() { Id = e.RowKey, Name = e.Name, Description = e.Description, ParentList = list });
        }

        public IQueryable<Note> GetAll(string userId)
        {
            var userLists = _tableUtils.GetEntitiesInPartition<TaskListEntity>(LIST_TABLE_NAME, userId.ToString());
            return userLists.Aggregate(new List<Note>(), (Func<List<Note>, TaskListEntity, List<Note>>)((l, e) =>
            {
                l.AddRange(GetAll(userId, e.RowKey));
                return l;
            })).AsQueryable<Note>();
        }

        public Note Create(string userId, string listId, Note noteData)
        {
            noteData.Id = Guid.NewGuid().ToString();
            var newEntity = new NoteEntity(listId, noteData.Id) { Name = noteData.Name, Description = noteData.Description, CreatorId = userId };
            if (_tableUtils.Insert(TABLE_NAME, newEntity))
            {
                return noteData;
            }
            // TODO excepcao nova
            throw new InvalidOperationException();
        }

        public Note Get(string listId, string noteId)
        {
            var entity = _tableUtils.GetEntity<NoteEntity>(TABLE_NAME, listId.ToString(), noteId.ToString());
            if (entity != null)
            {
                return entity.GetNote();
            }
            return null;
        }

        public Note Update(string listId, string noteId, Note noteData)
        {
            var entity = _tableUtils.GetEntity<NoteEntity>(TABLE_NAME, listId.ToString(), noteId.ToString());
            if (entity != null)
            {
                entity.UpdateData(noteData);
                if (!_tableUtils.MergeUpdate(TABLE_NAME, entity))
                {
                    // TODO exception?
                }
            }

            return noteData;
        }

        public void Delete(string listId, string noteId)
        {
            throw new NotImplementedException();
        }

        public void ChangeOrder(string listId, string noteId, int order)
        {
            throw new NotImplementedException();
        }

        public void ShareNote(string listId, string noteId, string userId)
        {
            throw new NotImplementedException();
        }*/

    }
}
