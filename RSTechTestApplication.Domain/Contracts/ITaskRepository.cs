using RSTechTestApplication.Domain.Entities;

namespace RSTechTestApplication.Domain.Contracts
{
    public interface ITaskRepository
    {
        Task<IEnumerable<TaskEntity>> GetAllAsync(CancellationToken ct = default);
        Task<TaskEntity> AddAsync(TaskEntity task, CancellationToken ct = default);
        Task<TaskEntity?> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task SoftDeleteAsync(Guid id, CancellationToken ct = default);
        Task UpdateCompletionStatusAsync(Guid id, bool isCompleted, CancellationToken ct = default);
    }
}
