using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectManager.API.Application.DTOs;
using ProjectManager.API.Application.Interfaces;
using ProjectManager.API.Application.Services;
using ProjectManager.API.Domain.Entities;

namespace ProjectManager.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
         private readonly IUserService _userService;
         public UserController(IUserService userService)
         {
            _userService = userService;
         }

         [HttpPost("register")]
         public async Task<IActionResult> Register([FromBody] RegisterUserDto dto)
         {
            //Validate the DTO
            if (!ModelState.IsValid)
            {                    
               var errors = ModelState.Values
                  .SelectMany(v => v.Errors)
                  .Select(e => e.ErrorMessage)
                  .ToList();

               return BadRequest(new { message = "Validation failed", errors });
            }

            // We try to regsiter the user
            var succes = await _userService.RegisterUserAsync(dto);

            // If false user already exist
            if (!succes) { return BadRequest(new {message = "Username already exist"});}

            return Ok("User created successfully");
         }

         [HttpPost("login")]
         public async Task<IActionResult> Login([FromBody] LoginUserDto dto)
         {
            //Validate the DTO
            if (!ModelState.IsValid)
            {                    
               var errors = ModelState.Values
                  .SelectMany(v => v.Errors)
                  .Select(e => e.ErrorMessage)
                  .ToList();

               return BadRequest(new { message = "Validation failed", errors });
            }

            //We try to Login the User and Generate the Token
            var token = await _userService.LogingUserAsync(dto);

            //If Token is null we return Unauthorized
            if (token == null) { return Unauthorized("Invalid credentials.");}

            //Else 
            return Ok(new {token});
         }

         [HttpGet("message")]
         [Authorize]
         public IActionResult Message()
         {
           return Ok(new{message = "Entraste con el Token"});
         }
    }
}