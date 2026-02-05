using Microsoft.Extensions.DependencyInjection;
using RSTechTestApplication.Domain.Contracts;
using RSTechTestApplication.Infrastructure.Repositories;

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
            services.AddScoped<ITaskRepository, TaskRepository>();

            return services;
        }
    }
}
