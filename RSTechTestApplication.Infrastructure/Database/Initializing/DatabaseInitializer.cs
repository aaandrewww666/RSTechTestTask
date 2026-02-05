using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace RSTechTestApplication.Infrastructure.Database.Initializing
{
    public sealed class DatabaseInitializer
    {
        private readonly IServiceProvider _services;
        private readonly ILogger<DatabaseInitializer> _logger;
        public DatabaseInitializer(IServiceProvider services, ILogger<DatabaseInitializer> logger)
        {
            _services = services;
            _logger = logger;
        }

        public DatabaseInitResult InitializeMigrations()
        {
            try
            {
                using var scope = _services.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<TaskDbContext>();

                _logger.LogInformation($"Trying to migrate");
                context.Database.Migrate();

                return DatabaseInitResult.Success();
            }
            catch (Npgsql.NpgsqlException ex)
            {
                return DatabaseInitResult.Failure($"PostgreSQL exception: {ex.Message}\n\n");
            }
            catch (DbUpdateException ex)
            {
                return DatabaseInitResult.Failure($"Migration application error: {ex.InnerException?.Message ?? ex.Message}\n");
            }
            catch (InvalidOperationException ex) when (ex.Message.Contains("migration"))
            {
                return DatabaseInitResult.Failure($"Migrations conflict: {ex.Message}\n");
            }
            catch (Exception ex)
            {
                return DatabaseInitResult.Failure($"Unexpected error: {ex.Message}\n");
            }
        }

        public async Task<DatabaseInitResult> InitializeMigrationsWithRetryAsync(int maxAttempts = 3)
        {
            var databaseInitResult = await Task.Run(() =>
            {
                DatabaseInitResult result = DatabaseInitResult.Failure("Couldn't connect to the database");

                for (int attempt = 1; attempt <= maxAttempts; attempt++)
                {
                    _logger.LogInformation($"Attempt {attempt} to initialize database");
                    result = InitializeMigrations();

                    if (result.IsSuccess)
                        return result;
                }
                return result;
            });

            return databaseInitResult;
        }
    }
}
