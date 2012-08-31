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
                // the request requires authorization
                if (!filterContext.HttpContext.Request.IsAuthenticated)
                {
                    // the request has not been authorized
                    filterContext.Result = new ViewResult()
                    {
                        ViewName = "Authorize",
                        ViewData = filterContext.Controller.ViewData,
                        MasterName = "_BasicLayout",
                        TempData = filterContext.Controller.TempData
                    };
                }
            }
        }
            

    }
}