namespace _01_TaskAPI.DTOs;

public record ResponseTodoItemDto(
    int Id,
    string Title,
    string Description,
    DateTimeOffset? DueAt,
    bool IsCompleted
);
