using ApplicationLayer.Services.TaskServices;
using DomainLayer.DTO;
using DomainLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TaskWebApi.Hubs;

namespace TaskWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly TaskService _service;
        private readonly IConfiguration _config;
        private readonly IHubContext<TaskHub> _hubContext;
        public TaskController(TaskService service, IConfiguration config, IHubContext<TaskHub> hubContext)
        {
            _service = service;
            _config = config;
            _hubContext = hubContext;
        }


        [HttpPost("Login")]
        public IActionResult Login([FromBody] LoginModel user)
        {
            if (user.Username == "admin" && user.Password == "password")
            {
                var token = GenerateJwtToken(user.Username);
                return Ok(new { token });
            }
            return Unauthorized("Credenciales incorrectas");
        }

        private string GenerateJwtToken(string username)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[] { new Claim(ClaimTypes.Name, username) };

            var token = new JwtSecurityToken(
                _config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddMinutes(60),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [HttpGet]
        public async Task<ActionResult<Response<TaskData>>> GetTaskAllAsync()
            => await _service.GetTaskAllAsync();

        [HttpGet("{id}")]
        public async Task<ActionResult<Response<TaskData>>> GetTaskByIdAllAsync(int id)
            => await _service.GetTaskByIdAllAsync(id);

        [HttpGet("pending")]
        public async Task<ActionResult<Response<TaskData>>> GetPendingTasks()
        {
            var result = await _service.GetPendingTasksAsync();

            if (result.Successful && result.DataList != null && result.DataList.Any())
            {
                // Notificar a los clientes conectados que se consultaron tareas pendientes
                await _hubContext.Clients.All.SendAsync("ReceiveTaskUpdate", $"Se consultaron las tareas pendientes.");

                return Ok(result);
            }
            else
            {
                return NoContent();
            }
        }

        [HttpPost]
        public async Task<ActionResult<Response<string>>> AddTaskAllAsync(TaskData taskData)
            => await _service.AddTaskAllAsync(taskData);

        [HttpPut]
        public async Task<ActionResult<Response<string>>> UpdateTaskAllAsync(TaskData taskData)
            => await _service.UpdateTaskAllAsync(taskData);

        [HttpDelete("{id}")]
        public async Task<ActionResult<Response<string>>> DeleteTaskAllAsync(int id)
            => await _service.DeleteTaskAllAsync(id);

        [HttpPost("high-priority")]
        public async Task<ActionResult<Response<string>>> AddHighPriorityTaskAsync([FromBody] string description)
    => await _service.AddHighPriorityTaskAsync(description);

        [HttpPost("low-priority")]
        public async Task<ActionResult<Response<string>>> AddLowPriorityTaskAsync([FromBody] string description)
            => await _service.AddLowPriorityTaskAsync(description);

    }
}
