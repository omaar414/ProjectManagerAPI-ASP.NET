using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProjectManager.API.Application.DTOs.Task;
using ProjectManager.API.Application.Interfaces;
using ProjectManager.API.Domain.Entities;
using ProjectManager.API.Infrastructure.Data;

namespace ProjectManager.API.Infrastructure.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly ApplicationDbContext _context;
        public TaskRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<bool> AddAsync(TaskItem task)
        {
            await _context.TaskItems.AddAsync(task);
            return await _context.SaveChangesAsync() > 0;

        }

        public async Task<List<TaskDto>> GetTasksAsync(int projectId)
        {
            return await _context.TaskItems
            .Where(t => t.ProjectId == projectId)
            .Select(t => new TaskDto(
                t.Title, 
                t.Description
            )).ToListAsync();
            
        }
    }
}