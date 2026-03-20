using ChatApp.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Room> Rooms => Set<Room>();
    public DbSet<Message> Messages => Set<Message>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<User>(u =>
        {
            u.HasKey(x => x.Id);
            u.HasIndex(x => x.Username).IsUnique();
            u.Property(x => x.Username).HasMaxLength(50).IsRequired();
            u.Property(x => x.PasswordHash).IsRequired();
        });

        builder.Entity<Room>(r =>
        {
            r.HasKey(x => x.Id);
            r.Property(x => x.Name).HasMaxLength(100).IsRequired();
            r.Property(x => x.Description).HasMaxLength(500);
        });

        builder.Entity<Message>(m =>
        {
            m.HasKey(x => x.Id);
            m.Property(x => x.Content).HasMaxLength(2000).IsRequired();
            m.HasOne(x => x.Sender)
             .WithMany(u => u.Messages)
             .HasForeignKey(x => x.SenderId)
             .OnDelete(DeleteBehavior.Restrict);
            m.HasOne(x => x.Room)
             .WithMany(r => r.Messages)
             .HasForeignKey(x => x.RoomId)
             .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
