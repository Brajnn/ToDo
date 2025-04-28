using Moq;
using ToDo.Application.Commands.SetTodoPercentCommand;
using ToDo.Domain.Entities;
using ToDo.Domain.Interafaces;
using FluentAssertions;

namespace ToDo.Tests.Application.Commands.SetTodoPercentCommandHandlerTests
{
    public class SetTodoPercentCommandHandlerTests
    {
        [Fact]
        public async Task Handle_ShouldSetPercent_WhenTodoExists()
        {
            // Arrange
            var id = Guid.NewGuid();
            var todo = new Todo
            {
                Id = id,
                Title = "Example",
                PercentComplete = 0
            };

            var mockRepo = new Mock<ITodoRepository>();
            mockRepo.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync(todo);
            mockRepo.Setup(r => r.UpdateAsync(It.IsAny<Todo>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            var handler = new SetTodoPercentCommandHandler(mockRepo.Object);
            var command = new SetTodoPercentCommand { Id = id, PercentComplete = 75 };

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeTrue();
            mockRepo.Verify(r => r.UpdateAsync(It.Is<Todo>(t => t.PercentComplete == 75), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnFalse_WhenTodoNotFound()
        {
            // Arrange
            var id = Guid.NewGuid();
            var mockRepo = new Mock<ITodoRepository>();
            mockRepo.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync((Todo?)null);

            var handler = new SetTodoPercentCommandHandler(mockRepo.Object);
            var command = new SetTodoPercentCommand { Id = id, PercentComplete = 50 };

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeFalse();
            mockRepo.Verify(r => r.UpdateAsync(It.IsAny<Todo>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
