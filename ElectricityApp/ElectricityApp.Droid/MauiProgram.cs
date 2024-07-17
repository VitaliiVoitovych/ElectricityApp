using Microsoft.Maui.Handlers;

namespace ElectricityApp.Droid;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        EntryHandler.Mapper.AppendToMapping(nameof(Entry), (handler, view) =>
        {
            handler.PlatformView.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(Android.Graphics.Color.ParseColor("#abb0b3"));
        });

        PickerHandler.Mapper.AppendToMapping(nameof(Picker), (handler, view) =>
        {
            handler.PlatformView.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(Android.Graphics.Color.ParseColor("#abb0b3"));
        });

        StepperHandler.Mapper.AppendToMapping(nameof(Stepper), (handler, view) =>
        {
            var buttonBackgroundColor = Android.Graphics.Color.ParseColor("#1d3039");
            var enabledTextColor = Android.Graphics.Color.ParseColor("#abb0b3");
            var disabledTextColor = Android.Graphics.Color.ParseColor("#696d70");

            var states = new int[][]
            {
                [Android.Resource.Attribute.StateEnabled],
                [-Android.Resource.Attribute.StateEnabled],
            };

            int[] colors = [enabledTextColor, disabledTextColor];

            var textColorList = new Android.Content.Res.ColorStateList(states, colors);

            var linearLayout = handler.PlatformView as Android.Widget.LinearLayout;
            var button1 = linearLayout.GetChildAt(0) as Android.Widget.Button;
            var button2 = linearLayout.GetChildAt(1) as Android.Widget.Button;

            button1!.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(buttonBackgroundColor);
            button2!.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(buttonBackgroundColor);

            button1!.SetTextColor(textColorList);
            button2!.SetTextColor(textColorList);
        });

        builder
            .UseSharedMauiApp();

        return builder.Build();
    }
}