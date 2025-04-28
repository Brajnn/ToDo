using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDo.Domain.Entities;

public class Todo
{
    public Guid Id { get; set; }
    public string Title { get; set; } = default!;
    public string? Description { get; set; }
    public DateTime ExpiryDate { get; set; }
    public int PercentComplete { get; set; }
    public bool IsDone { get; set; }
}
