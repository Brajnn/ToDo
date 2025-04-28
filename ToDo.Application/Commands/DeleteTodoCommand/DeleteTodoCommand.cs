using MediatR;


namespace ToDo.Application.Commands.DeleteTodoCommand;

public class DeleteTodoCommand : IRequest<bool>
{
    public Guid Id { get; set; }
}
