using DomainLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace InfrastructureLayer.Context
{
    public class TaskWebApiContext : DbContext
    {
        public TaskWebApiContext(DbContextOptions options ) :
            base( options )
        {
            
        }
        public DbSet<TaskData> TaskData { get; set; }
    }
}
