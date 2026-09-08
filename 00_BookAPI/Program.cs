using _00_BookAPI.Models;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var books = new List<Book> {
    new Book(1, "Paper Towns", "John Green", 2008),
    new Book(2, "Six of Crows", "Leigh Bardugo", 2015)
};

app.MapGet("/", () => "Hello World!");

app.MapGet("/books", () => books);

app.MapGet("/books/{id}", (int id) =>
{
    Book? book = books.FirstOrDefault(book => book.Id == id);

    if (book is null)
    {
        return Results.NotFound();
    }

    return Results.Ok(book);
});

app.MapPost("/books", (Book book) =>
{
    books.Add(book);

    return Results.Created($"/books/{book.Id}", book);
});

app.MapPut("/books/{id}", (int id, Book newBook) =>
{
    var book = books.FirstOrDefault(book => book.Id == id);

    if (book is null)
    {
        return Results.NotFound();
    }

    book.Title = newBook.Title;
    book.Author = newBook.Author;
    book.PublishedYear = newBook.PublishedYear;

    return Results.NoContent();
});

app.MapDelete("/books/{id}", (int id) =>
{
    var book = books.FirstOrDefault(book => book.Id == id);

    if (book is null)
    {
        return Results.NotFound();
    }

    books.Remove(book);
    return Results.NoContent();
});

app.Run();
