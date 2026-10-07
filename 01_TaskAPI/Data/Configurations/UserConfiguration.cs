using _01_TaskAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(item => item.Id);

        builder.Property(item => item.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(item => item.Email)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(item => item.PasswordHash)
            .HasMaxLength(1000) // must change once hashing is implemented
            .IsRequired();
    }
}
