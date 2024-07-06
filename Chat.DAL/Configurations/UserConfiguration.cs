using Chat.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Chat.DAL.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(r => r.Id);
        
        builder.HasMany(u => u.JoinedRooms)
            .WithMany(r => r.Users)
            .UsingEntity(
                "UsersRooms",
                j => j
                    .HasOne(typeof(Room))
                    .WithMany()
                    .HasForeignKey("RoomId")
                    .OnDelete(DeleteBehavior.Cascade), // Изменение поведения удаления
                j => j
                    .HasOne(typeof(User))
                    .WithMany()
                    .HasForeignKey("UserId")
                    .OnDelete(DeleteBehavior.Restrict));

        builder.HasMany(r => r.CreatedRooms)
            .WithOne(r => r.Creator)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasMany(r => r.Messages)
            .WithOne(r => r.User)
            .OnDelete(DeleteBehavior.Restrict);
    }
}