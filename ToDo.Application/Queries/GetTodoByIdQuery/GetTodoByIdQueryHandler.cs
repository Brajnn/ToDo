using MediatR;
using ToDo.Domain.Entities;
using ToDo.Domain.Interafaces;

namespace ToDo.Application.Queries.GetTodoByIdQuery;

public class GetTodoByIdQueryHandler(ITodoRepository repository) : IRequestHandler<GetTodoByIdQuery, Todo?>
{
    public async Task<Todo?> Handle(GetTodoByIdQuery request, CancellationToken cancellationToken)
    {
        return await repository.GetByIdAsync(request.Id, cancellationToken);
    }
}
