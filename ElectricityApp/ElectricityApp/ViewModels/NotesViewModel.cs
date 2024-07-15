﻿using CommunityToolkit.Mvvm.ComponentModel;
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
        var options = new PickOptions()
        {
            PickerTitle = "Виберіть файл з даними (electricity.json)",
            FileTypes = new FilePickerFileType(
                new Dictionary<DevicePlatform, IEnumerable<string>>
                {
                    { DevicePlatform.Android, new[] { "application/json" } }
                }),
        };

        try
        {
            var result = await FilePicker.Default.PickAsync(options) ?? throw new FileNotFoundException("Ви не обрали файл!");
            var stream = await result.OpenReadAsync();

            var content = JsonSerializer.Deserialize<IAsyncEnumerable<ElectricityConsumption>>(stream)!;

            await NotesService.ClearAsync();

            await foreach (var item in content)
            {
                await _notesService.AddNoteAsync(item);
            }
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