using Microsoft.EntityFrameworkCore;
using ToDo.Domain.Entities;
using ToDo.Infrastructure.Presistence;
using ToDo.Infrastructure.Repositories;
using FluentAssertions;

namespace ToDo.Tests.Infrastructure.TodoRepositoryTests
{
    public class TodoRepositoryTests
    {
        private async Task<ToDoDbContext> CreateInMemoryDbContextAsync()
        {
            var options = new DbContextOptionsBuilder<ToDoDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var context = new ToDoDbContext(options);
            await context.Database.EnsureCreatedAsync();
            return context;
        }

        [Fact]
        public async Task AddAsync_ShouldAddTodo()
        {
            // Arrange
            var context = await CreateInMemoryDbContextAsync();
            var repo = new TodoRepository(context);
            var todo = new Todo { Title = "Test", Description = "Desc", ExpiryDate = DateTime.Today };

            // Act
            await repo.AddAsync(todo);
            var result = await context.Todos.FirstOrDefaultAsync();

            // Assert
            result.Should().NotBeNull();
            result!.Title.Should().Be("Test");
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnTodo_WhenExists()
        {
            // Arrange
            var context = await CreateInMemoryDbContextAsync();
            var todo = new Todo { Id = Guid.NewGuid(), Title = "Test" };
            context.Todos.Add(todo);
            await context.SaveChangesAsync();
            var repo = new TodoRepository(context);

            // Act
            var result = await repo.GetByIdAsync(todo.Id);

            // Assert
            result.Should().NotBeNull();
            result!.Id.Should().Be(todo.Id);
        }

        [Fact]
        public async Task DeleteAsync_ShouldRemoveTodo()
        {
            // Arrange
            var context = await CreateInMemoryDbContextAsync();
            var todo = new Todo { Id = Guid.NewGuid(), Title = "ToDelete" };
            context.Todos.Add(todo);
            await context.SaveChangesAsync();
            var repo = new TodoRepository(context);

            // Act
            await repo.DeleteAsync(todo.Id);
            var result = await context.Todos.FindAsync(todo.Id);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetIncomingAsync_ShouldReturnTodosWithinRange()
        {
            // Arrange
            var context = await CreateInMemoryDbContextAsync();
            var today = DateTime.Today;
            context.Todos.AddRange(
                new Todo { Title = "Inside", ExpiryDate = today.AddDays(1) },
                new Todo { Title = "Outside", ExpiryDate = today.AddDays(10) }
            );
            await context.SaveChangesAsync();
            var repo = new TodoRepository(context);

            // Act
            var result = await repo.GetIncomingAsync(today, today.AddDays(3));

            // Assert
            result.Should().ContainSingle(t => t.Title == "Inside");
        }
        [Fact]
        public async Task UpdateAsync_ShouldModifyTodo()
        {
            // Arrange
            var context = await CreateInMemoryDbContextAsync();
            var todo = new Todo { Id = Guid.NewGuid(), Title = "Old" };
            context.Todos.Add(todo);
            await context.SaveChangesAsync();
            var repo = new TodoRepository(context);

            // Act
            todo.Title = "Updated";
            await repo.UpdateAsync(todo);
            var result = await context.Todos.FindAsync(todo.Id);

            // Assert
            result!.Title.Should().Be("Updated");
        }

    }
}
