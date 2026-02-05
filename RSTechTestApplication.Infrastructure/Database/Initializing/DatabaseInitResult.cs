namespace RSTechTestApplication.Infrastructure.Database.Initializing
{
    public sealed class DatabaseInitResult
    {
        public bool IsSuccess { get; private set; }
        public string? ErrorMessage { get; private set; }
        public Exception? Exception { get; private set; }
        public int MigrationsApplied { get; private set; }

        public static DatabaseInitResult Success(int migrationsApplied = 0) => new()
        {
            IsSuccess = true,
            MigrationsApplied = migrationsApplied
        };

        public static DatabaseInitResult Failure(string message, Exception? ex = null) => new()
        {
            IsSuccess = false,
            ErrorMessage = message,
            Exception = ex
        };
    }
}
