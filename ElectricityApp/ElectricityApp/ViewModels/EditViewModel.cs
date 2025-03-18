namespace ElectricityApp.ViewModels;

public partial class EditViewModel(NotesService notesService) : ObservableObject, IQueryAttributable
{
    public ObservableElectricityConsumption Consumption { get; private set; }
    
    [ObservableProperty] private int _dayKilowattConsumed;
    [ObservableProperty] private int _nightKilowattConsumed;
    [ObservableProperty] private decimal _kilowattPerHourPrice = 4.32m;

    [RelayCommand]
    private void GoToBack() => Shell.Current.GoToAsync("..", true);
    
    [RelayCommand]
    private void Update()
    {
        var amountToPay = 
            DayKilowattConsumed * KilowattPerHourPrice + NightKilowattConsumed * (KilowattPerHourPrice * 0.5m);
        
        Consumption.DayKilowattConsumed = DayKilowattConsumed;
        Consumption.NightKilowattConsumed = NightKilowattConsumed;
        Consumption.AmountToPay = amountToPay;
        
        notesService.UpdateNote(Consumption);
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        Consumption = (ObservableElectricityConsumption)query[nameof(Consumption)];
        OnPropertyChanged(nameof(Consumption));
        
        DayKilowattConsumed = Consumption.DayKilowattConsumed;
        NightKilowattConsumed = Consumption.NightKilowattConsumed;
    }
}