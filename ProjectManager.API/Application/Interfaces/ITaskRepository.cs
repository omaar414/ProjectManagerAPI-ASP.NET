using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProjectManager.API.Application.DTOs.Task;
using ProjectManager.API.Domain.Entities;

namespace ProjectManager.API.Application.Interfaces
{
    public interface ITaskRepository
    {
        Task<bool> AddAsync(TaskItem task);
        Task<List<TaskDto>> GetTasksAsync(int projectId);
    }
}