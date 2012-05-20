using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.StorageClient;

namespace WorkerRole1
{
    public class WorkerRole : RoleEntryPoint
    {
        private CloudQueue _queue;

        public override void Run()
        {
            // This is a sample worker implementation. Replace with your logic.
            Trace.WriteLine("WorkerRole entry point called", "Information");

            while (true)
            {
                var message = _queue.GetMessage();
                if (message != null)
                {
                    _queue.DeleteMessage(message);
                    Trace.WriteLine("Received the message: " + message.AsString, "Information");
                }
                Thread.Sleep(10000);
            }
        }

        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections 
            ServicePointManager.DefaultConnectionLimit = 12;

            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.

            var storageAccount = CloudStorageAccount.Parse(RoleEnvironment.GetConfigurationSettingValue("QueueInterRole"));
            var queueClient = storageAccount.CreateCloudQueueClient();
            _queue = queueClient.GetQueueReference("myqueue");
            _queue.CreateIfNotExist();

            return base.OnStart();
        }
    }
}
