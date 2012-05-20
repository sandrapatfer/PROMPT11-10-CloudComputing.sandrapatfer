using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;

namespace TopicsSender
{
    class Program
    {
        private static string ServiceNamespace;
        private static string IssuerName;
        private static string IssuerKey;
        private static string TopicName = "IssueTrackingTopic";

        static void Main(string[] args)
        {
            GetUserCredentials();
            TokenProvider tokenProvider = null;
            Uri serviceUri = null;
            CreateTokenProviderAndServiceUri(out tokenProvider, out serviceUri);

            NamespaceManager namespaceManager = new NamespaceManager(serviceUri, tokenProvider);
            Console.WriteLine("Creating Topic 'IssueTrackingTopic'...");
            if (namespaceManager.TopicExists("IssueTrackingTopic"))
                namespaceManager.DeleteTopic("IssueTrackingTopic");

            MessagingFactory factory = null;
            TopicDescription myTopic = namespaceManager.CreateTopic(TopicName);
            // Criar duas Subscrições
            Console.WriteLine("Creating Subscriptions 'AuditSubscription' and 'AgentSubscription'...");
            SubscriptionDescription myAuditSubscription = namespaceManager.CreateSubscription((myTopic.Path, "AuditSubscription");
            SubscriptionDescription myAgentSubscription = namespaceManager.CreateSubscription(myTopic.Path, "AgentSubscription");
            TopicClient myTopicClient = CreateTopicClient(serviceUri, tokenProvider, myTopic, out factory);
            List<BrokeredMessage> messageList = new List<BrokeredMessage>();
            messageList.Add(CreateIssueMessage("1", "First message information"));
            messageList.Add(CreateIssueMessage("2", "Second message information"));
            messageList.Add(CreateIssueMessage("3", "Third message information"));
            Console.WriteLine("\nSending messages to topic...");
            SendListOfMessages(messageList, myTopicClient);

            Console.WriteLine("\nFinished sending messages, press ENTER to clean up and exit.");
            myTopicClient.Close();
            Console.ReadLine();
            namespaceManager.DeleteTopic(TopicName);
        }

        //Criar o TopicClient a partir do uri do serviço, do tokenProvider e do TopicDescription
        private static TopicClient CreateTopicClient(Uri serviceUri, TokenProvider tokenProvider, TopicDescription myTopic, out MessagingFactory factory)
        {
            factory = MessagingFactory.Create(serviceUri, tokenProvider);
            return factory.CreateTopicClient(myTopic.Path);
        }

        //Enviar uma lista de mensagens para o cliente topic e imprimir as mensagens enviadas na consola
        static void SendListOfMessages(List<BrokeredMessage> list, TopicClient myTopicClient)
        {
            foreach (var message in list)
            {
                Console.Write(string.Format("Sending message with id: {0}", message.MessageId));
                myTopicClient.Send(message);
                Console.WriteLine("...sent");
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
        static BrokeredMessage CreateIssueMessage(string issueId, string issueBody)
        {
            var message = new BrokeredMessage(issueBody);
            message.MessageId = issueId;
            return message;
        }
    }
}
