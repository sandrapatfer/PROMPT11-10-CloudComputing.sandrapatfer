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
            return Get(partitionKey, rowKey, e => e.GetNotification());
        }

        public void Create(Notification newEntity)
        {
            Create(new NotificationEntity(newEntity));
        }

        public void Update(string partitionKey, string rowKey, Notification changedEntity)
        {
            throw new NotImplementedException();
        }

        public void Delete(string partitionKey, string rowKey)
        {
            throw new NotImplementedException();
        }
    }
}
