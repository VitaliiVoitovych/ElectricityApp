using ElectricityApp.Services.Charting;

namespace ElectricityApp.ViewModels;

public partial class MainViewModel(NotesService notesService) : ObservableObject
{
    public NotesService NotesService => notesService;
    public ChartsService ChartsService => NotesService.ChartsService;
}