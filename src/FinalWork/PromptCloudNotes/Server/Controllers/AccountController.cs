using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.IdentityModel.Web;
using Server.Utils;
using Microsoft.IdentityModel.Protocols.WSFederation;

namespace Server.Controllers
{
    public class AccountController : Controller
    {
        //
        // GET: /Account/LogOn
        [AllowAnonymous]
        public ActionResult LogOn()
        {
            var signin = FederatedAuthentication.WSFederationAuthenticationModule.CreateSignInRequest("1",
                Request.Url.AbsoluteUri, false);
            return Redirect(signin.WriteQueryString());
        }

        // POST: /Account/LogOn
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult LogOn(FormCollection forms)
        {
            // We use return url as context
            WSFederationMessage message = WSFederationMessage.CreateFromNameValueCollection(Request.Url, forms);
            if (message != null)
            {
                string returnUrl = message.Context;
            }

            return null;
        }
 
        //
        // GET: /Account/LogOff
        public ActionResult LogOff()
        {
            WSFederationAuthenticationModule fam = FederatedAuthentication.WSFederationAuthenticationModule;
            fam.SignOut(true);

            // Return to home after LogOff
            return RedirectToAction("Index", "Home");
        }
    }
}
