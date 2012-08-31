using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PromptCloudNotes.Model;
using PromptCloudNotes.Interfaces;
using StructureMap;
using Exceptions;

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
            var tl2 = _taskListManager.CreateTaskList(_user, tl);
            Assert.IsNotNull(tl2.Id);
        }

        TaskList CreateTaskList()
        {
            TaskList tl = new TaskList() { Name = "new list" };
            return _taskListManager.CreateTaskList(_user, tl);
        }

        [TestMethod]
        public void GetListTest()
        {
            var tl = CreateTaskList();
            var tl2 = _taskListManager.GetTaskList(_user.UniqueId, tl.Id);
            Assert.IsNotNull(tl2);
            Assert.AreEqual(tl.Id, tl2.Id);
        }

        [TestMethod]
        public void UpdateListTest()
        {
            var tl = CreateTaskList();
            tl.Name = "New name";
            _taskListManager.UpdateTaskList(_user.UniqueId, tl.Id, tl);
            var tl2 = _taskListManager.GetTaskList(_user.UniqueId, tl.Id);
            Assert.AreEqual(tl.Name, tl2.Name);
        }

        [TestMethod]
        [ExpectedException(typeof(ObjectNotFoundException))]
        public void DeleteListTest()
        {
            var tl = CreateTaskList();
            _taskListManager.DeleteTaskList(_user.UniqueId, tl.Id);
            var tl2 = _taskListManager.GetTaskList(_user.UniqueId, tl.Id);
        }

        [TestMethod]
        public void ShareListTest()
        {
            var tl = CreateTaskList();

            var user = new User();
            IUserManager um = ObjectFactory.GetInstance<IUserManager>();
            um.CreateUser(user);

            _taskListManager.ShareTaskList(_user.UniqueId, tl.Id, user.UniqueId);

            var tl2 = _taskListManager.GetTaskList(_user.UniqueId, tl.Id);
            Assert.IsTrue(tl2.Users.Contains(user));

            var tl3 = _taskListManager.GetTaskList(user.UniqueId, tl.Id);
            Assert.IsNotNull(tl3);
        }
    }
}
