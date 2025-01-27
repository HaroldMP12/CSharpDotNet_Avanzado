using DomainLayer.DTO;
using DomainLayer.Models;
using InfrastructureLayer.Repositorio.Commons;

namespace ApplicationLayer.Services.TaskServices
{
    public class TaskService
    {
        private readonly ICommonProcess<TaskData> _commonProcess;

        public TaskService(ICommonProcess<TaskData> commonProcess)
        {
            _commonProcess = commonProcess;
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
                var result = await _commonProcess.AddAsync(taskData);
                response.Message = result.Message;
                response.Successful = result.IsSuccess;
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
                var result = await _commonProcess.UpdateAsync(taskData);
                response.Message = result.Message;
                response.Successful = result.IsSuccess;
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
                var result = await _commonProcess.DeleteAsync(id);
                response.Message = result.Message;
                response.Successful = result.IsSuccess;
            }
            catch (Exception e)
            {

                response.Errors.Add(e.Message);
            }
            return response;

        }
    }
}
