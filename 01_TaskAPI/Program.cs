using _01_TaskAPI.Models;
using _01_TaskAPI.DTOs;
using _01_TaskAPI.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using _01_TaskAPI.Validators;
using _01_TaskAPI.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddValidatorsFromAssemblyContaining<Program>();

builder.Services.AddScoped<ITokenService, TokenService>();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<TodoItemDbContext>(optionsBuilder =>
    {
        optionsBuilder
            .UseNpgsql(connectionString)
            .UseSnakeCaseNamingConvention();

        if (builder.Environment.IsDevelopment())
        {
            optionsBuilder
                .EnableSensitiveDataLogging()
                .LogTo(Console.WriteLine, LogLevel.Information);
        }
    }
);

var app = builder.Build();

app.Use(async (context, next) =>
{
    var sw = System.Diagnostics.Stopwatch.StartNew();
    await next(context);
    sw.Stop();

    Console.WriteLine($"Request [{context.Request.Path}] took {sw.ElapsedMilliseconds} ms");
});

app.MapGet("/", () => "u + me = <3");

app.MapGet("/tasksDeep", async (
    TodoItemDbContext db,
    CancellationToken ct) =>
{
    var tasks = await db.Todos
        .AsNoTracking()
        .Select(t => new
        {
            t.Id,
            t.Title,
            t.Description,
            t.IsCompleted,
            t.DueAt,
            t.User,
            t.UserId,
            SubTasks = t.SubTasks.Select(s => new
            {
                s.Id,
                s.Title,
                s.IsCompleted,
                s.TodoItemId
            })
        })
        .ToListAsync(ct);

    return Results.Ok(tasks);
});

app.MapGet("/tasksAll", async (
    TodoItemDbContext db,
    CancellationToken ct) =>
{
    // await Task.Delay(5000, ct);
    // Console.WriteLine("Await ran fully...");

    var res = await db.Todos
        .AsNoTracking()
        .Select(item => ResponseTodoItemDto.FromEntity(item))
        .ToListAsync(ct);

    Console.WriteLine("Response is ready...");

    return Results.Ok(res);
});

app.MapGet("/tasks", async (
    TodoItemDbContext db,
    [AsParameters] TaskQueryParameters query,
    TaskQueryParametersValidator validator) =>
{
    var validationResult = await validator.ValidateAsync(query);

    if (!validationResult.IsValid)
    {
        return Results.ValidationProblem(validationResult.ToDictionary());
    }

    var queryable = db.Todos.AsNoTracking();

    if (query.isCompleted is not null)
    {
        queryable = queryable.Where(task => task.IsCompleted == query.isCompleted);
    }

    if (query.dueBefore is not null)
    {
        queryable = queryable.Where(task => task.DueAt <= query.dueBefore);
    }

    if (query.dueAfter is not null)
    {
        queryable = queryable.Where(task => task.DueAt >= query.dueAfter);
    }

    // TODO: implement sortBy and isDescending

    var tasks = await queryable
        .OrderBy(task => task.Id)
        .Skip((query.page - 1) * query.pageSize)
        .Take(query.pageSize)
        .ToListAsync();

    return Results.Ok(tasks
        .Select(task => ResponseTodoItemDto.FromEntity(task))
    );
});

app.MapGet("/tasks/{id:guid}", async (Guid id, TodoItemDbContext db) =>
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
        id: Guid.NewGuid(),
        title: todoItemDto.Title,
        description: todoItemDto.Description,
        dueAt: todoItemDto.DueAt,
        userId: todoItemDto.UserId
    );

    db.Todos.Add(newTodoItem);
    await db.SaveChangesAsync();

    return Results.Created(
        $"/tasks/{newTodoItem.Id}",
        ResponseTodoItemDto.FromEntity(newTodoItem)
    );
});

app.MapPatch("/tasks/{id:guid}/mark", async (
    Guid id,
    TodoItemDbContext db,
    MarkTodoItemDtoValidator validator,
    MarkTodoItemDto dto) =>
{
    var todoItem = await db.Todos.FindAsync(id);

    if (todoItem is null)
    {
        return Results.NotFound();
    }

    var validationResult = await validator.ValidateAsync(dto);

    if (!validationResult.IsValid)
    {
        return Results.ValidationProblem(validationResult.ToDictionary());
    }

    todoItem.IsCompleted = dto.IsCompleted!.Value;
    await db.SaveChangesAsync();

    return Results.Ok(ResponseTodoItemDto.FromEntity(todoItem));
});


app.MapPatch("/tasks/{id:guid}/due", async (
    Guid id,
    TodoItemDbContext db,
    DueTodoItemDtoValidator validator,
    DueTodoItemDto dto) =>
{
    var todoItem = await db.Todos.FindAsync(id);

    if (todoItem is null)
    {
        return Results.NotFound();
    }

    var validationResult = await validator.ValidateAsync(dto);

    if (!validationResult.IsValid)
    {
        return Results.ValidationProblem(validationResult.ToDictionary());
    }

    todoItem.DueAt = dto.DueAt;
    await db.SaveChangesAsync();

    return Results.Ok(ResponseTodoItemDto.FromEntity(todoItem));
});

app.MapPut("/tasks/{id:guid}", async (
    Guid id,
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

app.MapDelete("/tasks/{id:guid}", async (
    Guid id,
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

app.MapPost("/api/auth/register", async (
    RegisterRequestDto dto,
    TodoItemDbContext db,
    ITokenService tokenService
) =>
{
    var normalizedEmail = dto.Email.Trim().ToLowerInvariant();

    var emailExists = await db.Users.AnyAsync(u => u.Email == normalizedEmail);
    if (emailExists)
    {
        return Results.Conflict("Email is already registered");
    }

    string passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

    var user = new User(
        id: Guid.NewGuid(),
        name: dto.Name,
        email: dto.Email,
        passwordHash: passwordHash
    );

    db.Users.Add(user);

    await db.SaveChangesAsync();

    string token = tokenService.GenerateToken(user);

    return Results.Accepted($"/users/{user.Id}", new
    {
        Message = "Registration successful",
        user.Id,
        user.Email,
        user.Name,
        Token = token
    });
});

app.MapPost("/api/auth/login", async (
    LoginRequestDto dto,
    TodoItemDbContext db,
    ITokenService tokenService
) =>
{
    var normalizedEmail = dto.Email.Trim().ToLowerInvariant();
    var user = await db.Users.FirstOrDefaultAsync(u => u.Email == normalizedEmail);

    if (user is null)
    {
        return Results.Unauthorized();
    }

    bool isPasswordValid = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);

    if (!isPasswordValid)
    {
        return Results.Unauthorized();
    }

    string token = tokenService.GenerateToken(user);

    return Results.Ok(new
    {
        Message = "Login Successful",
        user.Id,
        user.Email,
        user.Name,
        Token = token
    });
});


app.Run();
