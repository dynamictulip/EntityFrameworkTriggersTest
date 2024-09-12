using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkTriggersTest;

internal class Program
{
    private static void Main()
    {
        var dbConnectionString = Environment.GetEnvironmentVariable("dbConnectionString") ?? string.Empty;

        Console.WriteLine("Super spike initiating");
        Console.WriteLine($"DB connection string is \"{dbConnectionString}\"");

        var bloggingContext = new BloggingContext(dbConnectionString);
        

        //Never do this in production code - don't want to risk multiple migrations running at the same time!!
        bloggingContext.Database.Migrate();
        
        
    }
}

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

public class Blog
{
    public int BlogId { get; set; }
    public string Url { get; set; }

    public List<Post> Posts { get; } = new();
}

public class Post
{
    public int PostId { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }

    public int BlogId { get; set; }
    public Blog Blog { get; set; }
}