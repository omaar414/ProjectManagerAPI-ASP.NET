using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManager.API.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string Username { get; set; } = string.Empty;

        public string PasswordHash { get; set; } = string.Empty;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public ICollection<TeamUser> TeamUsers { get; set; } = new List<TeamUser>();
       
        

        public User() {}

        public User(string firstName, string lastName, string username, string passwordHash)
        {
            
            FirstName = firstName;
            LastName = lastName;
            Username = username;
            PasswordHash = passwordHash;
        }

    }
}