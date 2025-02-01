using ApplicationLayer.Services.TaskServices;
using DomainLayer.DTO;
using DomainLayer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TaskWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly TaskService _service;
        public TaskController(TaskService service)
        {
            _service = service;
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
    }
}
