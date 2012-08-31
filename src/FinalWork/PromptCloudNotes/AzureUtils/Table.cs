using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;
using System.Data.Services.Client;

namespace AzureUtils
{
    public class Table
    {
        private CloudStorageAccount _account;
        private CloudTableClient _client;
        private TableServiceContext _context;

        public Table(string connectionString)
        {
            _account = CloudStorageAccount.Parse(connectionString);
            _client = _account.CreateCloudTableClient();
            _context = _client.GetDataServiceContext();
        }

        public Table(CloudStorageAccount account)
        {
            _account = account;
            _client = _account.CreateCloudTableClient();
            _context = _client.GetDataServiceContext();
        }

        public bool CreateTable(string tableName)
        {
            try
            {
                _client.CreateTableIfNotExist(tableName);
                return true;
            }
            catch (StorageClientException)
            {
                return false;
            }
        }

        public bool Insert(string tableName, TableServiceEntity entity)
        {
            try
            {
                entity.PartitionKey = RemoveNotAcceptedChars(entity.PartitionKey);
                entity.RowKey = RemoveNotAcceptedChars(entity.RowKey);

                _context.AddObject(tableName, entity);
                _context.SaveChanges();
                return true;
            }
            catch (StorageClientException)
            {
                return false;
            }
            catch (DataServiceRequestException)
            {
                return false;
            }
        }

        public bool MergeUpdate<T>(string tableName, Func<T, bool> entityFilter, object entity)
        {
            try
            {
                T dbObject = _context.CreateQuery<T>(tableName).Where(entityFilter).FirstOrDefault();
                if (dbObject != null)
                {
                    var props = typeof(T).GetProperties();
                    foreach (var prop in entity.GetType().GetProperties())
                    {
                        var dbProp = props.FirstOrDefault(p => p.Name == prop.Name);
                        if (dbProp != null)
                        {
                            object value = prop.GetValue(entity, null);
                            dbProp.SetValue(dbObject, value, null);
                        }
                    }
                    _context.UpdateObject(dbObject);
                    _context.SaveChanges();
                }
                return true;
            }
            catch (StorageClientException)
            {
                return false;
            }
            catch (DataServiceRequestException)
            {
                return false;
            }
        }

        public bool MergeUpdate(string tableName, TableServiceEntity dbObject)
        {
            try
            {
                _context.UpdateObject(dbObject);
                _context.SaveChanges();
                return true;
            }
            catch (StorageClientException)
            {
                return false;
            }
            catch (DataServiceRequestException)
            {
                return false;
            }
        }

        public T GetEntity<T>(string tableName, string partitionKey, string rowKey)
        {
            // the pair of keys can not be repeated, so a query with both should only return one value
            try
            {
                return _context.CreateQuery<TableServiceEntity>(tableName).
                    Where(e => e.PartitionKey == RemoveNotAcceptedChars(partitionKey) && e.RowKey == RemoveNotAcceptedChars(rowKey)).
                    Cast<T>().FirstOrDefault();
            }
            catch (DataServiceClientException)
            {
                // resource does not exist
                return default(T);
            }
            catch (DataServiceQueryException)
            {
                // resource does not exist
                return default(T);
            }
        }

        public IEnumerable<T> GetAllEntities<T>(string tableName)
            where T : TableServiceEntity
        {
            return _context.CreateQuery<T>(tableName).ToList();
        }

        public IEnumerable<T> GetEntitiesInPartition<T>(string tableName, string partitionKey)
            where T : TableServiceEntity
        {
            var query = _context.CreateQuery<T>(tableName).
                Where(e => e.PartitionKey == partitionKey);
            return query.ToList();
        }

        public IEnumerable<T> GetEntitiesInRow<T>(string tableName, string rowKey)
            where T : TableServiceEntity
        {
            return _context.CreateQuery<T>(tableName).
                Where(e => e.RowKey == rowKey).ToList();
        }

        public void DeleteEntity(string tableName, TableServiceEntity entity)
        {
            _context.DeleteObject(entity);
            _context.SaveChanges();
        }

        public void DeleteEntities(string tableName, ICollection<TableServiceEntity> entities)
        {
            foreach (var entity in entities)
            {
                _context.DeleteObject(entity);
            }
            _context.SaveChanges();
        }

        private string RemoveNotAcceptedChars(string key)
        {
            return new string(key.Where(c => c != '\\' && c!= '/' && c!= '#' && c!= '?').ToArray());
        }
    }
}
