using Lab1.Models.Tools.ROI;

namespace Lab1.Models;

public class App
{
    float currentDepth = 0;
    public float CurrentDepth
    {
        get => currentDepth;
        set => currentDepth = value switch
        {
            (< 0) => 0,
            (> 1) => 1,
            var otherwise => otherwise
        };
    }
    public RectangleROI RegionOfInterest { get; set; } = new();
    public DicomManager? Dicom { get; set; } = null;
}
