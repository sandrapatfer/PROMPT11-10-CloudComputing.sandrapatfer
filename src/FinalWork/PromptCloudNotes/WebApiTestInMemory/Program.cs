using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Http;
using System.Net.Http;
using Utils;

namespace WebApiTestInMemory
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = new HttpConfiguration();
            config.Routes.MapHttpRoute("default", "{controller}/{id}", new { id = RouteParameter.Optional });

            config.MessageHandlers.Add(new OAuthAuthorizationMessageHandler());

            var server = new HttpServer(config);
            var client = new HttpClient(server);

            var r = client.GetAsync("http://localhost/resource");

            Console.ReadKey();
        }
    }
}
