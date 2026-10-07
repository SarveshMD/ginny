namespace _01_TaskAPI.Models;

public class TodoItem
{
    public Guid Id { get; set; }
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";
    public DateTimeOffset? DueAt { get; set; }
    public bool IsCompleted { get; set; }
    public DateTimeOffset CreatedAt { get; set; }

    public ICollection<SubTask> SubTasks { get; set; } = new List<SubTask>();

    // For EFCore to generate TodoItem and populate them with rows from PostgreSQL
    private TodoItem() { }

    // For our application endpoints
    public TodoItem(string title, DateTimeOffset? dueAt, string description)
    {
        Id = Guid.NewGuid();
        Title = title;
        Description = description ?? "";
        DueAt = dueAt?.ToUniversalTime();

        IsCompleted = false;
        CreatedAt = DateTime.UtcNow;
    }
}
