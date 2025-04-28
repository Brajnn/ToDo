using Moq;
using ToDo.Application.Commands.MarkTodoAsDoneCommand;
using ToDo.Domain.Entities;
using ToDo.Domain.Interafaces;
using FluentAssertions;

namespace ToDo.Tests.Application.Commands.MarkTodoAsDoneCommandHandlerTests
{
    public class MarkTodoAsDoneCommandHandlerTests
    {
        [Fact]
        public async Task Handle_ShouldMarkTodoAsDone_WhenTodoExists()
        {
            // Arrange
            var id = Guid.NewGuid();
            var todo = new Todo
            {
                Id = id,
                Title = "Zrobić test",
                IsDone = false
            };

            var mockRepo = new Mock<ITodoRepository>();
            mockRepo.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync(todo);
            mockRepo.Setup(r => r.UpdateAsync(It.IsAny<Todo>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            var handler = new MarkTodoAsDoneCommandHandler(mockRepo.Object);
            var command = new MarkTodoAsDoneCommand { Id = id };

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeTrue();
            mockRepo.Verify(r => r.UpdateAsync(It.Is<Todo>(t => t.IsDone == true), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnFalse_WhenTodoNotFound()
        {
            // Arrange
            var id = Guid.NewGuid();
            var mockRepo = new Mock<ITodoRepository>();
            mockRepo.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync((Todo?)null);

            var handler = new MarkTodoAsDoneCommandHandler(mockRepo.Object);
            var command = new MarkTodoAsDoneCommand { Id = id };

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeFalse();
            mockRepo.Verify(r => r.UpdateAsync(It.IsAny<Todo>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}

