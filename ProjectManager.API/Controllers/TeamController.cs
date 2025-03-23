using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectManager.API.Application.DTOs.Team;
using ProjectManager.API.Application.Interfaces;

namespace ProjectManager.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TeamController : ControllerBase
    {
        private readonly ITeamService _teamService;
        public TeamController(ITeamService teamService)
        {
            _teamService = teamService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateTeam([FromBody] CreateTeamDto teamDto)
        {
            var userId = GetUserIdFromToken();
            
            try {
                var newTeamDto = await _teamService.CreateTeamAsync(userId, teamDto); 
                if(newTeamDto is null) return NoContent();
                return Ok(newTeamDto);

            } catch(Exception ex)
            {
                return BadRequest(new {message = ex.Message});
            }

        }

        [HttpPut("update/{teamId}")]
        public async Task<IActionResult> UpdateTeam([FromRoute] int teamId, [FromBody] UpdateTeamDto teamDto)
        {
            var userId = GetUserIdFromToken();
            if (userId is -1) {return Unauthorized(new {message = "User not authenticated"});}

            try {

                var teamUpdated = await _teamService.UpdateTeamAsync(userId, teamId, teamDto);  
                if (teamUpdated is null) return NotFound(new { message = "Team not found"});

                return Ok(teamUpdated);

            } catch (Exception ex)
            {
                return BadRequest(new {message = ex.Message});
            }

        }

        [HttpGet("{teamId}/members")]
        public async Task<IActionResult> GetTeamMembers([FromRoute] int teamId)
        {
            var userId = GetUserIdFromToken();
            if (userId is -1) {return Unauthorized(new {message = "User not authenticated"});}

            try {
                var members = await _teamService.GetMembersOfTeamAsync(userId, teamId);
                return Ok(members);
            } catch (Exception ex)
            {
                return BadRequest(new {message = ex.Message});
            }

        }


        [HttpDelete("delete/{teamId}")]
        public async Task<IActionResult> DeleteTeam([FromRoute] int teamId)
        {
            var userId = GetUserIdFromToken();
            if (userId is -1) {return Unauthorized(new {message = "User not authenticated"});}

            try {

                var teamDeleted = await _teamService.DeleteTeamAsync(userId, teamId);
                if (teamDeleted is false) return NotFound(new {mesage = "Team not found"});

                return Ok(new {message = "Team deleted"});

            } catch (Exception ex)
            {
                return BadRequest(new {message = ex.Message});
            }

        }
        
        [HttpDelete("{teamId}/remove-member/{memberId}")]
        public async Task<IActionResult> RemoveMemberFromTeam([FromRoute] int teamId, [FromRoute]int memberId)
        {
            var userId = GetUserIdFromToken();
            if (userId is -1) {return Unauthorized(new {message = "User not authenticated"});}

            try {
                var success = await _teamService.RemoveMemberFromTeamAsync(userId, teamId, memberId);
                if (success is false) return BadRequest(new {message = "Could not remove member of the team"});

                return Ok(new {message = "Member removed successfully"});
            } catch (Exception ex)
            {
                return BadRequest(new {message = ex.Message});
            }
        }

        [HttpGet("{teamId}")]
        public async Task<IActionResult> GetTeam([FromRoute] int teamId)
        {
            var userId = GetUserIdFromToken();
            
            try {
                var team = await _teamService.GetTeamByIdAsync(userId, teamId);

                if (team is null) {return NotFound(new {message = "Team not found"});}

                return Ok(team);
                
            } catch(Exception ex)
            {
                return BadRequest(new {message = ex.Message});
            }
        }

        [HttpGet("my-teams")]
        public async Task<IActionResult> GetUserTeams()
        {
            var userId = GetUserIdFromToken();
            if (userId == -1) { return Unauthorized(new {message = "User not authenticated"}); }

            var teams = await _teamService.GetMyTeamsAsync(userId);
            //if (!teams.Any()) return NotFound(new {message = "No teams"});

            return Ok(teams);
        }

        [HttpPost("{teamId}/add-member/{userToAddId}")]
        public async Task<IActionResult> AddMemberToTeam([FromRoute] int userToAddId, [FromRoute] int teamId)
        {
            var userId = GetUserIdFromToken();
            if (userId == -1) { return Unauthorized(new {message = "User not authenticated"}); }

            try {
                var success = await _teamService.AddMemberToTeamAsync(userId, teamId, userToAddId);
                return Ok(new {message = "User added to team successfully" });
            } catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message});
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