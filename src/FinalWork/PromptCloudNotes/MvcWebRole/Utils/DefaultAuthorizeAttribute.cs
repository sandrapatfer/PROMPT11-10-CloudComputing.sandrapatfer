using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Server.Utils
{
    // the default authorize attribute will require authorization except if the action has the AllowAnonymousAttribute

    public class DefaultAuthorizeAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (!filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), false))
            {
                base.OnAuthorization(filterContext);
            }
        }
            

    }
}