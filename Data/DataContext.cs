using FinalProject.Model;
using FinalProject.Model.Relations;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Data;
public class DataContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<Piece> Pieces { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<Like> Likes { get; set; }
    public DbSet<ReviewTag> ReviewTags { get; set; }
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Review>()
            .HasOne(r => r.Piece)
            .WithMany(p => p.Reviews)
            .HasForeignKey(r => r.PieceId);

        modelBuilder.Entity<ReviewTag>()
            .HasKey(rt => new { rt.ReviewId, rt.TagId });

        modelBuilder.Entity<ReviewTag>()
            .HasOne(rt => rt.Review)
            .WithMany(r => r.ReviewTags)
            .HasForeignKey(rt => rt.ReviewId);

        modelBuilder.Entity<ReviewTag>()
            .HasOne(rt => rt.Tag)
            .WithMany(t => t.ReviewTags)
            .HasForeignKey(rt => rt.TagId);

        modelBuilder.Entity<Review>()
            .HasOne(r => r.User)
            .WithMany(u => u.Reviews)
            .HasForeignKey(r => r.UserId);

        modelBuilder.Entity<Like>()
            .HasOne(l => l.User)
            .WithMany(u => u.Likes)
            .HasForeignKey(l => l.UserId)
            .OnDelete(DeleteBehavior.Restrict); 

        modelBuilder.Entity<Like>()
            .HasOne(l => l.Review)
            .WithMany(r => r.Likes)
            .HasForeignKey(l => l.ReviewId)
            .OnDelete(DeleteBehavior.Restrict); 
    }
}
