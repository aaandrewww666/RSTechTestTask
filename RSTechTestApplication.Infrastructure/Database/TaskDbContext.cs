using Microsoft.EntityFrameworkCore;
using RSTechTestApplication.Domain.Entities;

namespace RSTechTestApplication.Infrastructure.Database
{
    public sealed class TaskDbContext : DbContext
    {
        public DbSet<TaskEntity> Tasks => Set<TaskEntity>();

        public TaskDbContext(DbContextOptions<TaskDbContext> options) : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            //options.EnableDetailedErrors();
            //options.LogTo(message => Debug.WriteLine(message));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("tasks");
            base.OnModelCreating(modelBuilder);
        }
    }
}
