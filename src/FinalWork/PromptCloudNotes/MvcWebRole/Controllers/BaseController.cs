using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PromptCloudNotes.Interfaces.Managers;
using Microsoft.IdentityModel.Claims;

namespace Server.Controllers
{
    public class BaseController : Controller
    {
        protected IUserManager _userManager;

        public BaseController(IUserManager userManager)
        {
            _userManager = userManager;
        }

        protected new PromptCloudNotes.Model.User User
        {
            get
            {
                var user = Session["user"] as PromptCloudNotes.Model.User;
                if (user == null)
                {
                    var claimsPrincipal = base.User as IClaimsPrincipal;
                    var nameIdentifierClaim = claimsPrincipal.Identities[0].Claims.FirstOrDefault(c => c.ClaimType == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");
                    var identityProviderClaim = claimsPrincipal.Identities[0].Claims.FirstOrDefault(c => c.ClaimType == "http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider");

                    user = _userManager.GetUserByClaims(identityProviderClaim.Value, nameIdentifierClaim.Value);

                    Session.Add("user", user);
                }
                return user;
            }
        }
    }
}
