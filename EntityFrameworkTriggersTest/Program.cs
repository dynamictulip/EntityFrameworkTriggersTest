namespace EntityFrameworkTriggersTest;

internal class Program
{
    private static void Main()
    {
        var dbConnectionString = Environment.GetEnvironmentVariable("dbConnectionString") ?? string.Empty;

        Console.WriteLine("Super spike initiating");
        Console.WriteLine($"DB connection string is \"{dbConnectionString}\"");
        
        
    }
}