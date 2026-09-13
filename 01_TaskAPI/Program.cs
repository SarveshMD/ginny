using _01_TaskAPI.Models;
using _01_TaskAPI.DTOs;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<List<TodoItem>>();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

var app = builder.Build();

app.MapGet("/", () => "u + me = <3");

app.MapGet("/tasks", (List<TodoItem> todoList) =>
{
    return todoList;
});

app.MapPost("/tasks", (
    CreateTodoItemDto todoItemDto,
    List<TodoItem> todoList,
    IValidator<CreateTodoItemDto> validator) =>
{
    var validationResult = validator.Validate(todoItemDto);

    if (!validationResult.IsValid)
    {
        return Results.ValidationProblem(validationResult.ToDictionary());
    }

    var newTodoItem = new TodoItem(
        id: todoItemDto.Id,
        title: todoItemDto.Title,
        description: todoItemDto.Description,
        dueAt: todoItemDto.DueAt
    );

    todoList.Add(newTodoItem);
    return Results.Created($"/tasks/{newTodoItem.Id}", newTodoItem);
});

app.Run();
