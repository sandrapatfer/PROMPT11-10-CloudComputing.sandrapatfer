using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PromptCloudNotes.Model;

namespace PromptCloudNotes.Interfaces.Managers
{
    public interface ITaskListManager
    {
        IEnumerable<TaskList> GetAllLists(string userId);
        TaskList GetTaskList(string userId, string listId, string creatorId);

        void CreateTaskList(User creatorUser, TaskList listData);

        void UpdateTaskList(string userId, string listId, string creatorId, TaskList listData);

        void DeleteTaskList(string userId, string listId, string creatorId);

        void ShareTaskList(string userId, string listId, string creatorId, string shareUserId);
    }
}
