using MediatR;


namespace ToDo.Application.Commands.CreateTodoCommand;

public class CreateTodoCommand : IRequest<Guid>
{
    public string Title { get; set; } = default!;
    public string? Description { get; set; }
    public DateTime ExpiryDate { get; set; }
}
