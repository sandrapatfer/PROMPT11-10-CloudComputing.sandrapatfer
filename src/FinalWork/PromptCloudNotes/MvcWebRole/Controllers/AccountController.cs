using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.IdentityModel.Web;
using Server.Utils;
using Microsoft.IdentityModel.Protocols.WSFederation;
using PromptCloudNotes.Interfaces.Managers;
using Server.MvcModel;
using Server.Utils.Hrd;
using Microsoft.IdentityModel.Claims;

namespace Server.Controllers
{
    public class AccountController : Controller
    {
        private IUserManager _userManager;

        public AccountController(IUserManager userManager)
        {
            _userManager = userManager;
        }

        //
        // GET: /Account/LogOn
        [AllowAnonymous]
        [HttpGet]
        public ActionResult LogOn()
        {
            return View();
        }

        //
        // GET: /Account/LogOnPartial
        [AllowAnonymous]
        [HttpGet]
        public ActionResult LogOnPartial()
        {
            HrdClient hrdClient = new HrdClient();

            WSFederationAuthenticationModule fam = FederatedAuthentication.WSFederationAuthenticationModule;
            HrdRequest request = new HrdRequest(fam.Issuer, fam.Realm, context: Request.Url.AbsoluteUri);

            IEnumerable<HrdIdentityProvider> hrdIdentityProviders = hrdClient.GetHrdResponse(request);

            return PartialView("_LogOnPartial", hrdIdentityProviders);
        }

        // POST: /Account/LogOnToken
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult LogOnToken(FormCollection form)
        {
            // We use return url as context
            WSFederationMessage message = WSFederationMessage.CreateFromNameValueCollection(new Uri("http://www.notused.com"), form);
            string returnUrl = message != null ? message.Context : null;

            var claimsPrincipal = User as IClaimsPrincipal;
            if (claimsPrincipal != null)
            {
                var claimsIdentity = claimsPrincipal.Identities[0];
                var nameIdentifierClaim = claimsIdentity.Claims.FirstOrDefault(c => c.ClaimType == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");
                var emailAddressClaim = claimsIdentity.Claims.FirstOrDefault(c => c.ClaimType == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress");
                var identityProviderClaim = claimsIdentity.Claims.FirstOrDefault(c => c.ClaimType == "http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider");
                var nameClaim = claimsIdentity.Claims.FirstOrDefault(c => c.ClaimType == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name");

                var userClaims = new UserClaims
                {
                    Name = nameClaim != null ? nameClaim.Value : null,
                    Email = emailAddressClaim != null ? emailAddressClaim.Value : null,
                    Provider = identityProviderClaim != null ? identityProviderClaim.Value : null,
                    NameIdentifier = nameIdentifierClaim != null ? nameIdentifierClaim.Value : null
                };

                if (string.IsNullOrEmpty(userClaims.Name) || string.IsNullOrEmpty(userClaims.Email))
                {
                    var user = _userManager.GetUserByClaims(userClaims.Provider, userClaims.NameIdentifier);
                    if (user == null)
                    {
                        return View("Register", userClaims);
                    }
                    else
                    {
                        Session.Add("user", user);
                    }
                }
                else
                {
                    ValidateLogin(userClaims);
                }
                if (!string.IsNullOrEmpty(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                else
                {
                    return RedirectToAction("Index", "TaskLists");
                }
            }
            else
            {
                throw new HttpException((int)System.Net.HttpStatusCode.Unauthorized, "Unauthorized request");
            }
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

        //
        // POST: /Account/Register
        public ActionResult Register(UserClaims userClaims)
        {
            if (ModelState.IsValid)
            {
                var claimsPrincipal = User as IClaimsPrincipal;
                var nameIdentifierClaim = claimsPrincipal.Identities[0].Claims.FirstOrDefault(c => c.ClaimType == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");
                userClaims.NameIdentifier = nameIdentifierClaim.Value;
                var identityProviderClaim = claimsPrincipal.Identities[0].Claims.FirstOrDefault(c => c.ClaimType == "http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider");
                userClaims.Provider = identityProviderClaim.Value;

                var user = ValidateLogin(userClaims);
                return RedirectToAction("Index", "TaskLists");
            }
            else
            {
                return View(userClaims);
            }
        }

        private PromptCloudNotes.Model.User ValidateLogin(UserClaims userClaims)
        {
            var user = _userManager.GetUserByClaims(userClaims.Provider, userClaims.NameIdentifier);
            if (user == null)
            {
                user = new PromptCloudNotes.Model.User() { Provider = userClaims.Provider, NameIdentifier = userClaims.NameIdentifier, Name = userClaims.Name, Email = userClaims.Email };
                _userManager.CreateUser(user);
            }

            Session.Add("user", user);

            return user;
        }
    }
}
