using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System.Net.Http.Json;

namespace ToDo.PerformanceTests;

public class TodoApiBenchmark
{
    private readonly HttpClient _client;
    private Guid _todoId;

    public TodoApiBenchmark()
    {
        _client = new HttpClient { BaseAddress = new Uri("http://localhost:8080") };
    }

    [GlobalSetup]
    public async Task Setup()
    {
        var todo = new
        {
            Title = "Perf Todo",
            Description = "Perf description",
            ExpiryDate = DateTime.UtcNow.AddDays(1)
        };

        var response = await _client.PostAsJsonAsync("/api/todo", todo);
        response.EnsureSuccessStatusCode();
        var location = response.Headers.Location!.ToString();
        _todoId = Guid.Parse(location.Split('/').Last());
    }

    [Benchmark]
    public async Task GetAllTodos()
    {
        var response = await _client.GetAsync("/api/todo");
        response.EnsureSuccessStatusCode();
    }

    [Benchmark]
    public async Task GetTodoById()
    {
        var response = await _client.GetAsync($"/api/todo/{_todoId}");
        response.EnsureSuccessStatusCode();
    }

    [Benchmark]
    public async Task GetIncomingTodos()
    {
        var response = await _client.GetAsync("/api/todo/incoming?range=today");
        response.EnsureSuccessStatusCode();
    }

    [Benchmark]
    public async Task CreateTodo()
    {
        var todo = new
        {
            Title = "Benchmark Create",
            Description = "Testing create",
            ExpiryDate = DateTime.UtcNow.AddDays(2)
        };

        var response = await _client.PostAsJsonAsync("/api/todo", todo);
        response.EnsureSuccessStatusCode();
    }

    [Benchmark]
    public async Task UpdateTodo()
    {
        var updatedTodo = new
        {
            Id = _todoId,
            Title = "Updated Title",
            Description = "Updated Description",
            ExpiryDate = DateTime.UtcNow.AddDays(3)
        };

        var response = await _client.PutAsJsonAsync($"/api/todo/{_todoId}", updatedTodo);
        response.EnsureSuccessStatusCode();
    }

    [Benchmark]
    public async Task SetTodoPercent()
    {
        var percentContent = JsonContent.Create(80);
        var response = await _client.PatchAsync($"/api/todo/{_todoId}/percent", percentContent);
        response.EnsureSuccessStatusCode();
    }

    [Benchmark]
    public async Task MarkTodoAsDone()
    {
        var response = await _client.PatchAsync($"/api/todo/{_todoId}/done", null);
        response.EnsureSuccessStatusCode();
    }

    [Benchmark]
    public async Task DeleteTodo()
    {

        var createRequest = new HttpRequestMessage(HttpMethod.Post, "api/todo")
        {
            Content = JsonContent.Create(new
            {
                title = "Benchmark Delete",
                description = "Temporary todo for benchmark",
                expiryDate = DateTime.UtcNow.AddDays(1)
            })
        };

        var createResponse = await _client.SendAsync(createRequest);
        createResponse.EnsureSuccessStatusCode();

        var createdId = await createResponse.Content.ReadFromJsonAsync<Guid>();

        var deleteRequest = new HttpRequestMessage(HttpMethod.Delete, $"api/todo/{createdId}");
        var deleteResponse = await _client.SendAsync(deleteRequest);
        deleteResponse.EnsureSuccessStatusCode();
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        var summary = BenchmarkRunner.Run<TodoApiBenchmark>();
    }
}
