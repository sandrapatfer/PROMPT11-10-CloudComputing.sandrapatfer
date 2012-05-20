using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;

namespace Sender
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
                Console.WriteLine("\nCreating Queue '{0}'...", QueueName);
                //Eliminar se a queue já existir
                if (namespaceClient.QueueExists(QueueName))
                    namespaceClient.DeleteQueue(QueueName);
                namespaceClient.CreateQueue(QueueName);
                QueueClient myQueueClient = CreateQueueClient(serviceUri, credentials, out factory);
                List<BrokeredMessage> messageList = new List<BrokeredMessage>();
                messageList.Add(CreateIssueMessage("1", "First message "));
                messageList.Add(CreateIssueMessage("2", "Second message "));
                messageList.Add(CreateIssueMessage("3", "Third message "));
                SendListOfMessages(messageList, myQueueClient);
            }
            catch (Exception e)
            {
                Console.WriteLine("Unexpected exception {0}", e.ToString());
                throw;
            }
            finally
            {
                if (factory != null)
                    factory.Close();
            }
        }

        //inicializar o ServiceNamespace, IssuerName e IssuerKey com input da consola.
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

        //Criar o QueueClient a partir do uri do serviço e das credenciais
        static QueueClient CreateQueueClient(Uri serviceUri, TokenProvider credentials, out MessagingFactory factory)
        {
            factory = MessagingFactory.Create(serviceUri, credentials);
            return factory.CreateQueueClient(QueueName);
        }

        //Criar uma mensagem com body issueBody e Id issueid
        static BrokeredMessage CreateIssueMessage(string issueId, string issueBody)
        {
            var message = new BrokeredMessage(issueBody);
            message.MessageId = issueId;
            return message;
        }

        //Enviar uma lista de mensagens para o cliente queue e imprimir as mensagens enviadas na consola
        static void SendListOfMessages(List<BrokeredMessage> list, QueueClient myQueueClient)
        {
            foreach (var message in list)
            {
                Console.Write(string.Format("Sending message with id: {0}", message.MessageId));
                myQueueClient.Send(message);
                Console.WriteLine("...sent");
            }
        }
    }
}