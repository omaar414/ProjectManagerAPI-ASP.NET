using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProjectManager.API.Application.DTOs.Project;
using ProjectManager.API.Application.Interfaces;
using ProjectManager.API.Domain.Entities;

namespace ProjectManager.API.Application.Services
{
    public class ProjectService : IProjectService
    {
        private readonly ITeamRepository _teamRepository;
        private readonly IProjectRepository _projectRepository;
        public ProjectService(ITeamRepository teamRepository, IProjectRepository projectRepository)
        {
            _teamRepository = teamRepository;
            _projectRepository = projectRepository;
        }
        public async Task<ProjectDto> CreateProjectAsync(int requesterId, int teamId, CreateProjectDto projectDto)
        {
            var teamToAddProject = await _teamRepository.GetByIdAsync(teamId);
            if (teamToAddProject == null) {
                throw new Exception("Team not found");
            }

            if (teamToAddProject.OwnerId != requesterId) {
                throw new Exception("You are not the owner of this team, you can't create a project");
            }

            var newProject = new Project (projectDto.Name, projectDto.Description, teamId);

            var success = await _projectRepository.AddAsync(newProject);
            if(!success) {
                throw new Exception("Failed adding the project");
            }

            var newProjectDto = new ProjectDto(projectDto.Name, projectDto.Description);
            
            return newProjectDto;
            
        }
    }
}