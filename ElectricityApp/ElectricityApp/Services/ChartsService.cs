using System.Collections.ObjectModel;
using ElectricityApp.Models;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.Painting.Effects;
using SkiaSharp;

namespace ElectricityApp.Services;

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

    public void AddValues(ElectricityConsumption record)
    {
        _dateLabels.Add(record.Date.ToString("MMM yyyy"));
        _dayKilowattConsumed.Add(record.DayKilowattConsumed);
        _nightKilowattConsumed.Add(record.NightKilowattConsumed);
        _amountsToPayValues.Add(record.AmountToPay);
    }
    
    public IEnumerable<ISeries> AmountsToPaySeries =>
    [
        new LineSeries<decimal>
        {
            Name = "Оплата",
            Values = _amountsToPayValues,
            Fill = null,
            Stroke = new SolidColorPaint(SKColor.Parse("#256F33")) { StrokeThickness = 3},
            GeometryFill = new SolidColorPaint(SKColor.Parse("#256F33")),
            GeometryStroke = new SolidColorPaint(SKColor.Parse("#256F33")) { StrokeThickness = 2},
            GeometrySize = 5,
        }
    ];

    public IEnumerable<Axis> AmountToPayYAxes =>
    [
        new Axis
        {
            SeparatorsPaint = new SolidColorPaint(SKColors.LightGray) 
            { 
                StrokeThickness = 0.5f, 
                PathEffect = new DashEffect([4f,4f])
            },
            Labeler = d => d.ToString("f2"),
            TextSize = 15,
            LabelsPaint = new SolidColorPaint(SKColor.Parse("#95b5cf"))
        }
    ];

    public IEnumerable<ISeries> KilowattConsumedSeries =>
    [
        new LineSeries<int>
        {
            Name = "День",
            Values = _dayKilowattConsumed,
            Fill = null,
            Stroke = new SolidColorPaint(SKColor.Parse("#e2e38b")) { StrokeThickness = 3},
            GeometryFill = new SolidColorPaint(SKColor.Parse("#e2e38b")),
            GeometryStroke = new SolidColorPaint(SKColor.Parse("#e2e38b")) { StrokeThickness = 3},
            GeometrySize = 5
        },
        new LineSeries<int>
        {
            Name = "Ніч",
            Values = _nightKilowattConsumed,
            Fill = null,
            Stroke = new SolidColorPaint(SKColor.Parse("#29297D")) { StrokeThickness = 3},
            GeometryFill = new SolidColorPaint(SKColor.Parse("#29297D")),
            GeometryStroke = new SolidColorPaint(SKColor.Parse("#29297D")) { StrokeThickness = 3},
            GeometrySize = 5
        }
    ];
    
    public IEnumerable<Axis> KilowattConsumedYAxes => 
    [
        new Axis
        {
            SeparatorsPaint = new SolidColorPaint(SKColors.LightGray)
            {
                StrokeThickness = 0.5f,
                PathEffect = new DashEffect([4f,4f])
            },
            LabelsPaint = new SolidColorPaint(SKColor.Parse("#95b5cf")),
        }
    ];
    
    public IEnumerable<Axis> DateXAxes =>
    [
        new Axis
        {
            Labels = _dateLabels,
            MaxLimit = 12,
            LabelsPaint = new SolidColorPaint(SKColor.Parse("#95b5cf")),
        }
    ];

    public SolidColorPaint LegendTextPaint { get; set; } = new SolidColorPaint()
    {
        Color = SKColor.Parse("#abb0b3"),
    };
}