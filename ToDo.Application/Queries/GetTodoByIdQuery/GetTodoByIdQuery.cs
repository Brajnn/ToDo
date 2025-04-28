using MediatR;
using ToDo.Domain.Entities;

namespace ToDo.Application.Queries.GetTodoByIdQuery;

public class GetTodoByIdQuery : IRequest<Todo?>
{
    public Guid Id { get; set; }
}
