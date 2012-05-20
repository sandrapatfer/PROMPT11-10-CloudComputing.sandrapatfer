using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PromptCloudNotes.Model;

namespace PromptCloudNotes.Interfaces
{
    public interface ITaskManager
    {
        Note CopyNote(int userId, int destListId, Note noteData);

        Note MoveNote(int userId, int destListId, int sourceListId, Note noteData);
    }
}
