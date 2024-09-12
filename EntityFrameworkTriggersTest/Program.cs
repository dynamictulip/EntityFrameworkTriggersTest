using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkTriggersTest;

public static class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        // Add services to the container.
        builder.Services.AddAuthorization();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddHealthChecks();

        builder.Services.AddDbContext<BloggingContext>(options =>
            options.UseSqlServer("name=ConnectionStrings:BloggingDatabase"));

        var app = builder.Build();

        app.Logger.LogInformation("Plop");
        app.Logger.LogInformation("BloggingDatabase connection string is: " + app.Configuration.GetConnectionString("BloggingDatabase"));
        
        //Never do this in production code - don't want to risk multiple migrations running at the same time!!
        using (var scope = app.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<BloggingContext>();
            if (db.Database.GetPendingMigrations().Any())
            {
                db.Database.Migrate();
            }
        }

        app.UseHealthChecks("/health");
        
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        ConfigureMinimalApis(app);
        
        app.Run();
    }

    private static void ConfigureMinimalApis(WebApplication app)
    {
        app.MapGet("/blogs", async (BloggingContext db) =>
            await db.Blogs.ToListAsync())
            .WithOpenApi();

        app.MapGet("/blogs/{id}", async (int id, BloggingContext db) =>
            await db.Blogs.FindAsync(id)
                is { } blog
                ? Results.Ok(blog)
                : Results.NotFound());

        app.MapPost("/blogs", async (Blog blog, BloggingContext db) =>
        {
            db.Blogs.Add(blog);
            await db.SaveChangesAsync();

            return Results.Created($"/blogs/{blog.BlogId}", blog);
        });

        app.MapDelete("/blogs/{id}", async (int id, BloggingContext db) =>
        {
            if (await db.Blogs.FindAsync(id) is not { } blog) 
                return Results.NotFound();
            
            db.Blogs.Remove(blog);
            await db.SaveChangesAsync();
            return Results.NoContent();
        });
    }
}