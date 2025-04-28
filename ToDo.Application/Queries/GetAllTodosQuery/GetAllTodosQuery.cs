using MediatR;
using ToDo.Domain.Entities;

namespace ToDo.Application.Queries.GetAllTodosQuery;

public class GetAllTodosQuery : IRequest<IEnumerable<Todo>>
{
}
