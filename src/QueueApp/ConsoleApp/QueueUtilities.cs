using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;
using System.Collections.Specialized;
using System.Threading.Tasks;

namespace ConsoleApp
{
    class QueueUtilities
    {
        private CloudStorageAccount _account;
        private CloudQueueClient _client;

        public QueueUtilities(string connectionString)
        {
            _account = CloudStorageAccount.Parse(connectionString);
            _client = _account.CreateCloudQueueClient();
        }

        internal bool CreateQueue(string queueName)
        {
            var queue = _client.GetQueueReference(queueName);
            return queue.CreateIfNotExist();
        }

        internal bool ListQueues(out List<CloudQueue> queues)
        {
            try
            {
                queues = _client.ListQueues().ToList();
                return true;
            }
            catch (StorageClientException)
            {
                queues = null;
                return false;
            }
        }

        internal bool DeleteQueue(string queueName)
        {
            try
            {
                var queue = _client.GetQueueReference(queueName);
                queue.Delete();
                return true;
            }
            catch (StorageClientException)
            {
                return false;
            }
        }

        internal bool GetQueueMetadata(string queueName, out NameValueCollection metadata)
        {
            try
            {
                var queue = _client.GetQueueReference(queueName);
                queue.FetchAttributes();
                metadata = queue.Metadata;
                return true;
            }
            catch (StorageClientException)
            {
                metadata = null;
                return false;
            }
        }

        internal bool SetQueueMetadata(string queueName, NameValueCollection metadata)
        {
            try
            {
                var queue = _client.GetQueueReference(queueName);
                queue.Metadata.Add(metadata);
                queue.SetMetadata();
                return true;
            }
            catch (StorageClientException)
            {
                return false;
            }
        }

        internal bool PeekMessage(string queueName, out CloudQueueMessage message)
        {
            try
            {
                var queue = _client.GetQueueReference(queueName);
                message = queue.PeekMessage();
                return message != null;
            }
            catch (StorageClientException)
            {
                message = null;
                return false;
            }
        }

        internal bool PutMessage(string queueName, CloudQueueMessage message)
        {
            try
            {
                var queue = _client.GetQueueReference(queueName);
                queue.AddMessage(message);
                return true;
            }
            catch (StorageClientException)
            {
                return false;
            }
        }
        internal bool PutMessage(string queueName, CloudQueueMessage message, TimeSpan messageLiveTime, TimeSpan delay)
        {
            try
            {
                var queue = _client.GetQueueReference(queueName);
                queue.AddMessage(message, messageLiveTime, delay);
                return true;
            }
            catch (StorageClientException)
            {
                return false;
            }
        }


        internal bool GetMessage(string queueName, out CloudQueueMessage message)
        {
            try
            {
                var queue = _client.GetQueueReference(queueName);
                message = queue.GetMessage();
                return message != null;
            }
            catch (StorageClientException)
            {
                message = null;
                return false;
            }
        }

        internal bool GetMessages(string queueName, out List<CloudQueueMessage> messages, int maxMessages)
        {
            try
            {
                var queue = _client.GetQueueReference(queueName);
                messages = queue.GetMessages(maxMessages).ToList();
                return messages != null;
            }
            catch (StorageClientException)
            {
                messages = null;
                return false;
            }
        }

        internal bool ClearMessages(string queueName)
        {
            try
            {
                var queue = _client.GetQueueReference(queueName);
                queue.Clear();
                return true;
            }
            catch (StorageClientException)
            {
                return false;
            }
        }

        internal bool DeleteMessage(string queueName, CloudQueueMessage message)
        {
            try
            {
                var queue = _client.GetQueueReference(queueName);
                queue.DeleteMessage(message.Id, message.PopReceipt);
                return true;
            }
            catch (StorageClientException)
            {
                return false;
            }
        }


        internal CloudQueueMessage GetMessageRef(string queueName)
        {
            try
            {
                var queue = _client.GetQueueReference(queueName);
                return queue.GetMessage();
            }
            catch (StorageClientException)
            {
                return null;
            }
        }

        internal void UpdateMessage(string queueName, CloudQueueMessage message, TimeSpan timeSpan)
        {
            try
            {
                var queue = _client.GetQueueReference(queueName);
                queue.UpdateMessage(message, timeSpan, MessageUpdateFields.Visibility);
            }
            catch (StorageClientException)
            {
            }
        }

        internal Task<IEnumerable<CloudQueueMessage>> GetAllMessagesAsync(string queueName, int maxMessages)
        {
            var queue = _client.GetQueueReference(queueName);
            return Task.Factory.FromAsync((Func<int, AsyncCallback, object, IAsyncResult>)queue.BeginGetMessages, 
                (Func<IAsyncResult, IEnumerable<CloudQueueMessage>>)queue.EndGetMessages, maxMessages, null);
        }

        internal List<CloudQueueMessage> EndGetAllMessages(string queueName, IAsyncResult result)
        {
            var queue = _client.GetQueueReference(queueName);
            return queue.EndGetMessages(result).ToList();
        }
    }
}
