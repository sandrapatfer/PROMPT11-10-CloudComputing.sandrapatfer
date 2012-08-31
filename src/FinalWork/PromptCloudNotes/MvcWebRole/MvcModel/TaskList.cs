using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Server.MvcModel
{
    public class TaskList
    {
        public string id { get; set; }
        public string creatorId { get; set; }
 
        public string name { get; set; }
        public string description { get; set; }
    }
}