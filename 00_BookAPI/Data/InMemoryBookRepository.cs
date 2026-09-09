using _00_BookAPI.Models;

namespace _00_BookAPI.Data;

public class InMemoryBookRepository : IBookRepository
{

    private readonly List<Book> _books = new();

    public InMemoryBookRepository()
    {
        _books.Add(new Book(1, "Paper Towns", "John Green", 2008));
        _books.Add(new Book(2, "Six of Crows", "Leigh Bardugo", 2015));
    }

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
