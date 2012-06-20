﻿using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PromptCloudNotes.Model;
using PromptCloudNotes.Interfaces;
using StructureMap;

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

            _taskList = new TaskList() { Name = "new list", Creator = _user };
            _taskListManager = ObjectFactory.GetInstance<ITaskListManager>();
            _taskListManager.CreateTaskList(_user.Id, _taskList);

            _noteManager = ObjectFactory.GetInstance<INoteManager>();
        }

        [TestMethod]
        public void CreateNoteTest()
        {
            Note n = new Note() { Name = "new note" };
            var n2 = _noteManager.CreateNote(_user.Id, _taskList.Id, n);
            Assert.IsNotNull(n2.Id);
        }

        Note CreateNote()
        {
            Note n = new Note() { Name = "new note", Creator = _user };
            return _noteManager.CreateNote(_user.Id, _taskList.Id, n);
        }

        [TestMethod]
        public void GetNoteTest()
        {
            var n = CreateNote();
            var n2 = _noteManager.GetNote(_user.Id, _taskList.Id, n.Id);
            Assert.IsNotNull(n2);
        }

        [TestMethod]
        public void UpdateNoteTest()
        {
            var n = CreateNote();
            n.Name = "New name";
            _noteManager.UpdateNote(_user.Id, _taskList.Id, n.Id, n);
            var n2 = _noteManager.GetNote(_user.Id, _taskList.Id, n.Id);
            Assert.AreEqual(n.Name, n2.Name);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))] // TODO set the correct exception
        public void DeleteNoteTest()
        {
            var n = CreateNote();
            _noteManager.DeleteNote(_user.Id, _taskList.Id, n.Id);
            var n2 = _noteManager.GetNote(_user.Id, _taskList.Id, n.Id);
        }

        [TestMethod]
        public void ShareNoteTest()
        {
            var n = CreateNote();

            var user = new User();
            IUserManager um = ObjectFactory.GetInstance<IUserManager>();
            um.CreateUser(user);

            _noteManager.ShareNote(_user.Id, _taskList.Id, n.Id, user.Id);

            var n2 = _noteManager.GetNote(_user.Id, _taskList.Id, n.Id);
            Assert.IsTrue(n2.Users.Contains(user));
        }

        [TestMethod]
        public void ChangeNodeIndexTest()
        {
            var n1 = CreateNote();
            var n2 = CreateNote();
            var n3 = CreateNote();

            _noteManager.ChangeOrder(_user.Id, _taskList.Id, n3.Id, 0);

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

            _noteManager.DeleteNote(_user.Id, _taskList.Id, n2.Id);

            Assert.AreEqual(n1.ListOrder, 0);
            Assert.AreEqual(n3.ListOrder, 1);
        }

    }
}
