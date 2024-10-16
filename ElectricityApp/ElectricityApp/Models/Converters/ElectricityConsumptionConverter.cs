using System.Text.Json;
using System.Text.Json.Serialization;

namespace ElectricityApp.Models.Converters;

public class ElectricityConsumptionConverter : JsonConverter<ElectricityConsumption>
{
    public override ElectricityConsumption? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using var doc = JsonDocument.ParseValue(ref reader);
        var root = doc.RootElement;

        if (root.TryGetProperty(nameof(ElectricityConsumption.DayKilowattConsumed), out _))
        {
            return JsonSerializer.Deserialize<ElectricityConsumption>(root.GetRawText());
        }

        throw new JsonException("Wrong consumption type");
    }

    public override void Write(Utf8JsonWriter writer, ElectricityConsumption value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value, options);
    }
}
