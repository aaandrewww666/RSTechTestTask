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

        public async Task<DatabaseInitResult> InitializeMigrations()
        {
            try
            {
                using var scope = _services.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<TaskDbContext>();

                if (!await context.Database.CanConnectAsync())
                {
                    return DatabaseInitResult.Failure("Couldn't connect to the database");
                }

                var pendingMigrations = await context.Database.GetPendingMigrationsAsync();

                if (!pendingMigrations.Any())
                {
                    _logger.LogInformation("The database is up-to-date, no migrations are required");
                    return DatabaseInitResult.Success(0);
                }

                _logger.LogInformation($"Applying {pendingMigrations.Count()} migrations...");

                await context.Database.MigrateAsync();

                _logger.LogInformation("Migrations have been successfully applied");
                return DatabaseInitResult.Success(pendingMigrations.Count());
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
            DatabaseInitResult result = DatabaseInitResult.Failure("Couldn't connect to the database");

            for (int attempt = 1; attempt <= maxAttempts; attempt++)
            {
                _logger.LogInformation($"Attempt {attempt} to initialize database");
                result = await InitializeMigrations();

                if (result.IsSuccess)
                    return result;
            }
            return result;
        }
    }
}
