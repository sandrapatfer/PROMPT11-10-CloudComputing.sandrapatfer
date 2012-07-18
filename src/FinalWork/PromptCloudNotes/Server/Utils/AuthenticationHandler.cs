using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;
using System.Web.Http.Hosting;
using System.Security.Principal;

namespace Server.Utils
{
    public class AuthenticationHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var authHeader = request.Headers.Authorization;
            if (authHeader != null && authHeader.Scheme == "Bearer" && authHeader.Parameter == "456")
            {
                var identity = new GenericIdentity("Sandra Fernandes");
                if (request.Properties.Keys.Contains(HttpPropertyKeys.UserPrincipalKey))
                {
                    request.Properties.Remove(HttpPropertyKeys.UserPrincipalKey);
                }
                request.Properties.Add(HttpPropertyKeys.UserPrincipalKey, new GenericPrincipal(identity, null));
            }
            return base.SendAsync(request, cancellationToken);
        }
    }
}