using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PromptCloudNotes.Interfaces.Queues
{
    public interface IQueue<T>
    {
        T GetMessage();

        void SendMessage(T newEntity);

        void DeleteMessage(T newEntity);
    }
}
