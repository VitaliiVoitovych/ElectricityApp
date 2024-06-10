using System.Globalization;
using ElectricityApp.Pages;

namespace ElectricityApp;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("uk-UA");
        
        CurrentItem = PhoneTabs;
        Application.Current!.UserAppTheme = AppTheme.Dark;
    }
}