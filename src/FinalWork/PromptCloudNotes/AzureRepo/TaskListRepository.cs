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
    using Exceptions;

    public class TaskListRepository : AzureRepository<TaskList, TaskListEntity>, ITaskListRepository
    {
        private const string TABLE_NAME = "TaskListTable";

        public TaskListRepository()
            : base(TABLE_NAME)
        { }

        public IEnumerable<TaskList> GetAll()
        {
            throw new NotImplementedException();
        }

        public new IEnumerable<TaskList> GetAll(string partitionKey)
        {
            return GetAll(partitionKey, l => l.GetTaskList());
        }

        public new TaskList Get(string partitionKey, string rowKey)
        {
            return Get(partitionKey, rowKey, l => l.GetTaskList());
        }

        public void Create(TaskList newEntity)
        {
            newEntity.Id = Guid.NewGuid().ToString();
            Create(new TaskListEntity(newEntity));
        }

        public void Update(string partitionKey, string rowKey, TaskList changedEntity)
        {
            var list = base.Get(partitionKey, rowKey);
            if (list == null)
            {
                throw new ObjectNotFoundException();
            }
            list.Update(changedEntity);
            Update(list);
        }

        public void Delete(string partitionKey, string rowKey)
        {
            throw new NotImplementedException();
        }

        /*private const string SHARE_TABLE_NAME = "ShareTaskListTable";
        private AzureUtils.Table _tableUtils;

        public TaskListRepository()
        {
            //var connectionString = CloudStorageAccount.FromConfigurationSetting("DataConnectionString");
            var connectionString = "DefaultEndpointsProtocol=http;AccountName=spfcloudnotes;AccountKey=Gdi0M+gd1mO7a183WgRP8zxag5Fh2t0NNnh8Qvmz47V4vVDBe7JIXjdQS3wH0aLqRlpqNUqfzfuSC3TCgjVkLg==";
            _tableUtils = new AzureUtils.Table(connectionString);

            // ensure the tables are created
            _tableUtils.CreateTable(TABLE_NAME);
            _tableUtils.CreateTable(SHARE_TABLE_NAME);
        }

        public IEnumerable<TaskList> GetAll(string userId)
        {
            return _tableUtils.GetEntitiesInPartition<TaskListEntity>(TABLE_NAME, userId).
                Select(e => new TaskList() { Id = e.RowKey, Name = e.Name, Description = e.Description });
        }

        public TaskList Create(User user, TaskList listData)
        {
            listData.Id = Guid.NewGuid().ToString();
            var newEntity = new TaskListEntity(user.UniqueId, listData.Id) { Name = listData.Name, Description = listData.Description };
            if (_tableUtils.Insert(TABLE_NAME, newEntity))
            {

                var newShareEntity = new ShareTaskListEntity(listData.Id, user.UniqueId);
                if (_tableUtils.Insert(SHARE_TABLE_NAME, newShareEntity))
                {
                    return listData;
                }
            }

            // TODO excepcao nova
            throw new InvalidOperationException();
        }

        public TaskList Get(string creatorId, string listId)
        {
            var entity = _tableUtils.GetEntity<TaskListEntity>(TABLE_NAME, creatorId, listId);
            if (entity != null)
            {
                return new TaskList()
                {
                    Id = listId,
                    Name = entity.Name,
                    Description = entity.Description
                    /*,
                    Creator = user*//*
                };
            }

            return null;
        }

        public TaskList GetWithUsers(string creatorId, string listId)
        {
            var entity = _tableUtils.GetEntity<TaskListEntity>(TABLE_NAME, creatorId, listId);
            if (entity != null)
            {
                var users = _tableUtils.GetEntitiesInPartition<ShareTaskListEntity>(SHARE_TABLE_NAME, listId.ToString());
                return new TaskList() { Id = listId, Name = entity.Name, Description = entity.Description,
                    Users = users.Select( u => new User() { UniqueId = u.PartitionKey }).ToList()
                    /*,
                    Creator = user*//*
                };
            }

            return null;
        }

        public void Update(string creatorId, string listId, TaskList listData)
        {
            var entity = _tableUtils.GetEntity<TaskListEntity>(TABLE_NAME, creatorId, listId);
            if (entity != null)
            {
                entity.Name = listData.Name;
                entity.Description = listData.Description;
                _tableUtils.MergeUpdate(TABLE_NAME, entity);
            }
        }

        public void Delete(string listId)
        {
            throw new NotImplementedException();
        }

        public void Share(string listId, string userId)
        {
            var newEntity = new ShareTaskListEntity(listId, userId);
            if (!_tableUtils.Insert(SHARE_TABLE_NAME, newEntity))
            {
                // TODO excepcao nova
                throw new InvalidOperationException();
            }
        }*/

    }
}
