using MediatR;
using ToDo.Domain.Interafaces;

namespace ToDo.Application.Commands.MarkTodoAsDoneCommand;

public class MarkTodoAsDoneCommandHandler(ITodoRepository repository) : IRequestHandler<MarkTodoAsDoneCommand, bool>
{
    public async Task<bool> Handle(MarkTodoAsDoneCommand request, CancellationToken cancellationToken)
    {
        var todo = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (todo == null) return false;

        todo.IsDone = true;
        todo.PercentComplete = 100;

        await repository.UpdateAsync(todo, cancellationToken);
        return true;
    }
}
