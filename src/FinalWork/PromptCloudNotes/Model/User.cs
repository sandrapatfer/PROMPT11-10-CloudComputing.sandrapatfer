using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PromptCloudNotes.Model
{
    public class User
    {
        public string UniqueId { get; set; }
        public string Name { get; set; }
        public string Provider { get; set; }
        public string NameIdentifier { get; set; }
        public string Email { get; set; }

        public ICollection<TaskList> Lists { get; set; }
        public ICollection<Notification> Notifications;
    }
}
