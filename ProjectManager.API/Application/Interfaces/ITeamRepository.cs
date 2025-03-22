using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProjectManager.API.Domain.Entities;

namespace ProjectManager.API.Application.Interfaces
{
    public interface ITeamRepository
    {
        Task<Team?> GetByIdAsync(int teamId);
        Task<List<Team>> GetUserTeamsAsync(int userId);
        Task<Team> AddTeamAsync(Team team);
        Task<bool> UpdateTeamAsync(Team team);
        Task DeleteTeamAsync(Team team);
        void Update(Team team);
        void Delete(Team team);

    }
}