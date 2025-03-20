using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManager.API.Domain.Entities
{
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int TeamId { get; set; } 
        public Team Team { get; set; } = null!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<ProjectUser> ProjectUsers { get; set; } = new List<ProjectUser>();
        

        public Project() { }

        public Project(string name, string description, int teamId)
        {
            Name = name;
            Description = description;
            TeamId = teamId;

        }
    }
}