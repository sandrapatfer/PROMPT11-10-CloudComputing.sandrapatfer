using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PromptCloudNotes.Model;
using PromptCloudNotes.Interfaces.Managers;
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
            var taskList = new TaskList() { Name = "new list", Creator = _user, Users = new List<User>() { _user } };
            _taskListManager.CreateTaskList(_user, taskList);
            Note n = new Note() { Name = "new note" };
            _noteManager.CreateNote(_user, taskList.Id, _user.UniqueId, n);
            return n;
        }

        [TestMethod]
        public void CopyNoteTest()
        {
            var n1 = CreateNote();

            var tl2 = new TaskList() { Name = "second list", Creator = _user, Users = new List<User>() { _user } };
            _taskListManager.CreateTaskList(_user, tl2);

            var n2 = _taskManager.CopyNote(_user, tl2.Id, _user.UniqueId, n1);

            Assert.AreEqual(tl2.Id, n2.ParentList.Id);
        }

        [TestMethod]
        public void MoveNoteTest()
        {
            var n1 = CreateNote();
            string originalId = n1.Id;
            string originalList = n1.ParentList.Id;

            var tl2 = new TaskList() { Name = "second list", Creator = _user, Users = new List<User>() { _user } };
            _taskListManager.CreateTaskList(_user, tl2);

            _taskManager.MoveNote(_user, tl2.Id, _user.UniqueId, n1.ParentList.Id, _user.UniqueId, n1);

            Assert.AreEqual(tl2.Id, n1.ParentList.Id);

            try
            {
                var n3 = _noteManager.GetNote(_user.UniqueId, originalList, originalId);
                Assert.Fail();
            }
            catch (InvalidOperationException) // TODO set the correct exception
            {
            }
        }
    }
}
