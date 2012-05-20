using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PromptCloudNotes.Interfaces;
using PromptCloudNotes.Model;

namespace InMemoryRepo
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
            user.Id = ++_userId;
            return user;
        }

        public User Get(int userId)
        {
            return _users.First(u => u.Id == userId);
        }

        #endregion
    }
}
