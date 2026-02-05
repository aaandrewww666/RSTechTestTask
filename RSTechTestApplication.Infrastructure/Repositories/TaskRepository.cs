using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RSTechTestApplication.Domain.Contracts;
using RSTechTestApplication.Domain.Entities;
using RSTechTestApplication.Infrastructure.Database;

namespace RSTechTestApplication.Infrastructure.Repositories
{
    internal sealed class TaskRepository : ITaskRepository
    {
        private readonly TaskDbContext _context;
        private readonly ILogger<TaskRepository> _logger;

        private const int MinTitleLength = 1;
        private const int MaxTitleLength = 100;

        public TaskRepository(TaskDbContext context, ILogger<TaskRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<TaskEntity> AddAsync(TaskEntity task, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();
            ArgumentNullException.ThrowIfNull(task);

            try
            {
                ValidateTask(task);

                _logger.LogInformation("Adding task: {Title}", task.Title);

                await _context.Tasks.AddAsync(task, ct);
                await _context.SaveChangesAsync(ct);

                _logger.LogInformation("Task {TaskId} successfully added", task.Id);

                return task;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured on adding task");
                throw;
            }
        }

        public async Task<IEnumerable<TaskEntity>> GetAllAsync(CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();

            try
            {
                _logger.LogDebug("Getting all tasks from the database");

                return await _context.Tasks
                    .Where(t => !t.IsDeleted)
                    .AsNoTracking()
                    .OrderByDescending(t => t.CreatedAt)
                    .ToListAsync(ct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred when receiving the task list");
                throw;
            }
        }

        public async Task<TaskEntity?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();

            try
            {
                _logger.LogDebug("Getting all tasks from the database");

                return await _context.Tasks
                    .FirstOrDefaultAsync(t => t.Id == id, ct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred when receiving the task list");
                throw;
            }
        }

        public async Task SoftDeleteAsync(Guid id, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();

            try
            {
                _logger.LogInformation("Soft deleting task {TaskId}", id);

                var task = await _context.Tasks.FindAsync([id], ct);

                if (task is null)
                {
                    _logger.LogWarning("Task {TaskId} not found", id);
                    return;
                }

                task.IsDeleted = true;

                await _context.SaveChangesAsync(ct);

                _logger.LogInformation("Task {TaskId} soft deleted", id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occured on deleting {TaskId}", id);
                throw;
            }
        }

        public async Task UpdateCompletionStatusAsync(Guid id, bool isCompleted, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();

            try
            {
                _logger.LogInformation("Changing completed state of task {TaskId}", id);

                var task = await _context.Tasks.FindAsync([id], ct);

                if (task is null)
                {
                    _logger.LogWarning("Task {TaskId} not found", id);
                    return;
                }

                task.IsCompleted = isCompleted;

                await _context.SaveChangesAsync(ct);

                _logger.LogInformation("Task {TaskId} completed state changed", id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occured on changing completed state of task {TaskId}", id);
                throw;
            }
        }

        private void ValidateTask(TaskEntity task)
        {
            if (string.IsNullOrWhiteSpace(task.Title))
            {
                throw new ArgumentException("Title cannot be empty", nameof(task.Title));
            }

            if (task.Title.Length < MinTitleLength)
            {
                throw new ArgumentException(
                    $"Title must be at least {MinTitleLength} character long",
                    nameof(task.Title));
            }

            if (task.Title.Length > MaxTitleLength)
            {
                throw new ArgumentException(
                    $"Title must be at less {MaxTitleLength} characters long",
                    nameof(task.Title));
            }
        }
    }
}
