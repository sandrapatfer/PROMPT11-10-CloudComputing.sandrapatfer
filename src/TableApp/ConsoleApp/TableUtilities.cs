using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;
using System.Data.Services.Client;

namespace ConsoleApp
{
    class TableUtilities
    {
        private CloudStorageAccount _account;
        private CloudTableClient _client;
        private TableServiceContext _context;

        public TableUtilities(string connectionString)
        {
            _account = CloudStorageAccount.Parse(connectionString);
            _client = _account.CreateCloudTableClient();
            _context = _client.GetDataServiceContext();
        }

        internal bool ListTables(out List<string> tables)
        {
            try
            {
                tables = _client.ListTables().ToList();
                return true;
            }
            catch (StorageClientException)
            {
                tables = null;
                return false;
            }
        }

        internal bool CreateTable(string tableName)
        {
            try
            {
                _client.CreateTable(tableName);
                return true;
            }
            catch (StorageClientException)
            {
                return false;
            }
        }

        internal bool InsertEntity(string tableName, Entities.Contact contact)
        {
            try
            {
                _context.AddObject(tableName, contact);
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

        internal bool ReplaceUpdateEntity(string tableName, string country, string lastName, Entities.Contact contact)
        {
            try
            {
                IQueryable<Entities.Contact> contacts = _context.CreateQuery<Entities.Contact>(tableName).Where(c => c.PartitionKey == country && c.RowKey == lastName);
                var dbContact = contacts.FirstOrDefault();
                if (dbContact != null)
                {
                    foreach (var prop in typeof(Entities.Contact).GetProperties())
                    {
                        object value = prop.GetValue(contact, null);
                        prop.SetValue(dbContact, value, null);
                    }

                    _context.UpdateObject(dbContact);
                    _context.SaveChanges(SaveChangesOptions.ReplaceOnUpdate);
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

        internal bool MergeUpdateEntity(string tableName, string country, string lastName, Entities.MiniContact miniContact)
        {
            try
            {
                IQueryable<Entities.Contact> contacts = _context.CreateQuery<Entities.Contact>(tableName).Where(c => c.PartitionKey == country && c.RowKey == lastName);
                var dbContact = contacts.FirstOrDefault();
                if (dbContact != null)
                {
                    var contactProps = typeof(Entities.Contact).GetProperties();
                    foreach (var prop in typeof(Entities.MiniContact).GetProperties())
                    {
                        var contactProp = contactProps.FirstOrDefault(p => p.Name == prop.Name);
                        if (contactProp != null)
                        {
                            object value = prop.GetValue(miniContact, null);
                            contactProp.SetValue(dbContact, value, null);
                        }
                    }

                    _context.UpdateObject(dbContact);
                    _context.SaveChanges(SaveChangesOptions.ReplaceOnUpdate);
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
    }
}
