using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManager.API.Application.DTOs.Project
{
    public class ProjectDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public ProjectDto(int id, string name, string description)
        {
            Id = id;
            Name = name;
            Description = description;
        }
    }
}