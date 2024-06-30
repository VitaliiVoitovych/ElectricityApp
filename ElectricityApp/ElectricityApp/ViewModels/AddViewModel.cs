using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ElectricityApp.Models;
using ElectricityApp.Services;

namespace ElectricityApp.ViewModels;

public partial class AddViewModel(NotesService _notesService) : ObservableObject
{
    private static readonly Dictionary<string, int> _months = new()
    {
        { "Січень", 1}, { "Лютий" , 2}, { "Березень" , 3}, { "Квітень" , 4},
        { "Травень", 5}, { "Червень" , 6}, { "Липень" , 7}, { "Серпень" , 8},
        { "Вересень", 9}, { "Жовтень" , 10}, { "Листопад" , 11}, { "Грудень" , 12},
    };

    public List<string> Months => [.. _months.Keys];

    public List<int> Years => [.. Enumerable.Range(DateTime.Now.Year - 5, 20)];

    [ObservableProperty] private string _selectedMonth = _months.First(m => m.Value == DateTime.Now.Month).Key;
    [ObservableProperty] private int _selectedYear = DateTime.Now.Year;
    [ObservableProperty] private int _dayKilowattConsumed;
    [ObservableProperty] private int _nightKilowattConsumed;
    [ObservableProperty] private decimal _kilowattPerHourPrice = 4.32m;

    [RelayCommand]
    private async Task Add()
    {
        var amountToPlay = 
            DayKilowattConsumed * KilowattPerHourPrice + NightKilowattConsumed * (KilowattPerHourPrice * 0.5m);

        var record = new ElectricityConsumption(new DateOnly(SelectedYear, _months[SelectedMonth], 1), DayKilowattConsumed, NightKilowattConsumed, amountToPlay);
        try
        {
            await _notesService.AddNoteAsync(record);
            var monthNumber = _months.First(m => m.Key.Equals(SelectedMonth)).Value;
            SelectedMonth = _months.First(m => m.Value == monthNumber + 1).Key;
        }
        catch (ArgumentException ex)
        {
            await Shell.Current.DisplayAlert("Помилка!", ex.Message, "Зрозуміло");
        }
    }
}