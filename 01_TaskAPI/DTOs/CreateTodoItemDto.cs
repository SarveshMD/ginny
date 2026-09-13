namespace _01_TaskAPI.DTOs;

public record CreateTodoItemDto(int Id, string Title, string Description, DateTimeOffset? DueAt);
