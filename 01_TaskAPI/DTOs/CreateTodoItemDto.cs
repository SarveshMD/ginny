namespace _01_TaskAPI.DTOs;

public record CreateTodoItemDto(
    string Title,
    string Description,
    DateTimeOffset? DueAt
);
