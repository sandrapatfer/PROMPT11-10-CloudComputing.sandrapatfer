using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PromptCloudNotes.Interfaces;
using PromptCloudNotes.Model;

namespace BusinessLayer.Managers
{
    public class TaskManager : ITaskManager
    {
        private ITaskListManager _taskListManager;
        private INoteManager _noteManager;

        public TaskManager(ITaskListManager listMgr, INoteManager noteMgr)
        {
            _taskListManager = listMgr;
            _noteManager = noteMgr;
        }

        #region ITaskManager Members

        public Note CopyNote(int userId, int destListId, Note noteData)
        {
            return _noteManager.CreateNote(userId, destListId, noteData);
        }

        public Note MoveNote(int userId, int destListId, int sourceListId, Note noteData)
        {
            _noteManager.DeleteNote(userId, sourceListId, noteData.Id);
            Note newNote = _noteManager.CreateNote(userId, destListId, noteData);
            return newNote;
        }

        #endregion
    }
}
