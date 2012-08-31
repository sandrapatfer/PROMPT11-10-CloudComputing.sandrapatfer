using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;
using System.Collections.Specialized;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace AzureUtils
{
    public class Queue
    {
        private CloudStorageAccount _account;
        private CloudQueueClient _client;

        public Queue(string connectionString)
        {
            _account = CloudStorageAccount.Parse(connectionString);
            _client = _account.CreateCloudQueueClient();
        }

        public bool CreateQueue(string queueName)
        {
            try
            {
                var queue = _client.GetQueueReference(queueName);
                return queue.CreateIfNotExist();
            }
            catch
            {
                return false;
            }
        }

        public bool ListQueues(out List<CloudQueue> queues)
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

        public bool DeleteQueue(string queueName)
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

        public bool GetQueueMetadata(string queueName, out NameValueCollection metadata)
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

        public bool SetQueueMetadata(string queueName, NameValueCollection metadata)
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

        public bool PeekMessage(string queueName, out CloudQueueMessage message)
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

        public bool PutMessage(string queueName, CloudQueueMessage message)
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
        public bool PutMessage(string queueName, CloudQueueMessage message, TimeSpan messageLiveTime, TimeSpan delay)
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
        public bool PutMessage<T>(string queueName, T obj)
        {
            try
            {
                var queue = _client.GetQueueReference(queueName);
                string serializedMessage = new JavaScriptSerializer().Serialize(obj);
                var message = new CloudQueueMessage(serializedMessage);
                queue.AddMessage(message);
                return true;
            }
            catch (StorageClientException)
            {
                return false;
            }
        }


        public bool GetMessage(string queueName, out CloudQueueMessage message)
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

        public bool GetMessage<T>(string queueName, out T obj)
        {
            try
            {
                var queue = _client.GetQueueReference(queueName);
                var message = queue.GetMessage();
                if (message != null)
                {
                    var deserializedMessage = new JavaScriptSerializer().Deserialize<T>(message.AsString);
                    obj = deserializedMessage;
                    return true;
                }
            }
            catch (StorageClientException)
            {
            }
            obj = default(T);
            return false;
        }

        public bool GetMessages(string queueName, out List<CloudQueueMessage> messages, int maxMessages)
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

        public bool ClearMessages(string queueName)
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

        public bool DeleteMessage(string queueName, CloudQueueMessage message)
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


        public CloudQueueMessage GetMessageRef(string queueName)
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

        public void UpdateMessage(string queueName, CloudQueueMessage message, TimeSpan timeSpan)
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

        public Task<IEnumerable<CloudQueueMessage>> GetAllMessagesAsync(string queueName, int maxMessages)
        {
            var queue = _client.GetQueueReference(queueName);
            return Task.Factory.FromAsync((Func<int, AsyncCallback, object, IAsyncResult>)queue.BeginGetMessages, 
                (Func<IAsyncResult, IEnumerable<CloudQueueMessage>>)queue.EndGetMessages, maxMessages, null);
        }

        public List<CloudQueueMessage> EndGetAllMessages(string queueName, IAsyncResult result)
        {
            var queue = _client.GetQueueReference(queueName);
            return queue.EndGetMessages(result).ToList();
        }
    }
}
