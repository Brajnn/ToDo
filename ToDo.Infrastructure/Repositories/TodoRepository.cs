using ToDo.Domain.Entities;
using ToDo.Domain.Interafaces;
using ToDo.Infrastructure.Presistence;
using Microsoft.EntityFrameworkCore;
namespace ToDo.Infrastructure.Repositories;

public class TodoRepository(ToDoDbContext context) : ITodoRepository
{
    public async Task<IEnumerable<Todo>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await context.Todos.ToListAsync(cancellationToken);
    }

    public async Task<Todo?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Todos.FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task<IEnumerable<Todo>> GetIncomingAsync(DateTime from, DateTime to, CancellationToken cancellationToken = default)
    {
        return await context.Todos
            .Where(t => t.ExpiryDate >= from && t.ExpiryDate <= to)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Todo todo, CancellationToken cancellationToken = default)
    {
        await context.Todos.AddAsync(todo, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Todo todo, CancellationToken cancellationToken = default)
    {
        context.Todos.Update(todo);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var todo = await context.Todos.FindAsync(new object[] { id }, cancellationToken);
        if (todo != null)
        {
            context.Todos.Remove(todo);
            await context.SaveChangesAsync(cancellationToken);
        }
    }
}