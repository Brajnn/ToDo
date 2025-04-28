using MediatR;


namespace ToDo.Application.Commands.MarkTodoAsDoneCommand;

public class MarkTodoAsDoneCommand : IRequest<bool>
{
    public Guid Id { get; set; }
}
