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
    /// <summary>
    /// Summary description for TaskTest
    /// </summary>
    [TestClass]
    public class TaskTest
    {
        private User _user;
        private ITaskListManager _taskListManager;
        private INoteManager _noteManager;
        private ITaskManager _taskManager;

        public TaskTest()
        {
            IoC.Configure();

            _user = new User();
            IUserManager um = ObjectFactory.GetInstance<IUserManager>();
            um.CreateUser(_user);

            _taskListManager = ObjectFactory.GetInstance<ITaskListManager>();
            _noteManager = ObjectFactory.GetInstance<INoteManager>();
            _taskManager = ObjectFactory.GetInstance<ITaskManager>();
        }

        Note CreateNote()
        {
            var taskList = new TaskList() { Name = "new list" };
            _taskListManager.CreateTaskList(_user.Id, taskList);
            Note n = new Note() { Name = "new note" };
            return _noteManager.CreateNote(_user.Id, taskList.Id, n);
        }

        [TestMethod]
        public void CopyNoteTest()
        {
            var n1 = CreateNote();

            var tl2 = new TaskList() { Name = "second list" };
            _taskListManager.CreateTaskList(_user.Id, tl2);

            var n2 = _taskManager.CopyNote(_user.Id, tl2.Id, n1);

            Assert.AreEqual(tl2.Id, n2.ParentList.Id);
        }

        [TestMethod]
        public void MoveNoteTest()
        {
            var n1 = CreateNote();
            int originalId = n1.Id;
            int originalList = n1.ParentList.Id;

            var tl2 = new TaskList() { Name = "second list" };
            _taskListManager.CreateTaskList(_user.Id, tl2);

            var n2 = _taskManager.MoveNote(_user.Id, tl2.Id, n1.ParentList.Id, n1);

            Assert.AreEqual(tl2.Id, n2.ParentList.Id);

            try
            {
                var n3 = _noteManager.GetNote(originalList, originalId);
                Assert.Fail();
            }
            catch (InvalidOperationException) // TODO set the correct exception
            {
            }
        }
    }
}