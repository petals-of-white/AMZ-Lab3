using Lab1.Models.Tools.ROI;

namespace Lab1.Models;

public class CurrentImage(DicomManager dicom)
{
    private float currentDepth = 0f;

    public float CurrentDepth
    {
        get => currentDepth;
        private set => currentDepth = value switch
        {
            (< 0f) => 0f,
            (> 1f) => 1f,
            var other => other
        };
    }

    public void AdvanceInDepth(int move)
    {
        CurrentDepth += (move / Dicom.Depth);
    }

    public DicomManager Dicom { get; } = dicom;
}

public class App
{
    public CurrentImage? DisplayImage { get; set; }

    public RectangleROI? RegionOfInterest { get; set; } = new();
    
}