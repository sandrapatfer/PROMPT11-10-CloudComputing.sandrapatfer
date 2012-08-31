using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PromptCloudNotes.Interfaces.Repositories;
using PromptCloudNotes.Model;

namespace PromptCloudNotes.AzureRepo
{
    using Model;

    public class NotificationRepository : AzureRepository<Notification, NotificationEntity>, INotificationRepository
    {
        private const string TABLE_NAME = "NotificationTable";

        public NotificationRepository()
            : base(TABLE_NAME)
        { }

        public IEnumerable<Notification> GetAll()
        {
            return GetAll(n => n.GetNotification());
        }

        public new IEnumerable<Notification> GetAll(string partitionKey)
        {
            return GetAll(partitionKey, n => n.GetNotification());
        }

        public new Notification Get(string partitionKey, string rowKey)
        {
            throw new NotImplementedException();
        }

        public void Create(Notification newEntity)
        {
        }

        public void Update(string partitionKey, string rowKey, Notification changedEntity)
        {
            throw new NotImplementedException();
        }

        public void Delete(string partitionKey, string rowKey)
        {
            throw new NotImplementedException();
        }


/*        private AzureUtils.Table _tableUtils;

        public NotificationRepository()
        {
            //var connectionString = CloudStorageAccount.FromConfigurationSetting("DataConnectionString");
            var connectionString = "DefaultEndpointsProtocol=http;AccountName=spfcloudnotes;AccountKey=Gdi0M+gd1mO7a183WgRP8zxag5Fh2t0NNnh8Qvmz47V4vVDBe7JIXjdQS3wH0aLqRlpqNUqfzfuSC3TCgjVkLg==";
            _tableUtils = new AzureUtils.Table(connectionString);

            // ensure the table is created
            _tableUtils.CreateTable(TABLE_NAME);
        }

        public void CreateTaskListNotification(string userId, string listId, PromptCloudNotes.Model.Notification notificationData)
        {
            var newEntity = new NotificationEntity(userId, Guid.NewGuid().ToString()) { TaskId = listId, IsList = true };
            if (_tableUtils.Insert(TABLE_NAME, newEntity))
            {
                return;
            }
            throw new InvalidOperationException();
        }

        public void CreateNoteNotification(string userId, string noteId, PromptCloudNotes.Model.Notification notificationData)
        {
            var newEntity = new NotificationEntity(userId, Guid.NewGuid().ToString()) { TaskId = noteId, IsList = false };
            if (_tableUtils.Insert(TABLE_NAME, newEntity))
            {
                return;
            }
            throw new InvalidOperationException();
        }

        public IEnumerable<PromptCloudNotes.Model.Notification> GetAll(string userId)
        {
            return _tableUtils.GetEntitiesInPartition<NotificationEntity>(TABLE_NAME, userId.ToString()).Select(e =>
                new Notification()
                {
                    Id = e.RowKey,
                    Task = e.IsList? (Task) new TaskList() { Id = e.TaskId } :
                    new Note() { Id = e.TaskId }
                });
        }

        public PromptCloudNotes.Model.Notification Get(string notificationId)
        {
            throw new NotImplementedException();
        }

        public void Delete(string notificationId)
        {
            throw new NotImplementedException();
        }*/

    }
}
