using Chat.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Chat.DAL.Configurations;

public class MessageConfiguration : IEntityTypeConfiguration<Message>
{
    public void Configure(EntityTypeBuilder<Message> builder)
    {
        builder.HasKey(r => r.Id);
        builder.HasOne(r => r.Room)
            .WithMany(r => r.Messages)
            .HasForeignKey(r => r.RoomId)
            .OnDelete(DeleteBehavior.ClientCascade);
        
        builder.HasOne(r => r.User)
            .WithMany(r => r.Messages)
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.ClientCascade);
    }
}