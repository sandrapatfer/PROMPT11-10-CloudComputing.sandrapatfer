using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PromptCloudNotes.AzureRepo
{
    public class AzureQueue<T>
    {
        private string QUEUE_NAME;
        private AzureUtils.Queue _queueUtils;

        public AzureQueue(string queueName)
        {
            QUEUE_NAME = queueName;

            //var connectionString = CloudStorageAccount.FromConfigurationSetting("DataConnectionString");
            var connectionString = "DefaultEndpointsProtocol=http;AccountName=spfcloudnotes;AccountKey=Gdi0M+gd1mO7a183WgRP8zxag5Fh2t0NNnh8Qvmz47V4vVDBe7JIXjdQS3wH0aLqRlpqNUqfzfuSC3TCgjVkLg==";
            _queueUtils = new AzureUtils.Queue(connectionString);

            // ensure the queue is created
            _queueUtils.CreateQueue(QUEUE_NAME);
        }

        protected void PutMessage(T newEntity)
        {
            _queueUtils.PutMessage<T>(QUEUE_NAME, newEntity);
        }

        protected T GetMessage()
        {
            T obj;
            _queueUtils.GetMessage<T>(QUEUE_NAME, out obj);
            return obj;
        }
    }
}
