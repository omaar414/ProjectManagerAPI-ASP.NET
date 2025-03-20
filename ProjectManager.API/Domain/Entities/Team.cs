using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManager.API.Domain.Entities
{
    public class Team
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int OwnerId { get; set; }
        public User Owner { get; set; } = null!;

        public ICollection<TeamUser> Members { get; set; } = new List<TeamUser>();
        public ICollection<Project> Projects { get; set; } = new List<Project>();

        public Team(){}
        public Team(string name, int ownerId)
        {
            Name = name;
            OwnerId = ownerId;
        }
        
    }
}