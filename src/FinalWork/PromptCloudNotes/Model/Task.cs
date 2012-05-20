using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PromptCloudNotes.Model
{
    public class Task
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public User Creator { get; set; }

        public TaskList ParentList { get; set; }
        public int ListOrder { get; set; }

        public ICollection<User> Users { get; set; }
    }
}
