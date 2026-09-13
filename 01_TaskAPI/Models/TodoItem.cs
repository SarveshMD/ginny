namespace _01_TaskAPI.Models;

class TodoItem
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";
    public DateTimeOffset? DueAt { get; set; }
    public bool IsCompleted { get; set; }
    public DateTimeOffset CreatedAt { get; set; }

    public TodoItem(int id, string title, DateTimeOffset? dueAt, string description)
    {
        Id = id;
        Title = title;
        Description = description ?? "";
        DueAt = dueAt?.ToUniversalTime();

        IsCompleted = false;
        CreatedAt = DateTime.UtcNow;
    }
}
