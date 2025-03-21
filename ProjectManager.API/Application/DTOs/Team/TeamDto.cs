using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProjectManager.API.Domain.Entities;

namespace ProjectManager.API.Application.DTOs.Team
{
    public class TeamDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int OwnerId { get; set; }
        

        public TeamDto(int id, string name, int ownerId)
    {
        Id = id;
        Name = name;
        OwnerId = ownerId;

    }

    }

    
    
}