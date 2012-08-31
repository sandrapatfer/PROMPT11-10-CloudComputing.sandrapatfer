using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PromptCloudNotes.Interfaces;
using PromptCloudNotes.Model;

namespace PromptCloudNotes.InMemoryRepo
{
    public class UserRepository : IUserRepository
    {
        private List<User> _users = new List<User>();
        private int _userId = 0;

        #region IUserRepository Members

        public IEnumerable<User> GetAll()
        {
            return _users;
        }

        public User Create(User user)
        {
            _users.Add(user);
            user.UniqueId = (++_userId).ToString();
            return user;
        }

        public User GetById(string userId)
        {
            return _users.First(u => u.UniqueId == userId);
        }

        public User GetByClaims(string provider, string nameIdentifier)
        {
            return _users.FirstOrDefault(u => u.Provider == provider && u.NameIdentifier == nameIdentifier);
        }

        #endregion
    }
}
