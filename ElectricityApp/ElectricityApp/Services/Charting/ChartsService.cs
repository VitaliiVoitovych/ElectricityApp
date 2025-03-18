using ElectricityApp.Services.Charting.Styles;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;

namespace ElectricityApp.Services.Charting;

public class ChartsService
{
    private readonly ObservableCollection<decimal> _amountsToPayValues = [];
    private readonly ObservableCollection<int> _dayKilowattConsumed = [];
    private readonly ObservableCollection<int> _nightKilowattConsumed = [];
    private readonly ObservableCollection<string> _dateLabels = [];

    public void AddValues(ElectricityConsumption consumption)
    {
        _dateLabels.Add(consumption.Date.ToString(ChartConstants.DateFormat));
        _dayKilowattConsumed.Add(consumption.DayKilowattConsumed);
        _nightKilowattConsumed.Add(consumption.NightKilowattConsumed);
        _amountsToPayValues.Add(consumption.AmountToPay);
    }

    public void AddValues(int index, ElectricityConsumption consumption)
    {
        _dateLabels.Insert(index, consumption.Date.ToString(ChartConstants.DateFormat));
        _dayKilowattConsumed.Insert(index, consumption.DayKilowattConsumed);
        _nightKilowattConsumed.Insert(index, consumption.NightKilowattConsumed);
        _amountsToPayValues.Insert(index, consumption.AmountToPay);
    }

    public void RemoveValues(int index)
    {
        _dateLabels.RemoveAt(index);
        _dayKilowattConsumed.RemoveAt(index);
        _nightKilowattConsumed.RemoveAt(index);
        _amountsToPayValues.RemoveAt(index);
    }

    public void UpdateValues(int index, ElectricityConsumption consumption)
    {
        _dayKilowattConsumed[index] = consumption.DayKilowattConsumed;
        _nightKilowattConsumed[index] = consumption.NightKilowattConsumed;
        _amountsToPayValues[index] = consumption.AmountToPay;
    }
    
    public IEnumerable<ISeries> AmountsToPaySeries =>
    [
        ChartUtils.CreateLineSeries(_amountsToPayValues, ChartColors.AmountToPaySeriesColor),
    ];

    public IEnumerable<Axis> AmountToPayYAxes =>
    [
        ChartUtils.CreateValueYAxis(d => d.ToString("f2"))
    ];

    public IEnumerable<ISeries> KilowattConsumedSeries =>
    [
        ChartUtils.CreateLineSeries(_dayKilowattConsumed, ChartColors.DaySeriesColor, "День"),
        ChartUtils.CreateLineSeries(_nightKilowattConsumed, ChartColors.NightSeriesColor, "Ніч")
    ];

    public IEnumerable<Axis> KilowattConsumedYAxes =>
    [
        ChartUtils.CreateValueYAxis(),
    ];

    public IEnumerable<Axis> DateXAxes =>
    [
        new Axis
        {
            Labels = _dateLabels,
            MaxLimit = ChartConstants.MaxXAxisLabels,
            LabelsPaint = ChartPaints.AxisLabelsPaint,
        }
    ];

    public SolidColorPaint LegendTextPaint => ChartPaints.LegendTextPaint;
}