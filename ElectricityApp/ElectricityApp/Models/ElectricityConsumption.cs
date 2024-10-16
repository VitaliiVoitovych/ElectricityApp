using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ElectricityApp.Models;

public record ElectricityConsumption(
    DateOnly Date,
    int DayKilowattConsumed,
    int NightKilowattConsumed,
    decimal AmountToPay
    )
{
    [Key, JsonIgnore]
    public int Id { get; init; }
};