using Lab1.Models.Shapes;
using Model = Lab1.Models.Tools.ROI;

namespace Lab1.ViewModels.Tools.ROI;

public class RectangleROI(Model.RectangleROI rectangleROI) : SimpleNotifier
{
    private readonly Model.RectangleROI roiModel = rectangleROI;
    public bool IsSelected
    {
        get => roiModel.IsSelected;
        set
        {
            roiModel.IsSelected = value;
            NotifyPropertyChanged(nameof(IsSelected));
        }
    }

    public bool IsActive
    {
        get => roiModel.IsActive;
        set
        {
            roiModel.IsActive = value;
            NotifyPropertyChanged(nameof(IsSelected));
        }
    }

    public Rectangle? Region
    {
        get => roiModel.Region;
        set
        {
            roiModel.Region = value;
            NotifyPropertyChanged(nameof(Rectangle));
        }
    }

}


