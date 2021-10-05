using MauiPranksterApp.Shared.Services;
using Microsoft.AspNetCore.Components.WebView.Maui;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Controls.Xaml;
using Microsoft.Maui.Hosting;
using Microsoft.Extensions.Http;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;

[assembly: XamlCompilationAttribute(XamlCompilationOptions.Compile)]

namespace MauiPranksterApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .RegisterBlazorMauiWebView()
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                })
                .Services.AddHttpClient<CustomHttpClient>()
                .Services.AddBlazorWebView()

                .AddBlazoredLocalStorage()
                .AddOptions()
                .AddAuthorizationCore()
                .AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>()
                .AddScoped<BlazorTransitionableRoute.IRouteTransitionInvoker, BlazorTransitionableRoute.DefaultRouteTransitionInvoker>();


            return builder.Build();
        }
    }
}