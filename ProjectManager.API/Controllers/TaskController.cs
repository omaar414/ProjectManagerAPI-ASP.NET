using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectManager.API.Application.DTOs.Task;
using ProjectManager.API.Application.Interfaces;

namespace ProjectManager.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;
        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpPost("create/{projectId}")]
        public async Task<IActionResult> Create([FromRoute]int projectId, CreateTaskDto taskDto )
        {
            var requesterId = GetUserIdFromToken();
            if (requesterId == -1) { return Unauthorized(new {message = "User not authenticated"}); }

            try {
                var success = await _taskService.CreateTaskAsync(requesterId ,projectId, taskDto);
                if (!success) {
                    return BadRequest(new {message = "Task could not be created"});
                }
                return Ok(new {message = "Task created successfully"});

            } catch ( Exception ex )
            {
                return BadRequest(new {message = ex.Message });
            }
        }

        [HttpGet("project/{projectId}/tasks")]
        public async Task<IActionResult> GetTasks([FromRoute]int projectId)
        {
             var requesterId = GetUserIdFromToken();
            if (requesterId == -1) { return Unauthorized(new {message = "User not authenticated"}); }

            try {
                var tasks = await _taskService.GetTasksAsync(requesterId, projectId);
                return Ok(tasks);

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