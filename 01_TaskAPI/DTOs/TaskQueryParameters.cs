namespace _01_TaskAPI.DTOs;

public record TaskQueryParameters(
    bool? isCompleted,
    DateTimeOffset? dueBefore,
    DateTimeOffset? dueAfter,
    int page = 1,
    int pageSize = 5,
    string sortBy = "Id",
    bool? isDescending = false
);
