using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StructureMap;
using PromptCloudNotes.Interfaces.Queues;
using PromptCloudNotes.Model;

namespace PromptCloudNotes.NotificationsWorkerRole.Utils
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

                ex.For<INotificationQueue>().Use<PromptCloudNotes.AzureRepo.NotificationQueue>();

            });

        }
    }
}
