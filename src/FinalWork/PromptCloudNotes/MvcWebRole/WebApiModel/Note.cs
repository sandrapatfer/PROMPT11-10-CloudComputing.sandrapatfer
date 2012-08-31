using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Server.WebApiModel
{
    public class Note
    {
        public string id { get; set; }
        public string title { get; set; }
        public string listCreatorId { get; set; }
    }
}