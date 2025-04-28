using ToDo.Domain.Entities;

namespace ToDo.Domain.Interafaces;

public interface ITodoRepository
{
    Task<IEnumerable<Todo>> GetAllAsync(CancellationToken cancellationToken);
    Task<Todo?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<IEnumerable<Todo>> GetIncomingAsync(DateTime from, DateTime to, CancellationToken cancellationToken);
    Task AddAsync(Todo todo, CancellationToken cancellationToken);
    Task UpdateAsync(Todo todo, CancellationToken cancellationToken);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken);
}
