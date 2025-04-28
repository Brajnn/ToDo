using Moq;
using ToDo.Application.Commands.CreateTodoCommand;
using ToDo.Domain.Interafaces;
using ToDo.Domain.Entities;
using FluentAssertions;

namespace ToDo.Tests.Application.Commands.CreateTodoCommandTests
{
    public class CreateTodoCommandHandlerTests
    {
        [Fact]
        public async Task Handle_ShouldReturnGuid_WhenTodoIsCreated()
        {
            // Arrange
            var mockRepo = new Mock<ITodoRepository>();
            mockRepo.Setup(r => r.AddAsync(It.IsAny<Todo>(), It.IsAny<CancellationToken>()))
                    .Returns(Task.CompletedTask);
            var handler = new CreateTodoCommandHandler(mockRepo.Object);
            var command = new CreateTodoCommand
            {
                Title = "Test",
                Description = "Desc",
                ExpiryDate = DateTime.UtcNow.AddDays(1)
            };

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeEmpty();
            mockRepo.Verify(r => r.AddAsync(It.IsAny<Todo>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}

