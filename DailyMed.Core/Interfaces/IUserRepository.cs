﻿using DailyMed.Core.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyMed.Core.Interfaces
{
    public interface IUserRepository
    {
        Task<int> CreateUserAsync(LoginUser user);
        Task<LoginUser> GetByUsernameAsync(string username);
        Task<LoginUser> GetByIdAsync(int id);
    }
}
