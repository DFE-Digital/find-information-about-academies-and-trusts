using DfE.FindInformationAcademiesTrusts;
using Microsoft.ApplicationInsights.Extensibility;
using Serilog;

//Create logging mechanism before anything else to catch bootstrap errors
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);
    if (builder.Environment.IsLocalDevelopment())
        builder.Configuration.AddUserSecrets<Program>();

    //Reconfigure logging before proceeding so any bootstrap exceptions can be written to App Insights 
    if (builder.Environment.IsLocalDevelopment())
    {
        builder.Host.UseSerilog((context, loggerConfiguration) => loggerConfiguration
            .WriteTo.Console());
    }
    else
    {
        builder.Services.AddApplicationInsightsTelemetry();
        builder.Host.UseSerilog((context, services, loggerConfiguration) => loggerConfiguration
            .WriteTo.ApplicationInsights(services.GetRequiredService<TelemetryConfiguration>(),
                TelemetryConverter.Traces));
    }

    // Add services to the container.
    builder.Services.AddRazorPages();
    builder.Services.AddScoped<AcademiesApi>();
    builder.Services.AddOptions<AcademiesApiOptions>()
        .Bind(builder.Configuration.GetSection(AcademiesApiOptions.ConfigurationSection))
        .ValidateDataAnnotations()
        .ValidateOnStart();

    //Build and configure app
    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (!app.Environment.IsDevelopment() && !app.Environment.IsLocalDevelopment())
    {
        app.UseExceptionHandler("/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }

    app.UseHttpsRedirection();
    app.UseStaticFiles();

    app.UseSerilogRequestLogging();

    app.UseRouting();

    app.UseAuthorization();

    app.MapRazorPages();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "An unhandled exception occurred during bootstrapping");
}
finally
{
    Log.CloseAndFlush();
}
