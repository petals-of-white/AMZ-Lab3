using MathNet.Numerics.Statistics;

namespace Lab1.ViewModels;

public class ImageStatisticsViewModel : SimpleNotifier
{
    private bool isShown = false;
    private IReadOnlyCollection<ushort> pixels;

    public ImageStatisticsViewModel()
    {
        pixels = [];
    }

    public ImageStatisticsViewModel(IReadOnlyCollection<ushort> pixels) => this.pixels = pixels;

    public bool IsShown
    {
        get => isShown;
        set
        {
            isShown = value;
            NotifyPropertyChanged(nameof(IsShown));
        }
    }

    public double Mean => pixels.Select(p => (double) p).Mean();

    public IReadOnlyCollection<ushort> Pixels
    {
        get => pixels; set
        {
            pixels = value;
            NotifyPropertyChanged(nameof(Pixels));
            NotifyPropertyChanged(nameof(Mean));
            NotifyPropertyChanged(nameof(Variance));
            NotifyPropertyChanged(nameof(StdDeviation));
        }
    }

    public double StdDeviation => pixels.Select(p => (double) p).StandardDeviation();
    public double Variance => pixels.Select(p => (double) p).Variance();
}