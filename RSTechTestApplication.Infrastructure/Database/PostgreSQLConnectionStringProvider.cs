namespace RSTechTestApplication.Infrastructure.Database
{
    internal static class PostgreSQLConnectionStringProvider
    {
        private static readonly string connString = "Host=localhost;Port=5432;Database=tasks;Username=test;Password=test;";
        public static string ConnectionString
        {
            get => connString;
        }
    }
}
