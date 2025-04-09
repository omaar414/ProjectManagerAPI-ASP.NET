using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManager.API.Application.DTOs.Task
{
    public class UpdateTaskDto
    {
        public string? Title { get; set; } = null;
        public string? Description { get; set; } = null;

        public UpdateTaskDto(string? title, string? description)
        {
            Title = title;
            Description = description;
        }
    }

    
}