using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StructureMap;
using PromptCloudNotes.Interfaces;
using InMemoryRepo;
using BusinessLayer.Managers;
using System.Web.Mvc;

namespace Server.Utils
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

                ex.For<IUserManager>().HttpContextScoped().Use<UserManager>();
                ex.For<ITaskListManager>().HttpContextScoped().Use<TaskListManager>();
                ex.For<INoteManager>().HttpContextScoped().Use<NoteManager>();
                ex.For<ITaskManager>().HttpContextScoped().Use<TaskManager>();
                ex.For<INotificationManager>().HttpContextScoped().Use<NotificationManager>();
            });

            DependencyResolver.SetResolver(new SmDependencyResolver(ObjectFactory.Container));
        }
    }
}
