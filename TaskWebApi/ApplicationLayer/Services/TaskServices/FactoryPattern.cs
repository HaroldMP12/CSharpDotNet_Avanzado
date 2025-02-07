using DomainLayer.Models;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLayer.Services.TaskServices
{
    public class FactoryPattern
    {
        public static TaskData CreateHighPriorityTask(string description)
        {
            return new TaskData
            {
                Description = description,
                DueDate = DateTime.Now.AddDays(1),
                Status = "Pending",
                AdditionalData = "High Priority"
            };
        }

        public static TaskData CreateLowPriorityTask(string description)
        {
            return new TaskData
            {
                Description = description,
                DueDate = DateTime.Now.AddDays(7),
                Status = "Pending",
                AdditionalData = "Low Priority"
            };
        }
    }
}
