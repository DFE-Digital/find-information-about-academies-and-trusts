using System.Reflection;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace DfE.FindInformationAcademiesTrusts.TestDataMigrator;

internal static class Program
{
    static async Task Main(string[] args)
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .AddEnvironmentVariables()
            .Build();

        var builder = Host.CreateDefaultBuilder(args);

        var migrationsAssemblyName = Assembly.GetExecutingAssembly().GetName().Name;

        var host = builder
            .ConfigureServices(services =>
            {
                services.AddDbContext<AcademiesDbContext>(c =>
                    c.UseSqlServer(
                        config.GetConnectionString("AcademiesDb"),
                        options =>
                        {
                           options.MigrationsAssembly(migrationsAssemblyName);
                        }));
            })
            .Build();
        
        await ApplySchemaMigrationsAsync(host);
    }
    
    private static async Task ApplySchemaMigrationsAsync(IHost app)
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AcademiesDbContext>();

        // Check and apply pending migrations
        var pendingMigrations = await dbContext.Database.GetPendingMigrationsAsync();
        if (pendingMigrations.Any())
        {
            Console.WriteLine("Applying pending migrations...");
            await dbContext.Database.MigrateAsync();
            Console.WriteLine("Migrations applied successfully.");
        }
        else
        {
            Console.WriteLine("No pending migrations found.");
        }
    }
}
