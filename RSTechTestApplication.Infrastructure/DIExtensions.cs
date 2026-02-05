using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RSTechTestApplication.Domain.Contracts;
using RSTechTestApplication.Infrastructure.Database;
using RSTechTestApplication.Infrastructure.Database.Initializing;
using RSTechTestApplication.Infrastructure.Repositories;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("RSTechTestApplication.Tests")]

namespace RSTechTestApplication.Infrastructure
{
    public static class DIExtensions
    {
        /// <summary>
        /// Services for Infrastructure layer
        /// </summary>
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services
            )
        {
            services.AddDbContext<TaskDbContext>(options =>
            {
                options.UseNpgsql(PostgreSQLConnectionStringProvider.ConnectionString);
            }, ServiceLifetime.Scoped);

            services.AddScoped<ITaskRepository, TaskRepository>();

            services.AddSingleton<DatabaseInitializer>();

            return services;
        }
    }
}
