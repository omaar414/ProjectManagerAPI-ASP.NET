using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using ProjectManager.API.Application.DTOs;
using ProjectManager.API.Application.Helpers;
using ProjectManager.API.Application.Interfaces;
using ProjectManager.API.Domain.Entities;
using ProjectManager.API.Infrastructure.Repositories;


namespace ProjectManager.API.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        public UserService(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }


        public async Task<bool> RegisterUserAsync(RegisterUserDto dto)
        {
            //very if the User exist
            var existingUser = await _userRepository.GetByUsernameAsync(dto.Username);
            

            if (existingUser != null) { return false; }

            
            //Encrypt password
            string HashPassword = PasswordHasher.HashPassword(dto.Password);

            //Create new User entity
            var newUser = new User ( dto.FirstName, dto.LastName, dto.Username, HashPassword);

            //Add User to the db
            await _userRepository.AddUserAsync(newUser);

            
            //Return true if was done false if not
            return await _userRepository.SaveChangesAsync();
                
        }

        public async Task<string?> LogingUserAsync(LoginUserDto dto)
        {   
            //search for the User in the db
            var user = await _userRepository.GetByUsernameAsync(dto.Username);

            //If user not exist we return null
            if (user == null) {return null;} 

            //Validate the password
            if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash)) {return null;}

            return GenerateJwtToken(user);
        }


        private string GenerateJwtToken(User user)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Secret"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Username),
                new Claim("userId", user.Id.ToString())
            };

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(jwtSettings["ExpirationInMinutes"])),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        
    }
}
        
    
