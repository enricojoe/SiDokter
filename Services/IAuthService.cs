using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface IAuthService
    {
        public Task<string> AuthenticateUserAsync(User user);
        public Task<string> AddUserAsync(User user);
    }
}
