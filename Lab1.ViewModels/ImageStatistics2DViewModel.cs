namespace Lab1.ViewModels;

using FellowOakDicom;
using MathNet.Numerics.LinearAlgebra;

public class ImageStatistics2DViewModel : SimpleNotifier
{
    private IReadOnlyCollection<ushort> firstImage;
    private bool isShown = false;
    private Matrix<double> probabilityMatrix;
    private IReadOnlyCollection<ushort> secondImage;

    public ImageStatistics2DViewModel(IReadOnlyCollection<ushort> first, IReadOnlyCollection<ushort> second, ushort maxAllowedValue)
    {
        firstImage = first;
        secondImage = second;
        FillProbabilityMatrix(maxAllowedValue);
    }

    public double Energy2D => probabilityMatrix.Enumerate(Zeros.Include).Select(p => Math.Pow(p, 2)).Sum();

    public double Enthropy2D => probabilityMatrix.Enumerate(Zeros.Include).Select(p => p * Math.Log2(p)).Sum();

    public IReadOnlyCollection<ushort> FirstImage
    {
        get => firstImage;
        set { firstImage = value; NotifyPropertyChanged(nameof(FirstImage)); }
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
        set => FillProbabilityMatrix(value);
    }

    public IReadOnlyCollection<ushort> SecondImage
    {
        get => secondImage;
        set { secondImage = value; NotifyPropertyChanged(nameof(SecondImage)); }
    }

    private void FillProbabilityMatrix(ushort maxAllowedValue)
    {
        probabilityMatrix = Matrix<double>.Build.Dense(maxAllowedValue + 1, maxAllowedValue + 1);
        var pairs = firstImage.Zip(secondImage).ToArray();
        pairs.Each(pair => probabilityMatrix [pair.First, pair.Second]++);
        probabilityMatrix.MapInplace(v => v / pairs.Length);
    }
}