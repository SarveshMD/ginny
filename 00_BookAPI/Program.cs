using FluentValidation;
using Microsoft.EntityFrameworkCore;

using _00_BookAPI.Data;
using _00_BookAPI.Models;
using _00_BookAPI.DTOs;


var builder = WebApplication.CreateBuilder(args);

// builder.Services.AddSingleton<IBookRepository, InMemoryBookRepository>();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();
builder.Services.AddDbContext<BookDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IBookRepository, EfBookRepository>();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapGet("/books", (IBookRepository repository) => repository.GetAll());

app.MapGet("/books/{id}", (int id, IBookRepository repository) =>
{
    Book? book = repository.GetById(id);

    return (book is null)
        ? Results.NotFound()
        : Results.Ok(book);
});

app.MapPost("/books", (
    CreateBookDto dto,
    IValidator<CreateBookDto> validator,
    IBookRepository repository) =>
{
    var validationResult = validator.Validate(dto);

    if (!validationResult.IsValid)
    {
        return Results.ValidationProblem(validationResult.ToDictionary());
    }

    var bookToCreate = new Book(0, dto.Title, dto.Author, dto.PublishedYear);
    var createdBook = repository.Add(bookToCreate);

    return Results.Created($"/books/{createdBook.Id}", createdBook);
});

app.MapPut("/books/{id}", (
    int id,
    CreateBookDto dto,
    IValidator<CreateBookDto> validator,
    IBookRepository repository) =>
{
    var validationResult = validator.Validate(dto);

    if (!validationResult.IsValid)
    {
        return Results.ValidationProblem(validationResult.ToDictionary());
    }

    var bookToUpdate = new Book(id, dto.Title, dto.Author, dto.PublishedYear);
    bool updated = repository.Update(id, bookToUpdate);

    return updated
        ? Results.NoContent()
        : Results.NotFound();

});

app.MapDelete("/books/{id}", (int id, IBookRepository repository) =>
{
    bool deleted = repository.Delete(id);

    return deleted
        ? Results.NoContent()
        : Results.NotFound();
});

app.Run();
