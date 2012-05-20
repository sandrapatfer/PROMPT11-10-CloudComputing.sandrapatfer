using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;

namespace Receiver
{
    class Program
    {
        const string QueueName = "IssueTrackingQueue";
        static string ServiceNamespace;
        static string IssuerName;
        static string IssuerKey;

        static void Main(string[] args)
        {
            GetUserCredentials();
            TokenProvider credentials = null;
            Uri serviceUri = null;
            CreateTokenProviderAndServiceUri(out credentials, out serviceUri);
            MessagingFactory factory = null;
            try
            {
                NamespaceManager namespaceClient = new NamespaceManager(serviceUri, credentials);
                if (namespaceClient == null)
                {
                    Console.WriteLine("\nUnexpected Error: NamespaceManager is NULL");
                    return;
                }
                QueueDescription queueDescription = namespaceClient.GetQueue(QueueName);
                if (queueDescription == null)
                {
                    Console.WriteLine("\nUnexpected Error: QueueDescription is NULL");
                    return;
                }
                QueueClient myQueueClient = CreateQueueClient(serviceUri, credentials, out factory);
                Console.WriteLine("\nReceiving messages from Queue '{0}'...", QueueName);
                // Numero actual de mensagens na queue
                long messageCount = queueDescription.MessageCount;
                ReceiveNMessagesFromQueue(myQueueClient, messageCount);
                Console.WriteLine("\nEnd of scenario, press ENTER to exit.");
                Console.ReadLine();
            }
            catch (Exception e)
            {
                Console.WriteLine("Unexpected exception {0}", e.ToString());
                throw;
            }
            finally
            {
                if (factory != null) factory.Close();
            }
        }

        static void GetUserCredentials()
        {
            ServiceNamespace = "spf-nspace";
            IssuerName = "owner";
            IssuerKey = "y+4vWY/Ryoen5avmC4CPAYTHj/OpkUQ7eykVhxNjr2w=";
        }

        static void CreateTokenProviderAndServiceUri(out TokenProvider credentials, out Uri serviceUri)
        {
            credentials = TokenProvider.CreateSharedSecretTokenProvider(IssuerName, IssuerKey);
            serviceUri = ServiceBusEnvironment.CreateServiceUri("sb", ServiceNamespace, string.Empty);
        }

        static QueueClient CreateQueueClient(Uri serviceUri, TokenProvider credentials, out MessagingFactory factory)
        {
            factory = MessagingFactory.Create(serviceUri, credentials);
            return factory.CreateQueueClient(QueueName);
        }

        //Recebe messageCount mensagens da queue, com timeout de 5 segundos por mensagem.
        //Imprimir na consola o id e o body de cada mensagem
        static void ReceiveNMessagesFromQueue(QueueClient myQueueClient, long messageCount)
        {
            for (int i = 0; i < messageCount; i++)
            {
                var message = myQueueClient.Receive(new TimeSpan(0, 0, 5));
                if (message != null)
                {
                    Console.WriteLine("Message received. Id: {0} Body: {1}", message.MessageId, message.GetBody<string>());
                }
            }
        }
    }
}
