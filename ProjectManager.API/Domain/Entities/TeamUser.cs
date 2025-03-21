using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManager.API.Domain.Entities
{
    public class TeamUser
    {
        public int Id { get; set; }
        public int TeamId { get; set; }
        public Team Team { get; set; } = null!;
        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public DateTime JoinedDate { get; set; } = DateTime.UtcNow;
        public string Role { get; set; } = "Member";
    }
}