using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RSTechTestApplication.Domain.Entities;
using RSTechTestApplication.Infrastructure.Database;
using RSTechTestApplication.Infrastructure.Repositories;

namespace RSTechTestApplication.Tests
{
    public class TaskRepositoryInMemoryTests
    {
        private TaskDbContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<TaskDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new TaskDbContext(options);
        }

        private ILogger<TaskRepository> CreateLogger()
        {
            return LoggerFactory.Create(builder => { })
                .CreateLogger<TaskRepository>();
        }

        [Fact]
        public async Task AddTask_ValidTask_ShouldSave()
        {
            using var context = CreateContext();
            var logger = CreateLogger();

            var repository = new TaskRepository(context, logger);

            var task = new TaskEntity("hola");

            var addedTask = await repository.AddAsync(task);

            TaskEntity? foundTask = await repository.GetByIdAsync(addedTask.Id);

            foundTask.Should().NotBeNull();
            foundTask.Id.Should().Be(addedTask.Id);
        }

        [Theory]
        [InlineData("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA")]
        [InlineData("")]
        public async Task AddTask_EmptyTitle_ShouldNotSave(string title)
        {
            using var context = CreateContext();
            var logger = CreateLogger();

            var repository = new TaskRepository(context, logger);

            var task = new TaskEntity(title);

            var exception = await Assert.ThrowsAnyAsync<Exception>(
                async () => await repository.AddAsync(task));

            exception.Should().NotBeNull();
        }
    }
}