using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AzureRepo;
using PromptCloudNotes.Model;

namespace ConsoleTest
{
    class Program
    {
        private const string CONNECTION_STRING = "DefaultEndpointsProtocol=http;AccountName=spfcloudnotes;AccountKey=Gdi0M+gd1mO7a183WgRP8zxag5Fh2t0NNnh8Qvmz47V4vVDBe7JIXjdQS3wH0aLqRlpqNUqfzfuSC3TCgjVkLg==";

        static void Main(string[] args)
        {
            var tableUtil = new AzureUtils.Table(CONNECTION_STRING);

            var repo = new TaskListRepository(tableUtil);
            var list = repo.Create(1, new TaskList() { Name = "new list", Description = "new descr for new list" });
            Console.WriteLine(string.Format("Created: {0}", list.Id));

            var lists = repo.GetAll(1);
            foreach (var createdList in lists)
            {
                Console.WriteLine(string.Format("List in DB: name: {0}, descr: {1}", createdList.Name, createdList.Description));
            }

            var newlist = repo.Get(2);

            repo.Update(1, new TaskList() { Name = "xxx", Description = "yyy" });

            Console.ReadLine();
        }
    }
}
