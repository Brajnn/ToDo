using Moq;
using ToDo.Application.Commands.UpdateTodoCommand;
using ToDo.Domain.Entities;
using ToDo.Domain.Interafaces;
using FluentAssertions;

namespace ToDo.Tests.Application.Commands.UpdateTodoCommandHandlerTests
{
    public class UpdateTodoCommandHandlerTests
    {
        [Fact]
        public async Task Handle_ShouldUpdateTodo_WhenTodoExists()
        {
            // Arrange
            var id = Guid.NewGuid();
            var existingTodo = new Todo
            {
                Id = id,
                Title = "Old Title",
                Description = "Old Desc",
                ExpiryDate = DateTime.UtcNow,
                PercentComplete = 0
            };

            var mockRepo = new Mock<ITodoRepository>();
            mockRepo.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync(existingTodo);
            mockRepo.Setup(r => r.UpdateAsync(It.IsAny<Todo>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            var handler = new UpdateTodoCommandHandler(mockRepo.Object);

            var command = new UpdateTodoCommand
            {
                Id = id,
                Title = "New Title",
                Description = "New Desc",
                ExpiryDate = DateTime.UtcNow.AddDays(3),
                PercentComplete = 50
            };

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeTrue();
            mockRepo.Verify(r => r.UpdateAsync(It.Is<Todo>(t =>
                t.Title == "New Title" &&
                t.Description == "New Desc" &&
                t.ExpiryDate.Date == command.ExpiryDate.Date &&
                t.PercentComplete == 50
            ), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnFalse_WhenTodoNotFound()
        {
            // Arrange
            var id = Guid.NewGuid();

            var mockRepo = new Mock<ITodoRepository>();
            mockRepo.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync((Todo?)null);

            var handler = new UpdateTodoCommandHandler(mockRepo.Object);

            var command = new UpdateTodoCommand
            {
                Id = id,
                Title = "Test",
                Description = "Test",
                ExpiryDate = DateTime.UtcNow.AddDays(1),
                PercentComplete = 25
            };

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeFalse();
            mockRepo.Verify(r => r.UpdateAsync(It.IsAny<Todo>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}

