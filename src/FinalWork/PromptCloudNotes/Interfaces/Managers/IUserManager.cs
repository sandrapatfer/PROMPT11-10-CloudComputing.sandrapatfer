using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PromptCloudNotes.Model;

namespace PromptCloudNotes.Interfaces.Managers
{
    public interface IUserManager
    {
        IEnumerable<User> GetAllUsers();

        User GetUserByClaims(string provider, string nameIdentifier);
        
        void CreateUser(User userData);
    }
}
