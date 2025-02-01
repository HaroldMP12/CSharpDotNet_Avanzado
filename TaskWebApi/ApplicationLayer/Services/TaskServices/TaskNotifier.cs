using DomainLayer.Models;

namespace ApplicationLayer.Services.TaskServices
{
    public class TaskNotifier
    {
        public static Action<TaskData> NotifyCreation = task =>
        {
            Console.WriteLine($"Tarea creada: {task.Description}, vence el: {task.DueDate}");
        };
    }
}
