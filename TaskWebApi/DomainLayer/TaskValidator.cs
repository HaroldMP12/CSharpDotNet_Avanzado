using DomainLayer.Models;

namespace ApplicationLayer.Services.TaskServices
{
    public delegate bool ValidateTask(TaskData task);
    public static class TaskValidator
    {
        public static ValidateTask validate = task =>
       !string.IsNullOrWhiteSpace(task.Description) && task.DueDate > DateTime.Now;
    }
}
