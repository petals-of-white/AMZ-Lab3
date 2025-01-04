namespace Lab1.ViewModels;

public class HistogramViewModel : SimpleNotifier
{
    private int binCount;
    private IEnumerable<double> data = [];
    private bool isShown;

    public int BinCount
    {
        get => binCount;
        set
        {
            binCount = value;
            NotifyPropertyChanged(nameof(BinCount));
        }
    }

    public IEnumerable<double> Data
    {
        get => data; set
        {
            data = value;
            NotifyPropertyChanged(nameof(Data));
        }
    }

    public bool IsShown
    {
        get => isShown; set
        {
            isShown = value;
            NotifyPropertyChanged(nameof(IsShown));
        }
    }
}