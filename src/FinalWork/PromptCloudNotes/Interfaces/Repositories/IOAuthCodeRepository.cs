using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PromptCloudNotes.Interfaces.Repositories;
using PromptCloudNotes.Model;

namespace PromptCloudNotes.Interfaces.Repositories
{
    public interface IOAuthCodeRepository : IRepository<OAuthCode>
    {
    }
}
