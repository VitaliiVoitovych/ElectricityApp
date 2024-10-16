using ElectricityApp.Services.Charting.Styles;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.Painting.Effects;
using SkiaSharp;

namespace ElectricityApp.Services.Charting;

public class ChartsService
{
    private readonly ObservableCollection<decimal> _amountsToPayValues = [];
    private readonly ObservableCollection<int> _dayKilowattConsumed = [];
    private readonly ObservableCollection<int> _nightKilowattConsumed = [];
    private readonly ObservableCollection<string> _dateLabels = [];

    public async Task UpdateValues(ObservableCollection<ElectricityConsumption> electricityConsumptions)
    {
        await Task.Run(() =>
        {
            _amountsToPayValues.Clear();
            _dayKilowattConsumed.Clear();
            _nightKilowattConsumed.Clear();
            _dateLabels.Clear();

            foreach (var r in electricityConsumptions)
            {
                AddValues(r);
            }
        });
    }

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