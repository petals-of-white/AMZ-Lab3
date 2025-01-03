using MathNet.Numerics.Statistics;

namespace Lab1.ViewModels;

public class ImageStatisticsViewModel : SimpleNotifier
{
    private bool isShown = false;
    private IReadOnlyCollection<double> pixels;

    public ImageStatisticsViewModel(IReadOnlyCollection<double> pixels) => this.pixels = pixels;

    public bool IsShown
    {
        get => isShown;
        set
        {
            isShown = value;
            NotifyPropertyChanged(nameof(IsShown));
        }
    }

    public double Mean => pixels.Mean();

    public IReadOnlyCollection<double> Pixels
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

    public double StdDeviation => pixels.StandardDeviation();
    public double Variance => pixels.Variance();
}
