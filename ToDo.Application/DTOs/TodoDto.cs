namespace ToDo.Application.DTOs
{
    public record TodoDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = default!;
        public string? Description { get; set; }
        public DateTime ExpiryDate { get; set; }
        public int PercentComplete { get; set; }
        public bool IsDone { get; set; }
    }
}
