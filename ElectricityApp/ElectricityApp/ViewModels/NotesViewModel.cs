using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ElectricityApp.Models;
using ElectricityApp.Services;

namespace ElectricityApp.ViewModels;

public partial class NotesViewModel(NotesService _notesService) : ObservableObject
{
    public NotesService NotesService => _notesService;

    [RelayCommand]
    private void Remove(ElectricityConsumption record)
    {
        NotesService.RemoveNote(record);
    }
}