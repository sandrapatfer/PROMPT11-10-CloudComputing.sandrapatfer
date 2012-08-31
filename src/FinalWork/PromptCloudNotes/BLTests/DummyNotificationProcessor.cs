using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PromptCloudNotes.Interfaces;

namespace BLTests
{
    class DummyNotificationProcessor : INotificationProcessor
    {
        public bool Send(PromptCloudNotes.Model.Notification notification)
        {
            return false;
        }
    }
}
