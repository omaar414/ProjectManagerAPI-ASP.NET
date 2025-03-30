using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProjectManager.API.Application.DTOs.Task;

namespace ProjectManager.API.Application.Interfaces
{
    public interface ITaskService
    {
        Task<bool> CreateTaskAsync(int requesterId, int projectId, CreateTaskDto taskDto);
        Task<List<TaskDto>> GetTasksAsync(int requesterId, int projectId);

    }
}