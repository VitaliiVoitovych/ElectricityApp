using System.Collections.ObjectModel;
using ElectricityApp.EfStructures;
using ElectricityApp.Models;

namespace ElectricityApp.Services;

public class NotesService
{
    private readonly ElectricityDbContext _dbContext;
    private readonly ChartsService _chartService;

    public ChartsService ChartsService => _chartService;
    public ObservableCollection<ElectricityConsumption> ElectricityConsumptions { get; }

    public NotesService(ElectricityDbContext dbContext, ChartsService chartsService)
    {
        ElectricityConsumptions = [];
        _dbContext = dbContext;
        _chartService = chartsService;
        Task.Run(LoadData);
    }

    private async Task LoadData()
    {
        await foreach (var r in _dbContext.ElectricityConsumptions.AsAsyncEnumerable())
        {
            ElectricityConsumptions.Add(r);
            _chartService.AddValues(r);
        }
    }

    public void AddNote(ElectricityConsumption record)
    {
        // TODO: Fix this
        if ( ElectricityConsumptions
                .FirstOrDefault(r => r.Date == record.Date) is not null)
            throw new ArgumentException("Запис про цей місяць вже є");

        ElectricityConsumptions.Add(record);
        _chartService.AddValues(record);
        _dbContext.ElectricityConsumptions.Add(record);
        _dbContext.SaveChanges();
    }

    public async Task RemoveNote(ElectricityConsumption record)
    {
        ElectricityConsumptions.Remove(record);
        _dbContext.ElectricityConsumptions.Remove(record);
        _dbContext.SaveChanges();

        await _chartService.UpdateValues(ElectricityConsumptions);
    }
}