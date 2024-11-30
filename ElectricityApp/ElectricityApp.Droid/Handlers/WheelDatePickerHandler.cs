using Android.Views;
using Android.Widget;
using ElectricityApp.Controls;
using Microsoft.Maui.Handlers;
using MaterialAlertDialogBuilder = Google.Android.Material.Dialog.MaterialAlertDialogBuilder;
using TextInputEditText = Google.Android.Material.TextField.TextInputEditText;
using CultureInfo = System.Globalization.CultureInfo;
using ColorStateList = Android.Content.Res.ColorStateList;
using Android.Content;
using Microsoft.Maui.Platform;

// TODO: Review code: control's handler
namespace ElectricityApp.Droid.Handlers;

public class WheelDatePickerHandler : ViewHandler<WheelDatePicker, TextInputEditText>
{
    protected string? DateFormat { get; private set; }
    protected TextInputEditText TextField { get; private set; }
    public DatePickerDialogContainer DatePickerDialogContainer { get; private set; }

    public static IPropertyMapper<WheelDatePicker, WheelDatePickerHandler> PropertyMapper = new PropertyMapper<WheelDatePicker, WheelDatePickerHandler>(ViewMapper)
    {
        [nameof(WheelDatePicker.IsDayVisible)] = MapIsDayVisible, // Always First
        [nameof(WheelDatePicker.SelectedDate)] = MapSelectedDate,
        [nameof(WheelDatePicker.UnderlineColor)] = MapUnderlineColor,
        [nameof(ITextStyle.TextColor)] = MapTextColor,
        [nameof(ITextStyle.Font)] = MapFont,
    };

    private static void MapUnderlineColor(WheelDatePickerHandler handler, WheelDatePicker picker)
    {
        handler.TextField.BackgroundTintList = ColorStateList.ValueOf(picker.UnderlineColor.ToPlatform());
    }

    private static void MapTextColor(WheelDatePickerHandler handler, WheelDatePicker picker)
    {
        handler.TextField.UpdateTextColor(picker);
    }

    private static void MapFont(WheelDatePickerHandler handler, WheelDatePicker picker)
    {
        var fontManager = handler.MauiContext?.Services.GetRequiredService<IFontManager>()!;
        handler.TextField.UpdateFont(picker, fontManager);
    }

    public WheelDatePickerHandler() : base(PropertyMapper)
    {

    }

    public WheelDatePickerHandler(IPropertyMapper mapper, CommandMapper? commandMapper = null) : base(mapper, commandMapper)
    {
    }

    protected override TextInputEditText CreatePlatformView()
    {
        TextField = new TextInputEditText(Context)
        {
            InputType = Android.Text.InputTypes.Null,
            Focusable = true,
            Clickable = true,
        };

        DatePickerDialogContainer = new DatePickerDialogContainer(Context);

        return TextField;
    }

    protected override void ConnectHandler(TextInputEditText platformView)
    {
        platformView.FocusChange += PlatformView_FocusChange;
        platformView.Click += PlatformView_Click;

        base.ConnectHandler(platformView);
    }

    protected override void DisconnectHandler(TextInputEditText platformView)
    {
        platformView.FocusChange -= PlatformView_FocusChange;
        platformView.Click -= PlatformView_Click;

        base.DisconnectHandler(platformView);
    }

    private void PlatformView_FocusChange(object? sender, Android.Views.View.FocusChangeEventArgs e)
    {
        if (e.HasFocus)
        {
            if (PlatformView.Clickable)
                PlatformView.CallOnClick();
            else
                PlatformView_Click(PlatformView, EventArgs.Empty);
        }
    }

    private void PlatformView_Click(object? sender, EventArgs e)
    {
        using var builder = new MaterialAlertDialogBuilder(Context, Resource.Style.CustomMaterialDialogStyle);

        builder.SetTitle("Виберіть дату");
        builder.SetNegativeButton(Android.Resource.String.Cancel, (o, args) => { });
        builder.SetPositiveButton("Вибрати", (o, args) =>
        {
            VirtualView.SelectedDate = new DateOnly(DatePickerDialogContainer.YearPicker.Value, DatePickerDialogContainer.MonthPicker.Value, DatePickerDialogContainer.DayPicker.Value);
            PlatformView.Text = VirtualView.SelectedDate.ToString(DateFormat);
        });

        if (DatePickerDialogContainer.Parent is not null)
        {
            ((ViewGroup)DatePickerDialogContainer.Parent).RemoveView(DatePickerDialogContainer);
        }
        builder.SetView(DatePickerDialogContainer);

        var dialog = builder.Create();
        dialog.Show();
    }

    private static void MapSelectedDate(WheelDatePickerHandler handler, WheelDatePicker picker)
    {
        handler.DatePickerDialogContainer.DayPicker.Value = picker.SelectedDate.Day;
        handler.DatePickerDialogContainer.MonthPicker.Value = picker.SelectedDate.Month;
        handler.DatePickerDialogContainer.YearPicker.Value = picker.SelectedDate.Year;

        handler.PlatformView.Text = picker.SelectedDate.ToString(handler.DateFormat);
    }

    private static void MapIsDayVisible(WheelDatePickerHandler handler, WheelDatePicker picker)
    {
        handler.DatePickerDialogContainer.DayPicker.Visibility = picker.IsDayVisible ? ViewStates.Visible : ViewStates.Gone;
        handler.DateFormat = picker.IsDayVisible ? "dd MMMM yyyy" : "MMMM yyyy";
    }
}

public class DatePickerDialogContainer : LinearLayout
{
    public NumberPicker DayPicker { get; private set; }
    public NumberPicker MonthPicker { get; private set; }
    public NumberPicker YearPicker { get; private set; }

    public DatePickerDialogContainer(Context? context) : base(context)
    {
        Orientation = Orientation.Horizontal;
        SetGravity(GravityFlags.CenterHorizontal);
        SetPadding(20, 0, 20, 0);

        var layoutParams = new LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent)
        {
            LeftMargin = 20,
            RightMargin = 20,
            Weight = 1f,
        };

        DayPicker = new NumberPicker(Context)
        {
            MaxValue = 30,
            MinValue = 1,
            LayoutParameters = layoutParams
        };

        var months = CultureInfo.CurrentCulture.DateTimeFormat.MonthNames;
        MonthPicker = new NumberPicker(Context)
        {
            LayoutParameters = layoutParams,
            MinValue = 1,
            MaxValue = 12
        };
        MonthPicker.SetDisplayedValues(months);
        MonthPicker.ValueChanged += MonthPicker_ValueChanged;

        YearPicker = new NumberPicker(Context)
        {
            MinValue = DateTime.Now.Year - 10,
            MaxValue = DateTime.Now.Year,
            Value = DateTime.Now.Year,
            LayoutParameters = layoutParams
        };
        YearPicker.ValueChanged += YearPicker_ValueChanged;

        AddView(DayPicker);
        AddView(MonthPicker);
        AddView(YearPicker);
    }

    private void MonthPicker_ValueChanged(object? sender, NumberPicker.ValueChangeEventArgs e)
    {
        DayPicker.MaxValue = CultureInfo.CurrentCulture.Calendar.GetDaysInMonth(YearPicker.Value, MonthPicker.Value);
        DayPicker.Value = DayPicker.MaxValue;
    }

    private void YearPicker_ValueChanged(object? sender, NumberPicker.ValueChangeEventArgs e)
    {
        DayPicker.MaxValue = CultureInfo.CurrentCulture.Calendar.GetDaysInMonth(YearPicker.Value, MonthPicker.Value);
        DayPicker.Value = DayPicker.MaxValue;
    }
}