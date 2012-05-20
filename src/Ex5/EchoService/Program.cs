using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.ServiceBus;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace EchoService
{
    class Program
    {
        static void Main(string[] args)
        {
            string issuerName = "owner";
            string issuerSecret = "y+4vWY/Ryoen5avmC4CPAYTHj/OpkUQ7eykVhxNjr2w=";
            TransportClientEndpointBehavior sharedSecretServiceBusCredential = new TransportClientEndpointBehavior();
            sharedSecretServiceBusCredential.TokenProvider = TokenProvider.CreateSharedSecretTokenProvider(issuerName, issuerSecret);

            // Cria o URI do serviço baseado no namespace do serviço
            Uri address = ServiceBusEnvironment.CreateServiceUri("sb", "spf-nspace", "EchoService");
            // Cria o serviço host
            ServiceHost host = new ServiceHost(typeof(EchoService), address);
            //Cria o comportamento para o endpoint
            IEndpointBehavior serviceRegistrySettings = new ServiceRegistrySettings(DiscoveryType.Public);

            //Adiciona as credenciais do Service Bus a todos os endpoints configurados na configuração
            foreach (ServiceEndpoint endpoint in host.Description.Endpoints)
            {
                endpoint.Behaviors.Add(serviceRegistrySettings);
                endpoint.Behaviors.Add(sharedSecretServiceBusCredential);
            }

            // Abre o serviço.
            host.Open();
            Console.WriteLine("Service address: " + address);
            Console.WriteLine("Press [Enter] to exit");
            Console.ReadLine();

            // Fecha o serviço.
            host.Close();
        }
    }
}
