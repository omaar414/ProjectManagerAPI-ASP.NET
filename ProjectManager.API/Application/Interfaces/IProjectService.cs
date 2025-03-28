using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProjectManager.API.Application.DTOs.Project;

namespace ProjectManager.API.Application.Interfaces
{
    public interface IProjectService
    {
        Task CreateProjectAsync(int requesterId, int teamId, CreateProjectDto projectDto);
        Task<List<ProjectDto>> GetTeamProjectsAsync(int userId, int teamId);
        Task<ProjectDto> UpdateProjectAsync(int requesterId, int projectId, UpdateProjectDto projectDto);
        Task<bool> DeleteProjectAsync(int requesterId, int projectId);
        Task<ProjectDto?> GetProjectAsync(int requesterId,int projectId);
    }
}