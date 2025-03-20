using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProjectManager.API.Domain.Entities;

namespace ProjectManager.API.Application.Interfaces
{
    public interface IUserRepository 
    {
        Task<User?> GetByUsernameAsync(string username);
        Task AddUserAsync(User user);
        Task<bool> SaveChangesAsync();
        Task<List<User>> GetAllUsersAsync();
        
    }
}