using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using DocumentFormat.OpenXml.Wordprocessing;
using Examination.API;
using Examination.API.Extensions;
using Examination.API.Filters;
using Examination.Application.Commands.V1.Exams.StarExam;
using Examination.Application.Mapping;
using Examination.Application.Queries.V1.Exams.GetHomeExamList;
using Examination.Domain.AggregateModels.CategoryAggregate;
using Examination.Domain.AggregateModels.ExamAggregate;
using Examination.Domain.AggregateModels.ExamResultAggregate;
using Examination.Domain.AggregateModels.QuestionAggregate;
using Examination.Domain.AggregateModels.UserAggregate;
using Examination.Infrastructure;
using Examination.Infrastructure.Repositories;
using Examination.Infrastructure.SeedWork;
using HealthChecks.UI.Client;
using Humanizer.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using Serilog;
using System.Net.Mime;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Text.Json;



var appName = Assembly.GetEntryAssembly().GetName().Name;
try
{

   

    var builder = WebApplication.CreateBuilder(args);

    
    var user = builder.Configuration.GetValue<string>("DatabaseSettings:User");
    var password = builder.Configuration.GetValue<string>("DatabaseSettings:Password");
    var server = builder.Configuration.GetValue<string>("DatabaseSettings:Server");
    var databaseName = builder.Configuration.GetValue<string>("DatabaseSettings:DatabaseName");
    var mongodbConnectionString = "mongodb://" + user + ":" + password + "@" + server + "/" + databaseName + "?authSource=admin";


    //Configuration Serilog
    Log.Logger = new LoggerConfiguration()
        .MinimumLevel.Information()
            .Enrich.WithProperty("ApplicationContext", appName)
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day, shared: true)
            .ReadFrom.Configuration(builder.Configuration)
            .CreateLogger();
    builder.Host.UseSerilog();
    Log.Information("Starting web host ({ApplicationContext})...", appName);


    builder.Services.AddHttpContextAccessor();

    //Register Versioning
    builder.Services.AddApiVersioning(options =>
    {
        options.ReportApiVersions = true;

    }).AddApiExplorer(options =>
    {
        options.GroupNameFormat = "'v'VVV";
        options.SubstituteApiVersionInUrl = true;
    });


    //Register Mongodb
    builder.Services.AddSingleton<IMongoClient>(c =>
    {
         return new MongoClient(mongodbConnectionString);
    });

    builder.Services.AddAutoMapper(ass =>
    {
        ass.AddProfile(new MappingProfile());
    });
    builder.Services.AddMediatR(cfg =>
    {
        cfg.RegisterServicesFromAssemblies(typeof(StartExamCommandHandler).Assembly, typeof(GetHomeExamListQueryHandler).Assembly);
    });
    builder.Services.AddControllers();
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("CorsPolicy",
            builder => builder
                .SetIsOriginAllowed((host) => true)
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());
    });

    // Add services to the container.
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "Examination.API", Version = "v1" });
        c.SwaggerDoc("v2", new OpenApiInfo { Title = "Examination.API", Version = "v2" });
        c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.OAuth2,
            Flows = new OpenApiOAuthFlows()
            {
                Implicit = new OpenApiOAuthFlow()
                {
                    AuthorizationUrl = new Uri($"{builder.Configuration.GetValue<string>("IdentityUrl")}/connect/authorize"),
                    TokenUrl = new Uri($"{builder.Configuration.GetValue<string>("IdentityUrl")}/connect/token"),
                    Scopes = new Dictionary<string, string>()
                            {
                                {"exam_api", "exam_api"},
                            }
                }
            }
        });
        c.OperationFilter<AuthorizeCheckOperationFilter>();
    });

    var identityUrl = builder.Configuration.GetValue<string>("IdentityUrl");
    //Configuration Authentication
    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(options =>
    {
        options.Authority = identityUrl;
        options.RequireHttpsMetadata = false;
        options.Audience = "exam_api";
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
        {
            ValidateIssuerSigningKey = true,
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });


    //Health check
    builder.Services.AddHealthChecks()
            .AddCheck("self", () => HealthCheckResult.Healthy())
            .AddMongoDb(mongodbConnectionString: mongodbConnectionString,
                        name: "mongo",
                        failureStatus: HealthStatus.Unhealthy);

    //Health check ui
    builder.Services.AddHealthChecksUI(opt =>
    {
            opt.SetEvaluationTimeInSeconds(15); //time in seconds between check
            opt.MaximumHistoryEntriesPerEndpoint(60); //maximum history of checks
            opt.SetApiMaxActiveRequests(1); //api requests concurrency

            opt.AddHealthCheckEndpoint("Exam API", "/hc"); //map health check api
            
    })
                    .AddInMemoryStorage();

    builder.Services.AddScoped(c => c.GetRequiredService<IMongoClient>().StartSession());
    builder.Services.Configure<ExamSettings>(builder.Configuration);
    //Register Repositories
    builder.Services.RegisterCustomServices();

    Log.Information("Apply configuration web host ({ApplicationContext})...", appName);



    var app = builder.Build();
    
    //Seeding data mongodb

    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        var logger = services.GetRequiredService<ILogger<ExamMongoDbSeeding>>();
        var settings = services.GetRequiredService<IOptions<ExamSettings>>();
        var mongoClient = services.GetRequiredService<IMongoClient>();
        new ExamMongoDbSeeding()
            .SeedAsync(mongoClient, settings, logger)
            .Wait();
    }
    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Examination.API v1");

            c.SwaggerEndpoint("/swagger/v2/swagger.json", "Examination.API v2");

        });
    }
    app.UseErrorWrapping();
    app.UseHttpsRedirection();
    app.UseAuthentication();
    app.UseCors("CorsPolicy");
    app.UseAuthorization();
    
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

    app.MapControllers();

    app.Run();
   
    return 0;
}
catch (Exception ex)
{
    Log.Fatal(ex, "Program terminated unexpectedly ({ApplicationContext})!", appName);

    return 1;
}


