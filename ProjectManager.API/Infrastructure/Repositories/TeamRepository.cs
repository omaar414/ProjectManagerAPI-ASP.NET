using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProjectManager.API.Application.DTOs.Team;
using ProjectManager.API.Application.Interfaces;
using ProjectManager.API.Domain.Entities;
using ProjectManager.API.Infrastructure.Data;

namespace ProjectManager.API.Infrastructure.Repositories
{
    public class TeamRepository : ITeamRepository
    {
        private readonly ApplicationDbContext _context;
        public TeamRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddMemberToTeamAsync(int teamId, int userId)
        {
            var teamUserExist = await _context.TeamUsers
            .FirstOrDefaultAsync( tu => tu.TeamId == teamId && tu.UserId == userId);

            if (teamUserExist != null) {
                throw new Exception("User is already a member of the team");
            }
            
            var newTeamUser = new TeamUser (teamId, userId);

            await _context.TeamUsers.AddAsync(newTeamUser);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<Team> AddTeamAsync(Team team)
        {
            await _context.Teams.AddAsync(team);
            var result = await _context.SaveChangesAsync();
            
            if (result == 0) {
                throw new Exception("Failed to save the team");
            }

            return team;
        }

        public void Delete(Team team)
        {
            _context.Teams.Remove(team);
        }

        public async Task DeleteTeamAsync(Team team)
        {
            _context.Teams.Remove(team);
            var result = await _context.SaveChangesAsync();
            if (result == 0) {
                throw new Exception("Failed to delete Team `" + team.Name + "`");
            }

        }

        public async Task<Team?> GetByIdAsync(int teamId)
        {

             return await _context.Teams
            .Include(t => t.Owner)
            .Include(t => t.TeamUsers)
            .ThenInclude(tu => tu.User)
            .FirstOrDefaultAsync(t => t.Id == teamId);

            
        }

        public async Task<List<TeamDto>> GetUserTeamsAsync(int userId)
        {
            return await _context.TeamUsers
            .Where(tu => tu.UserId == userId)
            .Include(tu => tu.Team)
            .ThenInclude(t => t.Owner)
            .Select(tu => new TeamDto(
                tu.Team.Id,
                tu.Team.Name,
                tu.Team.OwnerId,
                tu.Team.Owner.FirstName,
                tu.Team.Owner.LastName
            ))
            .ToListAsync();
        }

        public void Update(Team team)
        {
            _context.Teams.Update(team);
        }

        public async Task<bool> UpdateTeamAsync(Team team)
        {
            _context.Teams.Update(team);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}