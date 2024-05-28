using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using DocumentFormat.OpenXml.Wordprocessing;
using Examination.Application.Commands.V1.StarExam;
using Examination.Application.Mapping;
using Examination.Application.Queries.V1.GetHomeExamList;
using Examination.Domain.AggregateModels.ExamAggregate;
using Examination.Domain.AggregateModels.ExamResultAggregate;
using Examination.Domain.AggregateModels.QuestionAggregate;
using Examination.Domain.AggregateModels.UserAggregate;
using Examination.Infrastructure.Repositories;
using Examination.Infrastructure.SeedWork;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using Serilog;
using System.Net.NetworkInformation;
using System.Reflection;



string appName = Assembly.GetEntryAssembly().GetName().Name;
try
{

   

    var builder = WebApplication.CreateBuilder(args);
    
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
        var user = builder.Configuration.GetValue<string>("DatabaseSettings:User");
        var password = builder.Configuration.GetValue<string>("DatabaseSettings:Password");
        var server = builder.Configuration.GetValue<string>("DatabaseSettings:Server");
        var databaseName = builder.Configuration.GetValue<string>("DatabaseSettings:DatabaseName");
        return new MongoClient(
            "mongodb://" + user + ":" + password + "@" + server + "/" + databaseName + "?authSource=admin");
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
    });
    builder.Services.AddScoped(c => c.GetRequiredService<IMongoClient>().StartSession());
    builder.Services.Configure<ExamSettings>(builder.Configuration);
    //Register Repositories
    builder.Services.AddTransient<IExamRepository, ExamRepository>();
    builder.Services.AddTransient<IExamResultRepository, ExamResultRepository>();
    builder.Services.AddTransient<IQuestionRepository, QuestionRepository>();
    builder.Services.AddTransient<IUserRepository, UserRepository>();

    Log.Information("Apply configuration web host ({ApplicationContext})...", appName);

    var app = builder.Build();
 
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

    app.UseHttpsRedirection();
    app.UseCors("CorsPolicy");
    app.UseAuthorization();

    var summaries = new[]
    {
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};
    app.MapControllers();

    app.Run();
   
    return 0;
}
catch (Exception ex)
{
    Log.Fatal(ex, "Program terminated unexpectedly ({ApplicationContext})!", appName);

    return 1;
}


