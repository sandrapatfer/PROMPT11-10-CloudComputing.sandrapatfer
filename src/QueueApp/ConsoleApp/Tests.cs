using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure.StorageClient;
using System.Collections.Specialized;

namespace ConsoleApp
{
    internal class Tests
    {
        private const string CONNECTION_STRING = "DefaultEndpointsProtocol=http;AccountName=storagespf;AccountKey=0FsVToOeZ1tDGhxuESR8oZr+BUNtYxk7Uvffmd9U2g24hsJ9e5qj95WyT/OmIb7AXdfjrlSvPv7PUDYXgiVdHA==";

        static void Separator()
        {
            Console.WriteLine("-------------------------------");
        }

        public static void TestingQueues1()
        {
            QueueUtilities queueUtil = new QueueUtilities(CONNECTION_STRING);
            Console.Write("Creating queue ");
            if (queueUtil.CreateQueue("samplequeue1"))
                Console.WriteLine("true");
            else
                Console.WriteLine("false");
            Separator();
            if (queueUtil.CreateQueue("samplequeue2"))
                Console.WriteLine("true");
            else
                Console.WriteLine("false");
            Separator();
        }

        public static void TestingQueues2()
        {
            QueueUtilities queueUtil = new QueueUtilities(CONNECTION_STRING);
            List<CloudQueue> queues;
            Console.Write("List queues ");
            if (queueUtil.ListQueues(out queues))
                foreach (CloudQueue queue in queues)
                    Console.Write(queue.Name + " ");
            Console.WriteLine();
            Separator();
            Console.Write("Delete queue ");
            if (queueUtil.DeleteQueue("samplequeue0"))
                Console.WriteLine("true");
            else
                Console.WriteLine("false");
            Separator();
            Console.Write("Delete queue ");
            if (queueUtil.DeleteQueue("samplequeue2"))
                Console.WriteLine("true");
            else
                Console.WriteLine("false");
        }

        public static void TestingQueues3()
        {
            QueueUtilities queueUtil = new QueueUtilities(CONNECTION_STRING);
            NameValueCollection metadata = new NameValueCollection();
            Console.WriteLine("Get queue metadata");
            Separator();
            if (queueUtil.GetQueueMetadata("samplequeue1", out metadata))
                if (metadata != null)
                {
                    for (int i = 0; i < metadata.Count; i++)
                    {
                        Console.WriteLine(metadata.GetKey(i) + ": " + metadata.Get(i));
                    }
                }
                else
                    Console.WriteLine("false");
            Separator();
            metadata.Add("property1", "Value1");
            metadata.Add("property2", "Value2");
            metadata.Add("property3", "Value3");
            Console.WriteLine("Set queue metadata ");
            if (queueUtil.SetQueueMetadata("samplequeue1", metadata))
                Console.WriteLine("true");
            else
                Console.WriteLine("false");
            Separator();
            Console.WriteLine("Get queue metadata ");
            if (queueUtil.GetQueueMetadata("samplequeue1", out metadata))
                if (metadata != null)
                {
                    for (int i = 0; i < metadata.Count; i++)
                    {
                        Console.WriteLine(metadata.GetKey(i) + ": " + metadata.Get(i));
                    }
                }
                else
                    Console.WriteLine("false");
            Separator();
        }

        public static void TestingQueues4()
        {
            QueueUtilities queueUtil = new QueueUtilities(CONNECTION_STRING);
            CloudQueueMessage message = null;
            Console.Write("Peek a message ");
            if (queueUtil.PeekMessage("samplequeue1", out message))
            {
                Console.WriteLine("true");
                Console.WriteLine("MessageId: " + message.Id);
                Console.WriteLine("POPReceipt: " + message.PopReceipt);
                Console.WriteLine(message.AsString);
            }
            else
                Console.WriteLine("false");
            Separator();
            message = new CloudQueueMessage("<Order id=\"1001\">This is test message 1</Order>");
            Console.Write("Put message ");
            if (queueUtil.PutMessage("samplequeue1", message))
                Console.WriteLine("true");
            else
                Console.WriteLine("false");
            Separator();
            message = null;
            Separator();
            message = null;
            Console.Write("Peek message ");
            if (queueUtil.PeekMessage("samplequeue1", out message))
            {
                Console.WriteLine("true");
                Console.WriteLine("MessageId: " + message.Id);
                Console.WriteLine("POPReceipt: " + message.PopReceipt);
                Console.WriteLine(message.AsString);
            }
            else
                Console.WriteLine("false");
        }

