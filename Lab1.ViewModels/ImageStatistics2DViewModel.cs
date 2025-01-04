namespace Lab1.ViewModels;

using FellowOakDicom;
using MathNet.Numerics.LinearAlgebra;

public class ImageStatistics2DViewModel : SimpleNotifier
{
    private IReadOnlyCollection<ushort> firstImage;
    private bool isShown = false;
    private Matrix<double> probabilityMatrix;
    private IReadOnlyCollection<ushort> secondImage;

    public ImageStatistics2DViewModel()
    {
        firstImage = [];
        secondImage = [];
        MaxAllowedValue = 1150;
    }

    public ImageStatistics2DViewModel(IReadOnlyCollection<ushort> first, IReadOnlyCollection<ushort> second, ushort maxAllowedValue)
    {
        firstImage = first;
        secondImage = second;
        FillProbabilityMatrix(maxAllowedValue);
    }

    public double Energy2D => probabilityMatrix.Enumerate(Zeros.Include).Select(p => Math.Pow(p, 2)).Sum();

    public double Enthropy2D => probabilityMatrix.Enumerate(Zeros.AllowSkip).Where(p => p != 0).Select(p => p * Math.Log2(p)).Sum();

    public IReadOnlyCollection<ushort> FirstImage
    {
        get => firstImage;
        set
        {
            firstImage = value;
            NotifyPropertyChanged(nameof(FirstImage));
            FillProbabilityMatrix(MaxAllowedValue);
            NotifyPropertyChanged(nameof(MaxAllowedValue));
            NotifyPropertyChanged(nameof(Energy2D));
            NotifyPropertyChanged(nameof(Enthropy2D));
            NotifyPropertyChanged(nameof(InverseDifference));
        }
    }

    public double InverseDifference
    {
        get
        {
            Func<(int, int, double), double> f = (probIndexed) =>
            {
                (var vk, var vl, var p) = probIndexed;

                return p / (1 + Math.Pow(vk - vl, 2)); // or whatever operation you need
            };

            return probabilityMatrix.EnumerateIndexed(Zeros.Include).Select(f).Sum();
        }
    }

    public bool IsShown
    {
        get => isShown;
        set
        {
            isShown = value;
            NotifyPropertyChanged(nameof(IsShown));
        }
    }

    public ushort MaxAllowedValue
    {
        get => (ushort) probabilityMatrix.ColumnCount;
        set
        {
            FillProbabilityMatrix(value);
            NotifyPropertyChanged(nameof(MaxAllowedValue));
            NotifyPropertyChanged(nameof(Energy2D));
            NotifyPropertyChanged(nameof(Enthropy2D));
            NotifyPropertyChanged(nameof(InverseDifference));
        }
    }

    public IReadOnlyCollection<ushort> SecondImage
    {
        get => secondImage;
        set
        {
            secondImage = value;

            NotifyPropertyChanged(nameof(SecondImage));

            FillProbabilityMatrix(MaxAllowedValue);

            NotifyPropertyChanged(nameof(MaxAllowedValue));
            NotifyPropertyChanged(nameof(Energy2D));
            NotifyPropertyChanged(nameof(Enthropy2D));
            NotifyPropertyChanged(nameof(InverseDifference));
        }
    }

    private void FillProbabilityMatrix(ushort maxAllowedValue)
    {
        probabilityMatrix = Matrix<double>.Build.Dense(maxAllowedValue, maxAllowedValue);
        var pairs = firstImage.Zip(secondImage).ToArray();
        pairs.Each(pair => probabilityMatrix [pair.First, pair.Second]++);
        probabilityMatrix.MapInplace(v => v / pairs.Length);
    }
}