using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure.StorageClient;

namespace PromptCloudNotes.AzureRepo.Model
{
    class NotificationEntity: TableServiceEntity
    {
        public NotificationEntity() { }

        public NotificationEntity(int userId, int notificationId)
            : base(userId.ToString(), notificationId.ToString())
        { }

        public int ListId { get; set; }
        public int NoteId { get; set; }
    }
}
