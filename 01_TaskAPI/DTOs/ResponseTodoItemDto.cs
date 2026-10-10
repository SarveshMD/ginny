using _01_TaskAPI.Models;

namespace _01_TaskAPI.DTOs;

public record ResponseTodoItemDto(
    Guid Id,
    string Title,
    string Description,
    DateTimeOffset? DueAt,
    bool IsCompleted,
    Guid userId
)
{
    public static ResponseTodoItemDto FromEntity(TodoItem todoItem) =>
        new ResponseTodoItemDto(
            todoItem.Id,
            todoItem.Title,
            todoItem.Description,
            todoItem.DueAt,
            todoItem.IsCompleted,
            todoItem.UserId
        );
}
