using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PromptCloudNotes.Interfaces.Repositories;
using PromptCloudNotes.Model;

namespace PromptCloudNotes.AzureRepo
{
    using Model;

    public class UserRepository : AzureRepository<User, UserEntity>, IUserRepository
    {
        private const string TABLE_NAME = "UserTable";

        public UserRepository()
            : base(TABLE_NAME)
        { }

        public IEnumerable<User> GetAll()
        {
            return GetAll(u => u.GetUser());
        }

        public new IEnumerable<User> GetAll(string partitionKey)
        {
            throw new NotImplementedException();
        }

        public new User Get(string partitionKey, string rowKey)
        {
            return Get(partitionKey, rowKey, u => u.GetUser());
        }

        public void Create(User newEntity)
        {
            newEntity.UniqueId = Guid.NewGuid().ToString();
            Create(new UserEntity(newEntity));
        }

        public void Update(string partitionKey, string rowKey, User changedEntity)
        {
            throw new NotImplementedException();
        }

        public void Delete(string partitionKey, string rowKey)
        {
            throw new NotImplementedException();
        }

/*        private AzureUtils.Table _tableUtils;

        public UserRepository()
        {
            //var connectionString = CloudStorageAccount.FromConfigurationSetting("DataConnectionString");
            var connectionString = "DefaultEndpointsProtocol=http;AccountName=spfcloudnotes;AccountKey=Gdi0M+gd1mO7a183WgRP8zxag5Fh2t0NNnh8Qvmz47V4vVDBe7JIXjdQS3wH0aLqRlpqNUqfzfuSC3TCgjVkLg==";
            _tableUtils = new AzureUtils.Table(connectionString);

            // ensure the table is created
            _tableUtils.CreateTable(TABLE_NAME);
        }

        public IEnumerable<User> GetAll()
        {
            return _tableUtils.GetAllEntities<UserEntity>(TABLE_NAME).Select(e => e.GetUser());
        }

        public User Create(User user)
        {
            var newEntity = new UserEntity(user.Provider, user.NameIdentifier) { UniqueId = Guid.NewGuid().ToString(), Name = user.Name, Email = user.Email };
            if (_tableUtils.Insert(TABLE_NAME, newEntity))
            {
                user.UniqueId = newEntity.PartitionKey;
                return user;
            }
            throw new InvalidOperationException();
        }

        public User GetByClaims(string provider, string nameIdentifier)
        {
            var entity = _tableUtils.GetEntity<UserEntity>(TABLE_NAME, provider, nameIdentifier);
            if (entity != null)
            {
                return entity.GetUser();
            }
            return null;
        }*/
    }
}
