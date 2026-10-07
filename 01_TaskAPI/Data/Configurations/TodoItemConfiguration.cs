using _01_TaskAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class TodoItemConfiguration : IEntityTypeConfiguration<TodoItem>
{
    public void Configure(EntityTypeBuilder<TodoItem> builder)
    {
        builder.HasKey(item => item.Id);

        builder.Property(item => item.Id)
            .HasDefaultValueSql("gen_random_uuid()");

        builder.Property(item => item.UserId)
            .IsRequired();

        builder.Property(item => item.Title)
            .HasMaxLength(256)
            .IsRequired();

        builder.Property(item => item.Description)
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(item => item.IsCompleted)
            .HasDefaultValue(false);

        builder.HasOne(TodoItem => TodoItem.User)
            .WithMany(user => user.TodoItems)
            .HasForeignKey(TodoItem => TodoItem.UserId);
    }
}
