using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PromptCloudNotes.Interfaces;
using PromptCloudNotes.Model;

namespace BusinessLayer.Managers
{
    public class UserManager : IUserManager
    {
        IUserRepository _repository;

        public UserManager(IUserRepository repo)
        {
            _repository = repo;
        }

        #region IUserManager Members

        public User GetUser(string name)
        {
            return _repository.Get(name);
        }

        public User CreateUser(User userData)
        {
            // validate user, example, check if the username is used, or the email address

            return _repository.Create(userData);
        }

        #endregion
    }
}
