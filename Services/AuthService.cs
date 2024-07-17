using Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        public AuthService(IAuthRepository authRepository) 
        { 
            _authRepository = authRepository;
        }
        public Task<string> AuthenticateUserAsync(User user)
        {
            var token = _authRepository.AuthenticateUserAsync(user);
            return token;
        }
        public Task<string> AddUserAsync(User user)
        {
            return _authRepository.AddUserAsync(user);
        }
    }
}
