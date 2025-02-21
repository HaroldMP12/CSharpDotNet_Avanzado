using DomainLayer.DTO;
using DomainLayer.Models;
using InfrastructureLayer.Repositorio.Commons;

namespace ApplicationLayer.Services.TaskServices
{
    public class TaskService
    {
        private readonly ICommonProcess<TaskData> _commonProcess;
        private readonly TaskSeqService _taskSeqService;

        public TaskService(ICommonProcess<TaskData> commonProcess, TaskSeqService taskSeqService)
        {
            _commonProcess = commonProcess;
            _taskSeqService = taskSeqService;
        }

        public async Task<Response<TaskData>> GetTaskAllAsync()
        {
            var response = new Response <TaskData>();

            try
            {
                response.DataList = await _commonProcess.GetAllAsync();
                response.Successful = true;
            }
            catch (Exception e)
            {

                response.Errors.Add(e.Message);
            }
            return response;

        }

        //Factory Pattern

        public async Task<Response<string>> AddHighPriorityTaskAsync(string description)
        {
            var response = new Response<string>();

            try
            {
                var task = FactoryPattern.CreateHighPriorityTask(description);

                _taskSeqService.EnqueueTask(async () =>
                {
                var result = await _commonProcess.AddAsync(task);
                response.Message = result.Message;
                response.Successful = result.IsSuccess;

                if (result.IsSuccess)
                    TaskNotifier.NotifyCreation(task);
                });
            }
            catch (Exception e)
            {
                response.Errors.Add(e.Message);
            }

            return response;
        }

        public async Task<Response<string>> AddLowPriorityTaskAsync(string description)
        {
            var response = new Response<string>();

            try
            {
                var task = FactoryPattern.CreateLowPriorityTask(description);

                _taskSeqService.EnqueueTask(async () =>
                {
                    var result = await _commonProcess.AddAsync(task);
                    response.Message = result.Message;
                    response.Successful = result.IsSuccess;

                    if (result.IsSuccess)
                        TaskNotifier.NotifyCreation(task);
                });
            }

            catch (Exception e)
            {
                response.Errors.Add(e.Message);
            }

            return response;
        }


        public async Task<Response<TaskData>> GetPendingTasksAsync()
        {
            var response = new Response<TaskData>();

            try
            {
              
                var allTasks = await _commonProcess.GetAllAsync();

                
                var pendingTasks = allTasks
                    .Where(task => task.Status == "Pendiente")
                    .ToList();

                if (pendingTasks.Any())
                {
                    response.DataList = pendingTasks;
                    response.Successful = true;
                }
                else
                {
                    response.Successful = false;
                    response.Message = "No hay tareas pendientes.";
                }
            }
            catch (Exception e)
            {
                response.Errors.Add(e.Message);
                response.Successful = false;
            }

            return response;
        }


        public async Task<Response<TaskData>> GetTaskByIdAllAsync(int id)
        {
            var response = new Response<TaskData>();

            try
            {
                var result = await _commonProcess.GetIdAsync(id);
                if (result != null)
                {
                    response.SingleData = result;
                    response.Successful = true;
                }
                else
                {
                    response.Successful = false;
                    response.Message = "No se encontro informacion.";
                }
            }
            catch (Exception e)
            {

                response.Errors.Add(e.Message);
            }
            return response;

        }

        public async Task<Response<string>> AddTaskAllAsync(TaskData taskData)
        {

            var response = new Response<string>();

            try
            {
                if (!TaskValidator.validate(taskData))
                {
                    response.Successful = false;
                    response.Message = "La tarea no es válida. Asegúrate de que tenga una descripción y una fecha futura.";
                    return response;
                }

                _taskSeqService.EnqueueTask(async () =>
                {
                    var result = await _commonProcess.AddAsync(taskData);
                    response.Message = result.Message;
                    response.Successful = result.IsSuccess;

                    if(result.IsSuccess)
                       TaskNotifier.NotifyCreation(taskData);
                });
            }
            catch (Exception e)
            {
                response.Errors.Add(e.Message);
            }

            return response;

        }

        public async Task<Response<string>> UpdateTaskAllAsync(TaskData taskData)
        {
            var response = new Response<string>();

            try
            {
                _taskSeqService.EnqueueTask(async () =>
                {
                    var result = await _commonProcess.UpdateAsync(taskData);
                    response.Message = result.Message;
                    response.Successful = result.IsSuccess;
                });
            }
            catch (Exception e)
            {

                response.Errors.Add(e.Message);
            }
            return response;

        }

        public async Task<Response<string>> DeleteTaskAllAsync(int id)
        {
            var response = new Response<string>();

            try
            {
                _taskSeqService.EnqueueTask(async () =>
                {
                    var result = await _commonProcess.DeleteAsync(id);
                    response.Message = result.Message;
                    response.Successful = result.IsSuccess;
                });
            }
            catch (Exception e)
            {

                response.Errors.Add(e.Message);
            }
            return response;

        }
    }
}
