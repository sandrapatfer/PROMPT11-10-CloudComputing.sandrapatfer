using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PromptCloudNotes.Interfaces.Repositories;
using PromptCloudNotes.Model;
using Microsoft.WindowsAzure;

namespace PromptCloudNotes.AzureRepo
{
    using Model;
    using Exceptions;

    public class TaskListRepository : AzureTable<TaskList, TaskListEntity>, ITaskListRepository
    {
        private const string TABLE_NAME = "TaskListTable";

        public TaskListRepository()
            : base(TABLE_NAME)
        { }

        public IEnumerable<TaskList> GetAll()
        {
            throw new NotImplementedException();
        }

        public new IEnumerable<TaskList> GetAll(string partitionKey)
        {
            return GetAll(partitionKey, l => l.GetTaskList());
        }

        public new TaskList Get(string partitionKey, string rowKey)
        {
            return Get(partitionKey, rowKey, l => l.GetTaskList());
        }

        public void Create(TaskList newEntity)
        {
            newEntity.Id = Guid.NewGuid().ToString();
            Create(new TaskListEntity(newEntity));
        }

        public void Update(string partitionKey, string rowKey, TaskList changedEntity)
        {
            var list = base.Get(partitionKey, rowKey);
            if (list == null)
            {
                throw new ObjectNotFoundException();
            }
            list.Update(changedEntity);
            Update(list);
        }

        public void Delete(string partitionKey, string rowKey)
        {
            throw new NotImplementedException();
        }

    }
}
