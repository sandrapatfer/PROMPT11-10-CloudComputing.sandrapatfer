using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PromptCloudNotes.Model;
using StructureMap;
using PromptCloudNotes.Interfaces.Managers;

namespace BLTests
{
    [TestClass]
    public class NoteTest
    {
        private User _user;
        private TaskList _taskList;
        private ITaskListManager _taskListManager;
        private INoteManager _noteManager;

        public NoteTest()
        {
            IoC.Configure();

            _user = new User();
            IUserManager um = ObjectFactory.GetInstance<IUserManager>();
            um.CreateUser(_user);

            _taskList = new TaskList() { Name = "new list", Creator = _user, Users = new List<User>() { _user } };
            _taskListManager = ObjectFactory.GetInstance<ITaskListManager>();
            _taskListManager.CreateTaskList(_user, _taskList);

            _noteManager = ObjectFactory.GetInstance<INoteManager>();
        }

        [TestMethod]
        public void CreateNoteTest()
        {
            Note n = new Note() { Name = "new note" };
            _noteManager.CreateNote(_user, _taskList.Id, _user.UniqueId, n);
            Assert.IsNotNull(n.Id);
        }

        Note CreateNote()
        {
            Note n = new Note() { Name = "new note", Creator = _user };
            _noteManager.CreateNote(_user, _taskList.Id, _user.UniqueId, n);
            return n;
        }

        [TestMethod]
        public void GetNoteTest()
        {
            var n = CreateNote();
            var n2 = _noteManager.GetNote(_user.UniqueId, _taskList.Id, n.Id);
            Assert.IsNotNull(n2);
        }

        [TestMethod]
        public void UpdateNoteTest()
        {
            var n = CreateNote();
            n.Name = "New name";
            _noteManager.UpdateNote(_user.UniqueId, _taskList.Id, n.Id, n);
            var n2 = _noteManager.GetNote(_user.UniqueId, _taskList.Id, n.Id);
            Assert.AreEqual(n.Name, n2.Name);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))] // TODO set the correct exception
        public void DeleteNoteTest()
        {
            var n = CreateNote();
            _noteManager.DeleteNote(_user.UniqueId, n.ParentList.Id, _user.UniqueId, n.Id);
            var n2 = _noteManager.GetNote(_user.UniqueId, _taskList.Id, n.Id);
        }

        [TestMethod]
        public void ShareNoteTest()
        {
            var n = CreateNote();

            var user = new User();
            IUserManager um = ObjectFactory.GetInstance<IUserManager>();
            um.CreateUser(user);

            _noteManager.ShareNote(_user.UniqueId, _taskList.Id, n.Id, user.UniqueId);

            var n2 = _noteManager.GetNote(_user.UniqueId, _taskList.Id, n.Id);
            Assert.IsTrue(n2.Users.Contains(user));
        }

        [TestMethod]
        public void ChangeNodeIndexTest()
        {
            var n1 = CreateNote();
            var n2 = CreateNote();
            var n3 = CreateNote();

            _noteManager.ChangeOrder(_user.UniqueId, n3.Id, 0);

            Assert.AreEqual(n1.ListOrder, 1);
            Assert.AreEqual(n2.ListOrder, 2);
            Assert.AreEqual(n3.ListOrder, 0);
        }

        [TestMethod]
        public void NodeOrderAfterDeleteTest()
        {
            var n1 = CreateNote();
            var n2 = CreateNote();
            var n3 = CreateNote();

            _noteManager.DeleteNote(_user.UniqueId, n1.ParentList.Id, _user.UniqueId, n2.Id);

            Assert.AreEqual(n1.ListOrder, 0);
            Assert.AreEqual(n3.ListOrder, 1);
        }

    }
}
