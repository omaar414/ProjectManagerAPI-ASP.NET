using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManager.API.Application.DTOs.Project
{
    public class ProjectDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public ProjectDto(string name, string description)
        {
            Name = name;
            Description = description;
        }
    }
}