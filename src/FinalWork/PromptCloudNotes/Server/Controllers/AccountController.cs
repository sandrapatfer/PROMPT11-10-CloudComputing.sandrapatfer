using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.IdentityModel.Web;

namespace Server.Controllers
{
    public class AccountController : Controller
    {
        //
        // GET: /Account/LogOn
        [HttpGet]
        public ActionResult LogOn()
        {
            var signin = FederatedAuthentication.WSFederationAuthenticationModule.CreateSignInRequest("1",
                Request.Url.AbsoluteUri, false);
            return Redirect(signin.WriteQueryString());
        }

        [HttpPost, ActionName("LogOn")]
        public ActionResult PostLogOn()
        {
            return RedirectToAction("Index", "TaskLists");
        }

        //
        // GET: /Account/LogOff
        [HttpGet]
        public ActionResult LogOff()
        {
            WSFederationAuthenticationModule fam = FederatedAuthentication.WSFederationAuthenticationModule;
            fam.SignOut(true);

            // Return to home after LogOff
            return RedirectToAction("Index", "Home");
        }
    }
}
