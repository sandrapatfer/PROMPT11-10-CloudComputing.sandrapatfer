using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;

namespace Server.Utils
{
    public class RedirectJsonResult : JsonResult
    {
        public RedirectJsonResult(string action, string controller, int listId)
        {
            Data = new { redirect = string.Format("/{0}/{1}?listId={2}", controller, action, listId) };
        }

        public RedirectJsonResult(string action, string controller, object routeValues)
        {
            Data = new { redirect = string.Format("/{0}/{1}", controller, action) };
        }
    }
}