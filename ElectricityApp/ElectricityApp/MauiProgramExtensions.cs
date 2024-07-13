﻿using ElectricityApp.EfStructures;
using ElectricityApp.Pages;
using ElectricityApp.Services;
using ElectricityApp.ViewModels;
using SkiaSharp.Views.Maui.Controls.Hosting;

namespace ElectricityApp;

public static class MauiProgramExtensions
{
    public static MauiAppBuilder UseSharedMauiApp(this MauiAppBuilder builder)
    {
        builder
            .UseSkiaSharp(true)
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            })
            .ConfigureEssentials(essentials =>
            {
                essentials
                    .AddAppAction("share_app", "Поділитися", icon: "qr_code")
                    .OnAppAction(HandleAppActions);
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
        builder.Services.AddSingleton<QrCodeViewModel>();

        builder.Services.AddTransient<MainPage>();
        builder.Services.AddTransient<NotesPage>();
        builder.Services.AddTransient<AddPage>();
        builder.Services.AddTransient<QrCodePage>();
        
        return builder;
    }

    private static void HandleAppActions(AppAction action)
    {
        Application.Current?.Dispatcher.Dispatch(async () =>
        {
            await Task.Delay(250);
            await Shell.Current.GoToAsync($"{nameof(QrCodePage)}", true);
        });
    }
}