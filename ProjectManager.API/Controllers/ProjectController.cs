using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectManager.API.Application.DTOs.Project;
using ProjectManager.API.Application.Interfaces;

namespace ProjectManager.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService _projectService;
        public ProjectController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        [HttpPost("create/{teamId}")]
        public async Task<IActionResult> CreateTeam([FromRoute] int teamId, [FromBody] CreateProjectDto projectDto)
        {
            var requesterId = GetUserIdFromToken();
            if (requesterId == -1) { return Unauthorized(new {message = "User not authenticated"}); }

            try {
                await _projectService.CreateProjectAsync(requesterId, teamId, projectDto);
                return Ok(new {message = "Project created successfully "});

            } catch (Exception ex)
            {
                return BadRequest(new {message = ex.Message});
            }
        }

        [HttpGet("my-projects/{teamId}")]
        public async Task<IActionResult> GetTeamProjects([FromRoute] int teamId)
        {
            var requesterId = GetUserIdFromToken();
            if (requesterId == -1) { return Unauthorized(new {message = "User not authenticated"}); }

            try {
                var teamProjects = await _projectService.GetTeamProjectsAsync(requesterId, teamId);
                return Ok(teamProjects);

            } catch (Exception ex)
            {
                return BadRequest(new {message = ex.Message});
            }
        }
        
        [HttpPut("update/{projectId}")]
        public async Task<IActionResult> UpdateProject([FromRoute] int projectId, UpdateProjectDto projectDto)
        {
            var requesterId = GetUserIdFromToken();
            if (requesterId == -1) { return Unauthorized(new {message = "User not authenticated"}); }

            try {
                var project = await _projectService.UpdateProjectAsync(requesterId, projectId, projectDto);
                return Ok(project);
            } catch (Exception ex) {
                return BadRequest(new {message = ex.Message});
            }

        }

        [HttpDelete("delete/{projectId}")]
        public async Task<IActionResult> DeleteProject([FromRoute]int projectId)
        {
            var requesterId = GetUserIdFromToken();
            if (requesterId == -1) { return Unauthorized(new {message = "User not authenticated"}); }

            try {
                var success = await _projectService.DeleteProjectAsync(requesterId,projectId);
                if (!success) {
                    return BadRequest(new { message = "Failed to delete the project"});
                }

                return Ok(new {message = "Project deleted successfully"});
            } catch (Exception ex)
            {
                return BadRequest(new {message = ex.Message});
            }
            
        }

        private int GetUserIdFromToken(){

            var IdFromToken = User.FindFirst("userId");
            if (IdFromToken == null) return -1;

            if (!int.TryParse(IdFromToken.Value, out int userId))
            {
                return -1;
            }
            return userId;
        }

    }
}