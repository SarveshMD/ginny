using _00_BookAPI.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<IBookRepository, InMemoryBookRepository>();

var app = builder.Build();

var books = new List<Book> {
    new Book(1, "Paper Towns", "John Green", 2008),
    new Book(2, "Six of Crows", "Leigh Bardugo", 2015)
};

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

// TEMPORARY ENDPOINT TO CREATE TOY DATA
app.MapGet("/books/CREATE_ALL", (IBookRepository repository) =>
{
    foreach (var book in books)
    {
        repository.Add(book);
    }

    return Results.Redirect("/books");
});

app.MapDelete("/books/{id}", (int id, IBookRepository repository) =>
{
    bool deleted = repository.Delete(id);

    return deleted
        ? Results.NoContent()
        : Results.NotFound();
});

app.Run();

public interface IBookRepository
{
    IEnumerable<Book> GetAll();
    Book? GetById(int id);
    Book Add(Book book);
    bool Update(int id, Book book);
    bool Delete(int id);
}

public class InMemoryBookRepository : IBookRepository
{

    private readonly List<Book> _books = new();

    public IEnumerable<Book> GetAll()
    {
        return _books;
    }

    public Book? GetById(int id)
    {
        return _books.FirstOrDefault(book => book.Id == id);
    }

    public Book Add(Book book)
    {
        int newId = _books.Count == 0
            ? 1
            : _books.Max(book => book.Id) + 1;

        var newBook = new Book(newId, book.Title, book.Author, book.PublishedYear);
        _books.Add(newBook);
        return newBook;
    }

    public bool Update(int id, Book book)
    {
        var old_book = _books.FirstOrDefault(book => book.Id == id);

        if (old_book is null)
        {
            return false;
        }

        old_book.Title = book.Title;
        old_book.Author = book.Author;
        old_book.PublishedYear = book.PublishedYear;
        return true;
    }

    public bool Delete(int id)
    {
        var book = _books.FirstOrDefault(book => book.Id == id);
        if (book is null) return false;
        _books.Remove(book);
        return true;
    }
}
