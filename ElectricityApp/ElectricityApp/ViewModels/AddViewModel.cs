using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ElectricityApp.Models;
using ElectricityApp.Services;

namespace ElectricityApp.ViewModels;

public partial class AddViewModel(NotesService _notesService) : ObservableObject
{
    [ObservableProperty] private DateTime _date = DateTime.Now;
    [ObservableProperty] private int _dayKilowattConsumed;
    [ObservableProperty] private int _nightKilowattConsumed;
    [ObservableProperty] private decimal _kilowattPerHourPrice = 4.32m;

    [RelayCommand]
    private void Add()
    {
        var amountToPlay = 
            DayKilowattConsumed * KilowattPerHourPrice + NightKilowattConsumed * (KilowattPerHourPrice * 0.5m);

        var record = new ElectricityConsumption(Date, DayKilowattConsumed, NightKilowattConsumed, amountToPlay);
        _notesService.AddNote(record);
    }
}