using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiGame.Models;

namespace WebApiGame.Services
{
    public interface IAuthenticationRepo
    {
        public Task<ServiceResponse<string>> Login(string userName, string password);
        public Task<ServiceResponse<int>> Register(User user, string password);
        public Task<bool> UserExists(string userName);
    
    }
}
