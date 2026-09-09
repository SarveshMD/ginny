using _00_BookAPI.Models;

namespace _00_BookAPI.Data;

public class EfBookRepository : IBookRepository
{
    private readonly BookDbContext _context;

    public EfBookRepository(BookDbContext context)
    {
        _context = context;
    }

    public IEnumerable<Book> GetAll()
    {
        return _context.Books.ToList();
    }

    public Book? GetById(int id)
    {
        return _context.Books.Find(id);
    }

    public Book Add(Book book)
    {
        int newId = !_context.Books.Any()
            ? 1
            : _context.Books.Max(book => book.Id) + 1;

        var newBook = new Book(newId, book.Title, book.Author, book.PublishedYear);
        _context.Books.Add(newBook);
        _context.SaveChanges();
        return newBook;
    }

    public bool Update(int id, Book book)
    {
        var old_book = _context.Books.Find(id);

        if (old_book is null)
        {
            return false;
        }

        old_book.Title = book.Title;
        old_book.Author = book.Author;
        old_book.PublishedYear = book.PublishedYear;

        _context.SaveChanges();
        return true;
    }

    public bool Delete(int id)
    {
        var book = _context.Books.FirstOrDefault(book => book.Id == id);
        if (book is null) return false;

        _context.Books.Remove(book);
        _context.SaveChanges();
        return true;
    }
}
