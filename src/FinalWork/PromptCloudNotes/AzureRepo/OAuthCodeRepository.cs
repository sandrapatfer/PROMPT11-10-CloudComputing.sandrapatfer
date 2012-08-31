using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PromptCloudNotes.Interfaces.Repositories;

namespace PromptCloudNotes.AzureRepo
{
    public class OAuthTokenRepository : AzureTable<PromptCloudNotes.Model.OAuthToken, PromptCloudNotes.AzureRepo.Model.OAuthTokenEntity>, IOAuthTokenRepository
    {
        private const string TABLE_NAME = "OAuthTokenTable";

        public OAuthTokenRepository()
            : base(TABLE_NAME)
        { }

        public IEnumerable<PromptCloudNotes.Model.OAuthToken> GetAll()
        {
            throw new NotImplementedException();
        }

        IEnumerable<PromptCloudNotes.Model.OAuthToken> IRepository<PromptCloudNotes.Model.OAuthToken>.GetAll(string partitionKey)
        {
            throw new NotImplementedException();
        }

        public new PromptCloudNotes.Model.OAuthToken Get(string partitionKey, string rowKey)
        {
            return Get(partitionKey, rowKey, e => new PromptCloudNotes.Model.OAuthToken()
            {
                TokenType = e.PartitionKey,
                Token = e.RowKey,
                User = e.User,
                CreatedAt = e.CreatedAt,
                RefreshToken = e.RefreshToken
            });
        }

        public void Create(PromptCloudNotes.Model.OAuthToken newEntity)
        {
            newEntity.Token = Guid.NewGuid().ToString();
            newEntity.RefreshToken = Guid.NewGuid().ToString();
            newEntity.CreatedAt = DateTime.Now;
            Create(new Model.OAuthTokenEntity(newEntity.TokenType, newEntity.Token) 
            {
                User = newEntity.User,
                CreatedAt = newEntity.CreatedAt,
                RefreshToken = newEntity.RefreshToken
            });
        }

        public void Update(string partitionKey, string rowKey, PromptCloudNotes.Model.OAuthToken changedEntity)
        {
            throw new NotImplementedException();
        }

        public void Delete(string partitionKey, string rowKey)
        {
            var entity = base.Get(partitionKey, rowKey);
            base.DeleteEntity(entity);
        }
    }
}
