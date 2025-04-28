using MediatR;
using ToDo.Domain.Entities;
using ToDo.Domain.Interafaces;

namespace ToDo.Application.Commands.CreateTodoCommand;

public class CreateTodoCommandHandler(ITodoRepository repository) : IRequestHandler<CreateTodoCommand, Guid>
{
    public async Task<Guid> Handle(CreateTodoCommand request, CancellationToken cancellationToken)
    {
        var todo = new Todo
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            Description = request.Description,
            ExpiryDate = request.ExpiryDate,
            PercentComplete = 0,
            IsDone = false
        };

        await repository.AddAsync(todo, cancellationToken);
        return todo.Id;
    }
}
