using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PromptCloudNotes.Model
{
    public class OAuthCode
    {
        public string ClientId { get; set; }
        public string Code { get; set; }
        public string User { get; set; }
    }
}
