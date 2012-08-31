using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PromptCloudNotes.Model
{
    public class Notification
    {
        public string Id { get; set; }
        public DateTime At { get; set; }
        public NotificationType Type { get; set; }

        public Task Task { get; set; }
        public User User { get; set; }

        public enum NotificationType
        {
            Share,
            Insert,
            Update,
            Delete
        }
    }
}
