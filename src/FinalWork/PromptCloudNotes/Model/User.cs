using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PromptCloudNotes.Model
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }

        public ICollection<Notification> Notifications;

        public override bool Equals(object obj)
        {
            // todo resharper???
            return base.Equals(obj);
        }
    }
}
