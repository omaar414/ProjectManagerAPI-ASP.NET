using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Build.Framework;
using ProjectManager.API.Application.DTOs.Task;
using ProjectManager.API.Application.Interfaces;
using ProjectManager.API.Domain.Entities;

namespace ProjectManager.API.Application.Services
{
    public class TaskService : ITaskService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly ITeamRepository _teamRepository;
        private readonly ITaskRepository _taskRepository;
        public TaskService(IProjectRepository projectRepository, ITaskRepository taskRepository, ITeamRepository teamRepository)
        {
            _projectRepository = projectRepository;
            _taskRepository = taskRepository;
            _teamRepository = teamRepository;
        }
        public async Task<bool> CreateTaskAsync(int requesterId, int projectId, CreateTaskDto taskDto)
        {
           var project = await _projectRepository.GetByIdAsync(projectId);
           if (project is null) {
            throw new Exception("Project not found.");
           }

           if ( project.Team.OwnerId != requesterId) {
            throw new Exception("You are not the owner of this team, you can't create a task");
           }
           
           var newTask = new TaskItem(taskDto.Title, taskDto.Description, taskDto.DueDate, projectId);

            var success = await _taskRepository.AddAsync(newTask);
            return success;
        }

        public async Task<List<TaskDto>> GetTasksAsync(int requesterId, int projectId)
        {
            var project = await _projectRepository.GetByIdAsync(projectId);
            if (project is null) {
            throw new Exception("Project not found.");
           }

           var isMemberPartOfTheTeam = await _teamRepository.VerifyIfMemberIsPartOfTheTeamAsync(project.TeamId, requesterId);
           if (!isMemberPartOfTheTeam) {
            throw new Exception("You are not member of this team");
           }

           var tasksDto = await _taskRepository.GetTasksAsync(projectId);
           return tasksDto;
        }

        public async Task<TaskDto> UpdateTaskAsync(int requesterId, int taskId, UpdateTaskDto taskDto)
        {
            var task = await _taskRepository.GetByIdAsync(taskId);
            if (task is null){
                throw new Exception("Task not found");
            }

            var isMemberPartOfTheTeam = await _teamRepository.VerifyIfMemberIsPartOfTheTeamAsync(task.Project.TeamId, requesterId);
           if (!isMemberPartOfTheTeam) {
            throw new Exception("You are not member of this team");
           }

            if (task.Project.Team.OwnerId != requesterId) {
                throw new Exception("Only the owner can Update a Task");
            }

            task.Title = !string.IsNullOrWhiteSpace(taskDto.Title) ? taskDto.Title : task.Title;
            task.Description = !string.IsNullOrWhiteSpace(taskDto.Description) ? taskDto.Description : task.Description;

            var success = await _taskRepository.UpdateAsync(task);
            if (!success) {
                throw new Exception("Error updating task");
            }

            return new TaskDto (task.Title, task.Description);

        }

        
    }
}