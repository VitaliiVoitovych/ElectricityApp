using ElectricityApp.EfStructures;
using ElectricityApp.Exceptions;
using ElectricityApp.Extensions;
using ElectricityApp.Services.Charting;
using Microsoft.EntityFrameworkCore;

namespace ElectricityApp.Services.Notes;

public partial class NotesService : ObservableObject
{
    private readonly ElectricityDbContext _dbContext;
    private readonly ChartsService _chartsService;

    public ChartsService ChartsService => _chartsService;
    public ObservableCollection<ElectricityConsumption> ElectricityConsumptions { get; }

    [ObservableProperty] private decimal _averageAmount;
    [ObservableProperty] private double _averageKilowattConsumed;

    public NotesService(ElectricityDbContext dbContext, ChartsService chartsService)
    {
        ElectricityConsumptions = [];
        _dbContext = dbContext;
        _chartsService = chartsService;
        Task.Run(LoadDataAsync);
    }

    private async Task LoadDataAsync()
    {
        var electricityConsumption = await _dbContext.ElectricityConsumptions.OrderBy(e => e.Date).ToListAsync();
        
        foreach (var consumption in electricityConsumption)
        {
            ElectricityConsumptions.Add(consumption);
            ChartsService.AddValues(consumption);
        }

        UpdateAverageValues();
    }

    public async Task ImportDataAsync(IAsyncEnumerable<ElectricityConsumption> data)
    {
        Clear();

        await foreach (var consumption in data)
        {
            AddNote(consumption);
        }
    }

    public void Clear()
    {
        var electricityConsumptions = ElectricityConsumptions.ToList();

        foreach (var consumption in electricityConsumptions)
        {
            RemoveNote(consumption);
        }
    }

    public void AddNote(ElectricityConsumption consumption)
    {
        DuplicateConsumptionNoteException.ThrowIfDuplicateExists(ElectricityConsumptions, consumption);

        var index = ElectricityConsumptions.LastMatchingIndex(c => c.Date < consumption.Date) + 1;

        ElectricityConsumptions.Insert(index, consumption);
        ChartsService.AddValues(index, consumption);

        _dbContext.ElectricityConsumptions.Add(consumption);
        _dbContext.SaveChanges();

        UpdateAverageValues();
    }

    public void RemoveNote(ElectricityConsumption consumption)
    {
        var index = ElectricityConsumptions.IndexOf(consumption);
        ElectricityConsumptions.RemoveAt(index);
        ChartsService.RemoveValues(index);

        _dbContext.ElectricityConsumptions.Remove(consumption);
        _dbContext.SaveChanges();

        UpdateAverageValues();
    }

    private void UpdateAverageValues()
    {
        AverageAmount = ElectricityConsumptions.Count > 0 
            ? ElectricityConsumptions.Average(e => e.AmountToPay) 
            : 0.0m;

        AverageKilowattConsumed = ElectricityConsumptions.Count > 0 
            ? ElectricityConsumptions.Average(e => e.DayKilowattConsumed + e.NightKilowattConsumed) 
            : 0.0;

    }
}