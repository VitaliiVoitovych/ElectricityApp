using CommunityToolkit.Maui;
using ElectricityApp.EfStructures;
using ElectricityApp.Pages;
using ElectricityApp.Services;
using ElectricityApp.ViewModels;
using Microsoft.Extensions.Logging;
using SkiaSharp.Views.Maui.Controls.Hosting;

namespace ElectricityApp;

public static class MauiProgramExtensions
{
    public static MauiAppBuilder UseSharedMauiApp(this MauiAppBuilder builder)
    {
        builder
            .UseSkiaSharp(true)
            .UseMauiCommunityToolkit()
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });
        
#if DEBUG
        builder.Logging.AddDebug();
#endif

        builder.Services.AddElectricityDbContext("electricity.db");

        builder.Services.AddSingleton<ChartsService>();
        builder.Services.AddSingleton<NotesService>();
        
        builder.Services.AddTransient<MainViewModel>();
        builder.Services.AddTransient<NotesViewModel>();
        builder.Services.AddTransient<AddViewModel>();

        builder.Services.AddTransient<MainPage>();
        builder.Services.AddTransient<NotesPage>();
        builder.Services.AddTransient<AddPage>();
        
        return builder;
    }
}