using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StructureMap;
using PromptCloudNotes.Interfaces;
using InMemoryRepo;
using BusinessLayer.Managers;

namespace Server
{
    internal class IoC
    {
        public static void Configure()
        {
            ObjectFactory.Initialize(ex =>
            {
                ex.Scan(scan=>
                {
                    scan.TheCallingAssembly();
                    scan.WithDefaultConventions();
                });

                ex.For<INotificationProcessor>().Use<DummyNotificationProcessor>();

                ex.For<IUserRepository>().Singleton().Use<UserRepository>();
                ex.For<ITaskListRepository>().Singleton().Use<TaskListRepository>();
                ex.For<INoteRepository>().Singleton().Use<NoteRepository>();
                ex.For<INotificationRepository>().Singleton().Use<NotificationRepository>();

                ex.For<IUserManager>().Use<UserManager>();
                ex.For<ITaskListManager>().Use<TaskListManager>();
                ex.For<INoteManager>().Use<NoteManager>();
                ex.For<ITaskManager>().Use<TaskManager>();
                ex.For<INotificationManager>().Use<NotificationManager>();
            });
        }
    }
}
