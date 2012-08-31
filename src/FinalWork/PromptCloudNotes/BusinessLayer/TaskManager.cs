using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PromptCloudNotes.Interfaces.Managers;
using PromptCloudNotes.Interfaces.Repositories;
using PromptCloudNotes.Model;

namespace PromptCloudNotes.BusinessLayer.Managers
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

        public Note CopyNote(User user, string destListId, string destListCreatorId, Note noteData)
        {
            var newNote = new Note()
            {
                Name = noteData.Name,
                Description = noteData.Description,
                Creator = user,
                ParentList = new TaskList() { Id = destListId, Creator = new User() { UniqueId = destListCreatorId } }
            };
            _noteManager.CreateNote(user, destListId, destListCreatorId, newNote);
            return newNote;
        }

        public void MoveNote(User user, string destListId, string destListCreatorId, 
            string sourceListId, string sourceListCreatorId, Note noteData)
        {
            var newList = _taskListManager.GetTaskList(user.UniqueId, destListId, destListCreatorId);
            if (newList == null)
            {
                // TODO exception
            }

            // TODO para simular uma transacao isto devia ser feito directamente nos repos
            _noteManager.DeleteNote(user.UniqueId, sourceListId, sourceListCreatorId, noteData.Id);

            // TODO nao pode ser um create porque senao muda-se o id
            _noteManager.CreateNote(user, destListId, destListCreatorId, noteData);
        }

        #endregion
    }
}
