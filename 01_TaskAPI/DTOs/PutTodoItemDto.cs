namespace _01_TaskAPI.DTOs;

public record PutTodoItemDto(
    string Title,
    string Description,
    DateTimeOffset? DueAt,
    bool IsCompleted
);
