using CommunityToolkit.Maui.Storage;
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
    private async Task UploadDataToFile()
    {
        var result = await FolderPicker.Default.PickAsync();
        result.EnsureSuccess();

        var path = Path.Combine(result.Folder.Path, "electricity.json");
        File.WriteAllText(path, JsonSerializer.Serialize(_notesService.ElectricityConsumptions));
    }

    [RelayCommand]
    private async Task LoadDataFromFile()
    {
        PickOptions options = new()
        {
            FileTypes = new FilePickerFileType(
                new Dictionary<DevicePlatform, IEnumerable<string>>
                {
                    { DevicePlatform.Android, new[] { "application/json" } }
                }),
        };

        var result = await FilePicker.Default.PickAsync(options);

        var stream = await result.OpenReadAsync();

        var content = JsonSerializer.Deserialize<IAsyncEnumerable<ElectricityConsumption>>(stream)!;

        await foreach (var item in content)
        {
            try
            {
                await _notesService.AddNoteAsync(item);
            }
            catch (Exception)
            {
            }
        }
    }
}