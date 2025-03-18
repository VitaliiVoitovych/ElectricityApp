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
    public ObservableCollection<ObservableElectricityConsumption> ObservableElectricityConsumptions { get; }

    public List<ElectricityConsumption> ElectricityConsumptions =>
        ObservableElectricityConsumptions.Select(c => c.Consumption).ToList();
    
    [ObservableProperty] private decimal _averageAmount;
    [ObservableProperty] private double _averageKilowattConsumed;

    public NotesService(ElectricityDbContext dbContext, ChartsService chartsService)
    {
        ObservableElectricityConsumptions = [];
        _dbContext = dbContext;
        _chartsService = chartsService;
        Task.Run(LoadDataAsync);
    }

    private async Task LoadDataAsync()
    {
        var electricityConsumption = await _dbContext.ElectricityConsumptions.OrderBy(e => e.Date).ToListAsync();
        
        foreach (var consumption in electricityConsumption)
        {
            ObservableElectricityConsumptions.Add(new ObservableElectricityConsumption(consumption));
            ChartsService.AddValues(consumption);
        }

        UpdateAverageValues();
    }

    public async Task ImportDataAsync(IAsyncEnumerable<ElectricityConsumption> data)
    {
        Clear();

        await foreach (var consumption in data)
        {
            AddNote(new ObservableElectricityConsumption(consumption));
        }
    }

    public void Clear()
    {
        var electricityConsumptions = ObservableElectricityConsumptions.ToList();

        foreach (var consumption in electricityConsumptions)
        {
            RemoveNote(consumption);
        }
    }

    public void AddNote(ObservableElectricityConsumption consumption)
    {
        DuplicateConsumptionNoteException.ThrowIfDuplicateExists(ElectricityConsumptions, consumption.Consumption);

        var index = ObservableElectricityConsumptions.LastMatchingIndex(c => c.Date < consumption.Date) + 1;

        ObservableElectricityConsumptions.Insert(index, consumption);
        ChartsService.AddValues(index, consumption.Consumption);

        _dbContext.ElectricityConsumptions.Add(consumption.Consumption);
        _dbContext.SaveChanges();

        UpdateAverageValues();
    }

    public void RemoveNote(ObservableElectricityConsumption consumption)
    {
        var index = ObservableElectricityConsumptions.IndexOf(consumption);
        ObservableElectricityConsumptions.RemoveAt(index);
        ChartsService.RemoveValues(index);

        _dbContext.ElectricityConsumptions.Remove(consumption.Consumption);
        _dbContext.SaveChanges();

        UpdateAverageValues();
    }

    public void UpdateNote(ObservableElectricityConsumption consumption)
    {
        var index = ObservableElectricityConsumptions.LastMatchingIndex(c => c.Date == consumption.Date);

        _dbContext.ElectricityConsumptions.Update(consumption.Consumption);
        ChartsService.UpdateValues(index, consumption.Consumption);
        
        UpdateAverageValues();
    }
    
    private void UpdateAverageValues()
    {
        AverageAmount = ObservableElectricityConsumptions.Count > 0 
            ? ObservableElectricityConsumptions.Average(e => e.AmountToPay) 
            : 0.0m;

        AverageKilowattConsumed = ObservableElectricityConsumptions.Count > 0 
            ? ObservableElectricityConsumptions.Average(e => e.DayKilowattConsumed + e.NightKilowattConsumed) 
            : 0.0;

    }
}