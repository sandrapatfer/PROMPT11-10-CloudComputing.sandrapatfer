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
    [TestClass]
    public class TaskListTest
    {
        private User _user;
        private ITaskListManager _taskListManager;

        public TaskListTest()
        {
            IoC.Configure();

            _user = new User();
            IUserManager um = ObjectFactory.GetInstance<IUserManager>();
            um.CreateUser(_user);

            _taskListManager = ObjectFactory.GetInstance<ITaskListManager>();
        }

        [TestMethod]
        public void CreateListTest()
        {
            TaskList tl = new TaskList() { Name = "new list" };
            var tl2 = _taskListManager.CreateTaskList(_user.Id, tl);
            Assert.IsNotNull(tl2.Id);
        }

        TaskList CreateTaskList()
        {
            TaskList tl = new TaskList() { Name = "new list" };
            return _taskListManager.CreateTaskList(_user.Id, tl);
        }

        [TestMethod]
        public void GetListTest()
        {
            var tl = CreateTaskList();
            var tl2 = _taskListManager.GetTaskList(tl.Id);
            Assert.IsNotNull(tl2);
            Assert.AreEqual(tl.Id, tl2.Id);
        }

        [TestMethod]
        public void UpdateListTest()
        {
            var tl = CreateTaskList();
            tl.Name = "New name";
            _taskListManager.UpdateTaskList(tl.Id, tl);
            var tl2 = _taskListManager.GetTaskList(tl.Id);
            Assert.AreEqual(tl.Name, tl2.Name);
        }

        [TestMethod]
        public void DeleteListTest()
        {
            var tl = CreateTaskList();
            _taskListManager.DeleteTaskList(tl.Id);
            var tl2 = _taskListManager.GetTaskList(tl.Id);
            Assert.IsNull(tl2);
        }

        [TestMethod]
        public void ShareListTest()
        {
            var tl = CreateTaskList();

            var user = new User();
            IUserManager um = ObjectFactory.GetInstance<IUserManager>();
            um.CreateUser(user);

            _taskListManager.ShareTaskList(tl.Id, user.Id);

            var tl2 = _taskListManager.GetTaskList(tl.Id);
            Assert.IsTrue(tl2.Users.Contains(user));
        }
    }
}
