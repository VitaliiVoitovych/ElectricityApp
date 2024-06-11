namespace ElectricityApp.Models;

public record ElectricityConsumption(
    DateOnly Date,
    int DayKilowattConsumed,
    int NightKilowattConsumed,
    decimal AmountToPay
    )
{ 
    public int Id { get; init; }
};