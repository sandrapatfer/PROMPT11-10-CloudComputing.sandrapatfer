using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Principal;

namespace Server.Utils
{
    public class UserIdentity : GenericIdentity
    {
        public PromptCloudNotes.Model.User User { get; set; }
    }
}