using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PromptCloudNotes.Interfaces.Repositories;

namespace PromptCloudNotes.AzureRepo
{
    public class OAuthCodeRepository : AzureRepository<PromptCloudNotes.Model.OAuthCode, PromptCloudNotes.AzureRepo.Model.OAuthCodeEntity>, IOAuthCodeRepository
    {
        private const string TABLE_NAME = "OAuthCodeTable";

        public OAuthCodeRepository()
            : base(TABLE_NAME)
        { }

        public IEnumerable<PromptCloudNotes.Model.OAuthCode> GetAll()
        {
            throw new NotImplementedException();
        }

        IEnumerable<PromptCloudNotes.Model.OAuthCode> IRepository<PromptCloudNotes.Model.OAuthCode>.GetAll(string partitionKey)
        {
            throw new NotImplementedException();
        }

        public new PromptCloudNotes.Model.OAuthCode Get(string clientId, string code)
        {
            return Get(clientId, code, e => new PromptCloudNotes.Model.OAuthCode()
            {
                ClientId = e.PartitionKey,
                Code = e.RowKey,
                User = e.User
            });
        }

        public void Create(PromptCloudNotes.Model.OAuthCode newEntity)
        {
            newEntity.Code = Guid.NewGuid().ToString();
            Create(new Model.OAuthCodeEntity(newEntity.ClientId, newEntity.Code) { User = newEntity.User });
        }

        public void Update(string partitionKey, string rowKey, PromptCloudNotes.Model.OAuthCode changedEntity)
        {
            throw new NotImplementedException();
        }

        public void Delete(string clientId, string code)
        {
            var entity = base.Get(clientId, code);
            base.DeleteEntity(entity);
        }
    }
}
