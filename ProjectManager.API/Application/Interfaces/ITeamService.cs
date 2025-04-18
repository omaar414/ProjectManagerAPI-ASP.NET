using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProjectManager.API.Application.DTOs.Team;
using ProjectManager.API.Application.DTOs.User;
using ProjectManager.API.Domain.Entities;

namespace ProjectManager.API.Application.Interfaces
{
    public interface ITeamService
    {
        Task<List<TeamDto>> GetMyTeamsAsync(int userId);
        Task<TeamDto?> CreateTeamAsync(int ownerId, CreateTeamDto teamDto);
        Task<TeamDto?> GetTeamByIdAsync(int userId, int teamId);
        Task<TeamDto?> UpdateTeamAsync(int userId, int teamId, UpdateTeamDto teamDto);
        Task<bool> DeleteTeamAsync(int userId, int teamId);
        Task<bool> AddMemberToTeamAsync(int userId, int teamId, int userToAddId);
        Task<List<UserDto>> GetMembersOfTeamAsync(int userId, int teamId);
        Task<bool> RemoveMemberFromTeamAsync(int userId, int teamId, int memberId);
        Task<UserDto?> GetMemberInfoAsync(int requesterId, int teamId, int userId);
        

    }
}