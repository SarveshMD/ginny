using _01_TaskAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class SubTaskConfiguration : IEntityTypeConfiguration<SubTask>
{
    public void Configure(EntityTypeBuilder<SubTask> builder)
    {
        builder.HasKey(item => item.Id);

        builder.Property(item => item.Id)
            .HasDefaultValueSql("gen_random_uuid()");

        builder.Property(item => item.Title)
            .HasMaxLength(256)
            .IsRequired();

        builder.Property(item => item.IsCompleted)
            .HasDefaultValue(false);

        builder.HasOne(SubTask => SubTask.TodoItem)
            .WithMany(todo => todo.SubTasks)
            .HasForeignKey(SubTask => SubTask.TodoItemId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
