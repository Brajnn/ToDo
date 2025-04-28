using MediatR;
using ToDo.Domain.Entities;

namespace ToDo.Application.Queries.GetIncomingTodosQuery;

public class GetIncomingTodosQuery : IRequest<IEnumerable<Todo>>
{
    public string Range { get; set; } = "today";
}
