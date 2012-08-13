using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Server.Utils;

namespace Server.Controllers
{
    public class OAuth2Controller : Controller
    {
        //
        // GET: /OAuth2/Auth

        public ActionResult Auth(string response_type, string client_id, string redirect_uri)
        {
            // neste pedido o user autenticado é o user da app

            if (response_type != "code")
            {
                // TODO bad request
            }
            // TODO validate client

            // TODO generate state base64 id for the post
            return View();
        }

        //
        // POST: /OAuth2/Approval
        [HttpPost]
        public ActionResult Approval(string client_id, string redirect_uri)
        {
            string code = "123";

            if (!string.IsNullOrEmpty(redirect_uri))
            {
                return new RedirectResult(string.Format("{0}?code={1}", redirect_uri, code));
            }
            else
            {
                Response.Cookies.Add(new HttpCookie("code", code));
                return new ContentResult();
            }
        }

        //
        // POST: /OAuth2/Token
        [AllowAnonymous]
        [HttpPost]
        public ActionResult Token(string code, string redirect_uri, string grant_type, string client_id, string client_secret)
        {
            // TODO validar todos os params

            return new JsonResult()
            {
                Data = new
                {
                    access_token = "456",
                    token_type = "Bearer",
                    expires_in = 3600,
                    refresh_toke = "1"
                }
            };
        }

    }
}
