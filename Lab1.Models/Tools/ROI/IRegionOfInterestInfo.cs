namespace Lab1.Models.Tools.ROI;

/// <summary>
/// Region of interest
/// </summary>
public interface IRegionOfInterestInfo
{
    double Area { get; }
    int NumberOfPixels { get; }
}