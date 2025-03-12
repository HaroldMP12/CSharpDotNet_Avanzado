using ApplicationLayer.Services.TaskServices;
using DomainLayer.DTO;
using DomainLayer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace TaskWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly TaskService _service;
        private readonly IConfiguration _configuration;
        public TaskController(TaskService service, IConfiguration configuration)
        {
            _service = service;
            _configuration = configuration;
        }


        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel loginModel)
        {
            // Validar las credenciales del usuario (esto debería ser con base de datos)
            if (loginModel.Username == "admin" && loginModel.Password == "password") // Simulación
            {
                var token = GenerateJwtToken(loginModel.Username);
                return Ok(new { Token = token });
            }

            return Unauthorized("Usuario o contraseña incorrectos.");
        }

        private string GenerateJwtToken(string username)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("role", "Admin") // Agregar roles si es necesario
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

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
