using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PromptCloudNotes.Model;

namespace PromptCloudNotes.Interfaces
{
    public interface IUserManager
    {
        IEnumerable<User> GetAllUsers();
        User GetUser(string name);
        User CreateUser(User userData);
    }
}
