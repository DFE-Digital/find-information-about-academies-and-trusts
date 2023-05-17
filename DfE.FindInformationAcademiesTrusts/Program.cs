using DfE.FindInformationAcademiesTrusts;
using Microsoft.ApplicationInsights.Extensibility;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddScoped<AcademiesApi>();
builder.Services.AddOptions<AcademiesApiOptions>()
    .Bind(builder.Configuration.GetSection(AcademiesApiOptions.ConfigurationSection))
    .ValidateDataAnnotations()
    .ValidateOnStart();

if (builder.Environment.IsDevelopment())
{
    builder.Host.UseSerilog((context, loggerConfiguration) => loggerConfiguration
        .WriteTo.Console());
}

builder.Services.AddApplicationInsightsTelemetry();
builder.Host.UseSerilog((context, services, loggerConfiguration) => loggerConfiguration
    .WriteTo.ApplicationInsights(services.GetRequiredService<TelemetryConfiguration>(), TelemetryConverter.Traces));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
