using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.ServiceBus;
using System.ServiceModel;

namespace EchoClient
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceBusEnvironment.SystemConnectivity.Mode = ConnectivityMode.AutoDetect;
            Console.Write("Your Service Namespace: ");
//            string serviceNamespace = Console.ReadLine();
            Console.Write("Your Issuer Name: ");
//            string issuerName = Console.ReadLine();
            Console.Write("Your Issuer Secret: ");
//            string issuerSecret = Console.ReadLine();

            string issuerName = "owner";
            string issuerSecret = "y+4vWY/Ryoen5avmC4CPAYTHj/OpkUQ7eykVhxNjr2w=";
            string serviceNamespace = "spf-nspace";

            Uri serviceUri = ServiceBusEnvironment.CreateServiceUri("sb", serviceNamespace, "EchoService");
            TransportClientEndpointBehavior sharedSecretServiceBusCredential = new TransportClientEndpointBehavior();
            sharedSecretServiceBusCredential.TokenProvider = TokenProvider.CreateSharedSecretTokenProvider(issuerName, issuerSecret);

            ChannelFactory<IEchoChannel> channelFactory = new ChannelFactory<IEchoChannel>("RelayEndpoint", new EndpointAddress(serviceUri));
            channelFactory.Endpoint.Behaviors.Add(sharedSecretServiceBusCredential);
            IEchoChannel channel = channelFactory.CreateChannel();
            channel.Open();
            Console.WriteLine("Enter text to echo (or [Enter] to exit):");
            string input = Console.ReadLine();
            while (input != String.Empty)
            {
                try
                {
                    Console.WriteLine("Server echoed: {0}", channel.Echo(input));
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error: " + e.Message);
                }
                input = Console.ReadLine();
            }
            channel.Close();
            channelFactory.Close();
        }
    }
}
