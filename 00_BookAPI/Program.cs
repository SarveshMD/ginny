using _00_BookAPI.Data;
using _00_BookAPI.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<IBookRepository, InMemoryBookRepository>();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapGet("/books", (IBookRepository repository) => repository.GetAll());

app.MapGet("/books/{id}", (int id, IBookRepository repository) =>
{
    Book? book = repository.GetById(id);

    if (book is null)
    {
        return Results.NotFound();
    }

    return Results.Ok(book);
});

app.MapPost("/books", (Book book, IBookRepository repository) =>
{
    var newBook = repository.Add(book);

    return Results.Created($"/books/{newBook.Id}", newBook);
});

app.MapPut("/books/{id}", (int id, Book book, IBookRepository repository) =>
{
    bool updated = repository.Update(id, book);

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
