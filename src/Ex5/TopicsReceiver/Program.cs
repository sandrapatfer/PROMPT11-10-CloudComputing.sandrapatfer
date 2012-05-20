using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;

namespace TopicsReceiver
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

            NamespaceManager namespaceClient = new NamespaceManager(serviceUri, tokenProvider);
            var topicDescription = namespaceClient.GetTopic(TopicName);

            MessagingFactory factory = null;
            var topicClient1 = CreateTopicSubscriptionClient(serviceUri, tokenProvider, "AuditSubscription", ref factory);
            var topicClient2 = CreateTopicSubscriptionClient(serviceUri, tokenProvider, "AgentSubscription", ref factory);
            Console.WriteLine("\nReceiving messages from Topic '{0}'...", TopicName);

            // Numero actual de mensagens na queue
            ReceiveAllMessagesFromSubscription(topicClient1);
            ReceiveAllMessagesFromSubscription(topicClient2);

            Console.WriteLine("\nEnd of scenario, press ENTER to exit.");
            Console.ReadLine();

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

        private static SubscriptionClient CreateTopicSubscriptionClient(Uri serviceUri, TokenProvider tokenProvider, string subscriptionName, ref MessagingFactory factory)
        {
            if (factory == null)
                factory = MessagingFactory.Create(serviceUri, tokenProvider);
            return factory.CreateSubscriptionClient(TopicName, subscriptionName);
        }

        static void ReceiveAllMessagesFromSubscription(SubscriptionClient myTopicClient)
        {
            while (true)
            {
                var message = myTopicClient.Receive(TimeSpan.FromSeconds(5));
                if (message != null)
                {
                    Console.WriteLine("Message received. Id: {0} Body: {1}", message.MessageId, message.GetBody<string>());
                }
                else
                {
                    Console.WriteLine("No more messages");
                    return;
                }
            }
        }
    }
}
