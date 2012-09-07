using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;
using System.Web.Http.Hosting;
using System.Security.Principal;
using StructureMap;
using PromptCloudNotes.Interfaces.Repositories;
using System.Net;

namespace Server.Utils
{
    public class AuthenticationHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var authHeader = request.Headers.Authorization;
            if (authHeader != null && authHeader.Scheme == "Bearer")
            {
                var oAuthTokenRepo = ObjectFactory.GetInstance<IOAuthTokenRepository>();
                var token = oAuthTokenRepo.Get("Bearer", authHeader.Parameter);

                /* testing
                if ((DateTime.Now - token.CreatedAt) > TimeSpan.FromHours(1))
                {
                    return Task.Factory.StartNew<HttpResponseMessage>(()=>
                        new HttpResponseMessage(HttpStatusCode.BadRequest)
                        {
                            Content = new StringContent("Token expired")
                        });
                }*/

                var identity = new UserIdentity(token.User);
                request.Properties[HttpPropertyKeys.UserPrincipalKey] = new GenericPrincipal(identity, null);
            }
            return base.SendAsync(request, cancellationToken);
        }
    }
}