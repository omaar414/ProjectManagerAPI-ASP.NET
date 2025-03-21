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

        public async Task<TeamDto?> CreateTeamAsync(int ownerId, CreateTeamDto teamDto)
        {
            if (ownerId == -1) {
                throw new Exception("User not authenticated");
            }
            
            var team = new Team (teamDto.Name, ownerId);
            var newTeam = await _teamRepository.AddTeamAsync(team);

            var teamWithOwner = await _teamRepository.GetByIdAsync(newTeam.Id);
            if (teamWithOwner is null) return null;

            
            
            return new TeamDto(teamWithOwner.Id, teamWithOwner.Name, teamWithOwner.OwnerId, teamWithOwner.Owner.FirstName, teamWithOwner.Owner.LastName);
        }

        public async Task<bool> DeleteTeamAsync(int userId, int teamId)
        {
            var team = await _teamRepository.GetByIdAsync(teamId);
            if (team is null) return false;

            if (team.OwnerId != userId) {
                throw new Exception("You are not the owner of this team");
            }

            await _teamRepository.DeleteTeamAsync(team);
            
            return true;

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
            return new TeamDto(team.Id, team.Name, team.OwnerId, team.Owner.FirstName, team.Owner.LastName);
        }

        public async Task<TeamDto?> UpdateTeamAsync(int userId, int teamId, UpdateTeamDto teamDto)
        {
            var team = await _teamRepository.GetByIdAsync(teamId);
            if (team is null) { return null; }

            if (team.OwnerId != userId) {
                throw new Exception("You are not the owner of this team");
            }

            team.Name = teamDto.Name;
            var success = await _teamRepository.UpdateTeamAsync(team);
            if (!success) { return null;}


            return new TeamDto(team.Id, team.Name, team.OwnerId, team.Owner.FirstName, team.Owner.LastName);
        }
    }
}