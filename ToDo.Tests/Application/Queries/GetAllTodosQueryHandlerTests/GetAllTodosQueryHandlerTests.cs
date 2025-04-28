using Moq;
using ToDo.Application.Queries.GetAllTodosQuery;
using ToDo.Domain.Entities;
using ToDo.Domain.Interafaces;
using FluentAssertions;

namespace ToDo.Tests.Application.Queries.GetAllTodosQueryHandlerTests
{
    public class GetAllTodosQueryHandlerTests
    {
        [Fact]
        public async Task Handle_ShouldReturnListOfTodos()
        {
            // Arrange
            var mockRepo = new Mock<ITodoRepository>();
            var todos = new List<Todo>
            {
                new Todo { Title = "Test 1" },
                new Todo { Title = "Test 2" }
            };
            mockRepo.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(todos);
            var handler = new GetAllTodosQueryHandler(mockRepo.Object);

            // Act
            var result = await handler.Handle(new GetAllTodosQuery(), CancellationToken.None);

            // Assert
            result.Should().NotBeNullOrEmpty();
            result.Should().HaveCount(2);
            mockRepo.Verify(r => r.GetAllAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
