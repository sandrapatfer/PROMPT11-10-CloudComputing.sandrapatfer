using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PromptCloudNotes.Model
{
    public class OAuthToken
    {
        public string TokenType { get; set; }
        public string Token { get; set; }
        public string User { get; set; }
        public DateTime CreatedAt { get; set; }
        public string RefreshToken { get; set; }
    }
}
