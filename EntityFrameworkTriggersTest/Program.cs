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