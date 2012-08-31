using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PromptCloudNotes.Interfaces.Repositories
{
    public interface IRepository<T>
    {
        // A good repository should use IQueryable, a repository over Azure has to convert the return to avoid errors of using operators not supported

        IEnumerable<T> GetAll();
        IEnumerable<T> GetAll(string partitionKey);

        T Get(string partitionKey, string rowKey);

        void Create(T newEntity);

        void Update(string partitionKey, string rowKey, T changedEntity);

        void Delete(string partitionKey, string rowKey);

    }
}
