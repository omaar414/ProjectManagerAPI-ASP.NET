using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Http.HttpResults;
using ProjectManager.API.Application.DTOs.Team;
using ProjectManager.API.Application.Interfaces;
using ProjectManager.API.Domain.Entities;

namespace ProjectManager.API.Application.Services
{
    public class TeamService : ITeamService
    {
        private readonly ITeamRepository _teamRepository;
        public TeamService(ITeamRepository teamRepository)
        {
            _teamRepository = teamRepository;
        }

        public async Task<TeamDto> CreateTeamAsync(int ownerId, CreateTeamDto teamDto)
        {
            if (ownerId == -1) {
                throw new Exception("User not authenticated");
            }
            
            var team = new Team (teamDto.Name, ownerId);
            var newTeam = await _teamRepository.AddTeamAsync(team);
            
            return new TeamDto(newTeam.Id, newTeam.Name, newTeam.OwnerId);
        }

        public async Task<List<Team>> GetMyTeamsAsync(int userId)
        {
            var teams = await _teamRepository.GetUserTeamsAsync(userId);
            return teams;
        }

        public async Task<TeamDto?> GetTeamByIdAsync(int userId, int teamId)
        {
            if (userId == -1) {
                throw new Exception("User not authenticated");
            }
            var team = await _teamRepository.GetByIdAsync(teamId);
            if (team is null) { return null; }
            return new TeamDto(team.Id, team.Name, team.OwnerId);
        }
    }
}