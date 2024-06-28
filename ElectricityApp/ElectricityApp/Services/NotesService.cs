using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using ElectricityApp.EfStructures;
using ElectricityApp.Models;
using Microsoft.EntityFrameworkCore;

namespace ElectricityApp.Services;

public partial class NotesService : ObservableObject
{
    private readonly ElectricityDbContext _dbContext;
    private readonly ChartsService _chartService;

    public ChartsService ChartsService => _chartService;
    public ObservableCollection<ElectricityConsumption> ElectricityConsumptions { get; }

    [ObservableProperty] private decimal _averageAmount;
    [ObservableProperty] private double _averageKilowattConsumed;

    public NotesService(ElectricityDbContext dbContext, ChartsService chartsService)
    {
        ElectricityConsumptions = [];
        _dbContext = dbContext;
        _chartService = chartsService;
        Task.Run(LoadData);
    }

    private async Task LoadData()
    {
        var electricityConsumption = await _dbContext.ElectricityConsumptions.OrderBy(e => e.Date).ToListAsync();
        foreach (var r in electricityConsumption)
        {
            ElectricityConsumptions.Add(r);
            _chartService.AddValues(r);
        }

        UpdateAverageValues();
    }

    public async Task AddNote(ElectricityConsumption record)
    {
        if ( ElectricityConsumptions
                .FirstOrDefault(r => EqualsYearAndMonth(r.Date, record.Date)) is not null)
            throw new ArgumentException("Запис про цей місяць вже є");

        ElectricityConsumptions.Add(record);
        _chartService.AddValues(record);
        _dbContext.ElectricityConsumptions.Add(record);
        _dbContext.SaveChanges();

        var electricityConsumption = ElectricityConsumptions.OrderBy(e => e.Date).ToList();
        ElectricityConsumptions.Clear();
        foreach (var r in electricityConsumption)
        {
            ElectricityConsumptions.Add(r);
        }

        UpdateAverageValues();
        await _chartService.UpdateValues(ElectricityConsumptions);
    }

    public async Task RemoveNote(ElectricityConsumption record)
    {
        ElectricityConsumptions.Remove(record);
        _dbContext.ElectricityConsumptions.Remove(record);
        _dbContext.SaveChanges();

        UpdateAverageValues();
        await _chartService.UpdateValues(ElectricityConsumptions);
    }

    private bool EqualsYearAndMonth(DateOnly date1, DateOnly date2)
    {
        return (date1.Year, date1.Month) == (date2.Year, date2.Month);
    }

    private void UpdateAverageValues()
    {
        if (ElectricityConsumptions.Count > 0)
        {
            AverageAmount = ElectricityConsumptions.Average(e => e.AmountToPay);
            AverageKilowattConsumed = ElectricityConsumptions.Average(e => e.DayKilowattConsumed + e.NightKilowattConsumed);
        }
        else
        {
            AverageAmount = 0.0m;
            AverageKilowattConsumed = 0.0;
        }
    }
}