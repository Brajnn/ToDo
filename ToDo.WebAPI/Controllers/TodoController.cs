using MediatR;
using Microsoft.AspNetCore.Mvc;
using ToDo.Application.Commands.CreateTodoCommand;
using ToDo.Application.Commands.DeleteTodoCommand;
using ToDo.Application.Commands.MarkTodoAsDoneCommand;
using ToDo.Application.Commands.SetTodoPercentCommand;
using ToDo.Application.Commands.UpdateTodoCommand;
using ToDo.Application.Queries.GetAllTodosQuery;
using ToDo.Application.Queries.GetIncomingTodosQuery;
using ToDo.Application.Queries.GetTodoByIdQuery;

namespace ToDo.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TodoController(IMediator mediator) : ControllerBase
{

    // GET: api/todo
    // Returns a list of all ToDo items
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var todos = await mediator.Send(new GetAllTodosQuery());
        return Ok(todos);
    }
    // GET: api/todo/{id}
    // Returns a specific ToDo by ID
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var todo = await mediator.Send(new GetTodoByIdQuery { Id = id });
        if (todo == null)
            return NotFound();

        return Ok(todo);
    }
    // POST: api/todo
    // Creates a new ToDo item
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTodoCommand command)
    {
        var id = await mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id }, id);
    }
    // GET: api/todo/incoming?range=today
    // Returns ToDo items filtered by time range: today, tomorrow, or week
    [HttpGet("incoming")]
    public async Task<IActionResult> GetIncoming([FromQuery] string range = "today")
    {
        var result = await mediator.Send(new GetIncomingTodosQuery { Range = range });
        return Ok(result);
    }
    // PUT: api/todo/{id}
    // Updates an existing ToDo item
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateTodoCommand command)
    {
        if (id != command.Id)
            return BadRequest("ID mismatch");

        var updated = await mediator.Send(command);
        return updated ? NoContent() : NotFound();
    }
    // PATCH: api/todo/{id}/percent
    // Sets the percent completion value for a ToDo
    [HttpPatch("{id}/percent")]
    public async Task<IActionResult> SetPercent(Guid id, [FromBody] int percent)
    {
        var command = new SetTodoPercentCommand { Id = id, PercentComplete = percent };
        var result = await mediator.Send(command);

        return result ? NoContent() : NotFound();
    }
    // DELETE: api/todo/{id}
    // Deletes a ToDo item
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await mediator.Send(new DeleteTodoCommand { Id = id });
        return result ? NoContent() : NotFound();
    }
    // PATCH: api/todo/{id}/done
    // Marks a ToDo as done
    [HttpPatch("{id}/done")]
    public async Task<IActionResult> MarkAsDone(Guid id)
    {
        var result = await mediator.Send(new MarkTodoAsDoneCommand { Id = id });
        return result ? NoContent() : NotFound();
    }
}