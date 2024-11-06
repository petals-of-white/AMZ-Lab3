using Lab1.Models;
using Lab1.ViewModels.Tools.ROI;

namespace Lab1.ViewModels;

public class App : SimpleNotifier
{
    private readonly Models.App appModel = new();

    public RectangleROI RegionOfInterest
    {
        get => new(appModel.RegionOfInterest);
        set
        {
            appModel.RegionOfInterest = new() { IsActive = value.IsActive, IsSelected = value.IsSelected, Region = value.Region };
            NotifyPropertyChanged(nameof(RegionOfInterest));
        }
    }
    public float CurrentDepth
    {
        get => appModel.CurrentDepth;
        set
        {
            appModel.CurrentDepth = value;
            NotifyPropertyChanged(nameof(CurrentDepth));
        }
    }


    public DicomManager? Dicom
    {
        get => appModel.Dicom;
        set
        {
            appModel.Dicom = value;
            NotifyPropertyChanged(nameof(Dicom));
        }
    }
}
