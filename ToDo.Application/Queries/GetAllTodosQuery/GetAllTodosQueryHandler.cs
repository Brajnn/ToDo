using MediatR;

using ToDo.Domain.Entities;
using ToDo.Domain.Interafaces;

namespace ToDo.Application.Queries.GetAllTodosQuery;

public class GetAllTodosQueryHandler(ITodoRepository repository) : IRequestHandler<GetAllTodosQuery, IEnumerable<Todo>>
{
    public async Task<IEnumerable<Todo>> Handle(GetAllTodosQuery request, CancellationToken cancellationToken)
    {
        return await repository.GetAllAsync(cancellationToken);
    }
}
