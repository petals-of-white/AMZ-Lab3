using Lab1.Models.Tools.Shapes;
using Model = Lab1.Models.Tools.ROI;

namespace Lab1.ViewModels.Tools.ROI;

public class RectangleROITool(Model.RectangleROITool roiModel) : SimpleNotifier
{
    private Model.RectangleROITool roiModel = roiModel;
    public bool IsSelected
    {
        get => roiModel.IsSelected; set
        {
            roiModel = roiModel with { IsSelected = value };
            NotifyPropertyChanged(nameof(IsSelected));
        }
    }

    public bool IsActive
    {
        get => roiModel.IsActive;
        set
        {
            roiModel = roiModel with { IsActive = value };
            NotifyPropertyChanged(nameof(IsSelected));
        }
    }

    public Rectangle Rectangle
    {
        get => roiModel.Rect;
        set
        {
            roiModel = roiModel with { Rect = value };
            NotifyPropertyChanged(nameof(Rectangle));
        }
    }

}


