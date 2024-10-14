namespace Lab1.Models.Tools.ROI;

/// <summary>
/// Region of interest
/// </summary>
public interface IRegionOfInterestInfo<Pixel>
{
    int NumberOfPixels { get; }
    double Square { get; }
    double Perimeter { get; }
    double GeometricMedian { get; }
    Pixel AvgIntensity { get; }

    Pixel StdDevIntensity { get; }

    Pixel SumIntensity { get; }
}

