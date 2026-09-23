using _01_TaskAPI.Models;

namespace _01_TaskAPI.DTOs;

public record ResponseTodoItemDto(
    int Id,
    string Title,
    string Description,
    DateTimeOffset? DueAt,
    bool IsCompleted
)
{
    public static ResponseTodoItemDto FromEntity(TodoItem todoItem) =>
        new ResponseTodoItemDto(
            todoItem.Id,
            todoItem.Title,
            todoItem.Description,
            todoItem.DueAt,
            todoItem.IsCompleted
        );
}
