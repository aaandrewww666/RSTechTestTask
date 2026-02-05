using Microsoft.Extensions.DependencyInjection;

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

            return services;
        }
    }
}
