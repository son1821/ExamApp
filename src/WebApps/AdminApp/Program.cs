using AdminApp.Core.Authentication;
using AdminApp.Services.Interfaces;
using AdminApp.Services;
using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using MudBlazor;

namespace AdminApp
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");
            builder.Services.AddAutoMapper(typeof(App));
            builder.Services.AddBlazoredSessionStorage();
            builder.Services.AddAuthorizationCore();
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<IQuestionService, QuestionService>();
            builder.Services.AddScoped<IExamService, ExamService>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<AuthenticationStateProvider, ApiAuthenticationStateProvider>();

            builder.Services.AddScoped(sp => new HttpClient { 
                
                BaseAddress = new Uri(builder.Configuration["BackendApiUrl"]) 
            });

            builder.Services.AddMudServices(config =>
            {
                config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.TopCenter;

                config.SnackbarConfiguration.PreventDuplicates = false;
                config.SnackbarConfiguration.NewestOnTop = false;
                config.SnackbarConfiguration.ShowCloseIcon = true;
                config.SnackbarConfiguration.VisibleStateDuration = 10000;
                config.SnackbarConfiguration.HideTransitionDuration = 500;
                config.SnackbarConfiguration.ShowTransitionDuration = 500;
                config.SnackbarConfiguration.SnackbarVariant = Variant.Filled;
            });



            await builder.Build().RunAsync();
        }
    }
}
