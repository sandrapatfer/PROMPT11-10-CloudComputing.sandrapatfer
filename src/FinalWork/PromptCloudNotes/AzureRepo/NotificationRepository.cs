using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PromptCloudNotes.Interfaces.Repositories;
using PromptCloudNotes.Model;

namespace PromptCloudNotes.AzureRepo
{
    using Model;

    public class NotificationRepository : AzureTable<Notification, NotificationEntity>, INotificationRepository
    {
        private const string TABLE_NAME = "NotificationTable";

        public NotificationRepository()
            : base(TABLE_NAME)
        { }

        public void Create(Notification newEntity)
        {
            newEntity.Id = Guid.NewGuid().ToString();
            Create(new NotificationEntity(newEntity));
        }

        public IEnumerable<Notification> GetAll()
        {
            throw new NotImplementedException();
        }

        IEnumerable<Notification> IRepository<Notification>.GetAll(string partitionKey)
        {
            throw new NotImplementedException();
        }

        public new Notification Get(string partitionKey, string rowKey)
        {
            throw new NotImplementedException();
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
