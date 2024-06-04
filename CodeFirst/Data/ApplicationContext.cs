using CodeFirst.Models;
using Microsoft.EntityFrameworkCore;

namespace CodeFirst.Data;

public class ApplicationContext : DbContext
{
    protected ApplicationContext()
    {
    }

    public ApplicationContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Author> Authors { get; set; }
    public DbSet<Book> Books { get; set; }
    public DbSet<Edition> Editions { get; set; }
    public DbSet<Award> Awards { get; set; }
    public DbSet<BookAward> BookAwards { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Author>().HasData(new List<Author>()
        {
            new() { Id = 1, FirstName = "John", LastName = "Doe"},
            new() { Id = 2, FirstName = "Ann", LastName = "Smith"},
            new() { Id = 3, FirstName = "Jack", LastName = "Taylor"}
        });
    }
}