using System.Text.Json;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkTriggersTest;

public class BloggingContext(DbContextOptions<BloggingContext> options) : DbContext(options)
{
    public DbSet<Blog> Blogs { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<AuditEvent> AuditEvents { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<AuditEvent>();

        entity.Property(auditEvent => auditEvent.PrimaryKeyValueAsJson)
            .HasConversion(o => JsonSerializer.Serialize(o, new JsonSerializerOptions()),
                provider => JsonSerializer.Deserialize<object>(provider, new JsonSerializerOptions()));
    }
}