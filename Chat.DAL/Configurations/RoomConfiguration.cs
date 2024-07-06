using Chat.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Chat.DAL.Configurations;

public class RoomConfiguration : IEntityTypeConfiguration<Room>
{
    public void Configure(EntityTypeBuilder<Room> builder)
    {
        builder.HasKey(r => r.Id);
        builder.HasOne(r => r.Creator)
            .WithMany(r => r.CreatedRooms)
            .HasForeignKey(r => r.CreatorId)
            .OnDelete(DeleteBehavior.ClientCascade);
        
        builder.HasMany(r => r.Users)
            .WithMany(u => u.JoinedRooms)
            .UsingEntity(
                "UsersRooms",
                j => j
                    .HasOne(typeof(User))
                    .WithMany()
                    .HasForeignKey("UserId")
                    .OnDelete(DeleteBehavior.Restrict), // Изменение поведения удаления
                j => j
                    .HasOne(typeof(Room))
                    .WithMany()
                    .HasForeignKey("RoomId")
                    .OnDelete(DeleteBehavior.Cascade));
        
                
        builder.HasMany(r => r.Messages)
            .WithOne(r => r.Room)
            .HasForeignKey(m => m.RoomId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}