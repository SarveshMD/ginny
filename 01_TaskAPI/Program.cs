using _01_TaskAPI.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<List<TodoItem>>();

var app = builder.Build();

app.MapGet("/", () => "u + me = <3");

app.MapGet("/tasks", (List<TodoItem> todoList) =>
{
    return todoList;
});

app.MapPost("/tasks", (TodoItem todoItem, List<TodoItem> todoList) =>
{
    todoList.Add(todoItem);
    return Results.Created($"/tasks/{todoItem.Id}", todoItem);
});

app.Run();
