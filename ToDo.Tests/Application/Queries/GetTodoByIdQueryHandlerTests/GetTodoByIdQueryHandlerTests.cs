using Moq;
using ToDo.Application.Queries.GetTodoByIdQuery;
using ToDo.Domain.Entities;
using ToDo.Domain.Interafaces;
using FluentAssertions;

namespace ToDo.Tests.Application.Queries.GetTodoByIdQueryHandlerTests
{
    public class GetTodoByIdQueryHandlerTests
    {
        [Fact]
        public async Task Handle_ShouldReturnTodo_WhenIdExists()
        {
            // Arrange
            var id = Guid.NewGuid();
            var todo = new Todo { Id = id, Title = "Test Todo" };

            var mockRepo = new Mock<ITodoRepository>();
            mockRepo.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync(todo);

            var handler = new GetTodoByIdQueryHandler(mockRepo.Object);

            // Act
            var result = await handler.Handle(new GetTodoByIdQuery { Id = id }, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result!.Id.Should().Be(id);
            result.Title.Should().Be("Test Todo");
        }

        [Fact]
        public async Task Handle_ShouldReturnNull_WhenTodoDoesNotExist()
        {
            // Arrange
            var id = Guid.NewGuid();
            var mockRepo = new Mock<ITodoRepository>();
            mockRepo.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync((Todo?)null);

            var handler = new GetTodoByIdQueryHandler(mockRepo.Object);

            // Act
            var result = await handler.Handle(new GetTodoByIdQuery { Id = id }, CancellationToken.None);

            // Assert
            result.Should().BeNull();
        }
    }
}