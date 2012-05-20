using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PromptCloudNotes.Model;

namespace PromptCloudNotes.Interfaces
{
    public interface IUserRepository
    {
        IEnumerable<User> GetAll();

        User Create(User user);
        User Get(int userId);
    }
}
