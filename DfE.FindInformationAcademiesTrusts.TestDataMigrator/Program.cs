using System.Reflection;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace DfE.FindInformationAcademiesTrusts.TestDataMigrator;

internal class Program
{
    static async Task Main(string[] args)
    {
        var builder = Host.CreateDefaultBuilder(args);

        var migrationsAssemblyName = Assembly.GetExecutingAssembly().GetName().Name;


        var host = builder
            .ConfigureServices(services =>
            {
                services.AddDbContext<AcademiesDbContext>(c =>
                    c.UseSqlServer(
                        "Server=academiesdb;Database=academies;User Id=sa;Password=mySuperStrong_pa55word!!!;TrustServerCertificate=True",
                        options =>
                        {
                           options.MigrationsAssembly(migrationsAssemblyName);
                        }));
            })
            .Build();
        
        await ApplyMigrationsAsync(host);
    }
    
    private static async Task ApplyMigrationsAsync(IHost app)
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
