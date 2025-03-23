using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProjectManager.API.Application.DTOs.Team;
using ProjectManager.API.Application.DTOs.User;
using ProjectManager.API.Domain.Entities;

namespace ProjectManager.API.Application.Interfaces
{
    public interface ITeamRepository
    {
        Task<Team?> GetByIdAsync(int teamId);
        Task<List<TeamDto>> GetUserTeamsAsync(int userId);
        Task<Team> AddTeamAsync(Team team);
        Task<bool> UpdateTeamAsync(Team team);
        Task DeleteTeamAsync(Team team);
        Task<bool> AddMemberToTeamAsync(int teamId, int userId);
        Task<List<UserDto>> GetMembersOfTeamAsync(int userId, int teamId);
        Task<bool> AddTeamUser(TeamUser teamUser);
        void Update(Team team);
        void Delete(Team team);

    }
}