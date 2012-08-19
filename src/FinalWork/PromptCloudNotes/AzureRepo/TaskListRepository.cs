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

    public class TaskListRepository : ITaskListRepository
    {
        private const string TABLE_NAME = "TaskListTable";
        private const string SHARE_TABLE_NAME = "ShareTaskListTable";
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

        public IEnumerable<TaskList> GetAll(int userId)
        {
            // TODO preencher os outros atributos como futuros para serem preenchidos se forem necessarios

            return _tableUtils.GetEntitiesInPartition<TaskListEntity>(TABLE_NAME, userId.ToString()).
                Select(e => new TaskList() { Id = Convert.ToInt32(e.RowKey), Name = e.Name, Description = e.Description });
        }

        public TaskList Create(User user, TaskList listData)
        {
            var allLists = _tableUtils.GetAllEntities<TaskListEntity>(TABLE_NAME);
            int max = -1;
            if (allLists != null && allLists.Count() > 0)
            {
                max = allLists.Max(e => Convert.ToInt32(e.RowKey));
            }
            var newEntity = new TaskListEntity(user.Id, ++max) { Name = listData.Name, Description = listData.Description };
            if (_tableUtils.Insert(TABLE_NAME, newEntity))
            {
                listData.Id = Convert.ToInt32(newEntity.RowKey);

                var newShareEntity = new ShareTaskListEntity(listData.Id, user.Id);
                newShareEntity.UserName = user.UserName;
                if (_tableUtils.Insert(SHARE_TABLE_NAME, newShareEntity))
                {
                    return listData;
                }
            }

            // TODO excepcao nova
            throw new InvalidOperationException();
        }

        public TaskList Get(int listId)
        {
            var entity = _tableUtils.GetEntitiesInRow<TaskListEntity>(TABLE_NAME, listId.ToString()).FirstOrDefault();
            if (entity != null)
            {
                return new TaskList()
                {
                    Id = listId,
                    Name = entity.Name,
                    Description = entity.Description
                    /*,
                    Creator = user*/
                };
            }

            return null;
        }

        public TaskList GetWithUsers(int listId)
        {
            var entity = _tableUtils.GetEntitiesInRow<TaskListEntity>(TABLE_NAME, listId.ToString()).FirstOrDefault();
            if (entity != null)
            {
                var users = _tableUtils.GetEntitiesInPartition<ShareTaskListEntity>(SHARE_TABLE_NAME, listId.ToString());
                return new TaskList() { Id = listId, Name = entity.Name, Description = entity.Description,
                    Users = users.Select( u => new User() { Id = u.Id, UserName = u.UserName }).ToList()
                    /*,
                    Creator = user*/
                };
            }

            return null;
        }

        public void Update(int listId, TaskList listData)
        {
            var entity = _tableUtils.GetEntitiesInRow<TaskListEntity>(TABLE_NAME, listId.ToString()).FirstOrDefault();
            if (entity != null)
            {
                entity.Name = listData.Name;
                entity.Description = listData.Description;
                _tableUtils.MergeUpdate(TABLE_NAME, entity);
            }
        }

        public void Delete(int listId)
        {
            throw new NotImplementedException();
        }

        public void Share(int listId, User user)
        {
            var newEntity = new ShareTaskListEntity(listId, user.Id);
            newEntity.UserName = user.UserName;
            if (!_tableUtils.Insert(SHARE_TABLE_NAME, newEntity))
            {
                // TODO excepcao nova
                throw new InvalidOperationException();
            }
        }
    }
}
