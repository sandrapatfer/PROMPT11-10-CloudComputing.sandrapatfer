using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Server.MvcModel
{
    public class Note
    {
        public int id { get; set; }
        public int listId { get; set; }
        public string name { get; set; }
        public string description { get; set; }
    }
}