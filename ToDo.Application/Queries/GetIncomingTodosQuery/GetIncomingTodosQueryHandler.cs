using MediatR;
using ToDo.Domain.Entities;
using ToDo.Domain.Interafaces;

namespace ToDo.Application.Queries.GetIncomingTodosQuery;

public class GetIncomingTodosQueryHandler(ITodoRepository repository) : IRequestHandler<GetIncomingTodosQuery, IEnumerable<Todo>>
{
    public async Task<IEnumerable<Todo>> Handle(GetIncomingTodosQuery request, CancellationToken cancellationToken)
    {
        var today = DateTime.UtcNow.Date;

        DateTime from, to;

        switch (request.Range.ToLower())
        {
            case "tomorrow":
                from = today.AddDays(1);
                to = today.AddDays(2).AddTicks(-1); // koniec jutrzejszego dnia
                break;
            case "week":
                from = today;
                to = today.AddDays(7).AddTicks(-1); // koniec 7. dnia
                break;
            case "today":
            default:
                from = today;
                to = today.AddDays(1).AddTicks(-1); // koniec dzisiejszego dnia
                break;
        }

        return await repository.GetIncomingAsync(from, to, cancellationToken);
    }
}