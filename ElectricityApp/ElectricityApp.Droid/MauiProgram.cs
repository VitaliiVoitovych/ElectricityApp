﻿using ElectricityApp.Controls;
using ElectricityApp.Droid.Handlers;
using Microsoft.Maui.Handlers;

namespace ElectricityApp.Droid;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        AddControlsHandlers();

        builder
            .ConfigureMauiHandlers(handlers =>
            {
                handlers.AddHandler<StepperWithInput, StepperWithInputHandler>();
            })
            .UseSharedMauiApp();

        return builder.Build();
    }

    private static void AddControlsHandlers()
    {
        EntryHandler.Mapper.AppendToMapping(nameof(Entry), (handler, view) =>
        {
            handler.PlatformView.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(Android.Graphics.Color.ParseColor("#abb0b3"));
        });

        PickerHandler.Mapper.AppendToMapping(nameof(Picker), (handler, view) =>
        {
            handler.PlatformView.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(Android.Graphics.Color.ParseColor("#abb0b3"));
        });
    }
}