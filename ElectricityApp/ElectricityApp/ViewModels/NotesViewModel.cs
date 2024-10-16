using System.Text.Json;

namespace ElectricityApp.ViewModels;

public partial class NotesViewModel(NotesService notesService) : ObservableObject
{
    public NotesService NotesService => notesService;

    [RelayCommand]
    private void Remove(ElectricityConsumption record)
    {
        NotesService.RemoveNote(record);
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
        catch (ArgumentException ex)
        {
            await Shell.Current.DisplayAlert("Помилка!", ex.Message, "Зрозуміло");
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
        if (!result.FileName.Equals("electricity.json")) throw new ArgumentException($"Ви обрали не файл з даними! {result.FileName}");
        return result;
    }

    private async Task<IAsyncEnumerable<ElectricityConsumption>> DeserializeFileAsync(FileResult file)
    {
        var stream = await file.OpenReadAsync();
        return JsonSerializer.Deserialize<IAsyncEnumerable<ElectricityConsumption>>(stream)!;
    }
}