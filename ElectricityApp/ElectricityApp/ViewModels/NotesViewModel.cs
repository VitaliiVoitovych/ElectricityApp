using ElectricityApp.Services.Files;
using System.Text.Json;

namespace ElectricityApp.ViewModels;

public partial class NotesViewModel(NotesService notesService, FileService fileService) : ObservableObject
{
    public NotesService NotesService => notesService;

    [RelayCommand]
    private void Remove(ObservableElectricityConsumption consumption)
    {
        NotesService.RemoveNote(consumption);
    }

    [RelayCommand]
    private void Edit(ObservableElectricityConsumption consumption)
    {
        Shell.Current.GoToAsync(nameof(EditPage), true, new Dictionary<string, object>
        {
            { "Consumption", consumption }
        });
    }
    
    [RelayCommand]
    private async Task ExportData()
    {
        var filename = $"electricity_{DateTime.UtcNow:dd-MM-yyyy}.json";
        var filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), filename);

        await DataExporterImporter.ExportAsync(filePath, NotesService.ElectricityConsumptions);

        await fileService.ShareFileAsync(filePath);
    }

    [RelayCommand]
    private async Task ImportData()
    {
        try
        {
            var file = await fileService.OpenFileAsync();
            var data = await DataExporterImporter.ImportAsync(file);
            await NotesService.ImportDataAsync(data);
        }
        catch (FileNotFoundException ex)
        {
            await Shell.Current.DisplayAlert("Помилка!", ex.Message, "Зрозуміло");
        }
        catch (JsonException)
        {
            await Shell.Current.DisplayAlert("Помилка!", "Ви обрали не файл з даними!", "Зрозуміло");
        }
    }
}