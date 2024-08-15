namespace ElectricityApp.Pages;

public partial class MainPage : ContentPage
{
    public MainPage(MainViewModel vm)
    {
        InitializeComponent();

        BindingContext = vm;
    }

    protected override void OnAppearing()
    {
        AmountToPayChart.UpdateChart();
        KilowattConsumedChart.UpdateChart();
        CommunityToolkit.Maui.Core.Platform.StatusBar.SetColor(Color.FromArgb("#101d24"));
    }
}