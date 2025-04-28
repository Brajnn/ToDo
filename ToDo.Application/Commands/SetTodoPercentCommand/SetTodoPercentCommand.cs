using MediatR;


namespace ToDo.Application.Commands.SetTodoPercentCommand;

public class SetTodoPercentCommand : IRequest<bool>
{
    public Guid Id { get; set; }
    public int PercentComplete { get; set; }
}
