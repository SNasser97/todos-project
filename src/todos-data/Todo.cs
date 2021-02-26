using System;

namespace todos_data
{
  public class Todo
  {
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; }
    public bool IsComplete { get; set; } = false;
    // Assume that Creation date is stored as a unix timestamp.
    public long CreatedAt { get; set; }
    public long UpdatedAt { get; set; }
  }
}