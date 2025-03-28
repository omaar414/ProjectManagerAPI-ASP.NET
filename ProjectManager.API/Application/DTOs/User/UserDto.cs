using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManager.API.Application.DTOs.User
{
    public class UserDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;

        public UserDto(int id, string firstName, string lastName, string username)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Username = username;
        }
    }
}