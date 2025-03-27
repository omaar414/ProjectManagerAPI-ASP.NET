using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProjectManager.API.Application.DTOs.Project;

namespace ProjectManager.API.Application.Interfaces
{
    public interface IProjectService
    {
        Task<ProjectDto> CreateProjectAsync(int requesterId, int teamId, CreateProjectDto projectDto);
        Task<List<ProjectDto>> GetTeamProjectsAsync(int userId, int teamId);
        Task<ProjectDto> UpdateProjectAsync(int requesterId, int projectId, UpdateProjectDto projectDto);
    }
}