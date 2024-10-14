using System.Collections.ObjectModel;
using FellowOakDicom;
using Lab1.Models.Tools.ROI;

namespace Lab1.ViewModels;

public class App : SimpleNotifier
{
    RectangleROITool? roiTool;
    ObservableCollection<DicomFile>? dicoms;
    public RectangleROITool? ROITool
    {
        get => roiTool; set
        {
            roiTool = value;
            NotifyPropertyChanged(nameof(ROITool));
        }
    }

    public ObservableCollection<DicomFile>? Dicoms
    {
        get => dicoms; set
        {
            dicoms = value;
            NotifyPropertyChanged(nameof(Dicoms));
        }
    }
}
