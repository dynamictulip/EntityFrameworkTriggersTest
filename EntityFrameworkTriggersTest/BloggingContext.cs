using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkTriggersTest;

public class BloggingContext : DbContext
{
    private readonly string _connectionString;
    public DbSet<Blog> Blogs { get; set; }
    public DbSet<Post> Posts { get; set; }

    //Required for ef migrations to work because there is no DI container in this app that it can reference
    public BloggingContext() { }

    public BloggingContext(string connectionString)
    {
        _connectionString = connectionString;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(_connectionString);
    }
}