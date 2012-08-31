using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure.StorageClient;

namespace PromptCloudNotes.AzureRepo
{
    public class AzureRepository<ModelEntity, AzureEntity>
        where AzureEntity : TableServiceEntity
    {
        private string TABLE_NAME;
        private AzureUtils.Table _tableUtils;

        public AzureRepository(string tableName)
        {
            TABLE_NAME = tableName;

            //var connectionString = CloudStorageAccount.FromConfigurationSetting("DataConnectionString");
            var connectionString = "DefaultEndpointsProtocol=http;AccountName=spfcloudnotes;AccountKey=Gdi0M+gd1mO7a183WgRP8zxag5Fh2t0NNnh8Qvmz47V4vVDBe7JIXjdQS3wH0aLqRlpqNUqfzfuSC3TCgjVkLg==";
            _tableUtils = new AzureUtils.Table(connectionString);

            // ensure the table is created
            _tableUtils.CreateTable(TABLE_NAME);
        }

        protected IEnumerable<AzureEntity> GetAll(string partitionKey)
        {
            return _tableUtils.GetEntitiesInPartition<AzureEntity>(TABLE_NAME, partitionKey);
        }

        protected IEnumerable<ModelEntity> GetAll(Func<AzureEntity, ModelEntity> ctor)
        {
            return _tableUtils.GetAllEntities<AzureEntity>(TABLE_NAME).Select(e => ctor(e));
        }

        protected IEnumerable<ModelEntity> GetAll(string partitionKey, Func<AzureEntity, ModelEntity> ctor)
        {
            return _tableUtils.GetEntitiesInPartition<AzureEntity>(TABLE_NAME, partitionKey).Select(e => ctor(e));
        }

        protected IEnumerable<AzureEntity> GetAllInRow(string rowKey)
        {
            return _tableUtils.GetEntitiesInRow<AzureEntity>(TABLE_NAME, rowKey);
        }

        protected AzureEntity Get(string partitionKey, string rowKey)
        {
            return _tableUtils.GetEntity<AzureEntity>(TABLE_NAME, partitionKey, rowKey);
        }

        protected ModelEntity Get(string partitionKey, string rowKey, Func<AzureEntity, ModelEntity> ctor)
        {
            var entity = _tableUtils.GetEntity<AzureEntity>(TABLE_NAME, partitionKey, rowKey);
            if (entity != null)
            {
                return ctor(entity);
            }
            return default(ModelEntity);
        }

        protected void Create(AzureEntity newEntity)
        {
            if (!_tableUtils.Insert(TABLE_NAME, newEntity))
            {
                // TODO excepcao nova
                throw new InvalidOperationException();
            }
        }

        protected void Update(AzureEntity changedEntity)
        {
            _tableUtils.MergeUpdate(TABLE_NAME, changedEntity);
        }

        protected void DeleteEntity(AzureEntity entity)
        {
            _tableUtils.DeleteEntity(TABLE_NAME, entity);
        }

        protected void DeleteEntities(ICollection<AzureEntity> entities)
        {
            _tableUtils.DeleteEntities(TABLE_NAME, (ICollection<TableServiceEntity>)entities);
        }

    }
}
