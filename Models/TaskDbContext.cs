using Microsoft.EntityFrameworkCore;
using System.Data;

namespace TaskManagementSystem.Models
{
    public class TaskDbContext : DbContext
    {
        public TaskDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<TaskModel> Tasks { get; set; }
    }
}
