namespace RSTechTestApplication.Domain.Entities
{
    public sealed class TaskEntity
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public bool IsCompleted { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsDeleted { get; set; } = false;

        public TaskEntity(string title)
        {
            Id = Guid.NewGuid();
            CreatedAt = DateTime.UtcNow;
            Title = title;
        }

        public TaskEntity(Guid id, string title, bool isCompleted, DateTime createdAt)
        {
            Id = id;
            Title = title;
            IsCompleted = isCompleted;
            CreatedAt = createdAt;
        }
    }
}
