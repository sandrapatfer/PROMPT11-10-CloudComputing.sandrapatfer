using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PromptCloudNotes.Interfaces;
using PromptCloudNotes.Model;

namespace PromptCloudNotes.AzureRepo
{
    using Model;

    public class UserRepository : IUserRepository
    {
        private const string TABLE_NAME = "UserTable";
        private AzureUtils.Table _tableUtils;

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
            return _tableUtils.GetAllEntities<UserEntity>(TABLE_NAME).Select(e => new User() { Id = Convert.ToInt32(e.PartitionKey), UserName = e.Name });
        }

        public User Create(User user)
        {
            var allUsers = _tableUtils.GetAllEntities<UserEntity>(TABLE_NAME);
            int max = -1;
            if (allUsers != null && allUsers.Count() > 0)
            {
                max = allUsers.Max(e => Convert.ToInt32(e.PartitionKey));
            }
            var newEntity = new UserEntity(++max, user.UserName) { Name = user.UserName };
            if (_tableUtils.Insert(TABLE_NAME, newEntity))
            {
                user.Id = Convert.ToInt32(newEntity.PartitionKey);
                return user;
            }
            throw new InvalidOperationException();
        }

        public User Get(int userId)
        {
            var entity = _tableUtils.GetEntitiesInPartition<UserEntity>(TABLE_NAME, userId.ToString()).FirstOrDefault();
            if (entity != null)
            {
                return new User() { Id = userId, UserName = entity.Name };
            }
            return null;
        }

        public User Get(string name)
        {
            var entity = _tableUtils.GetAllEntities<UserEntity>(TABLE_NAME).Where(e => e.Name == name).FirstOrDefault();
            if (entity != null)
            {
                return new User() { Id = Convert.ToInt32(entity.PartitionKey), UserName = entity.Name };
            }
            return null;
        }
    }
}
