using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProjectManager.API.Application.DTOs.Team;
using ProjectManager.API.Domain.Entities;

namespace ProjectManager.API.Application.Interfaces
{
    public interface ITeamService
    {
        Task<List<Team>> GetMyTeamsAsync(int userId);
        Task<TeamDto?> CreateTeamAsync(int ownerId, CreateTeamDto teamDto);
        Task<TeamDto?> GetTeamByIdAsync(int userId,int teamId);
        

    }
}