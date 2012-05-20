using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PromptCloudNotes.Model;
using PromptCloudNotes.Interfaces;
using StructureMap;

namespace BLTests
{
    /// <summary>
    /// Summary description for NotificationTest
    /// </summary>
    [TestClass]
    public class NotificationTest
    {
        private User _creationUser;
        private IUserManager _userManager;
        private ITaskListManager _taskListManager;
        private INoteManager _noteManager;

        public NotificationTest()
        {
            IoC.Configure();

            _creationUser = new User();
            _userManager = ObjectFactory.GetInstance<IUserManager>();
            _userManager.CreateUser(_creationUser);

            _taskListManager = ObjectFactory.GetInstance<ITaskListManager>();
            _noteManager = ObjectFactory.GetInstance<INoteManager>();
        }

        private TaskList CreateTaskList()
        {
            TaskList tl = new TaskList() { Name = "new list" };
            return _taskListManager.CreateTaskList(_creationUser.Id, tl);
        }

        [TestMethod]
        public void ShareListNotificationTest()
        {
            TaskList list = CreateTaskList();

            var user2 = new User();
            _userManager.CreateUser(user2);

            _taskListManager.ShareTaskList(list.Id, user2.Id);

            Assert.AreEqual(1, user2.Notifications.Count);
            Assert.AreEqual(Notification.NotificationType.Share, user2.Notifications.First().Type);
        }

        [TestMethod]
        public void UpdateListNotificationTest()
        {
            TaskList list = CreateTaskList();

            var user2 = new User();
            _userManager.CreateUser(user2);

            _taskListManager.ShareTaskList(list.Id, user2.Id);
            user2.Notifications.Clear();

            list.Name = "Changed name";
            _taskListManager.UpdateTaskList(list.Id, list);

            Assert.AreEqual(1, user2.Notifications.Count);
            Assert.AreEqual(Notification.NotificationType.Update, user2.Notifications.First().Type);
            Assert.AreEqual(list.Id, user2.Notifications.First().Task.Id);
        }

        [TestMethod]
        public void DeleteListNotificationTest()
        {
            TaskList list = CreateTaskList();

            var user2 = new User();
            _userManager.CreateUser(user2);

            _taskListManager.ShareTaskList(list.Id, user2.Id);
            user2.Notifications.Clear();

            _taskListManager.DeleteTaskList(list.Id);

            Assert.AreEqual(1, user2.Notifications.Count);
            Assert.AreEqual(Notification.NotificationType.Delete, user2.Notifications.First().Type);
            Assert.AreEqual(list.Id, user2.Notifications.First().Task.Id);
        }

        private Note CreateNote()
        {
            TaskList tl = new TaskList() { Name = "new list" };
            _taskListManager.CreateTaskList(_creationUser.Id, tl);

            Note n = new Note() { Name = "new note" };
            return _noteManager.CreateNote(_creationUser.Id, tl.Id, n);
        }

        [TestMethod]
        public void ShareNoteNotificationTest()
        {
            var note = CreateNote();
         
            var user2 = new User();
            _userManager.CreateUser(user2);

            _noteManager.ShareNote(note.ParentList.Id, note.Id, user2.Id);

            Assert.AreEqual(1, user2.Notifications.Count);
            Assert.AreEqual(Notification.NotificationType.Share, user2.Notifications.First().Type);
        }

        [TestMethod]
        public void UpdateNoteNotificationTest()
        {
            var note = CreateNote();

            var user2 = new User();
            _userManager.CreateUser(user2);

            _noteManager.ShareNote(note.ParentList.Id, note.Id, user2.Id);
            user2.Notifications.Clear();

            note.Name = "Changed name";
            _noteManager.UpdateNote(note.ParentList.Id, note.Id, note);

            Assert.AreEqual(1, user2.Notifications.Count);
            Assert.AreEqual(Notification.NotificationType.Update, user2.Notifications.First().Type);
            Assert.AreEqual(note.Id, user2.Notifications.First().Task.Id);
        }

        [TestMethod]
        public void DeleteNoteNotificationTest()
        {
            var note = CreateNote();

            var user2 = new User();
            _userManager.CreateUser(user2);

            _noteManager.ShareNote(note.ParentList.Id, note.Id, user2.Id);
            user2.Notifications.Clear();

            _noteManager.DeleteNote(note.ParentList.Id, note.Id);

            Assert.AreEqual(1, user2.Notifications.Count);
            Assert.AreEqual(Notification.NotificationType.Delete, user2.Notifications.First().Type);
            Assert.AreEqual(note.Id, user2.Notifications.First().Task.Id);
        }

        [TestMethod]
        public void InsertNoteInListNotificationTest()
        {
            TaskList list = CreateTaskList();

            var user2 = new User();
            _userManager.CreateUser(user2);

            _taskListManager.ShareTaskList(list.Id, user2.Id);
            user2.Notifications.Clear();

            Note note = new Note() { Name = "new note" };
            _noteManager.CreateNote(_creationUser.Id, list.Id, note);

            Assert.AreEqual(1, user2.Notifications.Count);
            Assert.AreEqual(Notification.NotificationType.Insert, user2.Notifications.First().Type);
            Assert.AreEqual(note.Id, user2.Notifications.First().Task.Id);
        }

        [TestMethod]
        public void DeleteNoteFromListNotificationTest()
        {
            TaskList list = CreateTaskList();

            var user2 = new User();
            _userManager.CreateUser(user2);
            _taskListManager.ShareTaskList(list.Id, user2.Id);

            Note note = new Note() { Name = "new note" };
            _noteManager.CreateNote(_creationUser.Id, list.Id, note);
            user2.Notifications.Clear();

            var user3 = new User();
            _userManager.CreateUser(user3);
            _noteManager.ShareNote(list.Id, note.Id, user3.Id);
            user3.Notifications.Clear();

            _noteManager.DeleteNote(list.Id, note.Id);

            Assert.AreEqual(1, user2.Notifications.Count);
            Assert.AreEqual(Notification.NotificationType.Update, user2.Notifications.First().Type);
            Assert.AreEqual(note.Id, user2.Notifications.First().Task.Id);

            Assert.AreEqual(1, user3.Notifications.Count);
            Assert.AreEqual(Notification.NotificationType.Delete, user3.Notifications.First().Type);
            Assert.AreEqual(note.Id, user3.Notifications.First().Task.Id);
        }
    }
}
