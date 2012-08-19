using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StructureMap;
using PromptCloudNotes.BusinessLayer.Managers;
using System.Web.Mvc;
using PromptCloudNotes.Interfaces;
using PromptCloudNotes.Model;

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

                ex.For<IUserRepository>().Use<PromptCloudNotes.AzureRepo.UserRepository>();
                ex.For<ITaskListRepository>().Use<PromptCloudNotes.AzureRepo.TaskListRepository>();
                ex.For<INoteRepository>().Use<PromptCloudNotes.AzureRepo.NoteRepository>();
                ex.For<INotificationRepository>().Use<PromptCloudNotes.AzureRepo.NotificationRepository>();

                ex.For<IUserManager>().HttpContextScoped().Use<UserManager>();
                ex.For<ITaskListManager>().HttpContextScoped().Use<TaskListManager>();
                ex.For<INoteManager>().HttpContextScoped().Use<NoteManager>();
                ex.For<ITaskManager>().HttpContextScoped().Use<TaskManager>();
                ex.For<INotificationManager>().HttpContextScoped().Use<NotificationManager>();
            });

            DependencyResolver.SetResolver(new SmDependencyResolver(ObjectFactory.Container));
        }

        public static void ConfigureTestEnv()
        {
            ObjectFactory.Initialize(ex =>
            {
                ex.Scan(scan =>
                {
                    scan.TheCallingAssembly();
                    scan.WithDefaultConventions();
                });

                ex.For<INotificationProcessor>().Use<DummyNotificationProcessor>();

                ex.For<IUserRepository>().Singleton().Use<PromptCloudNotes.InMemoryRepo.UserRepository>();
                ex.For<ITaskListRepository>().Singleton().Use<PromptCloudNotes.InMemoryRepo.TaskListRepository>();
                ex.For<INoteRepository>().Singleton().Use<PromptCloudNotes.InMemoryRepo.NoteRepository>();
                ex.For<INotificationRepository>().Singleton().Use<PromptCloudNotes.InMemoryRepo.NotificationRepository>();

                ex.For<IUserManager>().HttpContextScoped().Use<UserManager>();
                ex.For<ITaskListManager>().HttpContextScoped().Use<TaskListManager>();
                ex.For<INoteManager>().HttpContextScoped().Use<NoteManager>();
                ex.For<ITaskManager>().HttpContextScoped().Use<TaskManager>();
                ex.For<INotificationManager>().HttpContextScoped().Use<NotificationManager>();
            });

            DependencyResolver.SetResolver(new SmDependencyResolver(ObjectFactory.Container));

            // generate dummy data in test environment 
            var user = new User();
            user.UserName = "Sandra Fernandes";
            var um = ObjectFactory.GetInstance<IUserManager>();
            user = um.CreateUser(user);

            var list = new TaskList() { Name = "Dummy list", Description = "Dummy description" };
            var tlm = ObjectFactory.GetInstance<ITaskListManager>();
            list = tlm.CreateTaskList(user, list);

            var note = new Note() { Name = "note name", Description = "note description" };
            var nm = ObjectFactory.GetInstance<INoteManager>();
            note = nm.CreateNote(user.Id, list.Id, note);
        }

    }
}
