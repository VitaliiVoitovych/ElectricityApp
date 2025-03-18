using System.Globalization;

namespace ElectricityApp;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("uk-UA");

        Routing.RegisterRoute(nameof(EditPage), typeof(EditPage));
        
        CurrentItem = PhoneTabs;
    }
}