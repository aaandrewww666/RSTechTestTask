using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace RSTechTestApplication.Infrastructure.Database
{
    public class TaskDbContextFactory : IDesignTimeDbContextFactory<TaskDbContext>
    {
        public TaskDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<TaskDbContext>();

            optionsBuilder.UseNpgsql(PostgreSQLConnectionStringProvider.ConnectionString);

            return new TaskDbContext(optionsBuilder.Options);
        }
    }
}
