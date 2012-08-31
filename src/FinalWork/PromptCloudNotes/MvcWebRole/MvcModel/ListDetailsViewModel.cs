using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Server.MvcModel
{
    public class ListViewModel
    {
        public IEnumerable<PromptCloudNotes.Model.TaskList> Lists { get; set; }
        public PromptCloudNotes.Model.TaskList SelectedList { get; set; }
    }
}