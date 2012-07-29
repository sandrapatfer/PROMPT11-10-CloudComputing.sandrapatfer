using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PromptCloudNotes.Model;
using PromptCloudNotes.AzureRepo;

namespace ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var repo = new TaskListRepository();
            var list = repo.Create(1, new TaskList() { Name = "new list", Description = "new descr for new list" });
            Console.WriteLine(string.Format("Created: {0}", list.Id));

            var lists = repo.GetAll(1);
            foreach (var createdList in lists)
            {
                Console.WriteLine(string.Format("List in DB: name: {0}, descr: {1}", createdList.Name, createdList.Description));
            }

            repo.Update(1, new TaskList() { Name = "xxx", Description = "yyy" });
            Console.WriteLine("List updated");

            var noteRepo = new NoteRepository();
            var note = noteRepo.Create(1, list.Id, new Note() { Name = "new note", Description = "new descr for new note" });
            Console.WriteLine(string.Format("Created note: {0}", note.Id));

            var notes = noteRepo.GetAll(1, list.Id);
            foreach (var createdNote in notes)
            {
                Console.WriteLine(string.Format("Note in DB: name: {0}, descr: {1}", createdNote.Name, createdNote.Description));
            }

            noteRepo.Update(list.Id, note.Id, new Note() { Name = "xxx" });
            Console.WriteLine("Note updated");

            
            Console.ReadLine();
        }
    }
}
