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

        TaskList Create(User user, TaskList listData);
        TaskList Get(int listId);
        TaskList GetWithUsers(int listId);
        void Update(int listId, TaskList listData);
        void Delete(int listId);

        void Share(int listId, User userId);
    }
}
