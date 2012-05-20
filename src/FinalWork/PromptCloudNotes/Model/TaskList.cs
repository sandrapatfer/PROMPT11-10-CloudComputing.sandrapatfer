using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PromptCloudNotes.Model
{
    public class TaskList : Task
    {
        public ICollection<Task> Tasks { get; set; }
    }
}
