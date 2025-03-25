using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProjectManager.API.Application.DTOs.Project;
using ProjectManager.API.Application.Interfaces;
using ProjectManager.API.Domain.Entities;
using ProjectManager.API.Infrastructure.Data;

namespace ProjectManager.API.Infrastructure.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly ApplicationDbContext _context;
        public ProjectRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<bool> AddAsync(Project project)
        {
            await _context.Projects.AddAsync(project);
            return await _context.SaveChangesAsync() > 0;

        }

        public async Task<List<ProjectDto>> GetAllTeamProjectsAsync(int teamId)
        {
            return await _context.Projects
            .Where(p => p.TeamId == teamId)
            .Select(p => new ProjectDto (p.Name, p.Description))
            .ToListAsync();
            
        }
    }
}