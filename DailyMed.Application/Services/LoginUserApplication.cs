using DailyMed.Core.Applications;
using DailyMed.Core.Interfaces;
using DailyMed.Core.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyMed.Application.Services
{
    internal class LoginUserApplication : ILoginUserApplication
    {
        private readonly IUserRepository _userRepository;

        public LoginUserApplication(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public Task<int> CreateUserAsync(LoginUser user)
        {
            return _userRepository.CreateUserAsync(user);
        }

        public Task<LoginUser> GetByIdAsync(int id)
        {
            return _userRepository.GetByIdAsync(id);
        }

        public Task<LoginUser> GetByUsernameAsync(string username)
        {
            return _userRepository.GetByUsernameAsync(username);
        }
    }
}
