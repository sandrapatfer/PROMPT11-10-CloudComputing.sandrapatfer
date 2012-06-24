using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PromptCloudNotes.Model;

namespace PromptCloudNotes.Interfaces
{
    public interface ITaskListRepository
    {
        IEnumerable<TaskList> GetAll(int userId);

        TaskList Create(int userId, TaskList listData);
        TaskList Get(int listId);
        void Update(int listId, TaskList listData);
        void Delete(int listId);

        void Share(int listId, int userId);
    }
}
