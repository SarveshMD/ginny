using _00_BookAPI.Models;

namespace _00_BookAPI.Data;

public interface IBookRepository
{
    IEnumerable<Book> GetAll();
    Book? GetById(int id);
    Book Add(Book book);
    bool Update(int id, Book book);
    bool Delete(int id);
}
