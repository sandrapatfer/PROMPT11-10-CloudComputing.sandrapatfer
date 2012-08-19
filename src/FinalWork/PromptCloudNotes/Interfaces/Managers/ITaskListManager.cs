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

        TaskList CreateTaskList(User user, TaskList listData);

        TaskList GetTaskList(int userId, int listId);

        void UpdateTaskList(int userId, int listId, TaskList listData);

        void DeleteTaskList(int userId, int listId);

        void ShareTaskList(int userId, int listId, int shareUserId);
    }
}
