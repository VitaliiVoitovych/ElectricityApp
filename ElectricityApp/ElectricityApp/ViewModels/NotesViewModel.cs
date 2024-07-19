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
    private async Task GoToQrCode()
    {
        await Shell.Current.GoToAsync($"{nameof(QrCodePage)}", true);
    }

    [RelayCommand]
    private async Task ExportData()
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
    private async Task ImportData()
    {
        try
        {
            var file = await PickFileAsync();
            var data = await DeserializeFileAsync(file);
            await SaveDataAsync(data);
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

    private async Task<FileResult> PickFileAsync()
    {
        var options = new PickOptions()
        {
            PickerTitle = "Виберіть файл з даними (electricity.json)",
            FileTypes = new FilePickerFileType(
                new Dictionary<DevicePlatform, IEnumerable<string>>
                {
                    { DevicePlatform.Android, new[] { "application/json" } }
                }),
        };

        var result = await FilePicker.Default.PickAsync(options) ?? throw new FileNotFoundException("Ви не обрали файл!");
        return result;
    }

    private async Task<IAsyncEnumerable<ElectricityConsumption>> DeserializeFileAsync(FileResult file)
    {
        var stream = await file.OpenReadAsync();
        return JsonSerializer.Deserialize<IAsyncEnumerable<ElectricityConsumption>>(stream)!;
    }

    private async Task SaveDataAsync(IAsyncEnumerable<ElectricityConsumption> data)
    {
        await NotesService.ClearAsync();
        await foreach (var item in data)
        {
            await _notesService.AddNoteAsync(item);
        }
    }
}