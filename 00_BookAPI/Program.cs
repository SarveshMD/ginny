using FluentValidation;
using Microsoft.EntityFrameworkCore;

using _00_BookAPI.Data;
using _00_BookAPI.Models;
using _00_BookAPI.DTOs;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddValidatorsFromAssemblyContaining<Program>();
builder.Services.AddDbContext<BookDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapGet("/books", (BookDbContext db) => db.Books.ToList());

app.MapGet("/books/{id}", (int id, BookDbContext db) =>
{
    Book? book = db.Books.Find(id);

    return (book is null)
        ? Results.NotFound()
        : Results.Ok(book);
});

app.MapPost("/books", (
    CreateBookDto dto,
    BookDbContext db,
    IValidator<CreateBookDto> validator) =>
{
    var validationResult = validator.Validate(dto);

    if (!validationResult.IsValid)
    {
        return Results.ValidationProblem(validationResult.ToDictionary());
    }

    var book = new Book(0, dto.Title, dto.Author, dto.PublishedYear);
    db.Books.Add(book);
    db.SaveChanges();

    return Results.Created($"/books/{book.Id}", book);
});

app.MapPut("/books/{id}", (
    int id,
    CreateBookDto dto,
    BookDbContext db,
    IValidator<CreateBookDto> validator) =>
{
    var validationResult = validator.Validate(dto);

    if (!validationResult.IsValid)
    {
        return Results.ValidationProblem(validationResult.ToDictionary());
    }

    var book = db.Books.Find(id);
    if (book is null)
    {
        return Results.NotFound();
    }
    else
    {
        book.Title = dto.Title;
        book.Author = dto.Author;
        book.PublishedYear = dto.PublishedYear;
        db.SaveChanges();
        return Results.NoContent();
    }

});

app.MapDelete("/books/{id}", (int id, BookDbContext db) =>
{
    var book = db.Books.Find(id);
    if (book is null)
    {
        return Results.NotFound();
    }
    else
    {
        db.Books.Remove(book);
        db.SaveChanges();
        return Results.NoContent();
    }

});

app.Run();
