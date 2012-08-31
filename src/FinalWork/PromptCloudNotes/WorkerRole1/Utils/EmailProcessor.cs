using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PromptCloudNotes.Interfaces.Queues;
using StructureMap;

namespace PromptCloudNotes.NotificationsWorkerRole.Utils
{
    public class EmailProcessor
    {
        private INotificationQueue _repository;

        public EmailProcessor()
        {
            _repository = ObjectFactory.GetInstance<INotificationQueue>();
        }

        public void ProcessPendingMessages()
        {
            var message = _repository.GetMessage();
            while (message != null)
            {
                // TODO send email

                // TODO delete message ?

                message = _repository.GetMessage();
            }
        }
    }
}
