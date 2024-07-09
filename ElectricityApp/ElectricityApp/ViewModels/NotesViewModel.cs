using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ElectricityApp.Models;
using ElectricityApp.Services;
using System.Text.Json;

namespace ElectricityApp.ViewModels;

public partial class NotesViewModel(NotesService _notesService) : ObservableObject
{
    public NotesService NotesService => _notesService;

    [RelayCommand]
    private async Task Remove(ElectricityConsumption record)
    {
        await NotesService.RemoveNoteAsync(record);
    }

    [RelayCommand]
    private async Task UploadToFile()
    {
        var filename = "electricity.json";
        var filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), filename);

        File.WriteAllText(filePath, JsonSerializer.Serialize(NotesService.ElectricityConsumptions));

        await Share.Default.RequestAsync(new ShareFileRequest
        {
            Title = "Вивантажити дані",
            File = new ShareFile(filePath)
        });
    }

    [RelayCommand]
    private async Task LoadFromFile()
    {
        var options = new PickOptions()
        {
            FileTypes = new FilePickerFileType(
                new Dictionary<DevicePlatform, IEnumerable<string>>
                {
                    { DevicePlatform.Android, new[] { "application/json" } }
                }),
        };

        try
        {
            var result = await FilePicker.Default.PickAsync(options);
            var stream = await result?.OpenReadAsync()!;


            var content = JsonSerializer.Deserialize<IAsyncEnumerable<ElectricityConsumption>>(stream)!;

            await foreach (var item in content)
            {
                await _notesService.AddNoteAsync(item);
            }
        }
        catch (Exception)
        {
        }
    }
}