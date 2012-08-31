using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PromptCloudNotes.Interfaces.Managers;
using PromptCloudNotes.Interfaces.Repositories;
using PromptCloudNotes.Model;

namespace PromptCloudNotes.BusinessLayer.Managers
{
    public class UserManager : IUserManager
    {
        IUserRepository _repository;

        public UserManager(IUserRepository repo)
        {
            _repository = repo;
        }

        #region IUserManager Members

        public IEnumerable<User> GetAllUsers()
        {
            return _repository.GetAll();
        }

        public User GetUserByClaims(string provider, string nameIdentifier)
        {
            return _repository.Get(provider, nameIdentifier);
        }

        public void CreateUser(User userData)
        {
            // validate user, example, check if the username is used, or the email address

            _repository.Create(userData);
        }

        #endregion
    }
}
