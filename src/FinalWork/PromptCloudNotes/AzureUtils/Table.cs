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

        public bool Insert(string tableName, object entity)
        {
            try
            {
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

            return _context.CreateQuery<TableServiceEntity>(tableName).
                Where(e => e.PartitionKey == partitionKey && e.RowKey == rowKey).
                Cast<T>().FirstOrDefault();
        }

        public IEnumerable<T> GetAllEntities<T>(string tableName)
        {
            return _context.CreateQuery<TableServiceEntity>(tableName).Cast<T>();
        }

        public IEnumerable<T> GetEntitiesInPartition<T>(string tableName, string partitionKey)
        {
            return _context.CreateQuery<TableServiceEntity>(tableName).
                Where(e => e.PartitionKey == partitionKey).
                Cast<T>();
        }

        public IEnumerable<T> GetEntitiesInRow<T>(string tableName, string rowKey)
        {
            return _context.CreateQuery<TableServiceEntity>(tableName).
                Where(e => e.RowKey == rowKey).
                Cast<T>();
        }
    }
}
