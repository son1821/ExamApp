using AdminApp.Core.Authentication;
using AdminApp.Services.Interfaces;
using AdminApp.Services;
using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;

namespace AdminApp
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            builder.Services.AddBlazoredSessionStorage();
            builder.Services.AddAuthorizationCore();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<AuthenticationStateProvider, ApiAuthenticationStateProvider>();

            builder.Services.AddScoped(sp => new HttpClient { 
                
                BaseAddress = new Uri(builder.Configuration["BackendApiUrl"]) 
            });

            builder.Services.AddMudServices();

            

            await builder.Build().RunAsync();
        }
    }
}
