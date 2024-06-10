using CommunityToolkit.Mvvm.ComponentModel;
using ElectricityApp.Services;

namespace ElectricityApp.ViewModels;

public partial class MainViewModel(NotesService _notesService) : ObservableObject
{
    public ChartsService ChartsService => _notesService.ChartsService;
}