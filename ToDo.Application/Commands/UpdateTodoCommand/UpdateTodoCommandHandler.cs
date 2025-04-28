using MediatR;
using ToDo.Domain.Interafaces;

namespace ToDo.Application.Commands.UpdateTodoCommand;


public class UpdateTodoCommandHandler(ITodoRepository repo) : IRequestHandler<UpdateTodoCommand, bool>
{
    public async Task<bool> Handle(UpdateTodoCommand request, CancellationToken cancellationToken)
    {
        var existing = await repo.GetByIdAsync(request.Id, cancellationToken);
        if (existing == null) return false;

        existing.Title = request.Title;
        existing.Description = request.Description;
        existing.ExpiryDate = request.ExpiryDate;
        existing.PercentComplete = request.PercentComplete;
        existing.IsDone = request.IsDone;

        await repo.UpdateAsync(existing, cancellationToken);
        return true;
    }
}
