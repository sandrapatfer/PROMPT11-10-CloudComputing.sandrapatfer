using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PromptCloudNotes.Model;

namespace PromptCloudNotes.Interfaces
{
    public interface ITaskListManager
    {
        IEnumerable<TaskList> GetAllLists(int userId);

        TaskList CreateTaskList(int userId, TaskList listData);

        TaskList GetTaskList(int listId);

        void UpdateTaskList(int listId, TaskList listData);

        void DeleteTaskList(int listId);

        void ShareTaskList(int listId, int userId);
    }
}
