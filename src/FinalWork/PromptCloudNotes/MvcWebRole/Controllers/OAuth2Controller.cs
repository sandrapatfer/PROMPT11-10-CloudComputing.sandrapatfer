using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Server.Utils;
using PromptCloudNotes.Interfaces.Managers;
using StructureMap;
using PromptCloudNotes.Interfaces.Repositories;
using PromptCloudNotes.Model;

namespace Server.Controllers
{
    public class OAuth2Controller : BaseController
    {
        public OAuth2Controller(IUserManager userManager)
            : base(userManager)
        { }

        //
        // GET: /OAuth2/Auth

        public ActionResult Auth(string response_type, string client_id, string redirect_uri)
        {
            // this request authenticates the user

            if (response_type != "code")
            {
                throw new HttpException(400, "Invalid request");
            }
            if (client_id != "androidcloudnotes")
            {
                throw new HttpException(404, "Client application not found");
            }
            // TODO save info about this user already approving the access?

            return View((object)client_id);
        }

        //
        // POST: /OAuth2/Approval
        [HttpPost]
        public ActionResult Approval(string client_id, string redirect_uri)
        {
            if (client_id != "androidcloudnotes")
            {
                throw new HttpException(404, "Client application not found");
            }

            var oAuthCodeRepo = ObjectFactory.GetInstance<IOAuthCodeRepository>();
            var code = new OAuthCode() { ClientId = client_id, User = User.UniqueId };
            oAuthCodeRepo.Create(code);
            if (!string.IsNullOrEmpty(redirect_uri))
            {
                return new RedirectResult(string.Format("{0}?code={1}", redirect_uri, code.Code));
            }
            else
            {
                Response.Cookies.Add(new HttpCookie("code", code.Code));
                return new ContentResult();
            }
        }

        //
        // GET: /OAuth2/NotApproved
        public ActionResult NotApproved()
        {
            // just return an empty page
            return new ContentResult();
        }

        //
        // POST: /OAuth2/Token
        [AllowAnonymous]
        [HttpPost]
        public ActionResult Token(string code, string redirect_uri, string grant_type, string client_id, string client_secret, string refresh_token)
        {
            // TODO create a manager to hide the two repos...

            if (client_id != "androidcloudnotes" && client_secret != "prompt")
            {
                throw new HttpException(404, "Client application not found");
            }
            
            if (grant_type == "authorization_code")
            {
                var oAuthCodeRepo = ObjectFactory.GetInstance<IOAuthCodeRepository>();
                var oAuthCode = oAuthCodeRepo.Get(client_id, code);
                var user = oAuthCode.User;

                // TODO also validate the time, should be less than 10 minutes
                if (code != oAuthCode.Code)
                {
                    throw new HttpException(400, "Invalid request");
                }

                oAuthCodeRepo.Delete(client_id, code);

                var oAuthTokenRepo = ObjectFactory.GetInstance<IOAuthTokenRepository>();
                var token = new OAuthToken() { TokenType = "Bearer", User = user };
                oAuthTokenRepo.Create(token);

                return new JsonResult()
                {
                    Data = new
                    {
                        access_token = token.Token,
                        token_type = token.TokenType,
                        expires_in = 3600,
                        refresh_token = token.RefreshToken
                    }
                };
            }
            else if (grant_type == "refresh_token")
            {
                var oAuthTokenRepo = ObjectFactory.GetInstance<IOAuthTokenRepository>();

                // the token has to be resent (in code param) to use in query
                var token = oAuthTokenRepo.Get("Bearer", code);
                if (token.RefreshToken != refresh_token)
                {
                    throw new HttpException(400, "Invalid request");
                }
                // TODO this is the manager responsibitiy!
                var refreshedToken = new OAuthToken() { TokenType = token.TokenType, User = token.User };
                oAuthTokenRepo.Delete("Bearer", code);
                oAuthTokenRepo.Create(refreshedToken);
                return new JsonResult()
                {
                    Data = new
                    {
                        access_token = refreshedToken.Token,
                        token_type = refreshedToken.TokenType,
                        expires_in = TimeSpan.FromHours(1).Seconds,
                        refresh_token = refreshedToken.RefreshToken
                    }
                };
            }

            throw new HttpException(400, "Invalid request");

        }

    }
}
