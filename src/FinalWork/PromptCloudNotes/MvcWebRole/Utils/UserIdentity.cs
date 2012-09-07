using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Principal;

namespace Server.Utils
{
    public class UserIdentity : GenericIdentity
    {
        public UserIdentity(string userId)
            : base(userId)
        {
            UserId = userId;
        }

        public string UserId { get; set; }
    }
}