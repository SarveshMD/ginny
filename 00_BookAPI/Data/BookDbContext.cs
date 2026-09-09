using Microsoft.EntityFrameworkCore;
using _00_BookAPI.Models;

namespace _00_BookAPI.Data;

public class BookDbContext : DbContext
{
    public BookDbContext(DbContextOptions<BookDbContext> options) : base(options)
    {
    }

    public DbSet<Book> Books => Set<Book>();
}
