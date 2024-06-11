using ElectricityApp.ViewModels;

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
        AmountChart.CoreChart.Update();
        KilowattConsumedChart.CoreChart.Update();
    }
}