using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.IdentityModel.Web;
using Server.Utils;
using Microsoft.IdentityModel.Protocols.WSFederation;
using PromptCloudNotes.Interfaces;

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
            if (!Request.IsAuthenticated)
            {
                var signin = FederatedAuthentication.WSFederationAuthenticationModule.CreateSignInRequest("1",
                    Request.Url.AbsoluteUri, false);
                return Redirect(signin.WriteQueryString());
            }
            else
            {
                ValidateUser();
                var listMgr = DependencyResolver.Current.GetService<ITaskListManager>();
                var lists = listMgr.GetAllLists(_userManager.GetUser(User.Identity.Name).Id);
                if (lists.Count() > 0)
                {
                    return RedirectToAction("Index", "Notes", new { listId = lists.First().Id });
                }
                else
                {
                    return RedirectToAction("Create", "TaskLists");
                }
            }
        }

        // POST: /Account/LogOnToken
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult LogOnToken()

            // TODO - ver porque é q o POST nao é chamado e é redireccionado para o GET!!!! Este é o return URL no ACS :(
        {
            return RedirectToAction("Index", "TaskLists");
        }
 
/*        [HttpPost, ActionName("LogOn")]
        public ActionResult PostLogOn()
        {
            return RedirectToAction("Index", "TaskLists");
        }*/

        //
        // GET: /Account/LogOff
        public ActionResult LogOff()
        {
            WSFederationAuthenticationModule fam = FederatedAuthentication.WSFederationAuthenticationModule;
            fam.SignOut(true);

            // Return to home after LogOff
            return RedirectToAction("Index", "Home");
        }

        private bool ValidateUser()
        {
            var user = _userManager.GetUser(User.Identity.Name);
            if (user == null)
            {
                // TODO create a sign up action?
                _userManager.CreateUser(new PromptCloudNotes.Model.User() { UserName = User.Identity.Name });
            }

            return true;
        }
    }
}
