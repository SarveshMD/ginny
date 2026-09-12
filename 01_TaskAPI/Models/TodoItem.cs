namespace _01_TaskAPI.Models;

class TodoItem
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";
    public DateTime? DueAt { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime CreatedAt { get; set; }

    public TodoItem(int id, string title, string description = "", DateTime? dueAt = null)
    {
        Id = id;
        Title = title;
        Description = description;
        DueAt = dueAt;
        IsCompleted = false;
        CreatedAt = DateTime.UtcNow;
    }
}
