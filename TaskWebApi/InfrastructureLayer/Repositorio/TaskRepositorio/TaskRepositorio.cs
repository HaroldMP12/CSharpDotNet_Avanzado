using DomainLayer.Models;
using InfrastructureLayer.Context;
using InfrastructureLayer.Repositorio.Commons;
using Microsoft.EntityFrameworkCore;

namespace InfrastructureLayer.Repositorio.TaskRepositorio
{
    public class TaskRepositorio : ICommonProcess<TaskData>
    {
        private readonly TaskWebApiContext _context;
        public TaskRepositorio(TaskWebApiContext taskWebApiContext)
        {
            _context = taskWebApiContext;
        }
        public async Task<IEnumerable<TaskData>> GetAllAsync()
            => await _context.TaskData.ToListAsync();

        public async Task<TaskData> GetIdAsync(int id)
            => await _context.TaskData.FirstOrDefaultAsync(x => x.Id == id);

        public async Task<(bool IsSuccess, string Message)> AddAsync(TaskData entry)
        {
            try
            {
                await _context.TaskData.AddAsync(entry);
                await _context.SaveChangesAsync();
                return (true, "La tarea fue guardada correctamente.");
            }
            catch (Exception)
            {
                return (false, "No se pudo guardar la tarea correctamente.");
            }
        }
        public async Task<(bool IsSuccess, string Message)> UpdateAsync(TaskData entry)
        {

            try
            {
                _context.TaskData.Update (entry);
                await _context.SaveChangesAsync();
                return (true, "La tarea ha sido actualizada correctamente.");
            }
            catch (Exception)
            {
                return (false, "No se pudo actualizar la tarea correctamente.");
            }
        }
        public async Task<(bool IsSuccess, string Message)> DeleteAsync(int id)
        {

            try
            {
                var tarea = await _context.TaskData.FindAsync(id);
                if (tarea != null)
                {
                    _context.TaskData.Remove(tarea);
                    await _context.SaveChangesAsync();
                    return (true, "La tarea se pudo eliminar correctamente.");
                }
                else
                {
                    return (false, "No se encontro la tarea.");
                }
            }
            catch (Exception)
            {
                return (false, "No se pudo eliminar la tarea correctamente.");
            }
        }
    }
}
