using _01_TaskAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace _01_TaskAPI.Data;

class TodoItemDbContext : DbContext
{
    public TodoItemDbContext(DbContextOptions<TodoItemDbContext> options) : base(options)
    {
    }

    public DbSet<TodoItem> Todos => Set<TodoItem>();
}
