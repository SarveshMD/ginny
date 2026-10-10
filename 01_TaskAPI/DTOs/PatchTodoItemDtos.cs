namespace _01_TaskAPI.DTOs;

public record MarkTodoItemDto(
    bool? IsCompleted
);

public record DueTodoItemDto(
    DateTimeOffset? DueAt
);
