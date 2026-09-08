namespace _00_BookAPI.Models;

public class Book
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public string Author { get; set; } = "";
    public int PublishedYear { get; set; }

    public Book(int id, string title, string author, int publishedYear)
    {
        Id = id;
        Title = title;
        Author = author;
        PublishedYear = publishedYear;
    }
}
