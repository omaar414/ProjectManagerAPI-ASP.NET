using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManager.API.Application.DTOs.Task
{
    public class TaskDto
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        


        public TaskDto(string title, string description)
        {
            Title = title;
            Description = description;
            
        }
    }
}