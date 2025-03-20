using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManager.API.Application.DTOs
{
    public class RegisterUserDto
    {
        [Required(ErrorMessage = "First Name required")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last Name required")]
        public string LastName { get; set;} = string.Empty;

        [Required(ErrorMessage = "Username required")]
        public string Username { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Password required")]
        [MinLength(6, ErrorMessage = "Password need to have at least 6 characters")]
        public string Password { get; set; } = string.Empty;
    }
}