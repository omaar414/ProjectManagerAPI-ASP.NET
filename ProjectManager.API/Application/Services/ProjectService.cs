using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;
using ProjectManager.API.Application.DTOs.Project;
using ProjectManager.API.Application.Interfaces;
using ProjectManager.API.Domain.Entities;

namespace ProjectManager.API.Application.Services
{
    public class ProjectService : IProjectService
    {
        private readonly ITeamRepository _teamRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IUserRepository _userRepository;
        public ProjectService(ITeamRepository teamRepository, IProjectRepository projectRepository, IUserRepository userRepository)
        {
            _teamRepository = teamRepository;
            _projectRepository = projectRepository;
            _userRepository = userRepository;
        }
        public async Task CreateProjectAsync(int requesterId, int teamId, CreateProjectDto projectDto)
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
            
        }

        public async Task<List<ProjectDto>> GetTeamProjectsAsync(int userId, int teamId)
        {
            var team = await _teamRepository.GetByIdAsync(teamId);
            if (team is null) {
                throw new Exception("Team not found");
            }

            var user = await _userRepository.GetByIdAsync(userId);
            if (user is null) {
                throw new Exception("User not found");
            }

            var isUserPartOfTheTeam = await _teamRepository.VerifyIfMemberIsPartOfTheTeamAsync(teamId, userId);
            if (!isUserPartOfTheTeam) {
                throw new Exception("You are not member of this team");
            }

            var teamProjectsDTo = await _projectRepository.GetAllTeamProjectsAsync(teamId);

            return teamProjectsDTo;

        }

        public async Task<ProjectDto> UpdateProjectAsync(int requesterId, int projectId, UpdateProjectDto projectDto)
        {
            var project = await _projectRepository.GetByIdAsync(projectId);
            if (project is null) {
                throw new Exception("Project not found");
            }

            if (project.Team.OwnerId != requesterId){
                throw new Exception("Only the owner of the team can update a project");
            }

            project.Name = projectDto.Name;
            project.Description = projectDto.Description;

            var success = await _projectRepository.UpdateProjectAsync(project);
            if(!success) {
                throw new Exception("Failed to updated the project");
            }

            return new ProjectDto (project.Id, project.Name, project.Description);

        }

        public async Task<bool> DeleteProjectAsync(int requesterId, int projectId)
        {
            var project = await _projectRepository.GetByIdAsync(projectId);
            if (project is null) {
                throw new Exception("Project not found");
            }
            if (project.Team.OwnerId != requesterId) {
                throw new Exception("Only the owner of the team can delete a project");
            }

            return await _projectRepository.DeleteProjectAsync(project);
        }

        public async Task<ProjectDto?> GetProjectAsync(int requesterId ,int projectId)
        {
            var project = await _projectRepository.GetByIdAsync(projectId);
            if (project is null) {
                return null;
            }

            var isUserPartOfTheTeam = await _teamRepository.VerifyIfMemberIsPartOfTheTeamAsync(project.TeamId, requesterId);
            if (!isUserPartOfTheTeam) {
                throw new Exception("You are not member of this team");
            }

            var projectDto = new ProjectDto (project.Id, project.Name, project.Description);

            return projectDto;
        }

    }
}