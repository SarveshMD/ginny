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

app.MapGet("/tasks", async (TodoItemDbContext db) =>
{
    var tasks = await db.Todos
        .AsNoTracking()
        .ToListAsync();

    return Results.Ok(tasks
        .Select(task => ResponseTodoItemDto.FromEntity(task))
        .ToList()
    );
});

app.MapGet("/tasks/{id}", async (int id, TodoItemDbContext db) =>
{
    var res = await db.Todos
        .AsNoTracking()
        .Where(todo => todo.Id == id)
        .Select(todo => ResponseTodoItemDto.FromEntity(todo))
        .FirstOrDefaultAsync();

    return (res is null)
        ? Results.NotFound()
        : Results.Ok(res);
});

app.MapPost("/tasks", async (
    CreateTodoItemDto todoItemDto,
    TodoItemDbContext db,
    IValidator<CreateTodoItemDto> validator) =>
{
    var validationResult = await validator.ValidateAsync(todoItemDto);

    if (!validationResult.IsValid)
    {
        return Results.ValidationProblem(validationResult.ToDictionary());
    }

    var newTodoItem = new TodoItem(
        title: todoItemDto.Title,
        description: todoItemDto.Description,
        dueAt: todoItemDto.DueAt
    );

    db.Todos.Add(newTodoItem);
    await db.SaveChangesAsync();

    return Results.Created(
        $"/tasks/{newTodoItem.Id}",
        ResponseTodoItemDto.FromEntity(newTodoItem)
    );
});

app.MapPut("/tasks/{id}", async (
    int id,
    TodoItemDbContext db,
    IValidator<PutTodoItemDto> validator,
    PutTodoItemDto todoItemDto) =>
{
    var oldTodo = await db.Todos.FindAsync(id);

    if (oldTodo is null)
    {
        return Results.NotFound();
    }

    var validationResult = await validator.ValidateAsync(todoItemDto);
    if (!validationResult.IsValid)
    {
        return Results.ValidationProblem(validationResult.ToDictionary());
    }

    oldTodo.Title = todoItemDto.Title;
    oldTodo.Description = todoItemDto.Description;
    oldTodo.DueAt = todoItemDto.DueAt?.ToUniversalTime();
    oldTodo.IsCompleted = todoItemDto.IsCompleted;

    await db.SaveChangesAsync();

    return Results.NoContent();
});

app.MapDelete("/tasks/{id}", async (
    int id,
    TodoItemDbContext db) =>
{
    var todoItem = await db.Todos.FindAsync(id);
    if (todoItem is null)
    {
        return Results.NotFound();
    }

    db.Todos.Remove(todoItem);
    await db.SaveChangesAsync();

    return Results.NoContent();
});

app.Run();
