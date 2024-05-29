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
using Examination.Infrastructure.SeedWork;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using System.Text.Json;
using System.Net.Mime;
using DocumentFormat.OpenXml.Wordprocessing;

var appName = Assembly.GetEntryAssembly().GetName().Name;
try
{
   
    var builder = WebApplication.CreateBuilder(args);
    var connectionString = builder.Configuration.GetConnectionString("AppDbContext");
 
    Log.Logger = new LoggerConfiguration()
        .Enrich.WithProperty("ApplicationContext", appName)
        .MinimumLevel.Information()
        .Enrich.FromLogContext()
        .WriteTo.Console()
        .ReadFrom.Configuration(builder.Configuration)
        .CreateLogger();
    Log.Information("Configuring web host ({ApplicationContext}) ...",appName);
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

    //Health check
    builder.Services.AddHealthChecks()
            .AddCheck("self", () => HealthCheckResult.Healthy())
            .AddSqlServer(
                        connectionString,
                        name: "IdentityDB-check",
                        tags: new string[] { "identitydb" });


    //Health check ui
    builder.Services.AddHealthChecksUI(opt =>
    {
        opt.SetEvaluationTimeInSeconds(15); //time in seconds between check
        opt.MaximumHistoryEntriesPerEndpoint(60); //maximum history of checks
        opt.SetApiMaxActiveRequests(1); //api requests concurrency

        opt.AddHealthCheckEndpoint("Identity API", "/hc"); //map health check api

    })
                    .AddInMemoryStorage();

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

    Log.Information("Applying migrations ({ApplicationContext})...",appName);
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

    app.MapHealthChecks("/hc", new HealthCheckOptions
    {
        Predicate = _ => true,
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });
    app.MapHealthChecks("/liveness", new HealthCheckOptions
    {
        Predicate = r => r.Name.Contains("self")
    });

    app.MapHealthChecks("/hc-details",
                            new HealthCheckOptions
                            {
                                ResponseWriter = async (context, report) =>
                                {
                                    var result = JsonSerializer.Serialize(
                                        new
                                        {
                                            status = report.Status.ToString(),
                                            monitors = report.Entries.Select(e => new { key = e.Key, value = Enum.GetName(typeof(HealthStatus), e.Value.Status) })
                                        });
                                    context.Response.ContentType = MediaTypeNames.Application.Json;
                                    await context.Response.WriteAsync(result);
                                }
                            }
                        );
    app.MapHealthChecksUI(opt => opt.UIPath = "/hc-ui");





    Log.Information("Starting web host ({ApplicationContext}) ...",appName);
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


