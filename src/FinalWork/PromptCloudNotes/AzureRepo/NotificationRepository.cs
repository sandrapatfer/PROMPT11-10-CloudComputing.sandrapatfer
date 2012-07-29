using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PromptCloudNotes.Interfaces;
using PromptCloudNotes.Model;

namespace PromptCloudNotes.AzureRepo
{
    using Model;

    public class NotificationRepository : INotificationRepository
    {
        private const string TABLE_NAME = "NotificationTable";
        private AzureUtils.Table _tableUtils;

        public NotificationRepository()
        {
            //var connectionString = CloudStorageAccount.FromConfigurationSetting("DataConnectionString");
            var connectionString = "DefaultEndpointsProtocol=http;AccountName=spfcloudnotes;AccountKey=Gdi0M+gd1mO7a183WgRP8zxag5Fh2t0NNnh8Qvmz47V4vVDBe7JIXjdQS3wH0aLqRlpqNUqfzfuSC3TCgjVkLg==";
            _tableUtils = new AzureUtils.Table(connectionString);

            // ensure the table is created
            _tableUtils.CreateTable(TABLE_NAME);
        }

        public void CreateTaskListNotification(int userId, int listId, PromptCloudNotes.Model.Notification notificationData)
        {
            var allUserNotifications = _tableUtils.GetEntitiesInPartition<NotificationEntity>(TABLE_NAME, userId.ToString());
            int max = -1;
            if (allUserNotifications != null && allUserNotifications.Count() > 0)
            {
                max = allUserNotifications.Max(e => Convert.ToInt32(e.RowKey));
            }
            var newEntity = new NotificationEntity(userId, ++max) { ListId = listId, NoteId = -1 };
            if (_tableUtils.Insert(TABLE_NAME, newEntity))
            {
                return;
            }
            throw new InvalidOperationException();
        }

        public void CreateNoteNotification(int userId, int listId, int noteId, PromptCloudNotes.Model.Notification notificationData)
        {
            var allUserNotifications = _tableUtils.GetEntitiesInPartition<NotificationEntity>(TABLE_NAME, userId.ToString());
            int max = -1;
            if (allUserNotifications != null && allUserNotifications.Count() > 0)
            {
                max = allUserNotifications.Max(e => Convert.ToInt32(e.RowKey));
            }
            var newEntity = new NotificationEntity(userId, ++max) { ListId = listId, NoteId = noteId };
            if (_tableUtils.Insert(TABLE_NAME, newEntity))
            {
                return;
            }
            throw new InvalidOperationException();
        }

        public IEnumerable<PromptCloudNotes.Model.Notification> GetAll(int userId)
        {
            return _tableUtils.GetEntitiesInPartition<NotificationEntity>(TABLE_NAME, userId.ToString()).Select(e =>
                new Notification()
                {
                    Id = Convert.ToInt32(e.RowKey),
                    Task = e.NoteId == -1 ? (Task) new TaskList() { Id = e.ListId } :
                    new Note() { Id = e.NoteId, ParentList = new TaskList() { Id = e.ListId } }
                });
        }

        public PromptCloudNotes.Model.Notification Get(int notificationId)
        {
            throw new NotImplementedException();
        }

        public void Delete(int notificationId)
        {
            throw new NotImplementedException();
        }
    }
}
