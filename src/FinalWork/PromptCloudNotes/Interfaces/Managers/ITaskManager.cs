using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PromptCloudNotes.Model;

namespace PromptCloudNotes.Interfaces.Managers
{
    public interface ITaskManager
    {
        Note CopyNote(User user, string destListId, string destListCreatorId, Note noteData);

        void MoveNote(User user, string destListId, string destListCreatorId, 
            string sourceListId, string sourceListCreatorId, Note noteData);
    }
}
