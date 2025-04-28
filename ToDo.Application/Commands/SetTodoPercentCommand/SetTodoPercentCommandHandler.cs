using MediatR;
using ToDo.Domain.Interafaces;

namespace ToDo.Application.Commands.SetTodoPercentCommand;

public class SetTodoPercentCommandHandler(ITodoRepository repository) : IRequestHandler<SetTodoPercentCommand, bool>
{
    public async Task<bool> Handle(SetTodoPercentCommand request, CancellationToken cancellationToken)
    {
        var todo = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (todo == null) return false;

        todo.PercentComplete = request.PercentComplete;
        await repository.UpdateAsync(todo, cancellationToken);

        return true;
    }
}
