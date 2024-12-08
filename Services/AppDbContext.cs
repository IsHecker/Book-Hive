using BookHive.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace BookHive.Services;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Book> Books => Set<Book>();
    public DbSet<User> Users => Set<User>();
    public DbSet<ActivityLog> ActivityLogs => Set<ActivityLog>();
    public DbSet<Comment> Comments => Set<Comment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Comments: Set foreign key behavior to restrict or set null
        modelBuilder.Entity<Comment>()
            .HasOne(c => c.User)
            .WithMany()
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.SetNull); // Set UserId to null on user deletion

        // Books: Set foreign key behavior (optional, depending on requirements)
        modelBuilder.Entity<Book>()
            .HasOne(b => b.BorrowedBy)
            .WithMany()
            .HasForeignKey(b => b.BorrowedById)
            .OnDelete(DeleteBehavior.SetNull); // Set BorrowedById to null on user deletion
    }
}