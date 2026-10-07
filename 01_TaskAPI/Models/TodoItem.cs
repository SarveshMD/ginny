namespace _01_TaskAPI.Models;

public class TodoItem
{
    public Guid Id { get; set; }
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";
    public DateTimeOffset? DueAt { get; set; }
    public bool IsCompleted { get; set; }
    public DateTimeOffset CreatedAt { get; set; }

    public Guid UserId { get; set; }
    public User User { get; set; } = null!;

    public ICollection<SubTask> SubTasks { get; set; } = new List<SubTask>();

    public TodoItem(string title, DateTimeOffset? dueAt, string description, Guid userId)
    {
        Title = title;
        Description = description ?? "";
        DueAt = dueAt?.ToUniversalTime();

        IsCompleted = false;
        CreatedAt = DateTime.UtcNow;
        UserId = userId;
    }
}
