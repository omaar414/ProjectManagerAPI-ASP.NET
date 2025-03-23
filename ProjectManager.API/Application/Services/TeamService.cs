using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using ProjectManager.API.Application.DTOs.Team;
using ProjectManager.API.Application.DTOs.User;
using ProjectManager.API.Application.Interfaces;
using ProjectManager.API.Domain.Entities;

namespace ProjectManager.API.Application.Services
{
    public class TeamService : ITeamService
    {
        private readonly ITeamRepository _teamRepository;
        private readonly IUserRepository _userRepository;
        public TeamService(ITeamRepository teamRepository, IUserRepository userRepository)
        {
            _teamRepository = teamRepository;
            _userRepository = userRepository;
        }

        public async Task<bool> AddMemberToTeamAsync(int userId, int teamId, int userToAddId)
        {
            var teamExist = await _teamRepository.GetByIdAsync(teamId);
            if (teamExist == null) {
                 throw new Exception("Team not found");
            }

            if (teamExist.OwnerId != userId) {
                throw new Exception("Only the owner can add member to this team");
            }
        
            var userToAddExist = await _userRepository.GetByIdAsync(userToAddId);
            if (userToAddExist == null) {
                 throw new Exception("User to add not found");
            }
            
            var success = await _teamRepository.AddMemberToTeamAsync(teamId, userToAddId);
            if (!success) {
                throw new Exception("Failed to add the member");
            }

            return true;
        }

        public async Task<TeamDto?> CreateTeamAsync(int ownerId, CreateTeamDto teamDto)
        {
            if (ownerId == -1) {
                throw new Exception("User not authenticated");
            }
            
            var team = new Team (teamDto.Name, ownerId);
            var newTeam = await _teamRepository.AddTeamAsync(team);

            var newTeamUser = new TeamUser (newTeam.Id, ownerId, "Owner");

            var newTeamUserAdded = await _teamRepository.AddTeamUserAsync(newTeamUser);
            if(!newTeamUserAdded) { 
                throw new Exception("TeamUser failed to add");
            }

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

        public async Task<List<UserDto>> GetMembersOfTeamAsync(int userId, int teamId)
        {
            return await _teamRepository.GetMembersOfTeamAsync(userId, teamId);
        }

        public async Task<List<TeamDto>> GetMyTeamsAsync(int userId)
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

        public async Task<bool> RemoveMemberFromTeamAsync(int userId, int teamId, int memberId)
        {
            var team = await _teamRepository.GetByIdAsync(teamId);
            if (team == null) {
                throw new Exception("Team not found");
            }

            if (team.OwnerId != userId) {
                throw new Exception("Only the owner of the team can delete a member");
            }

            if (userId == memberId) {
                throw new Exception("You can't delete your self");
            }
            var userToRemove = await _userRepository.GetByIdAsync(memberId);
            if (userToRemove is null) {
                throw new Exception("User to delete not found");
            }

            var isMemberPartOfTheTeam = await _teamRepository.VerifyIfMemberIsPartOfTheTeamAsync(teamId, memberId);
            if (!isMemberPartOfTheTeam) {
                throw new Exception("User to delete is not member of the team");
            }

            var removed = await _teamRepository.RemoveMemberOfATeamAsync(teamId, memberId);
            
            return removed;
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