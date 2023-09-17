using FinalProject.Model;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Data;
public class DataContext : DbContext
{
    public DbSet<User> Users { get; set; }

    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = 1,
                Name = "admin",
                UserName = "admin",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin"), 
                Email = "constant@example.com",
                Role = Role.Admin
            }
        );
    }
}
