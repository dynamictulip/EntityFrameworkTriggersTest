using System.Text.Json;
using System.Text.Json.Serialization;
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

        //Configure custom json options
        builder.Services.ConfigureHttpJsonOptions(options => {
            options.SerializerOptions.WriteIndented = true;
            options.SerializerOptions.IncludeFields = true;
            options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase; //best practice for JSON
            options.SerializerOptions.ReferenceHandler = ReferenceHandler.Preserve; //stops cyclic references killing the app and gives better info
        });

        
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

        AddBlogEndpoints(app);
        AddPostEndpoints(app);
        
        app.Run();
    }

    private static void AddBlogEndpoints(WebApplication app)
    {
        app.MapGet("/blogs", async (BloggingContext db) =>
            await db.Blogs.ToListAsync())
            .WithOpenApi();

        app.MapGet("/blogs/{id}", async (int id, BloggingContext db) =>
            await db.Blogs.Include(b => b.Posts)
                   .SingleOrDefaultAsync(b => b.BlogId == id)
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
    
    private static void AddPostEndpoints(WebApplication app)
    {
        app.MapGet("/posts", async (BloggingContext db) =>
                await db.Posts.ToListAsync())
            .WithOpenApi();

        app.MapGet("/posts/{id}", async (int id, BloggingContext db) =>
            await db.Posts.FindAsync(id)
                is { } post
                ? Results.Ok(post)
                : Results.NotFound());

        app.MapPost("/posts", async (Post post, BloggingContext db) =>
        {
            db.Posts.Add(post);
            await db.SaveChangesAsync();

            return Results.Created($"/posts/{post.PostId}", post);
        });

        app.MapDelete("/posts/{id}", async (int id, BloggingContext db) =>
        {
            if (await db.Posts.FindAsync(id) is not { } post) 
                return Results.NotFound();
            
            db.Posts.Remove(post);
            await db.SaveChangesAsync();
            return Results.NoContent();
        });
    }
}