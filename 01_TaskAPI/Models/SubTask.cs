
namespace _01_TaskAPI.Models;

public class SubTask
{
    public Guid Id { get; set; }
    public string Title { get; set; } = "";
    public bool IsCompleted { get; set; }

    public Guid TodoItemId { get; set; } // Foreign Key

    public TodoItem TodoItem { get; set; } = null!; // Navigation

}
