using ElectricityApp.Models.Converters;
using System.Text.Json;

namespace ElectricityApp.Services.Files;

public static class DataExporterImporter
{
    private static readonly JsonSerializerOptions Options = new JsonSerializerOptions
    {
        Converters = { new ElectricityConsumptionConverter() }
    };

    public static async Task<IAsyncEnumerable<ElectricityConsumption>> ImportAsync(Stream stream)
    {
        return (await JsonSerializer.DeserializeAsync<IAsyncEnumerable<ElectricityConsumption>>(stream, Options))!;
    }

    public static async Task ExportAsync<T>(string path, IEnumerable<T> collection)
        where T : ElectricityConsumption
    {
        await using var stream = new FileStream(path, FileMode.Create, FileAccess.Write);
        await JsonSerializer.SerializeAsync(stream, collection);
    }
}
