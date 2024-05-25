using Identity.API;
using Identity.API.Database;
using Identity.API.Database.Configuration;
using Identity.API.Extensions;
using Identity.API.Services;
using IdentityServer4.AspNetIdentity;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Internal;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Serilog;
using System;
using System.Reflection;
using IdentityServer4.EntityFramework.Storage;


try
{
   
    var builder = WebApplication.CreateBuilder(args);
    var connectionString = builder.Configuration.GetConnectionString("AppDbContext");
    var migrationsAssembly = typeof(Program).GetType().Assembly.GetName().Name;
    

    Log.Logger = new LoggerConfiguration()
        .MinimumLevel.Information()
        .Enrich.FromLogContext()
        .WriteTo.Console()
        .ReadFrom.Configuration(builder.Configuration)
        .CreateLogger();
    Log.Information("Configuring web host ...");
    builder.Host.UseSerilog();
    // Add services to the container.
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddControllers();
    builder.Services.AddControllersWithViews();
    builder.Services.AddRazorPages();
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "Identity.API",
            Version = "v1"
        });
    });

    //Register Dbcontext
    builder.Services.AddDbContext<AppDbContext>(options =>
    {
        options.UseSqlServer(connectionString);

    });
    //Register Identity
    builder.Services.AddIdentity<AppUser, IdentityRole>()
        .AddEntityFrameworkStores<AppDbContext>()
        .AddDefaultTokenProviders();
    //Register Identity Server

    builder.Services.AddIdentityServer(x =>
    {
        x.IssuerUri = "";
        x.Authentication.CookieLifetime = TimeSpan.FromHours(2);
    })
               .AddDeveloperSigningCredential()
               .AddAspNetIdentity<AppUser>()
               .AddConfigurationStore(options =>
               {
                   options.ConfigureDbContext = builder => builder.UseSqlServer(connectionString);
               })
               .AddOperationalStore(options =>
               {
                   options.ConfigureDbContext = builder => builder.UseSqlServer(connectionString);
               })
               .Services.AddTransient<IProfileService, ProfileService>();
    builder.Services.Configure<AppSettings>(builder.Configuration);
    builder.Services.AddSingleton<AppDbContextSeed>();
    builder.Services.AddSingleton<ConfigurationDbContextSeed>();
    var app = builder.Build();

    

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Identity.API v1"));
    }

    app.UseHttpsRedirection();
    app.UseRouting();
    app.UseAuthentication();
    app.UseIdentityServer();
    app.UseAuthorization();

    Log.Information("Applying migrations...");
    app.MigrateDbContext<PersistedGrantDbContext>((_, __) => { })
        .MigrateDbContext<AppDbContext>((context, services) =>
        {
            var env = services.GetService<IWebHostEnvironment>();
            var logger = services.GetService<ILogger<AppDbContextSeed>>();
            var settings = services.GetService<IOptions<AppSettings>>();

            new AppDbContextSeed()
            .SeedAsync(context, env, logger, settings)
            .Wait();
        })
        .MigrateDbContext<ConfigurationDbContext>((context, services) =>
        {

            new ConfigurationDbContextSeed()

            .SeedAsync(context, app.Configuration)
            .Wait();
        });



    var summaries = new[]
    {
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

    app.MapGet("/weatherforecast", () =>
    {
        var forecast = Enumerable.Range(1, 5).Select(index =>
            new WeatherForecast
            (
                DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                Random.Shared.Next(-20, 55),
                summaries[Random.Shared.Next(summaries.Length)]
            ))
            .ToArray();
        return forecast;
    })
    .WithName("GetWeatherForecast")
    .WithOpenApi();
    Log.Information("Starting web host ...");
    app.Run();
    return 0;
}
catch (Exception ex)
{
    Log.Fatal(ex, "Program terminated unexpectedly!");
    return 1;
}
finally
{
    Log.CloseAndFlush();
}


record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
