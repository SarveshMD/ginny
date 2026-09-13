using _01_TaskAPI.Models;
using _01_TaskAPI.DTOs;
using _01_TaskAPI.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddValidatorsFromAssemblyContaining<Program>();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<TodoItemDbContext>(
    optionsBuilder => optionsBuilder
        .UseNpgsql(connectionString)
        .UseSnakeCaseNamingConvention()
);

var app = builder.Build();

app.MapGet("/", () => "u + me = <3");

app.MapGet("/tasks", (TodoItemDbContext db) =>
{
    return Results.Ok(db.Todos.ToList());
});

app.MapPost("/tasks", (
    CreateTodoItemDto todoItemDto,
    TodoItemDbContext db,
    IValidator<CreateTodoItemDto> validator) =>
{
    var validationResult = validator.Validate(todoItemDto);

    if (!validationResult.IsValid)
    {
        return Results.ValidationProblem(validationResult.ToDictionary());
    }

    var newTodoItem = new TodoItem(
        0,
        title: todoItemDto.Title,
        description: todoItemDto.Description,
        dueAt: todoItemDto.DueAt
    );

    db.Todos.Add(newTodoItem);
    db.SaveChanges();
    return Results.Created($"/tasks/{newTodoItem.Id}", newTodoItem);
});

app.Run();
