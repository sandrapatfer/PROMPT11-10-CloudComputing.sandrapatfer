using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Server.MvcModel
{
    public class Note
    {
        public string id { get; set; }
        public string listId { get; set; }
        public string listCreatorId { get; set; }
        public string name { get; set; }
        public string description { get; set; }
    }
}