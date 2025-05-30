using System.Reflection;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Contexts;
using DfE.FindInformationAcademiesTrusts.TestDataMigrator.Dapper;
using DfE.FindInformationAcademiesTrusts.TestDataMigrator.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DfE.FindInformationAcademiesTrusts.TestDataMigrator;

internal static class Program
{
    private static async Task Main(string[] args)
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
                services.AddSingleton<IDbConnectionFactory>(_ =>
                    new SqlConnectionFactory(config.GetConnectionString("AcademiesDb")!));

                services.AddTransient<FileParserService>();
                services.AddTransient<DataMigrationService>();
                services.AddTransient<GenericRepository>();

                services.AddDbContext<AcademiesDbContext>(c =>
                    c.UseSqlServer(
                        config.GetConnectionString("AcademiesDb"),
                        options => { options.MigrationsAssembly(migrationsAssemblyName); }));
            })
            .Build();

        using var scope = host.Services.CreateScope();

        await ApplySchemaMigrationsAsync(scope);

        var migrationService = scope.ServiceProvider.GetRequiredService<DataMigrationService>();

        await migrationService.StartMigrations();
    }

    private static async Task ApplySchemaMigrationsAsync(IServiceScope scope)
    {
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
