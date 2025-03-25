using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProjectManager.API.Application.DTOs.Project;
using ProjectManager.API.Domain.Entities;

namespace ProjectManager.API.Application.Interfaces
{
    public interface IProjectRepository
    {
        Task<bool> AddAsync(Project project);
        Task<List<ProjectDto>> GetAllTeamProjectsAsync(int teamId);
    }
}