namespace ElectricityApp.Models;

public record ElectricityConsumption(
    DateTime Date,
    int DayKilowattConsumed,
    int NightKilowattConsumed,
    decimal AmountToPay
    )
{ 
    public int Id { get; init; }
};