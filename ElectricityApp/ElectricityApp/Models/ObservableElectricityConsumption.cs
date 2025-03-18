namespace ElectricityApp.Models;

public class ObservableElectricityConsumption(ElectricityConsumption consumption) : ObservableObject
{
    public ElectricityConsumption Consumption { get; } = consumption;

    public DateOnly Date
    {
        get => Consumption.Date;
        set => SetProperty(Consumption.Date, value, Consumption, 
            (consumption, date) => consumption.Date = date);
    }

    public decimal AmountToPay
    {
        get => Consumption.AmountToPay;
        set => SetProperty(Consumption.AmountToPay, value, Consumption,
            (consumption, amount) => consumption.AmountToPay = amount);
    }
    
    public int DayKilowattConsumed
    {
        get => Consumption.DayKilowattConsumed;
        set => SetProperty(Consumption.DayKilowattConsumed, value, Consumption,
            (consumption, dayKilowattConsumed) => consumption.DayKilowattConsumed = dayKilowattConsumed);
    }

    public int NightKilowattConsumed
    {
        get => Consumption.NightKilowattConsumed;
        set => SetProperty(Consumption.NightKilowattConsumed, value, Consumption,
            (consumption, nightKilowattConsumed) => consumption.NightKilowattConsumed = nightKilowattConsumed);
    }
}