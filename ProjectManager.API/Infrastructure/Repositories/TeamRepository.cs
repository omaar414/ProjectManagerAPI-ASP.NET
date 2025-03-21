using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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

        public async Task AddASync(Team team)
        {
           await _context.Teams.AddAsync(team);
        }

        public void Delete(Team team)
        {
            _context.Teams.Remove(team);
        }

        public async Task<Team?> GetByIdAsync(int teamId)
        {
            return await _context.Teams
            .Include(t => t.TeamUsers)
            .ThenInclude(tu => tu.User)
            .FirstOrDefaultAsync(t => t.Id == teamId);
        }

        public void Update(Team team)
        {
            _context.Teams.Update(team);
        }
    }
}