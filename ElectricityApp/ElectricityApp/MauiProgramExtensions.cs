using CommunityToolkit.Maui;
using ElectricityApp.EfStructures;
using ElectricityApp.Services.Charting;
using ElectricityApp.Services.Files;
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
            })
            .ConfigureEssentials(essentials =>
            {
                essentials
                    .AddAppAction("add_record", "Додати запис", icon: "add_action")
                    .OnAppAction(HandleAppActions);
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif

        builder.Services.AddElectricityDbContext("electricity.db");

        builder.Services.AddSingleton<ChartsService>();
        builder.Services.AddSingleton<NotesService>();
        builder.Services.AddSingleton<FileService>();
        
        builder.Services.AddTransient<MainViewModel>();
        builder.Services.AddTransient<NotesViewModel>();
        builder.Services.AddTransient<AddViewModel>();

        builder.Services.AddTransient<MainPage>();
        builder.Services.AddTransient<NotesPage>();
        builder.Services.AddTransient<AddPage>();

        return builder;
    }

    private static void HandleAppActions(AppAction action)
    {
        Application.Current?.Dispatcher.Dispatch(() =>
        {
            if (action.Id != "add_record") return;
            var tabBar = Shell.Current.CurrentItem; // TabBar
            var addTab = tabBar.Items[^1]; // Add tab is last

            tabBar.CurrentItem = addTab;
        });
    }
}