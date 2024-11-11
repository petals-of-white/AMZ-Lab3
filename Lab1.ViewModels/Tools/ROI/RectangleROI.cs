using Lab1.Models.Shapes;
using Model = Lab1.Models.Tools.ROI;

namespace Lab1.ViewModels.Tools.ROI;

public class RectangleROI(Model.RectangleROI rectangleROI) : SimpleNotifier
{
    private readonly Model.RectangleROI roiModel = rectangleROI;

    public Rectangle Region
    {
        get => roiModel.Region;
        set { roiModel.Region = value; NotifyPropertyChanged(nameof(Region)); }
    }
}