using ElectricityApp.ViewModels;

namespace ElectricityApp.Pages;

public partial class NotesPage : ContentPage
{
    public NotesPage(NotesViewModel vm)
    {
        InitializeComponent();

        BindingContext = vm;
    }
}