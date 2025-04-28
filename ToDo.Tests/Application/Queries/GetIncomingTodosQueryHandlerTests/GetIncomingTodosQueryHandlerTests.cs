using Moq;
using ToDo.Application.Queries.GetIncomingTodosQuery;
using ToDo.Domain.Entities;
using ToDo.Domain.Interafaces;
using FluentAssertions;

namespace ToDo.Tests.Application.Queries.GetIncomingTodosQueryHandlerTests
{
    public class GetIncomingTodosQueryHandlerTests
    {
        [Fact]
        public async Task Handle_ShouldReturnTodos_WhenRangeIsToday()
        {
            // Arrange
            var today = DateTime.Today;

            var todos = new List<Todo>
            {
                new Todo { Title = "Today", ExpiryDate = today.AddHours(10) }
            };

            var mockRepo = new Mock<ITodoRepository>();
            mockRepo.Setup(r => r.GetIncomingAsync(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(todos);
            var handler = new GetIncomingTodosQueryHandler(mockRepo.Object);
            var command = new GetIncomingTodosQuery { Range = "today" };

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().ContainSingle(t => t.Title == "Today");
        }

        [Fact]
        public async Task Handle_ShouldReturnEmptyList_WhenNoTodosForTomorrow()
        {
            // Arrange
            var tomorrow = DateTime.Today.AddDays(1);

            var mockRepo = new Mock<ITodoRepository>();
            mockRepo.Setup(r => r.GetIncomingAsync(tomorrow, tomorrow, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Todo>());

            var handler = new GetIncomingTodosQueryHandler(mockRepo.Object);
            var command = new GetIncomingTodosQuery { Range = "tomorrow" };

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task Handle_ShouldReturnMultipleTodos_WhenRangeIsWeek()
        {
            // Arrange
            var from = DateTime.Today;
            var to = from.AddDays(7);

            var todos = new List<Todo>
            {
                new Todo { Title = "Task1", ExpiryDate = from.AddDays(1) },
                new Todo { Title = "Task2", ExpiryDate = from.AddDays(2) }
            };

            var mockRepo = new Mock<ITodoRepository>();
            mockRepo.Setup(r => r.GetIncomingAsync(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(todos);
            var handler = new GetIncomingTodosQueryHandler(mockRepo.Object);
            var command = new GetIncomingTodosQuery { Range = "week" };

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().HaveCount(2);
        }

        [Fact]
        public async Task Handle_ShouldDefaultToToday_WhenRangeIsUnknown()
        {
            // Arrange
            var today = DateTime.Today;

            var todos = new List<Todo>
            {
                new Todo { Title = "Default", ExpiryDate = today }
            };

            var mockRepo = new Mock<ITodoRepository>();
            mockRepo.Setup(r => r.GetIncomingAsync(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(todos);
            var handler = new GetIncomingTodosQueryHandler(mockRepo.Object);
            var command = new GetIncomingTodosQuery { Range = "over" };

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().ContainSingle(t => t.Title == "Default");
        }
    }
}
