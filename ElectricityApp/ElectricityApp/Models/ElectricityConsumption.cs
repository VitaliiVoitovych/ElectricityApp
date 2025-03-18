using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ElectricityApp.Models;

public class ElectricityConsumption(
    DateOnly date,
    int dayKilowattConsumed,
    int nightKilowattConsumed,
    decimal amountToPay
    )
{
    [Key, JsonIgnore]
    public int Id { get; init; }
    
    public  DateOnly Date { get; set; } = date;
    public decimal AmountToPay { get; set; } = amountToPay;
    
    public int DayKilowattConsumed { get; set; } = dayKilowattConsumed;
    public int NightKilowattConsumed { get; set; } = nightKilowattConsumed;
};