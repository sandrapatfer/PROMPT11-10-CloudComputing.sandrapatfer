using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PromptCloudNotes.Interfaces.Repositories;
using PromptCloudNotes.Model;

namespace PromptCloudNotes.InMemoryRepo
{
    public class UserRepository : IUserRepository
    {
        private List<User> _users = new List<User>();
        private int _userId = 0;

        public IEnumerable<User> GetAll()
        {
            return _users;
        }

        public IEnumerable<User> GetAll(string provider)
        {
            return _users;
        }

        public void Create(User user)
        {
            _users.Add(user);
            user.UniqueId = (++_userId).ToString();
        }

        public User Get(string provider, string nameIdentifier)
        {
            return _users.FirstOrDefault(u => u.Provider == provider && u.NameIdentifier == nameIdentifier);
        }

        public void Update(string provider, string userId, User userData)
        { }

        public void Delete(string provider, string userId)
        {
            var user = Get(provider, userId);
            if (user != null)
            {
                _users.Remove(user);
            }
        }

    }
}
