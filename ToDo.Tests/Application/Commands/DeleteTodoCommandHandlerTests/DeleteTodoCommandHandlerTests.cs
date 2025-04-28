using Moq;
using ToDo.Application.Commands.DeleteTodoCommand;
using ToDo.Domain.Entities;
using ToDo.Domain.Interafaces;
using FluentAssertions;

namespace ToDo.Tests.Application.Commands.DeleteTodoCommandHandlerTests
{
    public class DeleteTodoCommandHandlerTests
    {
        [Fact]
        public async Task Handle_ShouldDeleteTodo_WhenTodoExists()
        {
            // Arrange
            var id = Guid.NewGuid();
            var todo = new Todo { Id = id, Title = "Delete" };

            var mockRepo = new Mock<ITodoRepository>();
            mockRepo.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync(todo);
            mockRepo.Setup(r => r.DeleteAsync(id, It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            var handler = new DeleteTodoCommandHandler(mockRepo.Object);
            var command = new DeleteTodoCommand { Id = id };

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeTrue();
            mockRepo.Verify(r => r.DeleteAsync(id, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnFalse_WhenTodoNotFound()
        {
            // Arrange
            var id = Guid.NewGuid();
            var mockRepo = new Mock<ITodoRepository>();
            mockRepo.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync((Todo?)null);

            var handler = new DeleteTodoCommandHandler(mockRepo.Object);
            var command = new DeleteTodoCommand { Id = id };

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeFalse();
            mockRepo.Verify(r => r.DeleteAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
