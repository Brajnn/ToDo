using MediatR;
using ToDo.Domain.Interafaces;

namespace ToDo.Application.Commands.DeleteTodoCommand;

public class DeleteTodoCommandHandler(ITodoRepository repository) : IRequestHandler<DeleteTodoCommand, bool>
{
    public async Task<bool> Handle(DeleteTodoCommand request, CancellationToken cancellationToken)
    {
        var existing = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (existing == null) return false;

        await repository.DeleteAsync(request.Id, cancellationToken);
        return true;
    }
}