        public static void TestingQueues5()
        {
            QueueUtilities queueUtil = new QueueUtilities(CONNECTION_STRING);
            CloudQueueMessage message = null;
            message = new CloudQueueMessage("<Order id=\"1002\">This is test message 2</Order>");
            Console.Write("Put message ");
            if (queueUtil.PutMessage("samplequeue1", message))
                Console.WriteLine("true");
            else
                Console.WriteLine("false");
            message = null;
            Console.Write("Get message ");
            if (queueUtil.GetMessage("samplequeue1", out message))
            {
                Console.WriteLine("true");
                Console.WriteLine("MessageId: " + message.Id + " popReceipt=" + message.PopReceipt);
                Console.WriteLine("POPReceipt; " + message.PopReceipt);
                Console.WriteLine(message.AsString);
            }
            else
                Console.WriteLine("false");
            Separator();
            List<CloudQueueMessage> messages;
            Console.Write("List queues ");
            if (queueUtil.GetMessages("samplequeue1", out messages, 10))
                foreach (CloudQueueMessage queue in messages)
                    Console.Write(message.AsString + " ");
            Console.WriteLine();
            Separator();
            Console.WriteLine("Clear messages ");
            if (queueUtil.ClearMessages("samplequeue1"))
                Console.WriteLine("true");
            else
                Console.WriteLine("false");
            Separator();
            Console.WriteLine("Delete message ");
            if (queueUtil.DeleteMessage("samplequeue1", message))
                Console.WriteLine("true");
            else
                Console.WriteLine("false");
            Separator();
            message = null;
            Console.WriteLine("Get message ");
            if (queueUtil.GetMessage("samplequeue1", out message))
            {
                Console.WriteLine("true");
                Console.WriteLine("MessageId: " + message.Id + " popReceipt=" + message.PopReceipt);
                Console.WriteLine("POPReceipt; " + message.PopReceipt);
                Console.WriteLine(message.AsString);
            }
            else
                Console.WriteLine("false");
            Separator();
        }

        public static void TestingQueues6()
        {
            QueueUtilities queueUtil = new QueueUtilities(CONNECTION_STRING);
            
            queueUtil.ClearMessages("samplequeue1");
            CloudQueueMessage message = new CloudQueueMessage("<Order id=\"1003\">This is test message 2</Order>");
            var delay = TimeSpan.FromSeconds(3.1);
            var expiration = TimeSpan.FromSeconds(0.1);
            Console.WriteLine("Put message with live initial visible delay {0} seconds and with expiration time {1} seconds", delay, expiration);
            queueUtil.PutMessage("samplequeue1", message, expiration, delay);
            List<CloudQueueMessage> messages;
            queueUtil.GetMessages("samplequeue1", out messages, 10);
            foreach (CloudQueueMessage m in messages)
            {
                Console.Write(m.AsString + " ");
                Console.WriteLine();
            }
            Separator();
        }

        public static void TestingQueues7()
        {
            QueueUtilities queueUtil = new QueueUtilities(CONNECTION_STRING);

            queueUtil.ClearMessages("samplequeue1");
            CloudQueueMessage message = new CloudQueueMessage("<Order id=\"1006\">This is test message 6</Order>");
            Console.WriteLine("Put message with time to live… and expiration time…");
            queueUtil.PutMessage("samplequeue1", message, new TimeSpan(1, 0, 0), new TimeSpan(0, 0, 0));

            List<CloudQueueMessage> messages;
            queueUtil.GetMessages("samplequeue1", out messages, 10);
            foreach (CloudQueueMessage ms in messages)
                Console.WriteLine(ms.AsString);
            Separator();

            CloudQueueMessage myMessage = queueUtil.GetMessageRef("samplequeue1");
            if (myMessage != null)
            {
                Console.WriteLine("Update message");
                Console.WriteLine(myMessage.AsString);
                queueUtil.UpdateMessage("samplequeue1", myMessage, new TimeSpan(0, 0, 1));
            }
            else
            {
                Console.WriteLine("Update messages");
                foreach (CloudQueueMessage ms in messages)
                {
                    Console.WriteLine(ms.AsString);
                    queueUtil.UpdateMessage("samplequeue1", ms, new TimeSpan(0, 0, 1));
                }
            }

            Console.WriteLine("Get all messages");
            queueUtil.GetMessages("samplequeue1", out messages, 10);
            foreach (CloudQueueMessage queue in messages)
                Console.WriteLine(message.AsString);
            Console.WriteLine();
            Separator();

            Console.WriteLine("Wait 1 second and press enter");
            Console.ReadLine();
            Console.WriteLine("Get all messages");
            queueUtil.GetMessages("samplequeue1", out messages, 10);
            foreach (CloudQueueMessage queue in messages)
                Console.WriteLine(message.AsString);
            Console.WriteLine();
            Separator();
        }

        public static void TestingQueues8()
        {
            QueueUtilities queueUtil = new QueueUtilities(CONNECTION_STRING);

            CloudQueueMessage message = null;
            message = new CloudQueueMessage("<Order id=\"1002\">This is test message 2</Order>");
            Console.Write("Put message ");
            if (queueUtil.PutMessage("samplequeue1", message))
                Console.WriteLine("true");
            else
                Console.WriteLine("false");
            Separator();
            queueUtil.GetAllMessagesAsync("samplequeue1", 10).ContinueWith((task) =>
            {
                Separator();
                foreach (CloudQueueMessage ms in task.Result)
                    Console.WriteLine(ms.AsString);
            });
        }
    }
}
